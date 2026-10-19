using Data;
using System.Data;
using Domain.Abstract;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NTTData.AMOPortal.WebUI.Mapper;
using NTTData.AMOPortal.Domain.Service;
using System.IO;
using System.Text;
using Domain.Entities.PortfolioAnalyzer;
using WebUI.Areas.PortfolioAnalyzer.Dto;
using WebUI.Areas.PortfolioAnalyzer.Models;
using System.Web.Http;
using PagedList;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Domain.Entities;
using WebUI.Areas.RetirementAccelerator.Models;
using Domain.Entities.Survey;

namespace WebUI.Areas.PortfolioAnalyzer
{
    public class PortfolioAnalyzerHelper
    {
        public static IPagedList<ApplicationInformationModel> GetApplicationInfo(int? ClientId, int? page)
        {

            if (ClientId != null)
            {
                var db = new Db();
                var appList = (from app in db.Applications
                               where app.ClientId == ClientId
                               select new ApplicationInformationModel
                               {
                                   Id = app.Id,
                                   ClientApplicationNumber = app.ClientApplicationNumber,
                                   AliasName = app.AliasName,
                                   ClientId = ClientId,
                                   ApplicationName = app.ApplicationName,
                                   TechnicalOwner = app.TechnicalOwner,
                                   InScope = app.InScope
                               }).OrderBy(o => o.Id).ToPagedList(page ?? 1, 9);
                return appList;
            }
            else
            {
                return new List<ApplicationInformationModel>().ToPagedList(1, 9);
            }
        }





        public static ApplicationInformationModel GetApplicationInfo1(int ApplicationID)
        {


            using (var contextFactory = new DbContextFactory())
            {

                var entityMapper = new Mapper<ApplicationInformationModel, Application>();


                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var ApplicationInfo = unitOfWork.Application.GetByID(ApplicationID);
                var applicationInformationModel = new ApplicationInformationModel();
                entityMapper.MapToEntity(ApplicationInfo, applicationInformationModel);
                // AppViewModels.Add(ApplicationInformationModel);
                return applicationInformationModel;


            }



        }


        public static bool AddAppInfo(ApplicationInformationModel AppInfoinput)
        {
            var entityMapper = new Mapper<Application, ApplicationInformationModel>();
            try
            {
                var mappingTemplate = new Application();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    entityMapper.MapToEntity(AppInfoinput, mappingTemplate);
                    unitOfWork.Application.Insert(mappingTemplate);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool UpdateAppInfoBasic(ApplicationInformationModel AppInfoinput)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var appinfo = unitOfWork.Application.GetByID(AppInfoinput.Id);
                    appinfo.ApplicationName = AppInfoinput.ApplicationName;
                    appinfo.ClientApplicationNumber = AppInfoinput.ClientApplicationNumber;
                    appinfo.AliasName = AppInfoinput.AliasName;
                    appinfo.TechnicalOwner = AppInfoinput.TechnicalOwner;
                    appinfo.IsRetirementCandidate = AppInfoinput.IsRetirementCandidate;
                    appinfo.InScope = AppInfoinput.InScope;

                    unitOfWork.Application.Update(appinfo);

                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool UpdateAppDemographics(ApplicationInformationModel AppInfoinput, int ClientId)
        {
            try
            {
                var v = savecountry(AppInfoinput, ClientId);
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                    var appinfo = unitOfWork.Application.GetByID(AppInfoinput.Id);
                    //basic info
                    appinfo.ClientApplicationNumber = AppInfoinput.ClientApplicationNumber;
                    appinfo.BusinessFunction = AppInfoinput.BusinessFunction;
                    appinfo.AppTypeId = AppInfoinput.AppTypeId;
                    appinfo.AliasName = AppInfoinput.AliasName;
                    appinfo.AppLifeCycleId = AppInfoinput.AppLifeCycleId;

                    appinfo.APROwningCompanyId = AppInfoinput.APROwningCompanyId;
                    appinfo.AppDomainId = AppInfoinput.AppDomainId;
                    appinfo.Description = AppInfoinput.Description;

                    appinfo.LegacyReasonExists = AppInfoinput.LegacyReasonExists;
                    appinfo.PlannedRetirementDate = AppInfoinput.PlannedRetirementDate;
                    appinfo.HostingTypeId = AppInfoinput.HostingTypeId;
                    appinfo.ApplicationClassificationId = AppInfoinput.ApplicationClassificationId;
                    appinfo.MigrationDate = AppInfoinput.MigrationDate;

                    //life cycle and Criticality
                    appinfo.AppLifeCycleId = AppInfoinput.AppLifeCycleId;
                    appinfo.IsTechnicalDocAvailable = AppInfoinput.IsTechnicalDocAvailable;
                    appinfo.IsApplicationDocAvailable = AppInfoinput.IsApplicationDocAvailable;
                    appinfo.CodeComplexity = AppInfoinput.CodeComplexity;
                    appinfo.NumberOfBugs = AppInfoinput.NumberOfBugs;
                    appinfo.NumberOfLinesOfCode = AppInfoinput.NumberOfLinesOfCode;
                    appinfo.AppCriticalityId = AppInfoinput.AppCriticalityId;
                    appinfo.AppSecurityLevelId = AppInfoinput.AppSecurityLevelId;
                    appinfo.IsDisasterRecoveryPlanAvailable = AppInfoinput.IsDisasterRecoveryPlanAvailable;
                    appinfo.TransactionVolumn = AppInfoinput.TransactionVolumn;
                    appinfo.TransactionVolumnUnit = AppInfoinput.TransactionVolumnUnit;
                    appinfo.DocumentationNotes = AppInfoinput.DocumentationNotes;

                    //Performance
                    appinfo.AvgConcurrentUsers = AppInfoinput.AvgConcurrentUsers;
                    appinfo.UserRange = AppInfoinput.UserRange;
                    appinfo.OutageFrequency = AppInfoinput.OutageFrequency;
                    appinfo.OutageDuration = AppInfoinput.OutageDuration;
                    appinfo.MaxConcurrentUsers = AppInfoinput.MaxConcurrentUsers;
                    appinfo.AppEnvironmentId = AppInfoinput.AppEnvironmentId;
                    appinfo.RevenueImpactIfDown = AppInfoinput.RevenueImpactIfDown;
                    appinfo.PerformanceNotes = AppInfoinput.PerformanceNotes;
                    appinfo.AvgIncidentTickets = AppInfoinput.AvgIncidentTickets;

                    //Arch and Geography

                    appinfo.AppArchitectureId = AppInfoinput.AppArchitectureId;
                    appinfo.AppProcessingId = AppInfoinput.AppProcessingId;
                    appinfo.AppSourceCodeId = AppInfoinput.AppSourceCodeId;
                    appinfo.APRDeploymentModelId = AppInfoinput.APRDeploymentModelId;
                    appinfo.APRStandardStatusId = AppInfoinput.APRStandardStatusId;
                    appinfo.APRDifferentiationLevelId = AppInfoinput.APRDifferentiationLevelId;
                    appinfo.BusinessLogicInClientTier = AppInfoinput.BusinessLogicInClientTier;
                    appinfo.MOTSLevel = AppInfoinput.MOTSLevel;
                    appinfo.NumberOfInstances = AppInfoinput.NumberOfInstances;
                    appinfo.AppSize = AppInfoinput.AppSize;
                    appinfo.SizeUnits = AppInfoinput.SizeUnits;
                    //  appinfo.CountryId = AppInfoinput.CountryId;
                    appinfo.RegionNames = AppInfoinput.RegionNames;

                    //Financial
                    appinfo.FinancialNotes = AppInfoinput.FinancialNotes;

                    //Notes
                    string value = HttpUtility.HtmlDecode(AppInfoinput.GeneralNotes);
                    appinfo.GeneralNotes = value;

                    unitOfWork.Application.Update(appinfo);

                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool savecountry(ApplicationInformationModel AppInfoinput, int ClientId)
        {
            Db db = new Db();
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    APRApplicationCountry appinfo = new APRApplicationCountry();
                    var ExistingCountries = unitOfWork.APRApplicationCountry.Get().Where(e => e.AppId == AppInfoinput.Id).Select(e => e.CountryId).ToList();
                    var deleteCountries = from details in db.APRApplicationCountry
                                          where details.AppId == AppInfoinput.Id
                                          select details;

                    foreach (var detail in deleteCountries)
                    {
                        unitOfWork.APRApplicationCountry.Delete(detail.Id);
                        unitOfWork.Save();
                    }
                    foreach (string id in AppInfoinput.MultipleCountries)
                    {
                        List<int> cid = id.Split(',').Select(int.Parse).ToList();

                        foreach (int i in cid)
                        {
                            appinfo.AppId = AppInfoinput.Id;
                            appinfo.ClientId = ClientId;
                            appinfo.CountryId = i;
                            //if (ExistingCountries.Contains(appinfo.CountryId) == false)
                            //{
                            unitOfWork.APRApplicationCountry.Insert(appinfo);
                            unitOfWork.Save();
                            //}

                        }
                        //appinfo.AppId = AppInfoinput.Id;
                        //appinfo.ClientId = AppInfoinput.ClientId;
                        //appinfo.CountryId = Convert.ToInt32(id);
                        //unitOfWork.APRApplicationCountry.Insert(appinfo);

                    }


                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static int UpdateDisposition(DispositionDetailsModel DispositionInfoinput,int ClientId)
        {
            var entityMapper = new Mapper<APRAppDisposition, DispositionDetailsModel>();
            try
            {
                var v = SaveRoadMap(DispositionInfoinput, ClientId);
                var dispInfo = new APRAppDisposition();
                using (var contextFactory = new DbContextFactory())
                {

                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    entityMapper.MapToEntity(DispositionInfoinput, dispInfo);
                    if (DispositionInfoinput.Id > 0)
                        // dispInfo = unitOfWork.APRAppDisposition.GetByID(DispositionInfoinput.Id);
                        unitOfWork.APRAppDisposition.Update(dispInfo);
                    else
                        unitOfWork.APRAppDisposition.Insert(dispInfo);
                    unitOfWork.Save();
                    DispositionInfoinput.Id = dispInfo.Id;
                    //return true;
                    return dispInfo.Id;
                }
            }


            catch (Exception)
            {
                return 0;
            }
        }

        public static bool SaveRoadMap(DispositionDetailsModel DispositionInfoinput, int ClientId)
        {
            Db db = new Db();
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    APRRoadmapTargetApp appinfo = new APRRoadmapTargetApp();
                    var ExistingCountries = unitOfWork.APRRoadmapTargetApp.Get().Where(e => e.APRDisId== DispositionInfoinput.Id).Select(e => e.APRRoadmapTargetAppId).ToList();
                    var deleteRoadMap = from details in db.APRRoadmapTargetApp
                                          where details.APRDisId == DispositionInfoinput.Id
                                          select details;

                    foreach (var detail in deleteRoadMap)
                    {
                        unitOfWork.APRRoadmapTargetApp.Delete(detail.Id);
                        unitOfWork.Save();
                    }
                    foreach (string id in DispositionInfoinput.MultipleRoadmap)
                    {
                        List<int> cid = id.Split(',').Select(int.Parse).ToList();

                        foreach (int i in cid)
                        {
                            appinfo.APRDisId = DispositionInfoinput.Id;
                            appinfo.ClientId = ClientId;
                            appinfo.APRRoadmapTargetAppId = i;
                            appinfo.RoadmapTargetApp = "";
                          
                            unitOfWork.APRRoadmapTargetApp.Insert(appinfo);                            
                            unitOfWork.Save();


                        }
                        //appinfo.AppId = AppInfoinput.Id;
                        //appinfo.ClientId = AppInfoinput.ClientId;
                        //appinfo.CountryId = Convert.ToInt32(id);
                        //unitOfWork.APRApplicationCountry.Insert(appinfo);

                    }


                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static int InsertAppFinanceInfo(APRFinancialModel AppFinanceinput)
        {
            try
            {
                var entityMapper = new Mapper<APRAppFinancial, APRFinancialModel>();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    APRAppFinancial appFinanceinfo = new APRAppFinancial();
                    entityMapper.MapToEntity(AppFinanceinput, appFinanceinfo);
                    unitOfWork.APRAppFinancial.Insert(appFinanceinfo);
                    unitOfWork.Save();
                    //you will get inserted id
                    return appFinanceinfo.Id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool UpdateAppFinanceInfo(APRFinancialModel AppFinanceinput)
        {
            try
            {
                var entityMapper = new Mapper<APRAppFinancial, APRFinancialModel>();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var appFinanceinfo = unitOfWork.APRAppFinancial.GetByID(AppFinanceinput.Id);
                    entityMapper.MapToEntity(AppFinanceinput, appFinanceinfo);
                    unitOfWork.APRAppFinancial.Update(appFinanceinfo);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static bool DeleteApplicationInfo(int ApplicationID)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.Application.Delete(ApplicationID);
                unitOfWork.Save();

                //TempData["SLACreatedDeleted"] = "Deleted successfully.";
                return true;
            }
        }

        public static DataTable APRRatingHeatMap(int ClientId, int surId)
        {

            var paramName = new string[] { "ClientId", "SurveyId" };
            var paramValue = new object[] { ClientId, surId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                dt = unitOfWork.GetDataFromStoredProcedure("APRSurveyRatingHeatMap", paramName, paramValue);
            }
            return dt;
            //List<RatingHeatMapModel> retRatingHeatMapList = new List<RatingHeatMapModel>();
            //foreach(DataRow  dr in dt.Rows)
            //{
            //   RatingHeatMapModel retRatingHeatMap = new RatingHeatMapModel();
            //    retRatingHeatMap.LOBName =   dr["LobName"] == null? "": dr["LobName"].ToString();
            //    retRatingHeatMap.BusinessRisk = dr["Business Risk"]==DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(dr["Business Risk"]);
            //    retRatingHeatMap.Maintainability = dr["Maintainability"]== DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(dr["Maintainability"]);
            //    retRatingHeatMap.Operatibility = dr["Operatibility"]== DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(dr["Operatibility"]);
            //    retRatingHeatMap.StretagicAlignment = dr["Stretagic Alignment"]== DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(dr["Stretagic Alignment"]);
            //    retRatingHeatMap.TechnicalCapability = dr["Technical Capability"]== DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(dr["Technical Capability"]);
            //    retRatingHeatMap.TechnicalRisk = dr["Technical Risk"]== DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(dr["Technical Risk"]);
            //    retRatingHeatMap.CloudReadiness= dr["Cloud Readiness"]== DBNull.Value ? (decimal)0.00 : Convert.ToDecimal(dr["Cloud Readiness"]);
            //    retRatingHeatMap.Id = (int)dr["lobid"];
            //    retRatingHeatMap.NoOfApplications = dr["ApplicationCount"]== DBNull.Value ? 0: Convert.ToInt32(dr["ApplicationCount"]);
            //    retRatingHeatMapList.Add(retRatingHeatMap);
            //}
            //return retRatingHeatMapList;
        }

        ///Raj added 1/30/2018
        public static DataTable APRAppFinancialDashbord(int ClientId)
        {

            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                dt = unitOfWork.GetDataFromStoredProcedure("APRAppFinancialDashboard", paramName, paramValue);
            }
            return dt;
        }

        private Dictionary<int, string> resolutionColors = new Dictionary<int, string>() {
            {1,"#0000FF"},
            {2,"#FFA500"},
            {3,"#008000" },
             {2,"#FFA500"},
            {3,"#008000" },
             {2,"#FFA500"},
            {3,"#008000" },
             {2,"#FFA500"},
            {3,"#008000" },
             {2,"#FFA500"},
            {3,"#008000" },
             {2,"#FFA500"},
            {3,"#008000" }
        };

        public static IList<DashboardModel> APRCriticalityDashboard(int ClientId)
        {
            IList<DashboardModel> Modellist = new List<DashboardModel>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("APRCriticalityDashboard", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DashboardModel();

                    coldet.PieChartColumnName = dr["CriticalityName"].ToString();
                    //coldet.Percentage = (dr["AppCount"]).ToString();
                    //coldet.Percentage = (coldet.Percentage * 100).ToString("##.##");

                    try
                    {

                        coldet.AppCount = Convert.ToInt32(dr["AppCount"]);
                    }

                    catch { }
                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }


        public static IList<DashboardModel> APRAppTypeDashboard(int ClientId)
        {
            IList<DashboardModel> Modellist = new List<DashboardModel>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("APRAppTypeDashboard", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DashboardModel();


                    coldet.PieChartColumnName = dr["AppType"].ToString();
                    //coldet.Percentage = (dr["AppCount"]).ToString();
                    // coldet.Percentage = Convert.ToDecimal(dr["PerAppCount"]);

                    try
                    {

                        coldet.AppCount = Convert.ToInt32(dr["AppCount"]);
                    }

                    catch { }
                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }

        public static IList<DashboardModel> APRAppSourceCodeTypeDashboard(int ClientId)
        {
            IList<DashboardModel> Modellist = new List<DashboardModel>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("APRAppSourceCodeTypeDashboard", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DashboardModel();


                    coldet.PieChartColumnName = dr["SourceCodeType"].ToString();
                    //coldet.Percentage = (dr["AppCount"]).ToString();
                    // coldet.Percentage = Convert.ToDecimal(dr["PerAppCount"]);

                    try
                    {

                        coldet.AppCount = Convert.ToInt32(dr["AppCount"]);
                    }

                    catch { }
                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }

        public static IList<DashboardModel> APRAppDispositionDashboard(int ClientId)
        {
            IList<DashboardModel> Modellist = new List<DashboardModel>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("APRAppDispositionDashboard", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DashboardModel();


                    coldet.PieChartColumnName = dr["DispositionName"].ToString();
                    //coldet.Percentage = (dr["AppCount"]).ToString();
                    // coldet.Percentage = Convert.ToDecimal(dr["PerAppCount"]);


                    try
                    {

                        coldet.AppCount = Convert.ToInt32(dr["AppCount"]);
                    }

                    catch { }
                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }



        public static IList<DashboardModel> APRAppBusinessFunctionDashboard(int ClientId)
        {
            IList<DashboardModel> Modellist = new List<DashboardModel>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("APRAppBusinessFunctionDashboard", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DashboardModel();


                    coldet.PieChartColumnName = dr["BusinessFunction"].ToString();
                    //coldet.Percentage = (dr["AppCount"]).ToString();
                    //coldet.Percentage = Convert.ToDecimal(dr["PerAppCount"]);

                    try
                    {

                        coldet.AppCount = Convert.ToInt32(dr["AppCount"]);
                    }

                    catch { }
                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }

        public static IList<DashboardModel> APRPortfolioBreakdownDashboard(int ClientId)
        {
            IList<DashboardModel> Modellist = new List<DashboardModel>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("APRPortfolioBreakdownDashboard", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DashboardModel();


                    coldet.PieChartColumnName = dr["LOBName"].ToString();
                    //coldet.Percentage = (dr["AppCount"]).ToString();
                    //coldet.Percentage = Convert.ToDecimal(dr["PerAppCount"]);

                    try
                    {

                        coldet.AppCount = Convert.ToInt32(dr["AppCount"]);
                    }

                    catch { }
                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }


        public static DashboardModel APRAppDashboard(int AppId)
        {

            var paramName = new string[] { "AppId" };
            var paramValue = new object[] { AppId };
            DataTable dt = new DataTable();
            DashboardModel model = new DashboardModel();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                dt = unitOfWork.GetDataFromStoredProcedure("APRDashboard", paramName, paramValue);
                foreach (DataRow dr in dt.Rows)
                {
                    // var coldet = new DashboardModel();


                    model.ClientApplicationNumber = dr["ClientApplicationNumber"].ToString();
                    model.Description = dr["Description"].ToString();
                    model.Criticality = dr["CriticalityName"].ToString();
                    model.NoOfUsers = dr["MaxConcurrentUsers"].ToString();
                    model.RevenueImpact = dr["RevenueImpactIfDown"].ToString();
                    model.CodeType = dr["SourceCodeType"].ToString();
                    model.AppDomain = dr["AppDomainName"].ToString();
                    model.SecurityLevel = dr["SecurityLevelName"].ToString();
                    model.TechnicalOwnerName = dr["TechnicalOwner"].ToString();
                    //model.BusinessOwnerName = dr["SecurityLevelName"].ToString();
                    model.Architecture = dr["ArchitectureType"].ToString();
                    model.AppSize = dr["AppSize"].ToString();
                    model.Analysis = dr["Analysis"].ToString();
                    model.Recommendation = dr["Recommendation"].ToString();

                    try
                    {

                        //coldet.IncidentCount = Convert.ToInt16(dr["IncidentCount"]);
                    }

                    catch { }

                }

            }
            return model;
        }

        public static IList<AnalysisModel> APRSourceColumnList(int ClientId)
        {
            IList<AnalysisModel> Modellist = new List<AnalysisModel>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("APRSourceColumnList", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new AnalysisModel();


                    coldet.ToolColumnName = dr["ToolColumnName"].ToString();
                    coldet.ClientColumnName = dr["ClientColumnName"].ToString();



                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }

        public static DataTable APRAnalysisReport(int ClientId)
        {

            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                dt = unitOfWork.GetDataFromStoredProcedure("APRAnalysisReport", paramName, paramValue);
            }
            return dt;
        }


        public static IList<MetaDataModel> GetMetaDataModel(int ClientId, int metaDatatypeId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                MetaDataModel model = new MetaDataModel();
                var entityMapper = new Mapper<MetaDataModel, APRAppLifeCycle>();
                //IEnumerable<APRAppLifeCycle> ApplicationInfo = null;
                IList<MetaDataModel> metadataList = new List<MetaDataModel>();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                switch (metaDatatypeId)
                {
                    case 1:
                        var lifeCycleList = unitOfWork.APRAppLifeCycle.Get(m => m.ClientId == ClientId).ToList();
                        foreach (var item in lifeCycleList)
                        {
                            var metadataItem = new MetaDataModel();
                            metadataItem.ClientId = ClientId;
                            metadataItem.Value = item.Id.ToString();
                            metadataItem.Text = item.AppLifeCycleName;
                            metadataItem.MetadataTypeId = metaDatatypeId;
                            metadataList.Add(metadataItem);
                        }
                        break;
                    case 2:
                        var aprSourceCodeList = unitOfWork.APRDispositionRationale.Get(m => m.ClientId == ClientId).ToList();
                        foreach (var item in aprSourceCodeList)
                        {
                            var metadataItem = new MetaDataModel();
                            metadataItem.ClientId = ClientId;
                            metadataItem.Value = item.Id.ToString();
                            metadataItem.Text = item.DispositionRationaleType;
                            metadataItem.MetadataTypeId = metaDatatypeId;
                            metadataList.Add(metadataItem);
                        }
                        break;
                    case 3:
                        var aprAppTypeList = unitOfWork.APRAppType.Get(m => m.ClientId == ClientId).ToList();
                        foreach (var item in aprAppTypeList)
                        {
                            var metadataItem = new MetaDataModel();
                            metadataItem.ClientId = ClientId;
                            metadataItem.Value = item.Id.ToString();
                            metadataItem.Text = item.AppType;
                            metadataItem.MetadataTypeId = metaDatatypeId;
                            metadataList.Add(metadataItem);
                        }
                        break;
                    case 4:
                        var aprAppDomainList = unitOfWork.APRDomain.Get(m => m.ClientId == ClientId).ToList();
                        foreach (var item in aprAppDomainList)
                        {
                            var metadataItem = new MetaDataModel();
                            metadataItem.ClientId = ClientId;
                            metadataItem.Value = item.Id.ToString();
                            metadataItem.Text = item.AppDomainName;
                            metadataItem.MetadataTypeId = metaDatatypeId;
                            metadataList.Add(metadataItem);
                        }
                        break;
                    case 5:
                        var aprcriticalityList = unitOfWork.APRAppCriticality.Get(m => m.ClientId == ClientId).ToList();
                        foreach (var item in aprcriticalityList)
                        {
                            var metadataItem = new MetaDataModel();
                            metadataItem.ClientId = ClientId;
                            metadataItem.Value = item.Id.ToString();
                            metadataItem.Text = item.CriticalityName;
                            metadataItem.MetadataTypeId = metaDatatypeId;
                            metadataList.Add(metadataItem);
                        }
                        break;
                    case 6:
                        //    var appArchitectureList = unitOfWork.APRAppArchitecture.Get(m => m.ClientId == ClientId).ToList();
                        //    foreach (var item in appArchitectureList)
                        //    {
                        //        var metadataItem = new MetaDataModel();
                        //        metadataItem.ClientId = ClientId;
                        //        metadataItem.Value = item.Id.ToString();
                        //        metadataItem.Text = item.ArchitectureType;
                        //        metadataItem.MetadataTypeId = metaDatatypeId;
                        //        metadataList.Add(metadataItem);
                        //    }
                        break;
                    case 7:
                        var appSecurity = unitOfWork.APRAppSecurity.Get(m => m.ClientId == ClientId).ToList();
                        foreach (var item in appSecurity)
                        {
                            var metadataItem = new MetaDataModel();
                            metadataItem.ClientId = ClientId;
                            metadataItem.Value = item.Id.ToString();
                            metadataItem.Text = item.SecurityLevelName;
                            metadataItem.MetadataTypeId = metaDatatypeId;
                            metadataList.Add(metadataItem);
                        }
                        break;
                    case 8:
                        //    var appDisasterRecovery = unitOfWork.APRAppProcessingType.Get(m => m.ClientId == ClientId).ToList();
                        //    foreach (var item in appDisasterRecovery)
                        //    {
                        //        var metadataItem = new MetaDataModel();
                        //        metadataItem.ClientId = ClientId;
                        //        metadataItem.Value = item.Id.ToString();
                        //        metadataItem.Text = item.ArchitectureType;
                        //        metadataItem.MetadataTypeId = metaDatatypeId;
                        //        metadataList.Add(metadataItem);
                        //    }
                        break;
                    case 9:
                        //    var TechDocAvailable = unitOfWork.APRAppArchitecture.Get(m => m.ClientId == ClientId).ToList();
                        //    foreach (var item in appTechDocAvailable)
                        //    {
                        //        var metadataItem = new MetaDataModel();
                        //        metadataItem.ClientId = ClientId;
                        //        metadataItem.Value = item.Id.ToString();
                        //        metadataItem.Text = item.ArchitectureType;
                        //        metadataItem.MetadataTypeId = metaDatatypeId;
                        //        metadataList.Add(metadataItem);
                        //    }
                        break;
                    case 10:
                        //    var AppDocAvailble = unitOfWork.APRAppArchitecture.Get(m => m.ClientId == ClientId).ToList();
                        //    foreach (var item in AppDocAvailble)
                        //    {
                        //        var metadataItem = new MetaDataModel();
                        //        metadataItem.ClientId = ClientId;
                        //        metadataItem.Value = item.Id.ToString();
                        //        metadataItem.Text = item.ArchitectureType;
                        //        metadataItem.MetadataTypeId = metaDatatypeId;
                        //        metadataList.Add(metadataItem);
                        //    }
                        break;
                    case 11:
                        var aprAppProcessingType = unitOfWork.APRAppProcessingType.Get(m => m.ClientId == ClientId).ToList();
                        foreach (var item in aprAppProcessingType)
                        {
                            var metadataItem = new MetaDataModel();
                            metadataItem.ClientId = ClientId;
                            metadataItem.Value = item.Id.ToString();
                            metadataItem.Text = item.ProcessingType;
                            metadataItem.MetadataTypeId = metaDatatypeId;
                            metadataList.Add(metadataItem);
                        }
                        break;
                    case 12:
                        var appArchitectureList = unitOfWork.APRAppArchitecture.Get(m => m.ClientId == ClientId).ToList();
                        foreach (var item in appArchitectureList)
                        {
                            var metadataItem = new MetaDataModel();
                            metadataItem.ClientId = ClientId;
                            metadataItem.Value = item.Id.ToString();
                            metadataItem.Text = item.ArchitectureType;
                            metadataItem.MetadataTypeId = metaDatatypeId;
                            metadataList.Add(metadataItem);
                        }
                        break;
                    case 13:
                        var aprAppSourceCodeType = unitOfWork.APRAppSourceCodeType.Get(m => m.ClientId == ClientId).ToList();
                        foreach (var item in aprAppSourceCodeType)
                        {
                            var metadataItem = new MetaDataModel();
                            metadataItem.ClientId = ClientId;
                            metadataItem.Value = item.Id.ToString();
                            metadataItem.Text = item.SourcecodeType;
                            metadataItem.MetadataTypeId = metaDatatypeId;
                            metadataList.Add(metadataItem);
                        }
                        break;
                    case 14:
                        var aprDisposition = unitOfWork.APRDisposition.Get(m => m.ClientId == ClientId).ToList();
                        foreach (var item in aprDisposition)
                        {
                            var metadataItem = new MetaDataModel();
                            metadataItem.ClientId = ClientId;
                            metadataItem.Value = item.Id.ToString();
                            metadataItem.Text = item.DispositionName;
                            metadataItem.MetadataTypeId = metaDatatypeId;
                            metadataList.Add(metadataItem);
                        }
                        break;
                    case 15:
                        //var appArchitectureList = unitOfWork.APRAppArchitecture.Get(m => m.ClientId == ClientId).ToList();
                        //foreach (var item in appArchitectureList)
                        //{
                        //    var metadataItem = new MetaDataModel();
                        //    metadataItem.ClientId = ClientId;
                        //    metadataItem.Value = item.Id.ToString();
                        //    metadataItem.Text = item.ArchitectureType;
                        //    metadataItem.MetadataTypeId = metaDatatypeId;
                        //    metadataList.Add(metadataItem);
                        //}
                        break;
                    case 16:
                        var aprDispositionAction = unitOfWork.APRDispositionAction.Get(m => m.ClientId == ClientId).ToList();
                        foreach (var item in aprDispositionAction)
                        {
                            var metadataItem = new MetaDataModel();
                            metadataItem.ClientId = ClientId;
                            metadataItem.Value = item.Id.ToString();
                            metadataItem.Text = item.DispositionActionType;
                            metadataItem.MetadataTypeId = metaDatatypeId;
                            metadataList.Add(metadataItem);
                        }
                        break;
                    case 17:
                        var aprAppEnvironment = unitOfWork.APRAppEnvironment.Get(m => m.ClientId == ClientId).ToList();
                        foreach (var item in aprAppEnvironment)
                        {
                            var metadataItem = new MetaDataModel();
                            metadataItem.ClientId = ClientId;
                            metadataItem.Value = item.Id.ToString();
                            metadataItem.Text = item.EnvironmentType;
                            metadataItem.MetadataTypeId = metaDatatypeId;
                            metadataList.Add(metadataItem);
                        }
                        break;
                    case 18:
                        var applicationClassification = unitOfWork.ApplicationClassification.Get(m => m.ClientId == ClientId).ToList();
                        foreach (var item in applicationClassification)
                        {
                            var metadataItem = new MetaDataModel();
                            metadataItem.ClientId = ClientId;
                            metadataItem.Value = item.Id.ToString();
                            metadataItem.Text = item.ApplicationClassificationName;
                            metadataItem.MetadataTypeId = metaDatatypeId;
                            metadataList.Add(metadataItem);
                        }
                        break;
                    case 19:
                        var externalHosting = unitOfWork.ExternalHosting.Get(m => m.ClientId == ClientId).ToList();
                        foreach (var item in externalHosting)
                        {
                            var metadataItem = new MetaDataModel();
                            metadataItem.ClientId = ClientId;
                            metadataItem.Value = item.Id.ToString();
                            metadataItem.Text = item.HostingTypeName;
                            metadataItem.MetadataTypeId = metaDatatypeId;
                            metadataList.Add(metadataItem);
                        }
                        break;
                    case 20:
                        var appredundancy = unitOfWork.APRAppRedundancy.Get(m => m.ClientId == ClientId).ToList();
                        foreach (var item in appredundancy)
                        {
                            var metadataItem = new MetaDataModel();
                            metadataItem.ClientId = ClientId;
                            metadataItem.Value = item.Id.ToString();
                            metadataItem.Text = item.RedundancyName;
                            metadataItem.MetadataTypeId = metaDatatypeId;
                            metadataList.Add(metadataItem);
                        }
                        break;
                    case 21:
                        var owningcompany = unitOfWork.APROwningCompany.Get(m => m.ClientId == ClientId).ToList();
                        foreach (var item in owningcompany)
                        {
                            var metadataItem = new MetaDataModel();
                            metadataItem.ClientId = ClientId;
                            metadataItem.Value = item.Id.ToString();
                            metadataItem.Text = item.OwningCompany;
                            metadataItem.MetadataTypeId = metaDatatypeId;
                            metadataList.Add(metadataItem);
                        }
                        break;
                    case 22:
                        var deploymentModel = unitOfWork.DeploymentModel.Get(m => m.ClientId == ClientId).ToList();
                        foreach (var item in deploymentModel)
                        {
                            var metadataItem = new MetaDataModel();
                            metadataItem.ClientId = ClientId;
                            metadataItem.Value = item.Id.ToString();
                            metadataItem.Text = item.DeploymentModel;
                            metadataItem.MetadataTypeId = metaDatatypeId;
                            metadataList.Add(metadataItem);
                        }
                        break;
                    case 23:
                        var standardStatus = unitOfWork.StandardStatusModel.Get(m => m.ClientId == ClientId).ToList();
                        foreach (var item in standardStatus)
                        {
                            var metadataItem = new MetaDataModel();
                            metadataItem.ClientId = ClientId;
                            metadataItem.Value = item.Id.ToString();
                            metadataItem.Text = item.StandardStatus;
                            metadataItem.MetadataTypeId = metaDatatypeId;
                            metadataList.Add(metadataItem);
                        }
                        break;

                    case 24:
                        var differentiationLevel = unitOfWork.DifferentiationLevelModel.Get(m => m.ClientId == ClientId).ToList();
                        foreach (var item in differentiationLevel)
                        {
                            var metadataItem = new MetaDataModel();
                            metadataItem.ClientId = ClientId;
                            metadataItem.Value = item.Id.ToString();
                            metadataItem.Text = item.DifferentiationLevel;
                            metadataItem.MetadataTypeId = metaDatatypeId;
                            metadataList.Add(metadataItem);
                        }
                        break;
                    case 25:
                        var interfaceMethods = unitOfWork.APRInterfaceMethod.Get(m => m.ClientId == ClientId).ToList();
                        foreach (var item in interfaceMethods)
                        {
                            var metadataItem = new MetaDataModel();
                            metadataItem.ClientId = ClientId;
                            metadataItem.Value = item.Id.ToString();
                            metadataItem.Text = item.InterfaceMethod;
                            metadataItem.MetadataTypeId = metaDatatypeId;
                            metadataList.Add(metadataItem);
                        }
                        break;
                    case 26:
                        var interDispSolutions = unitOfWork.APRDispositionSolution.Get(m => m.ClientId == ClientId).ToList();
                        foreach (var item in interDispSolutions)
                        {
                            var metadataItem = new MetaDataModel();
                            metadataItem.ClientId = ClientId;
                            metadataItem.Value = item.Id.ToString();
                            metadataItem.Text = item.DispositionSolution;
                            metadataItem.MetadataTypeId = metaDatatypeId;
                            metadataList.Add(metadataItem);
                        }
                        break;

                }
                return metadataList;
            }
        }

        public static bool SaveMeta(int MetadataTypeId, string MetaItem, int? metadataid, int clientid)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    switch (MetadataTypeId)
                    {
                        case 1:

                            APRAppLifeCycle APRLife = new APRAppLifeCycle();
                            if (metadataid > 0)
                            {
                                APRLife = unitOfWork.APRAppLifeCycle.GetByID(metadataid);
                                APRLife.AppLifeCycleName = MetaItem;
                                unitOfWork.APRAppLifeCycle.Update(APRLife);
                            }
                            else
                            {
                                APRLife.AppLifeCycleName = MetaItem;
                                APRLife.ClientId = clientid;
                                unitOfWork.APRAppLifeCycle.Insert(APRLife);
                            }
                            break;
                        case 2:

                            APRDispositionRationale aprDispositionRationale = new APRDispositionRationale();
                            if (metadataid > 0)
                            {
                                aprDispositionRationale = unitOfWork.APRDispositionRationale.GetByID(metadataid);
                                aprDispositionRationale.DispositionRationaleType = MetaItem;
                                unitOfWork.APRDispositionRationale.Update(aprDispositionRationale);
                            }
                            else
                            {
                                aprDispositionRationale.DispositionRationaleType = MetaItem;
                                aprDispositionRationale.ClientId = clientid;
                                unitOfWork.APRDispositionRationale.Insert(aprDispositionRationale);
                            }
                            unitOfWork.Save();
                            break;
                        case 3:

                            APRAppType aprAppType = new APRAppType();
                            if (metadataid > 0)
                            {
                                aprAppType = unitOfWork.APRAppType.GetByID(metadataid);
                                aprAppType.AppType = MetaItem;
                                unitOfWork.APRAppType.Update(aprAppType);
                            }
                            else
                            {
                                aprAppType.AppType = MetaItem;
                                aprAppType.ClientId = clientid;
                                unitOfWork.APRAppType.Insert(aprAppType);
                            }
                            unitOfWork.Save();
                            break;
                        case 4:

                            APRDomain aprDomain = new APRDomain();
                            if (metadataid > 0)
                            {
                                aprDomain = unitOfWork.APRDomain.GetByID(metadataid);
                                aprDomain.AppDomainName = MetaItem;
                                unitOfWork.APRDomain.Update(aprDomain);
                            }
                            else
                            {
                                aprDomain.AppDomainName = MetaItem;
                                aprDomain.ClientId = clientid;
                                unitOfWork.APRDomain.Insert(aprDomain);
                            }

                            break;
                        case 5:

                            APRAppCriticality aprCriticality = new APRAppCriticality();
                            if (metadataid > 0)
                            {
                                aprCriticality = unitOfWork.APRAppCriticality.GetByID(metadataid);
                                aprCriticality.CriticalityName = MetaItem;
                                aprCriticality.CriticalityShortName = "ShortName";
                                aprCriticality.Description = "Desc";
                                unitOfWork.APRAppCriticality.Update(aprCriticality);
                            }
                            else
                            {
                                aprCriticality.CriticalityName = MetaItem;
                                aprCriticality.ClientId = clientid;
                                aprCriticality.CriticalityShortName = "ShortName";
                                aprCriticality.Description = "Desc";
                                unitOfWork.APRAppCriticality.Insert(aprCriticality);
                            }

                            break;
                        //case 6:

                        //    APRAppArchitecture aprArchitecture = new APRAppArchitecture();
                        //    if (metadataid > 0)
                        //    {
                        //        aprArchitecture = unitOfWork.APRAppArchitecture.GetByID(metadataid);
                        //        aprArchitecture.ArchitectureType = MetaItem;
                        //        unitOfWork.APRAppArchitecture.Update(aprArchitecture);
                        //    }
                        //    else
                        //    {
                        //        aprArchitecture.ArchitectureType = MetaItem;
                        //        aprArchitecture.ClientId = clientid;
                        //        unitOfWork.APRAppArchitecture.Insert(aprArchitecture);
                        //    }

                        //    break;
                        case 7:

                            APRAppSecurity APRSecurity = new APRAppSecurity();
                            if (metadataid > 0)
                            {
                                APRSecurity = unitOfWork.APRAppSecurity.GetByID(metadataid);
                                APRSecurity.SecurityLevelName = MetaItem;
                                unitOfWork.APRAppSecurity.Update(APRSecurity);
                            }
                            else
                            {
                                APRSecurity.SecurityLevelName = MetaItem;
                                APRSecurity.ClientId = clientid;
                                unitOfWork.APRAppSecurity.Insert(APRSecurity);
                            }
                            break;
                        case 8:

                            //    APRAppSourceCodeType APRSourc = new APRAppSourceCodeType();
                            //    if (metadataid > 0)
                            //    {
                            //        APRSourc = unitOfWork.aprapp.GetByID(metadataid);
                            //        APRSourc.SourcecodeType = MetaItem;
                            //        unitOfWork.APRAppSourceCodeType.Update(APRSourc);
                            //    }
                            //    else
                            //    {
                            //        APRSourc.SourcecodeType = MetaItem;
                            //        APRSourc.ClientId = clientid;
                            //        unitOfWork.APRAppSourceCodeType.Insert(APRSourc);
                            //    }
                            //    unitOfWork.Save();
                            break;
                        case 9:

                            //    APRAppType aprAppType = new APRAppType();
                            //    if (metadataid > 0)
                            //    {
                            //        aprAppType = unitOfWork.APRAppType.GetByID(metadataid);
                            //        aprAppType.AppType = MetaItem;
                            //        unitOfWork.APRAppType.Update(aprAppType);
                            //    }
                            //    else
                            //    {
                            //        aprAppType.AppType = MetaItem;
                            //        aprAppType.ClientId = clientid;
                            //        unitOfWork.APRAppType.Insert(aprAppType);
                            //    }
                            //    unitOfWork.Save();
                            break;
                        case 10:

                            //    APRDomain aprDomain = new APRDomain();
                            //    if (metadataid > 0)
                            //    {
                            //        aprDomain = unitOfWork.APRDomain.GetByID(metadataid);
                            //        aprDomain.AppDomainName = MetaItem;
                            //        unitOfWork.APRDomain.Update(aprDomain);
                            //    }
                            //    else
                            //    {
                            //        aprDomain.AppDomainName = MetaItem;
                            //        aprDomain.ClientId = clientid;
                            //        unitOfWork.APRDomain.Insert(aprDomain);
                            //    }

                            break;
                        case 11:

                            APRAppProcessingType aprAppProcessingType = new APRAppProcessingType();
                            if (metadataid > 0)
                            {
                                aprAppProcessingType = unitOfWork.APRAppProcessingType.GetByID(metadataid);
                                aprAppProcessingType.ProcessingType = MetaItem;
                                unitOfWork.APRAppProcessingType.Update(aprAppProcessingType);
                            }
                            else
                            {
                                aprAppProcessingType.ProcessingType = MetaItem;
                                aprAppProcessingType.ClientId = clientid;
                                unitOfWork.APRAppProcessingType.Insert(aprAppProcessingType);
                            }

                            break;
                        case 12:

                            APRAppArchitecture aprArchitecture = new APRAppArchitecture();
                            if (metadataid > 0)
                            {
                                aprArchitecture = unitOfWork.APRAppArchitecture.GetByID(metadataid);
                                aprArchitecture.ArchitectureType = MetaItem;
                                unitOfWork.APRAppArchitecture.Update(aprArchitecture);
                            }
                            else
                            {
                                aprArchitecture.ArchitectureType = MetaItem;
                                aprArchitecture.ClientId = clientid;
                                unitOfWork.APRAppArchitecture.Insert(aprArchitecture);
                            }

                            break;
                        case 13:

                            APRAppSourceCodeType APRSourc = new APRAppSourceCodeType();
                            if (metadataid > 0)
                            {
                                APRSourc = unitOfWork.APRAppSourceCodeType.GetByID(metadataid);
                                APRSourc.SourcecodeType = MetaItem;
                                unitOfWork.APRAppSourceCodeType.Update(APRSourc);
                            }
                            else
                            {
                                APRSourc.SourcecodeType = MetaItem;
                                APRSourc.ClientId = clientid;
                                unitOfWork.APRAppSourceCodeType.Insert(APRSourc);
                            }
                            unitOfWork.Save();

                            break;
                        case 14:

                            APRDisposition aprDisposition = new APRDisposition();
                            if (metadataid > 0)
                            {
                                aprDisposition = unitOfWork.APRDisposition.GetByID(metadataid);
                                aprDisposition.DispositionName = MetaItem;
                                unitOfWork.APRDisposition.Update(aprDisposition);
                            }
                            else
                            {
                                aprDisposition.DispositionName = MetaItem;
                                aprDisposition.ClientId = clientid;
                                unitOfWork.APRDisposition.Insert(aprDisposition);
                            }

                            break;
                        case 15:

                            //    APRDisposition aprDisposition = new APRDisposition();
                            //    if (metadataid > 0)
                            //    {
                            //        aprDisposition = unitOfWork.APRDisposition.GetByID(metadataid);
                            //        aprDisposition.DispositionName = MetaItem;
                            //        unitOfWork.APRDisposition.Update(aprDisposition);
                            //    }
                            //    else
                            //    {
                            //        aprDisposition.DispositionName = MetaItem;
                            //        aprDisposition.ClientId = clientid;
                            //        unitOfWork.APRDisposition.Insert(aprDisposition);
                            //    }

                            break;
                        case 16:
                            APRDispositionAction aprDispositionAction = new APRDispositionAction();
                            if (metadataid > 0)
                            {
                                aprDispositionAction = unitOfWork.APRDispositionAction.GetByID(metadataid);
                                aprDispositionAction.DispositionActionType = MetaItem;
                                unitOfWork.APRDispositionAction.Update(aprDispositionAction);
                            }
                            else
                            {
                                aprDispositionAction.DispositionActionType = MetaItem;
                                aprDispositionAction.ClientId = clientid;
                                unitOfWork.APRDispositionAction.Insert(aprDispositionAction);
                            }

                            break;
                        case 17:

                            APRAppEnvironment aprAppEnvironment = new APRAppEnvironment();
                            if (metadataid > 0)
                            {
                                aprAppEnvironment = unitOfWork.APRAppEnvironment.GetByID(metadataid);
                                aprAppEnvironment.EnvironmentType = MetaItem;
                                unitOfWork.APRAppEnvironment.Update(aprAppEnvironment);
                            }
                            else
                            {
                                aprAppEnvironment.EnvironmentType = MetaItem;
                                aprAppEnvironment.ClientId = clientid;
                                unitOfWork.APRAppEnvironment.Insert(aprAppEnvironment);
                            }

                            break;

                        case 18:

                            ApplicationClassification applicationClassification = new ApplicationClassification();
                            if (metadataid > 0)
                            {
                                applicationClassification = unitOfWork.ApplicationClassification.GetByID(metadataid);
                                applicationClassification.ApplicationClassificationName = MetaItem;
                                unitOfWork.ApplicationClassification.Update(applicationClassification);
                            }
                            else
                            {
                                applicationClassification.ApplicationClassificationName = MetaItem;
                                applicationClassification.ClientId = clientid;
                                unitOfWork.ApplicationClassification.Insert(applicationClassification);
                            }

                            break;
                        case 19:

                            ExternalHosting externalHosting = new ExternalHosting();
                            if (metadataid > 0)
                            {
                                externalHosting = unitOfWork.ExternalHosting.GetByID(metadataid);
                                externalHosting.HostingTypeName = MetaItem;
                                unitOfWork.ExternalHosting.Update(externalHosting);
                            }
                            else
                            {
                                externalHosting.HostingTypeName = MetaItem;
                                externalHosting.ClientId = clientid;
                                unitOfWork.ExternalHosting.Insert(externalHosting);
                            }

                            break;
                        case 20:

                            APRAppRedundancy appredundancy = new APRAppRedundancy();
                            if (metadataid > 0)
                            {
                                appredundancy = unitOfWork.APRAppRedundancy.GetByID(metadataid);
                                appredundancy.RedundancyName = MetaItem;
                                unitOfWork.APRAppRedundancy.Update(appredundancy);
                            }
                            else
                            {
                                appredundancy.RedundancyName = MetaItem;
                                appredundancy.ClientId = clientid;
                                unitOfWork.APRAppRedundancy.Insert(appredundancy);
                            }

                            break;
                        case 21:

                            APROwningCompany owningCompany = new APROwningCompany();
                            if (metadataid > 0)
                            {
                                owningCompany = unitOfWork.APROwningCompany.GetByID(metadataid);
                                owningCompany.OwningCompany = MetaItem;
                                unitOfWork.APROwningCompany.Update(owningCompany);
                            }
                            else
                            {
                                owningCompany.OwningCompany = MetaItem;
                                owningCompany.ClientId = clientid;
                                unitOfWork.APROwningCompany.Insert(owningCompany);
                            }

                            break;
                        case 22:

                            APRDeploymentModel deploymentmodel = new APRDeploymentModel();
                            if (metadataid > 0)
                            {
                                deploymentmodel = unitOfWork.DeploymentModel.GetByID(metadataid);
                                deploymentmodel.DeploymentModel = MetaItem;
                                unitOfWork.DeploymentModel.Update(deploymentmodel);
                            }
                            else
                            {
                                deploymentmodel.DeploymentModel = MetaItem;
                                deploymentmodel.ClientId = clientid;
                                unitOfWork.DeploymentModel.Insert(deploymentmodel);
                            }

                            break;
                        case 23:

                            APRStandardStatus standardStatus = new APRStandardStatus();
                            if (metadataid > 0)
                            {
                                standardStatus = unitOfWork.StandardStatusModel.GetByID(metadataid);
                                standardStatus.StandardStatus = MetaItem;
                                unitOfWork.StandardStatusModel.Update(standardStatus);
                            }
                            else
                            {
                                standardStatus.StandardStatus = MetaItem;
                                standardStatus.ClientId = clientid;
                                unitOfWork.StandardStatusModel.Insert(standardStatus);
                            }

                            break;
                        case 24:

                            APRDifferentiationLevel differentiationLevel = new APRDifferentiationLevel();
                            if (metadataid > 0)
                            {
                                differentiationLevel = unitOfWork.DifferentiationLevelModel.GetByID(metadataid);
                                differentiationLevel.DifferentiationLevel = MetaItem;
                                unitOfWork.DifferentiationLevelModel.Update(differentiationLevel);
                            }
                            else
                            {
                                differentiationLevel.DifferentiationLevel = MetaItem;
                                differentiationLevel.ClientId = clientid;
                                unitOfWork.DifferentiationLevelModel.Insert(differentiationLevel);
                            }

                            break;
                        case 25:

                            APRInterfaceMethod interfaceMethod = new APRInterfaceMethod();
                            if (metadataid > 0)
                            {
                                interfaceMethod = unitOfWork.APRInterfaceMethod.GetByID(metadataid);
                                interfaceMethod.InterfaceMethod = MetaItem;
                                unitOfWork.APRInterfaceMethod.Update(interfaceMethod);
                            }
                            else
                            {
                                interfaceMethod.InterfaceMethod = MetaItem;
                                interfaceMethod.ClientId = clientid;
                                unitOfWork.APRInterfaceMethod.Insert(interfaceMethod);
                            }
                            break;
                        case 26:

                            APRDispositionSolution dispSolution = new APRDispositionSolution();
                            if (metadataid > 0)
                            {
                                dispSolution = unitOfWork.APRDispositionSolution.GetByID(metadataid);
                                dispSolution.DispositionSolution = MetaItem;
                                unitOfWork.APRDispositionSolution.Update(dispSolution);
                            }
                            else
                            {
                                dispSolution.DispositionSolution = MetaItem;
                                dispSolution.ClientId = clientid;
                                unitOfWork.APRDispositionSolution.Insert(dispSolution);
                            }
                            break;
                    }
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool DeleteMeta(int MetadataTypeId, int MetaItem)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                    switch (MetadataTypeId)
                    {
                        case 1:
                            unitOfWork.APRAppLifeCycle.Delete(MetaItem);
                            break;
                        case 2:
                            unitOfWork.APRDispositionRationale.Delete(MetaItem);
                            break;
                        case 3:
                            unitOfWork.APRAppType.Delete(MetaItem);
                            break;
                        case 4:
                            unitOfWork.APRDomain.Delete(MetaItem);
                            break;
                        case 5:
                            unitOfWork.APRAppCriticality.Delete(MetaItem);
                            break;
                        case 6:
                            //unitOfWork.APRAppArchitecture.Delete(MetaItem);
                            break;
                        case 7:
                            unitOfWork.APRAppSecurity.Delete(MetaItem);
                            break;
                        case 8:
                            //unitOfWork.APRAppSourceCodeType.Delete(MetaItem);
                            break;
                        case 9:
                            //unitOfWork.APRAppType.Delete(MetaItem);
                            break;
                        case 10:
                            //unitOfWork.APRDomain.Delete(MetaItem);
                            break;
                        case 11:
                            unitOfWork.APRAppProcessingType.Delete(MetaItem);
                            break;
                        case 12:
                            unitOfWork.APRAppArchitecture.Delete(MetaItem);
                            break;
                        case 13:
                            unitOfWork.APRAppSourceCodeType.Delete(MetaItem);
                            break;
                        case 14:
                            unitOfWork.APRDisposition.Delete(MetaItem);
                            break;
                        case 15:
                            //unitOfWork.APRAppArchitecture.Delete(MetaItem);
                            break;
                        case 16:
                            unitOfWork.APRDispositionAction.Delete(MetaItem);
                            break;
                        case 17:
                            unitOfWork.APRAppEnvironment.Delete(MetaItem);
                            break;
                        case 18:
                            unitOfWork.ApplicationClassification.Delete(MetaItem);
                            break;
                        case 19:
                            unitOfWork.ExternalHosting.Delete(MetaItem);
                            break;
                        case 20:
                            unitOfWork.APRAppRedundancy.Delete(MetaItem);
                            break;
                        case 21:
                            unitOfWork.APROwningCompany.Delete(MetaItem);
                            break;
                        case 22:
                            unitOfWork.DeploymentModel.Delete(MetaItem);
                            break;
                        case 23:
                            unitOfWork.StandardStatusModel.Delete(MetaItem);
                            break;
                        case 24:
                            unitOfWork.DifferentiationLevelModel.Delete(MetaItem);
                            break;
                        case 25:
                            unitOfWork.APRInterfaceMethod.Delete(MetaItem);
                            break;
                        case 26:
                            unitOfWork.APRDispositionSolution.Delete(MetaItem);
                            break;
                    }
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static IList<PortfolioModel> GetPortfolioDetails(int? ClientId)
        {
            IList<PortfolioModel> Portfolio = new List<PortfolioModel>();
            var entityMapper = new Mapper<PortfolioModel, LineOfBusinesses>();

            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<LineOfBusinesses> PortfoliofoDetail = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                if (ClientId != null && ClientId != 0)
                    PortfoliofoDetail = unitOfWork.LineOfBusinesses.Get(cid => cid.ClientId == ClientId).ToList();

                if (PortfoliofoDetail != null)
                {
                    foreach (var _portfoliodetail in PortfoliofoDetail)
                    {
                        PortfolioModel PortfolioViewModel = new PortfolioModel();
                        PortfolioViewModel.LOBName = _portfoliodetail.LOBName;
                        PortfolioViewModel.LOBId = _portfoliodetail.Id;
                        PortfolioViewModel.ClientId = _portfoliodetail.ClientId;
                        Portfolio.Add(PortfolioViewModel);
                    }
                }
                return Portfolio;
            }

        }

        public static IList<PortfolioAppModel> GetPortfolioAppDetails(int portFolioId)
        {
            Db db = new Db();
            var portFolioApp = (from papps in db.ApplicationLOBJunction
                                join apps in db.Applications on papps.AppId equals apps.Id
                                join lobs in db.LineOfBusiness on papps.LOBId equals lobs.Id
                                where papps.LOBId == portFolioId
                                select new PortfolioAppModel
                                {
                                    AppId = apps.Id,
                                    ApplicationAliasName = apps.AliasName,
                                    ApplicationName = apps.ApplicationName,
                                    ClientApplicationNumber = apps.ClientApplicationNumber,
                                    LobAppId = papps.Id,
                                    PortFolioName = lobs.LOBName,
                                    PortFolioId = lobs.Id

                                }
                                ).ToList();

            return portFolioApp;
        }

        public static int CreatePortfolioApps(PortfolioAppModel Portfolioinput)
        {
            int id = 0;
            try
            {

                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    ApplicationLOBJunctions appPortfolioApp = new ApplicationLOBJunctions();
                    appPortfolioApp.AppId = Portfolioinput.AppId;
                    appPortfolioApp.LOBId = Portfolioinput.PortFolioId;
                    unitOfWork.ApplicationLOBJunctions.Insert(appPortfolioApp);
                    unitOfWork.Save();
                    id = appPortfolioApp.Id;

                }
            }
            catch (Exception)
            {
                return 0;
            }
            return id;
        }


        public static int CreatePortfolioDetails(PortfolioModel Portfolioinput)
        {
            int id = 0;
            try
            {

                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    LineOfBusinesses appPortfolioinfo = new LineOfBusinesses();
                    appPortfolioinfo.LOBName = Portfolioinput.LOBName;
                    appPortfolioinfo.ClientId = Portfolioinput.ClientId;
                    unitOfWork.LineOfBusinesses.Insert(appPortfolioinfo);
                    unitOfWork.Save();
                    id = appPortfolioinfo.Id;

                }
            }
            catch (Exception)
            {
                return 0;
            }
            return id;
        }

        public static bool UpdatePortfolioInfo(PortfolioModel Portfolioinput)
        {
            try
            {
                var entityMapper = new Mapper<LineOfBusinesses, PortfolioModel>();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var appPortfolioinfo = unitOfWork.LineOfBusinesses.GetByID(Portfolioinput.LOBId);
                    // entityMapper.MapToEntity(Portfolioinput, appPortfolioinfo);
                    appPortfolioinfo.LOBName = Portfolioinput.LOBName;
                    unitOfWork.LineOfBusinesses.Update(appPortfolioinfo);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool UpdatePortfolioApps(PortfolioAppModel Portfolioinput)
        {
            try
            {

                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var appPortfolioinfo = unitOfWork.ApplicationLOBJunctions.GetByID(Portfolioinput.LobAppId);
                    // entityMapper.MapToEntity(Portfolioinput, appPortfolioinfo);
                    appPortfolioinfo.LOBId = Portfolioinput.PortFolioId;
                    appPortfolioinfo.AppId = Portfolioinput.AppId;
                    unitOfWork.ApplicationLOBJunctions.Update(appPortfolioinfo);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool DeletePortfolio(int portfolioId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.LineOfBusinesses.Delete(portfolioId);
                unitOfWork.Save();

                //TempData["SLACreatedDeleted"] = "Deleted successfully.";
                return true;
            }
        }

        public static bool DeletePortfolioApp(int portfolioAppId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.ApplicationLOBJunctions.Delete(portfolioAppId);
                unitOfWork.Save();
                return true;
            }
        }

        public static bool DeleteAppFinancial(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.APRAppFinancial.Delete(Id);
                unitOfWork.Save();

                //TempData["SLACreatedDeleted"] = "Deleted successfully.";
                return true;
            }
        }

        public static int InsertPersonnel(PersonnelModel Personnel)
        {
            try
            {
                var entityMapper = new Mapper<Personnel, PersonnelModel>();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    Personnel newitem = new Personnel();
                    entityMapper.MapToEntity(Personnel, newitem);
                    unitOfWork.Personnel.Insert(newitem);
                    unitOfWork.Save();
                    //you will get inserted id
                    return newitem.Id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool UpdatePersonnel(PersonnelModel Personnel)
        {
            try
            {
                var entityMapper = new Mapper<Personnel, PersonnelModel>();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.Personnel.GetByID(Personnel.Id);
                    entityMapper.MapToEntity(Personnel, updatetitem);
                    unitOfWork.Personnel.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool DeletePersonnel(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.Personnel.Delete(Id);
                unitOfWork.Save();


                return true;
            }
        }

        public static int InsertPersonnelRole(PersonnelRoleModel PersonnelRole)
        {
            try
            {
                var entityMapper = new Mapper<PersonnelRole, PersonnelRoleModel>();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    PersonnelRole newitem = new PersonnelRole();
                    entityMapper.MapToEntity(PersonnelRole, newitem);
                    unitOfWork.PersonnelRole.Insert(newitem);
                    unitOfWork.Save();
                    //you will get inserted id
                    return newitem.Id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool UpdatePersonnelRole(PersonnelRoleModel PersonnelRole)
        {
            try
            {
                var entityMapper = new Mapper<PersonnelRole, PersonnelRoleModel>();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.PersonnelRole.GetByID(PersonnelRole.Id);
                    entityMapper.MapToEntity(PersonnelRole, updatetitem);
                    unitOfWork.PersonnelRole.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool DeletePersonnelRole(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.PersonnelRole.Delete(Id);
                unitOfWork.Save();


                return true;
            }
        }



        public static IList<AprInterfaceModel> getall(int appId, string interfacetype)
        {

            IList<AprInterfaceModel> aprInterfaceModels = new List<AprInterfaceModel>();
            //PagedList<TicketInfoViewModel> ticketInfoDetailViewModels = new PagedList<TicketInfoViewModel>();
            var entityMapper = new Mapper<AprInterfaceModel, APRAppInterface>();

            using (var contextFactory = new DbContextFactory())
            {
               // IEnumerable<APRAppInterface> aprappinterlist = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var db = new Db();
                var interfacelist = (from i in db.APRAppInterface
                                     join app in db.Applications on i.ApplicationInterfaced equals app.Id
                                     where i.AppId == appId && i.InterfaceType == interfacetype
                                     select new { Id = i.Id, AppId = i.AppId, ApplicationInterfaced = i.ApplicationInterfaced, AppInterfacedText = app.ApplicationName }).ToList();

                //select new  { Id = i.Id, AppId = i.AppId, ApplicationInterfaced = i.ApplicationInterfaced, AppInterfacedValue = i.ApplicationInterfaced, AppInterfacedText = app.ApplicationName  }).ToList();
                //select new { appid = i.ApplicationInterfaced, interfaceApp = app.ApplicationName });
                //Id = i.Id, AppId = i.AppId, ApplicationInterfaced = i.ApplicationInterfaced, appinfo = new SelectListItem { Value = i.ApplicationInterfaced.ToString(), Text = app.ApplicationName }

                // aprappinterlist = unitOfWork.APRAppInterface.Get().Where(p=>p.AppId==appId && p.InterfaceType==interfacetype).ToList();  

                foreach (var app in interfacelist)
                {
                    AprInterfaceModel pai = new AprInterfaceModel();
                    pai.Id = app.Id;
                    pai.AppId = app.AppId;
                    pai.ApplicationInterfaced = app.ApplicationInterfaced;
                    pai.InterfaceType = interfacetype;
                    pai.strApplicationInterfaced = app.ApplicationInterfaced.ToString();
                    pai.ApplicationInterfacedText = app.AppInterfacedText;
                    pai.appinfo = new SelectListItem();
                    pai.appinfo.Value = app.ApplicationInterfaced.ToString();
                    pai.appinfo.Text = app.AppInterfacedText;
                    //pai.appinfo = new SelectListItem { Value = app.ApplicationInterfaced.ToString(), Text = app.AppInterfacedText };
                    aprInterfaceModels.Add(pai);
                }

                //if (aprappinterlist != null)
                //{
                //    foreach (var app in aprappinterlist)
                //    {
                //        AprInterfaceModel aprInterModel = new AprInterfaceModel();
                //        entityMapper.MapToEntity(app, aprInterModel);
                //        aprInterModel = unitOfWork.APRAppInterface.GetByID(appId);
                //        aprInterfaceModels.Add(aprInterModel);
                //    }
                //}
                return aprInterfaceModels;
                //return interfacelist;
            }
        }

        public static int AddInterface(AprInterfaceModel interfaceinput)
        {
            int id = 0;
            try
            {
                var entityMapper = new Mapper<APRAppInterface, AprInterfaceModel>();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    bool any = unitOfWork.APRAppInterface.Get(g => g.AppId == interfaceinput.AppId && g.InterfaceType == interfaceinput.InterfaceType && g.ApplicationInterfaced == interfaceinput.ApplicationInterfaced).Any();
                    if (!any)
                    {
                        APRAppInterface appinterfaceinfo = new APRAppInterface();
                        entityMapper.MapToEntity(interfaceinput, appinterfaceinfo);
                        // var check = unitOfWork.APRAppInterface.GetByID(interfaceinput.AppId).InterfaceType.Any();
                        //var z = unitOfWork.APRAppInterface.GetByID(interfaceinput.AppId);

                        unitOfWork.APRAppInterface.Insert(appinterfaceinfo);
                        unitOfWork.Save();
                        id = appinterfaceinfo.Id;
                    }
                }
            }

            catch (Exception)
            {
                id = 0;
            }
            return id;
        }


        public static bool UpdateInterface(AprInterfaceModel interfaceupdate)
        {
            try
            {
                //   var entityMapper = new Mapper<APRAppInterface, AprInterfaceModel>();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var appinterfaceinfo = unitOfWork.APRAppInterface.GetByID(interfaceupdate.Id);
                    appinterfaceinfo.AppId = interfaceupdate.AppId;
                    appinterfaceinfo.ApplicationInterfaced = interfaceupdate.ApplicationInterfaced;
                    appinterfaceinfo.InterfaceType = interfaceupdate.InterfaceType;
                    //    entityMapper.MapToEntity(interfaceupdate, appinterfaceinfo);
                    unitOfWork.APRAppInterface.Update(appinterfaceinfo);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool DeleteInterface(int appInterfaceId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.APRAppInterface.Delete(appInterfaceId);
                unitOfWork.Save();

                //TempData["SLACreatedDeleted"] = "Deleted successfully.";
                return true;
            }
        }



        public static IList<APRHostlistModel> GetHostlist(int? ClientId)
        {
            IList<APRHostlistModel> Hosts = new List<APRHostlistModel>();
            var entityMapper = new Mapper<APRHostlistModel, APRHostList>();

            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<APRHostList> Hostlist = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                if (ClientId != null && ClientId != 0)
                    Hostlist = unitOfWork.APRHostList.Get(cid => cid.ClientId == ClientId).ToList();

                if (Hostlist != null)
                {
                    foreach (var v in Hostlist)
                    {
                        APRHostlistModel PortfolioViewModel = new APRHostlistModel();
                        PortfolioViewModel.HostName = v.HostName;

                        PortfolioViewModel.ClientId = v.ClientId;
                        Hosts.Add(PortfolioViewModel);
                    }
                }
                return Hosts;
            }

        }


        //public static APRHostlistModel GetAppHost(int HostId)
        //{


        //    using (var contextFactory = new DbContextFactory())
        //    {

        //        var entityMapper = new Mapper<APRHostlistModel, APRHostList>();


        //        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
        //        var HostInfo = unitOfWork.APRHostList.GetByID(HostId);
        //        var aPRHostlistModel = new APRHostlistModel();
        //        entityMapper.MapToEntity(HostInfo, aPRHostlistModel);
        //        // AppViewModels.Add(ApplicationInformationModel);
        //        return aPRHostlistModel;


        //    }



        // }

        public static int InsertOrUpdateHostlist(APRHostlistModel Host)
        {
            try
            {
                var entityMapper = new Mapper<APRHostList, APRHostlistModel>();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var newitem = new APRHostList();
                    entityMapper.MapToEntity(Host, newitem);
                    if (newitem.Id > 0)
                        // dispInfo = unitOfWork.APRAppDisposition.GetByID(DispositionInfoinput.Id);
                        unitOfWork.APRHostList.Update(newitem);
                    else

                        unitOfWork.APRHostList.Insert(newitem);
                    unitOfWork.Save();
                    //you will get inserted id
                    return newitem.Id;
                }
            }
            catch (Exception)
            {
                return 0;
            }



        }


        public static int InsertAppHardware(APRAppHardwareModel AppHardwareinput)
        {
            try
            {
                var entityMapper = new Mapper<APRAppHardware, APRAppHardwareModel>();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    APRAppHardware appHardwareinfo = new APRAppHardware();
                    entityMapper.MapToEntity(AppHardwareinput, appHardwareinfo);
                    unitOfWork.APRAppHardware.Insert(appHardwareinfo);
                    unitOfWork.Save();
                    //you will get inserted id
                    return appHardwareinfo.Id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool UpdateAppHardware(APRAppHardwareModel AppHardwareinput)
        {
            try
            {
                var entityMapper = new Mapper<APRAppHardware, APRAppHardwareModel>();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var appHardwareinfo = unitOfWork.APRAppHardware.GetByID(AppHardwareinput.Id);
                    entityMapper.MapToEntity(AppHardwareinput, appHardwareinfo);
                    unitOfWork.APRAppHardware.Update(appHardwareinfo);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static bool DeleteAppHardware(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.APRAppHardware.Delete(Id);
                unitOfWork.Save();

                //TempData["SLACreatedDeleted"] = "Deleted successfully.";
                return true;
            }
        }


        public static IEnumerable<Vendor> Vendors
        {
            get
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var result = unitOfWork.Vendor.Get().OrderBy(x => x.VendorName).ToList();


                return result;
            }
        }

        public static IEnumerable<VendorProduct> VendorProducts
        {
            get
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var result = unitOfWork.VendorProduct.Get().ToList();


                return result;
            }
        }


        public static IEnumerable<VendorProductVersion> VendorProductVersions
        {
            get
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var result = unitOfWork.VendorProductVersion.Get().ToList();


                return result;
            }
        }

        public static IEnumerable<VendorProductVersionModel> GetVendorProductVersions(int ProductId)
        {
            List<VendorProductVersionModel> prodlist = new List<VendorProductVersionModel>();
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var result = unitOfWork.VendorProductVersion.Get().Where(x => x.ProductId == ProductId).OrderBy(x => x.ProductVersionName).ToList();

            foreach (var prod in result)
            {
                VendorProductVersionModel prodModel = new VendorProductVersionModel();
                prodModel.ProductId = prod.Id;
                prodModel.ProductVersionName = prod.ProductVersionName;
                prodModel.VersionId = prod.Id;
                prodlist.Add(prodModel);
            }


            return prodlist;

        }

        public static IEnumerable<VendorProductModel> GetVendorProducts(int vendorId)
        {
            List<VendorProductModel> prodlist = new List<VendorProductModel>();
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var result = unitOfWork.VendorProduct.Get().Where(x => x.VendorId == vendorId).OrderBy(x => x.ProductName).ToList();

            foreach (var prod in result)
            {
                VendorProductModel prodModel = new VendorProductModel();
                prodModel.ProductId = prod.Id;
                prodModel.ProductName = prod.ProductName;
                prodModel.VendorId = prod.VendorId;
                prodlist.Add(prodModel);
            }


            return prodlist;

        }

        public static IEnumerable<VendorProductVersionModel> GetVendorProductVersion(int productId)
        {
            List<VendorProductVersionModel> versList = new List<VendorProductVersionModel>();
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var result = unitOfWork.VendorProductVersion.Get().Where(x => x.ProductId == productId).ToList();

            foreach (var ver in result)
            {
                VendorProductVersionModel verModel = new VendorProductVersionModel();
                verModel.VersionId = ver.Id;
                verModel.ProductId = ver.ProductId;
                verModel.ProductVersionName = ver.ProductVersionName;
                versList.Add(verModel);
            }

            return versList;

        }


        public static int InsertAppSoftware(APRAppSoftwareModel AppSoftwareinput)
        {
            try
            {
                var entityMapper = new Mapper<APRAppSoftware, APRAppSoftwareModel>();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    APRAppSoftware appSoftwareinfo = new APRAppSoftware();
                    entityMapper.MapToEntity(AppSoftwareinput, appSoftwareinfo);
                    unitOfWork.APRAppSoftware.Insert(appSoftwareinfo);
                    unitOfWork.Save();
                    //you will get inserted id
                    return appSoftwareinfo.Id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }


        public static bool UpdateAppSoftware(APRAppSoftwareModel AppSoftwareinput)
        {
            try
            {
                var entityMapper = new Mapper<APRAppSoftware, APRAppSoftwareModel>();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var appSoftwareinfo = unitOfWork.APRAppSoftware.GetByID(AppSoftwareinput.Id);
                    entityMapper.MapToEntity(AppSoftwareinput, appSoftwareinfo);
                    unitOfWork.APRAppSoftware.Update(appSoftwareinfo);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static bool DeleteAppSoftware(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.APRAppSoftware.Delete(Id);
                unitOfWork.Save();

                //TempData["SLACreatedDeleted"] = "Deleted successfully.";
                return true;
            }
        }

        public static bool DeleteHostlist(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.APRHostList.Delete(Id);
                unitOfWork.Save();

                //TempData["SLACreatedDeleted"] = "Deleted successfully.";
                return true;
            }
        }


        public static IList<VendorMetadataModel> GetVendorlist()
        {
            IList<VendorMetadataModel> vendors = new List<VendorMetadataModel>();
            var entityMapper = new Mapper<VendorMetadataModel, Vendor>();

            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<Vendor> vendorlist = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                vendorlist = unitOfWork.Vendor.Get().ToList();

                if (vendorlist != null)
                {
                    foreach (var v in vendorlist)
                    {
                        VendorMetadataModel vendorModel = new VendorMetadataModel();
                        vendorModel.VendorName = v.VendorName;
                        vendorModel.VId = v.Id;


                        vendors.Add(vendorModel);
                    }
                }
                return vendors;
            }

        }

        public static int CreateVendorlist(VendorMetadataModel vendorInput)
        {
            int id = 0;
            try
            {

                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    Vendor v = new Vendor();
                    v.VendorName = vendorInput.VendorName;

                    unitOfWork.Vendor.Insert(v);
                    unitOfWork.Save();
                    id = v.Id;

                }
            }
            catch (Exception)
            {
                return 0;
            }
            return id;
        }


        public static bool UpdateVendor(VendorMetadataModel vendorInput)
        {
            try
            {
                var entityMapper = new Mapper<Vendor, VendorMetadataModel>();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var v = unitOfWork.Vendor.GetByID(vendorInput.VId);
                    // entityMapper.MapToEntity(Portfolioinput, appPortfolioinfo);
                    v.VendorName = vendorInput.VendorName;
                    unitOfWork.Vendor.Update(v);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static bool DeleteVendor(int vendorId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.Vendor.Delete(vendorId);
                unitOfWork.Save();

                //TempData["SLACreatedDeleted"] = "Deleted successfully.";
                return true;
            }
        }

        public static int CreateProductVerion(VendorProductVersionModel verInput)
        {
            int id = 0;
            try
            {

                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    VendorProductVersion v = new VendorProductVersion();
                    v.ProductId = verInput.ProductId;
                    v.ProductVersionName = verInput.ProductVersionName;
                    unitOfWork.VendorProductVersion.Insert(v);
                    unitOfWork.Save();
                    id = v.Id;

                }
            }
            catch (Exception)
            {
                return 0;
            }
            return id;
        }

        public static int CreateProduct(VendorProductModel productInput)
        {
            int id = 0;
            try
            {

                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    VendorProduct v = new VendorProduct();
                    v.VendorId = productInput.VendorId;
                    v.ProductName = productInput.ProductName;
                    unitOfWork.VendorProduct.Insert(v);
                    unitOfWork.Save();
                    id = v.Id;

                }
            }
            catch (Exception)
            {
                return 0;
            }
            return id;
        }


        public static bool UpdateProduct(VendorProductModel productInput)
        {
            try
            {

                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var v = unitOfWork.VendorProduct.GetByID(productInput.ProductId);
                    // entityMapper.MapToEntity(Portfolioinput, appPortfolioinfo);
                    v.VendorId = productInput.VendorId;
                    v.ProductName = productInput.ProductName;
                    unitOfWork.VendorProduct.Update(v);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool UpdateProductVersion(VendorProductVersionModel verInput)
        {
            try
            {

                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var v = unitOfWork.VendorProductVersion.GetByID(verInput.VersionId);
                    // entityMapper.MapToEntity(Portfolioinput, appPortfolioinfo);
                    v.ProductId = verInput.ProductId;
                    v.ProductVersionName = verInput.ProductVersionName;
                    unitOfWork.VendorProductVersion.Update(v);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool DeleteProduct(int productId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.VendorProduct.Delete(productId);
                unitOfWork.Save();

                //TempData["SLACreatedDeleted"] = "Deleted successfully.";
                return true;
            }
        }

        public static bool DeleteProductVersion(int VerId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.VendorProductVersion.Delete(VerId);
                unitOfWork.Save();
                return true;
            }
        }


        public static IList<RatingBySubperspective> APRGetAppsForPortfoli(string Portfolis)
        {
            IList<RatingBySubperspective> list = new List<RatingBySubperspective>();
            var paramName = new string[] { "Portfolis" };
            var paramValue = new object[] { Portfolis };


            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("APRGetApplicationListByPortfolio", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new RatingBySubperspective();


                    coldet.ApplicationName = dr["ApplicationName"].ToString();
                    coldet.check = true;

                    try
                    {
                        coldet.AppId = Convert.ToInt32(dr["Id"]);
                    }

                    catch { }
                    list.Add(coldet);
                }

            }
            return list;
        }



        public static RatingBySubperspective APRGetPerspectiveTypesBySurvey(string ClientId, string SurveyId)
        {
            List<string> list = new List<string>();
            RatingBySubperspective supperspective = new RatingBySubperspective();
            var paramName = new string[] { "ClientId", "SurveyId" };
            var paramValue = new object[] { ClientId, SurveyId };


            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("APRGetPerspectiveTypesBySurvey", paramName, paramValue);

                foreach (DataRow dr in res.Rows)
                {
                    string perspectivetype = null;


                    perspectivetype = dr["PerspectiveType"].ToString();
                    //// coldet.check = true;

                    try
                    {
                        // coldet.AppId = Convert.ToInt16(dr["Id"]);
                    }

                    catch { }
                    // list.PerspectiveTypelist.Add(dr["PerspectiveType"].ToString());
                    list.Add(perspectivetype);
                }
                supperspective.surveyId = Convert.ToInt32(SurveyId);
                supperspective.PerspectiveTypelist = list;
            }
            return supperspective;
        }



        public static IList<RatingPerspReport> APRSubperspectiveReport(string PerspectiveX, string PerspectiveY, string ApplicationID, string ClientId, string SurveyId)
        {
            IList<RatingPerspReport> list = new List<RatingPerspReport>();
            var paramName = new string[] { "PerspectiveX", "PerspectiveY", "ApplicationID", "ClientId", "SurveyId" };
            var paramValue = new object[] { PerspectiveX, PerspectiveY, ApplicationID, ClientId, SurveyId };


            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("SpSurveyByPerspectiveValue", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new RatingPerspReport();


                    coldet.ApplicationName = dr["ApplicationName"].ToString();
                    coldet.X = dr["X"].ToString();
                    coldet.Y = dr["Y"].ToString();

                    try
                    {
                        coldet.Xrating = Convert.ToDecimal(dr["Xrating"]);
                        coldet.Yrating = Convert.ToDecimal(dr["Yrating"]);
                    }

                    catch { }
                    list.Add(coldet);
                }

            }
            return list;
        }


        public static IList<DispositionRoadMapModel> APRAppDispositionRoadmap(int ClientId)
        {
            IList<DispositionRoadMapModel> Modellist = new List<DispositionRoadMapModel>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("APRAppDispositionRoadmap", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DispositionRoadMapModel();

                    coldet.ApplicationName = dr["ApplicationName"].ToString();
                    coldet.DispositionName = dr["DispositionName"].ToString();
                    coldet.DispositionRationaleType = dr["DispositionRationaleType"].ToString();
                    coldet.DispositionActionType = dr["DispositionActionType"].ToString();
                    coldet.Color = dr["Color"].ToString();
                    coldet.ApplicationClassificationName = dr["ApplicationClassificationName"].ToString();
                    coldet.ClientApplicationNumber = dr["ClientApplicationNumber"].ToString();
                    try
                    {
                        if (!string.IsNullOrEmpty(dr["InScope"].ToString()))
                        {
                            coldet.InScope = Convert.ToBoolean(dr["InScope"]);
                        }

                        if (!string.IsNullOrEmpty(dr["OriginalRetirementDate"].ToString()))
                        {
                            coldet.OriginalRetirementDate = Convert.ToDateTime(dr["OriginalRetirementDate"]);
                        }
                        if (!string.IsNullOrEmpty(dr["PlannedDispositionDate"].ToString()))
                        {
                            coldet.PlannedDispositionDate = Convert.ToDateTime(dr["PlannedDispositionDate"]);
                        }

                        if (!string.IsNullOrEmpty(dr["RoadmapTargetYear"].ToString()))
                        {
                            coldet.RoadmapTargetYear = Convert.ToInt32(dr["RoadmapTargetYear"]);
                        }

                        if (!string.IsNullOrEmpty(dr["Year"].ToString()))
                        {
                            coldet.Year = Convert.ToInt32(dr["Year"]);
                        }

                        if (!string.IsNullOrEmpty(dr["Totalcost"].ToString()))
                        {
                            coldet.Totalcost = Convert.ToDecimal(dr["Totalcost"]);
                        }


                    }

                    catch { }
                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }



        public static List<PerspectiveTypeRatingSummary> perspectiveTypeRating(int SurveyId, int PerspectiveId, int AppId)
        {
            var ratList = new List<PerspectiveTypeRatingSummary>();
            var paramName = new string[] { "SurveyId", "PespectiveId", "AppId" };
            var paramValue = new object[] { SurveyId, PerspectiveId, AppId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("APRSurveyPerspectiveReport", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new PerspectiveTypeRatingSummary();

                    coldet.Hint = dr["Hint"].ToString();
                    coldet.OriginalRating = dr["OriginalRating"].ToString();
                    coldet.FinalRating = dr["FinalRating"].ToString();
                    ratList.Add(coldet);
                }

            }
            return ratList;
        }

        //Client Access Control --- Linkning Client to Client -- Merger/acquisition
        public static void DeleteClientLink(int ClientLinkId, int uid)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var entity = unitOfWork.ClientAccessControl.GetByID(ClientLinkId);
                entity.IsDeleted = true;
                entity.UpdatedBy = uid;
                entity.UpdatedDt = DateTime.Now;
                unitOfWork.ClientAccessControl.Update(entity);
                unitOfWork.Save();


            }
        }



        public static IEnumerable<AssessmentModel> FetchAssessDetails(int SurveyId, int AppId, int ClientId, int uid)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            IEnumerable<AssessmentModel> returnList = new List<AssessmentModel>();
            Db db = new Db();
            var PersonnelId = 0;
            var RatingUser = unitOfWork.Personnel.Get(g => g.PersonnelName.ToLower() == "aprratinguser").FirstOrDefault();
            var IndexForSurveyCollection = false;
            var APRSurUserID = 0;
            //Or should add Client Name.... like 'Client'+'RatingUser'/ ClientId
            if (RatingUser == null)
            {
                Personnel entity = new Personnel()
                {
                    PersonnelName = "APRRatingUser",
                    FirstName = "APR",
                    LastName = "RatingUser"
                };
                unitOfWork.Personnel.Insert(entity);
                unitOfWork.Save();
                PersonnelId = entity.Id;
            }
            else
            {
                PersonnelId = RatingUser.Id;
            }

            if (SurveyId != 0 && PersonnelId != 0)//&& AppId != 0 
            {
                //Check any Rating is there or not!!!
                var SurUserID = 0;
                bool AnyUserRatings;
                var SurUser = unitOfWork.SurveyUser.Get(g =>  g.PersonnelId == PersonnelId && g.SurveyId == SurveyId).FirstOrDefault();//g.SurveyForRefId == AppId &&

                if (SurUser == null)
                {
                    SurveyUser suEntity = new SurveyUser()
                    {
                        PersonnelId = PersonnelId,
                        SurveyForRefId = AppId,
                        SurveyId = SurveyId,
                        CreatedBy = uid,
                        UpdatedBy = uid,
                        Createddt = DateTime.Now,
                        //This is removed becuase - To get the User Ratigs, who has submitted lately
                        //updateddt = DateTime.Now,
                        UserSurveyToken = "xx",
                        Completedt = DateTime.Now
                    };
                    unitOfWork.SurveyUser.Insert(suEntity);
                    unitOfWork.Save();
                    SurUserID = suEntity.Id;
                }
                else
                {
                    SurUserID = SurUser.Id;
                }


                //Check ratings from any Survey Participants lately---BY checking the UpdatedDate in SurveyUser Table
                //var surParticipant = unitOfWork.SurveyUser.Get(e => e.SurveyForRefId == AppId && e.SurveyId == SurveyId && e.PersonnelId != RatingUser.Id
                //                                                // && e.updateddt > SurUser.updateddt 
                //                                                && e.Completedt != null)
                //                                            .OrderByDescending(f => f.updateddt).FirstOrDefault();
                var surParticipant = unitOfWork.SurveyUser.Get(e =>e.SurveyId == SurveyId && e.PersonnelId != RatingUser.Id
                                                                // && e.updateddt > SurUser.updateddt 
                                                                && e.Completedt != null)
                                                            .OrderByDescending(f => f.updateddt).FirstOrDefault();
                if (surParticipant != null)
                {
                    AnyUserRatings = db.SurveyCollection.Where(e => e.SurveyUserId == surParticipant.Id).Any(e => e.FinalRating != null);
                }
                else
                {
                    AnyUserRatings = false;
                }

                if (SurUserID != 0)
                {

                    //Get APRSurveyUser's rows set from SurveyCollection
                    var AprSurUser = unitOfWork.SurveyUser.Get(g => g.PersonnelId == PersonnelId && //g.SurveyForRefId == AppId &&
                                                                   g.SurveyId == SurveyId).FirstOrDefault();
                    if (AprSurUser != null)
                        APRSurUserID = AprSurUser.Id;

                    //Data for Reference -fetch data for the above SurveyUser
                    var RefSurUser = unitOfWork.SurveyUser.GetByID(SurUserID);

                    //This query checks only the entries in surveyCollection : not the Original/FinalRatings
                    var CollectionThere = unitOfWork.SurveyCollection.Get(g => g.SurveyUserId == SurUserID);
                    //This below v=condition is for "SurveyUser"
                    var IsRatingPresentForSurveyUser = CollectionThere.Any(f => f.FinalRating != null);
                    //Already rows present in Collection
                    if (CollectionThere.Any())
                    {
                        //when there is no ratings entered by SurveyUser
                        if (!IsRatingPresentForSurveyUser)
                        {
                            //any ratings entered by other users - is its there
                            if (surParticipant != null)
                            {
                                SurUserID = surParticipant.Id;
                                IndexForSurveyCollection = true;
                            }
                        }
                        //when ratings entered by SurveyUser
                        else
                        {//2cases--
                            //1. When other user ratings present
                            if (surParticipant != null)
                            {//Check dates- who entered the latest
                                //When other user updated lately, then assigned his ratings
                                if (RefSurUser.updateddt < surParticipant.updateddt)
                                {
                                    SurUserID = surParticipant.Id;
                                    IndexForSurveyCollection = true;
                                }
                                ////Do nothing 
                                //else
                                //{

                                //}

                            }
                            //2.when other user ratings not present
                            //else
                            //{
                            //    //Do nothing
                            //}
                        }

                        //Fetch the data from Collection -Existing - to interchange the Ids-Rows' Ids
                        var APRSurUserList = unitOfWork.SurveyCollection.Get(g => g.SurveyUserId == APRSurUserID);

                        //With Linq query
                        var tempList = (from SM in db.SurveyModel
                                        where SM.SurveyId == SurveyId
                                        join ASS in db.SurveyCollection on SM.Id equals ASS.SurveyModelId into tempAssess
                                        from Assess in tempAssess.DefaultIfEmpty()
                                        join SU in db.SurveyUser  on Assess.SurveyUserId equals  SU.Id
                                        join Apps in db.Applications on SU.SurveyForRefId equals Apps.Id
                                        where Assess.SurveyUserId == SurUserID
                                        select new
                                        {
                                            ApplicationId = AppId,
                                            ApplicationName = Apps.ApplicationName,
                                            subPerspective = SM.Hint,
                                            Qid = SM.Id,
                                            Question = SM.Question,
                                            OriginalRating = Assess.OriginalRating,
                                            FinalRating = Assess.FinalRating,
                                            Comments = Assess.Comments,
                                            Options = (from op in db.SurveyModelRatingFactor
                                                       where op.SurveyModelId == SM.Id
                                                       orderby op.RatingFactorValue descending
                                                       select new
                                                       {
                                                           RatingText = op.RatingFactor,
                                                           RatingId = op.Id,
                                                           RatingContext=op.RatingContext
                                                       }),
                                            Id = Assess.Id
                                        }

                            ).ToList();

                        var returnTempList = tempList.AsEnumerable().Select(e => new AssessmentModel
                        {
                            Id = e.Id,
                            ApplicationName = e.ApplicationName,
                            subPerspective = e.subPerspective,
                            SurveyModelId = e.Qid,
                            Question = e.Question,
                            OriginalRating = e.OriginalRating,
                            FinalRating = e.FinalRating,
                            Comments = e.Comments,
                            QuestionOptions = e.Options.ToList().Select(f => new ratingMod
                            {
                                Id = f.RatingId,
                                Rating = f.RatingText,
                                RatingContext=f.RatingContext
                            }),
                            RAApplicationid = e.ApplicationId
                        }).ToList();

                        if (IndexForSurveyCollection)
                        {
                            foreach(var dest in returnTempList)
                            {
                                var r = APRSurUserList.Where(f => f.SurveyModelId == dest.SurveyModelId).FirstOrDefault();
                                if (r != null)
                                    dest.Id = r.Id;
                            }
                        }
                        returnList = returnTempList.AsEnumerable();
                    }
                    //When there is no rows present in Collection
                    else
                    {
                        var tempList = (from SM in db.SurveyModel
                                        where SM.SurveyId == SurveyId
                                        select new
                                        {
                                            SurveyModelId = SM.Id
                                        }

                            ).ToList();

                        //After fetching the SurveyQuestions, write it to database.
                        foreach (var ra in tempList)
                        {
                            var entity = new SurveyCollection();
                            entity.SurveyUserId = SurUserID;
                            entity.SurveyModelId = ra.SurveyModelId;
                            //entity.OriginalRating = ra.OriginalRating;
                            //entity.FinalRating = ra.FinalRating;
                            //entity.Comments = ra.Comments;
                            entity.Createddt = DateTime.Now;
                            entity.updateddt = DateTime.Now;
                            unitOfWork.SurveyCollection.Insert(entity);
                        }
                        unitOfWork.Save();


                        //Fetch the data from Collection -Existing - to interchange the Ids-Rows' Ids
                        var APRSurUserList = unitOfWork.SurveyCollection.Get(g => g.SurveyUserId == SurUserID);

                        //1. When other user ratings present
                        if (surParticipant != null)
                        {
                            SurUserID = surParticipant.Id;
                        }
                        //2.when other user ratings not present
                        //else
                        //{
                        //    //Do nothing
                        //}

                        //Read agan with joins to Questions and Ratings
                        var AssessmentList = (from SM in db.SurveyModel
                                              where SM.SurveyId == SurveyId
                                              join ASS in db.SurveyCollection on SM.Id equals ASS.SurveyModelId into tempAssess
                                              from Assess in tempAssess.DefaultIfEmpty()
                                              join SU in db.SurveyUser on Assess.SurveyUserId equals SU.Id
                                              join Apps in db.Applications on SU.SurveyForRefId equals Apps.Id
                                              where Assess.SurveyUserId == SurUserID
                                              select new
                                              {
                                                  ApplicationId = AppId,
                                                  ApplicationName = Apps.ApplicationName,
                                                  subPerspective = SM.Hint,
                                                  SurveyModelId = SM.Id,
                                                  Question = SM.Question,
                                                  OriginalRating = Assess.OriginalRating,
                                                  FinalRating = Assess.FinalRating,
                                                  Comments = Assess.Comments,
                                                  Options = (from op in db.SurveyModelRatingFactor
                                                             where op.SurveyModelId == SM.Id
                                                             orderby op.RatingFactorValue descending
                                                             select new
                                                             {
                                                                 RatingText = op.RatingFactor,
                                                                 RatingId = op.Id
                                                             }),
                                                  Id = Assess.Id
                                              }

                            ).ToList();

                        var returnTempList = AssessmentList.Select(e => new AssessmentModel
                        {
                            Id = e.Id,
                            ApplicationName = e.ApplicationName,
                            subPerspective = e.subPerspective,
                            SurveyModelId = e.SurveyModelId,
                            Question = e.Question,
                            OriginalRating = e.OriginalRating,
                            FinalRating = e.FinalRating,
                            Comments = e.Comments,
                            QuestionOptions = e.Options.ToList().Select(f => new ratingMod
                            {
                                Id = f.RatingId,
                                Rating = f.RatingText
                            }),
                            RAApplicationid = e.ApplicationId
                        }).ToList();
                        
                        foreach (var dest in returnTempList)
                        {
                            var r = APRSurUserList.Where(f => f.SurveyModelId == dest.SurveyModelId).FirstOrDefault();
                            if (r != null)
                                dest.Id = r.Id;
                        }
                        
                        returnList = returnTempList.AsEnumerable();
                        
                                                
                    }
                }


            }

            return returnList;

        }
        
        public static IEnumerable<AssessmentModel> FetchDynamicAssessDetails(int SurveyId, int AppId, int ClientId, int uid)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            IEnumerable<AssessmentModel> returnList = new List<AssessmentModel>();
            Db db = new Db();
            //var PersonnelId = 0;
            //var IndexForSurveyCollection = false;
            if (SurveyId != 0 )//&& PersonnelId != 0)//&& AppId != 0 
            {
                //Check any Rating is there or not!!!
                //var SurUserID = 0;
                //bool AnyUserRatings;
                var surParticipant = unitOfWork.SurveyUser.Get(e => e.SurveyId == SurveyId //&& e.PersonnelId != RatingUser.Id
                                                                // && e.updateddt > SurUser.updateddt 
                                                                && e.Completedt != null)
                                                            .OrderByDescending(f => f.updateddt).FirstOrDefault();


                        //With Linq query
                        var tempList = (from SM in db.SurveyModel
                                        where SM.SurveyId == SurveyId
                                        join ASS in db.SurveyCollection on SM.Id equals ASS.SurveyModelId into tempAssess
                                        from Assess in tempAssess.DefaultIfEmpty()
                                        join SU in db.SurveyUser on Assess.SurveyUserId equals SU.Id
                                        join Apps in db.Applications on SU.SurveyForRefId equals Apps.Id
                                        join P in db.Personnels  on SU.PersonnelId equals P.Id
                                        join SMRF in db.SurveyModelRatingFactor on Assess.FinalRating equals SMRF.Id
                                        //  where Assess.SurveyUserId == SurUserID //  P.PersonnelName,
                                        select new
                                        {
                                            ApplicationId = AppId,
                                            ApplicationName = Apps.ApplicationName,
                                            PersonnelName = P.FirstName + " "+P.LastName,
                                            subPerspective = SM.Hint,
                                            Qid = SM.Id,
                                            Question = SM.Question,
                                            OriginalRating = Assess.OriginalRating,
                                            FinalRating =  Assess.FinalRating,
                                            Answer = SMRF.RatingFactor,
                                            Comments = Assess.Comments,
                                            Options = (from op in db.SurveyModelRatingFactor
                                                       where op.SurveyModelId == SM.Id
                                                       orderby op.RatingFactorValue descending
                                                       select new
                                                       {
                                                           RatingText = op.RatingFactor,
                                                           RatingId = op.Id,
                                                           RatingContext = op.RatingContext
                                                       }),
                                            Id = Assess.Id
                                        }

                            ).ToList();

                        var returnTempList = tempList.AsEnumerable().Select(e => new AssessmentModel
                        {
                            Id = e.Id,
                            ApplicationName = e.ApplicationName,
                            subPerspective = e.subPerspective,
                            PersonnelName = e.PersonnelName,
                            SurveyModelId = e.Qid,
                            Question = e.Question,
                            Answer = e.Answer,
                            OriginalRating = e.OriginalRating,
                            FinalRating = e.FinalRating,
                            Comments = e.Comments,
                            QuestionOptions = e.Options.ToList().Select(f => new ratingMod
                            {
                                Id = f.RatingId,
                                Rating = f.RatingText,
                                RatingContext = f.RatingContext
                            }),
                            RAApplicationid = e.ApplicationId
                        }).ToList();

                        returnList = returnTempList.AsEnumerable();
                    }
            
            return returnList;

        }
        public static void AssessUpdate(AssessmentModel model)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var exists = unitOfWork.SurveyCollection.GetByID(model.Id);
            if (exists != null && (exists.OriginalRating != model.OriginalRating || exists.FinalRating != model.FinalRating || exists.Comments != model.Comments))
            {
                exists.OriginalRating = model.OriginalRating;
                exists.FinalRating = model.FinalRating;
                exists.updateddt = DateTime.Now;
                exists.Comments = model.Comments;
                unitOfWork.SurveyCollection.Update(exists);
                unitOfWork.Save();
            }
        }

        /// <summary>
        /// To Update date for the Survey/Application ratings
        /// </summary>
        /// <param name="SurveyUserId"></param>
        public static void SurveyUserUpdateDate(int SurveyUserId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var exists = unitOfWork.SurveyUser.GetByID(SurveyUserId);
            exists.updateddt = DateTime.Now;
            unitOfWork.SurveyUser.Update(exists);
            unitOfWork.Save();

        }
        //get the surveyUserId from SurveyCollection
        public static int GetSurUserIdFromSCId(int SCollecId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var exists = unitOfWork.SurveyCollection.GetByID(SCollecId);
            return exists.SurveyUserId.Value;
        }


        public static Survey GetSurveyType(int Sid)
        {
            var contextfactory = new DbContextFactory();
            var unitofwork = new UnitOfWorks(contextfactory.GetContext());
            var check = unitofwork.Survey.GetByID(Sid);
            return check;
        }


        public static int InsertEcosystem(APREcoSystemModel input)
        {
            try
            {
                var entityMapper = new Mapper<APREcoSystem, APREcoSystemModel>();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    APREcoSystem ecosystem = new APREcoSystem();
                    entityMapper.MapToEntity(input, ecosystem);
                    unitOfWork.APREcoSystem.Insert(ecosystem);
                    unitOfWork.Save();
                    //you will get inserted id
                    return ecosystem.Id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool UpdateEcosystem(APREcoSystemModel update)
        {
            try
            {
                var entityMapper = new Mapper<APREcoSystem, APREcoSystemModel>();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var Ecosystem = unitOfWork.APREcoSystem.GetByID(update.Id);
                    entityMapper.MapToEntity(update, Ecosystem);
                    unitOfWork.APREcoSystem.Update(Ecosystem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static bool DeleteEcosystem(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.APREcoSystem.Delete(Id);
                unitOfWork.Save();


                return true;
            }
        }


        public static IList<PortfolioEcoModel> GetPortfolioEcoDetails(int portFolioId)
        {
            Db db = new Db();
            var portFolioApp = (from papps in db.ApplicationEcoSystemJunctions
                                join eco in db.APREcoSystem on papps.ESId equals eco.Id
                                join lobs in db.LineOfBusiness on papps.LOBId equals lobs.Id
                                where papps.LOBId == portFolioId
                                select new PortfolioEcoModel
                                {
                                    EcoId = eco.Id,

                                    EcoSystemName = eco.EcosystemName,

                                    LobEcoId = papps.Id,
                                    PortFolioName = lobs.LOBName,
                                    PortFolioId = lobs.Id

                                }
                                ).ToList();

            return portFolioApp;
        }

        public static int CreatePortfolioEco(PortfolioEcoModel Portfolioecoinput)
        {
            int id = 0;
            try
            {

                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    ApplicationEcoSystemJunctions LOBecosystem = new ApplicationEcoSystemJunctions();
                    LOBecosystem.ESId = Portfolioecoinput.EcoId;
                    LOBecosystem.LOBId = Portfolioecoinput.PortFolioId;
                    unitOfWork.ApplicationEcoSystemJunction.Insert(LOBecosystem);
                    unitOfWork.Save();
                    id = LOBecosystem.Id;

                }
            }
            catch (Exception)
            {
                return 0;
            }
            return id;
        }



        public static bool UpdatePortfolioEco(PortfolioEcoModel Portfolioecoinput)
        {
            try
            {

                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var ecoPortfolioinfo = unitOfWork.ApplicationEcoSystemJunction.GetByID(Portfolioecoinput.LobEcoId);
                    // entityMapper.MapToEntity(Portfolioinput, appPortfolioinfo);
                    ecoPortfolioinfo.LOBId = Portfolioecoinput.PortFolioId;
                    ecoPortfolioinfo.ESId = Portfolioecoinput.EcoId;
                    unitOfWork.ApplicationEcoSystemJunction.Update(ecoPortfolioinfo);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static bool DeletePortfolioEco(int portfolioEcoId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.ApplicationEcoSystemJunction.Delete(portfolioEcoId);
                unitOfWork.Save();
                return true;
            }
        }


        //public static IEnumerable<PortfolioEcoAppsModel> GetPortfolioEcoApps(int LobEcoId)
        //{
        //    List<PortfolioEcoAppsModel> list = new List<PortfolioEcoAppsModel>();
        //    var contextFactory = new DbContextFactory();
        //    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
        //    var result = unitOfWork.ApplicationLOBEcoSystemJunction.Get().Where(x => x.LobEcoId == LobEcoId).ToList();

        //    foreach (var prod in result)
        //    {
        //        PortfolioEcoAppsModel Model = new PortfolioEcoAppsModel();
        //        Model.LobEcoId = prod.LobEcoId;
        //        Model.AppId = prod.AppId;

        //        list.Add(Model);
        //    }


        //    return list;

        //}

        public static IList<PortfolioEcoAppsModel> GetPortfolioEcoApps(int LobEcoId)
        {


            Db db = new Db();
            var portFolioApp = (from papps in db.ApplicationLOBEcoSystemJunctions
                                join apps in db.Applications on papps.AppId equals apps.Id
                                join lobs in db.ApplicationEcoSystemJunctions on papps.LobEcoId equals lobs.Id
                                where papps.LobEcoId == LobEcoId
                                select new PortfolioEcoAppsModel
                                {
                                    AppId = apps.Id,
                                    ApplicationAliasName = apps.AliasName,
                                    ApplicationName = apps.ApplicationName,
                                    ClientApplicationNumber = apps.ClientApplicationNumber,
                                    PEAId = papps.Id,
                                    LobEcoId = papps.LobEcoId,

                                    //PortFolioName = lobs.LOBName,
                                    //PortFolioId = lobs.Id

                                }
                                ).ToList();

            return portFolioApp;

        }

        public static bool UpdatePortfolioEcoApps(PortfolioEcoAppsModel Input)
        {
            try
            {

                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var v = unitOfWork.ApplicationLOBEcoSystemJunction.GetByID(Input.LobEcoId);
                    // entityMapper.MapToEntity(Portfolioinput, appPortfolioinfo);
                    v.AppId = Input.AppId;

                    unitOfWork.ApplicationLOBEcoSystemJunction.Update(v);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static int CreatePortfolioEcoApps(PortfolioEcoAppsModel Input)
        {
            int id = 0;
            try
            {

                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    ApplicationLOBEcoSystemJunctions v = new ApplicationLOBEcoSystemJunctions();
                    v.LobEcoId = Input.LobEcoId;
                    v.AppId = Input.AppId;
                    unitOfWork.ApplicationLOBEcoSystemJunction.Insert(v);
                    unitOfWork.Save();
                    id = v.Id;

                }
            }
            catch (Exception)
            {
                return 0;
            }
            return id;
        }



        public static bool DeletePortfolioEcoApps(int PFAId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.ApplicationLOBEcoSystemJunction.Delete(PFAId);
                unitOfWork.Save();
                return true;
            }
        }

        public static IList<PortfolioEcoAppsModel> APRExportLOBEcoApp(int ClientId)
        {
            IList<PortfolioEcoAppsModel> list = new List<PortfolioEcoAppsModel>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };


            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("APRExportLOBEcoApp", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new PortfolioEcoAppsModel();


                    coldet.PortFolioName = dr["Portfolio"].ToString();
                    coldet.EcoSystemName = dr["Ecosystem"].ToString();

                    coldet.ApplicationName = dr["Application"].ToString();
                    coldet.ClientApplicationNumber = dr["ClientApplicationId"].ToString();
                    coldet.ApplicationType = dr["ApplicationType"].ToString();
                    list.Add(coldet);
                }

            }
            return list;
        }

        public static bool UpdateAprInterfaceModel(AprInterfaceModel AppInfoinput, int ClientId)
        {
            Db db = new Db();
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var appinfo = unitOfWork.Application.GetByID(AppInfoinput.Id);
                    //basic info
                    appinfo.CountInboundInterfaces = AppInfoinput.CountInboundInterfaces;

                    appinfo.CountOutboundInterfaces = AppInfoinput.CountOutboundInterfaces;
                    appinfo.EntangledInterface = AppInfoinput.EntangledInterface;
                    unitOfWork.Application.Update(appinfo);
                    unitOfWork.Save();
                    APRApplicationInterfaceMethod appinterface = new APRApplicationInterfaceMethod();

                    var deleteInterface = from details in db.APRApplicationInterfaceMethod
                                          where details.AppId == AppInfoinput.Id
                                          select details;

                    foreach (var detail in deleteInterface)
                    {
                        unitOfWork.APRApplicationInterfaceMethod.Delete(detail.Id);
                        unitOfWork.Save();
                    }
                    foreach (string id in AppInfoinput.InterfaceMethodsId)
                    {
                        List<int> cid = id.Split(',').Select(int.Parse).ToList();

                        foreach (int i in cid)
                        {
                            appinterface.AppId = AppInfoinput.Id;
                            appinterface.ClientId = ClientId;
                            appinterface.InterfaceMethodId = i;
                            unitOfWork.APRApplicationInterfaceMethod.Insert(appinterface);
                            unitOfWork.Save();


                        }
                    }

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static AprInterfaceModel getInterfaceDetail(int AppId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            AprInterfaceModel model = new AprInterfaceModel();

            var CommonDetails = unitOfWork.Application.Get(g => g.Id == AppId).FirstOrDefault();
            // var InterfaceDetail=unitOfWork.APRApplicationInterfaceMethod.Get(g => g.AppId == AppId).Select(e => e.InterfaceMethodId.ToString()).ToArray();
            var InterfaceDetail = unitOfWork.APRApplicationInterfaceMethod.Get().Where(e => e.AppId == AppId).Select(e => e.InterfaceMethodId.ToString()).ToArray();

            if (CommonDetails != null)
            {
                //Common Details
                model.EntangledInterface = CommonDetails.EntangledInterface;
                model.CountInboundInterfaces = CommonDetails.CountInboundInterfaces;
                model.CountOutboundInterfaces = CommonDetails.CountOutboundInterfaces;
                model.InterfaceMethods = InterfaceDetail;
            }



            return model;

        }

        public IEnumerable<SelectListItem> CountryList()
        {

            // var client = (Client)Session["Client"];
            // int ClientId = client.Id;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            //var applicationList = unitOfWork.Countries.Get().Where(x => x.RegionId == 0).OrderBy(m => m.CountryName).ToList();
            var contries = unitOfWork.Countries.Get().OrderBy(m => m.CountryName).ToList();
            var item = contries.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.CountryName.ToString() });
            return item;

        }

        public static List<SelectListItem> ListOfInterfaceMethod(int clientId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var InterfaceMethodList = unitOfWork.APRInterfaceMethod.Get().Where(x => x.ClientId == clientId).ToList();
            var item = InterfaceMethodList.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.InterfaceMethod.ToString() }).ToList();
            return item;
        }


        public static IEnumerable<DispositionModel> readDisposition(int clientId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var disp = unitOfWork.APRDisposition.Get(g => g.ClientId == clientId);
            var returnModel = disp.Select(f => new DispositionModel {
                DispId = f.Id,
                ClientId = f.ClientId,
                DispositionName = f.DispositionName
            });
            return returnModel;
        }

        public static IEnumerable<APRDispositionSolution> dispSolutionlist(int clientId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var dispSol = unitOfWork.APRDispositionSolution.Get(g => g.ClientId == clientId);
            var returnModel = dispSol.Select(f => new APRDispositionSolution { DispositionSolution = f.DispositionSolution, Id = f.Id });
            return returnModel;
        }

        public static int dispsolutionLinkCreate(DispNDispSlnModel model)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var entity = new APRDispAndDispSollution()
            {
                ClientId = model.ClientId,
                DispId = model.DispId,
                DispSolutionId = model.DispSolutionId,

            };
            unitOfWork.APRDispAndDispSollution.Insert(entity);
            unitOfWork.Save();
            return entity.Id;

        }


        public static void dispsolutionLinkUpdate(DispNDispSlnModel model)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var entity = unitOfWork.APRDispAndDispSollution.GetByID(model.MapId);

            entity.DispSolutionId = model.DispSolutionId;

            unitOfWork.APRDispAndDispSollution.Update(entity);
            unitOfWork.Save();

        }

        public static void dispsolutionLinkDelete(int Id)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            unitOfWork.APRDispAndDispSollution.Delete(Id);
            unitOfWork.Save();
        }

        public static IEnumerable<DispNDispSlnModel> dispsolutionLinkRead(int DispositionId, int ClientId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var dispositionLinkList =
                unitOfWork.APRDispAndDispSollution.Get(g=>g.DispId ==DispositionId && g.ClientId == ClientId);
            var returnModel = dispositionLinkList.Select(f => new DispNDispSlnModel
            {
                ClientId = f.ClientId,
                DispId = f.DispId,
                DispSolutionId = f.DispSolutionId,
                MapId = f.Id
            });
            return returnModel;
        }

        public static IList<DispositionDetailsModel> GetPlannedDisposition(int PlannedDisposition,int ClientId)
        {
            Db db = new Db();
            var result = (from dissol in db.APRDispAndDispSollution
                          join sol in db.APRDispositionSolution on dissol.DispSolutionId equals sol.Id into tmpsol
                          from pst in tmpsol.DefaultIfEmpty()
                          where dissol.DispId == PlannedDisposition && dissol.ClientId== ClientId
                          select new DispositionDetailsModel
                          {
                              PlannedDispSolution = pst.DispositionSolution

                          }).ToList();
            return result;
        }
        public static IList<ExAppInventoryDataModel> getDrillDownApps(int ClientId, string ChartType,string Category)
        {
            IList<ExAppInventoryDataModel> Modellist = new List<ExAppInventoryDataModel>();
            var paramName = new string[] { "ClientId", "Charttype", "CategoryValue" };
            var paramValue = new object[] { ClientId, ChartType, Category };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("APRDashboardGetChartData_drilldown", paramName, paramValue);
                //if (dpnVal.Length > 0)
                //{
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new ExAppInventoryDataModel();
                    coldet.Id = Convert.ToInt32(dr["ID"]);
                    if (!(dr["ApplicationName"] is DBNull))
                        coldet.ApplicationName = dr["ApplicationName"].ToString();
                     else
                        coldet.ApplicationName = "-";
                   if (!(dr["Description"] is DBNull))
                        coldet.Description = dr["Description"].ToString();
                   else
                        coldet.Description = "-";
                    if (!(dr["ClientApplicationNumber"] is DBNull))
                        coldet.ClientApplicationNumber = dr["ClientApplicationNumber"].ToString();
                    else
                        coldet.ClientApplicationNumber = "-";
                    if (!(dr["BusinessFunction"] is DBNull))
                        coldet.BusinessFunction = dr["BusinessFunction"].ToString();
                     else
                        coldet.BusinessFunction = "-";
                    if (!(dr["AppCriticality"] is DBNull))
                        coldet.AppCriticality = dr["AppCriticality"].ToString();
                     else
                        coldet.AppCriticality = "-";
                    if (!(dr["ArchitectureType"] is DBNull))
                        coldet.ArchitectureType = dr["ArchitectureType"].ToString();
                      else
                        coldet.ArchitectureType = "-";
                    if (!(dr["SourceType"] is DBNull))
                        coldet.SourceType = dr["SourceType"].ToString();
                    else
                        coldet.SourceType = "-";
                    if (!(dr["AvgConcurrentUsers"] is DBNull))
                        coldet.AvgConcurrentUsers = Convert.ToInt32(dr["AvgConcurrentUsers"]);
                    else
                        coldet.AvgConcurrentUsers = null;
                    if (!(dr["AppSize"] is DBNull))
                        coldet.AppSize = dr["AppSize"].ToString();
                    else
                        coldet.AppSize = "-";
                    if (!(dr["LOBName"] is DBNull))
                        coldet.LOBName = dr["LOBName"].ToString();
                     else
                        coldet.LOBName = "-";
                    if (!(dr["DispositionName"] is DBNull))
                        coldet.DispositionName = dr["DispositionName"].ToString();
                    else
                        coldet.DispositionName = "-";
                    if (!(dr["AppType"] is DBNull))
                        coldet.AppType = dr["AppType"].ToString();
                     else
                        coldet.AppType = "-";
                    Modellist.Add(coldet);
                }
            }
            return Modellist;
        }

    }
}