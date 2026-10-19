using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.IdeaManager.Dto
{
    public class IMSystemImpactedServiceModal 
    {
        public int Id { get; set; }
        public string SystemImpactedService { get; set; }
        public int? ClientId { get; set; }

    }
}
