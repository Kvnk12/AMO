using System.Web.Mvc;

namespace WebUI.Areas.DemandManager
{
    public class DemandManagerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DemandManager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
           context.MapRoute(
                "DemandManager_default",
                "DemandManager/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
