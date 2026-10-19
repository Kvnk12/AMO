using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.IncidentNavigator.Models
{
    public class KEDBSolutionStatusModel
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public int Weightage { get; set; }
    }

    public enum KEDBSolutionStatus
    {
        Approved = 1,
        Draft =2,
        Retired = 3
    }
}