using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using AutoMapper;
using FluentNHibernate.Utils;
using TheDessertHouse.Domain;
using TheDessertHouse.Infrastructure;
using TheDessertHouse.Web.Configuration;
using TheDessertHouse.Web.Models;

namespace TheDessertHouse.Web.Controllers
{
    public class StoreController : SuperController
    {
        private IRepositoryProvider _repositoryProvider;

        public StoreController(IRepositoryProvider repositoryProvider)
        {
            _repositoryProvider = repositoryProvider;
        }

        public ActionResult Index()
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                ViewBag.PageTitle = "Welcome to The Dessert house store";
                return
                    View(Mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentView>>(repository.Get<Department>()));
            }
        }

        [HttpPost]
        public ActionResult AddItemToCart(int productId, int quantity = 1)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var product = repository.Get<Product>().Where(p => p.Id == productId).FirstOrDefault();
                if (product == null)
                    return HttpNotFound("No product found");
                var profileBase = HttpContext.Profile;
                var shoppingCart = profileBase.GetPropertyValue("ShoppingCart") as ShoppingCart;
                if (shoppingCart == null || shoppingCart.Count==0)
                {
                    shoppingCart = new ShoppingCart();
                    var shippingMethod = repository.Get<ShippingMethod>().OrderBy(s => s.Price).FirstOrDefault();
                    shoppingCart.ShippingMethod = Mapper.Map<ShippingMethod, ShippingMethodView>(shippingMethod);
                    profileBase.SetPropertyValue("ShoppingCart", shoppingCart);
                }
                var item = new ShoppingCartItem(product) { Quantity = quantity };
                shoppingCart.Add(item);
            }
            return RedirectToAction("ViewCart");
        }

        public ActionResult ViewDepartment(int departmentId)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var department = repository.Get<Department>().Where(d => d.Id == departmentId).FirstOrDefault();
                if (department == null)
                    return HttpNotFound("No department found with specified Id");
                ViewBag.PageTitle = "Category " + department.Title;
                return View(Mapper.Map<IEnumerable<Product>, IEnumerable<ProductView>>(department.Products));
            }
        }

        public ActionResult ViewProduct(int productId)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var product = repository.Get<Product>().Where(d => d.Id == productId).FirstOrDefault();
                if (product == null)
                    return HttpNotFound("No product found with specified Id");
                ViewBag.PageTitle = product.Title;
                return View(Mapper.Map<Product, ProductView>(product));
            }
        }

        public ActionResult ViewCart(int? shippingMethod)
        {
            var profile = HttpContext.Profile;
            var shoppingCart = profile.GetPropertyValue("ShoppingCart") as ShoppingCart;
            using (var repository = _repositoryProvider.GetRepository())
            {
                if (shoppingCart == null)
                    shoppingCart = new ShoppingCart();
                var shippingMethods = repository.Get<ShippingMethod>().OrderBy(s => s.Price);
                var cheapestShipping = shippingMethod.HasValue ?
                                       repository.Get<ShippingMethod>().Where(s => s.Id == shippingMethod.Value).FirstOrDefault() :
                                       shippingMethods.FirstOrDefault();
                shoppingCart.ShippingMethod = shoppingCart.ShippingMethod ?? Mapper.Map<ShippingMethod, ShippingMethodView>(cheapestShipping);
                profile.SetPropertyValue("ShoppingCart", shoppingCart);
                ViewData["ShippingOptions"] = new SelectList(Mapper.Map<IEnumerable<ShippingMethod>, IEnumerable<ShippingMethodView>>(shippingMethods), "Id", "Title", shoppingCart.ShippingMethod.Id);
                
                ViewBag.PageTitle = shoppingCart.Count == 0 ? "Your cart is empty" : "Items in your cart";
                return View(shoppingCart);
            }
        }

        public ActionResult UpdateShipping(int shippingId)
        {
            var profile = HttpContext.Profile;
            var shoppingCart = profile.GetPropertyValue("ShoppingCart") as ShoppingCart ?? new ShoppingCart();
            using (var repository = _repositoryProvider.GetRepository())
            {
                var selectedShipping = repository.Get<ShippingMethod>().Where(s => s.Id == shippingId).FirstOrDefault();
                
                shoppingCart.ShippingMethod = shoppingCart.ShippingMethod ?? Mapper.Map<ShippingMethod, ShippingMethodView>(selectedShipping);
                profile.SetPropertyValue("ShoppingCart", shoppingCart);
                return Json(new
                            {
                                ShippingPrice = selectedShipping.Price.ToString("c"),
                                Total = (selectedShipping.Price + shoppingCart.Total).ToString("c")
                            }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteItemFromCart(int productId)
        {
            var profile = HttpContext.Profile;
            var shoppingCart = profile.GetPropertyValue("ShoppingCart") as ShoppingCart;
            if (shoppingCart == null)
                return HttpNotFound("Shopping cart not present");
            var item = shoppingCart.Find(s => s.Item.Id == productId);
            if (item != null)
                shoppingCart.Remove(item);
            var updatedPrice = shoppingCart.Total == 0
                                   ? "0"
                                   : (shoppingCart.Total + shoppingCart.ShippingPrice).ToString("c");
            return Json(new { Id = productId,Total=updatedPrice });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CompleteOrder(string tx, decimal amt)
        {
            string response = TransactionDataRequest(tx);
            var profile = HttpContext.Profile;
            var shoppingCart = profile.GetPropertyValue("ShoppingCart") as ShoppingCart;
            if (shoppingCart != null && shoppingCart.Total == amt)
            {
                var order = new Order
                                {
                                    AddedBy = User.Identity.Name,
                                    DateAdded = DateTime.Now,
                                    CustomerEmail = ExtractFromResponse(response, "payer_email"),
                                    Shipping = shoppingCart.ShippingPrice,
                                    ShippingCity = ExtractFromResponse(response, "address_city"),
                                    ShippingFirstName = ExtractFromResponse(response, "first_name"),
                                    ShippingLastName = ExtractFromResponse(response, "last_name"),
                                    ShippingMethod = shoppingCart.ShippingMethod.Title,
                                    ShippingStreet = ExtractFromResponse(response, "address_street"),
                                    ShippingState = ExtractFromResponse(response, "address_state"),
                                    ShippingZipCode = ExtractFromResponse(response, "address_zip"),
                                    Status = "Order Received",
                                    TransactionId = tx,
                                    SubTotal = shoppingCart.SubTotal
                                };
                order.Items=new List<OrderItem>();
                var productIds = shoppingCart.Select(i => i.Item.Id);
                using (var repository = _repositoryProvider.GetRepository())
                {
                    //var items = repository.Get<Product>().Where(p => productIds.Any(pId=>pId==p.Id)).OrderBy(p => p.Title).ToList();
                    var items = new List<Product>();
                    productIds.Each(id => items.Add(repository.Get<Product>().Where(p => p.Id == id).FirstOrDefault()));
                    shoppingCart.ForEach(
                        p =>
                            {
                                order.Items.Add(new OrderItem
                                                    {
                                                        AddedBy = User.Identity.Name,
                                                        DateAdded = DateTime.Now,
                                                        Order = order,
                                                        ProductId = p.Item.Id,
                                                        Quantity = p.Quantity,
                                                        UnitPrice = p.Item.UnitPrice,
                                                        SKU = p.Item.SKU,
                                                        Title=p.Item.Title
                                                    });
                                items.Each(i => { i.UnitsInStock -= p.Quantity;
                                                    repository.Save(i);
                                });
                            });
                    repository.Save(order);
                }
                profile.SetPropertyValue("ShoppingCart",new ShoppingCart());
                ViewBag.OrderNumber = order.Id;
                ViewBag.PageTitle = "Order Received";
                return View();
            }
            return View("TransactionError");
        }

        private string ExtractFromResponse(string response, string token)
        {
            var keys = response.Split('\n');
            string output = string.Empty;
            foreach (var key in keys)
            {
                var bits = key.Split('=');
                if (bits.Length > 1)
                {
                    output = bits[1];
                    if (bits[0].Equals(token, StringComparison.InvariantCultureIgnoreCase))
                        break;
                }
            }
            return HttpContext.Server.UrlDecode(output);
        }

        private string TransactionDataRequest(string trxn)
        {
            var payPalServer = DessertHouseConfigurationSection.Current.Store.PayPalServer;
            var request = Encoding.ASCII.GetString(HttpContext.Request.BinaryRead(HttpContext.Request.ContentLength));
            var formValues = request +
                    string.Format("&cmd={0}&at={1}&tx={2}", "_notify-synch",
                                  DessertHouseConfigurationSection.Current.Store.PayPalToken, trxn);
            var webRequest = WebRequest.Create(payPalServer) as HttpWebRequest;
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = formValues.Length;
            using (var sw = new StreamWriter(webRequest.GetRequestStream(), Encoding.ASCII))
                sw.Write(formValues);
            using (var payPalResponse=webRequest.GetResponse() as HttpWebResponse)
            using (var payPalResponseStream=payPalResponse.GetResponseStream())
            using (var strmRdr = new StreamReader(payPalResponseStream, Encoding.UTF8))
                return strmRdr.ReadToEnd();
        }
    }
}