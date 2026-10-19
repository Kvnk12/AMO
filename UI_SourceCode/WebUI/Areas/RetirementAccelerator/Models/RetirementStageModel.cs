using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Areas.RetirementAccelerator.Models
{
    public class RetirementStageModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        [Required]
        public int RetirementPhaseId { get; set; }
        [Required]
        public string RetirementStage { get; set; }
    }
}