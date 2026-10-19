using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.IncidentNavigator.Models
{
    public class PivotDataSourceRequestAttribute : CustomModelBinderAttribute
    {
        public override IModelBinder GetBinder()
        {
            return new PivotDataSourceRequestModelBinder();
        }
    }
}