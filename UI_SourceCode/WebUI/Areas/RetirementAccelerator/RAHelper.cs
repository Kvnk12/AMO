using Data;
using Domain.Entities.RetirementAccelerator;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.RetirementAccelerator.Models;

namespace WebUI.Areas.RetirementAccelerator
{
    public class RAHelper
    {


        public static IEnumerable<RolesModel> getAllRoles(int ClientId)
        {
            IList<RolesModel> Roles = new List<RolesModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<RARoles> rolesList = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (ClientId != 0)
                    rolesList = unitOfWork.RARoles.Get(cid => cid.ClientId == ClientId).ToList();

                if (rolesList != null)
                {
                    foreach (var _roles in rolesList)
                    {
                        RolesModel role = new RolesModel();
                        role.Id = _roles.Id;
                        role.RoleName = _roles.RoleName;
                        Roles.Add(role);
                    }
                }
                return Roles;
            }
        }


        public static int CreateRole(RolesModel roleModel, int ClientId, int uid)
        {
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var cns = new RARoles();
                    cns.ClientId = ClientId;
                    cns.RoleName = roleModel.RoleName.Trim();
                    cns.CreatedDt = DateTime.Now;
                    cns.UpdatedDt = DateTime.Now;
                    cns.UpdatedBy = uid;
                    cns.CreatedBy = uid;
                    unitOfWork.RARoles.Insert(cns);
                    unitOfWork.Save();
                    id = cns.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool UpdateRole(RolesModel rolemodel, int uid)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.RARoles.GetByID(rolemodel.Id);
                    updatetitem.UpdatedDt = DateTime.Now;
                    updatetitem.UpdatedBy = uid;
                    updatetitem.RoleName = rolemodel.RoleName.Trim();
                    unitOfWork.RARoles.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveRole(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.RARoles.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }

        //------------------------------------Resource Group---------------------------------//
        public static IEnumerable<ResourceGroupModel> getAllResourceGroups(int ClientId)
        {
            IList<ResourceGroupModel> ListModel = new List<ResourceGroupModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<RAResourceGroup> EntityList = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (ClientId != 0)
                    EntityList = unitOfWork.RAResourceGroup.Get(cid => cid.ClientId == ClientId).ToList();

                if (EntityList != null)
                {
                    foreach (var _entity in EntityList)
                    {
                        ResourceGroupModel model = new ResourceGroupModel();
                        model.Id = _entity.Id;
                        model.ClientId = _entity.ClientId;
                        model.ResourceGroup = _entity.ResourceGroup;
                        ListModel.Add(model);
                    }
                }
                return ListModel;
            }
        }


        public static int CreateResourceGroup(ResourceGroupModel Model, int ClientId, int uid)
        {
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var entity = new RAResourceGroup();
                    entity.ClientId = ClientId;
                    entity.ResourceGroup = Model.ResourceGroup.Trim();
                    entity.CreatedDt = DateTime.Now;
                    entity.UpdatedDt = DateTime.Now;
                    entity.UpdatedBy = uid;
                    entity.CreatedBy = uid;
                    unitOfWork.RAResourceGroup.Insert(entity);
                    unitOfWork.Save();
                    id = entity.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool UpdateResourceGroup(ResourceGroupModel Model, int uid)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.RAResourceGroup.GetByID(Model.Id);
                    updatetitem.UpdatedDt = DateTime.Now;
                    updatetitem.UpdatedBy = uid;
                    updatetitem.ResourceGroup = Model.ResourceGroup.Trim();
                    unitOfWork.RAResourceGroup.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool RemoveResourceGroup(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.RAResourceGroup.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }

        //------------------------------------Resource ---------------------------------//
        public static IEnumerable<ResourceModel> getAllResource(int ClientId)
        {
            IList<ResourceModel> ListModel = new List<ResourceModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<RAResource> EntityList = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (ClientId != 0)
                    EntityList = unitOfWork.RAResource.Get(cid => cid.ClientId == ClientId).ToList();

                if (EntityList != null)
                {
                    foreach (var _entity in EntityList)
                    {
                        ResourceModel model = new ResourceModel();
                        model.Id = _entity.Id;
                        model.RAResource = _entity.Resource;
                        model.ClientId = _entity.ClientId;
                        ListModel.Add(model);
                    }
                }
                return ListModel;
            }
        }


        public static int CreateResource(ResourceModel Model, int ClientId, int uid)
        {
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var entity = new RAResource();
                    entity.ClientId = ClientId;
                    entity.Resource = Model.RAResource.Trim();
                    entity.CreatedDt = DateTime.Now;
                    entity.UpdatedDt = DateTime.Now;
                    entity.UpdatedBy = uid;
                    entity.CreatedBy = uid;
                    unitOfWork.RAResource.Insert(entity);
                    unitOfWork.Save();
                    id = entity.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool UpdateResource(ResourceModel Model, int uid)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.RAResource.GetByID(Model.Id);
                    updatetitem.UpdatedDt = DateTime.Now;
                    updatetitem.UpdatedBy = uid;
                    updatetitem.Resource = Model.RAResource.Trim();
                    unitOfWork.RAResource.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool RemoveResource(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.RAResource.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }


        //------------------------------------Retirement Phase---------------------------------//
        public static IEnumerable<RetirementPhaseModel> getAllRetirementPhases(int ClientId)
        {
            IList<RetirementPhaseModel> ListModel = new List<RetirementPhaseModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<RARetirementPhase> EntityList = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (ClientId != 0)
                    EntityList = unitOfWork.RARetirementPhase.Get(cid => cid.ClientId == ClientId).ToList();

                if (EntityList != null)
                {
                    foreach (var _entity in EntityList)
                    {
                        RetirementPhaseModel model = new RetirementPhaseModel();
                        model.Id = _entity.Id;
                        model.RetirementPhase = _entity.RetirementPhase;
                        ListModel.Add(model);
                    }
                }
                return ListModel;
            }
        }


        public static int CreateRetirementPhase(RetirementPhaseModel Model, int ClientId, int uid)
        {
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var entity = new RARetirementPhase();
                    entity.ClientId = ClientId;
                    entity.RetirementPhase = Model.RetirementPhase.Trim();
                    entity.CreatedDt = DateTime.Now;
                    entity.UpdatedDt = DateTime.Now;
                    entity.UpdatedBy = uid;
                    entity.CreatedBy = uid;
                    unitOfWork.RARetirementPhase.Insert(entity);
                    unitOfWork.Save();
                    id = entity.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool UpdateRetirementPhase(RetirementPhaseModel Model, int uid)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.RARetirementPhase.GetByID(Model.Id);
                    updatetitem.UpdatedDt = DateTime.Now;
                    updatetitem.UpdatedBy = uid;
                    updatetitem.RetirementPhase = Model.RetirementPhase.Trim();
                    unitOfWork.RARetirementPhase.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool RemoveRetirementPhase(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.RARetirementPhase.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }

        //------------------------------------Retirement Stage---------------------------------//
        public static IEnumerable<RetirementStageModel> getAllRetirementStages(int ClientId)
        {
            IList<RetirementStageModel> ListModel = new List<RetirementStageModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<RARetirementStage> EntityList = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (ClientId != 0)
                    EntityList = unitOfWork.RARetirementStage.Get(cid => cid.ClientId == ClientId).ToList();

                if (EntityList != null)
                {
                    foreach (var _entity in EntityList)
                    {
                        RetirementStageModel model = new RetirementStageModel();
                        model.Id = _entity.Id;
                        model.RetirementPhaseId = _entity.RetirementPhaseId;
                        model.RetirementStage = _entity.RetirementStage;
                        ListModel.Add(model);
                    }
                }
                return ListModel;
            }
        }


        public static int CreateRetirementStage(RetirementStageModel Model, int ClientId, int uid)
        {
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var entity = new RARetirementStage();
                    entity.ClientId = ClientId;
                    entity.RetirementPhaseId = Model.RetirementPhaseId;
                    entity.RetirementStage = Model.RetirementStage.Trim();
                    entity.CreatedDt = DateTime.Now;
                    entity.UpdatedDt = DateTime.Now;
                    entity.UpdatedBy = uid;
                    entity.CreatedBy = uid;
                    unitOfWork.RARetirementStage.Insert(entity);
                    unitOfWork.Save();
                    id = entity.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool UpdateRetirementStage(RetirementStageModel Model, int uid)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.RARetirementStage.GetByID(Model.Id);
                    updatetitem.UpdatedDt = DateTime.Now;
                    updatetitem.UpdatedBy = uid;
                    updatetitem.RetirementPhaseId = Model.RetirementPhaseId;
                    updatetitem.RetirementStage = Model.RetirementStage.Trim();
                    unitOfWork.RARetirementStage.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveRetirementStage(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.RARetirementStage.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }

        //------------------------------------WBS Status---------------------------------//
        public static IEnumerable<WbsStatusModel> getAllWbsStatus(int ClientId)
        {
            IList<WbsStatusModel> ListModel = new List<WbsStatusModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<RAWbsStatus> EntityList = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (ClientId != 0)
                    EntityList = unitOfWork.RAWbsStatus.Get(cid => cid.ClientId == ClientId).ToList();

                if (EntityList != null)
                {
                    foreach (var _entity in EntityList)
                    {
                        WbsStatusModel model = new WbsStatusModel();
                        model.Id = _entity.Id;
                        model.WbsStatus = _entity.WbsStatus;
                        ListModel.Add(model);
                    }
                }
                return ListModel;
            }
        }


        public static int CreateWbsStatus(WbsStatusModel Model, int ClientId, int uid)
        {
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var entity = new RAWbsStatus();
                    entity.ClientId = ClientId;
                    entity.WbsStatus = Model.WbsStatus.Trim();
                    entity.CreatedDt = DateTime.Now;
                    entity.UpdatedDt = DateTime.Now;
                    entity.UpdatedBy = uid;
                    entity.CreatedBy = uid;
                    unitOfWork.RAWbsStatus.Insert(entity);
                    unitOfWork.Save();
                    id = entity.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool UpdateWbsStatus(WbsStatusModel Model, int uid)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.RAWbsStatus.GetByID(Model.Id);
                    updatetitem.UpdatedDt = DateTime.Now;
                    updatetitem.UpdatedBy = uid;
                    updatetitem.WbsStatus = Model.WbsStatus.Trim();
                    unitOfWork.RAWbsStatus.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveWbsStatus(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.RAWbsStatus.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }


        //------------------------------------Work Product---------------------------------//
        public static IEnumerable<WorkProductModel> getAllWorkProducts(int ClientId)
        {
            IList<WorkProductModel> ListModel = new List<WorkProductModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<RAWorkProduct> EntityList = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (ClientId != 0)
                    EntityList = unitOfWork.RAWorkProduct.Get(cid => cid.ClientId == ClientId).ToList();

                if (EntityList != null)
                {
                    foreach (var _entity in EntityList)
                    {
                        WorkProductModel model = new WorkProductModel();
                        model.Id = _entity.Id;
                        model.WorkProduct = _entity.WorkProduct;
                        model.Url = _entity.Url;
                        ListModel.Add(model);
                    }
                }
                return ListModel;
            }
        }


        public static int CreateWorkProduct(WorkProductModel Model, int ClientId, int uid)
        {
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var entity = new RAWorkProduct();
                    entity.ClientId = ClientId;
                    entity.WorkProduct = Model.WorkProduct.Trim();
                    entity.Url = Model.Url.Trim();
                    entity.CreatedDt = DateTime.Now;
                    entity.UpdatedDt = DateTime.Now;
                    entity.UpdatedBy = uid;
                    entity.CreatedBy = uid;
                    unitOfWork.RAWorkProduct.Insert(entity);
                    unitOfWork.Save();
                    id = entity.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool UpdateWorkProduct(WorkProductModel Model, int uid)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.RAWorkProduct.GetByID(Model.Id);
                    updatetitem.UpdatedDt = DateTime.Now;
                    updatetitem.UpdatedBy = uid;
                    updatetitem.WorkProduct = Model.WorkProduct.Trim();
                    updatetitem.Url = Model.Url.Trim();
                    unitOfWork.RAWorkProduct.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveWorkProduct(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.RAWorkProduct.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }


        //------------------------------------Project Health---------------------------------//
        public static IEnumerable<ProjectHealthModel> getAllProjectHealth(int ClientId)
        {
            IList<ProjectHealthModel> ListModel = new List<ProjectHealthModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<RAProjectHealth> EntityList = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (ClientId != 0)
                    EntityList = unitOfWork.RAProjectHealth.Get(cid => cid.ClientId == ClientId).ToList();

                if (EntityList != null)
                {
                    foreach (var _entity in EntityList)
                    {
                        ProjectHealthModel model = new ProjectHealthModel();
                        model.Id = _entity.Id;
                        model.ProjectHealth = _entity.ProjectHealth;
                        model.Color = _entity.Color;
                        ListModel.Add(model);
                    }
                }
                return ListModel;
            }
        }


        public static int CreateProjectHealth(ProjectHealthModel Model, int ClientId, int uid)
        {
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var entity = new RAProjectHealth();
                    entity.ClientId = ClientId;
                    entity.ProjectHealth = Model.ProjectHealth.Trim();
                    entity.Color = Model.Color.Trim();
                    entity.CreatedDt = DateTime.Now;
                    entity.UpdatedDt = DateTime.Now;
                    entity.UpdatedBy = uid;
                    entity.CreatedBy = uid;
                    unitOfWork.RAProjectHealth.Insert(entity);
                    unitOfWork.Save();
                    id = entity.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool UpdateProjectHealth(ProjectHealthModel Model, int uid)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.RAProjectHealth.GetByID(Model.Id);
                    updatetitem.UpdatedDt = DateTime.Now;
                    updatetitem.UpdatedBy = uid;
                    updatetitem.ProjectHealth = Model.ProjectHealth.Trim();
                    updatetitem.Color = Model.Color.Trim();
                    unitOfWork.RAProjectHealth.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveProjectHealth(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.RAProjectHealth.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }
        //-----------------------------Estimate Category------------------------------------//
        public static IList<EstimateCategoryModel> GetEstimateCategory(int ClientId)
        {

            IList<EstimateCategoryModel> EstimateCategoryList = new List<EstimateCategoryModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<RAEstimateCategory> EstimateCategoryDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (ClientId != 0)
                    EstimateCategoryDetails = unitOfWork.RAEstimateCategory.Get(cid => cid.ClientId == ClientId).ToList();

                if (EstimateCategoryDetails != null)
                {
                    foreach (var _estimatedetail in EstimateCategoryDetails)
                    {
                        EstimateCategoryModel bb = new EstimateCategoryModel();
                        bb.Id = _estimatedetail.Id;
                        bb.EstimateCategory = _estimatedetail.EstimateCategory;
                        EstimateCategoryList.Add(bb);
                    }
                }
                return EstimateCategoryList;
            }

        }

        public static int CreateEstimateCategory(EstimateCategoryModel raEstimateCategoryInput, int ClientId, int uid)
        {
            int ra_id = 0;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var estimateDetails = new RAEstimateCategory();
                    estimateDetails.EstimateCategory = raEstimateCategoryInput.EstimateCategory;
                    estimateDetails.CreatedBy = uid;
                    estimateDetails.UpdatedBy = uid;
                    estimateDetails.ClientId = ClientId;
                    estimateDetails.CreatedDt = DateTime.Now;
                    estimateDetails.UpdatedDt = DateTime.Now;
                    unitOfWork.RAEstimateCategory.Insert(estimateDetails);
                    unitOfWork.Save();
                    ra_id = estimateDetails.Id;

                    return ra_id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }



        public static bool UpdateEstimateCategory(EstimateCategoryModel raEstimateCategoryInput, int uid)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.RAEstimateCategory.GetByID(raEstimateCategoryInput.Id);
                    updatetitem.EstimateCategory = raEstimateCategoryInput.EstimateCategory;
                    updatetitem.UpdatedBy = uid;
                    updatetitem.UpdatedDt = DateTime.Now;
                    unitOfWork.RAEstimateCategory.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static bool RemoveEstimateCategory(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.RAEstimateCategory.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }


        public static IEnumerable<SelectListItem> getRetirePhasesList(int ClientId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var Templist = unitOfWork.RARetirementPhase.Get(g => g.ClientId == ClientId).ToList();
            var list = Templist.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.RetirementPhase });
            return list;
        } 

        //------------------------------------Retirement Canditate---------------------------------//
        public static IEnumerable<RetirementApplicationModel> getAllRetirementCandidates(int ClientId)
        {
            IList<RetirementApplicationModel> ListModel = new List<RetirementApplicationModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<Application> EntityList = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (ClientId != 0)
                    EntityList = unitOfWork.Application.Get(f => f.ClientId == ClientId && f.IsRetirementCandidate == true).ToList();

                if (EntityList != null)
                {
                    foreach (var _entity in EntityList)
                    {
                        RetirementApplicationModel model = new RetirementApplicationModel();
                        model.Id = _entity.Id;
                        model.ApplicationName = _entity.ApplicationName;
                        //model.TechnicalOwner = _entity.TechnicalOwner;
                        ListModel.Add(model);
                    }
                }
                return ListModel;
            }
        }


        public static int CreateRetirementCandidate(RetirementApplicationModel Model, int ClientId, int uid)
        {
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var entity = new Application();
                    entity.ClientId = ClientId;
                    entity.InScope = true;
                    entity.ApplicationName = Model.ApplicationName.Trim();
                    //entity.TechnicalOwner = Model.TechnicalOwner.Trim();
                    entity.CreatedDatetime = DateTime.Now;
                    entity.LastModifiedDatetime = DateTime.Now;
                    entity.IsRetirementCandidate = true;
                    //entity.LastModifiedBy = uid;
                    //entity.CreatedBy = uid;
                    unitOfWork.Application.Insert(entity);
                    unitOfWork.Save();
                    id = entity.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool UpdateRetirementCandidate(RetirementApplicationModel Model, int uid)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.Application.GetByID(Model.Id);
                    updatetitem.LastModifiedDatetime = DateTime.Now;
                    //updatetitem.LastModifiedBy = uid;
                    updatetitem.ApplicationName = Model.ApplicationName.Trim();
                    //updatetitem.TechnicalOwner = Model.TechnicalOwner.Trim();
                    unitOfWork.Application.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveRetirementCandidate(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.Application.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }


        //WBS Template and Details


        /// <summary>
        /// Roles - For dropdownList in WBS Template
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> getRolesList(int ClientId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var Templist = unitOfWork.RARoles.Get(g => g.ClientId == ClientId).ToList();
            var list = Templist.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.RoleName });
            return list;
        }

        /// <summary>
        /// Resource Group - For dropdownList in WBS Template
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> getResourceGroupsList(int ClientId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var Templist = unitOfWork.RAResourceGroup.Get(g => g.ClientId == ClientId).ToList();
            var list = Templist.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.ResourceGroup });
            return list;
        }

        /// <summary>
        /// Work Product - For dropdownList in WBS Template
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> getWorkProductsList(int ClientId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var Templist = unitOfWork.RAWorkProduct.Get(g => g.ClientId == ClientId).ToList();
            var list = Templist.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.WorkProduct });
            return list;
        }



        public static IEnumerable<SelectListItem> getAllWbsTemplates(int ClientId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var tempList = unitOfWork.RAWbsTemplate.Get(g => g.ClientId == ClientId);
            var list = tempList.Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.WBSName });
            return list;
        }

        public static IEnumerable<SelectListItem> getApplications(int ClientId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var tempList = unitOfWork.Application.Get(g => g.ClientId == ClientId && g.IsRetirementCandidate == true);
            var list = tempList.Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.ApplicationName });
            return list;
        }

        public static IEnumerable<WbsTemplateDetailsModel> GetAllWbsTasks(int WbsId)
        {
            //var contextFactory = new DbContextFactory();
            //var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();
            var templatesList = (from Wbs in db.RAWbsTemplateDetails

                                 join Resource in db.RAResourceGroup on Wbs.ResourceGroupId equals Resource.Id into tempRG

                                 join Role in db.RARoles on Wbs.RoleId equals Role.Id into tempRole

                                 join WorkProd in db.RAWorkProduct on Wbs.WorkProductId equals WorkProd.Id into tempWP

                                 where Wbs.WBSTemplateId == WbsId

                                 from Resource in tempRG.DefaultIfEmpty()

                                 from Role in tempRole.DefaultIfEmpty()

                                 from WorkProd in tempWP.DefaultIfEmpty()
                                     
                                 select new
                                 {
                                     TaskId = Wbs.Id,
                                     Task = Wbs.Task,
                                     ClientId = Wbs.ClientId,
                                     TParentId = Wbs.ParentId,
                                    ResourceGroupId = Resource == null ? 0 : Resource.Id,
                                     ResourceGroup = Resource.ResourceGroup,
                                     RoleId = Role == null ? 0 : Role.Id,
                                     Role = Role.RoleName,
                                     WBSTemplateId = Wbs.WBSTemplateId,
                                     WorkProductId = WorkProd == null ? 0 : WorkProd.Id,
                                     WorkProduct = WorkProd.WorkProduct
                                
        }
                ).ToList();
            //var templatesList = unitOfWork.RAWbsTemplateDetails.Get(g => g.WBSTemplateId == WbsId);
            var templatesList_1 = templatesList.Select(c => new WbsTemplateDetailsModel
            {
                TaskId = c.TaskId,
                Task = c.Task,
                ClientId = c.ClientId,
                TParentId = c.TParentId,
                ResourceGroupId = c.ResourceGroupId,
                RoleId = c.RoleId,
                ResourceGroup = c.ResourceGroup,
                Role = c.Role,
                WBSTemplateId = c.WBSTemplateId,
                WorkProductId = c.WorkProductId,
                WorkProduct = c.WorkProduct
            });
            return templatesList_1;
        }

        /// <summary>
        /// Returns True if WBS Template Name Exists
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="WbsTempName"></param>
        /// <returns></returns>
        public static bool checkWbsNameExists(int ClientId, string WbsTempName)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var check = unitOfWork.RAWbsTemplate.Get(g => g.ClientId == ClientId && g.WBSName.Trim() == WbsTempName.Trim()).Any();
            return check;
        }

        /// <summary>
        /// Add new WBS Template Name
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="uid"></param>
        /// <param name="WbsTempName"></param>
        /// <returns>Int ID of newly created WBS Template</returns>
        public static int AddWbsTempName(int ClientId, int uid, string WbsTempName)
        {
            try
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var new_Id = 0;
                var entity = new RAWbsTemplate()
                {
                    WBSName = WbsTempName.Trim(),
                    ClientId = ClientId,
                    CreatedBy = uid,
                    UpdatedBy = uid,
                    CreatedDt = DateTime.Now,
                    UpdatedDt = DateTime.Now
                };
                unitOfWork.RAWbsTemplate.Insert(entity);
                unitOfWork.Save();
                new_Id = entity.Id;
                return new_Id;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        /// <summary>
        /// Deletes all WBS tasks and Wbs Template Name
        /// </summary>
        /// <param name="WbsId"></param>
        public static void deleteWbsTemplate(int WbsId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var AllWbsTasks = unitOfWork.RAWbsTemplateDetails.Get(g => g.WBSTemplateId == WbsId);
            foreach (var task in AllWbsTasks)
            {
                unitOfWork.RAWbsTemplateDetails.Delete(task.Id);
            }
            unitOfWork.Save();//required
            unitOfWork.RAWbsTemplate.Delete(WbsId);
            unitOfWork.Save();
        }





        //Helper to Create New Template/Task
        public static int AddWbsTask(WbsTemplateDetailsModel template, int ClientId, int uid, int roleid, int rg, int wp)
        {
            var NewId = 0;
            if (template != null)
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var cv = new RAWbsTemplateDetails();
                cv.Task = template.Task;
                cv.ClientId = ClientId;
                cv.ParentId = template.TParentId;
                cv.WBSTemplateId = template.WBSTemplateId;

                cv.ResourceGroupId = rg;
                cv.RoleId = roleid;
                cv.WorkProductId = wp;


                cv.CreatedBy = uid;
                cv.UpdatedBy = uid;
                cv.CreatedDt = DateTime.Now;
                cv.UpdatedDt = DateTime.Now;
                unitOfWork.RAWbsTemplateDetails.Insert(cv);
                unitOfWork.Save();
                NewId = cv.Id;
            }
            return NewId;
        }

        /// <summary>
        /// Helper to Update Task/Sub-tasks
        /// </summary>
        /// <param name="template"></param>
        /// <param name="uid"></param>
        public static void EditWbsTask(WbsTemplateDetailsModel template, int uid,int roleid, int rg, int wp)
        {
            if (template != null)
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var cv = unitOfWork.RAWbsTemplateDetails.GetByID(template.TaskId);
                cv.Task = template.Task;

                cv.ResourceGroupId = rg;
                cv.RoleId = roleid;
                cv.WorkProductId = wp;

                cv.UpdatedBy = uid;
                cv.UpdatedDt = DateTime.Now;
                unitOfWork.RAWbsTemplateDetails.Update(cv);
                unitOfWork.Save();
            }
        }

        /// <summary>
        /// Helper to Delete the WBS_Template/Phase/Progress
        /// </summary>
        /// <param name="template"></param>
        public static void DeleteWbsTask(int WbsTaskId)
        {
            if (WbsTaskId > 0)
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.RAWbsTemplateDetails.Delete(WbsTaskId);
                unitOfWork.Save();
            }
        }
        //-------------------Estimate--------------------------------------

        public static IEnumerable<ArchivalDecommissionModel> FetchEstimateDetails(int AppId, int ClientId, int uid)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            IEnumerable<ArchivalDecommissionModel> returnList = new List<ArchivalDecommissionModel>();
            var latestSurvey = unitOfWork.Survey.Get(g => g.SurveyType.ToLower() == "ra" && g.ClientId == ClientId).OrderByDescending(f => f.Createddt).FirstOrDefault();
            Db db = new Db();
            if (latestSurvey != null && AppId != 0)
            {
                var SurveyId = latestSurvey.Id;
          
                    var tempList = (from SM in db.SurveyModel
                                    where SM.SurveyId == SurveyId

                                    join ASS in db.RAAssessment on SM.Id equals ASS.SurveyModelId
                                    join ANS in db.SurveyModelRatingFactor on ASS.FinalRating equals ANS.Id
                                    join estType in db.EstimateType on ANS.EstimateTypeId equals estType.Id into tempEsti
                                    from EstimeteType in tempEsti.DefaultIfEmpty()
                                    where ASS.RAApplicationid == AppId
                                    select new ArchivalDecommissionModel
                                    {
                                        RAApplicationid = AppId,
                                        subPerspective = SM.QuestionIdentifier,
                                        Question = SM.Question,
                                        Hint = SM.Hint,
                                        FinalRating = ANS.RatingFactorValue,
                                        AdjRationale = ASS.AdjRationale,
                                        FinalRatingROMEstimate= ANS.ROMEstimate,
                                        Id = ASS.Id,
                                        AdjROMEstimate=ASS.EstAdjHrs,
                                        EstimateAssumption=ANS.EstimateAssumption,
                                        EstimateType= EstimeteType.EstimateType,
                                        FinalRatingFactorValue=ANS.RatingFactor,
                                        SurveyModelId=SM.Id
                                        
                                    }

                        ).ToList();
                
                    returnList = tempList;
                }
               

            return returnList;

        }

        public static void EstimateUpdate(ArchivalDecommissionModel model, int uid)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var exists = unitOfWork.RAAssessment.GetByID(model.Id);
            if (exists != null)
            {
                exists.EstAdjHrs = model.AdjROMEstimate;
                exists.UpdatedBy = uid;
                exists.UpdatedDt = DateTime.Now;
                exists.AdjRationale = model.AdjRationale;
                unitOfWork.RAAssessment.Update(exists);
                unitOfWork.Save();
            }
           
        }


        //------------------End Estimate----------------------------------


        //--------------------------------------------------------Assessment-------------------------------------------------------------//

        public static IEnumerable<AssessmentModel> FetchAssessDetails(int AppId, int ClientId, int uid)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            IEnumerable<AssessmentModel> returnList = new List<AssessmentModel>();
            var latestSurvey = unitOfWork.Survey.Get(g => g.SurveyType.ToLower() == "ra" && g.ClientId == ClientId).OrderByDescending(f => f.Createddt).FirstOrDefault();
            Db db = new Db();
            if (latestSurvey != null && AppId != 0)
            {
                var SurveyId = latestSurvey.Id;
                //Check any RAAssessment is there or not!!!
                var IsThere = unitOfWork.RAAssessment.Get(g => g.RAApplicationid == AppId).Any();
                if (IsThere)
                {
                    //With Linq query
                    var tempList = (from SM in db.SurveyModel
                                    where SM.SurveyId == SurveyId
                                    join ASS in db.RAAssessment on SM.Id equals ASS.SurveyModelId into tempAssess
                                    from Assess in tempAssess.DefaultIfEmpty()
                                    where Assess.RAApplicationid == AppId
                                    select new
                                    {
                                        ApplicationId = AppId,
                                        subPerspective = SM.QuestionIdentifier,
                                        Question = SM.Question,
                                        OriginalRating = Assess.OriginalRating,
                                        FinalRating = Assess.FinalRating,
                                        Comments = Assess.Comments,
                                        Options = (from op in db.SurveyModelRatingFactor
                                                   where op.SurveyModelId == SM.Id
                                                   select new
                                                   {
                                                       RatingText = op.RatingFactor,
                                                       RatingId = op.Id
                                                   }),
                                        Id = Assess.Id
                                    }

                        ).ToList();

                    var returnTempList = tempList.AsEnumerable().Select(e => new AssessmentModel
                    {
                        Id = e.Id,
                        subPerspective = e.subPerspective,
                        Question = e.Question,
                        OriginalRating = e.OriginalRating,
                        FinalRating = e.FinalRating,
                        Comments = e.Comments,
                        QuestionOptions = e.Options.ToList().Select(f => new ratingMod
                        {
                            Id = f.RatingId,
                            Rating = f.RatingText
                        }),
                        RAApplicationid = e.ApplicationId
                    });
                    returnList = returnTempList;
                }
                else
                {
                    var tempList = (from SM in db.SurveyModel
                                    where SM.SurveyId == SurveyId
                                    select new
                                    {
                                        ApplicationId = AppId,
                                        //subPerspective = SM.QuestionIdentifier,
                                        SurveyModelId = SM.Id,
                                        //Question = SM.Question,
                                        //OriginalRating = Assess.OriginalRating,
                                        //FinalRating = Assess.FinalRating,
                                        //Comments = Assess.Comments,
                                        //Options = (from op in db.SurveyModelRatingFactor
                                        //           where op.SurveyModelId == SM.Id
                                        //           select new
                                        //           {
                                        //               RatingText = op.RatingFactor,
                                        //               RatingId = op.Id
                                        //           }),
                                        //Id = Assess.Id
                                    }

                        ).ToList();

                    //After fetching the SurveyQuestions, write it to database.
                    foreach (var ra in tempList)
                    {
                        var entity = new RAAssessment();
                        entity.RAApplicationid = ra.ApplicationId;
                        entity.SurveyModelId = ra.SurveyModelId;
                        //entity.OriginalRating = ra.OriginalRating;
                        //entity.FinalRating = ra.FinalRating;
                        //entity.Comments = ra.Comments;
                        entity.CreatedBy = uid;
                        entity.UpdatedBy = uid;
                        entity.CreatedDt = DateTime.Now;
                        entity.UpdatedDt = DateTime.Now;
                        unitOfWork.RAAssessment.Insert(entity);
                        unitOfWork.Save();
                    }

                    //Read agan with joins to Questions and Ratings
                    var AssessmentList = (from SM in db.SurveyModel
                                          where SM.SurveyId == SurveyId
                                          join ASS in db.RAAssessment on SM.Id equals ASS.SurveyModelId into tempAssess
                                          from Assess in tempAssess.DefaultIfEmpty()
                                          where Assess.RAApplicationid == AppId
                                          select new
                                          {
                                              ApplicationId = AppId,
                                              subPerspective = SM.QuestionIdentifier,
                                              SurveyModelId = SM.Id,
                                              Question = SM.Question,
                                              OriginalRating = Assess.OriginalRating,
                                              FinalRating = Assess.FinalRating,
                                              Comments = Assess.Comments,
                                              Options = (from op in db.SurveyModelRatingFactor
                                                         where op.SurveyModelId == SM.Id
                                                         select new
                                                         {
                                                             RatingText = op.RatingFactor,
                                                             RatingId = op.Id
                                                         }),
                                              Id = Assess.Id
                                          }

                        ).ToList();

                    var returnTempList = AssessmentList.Select(e => new AssessmentModel
                    {
                        Id = e.Id,
                        subPerspective = e.subPerspective,
                        Question = e.Question,
                        OriginalRating = e.OriginalRating,
                        FinalRating = e.FinalRating,
                        Comments = e.Comments,
                        QuestionOptions = e.Options.ToList().Select(f => new ratingMod
                        {
                            Id = f.RatingId,
                            Rating = f.RatingText
                        }),
                        RAApplicationid = e.ApplicationId
                    });
                    returnList = returnTempList;
                }


            }

            return returnList;

        }

        //To create 
        public static void AssessUpdate(AssessmentModel model, int uid)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var exists = unitOfWork.RAAssessment.GetByID(model.Id);
            if (exists != null && (exists.OriginalRating != model.OriginalRating || exists.FinalRating != model.FinalRating || exists.Comments != model.Comments))
            {
                exists.OriginalRating = model.OriginalRating;
                exists.FinalRating = model.FinalRating;
                exists.UpdatedBy = uid;
                exists.UpdatedDt = DateTime.Now;
                exists.Comments = model.Comments;
                unitOfWork.RAAssessment.Update(exists);
                unitOfWork.Save();
            }
            //else
            //{
            //    var entity = new RAAssessment();
            //    entity.RAApplicationid = model.RAApplicationid;
            //    entity.SurveyModelId = model.SurveyModelId;
            //    entity.OriginalRating = model.OriginalRating;
            //    entity.FinalRating = model.FinalRating;
            //    entity.Comments = model.Comments;
            //    entity.CreatedBy = uid;
            //    entity.UpdatedBy = uid;
            //    entity.CreatedDt = DateTime.Now;
            //    entity.UpdatedDt = DateTime.Now;
            //    unitOfWork.RAAssessment.Insert(entity);
            //    unitOfWork.Save();
            //    return entity.Id;
            //}
        }


        //-----------------------RA Disposition and Recommendation---------------------//
        public static AssessDispRecomModel getDispDetails(int AppId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            AssessDispRecomModel model = new AssessDispRecomModel();

            var CommonDetails = unitOfWork.RADispCandidate.Get(g => g.RAApplicationId == AppId).FirstOrDefault();
            if (CommonDetails != null)
            {
                //Common Details
                model.RAApplicationId = AppId;
                model.IsDeCommissioned = CommonDetails.IsDeCommissioned;
                model.ArchiveData = CommonDetails.ArchiveData;
                model.ReplacementScopeId = CommonDetails.ReplacementScopeId;

                var Details = unitOfWork.RADispRecommendation.Get(g => g.RADispCandidateId == CommonDetails.Id);
                foreach (var en in Details)
                {
                    switch (en.RecommendationType)
                    {
                        //Recommendation Details
                        case (int)RecommendationType.Replace:
                            model.ReplaceAction = en.Action;
                            model.ReplaceEstimate = en.Estimate;
                            model.ReplaceRecommendation = en.Recommendation;
                            break;

                        case (int)RecommendationType.Archival:
                            model.ArchivalAction = en.Action;
                            model.ArchivalEstimate = en.Estimate;
                            model.ArchivalRecommendation = en.Recommendation;
                            break;

                        case (int)RecommendationType.Decommission:
                            model.DecommAction = en.Action;
                            model.DecommEstimate = en.Estimate;
                            model.DecommRecommendation = en.Recommendation;
                            break;
                    }

                }

            }
            return model;

        }

        //RA Disp -Recommendation save
        public static bool SaveDispDetails(AssessDispRecomModel model, int uid)
        {
            try
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var fetch = unitOfWork.RADispCandidate.Get(g => g.RAApplicationId == model.RAApplicationId).Any();
                if (!fetch)
                {
                    RADispCandidate entity = new RADispCandidate();
                    entity.ArchiveData = model.ArchiveData;
                    entity.RAApplicationId = model.RAApplicationId;
                    entity.ReplacementScopeId = model.ReplacementScopeId;
                    entity.IsDeCommissioned = model.IsDeCommissioned;
                    entity.CreatedBy = uid;
                    entity.CreatedDt = DateTime.Now;
                    entity.UpdatedBy = uid;
                    entity.UpdatedDt = DateTime.Now;
                    unitOfWork.RADispCandidate.Insert(entity);

                    unitOfWork.Save();

                    //Details

                    RADispRecommendation ReplaceEntity = new RADispRecommendation();
                    RADispRecommendation ArchiveEntity = new RADispRecommendation();
                    RADispRecommendation DecommEntity = new RADispRecommendation();
                    ReplaceEntity.RADispCandidateId = entity.Id;
                    ArchiveEntity.RADispCandidateId = entity.Id;
                    DecommEntity.RADispCandidateId = entity.Id;

                    ReplaceEntity.Recommendation = model.ReplaceRecommendation;
                    ArchiveEntity.Recommendation = model.ArchivalRecommendation;
                    DecommEntity.Recommendation = model.DecommRecommendation;

                    ReplaceEntity.RecommendationType = (int)RecommendationType.Replace;
                    ArchiveEntity.RecommendationType = (int)RecommendationType.Archival;
                    DecommEntity.RecommendationType = (int)RecommendationType.Decommission;

                    ReplaceEntity.Action = model.ReplaceAction;
                    ArchiveEntity.Action = model.ArchivalAction;
                    DecommEntity.Action = model.DecommAction;

                    ReplaceEntity.Estimate = model.ReplaceEstimate;
                    ArchiveEntity.Estimate = model.ArchivalEstimate;
                    DecommEntity.Estimate = model.DecommEstimate;

                    ReplaceEntity.CreatedBy = uid;
                    ReplaceEntity.CreatedDt = DateTime.Now;

                    ArchiveEntity.CreatedBy = uid;
                    ArchiveEntity.CreatedDt = DateTime.Now;

                    DecommEntity.CreatedBy = uid;
                    DecommEntity.CreatedDt = DateTime.Now;

                    ReplaceEntity.UpdatedBy = uid;
                    ReplaceEntity.UpdatedDt = DateTime.Now;

                    ArchiveEntity.UpdatedBy = uid;
                    ArchiveEntity.UpdatedDt = DateTime.Now;

                    DecommEntity.UpdatedBy = uid;
                    DecommEntity.UpdatedDt = DateTime.Now;

                    unitOfWork.RADispRecommendation.Insert(ReplaceEntity);
                    unitOfWork.RADispRecommendation.Insert(ArchiveEntity);
                    unitOfWork.RADispRecommendation.Insert(DecommEntity);
                    unitOfWork.Save();
                }
                else
                {
                    var InDB = unitOfWork.RADispCandidate.Get(g => g.RAApplicationId == model.RAApplicationId).FirstOrDefault();
                    InDB.IsDeCommissioned = model.IsDeCommissioned;
                    InDB.ReplacementScopeId = model.ReplacementScopeId;
                    InDB.ArchiveData = model.ArchiveData;
                    unitOfWork.RADispCandidate.Update(InDB);

                    var inDBDetails = unitOfWork.RADispRecommendation.Get(g => g.RADispCandidateId == InDB.Id).ToList();
                    foreach (var rec in inDBDetails)
                    {
                        switch (rec.RecommendationType)
                        {
                            case (int)RecommendationType.Replace:
                                rec.Recommendation = model.ReplaceRecommendation;
                                rec.Action = model.ReplaceAction;
                                rec.Estimate = model.ReplaceEstimate;
                                break;
                            case (int)RecommendationType.Archival:
                                rec.Recommendation = model.ArchivalRecommendation;
                                rec.Action = model.ArchivalAction;
                                rec.Estimate = model.ArchivalEstimate;
                                break;
                            case (int)RecommendationType.Decommission:
                                rec.Recommendation = model.DecommRecommendation;
                                rec.Action = model.DecommAction;
                                rec.Estimate = model.DecommEstimate;
                                break;
                        }

                        unitOfWork.RADispRecommendation.Update(rec);
                    }
                    unitOfWork.Save();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// Save Notes for RetiredApp in Disposition Recommendation
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="EncodeString"></param>
        public static void saveNotes(int Id, string EncodeString)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var AnalysisExists = unitOfWork.Application.Get(g => g.Id == Id).FirstOrDefault();
            if (AnalysisExists != null)
            {
                AnalysisExists.GeneralNotes = EncodeString;
                unitOfWork.Application.Update(AnalysisExists);
            }
            unitOfWork.Save();
        }

        public static string getNotesDetails(int AppId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Application model = new Application();
            //var c = unitOfWork.Application.GetByID(model.GeneralNotes);
            var CommonDetails = unitOfWork.Application.Get(g => g.Id == AppId  ).FirstOrDefault();
            if (CommonDetails != null)
            {
                //Common Details
                model.GeneralNotes = CommonDetails.GeneralNotes;
            }
            var c = model.GeneralNotes;
            return c;

        }

        public static IEnumerable<ProjectManagementModel> FetchProjManageDetails(int AppId, int ClientId, int uid)
        {

            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            IEnumerable<ProjectManagementModel> returnList = new List<ProjectManagementModel>();
            Db db = new Db();
            if (AppId != 0)
            {
                //Check any Activities are there or not!!!
                var IsThere = unitOfWork.RAProjectManagement.Get(g => g.RAApplicationId == AppId).Any();
                if (IsThere)
                {
                    //With Linq query
                    var tempList = (from SM in db.RAProjectManagement
                                    join acti in db.RAProjectManagementActivity on SM.ProjectManagementActivityId equals acti.Id
                                    where SM.RAApplicationId == AppId
                                    select new
                                    {
                                        Activity = acti.Activity,
                                        ApplicationId = AppId,
                                        EstimateHours = SM.EstimatedHours,
                                        EstimatedCost = SM.EstimatedCost,
                                        EstimateAssumptions = SM.EstimateAssumptions,
                                        //Attachments = SM.Attachment,
                                        AttachmentName = SM.AttachmentName,
                                        Id = SM.Id
                                    }

                        ).ToList();

                    var returnTempList = tempList.AsEnumerable().Select(e => new ProjectManagementModel
                    {
                        Id = e.Id,
                        ProjActivity = e.Activity,
                        EstimatedHours = e.EstimateHours,
                        EstimatedCost = e.EstimatedCost,
                        EstimateAssumptions = e.EstimateAssumptions,
                        //Attachment = e.Attachments,
                        AttachmentName = e.AttachmentName

                    });
                    returnList = returnTempList;
                }
                else
                {
                    var tempList = (from SM in db.RAProjectManagementActivity
                                    select new
                                    {
                                        Id = SM.Id,
                                        Activity = SM.Activity
                                    }

                        ).ToList();

                    foreach (var ra in tempList)
                    {
                        var entity = new RAProjectManagement();
                        entity.RAApplicationId = AppId;
                        entity.ProjectManagementActivityId = ra.Id;
                        entity.CreatedBy = uid;
                        entity.UpdatedBy = uid;
                        entity.CreatedDt = DateTime.Now;
                        entity.UpdatedDt = DateTime.Now;
                        unitOfWork.RAProjectManagement.Insert(entity);
                        unitOfWork.Save();
                    }

                    var AssessmentList = (from SM in db.RAProjectManagement
                                          join acti in db.RAProjectManagementActivity on SM.ProjectManagementActivityId equals acti.Id
                                          where SM.RAApplicationId == AppId
                                          select new
                                          {
                                              Activity = acti.Activity,
                                              ApplicationId = AppId,
                                              EstimateHours = SM.EstimatedHours,
                                              EstimatedCost = SM.EstimatedCost,
                                              EstimateAssumptions = SM.EstimateAssumptions,
                                              //Attachments = SM.Attachment,
                                              AttachmentName = SM.AttachmentName,
                                              Id = SM.Id
                                          }

                        ).ToList();

                    var returnTempList = AssessmentList.AsEnumerable().Select(e => new ProjectManagementModel
                    {
                        Id = e.Id,
                        ProjActivity = e.Activity,
                        EstimatedHours = e.EstimateHours,
                        EstimatedCost = e.EstimatedCost,
                        EstimateAssumptions = e.EstimateAssumptions,
                        //Attachment = e.Attachments,
                        AttachmentName = e.AttachmentName

                    });
                    returnList = returnTempList;
                }


            }

            return returnList;
        }




        public static IEnumerable<SupportCostModel> FetchSupportCostDetails(int AppId, int ClientId, int uid)
        {

            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            IEnumerable<SupportCostModel> returnList = new List<SupportCostModel>();
            Db db = new Db();
            if (AppId != 0)
            {
                var IsThere = unitOfWork.RASupportCost.Get(g => g.RAApplicationId == AppId).Any();
                if (IsThere)
                {
                    //With Linq query
                    var tempList = (from SM in db.RASupportCost
                                    join acti in db.RASupportCostActivity on SM.SupportActivityId equals acti.Id
                                    where SM.RAApplicationId == AppId
                                    select new
                                    {
                                        Activity = acti.Activity,
                                        ApplicationId = AppId,
                                        SupportFTE = SM.AnnualFTE,
                                        BlendedRate = SM.BlendedRate,
                                        SupportCost = SM.AnnualSupportCost,
                                        FutSupportFTE = SM.FutureAnnualSupportFTE,
                                        FutBlendedRate = SM.FutureBlendedRate,
                                        FutSupportCost = SM.FutureAnnualSupportCost,
                                        Id = SM.Id
                                    }

                         ).ToList();

                    var returnTempList = tempList.AsEnumerable().Select(e => new SupportCostModel
                    {
                        Id = e.Id,
                        SupportActivity = e.Activity,
                        AnnualFTE = e.SupportFTE,
                        BlendedRate = e.BlendedRate,
                        AnnualSupportCost = e.SupportCost,
                        FutureAnnualSupportFTE = e.FutSupportFTE,
                        FutureBlendedRate = e.FutBlendedRate,
                        FutureAnnualSupportCost = e.FutSupportCost

                    });
                    returnList = returnTempList;
                }
                else
                {
                    var tempList = (from SM in db.RASupportCostActivity
                                    select new
                                    {
                                        Id = SM.Id,
                                        Activity = SM.Activity
                                    }

                        ).ToList();

                    foreach (var ra in tempList)
                    {
                        var entity = new RASupportCost();
                        entity.RAApplicationId = AppId;
                        entity.SupportActivityId = ra.Id;
                        entity.CreatedBy = uid;
                        entity.UpdatedBy = uid;
                        entity.CreatedDt = DateTime.Now;
                        entity.UpdatedDt = DateTime.Now;
                        unitOfWork.RASupportCost.Insert(entity);
                        unitOfWork.Save();
                    }


                    var AssessmentList = (from SM in db.RASupportCost
                                          join acti in db.RASupportCostActivity on SM.SupportActivityId equals acti.Id
                                          where SM.RAApplicationId == AppId
                                          select new
                                          {
                                              Activity = acti.Activity,
                                              ApplicationId = AppId,
                                              SupportFTE = SM.AnnualFTE,
                                              BlendedRate = SM.BlendedRate,
                                              SupportCost = SM.AnnualSupportCost,
                                              FutSupportFTE = SM.FutureAnnualSupportFTE,
                                              FutBlendedRate = SM.FutureBlendedRate,
                                              FutSupportCost = SM.FutureAnnualSupportCost,
                                              Id = SM.Id
                                          }

                         ).ToList();

                    var returnTempList = AssessmentList.AsEnumerable().Select(e => new SupportCostModel
                    {
                        Id = e.Id,
                        SupportActivity = e.Activity,
                        AnnualFTE = e.SupportFTE,
                        BlendedRate = e.BlendedRate,
                        AnnualSupportCost = e.SupportCost,
                        FutureAnnualSupportFTE = e.FutSupportFTE,
                        FutureBlendedRate = e.FutBlendedRate,
                        FutureAnnualSupportCost = e.FutSupportCost

                    });
                    returnList = returnTempList;
                }


            }

            return returnList;
        }



        public static void ProjManageUpdate(ProjectManagementModel model, int uid)
        {
            if (model.Id != 0)
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var entity = unitOfWork.RAProjectManagement.GetByID(model.Id);
                //entity.Attachment = model.Attachment;
                entity.EstimateAssumptions = model.EstimateAssumptions;
                entity.EstimatedCost = model.EstimatedCost;
                entity.EstimatedHours = model.EstimatedHours;
                entity.UpdatedBy = uid;
                entity.UpdatedDt = DateTime.Now;
                unitOfWork.RAProjectManagement.Update(entity);
                unitOfWork.Save();
            }

        }


        public static void SupportCostUpdate(SupportCostModel model, int uid)
        {
            if (model.Id != 0)
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var entity = unitOfWork.RASupportCost.GetByID(model.Id);
                entity.AnnualFTE = model.AnnualFTE;
                entity.BlendedRate = model.BlendedRate;
                entity.AnnualSupportCost = model.AnnualSupportCost;
                entity.FutureAnnualSupportFTE = model.FutureAnnualSupportFTE;
                entity.FutureBlendedRate = model.FutureBlendedRate;
                entity.FutureAnnualSupportCost = model.FutureAnnualSupportCost;
                entity.UpdatedBy = uid;
                entity.UpdatedDt = DateTime.Now;
                unitOfWork.RASupportCost.Update(entity);
                unitOfWork.Save();
            }
        }


        public static void ExecutionCostUpdate(ExecutionCostModel model, int uid)
        {
            if(model.Id!=null && model.Id != 0)
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var entity = unitOfWork.RAExecutionCost.GetByID(model.Id);
                //entity.Attachment = model.Attachment;
                entity.BlendedRate = model.BlendedRate;
                entity.EstimatedFTE = model.EstimatedFTE;
                entity.EstimatedCost = model.EstimatedCost;
                entity.EstimatedDuration = model.EstimatedDuration;
                entity.EstimatedHours = model.EstimatedHours;
                entity.UpdatedDt = DateTime.Now;
                entity.UpdatedBy = uid;
                unitOfWork.RAExecutionCost.Update(entity);
                unitOfWork.Save();

            }
        }

        public static IEnumerable<ExecutionCostModel> FetchExecutionCostDetails(int AppId, int ClientId, int uid)
        {

            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            IEnumerable<ExecutionCostModel> returnList = new List<ExecutionCostModel>();
            Db db = new Db();
            if (AppId != 0)
            {
                var IsThere = unitOfWork.RAExecutionCost.Get(g => g.RAApplicationId == AppId).Any();
                if (IsThere)
                {
                    //With Linq query
                    var tempList = (from SM in db.RAExecutionCost
                                    join acti in db.RAExecutionCostActivity on SM.ExecutionActivityId equals acti.Id
                                    where SM.RAApplicationId == AppId
                                    select new
                                    {
                                        Activity = acti.Activity,
                                        ApplicationId = AppId,
                                        EstimatedHours = SM.EstimatedHours,
                                        BlendedRate = SM.BlendedRate,
                                        EstimatedCost = SM.EstimatedCost,
                                        EstimatedDuration = SM.EstimatedDuration,
                                        EstimatedFTE = SM.EstimatedFTE,
                                        //Attachment = SM.Attachment,
                                        AttachmentName = SM.AttachmentName,
                                        Id = SM.Id
                                    }

                         ).ToList();

                    var returnTempList = tempList.AsEnumerable().Select(e => new ExecutionCostModel
                    {
                        Id = e.Id,
                        ExecutionActivity = e.Activity,
                        RAApplicationId = e.ApplicationId,
                        EstimatedFTE = e.EstimatedFTE,
                        BlendedRate = e.BlendedRate,
                        EstimatedCost = e.EstimatedCost,
                        EstimatedDuration = e.EstimatedDuration,
                        EstimatedHours = e.EstimatedHours,
                        //Attachment = e.Attachment,
                        AttachmentName = e.AttachmentName,
                    });
                    returnList = returnTempList;
                }
                else
                {
                    var tempList = (from SM in db.RAExecutionCostActivity
                                    select new
                                    {
                                        Id = SM.Id,
                                        Activity = SM.Activity
                                    }

                        ).ToList();

                    foreach (var ra in tempList)
                    {
                        var entity = new RAExecutionCost();
                        entity.RAApplicationId = AppId;
                        entity.ExecutionActivityId = ra.Id;
                        entity.CreatedBy = uid;
                        entity.UpdatedBy = uid;
                        entity.CreatedDt = DateTime.Now;
                        entity.UpdatedDt = DateTime.Now;
                        unitOfWork.RAExecutionCost.Insert(entity);
                        unitOfWork.Save();
                    }


                    var AssessmentList = (from SM in db.RAExecutionCost
                                          join acti in db.RAExecutionCostActivity on SM.ExecutionActivityId equals acti.Id
                                          where SM.RAApplicationId == AppId
                                          select new
                                          {
                                              Activity = acti.Activity,
                                              ApplicationId = AppId,
                                              EstimatedHours = SM.EstimatedHours,
                                              BlendedRate = SM.BlendedRate,
                                              EstimatedCost = SM.EstimatedCost,
                                              EstimatedDuration = SM.EstimatedDuration,
                                              EstimatedFTE = SM.EstimatedFTE,
                                              //Attachment = SM.Attachment,
                                              AttachmentName = SM.AttachmentName,
                                              Id = SM.Id
                                          }

                         ).ToList();

                    var returnTempList = AssessmentList.AsEnumerable().Select(e => new ExecutionCostModel
                    {
                        Id = e.Id,
                        ExecutionActivity = e.Activity,
                        RAApplicationId = e.ApplicationId,
                        EstimatedFTE = e.EstimatedFTE,
                        BlendedRate = e.BlendedRate,
                        EstimatedCost = e.EstimatedCost,
                        EstimatedDuration = e.EstimatedDuration,
                        EstimatedHours = e.EstimatedHours,
                        //Attachment = e.Attachment,
                        AttachmentName = e.AttachmentName,

                    });
                    returnList = returnTempList;
                }


            }

            return returnList;
        }

        public static IEnumerable<OngoingArchivalSupportModel> FetchOngoingArchivalDetails(int AppId, int ClientId, int uid)
        {

            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            IEnumerable<OngoingArchivalSupportModel> returnList = new List<OngoingArchivalSupportModel>();
            Db db = new Db();
            if (AppId != 0)
            {
                //Check any Activities are there or not!!!
                var IsThere = unitOfWork.RAOngoingArchivalSupport.Get(g => g.RAApplicationId == AppId).Any();
                if (IsThere)
                {
                    //With Linq query
                    var tempList = (from SM in db.RAOngoingArchivalSupport
                                    join acti in db.RAOngoingArchivalActivity on SM.OngoingArchivalActivityId equals acti.Id
                                    where SM.RAApplicationId == AppId
                                    select new
                                    {
                                        Activity = acti.Activity,
                                        ApplicationId = AppId,
                                        EstimateHours = SM.EstimatedHours,
                                        EstimatedCost = SM.EstimatedCost,
                                        EstimateAssumptions = SM.EstimateAssumptions,
                                        //Attachments = SM.Attachment,
                                        AttachmentName = SM.AttachmentName,
                                        Id = SM.Id
                                    }

                        ).ToList();

                    var returnTempList = tempList.AsEnumerable().Select(e => new OngoingArchivalSupportModel
                    {
                        Id = e.Id,
                        OngoingActivity = e.Activity,
                        EstimatedHours = e.EstimateHours,
                        EstimatedCost = e.EstimatedCost,
                        EstimateAssumptions = e.EstimateAssumptions,
                        //Attachment = e.Attachments,
                        AttachmentName =e.AttachmentName

                    });
                    returnList = returnTempList;
                }
                else
                {
                    var tempList = (from SM in db.RAOngoingArchivalActivity
                                    select new
                                    {
                                        Id = SM.Id,
                                        Activity = SM.Activity
                                    }

                        ).ToList();

                    foreach (var ra in tempList)
                    {
                        var entity = new RAOngoingArchivalSupport();
                        entity.RAApplicationId = AppId;
                        entity.OngoingArchivalActivityId = ra.Id;
                        entity.CreatedBy = uid;
                        entity.UpdatedBy = uid;
                        entity.CreatedDt = DateTime.Now;
                        entity.UpdatedDt = DateTime.Now;
                        unitOfWork.RAOngoingArchivalSupport.Insert(entity);
                        unitOfWork.Save();
                    }

                    var AssessmentList = (from SM in db.RAOngoingArchivalSupport
                                          join acti in db.RAOngoingArchivalActivity on SM.OngoingArchivalActivityId equals acti.Id
                                          where SM.RAApplicationId == AppId
                                          select new
                                          {
                                              Activity = acti.Activity,
                                              ApplicationId = AppId,
                                              EstimateHours = SM.EstimatedHours,
                                              EstimatedCost = SM.EstimatedCost,
                                              EstimateAssumptions = SM.EstimateAssumptions,
                                              //Attachments = SM.Attachment,
                                              AttachmentName = SM.AttachmentName,
                                              Id = SM.Id
                                          }

                        ).ToList();

                    var returnTempList = AssessmentList.AsEnumerable().Select(e => new OngoingArchivalSupportModel
                    {
                        Id = e.Id,
                        OngoingActivity = e.Activity,
                        EstimatedHours = e.EstimateHours,
                        EstimatedCost = e.EstimatedCost,
                        EstimateAssumptions = e.EstimateAssumptions,
                        //Attachment = e.Attachments,
                        AttachmentName = e.AttachmentName,


                    });
                    returnList = returnTempList;
                }


            }

            return returnList;
        }


        public static void OngoingArchivalUpdate(OngoingArchivalSupportModel model, int uid)
        {
            if (model.Id != 0)
            {
                var contextFactory = new DbContextFactory();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var entity = unitOfWork.RAOngoingArchivalSupport.GetByID(model.Id);
                entity.EstimateAssumptions = model.EstimateAssumptions;
                //entity.Attachment = model.Attachment;
                entity.EstimatedCost = model.EstimatedCost;
                entity.EstimatedHours = model.EstimatedHours;
                entity.UpdatedDt = DateTime.Now;
                entity.UpdatedBy = uid;

                unitOfWork.RAOngoingArchivalSupport.Update(entity);
                unitOfWork.Save();
            }
        }




        public static byte[] GetFilesBytes(HttpPostedFileBase file)
        {
            MemoryStream target = new MemoryStream();

            file.InputStream.CopyTo(target);

            return target.ToArray();
        }

        /// <summary>
        /// OverAll file upload
        /// </summary>
        /// <param name="file"></param>
        /// <param name="ExeCostId"></param>
        /// <param name="uid"></param>
        public static void FileSave(HttpPostedFileBase file, int ExeCostId, int uid, int Table)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            switch (Table)
            {
                case (int)CostScreen.ArchivalSupport:
                    var ArchivalEntity = unitOfWork.RAOngoingArchivalSupport.GetByID(ExeCostId);
                    ArchivalEntity.Attachment = GetFilesBytes(file);
                    ArchivalEntity.AttachmentName = Path.GetFileName(file.FileName);
                    ArchivalEntity.UpdatedBy = uid;
                    ArchivalEntity.UpdatedDt = DateTime.Now;
                    unitOfWork.RAOngoingArchivalSupport.Update(ArchivalEntity);
                    unitOfWork.Save();
                    break;

                case (int)CostScreen.ExecutionCost:
                    var ExeEntity = unitOfWork.RAExecutionCost.GetByID(ExeCostId);
                    ExeEntity.Attachment = GetFilesBytes(file);
                    ExeEntity.AttachmentName = Path.GetFileName(file.FileName);
                    ExeEntity.UpdatedBy = uid;
                    ExeEntity.UpdatedDt = DateTime.Now;
                    unitOfWork.RAExecutionCost.Update(ExeEntity);
                    unitOfWork.Save();
                    break;

                case (int)CostScreen.ProjectManagement:
                    var ProjEntity = unitOfWork.RAProjectManagement.GetByID(ExeCostId);
                    ProjEntity.Attachment = GetFilesBytes(file);
                    ProjEntity.AttachmentName = Path.GetFileName(file.FileName);
                    ProjEntity.UpdatedBy = uid;
                    ProjEntity.UpdatedDt = DateTime.Now;
                    unitOfWork.RAProjectManagement.Update(ProjEntity);
                    unitOfWork.Save();
                    break;                
            }

        }

        /// <summary>
        /// Over all file Delete
        /// </summary>
        /// <param name="ExeCostId"></param>
        /// <param name="uid"></param>
        public static void FileRemove(int ExeCostId, int uid,int Table)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            switch (Table)
            {
                case (int)CostScreen.ArchivalSupport:
                    var ArchivalEntity = unitOfWork.RAOngoingArchivalSupport.GetByID(ExeCostId);
                    ArchivalEntity.Attachment = null;
                    ArchivalEntity.AttachmentName = null;
                    ArchivalEntity.UpdatedBy = uid;
                    ArchivalEntity.UpdatedDt = DateTime.Now;
                    unitOfWork.RAOngoingArchivalSupport.Update(ArchivalEntity);
                    unitOfWork.Save();
                    break;

                case (int)CostScreen.ExecutionCost:
                    var ExeEntity = unitOfWork.RAExecutionCost.GetByID(ExeCostId);
                    ExeEntity.Attachment = null;
                    ExeEntity.AttachmentName = null;
                    ExeEntity.UpdatedBy = uid;
                    ExeEntity.UpdatedDt = DateTime.Now;
                    unitOfWork.RAExecutionCost.Update(ExeEntity);
                    unitOfWork.Save();
                    break;

                case (int)CostScreen.ProjectManagement:
                    var ProjEntity = unitOfWork.RAProjectManagement.GetByID(ExeCostId);
                    ProjEntity.Attachment = null;
                    ProjEntity.AttachmentName = null;
                    ProjEntity.UpdatedBy = uid;
                    ProjEntity.UpdatedDt = DateTime.Now;
                    unitOfWork.RAProjectManagement.Update(ProjEntity);
                    unitOfWork.Save();
                    break;

            }
            
        }



        public static AttachmentModel filedownload(int rowId, int table)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            AttachmentModel File = new AttachmentModel();
            switch (table)
            {
                case (int)CostScreen.ArchivalSupport:
                    var entity1 = unitOfWork.RAExecutionCost.GetByID(rowId);
                    File.file = entity1.Attachment;
                    File.fileName = entity1.AttachmentName;
                    break;

                case (int)CostScreen.ExecutionCost:
                    var entity2 = unitOfWork.RAExecutionCost.GetByID(rowId);
                    File.file = entity2.Attachment;
                    File.fileName = entity2.AttachmentName;
                    break;

                case (int)CostScreen.ProjectManagement:
                    var entity3 = unitOfWork.RAExecutionCost.GetByID(rowId);
                    File.file = entity3.Attachment;
                    File.fileName = entity3.AttachmentName;
                    break;

            }
            return File;
        }


        ///----------------------------Dashboard----------------------------------//

        public static RADashboardModel getDashboardDetails(int AppId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            RADashboardModel model = new RADashboardModel();
            Rating r = new Rating();
            Db db = new Db();

            var CommonDetails = unitOfWork.Application.Get(g => g.Id == AppId && g.IsRetirementCandidate == true).FirstOrDefault();
            if (CommonDetails != null)
            {
                model.Id = CommonDetails.Id;
                model.GeneralNotes = CommonDetails.GeneralNotes;
                var survey = unitOfWork.PersonnelRole.Get(g => g.AppId == AppId).ToList();
                foreach (var i in survey)
                {
                    var t = (from a in db.PersonnelRole
                             join b in db.Personnels on a.PersonnelId equals b.Id
                             where a.AppId == AppId
                             select new
                             {
                                 surveytype = a.IsSurveyParticipant,
                                 name = b.FirstName + " " + b.LastName,
                             }).ToList();
                    foreach (var item in t)
                    {
                        if (item.surveytype == "Technical")
                            model.TechnicalOwner = item.name;
                        else if (item.surveytype == "Business")
                            model.BusinessFunction = item.name;
                        else if (item.surveytype == "Both")
                        {
                            model.TechnicalOwner = item.name;
                            model.BusinessFunction = item.name;
                        }
                    }
                }
                model.RetirementPhaseId = CommonDetails.RetirementPhaseId;
                var RetirementPhase = unitOfWork.RARetirementPhase.Get(g => g.Id == model.RetirementPhaseId).FirstOrDefault();
                if (RetirementPhase != null)
                {
                    model.RetirementPhase = RetirementPhase.RetirementPhase;
                }
                model.RetirementStageId = CommonDetails.RetirementStageId;
                var RetirementStage = unitOfWork.RARetirementStage.Get(g => g.Id == model.RetirementStageId).FirstOrDefault();
                if(RetirementStage != null)
                {
                    model.RetirementStage = RetirementStage.RetirementStage;
                }
            }
            //----------------------
            var CommonDetail = unitOfWork.RADispCandidate.Get(g => g.RAApplicationId == AppId).FirstOrDefault();
            if (CommonDetail != null)
            {
                //Common Details
                model.RAApplicationId = AppId;
                model.IsDeCommissioned = CommonDetail.IsDeCommissioned;
                model.ArchiveData = CommonDetail.ArchiveData;
                model.ReplacementScopeId = CommonDetail.ReplacementScopeId;
                var Detail = unitOfWork.RADispRecommendation.Get(g => g.RADispCandidateId == CommonDetail.Id);
                foreach (var en in Detail)
                {
                    switch (en.RecommendationType)
                    {
                        //Recommendation Details
                        case (int)RecommendationType.Replace:
                            model.ReplaceAction = en.Action;
                            break;

                        case (int)RecommendationType.Archival:
                            model.ArchivalAction = en.Action;
                            break;

                        case (int)RecommendationType.Decommission:
                            model.DecommAction = en.Action;
                            break;
                    }
                }
            }






            return model;
        }

        public static List<Rating> AppSurveyPerspectiveRating(int AppId, int ClientId, int uid)
        {
            List<Rating> returnList = new List<Rating>();
            var paramName = new string[] { "AppId", "ClientId" };
            var paramValue = new object[] { AppId, ClientId };

            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var t = unitOfWork.GetDataFromStoredProcedure("RADashboardAppAvgRating", paramName, paramValue);
                if (t != null)
                {
                    foreach (DataRow dr in t.Rows)
                    {
                        var k = new Rating();
                        k.PerspectiveCategory = dr["PerspectiveCategoryName"].ToString();
                        k.PerspectiveType = dr["PerspectiveType"].ToString();
                        k.Rate = (decimal)dr["Rating"];
                        returnList.Add(k);
                    }                    
                }
            
            var prevVal = "";
            foreach(var r in returnList)
            {
                if (prevVal != r.PerspectiveCategory)
                {
                    r.AvgRating = returnList.Where(d => d.PerspectiveCategory == r.PerspectiveCategory).Average(e => e.Rate).ToString("#.##");
                }
                prevVal = r.PerspectiveCategory;
            }

            return returnList;

        }

        public static IList<RATaskModel> getTaskDetails(int AppId)
        {
            Db db = new Db();
            var result = (from task in db.RATask
                          join Status in db.RAWbsStatus on task.ProgressStatus equals Status.Id into tempStatus
                          where task.RAApplicationId == AppId && task.ParentId == null
                          from status in tempStatus.DefaultIfEmpty()
                          select new RATaskModel
                          {
                              Title = task.TaskName,
                              Status = status.WbsStatus,
                              TaskId = task.Id,
                              ApplicationId = task.RAApplicationId,
                              Start = task.StartDate,
                              End = task.EndDate,
                              //ParentId = task.ParentId,
                              PercentComplete = task.Completion,
                              //OrderId = task.OrderId,

                              //WorkProdId = task.WorkProductId,
                              //ResourceGroupId = task.ResourceGroupId,
                              //RoleId = task.RoleId,
                              ActualEfforts = task.ActualEfforts == null ? 0 : task.ActualEfforts,
                              EstimatedEfforts = task.EstimatedEfforts == null ? 0 : task.EstimatedEfforts
                          }).ToList();
            return result;
        }




    }
}