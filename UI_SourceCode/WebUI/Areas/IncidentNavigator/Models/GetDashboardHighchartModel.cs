using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.IncidentNavigator.Models
{
    public class GetDashboardHighchartModel
    {
   public List<int> Incnt { get; set; }
   public List<string> IncCat { get; set; }
    }
}
