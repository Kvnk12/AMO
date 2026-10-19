using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Areas.DemandManager.Dto
{
    public class DMRiskAndMitigationModel
    {
        public int Id { get; set; }
        public int DemandId { get; set; }
        //[StringLength(255)]
        [Required]
        public string Risk { get; set; }
        //[StringLength(255)]
        [Required]
        public string MitigationPlan { get; set; }
        //[StringLength(255)]
        public string Comments { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedDt { get; set; }
        public DateTime? UpdatedDt { get; set; }
        public string CreateDateString
        {
            get
            {
                return (string.Format("{0:dd-MMM-yyyy hh:mm}", CreatedDt));
            }
        }
    }
}