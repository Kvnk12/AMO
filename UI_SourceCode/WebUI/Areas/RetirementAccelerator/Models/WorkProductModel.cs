using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Areas.RetirementAccelerator.Models
{
    public class WorkProductModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        [Required]
        public string WorkProduct { get; set; }
        [Url]
        public string Url { get; set; }
    }
}