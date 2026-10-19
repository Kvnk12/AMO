using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Areas.PortfolioAnalyzer.Models
{
    public class VendorProductVersionModel
    {

        public int VersionId { get; set; }
        public int ProductId { get; set; }
        [UIHint("String")]
        public string ProductVersionName { get; set; }
    }

    public class versionParmModel
    {
        public int prodid { get; set; }
    }

}