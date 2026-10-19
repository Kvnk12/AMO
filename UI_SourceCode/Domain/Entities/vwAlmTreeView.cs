using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.Domain.Entities
    {

    [Table("VWAlmTreeView")]
    public class vwAlmTreeView :Entity
        {

              public int   ClientId { get; set; }
              public string VARTYPE { get; set; }
              public int vartypeid { get; set; }
              public int VariableLabelId { get; set; }
              public string VariableLabel { get; set; }
              public int? RelationId { get; set; }
              public int? RelationshipTypeId { get; set; }
              public string RelationshipType { get; set; }
              public int RelationshipVariableId2 { get; set; }
              public string VAR_LBL_2 { get; set; }
              public int? VARCATID { get; set; }
              public string VariableCategory { get; set; }

        }
    }
