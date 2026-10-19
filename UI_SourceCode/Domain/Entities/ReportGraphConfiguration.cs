using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NTTData.AMOPortal.Domain.Entities
    {

    [Table("ReportGraphConfiguration")]
    public class ReportGraphConfiguration : DelEntity
        {

        public int     ReportConfigId {get;set;}
        public int SSRSTemplateFileID { get; set; }
	    public string  XColCaption {get;set;}
	    public string  YcolCaption {get;set;}
        public string ChartTitle { get; set; }
        public string CreatedByUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string LastUpdatedUser { get; set; }
        public DateTime? LastUpdatedDate { get; set; }

        }
    }