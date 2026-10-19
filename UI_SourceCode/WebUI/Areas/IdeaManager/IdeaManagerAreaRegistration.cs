using System.Web.Mvc;

namespace WebUI.Areas.IdeaManager
{
    public class IdeaManagerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "IdeaManager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {




            //context.MapRoute(
            //    "IdeaManager_default",
            //    "IdeaManager/{controller}/{action}/{id}",
            //    new { controller = "IdeaManager", action = "DisplayChallanges", id = UrlParameter.Optional }
            //);
                        context.MapRoute(
                "IdeaManager_default",
                "IdeaManager/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
            //context.MapRoute(
            //    "AlignmentManager_root",
            //    "{controller}/{action}/{id}",
            //    new { controller = "AlignmentManager", action = "DisplayHomePage", id = UrlParameter.Optional }
            //);

        }
    }
}
