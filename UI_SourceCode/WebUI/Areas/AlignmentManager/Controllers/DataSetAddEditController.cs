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
using WebUI.Areas.AlignmentManager.Dto;
using WebUI.Dto;


namespace WebUI.Areas.AlignmentManager.Controllers
    {
    public class DataSetAddEditController : Cruder<RegDataSet, DataSetupInput>
        {

        //Entities
        private RegDataSet entityRegDataSet;

        //Service

        private CrudService<RegDataSet> RegDataSetService;

        //Mappers

        private Mapper<RegDataSet, DataSetupInput> RegDataSetMapper;

        //Repo

        private Repo<RegDataSet> repoRegDataSet;



        //Entities
        private RegisterDataset entityRegisterDataSet;

        //Service

        private CrudService<RegisterDataset> RegisterDataSetService;

        //Mappers

        private Mapper<RegisterDataset, RegisterDatasetInput> RegisterDataSetMapper;

        //Repo

        private Repo<RegisterDataset> repoRegisterDataSet;


        //Entities
        private RegisterDatasetMap entityRegisterDataSetMap;

        //Service

        private CrudService<RegisterDatasetMap> RegisterDataSetMapService;

        //Mappers

        private Mapper<RegisterDatasetMap, RegisterDatasetMapInput> RegisterDataSetMapMapper;

        //Repo

        private Repo<RegisterDatasetMap> repoRegisterDataSetMap;






        public DataSetAddEditController(ICrudService<RegDataSet> service, IMapper<RegDataSet, DataSetupInput> v)
            : base(service, v)
            {


            DbContextFactory Dbcon = new DbContextFactory();
            repoRegDataSet = new Repo<RegDataSet>(Dbcon);
            RegDataSetMapper = new Mapper<RegDataSet, DataSetupInput>();
            entityRegDataSet = new RegDataSet();
            RegDataSetService = new CrudService<RegDataSet>(repoRegDataSet);



            repoRegisterDataSet = new Repo<RegisterDataset>(Dbcon);
            RegisterDataSetMapper = new Mapper<RegisterDataset, RegisterDatasetInput>();
            entityRegisterDataSet = new RegisterDataset();
            RegisterDataSetService = new CrudService<RegisterDataset>(repoRegisterDataSet);



            repoRegisterDataSetMap = new Repo<RegisterDatasetMap>(Dbcon);
            RegisterDataSetMapMapper = new Mapper<RegisterDatasetMap, RegisterDatasetMapInput>();
            entityRegisterDataSetMap = new RegisterDatasetMap();
            RegisterDataSetMapService = new CrudService<RegisterDatasetMap>(repoRegisterDataSetMap);


          

            }


        //
        // GET: /AlignmentManager/HeatMapSetup/

        public ActionResult DisplayViewDataSet(int ClientId = 0, int RegDataSetId=0)
            {

            DataSetupInput dataSetupInput= new DataSetupInput();
            dataSetupInput.registerDataSetInput = new RegisterDatasetInput();
            //dataSetupInput.registerDataSetMapInput = new RegisterDatasetMapInput();

            if (ClientId > 0)
                {
                dataSetupInput.ClientId = ClientId;
                dataSetupInput.registerDataSetInput.ClientId = ClientId;
                dataSetupInput.regDataSetStatus = "Add";
                }
            if (RegDataSetId > 0)
                {

                dataSetupInput.registerDataSetInput.Id = RegDataSetId;
                dataSetupInput.DataSetId = RegDataSetId;
                dataSetupInput.regDataSetStatus = "Edit";
                dataSetupInput.registerDataSetInput = RegisterDataSetMapper.MapToInput(RegisterDataSetService.Get(RegDataSetId));
                
                
                }
            
            //dataSetupInput.DataSetId = RegDataSetId;

            if (TempData["Success"] != null)
                {
                ModelState.AddModelError("Success", "Data has been saved successfully");
                }


            return View("DataSetAdd", dataSetupInput);
            }




        public ActionResult DisplayDataSetColumns(int ClientId = 0, int RegDataSetId = 0, string DataSetObjectName = "", string DataSetName="")
            {

            DataSetupInput dataSetupInput = new DataSetupInput();
            dataSetupInput.registerDataSetInput = new RegisterDatasetInput();
            //dataSetupInput.registerDataSetMapInput = new RegisterDatasetMapInput();

            if (ClientId > 0)
                {
                dataSetupInput.ClientId = ClientId;
                dataSetupInput.registerDataSetInput.ClientId = ClientId;
                dataSetupInput.registerDataSetInput.DatabaseObjectName = DataSetObjectName;
                dataSetupInput.registerDataSetInput.DataSetName = DataSetName;
                }
            if (RegDataSetId > 0)
                {

                dataSetupInput.registerDataSetInput.Id = RegDataSetId;
                dataSetupInput.DataSetId = RegDataSetId;
                dataSetupInput.registerDataSetInput.DataSetName = DataSetName;
                dataSetupInput.registerDataSetInput = RegisterDataSetMapper.MapToInput(RegisterDataSetService.Get(RegDataSetId));
                if (dataSetupInput.registerDataSetInput.DatabaseObjectName != DataSetObjectName)
                    {
                    dataSetupInput.registerDataSetInput.DatabaseObjectName = DataSetObjectName;
                    //Need to re-populate the Grid with New source data
                    }


                }

            //dataSetupInput.DataSetId = RegDataSetId;


            return View("DataSetAdd", dataSetupInput);
            }


                public ActionResult DisplayHomePage(int ClientId = 0)
            {

            DataSetupInput dataSetupInput= new DataSetupInput(); 
            if (ClientId > 0)
                {
                dataSetupInput.ClientId = ClientId;
                
                }
            

            return View("DataSetAdd", dataSetupInput);
            }


                public ActionResult DeleteDatSetName(int RegDataSetId = 0)
                {

                RegisterDatasetInput registerDataSetInput = RegisterDataSetMapper.MapToInput(RegisterDataSetService.Get(RegDataSetId));
                registerDataSetInput.IsDeleted = true;
                registerDataSetInput.LastUpdatedDate = DateTime.Now;
                registerDataSetInput.LastUpdatedUser = User.Identity.Name;
                RegisterDataSetMapper.MapToEntity(registerDataSetInput, RegisterDataSetService.Get(RegDataSetId));
                RegisterDataSetService.Save();
                int ClientId = registerDataSetInput.ClientId;
                return RedirectToAction("DisplayHomePage", "DataSet", new { ClientId = ClientId });
                    
                }


                [HttpPost]
                public ActionResult SaveRegisterDataSet(FormCollection form, DataSetupInput inputs, int ClientId = 0, int RegDataSetId = 0, string RegDataSetCreate = "")
                {

                string selectRow = form.GetValue("radiochk").AttemptedValue;
                string VariableType = form.GetValue("virtualColumn"+selectRow).AttemptedValue;
                string sourceColumnName = form.GetValue("sourecolumn" + selectRow).AttemptedValue;
                int datasetmapid = Convert.ToInt32(form.GetValue("datasetmapid"+selectRow).AttemptedValue);

                ViewData.Add("selectRow", selectRow);
                ViewData.Add("vircolname", VariableType);

                RegisterDatasetInput regDataSetInput = inputs.registerDataSetInput;
                inputs.registerDataSetInput.Id = RegDataSetId;
                inputs.registerDataSetInput.ClientId = ClientId;

                RegisterDatasetMapInput regDataSetMapInput = new RegisterDatasetMapInput();

                if (inputs.registerDataSetInput.Id > 0)
                    {
                    //Existing Entry

                    RegisterDataSetMapper.MapToEntity(inputs.registerDataSetInput, RegisterDataSetService.Get(inputs.registerDataSetInput.Id));
                    RegisterDataSetService.Save();

                    //Delete Old Entry From mapInput
                    //if(datasetmapid > 0)

                    var oldMapRows =  RegisterDataSetMapService.Where(regid => regid.RegisterDataSetId == inputs.registerDataSetInput.Id);

                    foreach (var oldId in oldMapRows)
                    {
                    RegisterDataSetMapService.Delete(oldId.Id);    
                    }

                        

                    //var Id = RegisterDataSetService.Create(RegisterDataSetMapper.MapToEntity(inputs.registerDataSetInput, entityRegisterDataSet));
                    int registerDataSetId = inputs.registerDataSetInput.Id;

                    RegisterDatasetMapInput mapInput = new RegisterDatasetMapInput();
                    mapInput.DataSetColumnName = sourceColumnName;
                    mapInput.VariableType = VariableType;
                    mapInput.RegisterDataSetId = inputs.registerDataSetInput.Id;
                    mapInput.CreatedByUser = User.Identity.Name;
                    mapInput.CreatedDate = DateTime.Now;

                    var id = RegisterDataSetMapService.Create(RegisterDataSetMapMapper.MapToEntity(mapInput, entityRegisterDataSetMap));




                    }
                else
                    {
                     //New Entry
                    //DbContextFactory Dbcon = new DbContextFactory();
                    //repoRegisterDataSet = new Repo<RegisterDataset>(Dbcon);
                    //RegisterDataSetMapper = new Mapper<RegisterDatasetMap, RegisterDatasetMapInput>();
                    //RegisterDataSetService = new CrudService<RegisterDatasetMap>(repoRegDataSet);
                    //entityRegDataSet = new RegisterDatasetMap();

                    //variableInput.variableLabelInput.FullDescription = variableInput.GetvariableLabelInput.FullDescription;
                    //variableInput.variableLabelInput.VariableLabel = variableInput.GetvariableLabelInput.VariableLabel;
                    //variableInput.variableLabelInput.Id = variableInput.VariableLabelId;
                    //variableInput.variableLabelInput.CreatedByUser = User.Identity.Name;

                    var Id = RegisterDataSetService.Create(RegisterDataSetMapper.MapToEntity(inputs.registerDataSetInput, entityRegisterDataSet));
                    int registerDataSetId = Convert.ToInt32(Id);

                    RegisterDatasetMapInput mapInput = new RegisterDatasetMapInput();
                    mapInput.DataSetColumnName = sourceColumnName;
                    mapInput.VariableType = VariableType;
                    mapInput.RegisterDataSetId = Id;
                    mapInput.CreatedByUser = User.Identity.Name;
                    mapInput.CreatedDate = DateTime.Now;

                    var id = RegisterDataSetMapService.Create(RegisterDataSetMapMapper.MapToEntity(mapInput, entityRegisterDataSetMap));
                    RegDataSetId = registerDataSetId;

                    }





                
                
                //return View("DataSetAdd", inputs);
                TempData["Success"] = "Data has been saved successfully";
                     
                    return RedirectToAction("DisplayViewDataSet", new {ClientId = ClientId, RegDataSetId=RegDataSetId});
                }



        //[HttpGet]
        //public ActionResult DisplayClientHeatMaps(int ClientId = 0)
        //    {
        //            heatMapSetupInput varheatMapSetupInput = new heatMapSetupInput(ClientId);
        //            return View("HeatMapSetupHomePage", varheatMapSetupInput);

        //    }
        //}


                [HttpPost]
                public JsonResult CheckDatSetName(string datasetName="", int dataSetId=0)
                    {


                    DbContextFactory Dbcon = new DbContextFactory();
                    Db db = new Db();
                    var existDataSetNames = from dataSetNames in db.registerDataset
                                        where dataSetNames.DataSetName.ToLower() == datasetName.ToLower()
                                        select new 
                                        {
                                            Id = dataSetNames.Id,
                                            Name = dataSetNames.DataSetName
                                        };
                        {}

                    bool resultFound=false;
                    if(dataSetId > 0)
                      {
                        if(existDataSetNames.Where(c=> c.Id != dataSetId).Count() > 0)
                            resultFound=true;
                      }
                    if(dataSetId == 0 && existDataSetNames.Count() > 0)
                        {
                            resultFound = true;
                        }
                    return Json(resultFound, JsonRequestBehavior.AllowGet);
                    }

        }


    }
