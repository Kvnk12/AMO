using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.IdeaManager.Dto
{
    public class IdeaTaskDependencyModel:IGanttDependency
    {
      
            public int DependencyID { get; set; }
            public int IdeaPostId { get; set; }
            public int PredecessorID { get; set; }
            public int SuccessorID { get; set; }
            public DependencyType Type { get; set; }
        
    }
}