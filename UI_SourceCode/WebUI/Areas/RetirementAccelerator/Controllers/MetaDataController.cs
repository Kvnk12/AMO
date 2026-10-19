using Data;
using Domain.Entities.RetirementAccelerator;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.IdeaManager.Dto;
using WebUI.Areas.RetirementAccelerator.Models;
using Newtonsoft.Json;
using NTTData.AMOPortal.WebUI.Filters;
using WebUI.Dto;

namespace WebUI.Areas.RetirementAccelerator.Controllers
{
    public class MetaDataController : Controller
    {
        //
        // GET: /RetirementAccelerator/MetaData/

        public ActionResult Index()
        {
            
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            ViewData["RPhaseList"] = GetRetirePhasesList(client.ClientId);
            return View();

        }

        public ActionResult WBS()
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            ViewPageWbs model = new ViewPageWbs();
            model.ResourceGroup = RAHelper.getResourceGroupsList(client.ClientId);
            model.Role = RAHelper.getRolesList(client.ClientId);
            model.WorkProduct = RAHelper.getWorkProductsList(client.ClientId);
            return View("WBS",model);
        }
        
        public ActionResult RetirementCandidate()
        {
            return View("RetirementCandidate");
        }

        //---------------------------------
        //[HttpPost]
        public ActionResult GetRoles([DataSourceRequest] DataSourceRequest request)
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            return Json(RAHelper.getAllRoles(client.ClientId).ToDataSourceResult(request));
        }


        [HttpPost]
        public ActionResult AddRoles([DataSourceRequest] DataSourceRequest request, RolesModel rolemodel)
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            if (rolemodel != null && rolemodel.RoleName.Trim() != "" && ModelState.IsValid)
            {
                //var x = Convert.ToInt32(IdeaId);
                int usid = (int)Session["UserId"];
                var res = RAHelper.CreateRole(rolemodel,client.ClientId, usid);
                rolemodel.Id = res;
            }
            return Json(new[] { rolemodel }.ToDataSourceResult(request, ModelState));

        }


        [HttpPost]
        public ActionResult EditRoles([DataSourceRequest] DataSourceRequest request, RolesModel rolemodel)
        {
            if (rolemodel != null && rolemodel.RoleName.Trim() != "" && ModelState.IsValid)
            {
                int usid = (int)Session["UserId"];
                var res = RAHelper.UpdateRole(rolemodel,usid);
            }

            return Json(new[] { rolemodel }.ToDataSourceResult(request, ModelState));
        }


        [HttpPost]
        public ActionResult DeleteRoles([DataSourceRequest] DataSourceRequest request, RolesModel rolemodel)
        {
            if (rolemodel != null)
            {
                var v = RAHelper.RemoveRole(rolemodel.Id);
            }

            return Json(new[] { rolemodel }.ToDataSourceResult(request, ModelState));
        }

        //-----------------Resource Group---------------//
        //[HttpPost]
        public ActionResult GetResourceGroups([DataSourceRequest] DataSourceRequest request)
        {
          //  Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];

            return Json(RAHelper.getAllResourceGroups(client.ClientId).ToDataSourceResult(request));
        }


        [HttpPost]
        public ActionResult AddResourceGroup([DataSourceRequest] DataSourceRequest request, ResourceGroupModel RGModel)
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            if (RGModel != null && RGModel.ResourceGroup.Trim() != "" && ModelState.IsValid)
            {
                //var x = Convert.ToInt32(IdeaId);
                int usid = (int)Session["UserId"];
                var res = RAHelper.CreateResourceGroup(RGModel, client.ClientId, usid);
                RGModel.Id = res;
            }
            return Json(new[] { RGModel }.ToDataSourceResult(request, ModelState));

        }


        [HttpPost]
        public ActionResult EditREsourceGroup([DataSourceRequest] DataSourceRequest request, ResourceGroupModel RGModel)
        {
            if (RGModel != null && RGModel.ResourceGroup.Trim() != "" && ModelState.IsValid)
            {
                int usid = (int)Session["UserId"];
                var res = RAHelper.UpdateResourceGroup(RGModel, usid);
            }

            return Json(new[] { RGModel }.ToDataSourceResult(request, ModelState));
        }


        [HttpPost]
        public ActionResult DeleteResourceGroup([DataSourceRequest] DataSourceRequest request, ResourceGroupModel RGModel)
        {
            if (RGModel != null)
            {
                var v = RAHelper.RemoveResourceGroup(RGModel.Id);
            }

            return Json(new[] { RGModel }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult GetRAResource([DataSourceRequest] DataSourceRequest request)
        {
            //try
            //{
               // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            return Json(RAHelper.getAllResource(client.ClientId).ToDataSourceResult(request));
            //}
            //catch(Exception ex)
            //{
            //    List<ResourceModel> rlist = new List<ResourceModel>();
            //    rlist.Add(new ResourceModel { ClientId = 1, Id = 1, RAResource = ex.Message });
            //    return Json(rlist.ToDataSourceResult(request));
            //}
        }


      

        [HttpPost]
        public ActionResult AddRAResource([DataSourceRequest] DataSourceRequest request, ResourceModel RGModel)
        {

            if (RGModel != null && RGModel.RAResource.Trim() != "" && ModelState.IsValid)
            {
                int usid = (int)Session["UserId"];
               // var clid = (Client)Session["Client"];
                ClientDetail clid = (ClientDetail)Session["SessionClient"];
                var res = RAHelper.CreateResource(RGModel, clid.ClientId, usid);
                RGModel.Id = res;
            }
            return Json(new[] { RGModel }.ToDataSourceResult(request, ModelState));

        }




        [HttpPost]
        public ActionResult EditRAResource([DataSourceRequest] DataSourceRequest request, ResourceModel RGModel)
        {
            if (RGModel != null && RGModel.RAResource.Trim() != "" && ModelState.IsValid)
            {
                int usid = (int)Session["UserId"];
                var res = RAHelper.UpdateResource(RGModel, usid);
            }

            return Json(new[] { RGModel }.ToDataSourceResult(request, ModelState));
        }


        [HttpPost]
        public ActionResult DeleteRAResource([DataSourceRequest] DataSourceRequest request, ResourceModel RGModel)
        {
            if (RGModel != null)
            {
                var v = RAHelper.RemoveResource(RGModel.Id);
            }

            return Json(new[] { RGModel }.ToDataSourceResult(request, ModelState));
        }


        //-----------------Retirement phase---------------//
        //[HttpPost]
        public ActionResult GetRetirementPhase([DataSourceRequest] DataSourceRequest request)
        {
            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            return Json(RAHelper.getAllRetirementPhases(client.ClientId).ToDataSourceResult(request));
        }


        [HttpPost]
        public ActionResult AddRetirementPhase([DataSourceRequest] DataSourceRequest request, RetirementPhaseModel RPModel)
        {
          //  Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            if (RPModel != null && RPModel.RetirementPhase.Trim() != "" && ModelState.IsValid)
            {
                int usid = (int)Session["UserId"];
                var res = RAHelper.CreateRetirementPhase(RPModel, client.ClientId, usid);
                RPModel.Id = res;
            }
            return Json(new[] { RPModel }.ToDataSourceResult(request, ModelState));

        }


        [HttpPost]
        public ActionResult EditRetirementPhase([DataSourceRequest] DataSourceRequest request, RetirementPhaseModel RPModel)
        {
            if (RPModel != null && RPModel.RetirementPhase.Trim() != "" && ModelState.IsValid)
            {
                int usid = (int)Session["UserId"];
                var res = RAHelper.UpdateRetirementPhase(RPModel, usid);
            }
            return Json(new[] { RPModel }.ToDataSourceResult(request, ModelState));
        }


        [HttpPost]
        public ActionResult DeleteRetirementPhase([DataSourceRequest] DataSourceRequest request, RetirementPhaseModel RPModel)
        {
            if (RPModel != null)
            {
                var v = RAHelper.RemoveRetirementPhase(RPModel.Id);
            }
            return Json(new[] { RPModel }.ToDataSourceResult(request, ModelState));
        }

        //-----------------Retirement Stage---------------//
        [HttpPost]
        public ActionResult GetRetirementStage([DataSourceRequest] DataSourceRequest request)
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            return Json(RAHelper.getAllRetirementStages(client.ClientId).ToDataSourceResult(request));
        }


        [HttpPost]
        public ActionResult AddRetirementStage([DataSourceRequest] DataSourceRequest request, RetirementStageModel RSModel)
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            if (RSModel != null && RSModel.RetirementStage.Trim() != ""  && ModelState.IsValid)
            {
                int usid = (int)Session["UserId"];
                var res = RAHelper.CreateRetirementStage(RSModel, client.ClientId, usid);
                RSModel.Id = res;
            }
            return Json(new[] { RSModel }.ToDataSourceResult(request, ModelState));

        }


        [HttpPost]
        public ActionResult EditRetirementStage([DataSourceRequest] DataSourceRequest request, RetirementStageModel RSModel)
        {
            if (RSModel != null && RSModel.RetirementStage.Trim() != ""  && ModelState.IsValid)
            {
                int usid = (int)Session["UserId"];
                var res = RAHelper.UpdateRetirementStage(RSModel, usid);
            }
            return Json(new[] { RSModel }.ToDataSourceResult(request, ModelState));
        }


        [HttpPost]
        public ActionResult DeleteRetirementStage([DataSourceRequest] DataSourceRequest request, RetirementStageModel RSModel)
        {
            if (RSModel != null)
            {
                var v = RAHelper.RemoveRetirementStage(RSModel.Id);
            }
            return Json(new[] { RSModel }.ToDataSourceResult(request, ModelState));
        }

        //-----------------Wbs Status---------------//
        [HttpPost]
        public ActionResult GetWbsStatus([DataSourceRequest] DataSourceRequest request)
        {
          //  Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            return Json(RAHelper.getAllWbsStatus(client.ClientId).ToDataSourceResult(request));
        }


        [HttpPost]
        public ActionResult AddWbsStatus([DataSourceRequest] DataSourceRequest request, WbsStatusModel WbModel)
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            if (WbModel != null && WbModel.WbsStatus.Trim() != ""  && ModelState.IsValid)
            {
                int usid = (int)Session["UserId"];
                var res = RAHelper.CreateWbsStatus(WbModel, client.ClientId, usid);
                WbModel.Id = res;
            }
            return Json(new[] { WbModel }.ToDataSourceResult(request, ModelState));

        }


        [HttpPost]
        public ActionResult EditWbsStatus([DataSourceRequest] DataSourceRequest request, WbsStatusModel WbModel)
        {
            if (WbModel != null && WbModel.WbsStatus.Trim() != ""  && ModelState.IsValid)
            {
                int usid = (int)Session["UserId"];
                var res = RAHelper.UpdateWbsStatus(WbModel, usid);
            }
            return Json(new[] { WbModel }.ToDataSourceResult(request, ModelState));
        }


        [HttpPost]
        public ActionResult DeleteWbsStatus([DataSourceRequest] DataSourceRequest request, WbsStatusModel WbModel)
        {
            if (WbModel != null)
            {
                var v = RAHelper.RemoveWbsStatus(WbModel.Id);
            }
            return Json(new[] { WbModel }.ToDataSourceResult(request, ModelState));
        }



        //-----------------Work Product---------------//
      //  [HttpPost]
        public ActionResult GetWorkProduct([DataSourceRequest] DataSourceRequest request)
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            return Json(RAHelper.getAllWorkProducts(client.ClientId).ToDataSourceResult(request));
        }


        [HttpPost]
        public ActionResult AddWorkProduct([DataSourceRequest] DataSourceRequest request, WorkProductModel WPModel)
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            if (WPModel != null && WPModel.WorkProduct.Trim() != "" && ModelState.IsValid)
            {
                int usid = (int)Session["UserId"];
                var res = RAHelper.CreateWorkProduct(WPModel, client.ClientId, usid);
                WPModel.Id = res;
            }
            return Json(new[] { WPModel }.ToDataSourceResult(request, ModelState));

        }


        [HttpPost]
        public ActionResult EditWorkProduct([DataSourceRequest] DataSourceRequest request, WorkProductModel WPModel)
        {
            if (WPModel != null && WPModel.WorkProduct.Trim() != "" && ModelState.IsValid)
            {
                int usid = (int)Session["UserId"];
                var res = RAHelper.UpdateWorkProduct(WPModel, usid);
            }
            return Json(new[] { WPModel }.ToDataSourceResult(request, ModelState));
        }


        [HttpPost]
        public ActionResult DeleteWorkProduct([DataSourceRequest] DataSourceRequest request, WorkProductModel WPModel)
        {
            if (WPModel != null)
            {
                var v = RAHelper.RemoveWorkProduct(WPModel.Id);
            }
            return Json(new[] { WPModel }.ToDataSourceResult(request, ModelState));
        }


        //-----------------Project Health---------------//
       // [HttpPost]
        public ActionResult GetProjectHealth([DataSourceRequest] DataSourceRequest request)
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            return Json(RAHelper.getAllProjectHealth(client.ClientId).ToDataSourceResult(request));
        }


        [HttpPost]
        public ActionResult AddProjectHealth([DataSourceRequest] DataSourceRequest request, ProjectHealthModel PHModel)
        {
          //  Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            if (PHModel != null && PHModel.ProjectHealth.Trim() != "" && PHModel.Color!="" && ModelState.IsValid)
            {
                int usid = (int)Session["UserId"];
                var res = RAHelper.CreateProjectHealth(PHModel, client.ClientId, usid);
                PHModel.Id = res;
            }
            return Json(new[] { PHModel }.ToDataSourceResult(request, ModelState));

        }


        [HttpPost]
        public ActionResult EditProjectHealth([DataSourceRequest] DataSourceRequest request, ProjectHealthModel PHModel)
        {
            if (PHModel != null && PHModel.ProjectHealth.Trim() != "" && PHModel.Color!="" && ModelState.IsValid)
            {
                int usid = (int)Session["UserId"];
                var res = RAHelper.UpdateProjectHealth(PHModel, usid);
            }
            return Json(new[] { PHModel }.ToDataSourceResult(request, ModelState));
        }


        [HttpPost]
        public ActionResult DeleteProjectHealth([DataSourceRequest] DataSourceRequest request, ProjectHealthModel PHModel)
        {
            if (PHModel != null)
            {
                var v = RAHelper.RemoveProjectHealth(PHModel.Id);
            }
            return Json(new[] { PHModel }.ToDataSourceResult(request, ModelState));
        }
        //--------------------------Estimate CATEGORY-------------------//
        [HttpPost]
        public ActionResult GetEstimateCategory([DataSourceRequest] DataSourceRequest request)
        {
           // var clid = (Client)Session["Client"];
            ClientDetail clid = (ClientDetail)Session["SessionClient"];
            return Json(RAHelper.GetEstimateCategory(clid.ClientId).AsEnumerable().ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddEstimateCategory([DataSourceRequest] DataSourceRequest request, EstimateCategoryModel raEstimateCategoryInput)
        {

            if (raEstimateCategoryInput != null && raEstimateCategoryInput.EstimateCategory.Trim() != "" && ModelState.IsValid)
            {
                int usid = (int)Session["UserId"];
               // var clid = (Client)Session["Client"];
                ClientDetail clid = (ClientDetail)Session["SessionClient"];
                var res = RAHelper.CreateEstimateCategory(raEstimateCategoryInput, clid.ClientId, usid);
                raEstimateCategoryInput.Id = res;
            }
            return Json(new[] { raEstimateCategoryInput }.ToDataSourceResult(request, ModelState));

        }

        [HttpPost]
        public ActionResult EditEstimateCategory([DataSourceRequest] DataSourceRequest request, EstimateCategoryModel raEstimateCategoryInput)
        {
            //if (imBusinessBenefitsInput != null && imBusinessBenefitsInput.Name.Trim() != "" && imBusinessBenefitsInput.Detail.Trim() != "" && ModelState.IsValid)
            if (raEstimateCategoryInput != null && raEstimateCategoryInput.EstimateCategory.Trim() != "" && ModelState.IsValid)
            {
                int usid = (int)Session["UserId"];
                var res = RAHelper.UpdateEstimateCategory(raEstimateCategoryInput, usid);
            }
            return Json(new[] { raEstimateCategoryInput }.ToDataSourceResult(request, ModelState));
        }



        [HttpPost]
        public ActionResult DeleteEstimateCategory([DataSourceRequest] DataSourceRequest request, EstimateCategoryModel raEstimateCategoryInput)
        {
            if (raEstimateCategoryInput != null)
            {
                var v = RAHelper.RemoveEstimateCategory(raEstimateCategoryInput.Id);
            }
            return Json(new[] { raEstimateCategoryInput }.ToDataSourceResult(request, ModelState));
        }

        //------------------------------------Retirement Candidate---------------------------------//
        [HttpPost]
        public ActionResult GetRetirementCanditate([DataSourceRequest] DataSourceRequest request)
        {
          //  var clid = (Client)Session["Client"];
            ClientDetail clid = (ClientDetail)Session["SessionClient"];
            return Json(RAHelper.getAllRetirementCandidates(clid.ClientId).AsEnumerable().ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult AddRetirementCandidate([DataSourceRequest] DataSourceRequest request, RetirementApplicationModel RCModel)
        {

            if (RCModel != null && RCModel.ApplicationName.Trim() != "" && ModelState.IsValid)
            {
                int usid = (int)Session["UserId"];
               // var clid = (Client)Session["Client"];
                ClientDetail clid = (ClientDetail)Session["SessionClient"];
                var res = RAHelper.CreateRetirementCandidate(RCModel, clid.ClientId, usid);
                RCModel.Id = res;
            }
            return Json(new[] { RCModel }.ToDataSourceResult(request, ModelState));

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditRetirementCandidate([DataSourceRequest] DataSourceRequest request, RetirementApplicationModel RCModel)
        {
            //if (imBusinessBenefitsInput != null && imBusinessBenefitsInput.Name.Trim() != "" && imBusinessBenefitsInput.Detail.Trim() != "" && ModelState.IsValid)
            if (RCModel != null && RCModel.ApplicationName.Trim() != "" && ModelState.IsValid)
            {
                int usid = (int)Session["UserId"];
                var res = RAHelper.UpdateRetirementCandidate(RCModel, usid);
            }
            return Json(new[] { RCModel }.ToDataSourceResult(request, ModelState));
        }



        [HttpPost]
        public ActionResult DeleteRetirementCanditate([DataSourceRequest] DataSourceRequest request, RetirementApplicationModel RCModel)
        {
            if (RCModel != null)
            {
                var v = RAHelper.RemoveRetirementCandidate(RCModel.Id);
            }
            return Json(new[] { RCModel }.ToDataSourceResult(request, ModelState));
        }


       // WBS -Template


       // [HttpPost]
        public JsonResult GetAllTasks([DataSourceRequest] DataSourceRequest request, int WbsTemplateId=0)
        {
          //  var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var result = new TreeDataSourceResult();
            if (WbsTemplateId > 0)
            {
                    result = RAHelper.GetAllWbsTasks(WbsTemplateId).ToTreeDataSourceResult(request,
                    e => e.TaskId,
                    e => e.TParentId,
                    e => e
                );
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Delete WBS Template 
        /// </summary>
        /// <param name="WbsTemplateId"></param>
        /// <returns></returns>
        
        [HttpPost]
        public JsonResult DeleteTemplate(int WbsTempNameId = 0)
        {
            if (WbsTempNameId > 0)
            {
                RAHelper.deleteWbsTemplate(WbsTempNameId);
            }

            return Json(new { Success="Success"});
        }


        [HttpPost]
        public JsonResult DestroyWbsTask([DataSourceRequest] DataSourceRequest request, WbsTemplateDetailsModel template)
        {
            if (ModelState.IsValid && template != null)
            {
                RAHelper.DeleteWbsTask(template.TaskId);
            }
            return Json(new[] { template }.ToTreeDataSourceResult(request, ModelState));
        }
        

        [HttpPost]
        public JsonResult CreateWbsTask([DataSourceRequest] DataSourceRequest request, WbsTemplateDetailsModel template, int WbsTemplateId=0,int roleid=0, int rg=0,int wp=0)
        {
            var uid = (int)Session["UserId"];
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            if (ModelState.IsValid && WbsTemplateId >0)
            {
                var something = RAHelper.AddWbsTask(template,client.ClientId,uid, roleid, rg, wp);
                template.TaskId = something;
            }

            return Json(new[] { template }.ToTreeDataSourceResult(request, ModelState));
        }

        [HttpPost]
        public ActionResult CreateWbsTemplate(string WbsTempName="")
        {
            var uid = (int)Session["UserId"];
         //   var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var ReturnID = 0;
            if (!String.IsNullOrWhiteSpace(WbsTempName))
            {
                var something = RAHelper.AddWbsTempName(client.ClientId, uid, WbsTempName.Trim());
                ReturnID = something;
            }

            return Json(new{ ReturnID });
        }

        [HttpPost]
        public JsonResult UpdateWbsTemplate([DataSourceRequest] DataSourceRequest request, WbsTemplateDetailsModel template,int roleid = 0, int rg = 0, int wp = 0)
        {
            var uid = (int)Session["UserId"];
            if (ModelState.IsValid)
            {
                RAHelper.EditWbsTask(template,uid, roleid, rg, wp);
            }
            return Json(new[] { template }.ToTreeDataSourceResult(request, ModelState));
        }


        public ActionResult GetWbsTempNames()
        {
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var TemplatesList = RAHelper.getAllWbsTemplates(client.ClientId);
            return Json(TemplatesList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CheckWBSAvailability(string WbsName="")
        {
          //  var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var Isthere = false;
            if (WbsName.Trim() != "")
            {
                Isthere = RAHelper.checkWbsNameExists(client.ClientId,WbsName);
            }
            else
            {
                Isthere = true;
            }
            return Json(new { Isthere });    
        }


        








        //------------------------------------------Drop Down Values----------------------------------//
        public IEnumerable<SelectListItem> GetRetirePhasesList(int ClientID)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var list = RAHelper.getRetirePhasesList(ClientID);
            return list;
        }



    }
}
