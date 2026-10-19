using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.WebUI.Dto
{
    public class BusinessCapabilitiesModel
    {

        public int BCId { get; set; }
        [Required]
        [Display(Name = "Business Capability Name")]
        public string BusinessCapName { get; set; }
        public int? BCParentId { get; set; }
        public int? SubVerticalId { get; set; }
        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string InternalExternal { get; set; }

        public string SubverticalName { get; set; }
        public int? ClientId { get; set; }
        // public bool checked {get;set;}

        [Display(Name = "Process Importance")]
        public int? ProcessImportanceId { get; set; }
        public string ProcessImportance { get; set; }
        [Display(Name = "Process Health")]
        public int? ProcessHealthId { get; set; }
        public string ProcessHealth { get; set; }

        public string Description { get; set; }

        public int? OrderId { get; set; }

        public string Notes { get; set; }
        public IEnumerable<SelectListItem> PortfolioList { get; set; }
    }
}