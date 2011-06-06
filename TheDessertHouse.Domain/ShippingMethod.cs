using System;

namespace TheDessertHouse.Domain
{
    public class ShippingMethod:IEntity
    {
        public virtual DateTime DateAdded { get; set; }

        public virtual string AddedBy { get; set; }

        public virtual string Title { get; set; }

        public virtual decimal Price { get; set; }

        public virtual int Id { get; set; }
        
    }
}