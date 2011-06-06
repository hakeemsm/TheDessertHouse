using System.Collections.Generic;
using System.Web.Mvc;
using FluentNHibernate.Utils;
using StructureMap;
using TheDessertHouse.Infrastructure;

namespace TheDessertHouse.Web
{
    public class DessertHouseFilterProvider : FilterAttributeFilterProvider
    {
        private readonly IContainer _container;

        public DessertHouseFilterProvider(IContainer container)
        {
            _container = container;
        }

        public override IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var filters = base.GetFilters(controllerContext, actionDescriptor);
            filters.Each(f =>
                             {
                                 if (typeof(CheckTitleAttribute)==f.Instance.GetType())
                                 {
                                     ((CheckTitleAttribute) f.Instance).RepositoryProviderObject =
                                         _container.GetInstance<IRepositoryProvider>();
                                 }
                                 _container.BuildUp(f.Instance);
                             });
            return filters;
        }
    }
}