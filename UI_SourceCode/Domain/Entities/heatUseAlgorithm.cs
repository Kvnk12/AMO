using System;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NTTData.AMOPortal.Domain.Entities
{
    /// <summary>
    ///     No Metadata Documentation available.
    /// </summary>
    [EdmEntityType(NamespaceName = "NTTData.AMOPortal.Domain.Entities", Name = "heatUseAlgorithm")]
    [Serializable]
    [DataContract(IsReference = true)]
    public partial class heatUseAlgorithm : DelEntity
    {
        

        //#region Primitive Properties

        ///// <summary>
        /////     No Metadata Documentation available.
        ///// </summary>
        //[EdmScalarProperty(EntityKeyProperty = true, IsNullable = false)]
        //[DataMember]
        //public Int32 AlgorithmId { get; set; }

        ///// <summary>
        /////     No Metadata Documentation available.
        ///// </summary>
        //[EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        //[DataMember]
        //public int? AlgorithmVariable { get; set; }

        ///// <summary>
        /////     No Metadata Documentation available.
        ///// </summary>
        //[EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        //[DataMember]
        //public int? AlgorithmVariableWeight { get; set; }

        ///// <summary>
        /////     No Metadata Documentation available.
        ///// </summary>
        //[EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        //[DataMember]
        //public String VariableWeightSelected { get; set; }
        
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

     
    }
}