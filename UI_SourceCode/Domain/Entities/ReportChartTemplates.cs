using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NTTData.AMOPortal.Domain.Entities
    {

    [Table("ReportChartTemplates")]
    public class ReportChartTemplates : DelEntity
        {

        public string SSRSReportTemplateFile { get; set; }
        public string ReportNameToDisplay { get; set; }
        public string ParameterToPass { get; set; }
        public string ParameterValueToPass { get; set; }
        //public string CreatedByUser { get; set; }
        //public DateTime? CreatedDate { get; set; }
        //public string LastUpdatedUser { get; set; }
        //public DateTime? LastUpdatedDate { get; set; }

        }
    }