using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("DMDemandType")]
    public class DMDemandType : DelEntity
    {
        public string DemandType { get; set; }
        // public string CreatedByUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        //public string LastUpdatedUser { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public int? ClientId { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}