using Data;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Filters;
using System;
using System.Collections.Generic;
using System.Data;

using System.Linq;
using System.Web;
using System.Web.Mvc;
//using Newtonsoft..JSON;
using WebUI.Areas.PortfolioAnalyzer.Models;
using WebUI.Dto;

namespace WebUI.Areas.PortfolioAnalyzer.Controllers
{
    public class DashboardController : Controller
    {
        //
        // GET: /PortfolioAnalyzer/Dashboard/
        [ActionTrack]
        public ActionResult Index(DashboardModel c)
        {
            //DashboardModel c = new DashboardModel();
            return View(c);
            // return View();
        }

        [ActionTrack]
        public ActionResult Landing(DashboardModel c)
        {
            //DashboardModel newC = new DashboardModel();
            return View(c);
            // return View();
        }

        [ActionTrack]
        public ActionResult RatingBySubPerspectiveReport(RatingBySubperspective c)
        {
            //using (var contextFactory = new DbContextFactory())
            //{
            //    var client = (Client)Session["Client"];
            //    int ClientId = client.Id;
            //   RatingBySubperspective perspectivelist = new RatingBySubperspective();
            //    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            //    var list = unitOfWork.SurveyPerspective.Get(x => x.ClientId == ClientId).Select(x => x.PerspectiveType).ToList();



            //    if (list != null)
            //    {

            //        c.PerspectiveTypelist = list.ToList();
            //    }


            //}
            return View(c);

        }

        [ActionTrack]
        public ActionResult DispositionRoadMapReport()
        {

            return View();

        }



        [ActionTrack]
        [HttpPost]
        public ActionResult StackedGraph(string PortfolioName = "ALL", string PerspectiveType = "ALL", string SurveyName = "ALL", string Hints = "ALL")
        {
            // Client cl = (Client)Session["Client"];
            ClientDetail cl = (ClientDetail)Session["SessionClient"];
            var u = JsonConvert.DeserializeObject<IList<string>>(HttpUtility.UrlDecode(PerspectiveType));
            var v = JsonConvert.DeserializeObject<IList<string>>(HttpUtility.UrlDecode(Hints));
            string PType = string.Join(",", u);
            string HType = string.Join(",", v);
            if (PortfolioName.ToUpper() == "ALL")
            {
                PortfolioName = "0";
            }
            if (PerspectiveType.ToUpper() == "ALL" || PerspectiveType == "")
            {
                PerspectiveType = "0";
            }
            if (SurveyName.ToUpper() == "ALL" || SurveyName == "")
            {
                SurveyName = "0";
            }
            if (Hints.ToUpper() == "ALL" || Hints == "")
            {
                Hints = "0";
            }
            var model = APRStackedGraph(cl.ClientId, PortfolioName, PType, SurveyName, HType);

            return Json(model, JsonRequestBehavior.AllowGet);
        }


        public static List<AprStackedGraphModel> APRStackedGraph(int clientId, string portfolioName, string perspectivetype, string surveyname, string hints)
        {

            var paramName = new string[] { "SurveyId", "modelIds", "LOBIds", "perspectiveid" };
            //  Conversion Required!
            var sid = 0;

            if (surveyname == "0")
            {
                sid = 0;
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork1 = new UnitOfWorks(contextFactory.GetContext());
                    var ss = unitOfWork1.Survey.Get(g => g.ClientId == clientId).FirstOrDefault();
                    if (ss != null)
                    {
                        sid = ss.Id;
                    }
                }
            }
            else { sid = Convert.ToInt32(surveyname); }


            var paramValue = new object[] { sid, hints, portfolioName, perspectivetype };
            //DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                List<AprStackedGraphModel> appModel1 = new List<AprStackedGraphModel>();
                var unitOfWork = new Data.UnitOfWorks(contextFactory.GetContext());
                var dt = unitOfWork.GetDataFromStoredProcedure("APRPortFolioRatingSummary", paramName, paramValue);
                // var portfolioNames = ds.Tables[0];
                // var appdetail = ds;
                foreach (DataRow dr in dt.Rows)
                {
                    AprStackedGraphModel appModel = new AprStackedGraphModel();
                    appModel.Hint = (dr["Hint"]).ToString();

                    try
                    {
                        appModel.QId = Convert.ToInt32(dr["Id"]);
                    }
                    catch
                    { }
                    try
                    {
                        appModel.PoorRatingCount = Convert.ToInt32(dr["Poorrating"]);
                    }
                    catch
                    { }
                    try
                    {
                        appModel.AvgRatingCount = Convert.ToInt32(dr["Avgrating"]);
                    }
                    catch
                    { }
                    try
                    {
                        appModel.GoodRatingCount = Convert.ToInt32(dr["Goodrating"]);
                    }
                    catch
                    { }
                    appModel1.Add(appModel);
                }
                return appModel1;
            }

        }


        // Helper Methods for HeaderFilters
        public IEnumerable<SelectListItem> portfolioNameList(int ClientID)
        {
            //Db db = new Db();
            //var item = (from Lob in db.LineOfBusiness
            //            where Lob.ClientId == ClientID
            //            select new SelectListItem { Value = Lob.Id.ToString(), Text = Lob.LOBName, Selected = true }).Distinct().ToList();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var item1 = unitOfWork.LineOfBusinesses.Get(g => g.ClientId == ClientID);
                var item = item1.OrderBy(x => x.LOBName).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.LOBName }).ToList();
                if (item.Count > 0)
                {
                    item.Add(new SelectListItem { Text = "All", Value = "All" });
                }
                return item;
            }

        }

        public IEnumerable<SelectListItem> perspectiveTypeList(int ClientID)
        {
            //Db db = new Db();
            //var TypeList = (from pers in db.SurveyPerspective
            //                where pers.ClientId == ClientID
            //                select new SelectListItem { Value = pers.Id.ToString(), Text = pers.PerspectiveName }).Distinct().ToList();
            //if (TypeList.Count > 0)
            //{
            //    TypeList.Add(new SelectListItem { Text = "All", Value = "All" });
            //}
            //return TypeList;
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var item1 = unitOfWork.SurveyPerspective.Get(g => g.ClientId == ClientID);
                var item = item1.OrderBy(x => x.PerspectiveType).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.PerspectiveType }).ToList();
                //if (item.Count > 0)
                //{
                //    item.Add(new SelectListItem { Text = "All", Value = "All" });
                //}
                return item;
            }
        }

        public IEnumerable<SelectListItem> surveyList(int ClientID)
        {

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var item1 = unitOfWork.Survey.Get(g => g.ClientId == ClientID && (g.SurveyType.ToUpper().Trim() == "RATING" || g.SurveyType.ToUpper().Trim() == "APR"
                                                    || g.SurveyType.ToUpper().Trim() == "APR2"   || g.SurveyType.ToUpper().Trim() == "DYNAMIC"));
                var item = item1.OrderBy(x => x.Name).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();

                return item;
            }
        }


        public JsonResult GetPerspectiveTypelist(string surveyId)
        {
            //var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            RatingBySubperspective model = PortfolioAnalyzerHelper.APRGetPerspectiveTypesBySurvey(ClientId.ToString(), surveyId);
            List<string> plist = model.PerspectiveTypelist;
            var item = plist.Select(p => new SelectListItem { Value = p.ToString(), Text = p.ToString() }).ToList();
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        public JsonResult hintList(string surveyId)
        {
            //Db db = new Db();
            var sid = Convert.ToInt32(surveyId);
            //var HintList = (from sQ in db.SurveyModel
            //                where sQ.SurveyId == sid
            //                select new SelectListItem { Value = sQ.Id.ToString(), Text = sQ.Hint }).Distinct().ToList();
            //if (HintList.Count > 0)
            //{
            //    HintList.Add(new SelectListItem { Text = "All", Value = "All" });
            //}
            //return HintList;
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var item1 = unitOfWork.SurveyModel.Get(g => g.SurveyId == sid);
                var item = item1.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Hint }).ToList();
                //if (item.Count > 0)
                //{
                //    item.Add(new SelectListItem { Text = "All", Value = "All" });
                //}
                return Json(item, JsonRequestBehavior.AllowGet);
            }
        }
        [ActionTrack]
        [HttpPost]
        public ActionResult UnassignedApps(string surveyId)
        {
            //Client cl = (Client)Session["Client"];
            ClientDetail cl = (ClientDetail)Session["SessionClient"];
            var Count = 0;
            var sid = Convert.ToInt32(surveyId);

            try
            {
                Db db = new Db();
                var SurRefIDs = (from su in db.SurveyUser where su.SurveyId == sid select su.SurveyForRefId).ToList();
                var apps = (from s in db.Applications
                            where s.ClientId == cl.ClientId && !SurRefIDs.Contains(s.Id)
                            select s.Id).Count();
                Count = apps;
            }
            catch { }
            return Json(Count);
        }
        public ActionResult ExAppIndex()
        {
            Application app = new Application();
            return View("ExAppIndex", app);
        }
        [HttpPost]
        public ActionResult ExAppInventoryData([DataSourceRequest] DataSourceRequest request)
        {
            // var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            Db db = new Db();
            var data = (from app in db.Applications
                        join aprc in db.APRAppCriticality on app.AppCriticalityId equals aprc.Id into TempCritically
                        join aprtype in db.APRAppType on app.AppTypeId equals aprtype.Id into TempType
                        join aprdomain in db.APRDomain on app.AppDomainId equals aprdomain.Id into TempDomain
                        join aprarch in db.APRAppArchitecture on app.AppArchitectureId equals aprarch.Id into TempArchitecture
                        join aprpro in db.APRAppProcessingType on app.AppProcessingId equals aprpro.Id into TempProcess
                        join aprsrc in db.APRAppSourceCodeType on app.AppSourceCodeId equals aprsrc.Id into TempSource
                        join aprdeploy in db.APRDeploymentModel on app.APRDeploymentModelId equals aprdeploy.Id into TempDeploy
                        join aprstd in db.APRStandardStatus on app.APRStandardStatusId equals aprstd.Id into TempStandard
                        join aprdiff in db.APRDifferentiationLevel on app.APRDifferentiationLevelId equals aprdiff.Id into TempDifferential
                        join apr in db.APRAppDisposition on app.Id equals apr.AppId into TempData
                        join appli in db.ApplicationClassification on app.ApplicationClassificationId equals appli.Id into TempAppClass
                        join exte in db.ExternalHosting on app.HostingTypeId equals exte.Id into TempExtHost
                        //join aprcountry in db.APRApplicationCountry on app.Id equals aprcountry.AppId into Tempcountry
                        where app.ClientId == client.ClientId
                        from AprData in TempData.DefaultIfEmpty()
                        from AppClassData in TempAppClass.DefaultIfEmpty()
                        from ExtHostData in TempExtHost.DefaultIfEmpty()
                        from Critical in TempCritically.DefaultIfEmpty()
                        from Type in TempType.DefaultIfEmpty()
                        from Domain in TempDomain.DefaultIfEmpty()
                        from Architecture in TempArchitecture.DefaultIfEmpty()
                        from Process in TempProcess.DefaultIfEmpty()
                        from Source in TempSource.DefaultIfEmpty()
                        from Deployment in TempDeploy.DefaultIfEmpty()
                        from StandardStatus in TempStandard.DefaultIfEmpty()
                        from Differentiation in TempDifferential.DefaultIfEmpty()
                            //from Countries in Tempcountry.ToList()
                        select new ExAppInventoryDataModel
                        {
                            AliasName = app.AliasName,
                            Analysis = AprData.Analysis,
                            ApplicationName = app.ApplicationName,
                            AppSize = app.AppSize,
                            AvgConcurrentUsers = app.AvgConcurrentUsers,
                            AppId = AprData.AppId,
                            AvgIncidentTickets = app.AvgIncidentTickets,
                            AvgProblemTickets = app.AvgProblemTickets,
                            AvgServiceRequestTickets = app.AvgServiceRequestTickets,
                            BusinessDispositionCost = AprData.BusinessDispositionCost,
                            BusinessDispositionRate = AprData.BusinessDispositionRate,
                            BusinessDispositionEffort = AprData.BusinessDispositionEffort,
                            BusinessDisSupportFTEs = AprData.BusinessDisSupportFTEs,
                            BusinessFunction = app.BusinessFunction,
                            BusinessLogicInClientTier = app.BusinessLogicInClientTier,
                            ClientApplicationNumber = app.ClientApplicationNumber,
                            CodeComplexity = app.CodeComplexity,
                            Description = app.Description,
                            DispositionDuration = AprData.DispositionDuration,
                            DispositionEfforts = AprData.DispositionEfforts,
                            DocumentationNotes = app.DocumentationNotes,
                            FinancialNotes = app.FinancialNotes,
                            FTEImpact = AprData.FTEImpact,
                            GeneralNotes = app.GeneralNotes,
                            HardwareDisAmountofCPUFreed = AprData.HardwareDisAmountofCPUFreed,
                            HardwareDisSupportSavings = AprData.HardwareDisSupportSavings,
                            IsApplicationDocAvailable = app.IsApplicationDocAvailable,
                            IsDisasterRecoveryPlanAvailable = app.IsDisasterRecoveryPlanAvailable,
                            ExternalHosting = ExtHostData.HostingTypeName,
                            ApplicationClassification = AppClassData.ApplicationClassificationName,
                            IsTechnicalDocAvailable = app.IsTechnicalDocAvailable,
                            ITDispositionCost = AprData.ITDispositionCost,
                            ITDispositionEffort = AprData.ITDispositionEffort,
                            ITDispositionRate = AprData.ITDispositionRate,
                            ITDispostionDevFTEs = AprData.ITDispostionDevFTEs,
                            ITDispostionSupportFTEs = AprData.ITDispostionSupportFTEs,
                            LegacyReasonExists = app.LegacyReasonExists,
                            MaxConcurrentUsers = app.MaxConcurrentUsers,
                            MigrationDate = app.MigrationDate,
                            MOTSLevel = app.MOTSLevel,
                            NumberOfBugs = app.NumberOfBugs,
                            NumberOfInstances = app.NumberOfInstances,
                            NumberOfLinesOfCode = app.NumberOfLinesOfCode,
                            OperationalEfficiencyAmount = AprData.OperationalEfficiencyAmount,
                            OperationalEfficiencyUnits = AprData.OperationalEfficiencyUnits,
                            OriginalRetirementDate = AprData.OriginalRetirementDate,
                            OutageDuration = app.OutageDuration,
                            OutageFrequency = app.OutageFrequency,
                            OwningCompanies = app.APROwningCompanyId,
                            PerformanceNotes = app.PerformanceNotes,
                            PlannedDispositionDate = AprData.PlannedDispositionDate,
                            PlannedRetirementDate = app.PlannedRetirementDate,
                            Recommendation = AprData.Recommendation,
                            RegionNames = app.RegionNames,
                            RevenueImpactIfDown = app.RevenueImpactIfDown,
                            Risk = AprData.Risk,
                            //RoadmapTargetApp = AprData.RoadmapTargetApp,
                            APRRoadmapTargetAppId = AprData.APRRoadmapTargetAppId,
                            RoadmapTargetYear = AprData.RoadmapTargetYear,
                            SizeUnits = app.SizeUnits,
                            TargetedPlatformCapability = AprData.TargetedPlatformCapability,
                            TCO = AprData.TCO,
                            TechnicalOwner = app.TechnicalOwner,
                            TechnicalOwnerEmail = app.TechnicalOwnerEmail,
                            TransactionVolumn = app.TransactionVolumn,
                            TransactionVolumnUnit = app.TransactionVolumnUnit,
                            Value = AprData.Value,
                            VendorDisCommercialLicenceSavings = AprData.VendorDisCommercialLicenceSavings,
                            VendorDispositionCost = AprData.VendorDispositionCost,
                            VendorDispositionEffort = AprData.VendorDispositionEffort,
                            VendorDispositionRate = AprData.VendorDispositionRate,
                            VendorDisSupportSavings = AprData.VendorDisSupportSavings,
                            AppCriticality = Critical.CriticalityName,
                            AppType = Type.AppType,
                            Domain = Domain.AppDomainName,
                            ArchitectureType = Architecture.ArchitectureType,
                            Processing = Process.ProcessingType,
                            SourceType = Source.SourcecodeType,
                            UserRange = app.UserRange,
                            DeploymentModel = Deployment.DeploymentModel,
                            StandardStatus = StandardStatus.StandardStatus,
                            DifferentiationLevel = Differentiation.DifferentiationLevel,
                            CountInboundInterfaces = app.CountInboundInterfaces,
                            CountOutboundInterfaces = app.CountOutboundInterfaces,
                            EntangledInterface = app.EntangledInterface,
                            //countriesId = Countries.Id,

                        }
                        );
            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

       [HttpPost]
        public ActionResult GetAppsdrilldown(string ChartType, string Category, [DataSourceRequest] DataSourceRequest request)
        {
            var client = (ClientDetail)Session["SessionClient"];
            var data = PortfolioAnalyzerHelper.getDrillDownApps(client.ClientId, ChartType, Category);
            return Json(data.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }


        public IEnumerable<TreeViewItemModel> GetPortfoliolistbyClient(int ClientID)
        {
            List<TreeViewItemModel> inlineDefault = new List<TreeViewItemModel>();

            using (var contextFactory = new DbContextFactory())
            {

                RatingBySubperspective perspectivelist = new RatingBySubperspective();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var list = unitOfWork.LineOfBusinesses.Get(x => x.ClientId == ClientID).OrderBy(x => x.LOBName).ToList();



                //var clientList = from cl in clients.GetAll().ToList()
                //                 where cl.IsDeleted == false
                //                 select new { Value = cl.Id, Text = cl.ClientName };
                inlineDefault = list.Select(lst => new TreeViewItemModel
                {
                    Id = lst.Id.ToString(),
                    Text = lst.LOBName.ToString()


                }).GroupBy(g => g.Id, (key, group) => group.First()).ToList();


                return inlineDefault;

            }
        }

        public IEnumerable<TreeViewItemModel> GetAppsByPortfolio(string portfolios)
        {
            List<TreeViewItemModel> inlineDefault = new List<TreeViewItemModel>();
            List<RatingBySubperspective> list = PortfolioAnalyzerHelper.APRGetAppsForPortfoli(portfolios).OrderBy(x => x.ApplicationName).ToList();
            using (var contextFactory = new DbContextFactory())
            {

                RatingBySubperspective perspectivelist = new RatingBySubperspective();



                //var clientList = from cl in clients.GetAll().ToList()
                //                 where cl.IsDeleted == false
                //                 select new { Value = cl.Id, Text = cl.ClientName };
                inlineDefault = list.Select(lst => new TreeViewItemModel
                {
                    Id = lst.AppId.ToString(),
                    Text = lst.ApplicationName.ToString()


                }).GroupBy(g => g.Id, (key, group) => group.First()).ToList();


                return inlineDefault;

            }
        }

    }
}
