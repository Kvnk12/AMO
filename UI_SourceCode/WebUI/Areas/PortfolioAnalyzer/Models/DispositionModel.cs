using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.PortfolioAnalyzer.Models
{
    public class DispositionModel
    {
        public int DispId { get; set; }
        public string DispositionName { get; set; }
        public int ClientId { get; set; }
    }
}