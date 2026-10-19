using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.TransformationAccelerator.Model
{
    public class ResourceDetailsModel
    {
        //Resource Details Data
        public int Id { get; set; }
        public string Location { get; set; }
        public int? Year { get; set; }
        public string Quarter { get; set; }
        public decimal FTE { get; set; }

    }
}