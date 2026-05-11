using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Html;

namespace WebApplication1.Helpers
{
    public static class HtmlHelper
    {

        public static string IsActive(this IHtmlHelper html, string controller, string action)
        {
            var routeData = html.ViewContext.RouteData.Values;
            var currentController = routeData["controller"] as string;
            var currentAction = routeData["action"] as string;

            return controller.Equals(currentController, StringComparison.OrdinalIgnoreCase) &&
                   action.Equals(currentAction, StringComparison.OrdinalIgnoreCase)
                   ? "active" : "";
        }


    }

}