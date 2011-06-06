using System;
using System.Collections.Generic;

namespace TheDessertHouse.Web.Models
{
    public class ForumView
    {
        public DateTime DateAdded { get; set; }

        public string AddedBy { get; set; }

        public string Title { get; set; }

        public string Path { get; set; }

        public bool Moderated { get; set; }

        public int Importance { get; set; }

        public string Description { get; set; }

        public int Id { get; set; }

        public IList<ForumPostView> Posts { get; set; }

        public bool ForumExists { get; set; }
    }
}