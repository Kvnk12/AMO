using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{

   [Table("VwMDMrelationwithlabel")]
    public class VwMDMrelationwithlabel : Entity 
    {
        public int ClientId { get; set; }
        public int fkVariableLabelId{ get; set; }
        public int fkVariableCategoryId { get; set; }
       public   int fkVariableTypeId {get;set;}
       public   int RelationshipTypeId {get;set;}
       public int RelationshipVariableId1{get;set;}
       public int RelationshipVariableId2{get;set;}
       public string VariableCategory { get; set; }
       public string VariableType { get; set; }
       public string rel1VariableLable { get; set; }
       public string rel2VariableLable { get; set; }
    }
}
