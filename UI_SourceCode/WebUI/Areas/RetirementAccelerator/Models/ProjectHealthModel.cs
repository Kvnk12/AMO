using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Areas.RetirementAccelerator.Models
{
    public class ProjectHealthModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        [Required]
        public string ProjectHealth { get; set; }
        [Required]
        public string Color { get; set; }
    }
}