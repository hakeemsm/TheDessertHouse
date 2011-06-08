using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using FluentNHibernate.Utils;
using TheDessertHouse.Domain;
using TheDessertHouse.Infrastructure;
using TheDessertHouse.Web.Configuration;
using TheDessertHouse.Web.Models;

namespace TheDessertHouse.Web.Controllers
{
    public class ForumController : SuperController
    {
        private readonly IRepositoryProvider _repositoryProvider;
        private IMembershipProvider _membershipProvider;

        public ForumController(IRepositoryProvider repositoryProvider, IMembershipProvider membershipProvider)
        {
            _repositoryProvider = repositoryProvider;
            _membershipProvider = membershipProvider;
        }

        public ActionResult Index()
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                ViewBag.PageTitle = "Forums";
                var forums = repository.Get<Forum>();
                return View(Mapper.Map<IEnumerable<Forum>, IEnumerable<ForumView>>(forums));
            }
        }

        public ActionResult ViewForum(int forumId, string path, int pageNum = 1)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var forum = repository.Get<Forum>().Where(f => f.Id == forumId).FirstOrDefault();
                if (forum == null)
                    return HttpNotFound("No forum found with Id " + forumId);
                if (!string.Equals(path, forum.Path, StringComparison.OrdinalIgnoreCase))
                    return RedirectToActionPermanent("ViewForum", new { forumId, path = forum.Path });
                int pageSize = DessertHouseConfigurationSection.Current.Forums.ForumPageSize;
                var approvedPosts = forum.Posts.Where(f => f.Approved).Skip((pageNum - 1) * pageSize).Take(pageSize);
                var posts = Mapper.Map<IEnumerable<ForumPost>, IEnumerable<ForumPostView>>(approvedPosts.ToList());
                posts.Each(p =>
                {
                    p.Avatar = GetLastPostByAvatarUrl(p, 16);
                    if (p.ReplyCount == 0)
                        p.PagedReplies = new Pagination<ForumPostReplyView>(p.Replies, 0, 0, 0);
                });
                var forumPosts = new Pagination<ForumPostView>(posts.ToList(), pageNum, pageSize, forum.Posts.Count);
                ViewBag.PageTitle = string.Format("{0} Forum", forum.Title);
                ViewBag.ForumId = forumId;
                return View(forumPosts);
            }
        }

        private string GetLastPostByAvatarUrl(ForumPostViewBase post, int size)
        {
            var user = _membershipProvider.GetUserByUserName(post.LastPostBy);
            var identity = post.AddedByIP;
            if (user != null && !string.IsNullOrEmpty(user.Email))
                identity = user.Email.ToLower();
            return string.Format("http://www.gravatar.com/avatar/{0}?s={1}&d=identicon", identity.GetHashCode(), size);
        }


        public ActionResult ViewPost(int postId, string path, int pageNum = 1)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var post = repository.Get<ForumPost>().Where(f => f.Id == postId).FirstOrDefault();
                if (post == null)
                    return HttpNotFound("No post found with Id " + postId);
                if (!string.Equals(path, post.Path, StringComparison.OrdinalIgnoreCase))
                    return RedirectToActionPermanent("ViewForum", new { postId, path = post.Path });
                post.ViewCount++;
                
                int pageSize = DessertHouseConfigurationSection.Current.Forums.PostReplyPageSize;
                var forumPostView = Mapper.Map<ForumPost, ForumPostView>(post);
                var replies = post.Replies.Where(r=>r.Approved).Skip((pageNum - 1) * pageSize).Take(pageSize);
                forumPostView.Replies = Mapper.Map<IEnumerable<ForumPost>, IEnumerable<ForumPostReplyView>>(replies);
                var postReplies = new Pagination<ForumPostReplyView>(forumPostView.Replies, pageNum, pageSize, post.Replies.Count);
                postReplies.Each(p => p.Avatar = GetLastPostByAvatarUrl(p, 16));
                forumPostView.PagedReplies = postReplies;
                post.Replies.Each(p=>p.ViewCount++);
                repository.Save(post);
                forumPostView.Avatar = GetLastPostByAvatarUrl(forumPostView, 16);
                var vote =
                    repository.Get<ForumPostVote>().Where(v => v.Post.Id == postId && v.AddedBy == User.Identity.Name).
                        SingleOrDefault();
                forumPostView.UserVote = vote == null ? 0 : vote.Direction;
                ViewBag.PageTitle = string.Format("{0} Forum", post.Title);
                return View(forumPostView);
            }
        }

        [Authorize(Roles = "Editor")]
        public ActionResult CreateForum()
        {
            ViewBag.PageTitle = "New Forum";
            return View(new ForumView());
        }

        [Authorize(Roles = "Editor"), HttpPost]
        public ActionResult CreateForum(ForumView forumView)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var forum = Mapper.Map<ForumView, Forum>(forumView);
                forum.AddedBy = HttpContext.User.Identity.Name;
                forum.DateAdded = DateTime.Now;
                forum.Path = forumView.Title.ToUrlFormat();
                repository.SaveOrUpdate(forum);
                ViewBag.ExistingForum = forumView.ForumExists;
                return RedirectToAction("ManageForums");
            }
        }

        [Authorize(Roles = "Editor")]
        public ActionResult ManageForums()
        {
            using (var repostory = _repositoryProvider.GetRepository())
            {
                var viewModel = Mapper.Map<IEnumerable<Forum>, IEnumerable<ForumView>>(repostory.Get<Forum>());
                ViewBag.PageTitle = "Manage Forums";
                return View(viewModel);
            }
        }

        [Authorize(Roles = "Editor")]
        public ActionResult ManagePosts()
        {
            using (var repostory = _repositoryProvider.GetRepository())
            {
                var viewModel = Mapper.Map<IEnumerable<ForumPost>, IEnumerable<ForumPostView>>(repostory.Get<ForumPost>().Where(f=>!f.Approved));
                ViewBag.PageTitle = "Manage Posts";
                return View(viewModel);
            }
        }

        [Authorize(Roles = "Editor")]
        public ActionResult EditForum(int forumId)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var forum = repository.Get<Forum>().Where(f => f.Id == forumId).FirstOrDefault();
                if (forum == null)
                    return HttpNotFound("No forum found with the specified Id");
                var forumView = Mapper.Map<Forum, ForumView>(forum);
                forumView.ForumExists = true;
                ViewBag.PageTitle = "Edit Forum " + forumView.Title;
                return View("CreateForum", forumView);
            }
        }

        [Authorize(Roles = "Editor")]
        public ActionResult RemoveForum(int forumId)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var forum = repository.Get<Forum>().Where(f => f.Id == forumId).FirstOrDefault();
                if (forum == null)
                    return HttpNotFound("No forum found with the specified Id");
                repository.Delete<Forum>(forum);
                return Json(new { Id = forumId, deleted = true });
            }
        }

        [Authorize(Roles = "Editor")]
        public ActionResult RemovePost(int postId)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var post = repository.Get<ForumPost>().Where(f => f.Id == postId).SingleOrDefault();
                if (post == null)
                    return HttpNotFound("No post found with the specified Id");
                repository.Delete<ForumPost>(post);
                return Json(new { Id = postId, deleted = true });
            }
        }

        [Authorize(Roles = "Editor")]
        public ActionResult CheckForumTitle(string title)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var forum = repository.Get<Forum>().Where(f => f.Title == title).FirstOrDefault();
                return Json(new { Exists = forum != null, Title = title }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult CreatePost(int? forumId, int? postId, string title, string post)
        {
            if ((!forumId.HasValue && !postId.HasValue) || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(post))
                throw new ArgumentException("Invaid action parameters");
            using (var repository = _repositoryProvider.GetRepository())
            {
                var postItem = new ForumPost
                                   {
                                       AddedBy = HttpContext.User.Identity.Name,
                                       AddedByIP = Request.UserHostAddress,
                                       Body = post,
                                       DateAdded = DateTime.Now,
                                       LastPostBy = User.Identity.Name,
                                       LastPostDate = DateTime.Now,
                                       Title = title,
                                       Path = title.ToUrlFormat()
                                   };
                if (forumId.HasValue)
                {
                    var forum = repository.Get<Forum>().Where(f => f.Id == forumId).FirstOrDefault();
                    if (forum == null)
                        return Json(false);
                    postItem.Approved = !forum.Moderated;
                    postItem.Forum = forum;
                    forum.Posts.Add(postItem);
                    repository.Save(forum);


                }
                else
                {
                    var forumPost = repository.Get<ForumPost>().Where(p => p.Id == postId).FirstOrDefault();
                    if (forumPost == null)
                        return Json(false);
                    var forum = forumPost.Forum;
                    postItem.Approved = !forum.Moderated;
                    postItem.ParentPostId = forumPost.Id;
                    forumPost.Replies.Add(postItem);
                    forumPost.ReplyCount++;
                    repository.Save(forumPost);

                }
                var replyView = Mapper.Map<ForumPost, ForumPostReplyView>(postItem);
                replyView.Avatar = GetLastPostByAvatarUrl(replyView, 16);
                return Json(replyView);
            }
        }

        [HttpPost]
        public ActionResult CastVote(int postId, int vote)
        {
            if (!User.Identity.IsAuthenticated)
                return Json(new { error = "Not authenticated" });

            using (var repository = _repositoryProvider.GetRepository())
            {
                var forumPost = repository.Get<ForumPost>().Where(f => f.Id == postId).SingleOrDefault();
                if (forumPost == null)
                    return Json(new { error = "notfound" });
                var userVote = forumPost.Votes.Where(v => v.AddedBy == User.Identity.Name).SingleOrDefault();
                if (userVote != null)
                    return Json(new { error = "Already voted" });
                forumPost.Votes.Add(new ForumPostVote { AddedBy = User.Identity.Name, AddedByIP = Request.UserHostAddress, DateAdded = DateTime.Now, Post = forumPost, Direction = vote });
                forumPost.VoteCount += vote;
                repository.Save(forumPost);
                return Json(new { forumPost.VoteCount, forumPost.Id, Direction = vote });
            }
        }

        [HttpPost]
        public ActionResult ApprovePost(int postId, bool approved)
        {
            using (var repository=_repositoryProvider.GetRepository())
            {
                var forumPost = repository.Get<ForumPost>().Where(f => f.Id == postId).SingleOrDefault();
                if (forumPost == null)
                    return Json(new {error = "Post not found with the specified Id"});
                forumPost.Approved = true;
                repository.Save(forumPost);
                return Json(new {Id = postId});
            }
        }
    }
}