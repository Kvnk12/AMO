using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.TransformationAccelerator.Model
{
    public class ServicePlanningModel
    {
        public int Id { get; set; }
        public string ImprovementType { get; set; }

        public decimal Goal { get; set; }

        public decimal GoalPlan { get; set; }
		public decimal GoalAchievement { get; set; }
        
        public int? Year { get; set; }
        public List<SelectList> yrs { get; set; }

        public string Quarter { get; set; }

        public string Comment { get; set; }

        public int? yearQtr { get; set; }
    }
}