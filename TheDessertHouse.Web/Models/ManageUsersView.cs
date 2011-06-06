using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TheDessertHouse.Domain;

namespace TheDessertHouse.Web.Models
{
    public class ManageUsersView
    {
        [Display(Name = "Search by")]
        public SelectList SearchOptionList { get; set; }

        public IPagination<MembershipUserWrapper> UserList { get; set; }

        public int RegisteredUsers { get; set; }

        public int UsersOnline { get; set; }

        [Display(Name = "Search for")]
        public string SearchInput { get; set; }

        public string SearchType { get; set; }
    }
}