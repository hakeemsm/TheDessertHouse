using System.Web.Mvc;
using NUnit.Framework;
using TheDessertHouse.Domain;
using TheDessertHouse.Web.Controllers;
using TheDessertHouse.Web.Models;

namespace TheDessertHouse.Tests.ControllerTests
{
    [TestFixture]
    public class CategoryControllerTests : BaseTestController
    {
        [Test]
        public void CreateCategory_Redirects_To_ManageArticles_After_Creating_Category()
        {
            var categoryController = CreateController<CategoryController>();
            var category = new Category { Title = "Pies", Path = "All Pies" };
            MockRepository.Setup(x => x.Save(category));
            var categoryView = new CategoryView { Title = "Pies", Path = "All Pies" };

            ActionResult result = categoryController.CreateCategory(categoryView);

            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());

        }



        [Test]
        public void EditArticle_Returns_HttpNotFound_For_Article_Not_Found()
        {
            MockRepository.Setup(x => x.Get<Category>(1)).Returns(null as Category);
            var categoryController = CreateController<CategoryController>();
            ActionResult result = categoryController.EditCategory(1);

            Assert.That(result, Is.TypeOf<HttpNotFoundResult>());
            Assert.That(((HttpNotFoundResult)result).StatusDescription,
                        Is.EqualTo("The specified category could not be found"));
        }
    }
}
