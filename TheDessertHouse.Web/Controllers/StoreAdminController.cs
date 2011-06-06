using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using TheDessertHouse.Domain;
using TheDessertHouse.Infrastructure;
using TheDessertHouse.Web.Models;

namespace TheDessertHouse.Web.Controllers
{
    [Authorize(Roles = "StoreKeeper")]
    public class StoreAdminController : SuperController
    {
        private readonly IRepositoryProvider _repositoryProvider;

        public StoreAdminController(IRepositoryProvider repositoryProvider)
        {
            _repositoryProvider = repositoryProvider;
        }

        public ActionResult Index()
        {
            return RedirectToAction("ManageDepartments");
        }


        public ActionResult CreateDepartment()
        {
            ViewBag.PageTitle = "Create Department";
            return View(new DepartmentView());
        }

        [HttpPost]
        public ActionResult CreateDepartment(DepartmentView departmentView)
        {
            if (ModelState.IsValid)
            {
                using (var repository = _repositoryProvider.GetRepository())
                {
                    var department = Mapper.Map<DepartmentView, Department>(departmentView);
                    department.DateAdded = DateTime.Now;
                    department.AddedBy = User.Identity.Name;
                    department.ImageUrl =
                        string.Format("~/Content/images/category/{0}", departmentView.ImageUrl);
                    repository.Save(department);
                    return RedirectToAction("ManageDepartments");
                }
            }
            ViewBag.PageTitle = "Create Department";
            return View();
        }

        public ActionResult CreateProduct()
        {
            
            ViewBag.PageTitle = "Create Product";
            return View(new ProductView { Departments = GetDepartments(null) });
        }

        private SelectList GetDepartments(Func<Department,bool> whereExpr)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var deptQuery = repository.Get<Department>();
                var departments = whereExpr == null ? deptQuery : deptQuery.Where(whereExpr);
                return new SelectList(Mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentView>>(departments), "Id", "Title");
            }
        }

        [HttpPost]
        public ActionResult CreateProduct(ProductView productView)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var departments = repository.Get<Department>();
                var department = departments.Where(d => d.Id == productView.CategoryId).FirstOrDefault();

                if (department != null)
                {
                    Product product = productView.ProductExists
                                      ? repository.Get<Product>().Where(p => p.Title.Equals(productView.Title, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault()
                                      : Mapper.Map<ProductView, Product>(productView);

                    product.DateAdded = DateTime.Now;
                    product.AddedBy = (User != null && User.Identity != null) ? User.Identity.Name : "";
                    product.SmallImageUrl =
                        string.Format("~/Content/images/item/{0}/{1}", department.Title,
                                                  productView.SmallImageUrl);
                    product.FullImageUrl = string.Format("~/Content/images/item/{0}/{1}", department.Title,
                                                  productView.FullImageUrl);
                    department.Products.Add(product);
                    product.Category = department;
                    repository.Save(department);
                    return RedirectToAction("ManageProducts");
                }
                ViewBag.PageTitle = "Create Product";
                var deptList = new SelectList(Mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentView>>(departments), "Id", "Title", "");

                return View(new ProductView { Departments = deptList });

            }

        }

        [HttpPost]
        public ActionResult DeleteDepartment(int departmentId)
        {
            return DeleteEntity<Department>(departmentId);
        }

        [HttpPost]
        public ActionResult DeleteProduct(int productId)
        {
            return DeleteEntity<Product>(productId);
        }

        [NonAction]
        public ActionResult DeleteEntity<T>(int entityId) where T : class, IEntity
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var entity = repository.Get<T>().Where(t => t.Id == entityId).FirstOrDefault();
                if (entity != null)
                {
                    repository.Delete<T>(entity);
                    return Json(new { Id = entityId });
                }
                return Json(new { Error = "404" });

            }
        }

        public ActionResult ManageDepartments()
        {
            ViewBag.PageTitle = "Manage Departments";
            return View(GetCollection<Department, DepartmentView>());
        }

        private IEnumerable<T1> GetCollection<T, T1>() where T : IEntity
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                return Mapper.Map<IEnumerable<T>, IEnumerable<T1>>(repository.Get<T>());

            }
        }

        public ActionResult ManageProducts()
        {
            ViewBag.PageTitle = "Manage Products";
            return View(GetCollection<Product, ProductView>());
        }


        public ActionResult EditProduct(int productId)
        {
            var model = GetEntity<Product, ProductView>(productId);
            if (model == null)
                return HttpNotFound("No product found with specified Id");
            ViewBag.PageTitle = "Edit Product " + model.Title;
            model.ProductExists = true;
            model.Departments = GetDepartments(d => d.Id == model.CategoryId);
            return View("CreateProduct", model);
        }

        public ActionResult EditDepartment(int departmentId)
        {
            var model = GetEntity<Department, DepartmentView>(departmentId);
            if (model == null)
                return HttpNotFound("No department found with specified Id");
            model.DepartmentExists = true;
            ViewBag.PageTitle = "Edit Department " + model.Title;
            return View("CreateDepartment", model);
        }

        [HttpPost]
        public ActionResult EditDepartment(DepartmentView departmentView)
        {
            if (ModelState.IsValid)
            {
                using (var repository = _repositoryProvider.GetRepository())
                {
                    var department = repository.Get<Department>().Where(d => d.Id == departmentView.Id).FirstOrDefault();
                    department.Title = departmentView.Title;
                    department.Description = departmentView.Description;
                    department.ImageUrl = departmentView.ImageUrl;
                    department.Importance = departmentView.Importance;
                    repository.Save(department);
                    return RedirectToAction("ManageDepartments");
                }
            }
            return RedirectToAction("EditDepartment", new { departmentId = departmentView.Id });

        }

        [HttpPost]
        public ActionResult EditProduct(ProductView productView)
        {
            if (ModelState.IsValid)
            {
                using (var repository = _repositoryProvider.GetRepository())
                {
                    var product = repository.Get<Product>().Where(d => d.Id == productView.Id).FirstOrDefault();
                    Mapper.Map(productView, product);
                    
                    repository.Save(product);
                    return RedirectToAction("ManageProducts");
                }
            }
            return RedirectToAction("EditDepartment", new { departmentId = productView.Id });

        }

        public ActionResult ManageShipping()
        {
            ViewBag.PageTitle = "Manage Shipping";
            using (var repository=_repositoryProvider.GetRepository())
            {
                var shippingMethods = repository.Get<ShippingMethod>().OrderBy(s => s.Title);
                return View(Mapper.Map<IEnumerable<ShippingMethod>, IEnumerable<ShippingMethodView>>(shippingMethods));
            }
        }

        [HttpPost]
        public ActionResult CreateShippingMethod(string shippingMethod,decimal price)
        {
            using (var repository=_repositoryProvider.GetRepository())
            {
                var shipping = new ShippingMethod{AddedBy = User.Identity.Name, DateAdded = DateTime.Now, Price = price, Title = shippingMethod};
                repository.Save(shipping);
                return Json(new {shipping.Id, shipping.Title, shipping.Price});
            }
        }

        [HttpPost]
        public ActionResult DeleteShippingMethod(int shippingId)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var shippingMethod = repository.Get<ShippingMethod>().Where(s => s.Id == shippingId).FirstOrDefault();
                repository.Delete<ShippingMethod>(shippingMethod);
                return Json(new { Id=shippingId });
            }
        }


        private T1 GetEntity<T, T1>(int entityId)
            where T : class, IEntity
            where T1 : class
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var entity = repository.Get<T>().Where(t => t.Id == entityId).FirstOrDefault();
                return entity == null ? null : Mapper.Map<T, T1>(entity);
            }
        }

        public JsonResult CheckDepartmentTitle(string title)
        {
            return Json(CheckTitle<Department>(title), JsonRequestBehavior.AllowGet);

        }

        public JsonResult CheckProductTitle(string title)
        {
            return Json(CheckTitle<Product>(title), JsonRequestBehavior.AllowGet);

        }

        private bool CheckTitle<T>(string title) where T : CommonEntity
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                return repository.Get<T>().Any(c => c.Title == title);
            }
        }
    }
}