using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.RetirementAccelerator.Models
{
    public class RATaskDependencyModel : IGanttDependency
    {
        public int DependencyID { get; set; }
        public int ApplicationId { get; set; }
        public int? PredecessorID { get; set; }
        public int? SuccessorID { get; set; }
        public DependencyType Type { get; set; }
    }
}