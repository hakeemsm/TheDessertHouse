using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TheDessertHouse.Web.Models
{
    public class ProfileInformation
    {
        [Display(Name = "First Name"),Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name"),Required]
        public string LastName { get; set; }
        
        public DateTime BirthDate { get; set; }

        public string Website { get; set; }

        public string Street { get; set; }
        
        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        
        public SelectList Country { get; set; }
        
        public string CountryField { get; set; }

        [Display(Name = "Zip"),Required]
        public string Zipcode { get; set; }
        

        [Display(Name = "Subscription Type")]
        public string SubscriptionField { get; set; }
        
        public SelectList SubscriptionType { get; set; }


        public SelectList Language { get; set; }
        
        [Display(Name = "Language")]
        public string LanguageField { get; set; }

        
        public string Gender { get; set; }

        public SelectList GenderType { get; set; }

        public string Occupation { get; set; }


        [Display(Name = "Date of birth"), Required] /*Remote("CheckBirthDate", "Profile", ErrorMessage = "You must be 18 or older")*/
        public string BirthDateField
        {
            get
            {
                return BirthDate == default(DateTime) ? "" : BirthDate.ToString("MM/dd/yyyy");
            }
            set { BirthDate = Convert.ToDateTime(value); }
            
        }
    }
}