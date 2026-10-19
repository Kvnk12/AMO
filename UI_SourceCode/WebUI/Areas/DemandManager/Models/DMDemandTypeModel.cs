using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.DemandManager.Models
{
    public class DMDemandTypeModel
    {
        public int Id { get; set; }
        public string DemandType { get; set; }
        //public string CreatedByUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        //public string LastUpdatedUser { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public int? ClientId { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}