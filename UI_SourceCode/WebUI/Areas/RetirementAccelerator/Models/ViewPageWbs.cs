using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.RetirementAccelerator.Models
{
    public class ViewPageWbs
    {
        public IEnumerable<SelectListItem> WorkProduct { get; set; }
        public IEnumerable<SelectListItem> Role { get; set; }
        public IEnumerable<SelectListItem> ResourceGroup { get; set; }
    }
}