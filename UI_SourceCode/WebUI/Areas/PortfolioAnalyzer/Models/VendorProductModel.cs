using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Areas.PortfolioAnalyzer.Models
{
    public class VendorProductModel
    {
        public int ProductId { get; set; }
        public int VendorId { get; set; }
        [UIHint("String")]
        public string ProductName { get; set; }
    }
}