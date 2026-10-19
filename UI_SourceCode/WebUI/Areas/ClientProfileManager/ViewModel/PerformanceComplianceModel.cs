using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Areas.ClientProfileManager.Dto;
using System.ComponentModel.DataAnnotations;
using System.Data;


namespace WebUI.Areas.ClientProfileManager.ViewModel
{
    public class PerformanceComplianceModel
    {

        public string ClientName { get; set; }
        public decimal? NoOfIncidents { get; set; }
        public decimal? NoOfUserSuport { get; set; }
        public decimal? NoOfEnhancement { get; set; }
        public decimal? NoOfProblem { get; set; }
        public decimal? NoOfSRBacklog { get; set; }
        public decimal? EffortHrIncidents { get; set; }

        public decimal? EffortHrRequest { get; set; }
        public decimal? EffortHrEnhancement { get; set; }
        public decimal? EffortPerIncident { get; set; }
        public decimal? EffortPerRequest { get; set; }


    }
}