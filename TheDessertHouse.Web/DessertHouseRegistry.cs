using System;
using System.Web.Mvc;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;
using TheDessertHouse.Domain;
using TheDessertHouse.Infrastructure;
using TheDessertHouse.Services;
using TheDessertHouse.Services.ControllerTests;

namespace TheDessertHouse.Web
{
    public class DessertHouseRegistry : Registry
    {
        public DessertHouseRegistry()
        {
            For<IMembershipProvider>().Use<MembershipProviderWrapper>();
            For<IDependencyResolver>().Use<TheDessertHouseDependencyResolver>();
            For<IControllerActivator>().Use<DessertHouseControllerActivator>();
            //For<IControllerFactory>().Use<DessertHouseControllerFactory>();
            For<IEmailService>().Use<EmailService>();
            For<IRepositoryProvider>().Use<RepositoryProvider>().OnCreation(
                r => r.SessionFactory = BootStrapper.SessionFactory).Named("InjectedRepostoryFactory");
            //SetAllProperties(p=>p.OfType<IRepositoryProvider>());
            For<IFilterProvider>().Use<DessertHouseFilterProvider>();
            ForConcreteType<CheckTitleAttribute>().Configure.OnCreation(
                (c, i) => i.RepositoryProviderObject = c.GetInstance<IRepositoryProvider>("InjectedRepostoryFactory"));
        }
    }
}