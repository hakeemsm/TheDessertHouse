using System.Web;
using System.Web.Mvc;
using NUnit.Framework;
using TheDessertHouse.Domain;
using TheDessertHouse.Web.Controllers;

namespace TheDessertHouse.Tests.ControllerTests
{
    [TestFixture]
    public class ArticleControllerTests:BaseTestController
    {

        [Test]
        public void ViewArticle_Returns_HttpNotFound_For_Article_Not_Found()
        {
            MockRepository.Setup(x => x.Get<Article>(1)).Returns(null as Article);
            var articleController = CreateController<ArticleController>();
            var result = articleController.ViewArticle(1, "");

            Assert.That(result, Is.TypeOf<HttpNotFoundResult>());
            Assert.That(((HttpNotFoundResult) result).StatusDescription,
                        Is.EqualTo("An article with Id 1 is not present"));
        }

        [Test]
        public void ViewArticle_Sets_Permanent_Redirection_For_Invalid_Path()
        {
            var article=new Article{Path="Correct Path"};
            MockRepository.Setup(x => x.Get<Article>(1)).Returns(article);
            var articleController = CreateController<ArticleController>();
            var result = articleController.ViewArticle(1, "Incorrect Path");

            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());
            Assert.That(((RedirectToRouteResult) result).Permanent);
            Assert.That(((RedirectToRouteResult) result).RouteValues.ContainsKey("articleId"));
            Assert.That(((RedirectToRouteResult) result).RouteValues.ContainsKey("path"));
            Assert.That(((RedirectToRouteResult)result).RouteValues.ContainsValue("Correct Path"));

        }

        
    }
}
