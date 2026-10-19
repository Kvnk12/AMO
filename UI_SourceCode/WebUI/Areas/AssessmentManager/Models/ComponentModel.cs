using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.AssessmentManager.Models
{
    public class ComponentModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string ComponentName { get; set; }
        public string ComponentDefinition { get; set; }
        public int DisciplineId { get; set; }
    }
}