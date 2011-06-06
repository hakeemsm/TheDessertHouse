using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace TheDessertHouse.Web
{
    public class DessertHouseControllerActivator:IControllerActivator
    {
        public IController Create(RequestContext requestContext, Type controllerType)
        {
            return DependencyResolver.Current.GetService(controllerType) as IController;
        }
    }
}