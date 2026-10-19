using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Areas.RetirementAccelerator.Models
{
    public class EstimateCategoryModel
    {
        public int Id { get; set; }
        [Required]
        public string EstimateCategory { get; set; }
        public int ClientId { get; set; }
    }
}

