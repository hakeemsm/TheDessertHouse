using System;
using System.Collections.Generic;

namespace TheDessertHouse.Domain
{
    public class Category:IEntity,CommonEntity
    {
        public virtual int Id { get; set; }

        public virtual DateTime DateAdded { get; set; }

        public virtual string AddedBy { get; set; }

        public virtual string Title { get; set; }

        public virtual string Path { get; set; }

        public virtual int Importance { get; set; }

        public virtual string Description { get; set; }

        public virtual string ImageUrl { get; set; }

        public virtual IList<Article> Articles { get; set; }
    }
}