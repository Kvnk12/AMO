using Data;
using Domain.Entities;
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
using WebUI.Areas.ClientProfileManager;
using WebUI.Areas.IdeaManager.Dto;
using WebUI.Areas.IdeaManager.Models;
using WebUI.Areas.IdeaManager.Objects;
using WebUI.Dto;

namespace WebUI.Areas.IdeaManager.Controllers
{
    public class ConfigController : Controller
    {
        //
        // GET: /IdeaManager/Config/

        public ActionResult Index()
        {
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);
            return View("Phase_Status");
        }

        
             public ActionResult Index1()
        {
            return View("Index");
        }

        public ActionResult Complexity1()
        {
            var client = (ClientDetail)Session["SessionClient"];
            AuthoriseRolesModel cv = new AuthoriseRolesModel();
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);
            cv.IMAutoApproval = IMHelper.isAutoApproved(client.ClientId);
            return View("Complexity",cv);
        }

        public ActionResult ClassificationCategory()
        {
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);
            return View("ClassificationCategory");
        }

    
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        [HttpPost]
        public ActionResult readProgressStatus([DataSourceRequest] DataSourceRequest request)
        {
            //var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];

            var Db = new Db();
            var progessStatus = (from PS in Db.IMProgressStatus
                                 where PS.ClientId == client.ClientId
                                 select PS);

            return Json(progessStatus.AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }



        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddProgressStatus([DataSourceRequest] DataSourceRequest request, IMProgressStatusModel imProgressStatus)
        {
            if (imProgressStatus != null && ModelState.IsValid)
            {
                var uid = (int)Session["UserId"];
               // var client = (Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];

                imProgressStatus.ClientId = client.ClientId;
                imProgressStatus.CreatedBy = uid;
                imProgressStatus.UpdatedBy = uid;
                var res = add_ps(imProgressStatus);
                imProgressStatus.Id = res;
            }
            return Json(new[] { imProgressStatus }.ToDataSourceResult(request, ModelState));
        }

        public static int add_ps(IMProgressStatusModel imProgressStatus)
        {
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    IMProgressStatus cv = new IMProgressStatus();
                    cv.ClientId = imProgressStatus.ClientId;
                    cv.CreatedBy = imProgressStatus.CreatedBy;
                    cv.UpdatedBy = imProgressStatus.UpdatedBy;
                    cv.Color = imProgressStatus.Color;
                    cv.Status = imProgressStatus.Status;
                    cv.CreatedDt = DateTime.Now;
                    cv.UpdatedDt = DateTime.Now;
                    unitOfWork.IMProgressStatus.Insert(cv);
                    unitOfWork.Save();
                    id = imProgressStatus.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        [HttpPost]
        public ActionResult UpdateProgressStatus([DataSourceRequest] DataSourceRequest request, IMProgressStatusModel imProgressStatus)
        {
            if (imProgressStatus != null && ModelState.IsValid)
            {
                var uid = (int)Session["UserId"];
                imProgressStatus.UpdatedBy = uid;
                var res = update_ps(imProgressStatus);
            }
            return Json(new[] { imProgressStatus }.ToDataSourceResult(request, ModelState));
        }

        public static bool update_ps(IMProgressStatusModel imProgressStatus)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var x = unitOfWork.IMProgressStatus.GetByID(imProgressStatus.Id);
                    x.Status = imProgressStatus.Status;
                    x.Color = imProgressStatus.Color;
                    x.UpdatedBy = imProgressStatus.UpdatedBy;
                    x.UpdatedDt= DateTime.Now;
                    unitOfWork.IMProgressStatus.Update(x);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteProgressStatus([DataSourceRequest] DataSourceRequest request, IMProgressStatusModel imProgressStatus)
        {
            try
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.IMProgressStatus.Delete(imProgressStatus.Id);
                unitOfWork.Save();
                return Json(new[] { imProgressStatus }.ToDataSourceResult(request, ModelState));
            }
            catch(Exception)
            {
                return Json(new[] { imProgressStatus }.ToDataSourceResult(request, ModelState));
            }
        }

        //-------------------------
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        [HttpPost]
        public ActionResult readStatus([DataSourceRequest] DataSourceRequest request)
        {
            //var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];

            var Db = new Db();
            var Statuss = (from PS in Db.IMStatus
                                 where PS.ClientId == client.ClientId
                                 select PS);

            return Json(Statuss.AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        [HttpPost]
        public ActionResult AddStatus([DataSourceRequest] DataSourceRequest request, IMStatusModel imStatus)
        {
            if (imStatus != null && ModelState.IsValid)
            {
                var uid = (int)Session["UserId"];
                //var client = (Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];

                imStatus.ClientId = client.ClientId;
                imStatus.CreatedBy = uid;
                imStatus.UpdatedBy = uid;
                var res = add_s(imStatus);
                imStatus.Id = res;
            }
            return Json(new[] { imStatus }.ToDataSourceResult(request, ModelState));
        }
        public static int add_s(IMStatusModel imStatus)
        {
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    IMStatuss cv = new IMStatuss();
                    cv.Status = imStatus.Status;
                    cv.ClientId = imStatus.ClientId;
                    cv.CreatedBy = imStatus.CreatedBy;
                    cv.UpdatedBy = imStatus.UpdatedBy;
                    cv.CreatedDt = DateTime.Now;
                    cv.UpdatedDt = DateTime.Now;
                    unitOfWork.IMStatus.Insert(cv);
                    unitOfWork.Save();
                    id = imStatus.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        [HttpPost]
        public ActionResult UpdateStatus([DataSourceRequest] DataSourceRequest request, IMStatusModel imStatus)
        {
            if (imStatus != null && ModelState.IsValid)
            {
                var uid = (int)Session["UserId"];
                imStatus.UpdatedBy = uid;
                var res = update_s(imStatus);
            }
            return Json(new[] { imStatus }.ToDataSourceResult(request, ModelState));
        }
        public static bool update_s(IMStatusModel imStatus)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var x = unitOfWork.IMStatus.GetByID(imStatus.Id);
                    x.Status = imStatus.Status;
                    x.UpdatedBy = imStatus.UpdatedBy;
                    x.UpdatedDt = DateTime.Now;
                    unitOfWork.IMStatus.Update(x);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        [HttpPost]
        public ActionResult DeleteStatus([DataSourceRequest] DataSourceRequest request, IMStatuss imStatus)
        {
            try
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.IMStatus.Delete(imStatus.Id);
                unitOfWork.Save();
                return Json(new[] { imStatus }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception)
            {
                return Json(new[] { imStatus }.ToDataSourceResult(request, ModelState));
            }
        }

        

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        [HttpPost]
        public ActionResult GetRole([DataSourceRequest] DataSourceRequest request)
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];

            int usid = (int)Session["UserId"];
            int ClientId = client.ClientId;
            return Json(IdeaManagerHelper.GetRoleDetails(ClientId, usid).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        [HttpPost]
        public ActionResult AddRole(IMRoleModel imrolemodel, int Id)
        {

            if (imrolemodel != null && ModelState.IsValid)
            {
                //var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];

                int usid = (int)Session["UserId"];
                imrolemodel.ClientId = Client.ClientId;
                imrolemodel.CreatedBy = usid;
                imrolemodel.UpdatedBy = usid;

                var res = IdeaManagerHelper.CreateRole(imrolemodel, Id);
                imrolemodel.Id = res;
            }
            return Json(new[] { imrolemodel }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        [HttpPost]
        public ActionResult EditRole([DataSourceRequest] DataSourceRequest request, IMRoleModel imrolemodel)
        {
            if (imrolemodel != null && ModelState.IsValid)
            {
                //var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];

                int usid = (int)Session["UserId"];
                imrolemodel.ClientId = Client.ClientId;
                imrolemodel.UpdatedBy = usid;
                var res = IdeaManagerHelper.UpdateRole(imrolemodel);
            }
            return Json(new[] { imrolemodel }.ToDataSourceResult(request, ModelState));
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        [HttpPost]
        public ActionResult DeleteRole([DataSourceRequest] DataSourceRequest request, IMRoleModel imrolemodel)
        {
            if (imrolemodel != null)
            {
                var v = IdeaManagerHelper.RemoveRole(imrolemodel.Id);
            }
            return Json(new[] { imrolemodel }.ToDataSourceResult(request, ModelState));
        }



        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        [HttpPost]
        public ActionResult GetResource([DataSourceRequest] DataSourceRequest request)
        {
            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            int ClientId = client.ClientId;
            return Json(IdeaManagerHelper.GetResourceDetails(ClientId, usid).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddResource(IMResourceResponseModel imResourceResponseModel, int Id)
        {

            if (imResourceResponseModel != null && ModelState.IsValid)
            {
                // var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];

                int usid = (int)Session["UserId"];
                imResourceResponseModel.ClientId = Client.ClientId;
                imResourceResponseModel.CreatedBy = usid;
                imResourceResponseModel.UpdatedBy = usid;

                var res = IdeaManagerHelper.CreateResource(imResourceResponseModel, Id);
                imResourceResponseModel.Id = res;
            }
            return Json(new[] { imResourceResponseModel }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditResource([DataSourceRequest] DataSourceRequest request, IMResourceResponseModel imResourceResponseModel)
        {
            if (imResourceResponseModel != null && ModelState.IsValid)
            {
                //var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                imResourceResponseModel.ClientId = Client.ClientId;
                imResourceResponseModel.UpdatedBy = usid;
                var res = IdeaManagerHelper.UpdateResource(imResourceResponseModel);
            }
            return Json(new[] { imResourceResponseModel }.ToDataSourceResult(request, ModelState));
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteResource([DataSourceRequest] DataSourceRequest request, IMResourceResponseModel imResourceResponseModel)
        {
            if (imResourceResponseModel != null)
            {
                var v = IdeaManagerHelper.RemoveResource(imResourceResponseModel.Id);
            }
            return Json(new[] { imResourceResponseModel }.ToDataSourceResult(request, ModelState));
        }



       
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        [HttpPost]
        public JsonResult GetAllTemplates([DataSourceRequest] DataSourceRequest request)
        {
            // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var result = GetAll(client.ClientId).ToTreeDataSourceResult(request,
                e => e.TaskId,
                e => e.TParentId,
                e => e
            );

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //Helper Method to Get all templates for a Client_Id       
        private static IEnumerable<WBSTemplateModel> GetAll(int clientId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var templatesList = unitOfWork.IMWbsTemplate.Get(g => g.ClientId == clientId);
            var templatesList_1 = templatesList.Select(c => new WBSTemplateModel
            {
                TaskId = c.Id,
                Name = c.Name,
                ClientId = c.ClientId,
                TParentId = c.ParentId,
                CreatedBy = c.CreatedBy,
                CreatedDt = c.CreatedDt,
                UpdatedBy = c.UpdatedBy,
                UpdatedDt = c.UpdatedDt
            });
            return templatesList_1;
        }
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        [HttpPost]
        public JsonResult Destroy([DataSourceRequest] DataSourceRequest request, WBSTemplateModel template)
        {
            if (ModelState.IsValid && template != null)
            {
                WBS_Delete(template);
            }

            return Json(new[] { template }.ToTreeDataSourceResult(request, ModelState));
        }
        //Helper to Delete the WBS_Template/Phase/Progress
        public static void WBS_Delete(WBSTemplateModel template)
        {
            if (template.TaskId > 0)
            {               
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.IMWbsTemplate.Delete(template.TaskId);
                unitOfWork.Save();
            }
        }
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        [HttpPost]
        public JsonResult Create([DataSourceRequest] DataSourceRequest request, WBSTemplateModel template)
        {
            var uid = (int)Session["UserId"];
            //var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            if (ModelState.IsValid)
            {
                template.ClientId = client.ClientId;
                template.CreatedBy = uid;
                template.UpdatedBy = uid;
                var something = WBS_Insert(template);
                template.TaskId = something;
            }

            return Json(new[] { template }.ToTreeDataSourceResult(request, ModelState));
        }
        //Helper to Create New Template/Task
        public static int WBS_Insert(WBSTemplateModel template)
        {
            var NewId = 0;
            if (template != null)
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var cv = new IMWbsTemplate();
                cv.Name = template.Name;
                cv.ClientId = template.ClientId;
                cv.ParentId = template.TParentId;
                cv.CreatedBy = template.CreatedBy;
                cv.UpdatedBy = template.UpdatedBy;
                cv.CreatedDt = DateTime.Now;
                cv.UpdatedDt = DateTime.Now;
                unitOfWork.IMWbsTemplate.Insert(cv);
                unitOfWork.Save();
                NewId = cv.Id;
            }
            return NewId;
        }
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        [HttpPost]
        public JsonResult Update([DataSourceRequest] DataSourceRequest request, WBSTemplateModel template)
        {
            var uid = (int)Session["UserId"];
            template.UpdatedBy = uid;
            if (ModelState.IsValid)
            {
                WBS_Update(template);
            }

            return Json(new[] { template }.ToTreeDataSourceResult(request, ModelState));
        }
        //Helper to Create New Template/Task
        public static void WBS_Update(WBSTemplateModel template)
        {
            if (template != null)
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var cv = unitOfWork.IMWbsTemplate.GetByID(template.TaskId);
                cv.Name = template.Name;
                cv.UpdatedBy = template.UpdatedBy;
                cv.UpdatedDt = DateTime.Now;
                unitOfWork.IMWbsTemplate.Update(cv);
                unitOfWork.Save();
            }
        }


        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        [HttpPost]
        public ActionResult GetComplexity([DataSourceRequest] DataSourceRequest request)
        {
            // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            int ClientId = client.ClientId;
            return Json(IdeaManagerHelper.GetComplexityDetails(ClientId, usid).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddComplexity(IMComplexityModel imComplexityModel, int Id)
        {

            if (imComplexityModel != null && imComplexityModel.Complexity.Trim()!= " " && ModelState.IsValid)
            {
                //var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                imComplexityModel.ClientId = Client.ClientId;
                imComplexityModel.CreatedBy = usid;
                var res = IdeaManagerHelper.CreateComplexity(imComplexityModel, Id);
                imComplexityModel.Id = res;
            }
            return Json(new[] { imComplexityModel }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditComplexity([DataSourceRequest] DataSourceRequest request, IMComplexityModel imComplexityModel)
        {
            if (imComplexityModel != null && imComplexityModel.Complexity.Trim() != " " && ModelState.IsValid)
            {
                // var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                imComplexityModel.ClientId = Client.ClientId;
                imComplexityModel.UpdatedBy = usid;
                var res = IdeaManagerHelper.UpdateComplexity(imComplexityModel);
            }
            return Json(new[] { imComplexityModel }.ToDataSourceResult(request, ModelState));
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteComplexity([DataSourceRequest] DataSourceRequest request, IMComplexityModel imComplexityModel)
        {
            if (imComplexityModel != null)
            {
                var v = IdeaManagerHelper.RemoveComplexity(imComplexityModel.Id);
            }
            return Json(new[] { imComplexityModel }.ToDataSourceResult(request, ModelState));
        }



        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        [HttpPost]
        [ActionTrack]
        public ActionResult GetIdeaType([DataSourceRequest] DataSourceRequest request)
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            int ClientId = client.ClientId;
            return Json(IdeaManagerHelper.GetIdeaTypeDetails(ClientId, usid).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddIdeaType(IMIdeaTypeModel imIdeaTypeModel, int Id)
        {

            if (imIdeaTypeModel != null && imIdeaTypeModel.IdeaType.Trim() != " " && ModelState.IsValid)
            {
                //var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                imIdeaTypeModel.ClientId = Client.ClientId;
                imIdeaTypeModel.CreatedBy = usid;
                var res = IdeaManagerHelper.CreateIdeaType(imIdeaTypeModel, Id);
                imIdeaTypeModel.Id = res;
            }
            return Json(new[] { imIdeaTypeModel }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditIdeaType([DataSourceRequest] DataSourceRequest request, IMIdeaTypeModel imIdeaTypeModel)
        {
            if (imIdeaTypeModel != null && imIdeaTypeModel.IdeaType.Trim() != " " && ModelState.IsValid)
            {
                //var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                imIdeaTypeModel.ClientId = Client.ClientId;
                imIdeaTypeModel.UpdatedBy = usid;
                var res = IdeaManagerHelper.UpdateIdeaType(imIdeaTypeModel);
            }
            return Json(new[] { imIdeaTypeModel }.ToDataSourceResult(request, ModelState));
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteIdeaType([DataSourceRequest] DataSourceRequest request, IMIdeaTypeModel imIdeaTypeModel)
        {
            if (imIdeaTypeModel != null)
            {
                var v = IdeaManagerHelper.RemoveIdeaType(imIdeaTypeModel.Id);
            }
            return Json(new[] { imIdeaTypeModel }.ToDataSourceResult(request, ModelState));
        }



        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        [HttpPost]
        public ActionResult GetIdeaSource([DataSourceRequest] DataSourceRequest request)
        {
            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            int ClientId = client.ClientId;
            return Json(IdeaManagerHelper.GetIdeaSourceDetails(ClientId, usid).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult AddIdeaSource(IMIdeaSourceModel imIdeaSourceModel, int Id)
        {

            if (imIdeaSourceModel != null && imIdeaSourceModel.IdeaSource.Trim() != " " && ModelState.IsValid)
            {
                //var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                imIdeaSourceModel.ClientId = Client.ClientId;
                imIdeaSourceModel.CreatedBy = usid;
                var res = IdeaManagerHelper.CreateIdeaSource(imIdeaSourceModel, Id);
                imIdeaSourceModel.Id = res;
            }
            return Json(new[] { imIdeaSourceModel }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult EditIdeaSource([DataSourceRequest] DataSourceRequest request, IMIdeaSourceModel imIdeaSourceModel)
        {
            if (imIdeaSourceModel != null && imIdeaSourceModel.IdeaSource.Trim() != " " && ModelState.IsValid)
            {
                // var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                imIdeaSourceModel.ClientId = Client.ClientId;
                imIdeaSourceModel.UpdatedBy = usid;
                var res = IdeaManagerHelper.UpdateIdeaSource(imIdeaSourceModel);
            }
            return Json(new[] { imIdeaSourceModel }.ToDataSourceResult(request, ModelState));
        }



        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult DeleteIdeaSource([DataSourceRequest] DataSourceRequest request, IMIdeaSourceModel imIdeaSourceModel)
        {
            if (imIdeaSourceModel != null)
            {
                var v = IdeaManagerHelper.RemoveIdeaSource(imIdeaSourceModel.Id);
            }
            return Json(new[] { imIdeaSourceModel }.ToDataSourceResult(request, ModelState));
        }


        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Idea Manager" }, null)]
        [HttpPost]
        [ActionTrack]
        public ActionResult GetLOB([DataSourceRequest] DataSourceRequest request)
        {
            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            int ClientId = client.ClientId;
            return Json(IdeaManagerHelper.GetLOBDetails(ClientId, usid).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult AddLOB(IMLOBModel imLOBModel, int Id)
        {

            if (imLOBModel != null && imLOBModel.LOB.Trim() != " " && ModelState.IsValid)
            {
                // var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                imLOBModel.ClientId = Client.ClientId;
                imLOBModel.CreatedBy = usid;
                var res = IdeaManagerHelper.CreateLOB(imLOBModel, Id);
                imLOBModel.Id = res;
            }
            return Json(new[] { imLOBModel }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult EditLOB([DataSourceRequest] DataSourceRequest request, IMLOBModel imLOBModel)
        {
            if (imLOBModel != null && imLOBModel.LOB.Trim() != " " && ModelState.IsValid)
            {
                //  var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                imLOBModel.ClientId = Client.ClientId;
                imLOBModel.UpdatedBy = usid;
                var res = IdeaManagerHelper.UpdateLOB(imLOBModel);
            }
            return Json(new[] { imLOBModel }.ToDataSourceResult(request, ModelState));
        }



        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult DeleteLOB([DataSourceRequest] DataSourceRequest request, IMLOBModel imLOBModel)
        {
            if (imLOBModel != null)
            {
                var v = IdeaManagerHelper.RemoveLOB(imLOBModel.Id);
            }
            return Json(new[] { imLOBModel }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        [ActionTrack]
        public ActionResult GetIdeaSourceMaster([DataSourceRequest] DataSourceRequest request)
        {
            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            int ClientId = client.ClientId;
            return Json(IdeaManagerHelper.GetIdeaSourceMasterDetails(ClientId, usid).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult AddIdeaSourceMaster(IMIdeaSourceMasterModel imIdeaSourceModel, int Id)
        {

            if (imIdeaSourceModel != null && imIdeaSourceModel.IdeaSource.Trim() != " " && ModelState.IsValid)
            {
                // var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                imIdeaSourceModel.ClientId = Client.ClientId;
                imIdeaSourceModel.CreatedBy = usid;
                var res = IdeaManagerHelper.CreateIdeaSourceMaster(imIdeaSourceModel, Id);
                imIdeaSourceModel.Id = res;
            }
            return Json(new[] { imIdeaSourceModel }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult EditIdeaSourceMaster([DataSourceRequest] DataSourceRequest request, IMIdeaSourceMasterModel imIdeaSourceModel)
        {
            if (imIdeaSourceModel != null && imIdeaSourceModel.IdeaSource.Trim() != " " && ModelState.IsValid)
            {
                //  var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                imIdeaSourceModel.ClientId = Client.ClientId;
                imIdeaSourceModel.UpdatedBy = usid;
                var res = IdeaManagerHelper.UpdateIdeaSourceMaster(imIdeaSourceModel);
            }
            return Json(new[] { imIdeaSourceModel }.ToDataSourceResult(request, ModelState));
        }



        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult DeleteIdeaSourceMaster([DataSourceRequest] DataSourceRequest request, IMIdeaSourceMaster imModel)
        {
            if (imModel != null)
            {
                var v = IdeaManagerHelper.RemoveIdeaSourceMaster(imModel.Id);
            }
            return Json(new[] { imModel }.ToDataSourceResult(request, ModelState));
        }






        [HttpPost]
        [ActionTrack]
        public ActionResult GetClassification([DataSourceRequest] DataSourceRequest request)
        {
            //  Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            int ClientId = client.ClientId;
            return Json(IdeaManagerHelper.GetClassificationDetails(ClientId, usid).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
       
        
        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult AddClassification([DataSourceRequest]DataSourceRequest request, IMClassificationModel imClassificationModel)
        {          
                if (imClassificationModel != null && imClassificationModel.Classification.Trim()!="" && ModelState.IsValid)
            {
                var currentTime = DateTime.Now;
                //   var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                imClassificationModel.ClientId = Client.ClientId;
                imClassificationModel.CreatedBy = usid;
                imClassificationModel.CreatedDate = currentTime;
                var res = IdeaManagerHelper.CreateClassification(imClassificationModel);
                imClassificationModel.CalId = res;
            }
            return Json(new[] { imClassificationModel }.ToDataSourceResult(request, ModelState));
        }

       
        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult EditClassification([DataSourceRequest] DataSourceRequest request, IMClassificationModel imClassificationModel)
        {
            if (imClassificationModel != null && imClassificationModel.Classification.Trim() != "" && ModelState.IsValid)
            {
                //  var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                imClassificationModel.ClientId = Client.ClientId;
                imClassificationModel.UpdatedBy = usid;
                var res = IdeaManagerHelper.UpdateClassification(imClassificationModel);
            }

            return Json(new[] { imClassificationModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult DeleteClassification([DataSourceRequest] DataSourceRequest request, IMClassificationModel imClassificationModel)
        {
            if (imClassificationModel != null)
            {
                //IdeaManagerHelper.deleteAllClassifications(imClassificationModel.Id);
                var v = IdeaManagerHelper.RemoveClassification(imClassificationModel.CalId);
            }

            return Json(new[] { imClassificationModel }.ToDataSourceResult(request, ModelState));
        }


        [ActionTrack]
        public ActionResult GetCategory([DataSourceRequest]DataSourceRequest request, int CattId)
        {
            //  NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            return Json(IdeaManagerHelper.GetCategoryDetails(CattId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            //return View("Index", PortfolioAnalyzerHelper.GetPortfolioDetails(client.Id));
        }

        [ActionTrack]
        public ActionResult AddCategory(IMCategoryModel imCategoryModel, int Id)
        {
            if (imCategoryModel != null && imCategoryModel.Category.Trim() != "" && ModelState.IsValid)
            {
               // NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];

                var currentTime = DateTime.Now;
                int usid = (int)Session["UserId"];
                imCategoryModel.CreatedBy = usid;
                imCategoryModel.CreatedDate = currentTime;
                imCategoryModel.clientid = client.ClientId;
                imCategoryModel.ClassificationId = Id;
                var res = IdeaManagerHelper.CreateCategory(imCategoryModel);
                imCategoryModel.CaId = res;
            }
            return Json(new[] { imCategoryModel }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }
        

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult EditCategory([DataSourceRequest] DataSourceRequest request, IMCategoryModel imCategoryModel)
        {
            if (imCategoryModel != null && imCategoryModel.Category.Trim() != "" && ModelState.IsValid)
            {

                var currentTime = DateTime.Now;
                int usid = (int)Session["UserId"];
                imCategoryModel.UpdatedBy = usid;
                imCategoryModel.LastUpdatedDate = currentTime;
                var res = IdeaManagerHelper.UpdateCategory(imCategoryModel);
            }

            return Json(new[] { imCategoryModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult DeleteCategory([DataSourceRequest] DataSourceRequest request, IMCategoryModel imCategoryModel)
        {
            if (imCategoryModel != null)
            {
                var v = IdeaManagerHelper.RemoveCategory(imCategoryModel.CaId);
            }
            return Json(new[] { imCategoryModel }.ToDataSourceResult(request, ModelState));
        }




        //--------------------------------------Configuration Type-----------------------------------------------//
        //[HttpPost]
        //public ActionResult GetConfigurationtype([DataSourceRequest] DataSourceRequest request)
        //{
        //    Client client = (Client)Session["Client"];
        //    int usid = (int)Session["UserId"];
        //    int ClientId = client.Id;
        //    return Json(IdeaManagerHelper.GetConfigurationtypeDetails(ClientId, usid).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        [ActionTrack]
        public ActionResult GetConfigurationtype([DataSourceRequest] DataSourceRequest request)
        {
            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            //int ClientId = client.Id;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
           ////var ap = unitOfWork.IMConfigType.Get().Where(x => x.ClientId == client.Id && x.ConfigType.Trim().ToLower() == "application").Any();
           //// if (!ap)
           //// {
           ////     IMConfigTypeModel imConfigTypeModel = new IMConfigTypeModel();
           ////     var currentTime = DateTime.Now;
           ////     //int usid = (int)Session["UserId"];
           ////     imConfigTypeModel.ClientId = client.Id;
           ////     imConfigTypeModel.ConfigType = "Application";
           ////     imConfigTypeModel.CreatedBy = usid;
           ////     imConfigTypeModel.CreatedDt = currentTime;
           ////     imConfigTypeModel.UpdatedBy = usid;
           ////     imConfigTypeModel.UpdatedDt = currentTime;
           ////     var res = IdeaManagerHelper.CreateConfigurationtype(imConfigTypeModel);
           ////     imConfigTypeModel.CTId = res;
           ////     var parentid = res;
           ////     var applicationList = unitOfWork.Application.Get().Where(x => x.ClientId == client.Id).ToList();
           ////     foreach (var ds in applicationList)
           ////     {
           ////         IMConfigItemModel imConfigItemModel = new IMConfigItemModel();
           ////         imConfigItemModel.ConfigTypeId = parentid;
           ////         imConfigItemModel.ConfigItem = ds.ApplicationName;
           ////         imConfigItemModel.ClientId = ds.ClientId;
           ////         imConfigItemModel.UpdatedBy = usid;
           ////         imConfigItemModel.UpdatedDt = currentTime;
           ////         imConfigItemModel.CreatedBy = usid;
           ////         imConfigItemModel.CreatedDt = currentTime;
           ////         var child = IdeaManagerHelper.CreateConfigurationitem(imConfigItemModel);
                    
           ////     }
           //// }


            return Json(IdeaManagerHelper.GetConfigurationtypeDetails(client.ClientId,usid).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult AddConfigurationtype([DataSourceRequest]DataSourceRequest request, IMConfigTypeModel imConfigTypeModel)
        {
            if (imConfigTypeModel != null && imConfigTypeModel.ConfigType.Trim() != "" && ModelState.IsValid)
            {
                var currentTime = DateTime.Now;
                //  var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                imConfigTypeModel.ClientId = Client.ClientId;
                imConfigTypeModel.CreatedBy = usid;
                imConfigTypeModel.CreatedDt = currentTime;
                var res = IdeaManagerHelper.CreateConfigurationtype(imConfigTypeModel);
                imConfigTypeModel.CTId = res;

                
            }

            return Json(new[] { imConfigTypeModel }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult EditConfigurationtype([DataSourceRequest] DataSourceRequest request, IMConfigTypeModel imConfigTypeModel)
        {
            if (imConfigTypeModel != null && imConfigTypeModel.ConfigType.Trim() != "" && ModelState.IsValid)
            {
                // var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                imConfigTypeModel.ClientId = Client.ClientId;
                imConfigTypeModel.UpdatedBy = usid;
                var res = IdeaManagerHelper.UpdateConfigurationtype(imConfigTypeModel);
            }

            return Json(new[] { imConfigTypeModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult DeleteConfigurationtype([DataSourceRequest] DataSourceRequest request, IMConfigTypeModel imConfigTypeModel)
        {
            if (imConfigTypeModel != null)
            {                
                var v = IdeaManagerHelper.RemoveConfigurationtype(imConfigTypeModel.CTId);
            }

            return Json(new[] { imConfigTypeModel }.ToDataSourceResult(request, ModelState));

        }



        //-----------------------------------------Configuration Item---------------------------------------------------------//
        [ActionTrack]
        public ActionResult GetConfigurationitem([DataSourceRequest]DataSourceRequest request, int ConId)
        {
          //  NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            return Json(IdeaManagerHelper.GetConfigurationitemDetails(ConId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [ActionTrack]
        public ActionResult AddConfigurationitem(IMConfigItemModel imConfigItemModel, int Id)
        {
            if (imConfigItemModel != null && imConfigItemModel.ConfigItem.Trim() != "" && ModelState.IsValid)
            {
                //NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];

                var currentTime = DateTime.Now;
                int usid = (int)Session["UserId"];
                imConfigItemModel.CreatedBy = usid;
                imConfigItemModel.CreatedDt = currentTime;
                imConfigItemModel.ClientId = client.ClientId;
                imConfigItemModel.ConfigTypeId = Id;
                var res = IdeaManagerHelper.CreateConfigurationitem(imConfigItemModel);
                imConfigItemModel.CIId = res;
            }
            return Json(new[] { imConfigItemModel }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }


        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult EditConfigurationitem([DataSourceRequest] DataSourceRequest request, IMConfigItemModel imConfigItemModel)
        {
            if (imConfigItemModel != null && imConfigItemModel.ConfigItem.Trim() != "" && ModelState.IsValid)
            {

                var currentTime = DateTime.Now;
                int usid = (int)Session["UserId"];
                imConfigItemModel.UpdatedBy = usid;
                imConfigItemModel.UpdatedDt = currentTime;
                var res = IdeaManagerHelper.UpdateConfigurationitem(imConfigItemModel);
            }

            return Json(new[] { imConfigItemModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult DeleteConfigurationitem([DataSourceRequest] DataSourceRequest request, IMConfigItemModel imConfigItemModel)
        {
            if (imConfigItemModel != null)
            {
                var v = IdeaManagerHelper.RemoveConfigurationitem(imConfigItemModel.CIId);
            }
            return Json(new[] { imConfigItemModel }.ToDataSourceResult(request, ModelState));
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
        public ActionResult GetTeams([DataSourceRequest] DataSourceRequest request)
        {
            var client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];

            return Json(ClientProfileManagerHelper.GetTeamsDetails(client.ClientId, usid).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult AddTeams(IMTeamModel imTeamModel)
        {

            if (imTeamModel != null && imTeamModel.Team.Trim() != " " && ModelState.IsValid)
            {

                int usid = (int)Session["UserId"];
                var client = (ClientDetail)Session["SessionClient"];
                imTeamModel.ClientId = client.ClientId;
                imTeamModel.CreatedBy = usid;
                var res = ClientProfileManagerHelper.CreateTeams(imTeamModel);
                imTeamModel.Id = res;
            }
            return Json(new[] { imTeamModel }.ToDataSourceResult(new DataSourceRequest(), ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult EditTeams([DataSourceRequest] DataSourceRequest request, IMTeamModel imTeamModel)
        {
            if (imTeamModel != null && imTeamModel.Team.Trim() != " " && ModelState.IsValid)
            {
                // var Client = (Client)Session["Client"];
                var client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                imTeamModel.ClientId = client.ClientId;
                imTeamModel.UpdatedBy = usid;
                var res = ClientProfileManagerHelper.UpdateTeams(imTeamModel);
            }
            return Json(new[] { imTeamModel }.ToDataSourceResult(request, ModelState));
        }



        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult DeleteTeams([DataSourceRequest] DataSourceRequest request, IMTeamModel imTeamModel)
        {
            if (imTeamModel != null)
            {
                var v = ClientProfileManagerHelper.RemoveTeams(imTeamModel.Id);
            }
            return Json(new[] { imTeamModel }.ToDataSourceResult(request, ModelState));
        }

[HttpPost]
[ActionTrack]
public ActionResult GetSystemImpactedService([DataSourceRequest] DataSourceRequest request)
{
    var client = (ClientDetail)Session["SessionClient"];
    int usid = (int)Session["UserId"];

    return Json(IMHelper.GetSysImpactedDetails(client.ClientId, usid).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
}

[AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult AddSystemImpactedService(IMSystemImpactedServiceModal imTeamModel)
{

    if (imTeamModel != null && imTeamModel.SystemImpactedService.Trim() != " " && ModelState.IsValid)
    {

        int usid = (int)Session["UserId"];
        var client = (ClientDetail)Session["SessionClient"];
        imTeamModel.ClientId = client.ClientId;
        var res = IMHelper.CreateSysImpacted(imTeamModel);
        imTeamModel.Id = res;
    }
    return Json(new[] { imTeamModel }.ToDataSourceResult(new DataSourceRequest(), ModelState));
}

[AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult EditSystemImpactedService([DataSourceRequest] DataSourceRequest request, IMSystemImpactedServiceModal imTeamModel)
{
    if (imTeamModel != null && imTeamModel.SystemImpactedService.Trim() != " " && ModelState.IsValid)
    {
        // var Client = (Client)Session["Client"];
        var client = (ClientDetail)Session["SessionClient"];
        int usid = (int)Session["UserId"];
        imTeamModel.ClientId = client.ClientId;
        var res = IMHelper.UpdateSysImpacted(imTeamModel);
    }
    return Json(new[] { imTeamModel }.ToDataSourceResult(request, ModelState));
}



[AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult DeleteSystemImpactedService([DataSourceRequest] DataSourceRequest request, IMSystemImpactedServiceModal imTeamModel)
{
    if (imTeamModel != null)
    {
        var v = IMHelper.RemoveSysImpacted(imTeamModel.Id);
    }
    return Json(new[] { imTeamModel }.ToDataSourceResult(request, ModelState));
}

        [HttpPost]
        public ActionResult ListOfAdmins([DataSourceRequest] DataSourceRequest request)
        {
            var client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];

            return Json(IMHelper.ListOfMetadataAdmins(client.ClientId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ActionTrack]
        public ActionResult AddSelectedAdmins(bool IMAutoApproval = false, string ApproverIds = "")
        {
            int Result;
            int usid = (int)Session["UserId"];
            var client = (ClientDetail)Session["SessionClient"];
            Db db = new Db();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var ExistingApprovers = unitOfWork.Clients.Get().Where(e => e.Id == client.ClientId).ToList();
                foreach (var item in ExistingApprovers)
                {
                    item.IMAutoApprovalSelection = IMAutoApproval;
                    unitOfWork.Clients.Update(item);
                    unitOfWork.Save();
                }
                if (IMAutoApproval)
                {
                    var deleteApprovers = from details in db.IMAutoApprovers
                                          where details.ClientId == client.ClientId
                                          select details;

                    foreach (var detail in deleteApprovers)
                    {
                        unitOfWork.IMAutoApprovers.Delete(detail.Id);

                    }
                    string[] SelectedAdminIds = ApproverIds.Split(',');
                    foreach (var item in SelectedAdminIds)
                    {
                        if (item != " " || item != "," || item != "" || item != "0")
                        {
                            var ApproverId = Convert.ToInt32(item);
                            IMAutoApprovers ideaApprover = new IMAutoApprovers();
                            ideaApprover.ApproverId = ApproverId;
                            ideaApprover.ClientId = client.ClientId;
                            ideaApprover.CreatedBy = usid;
                            ideaApprover.CreatedDate = DateTime.Now;
                            unitOfWork.IMAutoApprovers.Insert(ideaApprover);
                            unitOfWork.Save();
                            Result = ideaApprover.Id;
                        }
                    }
                }
                Result = -1;
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
        }              

    }
}
