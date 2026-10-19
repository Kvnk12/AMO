using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace WebUI.Areas.IncidentNavigator.Models
{
    public class PivotDataSourceRequest
    {
        public PivotDataSourceRequest()
        {
            Restrictions = new Dictionary<string, string>();
        }

        public string Statement { get; set; }

        public string Command { get; set; }
        public Dictionary<string, string> Restrictions { get; set; }
    }
}