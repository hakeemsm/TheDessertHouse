using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Profile;
using TheDessertHouse.Web.Models;

namespace TheDessertHouse.Web.Controllers
{
    public class ProfileController : Controller
    {
        //
        // GET: /Profile/

        public ActionResult Index()
        {
            return RedirectToAction("UserProfile");
        }

        [Authorize]
        public ActionResult UserProfile()
        {
            ProfileBase profileBase = HttpContext.Profile;
            var profileInformation = new ProfileInformation
            {
                FirstName = profileBase.GetPropertyValue("PersonalInformation.FirstName").ToString(),
                LastName =  profileBase.GetPropertyValue("PersonalInformation.LastName").ToString(),
                BirthDate = (DateTime)profileBase.GetPropertyValue("PersonalInformation.BirthDate"),
                City = profileBase.GetPropertyValue("ContactInformation.City").ToString(),
                State = profileBase.GetPropertyValue("ContactInformation.State").ToString(),
                Zipcode = profileBase.GetPropertyValue("ContactInformation.ZipCode").ToString()
            };
            var webSite = profileBase.GetPropertyValue("PersonalInformation.Website");
            profileInformation.Website = webSite == null ? "" : webSite.ToString();
            var streetAddr = profileBase.GetPropertyValue("ContactInformation.Street");
            profileInformation.Street = streetAddr == null ? "" : streetAddr.ToString();
            
            profileInformation.SubscriptionType = GetSubscriptionList(GetProfileValue("Subscription", profileBase));
            profileInformation.Language = GetLanguageList(GetProfileValue("Language", profileBase));
            profileInformation.Country = GetCountryList(GetProfileValue("ContactInformation.Country", profileBase));
            profileInformation.GenderType = GetGenderList(GetProfileValue("PersonalInformation.Gender", profileBase));

            ViewBag.PageTitle = "Manage Profile";

            return View(profileInformation);
        }

        [Authorize]
        [HttpPost]
        public ActionResult UserProfile(ProfileInformation profileInformation)
        {
            var profileBase = HttpContext.Profile;
            profileBase.SetPropertyValue("Subscription", profileInformation.SubscriptionField);
            profileBase.SetPropertyValue("Language", profileInformation.LanguageField);

            profileBase.SetPropertyValue("PersonalInformation.FirstName", profileInformation.FirstName);
            profileBase.SetPropertyValue("PersonalInformation.LastName", profileInformation.LastName);
            profileBase.SetPropertyValue("PersonalInformation.Gender", profileInformation.Gender);
            profileBase.SetPropertyValue("PersonalInformation.BirthDate", profileInformation.BirthDate);
            profileBase.SetPropertyValue("PersonalInformation.Occupation", profileInformation.Occupation);
            profileBase.SetPropertyValue("PersonalInformation.Website", profileInformation.Website);

            profileBase.SetPropertyValue("ContactInformation.Street", profileInformation.Street);
            profileBase.SetPropertyValue("ContactInformation.City", profileInformation.City);
            profileBase.SetPropertyValue("ContactInformation.State", profileInformation.State);
            profileBase.SetPropertyValue("ContactInformation.ZipCode", profileInformation.Zipcode);
            profileBase.SetPropertyValue("ContactInformation.Country", profileInformation.CountryField);
            profileBase.Save();

            profileInformation.SubscriptionType = GetSubscriptionList(GetProfileValue("Subscription", profileBase));
            profileInformation.Language = GetLanguageList(GetProfileValue("Language", profileBase));
            profileInformation.Country = GetCountryList(GetProfileValue("ContactInformation.Country", profileBase));
            profileInformation.GenderType = GetGenderList(GetProfileValue("PersonalInformation.Gender", profileBase));

            ViewBag.SuccessMessage = "Your profile information has been saved";
            ViewBag.PageTitle = "My Profile";
            return View(profileInformation);

        }

        public ActionResult CheckBirthDate(string birthDateField)
        {
            var retVal = false;
            var birthDay = Convert.ToDateTime(birthDateField);
            var yearDiff = DateTime.Now.Year - birthDay.Year;
            if (yearDiff > 18)
                retVal = true;
            else if (yearDiff==18)
            {
                retVal = true;
                if ((DateTime.Now.Month == birthDay.Month && DateTime.Now.Day < birthDay.Day) ||
                    (DateTime.Now.Month < birthDay.Month))
                    retVal = false;
                
            }
            return Json(retVal, JsonRequestBehavior.AllowGet);
        }

        private string GetProfileValue(string profileValue, ProfileBase profileBase)
        {
            string subsType = null;
            if (profileBase.GetPropertyValue(profileValue) != null)
                subsType = profileBase.GetPropertyValue(profileValue).ToString();
            return subsType;
        }

        public static SelectList GetSubscriptionList(string property)
        {
            var subscriptionList = new List<SelectListItem>
                                       {
                new SelectListItem { Value = "HTML", Text = "Subscribe to HTML Version" },
                new SelectListItem { Value = "Plain", Text = "Subscribe to Plain Text Version" },
                new SelectListItem { Value = "None", Text = "No Thanks" }
            };
            return new SelectList(subscriptionList, "Value", "Text", property ?? "HTML");
        }

        public static SelectList GetGenderList(string property)
        {
            var genderList = new List<SelectListItem>
                                 {
                new SelectListItem { Value = "M", Text = "Male" },
                new SelectListItem { Value = "F", Text = "Female" }
            };
            return new SelectList(genderList, "Value", "Text", property ?? "M");
        }

        public static SelectList GetLanguageList(string property)
        {
            var languages = new List<SelectListItem>
                                {
                                    new SelectListItem {Value = "ENG", Text = "English"},
                                    new SelectListItem {Value = "FRA", Text = "French"},
                                    new SelectListItem {Value = "ESP", Text = "Spanish"}
                                };
            return new SelectList(languages, "Value", "Text", property ?? "ENG");
        }

        public static SelectList GetOccupationList(string property)
        {
            var occupations = new List<SelectListItem>
                                {
                                    new SelectListItem {Value = "Dev", Text = "Developer"},
                                    new SelectListItem {Value = "QA", Text = "Tester"},
                                    new SelectListItem {Value = "Arc", Text = "Architect"}
                                };
            return new SelectList(occupations, "Value", "Text", property ?? "Dev");
        }

        public static SelectList GetCountryList(string property)
        {
            var countries = new List<SelectListItem>
                                {
                                    new SelectListItem {Value = "USA", Text = "USA"},
                                    new SelectListItem {Value = "CA", Text = "Canada"},
                                    new SelectListItem {Value = "UK", Text = "United Kingdom"}
                                };
            return new SelectList(countries, "Value", "Text", property ?? "USA");

        }
    }

    public class ProfileAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (null == filterContext)
                throw new ArgumentException("filterContext is null");

            base.OnAuthorization(filterContext);
        }
    }
}
