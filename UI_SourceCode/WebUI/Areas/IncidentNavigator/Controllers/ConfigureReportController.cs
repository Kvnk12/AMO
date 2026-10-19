using Data;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Dto;
using NTTData.AMOPortal.WebUI.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using WebUI.Areas.IncidentNavigator.Models;
using log4net;
using System.Data;
using WebUI.Dto;
using System.IO;
/// <summary>
/// /
/// 
/// </summary>
namespace WebUI.Areas.IncidentNavigator.Controllers
{
    public class ConfigureReportController : Controller
    {
        //
        // GET: /IncidentNavigator/ConfigureReport/

        public static int ClientId;
        public static DateTime startdate;
        public static DateTime enddate;
        public static int ReportGrp;
        public static int[] TypeList;
        public static int[] StatusList;
        public static int[] PriorityList;
        public static int[] GroupList;
        public static int[] TowerList;
        public static int WaiveOption;
        public static string DateRange;

        private static readonly ILog Log = LogManager.GetLogger(typeof(ConfigureReportController));

        ~ConfigureReportController()
        {
            //FromYear = mod.FromYear;
            //ToYear = mod.ToYear;
            //DateRange = mod.DateRange;
        }
        [ActionTrack]
        public ActionResult Index()
        {
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);

            return View();
        }

        public ActionResult AnalysisReport()
        {
            try
            {
                INReportObservationModel mod = prepareCommonParameters();
                //WebConfigurationManager.AppSettings["TelerikReportServer"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                IEnumerable<string> userEmailList = IncidentNavigatorHelper.getEmailUserPersonnel(client.ClientId);
                var UserId = (int)Session["UserId"];
                ViewData["GetStringResult"] = getFullName(UserId);

                ViewBag.userEmailList = userEmailList;
                return View("AnalysisReport", mod);
            }
            catch(Exception ex)
            {
                Log.Error("Analysis Index Action error 'AnalysisReport()' ", ex);
                throw ex;
            }

        }
        public ActionResult Clustering()
        {
            try
            {
                INReportObservationModel mod = prepareCommonParameters();
                var UserId = (int)Session["UserId"];
                ViewData["GetStringResult"] = getFullName(UserId);

                //WebConfigurationManager.AppSettings["TelerikReportServer"];
                return View("Clustering", mod);
            }
            catch (Exception ex)
            {
                Log.Error("Clustering Index Action error 'Clustering()' ", ex);
                throw ex;
            }
        }
        public ActionResult Maintenance()
        {
            try
            {


                INReportObservationModel mod = prepareCommonParameters();

                ClientDetail client = (ClientDetail)Session["SessionClient"];
                IEnumerable<string> userEmailList = IncidentNavigatorHelper.getEmailUserPersonnel(client.ClientId);
                var UserId = (int)Session["UserId"];
                ViewData["GetStringResult"] = getFullName(UserId);

                ViewBag.userEmailList = userEmailList;
                return View(mod);
            }
            catch (Exception ex)
            {
                Log.Error("Maintenance Index Action error 'Maintenance' ", ex);
                throw ex;
            }
        }
        //public ActionResult TAReports()
        //{
        //    try
        //    {
        //        INReportObservationModel mod = prepareCommonParameters();
        //        return View(mod);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error("TAReports Index Action error 'TAReports()' ", ex);
        //        throw ex;
        //    }
        //}

        INReportObservationModel mod = new INReportObservationModel();

        private INReportObservationModel prepareCommonParameters()
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            try
            { 

            mod.ClientName = client.ClientName;
            mod.UserName = IdeaManager.IMHelper.GetUserName(uid);
            mod.ClientId = client.ClientId;
            mod.UserId = uid;
            mod.ToYear = DateTime.Today.Date;
            mod.FromYear = DateTime.Today.Date.AddMonths(-1);
            mod.ClusterPath = WebConfigurationManager.AppSettings["ClusterPath"];
            mod.CreatedDt = DateTime.Now;
            if (mod.ClientId == ClientId)
            {
                mod.FromYear = startdate;
                mod.ToYear = enddate;
                mod.SelectedReportGrp = ReportGrp;
                mod.SelectedTypeList    = TypeList;
                mod.SelectedStatusList  = StatusList;
                mod.SelectedPriorityList    = PriorityList;
                mod.SelectedGroupList   = GroupList;
                mod.SelectedTowerList   = TowerList;
                mod.SelectedWaiveOption = WaiveOption;

            }
            else
            {
                ClientId    = mod.ClientId;
                startdate   = mod.FromYear.Value;
                enddate     = mod.ToYear.Value;
                ReportGrp   = mod.SelectedReportGrp = 2;
                TypeList = null;// mod.SelectedTypeList;
                TypeList = mod.SelectedTypeList;
                StatusList  = mod.SelectedStatusList;
                PriorityList = mod.SelectedPriorityList;
                GroupList   = mod.SelectedGroupList;
                TowerList   = mod.SelectedTowerList;
                WaiveOption = mod.SelectedWaiveOption = 1;

            }
            GetFilterInfo(mod, client.ClientId);
            mod.DateRange = mod.FromYear.Value.ToString("MM/dd/yyyy") + " To " + mod.ToYear.Value.ToString("MM/dd/yyyy");
                //  FromYear = mod.FromYear;
                //ToYear = mod.ToYear;

            }
            catch (Exception ex)
            {
                Log.Error("TAReports Index Action error 'prepareCommonParameters() for client "+ client.ClientId.ToString(), ex);
                throw ex;
            }

            return mod;
        }

        //public ActionResult Support()
        //{
        //    try
        //    {
        //        INReportObservationModel mod = prepareCommonParameters();
        //        return View("Support", mod);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error("Support Index Action error 'Support()' ", ex);
        //        throw ex;
        //    }

        //}
        public ActionResult Trend()
        {
            try
            {
                INReportObservationModel mod = prepareCommonParameters();


            ClientDetail client = (ClientDetail)Session["SessionClient"];
            IEnumerable<string> userEmailList = IncidentNavigatorHelper.getEmailUserPersonnel(client.ClientId);

            ViewBag.userEmailList = userEmailList;
                var UserId = (int)Session["UserId"];
                ViewData["GetStringResult"] = getFullName(UserId);

                return View("Trend", mod);
            }
            catch (Exception ex)
            {
                Log.Error("Trend Index Action error 'Trend()' ", ex);
                throw ex;
            }
        }
        public ActionResult WSR()
        {
            try
            {
                INReportObservationModel mod = prepareCommonParameters();


                ClientDetail client = (ClientDetail)Session["SessionClient"];
                IEnumerable<string> userEmailList = IncidentNavigatorHelper.getEmailUserPersonnel(client.ClientId);

                ViewBag.userEmailList = userEmailList;
                var UserId = (int)Session["UserId"];
                ViewData["GetStringResult"] = getFullName(UserId);

                return View("WSR", mod);
            }
            catch (Exception ex)
            {
                Log.Error("WSR Index Action error 'WSR()' ", ex);
                throw ex;
            }
        }
        public ActionResult ComplianceReport()
        {
            try
            {
                INReportObservationModel mod = prepareCommonParameters();


                ClientDetail client = (ClientDetail)Session["SessionClient"];
                IEnumerable<string> userEmailList = IncidentNavigatorHelper.getEmailUserPersonnel(client.ClientId);

                ViewBag.userEmailList = userEmailList;
                var UserId = (int)Session["UserId"];
                ViewData["GetStringResult"] = getFullName(UserId);

                return View("ComplianceReport", mod);
            }
            catch (Exception ex)
            {
                Log.Error("ComplianceReport Index Action error 'ComplianceReport()' ", ex);
                throw ex;
            }
        }
        
        public ActionResult DailyReports()
        {
            try
            {
                INReportObservationModel mod = prepareCommonParameters();


                ClientDetail client = (ClientDetail)Session["SessionClient"];
                IEnumerable<string> userEmailList = IncidentNavigatorHelper.getEmailUserPersonnel(client.ClientId);

                ViewBag.userEmailList = userEmailList;
                var UserId = (int)Session["UserId"];
                ViewData["GetStringResult"] = getFullName(UserId);

                return View("DailyReports", mod);
            }
            catch (Exception ex)
            {
                Log.Error("DailyReports Index Action error 'DailyReports()' ", ex);
                throw ex;
            }
        }
        public ActionResult Volume()
        {
            try
            {
                INReportObservationModel mod = prepareCommonParameters();

            ClientDetail client = (ClientDetail)Session["SessionClient"];
            IEnumerable<string> userEmailList = IncidentNavigatorHelper.getEmailUserPersonnel(client.ClientId);
                var UserId = (int)Session["UserId"];
                ViewData["GetStringResult"] = getFullName(UserId);

                ViewBag.userEmailList = userEmailList;
            return View("Volume", mod);
            }
            catch (Exception ex)
            {
                Log.Error("Volume Index Action error 'Volume()' ", ex);
                throw ex;
            }
        }
        
        public ActionResult CustomerSatisfaction()
        {
            try
            {
                INReportObservationModel mod = prepareCommonParameters();

                ClientDetail client = (ClientDetail)Session["SessionClient"];
                IEnumerable<string> userEmailList = IncidentNavigatorHelper.getEmailUserPersonnel(client.ClientId);
                var UserId = (int)Session["UserId"];
                ViewData["GetStringResult"] = getFullName(UserId);

                ViewBag.userEmailList = userEmailList;
                return View("CustomerSatisfaction", mod);
            }
            catch (Exception ex)
            {
                Log.Error("CustomerSatisfaction Index Action error 'CustomerSatisfaction()' ", ex);
                throw ex;
            }
        }
        public ActionResult Problem()
        {
            try
            {

            INReportObservationModel mod = prepareCommonParameters();

            ClientDetail client = (ClientDetail)Session["SessionClient"];
            IEnumerable<string> userEmailList = IncidentNavigatorHelper.getEmailUserPersonnel(client.ClientId);
                var UserId = (int)Session["UserId"];
                ViewData["GetStringResult"] = getFullName(UserId);

                ViewBag.userEmailList = userEmailList;
            return View("Problem", mod);
           }
         catch (Exception ex)
            {
                Log.Error("Problem Index Action error 'Problem()' ", ex);
                throw ex;
            }
        }

        public ActionResult SLA_Reports()
        {
            try
            {
                INReportObservationModel mod = prepareCommonParameters();
                List<SelectListItem> ls;
                ls = new List<SelectListItem>();
                ls.Add(new SelectListItem() { Text = "Response", Value = "0" });
                ls.Add(new SelectListItem() { Text = "Resolution", Value = "1" });
                mod.ReportList = ls;

                ClientDetail client = (ClientDetail)Session["SessionClient"];
                IEnumerable<string> userEmailList = IncidentNavigatorHelper.getEmailUserPersonnel(client.ClientId);
                var UserId = (int)Session["UserId"];
                ViewData["GetStringResult"] = getFullName(UserId);

                ViewBag.userEmailList = userEmailList;
                return View("SLA_Reports", mod);
            }

            catch (Exception ex)
            {
                Log.Error("SLA_Reports Index Action error 'SLA_Reports()' ", ex);
                throw ex;
            }
        }
        public ActionResult KPI_Reports()
        {
            try
            {
                INReportObservationModel mod = prepareCommonParameters();
            List<SelectListItem> ls;
            ls = new List<SelectListItem>();
            ls.Add(new SelectListItem() { Text = "MTTA", Value = "1" });
            ls.Add(new SelectListItem() { Text = "MTTR", Value = "2" });
            ls.Add(new SelectListItem() { Text = "FirstTimeRight", Value = "3" });
            ls.Add(new SelectListItem() { Text = "Aging", Value = "0" });
            mod.ReportList = ls;

            ClientDetail client = (ClientDetail)Session["SessionClient"];
            IEnumerable<string> userEmailList = IncidentNavigatorHelper.getEmailUserPersonnel(client.ClientId);
                var UserId = (int)Session["UserId"];
                ViewData["GetStringResult"] = getFullName(UserId);

                ViewBag.userEmailList = userEmailList;
            //WebConfigurationManager.AppSettings["TelerikReportServer"];
            return View("KPI_Reports", mod);
            }
            catch (Exception ex)
            {
                Log.Error("KPI_Reports Index Action error 'KPI_Reports()' ", ex);
                throw ex;
            }
        }

        public ActionResult InflowOutflowReport()
        {
            try
            {
                INReportObservationModel mod = prepareCommonParameters();


                ClientDetail client = (ClientDetail)Session["SessionClient"];
                IEnumerable<string> userEmailList = IncidentNavigatorHelper.getEmailUserPersonnel(client.ClientId);
                var UserId = (int)Session["UserId"];
                ViewData["GetStringResult"] = getFullName(UserId);

                ViewBag.userEmailList = userEmailList;

                return View("InflowOutflowReport", mod);
            }
            catch (Exception ex)
            {
                Log.Error("InflowOutflowReport Index Action error 'InflowOutflowReport()' ", ex);
                throw ex;
            }
        }
        public ActionResult TokenChartRD()
        {
            try
            {
                INReportObservationModel mod = prepareCommonParameters();


                ClientDetail client = (ClientDetail)Session["SessionClient"];
                IEnumerable<string> userEmailList = IncidentNavigatorHelper.getEmailUserPersonnel(client.ClientId);
                var UserId = (int)Session["UserId"];
                ViewData["GetStringResult"] = getFullName(UserId);

                ViewBag.userEmailList = userEmailList;

                return View("TokenChartRD", mod);
            }
            catch (Exception ex)
            {
                Log.Error("TokenChartRD Index Action error 'TokenChartRD()' ", ex);
                throw ex;
            }
        }

        public ActionResult UPSRuleOf1_Reports()
        {
            try
            {
                INReportObservationModel mod = prepareCommonParameters();
                List<SelectListItem> ls;
                ls = new List<SelectListItem>();
                ls.Add(new SelectListItem() { Text = "Response", Value = "0" });
                ls.Add(new SelectListItem() { Text = "Resolution", Value = "1" });
                mod.ReportList = ls;

                ClientDetail client = (ClientDetail)Session["SessionClient"];
                IEnumerable<string> userEmailList = IncidentNavigatorHelper.getEmailUserPersonnel(client.ClientId);
                var UserId = (int)Session["UserId"];
                ViewData["GetStringResult"] = getFullName(UserId);

                ViewBag.userEmailList = userEmailList;
                return View("SLA_Reports", mod);
            }

            catch (Exception ex)
            {
                Log.Error("UPSRuleOf1_Reports Index Action error 'UPSRuleOf1_Reports()' ", ex);
                throw ex;
            }
        }
        //------------------------------------------Drop Down Values----------------------------------//
        public IEnumerable<SelectListItem> GetINReportList()
        {
            try
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var list = IncidentNavigatorHelper.getINReportList();
                return list;
            }
            catch (Exception ex)
            {
                Log.Error("KPI_Reports Index Action error 'KPI_Reports()' ", ex);
                throw ex;
            }
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Incident Navigator" }, null)]
        [HttpPost]
        public ActionResult GetReport([DataSourceRequest] DataSourceRequest request)
        {
            //  Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            return Json(IncidentNavigatorHelper.getAllReport(client.ClientId).ToDataSourceResult(request));
        }


        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Incident Navigator" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult AddReport([DataSourceRequest] DataSourceRequest request, INClientReportModel Report)
        {
            //  var xcxc = PortfolioAnalyzerHelper.getall(ApplicationID, interfacetype);
            if (Report != null && ModelState.IsValid)
            {

                //Client client = (Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                //Report.INReportId = Reportid;

                var res = IncidentNavigatorHelper.AddReport(Report, client.ClientId, usid);
                Report.Id = res;




                //// interfacemodel.ApplicationInterfacedText=<GetAppNameFor(ApplicationID)> todo to avoid null display
            }
            return Json(new[] { Report }.ToDataSourceResult(request, ModelState));

        }


        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Incident Navigator" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult DeleteReport([DataSourceRequest] DataSourceRequest request, INClientReportModel interfacemodel)
        {
            if (interfacemodel != null)
            {
                var v = IncidentNavigatorHelper.DeleteReport(interfacemodel.Id);
            }

            return Json(new[] { interfacemodel }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        [ActionTrack]
        public ActionResult UpdateObservation(string Range = "", string Observation = "", int ReportId = 0, string Type = "")
        {
            //  var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            var ObservationString = Server.HtmlDecode(Observation);
            if (Observation != null)
            {

                IncidentNavigatorHelper.saveObservation(client.ClientId, Range, uid, Observation, ReportId, Type);
            }
            return Json(new { ObservationString }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult GetObservation([DataSourceRequest] DataSourceRequest request)
        {
            //var clid = (Client)Session["Client"];
            ClientDetail clid = (ClientDetail)Session["SessionClient"];
            return Json(IncidentNavigatorHelper.ReadObservation(clid.ClientId).AsEnumerable().ToDataSourceResult(request));
        }


        public ActionResult Datarepository()
        {
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);

            return View();
        }
        public ActionResult CSATReport()
        {
            try
            {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                var result = IncidentNavigatorHelper.getCSATCluster(client.ClientId);
                var UserId = (int)Session["UserId"];
                ViewData["GetStringResult"] = getFullName(UserId);

                ViewBag.Col1 = "ViewBag Reference";
                DataTable dt = IncidentNavigatorHelper.getCSATCluster(client.ClientId);
                //var i = 0;
                if (dt.Columns.Count != 0)
                {
                    ViewBag.col1 = dt.Columns[6].ToString();
                    ViewBag.col2 = dt.Columns[7].ToString();
                    ViewBag.col3 = dt.Columns[8].ToString();
                    ViewBag.col4 = dt.Columns[9].ToString();
                }
                else
                {
                    ViewBag.col1 = ViewBag.col2 = ViewBag.col3 = ViewBag.col4 = null;
                }

                return View(dt);
            }
            catch (Exception ex)
            {
                Log.Error("KPI_Reports Index Action error 'KPI_Reports()' ", ex);
                throw ex;
            }
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetTickets([DataSourceRequest] DataSourceRequest request)
        {
            try {
                // var client = (Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                IList<SLAMaintenenceModel> SLAMaintenencList = new List<SLAMaintenenceModel>();
                using (var contextFactory = new DbContextFactory())
                {
                    IEnumerable<SLAMaintenenceModel> IncidentDetails1 = IncidentNavigatorHelper.Sp_getIncidentsCluster(client.ClientId);
                    SLAMaintenencList = IncidentDetails1.ToList();
                }
                foreach (var item in SLAMaintenencList)
                {
                    if (!(item.CreatedDateTime is null))
                        item.CreatedDateTime = Convert.ToDateTime(item.CreatedDateTime.Value.ToShortDateString());
                    if (!(item.AssignedDateTime is null))
                        item.AssignedDateTime = Convert.ToDateTime(item.AssignedDateTime.Value.ToShortDateString());
                    if (!(item.ResolutionDateTime is null))
                        item.ResolutionDateTime = Convert.ToDateTime(item.ResolutionDateTime.Value.ToShortDateString());
                    if (!(item.ClosedDateTime is null))
                        item.ClosedDateTime = Convert.ToDateTime(item.ClosedDateTime.Value.ToShortDateString());
                    if (!(item.LastUpdatedDateTime is null))
                        item.LastUpdatedDateTime = Convert.ToDateTime(item.LastUpdatedDateTime.Value.ToShortDateString());
                }
                var jsonResult = Json(SLAMaintenencList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                // jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                Log.Error("GetTickets Index Action error 'GetTickets()' ", ex);
                throw ex;
            }
        }


        [HttpPost]
        public ActionResult GetConsolidatedTickets([DataSourceRequest] DataSourceRequest request, string DateFilters, DateTime StartDate, DateTime EndDate, string TypeArray,
                string ContractIDs, string StatusArray, string PriorityArray, string GroupArray, string TowerArray, string Account, string ReportBy, string WaivedindicatorOp1,
                string graphPriority, string graphDate , string param3 , string param4 , string param5 , int count)

        {
            try
            {

        
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            IList<SLAMaintenenceModel> SLAMaintenencList = new List<SLAMaintenenceModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<SLAMaintenenceModel> IncidentDetails1 = IncidentNavigatorHelper.getConsolidatedCluster(client.ClientId, DateFilters, StartDate, EndDate, TypeArray, ContractIDs
                                                                          , StatusArray, PriorityArray, TowerArray, ReportBy, GroupArray, WaivedindicatorOp1, Account , graphPriority, graphDate, 
                                                                            param3, param4 , param5 , count);
                SLAMaintenencList = IncidentDetails1.ToList();

            }



            Console.WriteLine("WriteLine");
            Console.Write("Write");

            return Json(SLAMaintenencList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
            catch (Exception ex)
            {
                Log.Error("GetTickets Index Action error 'GetTickets()' ", ex);
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult GetResolutionTickets([DataSourceRequest] DataSourceRequest request, string DateFilters, DateTime StartDate, DateTime EndDate, string TypeArray,
                string ContractIDs, string StatusArray, string PriorityArray, string GroupArray, string TowerArray, string WaivedindicatorOp1, string Account, string ReportBy,
                string graphPriority, string tower, string typeval, int count, string ReportSelectedname, string graphDate, string waiveOption,string contractnameReport)

        {
           try {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                IList<SLAMaintenenceModel> SLAMaintenencList = new List<SLAMaintenenceModel>();
                using (var contextFactory = new DbContextFactory())
                {
                    if (ReportSelectedname == "Response")
                    {
                        IEnumerable<SLAMaintenenceModel> IncidentDetails2 = IncidentNavigatorHelper.getResponseCluster(client.ClientId, DateFilters, StartDate, EndDate, TypeArray, ContractIDs
                                                                                          , StatusArray, PriorityArray, TowerArray, ReportBy, GroupArray, WaivedindicatorOp1, Account, graphPriority, tower, typeval, count, waiveOption, contractnameReport);
                        SLAMaintenencList = IncidentDetails2.ToList();
                    }
                    else if (ReportSelectedname == "Resolution")
                    {
                      IEnumerable<SLAMaintenenceModel> IncidentDetails1 = IncidentNavigatorHelper.getResolutionCluster(client.ClientId, DateFilters, StartDate, EndDate, TypeArray, ContractIDs
                                                                                      , StatusArray, PriorityArray, TowerArray, ReportBy, GroupArray, WaivedindicatorOp1, Account, graphPriority, tower, typeval, count, waiveOption, contractnameReport);
                        SLAMaintenencList = IncidentDetails1.ToList();
                    }
                    else if (ReportSelectedname == "ResponseProd")
                    {
                        Log.Error("ClientId" + client.ClientId);
                        Log.Error("DateFilters" + DateFilters);
                        Log.Error("StartDate" + StartDate);
                        Log.Error("EndDate" + EndDate);
                        Log.Error("TypeArray" + TypeArray);
                        Log.Error("ContractIDs" + ContractIDs);
                        Log.Error("PriorityArray" + PriorityArray);
                        Log.Error("TowerArray" + TowerArray);
                        Log.Error("ReportBy" + ReportBy);
                        Log.Error("GroupArray" + GroupArray);
                        Log.Error("WaivedindicatorOp1" + WaivedindicatorOp1);
                        Log.Error("Account" + Account);
                        Log.Error("graphPriority" + graphPriority);
                        Log.Error("tower" + tower);
                        Log.Error("typeval" + typeval);
                        Log.Error("count" + count);
                        Log.Error("graphDate" + graphDate);
                        Log.Error("waiveOption" + waiveOption);
                        IEnumerable<SLAMaintenenceModel> IncidentDetails3 = IncidentNavigatorHelper.getResponseProdCluster(client.ClientId, DateFilters, StartDate, EndDate, TypeArray, ContractIDs
                                                                                      , StatusArray, PriorityArray, TowerArray, ReportBy, GroupArray, WaivedindicatorOp1, Account, graphPriority, tower, typeval, count, graphDate, waiveOption);
                        SLAMaintenencList = IncidentDetails3.ToList();
                    }
                    else if (ReportSelectedname == "ResolutionProd")
                    {
                      IEnumerable<SLAMaintenenceModel> IncidentDetails4 = IncidentNavigatorHelper.getResolutionProdCluster(client.ClientId, DateFilters, StartDate, EndDate, TypeArray, ContractIDs
                                                                                      , StatusArray, PriorityArray, TowerArray, ReportBy, GroupArray, WaivedindicatorOp1, Account, graphPriority, tower, typeval, count, graphDate, waiveOption);
                        SLAMaintenencList = IncidentDetails4.ToList();
                    
                }
                }
                
                return Json(SLAMaintenencList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("GetResolutionTickets Index Action error 'GetResolutionTickets()' ", ex);
                throw ex;
            }
        }



        [HttpPost]
        public ActionResult GetKPITickets([DataSourceRequest] DataSourceRequest request, string DateFilters, DateTime StartDate, DateTime EndDate, string TypeArray,
               string ContractIDs, string StatusArray, string PriorityArray, string GroupArray, string TowerArray, string WaivedindicatorOp1, string Account, string ReportBy,
               string graphPriority, string tower, string typeval, int count, string ReportSelectedname, string graphDate, string waiveOption, string Agingtitle, int Recyclecount)

        {
            try {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                IList<SLAMaintenenceModel> SLAMaintenencList = new List<SLAMaintenenceModel>();
                using (var contextFactory = new DbContextFactory())
                {
                    IEnumerable<SLAMaintenenceModel> IncidentDetails2 = IncidentNavigatorHelper.getKPICluster(client.ClientId, DateFilters, StartDate, EndDate, TypeArray, ContractIDs
                                                                                      , StatusArray, PriorityArray, TowerArray, ReportBy, GroupArray, WaivedindicatorOp1, Account, graphPriority, ReportSelectedname, graphDate, waiveOption, Agingtitle, Recyclecount);
                    SLAMaintenencList = IncidentDetails2.ToList();
                }

                return Json(SLAMaintenencList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("GetKPITickets Index Action error 'GetKPITickets()' ", ex);
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult getClusterTickets([DataSourceRequest] DataSourceRequest request, string clusterlabel, string Configitemname, string ReportSelected)

        {
            try {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                IList<SLAMaintenenceModel> SLAMaintenencList = new List<SLAMaintenenceModel>();
                using (var contextFactory = new DbContextFactory())
                {
                    IEnumerable<SLAMaintenenceModel> IncidentDetails2 = IncidentNavigatorHelper.getClusterDetails(client.ClientId, clusterlabel, Configitemname, ReportSelected);
                    SLAMaintenencList = IncidentDetails2.ToList();
                }

                return Json(SLAMaintenencList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("getClusterTickets Index Action error 'getClusterTickets()' ", ex);
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult GetVolumetricTickets([DataSourceRequest] DataSourceRequest request, string DateFilters, DateTime StartDate, DateTime EndDate, string TypeArray,
        string ContractIDs, string StatusArray, string PriorityArray, string GroupArray, string TowerArray, string WaivedindicatorOp1, string Account, string ReportBy, 
        string param1, string param2, string param3, string param4,string param5 , int count)

        {
            try
            {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                IList<SLAMaintenenceModel> SLAMaintenencList = new List<SLAMaintenenceModel>();
                using (var contextFactory = new DbContextFactory())
                {
                    IEnumerable<SLAMaintenenceModel> IncidentDetails1 = IncidentNavigatorHelper.getVolumeCluster(client.ClientId, DateFilters, StartDate, EndDate, TypeArray, ContractIDs
                                                                              , StatusArray, PriorityArray, TowerArray, ReportBy, GroupArray, WaivedindicatorOp1, Account, param1, param2,
                                                                                param3, param4, param5, count);
                    SLAMaintenencList = IncidentDetails1.ToList();

                }
                Console.WriteLine("WriteLine");
                Console.Write("Write");
                return Json(SLAMaintenencList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("GetVolumetricTickets Index Action error 'GetVolumetricTickets()' ", ex);
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Export_Tickets_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);


            return File(fileContents, contentType, fileName);
        }

        public ActionResult GetCSAT([DataSourceRequest] DataSourceRequest request)
        {
            try {
                // var client = (Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                IList<CSATModel> CSATList = new List<CSATModel>();
                DataTable dt = IncidentNavigatorHelper.getCSATCluster(client.ClientId);

                foreach (DataRow dr in dt.Rows)
                {
                    var values = dr.ItemArray;

                    var csat1 = new CSATModel()
                    {
                        TicketID = values[0].ToString(),
                        Track = values[1].ToString(),
                        Status = values[2].ToString(),
                        TicketClosedDate = values[4].ToString(),
                        SurveySentdate = values[5].ToString(),
                        Question1 = Convert.ToInt32(values[6]),
                        Question2 = Convert.ToInt32(values[7]),
                        Question3 = Convert.ToInt32(values[8]),
                        Question4 = Convert.ToInt32(values[9]),
                        Comments = values[3].ToString(),
                        //Weightage = Convert.ToInt32(values[4])
                        Weightage1 = 20,
                        Weightage2 = 30,
                        Weightage3 = 30,
                        Weightage4 = 20,

                    };
                    CSATList.Add(csat1);
                }

                var Q1Total = 0.0;
                var Q2Total = 0.0;
                var Q3Total = 0.0;
                var Q4Total = 0.0;

                var TotalResponse = 0;
                var totalAvg = 0.0;

                foreach (var a in CSATList)
                {
                    Q1Total = Q1Total + Convert.ToInt32(a.Question1);
                    Q2Total = Q2Total + Convert.ToInt32(a.Question2);
                    Q3Total = Q3Total + Convert.ToInt32(a.Question3);
                    Q4Total = Q4Total + Convert.ToInt32(a.Question4);
                    TotalResponse += 1;

                    a.Total = Convert.ToInt32(a.Question1) + Convert.ToInt32(a.Question2) + Convert.ToInt32(a.Question3) +
                              Convert.ToInt32(a.Question4);

                }

                foreach (var a in CSATList)
                {
                    totalAvg += a.Total;

                }

                if (CSATList.Count() != 0)
                {
                    Q1Total /= CSATList.Count();
                    Q2Total /= CSATList.Count();
                    Q3Total /= CSATList.Count();
                    Q4Total /= CSATList.Count();
                    totalAvg = (totalAvg) / CSATList.Count();
                }

                if (CSATList.Count() != 0)
                {
                    var csat = new CSATModel()
                    {
                        TicketID = null,
                        Track = "Total",
                        Status = TotalResponse.ToString(),
                        TicketClosedDate = null,
                        SurveySentdate = "Average",
                        Question1 = Convert.ToInt32(Math.Round((double)(Q1Total), 2)),
                        Question2 = Convert.ToInt32(Math.Round((double)(Q2Total), 2)),
                        Question3 = Convert.ToInt32(Math.Round((double)(Q3Total), 2)),
                        Question4 = Convert.ToInt32(Math.Round((double)(Q4Total), 2)),
                        Comments = null,
                        //Weightage = Convert.ToInt32(values[4])
                        Weightage1 = Math.Round(((decimal)(20 * CSATList.Count()) / CSATList.Count()), 2),
                        Weightage2 = Math.Round(((decimal)(30 * CSATList.Count()) / CSATList.Count()), 2),
                        Weightage3 = Math.Round(((decimal)(30 * CSATList.Count()) / CSATList.Count()), 2),
                        Weightage4 = Math.Round(((decimal)(20 * CSATList.Count()) / CSATList.Count()), 2),
                        //Total = totalAvg
                    };

                    //CSATList.Add(csat);
                }
                //return Json(SLAMaintenencList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                //return Json(CSATList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                return Json(CSATList.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                Log.Error("GetCSAT Index Action error 'GetCSAT()' ", ex);
                throw ex;
            }
        }



        public static void GetFilterInfo(INReportObservationModel mod, int ClientId)
        {
            //IList<TrendingReportInput> Tredingdata = new List<TrendingReportInput>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                DataSet ds = new DataSet();


                ds = unitOfWork.GetDataFromStoredProcedureDS("FetchAllFilterInfo", paramName, paramValue);

                List<SelectListItem> ls;

                for (int t = 0; t < ds.Tables.Count; t++)
                {
                    int i = 0;
                    ls = new List<SelectListItem>();
                    if (t < 10)
                    {
                        foreach (DataRow row in ds.Tables[t].Rows)
                        {
                            ls.Add(new SelectListItem() { Text = row.ItemArray[0].ToString(), Value = i.ToString() });
                            i++;
                        }
                    }
                    else
                    {
                        foreach (DataRow row in ds.Tables[t].Rows)
                        {
                            ls.Add(new SelectListItem() { Text = row.ItemArray[0].ToString(), Value = row.ItemArray[1].ToString() });
                            i++;
                        }
                    }
                    switch (t)
                    {
                        case 0:
                            mod.TypeList = ls;
                            mod.DefTicketType = "";
                            foreach (var item in mod.TypeList)
                            {
                                var str = item.Text.ToLower();
                                if (str.Substring(0, 2) == "in")
                                {
                                    if (mod.DefTicketType.Length > 1)
                                    { mod.DefTicketType += ","; }

                                    mod.DefTicketType += item.Text;
                                }

                            }

                            break;
                        case 1:
                            mod.StatusList = ls;
                            break;
                        case 2:
                            mod.PriorityList = ls;
                            break;
                        case 3:
                            mod.GroupList = ls;
                            break;
                        case 4:
                            mod.TowerList = ls;
                            break;
                        case 5:
                            mod.DateColumnsList = ls;
                            break;
                        case 6:
                            mod.Maintnenance_TypeList = ls;
                            break;
                        case 7:
                            mod.Maintnenance_StatusList = ls;
                            break;
                        case 8:
                            mod.Maintnenance_PriorityList = ls;
                            break;

                        case 9:
                            mod.WaivedFilterList = ls;
                            break;

                        case 10:
                            mod.AccountNamesList = ls;
                            break;
                        case 11:
                            mod.ContractList = ls;
                            break;

                        case 12:
                            mod.Maintnenance_ContractList = ls;
                            break;



                    }

                }


                // To select incidetn type from typelist
                
                if (TypeList == null ||  TypeList.Length <= 0)
                {
                    List<int> Typeselct = new List<int>();

                    int sel = 0, index =0;
                    foreach (var item in mod.TypeList)
                    {
                        var str = item.Text.ToLower();
                        if (str.Substring(0, 2) == "in")
                        {
                            sel++;
                        }

                    }
                    foreach (var item in mod.TypeList)
                    {
                        var str = item.Text.ToLower();

                        if (str.Substring(0, 2) == "in" || sel < 1)
                        {
                            Typeselct.Add( index);
                        }
                        index++;
                    }
                    TypeList = Typeselct.ToArray();
                    mod.SelectedTypeList = Typeselct.ToArray();
                }
                return;
            }

        }

        public ActionResult ExportTicketData()
        {
            // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            string fileName = client.ClientName.Replace(" ", "_") + "_TicketData" + ".xlsx";
            System.IO.Stream stream = App_Start.Helper.GetXLFileStream(GetTicketData(ClientId));
            stream.Position = 0;
            string mimeType = "application/ms-excel";
            var fileStreamResult = new FileStreamResult(stream, mimeType);
            fileStreamResult.FileDownloadName = fileName;
            fileStreamResult.FileStream.Seek(0, SeekOrigin.Begin);
            return fileStreamResult;
        }

        private System.Data.DataTable GetTicketData(int ClientId)
        {
            var contextFactory = new DbContextFactory();
            DataTable returntable = new DataTable();
            using (var unitOfWork = new UnitOfWorks(contextFactory.GetContext()))
            {
                var paramName = new string[] { "clientid" };
                var paramValue = new object[] { ClientId };
                returntable = unitOfWork.GetDataFromStoredProcedure("INTicketInventory", paramName, paramValue);

            }
            return returntable;
        }

        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveReportDetails(DateTime FromYear, DateTime  ToYear,int DateTimeGroup, int[] TypeDropDown, int[] StatusDropDown, int[] PriorityDropDown, int[] GroupDropDown, int[] TowerDropDown, int WaivedindicatorOption)
        {
            //var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            enddate = ToYear;
            startdate = FromYear;
            TowerList = TowerDropDown;
            if (DateTimeGroup >= 0) // for customer satifaction passing -1 and we can skip all these details
            {
                ReportGrp = DateTimeGroup;
                TypeList = TypeDropDown;
                StatusList = StatusDropDown;
                PriorityList = PriorityDropDown;
                GroupList = GroupDropDown;
                WaiveOption = WaivedindicatorOption;
            }



            return Json(0);
        }
        public ActionResult getContractId(int Accid = 0)
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
            return Json(dispSln.ToList().Select(A => new { Value = A.Id.ToString(), Text = A.ContractName, Selected = true }), JsonRequestBehavior.AllowGet);

        }
        public ActionResult CompliReport()
        {
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);

            return View();
        }
        [HttpPost]
        public ActionResult GetRecords([DataSourceRequest] DataSourceRequest request)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var rec = IncidentNavigatorHelper.GetCompRecords(client.ClientId);

            return Json(rec.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }
        public string getFullName(int userId)
        {
            Db db = new Db();
            var fullName = (from b in db.UserProfiles
                            where b.Id == userId
                            select b.FirstName + " " + b.LastName).FirstOrDefault();

            return fullName;
        }
    }
}


