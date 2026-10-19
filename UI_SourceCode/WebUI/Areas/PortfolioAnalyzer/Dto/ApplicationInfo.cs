using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.PortfolioAnalyzer.Models;
using Domain.Abstract;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using Domain.Entities.PortfolioAnalyzer;
using Data;
using NTTData.AMOPortal.WebUI.Dto;
using PagedList;
namespace WebUI.Areas.PortfolioAnalyzer.Dto
{
    public class ApplicationInfo
    {
        public int? ClientId { get; set; }
        public string ClientName { get; set; }
        public IPagedList<ApplicationInformationModel> AppInfo { get; set; }
      //  public IEnumerable<ApplicationInformationModel> AppInfoList { get; set; }
        


       
    }

   

}