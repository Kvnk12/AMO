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
using WebUI.Dto;
using System.Data;
using log4net;

namespace WebUI.Areas.IncidentNavigator.Controllers
{
    public class DashboardController : Controller
    {
        //
        // GET: /IncidentNavigator/Dashboard/
        private static readonly ILog Log = LogManager.GetLogger(typeof(ConfigureReportController));
        public ActionResult Index()
        {
            try
            {
                INNewDashboardModel c = prepareCommonParameters();
                return View(c);
            }
            catch (Exception ex)
            {
                Log.Error("Dashboard controller Index  Action error 'Index()' ", ex);
                throw ex;
            }
        }
        private INNewDashboardModel prepareCommonParameters()
        {
            //  Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            INNewDashboardModel mod = new INNewDashboardModel();
     
            GetFilterInfo(mod, client.ClientId);


            return mod;
        }
        public static void GetFilterInfo(INNewDashboardModel mod, int ClientId)
        {
            try
            {
                //IList<TrendingReportInput> Tredingdata = new List<TrendingReportInput>();
                var paramName = new string[] { "ClientId" };
                var paramValue = new object[] { ClientId };
                DataTable dt = new DataTable();

                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    DataSet ds = new DataSet();


                    ds = unitOfWork.GetDataFromStoredProcedureDS("FetchAllFilterforDashboardInfo_Temp", paramName, paramValue);

                    List<SelectListItem> ls;

                    for (int t = 0; t < ds.Tables.Count; t++)
                    {
                        int i = 0;
                        ls = new List<SelectListItem>();
                        foreach (DataRow row in ds.Tables[t].Rows)
                        {
                            if (t == 8 || t == 6 || t == 7)
                            {
                                ls.Add(new SelectListItem() { Text = row.ItemArray[0].ToString(), Value = row.ItemArray[1].ToString() });
                            }
                            else
                            {
                                ls.Add(new SelectListItem() { Text = row.ItemArray[0].ToString(), Value = i.ToString() });
                            }
                            i++;
                        }
                        switch (t)
                        {
                            case 0:
                                mod.TypeList = ls;
                                break;
                            case 1:
                                mod.StatusList = ls;
                                break;
                            case 2:
                                mod.PriorityList = ls;
                                break;
                            case 3:
                                // mod.GroupList = ls;
                                break;
                            case 4:
                                mod.TowerList = ls;
                                break;
                            case 5:
                                //  mod.DateColumnsList = ls;
                                break;
                            case 6:
                                // mod.Maintnenance_TypeList = ls;
                                mod.AccountNamesList = ls;
                                var tempacc = ls.FirstOrDefault();
                                if (tempacc != null)
                                {
                                    var temp = Convert.ToInt32(tempacc.Value);
                                    Db db = new Db();
                                    var tempcon = (from acc in db.Incidents
                                                   join con in db.ClientAccountContract on acc.ContractId equals con.Id
                                                   where con.AccountId == temp && acc.ClientId == ClientId
                                                   select new
                                                   {
                                                       Id = con.Id,
                                                       ContractName = con.ContractName
                                                   }).ToList().Distinct().FirstOrDefault();
                                    //if (tempcon != null)
                                    //{
                                    //    mod.Accid = tempcon.Id;
                                    //}
                                }
                                break;
                            case 7:
                                // mod.Maintnenance_StatusList = ls;
                                mod.AccContractList = ls;
                                break;
                            case 8:
                                // mod.Maintnenance_PriorityList = ls;
                                mod.ContractList = ls;
                                break;

                            case 9:
                                // mod.WaivedFilterList = ls;
                                break;
                        }

                    }
                    // mod.INMappedColumn= IncidentNavigatorHelper.getMappedColumnValuesList(ClientId).ToList().Select(f => new SelectListItem { Value = f.INFieldName, Text = f.INFieldName });
                    return;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Dashboard controller GetFilterInfo Action error 'GetFilterInfo()' ", ex);
                throw ex;
            }
        }


        //[HttpPost]
        public ActionResult Pdf_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }


        public ActionResult OldIndex()
        {
            DashboardTrendModel c = new DashboardTrendModel();
            return View("OldIndex", c);
        }

        [HttpPost]
        public ActionResult GetDashboardTower(string drpval, string TicketType, string Status, string Track, string Priority, string Datetype, DateTime StartDate, DateTime EndDate, string contract)
        {
            //var v = JsonConvert.DeserializeObject<IList<string>>(HttpUtility.UrlDecode(TicketType));
            //string ticketType = string.Join(",", v);
            //var u = JsonConvert.DeserializeObject<IList<string>>(HttpUtility.UrlDecode(Status));
            //string status = string.Join(",", u);
            //var w = JsonConvert.DeserializeObject<IList<string>>(HttpUtility.UrlDecode(Track));
            //string track = string.Join(",", w);
            //var x = JsonConvert.DeserializeObject<IList<string>>(HttpUtility.UrlDecode(Priority));
            //string priority = string.Join(",", x);
            //// var client = (Client)Session["Client"];
            try
            {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                int ClientId = client.ClientId;
                List<INChartDashboardModel> model = IncidentNavigatorHelper.INDashboardChartData(drpval, ClientId, TicketType, Status, Track, Priority, Datetype, StartDate, EndDate, contract).ToList();
                var calpercent = IncidentNavigatorHelper.calculatePercent(model);
                return Json(calpercent, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Dashboard controller Action error 'GetDashboardTower()' ", ex);
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult GetINDashboardDataDynamicGrid(string drpval, string TicketType, string Status, string Track, string Priority, string Datetype, DateTime StartDate, DateTime EndDate, string contract, [DataSourceRequest] DataSourceRequest request)
        {
            //var v = JsonConvert.DeserializeObject<IList<string>>(HttpUtility.UrlDecode(TicketType));
            //string ticketType = string.Join(",", v);
            //var u = JsonConvert.DeserializeObject<IList<string>>(HttpUtility.UrlDecode(Status));
            //string status = string.Join(",", u);
            //var w = JsonConvert.DeserializeObject<IList<string>>(HttpUtility.UrlDecode(Track));
            //string track = string.Join(",", w);
            //var x = JsonConvert.DeserializeObject<IList<string>>(HttpUtility.UrlDecode(Priority));
            //string priority = string.Join(",", x);
            // var client = (Client)Session["Client"];
            try
            {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                int ClientId = client.ClientId;
                List<INChartDashboardModel> model = IncidentNavigatorHelper.INDashboardChartData(drpval, ClientId, TicketType, Status, Track, Priority, Datetype, StartDate, EndDate, contract).ToList();
                var calpercent = IncidentNavigatorHelper.calculatePercent(model);
                return Json(calpercent.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Dashboard controller GetINDashboardDataDynamicGrid Action error 'GetINDashboardDataDynamicGrid()' ", ex);
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult GetDashboardTrend(string ChartType, string Datetype, string TicketType, string Status, string Track, string Priority, DateTime StartDate, DateTime EndDate,string contract, [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                int ClientId = client.ClientId;
                List<INChartDashboardModel> model = IncidentNavigatorHelper.INDashboardTrendChartData(ClientId, ChartType, Datetype, StartDate, EndDate, TicketType, Status, Track, Priority, contract).ToList();
                var calpercent = IncidentNavigatorHelper.calculatePercent(model);
                var calTrend = IncidentNavigatorHelper.calculateTrend(model);
                return Json(calpercent, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Dashboard controller GetDashboardTrend Action error 'GetDashboardTrend()' ", ex);
                throw ex;
            }
        }
        
        [HttpPost]
        public ActionResult GetDashboardTrendGrid(string ChartType, string Datetype,  string TicketType, string Status, string Track, string Priority, DateTime StartDate, DateTime EndDate, string contract, [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                int ClientId = client.ClientId;
                List<INChartDashboardModel> model = IncidentNavigatorHelper.INDashboardTrendChartData(ClientId, ChartType, Datetype, StartDate, EndDate, TicketType, Status, Track, Priority, contract).ToList();
                var calpercent = IncidentNavigatorHelper.calculatePercent(model);

                return Json(calpercent.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Dashboard controller GetDashboardTrendGrid Action error 'GetDashboardTrendGrid()' ", ex);
                throw ex;
            }
        }

        [HttpPost]
       public ActionResult GetDashboard2Trend(string drpval, string TicketType, string Status, string Track, string Priority, string dpnVal, string Datetype, DateTime StartDate, DateTime EndDate, string contract)
        {
            try
            {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                int ClientId = client.ClientId;
                List<INChartDashboardModel> model = IncidentNavigatorHelper.INDashboard2TrendChartData(ClientId, drpval, TicketType, Status, Track, Priority, dpnVal, Datetype, StartDate, EndDate, contract).ToList();
                var calpercent = IncidentNavigatorHelper.calculatePercent(model);


                return Json(calpercent, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Dashboard controller GetDashboard2Trend Action error 'GetDashboard2Trend()' ", ex);
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult GetDashboard2TrendGrid(string drpval, string TicketType, string Status, string Track, string Priority, string dpnVal, string Datetype, DateTime StartDate, DateTime EndDate, string contract, [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                int ClientId = client.ClientId;
                List<INChartDashboardModel> model = IncidentNavigatorHelper.INDashboard2TrendChartData(ClientId, drpval, TicketType, Status, Track, Priority, dpnVal, Datetype, StartDate, EndDate, contract).ToList();
                var calpercent = IncidentNavigatorHelper.calculatePercent(model);

                return Json(calpercent.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Dashboard controller GetDashboard2TrendGrid Action error 'GetDashboard2TrendGrid()' ", ex);
                throw ex;
            }
        }
        public ActionResult getContractId(int Accid=0)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            Db db = new Db();
            var dispSln = (from acc in db.Incidents
                           join con in db.ClientAccountContract on acc.ContractId equals con.Id
                           where con.AccountId == Accid && acc.ClientId == ClientId
                           select new
                           {
                               Id = con.Id,
                               ContractName = con.ContractName
                           }).ToList().Distinct();
            return Json(dispSln.ToList().Select(A => new { Value = A.Id.ToString(), Text = A.ContractName , Selected = true }), JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult GetTicketsdrilldown(string ChartType, string Datetype, string TicketType, string Status, string Track, string Priority, DateTime StartDate, DateTime EndDate, string contract, string Category, string InflowOutFlowType, [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                int ClientId = client.ClientId;
                List<SLAMaintenenceModel> model = IncidentNavigatorHelper.INTrendDrillDownData(ClientId, ChartType, Datetype, StartDate, EndDate, TicketType, Status, Track, Priority, contract, Category, InflowOutFlowType).ToList();
                var jsonResult = Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                Log.Error("Dashboard controller GetTicketsdrilldown Action error 'GetTicketsdrilldown()' ", ex);
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult GetINDashboardDataDrilldownGrid(string drpval, string TicketType, string Status, string Track, string Priority, string Datetype, DateTime StartDate, DateTime EndDate, string contract, string Category, [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                int ClientId = client.ClientId;
                List<SLAMaintenenceModel> model = IncidentNavigatorHelper.INDashboardDrilldownData(drpval, ClientId, TicketType, Status, Track, Priority, Datetype, StartDate, EndDate, contract, Category).ToList();
                var jsonResult = Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                Log.Error("Dashboard controller GetINDashboardDataDrilldownGrid Action error 'GetINDashboardDataDrilldownGrid()' ", ex);
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult GetDashboardDrilldownGrid(string drpval, string TicketType, string Status, string Track, string Priority, string dpnVal, string Datetype, DateTime StartDate, DateTime EndDate, string contract, string Category, string Categoryseries,[DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                int ClientId = client.ClientId;
                List<SLAMaintenenceModel> model = IncidentNavigatorHelper.INDashboard2DrilldownData(ClientId, drpval, TicketType, Status, Track, Priority, dpnVal, Datetype, StartDate, EndDate, contract, Category, Categoryseries).ToList();
                var jsonResult = Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                Log.Error("Dashboard Index GetDashboardDrilldownGrid error 'GetDashboardDrilldownGrid()' ", ex);
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Export_Tickets_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);


            return File(fileContents, contentType, fileName);
        }

        
    }
}
