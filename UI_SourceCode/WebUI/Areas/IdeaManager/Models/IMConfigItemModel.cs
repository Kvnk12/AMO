using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.IdeaManager.Models
{
    public class IMConfigItemModel
    {
        public int CIId { get; set; }
        public int ConfigTypeId { get; set; }
        public string ConfigItem { get; set; }
        public int? ClientId { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedDt { get; set; }
        public DateTime? UpdatedDt { get; set; }
    }
}