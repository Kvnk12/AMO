using Data;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.IdeaManager.Models;
using WebUI.Areas.IdeaManager.Objects;
using WebUI.Areas.PortfolioAnalyzer.Models;
using WebUI.Areas.PortfolioAnalyzer;
using WebUI.Dto;
using log4net;
using WebUI.Areas.IdeaManager.Dto;
using NTTData.AMOPortal.WebUI.Dto;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;
using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using DocumentFormat.OpenXml.Spreadsheet;
using static DotNetOpenAuth.OpenId.Extensions.AttributeExchange.WellKnownAttributes.Contact;
using Domain.Abstract;
using System.IO;

namespace WebUI.Areas.IdeaManager.Controllers
{
    public class DetailIdeaController : Controller
    {
        //
        // GET: /IdeaManager/DetailIdea/
        private static readonly ILog Log = LogManager.GetLogger(typeof(PostIdeaController));
        public static IEnumerable<IMIdeaReassignModel> reassignData;
        [ActionTrack]
        public ActionResult Index(int IdeaId)
        {

            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);
            // var Cl = (Client)Session["Client"];
            ClientDetail Cl = (ClientDetail)Session["SessionClient"];
            var IdeaAuth = IMHelper.IdeaAuthorize(Cl.ClientId, IdeaId);

            if (IdeaAuth)
            {
                IdeaViewModel IdeaViewModelList = new IdeaViewModel();

                IdeaViewModelList = IMHelper.getIdeaDetails(IdeaId);//
                IdeaViewModelList.BusinessBenefitsList = IMHelper.GetBusinessBenefitsDetails(IdeaId);
                IdeaViewModelList.ITSavingsList = IMHelper.GetITSavingsDetails(IdeaId);
                IdeaViewModelList.PriorityHistory = IMHelper.GetPriorityHistoryDet(IdeaId);
                IdeaViewModelList.ImpPriorityHistory = IMHelper.GetImpPriorityHistoryDetails(IdeaId);
                IdeaViewModelList.StatusHistory = IMHelper.GetStatusHistoryDetails(IdeaId);
                IdeaViewModelList.Approvers = IMHelper.getApproversForView(IdeaId);
                IdeaViewModelList.Comments = IMHelper.GetCommentsDetails(IdeaId);
                IdeaViewModelList.RiskMitigationList = IMHelper.GetRiskAndMitigationDetails(IdeaId);
                IdeaViewModelList.MetricTargetList = IMHelper.GetMetricTargetDetails(IdeaId);
                IdeaViewModelList.DetailClassification = IMHelper.GetDetailClassification(IdeaId);
                IdeaViewModelList.DetailConfigItem = IMHelper.GetDetailConfigItem(IdeaId);
                IdeaViewModelList.ImpPriorityHistoryList = IMHelper.GetImpPriorityHistoryDetails(IdeaId);
                IdeaViewModelList.CompletedAndNextStepList = IMHelper.GetCompletedAndNextStepsDetails(IdeaId);
                return View(IdeaViewModelList);
            }
            else
            {
                return View("IdeaNotFound");
            }
        }

        public string getFullName(int userId)
        {
            NTTData.AMOPortal.Data.Db db = new NTTData.AMOPortal.Data.Db();
            var fullName = (from b in db.UserProfiles
                            where b.Id == userId
                            select b.FirstName + " " + b.LastName).FirstOrDefault();

            return fullName;
        }
        [ActionTrack]
        public ActionResult ReassignIdea()
        {
            int uid = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(uid);
            return View("ReassignIdea");
        }
        [HttpPost]
        public ActionResult ReadAssignIdea([DataSourceRequest] DataSourceRequest request)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var client = (ClientDetail)Session["SessionClient"];
                int ClientId = client.ClientId;
                var IdeaList = IdeaManagerHelper.GetIdeaReassignList(ClientId).Where(x => x.Status != "Declined" || x.Status != "Cancelled" || x.Status != "Implemented");
                reassignData = IdeaList;
                return Json(IdeaList.ToList().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

            }
        }


        public ActionResult getIdeadetail(int? Id)
        {
           var dtl= reassignData.Where(z => z.IdeaId == Id).FirstOrDefault();
          
            return Json(dtl, JsonRequestBehavior.AllowGet);

        }
           public ActionResult SaveNewUser(int Id,int? NewOwnerId,string Ideaname)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];

            IdeaManagerHelper.UpdateIdeaOwner(Id, NewOwnerId,uid, client.ClientId);
            var IdeaLink = Url.Action("Index", "PostIdea", new { ViewIdea = false }, "http");

            var up = unitOfWork.UserProfile.GetByID(NewOwnerId);
            if (up != null)
            {
                SendIdeaNotification(up.Id,up.Email, up.FirstName + " " + up.LastName, Ideaname, IMStatus.Created, client.ClientName, IdeaLink);
            }

            return Json(true, JsonRequestBehavior.AllowGet);

        }
        [ActionTrack]
        public void SendIdeaNotification(int Userid,string email, string fullname, string IdeaName, IMStatus status, string clientname, string IdeaLink)
        {
            try
            {
             //   var body = "";
                var mailId = email;
                var Subject = "Optimization Tool IM AutoNotification";

                var username = "";
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var som = unitOfWork.UserProfile.GetByID(Userid);
                    username = som.FirstName + som.LastName;
                }

                string body = string.Empty;
                using (StreamReader reader = new StreamReader(Server.MapPath("~/Areas/IdeaManager/Templates/ReassignOwnerNotification.html")))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{username}", username);
                body = body.Replace("{idealink}", IdeaLink);
                body = body.Replace("{ideaname}", IdeaName);
                body = body.Replace("{clientname}", clientname);

                App_Start.Helper.SendEMail(mailId, Subject, body);
            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: Reassign Idea Creator " + ex);
            }
        }







    }

}

