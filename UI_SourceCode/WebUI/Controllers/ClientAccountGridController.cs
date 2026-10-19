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
using WebUI.Dto;
using Kendo.Mvc.UI;
using WebUI.Models;
using WebUI.Areas.ClientProfileManager;
using Kendo.Mvc.Extensions;

namespace NTTData.AMOPortal.WebUI.Controllers
    {


    public class ClientAccountGridController : Cruder<ClientAndAccounts, ClientAndAccountsInput>    //BaseController  //Cruder<ClientAndAccounts, ClientAndAccountsInput>
        {

        //Entities
        private ClientAndAccounts entityClientAccounts;

        //Service

        private CrudService<ClientAndAccounts> clientAndAccountsService;

        //Mappers

        private Mapper<ClientAndAccounts, ClientAndAccountsInput> clientAndAccountsMapper;

        //Repo

        private Repo<ClientAndAccounts> repoClientAndAccounts;

        /// <summary>
        /// Get Selected Client Account Contract
        /// </summary>
        /// <param name="service"></param>
        /// <param name="v"></param>
        /// 


        //Entities
        private ClientAccountContract entityClientAccountContract;

        //Service

        private CrudService<ClientAccountContract> clientAccountContractService;

        //Mappers

        private Mapper<ClientAccountContract, ClientAccountContractInput> clientAccountContractMapper;

        //Repo

        private Repo<ClientAccountContract> repoClientAccountContract;



        //For Client Account mapper

        //Entities
        private ClientAccounSetup entityClientAccountSetup;

        //Service

        private CrudService<ClientAccounSetup> clientAccountService;

        //Mappers

        private Mapper<ClientAccounSetup, ClientAccountInput> clientAccountMapper;

        //Repo

        private Repo<ClientAccounSetup> repoClientAccount;





        public ClientAccountGridController(ICrudService<ClientAndAccounts> service, IMapper<ClientAndAccounts, ClientAndAccountsInput> v)
            : base(service, v)
            {


            DbContextFactory Dbcon = new DbContextFactory();
            repoClientAndAccounts = new Repo<ClientAndAccounts>(Dbcon);
            clientAndAccountsMapper = new Mapper<ClientAndAccounts, ClientAndAccountsInput>();

            entityClientAccounts = new ClientAndAccounts();

            clientAndAccountsService = new CrudService<ClientAndAccounts>(repoClientAndAccounts);

            ///Setup Client Account Contract Service
            ///
            //DbContextFactory Dbcon = new DbContextFactory();

            repoClientAccountContract = new Repo<ClientAccountContract>(Dbcon);
            clientAccountContractMapper = new Mapper<ClientAccountContract, ClientAccountContractInput>();
            entityClientAccountContract = new ClientAccountContract();
            clientAccountContractService = new CrudService<ClientAccountContract>(repoClientAccountContract);


            ///setup Client Account Service
            ///

            repoClientAccount = new Repo<ClientAccounSetup>(Dbcon);
            clientAccountMapper = new Mapper<ClientAccounSetup, ClientAccountInput>();
            entityClientAccountSetup = new ClientAccounSetup();
            clientAccountService = new CrudService<ClientAccounSetup>(repoClientAccount);

            }




        [HttpGet]
        public ActionResult DisplayClientAccountContracts(int ClientId = 0, int AccountId = 0, int ContractId = 0, string AccountName = "", string AccountCreate = "", string Contract = "")
            {
            ClientAndAccountsInput clientAccounts = new ClientAndAccountsInput();
          //  Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            if (ClientId <= 0)
            {
                ClientId = client.ClientId;
            }
            clientAccounts.ClientId = ClientId;
            clientAccounts.AccountId = AccountId;
            clientAccounts.ContractId = ContractId;
            clientAccounts.AccountName = Server.HtmlEncode(AccountName);
            clientAccounts.ContractCreate = Contract;
            clientAccounts.AccountCreate = AccountCreate;

            //clientAccounts = clientAndAccountsService.Get(ContractId);

            if (ClientId > 0 && AccountId > 0)
                {

                clientAccounts.clientAcctInput = clientAccountMapper.MapToInput(clientAccountService.Get(AccountId));
                }

            if (ClientId > 0 && ContractId > 0 && AccountId > 0)
                {
                clientAccounts.clientAccountContractInput = clientAccountContractMapper.MapToInput(clientAccountContractService.Get(ContractId));
                }
            if (ClientId > 0 && AccountId > 0 && clientAccounts.ContractCreate == "Contractcreate")
                {
                clientAccounts.clientAccountContractInput = clientAccountContractMapper.MapToInput(entityClientAccountContract);
                clientAccounts.clientAccountContractInput.ContractCloseDate = DateTime.Now;
                clientAccounts.clientAccountContractInput.ContractOpenDate = DateTime.Now;

                }

            if (ClientId > 0 && AccountCreate == "Accountcreate")
                {

                clientAccounts.clientAcctInput = clientAccountMapper.MapToInput(entityClientAccountSetup);
                clientAccounts.clientAcctInput.CloseDate = DateTime.Now;
                clientAccounts.clientAcctInput.OpenDate = DateTime.Now;



                }

            if (TempData["Success"] != null)
            {
                ModelState.AddModelError("Success", "Data has been saved successfully");
            }
            return View("NewClientAccountGrid", clientAccounts);

            }



        [HttpPost]
        public ActionResult DisplayClientAccountContracts(ClientAndAccountsInput inputs, int ClientId = 0, int AccountId = 0, int ContractId = 0, string AccountName = "", string Contract = "", string AccountCreate = "")
            {
            //ViewBag.StatusMessage = "";
            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            if (ClientId <= 0)
            {
                ClientId = client.ClientId;
            }
            inputs.ClientId = ClientId;
            inputs.AccountId = AccountId;
            inputs.ContractId = ContractId;
            inputs.AccountName = Server.HtmlEncode(AccountName);
            inputs.ContractCreate = Contract;
            inputs.AccountCreate = AccountCreate;



            //Go To action account details saving
            if ((inputs.ClientId > 0 && inputs.ContractId == 0 && Contract == ""))
                {
                return (SaveAccountDetails(inputs));
                }

            var input = inputs.clientAccountContractInput;
            input.AccountId = inputs.AccountId;
            input.ClientId = inputs.ClientId;

            if (!ModelState.IsValid)
                {
                return View("NewClientAccountGrid", inputs);
                }


            //Edit Existing Contract
            if (inputs.ContractId != 0)
                {
                input.Id = inputs.ContractId;
                using (Db db = new Db())
                    {
                    ClientAccountContract existingContractName = db.ClientAccountContract.FirstOrDefault(c => c.ContractName.ToLower().Trim() == input.ContractName.ToLower().Trim());
                    if (existingContractName != null && (existingContractName.Id != input.Id))
                        {
                        if (existingContractName != null)
                            {
                            ModelState.AddModelError("ContractName", "Contract Name already Exists");
                            return View(inputs);
                            }
                        }
                    }
                }

            if (inputs.clientAccountContractInput.SumOfFTEOnShoreOffShore)
                {
                inputs.clientAccountContractInput.AverageAnnualFTE = inputs.clientAccountContractInput.AverageAnnualFTEoffShore  + inputs.clientAccountContractInput.AverageAnnualFTEOnshore;
                }

            if (input.Id > 0)
                {
                clientAccountContractMapper.MapToEntity(input, clientAccountContractService.Get(input.Id));
                clientAccountContractService.Save();
                }
            else
                {
                //Add New Contract 

                using (Db db = new Db())
                    {
                    ClientAccountContract existingContractName = db.ClientAccountContract.FirstOrDefault(c => c.ContractName.ToLower().Trim() == input.ContractName.ToLower().Trim());
                    if (existingContractName != null && existingContractName.Id != input.Id)
                        {
                        if (existingContractName != null)
                            {
                            ModelState.AddModelError("ContractName", "Contract Name already Exists");
                            return View(inputs);
                            }
                        }
                    }

                var ClientAccountContractId = clientAccountContractService.Create(clientAccountContractMapper.MapToEntity(input, entityClientAccountContract));
                input.Id = Convert.ToInt32(ClientAccountContractId);

                }

            TempData["Success"] = "Data has been saved successfully";
            return RedirectToAction("DisplayClientAccountContracts", new { ClientId = ClientId, AccountId = inputs.AccountId, AccountName = inputs.AccountName, ContractId = input.Id});

            }



        [HttpPost]
        public ActionResult SaveAccountDetails(ClientAndAccountsInput inputs)
            {


            ClientAccountInput clientSetup = inputs.clientAcctInput;
            if (!ModelState.IsValid)
                {
                return View("NewClientAccountGrid", inputs);
                }
            else
                {
                  //if sumoffte is checked
                  if(inputs.clientAcctInput.SumOfFTEOnShoreOffShore)
                      {
                      inputs.clientAcctInput.AverageAnnualFTEs = inputs.clientAcctInput.AverageAnnualFTEsOnShore + inputs.clientAcctInput.AverageAnnualFTEsOffShore;
                      }

                if (inputs.ClientId > 0)
                    {
                    clientSetup.ClientId = inputs.ClientId;
                    clientSetup.Id = inputs.AccountId;
                    }


                if (clientSetup.Id == 0)
                    {
                    var newclientSetup = clientAccountService.Create(clientAccountMapper.MapToEntity(clientSetup, entityClientAccountSetup));
                    clientSetup.Id = Convert.ToInt32(newclientSetup);
                    }
                else
                    {

                    clientAccountMapper.MapToEntity(clientSetup, clientAccountService.Get(clientSetup.Id));
                    clientAccountService.Save();
                    }


                }

            TempData["Success"] = "Data has been saved successfully";
            return RedirectToAction("DisplayClientAccountContracts", new { ClientId = inputs.ClientId, AccountId = clientSetup.Id, AccountName = clientSetup.AccountName});
            }

        public ActionResult ClientAccountGrid_New()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetAccountList([DataSourceRequest] DataSourceRequest request)
       {
            var Cl = (ClientDetail)Session["SessionClient"];
            IEnumerable<ClientAccountList> klist = ClientProfileManagerHelper.getClientAccountList(Cl.ClientId);
            return Json(klist.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetAccContractList([DataSourceRequest] DataSourceRequest request, int AccountId=0)
        {
            var Cl = (ClientDetail)Session["SessionClient"];
            IEnumerable<ClientAccContract> klist = new List<ClientAccContract>();
            if(AccountId!=0)
            {
                 klist = ClientProfileManagerHelper.getAccContractList(Cl.ClientId, AccountId);
            }
            return Json(klist.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateClientAccount( ClientAccountList model)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            Db db = new Db();
            if (ModelState.IsValid)
            {
                if (model.SumOfFTEOnShoreOffShore)
                {
                    model.AverageAnnualFTEs = model.AverageAnnualFTEsOnShore + model.AverageAnnualFTEsOffShore;
                }
                var newAccSetup = ClientProfileManagerHelper.CreateNewClientAccount(client.ClientId, model);
            }
            return Json(new[] { model }.ToDataSourceResult(new DataSourceRequest(), ModelState));
        }
        [HttpPost]
        public ActionResult EditClientAccount([DataSourceRequest] DataSourceRequest request, ClientAccountList model)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            Db db = new Db();
           
            if (ModelState.IsValid)
            {
                if (model.SumOfFTEOnShoreOffShore)
                {
                    model.AverageAnnualFTEs = model.AverageAnnualFTEsOnShore + model.AverageAnnualFTEsOffShore;
                }
                var res=ClientProfileManagerHelper.UpdateClientAccount(client.ClientId, model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult DeleteClientAccount([DataSourceRequest] DataSourceRequest request, ClientAccountList model)
        {
            Db db = new Db();
            var uid = (int)Session["UserId"];
            var res=ClientProfileManagerHelper.DeleteClientAccount(model.AccountId);
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        
        [HttpPost]
        public ActionResult CreateAccContract([DataSourceRequest] DataSourceRequest request, ClientAccContract model, int AccountId = 0)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            ClientAccContract contdet = null;
            var uid = (int)Session["UserId"];
            Db db = new Db();
            if (ModelState.IsValid)
            {
                if(AccountId!=0)
                {
                    model.AccountId = AccountId;
                    if (model.SumOfFTEOnShoreOffShore)
                    {
                        model.AverageAnnualFTE = model.AverageAnnualFTEoffShore + model.AverageAnnualFTEOnshore;
                    }
                    ClientAccountContract existingContractName = db.ClientAccountContract.Where(c=>c.ClientId== client.ClientId).FirstOrDefault(c => c.ContractName.ToLower().Trim() == model.ContractName.ToLower().Trim());
                    if (existingContractName != null && existingContractName.Id != model.ContractId)
                    {
                        if (existingContractName != null)
                        {
                            ModelState.AddModelError("ContractName", "Contract Name already Exists");
                            return JavaScript("alert('Contract Name already Exists');");
                           // return Json(new[] { model }.ToDataSourceResult(request, ModelState));
                        }
                    }
                    var newContSetup = ClientProfileManagerHelper.CreateAccContract(client.ClientId, model);
                    return Json(new[] { model }.ToDataSourceResult(request, ModelState));
                }
            }
            return Json(new[] { contdet }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult EditAccContractList([DataSourceRequest] DataSourceRequest request, ClientAccContract model)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            Db db = new Db();

            if (ModelState.IsValid)
            {
                if (model.SumOfFTEOnShoreOffShore)
                {
                    model.AverageAnnualFTE = model.AverageAnnualFTEoffShore + model.AverageAnnualFTEOnshore;
                }
                var updateAcc = ClientProfileManagerHelper.UpdateAccContract(client.ClientId, model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult DeleteAccContractList([DataSourceRequest] DataSourceRequest request, ClientAccContract model)
        {
            Db db = new Db();
            var uid = (int)Session["UserId"];
            var delAcc = ClientProfileManagerHelper.DeleteAccContract(model.ContractId);
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
    }

}