using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WebUI.Areas.IncidentNavigator.Models
{
    public class INContractRuleModel
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public string INFieldName { get; set; }
        // [Required(ErrorMessage = "Column value is Required")]
        public string FieldValue { get; set; }
        // [Required(ErrorMessage = "Operator is Required")]
        public string Operator { get; set; }
        public string GroupStartOperator { get; set; }
        public string GroupEndOperator { get; set; }
        [Required(ErrorMessage = "Condition is Required")]
        public string Condition { get; set; }
        //public int FieldLength { get; set; }
        public string ConstantValue { get; set; }
    }
}