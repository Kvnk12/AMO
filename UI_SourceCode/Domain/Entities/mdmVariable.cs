using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("mdmVariable")]
    public class mdmVariable : DelEntity
    {

        public int ClientId { get; set; }

        [Column("fkVariableLabelId")]
        public int VariableLabelId { get; set; }
        
        //[ForeignKey("VariableLabelId")]
        //public virtual mdmVariableLabel Label { get; set; }

        [Column("fkVariableTypeId")]
        public int VariableTypeId { get; set; }
        
        //[ForeignKey("VariableTypeId")]
        //public virtual VariableType VariableType { get; set; }

        [Column("fkVariableCategoryId")]
        public int VariableCatId { get; set; }

        //[ForeignKey("VariableCategoryId")]
        //public virtual mdmVariableCategory Category { get; set; }

        public string CreatedByUser { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string  LastUpdatedUser { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        //public string Name { get { return Label != null ? Label.VariableLabel : string.Empty; } }



    }
}