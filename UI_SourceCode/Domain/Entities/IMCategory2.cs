using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("IMCategory2")]
    public class IMCategory2 : DelEntity
    {

        public string Category2 { set; get; }

        public int IdeaId { set; get; }

        public string CreatedByUser { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string LastUpdatedUser { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }
    }
}