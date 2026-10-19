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
using WebUI.Areas.IdeaManager.Dto;
using WebUI.Areas.IdeaManager.Objects;
using Kendo.Mvc.Export;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using WebUI.Dto;
using WebUI.Areas.IdeaManager.Models;
using System.Web.Configuration;
using log4net;
using WebUI.Areas.IncidentNavigator;


namespace WebUI.Areas.IdeaManager.Controllers
{

    public class DashboardController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DashboardController));

        // GET: /PortfolioAnalyzer/Dashboard/

        //public ActionResult Index()
        //{
        //    DashboardModel c = new DashboardModel();
        //    return View(c);
        //    // return View();
        //}
        [ActionTrack]
        public ActionResult Index(DashboardModel c)
        {
            //var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            //int ClientId = client.Id;
            int ClientId = client.ClientId;
            var UserId = (int)Session["UserId"];
            //DashboardModel c = new DashboardModel();
            c.IdeaPosted = IdeasPostedDashboard(ClientId).ToList();
            c.IdeaTypes = IdeaTypeDashboard(ClientId).ToList();
            c.IdeaSource = IdeaSourcesDashboard(ClientId).ToList();
            c.IdeaStatus = IdeaStatusDashboard(ClientId).ToList();
            ViewData["GetStringResult"] = getFullName(UserId);
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
            List<DashboardModel> model = null;
            switch (filterOption)
            {
                case 1:

                    model = IdeasPostedDashboard(ClientId).ToList();

                    break;

                case 2:

                    model = IdeaSourcesDashboard(ClientId).ToList();

                    break;

                case 3:

                    model = IdeaTypeDashboard(ClientId).ToList();

                    break;

                case 4:

                    model = ChallengePostedDashboard(ClientId).ToList();

                    break;


                case 5:

                    model = ChallengeIdeaRelationDashboard(ClientId).ToList();

                    break;
                case 6:

                    model = IdeaStatusDashboard(ClientId).ToList();

                    break;

                default:
                    model = null;
                    break;



            }
            return Json(model);

        }

        //helper
        public static IList<DashboardModel> IdeasPostedDashboard(int ClientId)
        {
            IList<DashboardModel> Modellist = new List<DashboardModel>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("IMIdeasPostedDashboard", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DashboardModel();

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

        public static IList<DashboardModel> IdeaSourcesDashboard(int ClientId)
        {
            IList<DashboardModel> Modellist = new List<DashboardModel>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("IMIdeaSourcesDashboard", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DashboardModel();

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

        public static IList<DashboardModel> IdeaTypeDashboard(int ClientId)
        {
            IList<DashboardModel> Modellist = new List<DashboardModel>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("IMIdeaTypeDashboard", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DashboardModel();

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

        public static IList<DashboardModel> ChallengePostedDashboard(int ClientId)
        {
            IList<DashboardModel> Modellist = new List<DashboardModel>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("IMChallengePostedDashboard", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DashboardModel();

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

        public static IList<DashboardModel> IdeaStatusDashboard(int ClientId)
        {
            //IMIdeaStatusDashboard
            IList<DashboardModel> Modellist = new List<DashboardModel>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("IMIdeaStatusDashboard", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DashboardModel();

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

        public static IList<DashboardModel> ChallengeIdeaRelationDashboard(int ClientId)
        {
            IList<DashboardModel> Modellist = new List<DashboardModel>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("IMChallengeIdeaRelationDashboard", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DashboardModel();

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
        public static IList<DashboardModel> sampletest()
        {
            var coldet = new DashboardModel();
            IList<DashboardModel> Modellist = new List<DashboardModel>();
            for (int i = 1; i < 4; i++)
            {
                coldet.Category = "Cat" + i.ToString();
                coldet.Count = i * 10;
                Modellist.Add(coldet);
            }
            return Modellist;
        }
        [ActionTrack]
        public ActionResult AllIdeas()
        {
            //DashboardModel c = new DashboardModel();
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);
            return View("AllIdeas");
            // return View();
        }

        public ActionResult ReadAllIdeas([DataSourceRequest] DataSourceRequest request)
        {
            //var client = (Client)Session["Client"];
            //int ClientId = client.Id;
            var client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            return Json(IdeaManagerHelper.New_GetIdeaList(ClientId).ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult Export_Ideas_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        [ActionTrack]
        public ActionResult ConsolidatedAnalysis()
        {
            IMReportParamModel im = prepareCommonParameters();
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);

            ClientDetail client = (ClientDetail)Session["SessionClient"];
            IEnumerable<string> userEmailList = IncidentNavigatorHelper.getEmailUserPersonnel(client.ClientId);
            
            ViewBag.userEmailList = userEmailList;
            return View("ConsolidatedAnalysis", im);

        }
        [ActionTrack]
        public ActionResult ColumnChartPOC()
        {
            IMReportParamModel im = prepareCommonParameters();
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);
            return View("ColumnChartPOC", im);
        }


        private IMReportParamModel prepareCommonParameters()
        {
            //  Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            IMReportParamModel mod = new IMReportParamModel();
            mod.ClientName = client.ClientName;
            mod.IdeaSource = "NTT Data";
            mod.UserName = IdeaManager.IMHelper.GetUserName(uid);
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

        public static void GetFilterInfo(IMReportParamModel mod, int ClientId)
        {
            //IList<TrendingReportInput> Tredingdata = new List<TrendingReportInput>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                DataSet ds = new DataSet();


                ds = unitOfWork.GetDataFromStoredProcedureDS("FetchIMFilterInfo", paramName, paramValue);

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
                            mod.TrackList = ls;
                            break;
                       

                    }

                }

                return;
            }

        }


        public string getFullName(int userId)
        {
            Db db = new Db();
            var fullName = (from b in db.UserProfiles
                            where b.Id == userId
                            select b.FirstName + " " + b.LastName).FirstOrDefault();

            return fullName;
        }
        [ActionTrack]
        public ActionResult New_Index()
        {
            List<IMChartDashboardModel> yearByVal = new List<IMChartDashboardModel>();
            ViewData["YearByValue"] = yearByVal;
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);
            return View();
        }
        [HttpPost]
        public ActionResult GetDashboardTrend(string ChartType, DateTime StartDate, DateTime EndDate,string IdeaSource ,[DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                int ClientId = client.ClientId;
                List<IMChartDashboardModel> model = IdeaManagerHelper.IMDashboardTrendChartData(ClientId, ChartType, StartDate, EndDate, IdeaSource).ToList();
                var calpercent = IdeaManagerHelper.calculatePercent(model);
                var calTrend = IdeaManagerHelper.calculateTrend(model);
                return Json(calpercent, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Error on Trend chart load:-", ex);
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult GetDashboardTrendGrid(string ChartType, DateTime StartDate, DateTime EndDate, string IdeaSource, [DataSourceRequest] DataSourceRequest request)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            List<IMChartDashboardModel> model = IdeaManagerHelper.IMDashboardTrendChartData(ClientId, ChartType, StartDate, EndDate, IdeaSource).ToList();
            var calpercent = IdeaManagerHelper.calculatePercent(model);

            return Json(calpercent.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetDashboardTower(string drpval,DateTime StartDate, DateTime EndDate, string IdeaSource)
        {
            try
            {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                int ClientId = client.ClientId;
                List<IMChartDashboardModel> model = IdeaManagerHelper.IMDashboardChartData(drpval, ClientId, StartDate, EndDate, IdeaSource).ToList();
                var calpercent = IdeaManagerHelper.calculatePercent(model);
                return Json(calpercent, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Error on Category chart load:-", ex);
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult GetIMDashboardDataDynamicGrid(string drpval,DateTime StartDate, DateTime EndDate, string IdeaSource, [DataSourceRequest] DataSourceRequest request)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            List<IMChartDashboardModel> model = IdeaManagerHelper.IMDashboardChartData(drpval, ClientId, StartDate, EndDate, IdeaSource).ToList();
            var calpercent = IdeaManagerHelper.calculatePercent(model);
            return Json(calpercent.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetDashboard2Trend(string drpval, string dpnVal, DateTime StartDate, DateTime EndDate, string IdeaSource)
        {
            try
            {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                int ClientId = client.ClientId;
                List<IMChartDashboardModel> model = IdeaManagerHelper.IMDashboard2TrendChartData(ClientId, drpval,dpnVal, StartDate, EndDate, IdeaSource).ToList();
                var calpercent = IdeaManagerHelper.calculatePercent(model);
                return Json(calpercent, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Error on Month/Year chart load:-", ex);
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult GetDashboard2TrendvalGrid(string drpval, string dpnVal, DateTime StartDate, DateTime EndDate, string IdeaSource, [DataSourceRequest] DataSourceRequest request)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            List<IMChartDashboardModel> model = IdeaManagerHelper.IMDashboard2TrendChartData(ClientId, drpval,dpnVal, StartDate, EndDate, IdeaSource).ToList();
            if (drpval == "Year")
            {
                List<IMChartDashboardModel> yearByVal = new List<IMChartDashboardModel>();
                foreach(var item in model)
                {
                    IMChartDashboardModel listval = new IMChartDashboardModel();
                    listval.Dim = item.Dim;
                    listval.cumm_value = item.cumm_value;
                    yearByVal.Add(listval);
                }
                ViewData["YearByValue"] = yearByVal;
            }
            var calpercent = IdeaManagerHelper.calculatePercent(model);

            return Json(calpercent.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetDashboard2TrendGrid(string drpval, string dpnVal, DateTime StartDate, DateTime EndDate, string IdeaSource, [DataSourceRequest] DataSourceRequest request)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            List<IMChartDashboardModel> model = IdeaManagerHelper.IMDashboard2TrendChartData(ClientId, drpval,dpnVal, StartDate, EndDate, IdeaSource).ToList();
            var calpercent = IdeaManagerHelper.calculatePercent(model);
            if (drpval == "Year")
            {
                List<IMChartDashboardModel> yearByVal = new List<IMChartDashboardModel>();
                foreach (var item in model)
                {
                    IMChartDashboardModel listval = new IMChartDashboardModel();
                    listval.Dim = item.Dim;
                    listval.cumm_value = item.cumm_value;
                    yearByVal.Add(listval);
                }
                ViewData["YearByValue"] = yearByVal;
            }
            return Json(calpercent.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetIMDashboardDataDrilldownGrid(string drpval, DateTime StartDate, DateTime EndDate, string Category, string IdeaSource, [DataSourceRequest] DataSourceRequest request)
        {
            Log.Debug("Trend chart selected Category " + Category);

            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            List<IdeaDashboardModel> model = IdeaManagerHelper.IMDashboardDrilldownData(drpval, ClientId, StartDate, EndDate, Category, IdeaSource).ToList();
            var jsonResult = Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public ActionResult GetDashboardDrilldownGrid(string drpval, string dpnVal, DateTime StartDate, DateTime EndDate, string Category, string Categoryseries, string IdeaSource, [DataSourceRequest] DataSourceRequest request)
        {
            Log.Debug("Category chart selected Category " + Category);
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            List<IdeaDashboardModel> model = IdeaManagerHelper.IMDashboard2DrilldownData(ClientId, drpval, dpnVal, StartDate, EndDate, Category, Categoryseries, IdeaSource).ToList();
            var jsonResult = Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public ActionResult GetIdeasdrilldown(string ChartType, DateTime StartDate, DateTime EndDate, string Category, string InflowOutFlowType, string IdeaSource, [DataSourceRequest] DataSourceRequest request)
        {
            Log.Debug("Month/Year chart selected Category " + Category);
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            List<IdeaDashboardModel> model = IdeaManagerHelper.IMTrendDrillDownData(ClientId, ChartType, StartDate, EndDate, Category, InflowOutFlowType, IdeaSource).ToList();
            var jsonResult = Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
    }

}
