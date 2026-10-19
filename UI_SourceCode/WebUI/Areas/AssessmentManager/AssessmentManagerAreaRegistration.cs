using System.Web.Mvc;

namespace WebUI.Areas.AssessmentManager
{
    public class AssessmentManagerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "AssessmentManager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "AssessmentManager_default",
                "AssessmentManager/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}