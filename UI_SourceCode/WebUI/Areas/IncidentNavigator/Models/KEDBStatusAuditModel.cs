using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.IncidentNavigator.Models
{
    public class KEDBStatusAuditModel
    {
        public int Id { get; set; }
        public int? KEDBId { get; set; }
        public int? StatusId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? PreviousStatusId { get; set; }
    }
}