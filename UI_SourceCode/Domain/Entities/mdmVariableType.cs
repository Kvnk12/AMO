using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("mdmVariableType")]
    public class mdmVariableType : DelEntity  //, ITypeClass
    {
        //[EdmScalarProperty(EntityKeyProperty = true, IsNullable = false)]
        //[DataMember]
        //[Column("VariableTypeId")]
        //public new Int32 Id { get; set; }

        //public int TypeId { get { return Id; } }

        
        //public int? fkParentVariableType { get; set; }

        //[ForeignKey("ParentVariableTypeId")]
        //public virtual VariableType ParentVariableType { get; set; }
        [MaxLength(255)]
        public string VariableType { get; set; }

        [EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        [DataMember]
        public string CreatedByUser { get; set; }

        [EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        [DataMember]
        public DateTime? CreatedDate { get; set; }

        [EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        [DataMember]
        public string LastUpdatedUser { get; set; }

        [EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        [DataMember]
        public DateTime? LastUpdatedDate { get; set; }

    }
}