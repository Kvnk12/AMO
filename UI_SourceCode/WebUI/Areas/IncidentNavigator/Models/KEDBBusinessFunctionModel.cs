using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.IncidentNavigator.Models
{
    public class KEDBBusinessFunctionModel
    {
        public int Id { get; set; }
        public string BusinessFunction { get; set; }
        public int ClientId { get; set; }
    }
}