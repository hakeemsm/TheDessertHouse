using System;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;

namespace TheDessertHouse.Web
{
    public class DessertHouseControllerFactory : DefaultControllerFactory
    {
        private Container _container;

        public DessertHouseControllerFactory(Container container)
        {
            _container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return _container.GetInstance(controllerType) as IController;
        }

        //public IController CreateController(RequestContext requestContext, string controllerName)
        //{
            
        //    base.GetControllerType(requestContext, controllerName);
        //    return ObjectFactory.GetNamedInstance(typeof (IController), controllerName) as IController;
        //}

        //public void ReleaseController(IController controller)
        //{
        //    throw new NotImplementedException();
        //}
    }

    
}