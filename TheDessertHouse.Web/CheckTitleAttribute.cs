using System;
using System.Linq;
using System.Web.Mvc;
using TheDessertHouse.Domain;
using TheDessertHouse.Infrastructure;
using TheDessertHouse.Web.Models;

namespace TheDessertHouse.Web
{
    public class CheckTitleAttribute : ActionFilterAttribute
    {
        private IRepositoryProvider _repositoryProvider;

        

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            object entity = EvaluateParmsAndDetermineResult(filterContext);

            filterContext.Result = new JsonResult { Data = entity, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }

        private object EvaluateParmsAndDetermineResult(ActionExecutingContext filterContext)
        {
            var names = filterContext.ActionDescriptor.ActionName.GetEntityAndParmNames();
            var entityName = names[0];
            var actionParmName = names[1];
            
            using (var repository = RepositoryProviderObject.GetRepository())
            {
                var actionParameter = filterContext.ActionParameters[actionParmName];
                switch (entityName)
                {
                    case "Department":
                        var department = actionParameter as DepartmentView;
                        return repository.Get<Department>().Where(e => e.Title == department.Title).FirstOrDefault();
                    case "Product":
                        var product = actionParameter as ProductView;
                        return repository.Get<Product>().Where(e => e.Title == product.Title).FirstOrDefault();
                    case "Category":
                        var category = actionParameter as CategoryView;
                        return repository.Get<Category>().Where(e => e.Title == category.Title).FirstOrDefault();
                    case "Article":
                        var article = actionParameter as ArticleView;
                        return repository.Get<Article>().Where(e => e.Title == article.Title).FirstOrDefault();
                    case "Forum":
                        var forum = actionParameter as ForumView;
                        return repository.Get<Forum>().Where(e => e.Title == forum.Title).FirstOrDefault();
                    default:
                        return null;
                }
            }
        }

        public IRepositoryProvider RepositoryProviderObject
        {
            get {
                return _repositoryProvider;
            }
            set {
                _repositoryProvider = value;
            }
        }
    }
}