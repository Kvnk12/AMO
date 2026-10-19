using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("IMPostItem")]
    public class IMPostItem : DelEntity
    {

    public int ClientId { get; set; }

    public string ItemName { get; set; }

    public int CreatedUserId { get; set; }

    public decimal QtyFication { get; set; }

    public string Description { get; set; }
    public int ItemTypeId { get; set; }
    public int CategoryId { get; set; }
    public int PriorityId { get; set; }
    public int StatusId { get; set; }
    
    public string CreatedByUser { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string LastUpdatedUser { get; set; }
    public DateTime? LastUpdatedDate { get; set; }

    }
}