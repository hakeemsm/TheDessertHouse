using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheDessertHouse.Web.Models
{
    public class LoginViewModel:IValidatableObject
    {
        
        public string ReturnUrl { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        [Display(Name="Remember me",Description="Remember")]
        public bool Persistent { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            string errMsg = "{0} is required";
            if (string.IsNullOrEmpty(UserName))
                yield return new ValidationResult(string.Format(errMsg, "User name"));
            if (string.IsNullOrEmpty(Password))
                yield return new ValidationResult(string.Format(errMsg, "Password"));
        }
    }
}