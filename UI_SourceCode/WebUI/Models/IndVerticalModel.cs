using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class IndVerticalModel
    {
        public int VerticalId { get; set; }
        [Required(ErrorMessage = "Enter Vertical Name")]
        public string VerticalName { get; set; }
        public string Description { get; set; }
    }
}