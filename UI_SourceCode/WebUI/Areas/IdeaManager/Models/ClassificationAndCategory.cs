using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.IdeaManager.Models
{
    public class ClassificationAndCategory
    {
        public int Id { get; set; }
        public bool Checked { get; set; }
        public bool Expanded { get; set; }
        public bool HasChildren { get; set; }
        public string Name { get; set; }
        public IList<ClassificationAndCategory> ChildModel { get; set; }
    }
}