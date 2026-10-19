using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NTTData.AMOPortal.Domain.Entities
    {

    [Table("RegisterDataset")]
    public class RegisterDataset : DelEntity
        {


        public int  ClientId{get;set;}
        public string DatabaseObjectName {get;set;}
        public string DataSetName { get; set; }
        
        public string CreatedByUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string LastUpdatedUser { get; set; }
        public DateTime? LastUpdatedDate { get; set; }

        }
    }