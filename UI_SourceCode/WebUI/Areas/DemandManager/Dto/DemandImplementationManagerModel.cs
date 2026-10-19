using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.DemandManager.Dto
{
    public class DemandImplementationManagerModel
    {
        public int DemandId { get; set; }
        public string DemandName { get;set;}
        public List<SelectListItem> ManagerList { get; set; }
        public string Comments { get; set; }
    }
}