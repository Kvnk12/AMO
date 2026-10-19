using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Areas.PortfolioAnalyzer.Models
{
    public class VendorMetadataModel
    {
        public int VId { get; set; }

        [UIHint("String")]
        public string VendorName { get; set; }
    }
}