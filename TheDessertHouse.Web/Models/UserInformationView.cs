using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheDessertHouse.Web.Models
{
    public class UserInformationView : IValidatableObject
    {
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        [Display(Name = "Answer")]
        public string HintAnswer { get; set; }

        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        public string ReturnUrl { get; set; }

        [Display(Name = "Password hint")]
        public string SecretQuestion { get; set; }

        [Display(Name = "New Password")]
        public string ChangePassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (!string.IsNullOrEmpty(ChangePassword))
            {
                if (!ChangePassword.Equals(ConfirmPassword))
                    yield return
                        new ValidationResult("Passwords don't match",
                                             new List<string> {"ChangePassword", "ConfirmPassword"});
            }
            else
            {
                if (!Password.Equals(ConfirmPassword))
                    yield return
                        new ValidationResult("Passwords should match", new List<string> { "Password", "ConfirmPassword" });
            }
        }
    }


}