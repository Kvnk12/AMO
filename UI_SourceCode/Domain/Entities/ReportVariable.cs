using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NTTData.AMOPortal.Domain.Entities
    {

    [Table("ReportVariable")]
    public class ReportVariable : DelEntity
        {

        public int  VariableTypeId  {get;set;}
        public int?  VariableId  {get;set;}
        public int?  VariableLabelId  {get;set;}
        
        public string CreatedByUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string LastUpdatedUser { get; set; }
        public DateTime? LastUpdatedDate { get; set; }

        }
    }