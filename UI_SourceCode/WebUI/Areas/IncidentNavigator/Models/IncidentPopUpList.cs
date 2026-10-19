using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace WebUI.Areas.IncidentNavigator.Models
{
    public class IncidentPopUpList
    {
        public string searchcriteria { get; set; }

        public int IncidentID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool Checked { get; set; }
    }
}