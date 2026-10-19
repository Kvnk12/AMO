using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.IncidentNavigator.Models
{
    public class KEDBSolutionCategoryModel
    {
        public int Id { get; set; }
        public string SolCategory { get; set; }
        public int ClientId { get; set; }
    }
}