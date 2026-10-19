using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("mdmVariableLabel")]
    public partial class mdmVariableLabel : DelEntity
    {
        //[EdmScalarProperty(EntityKeyProperty = true, IsNullable = false)]
        //[DataMember]
        //[Column("VariableLabelId")]
        //public new Int32 Id { get; set; }

        [EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        [DataMember]
        [MaxLength(50)]
        public String VariableLabel { get; set; }

        [MaxLength(500)]
        public string FullDescription { get; set; }

        [EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        [DataMember]
        public int? DisplayOrder { get; set; }

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