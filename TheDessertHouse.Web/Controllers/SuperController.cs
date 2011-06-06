using System.Web.Mvc;
using log4net;

namespace TheDessertHouse.Web.Controllers
{
    [HandleError]
    public class SuperController:Controller
    {
        private ILog _logger;

        public SuperController()
        {
            _logger = LogManager.GetLogger("DessertHouse");
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            var errMsg = string.Format("Exception: {0}\nStackTrack: {1}", filterContext.Exception.Message,
                                       filterContext.Exception.StackTrace);
            _logger.Error(errMsg);
            base.OnException(filterContext);
        }

        

        
    }
}