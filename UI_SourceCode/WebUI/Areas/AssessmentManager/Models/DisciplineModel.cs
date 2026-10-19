using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.AssessmentManager.Models
{
    public class DisciplineModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string DisciplineName { get; set; }  
      
    }
}