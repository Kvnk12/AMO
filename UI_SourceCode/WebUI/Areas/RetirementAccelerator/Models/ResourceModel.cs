using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.RetirementAccelerator.Models
{
    public class ResourceModel
    {
        public int Id { get; set; }
        [Required]
        public string RAResource { get; set; }
        public int ClientId { get; set; }
       
        //public int? CreatedBy { get; set; }
        //public DateTime? CreatedDt { get; set; }
        //public int? UpdatedBy { get; set; }
        //public DateTime? UpdatedDt { get; set; }
    }
}