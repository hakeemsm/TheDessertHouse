using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TheDessertHouse.Domain;

namespace TheDessertHouse.Web.Models
{
    [Serializable]
    public class ShoppingCart : List<ShoppingCartItem>
    {
        public ShippingMethodView ShippingMethod { get; set; }

        public decimal SubTotal
        {
            get
            {
                var totalPrice = 0m;
                ForEach(i => totalPrice += i.TotalPrice);
                return totalPrice;
            }
        }

        public decimal ShippingPrice { get { return ShippingMethod.Price; } }

        public decimal Total { get { return SubTotal + ShippingPrice; } }

        

        public int SelectedShippingMethod { get; set; }
    }
}