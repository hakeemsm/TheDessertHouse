using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using FluentNHibernate.Utils;

namespace TheDessertHouse.Web
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString GetMessages(dynamic viewBag)
        {
            const string msgsDiv = "<div class={0}>{1}</div>";
            string retStr = string.Empty;
            if (null != viewBag.ErrorMessage)
                retStr += string.Format(msgsDiv, "error", viewBag.ErrorMessage);
            if (null != viewBag.SuccessMessage)
                retStr += string.Format(msgsDiv, "success", viewBag.SuccessMessage);
            if (null != viewBag.WarningMessage)
                retStr += string.Format(msgsDiv, "warning", viewBag.WarningMessage);
            if (null != viewBag.InformationalMessage)
                retStr += string.Format(msgsDiv, "info", viewBag.InformationalMessage);
            return new MvcHtmlString(retStr);
        }

        public static MvcHtmlString GetPreviousLink(this HtmlHelper html, WebViewPage webPage, int pageNum)
        {
            if (webPage.ViewContext.HttpContext.Request.Form.Count == 0)
                return html.ActionLink("Previous", webPage.ViewContext.RouteData.Values["action"].ToString(),
                                       webPage.ViewContext.RouteData.Values["controller"].ToString(),
                                       new { pageNum = pageNum - 1 }, new { style = "text-align:left", id = "lnkPrevious" });

            return new MvcHtmlString("<a href='#' id='lnkPrevious' style='text-align: left'>Previous</a>");

        }

        public static MvcHtmlString GetNextLink(this HtmlHelper html, WebViewPage webPage, int pageNum)
        {
            if (webPage.ViewContext.HttpContext.Request.Form.Count == 0)
                return html.ActionLink("Next", webPage.ViewContext.RouteData.Values["action"].ToString(),
                                       webPage.ViewContext.RouteData.Values["controller"].ToString(),
                                       new { pageNum = pageNum + 1 }, new { style = "text-align:right", id = "lnkNext" });

            return new MvcHtmlString("<a href='#' id='lnkNext' style='text-align: right'>Next</a>");

        }

        public static HtmlString Image(this HtmlHelper helper, string id, string url, string alternateText)
        {
            return Image(helper, id, url, alternateText, null);
        }

        public static HtmlString Image(this HtmlHelper helper, string id, string url, string alternateText, object htmlAttributes)
        {
            // Instantiate a UrlHelper 
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            // Create tag builder
            var builder = new TagBuilder("img");

            // Create valid id
            builder.GenerateId(id);

            // Add attributes
            builder.MergeAttribute("src", urlHelper.Content(url));
            builder.MergeAttribute("alt", alternateText);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            // Render tag
            var ret = new MvcHtmlString(builder.ToString(TagRenderMode.SelfClosing));
            return ret;
        }

        public static bool CanDisplayPoll(this ViewContext viewContext)
        {
            if (new[] { "Editor", "Contributor", "Admin","StoreKeeper" }.Any(role => viewContext.HttpContext.User.IsInRole(role)))
                return false;
            if ((string)viewContext.RouteData.Values["controller"] == "Poll" &&
                (string)viewContext.RouteData.Values["action"] == "Index")
                return false;
            return viewContext.HttpContext.Session["CurrentPoll"] != null;
        }
    }
}