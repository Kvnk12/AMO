using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.TransformationAccelerator.Model
{
    public class BaseLineDataModel
    {
        public string BaseLinePeriod { get; set; }
        public int? IncMonth { get; set; }
        public int? EveMonth { get; set; }
        public int? SerReqMonth { get; set; }

        public IList<ResourceDetailsModel>  resDetails{get;set;}
    }
}