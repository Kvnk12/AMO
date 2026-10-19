using System.Web.Mvc;

namespace WebUI.Areas.AlignmentManager
{
    public class AlignmentManagerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "AlignmentManager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

        //context.MapRoute(
        //    "AlignmentManager_default_subpages",
        //    "AlignmentManager/{controller}/{action}/{id}",
        //    new { controller = "AlignmentManager", action = "DisplayHomePage", id = UrlParameter.Optional }
        //);


        context.MapRoute(
            "AlignmentManager_default",
            "AlignmentManager/{controller}/{action}/{id}",
            new { controller = "AlignmentManager", action = "DisplayHomePage", id = UrlParameter.Optional }
        );

        //context.MapRoute(
        //    "AlignmentManager_root",
        //    "{controller}/{action}/{id}",
        //    new { controller = "AlignmentManager", action = "DisplayHomePage", id = UrlParameter.Optional }
        //);

        }
    }
}
