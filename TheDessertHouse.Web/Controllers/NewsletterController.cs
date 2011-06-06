using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using System.Web.Profile;
using AutoMapper;
using TheDessertHouse.Domain;
using TheDessertHouse.Infrastructure;
using TheDessertHouse.Services.ControllerTests;
using TheDessertHouse.Web.Models;

namespace TheDessertHouse.Web.Controllers
{
    public class NewsletterController : SuperController
    {
        private readonly IRepositoryProvider _repositoryProvider;
        private readonly IEmailService _emailService;
        private readonly IMembershipProvider _membershipProvider;

        public NewsletterController(IRepositoryProvider repositoryProvider, IEmailService emailService, IMembershipProvider membershipProvider)
        {
            _repositoryProvider = repositoryProvider;
            _emailService = emailService;
            _membershipProvider = membershipProvider;
        }

        public ActionResult Index()
        {
            ViewBag.PageTitle = "Newsletters";
            return View();
        }

        [Authorize(Roles = "Editor")]
        public ActionResult CreateNewsletter()
        {
            ViewBag.PageTitle = "Create Newsletter";
            return View(new NewsletterView());
        }


        [ValidateInput(false), Authorize(Roles = "Editor"),HttpPost]
        public ActionResult CreateNewsletter(NewsletterView newsletterView)
        {
            if (!string.IsNullOrEmpty(newsletterView.Subject) && !string.IsNullOrEmpty(newsletterView.HtmlBody))
            {
                var newsletter = Mapper.Map<NewsletterView, Newsletter>(newsletterView);
                newsletter.AddedBy = User.Identity.Name;
                newsletter.DateAdded = DateTime.Now;
                newsletter.Status = "Not Sent";
                newsletter.DateSent = null;
                using (var repository = _repositoryProvider.GetRepository())
                    repository.Save(newsletter);
                var config = Configuration.DessertHouseConfigurationSection.Current.Newsletters;
               var subscriberList = GetSubscriberList();

                var obj =
                    new NewsletterMessage { Newsletter = newsletter, FromEmail = config.FromEmail, FromEmailDisplayName = config.FromEmailDisplayName, EmailList=subscriberList };
                var emailThread = new Thread(_emailService.SendNewsLetter);
                emailThread.Start(obj);
                return RedirectToAction("ManageNewsletters", new { pendingUpdate = true });
            }
            ViewBag.PageTitle = "Create Newsletter";
            return View(new NewsletterView());
        }

        private List<string> GetSubscriberList()
        {
            var subscriberList = new List<string>();
            _membershipProvider.GetAllUsers().ToList().ForEach(m =>
            {
                var userProfile = ProfileBase.Create(m.UserName);
                if (userProfile.GetPropertyValue("Subscription").ToString() != "None")
                    subscriberList.Add(m.Email);
            });
            return subscriberList;
        }

        [Authorize(Roles = "Editor")]
        public ActionResult ManageNewsletters(bool pendingUpdate)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var newsletters = Mapper.Map<IEnumerable<Newsletter>,IEnumerable<NewsletterView>>(repository.Get<Newsletter>().OrderByDescending(n => n.DateAdded));
                ViewBag.PageTitle = "Manage Newsletters";
                ViewBag.PendingNewsletter = pendingUpdate;
                return View(newsletters);
            }
        }

        [Authorize(Roles = "Editor")]
        public ActionResult EditNewsletter(int? newsletterId)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var newsletter = repository.Get<Newsletter>().Where(n => n.Id == newsletterId).FirstOrDefault();
                if (newsletter == null)
                    return HttpNotFound("Newsletter not found");
                return View("CreateNewsletter", Mapper.Map<Newsletter, NewsletterView>(newsletter));
            }
        }

        [ValidateInput(false), Authorize(Roles = "Editor"), HttpPost]
        public ActionResult EditNewsletter(NewsletterView newsLetterView)
        {
            if (string.IsNullOrEmpty(newsLetterView.Subject) && string.IsNullOrEmpty(newsLetterView.HtmlBody))
            {
                ViewBag.PageTitle = "Edit Newsletter";
                return View("CreateNewsletter",newsLetterView);
            }

            Newsletter newsletter;
            using (var repository = _repositoryProvider.GetRepository())
            {
                newsletter = repository.Get<Newsletter>().Where(n => n.Id == newsLetterView.Id).FirstOrDefault();
                if (newsletter == null)
                    return HttpNotFound("Newsletter not found");


                newsletter.Subject = newsLetterView.Subject;
                newsletter.HtmlBody = newsLetterView.HtmlBody;
                newsletter.PlainTextBody = newsLetterView.PlainTextBody;
                repository.SaveOrUpdate(newsletter);
                var emailThread = new Thread(_emailService.SendNewsLetter);
                var config = Configuration.DessertHouseConfigurationSection.Current.Newsletters;
                IList<string> subscriberList = GetSubscriberList();

                dynamic obj =
                    new { Newsletter = newsletter, config.FromEmail, config.FromEmailDisplayName, subscriberList };
                emailThread.Start(obj);
                return RedirectToAction("ManageNewsletters",new{pendingUpdate=true});

            }

        }

        [Authorize(Roles = "Editor"), HttpPost]
        public ActionResult RemoveNewsletter(int? newsletterId)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var newsletter = repository.Get<Newsletter>().Where(n => n.Id == newsletterId).FirstOrDefault();
                if (newsletter == null)
                    return Json(new {IsDeleted=false, Error = "News letter not found"});
                repository.Delete<Newsletter>(newsletter);
                return Json(new {IsDeleted = true,Id=newsletterId});
            }
        }

        [Authorize(Roles = "Editor")]
        public ActionResult UpdateStatus()
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var viewData = Mapper.Map<IEnumerable<Newsletter>,IEnumerable<NewsletterView>>(repository.Get<Newsletter>().OrderByDescending(n => n.DateAdded));

                return PartialView("NewsletterStatus", viewData);
            }
        }

    }
}