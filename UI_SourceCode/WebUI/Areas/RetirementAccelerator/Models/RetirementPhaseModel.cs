using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.RetirementAccelerator.Models
{
    public class RetirementPhaseModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string RetirementPhase { get; set; }
    }
}