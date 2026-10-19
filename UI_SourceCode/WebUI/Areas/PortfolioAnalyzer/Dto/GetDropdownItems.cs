using System;
using System.Collections.Generic;
using Domain.Abstract;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using System.Linq;
using System.Web;
using Domain.Entities.PortfolioAnalyzer;
using Domain.Entities.ClientProfileManager;
using System.Web.Mvc;
using Data;


namespace WebUI.Areas.PortfolioAnalyzer.Dto
{
    public class GetDropdownItems
    {
        public List<SelectListItem> Sfields { get; set; }
        public List<SelectListItem> SourceFields { get; set; }
    }

}