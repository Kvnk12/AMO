using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NTTData.AMOPortal.Domain.Entities
    {

    [Table("ReportFilter")]
    public class ReportFilter : DelEntity
        {

        public int  ReportConfigId {get;set;}
        //public int FilterId { get; set; }
        public string FilterValue { get; set; }
        public string FilterColumn {get;set;}
        public string CreatedByUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string LastUpdatedUser { get; set; }
        public DateTime? LastUpdatedDate { get; set; }

        }
    }