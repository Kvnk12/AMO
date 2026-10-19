using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Areas.PortfolioAnalyzer.Models
{
    public class ClientHeatmapModel
    {
        public int PerspectiveId { get; set; }
        [DisplayName("Perspective name")]
        public string PerspectiveName { get; set; }

        [DisplayName("BB&T")]
        public decimal BBTRating { get; set; }

        [DisplayName("SunTrust")]
        public decimal STRating { get; set; }
    }
}