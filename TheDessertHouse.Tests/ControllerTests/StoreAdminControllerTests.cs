using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NUnit.Framework;
using TheDessertHouse.Domain;
using TheDessertHouse.Web.Controllers;
using TheDessertHouse.Web.Models;

namespace TheDessertHouse.Tests.ControllerTests
{
    [TestFixture]
    public class StoreAdminControllerTests:BaseTestController
    {
        [Test]
        public void Adding_A_Product_Adds_Product_And_Redirects_To_ManageProducts()
        {
            var departments =
                new List<Department> {new Department {Id = 100, Products = new List<Product>(),Title = "Prodn"}}.AsQueryable();
            var product = new ProductView
                              {
                                  CategoryId = 100,
                                  Description = "This is a cool product",
                                  DiscountPercentage = 3.3M,
                                  FullImageUrl = "actualImg.jpg",
                                  SKU = "SKU1",
                                  SmallImageUrl = "img1.png",
                                  Title = "Awesome Product",
                                  UnitPrice = 101.33M,
                                  UnitsInStock = 100
                              };
            MockRepository.Setup(x => x.Get<Department>()).Returns(departments);

            var storeAdminController = CreateController<StoreAdminController>();

            ActionResult actionResult = storeAdminController.CreateProduct(product);
            Assert.That(actionResult,Is.TypeOf<RedirectToRouteResult>());
            Assert.That(((RedirectToRouteResult)actionResult).RouteValues["action"],Is.EqualTo("ManageProducts"));


        }

        [Test]
        public void EditProduct_Returns_View_With_Correct_Product_As_The_Model()
        {
            var productList = new List<Product> {new Product {Id = 101}}.AsQueryable();
            MockRepository.Setup(x => x.Get<Product>()).Returns(productList);

            var storeAdminController = CreateController<StoreAdminController>();
            ActionResult actionResult = storeAdminController.EditProduct(101);

            var viewResult = ((ViewResult)actionResult);
            Assert.That(viewResult.Model,Is.TypeOf<ProductView>());
            Assert.That(((ProductView)viewResult.Model).Id,Is.EqualTo(101));
        }

        [Test]
        public void Modifying_Department_With_Invalid_Values_Redirects_To_EditDeparment()
        {
            var storeAdminController = CreateController<StoreAdminController>();
            storeAdminController.ModelState.AddModelError("","");
            ActionResult actionResult = storeAdminController.EditDepartment(new DepartmentView {Id = 303});
            Assert.That(actionResult, Is.TypeOf<RedirectToRouteResult>());
            Assert.That(((RedirectToRouteResult)actionResult).RouteValues["departmentId"], Is.EqualTo(303));
            Assert.That(((RedirectToRouteResult)actionResult).RouteValues["action"], Is.EqualTo("EditDepartment"));

        }

        [Test]
        public void ProductValues_Are_Correctly_Mapped_And_Saved_For_EditProduct()
        {
            var product = new Product
                              {
                                  Id = 101,
                                  AddedBy = "skp1",
                                  DateAdded = DateTime.Now.AddDays(-5),
                                  Description = "cool product",
                                  SmallImageUrl = "small.png",
                                  Title = "awesome product",
                                  UnitPrice = 33.99M,
                                  UnitsInStock = 10
                              };
            var productList = new List<Product> { product }.AsQueryable();
            MockRepository.Setup(x => x.Get<Product>()).Returns(productList);
            MockRepository.Setup(x => x.Save(product));
            var storeAdminController = CreateController<StoreAdminController>();
            var productView = new ProductView
                                          {
                                              Description = "cooler product",
                                              Title = "awesomer product",
                                              UnitPrice = 21.33M,
                                              DiscountPercentage = 10.11M,
                                              FullImageUrl = "fullerImg.jpg",
                                              Id = 101
                                          };
            ActionResult actionResult = storeAdminController.EditProduct(productView);
            MockRepository.VerifyAll();
            Assert.That(actionResult,Is.TypeOf<RedirectToRouteResult>());
            Assert.That(((RedirectToRouteResult)actionResult).RouteValues["action"],Is.EqualTo("ManageProducts"));
        }
    }
}
