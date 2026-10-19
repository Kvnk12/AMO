using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace WebUI.Areas.IdeaManager.Dto
{
    public class MetricTargetModel
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        [Required]
        //[StringLength(255)]
        public string MetricTarget { get; set; }      
        public DateTime? CreatedDate { get; set; }      
        public DateTime? UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }

        public string PostedBy { get; set; }
    }
}