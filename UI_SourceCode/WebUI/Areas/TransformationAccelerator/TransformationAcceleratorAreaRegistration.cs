using System.Web.Mvc;

namespace WebUI.Areas.TransformationAccelerator
{
    public class TransformationAcceleratorAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "TransformationAccelerator";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "TransformationAccelerator_default",
                "TransformationAccelerator/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
