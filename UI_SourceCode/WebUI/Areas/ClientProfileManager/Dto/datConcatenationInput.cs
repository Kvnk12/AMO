using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace WebUI.Areas.ClientProfileManager.Dto
{
    public class datConcatenationInput
    {
        public List<SelectListItem> Sfields { get; set; }
        public List<SelectListItem> SourceFields { get; set; }
    }
}