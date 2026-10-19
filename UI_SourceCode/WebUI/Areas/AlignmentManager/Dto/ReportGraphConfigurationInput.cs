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

    public class ReportGraphConfigurationInput : Input
        {


        
        public int ReportConfigId { get; set; }
        [Required(ErrorMessage="Select Report Template")]
        public int  SSRSTemplateFileID { get; set; }
        [Required(ErrorMessage="Enter X-axis Caption")]
        public string XColCaption { get; set; }
        [Required(ErrorMessage = "Enter Y-axis Caption")]
        public string YcolCaption { get; set; }
        [Required(ErrorMessage = "Enter Chart Title")]
        public string ChartTitle { get; set; }

        public string CreatedByUser { get; set; }

        public DateTime? CreatedDate { get { return _appDate; } set { _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now; } }

        private DateTime  _appDate = DateTime.Now; 
        
        public string LastUpdatedUser { get; set; }

        public DateTime? LastUpdatedDate { get { return _appDate; } set {   _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now ; } }


        }
    }
