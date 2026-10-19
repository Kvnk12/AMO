using Data;
using Domain.Entities;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.IdeaManager.Dto;
using WebUI.Areas.IdeaManager.Models;
using WebUI.Dto;
using log4net;
using NTTData.AMOPortal.WebUI.Filters;
using System.Web.Hosting;

namespace WebUI.Areas.IdeaManager.Controllers
{
    public class PostIdeaController : Controller
    {
        //
        // GET: /IdeaManager/PostIdea/
        private static readonly ILog Log = LogManager.GetLogger(typeof(PostIdeaController));
        [ActionTrack]
        public ActionResult Index(bool ViewIdea = true, int IdeaId = 0, string tab = "")
        {
            try {
                //var client = (Client)Session["Client"];
                var client = (ClientDetail)Session["SessionClient"];
                var uid = (int)Session["UserId"];
                //Authorize Idea
                var IdeaAuth = IMHelper.IdeaAuthorize(client.ClientId, IdeaId);
                ViewData["GetStringResult"] = getFullName(uid);
                var user = GetDropDownListItems.UserRoleForClient(uid, client.ClientId);
                var adminList = IMHelper.ListOfAdminsSysAdmins(client.ClientId);
                var IsNttDataResource = IsNttResource(uid);

                if (IdeaId == 0)
                {
                    IdeaViewModel cv = new IdeaViewModel();
                    cv.PostNewIdea = true;
                    cv.ViewIdea = false;
                    cv.InternalInit = true;
                    cv.IsDeleted = true;
                    if (IsNttDataResource)
                    {
                        cv.IdeaSourceId = "NTT Data";
                    }
                    else
                    {
                        cv.IdeaSourceId = client.ClientName;
                    }
                    cv.IMAutoApprovalSelection = IMHelper.isAutoApproved(client.ClientId);
                    ViewBag.ClassAndCat = IMHelper.GetClassCat(client.ClientId, IdeaId);
                    ViewBag.ConfigItems = IMHelper.GetConfigType(client.ClientId, IdeaId);
                    return View(cv);
                }
                else
                {
                    if (IdeaAuth)
                    {
                        ViewBag.ClassAndCat = IMHelper.GetClassCat(client.ClientId, IdeaId);
                        ViewBag.ConfigItems = IMHelper.GetConfigType(client.ClientId, IdeaId);
                        if (ViewIdea)
                        {
                            var Details = IMHelper.getIdeaDetails(IdeaId);
                            Details.PostNewIdea = false;
                            Details.IMAutoApprovalSelection = IMHelper.isAutoApproved(client.ClientId);
                            var temp = IMHelper.getApprovers(IdeaId);
                            Details.ExistingApproversIds = temp != null ? temp : (new string[0]);


                            if (user.Text.ToLower() == "admin" || user.Text.ToLower() == "sysadmin")
                                Details.IsSysAdmin = true;
                            if (user.Text.ToLower() == "implementationmanager")
                                Details.IsImplementationManager = true;
                            if (Details.CreatedBy == uid)
                                Details.IsIdeaMine = true;

                            return View(Details);
                        }
                        else
                        {
                            var Details = IMHelper.getIdeaDetails(IdeaId);
                            Details.IMAutoApprovalSelection = IMHelper.isAutoApproved(client.ClientId);
                            //For Idea Approvers
                            var temp = IMHelper.getApprovers(IdeaId);
                            Details.ExistingApproversIds = temp != null ? temp : (new string[0]);

                            if (user.Text.ToLower() == "admin" || user.Text.ToLower() == "sysadmin")
                                Details.IsSysAdmin = true;
                            if (user.Text.ToLower() == "implementationmanager")
                                Details.IsImplementationManager = true;
                            if (Details.CreatedBy == uid)
                                Details.IsIdeaMine = true;


                            Details.TabName = tab;

                            return View(Details);
                        }
                    }
                    else
                    {
                        return Content("<h3>Sorry! Idea Not Found.</h3>");
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: Index ", ex);
                throw ex;
            }
        }
        public static bool IsNttResource(int uid)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var IsNTTResource = unitOfWork.UserProfile.Get().Where(g => g.Id == uid && g.NTTDataResource == true).Any();
            return IsNTTResource;
        }

        /// <summary>
        /// Check whether Idea Name already exists.
        /// </summary>
        /// <param name="ItemName"></param>
        /// <param name="ItemId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CheckItemName(string ItemName = "", int ItemId = 0)
        {

            int uid = (int)Session["UserId"];
            //var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var existItemNames = unitOfWork.IMPostIdea.Get().Where(e => e.ItemName.Trim() == ItemName.Trim() && e.ClientId == client.ClientId).ToList();

            bool resultFound = false;
            if (existItemNames.Count() > 0)
            {
                if (ItemId > 0)
                {
                    var temp = existItemNames.Where(e => e.Id == ItemId && e.ItemName.Trim() == ItemName.Trim()).Any();
                    if (temp)
                    {
                        resultFound = false;
                    }
                    else
                    {
                        resultFound = true;
                    }
                }
                else
                {
                    resultFound = true;
                }
            }
            //var existItemNames = from IMPostIdea in db.IMPostIdea
            //                     where IMPostIdea.ItemName.ToLower().Trim() == ItemName.ToLower().Trim() && IMPostIdea.ClientId == client.Id
            //                     select new
            //                     {
            //                         Id = IMPostIdea.Id,
            //                         Name = IMPostIdea.ItemName
            //                     };
            //{ }


            return Json(resultFound, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Save basic(Home Tab) Idea Details.
        /// </summary>
        /// <returns></returns>
        [ActionTrack]
        [HttpPost]
        //public ActionResult SaveBasicIdeaDetails(string IdeaName, int IdeaSourceId, bool InternalInit, int TeamId, string Description, decimal Value = 0, int IdeaTypeId=0,  int LobId=0, int ComplexId=0 )
        public ActionResult SaveBasicIdeaDetails(IdeaViewModel IVM)
        {
            try
            {
                //var client = (Client)Session["Client"];
                var client = (ClientDetail)Session["SessionClient"];
                HomeInput MenuItemsInput = (HomeInput)Session["MenuItemsInput"];
                var uid = (int)Session["UserId"];
                IVM.ClientId = client.ClientId;
                if (client.MultipleApprovals)
                    IVM.MultipleApprovals = client.MultipleApprovals;
                var tempDesc = IVM.Description == null ? null : Server.HtmlDecode(IVM.Description.Trim());
                var tempImpTheme = IVM.ImprovementTheme == null ? null : Server.HtmlDecode(IVM.ImprovementTheme.Trim());
                IVM.Description = tempDesc;
                IVM.ImprovementTheme = tempImpTheme;
                var IdeaLink = Url.Action("Index", "PostIdea", new { ViewIdea = false }, "http");

                if (IVM.IdeaId == 0)
                {
                    var tempId = IMHelper.saveIdea(IVM, uid, IdeaLink, client.ClientName);
                    if (IVM.IMAutoApprovalSelection)
                    {
                        var contextFactory = new DbContextFactory();
                        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                        var listOfAdmins = unitOfWork.IMAutoApprovers.Get().Where(e => e.ClientId == IVM.ClientId).ToList();
                        var temp = "";
                        foreach (var item in listOfAdmins)
                        {
                            temp += item.ApproverId + ",";
                        }
                        var adminlistlength = temp.Length - 1;
                        var adminlist = temp.Substring(0, adminlistlength);
                        SaveAdmins(IVM.IdeaId, adminlist);
                        var up = unitOfWork.UserProfile.GetByID(uid);
                        if (up != null)
                        {
                            SendIdeaNotification(up.Email, up.FirstName + " " + up.LastName, IVM.IdeaName, IMStatus.Created, client.ClientName, IdeaLink);
                        }
                    }
                    return RedirectToAction("Index", new { ViewIdea = false, IdeaId = tempId.IdeaId });
                    //return JavaScript("window.location = '" + Url.Action("Index", new { ViewIdea = false, IdeaId = tempId.IdeaId }) + "'");
                }
                else
                {
                    int id = 0;
                    if ((IVM.IdeaStatusID != (int)IMStatus.SubmitForReview &&
                            IVM.IdeaStatusID != (int)IMStatus.SubmitForApproval &&
                            IVM.IdeaStatusID != (int)IMStatus.Declined &&
                            IVM.IdeaStatusID != (int)IMStatus.Cancelled &&
                            IVM.IdeaStatusID != (int)IMStatus.Implemented) || MenuItemsInput.UserRole.ToLower() == "sysadmin")
                    {
                        var tempId = IMHelper.saveIdea(IVM, uid, IdeaLink, client.ClientName);
                        id = tempId.IdeaId;
                    }
                    return RedirectToAction("Index", new { ViewIdea = false, IdeaId = id });
                    //return JavaScript("window.location = '" + Url.Action("Index", new { ViewIdea = false, IdeaId = id }) + "'");
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: SaveBasicIdeaDetails ", ex);
                throw ex;
            }
        }
        [ActionTrack]
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult SaveIdeaIMPDetails(int IdeaId, int StatusId, int impPriority=0, int impManager=0, DateTime? LastReviewDt = null, DateTime? PSDt = null, DateTime? PEDt = null, DateTime? ASDt = null, DateTime? AEDt = null)
        public ActionResult SaveIdeaIMPDetails(IdeaViewModel IVM)
        {
            try
            {
                // var client = (Client)Session["Client"];
                var client = (ClientDetail)Session["SessionClient"];
                var uid = (int)Session["UserId"];
                if (IVM.IdeaId != 0 && IVM.ImpIdeaStatusID != 0)
                {
                    var IdeaLink = Url.Action("Index", "PostIdea", new { ViewIdea = false, IdeaId = IVM.IdeaId }, "http");
                    IVM.StatusComment = IVM.StatusComment == null ? null : Server.HtmlDecode(IVM.StatusComment.Trim()); ;
                    IMHelper.saveImpDetails(IVM, uid, client.ClientName, IdeaLink);
                }


                //return Json(IVM.IdeaId,JsonRequestBehavior.AllowGet);
                return RedirectToAction("Index", new { ViewIdea = false, IdeaId = IVM.IdeaId, tab = "Implementation" });
                //return JavaScript("window.location = '" + Url.Action("Index", new { ViewIdea = false, IdeaId = IdeaId, tab = "Implementation" }) + "'");
            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: SaveIdeaIMPDetails ", ex);
                throw ex;
            }
        }
        [ActionTrack]
        [HttpPost]
        public ActionResult ReviseIdea(int IdeaId = 0)
        {
            var uid = (int)Session["UserId"];
            if (IdeaId != 0)
            {
                IMHelper.Idea_Revised(IdeaId, uid);
            }
            return Json(new { Success = "Success" });
        }

        //-----------------------------------------------------------------------------------------------------------------------------//
        //-------------------------------------------------STATUS CHANGE Controller Actions--------------------------------------------//
        //-----------------------------------------------------------------------------------------------------------------------------//



        /// <summary>
        /// Submit for Review -- [from CreatedBy Users]
        /// </summary>
        /// <param name="IdeaIDs"></param>
        /// <returns></returns>
        [ActionTrack]
        [HttpPost]
        public ActionResult SubmitforReview(string IdeaIDs = "")
        {
            try
            {
                //  var client = (Client)Session["Client"];
                var client = (ClientDetail)Session["SessionClient"];
                var uid = (int)Session["UserId"];
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                if (IdeaIDs.Trim() != "")
                {
                    var IdeaLink = Url.Action("Index", "PostIdea", new { ViewIdea = false }, "http");
                    submitforreview(IdeaIDs, client.ClientName, uid, IdeaLink);
                    return Json(new { Success = "Success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Failure = "Select any Ideas" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: SubmitforReview ", ex);
                throw ex;
            }
        }

        public void submitforreview(string IdeaIDs, string ClientName, int uid, string IdeaLink)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

            string[] SelectedIdeaIds = IdeaIDs.Split(',');
            foreach (var i in SelectedIdeaIds)
            {
                var _ideaId = Convert.ToInt32(i);
                // (e.StatusId == (int)IMStatus.SubmitForRevision || e.StatusId == (int)IMStatus.Created || ) --- [Removed]
                // -->  Removed: Idea with created Status without Approver cannot be Submitted for Review
                var ExistIdea = unitOfWork.IMPostIdea.GetByID(_ideaId);
                var checkStatus = unitOfWork.IMPostIdea.Get().Where(e => e.Id == _ideaId && (e.StatusId == (int)IMStatus.Revised || e.StatusId == (int)IMStatus.Created)).Any();
                var checkApproverExists = unitOfWork.IMIdeaApprover.Get(e => e.IdeaId == _ideaId).Any();
                if (checkStatus && checkApproverExists)
                {
                    var idea = unitOfWork.IMPostIdea.GetByID(_ideaId);
                    var oldStatus = idea.StatusId;
                    idea.StatusId = (int)IMStatus.SubmitForReview;
                    idea.StatusDt = DateTime.Now;
                    unitOfWork.IMPostIdea.Update(idea);

                    IMIdeaStatusHistory statusHistory = new IMIdeaStatusHistory();
                    statusHistory.IdeaId = idea.Id;
                    statusHistory.LastUpdatedDate = DateTime.Now;
                    statusHistory.UpdatedBy = uid;
                    statusHistory.OldStatusId = oldStatus;
                    statusHistory.NewStatusId = idea.StatusId;
                    unitOfWork.IMIdeaStatusHistory.Insert(statusHistory);

                    unitOfWork.Save();
                    var Approvers = unitOfWork.IMIdeaApprover.Get().Where(e => e.IdeaId == _ideaId);
                    if (Approvers.Count() != 0)
                    {
                        IdeaLink = IdeaLink.Trim() + "&IdeaId=" + _ideaId.ToString();
                        foreach (var appr in Approvers)
                        {
                            var up = unitOfWork.UserProfile.GetByID(appr.ApproverId);
                            if (up != null)
                            {
                                SendIdeaNotification(up.Email, up.FirstName + " " + up.LastName, ExistIdea.ItemName, IMStatus.SubmitForReview, ClientName, IdeaLink);
                            }
                        }
                    }
                }
            }
        }
        [ActionTrack]
        [HttpPost]
        public ActionResult DeleteIdeas(string IdeaIDs = "")
        {
            try
            {
                //var client = (Client)Session["Client"];
                var client = (ClientDetail)Session["SessionClient"];
                var uid = (int)Session["UserId"];
                if (IdeaIDs.Trim() != "")
                {
                    IMHelper.deleteideas(IdeaIDs, client.ClientId, uid);
                    return Json(new { Success = "Success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Failure = "Select any Ideas" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: DeleteIdeas ", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Submit for Approval -- [By Admins and SysAdmins]
        /// </summary>
        /// <param name="IdeaIDs"></param>
        /// <returns></returns>
        [ActionTrack]
        [HttpPost]
        public ActionResult SubmitforApproval(string IdeaIDs = "")
        {
            try
            {
                // var client = (Client)Session["Client"];
                var client = (ClientDetail)Session["SessionClient"];
                var uid = (int)Session["UserId"];
                if (IdeaIDs.Trim() != "")
                {
                    // IMHelper.submitforapproval(IdeaIDs, client.Id, uid);
                    return RedirectToAction("Index", "IdeaApproval", new { page = 1 });
                }
                else
                {
                    return Json(new { Failure = "Select any Ideas" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: SubmitforApproval ", ex);
                throw ex;
            }
        }


        //-----------------//
        //Check metric for Idea > Implementation Status Change 
        public ActionResult CheckMetric(int IdeaId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var IsThere = unitOfWork.IMMetricTarget.Get(f => f.ItemId == IdeaId).Any();
            return Json(IsThere, JsonRequestBehavior.AllowGet);
        }

        //-----------------//
        //Check Comment for Idea > Implementation Status Change 
        public ActionResult CheckComment(int IdeaId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var IsThere = unitOfWork.IMItemComments.Get(f => f.ItemId == IdeaId).Any();
            return Json(IsThere, JsonRequestBehavior.AllowGet);
        }


        //-----------------------------------------------------------------------------------------------------------------------------//
        //-------------------------------------------------TABS Controller Actions---------------------------------------------------//
        //-----------------------------------------------------------------------------------------------------------------------------//


        /*--------------------------------------------------Business Benefits---------------------------------------------------------*/
        [HttpPost]
        public ActionResult GetBusinessBenefits([DataSourceRequest] DataSourceRequest request, int IdeaId = 0)
        {
            return Json(IMHelper.GetBusinessBenefitsDetails(IdeaId).AsEnumerable().ToDataSourceResult(request));
        }
        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddBusinessBenefits([DataSourceRequest] DataSourceRequest request, IMItemBusinessBenefitsInput imBusinessBenefitsInput, int IdeaId = 0)
        {
            try
            {
                //if (imBusinessBenefitsInput != null && imBusinessBenefitsInput.Name.Trim() != "" && imBusinessBenefitsInput.Detail.Trim() != "" && ModelState.IsValid)
                if (imBusinessBenefitsInput != null && imBusinessBenefitsInput.Name.Trim() != "" && IdeaId != 0 && ModelState.IsValid)
                {
                    int usid = (int)Session["UserId"];
                    imBusinessBenefitsInput.CreatedBy = usid;
                    imBusinessBenefitsInput.BenefitType = String.IsNullOrWhiteSpace(imBusinessBenefitsInput.BenefitType) ? "" : imBusinessBenefitsInput.BenefitType;
                    var res = IMHelper.CreateBusinessBenefits(imBusinessBenefitsInput, IdeaId);
                    imBusinessBenefitsInput.Id = res;
                }
                return Json(new[] { imBusinessBenefitsInput }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: AddBusinessBenefits ", ex);
                throw ex;
            }
        }
        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditBusinessBenefits([DataSourceRequest] DataSourceRequest request, IMItemBusinessBenefitsInput imBusinessBenefitsInput)
        {
            try
            {
                //if (imBusinessBenefitsInput != null && imBusinessBenefitsInput.Name.Trim() != "" && imBusinessBenefitsInput.Detail.Trim() != "" && ModelState.IsValid)
                if (imBusinessBenefitsInput != null && imBusinessBenefitsInput.Name.Trim() != "" && ModelState.IsValid)
                {
                    int usid = (int)Session["UserId"];
                    imBusinessBenefitsInput.UpdatedBy = usid;
                    imBusinessBenefitsInput.BenefitType = String.IsNullOrWhiteSpace(imBusinessBenefitsInput.BenefitType) ? "" : imBusinessBenefitsInput.BenefitType;
                    var res = IMHelper.UpdateBusinessBenefits(imBusinessBenefitsInput);
                }
                return Json(new[] { imBusinessBenefitsInput }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: EditBusinessBenefits ", ex);
                throw ex;
            }
        }


        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteBusinessBenefits([DataSourceRequest] DataSourceRequest request, IMItemBusinessBenefitsInput imBusinessBenefitsInput)
        {
            try
            {
                if (imBusinessBenefitsInput != null)
                {
                    var v = IMHelper.RemoveBusinessBenefits(imBusinessBenefitsInput.Id);
                }
                return Json(new[] { imBusinessBenefitsInput }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: DeleteBusinessBenefits ", ex);
                throw ex;
            }
        }


        /*---------------------------IT SAVINGS-----------------------------------*/

        [HttpPost]
        public ActionResult GetITSavings([DataSourceRequest] DataSourceRequest request, int IdeaId = 0)
        {
            //int usid = (int)Session["UserId"];
            return Json(IMHelper.GetITSavingsDetails(IdeaId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddITSavings([DataSourceRequest] DataSourceRequest request, IMItemITBenefitsInput imItemITBenefitsInput, int IdeaId = 0)
        {
            try
            {
                // if (imItemITBenefitsInput != null && imItemITBenefitsInput.Name.Trim() != "" && imItemITBenefitsInput.Detail.Trim() != "" && ModelState.IsValid)
                if (imItemITBenefitsInput != null && imItemITBenefitsInput.Name.Trim() != "" && IdeaId != 0 && ModelState.IsValid)
                {
                    int usid = (int)Session["UserId"];
                    imItemITBenefitsInput.CreatedBy = usid;
                    imItemITBenefitsInput.BenefitType = String.IsNullOrWhiteSpace(imItemITBenefitsInput.BenefitType) ? "" : imItemITBenefitsInput.BenefitType;
                    var res = IMHelper.CreateITSavings(imItemITBenefitsInput, IdeaId);
                    imItemITBenefitsInput.Id = res;
                }
                return Json(new[] { imItemITBenefitsInput }.ToDataSourceResult(request, ModelState));

            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: AddITSavings ", ex);
                throw ex;
            }
        }
        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditITSavings([DataSourceRequest] DataSourceRequest request, IMItemITBenefitsInput imItemITBenefitsInput)
        {
            try
            {
                // if (imItemITBenefitsInput != null && imItemITBenefitsInput.Name.Trim() != "" && imItemITBenefitsInput.Detail.Trim() != "" && ModelState.IsValid)
                if (imItemITBenefitsInput != null && imItemITBenefitsInput.Name.Trim() != "" && ModelState.IsValid)
                {
                    int usid = (int)Session["UserId"];
                    imItemITBenefitsInput.UpdatedBy = usid;
                    imItemITBenefitsInput.BenefitType = String.IsNullOrWhiteSpace(imItemITBenefitsInput.BenefitType) ? "" : imItemITBenefitsInput.BenefitType;
                    var res = IMHelper.UpdateITSavings(imItemITBenefitsInput);
                }
                return Json(new[] { imItemITBenefitsInput }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: EditITSavings ", ex);
                throw ex;
            }
        }


        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteITSavings([DataSourceRequest] DataSourceRequest request, IMItemITBenefitsInput imItemITBenefitsInput)
        {
            try
            {
                if (imItemITBenefitsInput != null)
                {
                    var v = IMHelper.RemoveITSavings(imItemITBenefitsInput.Id);
                }
                return Json(new[] { imItemITBenefitsInput }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: DeleteITSavings ", ex);
                throw ex;
            }
        }


        /*----------------------------------------------------Risk And Mitigation------------------------------------------------------------*/


        [HttpPost]
        public ActionResult GetRiskAndMitigation([DataSourceRequest] DataSourceRequest request, int IId = 0)
        {
            return Json(IMHelper.GetRiskAndMitigationDetails(IId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddRiskAndMitigation([DataSourceRequest] DataSourceRequest request, IMRiskAndMitigationModel imRiskAndMitigationModel, int IId = 0)
        {
            try
            {
                //  if (imRiskAndMitigationModel != null && imRiskAndMitigationModel.Risk.Trim() != "" && imRiskAndMitigationModel.MitigationPlan.Trim() != "" && imRiskAndMitigationModel.Comments.Trim() != "" && ModelState.IsValid)
                if (imRiskAndMitigationModel != null && imRiskAndMitigationModel.Risk.Trim() != "" && imRiskAndMitigationModel.MitigationPlan.Trim() != "" && IId != 0 && ModelState.IsValid)
                {
                    int usid = (int)Session["UserId"];
                    imRiskAndMitigationModel.CreatedBy = usid;
                    imRiskAndMitigationModel.CreatedDt = DateTime.Now;
                    imRiskAndMitigationModel.UpdatedDt = DateTime.Now;
                    var res = IMHelper.CreateRiskAndMitigation(imRiskAndMitigationModel, IId);
                    imRiskAndMitigationModel.Id = res;
                }
                return Json(new[] { imRiskAndMitigationModel }.ToDataSourceResult(request, ModelState));

            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: AddRiskAndMitigation ", ex);
                throw ex;
            }
        }
        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditRiskAndMitigation([DataSourceRequest] DataSourceRequest request, IMRiskAndMitigationModel imRiskAndMitigationModel)
        {
            try
            {
                //if (imRiskAndMitigationModel != null && imRiskAndMitigationModel.Risk.Trim() != "" && imRiskAndMitigationModel.MitigationPlan.Trim() != "" && imRiskAndMitigationModel.Comments.Trim() != "" && ModelState.IsValid)
                if (imRiskAndMitigationModel != null && imRiskAndMitigationModel.Risk.Trim() != "" && imRiskAndMitigationModel.MitigationPlan.Trim() != "" && ModelState.IsValid)
                {
                    int usid = (int)Session["UserId"];
                    imRiskAndMitigationModel.UpdatedBy = usid;
                    var res = IMHelper.UpdateRiskAndMitigation(imRiskAndMitigationModel);
                    imRiskAndMitigationModel.UpdatedDt = DateTime.Now;
                }
                return Json(new[] { imRiskAndMitigationModel }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: EditRiskAndMitigation ", ex);
                throw ex;
            }
        }


        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteRiskAndMitigation([DataSourceRequest] DataSourceRequest request, IMRiskAndMitigationModel imRiskAndMitigationModel)
        {
            try
            {
                if (imRiskAndMitigationModel != null)
                {
                    var v = IMHelper.RemoveRiskAndMitigation(imRiskAndMitigationModel.Id);
                }
                return Json(new[] { imRiskAndMitigationModel }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: DeleteRiskAndMitigation ", ex);
                throw ex;
            }
        }

        /*------------------------------COMMENTS-----------------------------------*/


        [HttpPost]
        public ActionResult GetComments([DataSourceRequest] DataSourceRequest request, int IdeaId = 0)
        {
            int usid = (int)Session["UserId"];
            return Json(IMHelper.GetCommentsDetails(IdeaId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddComments([DataSourceRequest] DataSourceRequest request, IMItemCommentsInput imItemCommentsInput, int IdeaId = 0)
        {
            try
            {
                if (imItemCommentsInput != null && imItemCommentsInput.Comments.Trim() != "" && IdeaId != 0 && ModelState.IsValid)
                {
                    int usid = (int)Session["UserId"];
                    imItemCommentsInput.CreatedBy = usid;
                    var res = IMHelper.CreateComments(imItemCommentsInput, IdeaId);
                    imItemCommentsInput.CreatedByName = IMHelper.GetUserName(usid);
                    imItemCommentsInput.CreatedDate = DateTime.Now;
                    imItemCommentsInput.Id = res;
                }
                return Json(new[] { imItemCommentsInput }.ToDataSourceResult(request, ModelState));

            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: AddComments ", ex);
                throw ex;
            }
        }
        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditComments([DataSourceRequest] DataSourceRequest request, IMItemCommentsInput imItemCommentsInput)
        {
            try
            {
                if (imItemCommentsInput != null && imItemCommentsInput.Comments.Trim() != "" && ModelState.IsValid)
                {
                    int usid = (int)Session["UserId"];
                    imItemCommentsInput.UpdatedBy = usid;
                    var res = IMHelper.UpdateComments(imItemCommentsInput);
                    imItemCommentsInput.CreatedByName = IMHelper.GetUserName(usid);
                    imItemCommentsInput.CreatedDate = DateTime.Now;
                }
                return Json(new[] { imItemCommentsInput }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: EditComments ", ex);
                throw ex;
            }
        }


        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteComments([DataSourceRequest] DataSourceRequest request, IMItemCommentsInput imItemCommentsInput)
        {
            try
            {
                if (imItemCommentsInput != null)
                {
                    var v = IMHelper.RemoveCommentss(imItemCommentsInput.Id);
                }
                return Json(new[] { imItemCommentsInput }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: DeleteComments ", ex);
                throw ex;
            }
        }

        //--------------------------Offline Approver-----------------------------------------
        [HttpPost]
        public ActionResult ReadOfflineApprover([DataSourceRequest] DataSourceRequest request, int IdeaId = 0)
        {
            int usid = (int)Session["UserId"];
            return Json(IMHelper.GetOfflineApproverDetails(IdeaId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //public ActionResult CreateNewApprover([DataSourceRequest] DataSourceRequest request,OfflineApproverModel offAppr, int IdeaId=0)
        //{
        //    try
        //    {
        //        if (offAppr != null && offAppr.ApproverName.Trim() != "" && IdeaId != 0 && ModelState.IsValid)
        //        {
        //            int usid = (int)Session["UserId"];
        //            offAppr.CreatedBy = usid;
        //            var res = IMHelper.CreateOfflineApprdetail(offAppr, IdeaId);
        //            offAppr.CreatedDate = DateTime.Now;
        //            offAppr.Id = res;
        //        }
        //        return Json(new[] { offAppr }.ToDataSourceResult(request, ModelState));

        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error("Error in PostIdeaController: AddOfflineApprover ", ex);
        //        throw ex;
        //    }
        //}
        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateNewApprover([DataSourceRequest] DataSourceRequest request, OfflineApproverModel offAppr, int IId = 0)
        {
            try
            {
                if (offAppr != null && offAppr.ApproverName.Trim() != "" && IId != 0 && ModelState.IsValid)
                {
                    int usid = (int)Session["UserId"];
                    offAppr.CreatedBy = usid;
                    // offAppr.CreatedDate = DateTime.Now;
                    offAppr.LastUpdatedDate = DateTime.Now;
                    var res = IMHelper.CreateOfflineApprdetail(offAppr, IId);
                    offAppr.Id = res;
                }
                return Json(new[] { offAppr }.ToDataSourceResult(request, ModelState));

            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: AddRiskAndMitigation ", ex);
                throw ex;
            }
        }
        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditOfflineApprover([DataSourceRequest] DataSourceRequest request, OfflineApproverModel offAppr)
        {
            try
            {
                if (offAppr != null && offAppr.ApproverName.Trim() != "" && ModelState.IsValid)
                {
                    int usid = (int)Session["UserId"];
                    offAppr.UpdatedBy = usid;
                    var res = IMHelper.UpdateOfflineappr(offAppr);
                    // offAppr.CreatedDate = DateTime.Now;
                }
                return Json(new[] { offAppr }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: EditoffApprover ", ex);
                throw ex;
            }
        }


        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteOfflineApprover([DataSourceRequest] DataSourceRequest request, OfflineApproverModel offAppr)
        {
            try
            {
                if (offAppr != null)
                {
                    var v = IMHelper.RemoveoffApprover(offAppr.Id);
                }
                return Json(new[] { offAppr }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: DeleteoffApprover ", ex);
                throw ex;
            }
        }




        //-------------------------------------MetricTarget-----------------------------

        public ActionResult GetMetricTarget([DataSourceRequest] DataSourceRequest request, int IdeaId = 0)
        {
            try
            {
                IList<MetricTargetModel> list = new List<MetricTargetModel>();
                //  Client client = (Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                if (IdeaId != 0)
                {
                    list = IMHelper.GetMetricTargetDetails(IdeaId);

                }
                return Json(list.AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }


        [ActionTrack]
        [HttpPost]
        public ActionResult AddMetricTarget([DataSourceRequest] DataSourceRequest request, MetricTargetModel metrictargetmodel, int IdeaId = 0)
        {
            try
            {
                if (metrictargetmodel != null && metrictargetmodel.MetricTarget.Trim() != "" && ModelState.IsValid && IdeaId != 0)
                {
                    //var x = Convert.ToInt32(ChallengeId);
                    int usid = (int)Session["UserId"];
                    metrictargetmodel.CreatedBy = usid;
                    metrictargetmodel.UpdatedBy = usid;

                    var res = IMHelper.CreateMetricTarget(metrictargetmodel, IdeaId);

                    metrictargetmodel.Id = res;
                }
                return Json(new[] { metrictargetmodel }.ToDataSourceResult(request, ModelState));

            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: AddMetricTarget ", ex);
                throw ex;
            }
        }

        [ActionTrack]
        [HttpPost]
        public ActionResult EditMetricTarget([DataSourceRequest] DataSourceRequest request, MetricTargetModel metrictargetmodel)
        {
            try
            {
                if (metrictargetmodel != null && ModelState.IsValid && metrictargetmodel.MetricTarget.Trim() != "")
                {
                    int usid = (int)Session["UserId"];
                    metrictargetmodel.UpdatedBy = usid;
                    var res = IMHelper.UpdateMetricTarget(metrictargetmodel);
                }

                return Json(new[] { metrictargetmodel }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: EditMetricTarget ", ex);
                throw ex;
            }
        }

        [ActionTrack]
        [HttpPost]
        public ActionResult DeleteMetricTarget([DataSourceRequest] DataSourceRequest request, MetricTargetModel metrictargetmodel)
        {
            try
            {
                if (metrictargetmodel != null)
                {
                    var v = IMHelper.RemoveMetricTarget(metrictargetmodel.Id);
                }

                return Json(new[] { metrictargetmodel }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: DeleteMetricTarget ", ex);
                throw ex;
            }
        }

        //------------------------------------Analysis, Solution-----------------------------//
        [ActionTrack]
        [HttpPost]
        public ActionResult UpdateAnalysis(int IdeaId = 0, string analysis = "")
        {
            try
            {
                int uid = (int)Session["UserId"];
                var EncodeString = Server.HtmlDecode(analysis);
                if (IdeaId != 0)
                {
                    IMHelper.saveAnalysis(IdeaId, uid, EncodeString);
                }
                return Json(new { EncodeString }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: UpdateAnalysis ", ex);
                throw ex;
            }
        }
        [ActionTrack]
        [HttpPost]
        public ActionResult UpdateSolution(int IdeaId = 0, string solution = "")
        {
            try
            {
                int uid = (int)Session["UserId"];
                var EncodeString = Server.HtmlDecode(solution);
                if (IdeaId != 0)
                {
                    IMHelper.saveSolution(IdeaId, uid, EncodeString);
                }
                return Json(new { EncodeString }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: UpdateSolution ", ex);
                throw ex;
            }
        }

        // ---------------------------------------------------- Approvers(submit) ------------------------------------------------------------ //
        [ActionTrack]
        [HttpPost]
        public ActionResult AddSelectedAdmins(int IdeaId = 0, string ApproverIds = "")
        {
            Db db = new Db();
            //  Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            var Appuser = GetDropDownListItems.UserRoleForClient(uid, client.ClientId);
            string Result = "Success";
            try
            {
                if (ApproverIds == "")
                {
                    throw new Exception("1");
                }
                if (IdeaId != 0)
                {
                    string[] SelectedAdminIds = ApproverIds.Split(',');
                    using (var contextFactory = new DbContextFactory())
                    {
                        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                        var idea = unitOfWork.IMPostIdea.GetByID(IdeaId);
                        //Getting Existing Approvers
                        var ExistingApprovers = unitOfWork.IMIdeaApprover.Get().Where(e => e.IdeaId == IdeaId).Select(e => e.ApproverId).ToList();

                        if (idea.ImpStatusId == null && (idea.StatusId == (int)IMStatus.Created || idea.StatusId == (int)IMStatus.Revised || idea.StatusId == (int)IMStatus.SubmitForRevision))
                        {
                            var OldStatus = idea.StatusId;
                            idea.StatusId = (int)IMStatus.SubmitForReview;
                            idea.StatusDt = DateTime.Now;
                            idea.LastUpdatedDate = DateTime.Now;
                            idea.UpdatedBy = uid;
                            unitOfWork.IMPostIdea.Update(idea);

                            IMIdeaStatusHistory cv = new IMIdeaStatusHistory();
                            cv.Comments = "";
                            cv.IdeaId = IdeaId;
                            cv.OldStatusId = OldStatus;
                            cv.NewStatusId = idea.StatusId;
                            cv.CreatedBy = uid;
                            cv.UpdatedBy = uid;
                            cv.CreatedDate = DateTime.Now;
                            cv.LastUpdatedDate = DateTime.Now;
                            unitOfWork.IMIdeaStatusHistory.Insert(cv);
                            var deleteApprovers = from details in db.IMIdeaApprover
                                                  where details.IdeaId == IdeaId
                                                  select details;

                            foreach (var detail in deleteApprovers)
                            {
                                unitOfWork.IMIdeaApprover.Delete(detail.Id);

                            }
                            foreach (var item in SelectedAdminIds)
                            {
                                if (item != " " || item != "," || item != "")
                                {
                                    var ApproverId = Convert.ToInt32(item);
                                    IMIdeaApprover ideaApprover = new IMIdeaApprover();
                                    ideaApprover.IdeaId = Convert.ToInt32(IdeaId);
                                    ideaApprover.ApproverId = ApproverId;
                                    ideaApprover.Createdby = uid;
                                    ideaApprover.CreatedDt = DateTime.Now;
                                    unitOfWork.IMIdeaApprover.Insert(ideaApprover);
                                    //If Approver for Idea already exists then not allow.
                                    //if (ExistingApprovers.Contains(ApproverId) == false)
                                    //    {
                                    //        unitOfWork.IMIdeaApprover.Insert(ideaApprover);
                                    //    }

                                    if (OldStatus == (int)IMStatus.Revised)
                                    {
                                        var u = unitOfWork.UserProfile.Get(g => g.Id == ideaApprover.ApproverId).FirstOrDefault();
                                        var IdeaLink = Url.Action("Index", "PostIdea", new { ViewIdea = false, IdeaId = idea.Id }, "http");
                                        SendIdeaNotification(u.Email, u.FirstName + " " + u.LastName, idea.ItemName, IMStatus.SubmitForReview, client.ClientName, IdeaLink);
                                    }
                                    else
                                    {
                                        var u = unitOfWork.UserProfile.Get(g => g.Id == ideaApprover.ApproverId).FirstOrDefault();
                                        var IdeaLink = Url.Action("Index", "PostIdea", new { ViewIdea = false, IdeaId = idea.Id }, "http");
                                        SendIdeaNotification(u.Email, u.FirstName + " " + u.LastName, idea.ItemName, IMStatus.SubmitForReview, client.ClientName, IdeaLink);
                                    }

                                }
                            }

                        }
                        else if (Appuser.Text.ToLower() == "sysadmin")
                        {
                            var OldStatus = idea.StatusId;
                            var deleteApprovers = from details in db.IMIdeaApprover
                                                  where details.IdeaId == IdeaId
                                                  select details;

                            foreach (var detail in deleteApprovers)
                            {
                                unitOfWork.IMIdeaApprover.Delete(detail.Id);

                            }
                            foreach (var item in SelectedAdminIds)
                            {
                                if (item != " " || item != "," || item != "")
                                {
                                    var ApproverId = Convert.ToInt32(item);
                                    IMIdeaApprover ideaApprover = new IMIdeaApprover();
                                    ideaApprover.IdeaId = Convert.ToInt32(IdeaId);
                                    ideaApprover.ApproverId = ApproverId;
                                    ideaApprover.Createdby = uid;
                                    ideaApprover.CreatedDt = DateTime.Now;
                                    unitOfWork.IMIdeaApprover.Insert(ideaApprover);

                                    if (OldStatus == (int)IMStatus.Revised)
                                    {
                                        var u = unitOfWork.UserProfile.Get(g => g.Id == ideaApprover.ApproverId).FirstOrDefault();
                                        var IdeaLink = Url.Action("Index", "PostIdea", new { ViewIdea = false, IdeaId = idea.Id }, "http");
                                        SendIdeaNotification(u.Email, u.FirstName + " " + u.LastName, idea.ItemName, IMStatus.SubmitForReview, client.ClientName, IdeaLink);
                                    }
                                    else
                                    {
                                        var u = unitOfWork.UserProfile.Get(g => g.Id == ideaApprover.ApproverId).FirstOrDefault();
                                        var IdeaLink = Url.Action("Index", "PostIdea", new { ViewIdea = false, IdeaId = idea.Id }, "http");
                                        SendIdeaNotification(u.Email, u.FirstName + " " + u.LastName, idea.ItemName, IMStatus.SubmitForReview, client.ClientName, IdeaLink);
                                    }
                                }
                            }

                        }
                        unitOfWork.Save();
                    }
                }

                else
                {
                    //throw new Exception("Select Idea/Submit the one!");
                    throw new Exception("2");
                }

            }
            catch (Exception exp)
            {
                Log.Error("Error in PostIdeaController: New_KeywordReportList " + exp);
                return Json(new { Result = "ERROR", Message = exp.Message });
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        // ----------------------------------------------------Save Approvers ------------------------------------------------------------ //
        [ActionTrack]
        [HttpPost]
        public ActionResult SaveAdmins(int IdeaId = 0, string ApproverIds = "")
        {
            Db db = new Db();
            // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            var Appuser = GetDropDownListItems.UserRoleForClient(uid, client.ClientId);
            string Result = "Success";
            try
            {

                if (ApproverIds == "")
                {
                    throw new Exception("1");
                }
                if (IdeaId != 0)
                {
                    string[] SelectedAdminIds = ApproverIds.Split(',');
                    using (var contextFactory = new DbContextFactory())
                    {

                        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                        var idea = unitOfWork.IMPostIdea.GetByID(IdeaId);
                        if (idea.StatusId == (int)IMStatus.Created || idea.StatusId == (int)IMStatus.Revised || idea.StatusId == (int)IMStatus.SubmitForRevision)
                        {
                            //Getting Existing Approvers
                            var ExistingApprovers = unitOfWork.IMIdeaApprover.Get().Where(e => e.IdeaId == IdeaId).Select(e => e.ApproverId).ToList();


                            idea.LastUpdatedDate = DateTime.Now;
                            idea.UpdatedBy = uid;
                            unitOfWork.IMPostIdea.Update(idea);



                            var deleteApprovers = from details in db.IMIdeaApprover
                                                  where details.IdeaId == IdeaId
                                                  select details;

                            foreach (var detail in deleteApprovers)
                            {
                                unitOfWork.IMIdeaApprover.Delete(detail.Id);

                            }
                            foreach (var item in SelectedAdminIds)
                            {
                                if (item != " " || item != "," || item != "")
                                {
                                    var ApproverId = Convert.ToInt32(item);
                                    IMIdeaApprover ideaApprover = new IMIdeaApprover();
                                    ideaApprover.IdeaId = Convert.ToInt32(IdeaId);
                                    ideaApprover.ApproverId = ApproverId;
                                    ideaApprover.Createdby = uid;
                                    ideaApprover.CreatedDt = DateTime.Now;
                                    unitOfWork.IMIdeaApprover.Insert(ideaApprover);
                                    //If Approver for Idea already exists then not allow.
                                    //if (ExistingApprovers.Contains(ApproverId) == false)
                                    //    {
                                    //        unitOfWork.IMIdeaApprover.Insert(ideaApprover);
                                    //    }

                                }
                            }
                        }

                        else if (Appuser.Text.ToLower() == "sysadmin")
                        {
                            var ExistingApprovers = unitOfWork.IMIdeaApprover.Get().Where(e => e.IdeaId == IdeaId).Select(e => e.ApproverId).ToList();
                            idea.LastUpdatedDate = DateTime.Now;
                            idea.UpdatedBy = uid;
                            unitOfWork.IMPostIdea.Update(idea);


                            //IMIdeaStatusHistory cv = new IMIdeaStatusHistory();
                            //cv.Comments = "";
                            //cv.IdeaId = IdeaId;
                            //cv.OldStatusId = OldStatus;
                            //cv.NewStatusId = idea.StatusId;
                            //cv.CreatedBy = uid;
                            //cv.UpdatedBy = uid;
                            //cv.CreatedDate = DateTime.Now;
                            //cv.LastUpdatedDate = DateTime.Now;
                            //unitOfWork.IMIdeaStatusHistory.Insert(cv);




                            var deleteApprovers = from details in db.IMIdeaApprover
                                                  where details.IdeaId == IdeaId
                                                  select details;

                            foreach (var detail in deleteApprovers)
                            {
                                unitOfWork.IMIdeaApprover.Delete(detail.Id);

                            }
                            foreach (var item in SelectedAdminIds)
                            {
                                if (item != " " || item != "," || item != "")
                                {
                                    var ApproverId = Convert.ToInt32(item);
                                    IMIdeaApprover ideaApprover = new IMIdeaApprover();
                                    ideaApprover.IdeaId = Convert.ToInt32(IdeaId);
                                    ideaApprover.ApproverId = ApproverId;
                                    ideaApprover.Createdby = uid;
                                    ideaApprover.CreatedDt = DateTime.Now;
                                    unitOfWork.IMIdeaApprover.Insert(ideaApprover);

                                }
                            }
                        }



                        unitOfWork.Save();

                    }
                }

                else
                {
                    //throw new Exception("Select Idea/Submit the one!");
                    throw new Exception("2");
                }

            }
            catch (Exception exp)
            {
                Log.Error("Error in PostIdeaController: New_KeywordReportList " + exp);
                return Json(new { Result = "ERROR", Message = exp.Message });
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }



        //-------------------------------------------Completed And Next Steps----------------------------------------------------//

        [HttpPost]
        public ActionResult GetCompletedAndNextSteps([DataSourceRequest] DataSourceRequest request, int IdeaId)
        {
            //int usid = (int)Session["UserId"];
            return Json(IMHelper.GetCompletedAndNextStepsDetails(IdeaId).ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        [HttpPost]
        public ActionResult AddCompletedAndNextSteps([DataSourceRequest] DataSourceRequest request, IMCompletedAndNextStepsModel imCompletedAndNextStepsModel, int idID)
        {
            try
            {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                int? primaryImpManager = IMHelper.getPost(idID, client.ClientId);

                if (Convert.ToInt32(imCompletedAndNextStepsModel.ImpManagers) == primaryImpManager && Convert.ToInt32(imCompletedAndNextStepsModel.ImpManagers) != 0)
                {
                    ModelState["ImpManagers"].Errors.Add("Selected Implementation manager already exist, please select other manager.");
                    return JavaScript("alert('Selected Implementation manager already exist, please select other manager.');");
                }



                if (Convert.ToInt32(imCompletedAndNextStepsModel.ImpManagers) != 0)
                {
                    List<IMCompletedAndNextSteps> list1 = IMHelper.getCompleted(idID, client.ClientId);
                    foreach (var item in list1)
                    {
                        if (item.ImpManagers != null && item.ImpManagers == Convert.ToInt32(imCompletedAndNextStepsModel.ImpManagers))
                        {
                            ModelState["ImpManagers"].Errors.Add("Selected Implementation manager already exist .");
                            return JavaScript("alert('Selected Implementation manager already exist.');");
                            //return Json(new[] { imCompletedAndNextStepsModel }.ToDataSourceResult(request, ModelState));
                            //break;
                        }
                    }

                }

                ////else
                //// {
                ////    ModelState.AddModelError("errorImpMgr", "Duplication in adding Implementation manager");
                ////    return JavaScript("alert('Duplication in adding Implementation manager.');");

                ////}

                //if (imCompletedAndNextStepsModel != null && ModelState.IsValid)
                //{
                imCompletedAndNextStepsModel.UpdatedDt = DateTime.Now;
                int usid = (int)Session["UserId"];
                imCompletedAndNextStepsModel.UpdatedBy = usid;

                ModelState["UpdatedDt"].Errors.Clear();
                var res = IMHelper.CreateCompletedAndNextSteps(imCompletedAndNextStepsModel, idID);
                imCompletedAndNextStepsModel.Id = res;
                return Json(new[] { imCompletedAndNextStepsModel }.ToDataSourceResult(request, ModelState));
                //}
                //else
                //{
                //    return JavaScript("alert('Duplication in adding Implementation manager.');");
                //}

            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: AddCompletedAndNextSteps ", ex);
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        [HttpPost]
        public ActionResult EditCompletedAndNextSteps([DataSourceRequest] DataSourceRequest request, IMCompletedAndNextStepsModel imCompletedAndNextStepsModel, int idID)
        {
            try
            {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                imCompletedAndNextStepsModel.UpdatedBy = usid;
                imCompletedAndNextStepsModel.UpdatedDt = DateTime.Now;

                int? PrimaryImpManager = IMHelper.getPost(idID, client.ClientId);
                if (PrimaryImpManager != null && Convert.ToInt32(imCompletedAndNextStepsModel.ImpManagers) == PrimaryImpManager && Convert.ToInt32(imCompletedAndNextStepsModel.ImpManagers) != 0)
                {
                    ModelState["ImpManagers"].Errors.Add("Selected Implementation manager already exist, please select other manager.");
                    return JavaScript("alert('Selected Implementation manager already exist, please select other manager.');");

                }

                List<IMCompletedAndNextSteps> list1 = IMHelper.getCompleted(idID, client.ClientId);


                if (list1.Count() != 0)
                {
                    foreach (var item in list1)
                    {
                        if (item.ImpManagers != null && item.ImpManagers == Convert.ToInt32(imCompletedAndNextStepsModel.ImpManagers) && Convert.ToInt32(imCompletedAndNextStepsModel.ImpManagers) != 0)
                        {
                            ModelState["ImpManagers"].Errors.Add("Selected Implementation manager already exist, please select other manager.");
                            return JavaScript("alert('Selected Implementation manager already exist, please select other manager.');");

                        }
                    }
                }

                ModelState["UpdatedDt"].Errors.Clear();
                var res = IMHelper.UpdateCompletedAndNextSteps(imCompletedAndNextStepsModel);
                return Json(new[] { imCompletedAndNextStepsModel }.ToDataSourceResult(request, ModelState));


            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: EditCompletedAndNextSteps ", ex);
                throw ex;
            }

        }
        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult DeleteCompletedAndNextSteps([DataSourceRequest] DataSourceRequest request, IMCompletedAndNextStepsModel imCompletedAndNextStepsModel)
        {
            try
            {
                if (imCompletedAndNextStepsModel != null)
                {
                    var v = IMHelper.RemoveCompletedAndNextSteps(imCompletedAndNextStepsModel.Id);
                }

                return Json(new[] { imCompletedAndNextStepsModel }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: DeleteCompletedAndNextSteps ", ex);
                throw ex;
            }
        }




        //----------------------------------------- LOGS -----------------------------------//


        [HttpPost]
        public ActionResult ReadBusinessBenefits([DataSourceRequest] DataSourceRequest request, int IdeaId = 0)
        {
            return Json(IMHelper.GetBusinessBenefitsDetails(IdeaId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ReadITSavings([DataSourceRequest] DataSourceRequest request, int IdeaId = 0)
        {
            return Json(IMHelper.GetITSavingsDetails(IdeaId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReadPriorityHistory([DataSourceRequest] DataSourceRequest request, int IdeaId = 0)
        {
            return Json(IMHelper.GetPriorityHistoryDet(IdeaId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ReadImpPriorityHistory([DataSourceRequest] DataSourceRequest request, int IdeaId = 0)
        {
            return Json(IMHelper.GetImpPriorityHistoryDetails(IdeaId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReadStatusHistory([DataSourceRequest] DataSourceRequest request, int IdeaId = 0)
        {
            return Json(IMHelper.GetStatusHistoryDetails(IdeaId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ReadReassignDetail([DataSourceRequest] DataSourceRequest request, int IdeaId = 0)
        {
            return Json(IMHelper.GetReassignHistoryDetails(IdeaId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        //-----------------------------------------------------------------------------------------------------------------------------//
        //------------------------------------------------------------------------------------------------------------------------------//


        //Ajax List for Dropdown
        public ActionResult TeamList()
        {
            //var ClientID = (Client)Session["Client"];
            var ClientID = (ClientDetail)Session["SessionClient"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.IMTeam.Get(x => x.ClientId == ClientID.ClientId).ToList();
            var item = applicationList.OrderBy(x => x.Team).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Team.ToString(), Selected = true });
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTeamFromSource(string IdeaSource)
        {

            var ClientID = (ClientDetail)Session["SessionClient"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();
            var applicationList = new List<IMTeam>();
            //   applicationList = unitOfWork.IMTeam.Get(x => x.ClientId == ClientID.ClientId && x.IdeaSourceVal.ToLower() == IdeaSource.ToLower()).ToList();
            if (IdeaSource.ToLower() == ClientID.ClientName.ToLower())
            {
                applicationList = unitOfWork.IMTeam.Get(x => x.ClientId == ClientID.ClientId).ToList();
            }
            else
            {
                applicationList = unitOfWork.IMTeam.Get(x => x.ClientId == ClientID.ClientId && x.IdeaSourceVal.ToLower() == IdeaSource.ToLower()).ToList();
            }
            //if (IdeaSource.ToLower()== ClientID.ClientName.ToLower())
            //{
            //     applicationList = unitOfWork.IMTeam.Get(x => x.ClientId == ClientID.ClientId).ToList();
            //}
            //else if(IdeaSource == "NTT Data" || IdeaSource == "NTT DATA_CESA3MM" || IdeaSource == "NTT DATA_CESIVV")
            //{
            //    applicationList = unitOfWork.IMTeam.Get(x => x.ClientId == ClientID.ClientId&&x.IdeaSourceVal==null).ToList();
            //}
            //else
            //{
            //    applicationList = unitOfWork.IMTeam.Get(x => x.ClientId == ClientID.ClientId && x.IdeaSourceVal.ToLower() == IdeaSource.ToLower()).ToList();
            //}
            //var SourceId = unitOfWork.IMIdeaSourceMaster.Get(x => x.ClientId == ClientID.ClientId && x.IdeaSource == IdeaSource).First().Id;
            //var TeamId = (from srm in db.SourceTeamMapping
            //              join tm in db.IMTeam on srm.TeamId equals tm.Id
            //              where srm.IdeaSourceId == SourceId
            //              select tm
            //             );
            var item = applicationList.OrderBy(x => x.Team).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Team.ToString(), Selected = true });
            return Json(item, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetTeamFromSourceforReport(string IdeaSource)
        {

            var ClientID = (ClientDetail)Session["SessionClient"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();
            var applicationList = new List<IMTeam>();
            //applicationList = unitOfWork.IMTeam.Get(x => x.ClientId == ClientID.ClientId && x.IdeaSourceVal.ToLower() == IdeaSource.ToLower()).ToList();
            if (IdeaSource.ToLower() == ClientID.ClientName.ToLower())
            {
                applicationList = unitOfWork.IMTeam.Get(x => x.ClientId == ClientID.ClientId).ToList();
            }
            else
            {
                applicationList = unitOfWork.IMTeam.Get(x => x.ClientId == ClientID.ClientId && x.IdeaSourceVal.ToLower() == IdeaSource.ToLower()).ToList();
            }
            //if (IdeaSource.ToLower() == ClientID.ClientName.ToLower())
            //{
            //    applicationList = unitOfWork.IMTeam.Get(x => x.ClientId == ClientID.ClientId).ToList();
            //}
            //else if (IdeaSource == "NTT Data" || IdeaSource == "NTT DATA_CESA3MM" || IdeaSource == "NTT DATA_CESIVV")
            //{
            //    applicationList = unitOfWork.IMTeam.Get(x => x.ClientId == ClientID.ClientId && x.IdeaSourceVal == null).ToList();
            //}
            //else
            //{
            //    applicationList = unitOfWork.IMTeam.Get(x => x.ClientId == ClientID.ClientId && x.IdeaSourceVal.ToLower() == IdeaSource.ToLower()).ToList();
            //}
            var item = applicationList.OrderBy(x => x.Team).Select(p => new SelectListItem { Value = p.Team.ToString(), Text = p.Team.ToString(), Selected = true });
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ComplexityList()
        {
            // var ClientID = (Client)Session["Client"];
            var ClientID = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.IMComplexity.Get(x => x.ClientId == ClientID.ClientId).ToList();
            if (applicationList.Count == 0)
            {
                var cmp = new IMComplexity();
                cmp.ClientId = ClientID.ClientId;
                cmp.Complexity = "NA";
                cmp.CreatedDt = DateTime.Now;
                cmp.CreatedBy = uid;
                unitOfWork.IMComplexity.Insert(cmp);
                unitOfWork.Save();
            }
            var applicationList2 = unitOfWork.IMComplexity.Get(x => x.ClientId == ClientID.ClientId).ToList();
            var item = applicationList2.OrderBy(x => x.Complexity).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = char.ToUpper(p.Complexity[0]) + p.Complexity.Substring(1), Selected = true });
            return Json(item, JsonRequestBehavior.AllowGet); ;
        }
        public ActionResult SystemImpactedList()
        {
            // var ClientID = (Client)Session["Client"];
            var ClientID = (ClientDetail)Session["SessionClient"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.IMSystemImpactedService.Get(x => x.ClientId == ClientID.ClientId).ToList();
            var item = applicationList.OrderBy(x => x.SystemImpactedService).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = char.ToUpper(p.SystemImpactedService[0]) + p.SystemImpactedService.Substring(1), Selected = true });
            return Json(item, JsonRequestBehavior.AllowGet); ;
        }
        public ActionResult LobList()
        {
            // var ClientID = (Client)Session["Client"];
            var ClientID = (ClientDetail)Session["SessionClient"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.IMLOB.Get(x => x.ClientId == ClientID.ClientId).ToList();
            var item = applicationList.OrderBy(x => x.LOB).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.LOB.ToString(), Selected = true });
            return Json(item, JsonRequestBehavior.AllowGet); ;
        }
        public IEnumerable<SelectListItem> GetIdeaSource(int clientid)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            //var TempList = new List<SelectListItem>();
            var TempList = unitOfWork.IMIdeaSourceMaster.Get().Where(s => s.ClientId == clientid || s.ClientId == 0).ToList();
            var item = TempList.Select(p => new SelectListItem { Value = p.IdeaSource.ToString(), Text = p.IdeaSource, Selected = true });
            return item;
        }
        public ActionResult IdeaSourceList()
        {
            var ClientID = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var TempList = new List<SelectListItem>();
            var TempList1 = unitOfWork.IMIdeaSourceMaster.Get().Where(s => s.ClientId == ClientID.ClientId || s.ClientId == 0).ToList();
            TempList = TempList1.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.IdeaSource.ToString(),
                    Value = a.IdeaSource.ToString(),
                    Selected = false
                };
            });

            //if (IsNTTResource)
            //{
            //    SelectListItem cli = new SelectListItem()
            //    {
            //        Text = ClientID.ClientName,
            //        Value = ClientID.ClientName,
            //        Selected = false
            //    };
            //    TempList.Add(cli);
            //    SelectListItem ntt = new SelectListItem()
            //    {
            //        Text = "NTT Data",
            //        Value = "NTT Data",
            //        Selected = true
            //    };
            //    TempList.Add(ntt);
            //}
            //else
            //{
            //    SelectListItem cli = new SelectListItem()
            //    {
            //        Text = ClientID.ClientName,
            //        Value = ClientID.ClientName,
            //        Selected = true
            //    };
            //    TempList.Add(cli);
            //    SelectListItem ntt = new SelectListItem()
            //    {
            //        Text = "NTT Data",
            //        Value = "NTT Data",
            //        Selected = false
            //    };
            //    TempList.Add(ntt);
            //}
            //SelectListItem ntt1 = new SelectListItem()
            //{
            //    Text = "NTT DATA_CESIVV",
            //    Value = "NTT DATA_CESIVV",
            //    Selected = false
            //};
            //TempList.Add(ntt1);
            //SelectListItem ntt2 = new SelectListItem()
            //{
            //    Text = "NTT DATA_CESA3MM",
            //    Value = "NTT DATA_CESA3MM",
            //    Selected = false
            //};
            //TempList.Add(ntt2);

            var item = TempList.AsEnumerable();
            return Json(item, JsonRequestBehavior.AllowGet); ;
        }
        public ActionResult IdeaTypeList()
        {
            // var ClientID = (Client)Session["Client"];
            var ClientID = (ClientDetail)Session["SessionClient"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.IMIdeaType.Get(x => x.ClientId == ClientID.ClientId).ToList();
            var item = applicationList.OrderBy(x => x.IdeaType).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = char.ToUpper(p.IdeaType[0]) + p.IdeaType.Substring(1), Selected = true });
            return Json(item, JsonRequestBehavior.AllowGet); ;
        }
        public ActionResult ListOfAdmins()
        {
            // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            Db db = new Db();
            var listOfAdmins = (from UA in db.UserApplications
                                join UP in db.UserProfiles on UA.UserId equals UP.Id
                                join ASA in db.AppStore on UA.ApplicationId equals ASA.Id
                                join UR in db.UserRoles on UA.RoleId equals UR.Id
                                where ASA.ApplicationName.ToLower() == "idea manager" && (UR.RoleName == "admin" || UR.RoleName == "sysadmin" || UR.RoleName == "implementationmanager") && UA.ClientId == client.ClientId
                                select UP
                                            ).Distinct().OrderBy(x => x.FirstName).ToList();
            var AdminsWithName = listOfAdmins.Select(g =>
                new SelectListItem
                {
                    Text = g.FirstName + " " + g.LastName,
                    Value = g.Id.ToString()
                }
                );
            //return Json(new { Result = "OK", Records = AdminsWithName });
            return Json(AdminsWithName, JsonRequestBehavior.AllowGet);
        }


        public IEnumerable<SelectListItem> GetListOfAdmin(int clientid)
        {
            var AdminsWithName = IMHelper.GetimpManagers(clientid);
            return AdminsWithName;
        }
        public IEnumerable<SelectListItem> GetAllIMUser(int clientid)
        {
            var AdminsWithName = IMHelper.GetAllIMUser(clientid);
            return AdminsWithName;
        }

        public ActionResult ListOfAdminsforApprover([DataSourceRequest] DataSourceRequest request, int ideaId)
        {
            if (ideaId != 0)
            {
                //Client client = (Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                Db db = new Db();
                var TEMPlistOfAdmins = (from UA in db.UserApplications
                                        join UP in db.UserProfiles on UA.UserId equals UP.Id
                                        join ASA in db.AppStore on UA.ApplicationId equals ASA.Id
                                        join UR in db.UserRoles on UA.RoleId equals UR.Id
                                        join APPRS in db.IMIdeaApprover.Where(w => w.IdeaId == ideaId) on UP.Id equals APPRS.ApproverId into tmpApprs
                                        from aprs in tmpApprs.DefaultIfEmpty()
                                        where ASA.ApplicationName.ToLower() == "idea manager" && (UR.RoleName == "admin" || UR.RoleName == "sysadmin") && UA.ClientId == client.ClientId
                                        select new ApproverModel
                                        {
                                            Id = UP.Id,
                                            FirstName = UP.FirstName,
                                            LastName = UP.LastName,
                                            IdeaId = (int)ideaId,
                                            IdeaApproverId = aprs.Id,
                                            ApprovedDate = aprs.ApprovedDate
                                        }
                                   ).Distinct().OrderBy(x => x.FirstName).ToList();

                //Adding Status infront it ...
                //Logic is => IF (__ApprovedDt__ is NULL) then Admin has not Approved(might have cancelled/Hold/Decline)
                //Else  (i.e, __ApprovedDt__ is NOT NULl) Admin has Approved the IDEA
                //Also checking IdeaApproverId property in above Object -> to check whether they are selected as aprrover or not
                var listOfAdmins = TEMPlistOfAdmins.Select(f =>
                        new ApproverModel
                        {
                            Id = f.Id,
                            FirstName = f.FirstName,
                            LastName = f.LastName,
                            IdeaId = f.IdeaId,
                            IdeaApproverId = f.IdeaApproverId,
                            ApprovedDate = f.ApprovedDate,
                            ApprovedOrNot = (f.ApprovedDate != null || f.ApprovedDate > DateTime.MinValue) ? "Approved" : (f.IdeaApproverId == null ? "" : "Not Approved")

                        }
                );

                return Json(listOfAdmins.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }



        //--------------------------------------------------Classification and Category -------------------------------------------------------------//



        //Saving---
        [ActionTrack]
        public ActionResult SaveClassificationCategory(int IdeaId, string CatIds = "")
        {
            try
            {
                int uid = (int)Session["UserId"];
                if (IdeaId > 0)
                {
                    //Delete Existing Categories
                    try
                    {
                        IMHelper.DeleteClassificationCategory(IdeaId);
                        if (CatIds.Trim() != "")
                        {
                            IMHelper.AddClassificationCategory(IdeaId, CatIds, uid);
                        }
                        return Json(true);
                    }
                    catch (Exception)
                    {
                        return Json(new { error = "Error while Updating" });
                    }
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: SaveClassificationCategory ", ex);
                throw ex;
            }
        }
        //ConfigItme
        [ActionTrack]
        public ActionResult SaveConfigItems(int IdeaId, string ConfigIds = "")
        {
            try
            {
                int uid = (int)Session["UserId"];
                if (IdeaId > 0)
                {
                    //Delete Existing Categories
                    try
                    {
                        IMHelper.DeleteConfigItem(IdeaId);
                        if (ConfigIds.Trim() != "")
                        {
                            IMHelper.AddConfigItem(IdeaId, ConfigIds, uid);
                        }
                        return Json(true);
                    }
                    catch (Exception)
                    {
                        return Json(new { error = "Error while Updating" });
                    }
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: SaveConfigItems ", ex);
                throw ex;
            }
        }


        //---------------------------------------------------------------- Email Notification --------------------------------------------------------------------//

        [ActionTrack]
        public void SendIdeaNotification(string email, string fullname, string IdeaName, IMStatus status, string clientname, string IdeaLink)
        {
            try
            {
                var body = "";
                var mailId = email;
                var Subject = "Optimization Tool IM AutoNotification";
                if (status == IMStatus.SubmitForReview || status == IMStatus.SubmitForApproval || status == IMStatus.SubmitForPlanning || status == IMStatus.SubmitForRevision)
                {
                    string StatusString = "";
                    //fulllname = Admin Name(Approver)
                    if (status == IMStatus.SubmitForReview)
                    {
                        StatusString = "Submitted For Review";
                        body = SubmitForReviewBody(fullname, IdeaName, StatusString, clientname, IdeaLink);
                    }
                    if (status == IMStatus.SubmitForRevision)
                    {
                        StatusString = "Submitted For Revision";
                        body = SubmitForReviewBody(fullname, IdeaName, StatusString, clientname, IdeaLink);//---------------Edit Required
                    }
                    if (status == IMStatus.SubmitForApproval)
                    {
                        StatusString = "Submitted For Approval";
                        body = SubmittedForApprovalBody(fullname, IdeaName, StatusString, clientname, IdeaLink);
                    }
                    if (status == IMStatus.SubmitForPlanning)
                    {
                        StatusString = "Submitted For Planning";
                        body = ImplementationManagerbody(fullname, IdeaName, clientname, IdeaLink);
                    }

                }
                else
                {
                    string StatusString = "";
                    //Hold
                    if (status == IMStatus.Hold)
                    {
                        StatusString = "put On Hold";
                    }
                    //Cancelled Idea
                    else if (status == IMStatus.Cancelled)
                    {
                        StatusString = "Cancelled";
                    }
                    else//Cancelled Idea
                    if (status == IMStatus.Created)
                    {
                        StatusString = "Created";
                    }
                    body = HoldOrCancelStatusBody(fullname, IdeaName, StatusString, clientname, IdeaLink);
                }

                App_Start.Helper.SendEMail(mailId, Subject, body);
            }
            catch (Exception ex)
            {
                Log.Error("Error in PostIdeaController: New_KeywordReportList " + ex);
            }
        }

        private string SubmittedForApprovalBody(string adminName, string Idea, string status, string clientname, string IdeaLink)
        {
            int uid = (int)Session["UserId"];
            var username = "";
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var som = unitOfWork.UserProfile.GetByID(uid);
                username = som.FirstName + som.LastName;
            }

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Areas/IdeaManager/Templates/SubmittedForApprovalEmailNotification.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{username}", username);
            body = body.Replace("{ideaname}", Idea);
            body = body.Replace("{idealink}", IdeaLink);
            body = body.Replace("{clientname}", clientname);
            body = body.Replace("{approver}", adminName);

            return body;
        }

        private string SubmitForReviewBody(string ReviewerName, string Idea, string status, string clientname, string IdeaLink)
        {
            int uid = (int)Session["UserId"];
            var username = "";
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var som = unitOfWork.UserProfile.GetByID(uid);
                username = som.FirstName + som.LastName;
            }

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Areas/IdeaManager/Templates/SubmitForReviewEmailNotification.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{username}", username);
            body = body.Replace("{idealink}", IdeaLink);
            body = body.Replace("{ideaname}", Idea);
            body = body.Replace("{clientname}", clientname);
            body = body.Replace("{approver}", ReviewerName);

            return body;
        }

        private string ImplementationManagerbody(string adminName, string Idea, string clientname, string IdeaLink)
        {
            int uid = (int)Session["UserId"];
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Areas/IdeaManager/Templates/ImplementationManagerEmailNotification.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{idealink}", IdeaLink);
            body = body.Replace("{ideaname}", Idea);
            body = body.Replace("{clientname}", clientname);
            body = body.Replace("{adminName}", adminName);

            return body;
        }


        private string HoldOrCancelStatusBody(string username, string Idea, string status, string clientname, string IdeaLink)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Areas/IdeaManager/Templates/HoldOrCancelEmailNotification.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{username}", username);
            body = body.Replace("{ideaname}", Idea);
            body = body.Replace("{status}", status);
            body = body.Replace("{clientname}", clientname);
            body = body.Replace("{idealink}", IdeaLink);

            return body;
        }

        public string getFullName(int userId)
        {
            Db db = new Db();
            var fullName = (from b in db.UserProfiles
                            where b.Id == userId
                            select b.FirstName + " " + b.LastName).FirstOrDefault();

            return fullName;
        }

        [HttpPost]
        [ActionTrack]
        public ActionResult Save_Reference(IEnumerable<HttpPostedFileBase> RefFiles,string apprid)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            byte[] filecnt = null;
            var filename = "";
            if (RefFiles != null)
            {
                Stream filestream = RefFiles.First().InputStream;
                filename = RefFiles.First().FileName;
                BinaryReader br = new BinaryReader(filestream);
                Byte[] bytes = br.ReadBytes((Int32)filestream.Length);
                filecnt = bytes;
            }

            int fileId = 0;
            using (var contextFactory = new DbContextFactory())
            {

                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                ApprovalAttachments rspAttach = new ApprovalAttachments();
                rspAttach.FileCnt = filecnt;
                rspAttach.ClientId = client.ClientId;
                rspAttach.FileName = filename;
                rspAttach.OffapprovalId = Convert.ToInt32(apprid);
                unitOfWork.ApprovalAttachments.Insert(rspAttach);

                unitOfWork.Save();
                fileId = rspAttach.Id;
            }

            return Content(fileId.ToString());
        }
        [ActionTrack]
        public FileResult DownloaduploadedfileAction(int fileId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var fileUpload = unitOfWork.ApprovalAttachments.GetByID(fileId);


                byte[] fileContent = null;
                if (fileUpload.AttachFile != null)
                {
                    var str = fileUpload.AttachFile.Substring(fileUpload.AttachFile.LastIndexOf(',') + 1);
                    fileContent = Convert.FromBase64String(str);
                }
                else
                    fileContent = fileUpload.FileCnt;
                return File(fileContent, System.Net.Mime.MediaTypeNames.Application.Octet, fileUpload.FileName);
            }
        }

        [HttpPost]
        [ActionTrack]
        public ActionResult RemoveFile(int fileId)
        {
            using (var contextFactory = new DbContextFactory())
            {

                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.ApprovalAttachments.Delete(fileId);
                unitOfWork.Save();
            }
            return Json(fileId, JsonRequestBehavior.AllowGet);
        }



    }
}
