using Data;
using System.Data;
using Domain.Abstract;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NTTData.AMOPortal.WebUI.Mapper;
using NTTData.AMOPortal.Domain.Service;
using System.IO;
using System.Text;
using Domain.Entities.PortfolioAnalyzer;
using WebUI.Areas.PortfolioAnalyzer.Dto;
using WebUI.Areas.PortfolioAnalyzer.Models;
using System.Web.Http;
using PagedList;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Web.Mvc;


namespace WebUI.Areas.PortfolioAnalyzer.Models
{
    public class AppSoftwareForeignKeyValues
    {
        public IEnumerable<Vendor> Vendor { get; set; }
        //public IEnumerable<Vendor> Vendors { get; set; }
        public IEnumerable<VendorProduct> VendorProduct { get; set; }

        public IEnumerable<VendorProductVersion> VendorProductVersion { get; set; }
    }
}