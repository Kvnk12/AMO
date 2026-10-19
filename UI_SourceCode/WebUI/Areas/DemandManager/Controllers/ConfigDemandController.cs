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
using WebUI.Areas.DemandManager.Dto;
using WebUI.Areas.DemandManager.Models;
using WebUI.Areas.DemandManager.Object;
using WebUI.Dto;

namespace WebUI.Areas.DemandManager.Controllers
{
    public class ConfigDemandController : Controller
    {
        //
        // GET: /DemandManager/Config/

        public ActionResult Index()
        {
            return View("Phase_Status");
        }

        
             public ActionResult Index1()
        {
            return View("Index");
        }

        public ActionResult Complexity1()
        {
            return View("Complexity");
        }

        public ActionResult ClassificationCategory()
        {
            return View("ClassificationCategory");
        }

    
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Demand Manager" }, null)]
        [HttpPost]
        public ActionResult readProgressStatus([DataSourceRequest] DataSourceRequest request)
        {
            //var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];

            var Db = new Db();
            var progessStatus = (from PS in Db.DMProgressStatus
                                 where PS.ClientId == client.ClientId
                                 select PS);

            return Json(progessStatus.AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }



        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Demand Manager" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddProgressStatus([DataSourceRequest] DataSourceRequest request, DMProgressStatusModel dmProgressStatus)
        {
            if (dmProgressStatus != null && ModelState.IsValid)
            {
                var uid = (int)Session["UserId"];
               // var client = (Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];

                dmProgressStatus.ClientId = client.ClientId;
                dmProgressStatus.CreatedBy = uid;
                dmProgressStatus.UpdatedBy = uid;
                var res = add_ps(dmProgressStatus);
                dmProgressStatus.Id = res;
            }
            return Json(new[] { dmProgressStatus }.ToDataSourceResult(request, ModelState));
        }

        public static int add_ps(DMProgressStatusModel imProgressStatus)
        {
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    DMProgressStatus cv = new DMProgressStatus();
                    cv.ClientId = imProgressStatus.ClientId;
                    cv.CreatedBy = imProgressStatus.CreatedBy;
                    cv.UpdatedBy = imProgressStatus.UpdatedBy;
                    cv.Color = imProgressStatus.Color;
                    cv.Status = imProgressStatus.Status;
                    cv.CreatedDt = DateTime.Now;
                    cv.UpdatedDt = DateTime.Now;
                    unitOfWork.DMProgressStatus.Insert(cv);
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

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Demand Manager" }, null)]
        [HttpPost]
        public ActionResult UpdateProgressStatus([DataSourceRequest] DataSourceRequest request, DMProgressStatusModel dmProgressStatus)
        {
            if (dmProgressStatus != null && ModelState.IsValid)
            {
                var uid = (int)Session["UserId"];
                dmProgressStatus.UpdatedBy = uid;
                var res = update_ps(dmProgressStatus);
            }
            return Json(new[] { dmProgressStatus }.ToDataSourceResult(request, ModelState));
        }

        public static bool update_ps(DMProgressStatusModel imProgressStatus)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var x = unitOfWork.DMProgressStatus.GetByID(imProgressStatus.Id);
                    x.Status = imProgressStatus.Status;
                    x.Color = imProgressStatus.Color;
                    x.UpdatedBy = imProgressStatus.UpdatedBy;
                    x.UpdatedDt= DateTime.Now;
                    unitOfWork.DMProgressStatus.Update(x);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Demand Manager" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteProgressStatus([DataSourceRequest] DataSourceRequest request, DMProgressStatusModel dmProgressStatus)
        {
            try
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.DMProgressStatus.Delete(dmProgressStatus.Id);
                unitOfWork.Save();
                return Json(new[] { dmProgressStatus }.ToDataSourceResult(request, ModelState));
            }
            catch(Exception)
            {
                return Json(new[] { dmProgressStatus }.ToDataSourceResult(request, ModelState));
            }
        }

        //-------------------------
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Demand Manager" }, null)]
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

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Demand Manager" }, null)]
        [HttpPost]
        public ActionResult AddStatus([DataSourceRequest] DataSourceRequest request, DMStatusModel dmStatus)
        {
            if (dmStatus != null && ModelState.IsValid)
            {
                var uid = (int)Session["UserId"];
                //var client = (Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];

                dmStatus.ClientId = client.ClientId;
                dmStatus.CreatedBy = uid;
                dmStatus.UpdatedBy = uid;
                var res = add_s(dmStatus);
                dmStatus.Id = res;
            }
            return Json(new[] { dmStatus }.ToDataSourceResult(request, ModelState));
        }
        public static int add_s(DMStatusModel imStatus)
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
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Demand Manager" }, null)]
        [HttpPost]
        public ActionResult UpdateStatus([DataSourceRequest] DataSourceRequest request, DMStatusModel dmStatus)
        {
            if (dmStatus != null && ModelState.IsValid)
            {
                var uid = (int)Session["UserId"];
                dmStatus.UpdatedBy = uid;
                var res = update_s(dmStatus);
            }
            return Json(new[] { dmStatus }.ToDataSourceResult(request, ModelState));
        }
        public static bool update_s(DMStatusModel imStatus)
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

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Demand Manager" }, null)]
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

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Demand Manager" }, null)]
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
        private static IEnumerable<DMWBSTemplateModel> GetAll(int clientId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var templatesList = unitOfWork.IMWbsTemplate.Get(g => g.ClientId == clientId);
            var templatesList_1 = templatesList.Select(c => new DMWBSTemplateModel
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
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Demand Manager" }, null)]
        [HttpPost]
        public JsonResult Destroy([DataSourceRequest] DataSourceRequest request, DMWBSTemplateModel template)
        {
            if (ModelState.IsValid && template != null)
            {
                WBS_Delete(template);
            }

            return Json(new[] { template }.ToTreeDataSourceResult(request, ModelState));
        }
        //Helper to Delete the WBS_Template/Phase/Progress
        public static void WBS_Delete(DMWBSTemplateModel template)
        {
            if (template.TaskId > 0)
            {               
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.IMWbsTemplate.Delete(template.TaskId);
                unitOfWork.Save();
            }
        }
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Demand Manager" }, null)]
        [HttpPost]
        public JsonResult Create([DataSourceRequest] DataSourceRequest request, DMWBSTemplateModel template)
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
        public static int WBS_Insert(DMWBSTemplateModel template)
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
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Demand Manager" }, null)]
        [HttpPost]
        public JsonResult Update([DataSourceRequest] DataSourceRequest request, DMWBSTemplateModel template)
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
        public static void WBS_Update(DMWBSTemplateModel template)
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


        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Demand Manager" }, null)]
        [HttpPost]
        public ActionResult GetComplexity([DataSourceRequest] DataSourceRequest request)
        {
            // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            int ClientId = client.ClientId;
            return Json(DemandManagerHelper.GetComplexityDetails(ClientId, usid).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddComplexity(DMComplexityModel imComplexityModel, int Id)
        {

            if (imComplexityModel != null && imComplexityModel.Complexity.Trim() != " " && ModelState.IsValid)
            {
                //var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                imComplexityModel.ClientId = Client.ClientId;
                imComplexityModel.CreatedBy = usid;
                var res = DemandManagerHelper.CreateComplexity(imComplexityModel, Id);
                imComplexityModel.Id = res;
            }
            return Json(new[] { imComplexityModel }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditComplexity([DataSourceRequest] DataSourceRequest request, DMComplexityModel imComplexityModel)
        {
            if (imComplexityModel != null && imComplexityModel.Complexity.Trim() != " " && ModelState.IsValid)
            {
                // var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                imComplexityModel.ClientId = Client.ClientId;
                imComplexityModel.UpdatedBy = usid;
                var res = DemandManagerHelper.UpdateComplexity(imComplexityModel);
            }
            return Json(new[] { imComplexityModel }.ToDataSourceResult(request, ModelState));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteComplexity([DataSourceRequest] DataSourceRequest request, DMComplexityModel imComplexityModel)
        {
            if (imComplexityModel != null)
            {
                var v = DemandManagerHelper.RemoveComplexity(imComplexityModel.Id);
            }
            return Json(new[] { imComplexityModel }.ToDataSourceResult(request, ModelState));
        }



        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Demand Manager" }, null)]
        [HttpPost]
        public ActionResult GetDemandType([DataSourceRequest] DataSourceRequest request)
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            int ClientId = client.ClientId;
            return Json(DemandManagerHelper.GetDemandTypeDetails(ClientId, usid).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddDemandType(DMDemandTypeModel dmDemandTypeModel, int Id)
        {

            if (dmDemandTypeModel != null && dmDemandTypeModel.DemandType.Trim() != " " && ModelState.IsValid)
            {
                //var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                dmDemandTypeModel.ClientId = Client.ClientId;
                dmDemandTypeModel.CreatedBy = usid;
                var res = DemandManagerHelper.CreateDemandType(dmDemandTypeModel, Id);
                dmDemandTypeModel.Id = res;
            }
            return Json(new[] { dmDemandTypeModel }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditDemandType([DataSourceRequest] DataSourceRequest request, DMDemandTypeModel dmDemandTypeModel)
        {
            if (dmDemandTypeModel != null && dmDemandTypeModel.DemandType.Trim() != " " && ModelState.IsValid)
            {
                //var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                dmDemandTypeModel.ClientId = Client.ClientId;
                dmDemandTypeModel.UpdatedBy = usid;
                var res = DemandManagerHelper.UpdateDemandType(dmDemandTypeModel);
            }
            return Json(new[] { dmDemandTypeModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteDemandType([DataSourceRequest] DataSourceRequest request, DMDemandTypeModel dmDemandTypeModel)
        {
            if (dmDemandTypeModel != null)
            {
                var v = DemandManagerHelper.RemoveDemandType(dmDemandTypeModel.Id);
            }
            return Json(new[] { dmDemandTypeModel }.ToDataSourceResult(request, ModelState));
        }


        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Demand Manager" }, null)]
        [HttpPost]
        public ActionResult GetLOB([DataSourceRequest] DataSourceRequest request)
        {
            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            int ClientId = client.ClientId;
            return Json(DemandManagerHelper.GetLOBDetails(ClientId, usid).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddLOB(DMLOBModel imLOBModel, int Id)
        {

            if (imLOBModel != null && imLOBModel.LOB.Trim() != " " && ModelState.IsValid)
            {
                // var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                imLOBModel.ClientId = Client.ClientId;
                imLOBModel.CreatedBy = usid;
                var res = DemandManagerHelper.CreateLOB(imLOBModel, Id);
                imLOBModel.Id = res;
            }
            return Json(new[] { imLOBModel }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditLOB([DataSourceRequest] DataSourceRequest request, DMLOBModel imLOBModel)
        {
            if (imLOBModel != null && imLOBModel.LOB.Trim() != " " && ModelState.IsValid)
            {
                //  var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                imLOBModel.ClientId = Client.ClientId;
                imLOBModel.UpdatedBy = usid;
                var res = DemandManagerHelper.UpdateLOB(imLOBModel);
            }
            return Json(new[] { imLOBModel }.ToDataSourceResult(request, ModelState));
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteLOB([DataSourceRequest] DataSourceRequest request, DMLOBModel imLOBModel)
        {
            if (imLOBModel != null)
            {
                var v = DemandManagerHelper.RemoveLOB(imLOBModel.Id);
            }
            return Json(new[] { imLOBModel }.ToDataSourceResult(request, ModelState));
        }


        
        [HttpPost]
        public ActionResult GetClassification([DataSourceRequest] DataSourceRequest request)
        {
            //  Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            int ClientId = client.ClientId;
            return Json(DemandManagerHelper.GetClassificationDetails(ClientId, usid).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
       
        
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddClassification([DataSourceRequest]DataSourceRequest request, DMClassificationModel imClassificationModel)
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
                var res = DemandManagerHelper.CreateClassification(imClassificationModel);
                imClassificationModel.CalId = res;
            }
            return Json(new[] { imClassificationModel }.ToDataSourceResult(request, ModelState));
        }

       
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditClassification([DataSourceRequest] DataSourceRequest request, DMClassificationModel imClassificationModel)
        {
            if (imClassificationModel != null && imClassificationModel.Classification.Trim() != "" && ModelState.IsValid)
            {
                //  var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                imClassificationModel.ClientId = Client.ClientId;
                imClassificationModel.UpdatedBy = usid;
                var res = DemandManagerHelper.UpdateClassification(imClassificationModel);
            }

            return Json(new[] { imClassificationModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteClassification([DataSourceRequest] DataSourceRequest request, DMClassificationModel imClassificationModel)
        {
            if (imClassificationModel != null)
            {
                //DemandManagerHelper.deleteAllClassifications(imClassificationModel.Id);
                var v = DemandManagerHelper.RemoveClassification(imClassificationModel.CalId);
            }

            return Json(new[] { imClassificationModel }.ToDataSourceResult(request, ModelState));
        }
      

        
        public ActionResult GetCategory([DataSourceRequest]DataSourceRequest request, int CattId)
        {
            //  NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            return Json(DemandManagerHelper.GetCategoryDetails(CattId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            //return View("Index", PortfolioAnalyzerHelper.GetPortfolioDetails(client.Id));
        }
       

        public ActionResult AddCategory(DMCategoryModel imCategoryModel, int Id)
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
                var res = DemandManagerHelper.CreateCategory(imCategoryModel);
                imCategoryModel.CaId = res;
            }
            return Json(new[] { imCategoryModel }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }
        

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditCategory([DataSourceRequest] DataSourceRequest request, DMCategoryModel imCategoryModel)
        {
            if (imCategoryModel != null && imCategoryModel.Category.Trim() != "" && ModelState.IsValid)
            {

                var currentTime = DateTime.Now;
                int usid = (int)Session["UserId"];
                imCategoryModel.UpdatedBy = usid;
                imCategoryModel.LastUpdatedDate = currentTime;
                var res = DemandManagerHelper.UpdateCategory(imCategoryModel);
            }

            return Json(new[] { imCategoryModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteCategory([DataSourceRequest] DataSourceRequest request, DMCategoryModel imCategoryModel)
        {
            if (imCategoryModel != null)
            {
                var v = DemandManagerHelper.RemoveCategory(imCategoryModel.CaId);
            }
            return Json(new[] { imCategoryModel }.ToDataSourceResult(request, ModelState));
        }




        //--------------------------------------Configuration Type-----------------------------------------------//

        [HttpPost]
        public ActionResult GetConfigurationtype([DataSourceRequest] DataSourceRequest request)
        {
            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            //int ClientId = client.Id;
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

            return Json(DemandManagerHelper.GetConfigurationtypeDetails(client.ClientId,usid).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddConfigurationtype([DataSourceRequest]DataSourceRequest request, DMConfigTypeModel imConfigTypeModel)
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
                var res = DemandManagerHelper.CreateConfigurationtype(imConfigTypeModel);
                imConfigTypeModel.CTId = res;

                
            }

            return Json(new[] { imConfigTypeModel }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditConfigurationtype([DataSourceRequest] DataSourceRequest request, DMConfigTypeModel imConfigTypeModel)
        {
            if (imConfigTypeModel != null && imConfigTypeModel.ConfigType.Trim() != "" && ModelState.IsValid)
            {
                // var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                imConfigTypeModel.ClientId = Client.ClientId;
                imConfigTypeModel.UpdatedBy = usid;
                var res = DemandManagerHelper.UpdateConfigurationtype(imConfigTypeModel);
            }

            return Json(new[] { imConfigTypeModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteConfigurationtype([DataSourceRequest] DataSourceRequest request, DMConfigTypeModel imConfigTypeModel)
        {
            if (imConfigTypeModel != null)
            {                
                var v = DemandManagerHelper.RemoveConfigurationtype(imConfigTypeModel.CTId);
            }

            return Json(new[] { imConfigTypeModel }.ToDataSourceResult(request, ModelState));

        }



        //-----------------------------------------Configuration Item---------------------------------------------------------//

        public ActionResult GetConfigurationitem([DataSourceRequest]DataSourceRequest request, int ConId)
        {
          //  NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            return Json(DemandManagerHelper.GetConfigurationitemDetails(ConId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult AddConfigurationitem(DMConfigItemModel imConfigItemModel, int Id)
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
                var res = DemandManagerHelper.CreateConfigurationitem(imConfigItemModel);
                imConfigItemModel.CIId = res;
            }
            return Json(new[] { imConfigItemModel }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditConfigurationitem([DataSourceRequest] DataSourceRequest request, DMConfigItemModel imConfigItemModel)
        {
            if (imConfigItemModel != null && imConfigItemModel.ConfigItem.Trim() != "" && ModelState.IsValid)
            {

                var currentTime = DateTime.Now;
                int usid = (int)Session["UserId"];
                imConfigItemModel.UpdatedBy = usid;
                imConfigItemModel.UpdatedDt = currentTime;
                var res = DemandManagerHelper.UpdateConfigurationitem(imConfigItemModel);
            }

            return Json(new[] { imConfigItemModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteConfigurationitem([DataSourceRequest] DataSourceRequest request, DMConfigItemModel imConfigItemModel)
        {
            if (imConfigItemModel != null)
            {
                var v = DemandManagerHelper.RemoveConfigurationitem(imConfigItemModel.CIId);
            }
            return Json(new[] { imConfigItemModel }.ToDataSourceResult(request, ModelState));
        }


        //------------------------------------------Task-------------------------------------------//

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Demand Manager" }, null)]
        [HttpPost]
        public ActionResult GetDemandTask([DataSourceRequest] DataSourceRequest request)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            int ClientId = client.ClientId;
            return Json(DemandManagerHelper.GetTaskDetails(ClientId,usid).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddDemandTask(DMDemandTaskModel dmDemandTaskModel, int Id)
        {

            if (dmDemandTaskModel != null && dmDemandTaskModel.DemandTask.Trim() != " " && ModelState.IsValid)
            {
                //var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                dmDemandTaskModel.ClientId = Client.ClientId;
                dmDemandTaskModel.CreatedBy = usid;
                var res = DemandManagerHelper.CreateDemandTask(dmDemandTaskModel, Id);
                dmDemandTaskModel.Id = res;
            }
            return Json(new[] { dmDemandTaskModel }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditDemandTask([DataSourceRequest] DataSourceRequest request, DMDemandTaskModel dmDemandTaskModel)
        {
            if (dmDemandTaskModel != null && dmDemandTaskModel.DemandTask.Trim() != " " && ModelState.IsValid)
            {
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                dmDemandTaskModel.ClientId = Client.ClientId;
                dmDemandTaskModel.UpdatedBy = usid;
                var res = DemandManagerHelper.UpdateDemandTask(dmDemandTaskModel);
            }
            return Json(new[] { dmDemandTaskModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteDemandTask([DataSourceRequest] DataSourceRequest request, DMDemandTaskModel dmDemandTaskModel)
        {
            if (dmDemandTaskModel != null)
            {
                var v = DemandManagerHelper.RemoveDemandTask(dmDemandTaskModel.Id);
            }
            return Json(new[] { dmDemandTaskModel }.ToDataSourceResult(request, ModelState));
        }

        //------------------------------------------Stage-------------------------------------------//

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Demand Manager" }, null)]
        [HttpPost]
        public ActionResult GetDemandStage([DataSourceRequest] DataSourceRequest request)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            int ClientId = client.ClientId;
            return Json(DemandManagerHelper.GetStageDetails(ClientId,usid).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddDemandStage(DMDemandStageModel dmDemandStageModel, int Id)
        {

            if (dmDemandStageModel != null && dmDemandStageModel.DemandStage.Trim() != " " && ModelState.IsValid)
            {
                //var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                dmDemandStageModel.ClientId = Client.ClientId;
                dmDemandStageModel.CreatedBy = usid;
                var res = DemandManagerHelper.CreateDemandStage(dmDemandStageModel, Id);
                dmDemandStageModel.Id = res;
            }
            return Json(new[] { dmDemandStageModel }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditDemandStage([DataSourceRequest] DataSourceRequest request, DMDemandStageModel dmDemandStageModel)
        {
            if (dmDemandStageModel != null && dmDemandStageModel.DemandStage.Trim() != " " && ModelState.IsValid)
            {
                //var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                dmDemandStageModel.ClientId = Client.ClientId;
                dmDemandStageModel.UpdatedBy = usid;
                var res = DemandManagerHelper.UpdateDemandStage(dmDemandStageModel);
            }
            return Json(new[] { dmDemandStageModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteDemandStage([DataSourceRequest] DataSourceRequest request, DMDemandStageModel dmDemandStageModel)
        {
            if (dmDemandStageModel != null)
            {
                var v = DemandManagerHelper.RemoveDemandStage(dmDemandStageModel.Id);
            }
            return Json(new[] { dmDemandStageModel }.ToDataSourceResult(request, ModelState));
        }


        //------------------------------------------Document Type-------------------------------------------//

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Demand Manager" }, null)]
        [HttpPost]
        public ActionResult GetDemandDocumentType([DataSourceRequest] DataSourceRequest request)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            int ClientId = client.ClientId;
            return Json(DemandManagerHelper.GetDocumentTypeDetails(ClientId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddDemandDocumentType(DMDemandDocumentTypeModel dmDemandDocumentTypeModel, int Id)
        {

            if (dmDemandDocumentTypeModel != null && dmDemandDocumentTypeModel.DocumentType.Trim() != " " && ModelState.IsValid)
            {
                //var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                dmDemandDocumentTypeModel.ClientId = Client.ClientId;
                dmDemandDocumentTypeModel.CreatedBy = usid;
                var res = DemandManagerHelper.CreateDemandDocumentType(dmDemandDocumentTypeModel, Id);
                dmDemandDocumentTypeModel.Id = res;
            }
            return Json(new[] { dmDemandDocumentTypeModel }.ToDataSourceResult(new DataSourceRequest(), ModelState));

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditDemandDocumentType([DataSourceRequest] DataSourceRequest request, DMDemandDocumentTypeModel dmDemandDocumentTypeModel)
        {
            if (dmDemandDocumentTypeModel != null && dmDemandDocumentTypeModel.DocumentType.Trim() != " " && ModelState.IsValid)
            {
                //var Client = (Client)Session["Client"];
                ClientDetail Client = (ClientDetail)Session["SessionClient"];
                int usid = (int)Session["UserId"];
                dmDemandDocumentTypeModel.ClientId = Client.ClientId;
                dmDemandDocumentTypeModel.UpdatedBy = usid;
                var res = DemandManagerHelper.UpdateDemandDocumentType(dmDemandDocumentTypeModel);
            }
            return Json(new[] { dmDemandDocumentTypeModel }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteDemandDocumentType([DataSourceRequest] DataSourceRequest request, DMDemandDocumentTypeModel dmDemandDocumentTypeModel)
        {
            if (dmDemandDocumentTypeModel != null)
            {
                var v = DemandManagerHelper.RemoveDemandDocumentType(dmDemandDocumentTypeModel.Id);
            }
            return Json(new[] { dmDemandDocumentTypeModel }.ToDataSourceResult(request, ModelState));
        }
    }
}
