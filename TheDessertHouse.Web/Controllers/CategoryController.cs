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
    [Authorize(Roles = "Editor")]
    public class CategoryController : SuperController
    {
        private IRepositoryProvider _repositoryProvider;
        
        public CategoryController(IRepositoryProvider repositoryProvider)
        {
            _repositoryProvider = repositoryProvider;
        }

        
        public ActionResult CreateCategory()
        {
            return View(new CategoryView{CategoryExists=false});
        }

        [HttpPost,Authorize(Roles = "Editor")]
        public ActionResult CreateCategory(CategoryView categoryView)
        {
            if (ModelState.IsValid)
            {
                categoryView.DateAdded = DateTime.Now;
                categoryView.AddedBy = HttpContext.User.Identity.Name;
                categoryView.Path = categoryView.Title.Trim().Replace(' ', '-');
                string msg = "Category {0} has been created";
                using (var repository = _repositoryProvider.GetRepository())
                {
                    Category category;
                    if (categoryView.CategoryExists)
                    {
                        category = repository.Get<Category>().Where(
                            c => c.Title == categoryView.Title).
                            FirstOrDefault();
                        Mapper.Map(categoryView, category);
                        //category.AddedBy = categoryView.AddedBy;
                        //category.DateAdded = categoryView.DateAdded;
                        //category.Description = categoryView.Description;
                        //category.ImageUrl = categoryView.ImageUrl;
                        //category.Importance = categoryView.Importance;
                        //category.Path = categoryView.Path;
                        //category.Title = categoryView.Title;
                        msg = "Category {0} has been updated";
                    }
                    else
                    {
                        category = Mapper.Map<CategoryView, Category>(categoryView);

                        category.ImageUrl =
                            string.Format("~/Content/images/category/{0}", categoryView.ImageUrl);
                    }
                    repository.SaveOrUpdate(category);
                    ViewBag.SuccessMessage = string.Format(msg, categoryView.Title);
                }
                return RedirectToAction("Index");
            }
            
            return View(categoryView);
        }

        public ActionResult Index()
        {
            ViewBag.PageTitle = "All Categories";
            return RedirectToAction("ManageCategories");
        }

        public static IEnumerable<CategoryView> Categories { get; set; }

        [Authorize(Roles = "Editor")]
        public ActionResult EditCategory(int categoryId)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var category = repository.Get<Category>(categoryId);
                if (null == category)
                    return HttpNotFound("The specified category could not be found");
                var updtCategory = Mapper.Map<Category, CategoryView>(category);
                updtCategory.CategoryExists = true;
                
                return View("CreateCategory", updtCategory);
            }
        }

        [Authorize(Roles = "Editor")]
        public ActionResult ManageCategories()
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                ViewBag.PageTitle = "Manage Categories";
                return View(Mapper.Map<IEnumerable<Category>, IEnumerable<CategoryView>>(repository.Get<Category>()));
            }
        }

        public ActionResult ViewCategory(int categoryId)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var category = repository.Get<Category>(categoryId);
                if (null == category)
                    return HttpNotFound("The specified category could not be found");
                return RedirectToAction("Index", "Home", new {category.Path, page = 1});
            }
        }

        [Authorize(Roles = "Editor")]
        public JsonResult RemoveCategory(int categoryId)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                var category = repository.Get<Category>(categoryId);
                if (null == category)
                    return Json(new {deleted = false});
                repository.Delete<Category>(category);
                return Json(new {deleted = true, catId = categoryId});
            }
        }


        public JsonResult CheckTitle(string title)
        {
            using (var repository = _repositoryProvider.GetRepository())
            {
                bool categoryExists = repository.Get<Category>().Any(c => c.Title==title);
                return Json(categoryExists, JsonRequestBehavior.AllowGet);
            }

        }
    }
}