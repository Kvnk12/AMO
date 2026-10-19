using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("mdmVariable")]
    public class Variable : DelEntity
    {
        [Column("VariableId")]
        public new int Id { get; set; }

        public int? CustomerId { get; set; }

        [Column("fkVariableLabelId")]
        public int? VariableLabelId { get; set; }
        
        [ForeignKey("VariableLabelId")]
        public virtual mdmVariableLabel Label { get; set; }

        [Column("fkVariableTypeId")]
        public int? VariableTypeId { get; set; }
        
        [ForeignKey("VariableTypeId")]
        public virtual VariableType VariableType { get; set; }

        [Column("fkVariableCategoryId")]
        public int? VariableCategoryId { get; set; }
        
        [ForeignKey("VariableCategoryId")]
        public virtual mdmVariableCategory Category { get; set; }

        public int? CreatedByUser { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? LastUpdatedUser { get; set; }
        
        public DateTime? LastUpdatedDate { get; set; }

        public string Name { get { return Label != null ? Label.VariableLabel : string.Empty; } }



    }
}