using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{

   [Table("vwMetaDataDetails")]
    public class vwMetaDataDetails : Entity 
    {
      public int ClientId { get; set; }
      public int variableId{ get; set; }
      public int metadataid { get; set; }
      public string  TypeName{get;set;}
      public string  MetadataType { get; set; }
      public string MetaDataValue { get; set; }
      public int MetaDataScore{ get; set; }
      public int MetaDataWeight { get; set; }
    }
}
