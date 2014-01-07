namespace Billapong.Administration.Helpers
{
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    /// <summary>
    /// Custom html helpers.
    /// </summary>
    public static class HtmlHelpers
    {
        /// <summary>
        /// Generates a link to an action with an class "active" if the link matches the current request.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <returns>Html string with the link</returns>
        public static MvcHtmlString MenuLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName)
        {
            var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");

            if (actionName == currentAction && controllerName == currentController)
            {
                return htmlHelper.ActionLink(linkText, actionName, controllerName, null, new { @class = "active" });
            }

            return htmlHelper.ActionLink(linkText, actionName, controllerName);
        }
    } 
}