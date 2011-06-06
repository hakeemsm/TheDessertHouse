using System;

namespace TheDessertHouse.Web.Models
{
    [Serializable]
    public class ShippingMethodView
    {
        public string Title { get; set; }

        public decimal Price { get; set; }

        public int Id { get; set; }
    }
}