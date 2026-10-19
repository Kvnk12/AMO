using System;
//using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NTTData.AMOPortal.Domain.Entities
{
    /// <summary>
    ///     No Metadata Documentation available.
    /// </summary>
    [System.Data.Objects.DataClasses.EdmEntityType(NamespaceName = "NTTData.AMOPortal.Domain.Entities", Name = "mdmRelationshipType")]
    [Serializable]
    [DataContract(IsReference = true)]
    public partial class mdmRelationshipType : DelEntity
    {
        #region Factory Method

        /// <summary>
        ///     Create a new mdmRelationshipType object.
        /// </summary>
        /// <param name="relationshipTypeId">Initial value of the RelationshipTypeId property.</param>
        public static mdmRelationshipType CreatemdmRelationshipType(Int32 relationshipTypeId)
        {
            var mdmRelationshipType = new mdmRelationshipType();
            mdmRelationshipType.RelationshipTypeId = relationshipTypeId;
            return mdmRelationshipType;
        }

        #endregion

        #region Primitive Properties

        /// <summary>
        ///     No Metadata Documentation available.
        /// </summary>
        [System.Data.Objects.DataClasses.EdmScalarProperty(EntityKeyProperty = true, IsNullable = false)]
        [DataMember]
        public Int32 RelationshipTypeId { get; set; }

        /// <summary>
        ///     No Metadata Documentation available.
        /// </summary>
        [System.Data.Objects.DataClasses.EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
        [DataMember]
        public int? RelationshipType { get; set; }
        
        /// <summary>
        ///     No Metadata Documentation available.
        /// </summary>
        [System.Data.Objects.DataClasses.EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
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
        [EdmRelationshipNavigationProperty("NTTData.AMOPortal.Domain.Entities", "FK_mdmVariableREL_mdmRelationshipType", "mdmVariableREL")]
        public EntityCollection<mdmVariableREL> mdmVariableREL
        {
            get
            {
                return
                    ((IEntityWithRelationships) this).RelationshipManager.GetRelatedCollection<mdmVariableREL>(
                        "NTTData.AMOPortal.Domain.Entities.FK_mdmVariableREL_mdmRelationshipType", "mdmVariableREL");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships) this).RelationshipManager.InitializeRelatedCollection(
                        "NTTData.AMOPortal.Domain.Entities.FK_mdmVariableREL_mdmRelationshipType", "mdmVariableREL", value);
                }
            }
        }

        #endregion
    }
}