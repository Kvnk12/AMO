using Data;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.IdeaManager.Dto;
using WebUI.Areas.IdeaManager.Models;
using PagedList.Mvc;
using PagedList;
using WebUI.Areas.IdeaManager.Objects;
using System.IO;
using WebUI.Dto;
using log4net;
using Domain.Entities.A3MM;
using NTTData.AMOPortal.WebUI.Filters;

namespace WebUI.Areas.IdeaManager.Controllers
{
    public class IdeaApprovalController : Controller
    {
        //
        // GET: /IdeaManager/ViewIdea/
        private static readonly ILog Log = LogManager.GetLogger(typeof(IdeaApprovalController));
        [ActionTrack]
        public ActionResult Index(int? page=1, string sortOrder="")
        {
          try { 
           // var Cl = (Client)Session["Client"];
            ClientDetail Cl = (ClientDetail)Session["SessionClient"];

            int uid = (int)Session["UserId"];
            ViewData["Approvers"] = IMHelper.ListOfAdminsSysAdmins(Cl.ClientId);
            ViewData["GetStringResult"] = getFullName(uid);
            // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            ViewBag.ideas = String.IsNullOrEmpty(sortOrder) ? "idea_desc" : "";
            ViewBag.Status = string.IsNullOrEmpty(sortOrder) ? "Status_desc" : "Status";
            ViewBag.Description = string.IsNullOrEmpty(sortOrder) ? "Description_desc" : "Description";
            ViewBag.Createdby = string.IsNullOrEmpty(sortOrder) ? "Createdby_desc" : "Createdby";
            ViewBag.ideatype = string.IsNullOrEmpty(sortOrder) ? "ideatype_desc" : "ideatype";
            ViewBag.ideasource = string.IsNullOrEmpty(sortOrder) ? "ideasource_desc" : "ideasource";
            ViewBag.LOB = string.IsNullOrEmpty(sortOrder) ? "LOB_desc" : "LOB";
            ViewBag.configitem = string.IsNullOrEmpty(sortOrder) ? "configitem_desc" : "configitem";
            var user = "";
            IList<IdeaModel> x = new List<IdeaModel>();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                user = unitOfWork.UserProfile.Get(g => g.Id == uid).Select(g => g.RoleId).FirstOrDefault();
            }

            if (user.ToLower() == "superuser")
            {
                x = AllPostIdea_SuperUser(client.ClientId);
            }
            else
            {
                x = AllPostIdeas(client.ClientId, uid);
            }


            /*  var x = AllPostIdeas(client.Id, uid);*////.ToPagedList(page ?? 1, 8)/*.OrderByDescending(s => s.IdeaName)*/ ;
            IPagedList<IdeaModel> ideas = null;
            switch (sortOrder)
            {
                case "idea_desc":
                    ideas = x.OrderByDescending(s => s.IdeaName).ToPagedList(page ?? 1, 100);
                    break;
                case "Status_desc":
                    ideas = x.OrderByDescending(s => s.Status).ToPagedList(page ?? 1, 100);
                    break;
                case "Status":
                    ideas = x.OrderBy(s => s.Status).ToPagedList(page ?? 1, 100);
                    ViewBag.Status = "Status_desc";
                    break;
                case "Description_desc":
                    ideas = x.OrderByDescending(s => s.Description).ToPagedList(page ?? 1, 100);
                    break;
                case "Description":
                    ideas = x.OrderBy(s => s.Description).ToPagedList(page ?? 1, 100);
                    ViewBag.Description = "Description_desc";
                    break;
                case "Createdby_desc":
                    ideas = x.OrderByDescending(s => s.CreatedByUser).ToPagedList(page ?? 1, 100);
                    break;
                case "Createdby":
                    ideas = x.OrderBy(s => s.CreatedByUser).ToPagedList(page ?? 1, 100);
                    ViewBag.Createdby = "Createdby_desc";
                    break;
                case "ideatype_desc":
                    ideas = x.OrderByDescending(s => s.IdeaType).ToPagedList(page ?? 1, 100);
                    break;
                case "ideatype":
                    ideas = x.OrderBy(s => s.IdeaType).ToPagedList(page ?? 1, 100);
                    ViewBag.ideatype = "ideatype_desc";
                    break;
                case "ideasource_desc":
                    ideas = x.OrderByDescending(s => s.IdeaSource).ToPagedList(page ?? 1, 100);
                    break;
                case "ideasource":
                    ideas = x.OrderBy(s => s.IdeaSource).ToPagedList(page ?? 1, 100);
                    ViewBag.ideasource = "ideasource_desc";
                    break;
                case "LOB_desc":
                    ideas = x.OrderByDescending(s => s.LOB).ToPagedList(page ?? 1, 100);
                    break;
                case "LOB":
                    ideas = x.OrderBy(s => s.LOB).ToPagedList(page ?? 1, 100);
                    ViewBag.LOB = "LOB_desc";
                    break;
                case "configitem_desc":
                    ideas = x.OrderByDescending(s => s.ConfigItemName).ToPagedList(page ?? 1, 100);
                    break;
                case "configitem":
                    ideas = x.OrderBy(s => s.ConfigItemName).ToPagedList(page ?? 1, 100);
                    ViewBag.configitem = "configitem_desc";
                    break;

                default:
                    ideas = x.OrderBy(s => s.IdeaName).ToPagedList(page ?? 1, 100);
                    break;
            }
            return View(ideas);
          }
          catch (Exception ex)
          {
              Log.Error("Error in IdeaApprovalController: Index ", ex);
                throw ex;
          }
        }

        public static IList<IdeaModel> AllPostIdea_SuperUser(int clientId)
        {

            Db db = new Db();
            var ideamod = (from posts in db.IMPostIdea
                           join usr in db.UserProfiles on posts.CreatedBy equals usr.Id
                           // join IA in db.IMIdeaApprover on posts.Id equals IA.IdeaId
                           join idt in db.IMIdeaType on posts.IdeaTypeId equals idt.Id into tmpidt
                          // join ids in db.IMIdeaSource on posts.IdeaSourceId equals ids.Id into tmpids
                           join idst in db.IMStatus on posts.StatusId equals idst.Id into tmpidst
                           join iteam in db.IMTeam on posts.TeamId equals iteam.Id into tmpteams
                           // join app in db.Applications on posts.ConfigItem equals app.Id into tmpapp
                           join lob in db.IMLOBSource on posts.LOBId equals lob.Id into tmplob
                           from status in tmpidst.DefaultIfEmpty()
                           //from source in tmpids.DefaultIfEmpty()
                           from idtype in tmpidt.DefaultIfEmpty()
                           from team in tmpteams.DefaultIfEmpty()
                               //  from apps in tmpapp.DefaultIfEmpty()
                           from lobs in tmplob.DefaultIfEmpty()
                           where posts.ClientId == clientId && posts.IsDeleted==false && (posts.StatusId == (int)IMStatus.SubmitForApproval ||
                               //  posts.StatusId == (int)IMStatus.Declined ||
                                // posts.StatusId == (int)IMStatus.Cancelled ||
                                 posts.StatusId == (int)IMStatus.SubmitForReview || posts.StatusId == (int)IMStatus.Hold)
                           select new IdeaModel
                           {
                               Ideasnumber = posts.IdeaNumber,
                               IsInternalInitiative = posts.InternalInitiative,
                               Team = team.Team,
                               ClientId = posts.ClientId,
                               //ConfigItem = posts.ConfigItem,
                               //ConfigItemName = apps.ApplicationName,
                               CreatedByUser = usr.FirstName + " " + usr.LastName,
                               CreatedBy = posts.CreatedBy,
                               CreatedDate = posts.CreatedDate,
                               Description = posts.Description,
                               IdeaId = posts.Id,
                               IdeaName = posts.ItemName,
                               IdeaSource = posts.IdeaSource,
                               //IdeaSourceId = posts.IdeaSourceId,
                               IdeaType = idtype.IdeaType,
                               //  IdeaTypeId = posts.IdeaTypeId,
                               IsDeleted = posts.IsDeleted,
                               //  ItemType= posts.id
                               LOB = lobs.LOB,
                               LOBId = posts.LOBId,
                               //  PriorityComments = posts.PriorityComments,
                               //StatusComments = posts.StatusComments,
                               //  PriorityId = posts.PriorityId,
                               QtyFication = posts.QtyFication,
                               Status = status.Status,
                               StatusId = posts.StatusId
                           }
                          ).ToList();
            foreach (var ideamodel in ideamod)
            {
                ideamodel.SelectedApproverIds = IdeaManagerHelper.GetIdeaApprovers(ideamodel.IdeaId);
            }
            return ideamod;

        }

        public static IList<IdeaModel> AllPostIdeas(int clientId, int uid)
        {

            Db db = new Db();
            var ideamod = (from posts in db.IMPostIdea
                           join usr in db.UserProfiles on posts.CreatedBy equals usr.Id
                           join IA in db.IMIdeaApprover on posts.Id equals IA.IdeaId
                           join idt in db.IMIdeaType on posts.IdeaTypeId equals idt.Id into tmpidt
                           //join ids in db.IMIdeaSource on posts.IdeaSourceId equals ids.Id into tmpids
                           join idst in db.IMStatus on posts.StatusId equals idst.Id into tmpidst
                           join iteam in db.IMTeam on posts.TeamId equals iteam.Id into tmpteams
                           //   join app in db.Applications on posts.ConfigItem equals app.Id into tmpapp
                           join lob in db.IMLOBSource on posts.LOBId equals lob.Id into tmplob
                           from status in tmpidst.DefaultIfEmpty()
                           //from source in tmpids.DefaultIfEmpty()
                           from idtype in tmpidt.DefaultIfEmpty()
                           from team in tmpteams.DefaultIfEmpty()
                               //  from apps in tmpapp.DefaultIfEmpty()
                           from lobs in tmplob.DefaultIfEmpty()
                           where  posts.ClientId == clientId && posts.IsDeleted==false && IA.ApproverId==uid
                                            && (posts.StatusId == (int)IMStatus.SubmitForApproval ||// posts.StatusId == (int)IMStatus.Declined || 
                                               // posts.StatusId == (int)IMStatus.Cancelled || 
                                                posts.StatusId == (int)IMStatus.SubmitForReview || 
                                                posts.StatusId == (int)IMStatus.Hold)
                           select new IdeaModel
                           {
                               Ideasnumber = posts.IdeaNumber,
                               ClientId = posts.ClientId,
                               IsInternalInitiative = posts.InternalInitiative,
                               Team = team.Team,
                               // ConfigItem = posts.ConfigItem,
                               //   ConfigItemName = apps.ApplicationName,
                               CreatedByUser = usr.FirstName + " " + usr.LastName,
                               CreatedBy = posts.CreatedBy,
                               CreatedDate = posts.CreatedDate,
                               Description = posts.Description,
                               IdeaId = posts.Id,
                               IdeaName = posts.ItemName,
                               IdeaSource = posts.IdeaSource,
                               //IdeaSourceId = posts.IdeaSourceId,
                               IdeaType = idtype.IdeaType,
                               //   IdeaTypeId = posts.IdeaTypeId,
                               IsDeleted = posts.IsDeleted,
                               //  ItemType= posts.id
                               LOB = lobs.LOB,
                               LOBId = posts.LOBId,
                               //   PriorityComments = posts.PriorityComments,
                               //StatusComments=posts.StatusComments,
                               //   PriorityId =posts.PriorityId,
                               QtyFication = posts.QtyFication,
                               Status = status.Status,
                               StatusId = posts.StatusId,
                               ApprovedDate = IA.ApprovedDate
                           }
                          ).ToList();
            foreach (var ideamodel in ideamod)
            {
                ideamodel.SelectedApproverIds = IdeaManagerHelper.GetIdeaApprovers(ideamodel.IdeaId);
            }

            return ideamod;

        }

        //[HttpPost]
        //public ActionResult SubmittedForReview([DataSourceRequest] DataSourceRequest request, int AllIdeas=1)
        //{
        //    var UserId = (int)Session["UserId"];
        //    var Cl = (Client)Session["Client"];
        //    var UserRoleWeight = GetDropDownListItems.UserRoleForClient(UserId, Cl.Id);
        //    bool IsAdmin = false;
        //    bool IsCusUser = false;
        //    if(UserRoleWeight.Text.ToLower() == "customeruser") { IsCusUser = true; }
        //    if (UserRoleWeight.Text.ToLower() == "admin" || UserRoleWeight.Text.ToLower() == "sysadmin") { IsAdmin = true; }
        //    IEnumerable<SubmitForReviewModel> IdeaListTomodel = null;
        //    if (AllIdeas == (int)IMViewIdea.ViewAllIdeas)
        //    {
        //        IdeaListTomodel = IMHelper.SubmitForReview(Cl.Id,IsCusUser);
        //    }
        //    else
        //    {
        //        IdeaListTomodel = IMHelper.SubmitForReview(Cl.Id, UserId, IsAdmin);
        //    }
        //    return Json(IdeaListTomodel.AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}

        [ActionTrack]
        public ActionResult ApproveIdeas(IList<IdeaModel> inputs, string btnSubmit)
        {
          try {
            int uid = (int)Session["UserId"];
                ViewData["GetStringResult"] = getFullName(uid);
                //string[] ;
                // var Cl = (Client)Session["Client"];
                ClientDetail Cl = (ClientDetail)Session["SessionClient"];
            //This variable to hold--:
            //To reset the Approved Data - If one of the approver 
           // bool Reset = false;
            ModelState.Clear();
            bool checkboxchecked = false;
            int chkcount = 0;
            int ideaSelectedForApproval = 0;
                if(inputs==null)
                {
                    ModelState.AddModelError("chkerr", "No Idea");
                    return RedirectToAction("", "IdeaApproval");
                }
                foreach (var c in inputs)
            {
                if (c.IsChecked)
                {
                    checkboxchecked = true;
                    chkcount++;
                    ideaSelectedForApproval = c.IdeaId;
                }
                if (c.SelectedApproverIds==null&& c.IsChecked == true)
                {
                    ModelState.AddModelError(c.IdeaId + "mandate", c.IdeaName + "- Please select the Approver.");
                }

                if (c.IsChecked == true && btnSubmit=="Approve" && c.PriorityId <= 0)
                {
                    ModelState.AddModelError(c.IdeaId + "mandate", c.IdeaName + "- Priority is mandatory.");
                    checkboxchecked = true;
                }

                if (c.IsChecked == true && (string.IsNullOrEmpty(c.StatusComments) && (btnSubmit == "Cancel" || btnSubmit == "Decline" || btnSubmit == "Hold" || btnSubmit == "Revise")))
                {
                    ModelState.AddModelError(c.IdeaId + "mandate", c.IdeaName + "- Approval comments are mandatory.");
                    checkboxchecked = true;
                }
            }
            if (!checkboxchecked)
            {
                ModelState.AddModelError("chkerr", "Please select any one of the ideas");
            }
            //if (chkcount>1 && btnSubmit == "Approve")
            //{
            //    ModelState.AddModelError("chkerr", "Please select only one Idea to approve.");
            //}
            if (!ModelState.IsValid)
            {
                ViewData["Approvers"] = ViewData["Approvers"] = IMHelper.ListOfAdminsSysAdmins(Cl.ClientId);
                return View("Index", inputs.ToPagedList(1, 100));
            }
            
            if (ModelState.IsValid)
            {                   
                IMStatus ideaStatus = IMStatus.SubmitForApproval;
                if (btnSubmit == "Approve")
                {
                     ideaStatus = IMStatus.Approved;
                }
                else if (btnSubmit == "Close")
                {
                     ideaStatus = IMStatus.Cancelled;
                  //  Reset = true;
                }
                else if (btnSubmit == "Decline")
                {
                     ideaStatus = IMStatus.Declined;
                   // Reset = true;
                }
                else if (btnSubmit == "Hold")
                {
                     ideaStatus = IMStatus.Hold;
                }
                else if (btnSubmit == "Revise")
                {
                    ideaStatus = IMStatus.SubmitForRevision;

                }
                foreach (var idea in inputs)
                {
                    if (idea.IsChecked == true)
                    {

                        UpdateIdeaStatus(idea, ideaStatus);
                    }
                }
                TempData.Add("ValidationSuccessMsg", "Status got updated successfully.");
            }

                //return View("IdeasApproved", inputs);
            if (btnSubmit == "Approve" && !checkAllApproved(ideaSelectedForApproval) && chkcount==1)
                return RedirectToAction("ImplementationMg", new { IdeaId = ideaSelectedForApproval });
            else
                return RedirectToAction("Index");
          }
          catch (Exception ex)
          {
              Log.Error("Error in IdeaApprovalController: ApproveIdeas ", ex);
              throw ex;
          }
        }
        private bool checkAllApproved (int IdeaId)
        {
            var ret = true;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            //True if any one of Approver has not Approved, False if all approvers has approved
            ret = unitOfWork.IMIdeaApprover.Get().Where(w => w.IdeaId == IdeaId && w.ApprovedDate == null).Any();
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ideainput"></param>
        /// <param name="status"></param>
        /// <param name="Reset">To Reset all the Approved Data who and all has Approved for that Idea(Status - Other than Approval/Approve)</param>
        [ActionTrack]
        public void UpdateIdeaStatus(IdeaModel ideainput, IMStatus status)//, string Comments, int priority)
        {
            int uid = (int)Session["UserId"];
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                {
                    var idea = unitOfWork.IMPostIdea.GetByID(ideainput.IdeaId);
                    //getting exixting approvers for Idea
                    var CheckApprovers = unitOfWork.IMIdeaApprover.Get().Where(w=>w.IdeaId == ideainput.IdeaId);
                    if (idea != null)
                    {
                        var oldStatus = idea.StatusId;
                        idea.StatusId = (int)status; //2;// 2 for Submitted; 1 for created
                        idea.StatusDt = DateTime.Now;                            //  idea.StatusComments = Comments;
                        idea.PriorityId = ideainput.PriorityId;

                        //Filtering by UserId
                        var ideaApprover = CheckApprovers.Where(w => w.ApproverId == uid).FirstOrDefault();
                        if (ideaApprover != null && status != IMStatus.SubmitForApproval)
                        {
                            //Since Idea status is not Submit For Approval - Reset the Approver Date in IdeaApprover table
                            ideaApprover.ApprovedDate = null;
                            unitOfWork.IMIdeaApprover.Update(ideaApprover);
                            //unitOfWork.Save();
                        }

                        if (ideaApprover != null && status==IMStatus.Approved)
                        {
                            ideaApprover.ApprovedDate = DateTime.Now;
                            ideaApprover.UpdatedDt = DateTime.Now;
                            ideaApprover.UpdatedBy = uid;

                            idea.ApprovedBy = uid;
                            idea.ApprovedDt = DateTime.Now;
                            
                            unitOfWork.IMIdeaApprover.Update(ideaApprover);
                        }
                        unitOfWork.Save();

                        //check if Multiple Approvals required for
                        //Two Statuses - SubmitForReview and SubmitForApproval
                        //if (client.MultipleApprovals && (status==IMStatus.Approved))
                        if (idea.MultipleApprovals && (status == IMStatus.Approved))
                        {
                            if (CheckApprovers.Where(e => e.ApprovedDate == null).Any())
                            {
                                idea.StatusId = oldStatus;
                            }
                            else
                            {
                                idea.StatusId = (int)status;
                                idea.ApprovedDt = DateTime.Now;
                            }
                        }
                        else
                        {
                            if (status == IMStatus.Approved)
                            {
                                idea.ApprovedBy = uid;
                                idea.ApprovedDt = DateTime.Now;
                            }
                        }
                        unitOfWork.IMPostIdea.Update(idea);

                        IMIdeaStatusHistory statHistory = new IMIdeaStatusHistory();
                        statHistory.IdeaId = idea.Id;
                        //  statusHistory.Comments = idea.StatusComments==null?"":idea.StatusComments;
                        statHistory.LastUpdatedDate = DateTime.Now;
                        statHistory.CreatedDate = DateTime.Now;
                        statHistory.CreatedBy = uid;
                        statHistory.UpdatedBy = uid;
                        statHistory.OldStatusId = oldStatus;
                        //statHistory.NewStatusId = idea.StatusId;
                        statHistory.NewStatusId = (int)status;
                        statHistory.Comments = String.IsNullOrWhiteSpace(ideainput.StatusComments) ? "" : ideainput.StatusComments;
                        unitOfWork.IMIdeaStatusHistory.Insert(statHistory);


                        var ExistingApprovers = unitOfWork.IMIdeaApprover.Get().Where(e => e.IdeaId == ideainput.IdeaId).Select(e => e.ApproverId).ToList();
                        //ideaApprover.

                        var IdeaLink = Url.Action("Index", "PostIdea", new { ViewIdea = false, IdeaId = idea.Id }, "http");
                        if (status==IMStatus.SubmitForApproval)
                        {
                            foreach(var id in ideainput.SelectedApproverIds)
                            {
                                Domain.Entities.IMIdeaApprover approver = new Domain.Entities.IMIdeaApprover();
                                approver.IdeaId = ideainput.IdeaId;
                                approver.ApproverId = Convert.ToInt32(id);
                                approver.Createdby = uid;
                                approver.CreatedDt = DateTime.Now;

                                if (ExistingApprovers.Contains(approver.ApproverId) == false)
                                {
                                    unitOfWork.IMIdeaApprover.Insert(approver);
                                }
                                var UserProfiles = unitOfWork.UserProfile.GetByID(approver.ApproverId);
                                SendIdeaNotification(UserProfiles.Email, UserProfiles.FirstName + " " + UserProfiles.LastName, idea.ItemName, IMStatus.SubmitForApproval, client.ClientName, IdeaLink, ideainput.StatusComments);
                                //send Notification
                            }
                        }
                        
                        //Submitted for Revision
                        if (status == IMStatus.SubmitForRevision)
                        {
                            //send Notification 
                            var CreatorProfiles = unitOfWork.UserProfile.GetByID(idea.CreatedBy);
                            SendIdeaNotification(CreatorProfiles.Email, CreatorProfiles.FirstName + " " + CreatorProfiles.LastName, idea.ItemName, IMStatus.SubmitForRevision, client.ClientName, IdeaLink,ideainput.StatusComments);
                        }
                        else
                        {
                            var m = unitOfWork.UserProfile.GetByID(idea.CreatedBy);
                            //SendIdeaNotification(m.Email, m.FirstName + " " + m.LastName, idea.ItemName, (IMStatus)idea.StatusId, client.ClientName, IdeaLink);
                            SendIdeaNotification(m.Email, m.FirstName + " " + m.LastName, idea.ItemName, status, client.ClientName, IdeaLink, ideainput.StatusComments);
                        }

                        var oldPrioritObject = unitOfWork.IMIdeaPriorityHistory.Get().Where(w => w.IdeaId == ideainput.IdeaId).OrderByDescending(o => o.Id).FirstOrDefault();
                       // int oldpId = 0;
                        if (oldPrioritObject == null && ideainput.PriorityId!=0)
                        {
                            IMIdeaPriorityHistory priorityHistory = new IMIdeaPriorityHistory();
                            priorityHistory.OldPriorityId = 0;
                            priorityHistory.IdeaId = ideainput.IdeaId;
                            priorityHistory.CreatedBy = uid;
                            priorityHistory.CreatedDate = DateTime.Now;
                            priorityHistory.NewPriorityId = ideainput.PriorityId;
                            priorityHistory.UpdatedBy = uid;
                            priorityHistory.LastUpdatedDate = DateTime.Now;
                            priorityHistory.Comments = String.IsNullOrWhiteSpace(ideainput.StatusComments) ? "" : ideainput.StatusComments;
                            priorityHistory.IsDeleted = false;
                            unitOfWork.IMIdeaPriorityHistory.Insert(priorityHistory);
                        }
                        else
                        {
                            if (oldPrioritObject != null)
                            {
                                if (oldPrioritObject.NewPriorityId != ideainput.PriorityId && ideainput.PriorityId != 0)
                                {
                                    IMIdeaPriorityHistory priorityHistory = new IMIdeaPriorityHistory();
                                    priorityHistory.OldPriorityId = oldPrioritObject.NewPriorityId;
                                    priorityHistory.IdeaId = ideainput.IdeaId;
                                    priorityHistory.CreatedBy = uid;
                                    priorityHistory.CreatedDate = DateTime.Now;
                                    priorityHistory.NewPriorityId = ideainput.PriorityId;
                                    priorityHistory.UpdatedBy = uid;
                                    priorityHistory.LastUpdatedDate = DateTime.Now;
                                    priorityHistory.Comments = String.IsNullOrWhiteSpace(ideainput.StatusComments) ? "" : ideainput.StatusComments;
                                    priorityHistory.IsDeleted = false;
                                    unitOfWork.IMIdeaPriorityHistory.Insert(priorityHistory);
                                }
                            }
                        }

                        //if (!string.IsNullOrEmpty(ideainput.StatusComments))
                        //{
                        //    IMIdeaStatusHistory statusHistory = new IMIdeaStatusHistory();
                        //    statusHistory.IdeaId = idea.Id;
                        //    //  statusHistory.Comments = idea.StatusComments==null?"":idea.StatusComments;
                        //    statusHistory.LastUpdatedDate = DateTime.Now;
                        //    statusHistory.CreatedBy = uid;
                        //    statusHistory.OldStatusId = oldStatus;
                        //    statusHistory.NewStatusId = idea.StatusId;
                        //    statusHistory.Comments = ideainput.StatusComments == null ? "" : ideainput.StatusComments;
                        //    unitOfWork.IMIdeaStatusHistory.Insert(statusHistory);
                        //}

                        ////var listOfAdmins = unitOfWork.IMIdeaApprover.Get(g => g.IdeaId == ideaid).Select(g => g.ApproverId).ToList();
                        ////foreach (var Adid in listOfAdmins)
                        ////{
                        ////    var m1 = unitOfWork.UserProfile.GetByID(idea.CreatedBy);
                        ////    var email1 = m1.Email;
                        ////    var fullname1 = m1.FirstName + " " + m1.LastName;
                        ////   // SendIdeaNotification(email1, fullname1, idea.ItemName, (IMStatus)idea.StatusId, client.ClientName);
                        ////}
                        

                    }

                    if (idea.ActionitemId != null||idea.ActionitemId > 0)
                    {
                        var updateString = "";
                        ActionItemTracker ait1 = unitOfWork.ActionItemTracker.GetByID(idea.ActionitemId);                       
                        ait1.Comments= String.IsNullOrWhiteSpace(ideainput.StatusComments) ? "" : ideainput.StatusComments;
                        var oldaitstatus = ait1.Status;                        
                        if (status == IMStatus.Hold)
                        {
                            ait1.Status = "Hold";
                            updateString = updateString + ", Status updated from " + oldaitstatus + " to " + ait1.Status + "  ";

                        }
                        else if(status == IMStatus.Declined)
                        {
                            ait1.Status = "Cancelled";
                            updateString = updateString + ", Status updated from " + oldaitstatus + " to " + ait1.Status + "  ";

                        }
                        else
                        {
                            ait1.Status = "Defined";
                            updateString = updateString + ", Status updated from " + oldaitstatus + " to " + ait1.Status + "  ";

                        }
                        if (updateString != "")
                        {
                            ActionItemTrackerLog alog = new ActionItemTrackerLog();
                            alog.ActionItemID = ait1.Id;
                            alog.Updates = updateString;
                            alog.ClientID = ait1.ClientID;
                            alog.updateDate = DateTime.Now;
                            unitOfWork.ActionItemTrackerLog.Insert(alog);
                        }
                        unitOfWork.ActionItemTracker.Update(ait1);
                        unitOfWork.Save();
                    }
                }
                unitOfWork.Save();
            }


        }


        public static IEnumerable<SelectListItem> ListOfAdmins(int clientId)
        {
            //Client client = (Client)Session["Client"];
            Db db = new Db();
            var listOfAdmins = (from UA in db.UserApplications
                                join UP in db.UserProfiles on UA.UserId equals UP.Id
                                join ASA in db.AppStore on UA.ApplicationId equals ASA.Id
                                join UR in db.UserRoles on UA.RoleId equals UR.Id
                                where ASA.ApplicationName.ToLower() == "idea manager" && (UR.RoleName == "admin" || UR.RoleName == "sysadmin" || UR.RoleName == "implementationmanager") && UA.ClientId == clientId
                                select UP
                                            ).Distinct().ToList();
            var AdminsWithName = listOfAdmins.Select(g =>
                new SelectListItem
                {
                    Text = g.FirstName + " " + g.LastName,
                    Value = g.Id.ToString()
                }
                ).OrderBy(x => x.Text);
            //return Json(new { Result = "OK", Records = AdminsWithName });
            return AdminsWithName;
        }

        [ActionTrack]
        public ActionResult ImplementationMg(int IdeaId = 0)
        {
          try { 
            var IsAdmin = false;
            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(uid);
            var user = GetDropDownListItems.UserRoleForClient(uid, client.ClientId);
            if (user.Text.ToLower() == "admin" || user.Text.ToLower() == "sysadmin"){ IsAdmin = true; }
            ImplementationManagerModel Imm = new ImplementationManagerModel();
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Imm.ManagerList = ListOfAdmins(client.ClientId).ToList();
            var obj = unitOfWork.IMPostIdea.GetByID(IdeaId);
            if (obj != null)
            {
                Imm.IdeaId = IdeaId;
                Imm.IdeaName = obj.ItemName;
            }
            
            var IdeaAuth = IMHelper.IdeaAuthorize(client.ClientId, IdeaId);
            if (IdeaAuth && IsAdmin && (obj.StatusId == (int)IMStatus.Approved && obj.ImpStatusId == null))
            {
                
                return View("IdeaPlanning", Imm);
            }
            else
            {
                return View("IdeaNotFound");
            }
          }
          catch (Exception ex)
          {
              Log.Error("Error in IdeaApprovalController: ImplementationMg ", ex);
              throw ex;
          }
        }

        //private ActionResult IdeaNotFound()
        //{
        //    return Content("<h3>Sorry! Idea Not Found.</h3>");
        //}

        [ActionTrack]
        [HttpPost]
        public ActionResult AddImplementor(int implementor,int ideaId=0, string comment="")
        {
          try { 
            // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            var IdeaAuth = IMHelper.IdeaAuthorize(client.ClientId, ideaId);
            if (IdeaAuth)
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                if (ideaId != 0)
                {
                    var idea = unitOfWork.IMPostIdea.GetByID(ideaId);
                    int oldstatus = idea.StatusId;
                    idea.ImplementationManager = implementor;
                    idea.LastUpdatedDate = DateTime.Now;
                    idea.UpdatedBy = uid;
                    idea.StatusId = (int)IMStatus.Approved;
                    idea.ImpStatusId = (int)IMStatus.SubmitForPlanning;
                    idea.ImpStatusDt = DateTime.Now;
                    unitOfWork.IMPostIdea.Update(idea);
                    //Notification
                    var IdeaLink = Url.Action("Index", "PostIdea", new { ViewIdea = false, IdeaId = ideaId }, "http");
                    var UserProfiles = unitOfWork.UserProfile.GetByID(implementor);
                    SendIdeaNotification(UserProfiles.Email, UserProfiles.FirstName + " " + UserProfiles.LastName, idea.ItemName, IMStatus.SubmitForPlanning, client.ClientName, IdeaLink);

                    IMIdeaStatusHistory statusHistory = new IMIdeaStatusHistory();
                    statusHistory.IdeaId = ideaId;
                    //  statusHistory.Comments = idea.StatusComments==null?"":idea.StatusComments;
                    statusHistory.LastUpdatedDate = DateTime.Now;
                    statusHistory.CreatedDate = DateTime.Now;
                    statusHistory.UpdatedBy = uid;
                    statusHistory.CreatedBy = uid;
                    statusHistory.OldStatusId = oldstatus;
                    statusHistory.NewStatusId = (int)IMStatus.SubmitForPlanning;
                    statusHistory.Comments = comment;
                    unitOfWork.IMIdeaStatusHistory.Insert(statusHistory);

                    unitOfWork.Save();
                    //return RedirectToAction("Index","ViewIdea");
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Content("<h3>Sorry! Idea Not Found.</h3>");
            }
          }
          catch (Exception ex)
          {
              Log.Error("Error in IdeaApprovalController: AddImplementor ", ex);
              throw ex;
          }

        }

        public void SendIdeaNotification(string email, string fullname, string IdeaName, IMStatus status, string clientname, string IdeaLink,string StatusComments="")
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
                        body = SubmitForReviewBody(fullname, IdeaName, StatusString, clientname,IdeaLink);
                    }
                    if (status == IMStatus.SubmitForRevision)
                    {
                        StatusString = "Submitted For Revision";
                        body = SubmitForRevisionBody(fullname, IdeaName, StatusString, clientname, IdeaLink, StatusComments);//---------------Edit Required
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
                    if (status == IMStatus.Cancelled)
                    {
                        StatusString = "Cancelled";
                    }
                    // Decline Idea
                    if(status == IMStatus.Declined)
                    {
                        StatusString = "Declined";
                    }
                    //Approved Idea
                    if (status == IMStatus.Approved)
                    {
                        StatusString = "Approved";
                    }
                    body = HoldOrCancelStatusBody(fullname, IdeaName, StatusString, clientname,IdeaLink, StatusComments);
                }

                App_Start.Helper.SendEMail(mailId, Subject, body);
            }
            catch
            {

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
            body = body.Replace("{status}", status);
            body = body.Replace("{clientname}", clientname);
            body = body.Replace("{approver}", adminName);
            body = body.Replace("{idealink}", IdeaLink);

            return body;
        }

        private string SubmitForReviewBody(string ReviewerName, string Idea, string status, string clientname,string IdeaLink)
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
        private string SubmitForRevisionBody(string CreatorName, string Idea, string status, string clientname, string IdeaLink,string StatusComments="")
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
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Areas/IdeaManager/Templates/SubmittedForRevisionEmailNotification.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{username}", CreatorName);
            body = body.Replace("{idealink}", IdeaLink);
            body = body.Replace("{ideaname}", Idea);
            body = body.Replace("{clientname}", clientname);
            body = body.Replace("{status}", status);
            body = body.Replace("{Comments}", StatusComments);

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

        private string HoldOrCancelStatusBody(string username, string Idea, string status, string clientname,string IdeaLink,string StatusComments="")
        {
            int uid = (int)Session["UserId"];
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
            body = body.Replace("{Comments}", StatusComments);

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

    }
}
