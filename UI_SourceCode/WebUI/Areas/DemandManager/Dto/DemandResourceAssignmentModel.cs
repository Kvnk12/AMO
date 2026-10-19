using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kendo.Mvc.UI;

namespace WebUI.Areas.DemandManager.Dto
{
    public class DemandResourceAssignmentModel
    { 
        public int Id { get; set; }
        public int DemandPostId { get; set; }
        public int TaskId { get; set; }
        public int ResourceId { get; set; }
        public decimal Units { get; set; }

    }
}