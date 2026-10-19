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
using WebUI.Areas.PortfolioAnalyzer.Models;
using WebUI.Dto;

namespace WebUI.Areas.PortfolioAnalyzer.Controllers
{
    public class AppDashboardController : Controller
    {
        //
        // GET: /PortfolioAnalyzer/AppDashboard/

        public ActionResult Index()
        {
            return View();
        }
        [ActionTrack]
        public ActionResult AppDashboardIndex()
        {
            ApplicationDashboardModel cv = new ApplicationDashboardModel();
            //Client ci = (Client)Session["Client"];
            ClientDetail ci = (ClientDetail)Session["SessionClient"];
            cv.ClientId = ci.ClientId;
            //var cv1 = APRAppDashboard(1);
            //cv1.ClientId = ci.Id;
            return View("Index", cv);
        }
        public ActionResult NewAppDashboardIndex()
        {
            ApplicationDashboardModel cv = new ApplicationDashboardModel();
            //Client ci = (Client)Session["Client"];
            ClientDetail ci = (ClientDetail)Session["SessionClient"];
            cv.ClientId = ci.ClientId;
            //var cv1 = APRAppDashboard(1);
            //cv1.ClientId = ci.Id;
            return View("New_Index", cv);
        }


        [HttpPost]
        //public PartialViewResult appData(string Id)
        public ActionResult appData(string Id)
        {
            int aId = Convert.ToInt32(Id);
            //  Client ci = (Client)Session["Client"];
            ClientDetail ci = (ClientDetail)Session["SessionClient"];
            var cv = APRAppDashboard(aId);
            cv.ClientId = ci.ClientId;
            //return PartialView("_appdashboard", cv);
            //return RedirectToAction("appDataAction", new { Id =aId});
            return Json(cv);
        }
        [HttpGet]
        public ActionResult appDataAction(int Id)
        {
            int aId = Convert.ToInt32(Id);
            //  Client ci = (Client)Session["Client"];
            ClientDetail ci = (ClientDetail)Session["SessionClient"];
            var cv = APRAppDashboard(aId);
            cv.ClientId = ci.ClientId;
            //return PartialView("_appdashboard", cv);
            return Json(cv);
        }


        [ActionTrack]
        //Helper
        public static ApplicationDashboardModel APRAppDashboard(int AppId)
        {

            var paramName = new string[] { "AppId" };
            var paramValue = new object[] { AppId };
            DataSet ds = new DataSet();
            using (var contextFactory = new DbContextFactory())
            {
                ApplicationDashboardModel appModel = new ApplicationDashboardModel();
                var unitOfWork = new Data.UnitOfWorks(contextFactory.GetContext());
                ds = unitOfWork.GetDataFromStoredProcedureDS("APRAppDashboard", paramName, paramValue);
                // var portfolioNames = ds.Tables[0];
                var appdetail = ds.Tables[0];
                //var Dispositions = ds.Tables[1];
                var Rating = ds.Tables[1];
                var Cost = ds.Tables[2];
                var Labor = ds.Tables[3];
                var personnel = ds.Tables[4];

                foreach (DataRow dr in appdetail.Rows)
                {
                    appModel.ApplicationName = (dr["ApplicationName"]).ToString();
                    appModel.AliasName = (dr["AliasName"]).ToString();
                    appModel.Analysis = (dr["Analysis"]).ToString();
                    appModel.Reccommendation = (dr["Recommendation"]).ToString();
                    try
                    {
                        appModel.AvgTickets = Convert.ToInt32(dr["AvgIncidentTickets"]);
                    }
                    catch
                    { }
                    appModel.ClientAppId = (dr["ClientAppId"]).ToString();
                    appModel.AppDescription = (dr["AppDescription"]).ToString();
                    appModel.portfolioName = (dr["portfolioName"]).ToString();
                    appModel.TechownerName = (dr["TechownerName"]).ToString();
                    appModel.Criticality = (dr["Criticality"]).ToString();
                    appModel.RevenueImpact = (dr["RevenueImpact"]).ToString();
                    appModel.CodeType = (dr["CodeType"]).ToString();
                    appModel.AppDomainName = (dr["AppDomainName"]).ToString();
                    appModel.SecurityLevel = (dr["SecurityLevel"]).ToString();
                    appModel.Architecture = (dr["Architecture"]).ToString();
                    appModel.ApplicationSize = (dr["ApplicationSize"]).ToString();
                    appModel.ApplicationClassification = (dr["ApplicationClassificationName"]).ToString();
                    appModel.ExternalHosting = (dr["HostingTypeName"]).ToString();

                    //Disposition detail
                    appModel.PlannedDisposition = (dr["PlannedDisposition"]).ToString();
                    appModel.DispositionImmediacy = (dr["DispositionImmediacy"]).ToString();

                    //dashboard
                    ////,a.UserRange,aprstd.[StandardStatus],difflvl.[DifferentiationLevel], 
                    ////			deployment.[DeploymentModel],
                    ////			a.[CountInboundInterfaces],a.[CountOutboundInterfaces],a.[EntangledInterface],
                    ////			originDisp.DispositionName as OriginalDisposition, Dis.[ReplacementAlternatives],
                    ////			rodmaptgt.[RoadmapTargetApp], dispsln.[DispositionSolution]

                    appModel.UserRange = (dr["UserRange"]).ToString();
                    appModel.StandardStatus = (dr["StandardStatus"]).ToString();
                    appModel.Differentiation = (dr["DifferentiationLevel"]).ToString();
                    appModel.DeploymentModel = (dr["DeploymentModel"]).ToString();
                    try
                    {
                        appModel.InterfacesIn = (dr["CountInboundInterfaces"]).ToString();
                    }
                    catch
                    {
                        appModel.InterfacesIn = "0";
                    }
                    try
                    {
                        appModel.InterfacesOut = (dr["CountOutboundInterfaces"]).ToString();
                    }
                    catch
                    {
                        appModel.InterfacesOut = "0";
                    }
                    try
                    {
                        appModel.InterfacesEntangled = (dr["EntangledInterface"]).ToString();
                    }
                    catch
                    {
                        appModel.InterfacesEntangled = "False";
                    }
                    appModel.InitialDisposition = (dr["OriginalDisposition"]).ToString();
                    appModel.APRDisposition = (dr["PlannedDisposition"]).ToString();
                    appModel.DispositionSolution = (dr["DispositionSolution"]).ToString();
                    appModel.ReplacementTarget = (dr["RoadmapTargetApp"]).ToString();
                    appModel.ReplacementAlternatives = (dr["ReplacementAlternatives"]).ToString();
                    appModel.SystemPriority = appModel.Criticality;

                    try
                    {
                        appModel.IncludeAlternativeDashboard = (int)dr["IncludeAlternativeDashboard"];
                    }
                    catch
                    {
                        appModel.IncludeAlternativeDashboard = 0;

                    }
                    try
                    {
                        appModel.RetirementDate = (dr["RetirementDate"]).ToString();
                    }
                    catch { }
                }

                appModel.RatingSummary = new List<_ratingSummary>();
                foreach (DataRow dr in Rating.Rows)
                {
                    var ratingSummary = new _ratingSummary();
                    ratingSummary.PerspectiveName = (dr["PerspectiveName"]).ToString();
                    try
                    {
                        ratingSummary.Rating = Convert.ToInt32(dr["Rating"]);
                    }
                    catch { }
                    try
                    {
                        ratingSummary.PerspectiveId = Convert.ToInt32(dr["PerspectiveId"]);
                    }
                    catch { }

                    appModel.RatingSummary.Add(ratingSummary);
                }
                appModel.CostSummary = new List<_costSummary>();
                foreach (DataRow dr in Cost.Rows)
                {
                    var costSummary = new _costSummary();
                    costSummary.CostType = "Discretionary";
                    try
                    {
                        costSummary.amount = Convert.ToDecimal(dr["Discretionary"]);
                    }
                    catch { }
                    appModel.CostSummary.Add(costSummary);
                    var costSummary2 = new _costSummary();
                    costSummary2.CostType = "Enhancement";
                    try
                    {
                        costSummary2.amount = Convert.ToDecimal(dr["Enhancement"]);
                    }
                    catch { }
                    appModel.CostSummary.Add(costSummary2);
                    var costSummary3 = new _costSummary();
                    costSummary3.CostType = "Support";
                    try
                    {
                        costSummary3.amount = Convert.ToDecimal(dr["Support"]);
                    }
                    catch { }
                    appModel.CostSummary.Add(costSummary3);
                    var costSummary4 = new _costSummary();
                    costSummary4.CostType = "Licensing";
                    try
                    {
                        costSummary4.amount = Convert.ToDecimal(dr["Licensing"]);
                    }
                    catch { }
                    appModel.CostSummary.Add(costSummary4);
                    var costSummary5 = new _costSummary();
                    costSummary5.CostType = "Infrastructure";
                    try
                    {
                        costSummary5.amount = (dr["Infrastructure"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(dr["Infrastructure"]));
                    }
                    catch { }
                    appModel.CostSummary.Add(costSummary5);
                }
                appModel.Labor = new List<_labor>();
                foreach (DataRow dr in Labor.Rows)
                {
                    var labor = new _labor();
                    labor.WorkType = "Business";
                    try
                    {
                        labor.InternalLabor = Convert.ToDecimal(dr["BLaborInternal"]);
                        labor.ExternalLabor = Convert.ToDecimal(dr["BLaborExternal"]);
                    }
                    catch { }
                    appModel.Labor.Add(labor);

                    var labor2 = new _labor();
                    labor2.WorkType = "Discretionary";
                    try
                    {
                        labor2.InternalLabor = Convert.ToDecimal(dr["DLaborInternal"]);
                        labor2.ExternalLabor = Convert.ToDecimal(dr["DLaborExternal"]);
                    }
                    catch { }
                    appModel.Labor.Add(labor2);
                    var labor3 = new _labor();
                    labor3.WorkType = "Enhancement";
                    try
                    {
                        labor3.InternalLabor = Convert.ToDecimal(dr["ELaborInternal"]);
                        labor3.ExternalLabor = Convert.ToDecimal(dr["ELaborExternal"]);
                    }
                    catch { }
                    appModel.Labor.Add(labor3);
                    var labor4 = new _labor();
                    labor4.WorkType = "Support";
                    try
                    {
                        labor4.InternalLabor = Convert.ToDecimal(dr["SlaborInternal"]);
                        labor4.ExternalLabor = Convert.ToDecimal(dr["SLaborExternal"]);
                    }
                    catch { }
                    appModel.Labor.Add(labor4);
                    var labor5 = new _labor();
                    labor5.WorkType = "Other IT";
                    try
                    {
                        labor5.InternalLabor = Convert.ToDecimal(dr["OLaborInternal"]);
                        labor5.ExternalLabor = Convert.ToDecimal(dr["OLaborExternal"]);
                    }
                    catch { }
                    appModel.Labor.Add(labor5);
                }
                appModel.OwnerShipType = new List<_ownershipType>();
                foreach (DataRow dr in personnel.Rows)
                {
                    var ownershipType = new _ownershipType();
                    try
                    {
                        ownershipType.Name = (dr["FirstName"]).ToString() + (dr["LastName"]).ToString();
                        ownershipType.Email = (dr["EmailAddress"]).ToString();
                        ownershipType.Title = (dr["Title"]).ToString();
                        ownershipType.IsSurveyParticipant = (dr["OwnershipType"]).ToString();
                    }
                    catch { }
                    appModel.OwnerShipType.Add(ownershipType);
                }

                //Survey Answers

                Db db = new Db();
                var RatingUser = unitOfWork.Personnel.Get(g => g.PersonnelName.ToLower() == "aprratinguser").FirstOrDefault();
                if (RatingUser != null)
                {
                    //Get latest SurveyUserId for particular Application Id to records from SurveyCollection Table
                    var surveyUserID = unitOfWork.SurveyUser.Get(g => g.SurveyForRefId == AppId && g.PersonnelId == RatingUser.Id)
                                                    .OrderByDescending(f => f.Createddt)
                                                    .FirstOrDefault();
                    if (surveyUserID != null)
                    {
                        var Ratcollection = (from sc in db.SurveyCollection
                                             join sm in db.SurveyModel on sc.SurveyModelId equals sm.Id
                                             join smr in db.SurveyModelRatingFactor on sc.FinalRating equals smr.Id
                                             where sc.SurveyUserId == surveyUserID.Id
                                             select new _surveyRating
                                             {
                                                 Answer = smr.RatingFactor == null ? "" : smr.RatingFactor,
                                                 Question = sm.Question == null ? "" : sm.Question,
                                                 answerValue = smr.RatingFactorValue.Value
                                             }
                                          ).AsEnumerable();
                        if (Ratcollection != null)
                        {
                            appModel.surveyRating = Ratcollection.ToList();
                        }
                    }
                }

                return appModel;

            }

        }
        private class stringSelectList
        {
            public string Text { get; set; }
            public int Value { get; set; }
        }
        public ActionResult getSurveyForApp(int ApplId = 0)
        {
            //var Cid = (Client)Session["Client"];
            var Cid = (ClientDetail)Session["SessionClient"];
            Db db = new Db();
            var somedat = (from su in db.SurveyUser
                           join app in db.Applications on su.SurveyForRefId equals app.Id
                           join s in db.Survey on su.SurveyId equals s.Id
                           where su.SurveyForRefId == ApplId && s.ClientId == Cid.ClientId
                           select s).Distinct().OrderBy(x => x.Name).ToList();
            var modelSelectList = somedat.Select(e => new SelectListItem
            {
                Text = e.Name,
                Value = e.Id.ToString()
            }).AsEnumerable();


            return Json(modelSelectList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getYears(int ApplId = 0)
        {
            List<SelectListItem> modelSelectList = new List<SelectListItem>();
            var thisyear = DateTime.Now.Year + 1;
            int i = 8;
            while (i >= 0)
            {
                SelectListItem oneItem = new SelectListItem();
                oneItem.Text = (thisyear - i).ToString();
                oneItem.Value = oneItem.Text;
                modelSelectList.Add(oneItem);
                i--;
            }
            return Json(modelSelectList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getAppYears(int ApplId = 0)
        {
            //var Cid = (Client)Session["Client"];
            var Cid = (ClientDetail)Session["SessionClient"];
            Db db = new Db();
            var YearList = (from Af in db.APRFinancial
                            where Af.AppId == ApplId
                            select Af).Distinct().OrderBy(x => x.Year).ToList();
            var modelSelectList = YearList.Select(e => new SelectListItem
            {
                Text = e.Year.ToString(),
                Value = e.Year.ToString()
            }).AsEnumerable();
            return Json(modelSelectList, JsonRequestBehavior.AllowGet);
        }

        private class bufferdata
        {
            public List<_labor> laborModel { get; set; }
            public List<_costSummary> costModel { get; set; }
        }
        [HttpPost]
        public ActionResult applicationFTECost(int AppId, int Year = 0)
        {
            List<_labor> laborModel1 = new List<_labor>();
            List<_costSummary> costModel1 = new List<_costSummary>();
            bufferdata buffer = new bufferdata();
            var paramName = new string[] { "AppId", "year" };
            var paramValue = new object[] { AppId, Year };
            //DataSet ds = new DataSet();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new Data.UnitOfWorks(contextFactory.GetContext());
                var ds = unitOfWork.GetDataFromStoredProcedure("APRAppDashboardLabor", paramName, paramValue);

                foreach (DataRow dr in ds.Rows)
                {
                    var labor = new _labor();
                    labor.WorkType = "Business";
                    try
                    {
                        labor.InternalLabor = Convert.ToDecimal(dr["BLaborInternal"]);
                        labor.ExternalLabor = Convert.ToDecimal(dr["BLaborExternal"]);
                    }
                    catch { }
                    laborModel1.Add(labor);


                    var labor2 = new _labor();
                    labor2.WorkType = "Discretionary";
                    try
                    {
                        labor2.InternalLabor = Convert.ToDecimal(dr["DLaborInternal"]);
                        labor2.ExternalLabor = Convert.ToDecimal(dr["DLaborExternal"]);
                    }
                    catch { }
                    laborModel1.Add(labor2);

                    var labor3 = new _labor();
                    labor3.WorkType = "Enhancement";
                    try
                    {
                        labor3.InternalLabor = Convert.ToDecimal(dr["ELaborInternal"]);
                        labor3.ExternalLabor = Convert.ToDecimal(dr["ELaborExternal"]);
                    }
                    catch { }
                    laborModel1.Add(labor3);

                    var labor4 = new _labor();
                    labor4.WorkType = "Support";
                    try
                    {
                        labor4.InternalLabor = Convert.ToDecimal(dr["SlaborInternal"]);
                        labor4.ExternalLabor = Convert.ToDecimal(dr["SLaborExternal"]);
                    }
                    catch { }
                    laborModel1.Add(labor4);

                    var labor5 = new _labor();
                    labor5.WorkType = "Other IT";
                    try
                    {
                        labor5.InternalLabor = Convert.ToDecimal(dr["OLaborInternal"]);
                        labor5.ExternalLabor = Convert.ToDecimal(dr["OLaborExternal"]);
                    }
                    catch { }
                    laborModel1.Add(labor5);

                }


                var ds2 = unitOfWork.GetDataFromStoredProcedure("APRAppDashboardCost", paramName, paramValue);
                foreach (DataRow dr in ds2.Rows)
                {
                    var costSummary = new _costSummary();
                    costSummary.CostType = "Discretionary";
                    try
                    {
                        costSummary.amount = Convert.ToDecimal(dr["Discretionary"]);
                        costSummary.color = "#76448A";
                    }
                    catch { }
                    if (costSummary.amount != 0)
                    {
                        costModel1.Add(costSummary);
                    }

                    var costSummary2 = new _costSummary();
                    costSummary2.CostType = "Enhancement";
                    try
                    {
                        costSummary2.amount = Convert.ToDecimal(dr["Enhancement"]);
                        costSummary2.color = "#F1C40F";
                    }
                    catch { }
                    if (costSummary2.amount != 0)
                    {
                        costModel1.Add(costSummary2);
                    }

                    var costSummary3 = new _costSummary();
                    costSummary3.CostType = "Support";
                    try
                    {
                        costSummary3.amount = Convert.ToDecimal(dr["Support"]);
                        costSummary3.color = "#2E86C1";
                    }
                    catch { }
                    if (costSummary3.amount != 0)
                    {
                        costModel1.Add(costSummary3);
                    }
                    var costSummary4 = new _costSummary();
                    costSummary4.CostType = "Licensing";
                    try
                    {
                        costSummary4.amount = Convert.ToDecimal(dr["Licensing"]);
                        costSummary4.color = "#D98880";
                    }
                    catch { }
                    if (costSummary4.amount != 0)
                    {
                        costModel1.Add(costSummary4);
                    }

                    var costSummary5 = new _costSummary();
                    costSummary5.CostType = "Infrastructure";
                    try
                    {
                        costSummary5.amount = (dr["Infrastructure"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(dr["Infrastructure"]));
                        costSummary5.color = "#0B5345";
                    }
                    catch { }
                    if (costSummary5.amount != 0)
                    {
                        costModel1.Add(costSummary5);
                    }

                    var costSummary6 = new _costSummary();
                    costSummary6.CostType = "Other";
                    try
                    {
                        costSummary6.amount = (dr["Other"] == DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(dr["Other"]));
                        costSummary6.color = "#03a9f4";
                    }
                    catch { }
                    if (costSummary6.amount != 0)
                    {
                        costModel1.Add(costSummary6);
                    }
                }
            }
            buffer.costModel = costModel1;
            buffer.laborModel = laborModel1;
            //return Json(new{ costModel1, laborModel1}, JsonRequestBehavior.AllowGet);
            return Json(buffer, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AppSurveyRating(int ApplicationID, int surId = 0)
        {
            List<_ratingSummary> ratingModel = new List<_ratingSummary>();
            var paramName = new string[] { "AppId", "SurveyId" };
            var paramValue = new object[] { ApplicationID, surId };
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new Data.UnitOfWorks(contextFactory.GetContext());
                var ds = unitOfWork.GetDataFromStoredProcedure("APRAppDashboardRating", paramName, paramValue);

                foreach (DataRow dr in ds.Rows)
                {
                    var ratingSummary = new _ratingSummary();
                    ratingSummary.PerspectiveName = (dr["PerspectiveName"]).ToString();
                    try
                    {
                        ratingSummary.Rating = Convert.ToDecimal(dr["Rating"]);
                    }
                    catch { }
                    try
                    {
                        ratingSummary.PerspectiveId = Convert.ToInt32(dr["PerspectiveId"]);
                    }
                    catch { }

                    ratingModel.Add(ratingSummary);
                }
            }
            return Json(new { ratingModel }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PerspectiveTypeRating(int SurveyId = 0, int PerspectiveId = 0, int AppId = 0)
        {
            var ratingList = new List<PerspectiveTypeRatingSummary>();
            if (AppId != 0 && PerspectiveId != 0)
            {
                ratingList = PortfolioAnalyzerHelper.perspectiveTypeRating(SurveyId, PerspectiveId, AppId);
            }
            return Json(ratingList.AsEnumerable(), JsonRequestBehavior.AllowGet);
        }



    }
}
