using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using WebUI.Areas.IncidentNavigator.Models;
using Kendo.Mvc.UI;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.IncidentNavigator.Dto
{
    public class ClusterReportInput
    {
        public int? ClientId { get; set; }

        public int TotalIncidents { get; set; }
        public string Type { get; set; }
        public string Configitemname { get; set; }

        public string ClusterLabel { get; set; }

        public int TotalResolutionTime { get; set; }
       [DisplayFormat(DataFormatString="{0:0.00}")]
        public decimal? AvgResolutionTime { get; set; }
        //public IList<ClusterReportInput> ClusterReportlist { get; set; }
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = false)]
        public decimal? AvgCostPerTicket { get; set; }



    }
}