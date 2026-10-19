using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.IncidentNavigator.Models;

namespace WebUI.Areas.IncidentNavigator.Dto
{
    public class INDictionaryList
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public int ClientId { get; set; }
        public string ExcludedBy { get; set; }
        public string ExcludedDate { get; set; }

    }
}