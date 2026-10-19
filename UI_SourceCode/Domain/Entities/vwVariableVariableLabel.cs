using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
{
    
   [Table("vwVariableVariableLabel")]
    public class vwVariableVariableLabel : Entity 
    {
        public int ClientId { get; set; }
        public int fkVariableLabelId{ get; set; }
        public int VariableId { get; set; }
        public int fkVariableCategoryId { get; set; }
       public   int fkVariableTypeId {get;set;}
       public string  VariableLabel{get;set;}
       public string  VariableType { get; set; }
       public string VariableCategory { get; set; }
       public string FullDescription { get; set; }

       
    }
}
