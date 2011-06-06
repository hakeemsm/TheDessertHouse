using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NUnit.Framework;
using TheDessertHouse.Domain;
using TheDessertHouse.Web.Controllers;

namespace TheDessertHouse.Tests.ControllerTests
{
    [TestFixture]
    public class ForumControllerTests:BaseTestController
    {
        [Test]
        public void ViewForum_Redirects_To_The_Correct_Forum_For_An_Incorrect_Path()
        {
            var forums = new List<Forum>{new Forum{Id = 1,Path = "sweet_path"}}.AsQueryable();
            MockRepository.Setup(x => x.Get<Forum>()).Returns(forums);
            var forumController = CreateController<ForumController>();

            ActionResult result= forumController.ViewForum(1, "sweetest_path");

            Assert.That(result,Is.TypeOf<RedirectToRouteResult>());
            Assert.That(((RedirectToRouteResult)result).RouteValues.ContainsKey("path"));
            Assert.That(((RedirectToRouteResult)result).RouteValues.ContainsValue("sweet_path"));
        }

        [Test]
        public void ViewPost_Redirects_To_The_Correct_Post_For_An_Incorrect_Path()
        {
            var forums = new List<ForumPost> { new ForumPost { Id = 1, Path = "happy_path" } }.AsQueryable();
            MockRepository.Setup(x => x.Get<ForumPost>()).Returns(forums);
            var forumController = CreateController<ForumController>();

            ActionResult result = forumController.ViewPost(1, "not_so_happy_path",1);

            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());
            Assert.That(((RedirectToRouteResult)result).RouteValues.ContainsKey("path"));
            Assert.That(((RedirectToRouteResult)result).RouteValues.ContainsValue("happy_path"));
        }
    }
}
