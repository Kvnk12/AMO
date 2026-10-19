using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using WebUI.Areas.IncidentNavigator.Models;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Dto;
//Added by Anandhi for Client Profile Begin
using System.ComponentModel.DataAnnotations.Schema;
using NTTData.AMOPortal.Domain.Service;
using NTTData.AMOPortal.WebUI.Mapper;
using System.Globalization;
//Added by Raj for validation
using Foolproof;
using System.ComponentModel;
using PagedList;

namespace WebUI.Areas.IncidentNavigator.Dto
{
    public class TrendingReportInput
    {
        public int ID { get; set; }
        public int? ClientId { get; set; }
        public string Incidentid { get; set; }
        
        public DateTime Createdattime { get; set; }

        public string Assignedtoperson { get; set; }
        public string Assignedtosupportgroup { get; set; }
        public string Resolutionyear { get; set; }
        public string Resolutionmonth { get; set; }
        public string ResolutionMonthName { get; set; }

        public string ResolutionQuarter { get; set; }
        public string ConfigItemName { get; set; }

        public string AffectedItemName { get; set; }
        public string ClosureCategory { get; set; }
        
        public string Type { get; set; }

        public string ToolColumnName { get; set; }
        public string ClientColumnName { get; set; }

    }
}