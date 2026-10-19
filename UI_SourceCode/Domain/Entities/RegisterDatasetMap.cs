using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NTTData.AMOPortal.Domain.Entities
    {

    [Table("RegisterDatasetMap")]
    public class RegisterDatasetMap : DelEntity
        {


        public int  RegisterDataSetId{get;set;}
        public string DataSetColumnName {get;set;}
        public string VariableType { get; set; }
        public bool IsSelected { get; set; }
        public string CreatedByUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string LastUpdatedUser { get; set; }
        public DateTime? LastUpdatedDate { get; set; }

        }
    }