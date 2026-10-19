using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.ClientProfileManager.Dto
{
    public class PerspectiveCategoryModel
    {
        public int Id { get; set; }
        public int? ClientId { get; set; }
        public int? ToolId { get; set; }
        public string PerspectiveCategoryName { get; set; }
    }
}