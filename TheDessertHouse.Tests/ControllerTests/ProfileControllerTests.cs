using System;
using System.Web.Mvc;
using NUnit.Framework;
using TheDessertHouse.Web.Controllers;
using TheDessertHouse.Web.Models;

namespace TheDessertHouse.Tests
{
    [TestFixture]
    public class ProfileControllerTests
    {
        [Test,Ignore]
        public void User_Profile_Data_Is_Set_In_View_Model()
        {

            var profileController = new ProfileController();
            new AspRuntimeMocks(profileController);
            //profileController.HttpContext.Profile.SetPropertyValue("Subscription","");
            
            ActionResult result = profileController.UserProfile();
            Assert.That(profileController.ViewBag.SubscriptionType, Is.Not.Null);
            Assert.That(((ViewResult)result).ViewData.Model, Is.TypeOf<ProfileInformation>());
        }

        [Test]
        public void User_Profile_Action_Has_Authorize_Attribute_Set()
        {
            var profileController = new ProfileController();
            Assert.That(Attribute.IsDefined(profileController.GetType().GetMethod("UserProfile",Type.EmptyTypes),typeof (AuthorizeAttribute)));
            Assert.That(Attribute.IsDefined(profileController.GetType().GetMethod("UserProfile",new[]{typeof(ProfileInformation)}),typeof (AuthorizeAttribute)));
        }
    }
}
