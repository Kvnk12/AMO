using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.IncidentNavigator.Models
{
    public class IncidentListModel
    {

        public string SearchIncidents { get; set; }

        public int KEDBId { get; set; }

        public int Id { get; set; }

        public int InID { get; set; }

        public int IncidentID { get; set; }
        
        public string Title { get; set; }
       
        public string Description { get; set; }
   
       
    }
}