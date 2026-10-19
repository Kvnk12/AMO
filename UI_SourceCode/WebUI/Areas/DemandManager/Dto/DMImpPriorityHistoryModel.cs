using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Areas.DemandManager.Dto
{
    public class DMImpPriorityHistoryModel
    {
        public int Id { get; set; }
        public int DemandId { get; set; }
        public int? OldImpPriority { get; set; }
        public int? NewImpPriority { get; set; }
        [StringLength(255)]
        public string Comments { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDt { get; set; }
        public string PostedBy { get; set; }

    }
}