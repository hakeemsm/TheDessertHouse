using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheDessertHouse.Web.Models
{
    public class DepartmentView
    {
        [Required,Display(Name = "Name")]
        public string Title { get; set; }

        public int Importance { get; set; }

        public string Description { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        public int Id { get; set; }

        public IList<ProductView> Products { get; set; }

        public bool DepartmentExists { get; set; }
    }
}