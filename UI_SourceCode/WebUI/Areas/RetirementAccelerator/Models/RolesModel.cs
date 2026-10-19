using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.RetirementAccelerator.Models
{
    public class RolesModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}