using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using TheDessertHouse.Domain;
using TheDessertHouse.Web;
using TheDessertHouse.Web.Controllers;
using TheDessertHouse.Web.Models;

namespace TheDessertHouse.Tests.ControllerTests
{
    [TestFixture]
    public class HomeControllerTests : BaseTestController
    {

        [Test]
        public void Index_Action_Returns_Articles_For_All_Categories_If_No_Category_Passed()
        {

            var articles = new List<Article>
                               {
                                   new Article
                                       {
                                           Id = 1,
                                           Title = "Bake a cake",
                                           ArticleCategory = new Category {Id = 101, Title = "Cakes"},
                                           Approved = true,
                                           OnlyForMembers = false,
                                           Listed = true,
                                           ReleaseDate = DateTime.Now.AddDays(-5),
                                           ExpireDate = DateTime.Now.AddDays(7)
                                       },
                                   new Article
                                       {
                                           Id = 2,
                                           Title = "Slurpy Ice creams",
                                           ArticleCategory = new Category {Id = 201, Title = "Ice creams"},
                                           Approved = true,
                                           OnlyForMembers = false,
                                           Listed = true,
                                           ReleaseDate = DateTime.Now.AddDays(-5),
                                           ExpireDate = DateTime.Now.AddDays(7)
                                       }
                               }.AsQueryable();

            MockRepository.Setup(m => m.Get<Article>()).Returns(articles);
            var homeController = CreateController<HomeController>();
            var viewResult = homeController.Index(1,null);

            //Check Articles set in Viewbag
            Assert.That(homeController.ViewBag.PageTitle, Is.EqualTo("The Dessert House / All Articles"));
            Assert.That(viewResult.Model, Is.TypeOf<HomeView>());
            var articleViews = ((HomeView) viewResult.Model).Articles;
            
            Assert.That(articleViews.TotalItemCount, Is.EqualTo(2));
            Assert.That(articleViews.HasNext, Is.False);
            Assert.That(articleViews.Where(a=>a.Id==1).FirstOrDefault().Title, Is.EqualTo("Bake a cake"));


        }

        [Test]
        public void Index_Action_Returns_Articles_For_Selected_Category()
        {
            var articles = new List<Article>
                               {
                                   new Article
                                       {
                                           Id = 1,
                                           Title = "Bake a cake",
                                           ArticleCategory = new Category {Id = 101, Title = "Cakes",Path = "Cake"},
                                           Approved = true,
                                           OnlyForMembers = false,
                                           Listed = true,
                                           ReleaseDate = DateTime.Now.AddDays(-5),
                                           ExpireDate = DateTime.Now.AddDays(7)
                                       },
                                   new Article
                                       {
                                           Id = 2,
                                           Title = "Bake 3 cakes",
                                           ArticleCategory = new Category {Id = 101, Title = "Cakes",Path = "Cake"},
                                           Approved = true,
                                           OnlyForMembers = false,
                                           Listed = true,
                                           ReleaseDate = DateTime.Now.AddDays(-5),
                                           ExpireDate = DateTime.Now.AddDays(7)
                                       }

                               }.AsQueryable();

            MockRepository.Setup(m => m.Get<Article>()).Returns(articles);
            var homeController = CreateController<HomeController>();
            var viewResult = homeController.Index(1, "Cake");

            //Check Articles set in Viewbag
            Assert.That(homeController.ViewBag.PageTitle, Is.EqualTo("The Dessert House / Cake Articles"));
            Assert.That(viewResult.Model, Is.TypeOf<HomeView>());
            var articleViews = ((HomeView) viewResult.Model).Articles;
            Assert.That(articleViews.TotalItemCount, Is.EqualTo(2));
            Assert.That(articleViews.TotalPageCount, Is.EqualTo(1));
            Assert.That(articleViews.OrderByDescending(a=>a.Id).FirstOrDefault().Id, Is.EqualTo(2));
            Assert.That(articleViews.LastOrDefault().Title, Is.EqualTo("Bake 3 cakes"));
        }

        [Test]
        public void Pagination_Returns_Page_Number_Collection()
        {
            var articles = new List<ArticleView>(GetArticles(15));

            var paginator = new Pagination<ArticleView>(articles, 0, 10, articles.Count);

            Assert.That(paginator.HasPrevious, Is.False);
            Assert.That(paginator.HasNext, Is.True);
            Assert.That(paginator.TotalPageCount, Is.EqualTo(2));

            foreach (var articleView in paginator)
                Assert.That(articles.Contains(articleView));
        }

        [Test]
        public void Pagination_Returns_True_For_Previous_If_CurrentPage_Greater_Than_1()
        {
            var articles = new List<ArticleView>(GetArticles(34));

            var paginator = new Pagination<ArticleView>(articles.Skip(10).Take(10), 2, 10, articles.Count);

            Assert.That(paginator.HasPrevious, Is.True);
            Assert.That(paginator.HasNext, Is.True);
            Assert.That(paginator.TotalPageCount, Is.EqualTo(4));


        }

        private IEnumerable<ArticleView> GetArticles(int count)
        {
            var articles = new List<ArticleView>();
            for (int i = 0; i < count; i++)
                articles.Add(new ArticleView { Id = i, Title = "Article" + i });
            return articles;
        }

        [Test]
        public void Index_Returns_Category_List()
        {
            var articles = new List<Article>
                               {
                                   new Article
                                       {
                                           Id = 1,
                                           Title = "Bake a cake",
                                           ArticleCategory = new Category {Id = 101, Title = "Cakes",Path = "Cake"},
                                           Approved = true,
                                           OnlyForMembers = false,
                                           Listed = true,
                                           ReleaseDate = DateTime.Now.AddDays(-5),
                                           ExpireDate = DateTime.Now.AddDays(7)
                                       },
                                   new Article
                                       {
                                           Id = 2,
                                           Title = "Bake 3 cakes",
                                           ArticleCategory = new Category {Id = 101, Title = "Cakes",Path = "Cake"},
                                           Approved = true,
                                           OnlyForMembers = false,
                                           Listed = true,
                                           ReleaseDate = DateTime.Now.AddDays(-5),
                                           ExpireDate = DateTime.Now.AddDays(7)
                                       }

                               }.AsQueryable();

            var categoryList = new List<Category>
                                    {
                                        new Category {Id = 101,Title="Pies"},
                                        new Category {Id = 201, Title = "Cakes"}
                                    }.AsQueryable();
            MockRepository.Setup(m => m.Get<Article>()).Returns(articles);
            MockRepository.Setup(m => m.Get<Category>()).Returns(categoryList);

            var viewResult = CreateController<HomeController>().Index(1,null);
            var categories = ((HomeView)viewResult.Model).Categories;

            Assert.That(categories, Is.TypeOf<List<CategoryView>>());
            Assert.That(categories.Count(), Is.EqualTo(2));
            Assert.That(categories.FirstOrDefault().Title, Is.EqualTo("Pies"));

        }
    }
}
