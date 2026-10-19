using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Areas.ClientProfileManager.ViewModel;

namespace WebUI.Areas.ClientProfileManager.Dto
{
    public class jobDetailInput
    {
        public int? ClientId { get; set; }
        public DateTime? fromDt { get; set; }
        public DateTime? toDt { get; set; }
        public IEnumerable<JobDetailViewModel> jobDetailList { get; set; }
    }
}