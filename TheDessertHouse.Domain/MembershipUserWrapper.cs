using System;

namespace TheDessertHouse.Domain
{
    public class MembershipUserWrapper
    {
        public string SecretQuestion { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public bool IsOnline { get; set; }

        public bool IsLockedOut { get; set; }

        public bool IsApproved { get; set; }

        public DateTime LastActivityDate { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastLoginDate { get; set; }
    }
}