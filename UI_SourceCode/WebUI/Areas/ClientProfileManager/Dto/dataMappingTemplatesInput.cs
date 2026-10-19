using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Areas.ClientProfileManager.ViewModel;

namespace WebUI.Areas.ClientProfileManager.Dto
{
    public class dataMappingTemplatesInput
    {
        public int? ClientId { get; set; }
        public IList<MappingTemplateViewModel> templates { get; set; }
    }
}