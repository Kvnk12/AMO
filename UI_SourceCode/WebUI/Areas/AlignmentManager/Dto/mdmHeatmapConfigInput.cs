using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
using WebUI.Areas.AlignmentManager.Objects;
using System.ComponentModel.DataAnnotations.Schema;


namespace NTTData.AMOPortal.WebUI.Dto
    {

    public class mdmHeatmapConfig : Input
        {



        public int HeatmapConfigId { get; set; }
        public int ClientId { get; set; }
        public int AlgorithmId { get; set; }
        public string HeatmapReportName { get; set; }
        public int HeatVariableID { get; set; }
        public int? HeatVariableRelationshipId { get; set; }
        public int? HeatUseMetadataId { get; set; }
        public int? HeatUseReportId { get; set; }
        public string CreatedByUser { get; set; }

        public DateTime? CreatedDate { get { return _appDate; } set { _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now; } }

        private DateTime  _appDate = DateTime.Now; 
        
        public string LastUpdatedUser { get; set; }



        public DateTime? LastUpdatedDate { get { return _appDate; } set {   _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now ; } }


        }
    }