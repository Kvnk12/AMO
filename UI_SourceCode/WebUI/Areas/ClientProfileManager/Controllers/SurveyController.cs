using Domain.Entities.ClientProfileManager;
using Newtonsoft.Json;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.ClientProfileManager.Dto;
using WebUI.Areas.ClientProfileManager.ViewModel;
using NTTData.AMOPortal.WebUI.Filters;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using NTTData.AMOPortal.Data;
using Data;
using Domain.Entities.Survey;
using System.IO;
using System.Web.Helpers;
using WebUI.Areas.ClientProfileManager;
using WebUI.Dto;

namespace WebUI.Areas.ClientProfileManager.Controllers
{
    public class SurveyController : Controller
    {
        //[CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        //public ActionResult Index()
        //{
        //    Client client = (Client)Session["Client"];
        //    int ClientId = client.Id;

        //    return null;
        //}                      int surId








        public ActionResult Index()
        {
            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            int usid = (int)Session["UserId"];
            ViewData["ToolList"] = SurveyHelper._ApplicationList(client.ClientId, usid);
            return View();
        }


        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        [HttpPost]
        public ActionResult SurveyInfo([DataSourceRequest] DataSourceRequest request)
        {
            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            int ClientId = client.ClientId;
            var surveyss = SurveyHelper.GetAllSurveys(ClientId, usid);
            return Json(surveyss.AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        //[CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult CreateSurvey([DataSourceRequest] DataSourceRequest request, Survey_Model surveymodel)
        //{
        //    var currentTime = DateTime.Now;
        //    int usid = (int)Session["UserId"];
        //    surveymodel.CreatedBy = usid;
        //    surveymodel.Createddt = currentTime;
        //    surveymodel.UpdatedBy = usid;
        //    surveymodel.updateddt = currentTime;
        //    surveymodel.LaunchedBy = usid;
        //    surveymodel.Launchdt = currentTime;
        //    surveymodel.CreatedByName = SurveyHelper.GetName(usid);
        //    if (surveymodel != null && ModelState.IsValid)
        //    {
        //        NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];


        //        surveymodel.ClientId = client.Id;

        //        var res = SurveyHelper.AddSurvey(surveymodel);
        //        surveymodel.Id = res;


        //    }
        //    ModelState.Clear();
        //    return Json(new[] { surveymodel }.ToDataSourceResult(request, ModelState));

        //}



        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateSurvey([DataSourceRequest] DataSourceRequest request, Survey_Model surveymodel)
        {
            var currentTime = DateTime.Now;
            int usid = (int)Session["UserId"];
            surveymodel.CreatedBy = usid;
            surveymodel.Createddt = currentTime;
            surveymodel.UpdatedBy = usid;
            surveymodel.updateddt = currentTime;
            surveymodel.LaunchedBy = usid;
            surveymodel.Launchdt = currentTime;
            // surveymodel.ToolId 
            surveymodel.CreatedByName = SurveyHelper.GetName(usid);
            if (surveymodel != null && ModelState.IsValid)
            {
                //  NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];


                surveymodel.ClientId = client.ClientId;



                if (surveymodel.SurveyType == "APR")
                {
                    var contextFactory = new DbContextFactory();
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var apr = unitOfWork.Survey.Get().Where(x => x.ClientId == client.ClientId && x.SurveyType == "APR").OrderByDescending(e => e.Createddt).FirstOrDefault();
                    var apr1 = unitOfWork.Survey.Get().Where(x => x.SurveyType == "APR").OrderByDescending(e => e.Createddt).FirstOrDefault();

                    IList<SurveyQuestionModel> Qs = new List<SurveyQuestionModel>();

                    if (apr == null)
                    {
                        if (apr1 != null)
                            Qs = SurveyHelper.GetSurveyQuestionsDetails(apr1.Id);
                    }
                    else
                    {
                        Qs = SurveyHelper.GetSurveyQuestionsDetails(apr.Id);
                    }
                    surveymodel.ClientId = client.ClientId;

                    //var res1 = SurveyHelper.AddSurvey(surveymodel);
                    //apr.Id = res1;
                    var res = SurveyHelper.AddSurvey(surveymodel);
                    surveymodel.Id = res;


                    //getting Options
                    foreach (var q in Qs)
                    {

                        //Qs = SurveyHelper.GetSurveyQuestionsDetails(surveymodel.Id);

                        var Ops = SurveyHelper.GetOptions(q.QId);
                        q.SurveyId = surveymodel.Id;
                        var cqid = SurveyHelper.AddQuestion(q);
                        q.QId = cqid;

                        foreach (var op in Ops)
                        {
                            op.SurveyModelId = q.QId;
                            var oid = SurveyHelper.addOptions(op);
                            op.Id = oid;
                        }
                    }

                }

                else
                {
                    var res = SurveyHelper.AddSurvey(surveymodel);
                    surveymodel.Id = res;
                }

            }
            ModelState.Clear();
            return Json(new[] { surveymodel }.ToDataSourceResult(request, ModelState));

        }




        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        [HttpPost]
        public ActionResult UpdateSurvey([DataSourceRequest] DataSourceRequest request, Survey_Model surveymodel)
        {

            if (surveymodel != null && ModelState.IsValid)
            {
                int usid = (int)Session["UserId"];
                surveymodel.UpdatedBy = usid;
                var res = SurveyHelper.Update(surveymodel);
            }

            ModelState.Clear();
            return Json(new[] { surveymodel }.ToDataSourceResult(request, ModelState));
        }






        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DestroySurvey([DataSourceRequest] DataSourceRequest request, Survey_Model surveymodel)
        {
            if (surveymodel != null)
            {
                SurveyHelper.Delete(surveymodel.Id);
            }

            return Json(new[] { surveymodel }.ToDataSourceResult(request, ModelState));
        }











        public ActionResult SetQues(int surrId)
        {
            //var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];

            Survey_Model sur = new Survey_Model();
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            sur.Id = surrId;
            sur.Name = SurveyHelper.surveyDetails(surrId);
            sur.ClientId = client.ClientId;
            ViewData["PerspectiveTypes"] = SurveyHelper._getPerspectiveTypes(client.ClientId, surrId);
            ViewData["EstimateTypes"] = SurveyHelper._getEstimateTypes(client.ClientId);
            ViewData["EstimateCategories"] = SurveyHelper._getEstimateCategories(client.ClientId);
            sur.IsLinkSent = unitOfWork.SurveyUser.Get().Where(g => g.SurveyId == surrId && g.UserSurveyToken != null).Any();
            return View("Ques", sur);
        }


        public ActionResult getSurveyQues([DataSourceRequest]DataSourceRequest request, int suryId)
        {
            //NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            return Json(SurveyHelper.GetSurveyQuestionsDetails(suryId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            //return View("Index", PortfolioAnalyzerHelper.GetPortfolioDetails(client.Id));
        }


        //to add question to survey
        public ActionResult AddQuestion(SurveyQuestionModel SurveyQuestion, int surId)
        {

            if (SurveyQuestion != null && ModelState.IsValid)
            {
                var currentTime = DateTime.Now;
                int usid = (int)Session["UserId"];
                SurveyQuestion.CreatedBy = usid;
                SurveyQuestion.Createddt = currentTime;
                SurveyQuestion.SurveyId = surId;
                var res = SurveyHelper.AddQuestion(SurveyQuestion);
                SurveyQuestion.QId = res;
            }
            return Json(new[] { SurveyQuestion }.ToDataSourceResult(new DataSourceRequest(), ModelState));


        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateQuestion([DataSourceRequest] DataSourceRequest request, SurveyQuestionModel SurveyQuestion)
        {
            if (SurveyQuestion != null && ModelState.IsValid)
            {
                var currentTime = DateTime.Now;
                int usid = (int)Session["UserId"];
                SurveyQuestion.UpdatedBy = usid;
                SurveyQuestion.updateddt = currentTime;
                var res = SurveyHelper.updateQuestion(SurveyQuestion);
            }

            return Json(new[] { SurveyQuestion }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteQuesion([DataSourceRequest] DataSourceRequest request, SurveyQuestionModel SurveyQuestion)
        {
            if (SurveyQuestion != null)
            {
                var v = SurveyHelper.deleteQuestion(SurveyQuestion.QId);
            }

            return Json(new[] { SurveyQuestion }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult GetOptions([DataSourceRequest]DataSourceRequest request, int surQuesId)
        {

            return Json(SurveyHelper.GetOptions(surQuesId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        public ActionResult AddOptions(SurveyRatingFactorModel survQoptions, int surQuesId)
        {
            if (survQoptions != null && ModelState.IsValid)
            {
                //var currentTime = DateTime.Now;
                int usid = (int)Session["UserId"];
                survQoptions.CreatedBy = usid;
                // survQoptions.c = currentTime;
                survQoptions.SurveyModelId = surQuesId;
                var res = SurveyHelper.addOptions(survQoptions);
                survQoptions.Id = res;
            }
            return Json(new[] { survQoptions }.ToDataSourceResult(new DataSourceRequest(), ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateOptions([DataSourceRequest] DataSourceRequest request, SurveyRatingFactorModel survQoptions)
        {
            if (survQoptions != null && ModelState.IsValid)
            {
                var res = SurveyHelper.updateOptions(survQoptions);
            }

            return Json(new[] { survQoptions }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteOptions([DataSourceRequest] DataSourceRequest request, SurveyRatingFactorModel survQoptions)
        {
            if (survQoptions != null)
            {
                var v = SurveyHelper.deleteOptions(survQoptions.Id.Value);
            }

            return Json(new[] { survQoptions }.ToDataSourceResult(request, ModelState));
        }

        //dropdown values for perspective  type
        public IEnumerable<SelectListItem> _getPerspectiveTypes(int ClientID,int SurveyId)
        {

            // var client = (Client)Session["Client"];
            // int ClientId = client.Id;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();
            // var ToolNo = unitOfWork.Survey.Get().Where(x => x.ClientId == ClientID && x.Id == SurveyId).Select(x => x.ToolId).ToList();

            var perspectCat = (from su in db.Survey


                               join sp in db.SurveyPerspective on su.ToolId equals sp.ToolId
                               where su.ClientId == ClientID && su.Id == SurveyId

                               select new { Id = sp.Id, App = sp.PerspectiveName }).ToList();


            var item = perspectCat.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.App.ToString() });
            return item;
        }

        //Manage Survey

        //to get individual survey info
        public static Survey_Model GetSurveyInfo(int SurveyId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var entityMapper = new Mapper<Survey_Model, Survey>();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var SurveyInfo = unitOfWork.Survey.GetByID(SurveyId);
                var surveyInformationModel = new Survey_Model();
                entityMapper.MapToEntity(SurveyInfo, surveyInformationModel);
                // AppViewModels.Add(ApplicationInformationModel);
                return surveyInformationModel;
            }
        }

        [HttpPost]
        public ActionResult ReadSurveyUsers([DataSourceRequest]DataSourceRequest request, int suryId)
        {
            // NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            return Json(GetPersonnelUsers(client.ClientId, suryId).AsEnumerable().ToDataSourceResult(request));
            //return View("Index", PortfolioAnalyzerHelper.GetPortfolioDetails(client.Id));
        }



        public ActionResult ManageSurvey(int surrID, string snma)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var ToolNo = unitOfWork.Survey.GetByID(surrID).ToolId;
            Survey_Model sur = new Survey_Model();
            sur.Id = surrID;
            sur.Name= snma;
            if (ToolNo == 13)
            {

                return RedirectToAction("Index", "INSurvey", new { id=sur.Id,surveyname=sur.Name});

            }
            else return PartialView("SurveyManage", sur);
        }

        //to show existing survey users
        private static IEnumerable<SurveyUserModel> getSurvyyUsers( int surveyid)
        {

            Db db = new Db();


            var personnellist = (from k in db.Personnels

                                 join s in db.SurveyUser on k.Id equals s.PersonnelId
                                 join sa in db.Survey on s.SurveyId equals sa.Id
                                 join app in db.Applications on s.SurveyForRefId equals app.Id into tmpApps
                                 where (s.SurveyId == surveyid && k.FirstName != "APR" &&k.LastName!="RatingUser")
                                 from ap in tmpApps.DefaultIfEmpty()
                                 select new SurveyUserModel
                                 {
                                     Id = s.Id,
                                     SurveyName = sa.Name,
                                     surveyType=sa.SurveyType,
                                     UserSurveyToken = s.UserSurveyToken,
                                     SurveyId = s.SurveyId,
                                     PersonnelId = k.Id,
                                     PersonnelName = k.FirstName + " " + k.LastName,
                                     Email = k.EmailAddress,
                                     SurveyFor = ap.ApplicationName,
                                     SurveyForRefId = s.SurveyForRefId,
                                     surveyenddate = sa.enddate,
                                     Completedt = s.Completedt


                                 }
                                ).ToList();
            var xh = personnellist.Select(s => new SurveyUserModel
            {

                Id = s.Id,
                UserSurveyToken = s.UserSurveyToken,
                SurveyId = s.SurveyId,
                PersonnelId = s.PersonnelId,
                PersonnelName = s.PersonnelName,
                Email = s.Email,
                surveyenddate = s.surveyenddate,
                Completedt = s.Completedt,
                SurveyFor = s.SurveyFor,
                SurveyForRefId = s.SurveyForRefId == null ? 0 : s.SurveyForRefId

            })
                .AsEnumerable();


            return xh;
        }


        public static IEnumerable<SurveyUserModel> GetPersonnelUsers(int clientId , int surveyid)
        {

            Db db = new Db();

            var personnellist = (from k in db.PersonnelRole
                                 join i in db.Personnels on k.PersonnelId equals i.Id
                                 join app in db.Applications on k.AppId equals app.Id into tmpApps
                                 //join s in db.SurveyUser.Where(w => w.SurveyId == surveyid) on i.Id equals s.PersonnelId into tempuserlist
                                 where (k.ClientId == clientId)
                                 from ap in tmpApps.DefaultIfEmpty()
                                     //from suv in tempuserlist.DefaultIfEmpty()
                                 select new SurveyUserModel
                                 {
                                     // Id = suv.Id,
                                     //SurveyId = suv.SurveyId,
                                     PersonnelId = i.Id,
                                     PersonnelName = i.FirstName + " " + i.LastName,
                                     Email = i.EmailAddress,
                                     SurveyForRefId=k.AppId,
                                     //  idforgridrow = i.Id.ToString()+"-"+k.AppId.Value.ToString(),
                                     SurveyFor= ap.ApplicationName
                                 }
                                ).ToList();
            var xh = personnellist.Select(s => new SurveyUserModel {
                // SurveyId = s.SurveyId,
                PersonnelId = s.PersonnelId,
                PersonnelName = s.PersonnelName,
                Email = s.Email,
                SurveyForRefId =s.SurveyForRefId,
                // idforgridrow = s.PersonnelId.ToString()+"-"+ s.SurveyForRefId==null ? "0": s.SurveyForRefId.ToString(),
                // idforgridrow = s.PersonnelId.ToString() + "-" + s.SurveyForRefId.ToString(),
                SurveyFor = s.SurveyFor
            }).AsEnumerable();


            return xh;
        }

        [HttpPost]
        public ActionResult addPersonnel(int surId,string parpId)
        {
            int Perid = (int)Session["UserId"];
            string[] arr = parpId.Split(",".ToCharArray());
            for (int i=0; i<arr.Length; i++)
            {
                SurveyUserModel asd = new SurveyUserModel();
                string[] ids = arr[i].Split("-".ToCharArray());
                int personnelid = Convert.ToInt32(ids[0]);
                if (!String.IsNullOrEmpty (ids[1]))
                    asd.SurveyForRefId = Convert.ToInt32(ids[1]);
                //  int surrefid = Convert.ToInt32(ids[1]);

                asd.SurveyId = surId;
                asd.PersonnelId = personnelid;

                asd.CreatedBy = Perid;
                var res = SurveyHelper.AddParticipant(asd);
            }
            return null;
        }

        [HttpPost]
        public ActionResult showExistinUsers([DataSourceRequest]DataSourceRequest request,int SurvyID)
        {
            // NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            return Json(getSurvyyUsers(SurvyID).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteSurveyUser([DataSourceRequest] DataSourceRequest request, SurveyUserModel Personnel)
        {
            if (Personnel != null)
            {
                var v = SurveyHelper.deleteSurveyUser(Personnel);
            }

            return Json(new[] { Personnel }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult SendSurveyLink(int surId)
        {
            int x = 0;
            // var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            int usrid = (int)Session["UserId"];
            var info = GetSurveyInfo(surId);
            string Clientname = client.ClientName;
            var tempexpdate = info.enddate;
            if (tempexpdate >= DateTime.Now)
            {
                var uss = getSurvyyUsers(surId).AsEnumerable();
                foreach (var u in uss)
                {
                    if (u.UserSurveyToken == null)
                    {
                        string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                        //u.UserSurveyToken = token 
                        bool userhaslink = SurveyHelper.usersurveylinks(u.SurveyId.Value, u.PersonnelId).Any();
                        token = token.Replace("+", "=");
                        u.UserSurveyToken = token;
                        u.SurveySentDate = DateTime.Now;
                        u.SurveyStatus = "Survey Sent";
                        u.UpdatedBy = usrid;
                        u.updateddt = DateTime.Now;
                        var c = SurveyHelper.updateToken(u);
                        //to orevent multiple emails
                        //var sameuser=
                        if (!userhaslink || (info.SurveyType=="APR" || info.SurveyType == "Rating" || info.SurveyType == "RA"))
                        {
                            var SurveyLink = Url.Action("index", "ClientProfileManager/SurveyUser", new { nextgensurveyId = surId, userSurveyToken = token }, "https");
                            //mail param
                            var mailId = u.Email;
                            var Subject = "Optimization tool Survey";
                            // var body = "Take this survey" + "\n" + SurveyLink;
                            var body = PopulateBody(SurveyLink, tempexpdate.ToString(), Clientname, u.SurveyFor);
                            // 
                            App_Start.Helper.SendEMail(mailId, Subject, body);
                        }
                        x = 2;
                    }

                }
            }
            else
            {
                // this is for : if Date is passed!!
                x = 1;
            }
            return Json(x);
        }


        //email body
        private string PopulateBody(string Surveylink, string SurvexpDate, string clientname, string survyname)
        {
            string body = string.Empty;

            using (StreamReader reader = new StreamReader(Server.MapPath("~/Areas/ClientProfileManager/Templates/UserSurveyLinkEmailInfo.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{Link}", Surveylink);
            //body = body.Replace("{UserName}", Participant);
            body = body.Replace("{expdate}", SurvexpDate);
            body = body.Replace("{clientname}", clientname);
            body = body.Replace("{surveyname}", survyname);
            ////body = body.Replace("{Link}", link);

            return body;
        }





        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Portfolio Analyzer" }, null)]
        public ActionResult GetClientApps()
        {
            if (ViewData["ClientApplicationList"] == null)
            {
                // NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                ViewData["ClientApplicationList"] = ApplicationList(client.ClientId);
                int ClientId = client.ClientId;
            }
            return Json(ViewData["ClientApplicationList"], JsonRequestBehavior.AllowGet);
        }


        public IEnumerable<SelectListItem> ApplicationList(int ClientID)
        {

            // var client = (Client)Session["Client"];
            // int ClientId = client.Id;
            //var contextFactory = new DbContextFactory();
            //var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            //var applicationList = unitOfWork.Application.Get(x => x.ClientId == ClientID).ToList();
            //var item = applicationList.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.ApplicationName.ToString(), Selected = true });
            Db db = new Db();
            var item = (from apps in db.Applications
                        where apps.ClientId == ClientID
                        select new
                        {
                            Value = apps.Id,
                            Text = apps.ApplicationName
                        }
                ).AsEnumerable().Select(s => new SelectListItem
                {
                    Value = s.Value.ToString(),
                    Text = s.Text
                });
            return item;

        }



        //to update Apllication in SurveyUsers table

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateSurveyUserApp([DataSourceRequest] DataSourceRequest request, SurveyUserModel surUSerApp)
        {
            if (surUSerApp != null && ModelState.IsValid)
            {

                var currentTime = DateTime.Now;
                int usid = (int)Session["UserId"];
                surUSerApp.UpdatedBy = usid;
                surUSerApp.updateddt = currentTime;

                if (surUSerApp.SurveyForRefId<=0)
                {
                    surUSerApp.SurveyForRefId = null;
                }
                var res = SurveyHelper.updateUserSurveyApp(surUSerApp);

            }

            return Json(new[] { surUSerApp }.ToDataSourceResult(request, ModelState));
        }


        [HttpPost]
        public JsonResult IsQuesIdExist(string QuestionIdentifier , int surveyId)
        {

            return Json(IsQuesAvailable(QuestionIdentifier, surveyId));

        }

        private bool IsQuesAvailable(string QuestionIdentifier, int surveyId)
        {
            Db db = new Db();
            return !db.SurveyModel.Any(k => k.QuestionIdentifier == QuestionIdentifier && k.SurveyId==surveyId);

        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SurveyStatus([DataSourceRequest] DataSourceRequest request)
        {
            // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            //int usid = (int)Session["UserId"];
            int ClientId = client.ClientId;

            return Json(SurveyHelper.surveystatus(ClientId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        //As an index
        public ActionResult SetSDFSDFSDQues()
        {
            SurveyReportModel sur = new SurveyReportModel();
            //sur.Id = surrId;
            //sur.Name = SurveyHelper.surveyDetails(surrId);
            return View("SurveyReport", sur);
        }

        //remainder mail
        [HttpPost]
        public ActionResult SendRemainderLink()
        {
            //int x = 0;
            // var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            int Clientid = client.ClientId;
            var uss = incompleteSurveyUsers(Clientid).AsEnumerable();
            foreach (var u in uss)
            {
                if (u.UserSurveyToken != null)
                {
                    var tempexpdate = u.surveyenddate.Value;
                    if (tempexpdate>=DateTime.Now)
                    {
                        //string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                        //u.UserSurveyToken = token;
                        //var c = SurveyHelper.updateToken(u);
                        var SurveyLink = Url.Action("index", "ClientProfileManager/SurveyUser", new { nextgensurveyId = u.SurveyId, userSurveyToken = u.UserSurveyToken }, "https");
                        //mail param
                        var mailId = u.Email;
                        var Subject = "Remainder: AMO Survey";


                        // var body = "Take this survey" + "\n" + SurveyLink;
                        var body = remainderBody(SurveyLink, tempexpdate.ToString(), u.clientname, u.SurveyFor);
                        // 
                        App_Start.Helper.SendEMail(mailId, Subject, body);

                    }

                }
            }
            return null;
        }


        private string remainderBody(string Surveylink, string SurvexpDate, string clientname, string survyname)
        {
            string body = string.Empty;

            using (StreamReader reader = new StreamReader(Server.MapPath("~/Areas/ClientProfileManager/Templates/SurveyRemainderEmailInfo.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{Link}", Surveylink);
            //body = body.Replace("{UserName}", Participant);
            body = body.Replace("{expdate}", SurvexpDate);
            body = body.Replace("{clientname}", clientname);
            body = body.Replace("{surveyname}", survyname);
            ////body = body.Replace("{Link}", link);

            return body;
        }

        private static IEnumerable<SurveyUserModel> incompleteSurveyUsers(int clientId)
        {

            Db db = new Db();


            var personnellist = (from k in db.SurveyUser
                                 join s in db.Survey on k.SurveyId equals s.Id
                                 join p in db.Personnels on k.PersonnelId equals p.Id
                                 join a in db.Applications on k.SurveyForRefId equals a.Id into tempapplist
                                 join c in db.Clients on s.ClientId equals c.Id
                                 where (k.Completedt == null && s.ClientId== clientId && k.UserSurveyToken !=null)
                                 from app in tempapplist.DefaultIfEmpty()
                                 select new SurveyUserModel
                                 {

                                     SurveyName = s.Name,
                                     surveyenddate = s.enddate,
                                     PersonnelId = k.PersonnelId,
                                     Email = p.EmailAddress,
                                     SurveyId = s.Id,
                                     UserSurveyToken = k.UserSurveyToken,
                                     clientname = c.ClientName,
                                     SurveyFor = app.ApplicationName


                                 }
                                ).ToList();
            var xh = personnellist.Select(s => new SurveyUserModel
            {
                SurveyId = s.SurveyId,
                SurveyName = s.SurveyName,
                surveyenddate = s.surveyenddate,
                PersonnelId = s.PersonnelId,
                Email = s.Email,
                UserSurveyToken = s.UserSurveyToken,
                clientname = s.clientname,
                SurveyFor = s.SurveyFor
            })
                .AsEnumerable();


            return xh;
        }

        public ActionResult GetEstimateType()
        {
            //  var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            var estiMateList = SurveyHelper._getEstimateTypes(client.ClientId);
            return Json(estiMateList, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetEstimateCategory()
        {
            //var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            Db db = new Db();

            var estiMateCatList = SurveyHelper._getEstimateCategories(client.ClientId);
            return Json(estiMateCatList, JsonRequestBehavior.AllowGet);
        }


        //for cloning survey
        //public ActionResult clonesurveyindex(int cloneSid)
        //{
        //    Client client = (Client)Session["Client"];
        //    int ClientId = client.Id;
        //    var sd = SurveyHelper.singlesurveyDetails(cloneSid);
        //    return View("CloneSurvey", sd);
        //}

        //[AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult cloneSurvey([DataSourceRequest] DataSourceRequest request, Survey_Model surveymodel/*, string btnsurvey*/)
        {
            // ModelState.Clear();
            var currentTime = DateTime.Now;
            int usid = (int)Session["UserId"];
            //  Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int OriginalSurveyId = surveymodel.Id;
            IList<SurveyUserModel> sum = new List<SurveyUserModel>();
            surveymodel.CreatedBy = usid;
            surveymodel.Createddt = currentTime;
            surveymodel.CreatedByName = SurveyHelper.GetName(usid);
            var nnnname = SurveyHelper.singlesurveyDetails(surveymodel.Id);
            if (surveymodel.Name == nnnname)
            {
                surveymodel.Name ="Copy_"+nnnname;
            }
            //if (btnsurvey == "newapr")
            //{
            //    var contextFactory = new DbContextFactory();
            //    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            //    var APR = unitOfWork.Survey.Get().Where(x => x.ClientId == client.Id && x.SurveyType == "APR").OrderByDescending(e => e.Createddt).FirstOrDefault();
            //}

            if (surveymodel != null && ModelState.IsValid)
            {

                if (surveymodel.Id != 0)
                {

                    //getting Questions
                    var Qs = SurveyHelper.GetSurveyQuestionsDetails(surveymodel.Id);
                    surveymodel.ClientId = client.ClientId;

                    var res = SurveyHelper.AddSurvey(surveymodel);
                    surveymodel.Id = res;


                    //getting Options
                    foreach (var q in Qs)
                    {
                        var Ops = SurveyHelper.GetOptions(q.QId);
                        q.SurveyId = surveymodel.Id;
                        var cqid = SurveyHelper.AddQuestion(q);
                        q.QId = cqid;

                        foreach (var op in Ops)
                        {
                            op.SurveyModelId = q.QId;
                            var oid = SurveyHelper.addOptions(op);
                            op.Id = oid;
                        }
                    }
                    //TempData.Add("ValidationSuccessMsg", "Survey Cloned Successfully");
                    try
                    {
                        sum = SurveyHelper.getpersonSurvey(OriginalSurveyId);
                        foreach (var i in sum)
                        {
                            i.CreatedBy = usid;
                            i.SurveyId = surveymodel.Id;
                            i.Createddt = DateTime.Now;

                        }
                    }
                    catch (Exception)
                    { }
                    try
                    {
                        var t = SurveyHelper.CloneParticipants(sum, OriginalSurveyId);
                    }
                    catch { }
                    ModelState.Clear();
                    return RedirectToAction("Index");
                }

                else
                {
                    ModelState.AddModelError("SaveErrMsg", "You cannot Clone, because you are adding new Survey.");
                    return View("Index");
                }

            }
            else {

                return RedirectToAction("Index");
            }


        }


        public ActionResult Emailconfig(int surrID, string snma)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Survey_ScheduleEmailConfigModel sur = new Survey_ScheduleEmailConfigModel();
            sur.Survey_ID = surrID;
            var su = unitOfWork.SB_ScheduleEmailConfig.Get().Where(x => x.Survey_ID == surrID).OrderByDescending(e => e.CreatedDatetime).FirstOrDefault();

            if (su != null)
            {
                
                    sur.Ack_Message = su.Ack_Message.ToString();
                    sur.FirstReminder_Message = su.FirstReminder_Message.ToString();
                    sur.SecondReminder_Message = su.SecondReminder_Message.ToString();
                    sur.Id = su.Id;

                    return PartialView("_EmailConfig", sur);    
            }
            else

                return PartialView("_EmailConfig", sur);
        }
        

        [HttpPost]
        public ActionResult AddSurveyEmailConfig( int SurveyID, string Ack_Message, string FirstReminder_Message, string SecondReminder_Message)
        {
            int usid = (int)Session["UserId"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            Db db = new Db();
            Survey_ScheduleEmailConfigModel asd = new Survey_ScheduleEmailConfigModel();
            asd.ClientId = client.ClientId;
            asd.Survey_ID = SurveyID;
            asd.Ack_Message = Ack_Message;
            asd.FirstReminder_Message = FirstReminder_Message;
            asd.SecondReminder_Message = SecondReminder_Message;
            asd.updatedBy=usid;
            var UpdateID = (from a in db.SB_ScheduleEmailConfig
                            where a.Survey_ID == SurveyID 
                            select a).ToList().FirstOrDefault();


            if (UpdateID != null)
            {
                asd.Id = UpdateID.Id;
                var res = SurveyHelper.UpdateSurveyEmailConfigs(asd);
                
            }
            else
            {
                var res = SurveyHelper.AddSurveyEmailConfigs(asd);
            }

            return null;
        }

    }
}

     
     