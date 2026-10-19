using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class IndSubVerticalModel
    {
        public int VerticalId { get; set; }
        public string VerticalName { get; set; }
        public int SubVerticalId { get; set; }
        [Required(ErrorMessage = "Enter SubVertical Name")]
        public string SubVerticalName { get; set; }
        public string Description { get; set; }
    }
}