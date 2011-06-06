using System;

namespace TheDessertHouse.Domain
{
    [Serializable]
    public class Product:IEntity,CommonEntity
    {
        public virtual DateTime DateAdded { get; set; }

        public virtual string AddedBy { get; set; }

        public virtual string Title { get; set; }

        public virtual string Description { get; set; }

        public virtual string SKU { get; set; }

        public virtual decimal UnitPrice { get; set; }

        public virtual decimal DiscountPercentage { get; set; }

        public virtual int UnitsInStock { get; set; }

        public virtual string SmallImageUrl { get; set; }

        public virtual string FullImageUrl { get; set; }

        public virtual int Id { get; set; }

        public virtual Department Category { get; set; }
    }
}