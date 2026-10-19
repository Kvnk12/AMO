using System.Web.Mvc;
namespace WebUI.Areas.IncidentNavigator
{
    public class IncidentNavigatorAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "IncidentNavigator";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "IncidentNavigator_default",
                "IncidentNavigator/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
             name: "AjaxServiceDefault",
             url: "IncidentNavigator/AjaxService/{action}",
             defaults: new { controller = "AjaxService", action = "GetData" }
         );

           
        }
    }
}
