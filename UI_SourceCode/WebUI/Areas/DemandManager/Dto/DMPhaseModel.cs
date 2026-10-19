using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Areas.DemandManager.Dto
{
    public class DMPhaseModel
    {
        public int Id { get; set; }
        [Required]
        public string Phase { get; set; }
        public int? ClientId { get; set; }
        public DateTime? CreatedDt { get; set; }
        public DateTime? UpdatedDt { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}