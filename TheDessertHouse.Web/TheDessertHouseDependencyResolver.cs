using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using StructureMap;

namespace TheDessertHouse.Web
{
    public class TheDessertHouseDependencyResolver:IDependencyResolver
    {
        private readonly IContainer _container;

        public TheDessertHouseDependencyResolver(IContainer container)
        {   
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                if (serviceType.IsAbstract || serviceType.IsInterface)
                    return _container.TryGetInstance(serviceType);

                return _container.GetInstance(serviceType);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.GetAllInstances<object>().Where(s => s.GetType() == serviceType);
        }
    }
}