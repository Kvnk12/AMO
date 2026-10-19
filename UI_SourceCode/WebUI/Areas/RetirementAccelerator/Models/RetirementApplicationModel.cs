using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Areas.RetirementAccelerator.Models
{
    public class RetirementApplicationModel
    {
        public int Id { get; set; }
        public bool InScope { get; set; }
        [Required]
        public string ApplicationName { get; set; }
        public bool? IsRetirementCandidate { get; set; }
    }
}