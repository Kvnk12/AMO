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
using WebUI.Areas.ClientProfileManager;
using WebUI.Areas.DemandManager.Dto;
using WebUI.Areas.DemandManager.Models;
using WebUI.Dto;

namespace WebUI.Areas.DemandManager.Controllers
{
    public class PostDemandController : Controller
    {
        //
        // GET: /DemandManager/PostDemand/

        public ActionResult Index(bool ViewDemand = true, int DemandId = 0, string tab = "")
        {
            //var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            //Authorize Demand
            var DemandAuth = DMHelper.DemandAuthorize(client.ClientId, DemandId);

            var user = GetDropDownListItems.UserRoleForClient(uid, client.ClientId);
            var adminList = DMHelper.ListOfAdminsSysAdmins(client.ClientId);
            var IsNttDataResource = IsNttResource(uid);
           
            if (DemandId == 0)
            {
                DemandViewModel cv = new DemandViewModel();
                cv.PostNewDemand = true;
                cv.ViewDemand = false;
                cv.InternalInit = false;
                cv.IsDeleted = true;
                if (IsNttDataResource)
                {
                    cv.DemandSourceId = "NTT Data";
                }
                else
                {
                    cv.DemandSourceId = client.ClientName;
                }
                ViewBag.ClassAndCat = DMHelper.GetClassCat(client.ClientId, DemandId);
                ViewBag.ConfigItems = DMHelper.GetConfigType(client.ClientId, DemandId);
                return View(cv);
            }
            else
            {
                if (DemandAuth)
                {
                    ViewBag.ClassAndCat = DMHelper.GetClassCat(client.ClientId, DemandId);
                    ViewBag.ConfigItems = DMHelper.GetConfigType(client.ClientId, DemandId);
                    if (ViewDemand)
                    {
                        var Details = DMHelper.getDemandDetails(DemandId);
                        Details.PostNewDemand = false;
                        var temp = DMHelper.getApprovers(DemandId);
                        Details.ExistingApproversIds = temp != null ? temp : (new string[0]);


                        if (user.Text.ToLower() == "admin" || user.Text.ToLower() == "sysadmin")
                            Details.IsSysAdmin = true;
                        if (user.Text.ToLower() == "implementationmanager")
                            Details.IsImplementationManager = true;
                        if (Details.CreatedBy == uid)
                            Details.IsDemandMine = true;

                        return View(Details);
                    }
                    else
                    {
                        var Details =DMHelper.getDemandDetails(DemandId);
                        //For Demand Approvers
                        var temp = DMHelper.getApprovers(DemandId);
                        Details.ExistingApproversIds = temp != null ? temp : (new string[0]);

                        if (user.Text.ToLower() == "admin" || user.Text.ToLower() == "sysadmin")
                            Details.IsSysAdmin = true;
                        if (user.Text.ToLower() == "implementationmanager")
                            Details.IsImplementationManager = true;
                        if (Details.CreatedBy == uid)
                            Details.IsDemandMine = true;


                        Details.TabName = tab;

                        return View(Details);
                    }
                }
                else
                {
                    return Content("<h3>Sorry! Demand Not Found.</h3>");
                }
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
        /// Check whether Demand Name already exists.
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
            var existItemNames = unitOfWork.DMPostDemand.Get().Where(e => e.ItemName.Trim() == ItemName.Trim() && e.ClientId == client.ClientId).ToList();

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

            return Json(resultFound, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Save basic(Home Tab) Demand Details.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveBasicDemandDetails(DemandViewModel IVM, string btnSubmit)
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
            if (IVM.DemandId == 0)
            {
                var id = 0;
                var tempId = DMHelper.saveDemand(IVM, uid);
                id = tempId.DemandId;
                if(id!=0&& btnSubmit== "Review")
                {
                    SubmitDemandForReview(id);
                }
                return RedirectToAction("Index", new { ViewDemand = false, DemandId = tempId.DemandId });
            }
            else
            {
                int id = 0;
                if ((IVM.DemandStatusID != (int)DMStatus.SubmitForReview &&
                        IVM.DemandStatusID != (int)DMStatus.SubmitForApproval &&
                        IVM.DemandStatusID != (int)DMStatus.Declined &&
                        IVM.DemandStatusID != (int)DMStatus.Cancelled &&
                        IVM.DemandStatusID != (int)DMStatus.Implemented) || MenuItemsInput.UserRole.ToLower() == "sysadmin")
                {
                    var tempId = DMHelper.saveDemand(IVM, uid);
                    id = tempId.DemandId;
                    if (id != 0 && btnSubmit == "Review")
                    {
                        SubmitDemandForReview(id);
                    }
                }
                return RedirectToAction("Index", new { ViewDemand = false, DemandId = id });
            }
        }
      
        public void SubmitDemandForReview(int DemandId)
        {
           
            var uid = (int)Session["UserId"];
            var client = (ClientDetail)Session["SessionClient"];
            var Appuser = GetDropDownListItems.UserRoleForClient(uid, client.ClientId);
            Db db = new Db();
            var TEMPlistOfAdmins = (from UA in db.UserApplications
                                    join UP in db.UserProfiles on UA.UserId equals UP.Id
                                    join ASA in db.AppStore on UA.ApplicationId equals ASA.Id
                                    join UR in db.UserRoles on UA.RoleId equals UR.Id
                                    where ASA.ApplicationName.ToLower() == "demand manager" && (UR.RoleName == "admin") && UA.ClientId == client.ClientId
                                    select new
                                    {
                                        Id=UP.Id,
                                        FirstName = UP.FirstName,
                                        LastName = UP.LastName,
                                        email = UP.Email
                                    }
                                  ).Distinct().OrderBy(x => x.FirstName).ToList();
           
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var Demand = unitOfWork.DMPostDemand.GetByID(DemandId);
                //Getting Existing Approvers
                var ExistingApprovers = unitOfWork.DMDemandApprover.Get().Where(e => e.DemandId == DemandId).Select(e => e.ApproverId).ToList();

                if (Demand.ImpStatusId == null && (Demand.StatusId == (int)DMStatus.Created || Demand.StatusId == (int)DMStatus.Revised || Demand.StatusId == (int)DMStatus.SubmitForRevision))
                {
                    var OldStatus = Demand.StatusId;
                    Demand.StatusId = (int)DMStatus.SubmitForReview;
                    Demand.StatusDt = DateTime.Now;
                    Demand.LastUpdatedDate = DateTime.Now;
                    Demand.UpdatedBy = uid;
                    unitOfWork.DMPostDemand.Update(Demand);
                    
                    DMStatusHistory cv = new DMStatusHistory();
                    cv.Comments = "";
                    cv.DemandId = DemandId;
                    cv.OldStatusId = OldStatus;
                    cv.NewStatusId = Demand.StatusId;
                    cv.CreatedBy = uid;
                    cv.UpdatedBy = uid;
                    cv.ClientId = client.ClientId;
                    cv.CreatedDate = DateTime.Now;
                    cv.LastUpdatedDate = DateTime.Now;
                    unitOfWork.DMStatusHistory.Insert(cv);
                    var deleteApprovers = from details in db.DMDemandApprover
                                          where details.DemandId == DemandId
                                          select details;

                    foreach (var detail in deleteApprovers)
                    {
                        unitOfWork.DMDemandApprover.Delete(detail.Id);
                    }
                    foreach (var item in TEMPlistOfAdmins)
                    {
                            var ApproverId = Convert.ToInt32(item.Id);
                            DMDemandApprover DemandApprover = new DMDemandApprover();
                            DemandApprover.DemandId = Convert.ToInt32(DemandId);
                            DemandApprover.ApproverId = ApproverId;
                            DemandApprover.Createdby = uid;
                            DemandApprover.ClientId = client.ClientId;
                            DemandApprover.CreatedDt = DateTime.Now;
                            unitOfWork.DMDemandApprover.Insert(DemandApprover);

                        var DemandLink = Url.Action("Index", "PostDemand", new { ViewDemand = false, DemandId = Demand.Id }, "http");
                            createDemandemail(Demand.ItemName, client.ClientName, uid, item.email, item.FirstName + " " + item.LastName, DemandLink);
                 
                    }
                }
                else if (Appuser.Text.ToLower() == "sysadmin")
                {
                    var OldStatus = Demand.StatusId;
                    var deleteApprovers = from details in db.DMDemandApprover
                                          where details.DemandId == DemandId
                                          select details;

                    foreach (var detail in deleteApprovers)
                    {
                        unitOfWork.DMDemandApprover.Delete(detail.Id);

                    }
                    foreach (var item in TEMPlistOfAdmins)
                    {
                        
                            var ApproverId = Convert.ToInt32(item);
                            DMDemandApprover DemandApprover = new DMDemandApprover();
                            DemandApprover.DemandId = Convert.ToInt32(DemandId);
                            DemandApprover.ApproverId = ApproverId;
                            DemandApprover.Createdby = uid;
                            DemandApprover.ClientId = client.ClientId;
                            DemandApprover.CreatedDt = DateTime.Now;
                            unitOfWork.DMDemandApprover.Insert(DemandApprover);

                        var DemandLink = Url.Action("Index", "PostDemand", new { ViewDemand = false, DemandId = Demand.Id }, "http");
                        createDemandemail(Demand.ItemName, client.ClientName, uid, item.email, item.FirstName + " " + item.LastName, DemandLink);


                    }

                }
                unitOfWork.Save();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveDemandIMPDetails(DemandViewModel IVM)
        {
            // var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            if (IVM.DemandId != 0 && IVM.ImpDemandStatusID != 0)
            {
                var DemandLink = Url.Action("Index", "PostDemand", new { ViewDemand = false, DemandId = IVM.DemandId }, "http");
                IVM.StatusComment = IVM.StatusComment == null ? null : Server.HtmlDecode(IVM.StatusComment.Trim());
                IVM.ClientId = client.ClientId;
               DMHelper.saveImpDetails(IVM, uid, client.ClientName, DemandLink);
            }
         return RedirectToAction("Index", new { ViewDemand = false, DemandId = IVM.DemandId, tab = "Implementation" });
        }

        [HttpPost]
        public ActionResult ReviseDemand(int DemandId = 0)
        {
            var uid = (int)Session["UserId"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            if (DemandId != 0)
            {
                DMHelper.Demand_Revised(DemandId,client.ClientId, uid);
            }
            return Json(new { Success = "Success" });
        }

        //-----------------------------------------------------------------------------------------------------------------------------//
        //-------------------------------------------------STATUS CHANGE Controller Actions--------------------------------------------//
        //-----------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Submit for Review -- [from CreatedBy Users]
        /// </summary>
        /// <param name="DemandIDs"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitforReview(string DemandIDs = "")
        {
            //  var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            if (DemandIDs.Trim() != "")
            {
                var DemandLink = Url.Action("Index", "PostDemand", new { ViewDemand = false }, "http");
                submitforreview(DemandIDs,client.ClientId, client.ClientName, uid, DemandLink);
                return Json(new { Success = "Success" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Failure = "Select any Demands" }, JsonRequestBehavior.AllowGet);
            }
        }
        public void createDemandemail(string DemandName,string clientName,int uid,string adminEmail, string adminName, string DemandLink)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var up = unitOfWork.UserProfile.GetByID(uid);
            var body = "";
            var mailId = adminEmail;
            var Subject = "Autogenerated Notification: Demand Creation";
            body = CreateDemandNotificationBody(DemandName, clientName, up.FirstName+" "+up.LastName, adminName, DemandLink);

            App_Start.Helper.SendEMail(mailId, Subject, body);

        }
        public void submitforreview(string DemandIDs, int clientId,string ClientName, int uid, string DemandLink)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

            string[] SelectedDemandIds = DemandIDs.Split(',');
            foreach (var i in SelectedDemandIds)
            {
                var _DemandId = Convert.ToInt32(i);
                // -->  Removed: Demand with created Status without Approver cannot be Submitted for Review
                var ExistDemand = unitOfWork.DMPostDemand.GetByID(_DemandId);
                var checkStatus = unitOfWork.DMPostDemand.Get().Where(e => e.Id == _DemandId && (e.StatusId == (int)DMStatus.Revised || e.StatusId == (int)DMStatus.Created)).Any();
                var checkApproverExists = unitOfWork.DMDemandApprover.Get(e => e.DemandId == _DemandId).Any();
                if (checkStatus && checkApproverExists)
                {
                    var Demand = unitOfWork.DMPostDemand.GetByID(_DemandId);
                    var oldStatus = Demand.StatusId;
                    Demand.StatusId = (int)DMStatus.SubmitForReview;
                    Demand.StatusDt = DateTime.Now;
                    unitOfWork.DMPostDemand.Update(Demand);

                    DMStatusHistory statusHistory = new DMStatusHistory();
                    statusHistory.DemandId = Demand.Id;
                    statusHistory.LastUpdatedDate = DateTime.Now;
                    statusHistory.UpdatedBy = uid;
                    statusHistory.ClientId = clientId;
                    statusHistory.OldStatusId = oldStatus;
                    statusHistory.NewStatusId = Demand.StatusId;
                    unitOfWork.DMStatusHistory.Insert(statusHistory);

                    unitOfWork.Save();
                    var Approvers = unitOfWork.DMDemandApprover.Get().Where(e => e.DemandId == _DemandId);
                    if (Approvers.Count() != 0)
                    {
                        DemandLink = DemandLink.Trim() + "&DemandId=" + _DemandId.ToString();
                        foreach (var appr in Approvers)
                        {
                            var up = unitOfWork.UserProfile.GetByID(appr.ApproverId);
                            if (up != null)
                            {
                                SendDemandNotification(up.Email, up.FirstName + " " + up.LastName, ExistDemand.ItemName, DMStatus.SubmitForReview, ClientName, DemandLink);
                            }
                        }
                    }
                }
            }
        }

        [HttpPost]
        public ActionResult DeleteDemands(string DemandIDs = "")
        {
            //var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            if (DemandIDs.Trim() != "")
            {
                DMHelper.deleteDemands(DemandIDs, client.ClientId, uid);
                return Json(new { Success = "Success" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Failure = "Select any Demands" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Submit for Approval -- [By Admins and SysAdmins]
        /// </summary>
        /// <param name="DemandIDs"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitforApproval(string DemandIDs = "")
        {
            // var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            if (DemandIDs.Trim() != "")
            {
                return RedirectToAction("Index", "DemandApproval", new { page = 1 });
            }
            else
            {
                return Json(new { Failure = "Select any Demands" }, JsonRequestBehavior.AllowGet);
            }
        }

        //-----------------//
        //Check metric for Demand > Implementation Status Change 
        public ActionResult CheckMetric(int DemandId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var IsThere = unitOfWork.DMMetricTarget.Get(f => f.ItemId == DemandId).Any();
            return Json(IsThere, JsonRequestBehavior.AllowGet);
        }

        //-----------------//
        //Check Comment for Demand > Implementation Status Change 
        public ActionResult CheckComment(int DemandId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var IsThere = unitOfWork.DMItemComments.Get(f => f.ItemId == DemandId).Any();
            return Json(IsThere, JsonRequestBehavior.AllowGet);
        }


        //-----------------------------------------------------------------------------------------------------------------------------//
        //-------------------------------------------------TABS Controller Actions---------------------------------------------------//
        //-----------------------------------------------------------------------------------------------------------------------------//
       
        /*------------------------------COMMENTS-----------------------------------*/

        [HttpPost]
        public ActionResult GetComments([DataSourceRequest] DataSourceRequest request, int DemandId = 0)
        {
            int usid = (int)Session["UserId"];
            return Json(DMHelper.GetCommentsDetails(DemandId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddComments([DataSourceRequest] DataSourceRequest request, DMItemCommentsInput dmItemCommentsInput, int DemandId = 0)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            if (dmItemCommentsInput != null && dmItemCommentsInput.Comments.Trim() != "" && DemandId != 0 && ModelState.IsValid)
            {
                int usid = (int)Session["UserId"];
                dmItemCommentsInput.CreatedBy = usid;
                var res = DMHelper.CreateComments(dmItemCommentsInput,client.ClientId, DemandId);
                dmItemCommentsInput.CreatedByName = DMHelper.GetUserName(usid);
                dmItemCommentsInput.CreatedDate = DateTime.Now;
                dmItemCommentsInput.Id = res;
            }
            return Json(new[] { dmItemCommentsInput }.ToDataSourceResult(request, ModelState));

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditComments([DataSourceRequest] DataSourceRequest request, DMItemCommentsInput dmItemCommentsInput)
        {
            if (dmItemCommentsInput != null && dmItemCommentsInput.Comments.Trim() != "" && ModelState.IsValid)
            {
                int usid = (int)Session["UserId"];
                dmItemCommentsInput.UpdatedBy = usid;
                var res = DMHelper.UpdateComments(dmItemCommentsInput);
                dmItemCommentsInput.CreatedByName = DMHelper.GetUserName(usid);
                dmItemCommentsInput.CreatedDate = DateTime.Now;
            }
            return Json(new[] { dmItemCommentsInput }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteComments([DataSourceRequest] DataSourceRequest request, DMItemCommentsInput dmItemCommentsInput)
        {
            if (dmItemCommentsInput != null)
            {
                var v = DMHelper.RemoveCommentss(dmItemCommentsInput.Id);
            }
            return Json(new[] { dmItemCommentsInput }.ToDataSourceResult(request, ModelState));
        }


        //------------------------------------Analysis, Solution-----------------------------//
        [HttpPost]
        public ActionResult UpdateAnalysis(int DemandId = 0, string analysis = "")
        {
            int uid = (int)Session["UserId"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var EncodeString = Server.HtmlDecode(analysis);
            if (DemandId != 0)
            {
                DMHelper.saveAnalysis(DemandId,client.ClientId, uid, EncodeString);
            }
            return Json(new { EncodeString }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateSolution(int DemandId = 0, string solution = "")
        {
            int uid = (int)Session["UserId"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var EncodeString = Server.HtmlDecode(solution);
            if (DemandId != 0)
            {
                DMHelper.saveSolution(DemandId,client.ClientId, uid, EncodeString);
            }
            return Json(new { EncodeString }, JsonRequestBehavior.AllowGet);
        }

        // ---------------------------------------------------- Approvers(submit) ------------------------------------------------------------ //
        [HttpPost]
        public ActionResult AddSelectedAdmins(int DemandId = 0, string ApproverIds = "")
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
                if (DemandId != 0)
                {
                    string[] SelectedAdminIds = ApproverIds.Split(',');
                    using (var contextFactory = new DbContextFactory())
                    {
                        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                        var Demand = unitOfWork.DMPostDemand.GetByID(DemandId);
                        //Getting Existing Approvers
                        var ExistingApprovers = unitOfWork.DMDemandApprover.Get().Where(e => e.DemandId == DemandId).Select(e => e.ApproverId).ToList();

                        if (Demand.ImpStatusId == null && (Demand.StatusId == (int)DMStatus.Created || Demand.StatusId == (int)DMStatus.Revised || Demand.StatusId == (int)DMStatus.SubmitForRevision))
                        {
                            var OldStatus = Demand.StatusId;
                            Demand.StatusId = (int)DMStatus.SubmitForReview;
                            Demand.StatusDt = DateTime.Now;
                            Demand.LastUpdatedDate = DateTime.Now;
                            Demand.UpdatedBy = uid;
                            unitOfWork.DMPostDemand.Update(Demand);

                            DMStatusHistory cv = new DMStatusHistory();
                            cv.Comments = "";
                            cv.DemandId = DemandId;
                            cv.OldStatusId = OldStatus;
                            cv.NewStatusId = Demand.StatusId;
                            cv.CreatedBy = uid;
                            cv.UpdatedBy = uid;
                            cv.ClientId = client.ClientId;
                            cv.CreatedDate = DateTime.Now;
                            cv.LastUpdatedDate = DateTime.Now;
                            unitOfWork.DMStatusHistory.Insert(cv);
                            var deleteApprovers = from details in db.DMDemandApprover
                                                  where details.DemandId == DemandId
                                                  select details;

                            foreach (var detail in deleteApprovers)
                            {
                                unitOfWork.DMDemandApprover.Delete(detail.Id);

                            }
                            foreach (var item in SelectedAdminIds)
                            {
                                if (item != " " || item != "," || item != "")
                                {
                                    var ApproverId = Convert.ToInt32(item);
                                    DMDemandApprover DemandApprover = new DMDemandApprover();
                                    DemandApprover.DemandId = Convert.ToInt32(DemandId);
                                    DemandApprover.ApproverId = ApproverId;
                                    DemandApprover.Createdby = uid;
                                    DemandApprover.ClientId = client.ClientId;
                                    DemandApprover.CreatedDt = DateTime.Now;
                                    unitOfWork.DMDemandApprover.Insert(DemandApprover);

                                    if (OldStatus == (int)DMStatus.Revised)
                                    {
                                        var u = unitOfWork.UserProfile.Get(g => g.Id == DemandApprover.ApproverId).FirstOrDefault();
                                        var DemandLink = Url.Action("Index", "PostDemand", new { ViewDemand = false, DemandId = Demand.Id }, "http");
                                        SendDemandNotification(u.Email, u.FirstName + " " + u.LastName, Demand.ItemName, DMStatus.SubmitForReview, client.ClientName, DemandLink);
                                    }
                                    else
                                    {
                                        var u = unitOfWork.UserProfile.Get(g => g.Id == DemandApprover.ApproverId).FirstOrDefault();
                                        var DemandLink = Url.Action("Index", "PostDemand", new { ViewDemand = false, DemandId = Demand.Id }, "http");
                                        SendDemandNotification(u.Email, u.FirstName + " " + u.LastName, Demand.ItemName, DMStatus.SubmitForReview, client.ClientName, DemandLink);
                                    }

                                }
                            }

                        }
                        else if (Appuser.Text.ToLower() == "sysadmin")
                        {
                            var OldStatus = Demand.StatusId;
                            var deleteApprovers = from details in db.DMDemandApprover
                                                  where details.DemandId == DemandId
                                                  select details;

                            foreach (var detail in deleteApprovers)
                            {
                                unitOfWork.DMDemandApprover.Delete(detail.Id);

                            }
                            foreach (var item in SelectedAdminIds)
                            {
                                if (item != " " || item != "," || item != "")
                                {
                                    var ApproverId = Convert.ToInt32(item);
                                    DMDemandApprover DemandApprover = new DMDemandApprover();
                                    DemandApprover.DemandId = Convert.ToInt32(DemandId);
                                    DemandApprover.ApproverId = ApproverId;
                                    DemandApprover.Createdby = uid;
                                    DemandApprover.ClientId = client.ClientId;
                                    DemandApprover.CreatedDt = DateTime.Now;
                                    unitOfWork.DMDemandApprover.Insert(DemandApprover);

                                    if (OldStatus == (int)DMStatus.Revised)
                                    {
                                        var u = unitOfWork.UserProfile.Get(g => g.Id == DemandApprover.ApproverId).FirstOrDefault();
                                        var DemandLink = Url.Action("Index", "PostDemand", new { ViewDemand = false, DemandId = Demand.Id }, "http");
                                        SendDemandNotification(u.Email, u.FirstName + " " + u.LastName, Demand.ItemName, DMStatus.SubmitForReview, client.ClientName, DemandLink);
                                    }
                                    else
                                    {
                                        var u = unitOfWork.UserProfile.Get(g => g.Id == DemandApprover.ApproverId).FirstOrDefault();
                                        var DemandLink = Url.Action("Index", "PostDemand", new { ViewDemand = false, DemandId = Demand.Id }, "http");
                                        SendDemandNotification(u.Email, u.FirstName + " " + u.LastName, Demand.ItemName, DMStatus.SubmitForReview, client.ClientName, DemandLink);
                                    }
                                }
                            }

                        }
                        unitOfWork.Save();
                    }
                }

                else
                {
                    //throw new Exception("Select Demand/Submit the one!");
                    throw new Exception("2");
                }

            }
            catch (Exception exp)
            {
                return Json(new { Result = "ERROR", Message = exp.Message });
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        // ----------------------------------------------------Save Approvers ------------------------------------------------------------ //
        [HttpPost]
        public ActionResult SaveAdmins(int DemandId = 0, string ApproverIds = "")
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
                if (DemandId != 0)
                {
                    string[] SelectedAdminIds = ApproverIds.Split(',');
                    using (var contextFactory = new DbContextFactory())
                    {

                        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                        var Demand = unitOfWork.DMPostDemand.GetByID(DemandId);
                        if (Demand.StatusId == (int)DMStatus.Created || Demand.StatusId == (int)DMStatus.Revised || Demand.StatusId == (int)DMStatus.SubmitForRevision)
                        {
                            //Getting Existing Approvers
                            var ExistingApprovers = unitOfWork.DMDemandApprover.Get().Where(e => e.DemandId == DemandId).Select(e => e.ApproverId).ToList();


                            Demand.LastUpdatedDate = DateTime.Now;
                            Demand.UpdatedBy = uid;
                            unitOfWork.DMPostDemand.Update(Demand);



                            var deleteApprovers = from details in db.DMDemandApprover
                                                  where details.DemandId == DemandId
                                                  select details;

                            foreach (var detail in deleteApprovers)
                            {
                                unitOfWork.DMDemandApprover.Delete(detail.Id);

                            }
                            foreach (var item in SelectedAdminIds)
                            {
                                if (item != " " || item != "," || item != "")
                                {
                                    var ApproverId = Convert.ToInt32(item);
                                    DMDemandApprover DemandApprover = new DMDemandApprover();
                                    DemandApprover.DemandId = Convert.ToInt32(DemandId);
                                    DemandApprover.ApproverId = ApproverId;
                                    DemandApprover.Createdby = uid;
                                    DemandApprover.ClientId = client.ClientId;
                                    DemandApprover.CreatedDt = DateTime.Now;
                                    unitOfWork.DMDemandApprover.Insert(DemandApprover);
                                   
                                }
                            }
                        }

                        else if (Appuser.Text.ToLower() == "sysadmin")
                        {
                            var ExistingApprovers = unitOfWork.DMDemandApprover.Get().Where(e => e.DemandId == DemandId).Select(e => e.ApproverId).ToList();
                            Demand.LastUpdatedDate = DateTime.Now;
                            Demand.UpdatedBy = uid;
                            unitOfWork.DMPostDemand.Update(Demand);

                            var deleteApprovers = from details in db.DMDemandApprover
                                                  where details.DemandId == DemandId
                                                  select details;

                            foreach (var detail in deleteApprovers)
                            {
                                unitOfWork.DMDemandApprover.Delete(detail.Id);

                            }
                            foreach (var item in SelectedAdminIds)
                            {
                                if (item != " " || item != "," || item != "")
                                {
                                    var ApproverId = Convert.ToInt32(item);
                                    DMDemandApprover DemandApprover = new DMDemandApprover();
                                    DemandApprover.DemandId = Convert.ToInt32(DemandId);
                                    DemandApprover.ApproverId = ApproverId;
                                    DemandApprover.Createdby = uid;
                                    DemandApprover.ClientId = client.ClientId;
                                    DemandApprover.CreatedDt = DateTime.Now;
                                    unitOfWork.DMDemandApprover.Insert(DemandApprover);

                                }
                            }
                        }
                        unitOfWork.Save();
                    }
                }
                else
                {
                    //throw new Exception("Select Demand/Submit the one!");
                    throw new Exception("2");
                }

            }
            catch (Exception exp)
            {
                return Json(new { Result = "ERROR", Message = exp.Message });
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }


        //-------------------------------------------Task And Activities----------------------------------------------------//

        [HttpPost]
        public ActionResult GetTaskAndActivities([DataSourceRequest] DataSourceRequest request, int DemandId)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            return Json(DMHelper.GetTaskAndActivitiesDetails(client.ClientId,DemandId).ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult AddTaskAndActivities([DataSourceRequest] DataSourceRequest request, DMTaskAndActivitiesModel DmTaskAndActivitiesModel, int idID)
        {

            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];

            Db db = new Db();
            var demanddetails = (from a in db.DMPostDemand
                                 where a.Id == idID && a.ClientId == client.ClientId
                                 select a).FirstOrDefault();
            var demandTasks = DMHelper.getDemandTask(idID, client.ClientId);

            string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            token = token.Replace("+", "=");
            DmTaskAndActivitiesModel.UserTaskToken = token;
            if(demanddetails.ImplementationManager!=null)
            {
                if (DmTaskAndActivitiesModel.TaskId != null)
                {

                    DmTaskAndActivitiesModel.Task = DMHelper.getTaskName(Convert.ToInt32(DmTaskAndActivitiesModel.TaskId));
                    DmTaskAndActivitiesModel.Stage = DMHelper.getStageName(Convert.ToInt32(DmTaskAndActivitiesModel.StageId));
                    foreach (var item in demandTasks)
                    {
                        if (item == Convert.ToInt16(DmTaskAndActivitiesModel.TaskId))
                        {
                            ModelState.AddModelError("TaskId", "Selected Task already exist, please click on edit.");
                            return Json(new[] { DmTaskAndActivitiesModel }.ToDataSourceResult(request, ModelState));
                        }
                    }
                    var s1 = (from a in db.DMStage
                              where a.ClientId == client.ClientId && a.DemandStage == "Cancelled"
                              select a.Id).FirstOrDefault();
                    var s2 = (from a in db.DMStage
                              where a.ClientId == client.ClientId && a.DemandStage == "Completed"
                              select a.Id).FirstOrDefault();
                    var s3 = (from a in db.DMStage
                              where a.ClientId == client.ClientId && a.DemandStage == "Closed"
                              select a.Id).FirstOrDefault();

                    if (DmTaskAndActivitiesModel.Task == "Gate 16 - CLOSED" && DmTaskAndActivitiesModel.EndDate != null)
                    {
                        var tasklist = (from a in db.DMTaskAndActivities
                                        where a.ClientId == client.ClientId && a.DemandId == idID && a.StageId != s1 && a.StageId != s2 && a.StageId != s3
                                        select a).ToList();
                        if (tasklist.Count > 0)
                        {
                            ModelState.AddModelError("TaskId", "Please mark other task Cancelled/Completed/Closed before adding this task");
                            return Json(new[] { DmTaskAndActivitiesModel }.ToDataSourceResult(request, ModelState));
                        }

                    }

                    if (DmTaskAndActivitiesModel.StartDate.Value.Date >= demanddetails.CreatedDate.Value.Date)
                    {
                        if (DmTaskAndActivitiesModel.EndDate != null && DmTaskAndActivitiesModel.EndDate.Value.Date < DmTaskAndActivitiesModel.StartDate.Value.Date)
                        {
                            ModelState.AddModelError("TaskId", "EndDate should be greater than StartDate.");
                            return Json(new[] { DmTaskAndActivitiesModel }.ToDataSourceResult(request, ModelState));
                        }
                        else if (DmTaskAndActivitiesModel.Task != "Gate 16 - CLOSED" && DmTaskAndActivitiesModel.EndDate == null)
                        {
                            ModelState.AddModelError("TaskId", "EndDate should be greater than StartDate.");
                            return Json(new[] { DmTaskAndActivitiesModel }.ToDataSourceResult(request, ModelState));
                        }
                        DmTaskAndActivitiesModel.UpdatedDate = DateTime.Now;
                        int usid = (int)Session["UserId"];
                        DmTaskAndActivitiesModel.CreatedBy = usid;
                        var res = DMHelper.CreateTaskAndActivities(DmTaskAndActivitiesModel, client.ClientId, idID);
                        DmTaskAndActivitiesModel.Id = res;

                        var DemandLink = Url.Action("Index", "PostDemand", new { ViewDemand = false }, "http");
                        sendTaskDetils(DmTaskAndActivitiesModel, idID, client.ClientId, client.ClientName, uid, DemandLink, token);
                    }
                    else
                    {
                        ModelState.AddModelError("TaskId", "Task StartDate should be greater than ActualStartDate.");
                    }
                }

            }
            else
            {
                ModelState.AddModelError("TaskId", "Please select Operational Lead before creating the Task");
            }
            return Json(new[] { DmTaskAndActivitiesModel }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult EditTaskAndActivities([DataSourceRequest] DataSourceRequest request, DMTaskAndActivitiesModel DmTaskAndActivitiesModel, int idID)
        {
             ClientDetail client = (ClientDetail)Session["SessionClient"];

            Db db = new Db();
            
            var demanddetails = (from a in db.DMPostDemand
                                 where a.Id == idID && a.ClientId == client.ClientId
                                 select a).FirstOrDefault();
            var demandTasks = DMHelper.getDemandTask(idID, client.ClientId);

            string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            token = token.Replace("+", "=");
            DmTaskAndActivitiesModel.UserTaskToken = token;
            if(demanddetails.ImplementationManager!=null)
            {
                if (DmTaskAndActivitiesModel.TaskId != null)
                {

                    DmTaskAndActivitiesModel.Task = DMHelper.getTaskName(Convert.ToInt32(DmTaskAndActivitiesModel.TaskId));
                    DmTaskAndActivitiesModel.Stage = DMHelper.getStageName(Convert.ToInt32(DmTaskAndActivitiesModel.StageId));
                    var demtaskid = (from a in db.DMTaskAndActivities
                                     where a.ClientId == client.ClientId && a.DemandId == idID && a.Id == DmTaskAndActivitiesModel.Id
                                     select a.TaskId).FirstOrDefault();
                    if (demtaskid != Convert.ToInt32(DmTaskAndActivitiesModel.TaskId))
                    {
                        ModelState.AddModelError("TaskId", "Selected Task already exist, please click on edit.");
                        return Json(new[] { DmTaskAndActivitiesModel }.ToDataSourceResult(request, ModelState));
                    }

                    var s1 = (from a in db.DMStage
                              where a.ClientId == client.ClientId && a.DemandStage == "Cancelled"
                              select a.Id).FirstOrDefault();
                    var s2 = (from a in db.DMStage
                              where a.ClientId == client.ClientId && a.DemandStage == "Completed"
                              select a.Id).FirstOrDefault();
                    var s3 = (from a in db.DMStage
                              where a.ClientId == client.ClientId && a.DemandStage == "Closed"
                              select a.Id).FirstOrDefault();

                    if (DmTaskAndActivitiesModel.Task == "Gate 16 - CLOSED" && DmTaskAndActivitiesModel.EndDate != null)
                    {
                        var tasklist = (from a in db.DMTaskAndActivities
                                        where a.ClientId == client.ClientId && a.DemandId == idID && a.StageId != s1 && a.StageId != s2 && a.StageId != s3
                                        select a).ToList();
                        if (tasklist.Count > 0)
                        {
                            ModelState.AddModelError("TaskId", "Please mark other task Cancelled/Completed/Closed before editing this task");
                            return Json(new[] { DmTaskAndActivitiesModel }.ToDataSourceResult(request, ModelState));
                        }

                    }

                    if (DmTaskAndActivitiesModel.StartDate.Value.Date >= demanddetails.CreatedDate.Value.Date)
                    {
                        if (DmTaskAndActivitiesModel.EndDate != null && DmTaskAndActivitiesModel.EndDate.Value.Date < DmTaskAndActivitiesModel.StartDate.Value.Date)
                        {
                            ModelState.AddModelError("TaskId", "EndDate should be greater than StartDate.");
                            return Json(new[] { DmTaskAndActivitiesModel }.ToDataSourceResult(request, ModelState));
                        }
                        else if (DmTaskAndActivitiesModel.Task != "Gate 16 - CLOSED" && DmTaskAndActivitiesModel.EndDate == null)
                        {
                            ModelState.AddModelError("TaskId", "EndDate should be greater than StartDate.");
                            return Json(new[] { DmTaskAndActivitiesModel }.ToDataSourceResult(request, ModelState));
                        }
                        int usid = (int)Session["UserId"];
                        DmTaskAndActivitiesModel.UpdatedBy = usid;
                        DmTaskAndActivitiesModel.UpdatedDate = DateTime.Now;

                        var res = DMHelper.UpdateTaskAndActivities(DmTaskAndActivitiesModel, client.ClientId);
                        if (res)
                        {
                            var DemandLink = Url.Action("Index", "PostDemand", new { ViewDemand = false }, "http");
                            sendTaskDetils(DmTaskAndActivitiesModel, idID, client.ClientId, client.ClientName, usid, DemandLink, token);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("TaskId", "Task StartDate should be greater than ActualStartDate.");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("TaskId", "Please select Operational Lead before creating the Task");
            }
            
     

            return Json(new[] { DmTaskAndActivitiesModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult DeleteTaskAndActivities([DataSourceRequest] DataSourceRequest request, DMTaskAndActivitiesModel DmTaskAndActivitiesModel)
        {
            if (DmTaskAndActivitiesModel != null && ModelState.IsValid)
            {
                var v = DMHelper.RemoveTaskAndActivities(DmTaskAndActivitiesModel.Id);
            }

            return Json(new[] { DmTaskAndActivitiesModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult ReadTaskActivitiesHistory([DataSourceRequest] DataSourceRequest request, int DemandId = 0)
        {
            return Json(DMHelper.GetTaskActivitiesHistoryDetails(DemandId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        
        //----------------------------------------- LOGS -----------------------------------//

        [HttpPost]
        public ActionResult ReadBusinessBenefits([DataSourceRequest] DataSourceRequest request, int DemandId = 0)
        {
            return Json(DMHelper.GetBusinessBenefitsDetails(DemandId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReadITSavings([DataSourceRequest] DataSourceRequest request, int DemandId = 0)
        {
            return Json(DMHelper.GetITSavingsDetails(DemandId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ReadPriorityHistory([DataSourceRequest] DataSourceRequest request, int DemandId = 0)
        {
            return Json(DMHelper.GetPriorityHistoryDet(DemandId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ReadImpPriorityHistory([DataSourceRequest] DataSourceRequest request, int DemandId = 0)
        {
            return Json(DMHelper.GetImpPriorityHistoryDetails(DemandId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ReadStatusHistory([DataSourceRequest] DataSourceRequest request, int DemandId = 0)
        {
            return Json(DMHelper.GetStatusHistoryDetails(DemandId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
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
        public ActionResult ComplexityList()
        {
            // var ClientID = (Client)Session["Client"];
            var ClientID = (ClientDetail)Session["SessionClient"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.IMComplexity.Get(x => x.ClientId == ClientID.ClientId).ToList();
            var item = applicationList.OrderBy(x => x.Complexity).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = char.ToUpper(p.Complexity[0]) + p.Complexity.Substring(1), Selected = true });
            return Json(item, JsonRequestBehavior.AllowGet); ;
        }
        public ActionResult LobList()
        {
            // var ClientID = (Client)Session["Client"];
            var ClientID = (ClientDetail)Session["SessionClient"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.DMLOB.Get(x => x.ClientId == ClientID.ClientId).ToList();
            var item = applicationList.OrderBy(x => x.DemandLOB).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.DemandLOB.ToString(), Selected = true });
            return Json(item, JsonRequestBehavior.AllowGet); ;
        }
        public ActionResult DemandSourceList()
        {
            // var ClientID = (Client)Session["Client"];
            var ClientID = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var IsNTTResource = unitOfWork.UserProfile.Get().Where(g => g.Id == uid && g.NTTDataResource == true).Any();
            var TempList = new List<SelectListItem>();
            if (IsNTTResource)
            {
                SelectListItem cli = new SelectListItem()
                {
                    Text = ClientID.ClientName,
                    Value = ClientID.ClientName,
                    Selected = false
                };
                TempList.Add(cli);
                SelectListItem ntt = new SelectListItem()
                {
                    Text = "NTT Data",
                    Value = "NTT Data",
                    Selected = true
                };
                TempList.Add(ntt);
            }
            else
            {
                SelectListItem cli = new SelectListItem()
                {
                    Text = ClientID.ClientName,
                    Value = ClientID.ClientName,
                    Selected = true
                };
                TempList.Add(cli);
                SelectListItem ntt = new SelectListItem()
                {
                    Text = "NTT Data",
                    Value = "NTT Data",
                    Selected = false
                };
                TempList.Add(ntt);
            }
            var item = TempList.AsEnumerable();
            return Json(item, JsonRequestBehavior.AllowGet); ;
        }
        public ActionResult DemandTypeList()
        {
            // var ClientID = (Client)Session["Client"];
            var ClientID = (ClientDetail)Session["SessionClient"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.DMDemandType.Get(x => x.ClientId == ClientID.ClientId).ToList();
            var item = applicationList.OrderBy(x => x.DemandType).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = char.ToUpper(p.DemandType[0]) + p.DemandType.Substring(1), Selected = true });
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
                                where ASA.ApplicationName.ToLower() == "demand manager" && (UR.RoleName == "admin" || UR.RoleName == "sysadmin" || UR.RoleName == "implementationmanager") && UA.ClientId == client.ClientId
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
            var AdminsWithName = DMHelper.GetimpManagers(clientid);
            return AdminsWithName;
        }


        //--------------------------------------------------Classification and Category -------------------------------------------------------------//

        //Saving---
        public ActionResult SaveClassificationCategory(int DemandId, string CatIds = "")
        {
            int uid = (int)Session["UserId"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            if (DemandId > 0)
            {
                //Delete Existing Categories
                try
                {
                    DMHelper.DeleteClassificationCategory(DemandId);
                    if (CatIds.Trim() != "")
                    {
                        DMHelper.AddClassificationCategory(DemandId,client.ClientId, CatIds, uid);
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
        //ConfigItme
        public ActionResult SaveConfigItems(int DemandId, string ConfigIds = "")
        {
            int uid = (int)Session["UserId"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            if (DemandId > 0)
            {
                //Delete Existing Categories
                try
                {
                    DMHelper.DeleteConfigItem(DemandId);
                    if (ConfigIds.Trim() != "")
                    {
                        DMHelper.AddConfigItem(DemandId,client.ClientId, ConfigIds, uid);
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


        //---------------------------------------------------------------- Email Notification --------------------------------------------------------------------//

        public void SendDemandNotification(string email, string fullname, string DemandName, DMStatus status, string clientname, string DemandLink)
        {
            try
            {
                var body = "";
                var mailId = email;
                var Subject = "Smart IT Demand Manager Auto notification";
                if (status == DMStatus.SubmitForReview || status == DMStatus.SubmitForApproval || status == DMStatus.SubmitForPlanning || status == DMStatus.SubmitForRevision)
                {
                    string StatusString = "";
                    //fulllname = Admin Name(Approver)
                    if (status == DMStatus.SubmitForReview)
                    {
                        StatusString = "Submitted For Review";
                        body = SubmitForReviewBody(fullname, DemandName, StatusString, clientname, DemandLink);
                    }
                    if (status == DMStatus.SubmitForRevision)
                    {
                        StatusString = "Submitted For Revision";
                        body = SubmitForReviewBody(fullname, DemandName, StatusString, clientname, DemandLink);//---------------Edit Required
                    }
                    if (status == DMStatus.SubmitForApproval)
                    {
                        StatusString = "Submitted For Approval";
                        body = SubmittedForApprovalBody(fullname, DemandName, StatusString, clientname, DemandLink);
                    }
                    if (status == DMStatus.SubmitForPlanning)
                    {
                        StatusString = "Submitted For Planning";
                        body = ImplementationManagerbody(fullname, DemandName, clientname, DemandLink);
                    }

                }
                else
                {
                    string StatusString = "";
                    //Hold
                    if (status == DMStatus.Hold)
                    {
                        StatusString = "put On Hold";
                    }
                    //Cancelled Demand
                    if (status == DMStatus.Cancelled)
                    {
                        StatusString = "Cancelled";
                    }
                    body = HoldOrCancelStatusBody(fullname, DemandName, StatusString, clientname, DemandLink);
                }

                App_Start.Helper.SendEMail(mailId, Subject, body);
            }
            catch
            {

            }
        }
        private string SubmittedForApprovalBody(string adminName, string Demand, string status, string clientname, string DemandLink)
        {
            int uid = (int)Session["UserId"];
            var username = "";
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var som = unitOfWork.UserProfile.GetByID(uid);
                username = som.FirstName +" "+ som.LastName;
            }

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Areas/DemandManager/Templates/SubmittedForApprovalEmailNotification.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{username}", username);
            body = body.Replace("{Demandname}", Demand);
            body = body.Replace("{Demandlink}", DemandLink);
            body = body.Replace("{clientname}", clientname);
            body = body.Replace("{approver}", adminName);

            return body;
        }

        private string SubmitForReviewBody(string ReviewerName, string Demand, string status, string clientname, string DemandLink)
        {
            int uid = (int)Session["UserId"];
            var username = "";
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var som = unitOfWork.UserProfile.GetByID(uid);
                username = som.FirstName +" "+ som.LastName;
            }

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Areas/DemandManager/Templates/SubmitForReviewEmailNotification.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{username}", username);
            body = body.Replace("{Demandlink}", DemandLink);
            body = body.Replace("{Demandname}", Demand);
            body = body.Replace("{clientname}", clientname);
            body = body.Replace("{approver}", ReviewerName);

            return body;
        }

        private string ImplementationManagerbody(string adminName, string Demand, string clientname, string DemandLink)
        {
            int uid = (int)Session["UserId"];
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Areas/DemandManager/Templates/ImplementationManagerEmailNotification.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{Demandlink}", DemandLink);
            body = body.Replace("{Demandname}", Demand);
            body = body.Replace("{clientname}", clientname);
            body = body.Replace("{adminName}", adminName);

            return body;
        }


        private string HoldOrCancelStatusBody(string username, string Demand, string status, string clientname, string DemandLink)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Areas/DemandManager/Templates/HoldOrCancelEmailNotification.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{username}", username);
            body = body.Replace("{Demandname}", Demand);
            body = body.Replace("{status}", status);
            body = body.Replace("{clientname}", clientname);
            body = body.Replace("{Demandlink}", DemandLink);

            return body;
        }

        //[HttpPost]
        //public ActionResult ListOfDMAdmins([DataSourceRequest] DataSourceRequest request, int DemandId)
        //{
        //    //Client client = (Client)Session["Client"];
        //    ClientDetail client = (ClientDetail)Session["SessionClient"];
        //    Db db = new Db();
        //    var TEMPlistOfAdmins = (from UA in db.UserApplications
        //                            join UP in db.UserProfiles on UA.UserId equals UP.Id
        //                            join ASA in db.AppStore on UA.ApplicationId equals ASA.Id
        //                            join UR in db.UserRoles on UA.RoleId equals UR.Id
        //                            join APPRS in db.DMDemandApprover.Where(w => w.DemandId == DemandId) on UP.Id equals APPRS.ApproverId into tmpApprs
        //                            from aprs in tmpApprs.DefaultIfEmpty()
        //                            where ASA.ApplicationName.ToLower() == "demand manager" && (UR.RoleName == "admin" || UR.RoleName == "sysadmin") && UA.ClientId == client.ClientId
        //                            select new DemandApproverModel
        //                            {
        //                                Id = UP.Id,
        //                                FirstName = UP.FirstName,
        //                                LastName = UP.LastName,
        //                                DemandId = DemandId,
        //                                DemandApproverId = aprs.Id,
        //                                ApprovedDate = aprs.ApprovedDate
        //                            }
        //                       ).Distinct().OrderBy(x => x.FirstName).ToList();

        //    //Adding Status infront it ...
        //    //Logic is => IF (__ApprovedDt__ is NULL) then Admin has not Approved(might have cancelled/Hold/Decline)
        //    //Else  (i.e, __ApprovedDt__ is NOT NULl) Admin has Approved the IDEA
        //    //Also checking DemandApproverId property in above Object -> to check whether they are selected as aprrover or not
        //    var listOfAdmins = TEMPlistOfAdmins.Select(f =>
        //            new DemandApproverModel
        //            {
        //                Id = f.Id,
        //                FirstName = f.FirstName,
        //                LastName = f.LastName,
        //                DemandId = f.DemandId,
        //                DemandApproverId = f.DemandApproverId,
        //                ApprovedDate = f.ApprovedDate,
        //                ApprovedOrNot = (f.ApprovedDate != null || f.ApprovedDate > DateTime.MinValue) ? "Approved" : (f.DemandApproverId == null ? "" : "Not Approved")

        //            }
        //    );

        //    return Json(listOfAdmins.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}


        public void sendTaskDetils(DMTaskAndActivitiesModel DmTaskAndActivitiesModel,int DemandId, int clientId, string ClientName, int uid, string DemandLink,string token)
        {
            var contextFactory = new DbContextFactory();
            Db db = new Db();
            var stageId = Convert.ToInt32(DmTaskAndActivitiesModel.StageId);
            var taskId = Convert.ToInt32(DmTaskAndActivitiesModel.TaskId);

            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var Demand = unitOfWork.DMPostDemand.GetByID(DemandId);
            var Task = (from a in db.DMTask
                        where a.Id == taskId
                        select a.DemandTask).FirstOrDefault();
            var Stage = (from a in db.DMStage
                         where a.Id == stageId
                         select a.DemandStage).FirstOrDefault();
            var u = unitOfWork.UserProfile.Get(g => g.Id == uid).FirstOrDefault();
            var oplead= unitOfWork.UserProfile.Get(g => g.Id == Demand.ImplementationManager).FirstOrDefault();
            var CancelTaskLink = Url.Action("cancelTask", "PostDemand", new { uniqueid = token, comments="" }, "http");
            var CompleteTaskLink = Url.Action("completeTask", "PostDemand", new { uniqueid = token, comments = "" }, "http");
            var TaskUpdateLink = Url.Action("updateTask", "PostDemand",  new { uniqueid = token },"http");
            var taskstartdate = DmTaskAndActivitiesModel.StartDate.Value.ToString("dd/MMM/yyyy");
            DemandLink = DemandLink.Trim() + "&DemandId=" + DemandId.ToString();
            SendTaskNotification(DmTaskAndActivitiesModel.AssignedTo,u.FirstName+" "+u.LastName,Task, DmTaskAndActivitiesModel.Description, Demand.ItemName, oplead.FirstName+" "+ oplead.LastName, taskstartdate, ClientName, CompleteTaskLink, CancelTaskLink,token, TaskUpdateLink);
        }

        public void SendTaskNotification(string email,string username, string Task, string taskDesc, string demand, string OpLead,string taskStartDate, string clientname, string CompleteTaskLink, string CancelTaskLink,string token,string TaskUpdateLink)
        {
            
           var body = "";
           var mailId = email;
           var Subject = "Smart IT Demand Manager Auto notification";
           body = TaskNotificationBody(Task, taskDesc, taskStartDate, demand, OpLead, username,clientname, CompleteTaskLink, CancelTaskLink,token,TaskUpdateLink);
        
           App_Start.Helper.SendEMail(mailId, Subject, body);

        }
        private string TaskNotificationBody(string task,string taskDesc, string taskStDate,string Demand,string OpLead,string username,string clientname, string CompleteTaskLink, string CancelTaskLink, string token, string TaskUpdateLink)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Areas/DemandManager/Templates/TaskAssignedNotification.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{task}", task);
            body = body.Replace("{taskDesc}", taskDesc);
            body = body.Replace("{taskStDate}", taskStDate);
            body = body.Replace("{Demandname}", Demand);
            body = body.Replace("{OpLead}", OpLead);
            body = body.Replace("{username}", username);
            body = body.Replace("{clientname}", clientname);
            body = body.Replace("{CompleteTaskLink}", CompleteTaskLink);
            body = body.Replace("{CancelTaskLink}", CancelTaskLink);    
            body = body.Replace("{token}", token);
            body = body.Replace("{TaskUpdateLink}", TaskUpdateLink);

            return body;
        }

        private string CreateDemandNotificationBody(string DemandName,string clientName,string CreatedBy,string adminName,string DemandLink)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Areas/DemandManager/Templates/DemandCreatedEmailNotification.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{Demandname}", DemandName);
            body = body.Replace("{adminName}", adminName);
            body = body.Replace("{CreatedBy}", CreatedBy);
            body = body.Replace("{clientname}", clientName);
            body = body.Replace("{Demandlink}", DemandLink);

            return body;
        }
        [AllowAnonymous]
        public ActionResult completeTask(string uniqueid, string comments)
        {
          
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();
           
                var taskData = (from a in db.DMTaskAndActivities
                                where a.UserTaskToken == uniqueid
                                select a).FirstOrDefault();
            var clientid = (from a in db.DMTaskAndActivities
                            where a.UserTaskToken == uniqueid
                            select a.ClientId).FirstOrDefault();
            DMTaskActivitiesModel data = new DMTaskActivitiesModel();

            // var taskData = unitOfWork.DMTaskAndActivities.Get().Where(f => f.UserTaskToken == uniqueid).FirstOrDefault();
            if (clientid > 0)
                {
                var Demand = unitOfWork.DMPostDemand.GetByID(taskData.DemandId);
                var Task = (from a in db.DMTask
                            where a.Id == taskData.TaskId
                            select a.DemandTask).FirstOrDefault();
                var clientName = (from a in db.Clients
                                  where a.Id == taskData.ClientId
                                  select a.ClientName).FirstOrDefault();
                data.ClientName = clientName;
                string imageDataURL = null;
                     var Img = ClientProfileManagerHelper.getProfileClientFile(taskData.ClientId.Value);
                      if (Img != null)
                       {
                           string imageBase64Data = Convert.ToBase64String(Img);
                           imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);

                       }
                       data.imgurl = imageDataURL;

                       var stageId = (from a in db.DMStage
                                   where a.ClientId == taskData.ClientId && a.DemandStage == "Completed"
                                   select a.Id).FirstOrDefault();
                     var su = ValidateTakUser(uniqueid, comments, stageId);
                    if (su)
                    {
                        data.DemandName = Demand.ItemName;
                        data.TaskName = Task;
                        
                        ViewBag.Status = "Completed";
                        ViewBag.Comments = comments;
                        ViewBag.Message = "Thank you! Your response was submitted  sucessfully";
                        return View("InvalidTask", data);
                    }
                }
                ViewBag.Message = "Invalid task access or You are no longer the link is valid";
                return View("InvalidTask",data);
            
        }

        [AllowAnonymous]
        public ActionResult cancelTask(string uniqueid, string comments)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();

            var taskData = (from a in db.DMTaskAndActivities
                            where a.UserTaskToken == uniqueid
                            select a).FirstOrDefault();
            DMTaskActivitiesModel data = new DMTaskActivitiesModel();
            var clientid = (from a in db.DMTaskAndActivities
                            where a.UserTaskToken == uniqueid
                            select a.ClientId).FirstOrDefault();
            //var taskData = unitOfWork.DMTaskAndActivities.Get().Where(f => f.UserTaskToken == uniqueid).FirstOrDefault();
            if (clientid > 0)
            {
                var Demand = unitOfWork.DMPostDemand.GetByID(taskData.DemandId);
                var Task = (from a in db.DMTask
                            where a.Id == taskData.TaskId
                            select a.DemandTask).FirstOrDefault();
                var clientName = (from a in db.Clients
                                  where a.Id == taskData.ClientId
                                  select a.ClientName).FirstOrDefault();
                data.ClientName = clientName;
                string imageDataURL = null;
                      var Img = ClientProfileManagerHelper.getProfileClientFile(taskData.ClientId.Value);
                      if (Img != null)
                        {
                           string imageBase64Data = Convert.ToBase64String(Img);
                           imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);

                       }
                     data.imgurl = imageDataURL;

                      var stageId = (from a in db.DMStage
                                   where a.ClientId == taskData.ClientId && a.DemandStage == "Cancelled"
                                   select a.Id).FirstOrDefault();
                    var su = ValidateTakUser(uniqueid, comments, stageId);
                    if (su)
                    {
                        data.DemandName = Demand.ItemName;
                        data.TaskName = Task;
                       
                        ViewBag.Status = "Cancelled";
                        ViewBag.Comments = comments;
                        ViewBag.Message = "Thank you! Your response was submitted  sucessfully";
                        return View("InvalidTask", data);
                    }
                }
                ViewBag.Message = "Invalid task access or You are no longer the link is valid";
                return View("InvalidTask",data);
            
        }

        private bool ValidateTakUser(string uniqueid, string comments, int taskState)
        {
            Db db = new Db();
            if (string.IsNullOrEmpty(uniqueid))
            {
                return false;
            }
           // var taskData = unitOfWork.DMTaskAndActivities.Get().Where(f => f.UserTaskToken == uniqueid).FirstOrDefault();
            var taskData = (from a in db.DMTaskAndActivities
                            where a.UserTaskToken == uniqueid
                            select a).FirstOrDefault();
            if (taskData != null)
            {
                taskData.StageId =taskState;
                taskData.Comments = comments;
                db.SaveChanges();
                return true;
            }
            return false;
        }
        [AllowAnonymous]
        public ActionResult updateTask(string uniqueid)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();
            var taskData = unitOfWork.DMTaskAndActivities.Get().Where(f => f.UserTaskToken == uniqueid).FirstOrDefault();
            if (taskData==null)
            {
                DMTaskActivitiesModel data = new DMTaskActivitiesModel();
                ViewBag.Message = "Invalid task access or You are no longer the link is valid";
                return View("InvalidTask",data);
            }
            else
            {
                string imageDataURL = null;
                var Img = ClientProfileManagerHelper.getProfileClientFile(taskData.ClientId.Value);
                if (Img != null)
                {
                    string imageBase64Data = Convert.ToBase64String(Img);
                    imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);

                }
                DMTaskActivitiesModel data = new DMTaskActivitiesModel();
                var Demand = unitOfWork.DMPostDemand.GetByID(taskData.DemandId);
                var Task = (from a in db.DMTask
                            where a.Id == taskData.TaskId
                            select a.DemandTask).FirstOrDefault();
                var oplead = unitOfWork.UserProfile.Get(g => g.Id == Demand.ImplementationManager).FirstOrDefault();
                var taskstartdate = taskData.StartDate.Value.ToString("dd/MMM/yyyy");
                var clientName = (from a in db.Clients
                                  where a.Id == taskData.ClientId
                                  select a.ClientName).FirstOrDefault();
                data.DemandName = Demand.ItemName;
                data.TaskName = Task;
                data.stDate = taskstartdate;
                data.opLead = oplead.FirstName+" "+oplead.LastName;
                data.taskDesc = taskData.Description;
                data.ClientId = taskData.ClientId.Value;
                data.ClientName = clientName;
                data.imgurl = imageDataURL;
                data.uniqueid = uniqueid;

                ViewBag.CancelTaskLink = Url.Action("cancelTask", "PostDemand", new { uniqueid = uniqueid }, "http");
                ViewBag.CompleteTaskLink = Url.Action("completeTask", "PostDemand", new { uniqueid = uniqueid }, "http");
                return View("UpdateTask", data);
            }
           
        }
    }
}
