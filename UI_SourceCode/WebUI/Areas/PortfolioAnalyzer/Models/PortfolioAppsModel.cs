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
    public class PortfolioAppModel
    {
        public int LobAppId { get; set; }
        public int AppId { get; set; }
        public int PortFolioId { get; set; }
        public string ApplicationName{ get; set; }
        public string PortFolioName { get; set; }
        public string ApplicationAliasName { get; set; }
        public string ClientApplicationNumber { get; set; }
        
    }
}