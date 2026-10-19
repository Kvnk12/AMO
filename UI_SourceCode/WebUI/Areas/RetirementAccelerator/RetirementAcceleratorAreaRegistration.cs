using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.RetirementAccelerator
{
    public class RetirementAcceleratorAreaRegistration :AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "RetirementAccelerator";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "RetirementAccelerator_default",
                "RetirementAccelerator/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

         //   context.MapRoute(
         //    name: "AjaxServiceDefault",
         //    url: "IncidentNavigator/AjaxService/{action}",
         //    defaults: new { controller = "AjaxService", action = "GetData" }
         //);


        }
    }
}