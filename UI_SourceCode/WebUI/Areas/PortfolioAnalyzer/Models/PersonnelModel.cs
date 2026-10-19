using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.PortfolioAnalyzer.Models
{
    public class PersonnelModel
    {
        public int Id { get; set; }
        public string PersonnelName { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}