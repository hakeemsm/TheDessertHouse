using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TheDessertHouse.Web.Models
{
    public class ProductView
    {
        public DateTime DateAdded { get; set; }

        public string AddedBy { get; set; }

        [Required,Display(Name = "Product Name")]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string SKU { get; set; }

        [Required,Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Discount Percentage")]
        public decimal DiscountPercentage { get; set; }

        [Display(Name = "Units in stock")]
        public int UnitsInStock { get; set; }

        [Display(Name = "Thumbnail image")]
        public string SmallImageUrl { get; set; }

        [Display(Name = "Full image")]
        public string FullImageUrl { get; set; }

        public int Id { get; set; }

        [Display(Name = "Department")]
        public int CategoryId { get; set; }

        public SelectList Departments { get; set; }

        public bool ProductExists { get; set; }
    }
}