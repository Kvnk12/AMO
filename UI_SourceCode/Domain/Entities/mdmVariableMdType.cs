using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("mdmVariableMdtype")]
    public class mdmVariableMdType:DelEntity
    {
        public string TypeName { get; set; }



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
