using System;
using System.Collections.Generic;

namespace TheDessertHouse.Domain
{
    [Serializable]
    public class Department:IEntity,CommonEntity
    {
        public virtual DateTime DateAdded { get; set; }

        public virtual string AddedBy { get; set; }

        public virtual string Title { get; set; }

        public virtual int Importance { get; set; }

        public virtual string Description { get; set; }

        public virtual string ImageUrl { get; set; }

        public virtual int Id { get; set; }

        public virtual IList<Product> Products { get; set; }
    }
}