using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("IMIdeaReassign_log")]
    public class IMIdeaReassign_log
    {


        public int Id { get; set; }
        public int IdeaId { get; set; }
        public int ClientId { get; set; }
        public int AssignedByUser { get; set; }
        public DateTime? AssignedDateTime { get; set; }
        public int Owner { get; set; }
        public int NewOwner { get; set; }
    }
}