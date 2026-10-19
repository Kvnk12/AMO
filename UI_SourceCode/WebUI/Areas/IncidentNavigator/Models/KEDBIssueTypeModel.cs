using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.IncidentNavigator.Models
{
    public class KEDBIssueTypeModel
    {
        public int Id { get; set; }
        public string IssueType { get; set; }
        public int ClientId { get; set; }
    }
}