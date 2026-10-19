using Data;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.ClientProfileManager.ViewModel;
using WebUI.Dto;

namespace WebUI.Areas.ClientProfileManager.Controllers
{
    public class DashboardController : Controller
    {
        //


        //public ActionResult NextGenUserReport()
        //{
        //    return View("NextGenUsers");

        //}

        // GET: /ClientProfileManager/Dashboard/
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        public ActionResult Index()
        {
            return View();
        }
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        public ActionResult AllClientsForUser()
        {
            int uid = (int)Session["UserId"];
            var li = ClientSummary(uid);

            return View("ClientSummary", li);
        }

        public static IList<ClientSummaryModel> ClientSummary(int uid)
        {
            IList<ClientSummaryModel> Modellist = new List<ClientSummaryModel>();
            var paramName = new string[] { "uid" };
            var paramValue = new object[] { uid };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("CPMClientSummary1", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new ClientSummaryModel();
                    try
                    {
                        coldet.ClientId = Convert.ToInt32(dr["ClientId"]);
                    }
                    catch { }
                    coldet.ClientName = (dr["cName"]).ToString();
                    try
                    {
                        coldet.TotalPortfolios = Convert.ToInt32(dr["TotalPortfolios"]);
                    }
                    catch { }
                    try
                    {
                        coldet.TotalIncidents = Convert.ToInt32(dr["TotalIncidents"]);
                    }
                    catch { }
                    try
                    {
                        coldet.TotalApplications = Convert.ToInt32(dr["TotalApplications"]);
                    }
                    catch { }
                    //try
                    //{
                    //    coldet.TotalAccounts = Convert.ToInt32(dr["TotalAccounts"]);
                    //}
                    //catch { }
                    try
                    {
                        coldet.TotalMetric = Convert.ToInt32(dr["TotalMetric"]);
                    }
                    catch { }
                    try
                    {
                        coldet.TotalServiceRequest = Convert.ToInt32(dr["TotalServiceRequest"]);
                    }
                    catch { }
                    try
                    {
                        coldet.TotalTicketsClosed_Resolved = Convert.ToInt32(dr["TotalTicketsClosed_Resolved"]);
                    }
                    catch { }
                    try
                    {
                        coldet.TotalTicketsRejected = Convert.ToInt32(dr["TotalTicketsRejected"]);
                    }
                    catch { }
                    try
                    {
                        coldet.TotalTicketSuspended = Convert.ToInt32(dr["TotalTicketSuspended"]);
                    }
                    catch { }
                    try
                    {
                        //coldet.AvgEffortPerTicket = Convert.ToDecimal(dr["AvgEffortPerTicket"]);
                        coldet.AvgEffortPerTicket = dr["AvgEffortPerTicket"].ToString();
                    }
                    catch { }
                    try
                    {
                        coldet.AvgCostPerTicket = Convert.ToDecimal(dr["AvgCostPerTicket"]);
                    }
                    catch { }
                    try
                    {
                        coldet.TotalSev1 = Convert.ToInt32(dr["TotalSev1"]);
                    }
                    catch { }
                    try
                    {
                        coldet.TotalSev2 = Convert.ToInt32(dr["TotalSev2"]);
                    }
                    catch { }
                    try
                    {
                        coldet.TotalSev3 = Convert.ToInt32(dr["TotalSev3"]);
                    }
                    catch { }
                    try
                    {
                        coldet.TotalSev4 = Convert.ToInt32(dr["TotalSev4"]);
                    }
                    catch { }
                    Modellist.Add(coldet);
                }
            }
            return Modellist;
        }

        public ActionResult authLog()
        {
            return View("MFALogDetails");
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        [HttpPost]
        public ActionResult GetUserAuthLogDetails([DataSourceRequest] DataSourceRequest request)
        {
            //var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            return Json(ClientProfileManagerHelper.getauthLogs(client.ClientId).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
    }
}

