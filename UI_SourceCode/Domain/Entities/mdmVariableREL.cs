using System;
using System.ComponentModel;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("mdmVariableREL")]
    public class mdmVariableREL : DelEntity
    {
        

        

        public int RelationshipTypeId { get; set; }

        public int? RelationshipVariableId1 { get; set; }

        public int? RelationshipVariableId2 { get; set; }

        public int? DisplayOrder { get; set; }

        public string CreatedByUser { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string LastUpdatedUser { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

    }
}