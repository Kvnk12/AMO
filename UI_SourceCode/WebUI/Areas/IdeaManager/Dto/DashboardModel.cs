using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.IdeaManager.Dto
{
    public class DashboardModel
    {
        public string Category { get; set; }
        public int Count { get; set; }
        public string Percent { get; set; }


        public List<DashboardModel> IdeaTypes { get; set; }
        public List<DashboardModel> IdeaStatus { get; set; }
        public List<DashboardModel> IdeaSource { get; set; }
        public List<DashboardModel> IdeaPosted { get; set; }
    }
}