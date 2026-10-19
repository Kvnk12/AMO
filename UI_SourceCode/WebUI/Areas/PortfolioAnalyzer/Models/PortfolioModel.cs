using Domain.Entities.ClientProfileManager;
using NTTData.AMOPortal.WebUI.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebUI.Areas.PortfolioAnalyzer.Models
{
    public class PortfolioModel
    {
        public int LOBId { get; set; }
        public string LOBName { get; set; }
        public int ClientId { get; set; }
    }
}