using System;
using System.ComponentModel;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NTTData.AMOPortal.Domain.Entities
{
    /// <summary>
    ///     No Metadata Documentation available.
    /// </summary>
    [EdmEntityType(NamespaceName = "NTTData.AMOPortal.Domain.Entities", Name = "heatDist")]
    [Serializable]
    [DataContract(IsReference = true)]
    public partial class heatDist : DelEntity
    {
        //#region Factory Method

        ///// <summary>
        /////     Create a new heatDist object.
        ///// </summary>
        ///// <param name="heatDistId">Initial value of the HeatDistId property.</param>
        //public static heatDist CreateheatDist(Int32 heatDistId)
        //{
        //    var heatDist = new heatDist();
        //    heatDist.HeatDistId = heatDistId;
        //    return heatDist;
        //}

        //#endregion

        //#region Primitive Properties

        ///// <summary>
        /////     No Metadata Documentation available.
        ///// </summary>
        //[EdmScalarProperty(EntityKeyProperty = true, IsNullable = false)]
        //[DataMember]
        //public Int32 HeatDistId { get; set; }

        ///// <summary>
        /////     No Metadata Documentation available.
        ///// </summary>
        //[EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        //[DataMember]
        //public int? HeatConfigId { get; set; }

        ///// <summary>
        /////     No Metadata Documentation available.
        ///// </summary>
        //[EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        //[DataMember]
        //public int? NotificationAlert { get; set; }

        ///// <summary>
        /////     No Metadata Documentation available.
        ///// </summary>
        //[EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        //[DataMember]
        //public int? ReportId { get; set; }

        ///// <summary>
        /////     No Metadata Documentation available.
        ///// </summary>
        //[EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        //[DataMember]
        //public int? GenerateReportOption { get; set; }

        ///// <summary>
        /////     No Metadata Documentation available.
        ///// </summary>
        //[EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        //[DataMember]
        //public int? HeatUseReportId { get; set; }
        
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
        //[EdmRelationshipNavigationProperty("NTTData.AMOPortal.Domain.Entities", "FK_heatDist_mdmHeatmapConfig", "mdmHeatmapConfig")]
        //public mdmHeatmapConfig mdmHeatmapConfig
        //{
        //    get
        //    {
        //        return
        //            ((IEntityWithRelationships) this).RelationshipManager.GetRelatedReference<mdmHeatmapConfig>(
        //                "NTTData.AMOPortal.Domain.Entities.FK_heatDist_mdmHeatmapConfig", "mdmHeatmapConfig").Value;
        //    }
        //    set
        //    {
        //        ((IEntityWithRelationships) this).RelationshipManager.GetRelatedReference<mdmHeatmapConfig>(
        //            "NTTData.AMOPortal.Domain.Entities.FK_heatDist_mdmHeatmapConfig", "mdmHeatmapConfig").Value = value;
        //    }
        //}

        ///// <summary>
        /////     No Metadata Documentation available.
        ///// </summary>
        //[Browsable(false)]
        //[DataMember]
        //public EntityReference<mdmHeatmapConfig> mdmHeatmapConfigReference
        //{
        //    get
        //    {
        //        return
        //            ((IEntityWithRelationships) this).RelationshipManager.GetRelatedReference<mdmHeatmapConfig>(
        //                "NTTData.AMOPortal.Domain.Entities.FK_heatDist_mdmHeatmapConfig", "mdmHeatmapConfig");
        //    }
        //    set
        //    {
        //        if ((value != null))
        //        {
        //            ((IEntityWithRelationships) this).RelationshipManager.InitializeRelatedReference(
        //                "NTTData.AMOPortal.Domain.Entities.FK_heatDist_mdmHeatmapConfig", "mdmHeatmapConfig", value);
        //        }
        //    }
        //}

        //#endregion
    }
}