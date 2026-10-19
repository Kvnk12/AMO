using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("IMIdeaPriorityHistory")]
    public class IMIdeaPriorityHistory /*: DelEntity*/
    {
        public int Id { get; set; }
    public int IdeaId { get; set; }
    public int? OldPriorityId { get; set; }
    public int? NewPriorityId { get; set; }
    public bool? IsDeleted { get; set; }
    public string Comments { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
    public int? CreatedBy { get; set; }
    public int? UpdatedBy { get; set; }
    }
}