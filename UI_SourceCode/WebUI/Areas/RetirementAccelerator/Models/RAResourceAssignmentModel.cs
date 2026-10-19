using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.RetirementAccelerator.Models
{
    public class RAResourceAssignmentModel
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public int TaskId { get; set; }
        public int ResourceId { get; set; }
        public decimal Units { get; set; }

    }
}