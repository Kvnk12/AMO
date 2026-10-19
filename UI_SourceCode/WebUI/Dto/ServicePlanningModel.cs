using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Specialized;
using System.Collections;

namespace WebUI.Dto
{
    public class ServicePlanningModel
    {
        public int Id { get; set; }
        public string ImprovementType { get; set; }

        public decimal Goal { get; set; }

        public decimal GoalPlan { get; set; }

        public int? Year { get; set; }
        public List<SelectList> yrs { get; set; }

        public string Quarter { get; set; }

       public string Comment { get; set; }

       

    }       

   
}