using System;
using Moq;
using NUnit.Framework;
using TheDessertHouse.Domain;
using TheDessertHouse.Infrastructure;
using TheDessertHouse.Services;
using TheDessertHouse.Services.ControllerTests;
using TheDessertHouse.Web.Controllers;

namespace TheDessertHouse.Tests.ControllerTests
{
    [TestFixture]
    public class NewsletterControllerTests:BaseTestController
    {
        [Test]
        public void CreateNewsLetter_Should_Send_Newsletter_To_All_Subscribers()
        {
            IEmailService emailService = new EmailService();
            var mockRepositoryProvider = new Mock<IRepositoryProvider>();
            const string htmlBody = "<html><head><title>The Dessert House Newsletter</title></head><body>Greetings from the DessertHouse!</body></html>";
            var newsletter = new Newsletter
                                 {
                                     AddedBy = "Tester",
                                     DateAdded = DateTime.Now.AddHours(-1),
                                     HtmlBody = htmlBody,
                                     PlainTextBody = "Greetings",
                                     Subject = "Hot and sweet",
                                     Status = "Not Sent"
                                 };
            mockRepositoryProvider.Setup(
                x =>
                x.GetRepository().Save(newsletter));
            var newsletterController = new NewsletterController(mockRepositoryProvider.Object, emailService, null);
            //newsletterController.CreateNewsLetter(newsletter,)
        }
    }
}
