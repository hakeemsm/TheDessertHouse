using System;
using TheDessertHouse.Domain;

namespace TheDessertHouse.Web.Models
{
    [Serializable]
    public class ShoppingCartItem
    {
        private Product _item;

        public ShoppingCartItem(Product product)
        {
            _item = product;
        }

        public Product Item
        {
            get { return _item; }
        }

        public int Quantity { get; set; }

        public decimal TotalPrice { get { return Item.UnitPrice*Quantity; } }
    }
}