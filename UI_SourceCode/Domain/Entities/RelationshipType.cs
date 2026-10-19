using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;


namespace NTTData.AMOPortal.Domain.Entities
{
    [EdmEntityType(NamespaceName = "NTTData.AMOPortal.Domain.Entities", Name = "mdmRelationshipType")]
    [Serializable]
    [DataContract(IsReference = true)]
    [Table("mdmRelationshipType")]
    public class RelationshipType : DelEntity, ITypeClass
    {
        [EdmScalarProperty(EntityKeyProperty = true, IsNullable = false)]
        [DataMember]
        [Column("RelationshipTypeId")]
        public new Int32 Id { get; set; }

        public int TypeId { get { return Id; } }

        [EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        [DataMember]
        [MaxLength(50)]
        [Column("RelationshipType")]
        public string TypeName { get; set; }
        
        [EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        [DataMember]
        public int? CreatedByUser { get; set; }

        [EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        [DataMember]
        public DateTime? CreatedDate { get; set; }

        [EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        [DataMember]
        public int? LastUpdatedUser { get; set; }

        [EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        [DataMember]
        public DateTime? LastUpdatedDate { get; set; }

    }
}