using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Areas.PortfolioAnalyzer.Models
{
    public class DispNDispSlnModel
    {
        public int MapId { get; set; }
        public int? ClientId { get; set; }
        public int DispId { get; set; }
        [UIHint("DispSolutionId")]
        public int DispSolutionId { get; set; }
    }
}