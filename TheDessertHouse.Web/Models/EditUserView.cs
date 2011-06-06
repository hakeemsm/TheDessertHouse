using System;
using TheDessertHouse.Domain;

namespace TheDessertHouse.Web.Models
{
    public class EditUserView
    {
        public string[] Roles { get; set; }

        public MembershipUserWrapper MembershipUser { get; set; }
    }
}