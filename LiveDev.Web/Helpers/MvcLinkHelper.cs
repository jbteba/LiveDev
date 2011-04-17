using System.Web.Mvc;
using System.Web.Mvc.Html;


namespace LiveDev.Web.Helpers
{
    public static class MvcLinkHelper
    {
        public static MvcHtmlString GenerateMVCMenuURL(this HtmlHelper helper, string link, string action, string controller)
        {
            string url = helper.ViewContext.RouteData.Values["controller"].Equals(controller)
                             ? "<li class=\"selected\">"
                             : "<li>";
            url += helper.ActionLink(link, action, controller) + "</li>";
            
            return new MvcHtmlString(url);
        }
    }
}