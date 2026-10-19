using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Areas.RetirementAccelerator.Models
{
    public class WbsStatusModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        [Required]
        public string WbsStatus { get; set; }
    }
}