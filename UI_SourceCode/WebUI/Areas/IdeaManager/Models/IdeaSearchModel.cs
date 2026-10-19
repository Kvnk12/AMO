using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.IdeaManager.Models
{
    public class IdeaSearchModel
    {
        public int IdeaId { get; set; }
        public string IdeaNumber { get; set; }
        public string IdeaName { get; set; }
        public string Status { get; set; }
    }
}