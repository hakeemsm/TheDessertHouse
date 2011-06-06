using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Profile;
using System.Web.Routing;
using Moq;

namespace TheDessertHouse.Tests
{
    public class AspRuntimeMocks
    {


        public AspRuntimeMocks(Controller forController)
        {
            HttpContextMock = new Mock<HttpContextBase>();
            ResponseMock = new Mock<HttpResponseBase>();
            RequestMock = new Mock<HttpRequestBase>();
            //UserMock= new Mock<IPrincipal>();
            //IdentityMock=new Mock<IIdentity>();
            //ProfileMock= new Mock<ProfileBase>();


            HttpContextMock.Setup(x => x.Request).Returns(RequestMock.Object);
            HttpContextMock.Setup(x => x.Response).Returns(ResponseMock.Object);
            HttpContextMock.Setup(x => x.Session).Returns(new SessionDouble());
            RequestMock.Setup(x => x.Cookies).Returns(new HttpCookieCollection());
            RequestMock.Setup(x => x.QueryString).Returns(new NameValueCollection());
            RequestMock.Setup(x => x.Form).Returns(new NameValueCollection());
            ResponseMock.Setup(x => x.Cookies).Returns(new HttpCookieCollection());
            HttpContextMock.Setup(x => x.User).Returns(UserMock.Object);
            //IdentityMock.Setup(x => x.Name).Returns(string.Empty);
            //UserMock.Setup(x => x.Identity).Returns(IdentityMock.Object);
            //ProfileMock.Setup(x => x.SetPropertyValue(It.IsAny<string>(), It.IsAny<string>()));
            
            HttpContextMock.Setup(x => x.Profile).Returns(ProfileMock.Object);
            

            var requestContext = new RequestContext(HttpContextMock.Object, new RouteData());
            forController.ControllerContext = new ControllerContext(requestContext, forController);
        }

        protected Mock<ProfileBase> ProfileMock { get; set; }

        protected Mock<IIdentity> IdentityMock { get; set; }

        protected Mock<IPrincipal> UserMock { get; set; }

        protected Mock<HttpRequestBase> RequestMock { get; set; }

        protected Mock<HttpResponseBase> ResponseMock { get; set; }

        protected Mock<HttpContextBase> HttpContextMock { get; set; }
    }

    public class SessionDouble : HttpSessionStateBase
    {
        private readonly Dictionary<string,object >_items=new Dictionary<string, object>();

        public override object this[string index]
        {
        get
        {
            return _items.ContainsKey(index) ? _items[index] : null;
        }
        set
        {
            _items[index] = value;
        }

    }

    }
}
