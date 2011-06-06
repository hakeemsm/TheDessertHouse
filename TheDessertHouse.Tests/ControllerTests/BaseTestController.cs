using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using MvcContrib.TestHelper.Fakes;
using StructureMap;
using TheDessertHouse.Domain;
using TheDessertHouse.Infrastructure;
using TheDessertHouse.Web;
using TheDessertHouse.Web.Configuration;
using TheDessertHouse.Web.Controllers;

namespace TheDessertHouse.Tests.ControllerTests
{
    public class BaseTestController
    {
        private readonly Mock<IRepositoryProvider> _mockRepositoryProvider;
        private readonly Mock<IRepository> _mockRepository;
        private readonly Container _container;
        private FakeHttpContext _fakeHttpContext;

        public BaseTestController()
        {
            _mockRepositoryProvider = new Mock<IRepositoryProvider>();
            _mockRepository = new Mock<IRepository>();
            var membershipProvider = new Mock<IMembershipProvider>().Object;
            _container = new Container(x =>
            {
                x.For<IRepositoryProvider>().Use(_mockRepositoryProvider.Object);
                x.For<IMembershipProvider>().Use(membershipProvider);
            });
            DessertHouseConfigurationSection.Current = new DessertHouseConfigurationSection();
            BootStrapper.MapDomainAndViewTypes();
        }

        protected Mock<IRepositoryProvider> MockRepositoryProvider
        {
            get { return _mockRepositoryProvider; }
        }

        public Mock<IRepository> MockRepository
        {
            get
            {
                _mockRepositoryProvider.Setup(m => m.GetRepository()).Returns(_mockRepository.Object);
                return _mockRepository;
            }
        }

        protected T CreateController<T>() where T : SuperController
        {
            var controller = _container.GetInstance<T>();
            RouteTable.Routes.Clear();
            BootStrapper.RegisterRoutes(RouteTable.Routes);
            controller.ControllerContext = new ControllerContext { HttpContext = FakeContext,RequestContext = new RequestContext(FakeContext,RouteTable.Routes.GetRouteData(FakeContext))};
            controller.Url = new UrlHelper(controller.ControllerContext.RequestContext);
            return controller;

        }

        public HttpContextBase FakeContext
        {
            get
            {
                if (_fakeHttpContext==null)
                {
                    //_fakeHttpContext = FakeHttpContext.Root();
                    _fakeHttpContext=new FakeHttpContext("/TheDessertHouse")
                                         {
                                             User = new FakePrincipal(new FakeIdentity("tester"), new[] {"Tester"})
                                         };
                }
                
                return _fakeHttpContext;
            }
        }
    }
}