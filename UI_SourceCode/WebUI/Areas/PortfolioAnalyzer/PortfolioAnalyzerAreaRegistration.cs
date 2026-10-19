using System.Web.Mvc;

namespace WebUI.Areas.PortfolioAnalyzer
{
    public class PortfolioAnalyzerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "PortfolioAnalyzer";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "PortfolioAnalyzer_default",
                "PortfolioAnalyzer/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
            name: "APRAjaxServiceDefault",
            url: "PortfolioAnalyzer/APRAjaxService/{action}",
            defaults: new { controller = "APRAjaxService", action = "ApplicationList" }
        );
        }
    }
}