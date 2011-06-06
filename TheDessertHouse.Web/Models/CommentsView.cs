using System;
using System.ComponentModel.DataAnnotations;
using TheDessertHouse.Domain;

namespace TheDessertHouse.Web.Models
{
    public class CommentsView
    {
        [Required,Display(Name = "Name")]
        public string AddedBy { get; set; }

        [Required,Display(Name = "Email")]
        public string AddedByEmail { get; set; }

        [Required]
        public string Body { get; set; }

        public DateTime DateAdded { get; set; }

        public string AddedByIP { get; set; }

        public Article ForArticle { get; set; }

        public int Id { get; set; }
    }
}