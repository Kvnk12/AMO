using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Domain.Abstract;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.Domain.Service;
using NTTData.AMOPortal.WebUI.Dto;
using NTTData.AMOPortal.WebUI.Mapper;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Service;
using WebUI.Dto;

namespace NTTData.AMOPortal.WebUI.Controllers
{
    public class SLASetupController : Cruder<MetricSpecification, MetricSpecificationInput>
    {

        public SLASetupController(ICrudService<MetricSpecification> service, IMapper<MetricSpecification, MetricSpecificationInput> v)
            : base(service, v)
        {

        }

        public ActionResult GetAppList(int ClientId)
        {
            MetricSpecificationInput input = new MetricSpecificationInput();
            //input.ClientId = ClientId;
          //  Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            input.ClientId = client.ClientId;
            // ModelState.AddModelError("DispMsg", "Please select a Metric from Metric Catalog");
            return View("Create", input);
        }

        [System.Web.Mvc.HttpGet]
        public JsonResult ClientAccountsContract(int accountid)
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int clientid = client.ClientId;
            DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.ClientAccountContract> accounts = new Data.Repo<Domain.Entities.ClientAccountContract>(Context);
                List<NTTData.AMOPortal.Domain.Entities.ClientAccountContract> list = accounts.GetAll().Where(a => a.ClientId == clientid && a.AccountId==accountid).OrderBy(o => o.ContractName).ToList();
                var Items = list.Select(Field => new SelectListItem
                {
                    Value = Field.Id.ToString(),
                    Text = Field.ContractName.ToString()
                });

                return Json(Items, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult SetUpView(MetricSpecificationInput input)
        {
           // Client client= (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int clientid = client.ClientId;
            if (!ModelState.IsValid)
                return View("Create", input);
            MetricSpecification entity = new MetricSpecification();
            if (input.Id == 0 || input.ISChecked == true)
            {
                //input.Id = 0;
               // input.Priority = priority;
               ////if (input.ClientAccounts.Count>0 && input.AccountId==null)
               //// {
               ////     ModelState.AddModelError("AccountSel", "Please select an account.");
               //// }
                input.ClientId = clientid;
             //   input.MetricId = mid;
                MetricSpecification Prioritycheck = service.GetAll().Where(pc => pc.ClientId == clientid && pc.Priority == input.Priority && pc.MetricId == input.MetricId).FirstOrDefault();
                if (Prioritycheck == null)
                {
                    if (input.AccountId == null)
                        input.AccountId = 0;
                    if (input.ContractId == null)
                        input.ContractId = 0;
                    var id = service.Create(createMapper.MapToEntity(input, entity));
                    ModelState.AddModelError("Success", "SLA Setup Created Successfully.");
                }
                else
                {
                    service.Save();
                    ModelState.AddModelError("Success", "SLA Setup detail saved Successfully.");
                   // ModelState.AddModelError("PriorityExist", "Priority already exists for this Metric Id.");
                }

            }
            else
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<MetricSpecification> repoM = new Repo<MetricSpecification>(Context);
                CrudService<MetricSpecification> serMU = new CrudService<MetricSpecification>(repoM);
                Mapper<MetricSpecification, MetricSpecificationInput> cMapper = new Mapper<MetricSpecification, MetricSpecificationInput>();
              //  input.Priority = priority;
                input.ClientId = clientid;
              //  input.MetricId = mid;
                var e = cMapper.MapToEntity(input, serMU.Get(input.Id));
               // MetricSpecification Prioritycheck = service.GetAll().Where(pc => pc.ClientId == clientid && pc.Priority == input.Priority && pc.MetricId == input.MetricId).FirstOrDefault();
                if (e != null)
                {
                    serMU.Save();
                    ModelState.AddModelError("Success", "SLA Setup modified successfully.");
                }
            }
            return View("Create", input);
        }

        public ActionResult SetUpView(int setupId, int mid, int clientid, string name, int priority, int? accountid, int? contractid)
        {
            if (accountid == null)
                accountid = 0;
            if (contractid == null)
                contractid = 0;

            ModelState.Clear();
            MetricSpecificationInput mInput = new MetricSpecificationInput();
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            clientid = client.ClientId;
            DbContextFactory Context = new DbContextFactory();
            MetricSpecification entity = new MetricSpecification();

            Repo<Metric> repoF = new Repo<Metric>(Context);
            CrudService<Metric> serM = new CrudService<Metric>(repoF);
            var eM = serM.Get(mid);

            Repo<MetricUnit> repoFilters = new Repo<MetricUnit>(Context);
            CrudService<MetricUnit> serMU = new CrudService<MetricUnit>(repoFilters);
            var eMU = serMU.Get(eM.MetricUnitId);

            if (setupId > 0)
            {
                entity.Id = setupId;
                entity = service.Get(entity.Id);
                var e = createMapper.MapToInput(entity);
                e.MetricUnit = eMU.UnitName;
                return View("Create", e);
            }
            else
            {
                MetricSpecification existingSpec = service.GetAll().Where(pc => pc.ClientId == clientid &&  
                pc.MetricId == mid && pc.Priority== priority && 
                pc.AccountId==accountid && pc.ContractId==contractid).FirstOrDefault();
                if (existingSpec != null)
                {
                    mInput = createMapper.MapToInput(existingSpec);
                }
                else
                {
                    mInput.ClientId = clientid;
                    mInput.MetricId = mid;
                    mInput.MetricName = name;
                    mInput.MetricUnit = eMU.UnitName;
                    mInput.Priority = priority;
                    mInput.ContractId = contractid.Value;
                    mInput.AccountId = accountid.Value;

                }
                //  ModelState.AddModelError("DispMsg", "Please select Priority Value");
                return View("Create", mInput);
            }

        }
        [HttpPost]
        public ActionResult PriorityView(int clientid, int priority, int metricid, string metricname)
        {

            MetricSpecificationInput mInput = new MetricSpecificationInput();
            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            clientid = client.ClientId;
            DbContextFactory Context = new DbContextFactory();
            MetricSpecification entity = new MetricSpecification();
            ModelState.Clear();
            if (priority > 0)
            {
                entity = service.GetAll().Where(a => a.ClientId == clientid && a.Priority == priority && a.MetricId == metricid).FirstOrDefault();
             }    //e.MetricUnit = eMU.UnitName;
            if (entity != null)
            {
                mInput = createMapper.MapToInput(entity);
                return View("Create", mInput);
            }
            else
            {
                //mInput.ClientId = clientid;
                //mInput.MetricName = metricname;
                //mInput.Priority = priority;
                //mInput.MetricId = metricid;
                return RedirectToAction("SetUpView", new { setupId = 0, mid = metricid, clientid = clientid, name = metricname, priority = priority });
            }
            
            
        }

        public ActionResult GetSetupView()
        {
            MetricSpecificationInput input = new MetricSpecificationInput();
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            input.ClientId = client.ClientId;
         //   ModelState.AddModelError("CreateMsg", "Please select a Client");
            return View("Create", input);
        }

        public ActionResult Cancel(int clientid)
        {
            MetricSpecificationInput input = new MetricSpecificationInput();
            //input.ClientId = clientid;
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            input.ClientId = client.ClientId;
            ModelState.AddModelError("DispMsg", "Please select a Metric from Metric Catalog");
            return View("Create", input);
        }

        public ActionResult DelSLASetup(int id, int clientid)
        {
            MetricSpecificationInput input = new MetricSpecificationInput();
            // input.ClientId = clientid;
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            input.ClientId = client.ClientId;
            try
            {
                service.Delete(id);
                ModelState.AddModelError("Success", "SLA SetUp Deleted Successfully.");
            }
            catch
            {
                ModelState.AddModelError("DelMsg", "SLA SetUp is not Selected for Delete");
            }
            return View("Create", input);
        }

        [HttpPost]
        public PartialViewResult SaveSLAcal(int clientid, string fieldname, int priority, string calType)
        {
            ModelState.Clear();
            DbContextFactory Context = new DbContextFactory();
            SlaCalSetup objSlaCalSetup = new SlaCalSetup();
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            clientid = client.ClientId;
            objSlaCalSetup.ClientId = clientid;
            objSlaCalSetup.InputFiled = calType;
            objSlaCalSetup.IPriority = priority;
            objSlaCalSetup.IFiledColValue = fieldname;
            Repo<NTTData.AMOPortal.Domain.Entities.SlaCalSetup> slaCal = new Data.Repo<Domain.Entities.SlaCalSetup>(Context);

            SlaCalSetup chk = slaCal.GetAll().Where(wh => wh.ClientId == clientid && wh.InputFiled == calType && (wh.IPriority == priority || wh.IFiledColValue == fieldname)).FirstOrDefault();
            if (chk == null)
            {
                slaCal.Insert(objSlaCalSetup);
                slaCal.Save();
                TempData["SLACreatedDeleted"] = "SLA calulation set-up added successfully.";
            }
            else
            {
                ModelState.AddModelError("PriorityExist", "Input field/priority already exists.");
            }
            //get all
            List<SlaCalSetup> slaCallist = slaCal.GetAll().Where(a => a.ClientId == clientid).OrderBy(o => o.InputFiled).ToList();
            return PartialView("_SLACalDetail", slaCallist);
        }

        [HttpPost]
        public ActionResult SaveSLASetup(int clientid, string fieldname, int priority, string calType)
        {
            ModelState.Clear();
            DbContextFactory Context = new DbContextFactory();
            SlaCalSetup objSlaCalSetup = new SlaCalSetup();
            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            clientid = client.ClientId;
            objSlaCalSetup.ClientId = clientid;
            objSlaCalSetup.InputFiled = calType;
            objSlaCalSetup.IPriority = priority;
            objSlaCalSetup.IFiledColValue = fieldname;
            Repo<NTTData.AMOPortal.Domain.Entities.SlaCalSetup> slaCal = new Data.Repo<Domain.Entities.SlaCalSetup>(Context);

            SlaCalSetup chk = slaCal.GetAll().Where(wh => wh.ClientId == clientid && wh.InputFiled == calType && (wh.IPriority == priority || wh.IFiledColValue == fieldname)).FirstOrDefault();
            if (chk == null)
            {
                slaCal.Insert(objSlaCalSetup);
                slaCal.Save();
                TempData["SLACreatedDeleted"] = "SLA calulation set-up added successfully.";
            }
            else
            {
                ModelState.AddModelError("PriorityExist", "Input field/priority already exists.");
            }
            //get all
            List<SlaCalSetup> slaCallist = slaCal.GetAll().Where(a => a.ClientId == clientid).OrderBy(o => o.InputFiled).ToList();
            return PartialView("_SLACalDetail", slaCallist);
        }

        [HttpPost]
        public PartialViewResult DelSLAcal( int slasetupid)
        {
            ModelState.Clear();
            DbContextFactory Context = new DbContextFactory();
            Repo<NTTData.AMOPortal.Domain.Entities.SlaCalSetup> slaCal = new Data.Repo<Domain.Entities.SlaCalSetup>(Context);
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];

            slaCal.Delete(slaCal.Get(slasetupid));
            slaCal.Save();
            TempData["SLACreatedDeleted"] = "Deleted successfully.";
            //get all
            List<SlaCalSetup> slaCallist = slaCal.GetAll().Where(a => a.ClientId == client.ClientId).OrderBy(o => o.InputFiled).ToList();
            return PartialView("_SLACalDetail", slaCallist);

        }


    }
}