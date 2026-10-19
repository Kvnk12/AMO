using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Areas.RetirementAccelerator.Models
{
    public class WbsTemplateDetailsModel
    {
        public int ClientId { get; set; }
        public int WBSTemplateId { get; set; }

        public int TaskId { get; set; }
        [Required]
        public string Task { get; set; }
        public int? TParentId { get; set; }

        public int? WorkProductId { get; set; }
        public int? RoleId { get; set; }
        public int? ResourceGroupId { get; set; }

        public string WorkProduct { get; set; }
        public string Role { get; set; }
        public string ResourceGroup { get; set; }
    }
}