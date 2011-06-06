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
    public class HomeController : SuperController
    {
        private IRepositoryProvider _repositoryProvider;

        public HomeController(IRepositoryProvider repositoryProvider)
        {
            _repositoryProvider = repositoryProvider;
        }

        

        [OutputCache(Duration = 60)]
        public PartialViewResult Categories()
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var categories = repository.Get<Category>();
                var categoryViews = Mapper.Map<IEnumerable<Category>, IEnumerable<CategoryView>>(categories);
                return PartialView(categoryViews);
                
            }
        }

        public ViewResult Index(int pageNum, string category)
        {
            using (IRepository repository = _repositoryProvider.GetRepository())
            {
                bool isAuthenticated = false;
                if (null != HttpContext && null != HttpContext.User)
                    isAuthenticated = HttpContext.User.Identity.IsAuthenticated;
                int pageSize = DessertHouseConfigurationSection.Current.Articles.PageSize;
                int skip = (pageNum - 1) * pageSize;
                var allArticles = repository.Get<Article>().Where(a => a.Approved &&
                (isAuthenticated || a.OnlyForMembers == false) &&
                 a.Listed && a.ReleaseDate <= DateTime.Now &&
                 a.ExpireDate > DateTime.Now &&
                (category == null || a.ArticleCategory.Path == category));
                var publishedArticles = allArticles.OrderByDescending(a => a.ReleaseDate).Skip(skip).Take(pageSize);
                var articles = Mapper.Map<IEnumerable<Article>, IEnumerable<ArticleView>>(publishedArticles);
                int totalArticles = allArticles.Count();
                var viewItems = new Pagination<ArticleView>(articles, pageNum, pageSize, totalArticles);


                var categories = repository.Get<Category>();
                var categoryViews = Mapper.Map<IEnumerable<Category>, IEnumerable<CategoryView>>(categories);
                if (Session!=null)
                {
                    var poll = repository.Get<Poll>().Where(p => p.IsCurrent).FirstOrDefault();
                    if (poll!=null)
                    {
                        var pollView = Mapper.Map<Poll,PollView>(poll);
                        poll.Options.Each(o => pollView.TotalVotes += o.Votes);
                        Session["CurrentPoll"] = pollView;
                    }
                }

                ViewBag.PageHeader = (string.IsNullOrEmpty(category) ? "All" : category) + " Articles";
                ViewBag.PageTitle = "The Dessert House / " + ViewBag.PageHeader;
                ViewBag.CurrentPage = pageNum;

                return View(new HomeView { Articles = viewItems, Categories = categoryViews });
            }
        }
    }
}
