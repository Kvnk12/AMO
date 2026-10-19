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

namespace WebUI.Areas.IdeaManager.Controllers
    {


    public class ClassificationAndCategoryGridController : Cruder<ClassAndCategory, ClassAndCategoryInput>    //BaseController  //Cruder<ClientAndAccounts, ClientAndAccountsInput>
        {


        

        //Entities
        

        private IMClassification entityIMClassification;
        private IMCategory entityIMCategory;
        private ClassAndCategory entityClassAndCategory;

        //Service

        
        private CrudService<IMClassification> ServiceIMClassification;
        private CrudService<IMCategory> ServiceIMCategory;
        private CrudService<ClassAndCategory> ServiceClassAndCategory;

        //Mappers

        //private Mapper<ClientAndAccounts, ClientAndAccountsInput> clientAndAccountsMapper;

        private Mapper<IMClassification, IMClassificationInput> MapperIMClassification;
        private Mapper<IMCategory, IMCategoryInput> MapperIMCategory;
        private Mapper<ClassAndCategory, ClassAndCategoryInput> MapperClassAndCategory;

        //Repo

        //private Repo<ClientAndAccounts> repoClientAndAccounts;
        private Repo<IMClassification> repoIMClassification;
        private Repo<IMCategory> repoIMCategory;
        private Repo<ClassAndCategory> repoClassAndCategory;


        /// <summary>
        /// Get Selected Client Account Contract
        /// </summary>
        /// <param name="service"></param>
        /// <param name="v"></param>
        /// 







        public ClassificationAndCategoryGridController(ICrudService<ClassAndCategory> service, IMapper<ClassAndCategory, ClassAndCategoryInput> v)
            : base(service, v)
            {


            DbContextFactory Dbcon = new DbContextFactory();

            repoClassAndCategory = new Repo<ClassAndCategory>(Dbcon);
            MapperClassAndCategory = new Mapper<ClassAndCategory, ClassAndCategoryInput>();
            entityClassAndCategory = new ClassAndCategory();
            ServiceClassAndCategory = new CrudService<ClassAndCategory>(repoClassAndCategory);



            repoIMCategory = new Repo<IMCategory>(Dbcon);
            MapperIMCategory = new Mapper<IMCategory, IMCategoryInput>();
            entityIMCategory = new IMCategory();
            ServiceIMCategory = new CrudService<IMCategory>(repoIMCategory);


            repoIMClassification = new Repo<IMClassification>(Dbcon);
            MapperIMClassification = new Mapper<IMClassification, IMClassificationInput>();
            entityIMClassification = new IMClassification();
            ServiceIMClassification = new CrudService<IMClassification>(repoIMClassification);


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
            ClassAndCategoryInput classAndCategoryInput = new ClassAndCategoryInput();
            if (ClientId == 0)
            {
                //var Client = (Client)Session["Client"];
                var Client = (ClientDetail)Session["SessionClient"];
                var UserId = (int)Session["UserId"];
                ViewData["GetStringResult"] = getFullName(UserId);
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
                classAndCategoryInput.IMClassificationInput = MapperIMClassification.MapToInput(ServiceIMClassification.Get(ClassId));
                }

            if (ClassId > 0)
                {
                classAndCategoryInput.IMCategoryInput = MapperIMCategory.MapToInput(ServiceIMCategory.Get(CatId));
                }
            if (ClassId > 0 && CatCreate == "catcreate")
                {
                classAndCategoryInput.IMCategoryInput = MapperIMCategory.MapToInput(entityIMCategory);
                }

            if (ClassCreate == "classcreate")
                {
                classAndCategoryInput.IMClassificationInput = MapperIMClassification.MapToInput(entityIMClassification);
                }

            if (TempData["Success"] != null)
            {
                ModelState.AddModelError("Success", "Data has been saved successfully");
            }
            return View("NewClassAndCatGrid", classAndCategoryInput);
            
            }


        [HttpPost]
        public ActionResult SaveClassType(ClassAndCategoryInput inputs, int ClassId = 0, string ClassCreate = "")
        {
            int uid = (int)Session["UserId"];
           // var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];

            if (ClassCreate == "classcreate")
            {
                inputs.IMClassificationInput.CreatedByUser = GetUserName(uid);
                inputs.IMClassificationInput.CreatedBy = uid;
                inputs.IMClassificationInput.CreatedDate = DateTime.Now;
                inputs.IMClassificationInput.LastUpdatedDate = DateTime.Now;
                inputs.IMClassificationInput.UpdatedBy = uid;
                inputs.IMClassificationInput.LastUpdatedUser = inputs.IMClassificationInput.CreatedByUser;
                inputs.IMClassificationInput.ClientId = client.ClientId;
                inputs.ClientId = client.ClientId;
                ServiceIMClassification.Create(MapperIMClassification.MapToEntity(inputs.IMClassificationInput, entityIMClassification));
            TempData["Success"] = "Data has been saved successfully";
            }
        else
        {
        if (ClassId > 0)
        {

            entityIMClassification = ServiceIMClassification.Get(ClassId);
                  //  inputs.IMClassificationInput.CreatedByUser = entityIMClassification.CreatedByUser;
                    inputs.IMClassificationInput.CreatedBy = entityIMClassification.CreatedBy;
                    inputs.IMClassificationInput.CreatedDate = entityIMClassification.CreatedDate;
                    //inputs.IMClassificationInput.Classification = entityIMClassification.Classification;
                    //inputs.IMClassificationInput.ClientId = entityIMClassification.ClientId;
                   
            inputs.IMClassificationInput.LastUpdatedDate = DateTime.Now;
            inputs.IMClassificationInput.LastUpdatedUser = GetUserName(uid);
                    inputs.IMClassificationInput.UpdatedBy = uid;
            inputs.IMClassificationInput.Id = entityIMClassification.Id;
            MapperIMClassification.MapToEntity(inputs.IMClassificationInput, entityIMClassification);
            ServiceIMCategory.Save();
            TempData["Success"] = "Data has been saved successfully";
        }
        }


            return RedirectToAction("DisplayClassAndCategory");
        }



        [HttpPost]
        public ActionResult SaveCatType(ClassAndCategoryInput inputs, int ClassId=0,  int CatId = 0, string CatCreate = "")
            {
            int uid = (int)Session["UserId"];
           
            if (CatCreate == "catcreate")
                {
                inputs.IMCategoryInput.ClassificationId = ClassId;
                inputs.IMCategoryInput.CreatedByUser = GetUserName(uid);
                inputs.IMCategoryInput.CreatedBy = uid;
                inputs.IMCategoryInput.CreatedDate = DateTime.Now;
                inputs.IMCategoryInput.LastUpdatedDate = DateTime.Now;
                inputs.IMCategoryInput.UpdatedBy = uid;
                inputs.IMCategoryInput.LastUpdatedUser = inputs.IMCategoryInput.CreatedByUser;
                    
                ServiceIMCategory.Create(MapperIMCategory.MapToEntity(inputs.IMCategoryInput, entityIMCategory));
                TempData["Success"] = "Data has been saved successfully";
                }
            else 
                {
                if (CatId > 0)
                    {

                    entityIMCategory = ServiceIMCategory.Get(CatId);
                    
                 //   inputs.IMCategoryInput.CreatedByUser = entityIMCategory.CreatedByUser;
                    inputs.IMCategoryInput.CreatedBy = entityIMCategory.CreatedBy ;
                    inputs.IMCategoryInput.CreatedDate = entityIMCategory.CreatedDate;

                    inputs.IMCategoryInput.LastUpdatedDate = DateTime.Now;
                    inputs.IMCategoryInput.LastUpdatedUser = GetUserName(uid);
                    inputs.IMCategoryInput.UpdatedBy = uid;
                    inputs.IMCategoryInput.Id = entityIMCategory.Id;
                    inputs.IMCategoryInput.ClassificationId = entityIMCategory.ClassificationId;
                    MapperIMCategory.MapToEntity(inputs.IMCategoryInput, entityIMCategory);
                    ServiceIMCategory.Save();
                    TempData["Success"] = "Data has been saved successfully";
                    }
                }


            return RedirectToAction("DisplayClassAndCategory",new { ClientId = 0, ClassId=ClassId });
            }

        //[HttpPost]
        //public ActionResult DisplayClientAccountContracts(ClientAndAccountsInput inputs, int ClientId = 0, int AccountId = 0, int ContractId = 0, string AccountName = "", string Contract = "", string AccountCreate = "")
        //    {
        //    //ViewBag.StatusMessage = "";

        //    inputs.ClientId = ClientId;
        //    inputs.AccountId = AccountId;
        //    inputs.ContractId = ContractId;
        //    inputs.AccountName = Server.HtmlEncode(AccountName);
        //    inputs.ContractCreate = Contract;
        //    inputs.AccountCreate = AccountCreate;



        //    //Go To action account details saving
        //    if ((inputs.ClientId > 0 && inputs.ContractId == 0 && Contract == ""))
        //        {
        //        return (SaveAccountDetails(inputs));
        //        }

        //    var input = inputs.clientAccountContractInput;
        //    input.AccountId = inputs.AccountId;
        //    input.ClientId = inputs.ClientId;

        //    if (!ModelState.IsValid)
        //        {
        //        return View("NewClientAccountGrid", inputs);
        //        }


        //    //Edit Existing Contract
        //    if (inputs.ContractId != 0)
        //        {
        //        input.Id = inputs.ContractId;
        //        using (Db db = new Db())
        //            {
        //            ClientAccountContract existingContractName = db.ClientAccountContract.FirstOrDefault(c => c.ContractName.ToLower().Trim() == input.ContractName.ToLower().Trim());
        //            if (existingContractName != null && (existingContractName.Id != input.Id))
        //                {
        //                if (existingContractName != null)
        //                    {
        //                    ModelState.AddModelError("ContractName", "Contract Name already Exists");
        //                    return View(inputs);
        //                    }
        //                }
        //            }
        //        }

        //    if (inputs.clientAccountContractInput.SumOfFTEOnShoreOffShore)
        //        {
        //        inputs.clientAccountContractInput.AverageAnnualFTE = inputs.clientAccountContractInput.AverageAnnualFTEoffShore  + inputs.clientAccountContractInput.AverageAnnualFTEOnshore;
        //        }

        //    if (input.Id > 0)
        //        {
        //        clientAccountContractMapper.MapToEntity(input, clientAccountContractService.Get(input.Id));
        //        clientAccountContractService.Save();
        //        }
        //    else
        //        {
        //        //Add New Contract 

        //        using (Db db = new Db())
        //            {
        //            ClientAccountContract existingContractName = db.ClientAccountContract.FirstOrDefault(c => c.ContractName.ToLower().Trim() == input.ContractName.ToLower().Trim());
        //            if (existingContractName != null && existingContractName.Id != input.Id)
        //                {
        //                if (existingContractName != null)
        //                    {
        //                    ModelState.AddModelError("ContractName", "Contract Name already Exists");
        //                    return View(inputs);
        //                    }
        //                }
        //            }

        //        var ClientAccountContractId = clientAccountContractService.Create(clientAccountContractMapper.MapToEntity(input, entityClientAccountContract));
        //        input.Id = Convert.ToInt32(ClientAccountContractId);

        //        }

        //    TempData["Success"] = "Data has been saved successfully";
        //    return RedirectToAction("DisplayClientAccountContracts", new { ClientId = ClientId, AccountId = inputs.AccountId, AccountName = inputs.AccountName, ContractId = input.Id});

        //    }



        //[HttpPost]
        //public ActionResult SaveAccountDetails(ClientAndAccountsInput inputs)
        //    {


        //    ClientAccountInput clientSetup = inputs.clientAcctInput;
        //    if (!ModelState.IsValid)
        //        {
        //        return View("NewClientAccountGrid", inputs);
        //        }
        //    else
        //        {
        //          //if sumoffte is checked
        //          if(inputs.clientAcctInput.SumOfFTEOnShoreOffShore)
        //              {
        //              inputs.clientAcctInput.AverageAnnualFTEs = inputs.clientAcctInput.AverageAnnualFTEsOnShore + inputs.clientAcctInput.AverageAnnualFTEsOffShore;
        //              }

        //        if (inputs.ClientId > 0)
        //            {
        //            clientSetup.ClientId = inputs.ClientId;
        //            clientSetup.Id = inputs.AccountId;
        //            }


        //        if (clientSetup.Id == 0)
        //            {
        //            var newclientSetup = clientAccountService.Create(clientAccountMapper.MapToEntity(clientSetup, entityClientAccountSetup));
        //            clientSetup.Id = Convert.ToInt32(newclientSetup);
        //            }
        //        else
        //            {

        //            clientAccountMapper.MapToEntity(clientSetup, clientAccountService.Get(clientSetup.Id));
        //            clientAccountService.Save();
        //            }


        //        }

        //    TempData["Success"] = "Data has been saved successfully";
        //    return RedirectToAction("DisplayClientAccountContracts", new { ClientId = inputs.ClientId, AccountId = clientSetup.Id, AccountName = clientSetup.AccountName});
        //    }
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