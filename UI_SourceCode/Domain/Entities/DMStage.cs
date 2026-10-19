using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("DMStage")]
    public class DMStage 
    {
        public int Id { get; set; }
        public string DemandStage { set; get; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public int? ClientId { get; set; }
        public int? UpdatedBy { get; set; }
        public int? CreatedBy { get; set; }
       
    }
}