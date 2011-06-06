using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using AutoMapper;
using TheDessertHouse.Domain;
using TheDessertHouse.Infrastructure;
using TheDessertHouse.Web.Configuration;
using TheDessertHouse.Web.Models;

namespace TheDessertHouse.Web.Controllers
{
    public class ArticleController : SuperController
    {
        private IRepositoryProvider _repositoryProvider;

        public ArticleController(IRepositoryProvider repositoryProvider)
        {
            _repositoryProvider = repositoryProvider;
        }

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }



        [Authorize(Roles = "Contributor")]
        public ActionResult CreateArticle()
        {
            List<Category> categories;
            using (var repository = _repositoryProvider.GetRepository())
            {
                categories = repository.Get<Category>().ToList();
            }
            var articleView = new ArticleView
            {
                Categories = new SelectList(Mapper.Map<List<Category>, List<CategoryView>>(categories), "Id", "Title")
            };
            return View(articleView);
        }

        [HttpPost, Authorize(Roles = "Contributor"), ValidateInput(false)]
        public ActionResult CreateArticle(ArticleView articleView)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                if (!articleView.HasData)
                {
                    var categories = repository.Get<Category>().ToList();
                    articleView.Categories = new SelectList(Mapper.Map<List<Category>, List<CategoryView>>(categories), "Id", "Title");
                    return View(articleView);
                }
                Article article;
                if (articleView.IsDirty)
                {
                    article = Mapper.Map<ArticleView, Article>(articleView);
                    article.AddedBy = HttpContext.User.Identity.Name;
                    var category =
                        repository.Get<Category>().Where(c => c.Id == int.Parse(articleView.CategoryTitle)).
                            FirstOrDefault();
                    article.ArticleCategory = category;
                    category.Articles.Add(article);
                    repository.Save(article);

                    ViewBag.SuccessMessage = "Your article has been posted";
                    return RedirectToAction("ViewArticle", new { articleId = article.Id, path = article.Path });
                }
                return View(articleView);
            }
        }


        [Authorize(Roles = "Editor")]
        public ActionResult EditArticle(int articleId)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var article = repository.Get<Article>(articleId);
                if (null == article)
                    return HttpNotFound("Article with Id " + articleId + " not found");
                var articleView = Mapper.Map<Article, ArticleView>(article);
                var categories = repository.Get<Category>().ToList();
                articleView.Categories = new SelectList(Mapper.Map<List<Category>, List<CategoryView>>(categories), "Id", "Title");
                ViewBag.PageTitle = "Edit Article";
                return View("CreateArticle", articleView);
            }
        }

        [Authorize(Roles = "Editor"), HttpPost, ValidateInput(false)]
        public ActionResult EditArticle(ArticleView articleView)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var article = repository.Get<Article>().Where(a => a.Id == articleView.Id).FirstOrDefault();
                article.CommentsEnabled = articleView.CommentsEnabled;
                article.Approved = articleView.Approved;
                article.Listed = articleView.Listed;
                article.OnlyForMembers = articleView.OnlyForMembers;
                repository.Update(article);
            }
            articleView.IsDirty = false;
            ViewBag.PageTitle = "Edit Article";
            return RedirectToAction("ManageArticles", new { pageNum = 1 });

        }


        public ActionResult ViewArticle(int articleId, string path)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var article = repository.Get<Article>(articleId);
                if (null == article)
                    return HttpNotFound(string.Format("An article with Id {0} is not present", articleId));
                if (!string.Equals(path, article.Path, StringComparison.OrdinalIgnoreCase))
                    return RedirectToActionPermanent("ViewArticle", new { articleId = article.Id, path = article.Path });

                if (article.OnlyForMembers && null != HttpContext.User && !HttpContext.User.Identity.IsAuthenticated)
                    return new HttpStatusCodeResult(401, "You do not have permissions to view this article");
                article.ViewCount++;
                repository.Save(article);
                var articleView = Mapper.Map<Article, ArticleView>(article);
                ViewBag.PageTitle = "The Dessert House / " + articleView.Title;
                return View(articleView);
            }
        }

        [HttpPost]
        public JsonResult RateArticle(int articleId, int rating)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var article = repository.Get<Article>(articleId);
                article.Rate(rating);
                repository.Save(article);

                return Json(Url.Content(new ArticleView{AverageRating = article.AverageRating}.ImageRatingUrl));
            }

        }

        [HttpPost, Authorize(Roles = "Editor")]
        public ActionResult DeleteArticle(int articleId)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var article = repository.Get<Article>(articleId);
                repository.Delete<Article>(article);
                ViewData["Message"] = "Article has been deleted";
            }

            return RedirectToAction("ManageArticles");
        }

        [Authorize(Roles = "Editor")]
        public ActionResult ManageArticles(int pageNum)
        {
            int pageSize = DessertHouseConfigurationSection.Current.Articles.PageSize;
            int skip = (pageNum - 1) * pageSize;
            int articleCount;
            IEnumerable<ArticleView> articleViews;
            using (var repository = _repositoryProvider.GetRepository())
            {
                var articles = repository.Get<Article>();
                articleCount = articles.Count();
                var reqArticles = articles.OrderByDescending(a => a.ReleaseDate).Skip(skip).Take(pageSize);
                articleViews = Mapper.Map<IEnumerable<Article>, IEnumerable<ArticleView>>(reqArticles);
            }
            var model = new Pagination<ArticleView>(articleViews, pageNum, pageSize, articleCount);
            ViewBag.PageTitle = "Manage Articles";
            ViewBag.InformationalMessage = ViewData["Message"];
            return View(model);

        }

        [HttpPost]
        public JsonResult AddComment(int articleId, CommentsView commentsView)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var article = repository.Get<Article>(articleId);
                commentsView.DateAdded = DateTime.Now;
                commentsView.AddedByIP = Request.UserHostAddress;
                var comment = Mapper.Map<CommentsView, Comments>(commentsView);
                comment.ForArticle = article;
                article.Comments.Add(comment);
                repository.Save(article);
                return new JsonResult { Data = commentsView };
            }
        }

        [HttpPost, Authorize(Roles = "Editor")]
        public JsonResult EditComment(int commentId, string body)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var comment = repository.Get<Comments>(commentId);
                comment.Body = body;
                repository.Update(comment);
                return new JsonResult { Data = new { commentId, body } };
            }
        }

        [HttpPost, Authorize(Roles = "Editor")]
        public JsonResult RemoveComment(int commentId)
        {
            var repository = _repositoryProvider.GetRepository();
            var comment = repository.Get<Comments>(commentId);
            repository.Delete<Comments>(comment);
            return new JsonResult { Data = commentId };
        }

        [Authorize(Roles = "Editor")]
        public ActionResult ManageComments(int pageNum)
        {
            int pageSize = DessertHouseConfigurationSection.Current.Articles.PageSize;
            int skip = (pageNum - 1) * pageSize;
            var repository = _repositoryProvider.GetRepository();

            var comments = repository.Get<Comments>();
            var commentList = Mapper.Map<IEnumerable<Comments>, IEnumerable<CommentsView>>(comments.Skip(skip).Take(pageSize));
            ViewBag.PageTitle = "Manage Comments";
            ViewBag.PageNumber = pageNum;
            return View(new Pagination<CommentsView>(commentList, pageNum, pageSize, comments.Count()));
        }

        public JsonResult CheckTitle(string title)
        {
            var titleExists = _repositoryProvider.GetRepository().Get<Article>().Any(
                a => a.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            return new JsonResult { Data = new { titleExists }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
