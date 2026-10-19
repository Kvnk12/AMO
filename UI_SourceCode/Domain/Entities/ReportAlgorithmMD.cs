using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NTTData.AMOPortal.Domain.Entities
    {

    [Table("ReportAlgorithmMD")]
    public class ReportAlgorithmMD : DelEntity
        {

        //public int  ReportConfigId {get;set;}
        public int AlgorithmId {get;set;}
        public string key { get; set; }
        public string Value { get; set; }
        public string KeyNames { get; set; }
        //public string UsedColumnOne {get;set;}
        //public string UsedColumnTwo {get;set;}
        public string CreatedByUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string LastUpdatedUser { get; set; }
        public DateTime? LastUpdatedDate { get; set; }

        }
    }