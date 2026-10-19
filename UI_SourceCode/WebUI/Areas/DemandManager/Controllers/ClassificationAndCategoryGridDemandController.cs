using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.Domain.Service;
using NTTData.AMOPortal.WebUI.Dto;
using NTTData.AMOPortal.WebUI.Mapper;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Service;
using System.Reflection;
using System.Globalization;
using NTTData.AMOPortal.WebUI.Controllers;
using Data;
using WebUI.Dto;

namespace WebUI.Areas.DemandManager.Controllers
    {

    public class ClassificationAndCategoryGridDemandController : Cruder<ClassAndCategory, DemandClassAndCategoryInput>    //BaseController  //Cruder<ClientAndAccounts, ClientAndAccountsInput>
        {

        //Entities
        private DMClassification entityDMClassification;
        private DMCategory entityDMCategory;
        private ClassAndCategory entityClassAndCategory;

        //Service
        private CrudService<DMClassification> ServiceDMClassification;
        private CrudService<DMCategory> ServiceDMCategory;
        private CrudService<ClassAndCategory> ServiceClassAndCategory;

        //Mappers
        //private Mapper<ClientAndAccounts, ClientAndAccountsInput> clientAndAccountsMapper;
        private Mapper<DMClassification, DMClassificationInput> MapperDMClassification;
        private Mapper<DMCategory, DMCategoryInput> MapperDMCategory;
        private Mapper<ClassAndCategory, DemandClassAndCategoryInput> MapperClassAndCategory;

        //Repo
        //private Repo<ClientAndAccounts> repoClientAndAccounts;
        private Repo<DMClassification> repoDMClassification;
        private Repo<DMCategory> repoDMCategory;
        private Repo<ClassAndCategory> repoClassAndCategory;

        /// <summary>
        /// Get Selected Client Account Contract
        /// </summary>
        /// <param name="service"></param>
        /// <param name="v"></param>
       
        public ClassificationAndCategoryGridDemandController(ICrudService<ClassAndCategory> service, IMapper<ClassAndCategory, DemandClassAndCategoryInput> v)
            : base(service, v)
            {

            DbContextFactory Dbcon = new DbContextFactory();

            repoClassAndCategory = new Repo<ClassAndCategory>(Dbcon);
            MapperClassAndCategory = new Mapper<ClassAndCategory, DemandClassAndCategoryInput>();
            entityClassAndCategory = new ClassAndCategory();
            ServiceClassAndCategory = new CrudService<ClassAndCategory>(repoClassAndCategory);

            repoDMCategory = new Repo<DMCategory>(Dbcon);
            MapperDMCategory = new Mapper<DMCategory, DMCategoryInput>();
            entityDMCategory = new DMCategory();
            ServiceDMCategory = new CrudService<DMCategory>(repoDMCategory);

            repoDMClassification = new Repo<DMClassification>(Dbcon);
            MapperDMClassification = new Mapper<DMClassification, DMClassificationInput>();
            entityDMClassification = new DMClassification();
            ServiceDMClassification = new CrudService<DMClassification>(repoDMClassification);

            }

        public static string GetUserName(int? uid)
        {
            //IList<SurveyViewModel> survey = new List<SurveyViewModel>();
            //var entityMapper = new Mapper<SurveyViewModel, UserProfile>();
            string name = "";
            using (var contextFactory = new DbContextFactory())
            {

                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var b = unitOfWork.UserProfile.GetByID(uid);
                name = b.FirstName + " " + b.LastName;
            }
            return name;
        }

        [HttpGet]
        public ActionResult DisplayClassAndCategory(int ClientId = 0, int ClassId = 0, int CatId = 0, string ClassificationName = "", string ClassCreate = "", string CategoryName = "", string CatCreate="")
            {
            DemandClassAndCategoryInput classAndCategoryInput = new DemandClassAndCategoryInput();
            if (ClientId == 0)
            {
                //var Client = (Client)Session["Client"];
                var Client = (ClientDetail)Session["SessionClient"];

                classAndCategoryInput.ClientId = Client.ClientId;
            }
            classAndCategoryInput.ClassifiationId = ClassId;
            classAndCategoryInput.CategoryId= CatId;
            classAndCategoryInput.Classification = Server.HtmlEncode(ClassificationName);
            classAndCategoryInput.ClassCreate = ClassCreate;
            classAndCategoryInput.CatCreate = CatCreate;

            //clientAccounts = clientAndAccountsService.Get(ContractId);

            if (ClassId > 0)
                {
                classAndCategoryInput.DMClassificationInput = MapperDMClassification.MapToInput(ServiceDMClassification.Get(ClassId));
                }

            if (ClassId > 0)
                {
                classAndCategoryInput.DMCategoryInput = MapperDMCategory.MapToInput(ServiceDMCategory.Get(CatId));
                }
            if (ClassId > 0 && CatCreate == "catcreate")
                {
                classAndCategoryInput.DMCategoryInput = MapperDMCategory.MapToInput(entityDMCategory);
                }

            if (ClassCreate == "classcreate")
                {
                classAndCategoryInput.DMClassificationInput = MapperDMClassification.MapToInput(entityDMClassification);
                }

            if (TempData["Success"] != null)
            {
                ModelState.AddModelError("Success", "Data has been saved successfully");
            }
            return View("NewClassAndCatGrid", classAndCategoryInput);
            
            }

        [HttpPost]
        public ActionResult SaveClassType(DemandClassAndCategoryInput inputs, int ClassId = 0, string ClassCreate = "")
        {
            int uid = (int)Session["UserId"];
           // var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];

            if (ClassCreate == "classcreate")
            {
                inputs.DMClassificationInput.CreatedByUser = GetUserName(uid);
                inputs.DMClassificationInput.CreatedBy = uid;
                inputs.DMClassificationInput.CreatedDate = DateTime.Now;
                inputs.DMClassificationInput.LastUpdatedDate = DateTime.Now;
                inputs.DMClassificationInput.UpdatedBy = uid;
                inputs.DMClassificationInput.LastUpdatedUser = inputs.DMClassificationInput.CreatedByUser;
                inputs.DMClassificationInput.ClientId = client.ClientId;
                inputs.ClientId = client.ClientId;
                ServiceDMClassification.Create(MapperDMClassification.MapToEntity(inputs.DMClassificationInput, entityDMClassification));
            TempData["Success"] = "Data has been saved successfully";
            }
        else
        {
        if (ClassId > 0)
        {

            entityDMClassification = ServiceDMClassification.Get(ClassId);
                    inputs.DMClassificationInput.CreatedBy = entityDMClassification.CreatedBy;
                    inputs.DMClassificationInput.CreatedDate = entityDMClassification.CreatedDate;
            inputs.DMClassificationInput.LastUpdatedDate = DateTime.Now;
            inputs.DMClassificationInput.LastUpdatedUser = GetUserName(uid);
                    inputs.DMClassificationInput.UpdatedBy = uid;
            inputs.DMClassificationInput.Id = entityDMClassification.Id;
            MapperDMClassification.MapToEntity(inputs.DMClassificationInput, entityDMClassification);
            ServiceDMCategory.Save();
            TempData["Success"] = "Data has been saved successfully";
        }
        }


            return RedirectToAction("DisplayClassAndCategory");
        }



        [HttpPost]
        public ActionResult SaveCatType(DemandClassAndCategoryInput inputs, int ClassId=0,  int CatId = 0, string CatCreate = "")
            {
            int uid = (int)Session["UserId"];
           
            if (CatCreate == "catcreate")
                {
                inputs.DMCategoryInput.ClassificationId = ClassId;
                inputs.DMCategoryInput.CreatedByUser = GetUserName(uid);
                inputs.DMCategoryInput.CreatedBy = uid;
                inputs.DMCategoryInput.CreatedDate = DateTime.Now;
                inputs.DMCategoryInput.LastUpdatedDate = DateTime.Now;
                inputs.DMCategoryInput.UpdatedBy = uid;
                inputs.DMCategoryInput.LastUpdatedUser = inputs.DMCategoryInput.CreatedByUser;
                    
                ServiceDMCategory.Create(MapperDMCategory.MapToEntity(inputs.DMCategoryInput, entityDMCategory));
                TempData["Success"] = "Data has been saved successfully";
                }
            else 
                {
                  if (CatId > 0)
                  {

                    entityDMCategory = ServiceDMCategory.Get(CatId);
                    
                    inputs.DMCategoryInput.CreatedBy = entityDMCategory.CreatedBy ;
                    inputs.DMCategoryInput.CreatedDate = entityDMCategory.CreatedDate;

                    inputs.DMCategoryInput.LastUpdatedDate = DateTime.Now;
                    inputs.DMCategoryInput.LastUpdatedUser = GetUserName(uid);
                    inputs.DMCategoryInput.UpdatedBy = uid;
                    inputs.DMCategoryInput.Id = entityDMCategory.Id;
                    inputs.DMCategoryInput.ClassificationId = entityDMCategory.ClassificationId;
                    MapperDMCategory.MapToEntity(inputs.DMCategoryInput, entityDMCategory);
                    ServiceDMCategory.Save();
                    TempData["Success"] = "Data has been saved successfully";
                  }
                }

            return RedirectToAction("DisplayClassAndCategory",new { ClientId = 0, ClassId=ClassId });
        }   
   }

}