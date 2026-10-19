using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.IdeaManager.Dto
{
    public class ImplementationManagerModel
    {
        public int IdeaId { get; set; }
        public string IdeaName { get;set;}
        public List<SelectListItem> ManagerList { get; set; }
        public string Comments { get; set; }
    }
}