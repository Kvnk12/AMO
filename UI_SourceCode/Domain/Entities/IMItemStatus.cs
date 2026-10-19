using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("IMItemStatus")]
    public class IMItemStatus : DelEntity
    {

    public string Status { get; set; }
    public string CreatedByUser { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string LastUpdatedUser { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}