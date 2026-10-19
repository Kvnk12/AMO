using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NTTData.AMOPortal.Domain.Entities
    {

    [Table("ReportConfiguration")]
    public class ReportConfiguration : DelEntity
        {


        public int  ClientId  {get;set;}
        public string ReportName {get;set;}
        public int?  ReportTypeID  {get;set;}
        public int?  ReportVariableId  {get;set;}
        //public int? AlgorithmId { get; set; }
        //public int VariableTypeId { get; set; }
        //public int RegisterDataSetId { get; set; }
        public int?  ReportVariableRelId  {get;set;}
        //public int?  ReportMetadataId  {get;set;}
        //public int?  UseReportId  {get;set;}
        public string CreatedByUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string LastUpdatedUser { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public string SSRSReportName { get; set; }

        }
    }