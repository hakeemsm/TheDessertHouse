using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheDessertHouse.Web.Models
{
    public class HomeView
    {
        public IEnumerable<CategoryView> Categories { get; set; }

        public Pagination<ArticleView> Articles { get; set; }
    }
}