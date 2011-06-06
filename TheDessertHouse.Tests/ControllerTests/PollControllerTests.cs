using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Moq;
using NUnit.Framework;
using TheDessertHouse.Domain;
using TheDessertHouse.Web;
using TheDessertHouse.Web.Controllers;
using TheDessertHouse.Web.Models;
using System.Collections.Generic;

namespace TheDessertHouse.Tests.ControllerTests
{
    [TestFixture]
    public class PollControllerTests:BaseTestController
    {

        [Test,ExpectedException(typeof(HttpException))]
        public void Poll_Index_Throws_Exception_For_Archived_Poll_For_Non_Editors()
        {
            var pollController = CreateController<PollController>();
            var result = pollController.Index(true, 1);
        }

        [Test]
        public void Index_Returns_All_Non_Archived_Polls()
        {
            var polls =
                new List<Poll>
                    {
                        new Poll
                            {
                                AddedBy = "admin",
                                DateAdded = DateTime.Now.AddHours(-2),
                                IsArchived = false,
                                IsCurrent = true,
                                PollQuestion = "blah"
                            }
                    }.AsQueryable();
            MockRepository.Setup(x => x.Get<Poll>()).Returns(polls);
            var pollController = CreateController<PollController>();
            var result = pollController.Index(false, 1);

            Assert.That(result.Model,Is.TypeOf<Pagination<PollView>>());
            Assert.That(((Pagination<PollView>)result.Model).Count(), Is.EqualTo(1));
        }


    }
}
