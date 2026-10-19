using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.DemandManager.Models
{
    public class DMReportParamModel
    {
      
            public int Id { get; set; }
            public int ReportId { get; set; }
            public int userId { get; set; }
            public string UserName { get; set; }
            public string ClientName { get; set; }
            public string Observation { get; set; }
            public string ObservationType { get; set; }
            public DateTime? CreatedDt { get; set; }
            public int? UpdatedBy { get; set; }
            public DateTime? UpdatedDt { get; set; }
            public DateTime? FromYear { get; set; }
            public DateTime? ToYear { get; set; }
            public string FromDate { get; set; }
            public string ToDate { get; set; }
            public string DateRange { get; set; }


            public int UserId { get; set; }
            public int ClientId { get; set; }


            public string ReportName { get; set; }
            public string CreatedByName { get; set; }

          
            public IEnumerable<SelectListItem> TypeList { get; set; }
            public IEnumerable<SelectListItem> StatusList { get; set; }         
            public IEnumerable<SelectListItem> PriorityList { get; set; }
            public IEnumerable<SelectListItem> LOBList { get; set; }



    }
}