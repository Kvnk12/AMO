using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("DMItemCategoryMap")]
    public class DMItemCategoryMap : DelEntity
    {
        public int ItemId { get; set; }
        public int CategoryId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public int? ClientId { get; set; }

    }
}