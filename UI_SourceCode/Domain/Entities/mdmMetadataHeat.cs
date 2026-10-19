using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;

namespace NTTData.AMOPortal.Domain.Entities
{
    /// <summary>
    ///     No Metadata Documentation available.
    /// </summary>
    [EdmEntityType(NamespaceName = "NTTData.AMOPortal.Domain.Entities", Name = "mdmMetadataHeat")]
    [Serializable]
    [DataContract(IsReference = true)]
    public partial class mdmMetadataHeat : DelEntity
    {
        #region Factory Method

        /// <summary>
        ///     Create a new mdmMetadataHeat object.
        /// </summary>
        /// <param name="mdmMetadataHeatId">Initial value of the mdmMetadataHeatId property.</param>
        public static mdmMetadataHeat CreatemdmMetadataHeat(Int32 mdmMetadataHeatId)
        {
            var mdmMetadataHeat = new mdmMetadataHeat();
            mdmMetadataHeat.mdmMetadataHeatId = mdmMetadataHeatId;
            return mdmMetadataHeat;
        }

        #endregion

        #region Primitive Properties

        //private Int32 _mdmMetadataHeatId;

        /// <summary>
        ///     No Metadata Documentation available.
        /// </summary>
        [EdmScalarProperty(EntityKeyProperty = true, IsNullable = false)]
        [DataMember]
        [Key]
        public Int32 mdmMetadataHeatId { get; set; }

        #endregion
    }
}