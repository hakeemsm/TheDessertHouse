using System;
using System.Collections.Generic;

namespace TheDessertHouse.Domain
{
    public class Order:IEntity
    {
        public virtual DateTime DateAdded { get; set; }

        public virtual string AddedBy { get; set; }

        public virtual string Status { get; set; }

        public virtual string ShippingMethod { get; set; }

        public virtual decimal SubTotal { get; set; }

        public virtual decimal Shipping { get; set; }

        public virtual string ShippingFirstName { get; set; }

        public virtual string ShippingLastName { get; set; }

        public virtual string ShippingStreet { get; set; }

        public virtual string ShippingZipCode { get; set; }

        public virtual string ShippingCity { get; set; }

        public virtual string ShippingState { get; set; }

        public virtual string CustomerEmail { get; set; }

        public virtual DateTime? ShippedDate { get; set; }

        public virtual string TransactionId { get; set; }

        public virtual string TrackingId { get; set; }

        public virtual int Id { get; set; }

        public virtual IList<OrderItem> Items { get; set; }
    }
}