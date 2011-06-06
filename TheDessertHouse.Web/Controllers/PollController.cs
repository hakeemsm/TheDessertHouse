using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using FluentNHibernate.Utils;
using TheDessertHouse.Domain;
using TheDessertHouse.Infrastructure;
using TheDessertHouse.Web.Models;

namespace TheDessertHouse.Web.Controllers
{
    public class PollController : SuperController
    {
        private IRepositoryProvider _repositoryProvider;

        public PollController(IRepositoryProvider repositoryProvider)
        {
            _repositoryProvider = repositoryProvider;
        }

        public ViewResult Index(bool? archived, int pageNum)
        {
            archived = archived ?? false;
            if (archived.Value && !Configuration.DessertHouseConfigurationSection.Current.Polls.ArchiveIsPublic
                && GetRole())
                throw new HttpException(401, "Archived polls are only available to site editors");
            ViewBag.PageTitle = "Polls";
            return View(GetPolls(pageNum, p => p.IsArchived == archived));

        }

        private Pagination<PollView> GetPolls(int pageNum, Func<Poll, bool> expression)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var pageSize = Configuration.DessertHouseConfigurationSection.Current.Polls.PageSize;
                var skip = (pageNum - 1) * pageSize;
                var polls = expression != null ? repository.Get<Poll>().Where(expression) : repository.Get<Poll>();
                var pollsForDisplay = polls.Skip(skip).Take(pageSize);
                var pollViews = Mapper.Map<IEnumerable<Poll>, IEnumerable<PollView>>(pollsForDisplay);
                return new Pagination<PollView>(pollViews, pageNum, pageSize, polls.Count());
            }
        }

        [Authorize(Roles = "Editor")]
        public ActionResult ManagePolls(int pageNum)
        {
            ViewBag.PageTitle = "Manage Polls";
            return View(GetPolls(pageNum, null));
        }

        private bool GetRole()
        {
            if (Roles.Enabled)
                return !Roles.IsUserInRole("Editor");
            return true;
        }

        [Authorize(Roles = "Editor")]
        public ActionResult CreatePoll()
        {
            ViewBag.PageTitle = "Create Poll";
            return View(new PollView { ExistingPoll = false });
        }

        [Authorize(Roles = "Editor"), HttpPost]
        public JsonResult CreatePoll(PollView pollView)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var poll = Mapper.Map<PollView, Poll>(pollView);
                poll.AddedBy = HttpContext.User.Identity.Name;
                poll.DateAdded = DateTime.Now;
                poll.Path = poll.PollQuestion.ToUrlFormat();
                poll.Options.Each(p =>
                {
                    p.DateAdded = DateTime.Now;
                    p.ForPoll = poll;
                });
                repository.Save(poll);

                return Json(Url.Action("ManagePolls"));
            }
        }

        [Authorize(Roles = "Editor")]
        public JsonResult SetCurrent(int pollId)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var polls = repository.Get<Poll>().Where(p => p.Id != pollId);
                polls.Each(p =>
                {
                    p.IsCurrent = false;
                    repository.Update(p);
                });
                var poll = repository.Get<Poll>(pollId);
                poll.IsCurrent = true;
                repository.Update(poll);
                Session["CurrentPoll"] = poll;
                return Json(pollId);
            }
        }

        [Authorize(Roles = "Editor")]
        public JsonResult SetArchived(int pollId)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var poll = repository.Get<Poll>(pollId);
                poll.IsArchived = true;
                repository.Update(poll);
                return Json(new{poll.IsArchived,PollId=poll.Id});
            }
        }

        [Authorize(Roles = "Editor")]
        public JsonResult DeletePoll(int pollId)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                repository.Delete<Poll>(repository.Get<Poll>(pollId));
                return Json(pollId);
            }
        }

        [Authorize(Roles = "Editor")]
        public ActionResult EditPoll(int pollId)
        {
            using (var repository=_repositoryProvider.GetRepository())
            {
                var poll = repository.Get<Poll>(pollId);
                if (poll == null)
                    return HttpNotFound("The specified poll does not exist");
                ViewBag.PageTitle = "Update Poll";
                var model = Mapper.Map<Poll,PollView>(poll);
                model.ExistingPoll = true;
                return View("CreatePoll", model);
            }
        }

        [Authorize(Roles = "Editor")]
        public JsonResult UpdateOption(int optionId, string optionText)
        {
            using (var repository=_repositoryProvider.GetRepository())
            {
                var option = repository.Get<PollOptions>().Where(o => o.Id == optionId).FirstOrDefault();
                option.OptionText = optionText;
                repository.Update(option);
                return Json(new {optionId, optionText});
            }
        }

        [Authorize(Roles = "Editor")]
        public ActionResult UpdatePoll(PollView pollView)
        {
            using (var repository=_repositoryProvider.GetRepository())
            {
                var poll = repository.Get<Poll>().Where(p => p.Id == pollView.Id).FirstOrDefault();
                if (poll == null)
                    throw new ArgumentException(string.Format("Poll with Id {0} not found", pollView.Id));
                var deleteList = new List<PollOptions>();
                poll.Options.Each(p=>
                {
                    var option = pollView.Options.Where(o => o.Id == p.Id).FirstOrDefault();
                    if (option == null)
                        deleteList.Add(p);
                    else
                        p.OptionText = option.OptionText;
                });
                deleteList.Each(d=>
                {
                    var deletedOption = poll.Options.Where(p => p.Id == d.Id).FirstOrDefault();
                    poll.Options.Remove(deletedOption);
                });
                repository.Update(poll);
                return PartialView("ManagePollItem", pollView);
            }
        }

        [Authorize(Roles = "Editor")]
        public JsonResult DeleteOption(int optionId)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var option = repository.Get<PollOptions>().Where(o => o.Id == optionId).FirstOrDefault();
                repository.Delete<PollOptions>(option);
                return Json(optionId);
            }
        }

        public ActionResult GetCurrent()
        {
            var poll = Session["CurrentPoll"] as PollView;
            return PartialView("PollItem", poll);
        }

        public ActionResult CastPoll(int pollId, int optionId)
        {
            using (var repository=_repositoryProvider.GetRepository())
            {
                var poll = repository.Get<Poll>(pollId);
                poll.Options.Where(p => p.Id == optionId).First().Votes++;
                repository.Save(poll);
                int votes=0;
                poll.Options.Each(o=> votes+= o.Votes);
                Response.Cookies.Set(new HttpCookie("poll_" + pollId, pollId.ToString()));
                PollView pollView=Mapper.Map<Poll,PollView>(poll);
                if (pollView.IsCurrent)
                    Session["CurrentPoll"] = pollView;
                pollView.TotalVotes = votes;
                return Json(pollView);
            }
        }
    }
}