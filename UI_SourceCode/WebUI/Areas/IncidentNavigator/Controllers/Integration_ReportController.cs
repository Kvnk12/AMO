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
using System.IO;
using log4net;
using NTTData.AMOPortal.WebUI.Filters;
using System.Diagnostics;

namespace WebUI.Areas.IncidentNavigator.Controllers
{
    public class Integration_ReportController : Controller
    {
        //
        // GET: /IncidentNavigator/Dashboard/
        private static readonly ILog Log = LogManager.GetLogger(typeof(Integration_ReportController));

        public static DataTable dtCluster;
        public static int ClientId;
        public static DateTime startdate;
        public static DateTime enddate;
        public static int ReportGrp;
        public static int[] TypeList;
        public static int[] StatusList;
        public static int[] PriorityList;
        public static int[] GroupList;
        public static int[] TowerList;
//        public static int WaiveOption;
        public static string DateRange;
        public static string Exp_TicketType,  Exp_Status, Exp_Track, Exp_Priority, Exp_Datetype, Exp_contract;
        public static DateTime Exp_StartDate,  Exp_EndDate;

        public ActionResult Index()
        {
            Integration_ReportListModel c = prepareCommonParameters();
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);

            return View(c);
        }
        public string getFullName(int userId)
        {
            Db db = new Db();
            var fullName = (from b in db.UserProfiles
                            where b.Id == userId
                            select b.FirstName + " " + b.LastName).FirstOrDefault();

            return fullName;
        }
        private Integration_ReportListModel prepareCommonParameters()
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            try
            { 
            //  Client client = (Client)Session["Client"];
                int uid = (int)Session["UserId"];
            Integration_ReportListModel mod = new Integration_ReportListModel();
            mod.ClientName = client.ClientName;
            mod.UserName = IdeaManager.IMHelper.GetUserName(uid);
            mod.ClientId = client.ClientId;
            mod.UserId = uid;
            mod.ToYear = DateTime.Today.Date;
            mod.FromYear = DateTime.Today.Date.AddMonths(-1);
            ClientId = 0;
                if(dtCluster!= null && dtCluster.Rows.Count>0)
                dtCluster.Rows.Clear();

                //mod.CreatedDt = DateTime.Now;
                if (mod.ClientId == ClientId)
            {
                mod.FromYear = startdate;
                mod.ToYear = enddate;
                mod.SelectedTypeList = TypeList;
                mod.SelectedStatusList = StatusList;
                mod.SelectedPriorityList = PriorityList;
                mod.SelectedGroupList = GroupList;
                mod.SelectedTowerList = TowerList;

            }
            else
            {
                startdate = mod.FromYear.Value;
                enddate = mod.ToYear.Value;
                TypeList = null;// mod.SelectedTypeList;
                TypeList = mod.SelectedTypeList;
                StatusList = mod.SelectedStatusList;
                PriorityList = mod.SelectedPriorityList;
                GroupList = mod.SelectedGroupList;
                TowerList = mod.SelectedTowerList;

            }
            Exp_StartDate = mod.FromYear.Value;
                    Exp_EndDate = mod.ToYear.Value;
                    
            GetFilterInfo(mod, client.ClientId);
            mod.DateRange = mod.FromYear.Value.ToString("MM/dd/yyyy") + " To " + mod.ToYear.Value.ToString("MM/dd/yyyy");
            //  FromYear = mod.FromYear;
            //ToYear = mod.ToYear;
            return mod;
        }
            catch (Exception ex)
            {
                Log.Error("Integration_Report Controller  prepareCommonParameters , Client ID is " + client.ClientId);
            }

            return null;

        }
        private Integration_ReportListModel ClusterReportByDesc()
        {
            //  Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            Integration_ReportListModel mod = new Integration_ReportListModel();

            GetFilterInfo(mod, client.ClientId);


            return mod;
        }

        public static void GetFilterInfo(Integration_ReportListModel mod, int ClientId)
        {
            try { 
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
                       if (t==8||t==6||t==7)
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
                            if (tempacc!=null)
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
                                if (tempcon != null)
                                {
                                    mod.Accid = tempcon.Id;
                                }
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
                Log.Error("Integration_Report Controller  prepareCommonParameters , Client ID is " + ClientId);
            }
            return ;

        }

        [HttpPost]
        public ActionResult GetIssueClusterReportNew(string drpval, string TicketType, string Status, string Track, string Priority, string Datetype, DateTime StartDate, DateTime EndDate, string contract, string TextType, int TextLength)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            try
            {
                if (ClientId != client.ClientId || dtCluster == null || dtCluster.Rows.Count < 1)
                {
                    dtCluster = IncidentNavigatorHelper.GetIssueClusterReportDataNew(client.ClientId, 200);
                    ClientId = client.ClientId;
                }

                IList<Integration_ReportModel> model = FilterDataforCluster(drpval, TicketType, Status, Track, Priority, Datetype, StartDate, EndDate, contract, TextType, TextLength);
                //                var calpercent = IncidentNavigatorHelper.calculatePercent(model);
                //                return Json(calpercent, JsonRequestBehavior.AllowGet);
                return Json(model, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Log.Error("Integration_Report Controller  GetIssueClusterReportNew , Client ID is " + client.ClientId, ex);
            }
            return null;

        }

        [HttpPost]
        public ActionResult GetIssueClusterReportforGridNew(string drpval, string TicketType, string Status, string Track, string Priority, string Datetype, DateTime StartDate, DateTime EndDate, string contract, string TextType, int TextLength, [DataSourceRequest] DataSourceRequest request)
        {

            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            try
            {
                IList<Integration_ReportModel> model = FilterDataforCluster(drpval, TicketType, Status, Track, Priority, Datetype, StartDate, EndDate, contract, TextType, TextLength);
                var calpercent = IncidentNavigatorHelper.calculatePercent(model);
                return Json(calpercent.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Integration_Report Controller  GetIssueClusterReportforGrid , Client ID is " + client.ClientId, ex);
            }
            return null;

        }
        public IList<Integration_ReportModel>  FilterDataforCluster(string drpval, string TicketType, string Status, string Track, string Priority, string Datetype, DateTime StartDate, DateTime EndDate, string contract, string TextType, int TextLength)
        {
            try
            {
                // Fileter data
                string filter = ""+Datetype + " >= '"+ StartDate.ToString()+ "' and  " + Datetype + " < '" + EndDate.AddDays(1).ToString()+"'" ;
                
                if (contract != null && contract.ToString().Length >= 1)
                    filter += " and ContractID  IN (" + contract + ") ";
                if (Track.Length > 2)
                    filter += " and Tower IN ('" + Track.Replace("#", "', '") + "') ";
                if (Priority.Length > 2)
                    filter += " and Priority IN ('" + Priority.Replace("#", "', '") + "') ";
                if (Status.Length > 2)
                    filter += " and Status IN ('" + Status.Replace("#", "', '") + "') ";
                if (TicketType.Length > 2)
                    filter += " and Type IN ('" + TicketType.Replace("#", "', '") + "') ";
                IList<Integration_ReportModel> model = new List<Integration_ReportModel>();

                if (dtCluster != null && dtCluster.Select(filter).Count() ==0)
                    {
                    return model;
                }
                DataTable results = dtCluster.Select(filter).CopyToDataTable();

                //  DataTable Filtereddt =  dtCluster.Select(filter);

                // Adding new column with text type and length
                if (!results.Columns.Contains("GrpDesc"))
                results.Columns.Add("GrpDesc");//.DefaultValue = "";

            TextType = TextType.ToString() + "Text";
            int Grpcol = results.Columns.Count - 1;

                    for (int count = 0; count < results.Rows.Count; count++)
                    {
                        if (results.Rows[count][TextType] != null && results.Rows[count][TextType].ToString().Length > 0)
                        {
                            int l = Math.Min(results.Rows[count][TextType].ToString().Length, TextLength);
                            Debug.WriteLine("{0}\t{1}\t{2}", results.Rows[count][TextType].ToString(), results.Rows[count]["Id"].ToString(), count);
                            results.Rows[count]["GrpDesc"] = results.Rows[count][TextType].ToString().Substring(0, l).Trim(' ', ';', '.', '!');
                        }

                    }



                // Group by desc
                // grouping 
                var query = from row in results.AsEnumerable()
                        group row by row.Field<string>("GrpDesc") into ICdata
                        where ICdata.Key != null //&& ICdata.Key.Length > 0
                        orderby ICdata.Count() descending
                        select new
                        {
                            Desc = ICdata.Key,
                            Count = ICdata.Count()
                        };
            foreach (var IssuCluster in query)
            {

                //                   Debug.WriteLine("{0}\t{1}", IssuCluster.Desc, IssuCluster.Count);
                var coldet = new Integration_ReportModel();
                coldet.IncidentCount = IssuCluster.Count;

                try
                {
                    if (IssuCluster.Desc != null)
                        coldet.LongCategory = IssuCluster.Desc;


                }
                catch { }
                model.Add(coldet);
            }

            return model;
        }
            catch (InvalidOperationException ex)
            {
                Log.Error("Integration_Report Controller  FilterDataforCluster , Client ID is " + ClientId+ ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error("Integration_Report Controller  FilterDataforCluster , Client ID is " + ClientId, ex);
            }
            return null;
        }
        [HttpPost]
        public ActionResult GetIssueClusterReport(string drpval, string TicketType, string Status, string Track, string Priority, string Datetype, DateTime StartDate, DateTime EndDate, string contract, string TextType, int TextLength)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            try
            {
                int ClientId = client.ClientId;
                List <Integration_ReportModel> model = IncidentNavigatorHelper.GetIssueClusterReportData(ClientId, TicketType, Status, Track, Priority, Datetype, StartDate, EndDate, contract,  TextType,  TextLength).ToList();
                var calpercent = IncidentNavigatorHelper.calculatePercent(model);
                return Json(calpercent, JsonRequestBehavior.AllowGet);
                //(new System.Collections.Generic.Mscorlib_CollectionDebugView<WebUI.Areas.IncidentNavigator.Models.Integration_ReportModel>(model).Items[0]).LongCategory
            }
            catch (Exception ex)
            {
                Log.Error("Integration_Report Controller  GetIssueClusterReport , Client ID is " + client.ClientId, ex);
            }
            return null;

        }

        [HttpPost]
        public ActionResult GetIssueClusterReportforGrid(string drpval, string TicketType, string Status, string Track, string Priority, string Datetype, DateTime StartDate, DateTime EndDate, string contract, [DataSourceRequest] DataSourceRequest request)
        {

            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            try
            {
                string TextType = "Description"; int TextLength = 50;
                List<Integration_ReportModel> model = IncidentNavigatorHelper.GetIssueClusterReportData(ClientId, TicketType, Status, Track, Priority, Datetype, StartDate, EndDate, contract, TextType, TextLength).ToList();
                var calpercent = IncidentNavigatorHelper.calculatePercent(model);
                return Json(calpercent.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Integration_Report Controller  GetIssueClusterReportforGrid , Client ID is " + client.ClientId, ex);
            }
            return null;

        }
        [HttpPost]
        public ActionResult GetIntegrationReportDashboard(string drpval, string TicketType, string Status, string Track, string Priority, string Datetype, DateTime StartDate, DateTime EndDate, string contract)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            try
            {
                int ClientId = client.ClientId;
            List<Integration_ReportModel> model = IncidentNavigatorHelper.INIntegration_ReportChartData(ClientId, TicketType, Status, Track,Priority, Datetype, StartDate, EndDate, contract).ToList();
               var calpercent = IncidentNavigatorHelper.calculatePercent(model);
                    return Json(calpercent, JsonRequestBehavior.AllowGet);
            //(new System.Collections.Generic.Mscorlib_CollectionDebugView<WebUI.Areas.IncidentNavigator.Models.Integration_ReportModel>(model).Items[0]).LongCategory
        }
            catch (Exception ex)
            {
                Log.Error("Integration_Report Controller  GetIntegrationReportDashboard , Client ID is " + client.ClientId, ex);
            }
            return null;

        }
        
        [HttpPost]
        public ActionResult GetIntegrationReportDashboardforGrid(string drpval, string TicketType, string Status, string Track, string Priority, string Datetype, DateTime StartDate, DateTime EndDate, string contract, [DataSourceRequest] DataSourceRequest request)
        {
    
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            try
            {
                List<Integration_ReportModel> model = IncidentNavigatorHelper.INIntegration_ReportChartData(ClientId, TicketType, Status, Track, Priority, Datetype, StartDate, EndDate, contract).ToList();
               var calpercent = IncidentNavigatorHelper.calculatePercent(model);
            return Json(calpercent.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
                        catch (Exception ex)
            {
                Log.Error("Integration_Report Controller  GetIntegrationReportDashboardforGrid , Client ID is " + client.ClientId, ex);
            }
            return null;

        }
        [HttpPost]
        public ActionResult GetIntegrationReportByToken(string drpval, string TicketType, string Status, string Track, string Priority, string Datetype, DateTime StartDate, DateTime EndDate, string contract)
        {
            
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            Exp_StartDate = StartDate;
            Exp_EndDate = EndDate;

            try
            {
                List<Integration_ReportModel> model = IncidentNavigatorHelper.INIntegration_ReportTokenChartData(ClientId, TicketType, Status, Track, Priority, Datetype, StartDate, EndDate, contract, drpval).ToList();
            var calpercent = IncidentNavigatorHelper.calculatePercent(model);
            return Json(calpercent, JsonRequestBehavior.AllowGet);
                }
            catch (Exception ex)
            {
                Log.Error("Integration_Report Controller  GetIntegrationReportByToken , Client ID is " + client.ClientId, ex);
            }
            return null;

        }
        [HttpPost]
        public ActionResult GetIntegrationReportByTokenforGrid(string drpval, string TicketType, string Status, string Track, string Priority, string Datetype, DateTime StartDate, DateTime EndDate, string contract, [DataSourceRequest] DataSourceRequest request)
        {
            //DateTime StartDate = new DateTime(2021, 1, 1); DateTime EndDate = new DateTime(2021, 12, 31); string contract = "";
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            Exp_StartDate = StartDate;
            Exp_EndDate = EndDate;

            try
            {
                int ClientId = client.ClientId;
                List<Integration_ReportModel> model = IncidentNavigatorHelper.INIntegration_ReportTokenChartData(ClientId, TicketType, Status, Track, Priority, Datetype, StartDate, EndDate, contract, drpval).ToList();
                var calpercent = IncidentNavigatorHelper.calculatePercent(model);
                return Json(calpercent.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Integration_Report Controller  GetIntegrationReportByTokenforGrid , Client ID is " + client.ClientId, ex);
            }
            return null;

        }
        [HttpPost]
        public ActionResult GetIntegrationReport_ByTower(string drpval, string TicketType, string Status, string Track, string Priority, string Datetype, DateTime StartDate, DateTime EndDate, string contract)
        {

            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;

            Exp_StartDate = StartDate;
            Exp_EndDate = EndDate;
            Exp_TicketType = TicketType;
            Exp_Status = Status;
            Exp_Track = Track;
            Exp_Priority = Priority; Exp_Datetype = Datetype;
            Exp_contract = contract;

            try
            {

                List<Integration_ReportModel> model = IncidentNavigatorHelper.INIntegration_ReportTowerChartData(ClientId, TicketType, Status, Track, Priority, Datetype, StartDate, EndDate, contract).ToList();
              var calcolor = IncidentNavigatorHelper.calculateColor(model);
            return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Integration_Report Controller  GetIntegrationReport_ByTower , Client ID is " + client.ClientId, ex);
            }
            return null;
        }
        [HttpPost]
        public ActionResult GetIntegrationReport_ByTowerforGrid(string drpval, string TicketType, string Status, string Track, string Priority, string Datetype,  DateTime StartDate, DateTime EndDate, string contract, [DataSourceRequest] DataSourceRequest request)
        {
    
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            Exp_StartDate = StartDate;
            Exp_EndDate = EndDate;
            Exp_TicketType = TicketType;
            Exp_Status = Status;
            Exp_Track = Track;
            Exp_Priority = Priority; Exp_Datetype = Datetype;
            Exp_contract = contract;

            try
            {
                List<Integration_ReportModel> model = IncidentNavigatorHelper.INIntegration_ReportTowerChartData(ClientId, TicketType, Status, Track, Priority, Datetype, StartDate, EndDate, contract).ToList();
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Integration_Report Controller  GetIntegrationReport_ByTowerforGrid , Client ID is " + client.ClientId, ex);
            }
            return null;

        }

        [HttpPost]
        public ActionResult GetIntegrationReport_ByLeverForTower(string drpval, string TicketType, string Status, string Track, string Priority, string Datetype, DateTime StartDate , DateTime EndDate, string contract)
        {

            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            try
            {
            
                if (drpval.Length < 1)
                drpval = "LeverChart";

                List<Integration_ReportModel> model = IncidentNavigatorHelper.FetchIntegrationReportDataByLeverForTower(ClientId, TicketType, Status, drpval, Priority, Datetype, StartDate, EndDate, contract).ToList();
                var calpercent = IncidentNavigatorHelper.calculatePercent(model);
                return Json(calpercent, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Integration_Report Controller  GetIntegrationReport_ByLeverForTower , Client ID is " + client.ClientId, ex);
            }
            return null;
        }
        [HttpPost]
        public ActionResult GetIntegrationReport_ByLeverForTowerforGrid(string drpval, string TicketType, string Status, string Track, string Priority, string Datetype, DateTime StartDate, DateTime EndDate, string contract, [DataSourceRequest] DataSourceRequest request)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            try
            {
                List<Integration_ReportModel> model = IncidentNavigatorHelper.FetchIntegrationReportDataByLeverForTower(ClientId, TicketType, Status, drpval, Priority, Datetype, StartDate, EndDate, contract).ToList();
            var calpercent = IncidentNavigatorHelper.calculatePercent(model);
            return Json(calpercent.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Integration_Report Controller  GetIntegrationReport_ByLeverForTowerforGrid , Client ID is " + client.ClientId, ex);
            }

            return null;

        }
        [HttpPost]
        public ActionResult GetIntegrationReport_ByTowerandLeverforGrid(string drpval, string TicketType, string Status, string Track, string Priority, string Datetype, DateTime StartDate, DateTime EndDate, string contract, [DataSourceRequest] DataSourceRequest request)
        {
        
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            try { 
            List<Integration_ReportModel> model = IncidentNavigatorHelper.INIntegration_ReportTowerandLeverChartData(ClientId, TicketType, Status, Track, Priority, Datetype, StartDate, EndDate, contract, drpval).ToList();
               var calpercent = IncidentNavigatorHelper.calculatePercent(model);
            return Json(calpercent.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                //(new System.Collections.Generic.Mscorlib_CollectionDebugView<WebUI.Areas.IncidentNavigator.Models.Integration_ReportModel>(model).Items[0]).LongCategory
            }
            catch (Exception ex)
            {
                Log.Error("Integration_Report Controller  GetIntegrationReport_ByTowerandLeverforGrid , Client ID is " + client.ClientId,ex);
            }

            return null;

        }
        [ActionTrack]
        [HttpPost]
        public ActionResult GetIntegrationReport_ByTowerandLever_drilldown(string drpval, string TicketType, string Status, string Track, string Lever, string Category, string Token, string Priority, string Datetype,  DateTime StartDate, DateTime EndDate, string contract, [DataSourceRequest] DataSourceRequest request)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            List<IntegrationReportModel> model = IncidentNavigatorHelper.INIntegration_ReportTowerandLever_drilldown(ClientId, TicketType, Status, Track,Lever,  Category,  Token,Priority, Datetype, StartDate, EndDate, contract, drpval).ToList();
            //   var calpercent = IncidentNavigatorHelper.calculatePercent(model);
            return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
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

        [ActionTrack]
        [HttpPost]
        public FileStreamResult ExportClusterRecords()
        {
            // int jobid = 0;

            //if (ViewData["JobId"]!=null)
            // jobid = Convert.ToInt32(ViewData["JobId"]);
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            string fileName = "IntegrationRecords_" + ClientId.ToString() + ".xlsx";
            var dt = GetTicketData(ClientId, Exp_TicketType, Exp_Status, Exp_Track, "", "", "", Exp_Priority, Exp_Datetype, Exp_StartDate, Exp_EndDate, Exp_contract, "");
            System.IO.Stream stream = App_Start.Helper.GetXLFileStream(dt);
            //System.IO.Stream stream = App_Start.Helper.GetXLFileStream();
            stream.Position = 0;
            string mimeType = "application/vnd.ms-excel";
            var fileStreamResult = new FileStreamResult(stream, mimeType);
            fileStreamResult.FileDownloadName = fileName;
            fileStreamResult.FileStream.Seek(0, SeekOrigin.Begin);
            return fileStreamResult;
        }
        [ActionTrack]
        [HttpPost]
        public FileStreamResult ExportIRRecords()
        {
            // int jobid = 0;

            //if (ViewData["JobId"]!=null)
            // jobid = Convert.ToInt32(ViewData["JobId"]);
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            string fileName = "IntegrationRecords_" + ClientId.ToString()+ ".xlsx";
            var dt = GetTicketData(ClientId, Exp_TicketType, Exp_Status, Exp_Track, "", "", "", Exp_Priority, Exp_Datetype, Exp_StartDate, Exp_EndDate, Exp_contract, "");
            System.IO.Stream stream = App_Start.Helper.GetXLFileStream(dt);
            //System.IO.Stream stream = App_Start.Helper.GetXLFileStream();
            stream.Position = 0;
            string mimeType = "application/vnd.ms-excel";
            var fileStreamResult = new FileStreamResult(stream, mimeType);
            fileStreamResult.FileDownloadName = fileName;
            fileStreamResult.FileStream.Seek(0, SeekOrigin.Begin);
            return fileStreamResult;
        }
        private System.Data.DataTable GetTicketData(int ClientId, string TicketType, string Status, string Track, string Lever, string Category, string Token, string Priority, string Datetype, DateTime? StartDate, DateTime? EndDate, string contract, string Tower)
        {
            DataTable returntable = new DataTable();
            try
            {
                IList<IntegrationReportModel> Modellist = new List<IntegrationReportModel>();
                var paramName = new string[] { "ClientId", "TicketType", "Status", "Track", "Lever", "Category", "Token", "Priority", "Datetype", "StartDate", "EndDate", "contract" };
                var paramValue = new object[] { ClientId, TicketType, Status, Track, Lever, Category, Token, Priority, "Createddatetime", StartDate, EndDate, contract };
                //var paramValue = new object[] { ClientId, TicketType, Status, Track, Lever, Category, Token, Priority, Datetype, StartDate, EndDate, contract };
                DataTable dt = new DataTable();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    dt = unitOfWork.GetDataFromStoredProcedure("FetchIntegrationReportDataByTowerandLever_drilldown", paramName, paramValue);
                }
                return dt;
                //List<IntegrationReportModel> model = IncidentNavigatorHelper.INIntegration_ReportTowerandLever_drilldown(2518, "Incident", "", "SAP", "", "MAIL", "MAILBOX ISSUE", "", "Createddatetime", StartDate, EndDate, "138", "SAP").ToList();
            //--exec FetchIntegrationReportDataByTowerandLever_drilldown @ClientId = 2518, @TicketType = N'Incident',@Status = N'',@Track = N'SAP',@Lever = N'',@Category = N'MAIL',@Token = N'',@Priority = N'',@Datetype = N'Createddatetime',@StartDate = '',@EndDate = '',@contract = N'138'
            }
            catch (Exception ex)
            {
                Log.Error("Integration_Report Controller  ExportIntegrationReport , Client ID is " + ClientId, ex);
            }

            return returntable;
        }
        public ActionResult SaveReportDetails(DateTime FromYear, DateTime ToYear, int DateTimeGroup, int[] TypeDropDown, int[] StatusDropDown, int[] PriorityDropDown, int[] GroupDropDown, int[] TowerDropDown, int WaivedindicatorOption)
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
       //         WaiveOption = WaivedindicatorOption;
            }



            return Json(0);
        }
        [HttpPost]
        public ActionResult GetTicketsdrilldown(string TextType,int TextLength, string Datetype, string TicketType, string Status, string Track, string Priority, DateTime StartDate, DateTime EndDate, string contract, string Category, string InflowOutFlowType, [DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                int ClientId = client.ClientId;
                //List<SLAMaintenenceModel> model = IncidentNavigatorHelper.INIntegration_ReportTowerandLever_drilldown(ClientId, "", Datetype, StartDate, EndDate, TicketType, Status, Track, Priority, contract, Category, InflowOutFlowType).ToList();
                List<IntegrationReportModel> model = IncidentNavigatorHelper.INIntegration_DescClusterRepor_drilldown( ClientId, TextType, TextLength,  Status,  Track, "",  Category, "",  Priority,  Datetype,  StartDate,  EndDate,  contract,  Track).ToList();
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Dashboard controller GetTicketsdrilldown Action error 'GetTicketsdrilldown()' ", ex);
                //throw ex;
            }
            return Json(0);
        }

    }
}
