using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Areas.PortfolioAnalyzer.Dto;
using System.Web.Mvc;
namespace WebUI.Areas.PortfolioAnalyzer.Models
{
    public class AprInterfaceModel
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public int ApplicationInterfaced { get; set; }
        public string strApplicationInterfaced { get; set; }
        public string ApplicationInterfacedText { get; set; }
        public string InterfaceType { get; set; }
        public SelectListItem appinfo { get; set; }
        public string CountInboundInterfaces { get; set; }
        public string CountOutboundInterfaces { get; set; }
        public bool? EntangledInterface { get; set; }
        public string[] InterfaceMethods { get; set; }
        public string[] InterfaceMethodsId { get; set; }
        public IEnumerable<SelectListItem> InterfaceMethodList { get; set; }

    }
}