using Domain.Entities.ClientProfileManager;
using Newtonsoft.Json;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Domain.Abstract;
using NTTData.AMOPortal.Domain.Service;
using NTTData.AMOPortal.WebUI.Dto;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Service;
using System.Globalization;
using WebUI.Areas.IncidentNavigator.Dto;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using WebUI.Areas.IncidentNavigator.Models;
using Data;
using Domain.Entities.IncidentNavigator;
using Kendo.Mvc.Export;
using Telerik.Documents.SpreadsheetStreaming;
using WebUI.Areas.ClientProfileManager.ViewModel;
using System.Data;

namespace WebUI.Areas.ClientProfileManager.Controllers
{
    public class ClientAjaxServiceController : Controller
    {
        public IEnumerable<TreeViewItemModel> DashboardUserClients(int userid)
        {
            List<TreeViewItemModel> inlineDefault = new List<TreeViewItemModel>();
            DbContextFactory Context = new DbContextFactory();
            Repo<NTTData.AMOPortal.Domain.Entities.Client> clients = new NTTData.AMOPortal.Data.Repo<NTTData.AMOPortal.Domain.Entities.Client>(Context);
            Repo<NTTData.AMOPortal.Domain.Entities.UserApplications> userApps = new NTTData.AMOPortal.Data.Repo<NTTData.AMOPortal.Domain.Entities.UserApplications>(Context);
            Repo<NTTData.AMOPortal.Domain.Entities.UserProfile> users = new NTTData.AMOPortal.Data.Repo<NTTData.AMOPortal.Domain.Entities.UserProfile>(Context);
            // List<SelectListItem> appsList = userApps.Where(w => w.UserId == userid).ToList();
            var usr = users.Get(userid);

            if (usr.RoleId.ToLower() == "superuser")
            {
                var clientList = from cl in clients.GetAll().ToList()
                                 where cl.IsDeleted == false
                                 select new { Value = cl.Id, Text = cl.ClientName };
                inlineDefault = clientList.Select(lst => new TreeViewItemModel
                {
                    Id = lst.Value.ToString(),
                    Text = lst.Text.ToString()
                }).GroupBy(g => g.Id, (key, group) => group.First()).ToList();

            }
            else
            {
                var clientList = from uApp in userApps.GetAll().ToList()
                                 join cl in clients.GetAll().ToList() on uApp.ClientId equals cl.Id
                                 where uApp.UserId == userid && cl.IsDeleted == false
                                 select new { Value = cl.Id, Text = cl.ClientName };
                inlineDefault = clientList.Select(lst => new TreeViewItemModel
                {
                    Id = lst.Value.ToString(),
                    Text = lst.Text.ToString()
                }).GroupBy(g => g.Id, (key, group) => group.First()).ToList();
            }

            return inlineDefault;

        }

        [HttpPost]
        //public ActionResult GetClientPerformanceDashboard([DataSourceRequest]DataSourceRequest request)

        public ActionResult GetClientPerformanceDashboard([DataSourceRequest]DataSourceRequest request, string ClientIds, string Quarter, DateTime yearDatetime,string Quarter1, DateTime yearDatetime1)
        {
           
            int year1 = yearDatetime.Year;
            int year2 = yearDatetime1.Year;
            int quarter1 = Convert.ToInt16(Quarter);
            int quarter2 = Convert.ToInt16(Quarter1);
            //string ClientIdlist = string.Join(",", v);
            //var client = (Client)Session["Client"];
            //int ClientId = client.Id;

            List<DashboardViewModel> DashboardViewModel = ClientProfileManagerHelper.CPMDashboard(ClientIds, quarter1,year1,quarter2,year2).ToList();
            //var model = new DashboardViewModel();
            //model.Table = dt;
            return Json((DashboardViewModel.ToDataSourceResult(request)), JsonRequestBehavior.AllowGet);

        }



        [HttpPost]
        //public ActionResult GetClientPerformanceDashboard([DataSourceRequest]DataSourceRequest request)

        public ActionResult GetClientPerformanceComplianceReport([DataSourceRequest]DataSourceRequest request, string Quarter, DateTime yearDatetime )
        {

            int year = yearDatetime.Year;
          
            int quarter = Convert.ToInt16(Quarter);
           
            //string ClientIdlist = string.Join(",", v);
            //var client = (Client)Session["Client"];
            //int ClientId = client.Id;

            List<PerformanceComplianceModel> DashboardViewModel = ClientProfileManagerHelper.CPMPerformanceComplianceReport((int)Session["UserId"], year, quarter).ToList();
            //var model = new DashboardViewModel();
            //model.Table = dt;
            return Json((DashboardViewModel.ToDataSourceResult(request)), JsonRequestBehavior.AllowGet);

        }

    }
}
