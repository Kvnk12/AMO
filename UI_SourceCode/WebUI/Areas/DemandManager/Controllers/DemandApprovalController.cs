using Data;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.DemandManager.Dto;
using WebUI.Areas.DemandManager.Models;
using PagedList.Mvc;
using PagedList;
using WebUI.Areas.DemandManager.Object;
using System.IO;
using WebUI.Dto;

namespace WebUI.Areas.DemandManager.Controllers
{
    public class DemandApprovalController : Controller
    {
        //
        // GET: /DemandManager/ViewDemand/

        public ActionResult Index(int? page=1, string sortOrder="")
        {
           // var Cl = (Client)Session["Client"];
            ClientDetail Cl = (ClientDetail)Session["SessionClient"];

            int uid = (int)Session["UserId"];
            ViewData["Approvers"] = DMHelper.ListOfAdminsSysAdmins(Cl.ClientId);
        
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            ViewBag.Demands = String.IsNullOrEmpty(sortOrder) ? "Demand_desc" : "";
            ViewBag.Status = string.IsNullOrEmpty(sortOrder) ? "Status_desc" : "Status";
            ViewBag.Description = string.IsNullOrEmpty(sortOrder) ? "Description_desc" : "Description";
            ViewBag.Createdby = string.IsNullOrEmpty(sortOrder) ? "Createdby_desc" : "Createdby";
            ViewBag.Demandtype = string.IsNullOrEmpty(sortOrder) ? "Demandtype_desc" : "Demandtype";
            ViewBag.Demandsource = string.IsNullOrEmpty(sortOrder) ? "Demandsource_desc" : "Demandsource";
            ViewBag.LOB = string.IsNullOrEmpty(sortOrder) ? "LOB_desc" : "LOB";
            ViewBag.configitem = string.IsNullOrEmpty(sortOrder) ? "configitem_desc" : "configitem";
            var user = "";
            IList<DemandModel> x = new List<DemandModel>();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                user = unitOfWork.UserProfile.Get(g => g.Id == uid).Select(g => g.RoleId).FirstOrDefault();
            }

            if (user.ToLower() == "superuser")
            {
                x = AllPostDemand_SuperUser(client.ClientId);
            }
            else
            {
                x = AllPostDemands(client.ClientId, uid);
            }


            IPagedList<DemandModel> Demands = null;
            switch (sortOrder)
            {
                case "Demand_desc":
                    Demands = x.OrderByDescending(s => s.DemandName).ToPagedList(page ?? 1, 100);
                    break;
                case "Status_desc":
                    Demands = x.OrderByDescending(s => s.Status).ToPagedList(page ?? 1, 100);
                    break;
                case "Status":
                    Demands = x.OrderBy(s => s.Status).ToPagedList(page ?? 1, 100);
                    ViewBag.Status = "Status_desc";
                    break;
                case "Description_desc":
                    Demands = x.OrderByDescending(s => s.Description).ToPagedList(page ?? 1, 100);
                    break;
                case "Description":
                    Demands = x.OrderBy(s => s.Description).ToPagedList(page ?? 1, 100);
                    ViewBag.Description = "Description_desc";
                    break;
                case "Createdby_desc":
                    Demands = x.OrderByDescending(s => s.CreatedByUser).ToPagedList(page ?? 1, 100);
                    break;
                case "Createdby":
                    Demands = x.OrderBy(s => s.CreatedByUser).ToPagedList(page ?? 1, 100);
                    ViewBag.Createdby = "Createdby_desc";
                    break;
                case "Demandtype_desc":
                    Demands = x.OrderByDescending(s => s.DemandType).ToPagedList(page ?? 1, 100);
                    break;
                case "Demandtype":
                    Demands = x.OrderBy(s => s.DemandType).ToPagedList(page ?? 1, 100);
                    ViewBag.Demandtype = "Demandtype_desc";
                    break;
                case "Demandsource_desc":
                    Demands = x.OrderByDescending(s => s.DemandSource).ToPagedList(page ?? 1, 100);
                    break;
                case "Demandsource":
                    Demands = x.OrderBy(s => s.DemandSource).ToPagedList(page ?? 1, 100);
                    ViewBag.Demandsource = "Demandsource_desc";
                    break;
                case "LOB_desc":
                    Demands = x.OrderByDescending(s => s.LOB).ToPagedList(page ?? 1, 100);
                    break;
                case "LOB":
                    Demands = x.OrderBy(s => s.LOB).ToPagedList(page ?? 1, 100);
                    ViewBag.LOB = "LOB_desc";
                    break;
                case "configitem_desc":
                    Demands = x.OrderByDescending(s => s.ConfigItemName).ToPagedList(page ?? 1, 100);
                    break;
                case "configitem":
                    Demands = x.OrderBy(s => s.ConfigItemName).ToPagedList(page ?? 1, 100);
                    ViewBag.configitem = "configitem_desc";
                    break;

                default:
                    Demands = x.OrderBy(s => s.DemandName).ToPagedList(page ?? 1, 100);
                    break;
            }
            return View(Demands);
        }

        public static IList<DemandModel> AllPostDemand_SuperUser(int clientId)
        {

            Db db = new Db();
            var Demandmod = (from posts in db.DMPostDemand
                           join usr in db.UserProfiles on posts.CreatedBy equals usr.Id
                           // join IA in db.DMDemandApprover on posts.Id equals IA.DemandDemandId
                           join idt in db.DMDemandType on posts.DemandTypeId equals idt.Id into tmpidt
                           // join ids in db.DMDemandSource on posts.DemandSourceId equals ids.Id into tmpids
                           join idst in db.IMStatus on posts.StatusId equals idst.Id into tmpidst
                           // join app in db.Applications on posts.ConfigItem equals app.Id into tmpapp
                           join lob in db.DMLOB on posts.LOBId equals lob.Id into tmplob
                           from status in tmpidst.DefaultIfEmpty()
                           //from source in tmpids.DefaultIfEmpty()
                           from idtype in tmpidt.DefaultIfEmpty()
                               //  from apps in tmpapp.DefaultIfEmpty()
                           from lobs in tmplob.DefaultIfEmpty()
                           where posts.ClientId == clientId && posts.IsDeleted==false && (posts.StatusId == (int)DMStatus.SubmitForApproval ||
                                 posts.StatusId == (int)DMStatus.SubmitForReview || posts.StatusId == (int)DMStatus.Hold)
                           select new DemandModel
                           {
                               Demandsnumber = posts.DemandNumber,
                               IsInternalInitiative = posts.InternalInitiative,
                               ClientId = posts.ClientId,
                               //ConfigItem = posts.ConfigItem,
                               //ConfigItemName = apps.ApplicationName,
                               CreatedByUser = usr.FirstName + " " + usr.LastName,
                               CreatedBy = posts.CreatedBy,
                               CreatedDate = posts.CreatedDate,
                               Description = posts.Description,
                               DemandId = posts.Id,
                               DemandName = posts.ItemName,
                               DemandSource = posts.DemandSource,
                               DemandType = idtype.DemandType,
                               IsDeleted = posts.IsDeleted,
                               //  ItemType= posts.id
                               LOB = lobs.DemandLOB,
                               LOBId = posts.LOBId,
                               //  PriorityComments = posts.PriorityComments,
                               //StatusComments = posts.StatusComments,
                               //  PriorityId = posts.PriorityId,
                               QtyFication = posts.QtyFication,
                               Status = status.Status,
                               StatusId = posts.StatusId
                           }
                          ).ToList();
            foreach (var Demandmodel in Demandmod)
            {
                Demandmodel.SelectedApproverIds = DemandManagerHelper.GetDemandApprovers(Demandmodel.DemandId);
            }
            return Demandmod;

        }


        public static IList<DemandModel> AllPostDemands(int clientId, int uid)
        {

            Db db = new Db();
            var Demandmod = (from posts in db.DMPostDemand
                           join usr in db.UserProfiles on posts.CreatedBy equals usr.Id
                           join IA in db.DMDemandApprover on posts.Id equals IA.DemandId
                           join idt in db.DMDemandType on posts.DemandTypeId equals idt.Id into tmpidt
                           join idst in db.IMStatus on posts.StatusId equals idst.Id into tmpidst
                           //   join app in db.Applications on posts.ConfigItem equals app.Id into tmpapp
                           join lob in db.DMLOB on posts.LOBId equals lob.Id into tmplob
                           from status in tmpidst.DefaultIfEmpty()
                           //from source in tmpids.DefaultIfEmpty()
                           from idtype in tmpidt.DefaultIfEmpty()
                               //  from apps in tmpapp.DefaultIfEmpty()
                           from lobs in tmplob.DefaultIfEmpty()
                           where  posts.ClientId == clientId && posts.IsDeleted==false && IA.ApproverId==uid
                                            && (posts.StatusId == (int)DMStatus.SubmitForApproval ||
                                                posts.StatusId == (int)DMStatus.SubmitForReview || 
                                                posts.StatusId == (int)DMStatus.Hold)
                           select new DemandModel
                           {
                               Demandsnumber = posts.DemandNumber,
                               ClientId = posts.ClientId,
                               IsInternalInitiative = posts.InternalInitiative,
                               // ConfigItem = posts.ConfigItem,
                               //   ConfigItemName = apps.ApplicationName,
                               CreatedByUser = usr.FirstName + " " + usr.LastName,
                               CreatedBy = posts.CreatedBy,
                               CreatedDate = posts.CreatedDate,
                               Description = posts.Description,
                               DemandId = posts.Id,
                               DemandName = posts.ItemName,
                               DemandSource = posts.DemandSource,
                               DemandType = idtype.DemandType,
                               IsDeleted = posts.IsDeleted,
                               //  ItemType= posts.id
                               LOB = lobs.DemandLOB,
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
            foreach (var Demandmodel in Demandmod)
            {
                Demandmodel.SelectedApproverIds = DemandManagerHelper.GetDemandApprovers(Demandmodel.DemandId);
            }

            return Demandmod;

        }

        public ActionResult ApproveDemands(IList<DemandModel> inputs, string btnSubmit)
        {
            int uid = (int)Session["UserId"];
            //string[] ;
           // var Cl = (Client)Session["Client"];
            ClientDetail Cl = (ClientDetail)Session["SessionClient"];
            ModelState.Clear();
            bool checkboxchecked = false;
            int chkcount = 0;
            int DemandSelectedForApproval = 0;
            foreach (var c in inputs)
            {
                if (c.IsChecked)
                {
                    checkboxchecked = true;
                    chkcount++;
                    DemandSelectedForApproval = c.DemandId;
                }

                if (c.IsChecked == true && btnSubmit=="Approve" && c.PriorityId <= 0)
                {
                    ModelState.AddModelError(c.DemandId + "mandate", c.DemandName + "- Priority is mandatory.");
                    checkboxchecked = true;
                }

                if (c.IsChecked == true && (string.IsNullOrEmpty(c.StatusComments) && (btnSubmit == "Cancel" || btnSubmit == "Decline" || btnSubmit == "Hold" || btnSubmit == "Revise")))
                {
                    ModelState.AddModelError(c.DemandId + "mandate", c.DemandName + "- Approval comments are mandatory.");
                    checkboxchecked = true;
                }
            }
            if (!checkboxchecked)
            {
                ModelState.AddModelError("chkerr", "Please select any one of the Demands");
            }
            if (chkcount>1 && btnSubmit == "Approve")
            {
                ModelState.AddModelError("chkerr", "Please select only one Demand to approve.");
            }
            if (!ModelState.IsValid)
            {
                ViewData["Approvers"] = ViewData["Approvers"] = DMHelper.ListOfAdminsSysAdmins(Cl.ClientId);
                return View("Index", inputs.ToPagedList(1, 100));
            }
            
            if (ModelState.IsValid)
            {
                DMStatus DemandStatus = DMStatus.SubmitForApproval;
                if (btnSubmit == "Approve")
                {
                    DemandStatus = DMStatus.Approved;
                }
                else if (btnSubmit == "Close")
                {
                    DemandStatus = DMStatus.Cancelled;
                }
                else if (btnSubmit == "Decline")
                {
                    DemandStatus = DMStatus.Declined;
                }
                else if (btnSubmit == "Hold")
                {
                    DemandStatus = DMStatus.Hold;
                }
                else if (btnSubmit == "Revise")
                {
                    DemandStatus = DMStatus.SubmitForRevision;

                }

                foreach (var Demand in inputs)
                {
                    if (Demand.IsChecked == true)
                    {

                        UpdateDemandStatus(Demand, DemandStatus);
                    }
                }
                TempData.Add("ValidationSuccessMsg", "Status got updated successfully.");
            }

            //return View("DemandsApproved", inputs);
            if (btnSubmit == "Approve" && !checkAllApproved(DemandSelectedForApproval))
                return RedirectToAction("ImplementationMg", new { DemandId = DemandSelectedForApproval });
            else
                return RedirectToAction("Index");
        }
        private bool checkAllApproved (int DemandId)
        {
            var ret = true;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            //True if any one of Approver has not Approved, False if all approvers has approved
            ret = unitOfWork.DMDemandApprover.Get().Where(w => w.DemandId == DemandId && w.ApprovedDate == null).Any();
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Demandinput"></param>
        /// <param name="status"></param>
        /// <param name="Reset">To Reset all the Approved Data who and all has Approved for that Demand(Status - Other than Approval/Approve)</param>
        public void UpdateDemandStatus(DemandModel Demandinput, DMStatus status)//, string Comments, int priority)
        {
            int uid = (int)Session["UserId"];
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                {
                    var Demand = unitOfWork.DMPostDemand.GetByID(Demandinput.DemandId);
                    //getting exixting approvers for Demand
                    var CheckApprovers = unitOfWork.DMDemandApprover.Get().Where(w=>w.DemandId == Demandinput.DemandId);
                    if (Demand != null)
                    {
                        var oldStatus = Demand.StatusId;
                        Demand.StatusId = (int)status; //2;// 2 for Submitted; 1 for created
                        Demand.StatusDt = DateTime.Now;                            //  Demand.StatusComments = Comments;
                        Demand.PriorityId = Demandinput.PriorityId;

                        //Filtering by UserId
                        var DemandApprover = CheckApprovers.Where(w => w.ApproverId == uid).FirstOrDefault();
                        if (DemandApprover != null && status != DMStatus.SubmitForApproval)
                        {
                            //Since Demand status is not Submit For Approval - Reset the Approver Date in DemandApprover table
                            DemandApprover.ApprovedDate = null;
                            unitOfWork.DMDemandApprover.Update(DemandApprover);
                            //unitOfWork.Save();
                        }

                        if (DemandApprover != null && status==DMStatus.Approved)
                        {
                            DemandApprover.ApprovedDate = DateTime.Now;
                            DemandApprover.UpdatedDt = DateTime.Now;
                            DemandApprover.UpdatedBy = uid;

                            Demand.ApprovedBy = uid;
                            Demand.ApprovedDt = DateTime.Now;
                            
                            unitOfWork.DMDemandApprover.Update(DemandApprover);
                        }
                        unitOfWork.Save();

                        //check if Multiple Approvals required for
                        //Two Statuses - SubmitForReview and SubmitForApproval
                        if (Demand.MultipleApprovals && (status == DMStatus.Approved))
                        {
                            if (CheckApprovers.Where(e => e.ApprovedDate == null).Any())
                            {
                                Demand.StatusId = oldStatus;
                            }
                            else
                            {
                                Demand.StatusId = (int)status;
                                Demand.ApprovedDt = DateTime.Now;
                            }
                        }
                        else
                        {
                            if (status == DMStatus.Approved)
                            {
                                Demand.ApprovedBy = uid;
                                Demand.ApprovedDt = DateTime.Now;
                            }
                        }
                        unitOfWork.DMPostDemand.Update(Demand);

                        DMStatusHistory statHistory = new DMStatusHistory();
                        statHistory.DemandId = Demand.Id;
                        //  statusHistory.Comments = Demand.StatusComments==null?"":Demand.StatusComments;
                        statHistory.LastUpdatedDate = DateTime.Now;
                        statHistory.CreatedDate = DateTime.Now;
                        statHistory.CreatedBy = uid;
                        statHistory.UpdatedBy = uid;
                        statHistory.ClientId = client.ClientId;
                        statHistory.OldStatusId = oldStatus;
                        //statHistory.NewStatusId = Demand.StatusId;
                        statHistory.NewStatusId = (int)status;
                        statHistory.Comments = String.IsNullOrWhiteSpace(Demandinput.StatusComments) ? "" : Demandinput.StatusComments;
                        unitOfWork.DMStatusHistory.Insert(statHistory);


                        var ExistingApprovers = unitOfWork.DMDemandApprover.Get().Where(e => e.DemandId == Demandinput.DemandId).Select(e => e.ApproverId).ToList();
                        //DemandApprover.

                        var DemandLink = Url.Action("Index", "PostDemand", new { ViewDemand = false, DemandId = Demand.Id }, "http");
                        if (status==DMStatus.SubmitForApproval)
                        {
                            foreach(var id in Demandinput.SelectedApproverIds)
                            {
                                Domain.Entities.DMDemandApprover approver = new Domain.Entities.DMDemandApprover();
                                approver.DemandId = Demandinput.DemandId;
                                approver.ApproverId = Convert.ToInt32(id);
                                approver.Createdby = uid;
                                approver.CreatedDt = DateTime.Now;
                                approver.ClientId = client.ClientId;
                                if (ExistingApprovers.Contains(approver.ApproverId) == false)
                                {
                                    unitOfWork.DMDemandApprover.Insert(approver);
                                }
                                var UserProfiles = unitOfWork.UserProfile.GetByID(approver.ApproverId);
                                SendDemandNotification(UserProfiles.Email, UserProfiles.FirstName + " " + UserProfiles.LastName, Demand.ItemName, DMStatus.SubmitForApproval, client.ClientName, DemandLink);
                                //send Notification
                            }
                        }
                        
                        //Submitted for Revision
                        if (status == DMStatus.SubmitForRevision)
                        {
                            //send Notification 
                            var CreatorProfiles = unitOfWork.UserProfile.GetByID(Demand.CreatedBy);
                            SendDemandNotification(CreatorProfiles.Email, CreatorProfiles.FirstName + " " + CreatorProfiles.LastName, Demand.ItemName, DMStatus.SubmitForRevision, client.ClientName, DemandLink);
                        }
                        else
                        {
                            var m = unitOfWork.UserProfile.GetByID(Demand.CreatedBy);
                            SendDemandNotification(m.Email, m.FirstName + " " + m.LastName, Demand.ItemName, status, client.ClientName, DemandLink);
                        }

                        var oldPrioritObject = unitOfWork.DMPriorityHistory.Get().Where(w => w.DemandId == Demandinput.DemandId).OrderByDescending(o => o.Id).FirstOrDefault();
                       // int oldpId = 0;
                        if (oldPrioritObject == null && Demandinput.PriorityId!=0)
                        {
                            DMPriorityHistory priorityHistory = new DMPriorityHistory();
                            priorityHistory.OldPriorityId = 0;
                            priorityHistory.DemandId = Demandinput.DemandId;
                            priorityHistory.CreatedBy = uid;
                            priorityHistory.CreatedDate = DateTime.Now;
                            priorityHistory.NewPriorityId = Demandinput.PriorityId;
                            priorityHistory.UpdatedBy = uid;
                            priorityHistory.ClientId = client.ClientId;
                            priorityHistory.LastUpdatedDate = DateTime.Now;
                            priorityHistory.Comments = String.IsNullOrWhiteSpace(Demandinput.StatusComments) ? "" : Demandinput.StatusComments;
                            priorityHistory.IsDeleted = false;
                            unitOfWork.DMPriorityHistory.Insert(priorityHistory);
                        }
                        else
                        {
                            if (oldPrioritObject != null)
                            {
                                if (oldPrioritObject.NewPriorityId != Demandinput.PriorityId && Demandinput.PriorityId != 0)
                                {
                                    DMPriorityHistory priorityHistory = new DMPriorityHistory();
                                    priorityHistory.OldPriorityId = oldPrioritObject.NewPriorityId;
                                    priorityHistory.DemandId = Demandinput.DemandId;
                                    priorityHistory.CreatedBy = uid;
                                    priorityHistory.CreatedDate = DateTime.Now;
                                    priorityHistory.NewPriorityId = Demandinput.PriorityId;
                                    priorityHistory.UpdatedBy = uid;
                                    priorityHistory.ClientId = client.ClientId;
                                    priorityHistory.LastUpdatedDate = DateTime.Now;
                                    priorityHistory.Comments = String.IsNullOrWhiteSpace(Demandinput.StatusComments) ? "" : Demandinput.StatusComments;
                                    priorityHistory.IsDeleted = false;
                                    unitOfWork.DMPriorityHistory.Insert(priorityHistory);
                                }
                            }
                        }
                        
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
                                where ASA.ApplicationName.ToLower() == "demand manager" && (UR.RoleName == "admin" || UR.RoleName == "sysadmin" || UR.RoleName == "implementationmanager") && UA.ClientId == clientId
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


        public ActionResult ImplementationMg(int DemandId = 0)
        {
            var IsAdmin = false;
            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            var user = GetDropDownListItems.UserRoleForClient(uid, client.ClientId);
            if (user.Text.ToLower() == "admin" || user.Text.ToLower() == "sysadmin"){ IsAdmin = true; }
            DemandImplementationManagerModel Imm = new DemandImplementationManagerModel();
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Imm.ManagerList = ListOfAdmins(client.ClientId).ToList();
            var obj = unitOfWork.DMPostDemand.GetByID(DemandId);
            if (obj != null)
            {
                Imm.DemandId = DemandId;
                Imm.DemandName = obj.ItemName;
            }
            
            var DemandAuth = DMHelper.DemandAuthorize(client.ClientId, DemandId);
            if (DemandAuth && IsAdmin && (obj.StatusId == (int)DMStatus.Approved && obj.ImpStatusId == null))
            {
                
                return View("DemandPlanning", Imm);
            }
            else
            {
                return View("DemandNotFound");
            }
        }

        [HttpPost]
        public ActionResult AddImplementor(int implementor,int DemandId = 0, string comment="")
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            var DemandAuth = DMHelper.DemandAuthorize(client.ClientId, DemandId);
            if (DemandAuth)
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                if (DemandId != 0)
                {
                    var Demand = unitOfWork.DMPostDemand.GetByID(DemandId);
                    int oldstatus = Demand.StatusId;
                    Demand.ImplementationManager = implementor;
                    Demand.LastUpdatedDate = DateTime.Now;
                    Demand.UpdatedBy = uid;
                    Demand.StatusId = (int)DMStatus.Approved;
                    Demand.ImpStatusId = (int)DMStatus.SubmitForPlanning;
                    Demand.ImpStatusDt = DateTime.Now;
                    unitOfWork.DMPostDemand.Update(Demand);
                    //Notification
                    var DemandLink = Url.Action("Index", "PostDemand", new { ViewDemand = false, DemandId = DemandId }, "http");
                    var UserProfiles = unitOfWork.UserProfile.GetByID(implementor);
                    SendDemandNotification(UserProfiles.Email, UserProfiles.FirstName + " " + UserProfiles.LastName, Demand.ItemName, DMStatus.SubmitForPlanning, client.ClientName, DemandLink);

                    DMStatusHistory statusHistory = new DMStatusHistory();
                    statusHistory.DemandId = DemandId;
                    statusHistory.LastUpdatedDate = DateTime.Now;
                    statusHistory.CreatedDate = DateTime.Now;
                    statusHistory.UpdatedBy = uid;
                    statusHistory.CreatedBy = uid;
                    statusHistory.OldStatusId = oldstatus;
                    client.ClientId = client.ClientId;
                    statusHistory.NewStatusId = (int)DMStatus.SubmitForPlanning;
                    statusHistory.Comments = comment;
                    unitOfWork.DMStatusHistory.Insert(statusHistory);

                    unitOfWork.Save();
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Content("<h3>Sorry! Demand Not Found.</h3>");
            }
                        
        }

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
                        body = SubmitForRevisionBody(fullname, DemandName, StatusString, clientname, DemandLink);//---------------Edit Required
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
                    // Decline Demand
                    if (status == DMStatus.Declined)
                    {
                        StatusString = "Declined";
                    }
                    //Approved Demand
                    if (status == DMStatus.Approved)
                    {
                        StatusString = "Approved";
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
                username = som.FirstName + som.LastName;
            }

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Areas/DemandManager/Templates/SubmittedForApprovalEmailNotification.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{username}", username);
            body = body.Replace("{Demandname}", Demand);
            body = body.Replace("{status}", status);
            body = body.Replace("{clientname}", clientname);
            body = body.Replace("{approver}", adminName);
            body = body.Replace("{Demandlink}", DemandLink);

            return body;
        }

        private string SubmitForReviewBody(string ReviewerName, string Demand, string status, string clientname,string DemandLink)
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
        private string SubmitForRevisionBody(string CreatorName, string Demand, string status, string clientname, string DemandLink)
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
            using (StreamReader reader = new StreamReader(Server.MapPath("~/Areas/DemandManager/Templates/SubmittedForRevisionEmailNotification.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{username}", CreatorName);
            body = body.Replace("{Demandlink}", DemandLink);
            body = body.Replace("{Demandname}", Demand);
            body = body.Replace("{clientname}", clientname);
            body = body.Replace("{status}", status);

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


        private string HoldOrCancelStatusBody(string username, string Demand, string status, string clientname,string DemandLink)
        {
            int uid = (int)Session["UserId"];
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

    }
}
