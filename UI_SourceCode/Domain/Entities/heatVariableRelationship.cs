using System;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NTTData.AMOPortal.Domain.Entities
{
    /// <summary>
    ///     No Metadata Documentation available.
    /// </summary>
    [EdmEntityType(NamespaceName = "NTTData.AMOPortal.Domain.Entities", Name = "heatVariableRelationship")]
    [Serializable]
    [DataContract(IsReference = true)]
    public partial class heatVariableRelationship : DelEntity
    {
        #region Factory Method

        /// <summary>
        ///     Create a new heatVariableRelationship object.
        /// </summary>
        /// <param name="heatVariableRelationshipId">Initial value of the HeatVariableRelationshipId property.</param>
        public static heatVariableRelationship CreateheatVariableRelationship(Int32 heatVariableRelationshipId)
        {
            var heatVariableRelationship = new heatVariableRelationship();
            heatVariableRelationship.HeatVariableRelationshipId = heatVariableRelationshipId;
            return heatVariableRelationship;
        }

        #endregion

        #region Primitive Properties
        
        /// <summary>
        ///     No Metadata Documentation available.
        /// </summary>
        [EdmScalarProperty(EntityKeyProperty = true, IsNullable = false)]
        [DataMember]
        public Int32 HeatVariableRelationshipId { get; set; }

        /// <summary>
        ///     No Metadata Documentation available.
        /// </summary>
        [EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        [DataMember]
        public int? VariableId { get; set; }

        /// <summary>
        ///     No Metadata Documentation available.
        /// </summary>
        [EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        [DataMember]
        public int? RelationshipVariableId { get; set; }

        /// <summary>
        ///     No Metadata Documentation available.
        /// </summary>
        [EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        [DataMember]
        public int? RelationshipVariableSelected { get; set; }

        /// <summary>
        ///     No Metadata Documentation available.
        /// </summary>
        [EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        [DataMember]
        public int? ReportId { get; set; }

        /// <summary>
        ///     No Metadata Documentation available.
        /// </summary>
        [EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        [DataMember]
        public int? CreatedByUser { get; set; }

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
        public int? LastUpdatedUser { get; set; }

        /// <summary>
        ///     No Metadata Documentation available.
        /// </summary>
        [EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        [DataMember]
        public DateTime? LastUpdatedDate { get; set; }

        #endregion

        #region Navigation Properties

        /// <summary>
        ///     No Metadata Documentation available.
        /// </summary>
        [XmlIgnore]
        [SoapIgnore]
        [DataMember]
        [EdmRelationshipNavigationProperty("NTTData.AMOPortal.Domain.Entities", "FK_mdmHeatmapConfig_heatVariableRelationship",
            "mdmHeatmapConfig")]
        public EntityCollection<mdmHeatmapConfig> mdmHeatmapConfig
        {
            get
            {
                return
                    ((IEntityWithRelationships) this).RelationshipManager.GetRelatedCollection<mdmHeatmapConfig>(
                        "NTTData.AMOPortal.Domain.Entities.FK_mdmHeatmapConfig_heatVariableRelationship", "mdmHeatmapConfig");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships) this).RelationshipManager.InitializeRelatedCollection(
                        "NTTData.AMOPortal.Domain.Entities.FK_mdmHeatmapConfig_heatVariableRelationship", "mdmHeatmapConfig", value);
                }
            }
        }

        #endregion
    }
}