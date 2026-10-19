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

namespace NTTData.AMOPortal.WebUI.Controllers
{
    public class ClientBCLevelsController : Cruder<ClientBCLevels, ClientBCLevelsInput>
    {
        // GET: /ClientBCLevels/
        //Entities
        private ClientBCLevels entityClientBCLevels;

        // Service
        private CrudService<ClientBCLevels> clientBCLevelsService;

        //Mappers
        private Mapper<ClientBCLevels, ClientBCLevelsInput> clientBCLevelsMapper;

        //Repo
        private Repo<ClientBCLevels> repoClientBCLevels;


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

        public ClientBCLevelsController(ICrudService<ClientBCLevels> service, IMapper<ClientBCLevels, ClientBCLevelsInput> v)
            : base(service, v)
        {

            DbContextFactory Dbcon = new DbContextFactory();
            repoClientBCLevels = new Repo<ClientBCLevels>(Dbcon);
            clientBCLevelsMapper = new Mapper<ClientBCLevels, ClientBCLevelsInput>();
            entityClientBCLevels = new ClientBCLevels();
            clientBCLevelsService = new CrudService<ClientBCLevels>(repoClientBCLevels);

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
        public ActionResult DisplayClientBClevels(int ClientId = 0, int CLevel1Id = 0, int CLevel2Id = 0, int CLevel3Id = 0, string CLevel1Name = "", string CLevel2Name = "", string CLevel3Name = "", string CLevel1Create = "", string CLevel2Create = "", string CLevel3Create = "", string InExValue = "", string InTabs = "", int LOBClientBCId = 0, string LOBName = "", string LobCbcCreate = "", int AppLOBClientBCId = 0, string AppLobCbcCreate = "")
        {
            Client client = (Client)Session["Client"];
            ClientId = client.Id;
            ClientBCLevelsInput clientBCLevels = new ClientBCLevelsInput();
            clientBCLevels.ClientId = ClientId;
            clientBCLevels.CLevel1Id = CLevel1Id;
            clientBCLevels.CLevel2Id = CLevel2Id;
            clientBCLevels.CLevel3Id = CLevel3Id;
            clientBCLevels.CLevel1Name = Server.HtmlEncode(CLevel1Name);
            clientBCLevels.CLevel2Name = Server.HtmlEncode(CLevel2Name);
            clientBCLevels.CLevel3Name = Server.HtmlEncode(CLevel3Name);
            clientBCLevels.CLevel1Create = CLevel1Create;
            clientBCLevels.CLevel2Create = CLevel2Create;
            clientBCLevels.CLevel3Create = CLevel3Create;
            clientBCLevels.InExValue = InExValue;
            clientBCLevels.InTabs = InTabs;
            clientBCLevels.LOBClientBCId = LOBClientBCId;
            clientBCLevels.LOBName = LOBName;
            clientBCLevels.LobCbcCreate = LobCbcCreate;
            clientBCLevels.AppLOBClientBCId = AppLOBClientBCId;
            clientBCLevels.AppLobCbcCreate = AppLobCbcCreate;


            if (ClientId > 0)
            {
                int verticalId = clientBCLevels.ClientListbyId.VerticalId;
                int subVerticalId = clientBCLevels.ClientListbyId.SubVerticalId;

                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Verticals> v = new Data.Repo<Domain.Entities.Verticals>(Context);
                Repo<NTTData.AMOPortal.Domain.Entities.SubVerticals> s = new Data.Repo<Domain.Entities.SubVerticals>(Context);

                if (verticalId > 0)
                {
                    clientBCLevels.verticalName = v.Get(verticalId).VerticalName;
                }
                if (subVerticalId > 0)
                {
                    clientBCLevels.subVerticalName = s.Get(subVerticalId).SubVerticalName;
                }

            }
            if (ClientId > 0 && CLevel1Id > 0)
            {
                clientBCLevels.clientBCLevel1Input = clientBCLevel1Mapper.MapToInput(clientBCLevel1Service.Get(CLevel1Id));

            }


            if (CLevel1Id > 0 && CLevel2Id > 0 && CLevel3Id == 0)
            {
                clientBCLevels.clientBCLevel2Input = clientBCLevel2Mapper.MapToInput(clientBCLevel2Service.Get(CLevel2Id));

            }

            if (CLevel1Id > 0 && CLevel2Id > 0 && CLevel3Id > 0)
            {
                clientBCLevels.clientBCLevel3Input = clientBCLevel3Mapper.MapToInput(clientBCLevel3Service.Get(CLevel3Id));

            }

            if (ClientId > 0 && CLevel1Id == 0 && clientBCLevels.CLevel1Create == "Level1Create")
            {
                clientBCLevels.clientBCLevel1Input = clientBCLevel1Mapper.MapToInput(entityClientBCLevel1);
            }



            if (CLevel1Id > 0 && CLevel2Create == "Level2Create")
            {

                clientBCLevels.clientBCLevel2Input = clientBCLevel2Mapper.MapToInput(entityClientBCLevel2);

            }


            if (CLevel1Id > 0 && CLevel2Id > 0 && CLevel3Create == "Level3Create")
            {

                clientBCLevels.clientBCLevel3Input = clientBCLevel3Mapper.MapToInput(entityClientBCLevel3);

            }
            if (ClientId > 0 && (CLevel1Id > 0 || CLevel2Id > 0 || CLevel3Id > 0) && LOBClientBCId > 0 && AppLOBClientBCId == 0)
            {
                clientBCLevels.junctionLOBClientBCInput = junctionLOBClientBCMapper.MapToInput(junctionLOBClientBCService.Get(LOBClientBCId));
            }

            if (ClientId > 0 && (CLevel1Id > 0 || CLevel2Id > 0 || CLevel3Id > 0) && clientBCLevels.LobCbcCreate == "LobCbcCreate")
            {
                clientBCLevels.junctionLOBClientBCInput = junctionLOBClientBCMapper.MapToInput(entityJunctionLOBClientBC);
            }

            if (ClientId > 0 && (CLevel1Id > 0 || CLevel2Id > 0 || CLevel3Id > 0) && LOBClientBCId > 0 && AppLOBClientBCId > 0)
            {
                clientBCLevels.junctionAppLOBClientBCInput = junctionAppLOBClientBCMapper.MapToInput(junctionAppLOBClientBCService.Get(AppLOBClientBCId));
            }

            if (ClientId > 0 && (CLevel1Id > 0 || CLevel2Id > 0 || CLevel3Id > 0) && LOBClientBCId > 0 && clientBCLevels.AppLobCbcCreate == "AppLobCbcCreate")
            {
                clientBCLevels.junctionAppLOBClientBCInput = junctionAppLOBClientBCMapper.MapToInput(entityJunctionAppLOBClientBC);

            }
            if (TempData["Success"] != null)
            {
                ModelState.AddModelError("Success", "Data has been saved successfully");
            }
            return View("NewClientBCLevels", clientBCLevels);

        }

        [HttpPost]
        public ActionResult DisplayClientBClevels(ClientBCLevelsInput inputs, int ClientId = 0, int CLevel1Id = 0, int CLevel2Id = 0, int CLevel3Id = 0, string CLevel1Name = "", string CLevel2Name = "", string CLevel3Name = "", string CLevel1Create = "", string CLevel2Create = "", string CLevel3Create = "", string InExValue = "", string InTabs = "", int LOBClientBCId = 0, string LOBName = "", string LobCbcCreate = "", int AppLOBClientBCId = 0, string AppLobCbcCreate = "")
        {
            //ViewBag.StatusMessage = "";
            Client client = (Client)Session["Client"];
            ClientId = client.Id;
            inputs.ClientId = ClientId;
            inputs.CLevel1Id = CLevel1Id;
            inputs.CLevel2Id = CLevel2Id;
            inputs.CLevel3Id = CLevel3Id;
            inputs.CLevel1Name = Server.HtmlEncode(CLevel1Name);
            inputs.CLevel2Name = Server.HtmlEncode(CLevel2Name);
            inputs.CLevel3Name = Server.HtmlEncode(CLevel3Name);
            inputs.CLevel1Create = CLevel1Create;
            inputs.CLevel2Create = CLevel2Create;
            inputs.CLevel3Create = CLevel3Create;
            inputs.InExValue = InExValue;
            inputs.InTabs = InTabs;
            inputs.LOBClientBCId = LOBClientBCId;
            inputs.LOBName = LOBName;
            inputs.LobCbcCreate = LobCbcCreate;
            inputs.AppLOBClientBCId = AppLOBClientBCId;
            inputs.AppLobCbcCreate = AppLobCbcCreate;



            //Go To action account details saving
            if ((inputs.ClientId > 0 && inputs.CLevel2Id == 0 && CLevel2Create == "" && inputs.CLevel3Id == 0 && CLevel3Create == "") && (inputs.LOBClientBCId == 0 && LobCbcCreate != "LobCbcCreate"))
            {
                return (SaveLevel1Details(inputs));
            }

            if ((inputs.CLevel1Id > 0 && inputs.CLevel2Id > 0 && (inputs.CLevel3Id > 0 || CLevel3Create == "Level3Create") && (inputs.LOBClientBCId == 0 && LobCbcCreate != "LobCbcCreate") && (inputs.AppLOBClientBCId == 0 && AppLobCbcCreate != "AppLobCbcCreate")))
            {
                return (SaveLevel3Details(inputs));
            }
            if (inputs.ClientId > 0 && CLevel1Id > 0 && CLevel2Id == 0 && CLevel3Id == 0 && (inputs.LOBClientBCId > 0 || LobCbcCreate == "LobCbcCreate") && (inputs.AppLOBClientBCId == 0 && AppLobCbcCreate != "AppLobCbcCreate"))
            {
                return (SaveJunctionLOBClientBCDetails(inputs));
            }
            if (inputs.ClientId > 0 && CLevel1Id > 0 && CLevel2Id > 0 && CLevel3Id == 0 && (inputs.LOBClientBCId > 0 || LobCbcCreate == "LobCbcCreate") && (inputs.AppLOBClientBCId == 0 && AppLobCbcCreate != "AppLobCbcCreate"))
            {
                return (SaveJunctionLOBClientBCDetailsLevel2(inputs));
            }
            if (inputs.ClientId > 0 && CLevel1Id > 0 && CLevel2Id > 0 && CLevel3Id > 0 && (inputs.LOBClientBCId > 0 || LobCbcCreate == "LobCbcCreate") && (inputs.AppLOBClientBCId == 0 && AppLobCbcCreate != "AppLobCbcCreate"))
            {
                return (SaveJunctionLOBClientBCDetailsLevel3(inputs));
            }
            if (inputs.ClientId > 0 && inputs.CLevel1Id > 0 && inputs.LOBClientBCId > 0 && (inputs.AppLOBClientBCId > 0 || AppLobCbcCreate == "AppLobCbcCreate"))
            {
                return (SaveAppJunctionLOBClientBCDetails(inputs));
            }
            var input = inputs.clientBCLevel2Input;
            input.ClientLevel1Id = inputs.CLevel1Id;


            if (!ModelState.IsValid)
            {
                return View("NewClientBCLevels", inputs);
            }


            //Edit Existing Contract
            if (inputs.CLevel2Id != 0)
            {
                input.Id = inputs.CLevel2Id;
                using (Db db = new Db())
                {
                    ClientBCLevel2 existingLevel2Name = db.ClientBCLevel2.FirstOrDefault(c => c.ClientLevel2Name.ToLower().Trim() == input.ClientLevel2Name.ToLower().Trim());
                    if (existingLevel2Name != null && (existingLevel2Name.Id != input.Id))
                    {
                        if (existingLevel2Name != null)
                        {
                            ModelState.AddModelError("Level2Name", "Level2 Name already Exists");
                            return View("NewClientBCLevels", inputs);
                        }
                    }
                }
            }



            if (input.Id > 0)
            {

                clientBCLevel2Mapper.MapToEntity(input, clientBCLevel2Service.Get(input.Id));
                clientBCLevel2Service.Save();

            }
            else
            {
                //Add New Contract 

                using (Db db = new Db())
                {
                    ClientBCLevel2 existingLevel2Name = db.ClientBCLevel2.FirstOrDefault(c => c.ClientLevel2Name.ToLower().Trim() == input.ClientLevel2Name.ToLower().Trim());
                    if (existingLevel2Name != null && (existingLevel2Name.Id != input.Id))
                    {
                        if (existingLevel2Name != null)
                        {
                            ModelState.AddModelError("Level2Name", "Level2 Name already Exists");
                            return View("NewClientBCLevels", inputs);
                        }
                    }
                }
                var SLevel2Id = clientBCLevel2Service.Create(clientBCLevel2Mapper.MapToEntity(input, entityClientBCLevel2));
                input.Id = Convert.ToInt32(SLevel2Id);

            }

            TempData["Success"] = "Data has been saved successfully";
            return RedirectToAction("DisplayClientBClevels", new { CLevel1Id = inputs.CLevel1Id, CLevel1Name = inputs.CLevel1Name, CLevel2Id = input.Id, CLevel2Name = input.ClientLevel2Name, InExValue = inputs.InExValue, ClientId = inputs.ClientId });

        }



        [HttpPost]
        public ActionResult SaveLevel1Details(ClientBCLevelsInput inputs)
        {
            Client client = (Client)Session["Client"];
            inputs.ClientId = client.Id;
            ClientBCLevel1Input bc1 = inputs.clientBCLevel1Input;
            bc1.ClientId = inputs.ClientId;

            if (!ModelState.IsValid)
            {
                return View("NewClientBCLevels", inputs);
            }
            else
            {


                if (inputs.CLevel1Id > 0)
                {

                    bc1.Id = inputs.CLevel1Id;
                }


                if (bc1.Id == 0)
                {

                    var newlevel1 = clientBCLevel1Service.Create(clientBCLevel1Mapper.MapToEntity(bc1, entityClientBCLevel1));
                    bc1.Id = Convert.ToInt32(newlevel1);
                }
                else
                {

                    clientBCLevel1Mapper.MapToEntity(bc1, clientBCLevel1Service.Get(bc1.Id));
                    clientBCLevel1Service.Save();
                }


            }

            TempData["Success"] = "Data has been saved successfully";
            return RedirectToAction("DisplayClientBClevels", new { CLevel1Id = bc1.Id, CLevel1Name = bc1.ClientLevel1Name, InExValue = bc1.ClientInternalExternal, ClientId = bc1.ClientId });
        }

        [HttpPost]
        public ActionResult SaveLevel3Details(ClientBCLevelsInput inputs)
        {
            Client client = (Client)Session["Client"];
            inputs.ClientId = client.Id;
            ClientBCLevel3Input bc3 = inputs.clientBCLevel3Input;
            bc3.ClientLevel2Id = inputs.CLevel2Id;
            
            if (!ModelState.IsValid)
            {
                return View("NewClientBCLevels", inputs);
            }
            else
            {


                if (inputs.CLevel3Id > 0)
                {

                    bc3.Id = inputs.CLevel3Id;
                }




                if (bc3.Id == 0)
                {

                    var newlevel3 = clientBCLevel3Service.Create(clientBCLevel3Mapper.MapToEntity(bc3, entityClientBCLevel3));
                    bc3.Id = Convert.ToInt32(newlevel3);
                }
                else
                {

                    clientBCLevel3Mapper.MapToEntity(bc3, clientBCLevel3Service.Get(bc3.Id));
                    clientBCLevel3Service.Save();
                }
            }

            TempData["Success"] = "Data has been saved successfully";
            return RedirectToAction("DisplayClientBClevels", new { CLevel3Id = bc3.Id, CLevel3Name = bc3.ClientLevel3Name, CLevel1Id = inputs.CLevel1Id, CLevel1Name = inputs.CLevel1Name, CLevel2Id = bc3.ClientLevel2Id, CLevel2Name = inputs.CLevel2Name, InExValue = inputs.InExValue, ClientId = inputs.ClientId });
        }

        [HttpPost]
        public ActionResult SaveJunctionLOBClientBCDetails(ClientBCLevelsInput inputs)
        {
            Client client = (Client)Session["Client"];
            inputs.ClientId = client.Id;
            JunctionLOBClientBCInput jLOB = inputs.junctionLOBClientBCInput;

            jLOB.CBCLevel1Id = inputs.CLevel1Id;
            if (!ModelState.IsValid)
            {
                return View("NewClientBCLevels", inputs);
            }
            else
            {


                if (inputs.LOBClientBCId > 0)
                {

                    jLOB.Id = inputs.LOBClientBCId;
                }




                if (jLOB.Id == 0)
                {

                    var newlevel3 = junctionLOBClientBCService.Create(junctionLOBClientBCMapper.MapToEntity(jLOB, entityJunctionLOBClientBC));
                    jLOB.Id = Convert.ToInt32(newlevel3);
                }
                else
                {

                    junctionLOBClientBCMapper.MapToEntity(jLOB, junctionLOBClientBCService.Get(jLOB.Id));
                    junctionLOBClientBCService.Save();
                }
            }

            TempData["Success"] = "Data has been saved successfully";
            return RedirectToAction("DisplayClientBClevels", new { LOBClientBCId = jLOB.Id, LOBName = inputs.LOBName, CLevel1Id = inputs.CLevel1Id, CLevel1Name = inputs.CLevel1Name, InExValue = inputs.InExValue, ClientId = inputs.ClientId, AppLOBClientBCId = inputs.AppLOBClientBCId, AppLobCbcCreate = inputs.AppLobCbcCreate, CLevel2Id = inputs.CLevel2Id, CLevel2Create = inputs.CLevel2Create, InTabs = inputs.InTabs });
        }


        [HttpPost]
        public ActionResult SaveJunctionLOBClientBCDetailsLevel2(ClientBCLevelsInput inputs)
        {
            JunctionLOBClientBCInput jLOB = inputs.junctionLOBClientBCInput;

            jLOB.CBCLevel2Id = inputs.CLevel2Id;
            if (!ModelState.IsValid)
            {
                return View("NewClientBCLevels", inputs);
            }
            else
            {


                if (inputs.LOBClientBCId > 0)
                {

                    jLOB.Id = inputs.LOBClientBCId;
                }




                if (jLOB.Id == 0)
                {

                    var newlevel3 = junctionLOBClientBCService.Create(junctionLOBClientBCMapper.MapToEntity(jLOB, entityJunctionLOBClientBC));
                    jLOB.Id = Convert.ToInt32(newlevel3);
                }
                else
                {

                    junctionLOBClientBCMapper.MapToEntity(jLOB, junctionLOBClientBCService.Get(jLOB.Id));
                    junctionLOBClientBCService.Save();
                }
            }

            TempData["Success"] = "Data has been saved successfully";
            return RedirectToAction("DisplayClientBClevels", new { LOBClientBCId = jLOB.Id, LOBName = inputs.LOBName, CLevel1Id = inputs.CLevel1Id, CLevel1Name = inputs.CLevel1Name, InExValue = inputs.InExValue, ClientId = inputs.ClientId, AppLOBClientBCId = inputs.AppLOBClientBCId, AppLobCbcCreate = inputs.AppLobCbcCreate, CLevel2Id = inputs.CLevel2Id, CLevel2Create = inputs.CLevel2Create, CLevel2Name = inputs.CLevel2Name, InTabs = inputs.InTabs });
        }

        [HttpPost]
        public ActionResult SaveJunctionLOBClientBCDetailsLevel3(ClientBCLevelsInput inputs)
        {
            Client client = (Client)Session["Client"];
            inputs.ClientId = client.Id;
            JunctionLOBClientBCInput jLOB = inputs.junctionLOBClientBCInput;

            jLOB.CBCLevel3Id = inputs.CLevel3Id;
            if (!ModelState.IsValid)
            {
                return View("NewClientBCLevels", inputs);
            }
            else
            {


                if (inputs.LOBClientBCId > 0)
                {

                    jLOB.Id = inputs.LOBClientBCId;
                }




                if (jLOB.Id == 0)
                {

                    var newlevel3 = junctionLOBClientBCService.Create(junctionLOBClientBCMapper.MapToEntity(jLOB, entityJunctionLOBClientBC));
                    jLOB.Id = Convert.ToInt32(newlevel3);
                }
                else
                {

                    junctionLOBClientBCMapper.MapToEntity(jLOB, junctionLOBClientBCService.Get(jLOB.Id));
                    junctionLOBClientBCService.Save();
                }
            }

            TempData["Success"] = "Data has been saved successfully";
            return RedirectToAction("DisplayClientBClevels", new { LOBClientBCId = jLOB.Id, LOBName = inputs.LOBName, CLevel1Id = inputs.CLevel1Id, CLevel1Name = inputs.CLevel1Name, InExValue = inputs.InExValue, ClientId = inputs.ClientId, AppLOBClientBCId = inputs.AppLOBClientBCId, AppLobCbcCreate = inputs.AppLobCbcCreate, CLevel2Id = inputs.CLevel2Id, CLevel2Create = inputs.CLevel2Create, CLevel2Name = inputs.CLevel2Name, CLevel3Id = inputs.CLevel3Id, CLevel3Name = inputs.CLevel3Name, InTabs = inputs.InTabs });
        }

        [HttpPost]
        public ActionResult SaveAppJunctionLOBClientBCDetails(ClientBCLevelsInput inputs)
        {
            Client client = (Client)Session["Client"];
            inputs.ClientId = client.Id;
            JunctionAppLOBClientBCInput jLOB = inputs.junctionAppLOBClientBCInput;
            jLOB.LOBClientBCId = inputs.LOBClientBCId;
            if (!ModelState.IsValid)
            {
                return View("NewClientBCLevels", inputs);
            }
            else
            {


                if (inputs.AppLOBClientBCId > 0)
                {

                    jLOB.Id = inputs.AppLOBClientBCId;
                }




                if (jLOB.Id == 0)
                {

                    var newlevel3 = junctionAppLOBClientBCService.Create(junctionAppLOBClientBCMapper.MapToEntity(jLOB, entityJunctionAppLOBClientBC));
                    jLOB.Id = Convert.ToInt32(newlevel3);
                }
                else
                {

                    junctionAppLOBClientBCMapper.MapToEntity(jLOB, junctionAppLOBClientBCService.Get(jLOB.Id));
                    junctionLOBClientBCService.Save();
                }
            }

            TempData["Success"] = "Data has been saved successfully";
            return RedirectToAction("DisplayClientBClevels", new { LOBClientBCId = jLOB.LOBClientBCId, AppLOBClientBCId = jLOB.Id, LOBName = inputs.LOBName, CLevel1Id = inputs.CLevel1Id, CLevel1Name = inputs.CLevel1Name, CLevel2Id = inputs.CLevel2Id, CLevel2Name = inputs.CLevel2Name, InExValue = inputs.InExValue, ClientId = inputs.ClientId, CLevel3Id = inputs.CLevel3Id, CLevel3Name = inputs.CLevel3Name, CLevel2Create = inputs.CLevel2Create });
        }


        public ActionResult DelAppLOB(ClientBCLevelsInput input)
        {
            Client client = (Client)Session["Client"];
            input.ClientId = client.Id;
            var applob = junctionAppLOBClientBCService.Get(input.AppLOBClientBCId);
            if (applob != null)
            {
                int id = input.AppLOBClientBCId;

                junctionAppLOBClientBCService.Delete(id);
            }
            input.AppLOBClientBCId = 0;

            //  TempData["Success"] = "Data has been saved successfully";
            return RedirectToAction("DisplayClientBClevels", new { LOBClientBCId = input.LOBClientBCId, AppLOBClientBCId = 0, LOBName = input.LOBName, CLevel1Id = input.CLevel1Id, CLevel1Name = input.CLevel1Name, InExValue = input.InExValue, ClientId = input.ClientId, CLevel2Id = input.CLevel2Id, CLevel2Name = input.CLevel2Name, CLevel3Id = input.CLevel3Id });
        }
    }
}
