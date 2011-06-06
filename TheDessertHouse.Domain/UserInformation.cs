using System;

namespace TheDessertHouse.Domain
{
    public class UserInformation
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }
        
        public string HintAnswer { get; set; }

        public string ConfirmPassword { get; set; }

        public string ReturnUrl { get; set; }

        public string SecretQuestion { get; set; }

        public string ChangePassword { get; set; }
    }
}