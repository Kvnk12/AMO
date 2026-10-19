using System;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NTTData.AMOPortal.Domain.Entities
{
    /// <summary>
    ///     No Metadata Documentation available.
    /// </summary>
    public partial class heatUseMetadata : DelEntity
    {
        //#region Factory Method

        ///// <summary>
        /////     Create a new heatUseMetadata object.
        ///// </summary>
        ///// <param name="heatUseMetadataId">Initial value of the HeatUseMetadataId property.</param>
        //public static heatUseMetadata CreateheatUseMetadata(Int32 heatUseMetadataId)
        //{
        //    var heatUseMetadata = new heatUseMetadata();
        //    heatUseMetadata.HeatUseMetadataId = heatUseMetadataId;
        //    return heatUseMetadata;
        //}

        //#endregion

        //#region Primitive Properties

        ///// <summary>
        /////     No Metadata Documentation available.
        ///// </summary>
        //[EdmScalarProperty(EntityKeyProperty = true, IsNullable = false)]
        //[DataMember]
        //public Int32 HeatUseMetadataId { get; set; }

        ///// <summary>
        /////     No Metadata Documentation available.
        ///// </summary>
        //[EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        //[DataMember]
        //public int? VariableId { get; set; }

        ///// <summary>
        /////     No Metadata Documentation available.
        ///// </summary>
        //[EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        //[DataMember]
        //public int? MetadataType { get; set; }

        ///// <summary>
        /////     No Metadata Documentation available.
        ///// </summary>
        //[EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        //[DataMember]
        //public int? MetaDataData { get; set; }

        ///// <summary>
        /////     No Metadata Documentation available.
        ///// </summary>
        //[EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        //[DataMember]
        //public int? MetaDataScore { get; set; }

        ///// <summary>
        /////     No Metadata Documentation available.
        ///// </summary>
        //[EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        //[DataMember]
        //public int? MetaDataWeight { get; set; }
        
        ///// <summary>
        /////     No Metadata Documentation available.
        ///// </summary>
        //[EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        //[DataMember]
        //public int? CreatedByUser { get; set; }

        ///// <summary>
        /////     No Metadata Documentation available.
        ///// </summary>
        //[EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        //[DataMember]
        //public DateTime? CreatedDate { get; set; }

        ///// <summary>
        /////     No Metadata Documentation available.
        ///// </summary>
        //[EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        //[DataMember]
        //public int? LastUpdatedUser { get; set; }

        ///// <summary>
        /////     No Metadata Documentation available.
        ///// </summary>
        //[EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        //[DataMember]
        //public DateTime? LastUpdatedDate { get; set; }
        
        //#endregion

        //#region Navigation Properties

        ///// <summary>
        /////     No Metadata Documentation available.
        ///// </summary>
        //[XmlIgnore]
        //[SoapIgnore]
        //[DataMember]
        //[EdmRelationshipNavigationProperty("NTTData.AMOPortal.Domain.Entities", "FK_mdmHeatmapConfig_heatUseMetadata", "mdmHeatmapConfig")]
        //public EntityCollection<mdmHeatmapConfig> mdmHeatmapConfig
        //{
        //    get
        //    {
        //        return
        //            ((IEntityWithRelationships) this).RelationshipManager.GetRelatedCollection<mdmHeatmapConfig>(
        //                "NTTData.AMOPortal.Domain.Entities.FK_mdmHeatmapConfig_heatUseMetadata", "mdmHeatmapConfig");
        //    }
        //    set
        //    {
        //        if ((value != null))
        //        {
        //            ((IEntityWithRelationships) this).RelationshipManager.InitializeRelatedCollection(
        //                "NTTData.AMOPortal.Domain.Entities.FK_mdmHeatmapConfig_heatUseMetadata", "mdmHeatmapConfig", value);
        //        }
        //    }
        //}

        //#endregion
    }
}