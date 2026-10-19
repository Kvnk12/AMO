using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.PortfolioAnalyzer.Models
{
    public class ClientAccessModel
    {
        public int Id { get; set; }
        public int ParentClientId { get; set; }
        public int ClientId { get; set; }
        public string Client { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedDt { get; set; }
        public DateTime? UpdatedDt { get; set; }
    }
}