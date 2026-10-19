using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.DemandManager.Dto
{
    public class DemandStatusHistoryModel
    {
        public int Id { get; set; }
        public int DemandId { get; set; }
        public int? OldStatusId { get; set; }
        public int? NewStatusId { get; set; }
        public string Comments { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }

        public string PostedBy { get; set; }
        public string OldStatus { get; set; }
        public string NewStatus { get; set; }
    }
}