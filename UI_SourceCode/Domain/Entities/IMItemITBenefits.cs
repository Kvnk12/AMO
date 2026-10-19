using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("IMItemITBenefits")]
    public class IMItemITBenefits : DelEntity
    {

    public int ItemId { get; set; }
    public string Name { get; set; }
    public decimal?  Value { get; set; }
    public string Detail { get; set; }
    public int? FTEHours { get; set; }
    public int? IncidentCount { get; set; }
        //public string CreatedByUser { get; set; }
        public DateTime CreatedDate { get; set; }
    //public string LastUpdatedUser { get; set; }
    public DateTime LastUpdatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string BenefitType { get; set; }
    }
}