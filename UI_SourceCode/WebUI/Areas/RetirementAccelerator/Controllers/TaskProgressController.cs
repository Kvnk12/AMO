using Data;
using Domain.Entities.RetirementAccelerator;
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
using WebUI.Areas.RetirementAccelerator.Models;
using WebUI.Dto;

namespace WebUI.Areas.RetirementAccelerator.Controllers
{
     public class TaskProgressController : Controller
    {
        //
        // GET: /IdeaManager/IdeaProgress/

        public ActionResult Index()
        {
            
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            ViewData["StatusList"] = wbsStatusList(client.ClientId).ToList();
            ViewData["WorkProdList"] = workProdList(client.ClientId).ToList();
            ViewData["ResourceGroupList"] = resourceGroupList(client.ClientId).ToList();
            ViewData["rolesList"] = rolesList(client.ClientId).ToList();
            return View();
            ////var  = cv.workProdList(cl.ClientId);
            ////var  = cv.(cl.ClientId);
            ////var rolesList = cv.(cl.ClientId);

        }
        public ActionResult GetApplications()
        {
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var ApplicationList = RAHelper.getApplications(client.ClientId);
            return Json(ApplicationList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Pdf_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        [HttpGet]
        public ActionResult getApplicationDetails (int ApplicationId = 0)
        {
          //  var Client = (Client)Session["Client"];
            ClientDetail Client = (ClientDetail)Session["SessionClient"];
            int phase =0;
            int stage=0;
            decimal EstEfforts =0;
            decimal ActEfforts =0;

            if (ApplicationId != 0)
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                Db db = new Db();
                var appData = unitOfWork.Application.Get(g => g.Id == ApplicationId && g.ClientId == Client.ClientId).FirstOrDefault();
                 EstEfforts = (from a in db.Applications
                                  join b in db.RATask on a.Id equals b.RAApplicationId
                               where ApplicationId == b.RAApplicationId
                               select b.EstimatedEfforts).Sum().GetValueOrDefault();

                 ActEfforts = (from a in db.Applications
                                  join b in db.RATask on a.Id equals b.RAApplicationId
                               where ApplicationId == b.RAApplicationId
                               select b.ActualEfforts).Sum().GetValueOrDefault();

                if (appData != null)
                {
                    phase = appData.RetirementPhaseId.GetValueOrDefault(0);
                    stage = appData.RetirementStageId.GetValueOrDefault(0);
                   }           

            }
            return Json(new { retirementPhase = phase, retirementStage = stage ,estimatedEfforts=EstEfforts, actualEfforts=ActEfforts},JsonRequestBehavior.AllowGet);
        }



        public IEnumerable<SelectListItem> GetRATaskProgressStatus(int Clientid)
        {
            Db db = new Db();
            var list = db.RAProjectHealth.Where(g => g.ClientId == Clientid).ToList();
            var slist = list.Select(ls => new SelectListItem
            {
                Text = ls.ProjectHealth,
                Value = ls.Id.ToString(),
                Selected = true
            });
            return slist;
        }

        public JsonResult GetProgressStatus()
        {
            Db db = new Db();
          //  var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var dbstatusList = db.RAProjectHealth.Where(g => g.ClientId == client.ClientId).ToList();
            var statusList = dbstatusList.Select(l=> new
                {
                    ProgressId = l.Id,
                    Status = l.ProjectHealth,
                    Color = l.Color,
                }
            );
            return Json(statusList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRetirementPhase()
        {
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var dbstatusList = unitOfWork.RARetirementPhase.Get(g => g.ClientId == client.ClientId).ToList();
            var statusList = dbstatusList.Select(l => new SelectListItem { Text = l.RetirementPhase, Value = l.Id.ToString() });
            return Json(statusList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetRetirementStage()
        {
          //  var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var dbstatusList = unitOfWork.RARetirementStage.Get(g => g.ClientId == client.ClientId).ToList();
            var statusList = dbstatusList.Select(l => new SelectListItem { Text = l.RetirementStage, Value = l.Id.ToString() });
            return Json(statusList, JsonRequestBehavior.AllowGet);
        }
        public virtual JsonResult Read_Tasks([DataSourceRequest] DataSourceRequest request, int ApplicationId = 0)
        {
            ModelState.Clear();
            return Json(GetAllTasks(ApplicationId).ToDataSourceResult(request),JsonRequestBehavior.AllowGet);
        }

        public static IList<RATaskModel> GetAllTasks(int AppId)
        {
            Db db = new Db();
            var result = (from task in db.RATask
                          //join status in db.RAWbsStatus on task.ProgressStatus equals status.Id into tmppst
                         // from pst in tmppst.DefaultIfEmpty()
                          where task.RAApplicationId == AppId
                          select new RATaskModel
                          {
                              TaskId = task.Id,
                              ApplicationId = task.RAApplicationId,
                              Start = task.StartDate,
                              End = task.EndDate,
                              Milestone = task.Milestone == null ? false : task.Milestone,
                              ParentId = task.ParentId,
                              PercentComplete = task.Completion,
                              Expanded=true,
                              Summary=true,
                              OrderId = task.OrderId,
                              Title = task.TaskName,
                              ProgressStatus = task.ProgressStatus,
                              WorkProdId = task.WorkProductId,
                              ResourceGroupId = task.ResourceGroupId,
                              RoleId = task.RoleId,
                              ActualEfforts = task.ActualEfforts == null ? 0 : task.ActualEfforts,
                              EstimatedEfforts = task.EstimatedEfforts == null? 0:task.EstimatedEfforts
                          }).ToList();
            return result;
        }



        public virtual JsonResult Destroy_Task([DataSourceRequest] DataSourceRequest request, RATaskModel task)
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
            var allTasks = db.RATask.Where(g => g.Id == taskId).FirstOrDefault();
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var childTasks = db.RATask.Where(t => t.ParentId == taskId).ToList();
            var preds = db.RATaskDependency.Where(w => w.PredecessorId == taskId);
            foreach(var dep in preds)
            {
                unitOfWork.RATaskDependency.Delete(dep.Id);
            }
            var succ = db.RATaskDependency.Where(w => w.SuccessorId == taskId);
            foreach (var dep in succ)
            {
                unitOfWork.RATaskDependency.Delete(dep.Id);
            }
            if(childTasks.Count()!=0)
            {
                foreach (var childTask in childTasks)
                {
                    DeleteSessionChildren(childTask.Id);
                }
            }
            unitOfWork.RATask.Delete(allTasks.Id);
            unitOfWork.Save();
        }



        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
       
        public virtual JsonResult Create_Task([DataSourceRequest] DataSourceRequest request, RATaskModel task, int ApplicationId=0)
        {
          // var Cl = (Client)Session["Client"];
            ClientDetail Cl = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            if (ModelState.IsValid && task != null)
            //if (task != null)
            {
               // Db db = new Db();
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var cv = new RATask();
                cv.RAApplicationId = task.ApplicationId;
                cv.ParentId = task.ParentId;
                cv.OrderId = task.OrderId;
                cv.TaskName = task.Title;
                cv.StartDate = task.Start;
                cv.EndDate = task.End;
                cv.Milestone = task.Milestone;
                cv.Completion = task.PercentComplete;
                cv.ProgressStatus = task.ProgressStatus;
                cv.RoleId = task.RoleId;
                cv.ResourceGroupId = task.ResourceGroupId;
                cv.WorkProductId = task.WorkProdId;
                cv.CreatedBy = uid;
                cv.CreatedDt = DateTime.Now;
                cv.UpdatedBy = uid;
                cv.UpdatedDt = DateTime.Now;
                
                unitOfWork.RATask.Insert(cv);
                
                unitOfWork.Save();
                task.TaskId = cv.Id;
                ModelState.Clear();
                // taskService.Insert(task, ModelState);
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
       
        public virtual JsonResult Update_Task([DataSourceRequest] DataSourceRequest request, RATaskModel task)
        {
            var uid = (int)Session["UserId"];
            if (ModelState.IsValid)
            //if (task != null)
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var updatingtask = unitOfWork.RATask.GetByID(task.TaskId);
                updatingtask.Milestone = task.Milestone;
                updatingtask.Completion = task.PercentComplete;
                updatingtask.EndDate = task.End;
                updatingtask.OrderId = task.OrderId;
                updatingtask.ParentId = task.ParentId;
                updatingtask.StartDate = task.Start;
                updatingtask.TaskName = task.Title;
                updatingtask.ResourceGroupId = task.ResourceGroupId;
                updatingtask.RoleId = task.RoleId;
                updatingtask.WorkProductId = task.WorkProdId;
                updatingtask.ProgressStatus = task.ProgressStatus;
                updatingtask.EstimatedEfforts = task.EstimatedEfforts;
                updatingtask.ActualEfforts = task.ActualEfforts;

                updatingtask.UpdatedBy = uid;
                updatingtask.UpdatedDt = DateTime.Now;
                unitOfWork.RATask.Update(updatingtask);
                unitOfWork.Save();
                ModelState.Clear();
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
            
        }


        public virtual JsonResult Read_Dependencies([DataSourceRequest] DataSourceRequest request, int ApplicationId = 0)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var dependency = unitOfWork.RATaskDependency.Get(w => w.RAApplicationId == ApplicationId).Select(s => new RATaskDependencyModel { DependencyID = s.Id, PredecessorID = s.PredecessorId, SuccessorID = s.SuccessorId, ApplicationId = s.RAApplicationId, Type = (DependencyType)s.type });
            return Json(dependency.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            // return null;
        }


        public virtual JsonResult Destroy_Dependency([DataSourceRequest] DataSourceRequest request, RATaskDependencyModel dependency)
        {
            if (ModelState.IsValid)
            //if (dependency != null)
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.RATaskDependency.Delete(dependency.DependencyID);
                unitOfWork.Save();
                ModelState.Clear();

            }

            return Json(new[] { dependency }.ToDataSourceResult(request, ModelState));
        }


        public virtual JsonResult Create_Dependency([DataSourceRequest] DataSourceRequest request, RATaskDependencyModel dependency, int ApplicationId = 0)
        {
            if (ModelState.IsValid)
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                RATaskDependency dbdependency = new RATaskDependency();
                dbdependency.RAApplicationId = ApplicationId;
                dbdependency.SuccessorId = dependency.SuccessorID;
                dbdependency.PredecessorId = dependency.PredecessorID;
                dbdependency.type = (int)dependency.Type;
                unitOfWork.RATaskDependency.Insert(dbdependency);
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
            var ResourcesList = (from j in db.RAResource
                                 where j.ClientId == Cid.ClientId select(new  { ID=j.Id, Name=j.Resource })).ToList();
            return Json(ResourcesList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]

        public virtual JsonResult Read_Assignments([DataSourceRequest] DataSourceRequest request, int ApplicationId = 0)
        {
            var assignments = GetAllAssignments(ApplicationId);
            return Json(assignments.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            //return null;
        }
        //Takes Longer time- as there is no ClientId nor IdeaId to filter the data..
        public static IList<RAResourceAssignmentModel> GetAllAssignments(int AppId)
        {
            Db db = new Db();
            var result = db.RATaskResource.Where(w => w.RAApplicationId == AppId).Select(task => new RAResourceAssignmentModel
            {
                ApplicationId = task.RAApplicationId,
                TaskId = task.TaskId,
                Id = task.Id,
                ResourceId = task.ResourceId,
                Units = task.Units
            }).ToList();
            return result;
        }

        //[CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]

        public virtual JsonResult Destroy_Assignment([DataSourceRequest] DataSourceRequest request, RAResourceAssignmentModel assignment)
        {
            if (ModelState.IsValid)
            //if(assignment!=null)
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.RATaskResource.Delete(assignment.Id);
                unitOfWork.Save();
                ModelState.Clear();
            }

            return Json(new[] { assignment }.ToDataSourceResult(request, ModelState));
        }
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]

        public virtual JsonResult Create_Assignment([DataSourceRequest] DataSourceRequest request, RAResourceAssignmentModel assignment, int ApplicationId = 0)
        {
            if (ModelState.IsValid)
            //if(assignment!=null && PostIdeaId!=0)
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                RATaskResource cv = new RATaskResource();
                cv.ResourceId = assignment.ResourceId;
                cv.RAApplicationId = ApplicationId;
                cv.TaskId = assignment.TaskId;
                cv.Units = assignment.Units;
                unitOfWork.RATaskResource.Insert(cv);
                unitOfWork.Save();
                assignment.Id = cv.Id;
                ModelState.Clear();
            }

            return Json(new[] { assignment }.ToDataSourceResult(request, ModelState));
        }

        public virtual JsonResult Update_Assignment([DataSourceRequest] DataSourceRequest request, RAResourceAssignmentModel assignment)
        {
            if (ModelState.IsValid)
            //if (assignment != null)
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var cv = unitOfWork.RATaskResource.GetByID(assignment.Id);
                cv.ResourceId = assignment.ResourceId;
                cv.TaskId = assignment.TaskId;
                cv.Units = assignment.Units;
                unitOfWork.RATaskResource.Update(cv);
                unitOfWork.Save();
                ModelState.Clear();
            }

            return Json(new[] { assignment }.ToDataSourceResult(request, ModelState));
        }




        //public IEnumerable<SelectListItem> GetWBS()
        public JsonResult GetWBS()
        {
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var WbsList = unitOfWork.RAWbsTemplate.Get(x => x.ClientId == client.ClientId).ToList();
            var item = WbsList.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.WBSName.ToString() });
            return Json(item,JsonRequestBehavior.AllowGet);
        }

        public JsonResult ImportWBSTemplateTask(int wbsid, int AppId)
        {
           // int orderid = 0;
            //need to delete prev tasks
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var PreviousTasks = unitOfWork.RATask.Get().Where(g => g.RAApplicationId == AppId).OrderByDescending(h=>h.Id);
            foreach(var ptsk in PreviousTasks)
            {
                DeleteSessionChildren(ptsk.Id);
            }
            ImportWBSTemplate(wbsid, AppId, null, client.ClientId);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

       // int orderid = 0;
        // private void ImportWBSTemplate(int wbsId, string taskname, int PIdeaId, int? parentid)
        private void ImportWBSTemplate(int wbsId,  int AppId, int? parentid,int clientid)
        {
            var uid = (int)Session["UserId"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

            var childitems = unitOfWork.RAWbsTemplateDetails.Get(w => w.WBSTemplateId == wbsId && w.ParentId== parentid);
            foreach (var chitem in childitems)
            {

                importRecursive(chitem.Id, AppId, null, clientid, uid,0);
            }
        }

        private void importRecursive(int wbstaskid, int AppId, int? parentid, int clientid, int uid, int orderid)
        {
           // var uid = (int)Session["UserId"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            if (parentid == null) //for root nodes only
            {
                var chitem = unitOfWork.RAWbsTemplateDetails.GetByID(wbstaskid);
                if (chitem != null)
                {
                    //orderid=0;
                    RATask ratask = new RATask();
                    ratask.RAApplicationId = AppId;
                    ratask.TaskName = chitem.Task; //taskname;
                    ratask.Milestone = true;
                    ratask.OrderId = orderid;
                    ratask.ParentId = parentid;
                    ratask.StartDate = DateTime.Now;
                    ratask.EndDate = ratask.StartDate.AddDays(1);
                    ratask.ResourceGroupId = chitem.ResourceGroupId;
                    ratask.WorkProductId = chitem.WorkProductId;
                    ratask.CreatedBy = uid;
                    ratask.UpdatedBy = uid;
                    ratask.CreatedDt = DateTime.Now;
                    ratask.UpdatedDt = DateTime.Now;
                    unitOfWork.RATask.Insert(ratask);
                    unitOfWork.Save();
                    int newteaskid = ratask.Id;
                    // ImportWBSTemplate(chitem.Id, chitem.Name, PIdeaId, newteaskid);
                    importRecursive(chitem.Id, AppId, newteaskid,clientid,uid, orderid+1);
                }
            }
            else
            {
                var chitems = unitOfWork.RAWbsTemplateDetails.Get(w => w.ParentId == wbstaskid);
                //orderid=+;
                foreach (var citem in chitems)
                {
                    
                    RATask ratask = new RATask();
                    ratask.RAApplicationId = AppId;
                    ratask.TaskName = citem.Task; //taskname;
                    ratask.Milestone = true;
                    ratask.OrderId = orderid;
                    ratask.ParentId = parentid;
                    ratask.StartDate = DateTime.Now;
                    ratask.EndDate = ratask.StartDate.AddDays(1);
                    ratask.ResourceGroupId = citem.ResourceGroupId;
                    ratask.WorkProductId = citem.WorkProductId;
                    ratask.CreatedBy = uid;
                    ratask.UpdatedBy = uid;
                    ratask.CreatedDt = DateTime.Now;
                    ratask.UpdatedDt = DateTime.Now;
                    unitOfWork.RATask.Insert(ratask);
                    unitOfWork.Save();
                    int newtaskid = ratask.Id;
                    importRecursive(citem.Id, AppId, newtaskid, clientid, uid,orderid+1);
                }
            }
        }
        

        [HttpPost]
        public ActionResult saveApplicationDetails(int AppId=0, int PhaseId=0, int StageId=0)
        {
            int uid = (int)Session["UserId"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Application idDet = new Application();
            if (AppId != 0)
            {
                idDet = unitOfWork.Application.GetByID(AppId);
                if (idDet!=null)
                {
                    if (PhaseId != 0)
                    {
                        idDet.RetirementPhaseId = PhaseId;
                    }
                    else
                    {
                        idDet.RetirementPhaseId = null;
                    }
                    if (StageId != 0)
                    {
                        idDet.RetirementStageId = StageId;
                    }
                    else
                    {
                        idDet.RetirementStageId = null;
                    }
                }
                unitOfWork.Application.Update(idDet);
                unitOfWork.Save();
            }
            
            return Json(true, JsonRequestBehavior.AllowGet);
        }


        // For Dashboard Report
        public ActionResult DashboardIndex()
        {
            ModelState.Clear();
            return View("DashboardReport");
        }





        public virtual JsonResult timeLineTasksList([DataSourceRequest] DataSourceRequest request, int PppIdeaId = 0)
        {
            return Json(timelineTasks(PppIdeaId).ToDataSourceResult(request));
        }
        //Helper To get the tasks for a Idea!
        //var result = db.IMIdeaTask.Where(e=>e.IdeaPostId == IdeaId).ToList().Select(task => new RATaskModel
        public static IList<RATaskModel> timelineTasks(int AppId)
        {
            Db db = new Db();
            var result = (from task in db.RATask
                          join status in db.IMProgressStatus on task.ProgressStatus equals status.Id into tmppst
                          from pst in tmppst.DefaultIfEmpty()
                          where task.RAApplicationId == AppId && task.Milestone == true
                          select new RATaskModel
                          {
                              TaskId = task.Id,
                              ApplicationId = task.RAApplicationId,
                              Start = task.StartDate,
                              End = task.EndDate,
                              //Milestone = task.Milestone == null ? false : task.Milestone,
                              ParentId = task.ParentId,
                              PercentComplete = task.Completion,
                              OrderId = task.OrderId,
                              //Comments = task.Comments,
                              //PriorityId = task.PriorityId,
                              Title = task.TaskName,
                              Expanded = true,
                              Summary=true,
                              //status = task.status,
                              //EstimatedEnd = task.EstimatedEnd,
                              //EstimatedStart = task.EstimatedStart,
                              ProgressStatus = task.ProgressStatus,
                              //progressstatusColor = pst.Color
                          }).ToList();
            return result;
        }


        public IEnumerable<SelectListItem> workProdList(int ClientId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var templist = unitOfWork.RAWorkProduct.Get(g => g.ClientId == ClientId);
            var returnList = templist.Select(g => new SelectListItem { Text = g.WorkProduct, Value = g.Id.ToString() });
            return returnList;
        }
        public IEnumerable<SelectListItem> resourceGroupList(int ClientId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var templist = unitOfWork.RAResourceGroup.Get(g => g.ClientId == ClientId);
            var returnList = templist.Select(g => new SelectListItem { Text = g.ResourceGroup, Value = g.Id.ToString() });
            return returnList;
        }
        public IEnumerable<SelectListItem> rolesList(int ClientId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var templist = unitOfWork.RARoles.Get(g => g.ClientId == ClientId);
            var returnList = templist.Select(g => new SelectListItem { Text = g.RoleName, Value = g.Id.ToString() });
            return returnList;
        }
        public IEnumerable<SelectListItem> wbsStatusList(int ClientId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var templist = unitOfWork.RAWbsStatus.Get(g => g.ClientId == ClientId);
            var returnList = templist.Select(g => new SelectListItem { Text = g.WbsStatus, Value = g.Id.ToString() });
            return returnList;
        }
        //public IEnumerable<SelectListItem> resourceList(int ClientId)
        //{
        //    var contextFactory = new DbContextFactory();
        //    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
        //    var templist = unitOfWork.Rare.Get(g => g.ClientId == ClientId);
        //    var returnList = templist.Select(g => new SelectListItem { Text = g.WbsStatus, Value = g.Id.ToString() });
        //    return returnList;
        //}

    }
}
