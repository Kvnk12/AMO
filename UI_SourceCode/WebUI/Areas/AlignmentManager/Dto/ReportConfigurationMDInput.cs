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

    public class ReportConfigurationMDInput : Input
        {



        public int ReportConfigId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        [Required(ErrorMessage="Select Column")]
        public string Xaxis1 { get; set; }
        [Required(ErrorMessage = "Select Column")]
        public string Yaxis1 {get;set;}
        public string Xaxis2 { get; set; }
        public string Yaxis2 { get; set; }


        public string CreatedByUser { get; set; }

        public DateTime? CreatedDate { get { return _appDate; } set { _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now; } }

        private DateTime  _appDate = DateTime.Now; 
        
        public string LastUpdatedUser { get; set; }

        public DateTime? LastUpdatedDate { get { return _appDate; } set {   _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now ; } }


        }
    }
