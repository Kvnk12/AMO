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
using WebUI.Dto;

namespace WebUI.Areas.ClientProfileManager.Controllers
{
    public class INSurveyController : Controller
    {
       

        public ActionResult Index(int id, string surveyname)
        {
           // var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            //var contextFactory = new DbContextFactory();
            //var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            // ViewData["SurveyId"] = id;
            Survey_Model sm = new Survey_Model();
            sm.Id = id;
            sm.Name = surveyname;
            ViewBag.Message = surveyname; //unitOfWork.Survey.Get(e => e.ClientId == client.Id && e.ToolId == 13).OrderByDescending(o => o.Createddt).FirstOrDefault().Name;

            return View(sm);
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin" }, new string[] { "Client Profile Manager" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ReadINSurvey([DataSourceRequest]DataSourceRequest request,int SurveyId)
        {
           // var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            int surveyid = SurveyId;
            var model = ClientProfileManagerHelper.readINSurvey(client.ClientId, surveyid);
            return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateINSurvey([DataSourceRequest]DataSourceRequest request, SurveyINUserModel model, int TrId = 0)
        {
           // var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            if (model.SurveyUserName.Trim() != "" && model.SurveyUserEmail.Trim() != "" && TrId != 0)
            {
             

                var ii = ClientProfileManagerHelper.addINSurvey(model);
                model.Id = ii;
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateINSurvey([DataSourceRequest]DataSourceRequest request, SurveyINUserModel model)
        {
            var uid = (int)Session["UserId"];
           // if (model.INFieldName.Trim() != "" && model.FieldValue.Trim() != "")
            {

                ClientProfileManagerHelper.updateINSurvey(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteINSurvey([DataSourceRequest]DataSourceRequest request, SurveyINUserModel model)
        {
            ClientProfileManagerHelper.RemoveINSurvey(model.Id);
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
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
        public ActionResult SendSurveyLink(string surId)
        {
           int x = 0;
        string[] SelectedIdeaIds = surId.Trim().Split(',');
            foreach (var i in SelectedIdeaIds)
            {
                int Id = Convert.ToInt32(i);
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var u = unitOfWork.SurveyINUser.GetByID(Id);
                var surINId = u.SurveyId;


               // var client = (Client)Session["Client"];
                var client = (ClientDetail)Session["SessionClient"];
                int usrid = (int)Session["UserId"];
                string Clientname = client.ClientName;
                var info = GetSurveyInfo(surINId);
                if (string.IsNullOrEmpty(u.SurveyUserEmail)) // == null) {
                {   // return Json(x);
                }
                else {
               // var tempexpdate = info.enddate;
                if (string.IsNullOrEmpty(u.UserSurveyToken))// == null)
                {
                    string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                    //u.UserSurveyToken = token 
                    token = token.Replace("+", "=");
                    u.UserSurveyToken = token;
                    u.UpdatedBy = usrid;
                    u.SurveyStatus = "Survey Sent";
                    u.updateddt = DateTime.Now;
                    var c = SurveyHelper.updateINToken(u);
                    var SurveyLink = Url.Action("index", "ClientProfileManager/INUserSurvey", new { nextgensurveyId = surINId, userSurveyToken = token }, "http");
                    //mail param
                    var mailId = u.SurveyUserEmail;
                    //var Subject = info.Description;
                        //Subject = Subject.Length <= 75 ? info.Description : " Application Support & Maintenance Feedback - " + u.TicketId;
                      var  Subject = Clientname + " Application Support & Maintenance Feedback" ;

                        // var body = "Take this survey" + "\n" + SurveyLink;
                        // var body = PopulateBody(SurveyLink,  Clientname, u.TicketId, u.SurveyUserName);
                        var body = PopulateBody(SurveyLink, client.ClientId, u.TicketId, u.SurveyUserName);
                        // str.Substring(0,8);
                        App_Start.Helper.SendEMail(mailId, Subject, body);
                    x = 2;
                }
            }
            }
            return Json(x,JsonRequestBehavior.AllowGet);

        }
        //to show existing survey users
      
        //email body
        private string PopulateBody(string Surveylink,  int clientid, string TickId,string Participant)
        {
            string body = string.Empty;

            using (StreamReader reader = new StreamReader(Server.MapPath("~/Areas/ClientProfileManager/Templates/UserINSurveyLink.html")))
            {
                body = reader.ReadToEnd();
            }

            string clientTag = null;
            // imageDataURL = null;
            var Img = ClientProfileManagerHelper.getProfileClientDetail(clientid);
            string clientname = Img.ClientName;
            if (Img.EmailTemplateImageCPMFile != null)
            {
                string imageBase64Data = Convert.ToBase64String(Img.EmailTemplateImageCPMFile);
                string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                clientTag = "<img src='" + imageDataURL + "' />";// "+ Ntt_Email_logo.png' />"
            }
            else
            {
                clientTag = "<p>"+clientname+"<p/>";// "+ Ntt_Email_logo.png' />"
            }

            body = body.Replace("{clientEmailLogo}", clientTag);
            body = body.Replace("{Link}", Surveylink);
            body = body.Replace("{SurveyUser}", Participant);
          //  body = body.Replace("{expdate}", SurvexpDate);
            body = body.Replace("{clientname}", clientname);
            
            body = body.Replace("{ticketID}", TickId);
            ////body = body.Replace("{Link}", link);

            return body;
        }





    }
}
