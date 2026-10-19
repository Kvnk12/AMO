using System.Web.Mvc;

namespace WebUI.Areas.ClientProfileManager
{
    public class ClientProfileManagerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ClientProfileManager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ClientProfileManager_default",
                "ClientProfileManager/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
            name: "ClientAjaxServiceDefault",
            url: "ClientProfileManager/ClientAjaxService/{action}",
            defaults: new { controller = "ClientAjaxService", action = "GetClientPerformanceDashboard" }
        );

        }
    }
}
