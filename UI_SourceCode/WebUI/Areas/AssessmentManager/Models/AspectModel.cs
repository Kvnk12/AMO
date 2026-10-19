using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.AssessmentManager.Models
{
    public class AspectModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string AspectName { get; set; }
        public int A3_DisciplineID { get; set; }
        public int A3_ComponentID { get; set; }
    }
}