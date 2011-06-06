using System;

namespace TheDessertHouse.Domain
{
    public class OrderItem:IEntity
    {
        public virtual DateTime DateAdded { get; set; }

        public virtual string AddedBy { get; set; }

        public virtual int ProductId { get; set; }

        public virtual string Title { get; set; }

        public virtual string SKU { get; set; }

        public virtual decimal UnitPrice { get; set; }

        public virtual int Quantity { get; set; }

        public virtual int Id { get; set; }

        public virtual Order Order { get; set; }
    }
}