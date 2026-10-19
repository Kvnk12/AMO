using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using WebUI.Areas.PortfolioAnalyzer.Models;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.PortfolioAnalyzer.Models
{

    ////public enum metadatatypeEnum
    ////{
    ////    Domain,
    ////    archi,
    ////    Sourcecodetype
    ////}

    public class MetaDataModel
    {

        public int Id { get; set; }
        public string Text { get; set; }
        public int ClientId { get; set; }
        public string Value { get; set; }
        // public metadatatypeEnum metadataType { get; set; }
        public int MetadataTypeId { get; set; }

    }
}