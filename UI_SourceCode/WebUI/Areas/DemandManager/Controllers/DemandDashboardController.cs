using Data;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.DemandManager.Dto;
using WebUI.Areas.DemandManager.Object;
using Kendo.Mvc.Export;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using WebUI.Dto;
using WebUI.Areas.DemandManager.Models;
using System.Web.Configuration;
using log4net;
using WebUI.Areas.IncidentNavigator;


namespace WebUI.Areas.DemandManager.Controllers
{

    public class DemandDashboardController : Controller
    {
        //
        // GET: /PortfolioAnalyzer/Dashboard/
        private static readonly ILog Log = LogManager.GetLogger(typeof(DemandDashboardController));

        [ActionTrack]
        public ActionResult Index(DashboardDemandModel c)
        {
            //var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            //int ClientId = client.Id;
            int ClientId = client.ClientId;
            //DashboardModel c = new DashboardModel();
            c.DemandPosted = DemandsPostedDashboard(ClientId).ToList();
            c.DemandTypes = DemandTypeDashboard(ClientId).ToList();
            c.DemandSource = DemandSourcesDashboard(ClientId).ToList();
            c.DemandStatus = DemandStatusDashboard(ClientId).ToList();
            return View(c);
            // return View();
        }


        [HttpPost]
        public ActionResult GetDashboardFilterOptions(int filterOption)
        {


            //var client = (Client)Session["Client"];
            //int ClientId = client.Id;
            var client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            List<DashboardDemandModel> model = null;
            switch (filterOption)
            {
                case 1:

                    model = DemandsPostedDashboard(ClientId).ToList();

                    break;

                case 2:

                    model = DemandSourcesDashboard(ClientId).ToList();

                    break;

                case 3:

                    model = DemandTypeDashboard(ClientId).ToList();

                    break;

                case 4:

                    model = ChallengePostedDashboard(ClientId).ToList();

                    break;


                case 5:

                    model = ChallengeDemandRelationDashboard(ClientId).ToList();

                    break;
                case 6:

                    model = DemandStatusDashboard(ClientId).ToList();

                    break;

                default:
                    model = null;
                    break;

            }
            return Json(model);

        }

        //helper
        public static IList<DashboardDemandModel> DemandsPostedDashboard(int ClientId)
        {
            IList<DashboardDemandModel> Modellist = new List<DashboardDemandModel>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("DMDemandsPostedDashboard", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DashboardDemandModel();

                    coldet.Category = dr["Category"].ToString();

                    try
                    {
                        coldet.Count = Convert.ToInt32(dr["tCount"]);
                    }
                    catch { }
                    Modellist.Add(coldet);
                }
            }
            return Modellist;

        }

        public static IList<DashboardDemandModel> DemandSourcesDashboard(int ClientId)
        {
            IList<DashboardDemandModel> Modellist = new List<DashboardDemandModel>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("DMDemandSourcesDashboard", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DashboardDemandModel();

                    coldet.Category = dr["Category"].ToString();
                    try
                    {
                        coldet.Count = Convert.ToInt32(dr["tCount"]);
                    }
                    catch { }
                    Modellist.Add(coldet);
                }
            }
            return Modellist;
        }

        public static IList<DashboardDemandModel> DemandTypeDashboard(int ClientId)
        {
            IList<DashboardDemandModel> Modellist = new List<DashboardDemandModel>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("DMDemandTypeDashboard", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DashboardDemandModel();

                    coldet.Category = dr["Category"].ToString();
                    try
                    {
                        coldet.Count = Convert.ToInt32(dr["tCount"]);
                    }
                    catch { }
                    Modellist.Add(coldet);
                }
            }
            return Modellist;
        }

        public static IList<DashboardDemandModel> ChallengePostedDashboard(int ClientId)
        {
            IList<DashboardDemandModel> Modellist = new List<DashboardDemandModel>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("DMChallengePostedDashboard", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DashboardDemandModel();

                    coldet.Category = dr["Category"].ToString();
                    try
                    {
                        coldet.Count = Convert.ToInt32(dr["tCount"]);
                    }
                    catch { }
                    Modellist.Add(coldet);
                }
            }
            return Modellist;
        }

        public static IList<DashboardDemandModel> DemandStatusDashboard(int ClientId)
        {
            //dMDemandStatusDashboard
            IList<DashboardDemandModel> Modellist = new List<DashboardDemandModel>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("DMDemandStatusDashboard", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DashboardDemandModel();

                    coldet.Category = dr["Category"].ToString();
                    try
                    {
                        coldet.Count = Convert.ToInt32(dr["tCount"]);
                    }
                    catch { }
                    Modellist.Add(coldet);
                }
            }
            return Modellist;
        }

        public static IList<DashboardDemandModel> ChallengeDemandRelationDashboard(int ClientId)
        {
            IList<DashboardDemandModel> Modellist = new List<DashboardDemandModel>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("DMChallengeDemandRelationDashboard", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DashboardDemandModel();

                    coldet.Category = dr["Category"].ToString();
                    try
                    {
                        coldet.Count = Convert.ToInt32(dr["tCount"]);
                    }
                    catch { }
                    Modellist.Add(coldet);
                }
            }
            return Modellist;
        }

        //test
        public static IList<DashboardDemandModel> sampletest()
        {
            var coldet = new DashboardDemandModel();
            IList<DashboardDemandModel> Modellist = new List<DashboardDemandModel>();
            for (int i = 1; i < 4; i++)
            {
                coldet.Category = "Cat" + i.ToString();
                coldet.Count = i * 10;
                Modellist.Add(coldet);
            }
            return Modellist;
        }

        public ActionResult AllDemands()
        {
            //DashboardModel c = new DashboardModel();
            return View("AllDemands");
            // return View();
        }

        public ActionResult ReadAllDemands([DataSourceRequest] DataSourceRequest request)
        {
            //var client = (Client)Session["Client"];
            //int ClientId = client.Id;
            var client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            return Json(DemandManagerHelper.New_GetDemandList(ClientId).ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult Export_Demands_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }


        public ActionResult ConsolidatedAnalysis()
        {
            DMReportParamModel dm = prepareCommonParameters();

            ClientDetail client = (ClientDetail)Session["SessionClient"];
            IEnumerable<string> userEmailList = IncidentNavigatorHelper.getEmailUserPersonnel(client.ClientId);

            ViewBag.userEmailList = userEmailList;
            return View("ConsolidatedAnalysis", dm);

        }


        private DMReportParamModel prepareCommonParameters()
        {
            //  Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            DMReportParamModel mod = new DMReportParamModel();
            mod.ClientName = client.ClientName;
            mod.UserName = DemandManager.DMHelper.GetUserName(uid);
            mod.ClientId = client.ClientId;
            mod.UserId = uid;
            mod.ToYear = DateTime.Today.Date;
            mod.FromYear = DateTime.Today.Date.AddMonths(-6); // cuurentdate - 6months
            mod.DateRange = mod.FromYear.Value.ToString("MM/dd/yyyy") + " To " + mod.ToYear.Value.ToString("MM/dd/yyyy");
         //   mod.ClusterPath = WebConfigurationManager.AppSettings["ClusterPath"];
            mod.CreatedDt = DateTime.Now;
            GetFilterInfo(mod, client.ClientId);


            return mod;
        }

        public static void GetFilterInfo(DMReportParamModel mod, int ClientId)
        {
            //IList<TrendingReportInput> Tredingdata = new List<TrendingReportInput>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                DataSet ds = new DataSet();


                ds = unitOfWork.GetDataFromStoredProcedureDS("FetchDMFilterInfo", paramName, paramValue);

                List<SelectListItem> ls;

                for (int t = 0; t < ds.Tables.Count; t++)
                {
                    int i = 0;
                    ls = new List<SelectListItem>();
                    foreach (DataRow row in ds.Tables[t].Rows)
                    {
                        ls.Add(new SelectListItem() { Text = row.ItemArray[0].ToString(), Value = i.ToString() });
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
                            mod.LOBList = ls;
                            break;
                    }

                }

                return;
            }

        }


        public ActionResult New_Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetDashboardTrend(string ChartType, DateTime StartDate, DateTime EndDate, [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                int ClientId = client.ClientId;
                List<DMChartDashboardModel> model = DemandManagerHelper.DMDashboardTrendChartData(ClientId, ChartType, StartDate, EndDate).ToList();
                var calpercent = DemandManagerHelper.calculatePercent(model);
                var calTrend = DemandManagerHelper.calculateTrend(model);
                return Json(calpercent, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Error on Trend chart load:-", ex);
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult GetDashboardTrendGrid(string ChartType, DateTime StartDate, DateTime EndDate, [DataSourceRequest] DataSourceRequest request)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            List<DMChartDashboardModel> model = DemandManagerHelper.DMDashboardTrendChartData(ClientId, ChartType, StartDate, EndDate).ToList();
            var calpercent = DemandManagerHelper.calculatePercent(model);

            return Json(calpercent.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetDashboardTower(string drpval, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                int ClientId = client.ClientId;
                List<DMChartDashboardModel> model = DemandManagerHelper.DMDashboardChartData(drpval, ClientId, StartDate, EndDate).ToList();
                var calpercent = DemandManagerHelper.calculatePercent(model);
                return Json(calpercent, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Error on Category chart load:-", ex);
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult GetDMDashboardDataDynamicGrid(string drpval, DateTime StartDate, DateTime EndDate, [DataSourceRequest] DataSourceRequest request)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            List<DMChartDashboardModel> model = DemandManagerHelper.DMDashboardChartData(drpval, ClientId, StartDate, EndDate).ToList();
            var calpercent = DemandManagerHelper.calculatePercent(model);
            return Json(calpercent.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetDashboard2Trend(string drpval, string dpnVal, DateTime StartDate, DateTime EndDate)
        {
            try
            {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                int ClientId = client.ClientId;
                List<DMChartDashboardModel> model = DemandManagerHelper.DMDashboard2TrendChartData(ClientId, drpval, dpnVal, StartDate, EndDate).ToList();
                var calpercent = DemandManagerHelper.calculatePercent(model);
                return Json(calpercent, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Error on Month/Year chart load:-", ex);
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult GetDashboard2TrendGrid(string drpval, string dpnVal, DateTime StartDate, DateTime EndDate, [DataSourceRequest] DataSourceRequest request)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            List<DMChartDashboardModel> model = DemandManagerHelper.DMDashboard2TrendChartData(ClientId, drpval, dpnVal, StartDate, EndDate).ToList();
            var calpercent = DemandManagerHelper.calculatePercent(model);

            return Json(calpercent.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetDMDashboardDataDrilldownGrid(string drpval, DateTime StartDate, DateTime EndDate, string Category, [DataSourceRequest] DataSourceRequest request)
        {
            Log.Debug("Trend chart selected Category " + Category);

            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            List<DemandDashboardModel> model = DemandManagerHelper.DMDashboardDrilldownData(drpval, ClientId, StartDate, EndDate, Category).ToList();
            var jsonResult = Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public ActionResult GetDashboardDrilldownGrid(string drpval, string dpnVal, DateTime StartDate, DateTime EndDate, string Category, string Categoryseries, [DataSourceRequest] DataSourceRequest request)
        {
            Log.Debug("Category chart selected Category " + Category);
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            List<DemandDashboardModel> model = DemandManagerHelper.DMDashboard2DrilldownData(ClientId, drpval, dpnVal, StartDate, EndDate, Category, Categoryseries).ToList();
            var jsonResult = Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public ActionResult GetDemandsdrilldown(string ChartType, DateTime StartDate, DateTime EndDate, string Category, string InflowOutFlowType, [DataSourceRequest] DataSourceRequest request)
        {
            Log.Debug("Month/Year chart selected Category " + Category);
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            List<DemandDashboardModel> model = DemandManagerHelper.DMTrendDrillDownData(ClientId, ChartType, StartDate, EndDate, Category, InflowOutFlowType).ToList();
            var jsonResult = Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }



    }



}
