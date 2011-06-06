using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using TheDessertHouse.Web.Controllers;

namespace TheDessertHouse.Web.Models
{
    public class CategoryView
    {
        [HiddenInput]
        public int Id { get; set; }

        public DateTime DateAdded { get; set; }

        public string AddedBy { get; set; }
        
        [Required]
        public string Title { get; set; }

        public string Path { get; set; }

        public int Importance { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        [HiddenInput]
        public bool CategoryExists { get; set; }

        public bool IsBeingUpdated { get; set; }

        
    }

    
    
}