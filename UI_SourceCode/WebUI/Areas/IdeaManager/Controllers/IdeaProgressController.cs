using Domain.Entities.ClientProfileManager;
using Newtonsoft.Json;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Service;
using System.Globalization;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Data;
using Domain.Entities.IncidentNavigator;
using Kendo.Mvc.Export;
using System.IO;
using Telerik.Documents.SpreadsheetStreaming;
using WebUI.Areas.IdeaManager.Dto;
using NTTData.AMOPortal.WebUI.Filters;
using WebUI.Dto;
//using WebUI.Areas.IdeaManager.Objects;


namespace WebUI.Areas.IdeaManager.Controllers
{
    public class IdeaProgressController : Controller
    {
        //
        // GET: /IdeaManager/IdeaProgress/

        public ActionResult Index()
        {
            IdeaImplementationDates cv = new IdeaImplementationDates();
            ModelState.Clear();
            return View("Index", cv);
        }

        [HttpPost]
        public ActionResult Pdf_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        public IEnumerable<SelectListItem> GetIMStatus(int Clientid)
        {
            //var c = (Client)Session["Client"];
            Db db = new Db();
            var list = db.IMStatus.Where(g => g.ClientId == Clientid).ToList();
            var slist = list.Select(ls => new SelectListItem
            {
                Text = ls.Status,
                Value = ls.Id.ToString(),
                Selected = true
            });
            return slist;
        }
        public IEnumerable<SelectListItem> GetIMIdeaTaskPriority(int Clientid)
        {
            //var c = (Client)Session["Client"];
            Db db = new Db();
            var list = db.IMIdeaTaskPriority.ToList();// .Select(  //.Where(g => g.ClientId == Clientid).ToList();
            var slist = list.Select(ls => new SelectListItem
            {
                Text = ls.TaskPriority,
                Value = ls.Id.ToString(),
                Selected = true
            });
            return slist;
        }
        public IEnumerable<SelectListItem> GetIMTaskProgressStatus(int Clientid)
        {
            //var c = (Client)Session["Client"];
            Db db = new Db();
            var list = db.IMProgressStatus.Where(g => g.ClientId == Clientid).ToList();
            var slist = list.Select(ls => new SelectListItem
            {
                Text = ls.Status,
                Value = ls.Id.ToString(),
                Selected = true
            });
            return slist;
        }

        public JsonResult GetProgressStatus()
        {
            Db db = new Db();
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var dbstatusList = db.IMProgressStatus.Where(g => g.ClientId == client.ClientId).ToList();
            var statusList = dbstatusList.Select(l=> new
                {
                    ProgressId = l.Id,
                    Status = l.Status,
                    Color = l.Color,
                }
            );
            return Json(statusList, JsonRequestBehavior.AllowGet);
        }
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
       
        public virtual JsonResult Read_Tasks([DataSourceRequest] DataSourceRequest request,int PostIdeaId=0)
        {
            ModelState.Clear();
            return Json(GetAllTasks(PostIdeaId).ToDataSourceResult(request));
        }
        //Helper To get the tasks for a Idea!
        //var result = db.IMIdeaTask.Where(e=>e.IdeaPostId == IdeaId).ToList().Select(task => new IdeaTaskModel
        public static IList<IdeaTaskModel> GetAllTasks(int IdeaId)
        {
            Db db = new Db();
            var result = (from task in db.IMIdeaTask
                          join status in db.IMProgressStatus on task.ProgressStatus equals status.Id into tmppst
                          from pst in tmppst.DefaultIfEmpty()
                          where task.IdeaPostId== IdeaId
                          select new IdeaTaskModel
                          {
                    TaskId = task.Id,
                    IdeaPostId = task.IdeaPostId,
                    Start = task.StartDate,
                    End=task.EndDate,
                    Milestone = task.Milestone==null?false:task.Milestone,
                    ParentId = task.ParentId,
                    PercentComplete = task.Completion,
                    OrderId = task.OrderId,
                    Comments = task.Comments,
                    //PriorityId = task.PriorityId,
                    Title = task.TaskName,
                Expanded = false,
                //status = task.status,
                EstimatedEnd = task.EstimatedEnd,
                EstimatedStart = task.EstimatedStart,
                progressstatus=task.ProgressStatus,
                progressstatusColor=pst.Color
                          }).ToList(); 
            return result;
        }



        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
       
        public virtual JsonResult Destroy_Task([DataSourceRequest] DataSourceRequest request, IdeaTaskModel task)
        {
            if (ModelState.IsValid)
            {
                //  taskService.Delete(task, ModelState);
                DeleteSessionChildren(task.TaskId);
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }
        //Helpers to delete
        private static void DeleteSessionChildren(int taskId)
        {
            Db db = new Db();
            var allTasks = db.IMIdeaTask.Where(g => g.Id == taskId).FirstOrDefault();
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var childTasks = db.IMIdeaTask.Where(t => t.ParentId == taskId).ToList();
            var preds = db.IMTaskDependency.Where(w => w.PredecessorId == taskId);
            foreach(var dep in preds)
            {
                unitOfWork.IMTaskDependency.Delete(dep.Id);
            }
            var succ = db.IMTaskDependency.Where(w => w.SuccessorId == taskId);
            foreach (var dep in succ)
            {
                unitOfWork.IMTaskDependency.Delete(dep.Id);
            }
            if(childTasks.Count()!=0)
            {
                foreach (var childTask in childTasks)
                {
                    DeleteSessionChildren(childTask.Id);
                    //allTasks.Remove(childTask);
                    // unitOfWork.IMIdeaTask.Delete(childTask.Id);
                }
            }
            unitOfWork.IMIdeaTask.Delete(allTasks.Id);
            unitOfWork.Save();
        }



        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
       
        public virtual JsonResult Create_Task([DataSourceRequest] DataSourceRequest request, IdeaTaskModel task, int PostIdeaId=0)
        {
            //var Cl = (Client)Session["Client"];
            ClientDetail Cl = (ClientDetail)Session["SessionClient"];

            if (ModelState.IsValid && task != null)
            //if (task != null)
            {
               // Db db = new Db();
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                //var taskName = task.Title.Trim();
                //var CheckExisting = unitOfWork.IMIdeaTask.Get(g => g.TaskName == taskName).Any();
                var cv = new IMIdeaTask();
                cv.IdeaPostId = task.IdeaPostId;
                cv.ParentId = task.ParentId;
                cv.OrderId = task.OrderId;
                cv.TaskName = task.Title;
                cv.StartDate = task.Start;
                cv.EndDate = task.End;
                //DateTime.ParseExact(aStartdt, "MM/dd/yyyy HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
                //cv.StartDate = DateTime.Parse(task.Start.ToString(), System.Globalization.CultureInfo.CurrentCulture);
                //cv.EndDate = DateTime.Parse(task.End.ToString(), System.Globalization.CultureInfo.CurrentCulture);
                cv.Milestone = task.Milestone;
                cv.Completion = task.PercentComplete;
                cv.status = task.status;
                //cv.ProgressStatus = task.progressstatus;
                cv.Comments = task.Comments;
                cv.EstimatedStart = task.EstimatedStart;
                cv.EstimatedEnd = task.EstimatedEnd;
                //cv.PriorityId = task.PriorityId;
                unitOfWork.IMIdeaTask.Insert(cv);
                
                unitOfWork.Save();
                task.TaskId = cv.Id;
                ModelState.Clear();
                // taskService.Insert(task, ModelState);
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
       
        public virtual JsonResult Update_Task([DataSourceRequest] DataSourceRequest request, IdeaTaskModel task)
        {
            if (ModelState.IsValid)
            //if (task != null)
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var updatingtask = unitOfWork.IMIdeaTask.GetByID(task.TaskId);
                // taskService.Update(task, ModelState);
                updatingtask.Comments = task.Comments;
                updatingtask.Milestone = task.Milestone;
                updatingtask.Completion = task.PercentComplete;
                updatingtask.EndDate = task.End;
                //.EndDate = DateTime.Parse(task.End.ToString(), System.Globalization.CultureInfo.CurrentCulture); ;
                updatingtask.OrderId = task.OrderId;
                updatingtask.ParentId = task.ParentId;
                // updatingtask.PriorityId = task.PriorityId;
                updatingtask.StartDate = task.Start;
                //updatingtask.StartDate = DateTime.Parse(task.Start.ToString(), System.Globalization.CultureInfo.CurrentCulture); ;
                updatingtask.status = task.status;
                //updatingtask.ProgressStatus = task.progressstatus;
                updatingtask.TaskName = task.Title;
                updatingtask.EstimatedStart = task.EstimatedStart;
                updatingtask.EstimatedEnd = task.EstimatedEnd;
                unitOfWork.IMIdeaTask.Update(updatingtask);
                unitOfWork.Save();
                ModelState.Clear();
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));

           // return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        
        public virtual JsonResult Read_Dependencies([DataSourceRequest] DataSourceRequest request, int PostIdeaId=0)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var dependency = unitOfWork.IMTaskDependency.Get(w => w.IdeaPostId == PostIdeaId).Select(s => new IdeaTaskDependencyModel { DependencyID = s.Id, PredecessorID = s.PredecessorId, SuccessorID = s.SuccessorId, IdeaPostId = s.IdeaPostId, Type = (DependencyType)s.type });
            return Json(dependency.ToDataSourceResult(request),JsonRequestBehavior.AllowGet);
           // return null;
        }
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        
        public virtual JsonResult Destroy_Dependency([DataSourceRequest] DataSourceRequest request, IdeaTaskDependencyModel dependency)
        {
            if (ModelState.IsValid)
            //if (dependency != null)
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.IMTaskDependency.Delete(dependency.DependencyID);
                unitOfWork.Save();
                ModelState.Clear();
                //  dependencyService.Delete(dependency, ModelState);
            }

            return Json(new[] { dependency }.ToDataSourceResult(request, ModelState));
        }
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
       
        public virtual JsonResult Create_Dependency([DataSourceRequest] DataSourceRequest request, IdeaTaskDependencyModel dependency, int PostIdeaId=0)
        {
            if (ModelState.IsValid)
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                IMTaskDependency dbdependency = new IMTaskDependency();
                dbdependency.IdeaPostId = PostIdeaId;
                dbdependency.SuccessorId = dependency.SuccessorID;
                dbdependency.PredecessorId = dependency.PredecessorID;
                dbdependency.type = (int)dependency.Type;
                unitOfWork.IMTaskDependency.Insert(dbdependency);
                unitOfWork.Save();
                dependency.DependencyID = dbdependency.Id;
                ModelState.Clear();
            }

            return Json(new[] { dependency }.ToDataSourceResult(request, ModelState));
        }
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        
        public virtual JsonResult Read_Resources([DataSourceRequest] DataSourceRequest request)//, int PostIdeaId=0)
        {
           // var Cid = (Client)Session["Client"];
            ClientDetail Cid = (ClientDetail)Session["SessionClient"];

            Db db = new Db();
            var ResourcesList = (from j in db.IMResource
                                 where j.ClientId == Cid.ClientId select(new  { ID=j.Id, Name=j.ResourceName, Color="" })).ToList();
            return Json(ResourcesList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        
        public virtual JsonResult Read_Assignments([DataSourceRequest] DataSourceRequest request,int PostIdeaId=0)
        {
            var assignments = GetAllAssignments(PostIdeaId);
            return Json(assignments.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            //return null;
        }
        //Takes Longer time- as there is no ClientId nor IdeaId to filter the data..
        public static IList<IdeaResourceAssignmentModel> GetAllAssignments(int PostIdeaId)
        {
            Db db = new Db();
            var result = db.IMTaskResource.Where(w=>w.IdeaPostId== PostIdeaId).Select(task => new IdeaResourceAssignmentModel
            {
                 IdeaPostId=task.IdeaPostId,
                TaskId = task.TaskId,
                Id = task.Id,
                ResourceId = task.ResourceId,
                Units = task.Units
            }).ToList();
            return result;
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        
        public virtual JsonResult Destroy_Assignment([DataSourceRequest] DataSourceRequest request, IdeaResourceAssignmentModel assignment)
        {
            if (ModelState.IsValid)
            //if(assignment!=null)
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.IMTaskResource.Delete(assignment.Id);
                unitOfWork.Save();
                ModelState.Clear();
            }

            return Json(new[] { assignment }.ToDataSourceResult(request, ModelState));
        }
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
       
        public virtual JsonResult Create_Assignment([DataSourceRequest] DataSourceRequest request, IdeaResourceAssignmentModel assignment, int PostIdeaId=0)
        {
            if (ModelState.IsValid)
            //if(assignment!=null && PostIdeaId!=0)
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                IMTaskResource cv = new IMTaskResource();
                cv.ResourceId = assignment.ResourceId;
                cv.IdeaPostId = PostIdeaId;
                cv.TaskId = assignment.TaskId;
                cv.Units = assignment.Units;
                unitOfWork.IMTaskResource.Insert(cv);
                unitOfWork.Save();
                assignment.Id = cv.Id;
                ModelState.Clear();
            }

            return Json(new[] { assignment }.ToDataSourceResult(request, ModelState));
        }
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        
        public virtual JsonResult Update_Assignment([DataSourceRequest] DataSourceRequest request, IdeaResourceAssignmentModel assignment)
        {
            if (ModelState.IsValid)
            //if (assignment != null)
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var cv = unitOfWork.IMTaskResource.GetByID(assignment.Id);
                cv.ResourceId = assignment.ResourceId;
                cv.TaskId = assignment.TaskId;
                cv.Units = assignment.Units;
                unitOfWork.IMTaskResource.Update(cv);
                unitOfWork.Save();
                ModelState.Clear();
            }

            return Json(new[] { assignment }.ToDataSourceResult(request, ModelState));
        }

        //Helper
        //
        //public IEnumerable<SelectListItem> GetApprovedIdeas()
        public JsonResult GetApprovedIdeas()
        {
            var client = (Client)Session["Client"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var approvedIdeaList = unitOfWork.IMPostIdea.Get(x => x.ClientId == client.Id).Where(g=>g.StatusId == (int)IMStatus.Approved).ToList();
            var item = approvedIdeaList.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.ItemName.ToString() });
            return Json(item, JsonRequestBehavior.AllowGet);

        }
        //public IEnumerable<SelectListItem> GetWBS()
        public JsonResult GetWBS()
        {
          //  var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var WbsList = unitOfWork.IMWbsTemplate.Get(x => x.ClientId == client.ClientId).Where(g=>g.ParentId==null).ToList();
            var item = WbsList.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name.ToString() });
            return Json(item,JsonRequestBehavior.AllowGet);
        }

        public JsonResult ImportWBSTemplateTask(int wbsid, string wbstemplateName, int PIdeaId)
        {
            orderid = 0;
            //need to delete prev tasks
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var PreviousTasks = unitOfWork.IMIdeaTask.Get().Where(g => g.IdeaPostId == PIdeaId).OrderByDescending(h=>h.Id);
            foreach(var ptsk in PreviousTasks)
            {
                DeleteSessionChildren(ptsk.Id);
            }
            //ImportWBSTemplate(wbsid, wbstemplateName, PIdeaId, null);
            ImportWBSTemplate(wbsid, PIdeaId, null);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        int orderid = 0;
        // private void ImportWBSTemplate(int wbsId, string taskname, int PIdeaId, int? parentid)
        private void ImportWBSTemplate(int wbsId,  int PIdeaId, int? parentid)
        {

            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            
            var childitems = unitOfWork.IMWbsTemplate.Get(w => w.ParentId == wbsId);
            foreach (var chitem in childitems)
            {
                orderid++;
                IMIdeaTask ideatask = new IMIdeaTask();
                ideatask.IdeaPostId = PIdeaId;
                ideatask.TaskName = chitem.Name; //taskname;
                ideatask.Milestone = true;
                ideatask.OrderId = orderid;
                ideatask.ParentId = parentid;
                ideatask.StartDate = DateTime.Now;
                ideatask.EndDate = ideatask.StartDate.AddDays(1);
                unitOfWork.IMIdeaTask.Insert(ideatask);
                unitOfWork.Save();
                int newteaskid = ideatask.Id;
                // ImportWBSTemplate(chitem.Id, chitem.Name, PIdeaId, newteaskid);
                ImportWBSTemplate(chitem.Id, PIdeaId, newteaskid);

            }
        }


        [HttpGet]
        public JsonResult getIdeaDateDetails(int PIdeaId=0)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            IdeaImplementationDates cv = new IdeaImplementationDates();
            if (PIdeaId != 0)
            {
                var idDet = unitOfWork.IMPostIdea.GetByID(PIdeaId);
                cv.PosIdeaId = PIdeaId;
                cv.ActualEndDate = idDet.ActualEndDate;
                cv.ActualStartDate = idDet.ActualStartDate;
                cv.PlannedStartDate = idDet.PlannedStartDate;
                cv.PlannedEndDate = idDet.PlannedEndDate;
                cv.ProgressStatusId = idDet.ProgressStatus;
            }
            return Json(cv,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult saveIdeaDetails(int PosIdeaIddd, string pStartDt, string pEndDt, string aStartdt, string aEndDt, int? ProStId)
        //public ActionResult saveIdeaDetails(int PosIdeaIddd, DateTime? pStartDt, DateTime? pEndDt, DateTime? aStartdt, DateTime? aEndDt, int? ProStId)
        {
            var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            IMPostIdea idDet = new IMPostIdea();
            if (PosIdeaIddd != 0)
            {
                idDet = unitOfWork.IMPostIdea.GetByID(PosIdeaIddd);
            }
            if (idDet != null && idDet.Id!=0)
            {
                if (aStartdt!="")
                {
                    idDet.ActualStartDate = DateTime.ParseExact(aStartdt, "MM/dd/yyyy HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture); 
                }
                if (aEndDt != "")
                {
                    idDet.ActualEndDate = DateTime.ParseExact(aEndDt, "MM/dd/yyyy HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
                }
                if (pEndDt != "")
                {
                    idDet.PlannedEndDate = DateTime.ParseExact(pEndDt, "MM/dd/yyyy HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
                }
                if (pStartDt != "")
                {
                    idDet.PlannedStartDate = DateTime.ParseExact(pStartDt, "MM/dd/yyyy HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
                }
                //idDet.ActualStartDate = DateTime.ParseExact(aStartdt, "ddd MMM dd yyyy HH:mm:ss 'GMT'zzz", System.Globalization.CultureInfo.InvariantCulture);
                //idDet.ActualEndDate = DateTime.ParseExact(aEndDt, "ddd MMM dd yyyy HH:mm:ss 'GMT'zzz", System.Globalization.CultureInfo.InvariantCulture);
                //idDet.PlannedEndDate = DateTime.ParseExact(pEndDt, "ddd MMM dd yyyy HH:mm:ss 'GMT'zzz", System.Globalization.CultureInfo.InvariantCulture);
                //idDet.PlannedStartDate = DateTime.ParseExact(pStartDt, "ddd MMM dd yyyy HH:mm:ss 'GMT'zzz", System.Globalization.CultureInfo.InvariantCulture);
                idDet.ProgressStatus = ProStId;
                unitOfWork.IMPostIdea.Update(idDet);
                unitOfWork.Save();
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        //public PartialViewResult saveIdeaDetails(IdeaImplementationDates inout)
        //{
        //    var contextFactory = new DbContextFactory();
        //    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
        //    IMPostIdea idDet = new IMPostIdea();
        //    if (inout.PosIdeaId != 0)
        //    {
        //        idDet = unitOfWork.IMPostIdea.GetByID(inout.PosIdeaId);
        //    }
        //    if(idDet!=null)
        //    {
        //        idDet.ActualStartDate = inout.ActualStartDate;
        //        idDet.ActualEndDate = inout.ActualEndDate;
        //        idDet.PlannedEndDate = inout.PlannedEndDate;
        //        idDet.PlannedStartDate = inout.PlannedStartDate;
        //        idDet.ProgressStatus = inout.ProgressStatusId;
        //        unitOfWork.IMPostIdea.Update(idDet);
        //        unitOfWork.Save();
        //    }
        //    return PartialView(inout);
        //}



        // For Dashboard Report
        public ActionResult DashboardIndex()
        {
            ModelState.Clear();
            return View("DashboardReport");
        }


        public ActionResult ReportIdeaDetails(int IdeaId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();
            var ideaDat = (from PI in db.IMPostIdea
                           join IA in db.IMIdeaApprover on PI.Id equals IA.IdeaId
                           join UP in db.UserProfiles on IA.ApproverId equals UP.Id
                           join IPS in db.IMProgressStatus on PI.ProgressStatus equals IPS.Id into tempIPS
                           join IPH in db.IMIdeaPriorityHistory on PI.Id equals IPH.IdeaId /*into tempIPH*/
                           //from IPH in tempIPH.DefaultIfEmpty()
                           from IPS in tempIPS.DefaultIfEmpty()
                           where PI.Id == IdeaId && IPH.LastUpdatedDate>= PI.LastUpdatedDate
                           select new 
                           {
                               //CurrentPriority = IPH ==null? 0 :IPH.NewPriorityId,
                               //LastUpdatedPriority = IPH ==null? 0 :IPH.OldPriorityId,
                               ActualStart = PI.ActualStartDate,
                               ActualEnd = PI.ActualEndDate,
                               PlannedStart = PI.PlannedStartDate,
                               PlannedEnd = PI.PlannedEndDate,
                               Clientid = PI.ClientId,
                               Complexity =PI.Complexity,
                               CurrentPriority = IPH.NewPriorityId,
                               LastUpdatedPriority = IPH.OldPriorityId,
                               IdeaDate = IA.ApprovedDate,
                               LOBManager = UP.FirstName+" "+UP.LastName,
                               IdeaName = PI.ItemName,
                               ProgressStatus = IPS.Status,
                               ProgressStatusColor = IPS.Color,
                               Risk = PI.Risk,
                               RiskMitigation = PI.RiskMitigation
                           }
                           ).Distinct().FirstOrDefault();
            //var conList = new TrackerDashboardModel();
            //if (ideaDat != null)
            //{
            //    //CurrentPriority = IPH ==null? 0 :IPH.NewPriorityId,
            //    //LastUpdatedPriority = IPH ==null? 0 :IPH.OldPriorityId,
            //    conList.ActualStart = ideaDat.ActualStart == null ? "" : ideaDat.ActualStart.ToString();
            //    conList.ActualEnd = ideaDat.ActualEnd == null ? "" : ideaDat.ActualEnd.ToString();
            //    conList.PlannedStart = ideaDat.PlannedStart == null ? "" : ideaDat.PlannedStart.ToString();
            //    conList.PlannedEnd = ideaDat.PlannedEnd == null ? "" : ideaDat.PlannedEnd.ToString();
            //    conList.Clientid = ideaDat.Clientid==null?0:(int)ideaDat.Clientid;
            //    conList.Complexity = ideaDat.Complexity==null?"":ideaDat.Complexity;
            //    conList.CurrentPriority = ideaDat == null ? 0 : (int?)ideaDat.CurrentPriority;
            //    conList.LastUpdatedPriority = ideaDat == null ? 0 : (int?)ideaDat.LastUpdatedPriority;
            //    conList.IdeaDate = ideaDat.IdeaDate == null ? "" : ideaDat.IdeaDate.ToString();
            //    conList.LOBManager = ideaDat.LOBManager==null?"":ideaDat.LOBManager;
            //    conList.IdeaName = ideaDat.IdeaName==null?"":ideaDat.IdeaName;
            //    conList.ProgressStatus = ideaDat.ProgressStatus==null?"":ideaDat.ProgressStatus;
            //    conList.ProgressStatusColor = ideaDat.ProgressStatusColor==null?"":ideaDat.ProgressStatusColor;

            //    //Responsible = IMR.ResourceName,
            //    conList.Risk = ideaDat.Risk==null?"":ideaDat.Risk;
            //    conList.RiskMitigation = ideaDat.RiskMitigation==null?"":ideaDat.RiskMitigation;

            //}
            //var ideaData = unitOfWork.IMPostIdea.GetByID(IdeaId);
            //TrackerDashboardModel cv = new TrackerDashboardModel()
            //{
            //ActualEnd = ideaData.ActualEndDate,
            //ActualStart = ideaData.ActualStartDate,
            //Clientid = ideaData.ClientId,
            //Complexity = ideaData.Complexity,
            //CurrentPriority = ideaData.PriorityId,
            ////LastUpdatedPriority = idea
            //Id = ideaData.Id,
            //IdeaName = ideaData.ItemName,
            //IdeaDate = ideaData.CreatedDate,
            //LOBManager = ideaData.LOBId,

            //};
            if(ideaDat==null)
            {
                var ideaDat1 = new TrackerDashboardModel();
                return Json(ideaDat1);
            }
            else
            {
                var conList = new TrackerDashboardModel();
                //CurrentPriority = IPH == null ? 0 : IPH.NewPriorityId,
                //LastUpdatedPriority = IPH ==null? 0 :IPH.OldPriorityId,
                //conList.ActualStart =ideaDat.ActualStart==null? ideaDat.ActualStart: DateTime.ParseExact(ideaDat.ActualStart.ToString(), "MM/dd/yyyy HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture); 
                //conList.ActualEnd = ideaDat.ActualEnd == null ? ideaDat.ActualEnd : DateTime.ParseExact(ideaDat.ActualEnd.ToString(), "MM/dd/yyyy HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture); ;
                //conList.PlannedStart = ideaDat.PlannedStart == null ? ideaDat.PlannedStart : DateTime.ParseExact(ideaDat.PlannedStart.ToString(), "MM/dd/yyyy HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
                //conList.PlannedEnd = ideaDat.PlannedEnd == null ? ideaDat.PlannedEnd : DateTime.ParseExact(ideaDat.PlannedEnd.ToString(), "MM/dd/yyyy HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);

                conList.ActualStart = ideaDat.ActualStart == null ? "" : ideaDat.ActualStart.ToString();
                conList.ActualEnd = ideaDat.ActualEnd == null ? "" : ideaDat.ActualEnd.ToString();
                conList.PlannedStart = ideaDat.PlannedStart == null ? "" : ideaDat.PlannedStart.ToString();
                conList.PlannedEnd = ideaDat.PlannedEnd == null ? "" : ideaDat.PlannedEnd.ToString();

                conList.Clientid = ideaDat.Clientid == null ? 0 : (int)ideaDat.Clientid;
                conList.Complexity = ideaDat.Complexity == null ? "" : ideaDat.Complexity;
                conList.CurrentPriority = ideaDat.CurrentPriority.ToString();
                conList.LastUpdatedPriority = ideaDat.LastUpdatedPriority.ToString();
                //conList.CurrentPriority = ideaDat.CurrentPriority ==  ? "0" : ideaDat.CurrentPriority.ToString();
                //conList.LastUpdatedPriority = ideaDat.LastUpdatedPriority == null ? "0" : ideaDat.LastUpdatedPriority.ToString();
                conList.IdeaDate = ideaDat.IdeaDate == null ? "" : ideaDat.IdeaDate.ToString();
                conList.LOBManager = ideaDat.LOBManager == null ? "" : ideaDat.LOBManager;
                conList.IdeaName = ideaDat.IdeaName == null ? "" : ideaDat.IdeaName;
                conList.ProgressStatus = ideaDat.ProgressStatus == null ? "" : ideaDat.ProgressStatus;
                conList.ProgressStatusColor = ideaDat.ProgressStatusColor == null ? "" : ideaDat.ProgressStatusColor;

                //Responsible = IMR.ResourceName,
                conList.Risk = ideaDat.Risk == null ? "" : ideaDat.Risk;
                conList.RiskMitigation = ideaDat.RiskMitigation == null ? "" : ideaDat.RiskMitigation;
                return Json(conList);
            }

        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        [HttpPost]
        public ActionResult ReadTaskDetails([DataSourceRequest] DataSourceRequest request, int PIdeaId=0)
        {
            //var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var Db = new Db();
            var Tasks = (from IMT in Db.IMIdeaTask
                         join IS in Db.IMStatus on IMT.status equals IS.Id into tempIS
                         from IS in tempIS.DefaultIfEmpty()
                         where IMT.IdeaPostId == PIdeaId && IMT.Milestone == true
                         select new
                         {
                             T_ActualStart = IMT.StartDate,
                             T_ActualEnd = IMT.EndDate,
                             T_PlannedStart = IMT.EstimatedStart,
                             T_PlannedEnd = IMT.EstimatedEnd,
                             T_PercentCompleted = IMT.Completion,
                             T_Phase = IMT.TaskName,
                             T_Status = IS.Status
                         });
            var NewTasks = Tasks.Select(c => new TasksDetails
            {
                T_ActualStart = c.T_ActualStart,
                T_ActualEnd = c.T_ActualEnd,
                T_PlannedStart = c.T_PlannedStart,
                T_PlannedEnd = c.T_PlannedEnd,
                T_PercentCompleted = c.T_PercentCompleted,
                T_Phase = c.T_Phase,
                T_Status = c.T_Status==null?"":c.T_Status
            }).ToList();
           

            return Json(NewTasks.AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult timeLineTasksList([DataSourceRequest] DataSourceRequest request, int PppIdeaId = 0)
        {
            return Json(timelineTasks(PppIdeaId).ToDataSourceResult(request));
        }
        //Helper To get the tasks for a Idea!
        //var result = db.IMIdeaTask.Where(e=>e.IdeaPostId == IdeaId).ToList().Select(task => new IdeaTaskModel
        public static IList<IdeaTaskModel> timelineTasks(int IdeaId)
        {
            //Db db = new Db();
            //var result = (from task in db.IMIdeaTask
            //              join status in db.IMStatus on task.status equals status.Id into tmppst
            //              from pst in tmppst.DefaultIfEmpty()
            //              where task.IdeaPostId == IdeaId && task.Milestone==true
            //              select new IdeaTaskModel
            //              {
            //                  TaskId = task.Id,
            //                  IdeaPostId = task.IdeaPostId,
            //                  Start = task.StartDate,
            //                  End = task.EndDate,
            //                  Milestone = task.Milestone == null ? false : task.Milestone,
            //                  ParentId = task.ParentId,
            //                  PercentComplete = task.Completion,
            //                  OrderId = task.OrderId,
            //                  Comments = task.Comments,
            //                  //PriorityId = task.PriorityId,
            //                  Title = task.TaskName,
            //                  Expanded = false,
            //                  status = task.status,
            //                  EstimatedEnd = task.EstimatedEnd,
            //                  EstimatedStart = task.EstimatedStart,
            //                  progressstatus = task.ProgressStatus,
            //                  //progressstatusColor = pst.Color
            //              }).ToList();
            //return result;

            Db db = new Db();
            var result = (from task in db.IMIdeaTask
                          join status in db.IMProgressStatus on task.ProgressStatus equals status.Id into tmppst
                          from pst in tmppst.DefaultIfEmpty()
                          where task.IdeaPostId == IdeaId && task.Milestone == true
                          select new IdeaTaskModel
                          {
                              TaskId = task.Id,
                              IdeaPostId = task.IdeaPostId,
                              Start = task.StartDate,
                              End = task.EndDate,
                              //Milestone = task.Milestone == null ? false : task.Milestone,
                              ParentId = task.ParentId,
                              PercentComplete = task.Completion,
                              OrderId = task.OrderId,
                              Comments = task.Comments,
                              //PriorityId = task.PriorityId,
                              Title = task.TaskName,
                              Expanded = false,
                              //status = task.status,
                              //EstimatedEnd = task.EstimatedEnd,
                              //EstimatedStart = task.EstimatedStart,
                              progressstatus = task.ProgressStatus,
                              progressstatusColor = pst.Color
                          }).ToList();
            return result;
        }


    }
}
