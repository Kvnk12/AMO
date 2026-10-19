using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace NTTData.AMOPortal.Domain.Entities
    {

    [Table("mdmVariableMD")]
    public class mdmVariableMD : DelEntity
        {


        public int? VariableID { get; set; }

        public int? MetadataType { get; set; }

        public int MetaDataData { get; set; }

        //public int? MetaDataScore { get; set; }
        public int MetaDataScore { get; set; }

        public string MetaDataValue { get; set; }

        //public int? MetaDataWeight { get; set; }
        public int MetaDataWeight { get; set; }

        public string CreatedByUser { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string LastUpdatedUser { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        }
    }