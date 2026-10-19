using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.FDBKManager.Models
{
    public class FeedbackProductModel
    {
        public int Id { get; set; }
        public string Product { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public int? ClientId { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}