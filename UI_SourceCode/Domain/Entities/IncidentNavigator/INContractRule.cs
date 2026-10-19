using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.IncidentNavigator
{
    [Table("INContractRule")]
    public class INContractRule
    {
        [Key]
        public int Id { get; set; }
        public int ContractId { get; set; }
        public string INFieldName { get; set; }
        public string FieldValue { get; set; }
        public string Operator { get; set; }
        public string GroupStartOperator { get; set; }
        public string GroupEndOperator { get; set; }
        public string Condition { get; set; }
        public string ConstantValue { get; set; }
    }
}

