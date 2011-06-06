using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.TestHelper.Fakes;
using NUnit.Framework;
using TheDessertHouse.Domain;
using TheDessertHouse.Web.Controllers;
using System.Collections.Generic;

namespace TheDessertHouse.Tests.ControllerTests
{
    [TestFixture]
    public class StoreControllerTests:BaseTestController
    {
        [Test,Ignore("This fails as there is no Profile in the FakeHttpContext")]
        public void ShoppingCart_Is_Updated_With_The_Selected_Product_And_The_Quantity()
        {
            var productList =
                new List<Product>
                    {
                        new Product
                            {
                                Id = 1,
                                Description = "cool product",
                                DiscountPercentage = 5.0M,
                                UnitPrice = 10.99M,
                                Title = "Awesome product"
                            }
                    }.AsQueryable();
            MockRepository.Setup(x => x.Get<Product>()).Returns(productList);
            var storeController = new StoreController(MockRepositoryProvider.Object)
                                      {
                                          ControllerContext =
                                              new ControllerContext(FakeHttpContext.Root(), new RouteData(),
                                                                    new SuperController())
                                      };
            ActionResult result =storeController.AddItemToCart(1);
            
            Assert.That(result,Is.TypeOf<RedirectToRouteResult>());

        }
    }
}
