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
using WebUI.Areas.PortfolioAnalyzer;
using Data;
using WebUI.Dto;

namespace NTTData.AMOPortal.WebUI.Controllers
{
    public class LOBandApplicationsController : Cruder<LOBandApplications, LOBandApplicationsInput>
    {
        //Entities
        private LOBandApplications entityLOBandApplications;

        //Service
        private CrudService<LOBandApplications> lOBandApplicationsService;

        //Mappers
        private Mapper<LOBandApplications, LOBandApplicationsInput> lOBandApplicationsMapper;

        //Repo
        private Repo<LOBandApplications> repoLOBandApplications;

        /// <summary>
        /// Get LOB
        /// </summary>
        /// <param name="service"></param>
        /// <param name="v"></param>
        /// 

        //Entities
        private LineOfBusinesses entityLineOfBusiness;

        //Service

        private CrudService<LineOfBusinesses> lineOfBusinessService;

        //Mappers

        private Mapper<LineOfBusinesses, LineOfBusinessInput> lineOfBusinessMapper;

        //Repo

        private Repo<LineOfBusinesses> repoLineOfBusiness;


        //For LOBApp mapper

        //Entities
        private ApplicationLOBJunctions entityApplicationLOBJunction;

        //Service

        private CrudService<ApplicationLOBJunctions> applicationLOBJunctionService;

        //Mappers

        private Mapper<ApplicationLOBJunctions, ApplicationLOBJunctionInput> applicationLOBJunctionMapper;

        //Repo

        private Repo<ApplicationLOBJunctions> repoApplicationLOBJunction;

        //For ClientBCLevel1 mapper

        //Entities
        private ClientBCLevel1 entityClientBCLevel1;

        //Service

        private CrudService<ClientBCLevel1> clientBCLevel1Service;

        //Mappers

        private Mapper<ClientBCLevel1, ClientBCLevel1Input> clientBCLevel1Mapper;

        //Repo

        private Repo<ClientBCLevel1> repoClientBCLevel1;

        //For ClientBCLevel2 mapper

        //Entities
        private ClientBCLevel2 entityClientBCLevel2;

        //Service

        private CrudService<ClientBCLevel2> clientBCLevel2Service;

        //Mappers

        private Mapper<ClientBCLevel2, ClientBCLevel2Input> clientBCLevel2Mapper;

        //Repo

        private Repo<ClientBCLevel2> repoClientBCLevel2;

        //For ClientBCLevel3 mapper

        //Entities
        private ClientBCLevel3 entityClientBCLevel3;

        //Service

        private CrudService<ClientBCLevel3> clientBCLevel3Service;

        //Mappers

        private Mapper<ClientBCLevel3, ClientBCLevel3Input> clientBCLevel3Mapper;

        //Repo

        private Repo<ClientBCLevel3> repoClientBCLevel3;

        //For JunctionLOBClientBCInput mapper

        //Entities
        private JunctionLOBClientBCs entityJunctionLOBClientBC;

        //Service

        private CrudService<JunctionLOBClientBCs> junctionLOBClientBCService;

        //Mappers

        private Mapper<JunctionLOBClientBCs, JunctionLOBClientBCInput> junctionLOBClientBCMapper;

        //Repo

        private Repo<JunctionLOBClientBCs> repoJunctionLOBClientBC;

        //For JunctionAppLOBClientBCs mapper

        //Entities
        private JunctionAppLOBClientBCs entityJunctionAppLOBClientBC;

        //Service

        private CrudService<JunctionAppLOBClientBCs> junctionAppLOBClientBCService;

        //Mappers

        private Mapper<JunctionAppLOBClientBCs, JunctionAppLOBClientBCInput> junctionAppLOBClientBCMapper;

        //Repo

        private Repo<JunctionAppLOBClientBCs> repoJunctionAppLOBClientBC;

        public LOBandApplicationsController(ICrudService<LOBandApplications> service, IMapper<LOBandApplications, LOBandApplicationsInput> v)
            : base(service, v)
        {


            DbContextFactory Dbcon = new DbContextFactory();
            repoLOBandApplications = new Repo<LOBandApplications>(Dbcon);
            lOBandApplicationsMapper = new Mapper<LOBandApplications, LOBandApplicationsInput>();

            entityLOBandApplications = new LOBandApplications();

            lOBandApplicationsService = new CrudService<LOBandApplications>(repoLOBandApplications);

            ///Setup Client Account Contract Service
            ///
            //DbContextFactory Dbcon = new DbContextFactory();

            repoLineOfBusiness = new Repo<LineOfBusinesses>(Dbcon);
            lineOfBusinessMapper = new Mapper<LineOfBusinesses, LineOfBusinessInput>();
            entityLineOfBusiness = new LineOfBusinesses();
            lineOfBusinessService = new CrudService<LineOfBusinesses>(repoLineOfBusiness);

            ///setup Client Account Service
            ///

            repoApplicationLOBJunction = new Repo<ApplicationLOBJunctions>(Dbcon);
            applicationLOBJunctionMapper = new Mapper<ApplicationLOBJunctions, ApplicationLOBJunctionInput>();
            entityApplicationLOBJunction = new ApplicationLOBJunctions();
            applicationLOBJunctionService = new CrudService<ApplicationLOBJunctions>(repoApplicationLOBJunction);

            ///setup Client BC Level1 Service
            ///

            repoClientBCLevel1 = new Repo<ClientBCLevel1>(Dbcon);
            clientBCLevel1Mapper = new Mapper<ClientBCLevel1, ClientBCLevel1Input>();
            entityClientBCLevel1 = new ClientBCLevel1();
            clientBCLevel1Service = new CrudService<ClientBCLevel1>(repoClientBCLevel1);


            ///setup Client BC Level2 Service
            ///

            repoClientBCLevel2 = new Repo<ClientBCLevel2>(Dbcon);
            clientBCLevel2Mapper = new Mapper<ClientBCLevel2, ClientBCLevel2Input>();
            entityClientBCLevel2 = new ClientBCLevel2();
            clientBCLevel2Service = new CrudService<ClientBCLevel2>(repoClientBCLevel2);

            ///setup Client BC Level3 Service
            ///

            repoClientBCLevel3 = new Repo<ClientBCLevel3>(Dbcon);
            clientBCLevel3Mapper = new Mapper<ClientBCLevel3, ClientBCLevel3Input>();
            entityClientBCLevel3 = new ClientBCLevel3();
            clientBCLevel3Service = new CrudService<ClientBCLevel3>(repoClientBCLevel3);

            ///setup JunctionLOBClientBC Service
            ///

            repoJunctionLOBClientBC = new Repo<JunctionLOBClientBCs>(Dbcon);
            junctionLOBClientBCMapper = new Mapper<JunctionLOBClientBCs, JunctionLOBClientBCInput>();
            entityJunctionLOBClientBC = new JunctionLOBClientBCs();
            junctionLOBClientBCService = new CrudService<JunctionLOBClientBCs>(repoJunctionLOBClientBC);

            ///setup JunctionLOBClientBC Service
            ///

            repoJunctionAppLOBClientBC = new Repo<JunctionAppLOBClientBCs>(Dbcon);
            junctionAppLOBClientBCMapper = new Mapper<JunctionAppLOBClientBCs, JunctionAppLOBClientBCInput>();
            entityJunctionAppLOBClientBC = new JunctionAppLOBClientBCs();
            junctionAppLOBClientBCService = new CrudService<JunctionAppLOBClientBCs>(repoJunctionAppLOBClientBC);
        }

        [HttpGet]
        public ActionResult DisplayLOBandApplications(int ClientId = 0, int LOBId = 0, int AppId = 0, string LOBName = "", string LOBCreate = "", string AppCreate = "")
        {
            LOBandApplicationsInput lOBandApplications = new LOBandApplicationsInput();
            
            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            ClientId = client.ClientId;
            //lOBandApplications.ClientId = ClientId;
            lOBandApplications.ClientId = client.ClientId;
            lOBandApplications.LOBId = LOBId;
            lOBandApplications.AppId = AppId;
            lOBandApplications.LOBName = Server.HtmlEncode(LOBName);
            lOBandApplications.LOBCreate = LOBCreate;
            lOBandApplications.AppCreate = AppCreate;
            lOBandApplications.SubVerticalId = client.SubVerticalId;
            if (ClientId > 0)
            {
                int verticalId = client.VerticalId; //lOBandApplications.ClientListbyId.VerticalId;
                int subVerticalId = client.SubVerticalId;// lOBandApplications.ClientListbyId.SubVerticalId;

                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Verticals> v = new Data.Repo<Domain.Entities.Verticals>(Context);
                Repo<NTTData.AMOPortal.Domain.Entities.SubVerticals> s = new Data.Repo<Domain.Entities.SubVerticals>(Context);

                if (verticalId > 0)
                {
                    lOBandApplications.verticalName = v.Get(verticalId).VerticalName;
                }
                if (subVerticalId > 0)
                {
                    lOBandApplications.subVerticalName = s.Get(subVerticalId).SubVerticalName;
                }

            }

            if (ClientId > 0 && LOBId > 0)
            {
                lOBandApplications.lineOfBusinessInput = lineOfBusinessMapper.MapToInput(lineOfBusinessService.Get(LOBId));

            }


            if (ClientId > 0 && LOBId > 0 && AppId > 0)
            {
                lOBandApplications.applicationLOBJunctionInput = applicationLOBJunctionMapper.MapToInput(applicationLOBJunctionService.Get(AppId));

            }


            if (ClientId > 0 && lOBandApplications.LOBCreate == "LOBCreate")
            {
                lOBandApplications.lineOfBusinessInput = lineOfBusinessMapper.MapToInput(entityLineOfBusiness);


            }

            if (ClientId > 0 && AppCreate == "AppCreate")
            {

                lOBandApplications.applicationLOBJunctionInput = applicationLOBJunctionMapper.MapToInput(entityApplicationLOBJunction);




            }

            if (TempData["Success"] != null)
            {
                ModelState.AddModelError("Success", "Data has been saved successfully");
            }
            return View("NewLOBandApplications", lOBandApplications);

        }

        [HttpPost]
        public ActionResult DisplayLOBandApplications(LOBandApplicationsInput inputs, int ClientId = 0, int LOBId = 0, int AppId = 0, string LOBName = "", string LOBCreate = "", string AppCreate = "", string tabs = "")
        {
            //ViewBag.StatusMessage = "";
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            ClientId = client.ClientId;
            inputs.ClientId = ClientId;
            inputs.LOBId = LOBId;
            inputs.AppId = AppId;
            inputs.LOBName = Server.HtmlEncode(LOBName);
            inputs.LOBCreate = LOBCreate;
            inputs.AppCreate = AppCreate;
            inputs.tabs = tabs;

            //Go To action account details saving
            if ((inputs.ClientId > 0 && inputs.AppId == 0 && AppCreate == ""))
            {
                return (SaveLOBDetails(inputs));
            }

            var input = inputs.applicationLOBJunctionInput;
            if (input.AppId != null)
            {
                inputs.AppId = input.AppId;
            }
            input.LOBId = inputs.LOBId;


            if (!ModelState.IsValid)
            {
                return View("NewLOBandApplications", inputs);
            }


            //Edit Existing Contract
            if (inputs.AppLOBId != 0)
            {
                input.Id = inputs.AppLOBId;
                using (Db db = new Db())
                {
                    ApplicationLOBJunctions existingApplicationLOBJunction = db.ApplicationLOBJunction.FirstOrDefault(c => c.AppId == input.AppId && c.LOBId == input.LOBId);
                    if (existingApplicationLOBJunction != null && (existingApplicationLOBJunction.Id != input.Id))
                    {
                        if (existingApplicationLOBJunction != null)
                        {
                            ModelState.AddModelError("ContractName", "Contract Name already Exists");
                            return View(inputs);
                        }
                    }
                }
            }



            if (input.Id > 0)
            {
                applicationLOBJunctionMapper.MapToEntity(input, applicationLOBJunctionService.Get(input.Id));

                applicationLOBJunctionService.Save();
            }
            else
            {
                //Add New Contract 

                using (Db db = new Db())
                {
                    ApplicationLOBJunctions existingApplicationLOBJunction = db.ApplicationLOBJunction.FirstOrDefault(c => c.AppId == input.AppId && c.LOBId == input.LOBId);
                    if (existingApplicationLOBJunction != null && (existingApplicationLOBJunction.Id != input.Id))
                    {
                        if (existingApplicationLOBJunction != null)
                        {
                            ModelState.AddModelError("ContractName", "Contract Name already Exists");
                            return View(inputs);
                        }
                    }
                }

                var applobId = applicationLOBJunctionService.Create(applicationLOBJunctionMapper.MapToEntity(input, entityApplicationLOBJunction));
                input.Id = Convert.ToInt32(applobId);

            }

            TempData["Success"] = "Data has been saved successfully";
            return RedirectToAction("DisplayLOBandApplications", new { ClientId = ClientId, LOBId = inputs.LOBId, LOBName = inputs.LOBName, AppLOBId = input.Id, tabs = inputs.tabs });

        }



        [HttpPost]
        public ActionResult SaveLOBDetails(LOBandApplicationsInput inputs)
        {
            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            inputs.ClientId = client.ClientId;
            LineOfBusinessInput lineOfBusiness = inputs.lineOfBusinessInput;
            if (!ModelState.IsValid)
            {
                return View("DisplayLOBandApplications", inputs);
            }
            else
            {


                if (inputs.ClientId > 0)
                {
                    // lineOfBusiness.ClientId = inputs.ClientId;
                    lineOfBusiness.ClientId = client.ClientId;
                    lineOfBusiness.Id = inputs.LOBId;
                }


                if (lineOfBusiness.Id == 0)
                {


                    var newlob = lineOfBusinessService.Create(lineOfBusinessMapper.MapToEntity(lineOfBusiness, entityLineOfBusiness));

                    lineOfBusiness.Id = Convert.ToInt32(newlob);
                }
                else
                {

                    lineOfBusinessMapper.MapToEntity(lineOfBusiness, lineOfBusinessService.Get(lineOfBusiness.Id));
                    lineOfBusinessService.Save();
                }


            }

            TempData["Success"] = "Data has been saved successfully";
            return RedirectToAction("DisplayLOBandApplications", new { ClientId = inputs.ClientId, LOBId = lineOfBusiness.Id, LOBName = lineOfBusiness.LOBName, tabs = inputs.tabs });
        }


        public ActionResult DelAppLOB(LOBandApplicationsInput inputs)
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];

            inputs.ClientId = client.ClientId;
            int clientId = inputs.ClientId;
            //ApplicationLOBJunctionInput aInput = new ApplicationLOBJunctionInput();
            //applicationLOBJunctionInput aInput = new applicationLOBJunctionInput();
            //var input = inputs.applicationLOBJunctionInput;
            //input.Id = inputs.AppLOBId;

            applicationLOBJunctionService.Delete(inputs.AppLOBId);
            inputs.AppLOBId = 0;
            inputs.AppId = 0;
            inputs.LOBCreate = "";
            inputs.AppCreate = "";
            inputs.tabs = "Tab3";
            // inputs.applicationLOBJunctionInput.Id = 0;
            //TempData["Success"] = "Data has been deleted successfully";
            return View("NewLOBandApplications", inputs);
        }
        public ActionResult DelLOB(LOBandApplicationsInput inputs)
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId; //inputs.ClientId;
            inputs.ClientId = client.ClientId;
            int LOBId = inputs.LOBId;
            var applist = applicationLOBJunctionService.Where(x => x.LOBId == LOBId).ToList();
            if (applist.Count > 0)
            {
                foreach (var a in applist)
                {
                    applicationLOBJunctionService.Delete(a.Id);
                }
            }
            lineOfBusinessService.Delete(LOBId);
            inputs.LOBId = 0;
            inputs.LOBName = "";
            inputs.tabs = "Tab3";
            //var listLevel2 = businessCapabilitiesLevel2Service.Where(x => x.Level1Id == id).ToList();
            //if (listLevel2.Count > 0)
            //{
            //    foreach (var level2Ids in listLevel2)
            //    {
            //        var listLevel3 = businessCapabilitiesLevel3Service.Where(x => x.Level2Id == level2Ids.Id).ToList();
            //        if (listLevel3.Count > 0)
            //        {
            //            foreach (var level3Ids in listLevel3)
            //            {
            //                businessCapabilitiesLevel3Service.Delete(level3Ids.Id);
            //            }
            //        }
            //        businessCapabilitiesLevel2Service.Delete(level2Ids.Id);
            //    }
            //}


            //businessCapabilitiesLevel1Service.Delete(id);
            //input.Level1Id = 0;
            //input.Level1Name = "";
            //input.Level2Name = "";
            return View("NewLOBandApplications", inputs);
        }

        public ActionResult ImportDefaultCapabilities(int SubVerticalId = 0)
        {
          //  var client = (Client)Session["ClientId"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var userId = (int)Session["UserId"];
            try
            {
                if (SubVerticalId == 0)
                {
                    ViewData["Error"] = "Vertical/Sub Vertical not assigned!!";
                    return RedirectToAction("DisplayLOBandApplications");
                }
                ImportDefaultBusinessCapabilities(client.ClientId, SubVerticalId, userId);
                ViewData["Success"] = "Imported Successfully";
                return RedirectToAction("DisplayLOBandApplications","");

                //if (res){

                //    ViewData["Success"] = "Imported Successfully";
                //    return RedirectToAction("DisplayLOBandApplications");
                //}
                //else
                //{
                //    ViewData["Error"] = "Internal Error! Please try again later!";
                //    return RedirectToAction("DisplayLOBandApplications");
                //}
            }
            catch(Exception ex)
            {
                ViewData["Error"] = "Internal Error! Please try again later!";
                return RedirectToAction("DisplayLOBandApplications");
            }
        }
        private void ImportDefaultBusinessCapabilities(int Clientid, int SubVerticalId, int UserId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var getParentIdNull = unitOfWork.BAMBusinessCapabilities.Get(g => g.SubVerticalId == SubVerticalId).OrderByDescending(g => g.Id).ToList();
            deleteBusinessCapabilities(Clientid);
            foreach (var iMod in getParentIdNull)
            {
                importOneCapability(iMod.Id, Clientid, UserId, null);
            }
        }
        private void importOneCapability(int BamId, int ClientId, int UserId, int? parentId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var childitems = unitOfWork.BAMBusinessCapabilities.Get(w => w.ParentId == BamId);
            foreach (var chitem in childitems)
            {
                BAMClientBusinessCapabilities cv = new BAMClientBusinessCapabilities()
                {
                    BusinessCapName = chitem.BusinessCapName,
                    ClientId = ClientId,
                    CreatedBy = UserId,
                    CreatedDate = DateTime.Now,
                    SubVerticalId = chitem.SubVerticalId,
                    ParentId = parentId
                };
                unitOfWork.BAMClientBusinessCapabilities.Insert(cv);
                unitOfWork.Save();
                var NewBamId = cv.Id;
                //Recursive
                importOneCapability(chitem.Id, ClientId, UserId, NewBamId);
            }
        }
        private void deleteBusinessCapabilities(int ClientId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var del = unitOfWork.BAMClientBusinessCapabilities.Get(g => g.ClientId == ClientId);
            foreach (var cap in del)
            {
                unitOfWork.BAMClientBusinessCapabilities.Delete(cap.Id);
            }
            unitOfWork.Save();

        }


        ////public ActionResult SaveClientBCLevels(LOBandApplicationsInput inputs)
        ////{
        ////    Client client = (Client)Session["Client"];
        ////    int clientId = client.Id; //inputs.ClientId;
        ////    inputs.ClientId = client.Id;
        ////    var BCLevel1List = clientBCLevel1Service.Where(x => x.ClientId == clientId).ToList();
        ////    if (BCLevel1List.Count > 0)
        ////    {
        ////        foreach (var L1 in BCLevel1List)
        ////        {

        ////            var BCLevel2List = clientBCLevel2Service.Where(x => x.ClientLevel1Id == L1.Id).ToList();
        ////            if (BCLevel2List.Count > 0)
        ////            {
        ////                foreach (var L2 in BCLevel2List)
        ////                {
        ////                    var BCLevel3List = clientBCLevel3Service.Where(x => x.ClientLevel2Id == L2.Id).ToList();

        ////                    if (BCLevel3List.Count > 0)
        ////                    {
        ////                        foreach (var L3 in BCLevel3List)
        ////                        {
        ////                            var lobClientlist = junctionLOBClientBCService.Where(x => x.CBCLevel3Id == L3.Id || x.CBCLevel2Id == L2.Id || x.CBCLevel1Id == L1.Id).ToList();
        ////                            if (lobClientlist.Count > 0)
        ////                            {
        ////                                foreach (var lc in lobClientlist)
        ////                                {
        ////                                    var applobClientlist = junctionAppLOBClientBCService.Where(j => j.LOBClientBCId == lc.Id).ToList();
        ////                                    if (applobClientlist.Count > 0)
        ////                                    {
        ////                                        foreach (var alc in applobClientlist)
        ////                                        {
        ////                                            junctionAppLOBClientBCService.Delete(alc.Id);
        ////                                        }
        ////                                    }
        ////                                    junctionLOBClientBCService.Delete(lc.Id);
        ////                                }


        ////                            }
        ////                            clientBCLevel3Service.Delete(L3.Id);
        ////                        }
        ////                    }
        ////                    clientBCLevel2Service.Delete(L2.Id);
        ////                }
        ////            }
        ////            clientBCLevel1Service.Delete(L1.Id);

        ////        }
        ////    }
        ////    if (clientId > 0)
        ////    {

        ////        int subVerticalId = inputs.ClientListbyId.SubVerticalId;

        ////        var level1List = IenumberableLists.BusinessCapabilitiesLevel1DbSet.Where(x => x.SubVerticalId == subVerticalId);
        ////        if (level1List.Count() > 0)
        ////        {
        ////            foreach (var l1 in level1List)
        ////            {
        ////                ClientBCLevel1Input clientBC1Input = new ClientBCLevel1Input();
        ////                clientBC1Input.ClientId = clientId;
        ////                clientBC1Input.ClientInternalExternal = l1.InternalExternal;
        ////                clientBC1Input.ClientLevel1Name = l1.Level1;
        ////                clientBC1Input.ClientLevel1Des = l1.Description1;
        ////                if (clientBC1Input.Id == 0)
        ////                {

        ////                    var newbc1 = clientBCLevel1Service.Create(clientBCLevel1Mapper.MapToEntity(clientBC1Input, entityClientBCLevel1));
        ////                    clientBC1Input.Id = Convert.ToInt32(newbc1);
        ////                }
        ////                var level2List = IenumberableLists.BusinessCapabilitiesLevel2DbSet.Where(x => x.Level1Id == l1.Id);
        ////                if (level2List.Count() > 0)
        ////                {
        ////                    foreach (var l2 in level2List)
        ////                    {
        ////                        ClientBCLevel2Input clientBC2Input = new ClientBCLevel2Input();
        ////                        clientBC2Input.ClientLevel1Id = clientBC1Input.Id;
        ////                        clientBC2Input.ClientLevel2Name = l2.Level2;
        ////                        clientBC2Input.ClientLevel2Des = l2.Description2;

        ////                        if (clientBC2Input.Id == 0)
        ////                        {
        ////                            var newbc2 = clientBCLevel2Service.Create(clientBCLevel2Mapper.MapToEntity(clientBC2Input, entityClientBCLevel2));
        ////                            clientBC2Input.Id = Convert.ToInt32(newbc2);
        ////                        }

        ////                        var level3List = IenumberableLists.BusinessCapabilitiesLevel3DbSet.Where(x => x.Level2Id == l2.Id);
        ////                        if (level3List.Count() > 0)
        ////                        {
        ////                            foreach (var l3 in level3List)
        ////                            {
        ////                                ClientBCLevel3Input clientBC3Input = new ClientBCLevel3Input();
        ////                                clientBC3Input.ClientLevel2Id = clientBC2Input.Id;
        ////                                clientBC3Input.ClientLevel3Name = l3.Level3;
        ////                                clientBC3Input.ClientLevel3Des = l3.Description3;
        ////                                if (clientBC2Input.Id == 0)
        ////                                {
        ////                                    var newbc3 = clientBCLevel3Service.Create(clientBCLevel3Mapper.MapToEntity(clientBC3Input, entityClientBCLevel3));
        ////                                    clientBC3Input.Id = Convert.ToInt32(newbc3);
        ////                                }
        ////                            }
        ////                        }
        ////                    }
        ////                }
        ////            }
        ////        }

        ////    }
        ////    TempData["Success"] = "Data has been saved successfully";
        ////    return RedirectToAction("DisplayLOBandApplications", new { ClientId = inputs.ClientId, verticalName = inputs.verticalName, subVerticalName = inputs.subVerticalName, tabs = inputs.tabs });


        ////}

        public ActionResult SaveInvApplication(int id, bool active, int clientId, string appName)
        {
            Application appEnt = new Application();
            ApplicationInvInput appInp = new ApplicationInvInput();
            Mapper<Application, ApplicationInvInput> appMapper = new Mapper<Application, ApplicationInvInput>();
            DbContextFactory Dbcon = new DbContextFactory();
            Repo<Application> apprepo = new Repo<Application>(Dbcon);
            CrudService<Application> appCrud = new CrudService<Application>(apprepo);
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            clientId = client.ClientId;
            appInp.Id = id;
            appInp.AppIsActive = active;
            appInp.ApplicationName = appName;
            appInp.ClientId = clientId;


            appMapper.MapToEntity(appInp, appCrud.Get(id));
            appCrud.Save();

            TempData["Success"] = "Data has been saved successfully";
            return RedirectToAction("DisplayLOBandApplications", new { appInp.ClientId, tabs = "Tab2" });
        }

        public ActionResult DeleteInvApplication(int id, int clientId, string appName)
        {
            //Application appEnt = new Application();
            ApplicationInvInput appInp = new ApplicationInvInput();
            Mapper<Application, ApplicationInvInput> appMapper = new Mapper<Application, ApplicationInvInput>();
            DbContextFactory Dbcon = new DbContextFactory();
            Repo<Application> apprepo = new Repo<Application>(Dbcon);
            CrudService<Application> appCrud = new CrudService<Application>(apprepo);
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            clientId = client.ClientId;
            appInp.Id = id;
            appInp.IsDeleted = true;
            appInp.ApplicationName = appName;
            appInp.ClientId = clientId;
            appMapper.MapToEntity(appInp, appCrud.Get(id));
            appCrud.Save();

            return RedirectToAction("DisplayLOBandApplications", new { appInp.ClientId, tabs = "Tab2" });
        }

        public ActionResult AddInvApplication(int clientId, string appName, bool active)
        {
            Application appEnt = new Application();
            ApplicationInvInput appInp = new ApplicationInvInput();
            Mapper<Application, ApplicationInvInput> appMapper = new Mapper<Application, ApplicationInvInput>();
            DbContextFactory Dbcon = new DbContextFactory();
            Repo<Application> apprepo = new Repo<Application>(Dbcon);
            CrudService<Application> appCrud = new CrudService<Application>(apprepo);
            LOBandApplicationsInput inputs = new LOBandApplicationsInput();
            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            clientId = client.ClientId;
            appEnt.ClientId = clientId;
            appInp.AppIsActive = active;
            appInp.ApplicationName = appName;
            appInp.ClientId = clientId;
            inputs.ClientId = clientId;
            inputs.tabs = "Tab2";
            using (Db db = new Db())
            {
                Application app = db.Applications.FirstOrDefault(c => c.ApplicationName.ToLower().Trim() == appInp.ApplicationName.ToLower().Trim() && c.ClientId == appInp.ClientId);
                if (app != null && (app.Id != appInp.Id))
                {
                    if (app != null)
                    {
                        TempData["IsDuplicate"] = "1";
                        ModelState.AddModelError("AppName", "Application Name already Exists");
                        // return View("NewLOBandApplications", inputs);
                        return RedirectToAction("DisplayLOBandApplications", new { appInp.ClientId, appInp.ApplicationName, tabs = "Tab2" });
                    }
                }
            }
            //appInp.Id = 100;
            appInp.IsDeleted = false;
            var newapp = appCrud.Create(appMapper.MapToEntity(appInp, appEnt));
            appInp.Id = Convert.ToInt32(newapp);


            TempData["Success"] = "Data has been saved successfully";
            return RedirectToAction("DisplayLOBandApplications", new { appInp.ClientId, tabs = "Tab2" });
        }
    }
}
