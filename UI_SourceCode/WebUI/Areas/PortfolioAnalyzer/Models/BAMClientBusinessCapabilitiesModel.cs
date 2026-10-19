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

namespace WebUI.Areas.PortfolioAnalyzer.Models
{
    public class BAMClientBusinessCapabilitiesModel
    {
        public int BCId { get; set; }
        [Required]
        public string BusinessCapName { get; set; }
        public int? BCParentId { get; set; }
        public int? SubVerticalId { get; set; }
        public int? CreatedBy { get; set; }

        public int? ClientId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string InternalExternal { get; set; }

 
    }
}