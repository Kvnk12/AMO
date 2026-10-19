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
using WebUI.Areas.PortfolioAnalyzer.Dto;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using WebUI.Areas.PortfolioAnalyzer.Models;
using Data;
using Domain.Entities.PortfolioAnalyzer;
using Kendo.Mvc.Export;
using System.IO;
using Telerik.Documents.SpreadsheetStreaming;

using System.Collections;
using NTTData.AMOPortal.WebUI.Filters;
using WebUI.Dto;

namespace WebUI.Areas.PortfolioAnalyzer.Controllers
{
    public class APRAjaxServiceController : Controller
    {
        //
        // GET: /PortfolioAnalyzer/AjaxService/

        public IEnumerable<SelectListItem> PortfolioList(int ClientID)
        {

            // var client = (Client)Session["Client"];
            // int ClientId = client.Id;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var lineofbusinessList = unitOfWork.LineOfBusinesses.Get(x => x.ClientId == ClientID).ToList();

            var item = lineofbusinessList.OrderBy(x => x.Id).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.LOBName.ToString(), Selected = true });
            return item;

        }


        public IEnumerable<SelectListItem> ApplicationList(int ClientID)
        {

            //Db db = new Db();
            // var client = (Client)Session["Client"];
            // int ClientId = client.Id;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.Application.Get(x => x.ClientId == ClientID).ToList();

            var item = applicationList.OrderBy(x => x.ApplicationName).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.ApplicationName.ToString(), Selected = true });
            return item;

        }
        public IEnumerable<SelectListItem> APRArchitectureList(int ClientID)
        {

            // var client = (Client)Session["Client"];
            // int ClientId = client.Id;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.APRAppArchitecture.Get().Where(x => x.ClientId == ClientID).ToList();
            var item = applicationList.OrderBy(x=>x.ArchitectureType).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.ArchitectureType.ToString() });
            return item;

        }

        public IEnumerable<SelectListItem> APRDomainList(int ClientID)
        {

            // var client = (Client)Session["Client"];
            // int ClientId = client.Id;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.APRDomain.Get().Where(x => x.ClientId == ClientID).ToList();
            var item = applicationList.OrderBy(x=>x.AppDomainName).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.AppDomainName.ToString() });
            return item;

        }

        public IEnumerable<SelectListItem> APRAppSourceCodeTypeList(int ClientID)
        {

            // var client = (Client)Session["Client"];
            // int ClientId = client.Id;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.APRAppSourceCodeType.Get().Where(x => x.ClientId == ClientID).ToList();
            var item = applicationList.OrderBy(x => x.SourcecodeType).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.SourcecodeType.ToString() });
            return item;

        }

        public IEnumerable<SelectListItem> APRAppLifeCycleList(int ClientID)
        {

            // var client = (Client)Session["Client"];
            // int ClientId = client.Id;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.APRAppLifeCycle.Get().Where(x => x.ClientId == ClientID).ToList();
            var item = applicationList.OrderBy(x=>x.AppLifeCycleName).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.AppLifeCycleName.ToString() });
            return item;

        }

        public IEnumerable<SelectListItem> APRAppCriticalityList(int ClientID)
        {

            // var client = (Client)Session["Client"];
            // int ClientId = client.Id;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.APRAppCriticality.Get().Where(x => x.ClientId == ClientID).ToList();
            var item = applicationList.OrderBy(x=>x.CriticalityName).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.CriticalityName.ToString() });
            return item;

        }

        public IEnumerable<SelectListItem> APRAppSecurityList(int ClientID)
        {

            // var client = (Client)Session["Client"];
            // int ClientId = client.Id;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.APRAppSecurity.Get().Where(x => x.ClientId == ClientID).ToList();
            var item = applicationList.OrderBy(x=>x.SecurityLevelName).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.SecurityLevelName.ToString() });
            return item;
        }

        public IEnumerable<SelectListItem> APRAppEnvironmentList(int ClientID)
        {

            // var client = (Client)Session["Client"];
            // int ClientId = client.Id;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.APRAppEnvironment.Get().Where(x => x.ClientId == ClientID).ToList();
            var item = applicationList.OrderBy(x=>x.EnvironmentType).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.EnvironmentType.ToString() });
            return item;
        }
        public IEnumerable<SelectListItem> APRAppProcessingList(int ClientID)
        {

            // var client = (Client)Session["Client"];
            // int ClientId = client.Id;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.APRAppProcessingType.Get().Where(x => x.ClientId == ClientID).ToList();
            var item = applicationList.OrderBy(x => x.ProcessingType).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.ProcessingType.ToString() });
            return item;
        }

        public IEnumerable<SelectListItem> APRAppTypeList(int ClientID)
        {

            // var client = (Client)Session["Client"];
            // int ClientId = client.Id;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.APRAppType.Get().Where(x => x.ClientId == ClientID).ToList();
            var item = applicationList.OrderBy(x=>x.AppType).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.AppType.ToString() });
            return item;
        }
        public IEnumerable<SelectListItem> APRAppRedundancyList(int ClientID)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.APRAppRedundancy.Get().Where(x => x.ClientId == ClientID).ToList();
            var item = applicationList.OrderBy(x => x.RedundancyName).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.RedundancyName.ToString() });
            return item;
        }
        public IEnumerable<SelectListItem> APRDispositionList(int ClientID)
        {

            // var client = (Client)Session["Client"];
            // int ClientId = client.Id;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.APRDisposition.Get().Where(x => x.ClientId == ClientID).ToList();
            var item = applicationList.OrderBy(x=>x.DispositionName).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.DispositionName.ToString() });
            return item;
        }
        public IEnumerable<SelectListItem> APRDispositionRationaleList(int ClientID)
        {

            // var client = (Client)Session["Client"];
            // int ClientId = client.Id;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.APRDispositionRationale.Get().Where(x => x.ClientId == ClientID).ToList();
            var item = applicationList.OrderBy(x => x.DispositionRationaleType).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.DispositionRationaleType.ToString() });
            return item;
        }
        public IEnumerable<SelectListItem> APRDispositionActionList(int ClientID)
        {

            // var client = (Client)Session["Client"];
            // int ClientId = client.Id;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.APRDispositionAction.Get().Where(x => x.ClientId == ClientID).ToList();
            var item = applicationList.OrderBy(x => x.DispositionActionType).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.DispositionActionType.ToString() });
            return item;
        }

        public IEnumerable<SelectListItem> ApplicationClassificationList(int ClientID)
        {
           
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.ApplicationClassification.Get().Where(x => x.ClientId == ClientID).ToList();
            var item = applicationList.OrderBy(x=>x.ApplicationClassificationName).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.ApplicationClassificationName.ToString() });
            return item;
        }

        public IEnumerable<SelectListItem> ExternalHostingList(int ClientID)
        {
        
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.ExternalHosting.Get().Where(x => x.ClientId == ClientID).ToList();
            var item = applicationList.OrderBy(x=>x.HostingTypeName).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.HostingTypeName.ToString() });
            return item;
        }
        public IEnumerable<SelectListItem> CountryList()
        {

            // var client = (Client)Session["Client"];
            // int ClientId = client.Id;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            //var applicationList = unitOfWork.Countries.Get().Where(x => x.RegionId == 0).OrderBy(m => m.CountryName).ToList();
            var contries = unitOfWork.Countries.Get().OrderBy(m => m.CountryName).ToList();
            var item = contries.OrderBy(x => x.CountryName).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.CountryName.ToString() });
            return item;

        }


        public ActionResult GetApplicationList()
        {

          //  var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];

            int ClientId = client.ClientId;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.Application.Get().Where(x => x.ClientId == ClientId).ToList();
            return Json(applicationList.ToList().OrderBy(x=>x.ApplicationName).Select(A => new { Value = A.Id.ToString(), Text = A.ApplicationName.ToString() }), JsonRequestBehavior.AllowGet);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ApplicationDashboard(int AppId, DashboardModel DashboardModel)
        {

            //using (var contextFactory = new DbContextFactory())
            //{
            //    //DashboardModel model = new DashboardModel();
            //    //var entityMapper = new Mapper<DashboardModel, Application>();
            //    //Application ApplicationInfo = null;
            //    //IList<DashboardModel> AppViewModels = new List<DashboardModel>();
            //    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            //    var ApplicationInfo = unitOfWork.Application.Get(m => m.ID == AppId).FirstOrDefault();
            //    if(ApplicationInfo != null)
            //    {
            //        DashboardModel.ClientApplicationNumber = string.IsNullOrEmpty(ApplicationInfo.ClientApplicationNumber)? "":ApplicationInfo.ClientApplicationNumber;
            //        DashboardModel.Description = ApplicationInfo.Description;
            //    }

            // }
            // return Json(DashboardModel, JsonRequestBehavior.AllowGet);
            DashboardModel model = PortfolioAnalyzerHelper.APRAppDashboard(AppId);

            return Json(model);
        }



        [HttpPost]
        public ActionResult GetDashboardFilterOptions(int filterOption)
        {


            //var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            List<DashboardModel> model = null;
            switch (filterOption)
            {
                case 1:

                    model = PortfolioAnalyzerHelper.APRCriticalityDashboard(ClientId).ToList();

                    break;

                case 2:

                    model = PortfolioAnalyzerHelper.APRAppTypeDashboard(ClientId).ToList();

                    break;

                case 3:

                    model = PortfolioAnalyzerHelper.APRAppSourceCodeTypeDashboard(ClientId).ToList();

                    break;

                case 4:

                    model = PortfolioAnalyzerHelper.APRAppDispositionDashboard(ClientId).ToList();

                    break;


                case 5:

                    model = PortfolioAnalyzerHelper.APRAppBusinessFunctionDashboard(ClientId).ToList();

                    break;

                case 6:

                    model = PortfolioAnalyzerHelper.APRPortfolioBreakdownDashboard(ClientId).ToList();
                    break;
                default:
                    model = null;
                    break;



            }
            return Json(model);

        }

        [HttpPost]
        public ActionResult APRRatingHeatMap(string surveyId, [DataSourceRequest]DataSourceRequest request)
        {
           // Client ci = (Client)Session["Client"];
            ClientDetail ci = (ClientDetail)Session["SessionClient"];

            int sid = 0;
            if (surveyId == "")
            {
                //using (var contextFactory = new DbContextFactory())
                //{
                //    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                //    var ss = unitOfWork.Survey.Get(g => g.ClientId == ci.Id).FirstOrDefault();
                //    if (ss != null)
                //        sid = ss.Id;
                //    ViewBag.FirstSurveyId = sid;
                //}
                sid = 0;

            }
            else
            {
                try
                {

                    sid = Convert.ToInt32(surveyId);
                }
                catch(Exception)
                {
                    sid = 0;
                }
            }

            var heatMapTable = PortfolioAnalyzerHelper.APRRatingHeatMap(ci.ClientId, sid);
            //string JSONString = string.Empty;
            //JSONString = JsonConvert.SerializeObject(heatMapTable);
            //return JSONString;
            // return Json(JSONString,JsonRequestBehavior.AllowGet);
            return Json(heatMapTable.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetDashboardFilterOptionsGrid(int filterOption, [DataSourceRequest]DataSourceRequest request)
        {


            //var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            List<DashboardModel> model = null;
            switch (filterOption)
            {
                case 1:

                    model = PortfolioAnalyzerHelper.APRCriticalityDashboard(ClientId).ToList();

                    break;

                case 2:

                    model = PortfolioAnalyzerHelper.APRAppTypeDashboard(ClientId).ToList();

                    break;

                case 3:

                    model = PortfolioAnalyzerHelper.APRAppSourceCodeTypeDashboard(ClientId).ToList();

                    break;

                case 4:

                    model = PortfolioAnalyzerHelper.APRAppDispositionDashboard(ClientId).ToList();

                    break;


                case 5:

                    model = PortfolioAnalyzerHelper.APRAppBusinessFunctionDashboard(ClientId).ToList();

                    break;
                case 6:

                    model = PortfolioAnalyzerHelper.APRPortfolioBreakdownDashboard(ClientId).ToList();
                    break;
                default:
                    model = null;
                    break;



            }
            return Json(model.OrderByDescending(x => x.AppCount).ToList().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }

        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ApplicationUpdate(ApplicationInformationModel ApplicationInformationModel)
        {
          //  var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            var res = PortfolioAnalyzerHelper.UpdateAppDemographics(ApplicationInformationModel,ClientId);


            return Json(ApplicationInformationModel);
        }
        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DispositionUpdate(DispositionDetailsModel DispositionInformation)
        {
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            var res = PortfolioAnalyzerHelper.UpdateDisposition(DispositionInformation, ClientId);


            return Json(DispositionInformation);
        }

        [HttpPost]
        [ActionTrack]
        public ActionResult UpdateFinancial([DataSourceRequest] DataSourceRequest request, APRFinancialModel Financial)
        {
            if (Financial != null && ModelState.IsValid)
            {
                var res = PortfolioAnalyzerHelper.UpdateAppFinanceInfo(Financial);
            }

            return Json(new[] { Financial }.ToDataSourceResult(request, ModelState));
        }


        [HttpPost]
        public ActionResult GetApplicationinCostSummary(int ApplicationID=0)
        {
            using (var contextFactory = new DbContextFactory())
            {

                var entityMapper = new Mapper<APRFinancialModel, APRAppFinancial>();
                IList<APRFinancialModel> aprFinDetailListViewModel = new List<APRFinancialModel>();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var APRFinancialInfoDetail = unitOfWork.APRAppFinancial.Get().Where(w => w.AppId == ApplicationID).OrderBy(x => x.Year);
                if (APRFinancialInfoDetail != null)
                {
                    foreach (var financialentity in APRFinancialInfoDetail)
                    {
                        APRFinancialModel aprFinViewModel = new APRFinancialModel();
                        entityMapper.MapToEntity(financialentity, aprFinViewModel);
                        aprFinDetailListViewModel.Add(aprFinViewModel);
                    }
                }

                return Json(aprFinDetailListViewModel);

            }

        }

        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreatePersonnel([DataSourceRequest] DataSourceRequest request, PersonnelModel Personnel)
        {
            if (Personnel != null && ModelState.IsValid)
            {
                var res = PortfolioAnalyzerHelper.InsertPersonnel(Personnel);
                Personnel.Id = res;
            }

            return Json(new[] { Personnel }.ToDataSourceResult(request, ModelState));
        }
        [ActionTrack]
        [HttpPost]
        public ActionResult UpdatePersonnel([DataSourceRequest] DataSourceRequest request, PersonnelModel Personnel)
        {


            if (Personnel != null && ModelState.IsValid)
            {
                var res = PortfolioAnalyzerHelper.UpdatePersonnel(Personnel);
            }

            return Json(new[] { Personnel }.ToDataSourceResult(request, ModelState));
        }
        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DestroyPersonnel([DataSourceRequest] DataSourceRequest request, PersonnelModel Personnel)
        {
            if (Personnel != null)
            {
                var v = PortfolioAnalyzerHelper.DeletePersonnel(Personnel.Id);
            }

            return Json(new[] { Personnel }.ToDataSourceResult(request, ModelState));
        }


        [HttpPost]
        public ActionResult GetAllPersonnel([DataSourceRequest]DataSourceRequest request)
        {
            using (var contextFactory = new DbContextFactory())
            {

                var entityMapper = new Mapper<PersonnelModel, Personnel>();
                IList<PersonnelModel> personnelList = new List<PersonnelModel>();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var list = unitOfWork.Personnel.Get().ToList();
                if (list != null)
                {
                    foreach (var l in list)
                    {
                        PersonnelModel personnelModel = new PersonnelModel();
                        entityMapper.MapToEntity(l, personnelModel);
                        personnelList.Add(personnelModel);
                    }
                }

                return Json(personnelList.ToList().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

            }
        }

        public class DictonaryContacts
        {
            public int id { get; set; }
            public int AppId { get; set; }
            //public string IncidentID { get; set; }
        }
        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult InsertPersonnelRole([DataSourceRequest] DataSourceRequest request, List<DictonaryContacts> listObject)
        {
            if (listObject != null && ModelState.IsValid)
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    //string targetOldValue = "";
                    //string IPAddress = IPHelper.GetIPAddress(Request.ServerVariables["HTTP_VIA"],
                    //                                  Request.ServerVariables["HTTP_X_FORWARDED_FOR"],
                    //                                  Request.ServerVariables["REMOTE_ADDR"]);
                    //var client = (Client)Session["Client"];
                    ClientDetail client = (ClientDetail)Session["SessionClient"];
                    int ClientId = client.ClientId;
                    foreach (var v in listObject)
                    {

                        var target = unitOfWork.PersonnelRole.Get(x => x.PersonnelId == v.id && x.AppId == v.AppId && x.ClientId == ClientId).FirstOrDefault();

                        if (target == null)
                        {
                            using (var db1 = new DbContextFactory())
                            {
                                var DB = new UnitOfWorks(db1.GetContext());
                                PersonnelRole newitem = new PersonnelRole();
                                newitem.PersonnelId = v.id;
                                newitem.AppId = v.AppId;
                                newitem.ClientId = ClientId;
                                DB.PersonnelRole.Insert(newitem);
                                DB.Save();

                            }

                        }




                    }



                }

            }

            return Json(ModelState.ToDataSourceResult());
        }

        [HttpPost]
        public ActionResult GetPersonnelRoleByApp([DataSourceRequest]DataSourceRequest request, int AppId)
        {
            using (var contextFactory = new DbContextFactory())
            {
               // var client = (Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                int ClientId = client.ClientId;
                 // var entityMapper = new Mapper<PersonnelRoleModel, PersonnelRole>();
                IList<PersonnelRoleModel> personnelRoleList = new List<PersonnelRoleModel>();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var list = unitOfWork.PersonnelRole.Get(x => x.AppId == AppId && x.ClientId == ClientId).ToList();
                if (list != null)
                {
                    foreach (var l in list)
                    {
                        PersonnelRoleModel personnelRoleModel = new PersonnelRoleModel();
                      
                        // entityMapper.MapToEntity(l, personnelRoleModel);
                        personnelRoleModel.AppId = l.AppId;
                        personnelRoleModel.Id = l.Id;
                        personnelRoleModel.IsSurveyParticipant = l.IsSurveyParticipant;
                        personnelRoleModel.PersonnelId = l.PersonnelId;
                        personnelRoleModel.ReceiveEmailNotification = l.ReceiveEmailNotification;
                        personnelRoleModel.Title = l.Title;
                        personnelRoleModel.Emailid = unitOfWork.Personnel.GetByID(l.PersonnelId).EmailAddress;
                        personnelRoleList.Add(personnelRoleModel);
                    }
                }

                return Json(personnelRoleList.ToList().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

            }
        }

        [ActionTrack]
        [HttpPost]
        public ActionResult UpdatePersonnelRole([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<PersonnelRoleModel> PersonnelRole)
        {
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;

            if (PersonnelRole != null && ModelState.IsValid)
            {
                foreach (var p in PersonnelRole)
                {
                    p.ClientId = ClientId;
                    PortfolioAnalyzerHelper.UpdatePersonnelRole(p);
                }

            }

            return Json(PersonnelRole.ToDataSourceResult(request, ModelState));
        }
        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DestroyPersonnelRole([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<PersonnelRoleModel> PersonnelRole)
        {
            if (PersonnelRole != null)
            {
                foreach (var p in PersonnelRole)
                {
                    PortfolioAnalyzerHelper.DeletePersonnelRole(p.Id);
                }

            }

            return Json(PersonnelRole.ToDataSourceResult(request, ModelState));
        }



        public IEnumerable<SelectListItem> Vendorlist()
        {

            // var client = (Client)Session["Client"];
            // int ClientId = client.Id;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            //var applicationList = unitOfWork.Countries.Get().Where(x => x.RegionId == 0).OrderBy(m => m.CountryName).ToList();
            var contries = unitOfWork.Vendor.Get().OrderBy(m => m.VendorName).ToList();
            var item = contries.Select(p => new SelectListItem { Value = p.VendorName.ToString(), Text = p.VendorName.ToString() });
            return item;

        }

        public IEnumerable<SelectListItem> AppHostlist(int ClientId)
        {

            //var client = (Client)Session["Client"];
            //int ClientId = client.Id;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            //var applicationList = unitOfWork.Countries.Get().Where(x => x.RegionId == 0).OrderBy(m => m.CountryName).ToList();
            var applicationList = unitOfWork.APRHostList.Get().Where(x => x.ClientId == ClientId).ToList();
            var item = applicationList.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.HostName.ToString() });
            return item;

        }

        public IEnumerable<SelectListItem> GetVendorlist()
        {

            //var client = (Client)Session["Client"];
            //int ClientId = client.Id;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            //var applicationList = unitOfWork.Countries.Get().Where(x => x.RegionId == 0).OrderBy(m => m.CountryName).ToList();
            var applicationList = unitOfWork.Vendor.Get().ToList();
            var item = applicationList.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.VendorName.ToString() });
            return item;

        }
        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult InsertUpdateHostlist(APRHostlistModel APRHostlistModel)
        {
            //var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            APRHostlistModel.ClientId = ClientId;
            var res = PortfolioAnalyzerHelper.InsertOrUpdateHostlist(APRHostlistModel);


            return Json(APRHostlistModel);
        }
        [ActionTrack]
        [HttpPost]
        public ActionResult UnassignedApps(int Id)
        {
           // Client cl = (Client)Session["Client"];
            ClientDetail cl= (ClientDetail)Session["SessionClient"];
            //var Count = 0;
            var sid = Convert.ToInt32(Id);
            var completedDate = 0;
            var AppsnotIncluded = 0;
            try
            {
                Db db = new Db();
                var SurRefIDs = (from su in db.SurveyUser where su.SurveyId == sid select su).Distinct().ToList();
                var apps = (from s in db.Applications
                            where s.ClientId == cl.ClientId
                            select s).ToList();
                AppsnotIncluded = apps.Where(e => !SurRefIDs.Select(s => s.SurveyForRefId).Contains(e.Id)).Count();
                completedDate = SurRefIDs.Where(e => e.Completedt == null).Count();
            }
            catch { }
            return Json(new { cd = completedDate, an = AppsnotIncluded });
        }


        public IEnumerable<SelectListItem> AppEnvironmentlist()
        {

            //var client = (Client)Session["Client"];
            //int ClientId = client.Id;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            //var applicationList = unitOfWork.Countries.Get().Where(x => x.RegionId == 0).OrderBy(m => m.CountryName).ToList();
            var applicationList = unitOfWork.APRAppEnvironment.Get(x => x.ClientId == 1).ToList();
            var item = applicationList.Select(p => new SelectListItem { Value = p.EnvironmentType.ToString(), Text = p.EnvironmentType.ToString() });
            return item;

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
        //}
        [HttpPost]
        public ActionResult  GetAppHost(int HostId)
        {
            //if (ServiceModel != null && ModelState.IsValid) 
            //{
            using (var contextFactory = new DbContextFactory())
            {

                var entityMapper = new Mapper<APRHostlistModel, APRHostList>();


                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var HostInfo = unitOfWork.APRHostList.GetByID(HostId);
                var aPRHostlistModel = new APRHostlistModel();
                entityMapper.MapToEntity(HostInfo, aPRHostlistModel);
                // AppViewModels.Add(ApplicationInformationModel);
                //return aPRHostlistModel;


                return Json(aPRHostlistModel, JsonRequestBehavior.AllowGet);
            }


            }


        [HttpPost]
        public ActionResult GetAppHostlist( [DataSourceRequest]DataSourceRequest request)
        {
            //NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];

            using (var contextFactory = new DbContextFactory())
            {

                var entityMapper = new Mapper<APRHostlistModel, APRHostList>();
                IList<APRHostlistModel> aPRHostlistModellist = new List<APRHostlistModel>();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var APRHostlist = unitOfWork.APRHostList.Get().Where(w => w.ClientId == client.ClientId);
                if (APRHostlist != null)
                {
                    foreach (var h in APRHostlist)
                    {
                        APRHostlistModel aPRHostlistModel = new APRHostlistModel();
                        entityMapper.MapToEntity(h, aPRHostlistModel);
                        aPRHostlistModellist.Add(aPRHostlistModel);
                    }
                }

                return Json(aPRHostlistModellist.ToList().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

            }
        }

        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DestroyAPRHost([DataSourceRequest] DataSourceRequest request, APRHostlistModel host)
        {
            if (host != null)
            {
                var v = PortfolioAnalyzerHelper.DeleteHostlist(host.Id);
            }

            return Json(new[] { host }.ToDataSourceResult(request, ModelState));
        }

        //[HttpPost]
        public ActionResult GetApplistForPortfolioClient(string PortfolioIds)
        {
            var v = (HttpUtility.UrlDecode(PortfolioIds));
           // List<int> TagIds = v.Split(',').Select(int.Parse).ToList();
            //using (var contextFactory = new DbContextFactory())
            //{
               // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
                //var entityMapper = new Mapper<RatingBySubperspective, Domain.Entities.Survey.SurveyPerspective>();
                //IList<RatingBySubperspective> perspectivelist = new List<RatingBySubperspective>();
                ////RatingBySubperspective perspectivelist = new RatingBySubperspective();
                //var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                //using (Db db = new Db())
                //{
                //    var target = from t1 in db.ApplicationLOBJunction
                //                 join t2 in db.Applications on t1.AppId equals t2.Id
                //                 where TagIds.Contains(t1.LOBId)
                //                 select new { Id = t1.AppId, Text = t2.ApplicationName };
                //    return Json(target.ToList(), JsonRequestBehavior.AllowGet);
                //}


            List<RatingBySubperspective> list = PortfolioAnalyzerHelper.APRGetAppsForPortfoli(v).ToList();
         

            return Json((list.ToList()), JsonRequestBehavior.AllowGet);






            // }
        }

        public JsonResult Cascading_Get_Applications(string portfolio)
        {
            var v = (HttpUtility.UrlDecode(portfolio));
         
            //var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
           

            List<RatingBySubperspective> list = PortfolioAnalyzerHelper.APRGetAppsForPortfoli(v).ToList();



            return Json(list.Select(p => new { Value = p.AppId, Text = p.ApplicationName }), JsonRequestBehavior.AllowGet);
        }


        //[HttpPost]
        //public ActionResult GetPerspectiveTypelist(string surveyId)
        //{
        //    var client = (Client)Session["Client"];
        //    int ClientId = client.Id;
        //    RatingBySubperspective model = PortfolioAnalyzerHelper.APRGetPerspectiveTypesBySurvey(ClientId.ToString(),surveyId);

        //    return Json(model, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public ActionResult Pdf_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        [HttpPost]
        public PartialViewResult GetPerspectiveTypelist(string surveyId)
        {
            //var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            RatingBySubperspective model = PortfolioAnalyzerHelper.APRGetPerspectiveTypesBySurvey(ClientId.ToString(), surveyId);
            List<string> plist = model.PerspectiveTypelist;
            return PartialView("_RatingPerspectiveTypes", plist);
        }



        [HttpPost]
        public ActionResult GetPerspectivesReport(string PerspectiveX, string PerspectiveY, string ApplicationID,  string SurveyId)
        {
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            List<RatingPerspReport> model = null;
            string Apps = null;
            if (string.IsNullOrEmpty(ApplicationID))
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var applicationList = unitOfWork.Application.Get(x => x.ClientId == ClientId).Select(x=>x.Id).ToList();
                Apps = string.Join(",", applicationList.ToList());
            }
            else
            {
                Apps = (HttpUtility.UrlDecode(ApplicationID));
            }
           model = PortfolioAnalyzerHelper.APRSubperspectiveReport(PerspectiveX, PerspectiveY, Apps, client.ClientId.ToString(), SurveyId).ToList();

            return Json(model);
        }

        [HttpPost]
        public ActionResult GetAppDispositionRoadmap([DataSourceRequest]DataSourceRequest request)
        {
          //  NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];

            List<DispositionRoadMapModel> model = PortfolioAnalyzerHelper.APRAppDispositionRoadmap(client.ClientId).ToList();

            return Json(model.ToList().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

            
        }

        [HttpPost]
        public ActionResult GetLEAData([DataSourceRequest]DataSourceRequest request)
        {

           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            List<PortfolioEcoAppsModel> output = PortfolioAnalyzerHelper.APRExportLOBEcoApp(ClientId).ToList();

            

            return Json((output.ToList().ToDataSourceResult(request)), JsonRequestBehavior.AllowGet);
            //}
            //else
            //    return Json(false, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public FileStreamResult EcoConfigureExportServer(string model, string data)
        {
            var columnsData = JsonConvert.DeserializeObject<IList<ExportColumnSettings>>(HttpUtility.UrlDecode(model));
            dynamic options = JsonConvert.DeserializeObject(HttpUtility.UrlDecode(data));
            SpreadDocumentFormat exportFormat = options.format.ToString() == "csv" ? exportFormat = SpreadDocumentFormat.Csv : exportFormat = SpreadDocumentFormat.Xlsx;
            Action<ExportCellStyle> cellStyle = new Action<ExportCellStyle>(ChangeCellStyle);
            Action<ExportRowStyle> rowStyle = new Action<ExportRowStyle>(ChangeRowStyle);
            Action<ExportColumnStyle> columnStyle = new Action<ExportColumnStyle>(ChangeColumnStyle);

            string fileName = string.Format("{0}.{1}", options.title, options.format);
            string mimeType = Helpers.GetMimeType(exportFormat);

            //var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;

            List<PortfolioEcoAppsModel> output = PortfolioAnalyzerHelper.APRExportLOBEcoApp(ClientId).ToList();
            Stream exportStream = exportFormat == SpreadDocumentFormat.Xlsx ? output.ToXlsxStream(columnsData, (string)options.title.ToString(), cellStyleAction: cellStyle, rowStyleAction: rowStyle, columnStyleAction: columnStyle) :
  output.ToCsvStream(columnsData);

            var fileStreamResult = new FileStreamResult(exportStream, mimeType);
            fileStreamResult.FileDownloadName = fileName;
            fileStreamResult.FileStream.Seek(0, SeekOrigin.Begin);

            return fileStreamResult;
        }

        private void ChangeCellStyle(ExportCellStyle e)
        {
            bool isHeader = e.Row == 0;
            SpreadBorder border = new SpreadBorder(SpreadBorderStyle.Thin, new SpreadThemableColor(new SpreadColor(0, 0, 0)));
            SpreadCellFormat format = new SpreadCellFormat
            {
                ForeColor = isHeader ? SpreadThemableColor.FromRgb(50, 54, 58) : SpreadThemableColor.FromRgb(0, 0, 0),
                //IsItalic = true,
                VerticalAlignment = SpreadVerticalAlignment.Center,
                WrapText = true,
                TopBorder = border,
                BottomBorder = border,
                RightBorder = border,
                LeftBorder = border,
                Fill = SpreadPatternFill.CreateSolidFill(isHeader ? new SpreadColor(201, 206, 211) : new SpreadColor(255, 255, 255))

            };
            e.Cell.SetFormat(format);
        }

        private void ChangeRowStyle(ExportRowStyle e)
        {
            //e.Row.SetHeightInPixels(e.Index == 0 ? 80 : 30);
        }

        private void ChangeColumnStyle(ExportColumnStyle e)
        {
            double width = e.Name == "PortFolio" || e.Name == "EcoSystem" ? 250 : 100;
            e.Column.SetWidthInPixels(width);

        }
        public IEnumerable<SelectListItem> APROwningCompaniesList(int ClientID)
        {

            // var client = (Client)Session["Client"];
            // int ClientId = client.Id;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var companiesList = unitOfWork.APROwningCompany.Get().Where(x => x.ClientId == ClientID).ToList();
            var item = companiesList.OrderBy(x=>x.OwningCompany).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.OwningCompany.ToString() });
            return item;

        }

        public IEnumerable<SelectListItem> InterfaceMethodList(int ClientID)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

            ///change to meta data values
            ///
            var applicationList = unitOfWork.Application.Get(x => x.ClientId == ClientID).ToList();

            var item = applicationList.OrderBy(x => x.ApplicationName).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.ApplicationName.ToString(), Selected = true });

            return item;

        }

        public IEnumerable<SelectListItem> DeploymentModelList(int ClientID)
        {

            // var client = (Client)Session["Client"];
            // int ClientId = client.Id;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var deploymentList = unitOfWork.DeploymentModel.Get().Where(x => x.ClientId == ClientID).ToList();
            var item = deploymentList.OrderBy(x => x.DeploymentModel).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.DeploymentModel.ToString() });
            return item;

        }
        public IEnumerable<SelectListItem> StandardStatusList(int ClientID)
        {

            // var client = (Client)Session["Client"];
            // int ClientId = client.Id;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var StandardStatusList = unitOfWork.StandardStatusModel.Get().Where(x => x.ClientId == ClientID).ToList();
            var item = StandardStatusList.OrderBy(x => x.StandardStatus).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.StandardStatus.ToString() });
            return item;

        }
        public IEnumerable<SelectListItem> DifferentiationLevelList(int ClientID)
        {

            // var client = (Client)Session["Client"];
            // int ClientId = client.Id;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var differentiationLevelList = unitOfWork.DifferentiationLevelModel.Get().Where(x => x.ClientId == ClientID).ToList();
            var item = differentiationLevelList.OrderBy(x => x.DifferentiationLevel).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.DifferentiationLevel.ToString() });
            return item;

        }

        public IEnumerable<SelectListItem> InterfaceMethodmetadataList(int ClientID)
        {

          
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var InterfaceMethodList = unitOfWork.APRInterfaceMethod.Get().Where(x => x.ClientId == ClientID).ToList();
            var item = InterfaceMethodList.OrderBy(x => x.InterfaceMethod).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.InterfaceMethod.ToString() });
            return item;

        }

        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SetInterfaceDetail(AprInterfaceModel AprInterfaceModel)
        {
            //var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            var res = PortfolioAnalyzerHelper.UpdateAprInterfaceModel(AprInterfaceModel,client.ClientId);


            return Json(AprInterfaceModel);
        }
        [HttpPost]
        public ActionResult GetInterfaceDetail(int AppId)
        {
            //var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            if (AppId != 0)
            {
                var model = PortfolioAnalyzerHelper.getInterfaceDetail(AppId);                
                return Json(model);
            }
            else
            {
                return Json(new { Failure = "Select any Application" });
            }
        }
        public IEnumerable<SelectListItem> APRDispositionSolutionlist(int ClientID)
        {

            // var client = (Client)Session["Client"];
            // int ClientId = client.Id;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.APRDispositionSolution.Get().Where(x => x.ClientId == ClientID).ToList();
            var item = applicationList.OrderBy(x => x.DispositionSolution).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.DispositionSolution.ToString() });
            return item;
        }
        public IEnumerable<SelectListItem> APRAppInventoryList(int ClientID)
        {

            // var client = (Client)Session["Client"];
            // int ClientId = client.Id;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.Application.Get().Where(x => x.ClientId == ClientID).ToList();
            var item = applicationList.OrderBy(x => x.ApplicationName).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.ApplicationName.ToString() });
            return item;
        }

        //public IEnumerable<SelectListItem> GetPlannedDisposition(int PlannedDisposition)
        //{

        //    var client = (Client)Session["Client"];
        //    int ClientId = client.Id;
        //    var contextFactory = new DbContextFactory();
        //    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
        //    var applicationList = unitOfWork.APRDispAndDispSollution.Get().Where(x => x.ClientId == ClientId && x.DispId == PlannedDisposition).ToList();
        //    var item = applicationList.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.DispSolutionId.ToString() });
        //    return item;
        //}
        //public virtual JsonResult APRSelectedDispositionSolutionlist(int PlannedDisposition = 0)
        //{
          
        //    return Json(GetPlannedDisposition(PlannedDisposition).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public ActionResult APRSelectedDispositionSolutionlist([DataSourceRequest] DataSourceRequest request, int PlannedDisposition = 4)
        //{
        //    Client client = (Client)Session["Client"];
        //    int uid = (int)Session["UserId"];
        //    IEnumerable<DispositionDetailsModel> model = new List<DispositionDetailsModel>();
        //    if (PlannedDisposition != 0)
        //    {
        //         model = PortfolioAnalyzerHelper.GetPlannedDisposition(PlannedDisposition, client.Id);
        //    }
        //    return Json(model.ToDataSourceResult(request));

        //}
        public ActionResult APRSelectedDispositionSolutionlist1(int PlannedDisposition)
        {
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
          //  var contextFactory = new DbContextFactory();
           /// var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
           // List<APRDispositionSolution> applicationList1 = new List<APRDispositionSolution>();
            ////var applicationList = unitOfWork.APRDispAndDispSollution.Get(x => x.ClientId == ClientId && x.DispId== PlannedDisposition).ToList();

            ////foreach (var item in applicationList)
            ////{
            ////    var temp = unitOfWork.APRDispositionSolution.Get(x => x.ClientId == ClientId && x.Id == item.DispSolutionId).ToList();
            ////    applicationList1.AddRange(temp);
            ////}
            Db db = new Db();
            var dispSln = (from dispandln in db.APRDispAndDispSollution
                           join displn in db.APRDispositionSolution on dispandln.DispSolutionId equals displn.Id
                          // join disp in db.APRDisposition on dispandln.DispId equals disp.Id
                           where dispandln.DispId == PlannedDisposition
                           select new 
                           {
                               Id = displn.Id,
                               DispositionSolution = displn.DispositionSolution

                           }
                          ).ToList();

            return Json(dispSln.ToList().Select(A => new { Value = A.Id.ToString(), Text = A.DispositionSolution }), JsonRequestBehavior.AllowGet);

        }
    }
}
