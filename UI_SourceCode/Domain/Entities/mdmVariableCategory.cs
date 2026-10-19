using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NTTData.AMOPortal.Domain.Entities
{
    /// <summary>
    ///     No Metadata Documentation available.
    /// </summary>
    //[EdmEntityType(NamespaceName = "NTTData.AMOPortal.Domain.Entities", Name = "mdmVariableCategory")]
    //[Serializable]
    //[DataContract(IsReference = true)]
    [Table("mdmVariableCategory")]
    public class mdmVariableCategory : DelEntity
    {
        /// <summary>
        ///     No Metadata Documentation available.
        /// </summary>
        //[EdmScalarProperty(EntityKeyProperty = true, IsNullable = false)]
        //[DataMember]
        //[Column("VariableCategoryId")]
        //public new Int32 Id { get; set; }

        /// <summary>
        ///     No Metadata Documentation available.
        /// </summary>
        [EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        [DataMember]
        [MaxLength(255)]
        public string VariableCategory { get; set; }
        
        /// <summary>
        ///     No Metadata Documentation available.
        /// </summary>
        [EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        [DataMember]
        public string CreatedByUser { get; set; }

        /// <summary>
        ///     No Metadata Documentation available.
        /// </summary>
        [EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        [DataMember]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        ///     No Metadata Documentation available.
        /// </summary>
        [EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        [DataMember]
        public string LastUpdatedUser { get; set; }

        /// <summary>
        ///     No Metadata Documentation available.
        /// </summary>
        [EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        [DataMember]
        public DateTime? LastUpdatedDate { get; set; }

    }
}