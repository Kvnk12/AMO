using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Areas.PortfolioAnalyzer.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WebUI.Areas.PortfolioAnalyzer.Dto
{
    public class metadatadto
    {
        public string MetadataTypeId { get; set; }
        public string MetadataType { get; set; }
        public string SelectedMetadataId { get; set; }
        public string SelectedMetadataValue { get; set; }
        public IList<MetaDataModel> AppInfo { get; set; }

       
        

    }
}