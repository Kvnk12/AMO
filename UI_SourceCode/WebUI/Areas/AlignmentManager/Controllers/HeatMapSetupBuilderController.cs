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
using WebUI.Areas.AlignmentManager.Objects;
using System.Text;

namespace WebUI.Areas.AlignmentManager.Controllers
{

    public class HeatMapSetupBuilderController : Cruder<HeatMapBuilderSetup, HeatMapBuilderSetupInput>
    {


        //Entities
        private ReportConfiguration entityReportConfiguration;
        private ReportVariable entityReportVariable;
        private ReportDataSet entityReportDataSet;
        private ReportFilter entityReportFilter;
        private ReportAlgorithmMD entityReportAlgorithmMD;
        private ReportAlgorithm entityReportAlgorithm;
        private ReportThreshold entityReportThreshold;
        private ReportalgorithmDefn entityReportalgorithmDefn;
        private ReportGraphConfiguration entityReportGraphConfiguration;
        private ReportConfigurationMD entityReportConfigurationMD;
        private ReportAlgorithmStageMD entityReportAlgorithmStageMD;

        //Service

        private CrudService<ReportConfiguration> ServiceReportConfiguration;
        private CrudService<ReportVariable> ServiceReportVariable;
        private CrudService<ReportDataSet> ServiceReportDataSet;
        private CrudService<ReportFilter> ServiceReportFilter;
        private CrudService<ReportAlgorithmMD> ServiceReportAlgorithmMD;
        private CrudService<ReportAlgorithm> ServiceReportAlgorithm;
        private CrudService<ReportThreshold> ServiceReportThreshold;
        private CrudService<ReportalgorithmDefn> ServiceReportalgorithmDefn;
        private CrudService<ReportGraphConfiguration> ServiceReportGraphConfiguration;
        private CrudService<ReportConfigurationMD> ServiceReportConfigurationMD;
        private CrudService<ReportAlgorithmStageMD> ServiceReportAlgorithmStageMD;

        //Mappers

        private Mapper<ReportConfiguration, ReportConfiguationInput> MapperReportConfiguration;
        private Mapper<ReportVariable, ReportVariableInput> MapperReportVariable;
        private Mapper<ReportDataSet, ReportDataSetInput> MapperReportDataSet;
        private Mapper<ReportFilter, ReportFilterInput> MapperReportFilter;
        private Mapper<ReportAlgorithmMD, ReportAlgorithmMDInput> MapperReportAlgorithmMD;
        private Mapper<ReportAlgorithm, ReportAlgorithmInput> MapperReportAlgorithm;
        private Mapper<ReportThreshold, ReportThresholdInput> MapperReportThreshold;
        private Mapper<ReportGraphConfiguration, ReportGraphConfigurationInput> MapperReportGraphConfiguration;
        private Mapper<ReportConfigurationMD, ReportConfigurationMDInput> MapperReportConfigurationMD;


        //Repo

        private Repo<ReportConfiguration> repoReportConfiguration;
        private Repo<ReportVariable> repoReportVariable;
        private Repo<ReportDataSet> repoReportDataSet;
        private Repo<ReportFilter> repoReportFilter;
        private Repo<ReportAlgorithmMD> repoReportAlgorithmMD;
        private Repo<ReportAlgorithm> repoReportAlgorithm;
        private Repo<ReportThreshold> repoReportThreshold;
        private Repo<ReportalgorithmDefn> repoReportalgorithmDefn;
        private Repo<ReportGraphConfiguration> repoReportGraphConfiguration;
        private Repo<ReportConfigurationMD> repoReportConfigurationMD;
        private Repo<ReportAlgorithmStageMD> repoReportAlgorithmStageMD;


        public HeatMapSetupBuilderController(ICrudService<HeatMapBuilderSetup> service, IMapper<HeatMapBuilderSetup, HeatMapBuilderSetupInput> v)
            : base(service, v)
        {


            DbContextFactory Dbcon = new DbContextFactory();
            repoReportConfiguration = new Repo<ReportConfiguration>(Dbcon);
            MapperReportConfiguration = new Mapper<ReportConfiguration, ReportConfiguationInput>();
            entityReportConfiguration = new ReportConfiguration();
            ServiceReportConfiguration = new CrudService<ReportConfiguration>(repoReportConfiguration);


            repoReportGraphConfiguration = new Repo<ReportGraphConfiguration>(Dbcon);
            MapperReportGraphConfiguration = new Mapper<ReportGraphConfiguration, ReportGraphConfigurationInput>();
            entityReportGraphConfiguration = new ReportGraphConfiguration();
            ServiceReportGraphConfiguration = new CrudService<ReportGraphConfiguration>(repoReportGraphConfiguration);


            repoReportConfigurationMD = new Repo<ReportConfigurationMD>(Dbcon);
            MapperReportConfigurationMD = new Mapper<ReportConfigurationMD, ReportConfigurationMDInput>();
            entityReportConfigurationMD = new ReportConfigurationMD();
            ServiceReportConfigurationMD = new CrudService<ReportConfigurationMD>(repoReportConfigurationMD);




            repoReportVariable = new Repo<ReportVariable>(Dbcon);
            MapperReportVariable = new Mapper<ReportVariable, ReportVariableInput>();
            entityReportVariable = new ReportVariable();
            ServiceReportVariable = new CrudService<ReportVariable>(repoReportVariable);



            repoReportDataSet = new Repo<ReportDataSet>(Dbcon);
            MapperReportDataSet = new Mapper<ReportDataSet, ReportDataSetInput>();
            entityReportDataSet = new ReportDataSet();
            ServiceReportDataSet = new CrudService<ReportDataSet>(repoReportDataSet);


            repoReportFilter = new Repo<ReportFilter>(Dbcon);
            MapperReportFilter = new Mapper<ReportFilter, ReportFilterInput>();
            entityReportFilter = new ReportFilter();
            ServiceReportFilter = new CrudService<ReportFilter>(repoReportFilter);


            repoReportAlgorithmMD = new Repo<ReportAlgorithmMD>(Dbcon);
            MapperReportAlgorithmMD = new Mapper<ReportAlgorithmMD, ReportAlgorithmMDInput>();
            entityReportAlgorithmMD = new ReportAlgorithmMD();
            ServiceReportAlgorithmMD = new CrudService<ReportAlgorithmMD>(repoReportAlgorithmMD);


            repoReportAlgorithm = new Repo<ReportAlgorithm>(Dbcon);
            MapperReportAlgorithm = new Mapper<ReportAlgorithm, ReportAlgorithmInput>();
            entityReportAlgorithm = new ReportAlgorithm();
            ServiceReportAlgorithm = new CrudService<ReportAlgorithm>(repoReportAlgorithm);




            repoReportThreshold = new Repo<ReportThreshold>(Dbcon);
            MapperReportThreshold = new Mapper<ReportThreshold, ReportThresholdInput>();
            entityReportThreshold = new ReportThreshold();
            ServiceReportThreshold = new CrudService<ReportThreshold>(repoReportThreshold);


            repoReportalgorithmDefn = new Repo<ReportalgorithmDefn>(Dbcon);
            entityReportalgorithmDefn = new ReportalgorithmDefn();
            ServiceReportalgorithmDefn = new CrudService<ReportalgorithmDefn>(repoReportalgorithmDefn);

            repoReportAlgorithmStageMD = new Repo<ReportAlgorithmStageMD>(Dbcon);
            entityReportAlgorithmStageMD = new ReportAlgorithmStageMD();
            ServiceReportAlgorithmStageMD = new CrudService<ReportAlgorithmStageMD>(repoReportAlgorithmStageMD);


        }



        [HttpGet]
        public ActionResult DisplayHeatMapBuilder(int ClientId = 0)
        {

            HeatMapBuilderSetupInput heatMapBuilderSetupInput = new HeatMapBuilderSetupInput(ClientId);
            heatMapBuilderSetupInput.reportConfiguationInput = new ReportConfiguationInput();

            if (ClientId > 0)
            {
                heatMapBuilderSetupInput.ClientId = ClientId;
                heatMapBuilderSetupInput.reportConfiguationInput.ClientId = ClientId;
                heatMapBuilderSetupInput.HeatMapReportStatus = "Add";
            }

            //dataSetupInput.DataSetId = RegDataSetId;

            if (TempData["Success"] != null)
            {
                ModelState.AddModelError("Success", "Data has been saved successfully");
            }


            return View("SetupHeatMapBuilderHome", heatMapBuilderSetupInput);
        }




        [HttpGet]
        public ActionResult SetupHeatMapBuilder(int ClientId = 0, int ReportId = 0, string ReportType = "")
        {

            HeatMapBuilderSetupInput heatMapBuilderSetupInput = new HeatMapBuilderSetupInput(ClientId);
            heatMapBuilderSetupInput.reportConfiguationInput = new ReportConfiguationInput();
            heatMapBuilderSetupInput.HeatMapReportStatus = ReportType;

            if (ClientId > 0)
            {
                heatMapBuilderSetupInput.ClientId = ClientId;
                heatMapBuilderSetupInput.reportConfiguationInput.ClientId = ClientId;
                heatMapBuilderSetupInput.HeatMapReportStatus = "Add";
            }
            if (ReportId > 0)
            {

                heatMapBuilderSetupInput.reportConfiguationInput.Id = ReportId;
                heatMapBuilderSetupInput.ReportId = ReportId;
                heatMapBuilderSetupInput.HeatMapReportStatus = "Edit";
                heatMapBuilderSetupInput.reportConfiguationInput = MapperReportConfiguration.MapToInput(ServiceReportConfiguration.Get(ReportId));
                ReportVariable reportVariable = ServiceReportVariable.Get(Convert.ToInt32(heatMapBuilderSetupInput.reportConfiguationInput.ReportVariableId));
                heatMapBuilderSetupInput.reportConfiguationInput.VariableTypeId = reportVariable == null ? 0 : reportVariable.VariableTypeId;
                ReportDataSet reportDataSet = ServiceReportDataSet.GetAll().Where(c => c.ReportConfigId == ReportId).FirstOrDefault();
                if (reportDataSet != null)
                {
                    heatMapBuilderSetupInput.reportConfiguationInput.RegisterDataSetId = reportDataSet.RegisterDataSetId;
                    heatMapBuilderSetupInput.reportConfiguationInput.reportDataSetInput = new ReportDataSetInput();
                    heatMapBuilderSetupInput.reportConfiguationInput.reportDataSetInput.RegisterDataSetId = reportDataSet.RegisterDataSetId;
                    heatMapBuilderSetupInput.reportConfiguationInput.reportDataSetInput.ReportConfigId = ReportId;
                }

                //set RegisterDataSetid



            }

            //dataSetupInput.DataSetId = RegDataSetId;

            if (TempData["Success"] != null)
            {
                ModelState.AddModelError("Success", "Data has been saved successfully");
            }


            return View("HeatMapAdd", heatMapBuilderSetupInput);
        }



        public ActionResult DisplayDataSets(int clientId = 0, int ReportId = 0, int vartypeid = 0, string vartypename = "", string ReportType = "", string ReportName = "")
        {



            HeatMapBuilderSetupInput heatMapBuilderSetupInput = new HeatMapBuilderSetupInput(clientId);
            heatMapBuilderSetupInput.reportConfiguationInput = new ReportConfiguationInput();
            heatMapBuilderSetupInput.reportConfiguationInput.VariableTypeId = vartypeid;
            heatMapBuilderSetupInput.reportConfiguationInput.variableTypeName = vartypename;
            heatMapBuilderSetupInput.reportConfiguationInput.ClientId = clientId;
            heatMapBuilderSetupInput.HeatMapReportStatus = ReportType;
            heatMapBuilderSetupInput.reportConfiguationInput.ReportName = ReportName;


            return View("HeatMapAdd", heatMapBuilderSetupInput);


        }

        public ActionResult GetDataSetColumnsForSelectedDataSet(int clientId = 0, int ReportId = 0, int vartypeid = 0, string vartypename = "", string ReportType = "", int DataSetId = 0, string ReportName = "")
        {

            HeatMapBuilderSetupInput heatMapBuilderSetupInput = new HeatMapBuilderSetupInput(clientId);
            heatMapBuilderSetupInput.reportConfiguationInput = new ReportConfiguationInput();
            heatMapBuilderSetupInput.reportConfiguationInput.VariableTypeId = vartypeid;
            heatMapBuilderSetupInput.reportConfiguationInput.variableTypeName = vartypename;
            heatMapBuilderSetupInput.reportConfiguationInput.ClientId = clientId;
            heatMapBuilderSetupInput.HeatMapReportStatus = ReportType;
            heatMapBuilderSetupInput.reportConfiguationInput.RegisterDataSetId = DataSetId;
            heatMapBuilderSetupInput.reportConfiguationInput.ReportName = ReportName;

            heatMapBuilderSetupInput.reportConfiguationInput.reportDataSetInput = new ReportDataSetInput();
            heatMapBuilderSetupInput.reportConfiguationInput.reportDataSetInput.RegisterDataSetId = DataSetId;
            heatMapBuilderSetupInput.reportConfiguationInput.reportDataSetInput.ReportConfigId = ReportId;



            return View("HeatMapAdd", heatMapBuilderSetupInput);

        }





        //[HttpPost]
        //public JsonResult CheckColumnCountForAlgorithm(string algorithmName)
        //    {




        //    DbContextFactory Dbcon = new DbContextFactory();
        //    Db db = new Db();
        //    var reportAlgorCongiration = from ReportReportalgorithmDefn in db.ReportReportalgorithmDefn
        //                                 where ReportReportalgorithmDefn.AlgorithmName.ToLower() == algorithmName.ToLower()
        //                            select new
        //                            {
        //                                NoofColumns = ReportReportalgorithmDefn.NoOfColumnsToSelect
        //                            };


        //    var resultFound = reportAlgorCongiration.FirstOrDefault().NoofColumns.ToString();
        //    return Json(resultFound, JsonRequestBehavior.AllowGet);
        //    }



        [HttpPost]
        public JsonResult CheckHeatMapReportName(string ReportName = "", int ReportId = 0, string reportType = "Add")
        {


            DbContextFactory Dbcon = new DbContextFactory();
            Db db = new Db();
            var existDataSetNames = from dataSetNames in db.reportConfiguration
                                    where dataSetNames.ReportName.ToLower() == ReportName.ToLower()
                                    select new
                                    {
                                        Id = dataSetNames.Id,
                                        Name = dataSetNames.ReportName
                                    };
            { }

            bool resultFound = false;
            if (ReportId > 0)
            {
                if (existDataSetNames.Where(c => c.Id != ReportId).Count() > 0)
                    resultFound = true;
            }
            if ((ReportId == 0) || (reportType == "Add" && ReportId > 0))
            {
                var ids = existDataSetNames.Where(c => c.Id != ReportId).ToList();
                if (ids.Count() > 0)
                    resultFound = true;
            }


            return Json(resultFound, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult DisplayHeatMapFilterSetup(int clientId = 186, int ReportId = 27, string ReportType = "Add")
        {

            HeatMapBuilderSetupInput heatMapBuilderSetupInput = new HeatMapBuilderSetupInput(clientId);
            heatMapBuilderSetupInput.ReportId = ReportId;
            heatMapBuilderSetupInput.HeatMapReportStatus = ReportType;
            heatMapBuilderSetupInput.reportFilterInput = new ReportFilterInput();
            heatMapBuilderSetupInput.reportFilterInput.ReportConfigId = ReportId;
            return View("HeatMapFilterSetUp", heatMapBuilderSetupInput);


        }


        [HttpPost]
        public ActionResult SaveHeatMapThreshold(FormCollection form, HeatMapBuilderSetupInput heatMapInput, int clientId = 0, int ReportId = 0, string ReportType = "")
        {


            //if (ReportType == "Add")
            {
                if (ReportId > 0)
                {
                    IEnumerable<ReportThreshold> rth = ServiceReportThreshold.GetAll().Where(c => c.ReportConfigId == ReportId);
                    foreach (ReportThreshold threshold in rth)
                    {
                        ServiceReportThreshold.Delete(threshold.Id);
                    }
                }
                //Get Algorithm Id

                ReportAlgorithm rmd = ServiceReportAlgorithm.GetAll().Where(c => c.ReportConfigId == ReportId).FirstOrDefault();
                int algorithemId = 0;
                if (rmd != null)
                {
                    algorithemId = rmd.Id;
                }



                string[] keys = form.AllKeys;
                string[] colorkeys = new string[] { "red", "blue", "green", "yellow", "orange" };
                foreach (string key in colorkeys)
                {
                    string mintxtkey = "txt" + key + "min";
                    string maxtxtkey = "txt" + key + "max";
                    string distxtkey = "txt" + key + "dis";
                    string mintxvalue = ""; //form.GetValue(mintxtkey).AttemptedValue;
                    string maxtxtvalue = "";  //form.GetValue(maxtxtkey).AttemptedValue;
                    string distxtvalue = "";
                    if (keys.Contains(distxtkey))
                    {
                        distxtvalue = form.GetValue(distxtkey).AttemptedValue;
                    }
                    if (keys.Contains(mintxtkey))
                    {
                        mintxvalue = form.GetValue(mintxtkey).AttemptedValue;
                    }
                    if (keys.Contains(maxtxtkey))
                    {
                        maxtxtvalue = form.GetValue(maxtxtkey).AttemptedValue;
                    }


                    //else
                    //{
                    //distxtvalue = "";
                    //}
                    if (mintxvalue != "" || maxtxtvalue != "" || distxtvalue != "")
                    {
                        //if (distxtvalue != "")
                        //{
                        //mintxvalue = "";
                        //maxtxtvalue = distxtvalue;
                        //}

                        ReportThresholdInput rthinput = new ReportThresholdInput();
                        rthinput.AlgorithmId = algorithemId;
                        rthinput.ReportConfigId = ReportId;
                        rthinput.MaximumThreshold = maxtxtvalue;
                        rthinput.MinimumThreshold = mintxvalue;
                        rthinput.DiscreetThreshold = distxtvalue;
                        rthinput.ThresholdColor = key;
                        rthinput.CreatedByUser = User.Identity.Name;
                        rthinput.CreatedDate = DateTime.Now;
                        var thresholdId = ServiceReportThreshold.Create(MapperReportThreshold.MapToEntity(rthinput, entityReportThreshold));

                    }



                }

            }

            HeatMapBuilderSetupInput heatMapBuilderSetupInput = new HeatMapBuilderSetupInput(clientId);

            //heatMapBuilderSetupInput.ReportId = ReportId;
            //heatMapBuilderSetupInput.HeatMapReportStatus = ReportType;
            ////heatMapBuilderSetupInput.reportFilterInput = new ReportFilterInput();
            ////heatMapBuilderSetupInput.reportFilterInput.ReportConfigId = ReportId;
            TempData["Success"] = "Success";

            return RedirectToAction("GetHeatMapThreshold", new { ClientId = clientId, ReportId = ReportId });

        }


        [HttpPost]
        public ActionResult SaveHeatMapAlgorithmReport(FormCollection form, HeatMapBuilderSetupInput heatMapInput, int clientId = 0, int ReportId = 0, string ReportType = "")
        {


            //return RedirectToAction("GetHeatMapThreshold", new { clientId = clientId, ReportId = ReportId, ReportType = ReportType });

            //if (ReportType == "Add")
            {
                ReportAlgorithmInput ralogInput = heatMapInput.reportAlgorithmInput;
                ReportAlgorithm rm = ServiceReportAlgorithm.GetAll().Where(c => c.ReportConfigId == ReportId).FirstOrDefault();
                if (rm != null)
                {


                    //Delete Corresponding entry in the 
                    IEnumerable<ReportAlgorithmMD> rmdCollection = ServiceReportAlgorithmMD.GetAll().Where(c => c.AlgorithmId == rm.Id);
                    foreach (ReportAlgorithmMD item in rmdCollection)
                    {
                        ServiceReportAlgorithmMD.Delete(item.Id);
                    }

                    //ServiceReportAlgorithm.Delete(rm.Id);
                }

                //Delete Previous Entries  from reportAlgorithmStageMd Table
                if (rm != null)
                {
                    IEnumerable<ReportAlgorithmStageMD> rmdstageCollection = ServiceReportAlgorithmStageMD.GetAll().Where(c => c.AlgorithmId == rm.Id);
                    foreach (ReportAlgorithmStageMD item in rmdstageCollection)
                    {
                        ServiceReportAlgorithmStageMD.Delete(item.Id);
                    }
                }


                //string AlgorithmSolution = ralogInput.AlgorithmSolution.Split(new[] { '~' }).ToArray()[0];

                //Selected Columns 
                //string selectedColumns = "";
                //selectedColumns = form.GetValue("SelectedColumns").AttemptedValue.ToString().Replace("\r\n", ",");
                //if (selectedColumns.Substring(selectedColumns.Length - 1) == ",")
                //{
                //selectedColumns = selectedColumns.Substring(0, selectedColumns.Length - 1);
                //}


                //Selected PartitionBy Columns 
                //string SelectedPartionByColumns = "";
                //SelectedPartionByColumns = form.GetValue("SelectedPartionByColumns").AttemptedValue.ToString().Replace("\r\n", ",");
                //if (SelectedPartionByColumns !="" &&  SelectedPartionByColumns.Substring(SelectedPartionByColumns.Length - 1) == ",")
                //    {
                //    SelectedPartionByColumns = SelectedPartionByColumns.Substring(0, selectedColumns.Length - 1);
                //    }


                //Selected PartitionDateBy Columns 
                //string SelectedPartionDateColumns = "";
                //SelectedPartionDateColumns = form.GetValue("SelectedPartionDateColumns").AttemptedValue.ToString().Replace("\r\n", ",");
                //if (SelectedPartionDateColumns !="" && SelectedPartionDateColumns.Substring(SelectedPartionDateColumns.Length - 1) == ",")
                //    {
                //    SelectedPartionDateColumns = SelectedPartionDateColumns.Substring(0, selectedColumns.Length - 1);
                //    }

                //9290303663 (ramana 14289458267)


                //Report Algorithm Input
                ralogInput.ReportConfigId = ReportId;
                ralogInput.AlgorithmSolution = form.GetValue("algorId").AttemptedValue;
                IEnumerable<ReportalgorithmDefn> lstRepoDefn = ServiceReportalgorithmDefn.GetAll().Where(c => c.AlgorithmSolution == ralogInput.AlgorithmSolution).OrderBy(c => c.Order).ToList();
                ralogInput.AlgorithmName = lstRepoDefn.FirstOrDefault().AlgorithmName;



                //ralogInput.AlgorithmSolution = AlgorithmSolution;
                var reportAlogId = 0;
                if (rm != null)
                {
                    reportAlogId = rm.Id;
                    ralogInput.LastUpdatedUser = User.Identity.Name;
                    ralogInput.LastUpdatedDate = DateTime.Now;
                    ralogInput.Id = rm.Id;
                    //ReportAlgorithmInput existingAlgo  = MapperReportAlgorithm.MapToInput(rm);
                    entityReportAlgorithm = ServiceReportAlgorithm.Get(rm.Id);
                    MapperReportAlgorithm.MapToEntity(ralogInput, entityReportAlgorithm);
                    ServiceReportAlgorithm.Save();

                }
                else
                {
                    ralogInput.CreatedByUser = User.Identity.Name;
                    ralogInput.CreatedDate = DateTime.Now;
                    reportAlogId = ServiceReportAlgorithm.Create(MapperReportAlgorithm.MapToEntity(ralogInput, entityReportAlgorithm));
                }

                ReportGraphConfiguration rgraphConfiguration = ServiceReportGraphConfiguration.GetAll().Where(c => c.ReportConfigId == ReportId).FirstOrDefault();
                ReportGraphConfigurationInput rgraphConfigInput = ralogInput.reportGraphConfigurationInput;
                rgraphConfigInput.ReportConfigId = ReportId;
                if (rgraphConfiguration != null)
                {
                    rgraphConfigInput.LastUpdatedUser = User.Identity.Name;
                    rgraphConfigInput.LastUpdatedDate = DateTime.Now;
                    rgraphConfigInput.Id = rgraphConfiguration.Id;
                    entityReportGraphConfiguration = ServiceReportGraphConfiguration.Get(rgraphConfigInput.Id);
                    MapperReportGraphConfiguration.MapToEntity(rgraphConfigInput, entityReportGraphConfiguration);
                    ServiceReportGraphConfiguration.Save();
                }
                else
                {

                    rgraphConfigInput.CreatedByUser = User.Identity.Name;
                    rgraphConfigInput.CreatedDate = DateTime.Now;
                    var rgraphConfigId = ServiceReportGraphConfiguration.Create(MapperReportGraphConfiguration.MapToEntity(rgraphConfigInput, entityReportGraphConfiguration));
                }

                //Inserting ReportConfiguartionMD

                IEnumerable<ReportConfigurationMD> rConfigurationMDList = ServiceReportConfigurationMD.GetAll().Where(c => c.ReportConfigId == ReportId);
                foreach (ReportConfigurationMD rconf in rConfigurationMDList)
                {
                    ServiceReportConfigurationMD.Delete(rconf.Id);
                }


                ReportConfigurationMDInput rconfigMdInput = new ReportConfigurationMDInput();
                rconfigMdInput.Key = "XAxis1";
                rconfigMdInput.ReportConfigId = ReportId;
                rconfigMdInput.Value = heatMapInput.reportAlgorithmInput.reportConfigurationMDInput.Xaxis1;
                rconfigMdInput.CreatedByUser = User.Identity.Name;
                rconfigMdInput.CreatedDate = DateTime.Now;
                var rconfigMdId = ServiceReportConfigurationMD.Create(MapperReportConfigurationMD.MapToEntity(rconfigMdInput, entityReportConfigurationMD));

                rconfigMdInput = new ReportConfigurationMDInput();
                rconfigMdInput.Key = "XAxis2";
                rconfigMdInput.ReportConfigId = ReportId;
                rconfigMdInput.Value = heatMapInput.reportAlgorithmInput.reportConfigurationMDInput.Xaxis2;
                rconfigMdInput.CreatedByUser = User.Identity.Name;
                rconfigMdInput.CreatedDate = DateTime.Now;
                rconfigMdId = ServiceReportConfigurationMD.Create(MapperReportConfigurationMD.MapToEntity(rconfigMdInput, entityReportConfigurationMD));


                rconfigMdInput = new ReportConfigurationMDInput();
                rconfigMdInput.Key = "YAxis1";
                rconfigMdInput.ReportConfigId = ReportId;
                rconfigMdInput.Value = heatMapInput.reportAlgorithmInput.reportConfigurationMDInput.Yaxis1;
                rconfigMdInput.CreatedByUser = User.Identity.Name;
                rconfigMdInput.CreatedDate = DateTime.Now;
                rconfigMdId = ServiceReportConfigurationMD.Create(MapperReportConfigurationMD.MapToEntity(rconfigMdInput, entityReportConfigurationMD));

                rconfigMdInput = new ReportConfigurationMDInput();
                rconfigMdInput.Key = "YAxis2";
                rconfigMdInput.ReportConfigId = ReportId;
                rconfigMdInput.Value = heatMapInput.reportAlgorithmInput.reportConfigurationMDInput.Yaxis2;
                rconfigMdInput.CreatedByUser = User.Identity.Name;
                rconfigMdInput.CreatedDate = DateTime.Now;
                rconfigMdId = ServiceReportConfigurationMD.Create(MapperReportConfigurationMD.MapToEntity(rconfigMdInput, entityReportConfigurationMD));




                //ReportConfiguration  rconfig = ServiceReportConfiguration.Get(ReportId);
                //ReportConfiguationInput rconfigInput =MapperReportConfiguration.MapToInput(rconfig);
                //rconfigInput.AlgorithmId = reportAlogId;
                //MapperReportConfiguration.MapToEntity(rconfigInput,rconfig);
                //ServiceReportConfiguration.Save();


                // Insert ReportAlogorithmMd






                foreach (ReportalgorithmDefn item in lstRepoDefn)
                {
                    string KeyFieldName = item.Type + "_" + item.Order + "_" + item.DisplayName;
                    if (!item.IsDeleted)
                    {
                        string SelectedValue = form.GetValue(KeyFieldName).AttemptedValue;
                        if (SelectedValue != "")
                        {
                            ReportAlgorithmStageMD rmd = new ReportAlgorithmStageMD();
                            rmd.AlgorithmId = reportAlogId;
                            rmd.key = item.DisplayName;
                            rmd.Value = SelectedValue;
                            rmd.KeyNames = "";
                            ServiceReportAlgorithmStageMD.Create(rmd);
                        }
                    }
                }

                List<SelectListItem> lst = lstRepoDefn.ToList().GroupBy(m => new { m.KeyName }).Select(m =>
                 new SelectListItem
                 {
                     Value = m.Key.KeyName,
                     Text = m.Key.KeyName
                 }).ToList();

                IList<ReportAlgorithmStageMD> stgMdList = AlignmentHelper.GetReportAlgorithmStageMd(reportAlogId);

                foreach (SelectListItem keyItem in lst)
                {
                    List<string> delimtedValues = new List<string>();
                    List<string> delimitedKeys = new List<string>();
                    foreach (ReportalgorithmDefn item in lstRepoDefn)
                    {
                        if (keyItem.Text == item.KeyName && stgMdList.Where(c => c.key == item.DisplayName).Count() > 0)
                        {
                            delimtedValues.Add(stgMdList.Where(c => c.key == item.DisplayName).FirstOrDefault().Value);
                            delimitedKeys.Add(stgMdList.Where(c => c.key == item.DisplayName).FirstOrDefault().key);
                            //delimtedValues += "," + stgMdList.Where(c => c.key == item.DisplayName).FirstOrDefault().Value;
                            //delimitedKeys += "," + stgMdList.Where(c => c.key == item.DisplayName).FirstOrDefault().key;       
                        }
                    }

                    if (delimtedValues.Count > 0 && delimtedValues[0] != "")
                    {
                        ReportAlgorithmMDInput rmdInput = new ReportAlgorithmMDInput();
                        rmdInput.AlgorithmId = reportAlogId;
                        rmdInput.key = keyItem.Text;
                        rmdInput.Value = String.Join(",", delimtedValues);
                        rmdInput.KeyNames = String.Join(",", delimitedKeys);
                        rmdInput.CreatedByUser = User.Identity.Name;
                        var reportAlgolMdId = ServiceReportAlgorithmMD.Create(MapperReportAlgorithmMD.MapToEntity(rmdInput, entityReportAlgorithmMD));
                    }


                }














                //List<SelectListItem> lst = lstRepoDefn.ToList().GroupBy(m=> new {m.KeyName}).Select(m =>
                //    new SelectListItem
                //    {
                //        Value = m.Key.KeyName,
                //        Text = m.Key.KeyName
                //    }).ToList();

                //foreach (SelectListItem slitem in lst)
                //    {
                //        string type = slitem.Text;
                //        string formattedString = "";
                //        string formattedColumnNames = "";
                //        string KeyName = "";
                //    foreach (ReportalgorithmDefn item in lstRepoDefn)
                //        {
                //        if (item.KeyName == slitem.Text)
                //            {

                //            KeyName = item.KeyName;
                //            string keyColumn = item.Type + "_" + item.Order + "_" + item.DisplayName;
                //            formattedString += form.GetValue(keyColumn).AttemptedValue + ",";
                //            formattedColumnNames += item.DisplayName + ",";
                //            string keyColumnValue = form.GetValue(keyColumn).AttemptedValue;

                //            }
                //        }
                //    if (formattedString.Length > 0)
                //    {
                //        formattedString = (formattedString.LastIndexOf(",") == formattedString.Length-1 ? formattedString.Substring(0,formattedString.Length-1) : formattedString);
                //        formattedColumnNames = (formattedColumnNames.LastIndexOf(",") == formattedColumnNames.Length - 1 ? formattedColumnNames.Substring(0, formattedColumnNames.Length - 1) : formattedColumnNames);
                //        if (formattedString.Length > 0)
                //        {
                //        ReportAlgorithmMDInput rmdInput = new ReportAlgorithmMDInput();
                //        rmdInput.AlgorithmId = reportAlogId;
                //        rmdInput.key = KeyName;
                //        rmdInput.Value = formattedString;
                //        rmdInput.KeyNames = formattedColumnNames;
                //        rmdInput.CreatedByUser = User.Identity.Name;
                //        var reportAlgolMdId = ServiceReportAlgorithmMD.Create(MapperReportAlgorithmMD.MapToEntity(rmdInput, entityReportAlgorithmMD));

                //        }
                //    }

                //    }



                //ralogInput.key = "SelectedColumn";
                //rmdInput.key = ralogInput.key;
                //rmdInput.Value = selectedColumns;
                ////rmdInput.ReportConfigId = ReportId;
                //rmdInput.CreatedByUser = User.Identity.Name;
                //var reportAlgolMdId = ServiceReportAlgorithmMD.Create(MapperReportAlgorithmMD.MapToEntity(rmdInput,entityReportAlgorithmMD));

                //if(SelectedPartionByColumns != "")
                //    {

                //    rmdInput = new ReportAlgorithmMDInput();
                //    rmdInput.AlgorithmId = reportAlogId;
                //    ralogInput.key = "PartitionBy";
                //    rmdInput.key = ralogInput.key;
                //    rmdInput.Value = SelectedPartionByColumns;
                //    //rmdInput.ReportConfigId = ReportId;
                //    rmdInput.CreatedByUser = User.Identity.Name;
                //    reportAlgolMdId = ServiceReportAlgorithmMD.Create(MapperReportAlgorithmMD.MapToEntity(rmdInput, entityReportAlgorithmMD));

                //    }
                //if (SelectedPartionDateColumns != "")
                //    {

                //    rmdInput = new ReportAlgorithmMDInput();
                //    rmdInput.AlgorithmId = reportAlogId;
                //    ralogInput.key = "PartitionDate";
                //    rmdInput.key = ralogInput.key;
                //    rmdInput.Value = SelectedPartionDateColumns;
                //    //rmdInput.ReportConfigId = ReportId;
                //    rmdInput.CreatedByUser = User.Identity.Name;
                //    reportAlgolMdId = ServiceReportAlgorithmMD.Create(MapperReportAlgorithmMD.MapToEntity(rmdInput, entityReportAlgorithmMD));

                //    }
            }

            //HeatMapBuilderSetupInput heatMapBuilderSetupInput = new HeatMapBuilderSetupInput(clientId);
            //heatMapBuilderSetupInput.ReportId = ReportId;
            //heatMapBuilderSetupInput.HeatMapReportStatus = ReportType;
            ////heatMapBuilderSetupInput.reportFilterInput = new ReportFilterInput();
            ////heatMapBuilderSetupInput.reportFilterInput.ReportConfigId = ReportId;
            return RedirectToAction("GetHeatMapThreshold", new { clientId = clientId, ReportId = ReportId, ReportType = ReportType });
            //return View("HeatMapThresholdSetup", new { clientId = clientId, ReportId = heatMapBuilderSetupInput.ReportId, ReportType = heatMapBuilderSetupInput.HeatMapReportStatus });

        }



        [HttpPost]
        public ActionResult SaveHeatMapReport(FormCollection form, HeatMapBuilderSetupInput heatMapInput, int clientId = 0, int ReportId = 0, string ReportType = "")
        {


            //int ReportDataSetId = 0;
            int RegisterDataSetId = Convert.ToInt32(form.GetValue("DataSetId").AttemptedValue);



            //if report id = 0 and ReportType = "Add" inser new records in 
            //1.  ReportConfiguration
            //2.  ReportVariable
            //3.  ReportDataset
            if (ReportId == 0 && ReportType.ToLower() == "add")
            {
                //Insert a record in Report Varible for Variable Type ID
                ReportVariableInput reportVariableInput = new ReportVariableInput();
                reportVariableInput.VariableTypeId = heatMapInput.reportConfiguationInput.VariableTypeId; //Convert.ToInt32(form.GetValue("VartypeId").AttemptedValue);
                reportVariableInput.CreatedByUser = User.Identity.Name;
                reportVariableInput.CreatedDate = DateTime.Now;
                var reportVariableId = ServiceReportVariable.Create(MapperReportVariable.MapToEntity(reportVariableInput, entityReportVariable));

                //Report Configuration

                ReportConfiguationInput reportConfigInput = heatMapInput.reportConfiguationInput;
                reportConfigInput.ReportVariableId = reportVariableId;
                reportConfigInput.ClientId = clientId;
                reportConfigInput.ReportTypeID = 1;
                reportConfigInput.CreatedByUser = User.Identity.Name;
                reportConfigInput.CreatedDate = DateTime.Now;
                reportConfigInput.SSRSReportName = "";
                var reportConfigId = ServiceReportConfiguration.Create(MapperReportConfiguration.MapToEntity(reportConfigInput, entityReportConfiguration));
                ReportId = reportConfigId;

                //Report DataSet Insert
                RegisterDataSetId = Convert.ToInt32(form.GetValue("DataSetId").AttemptedValue);
                string[] selectedCols = form.GetValues("chkCols");
                foreach (string colName in selectedCols)
                {

                    ReportDataSetInput reportDataSetInput = new ReportDataSetInput();

                    reportDataSetInput.RegisterDataSetId = RegisterDataSetId;
                    reportDataSetInput.ReportConfigId = reportConfigId;
                    reportDataSetInput.CreatedByUser = User.Identity.Name;
                    reportDataSetInput.CreatedDate = DateTime.Now;
                    reportDataSetInput.SelectedColumn = colName;
                    var reportDataSetId = ServiceReportDataSet.Create(MapperReportDataSet.MapToEntity(reportDataSetInput, entityReportDataSet));


                }



            }


            if (ReportId > 0 && ReportType.ToLower() == "edit")
            {

                //Check Report Variable Type If it is changed by User, update ReportVariableInput
                int VarTypeId = heatMapInput.reportConfiguationInput.VariableTypeId;
                entityReportConfiguration = ServiceReportConfiguration.Get(ReportId);
                if (entityReportConfiguration != null)
                {
                    entityReportVariable = ServiceReportVariable.Get(Convert.ToInt32(entityReportConfiguration.ReportVariableId));
                    if (entityReportVariable == null)
                    {

                        ReportVariableInput reportVariableInput = new ReportVariableInput();
                        reportVariableInput.VariableTypeId = heatMapInput.reportConfiguationInput.VariableTypeId; //Convert.ToInt32(form.GetValue("VartypeId").AttemptedValue);
                        reportVariableInput.CreatedByUser = User.Identity.Name;
                        reportVariableInput.CreatedDate = DateTime.Now;
                        entityReportVariable = new ReportVariable();
                        var reportVariableId = ServiceReportVariable.Create(MapperReportVariable.MapToEntity(reportVariableInput, entityReportVariable));
                        entityReportConfiguration.ReportVariableId = reportVariableId;
                    }
                    else
                    {
                        if (entityReportVariable != null)
                        {
                            if (VarTypeId != entityReportVariable.VariableTypeId)
                            {
                                ReportVariableInput rptVaribleInput = MapperReportVariable.MapToInput(entityReportVariable);
                                rptVaribleInput.VariableTypeId = VarTypeId;
                                rptVaribleInput.LastUpdatedUser = User.Identity.Name;
                                rptVaribleInput.LastUpdatedDate = DateTime.Now;
                                MapperReportVariable.MapToEntity(rptVaribleInput, entityReportVariable);
                                ServiceReportVariable.Save();
                            }
                        }
                    }

                    entityReportConfiguration = ServiceReportConfiguration.Get(ReportId);
                    ReportConfiguationInput reportConfigInput = heatMapInput.reportConfiguationInput;
                    reportConfigInput.LastUpdatedDate = DateTime.Now;
                    reportConfigInput.LastUpdatedUser = User.Identity.Name;
                    ReportConfiguationInput existingConfig = MapperReportConfiguration.MapToInput(entityReportConfiguration);
                    existingConfig.LastUpdatedUser = User.Identity.Name;
                    existingConfig.LastUpdatedDate = DateTime.Now;
                    existingConfig.ReportName = reportConfigInput.ReportName;
                    existingConfig.VariableTypeId = VarTypeId;

                    MapperReportConfiguration.MapToEntity(existingConfig, entityReportConfiguration);
                    ServiceReportConfiguration.Save();



                }

                //Check DataSet Variable selected 
                IEnumerable<ReportDataSet> entityReportDataSets = ServiceReportDataSet.GetAll().Where(c => c.ReportConfigId == ReportId);
                if (entityReportDataSets.Count() > 0)
                {
                    entityReportDataSet = entityReportDataSets.FirstOrDefault();
                    int existingRegisterDataSetId = entityReportDataSet.RegisterDataSetId;
                    int SelectedRegisterDataSetId = Convert.ToInt32(form.GetValue("DataSetId").AttemptedValue);
                    if (SelectedRegisterDataSetId != existingRegisterDataSetId)
                    {
                        //User has selected New Dataset Id (Delete  all records from reportdataset, reportFilter) 
                        foreach (ReportDataSet item in entityReportDataSets)
                        {
                            ServiceReportDataSet.Delete(item.Id);
                        }

                        //Delete Filter Data Rows
                        IEnumerable<ReportFilter> listReportFilter = ServiceReportFilter.GetAll().Where(c => c.ReportConfigId == ReportId);
                        foreach (ReportFilter rptFilter in listReportFilter)
                        {
                            ServiceReportFilter.Delete(rptFilter.Id);
                        }

                        //Insert New records in the reportdataset
                        string[] selectedCols = form.GetValues("chkCols");
                        foreach (string colName in selectedCols)
                        {
                            ReportDataSetInput reportDataSetInput = new ReportDataSetInput();
                            reportDataSetInput.RegisterDataSetId = SelectedRegisterDataSetId;
                            reportDataSetInput.ReportConfigId = ReportId;
                            reportDataSetInput.CreatedByUser = User.Identity.Name;
                            reportDataSetInput.CreatedDate = DateTime.Now;
                            reportDataSetInput.SelectedColumn = colName;
                            var reportDataSetId = ServiceReportDataSet.Create(MapperReportDataSet.MapToEntity(reportDataSetInput, entityReportDataSet));
                        }

                    }
                    else
                    {
                        //Same Dataset, if user has unchecked the previous columns and selected New Columns
                        //Then delete unselected columns and insert new columns 
                        string[] selectedCols = form.GetValues("chkCols");
                        foreach (ReportDataSet item in entityReportDataSets)
                        {
                            string isColExists = selectedCols.Where(c => c.ToLower() == item.SelectedColumn.ToLower()).FirstOrDefault();
                            if (isColExists == null)
                            {

                                //Delete Corresponding records in ReportFilter data table
                                IEnumerable<ReportFilter> rptFilterDataSet = ServiceReportFilter.GetAll().Where(c => c.ReportConfigId == ReportId && c.FilterColumn.ToLower() == item.SelectedColumn.ToLower());
                                foreach (ReportFilter fiteritem in rptFilterDataSet)
                                {
                                    ServiceReportFilter.Delete(fiteritem.Id);
                                }
                                ServiceReportDataSet.Delete(item.Id);

                            }

                        }



                        foreach (string colName in selectedCols)
                        {
                            ReportDataSet rptItem = entityReportDataSets.Where(c => c.SelectedColumn.ToLower() == colName.ToLower()).FirstOrDefault();
                            if (rptItem == null)
                            {
                                //Insert New Record in ReportData Set
                                ReportDataSetInput reportDataSetInput = new ReportDataSetInput();
                                entityReportDataSet = new ReportDataSet();
                                reportDataSetInput.RegisterDataSetId = RegisterDataSetId;
                                reportDataSetInput.ReportConfigId = ReportId;
                                reportDataSetInput.CreatedByUser = User.Identity.Name;
                                reportDataSetInput.CreatedDate = DateTime.Now;
                                reportDataSetInput.SelectedColumn = colName;
                                var reportDataSetId = ServiceReportDataSet.Create(MapperReportDataSet.MapToEntity(reportDataSetInput, entityReportDataSet));

                            }

                        }








                    }


                }




            }






            //HeatMapBuilderSetupInput heatMapBuilderSetupInput = new HeatMapBuilderSetupInput(clientId);
            //heatMapBuilderSetupInput.ReportId = ReportId;
            //heatMapBuilderSetupInput.HeatMapReportStatus = ReportType;
            //heatMapBuilderSetupInput.reportFilterInput = new ReportFilterInput();
            //heatMapBuilderSetupInput.reportFilterInput.ReportConfigId = ReportId;
            return RedirectToAction("GetHeatMapFilters", new { clientId = clientId, ReportId = ReportId, ReportType = ReportType });
            //return View("HeatMapFilterSetUp", heatMapBuilderSetupInput);
        }



        public ActionResult GetHeatMapFilters(int clientId = 186, int ReportId = 27, string ReportType = "Add")
        {

            HeatMapBuilderSetupInput heatMapBuilderSetupInput = new HeatMapBuilderSetupInput(clientId);
            heatMapBuilderSetupInput.ReportId = ReportId;
            heatMapBuilderSetupInput.HeatMapReportStatus = ReportType;
            heatMapBuilderSetupInput.reportFilterInput = new ReportFilterInput();
            heatMapBuilderSetupInput.reportFilterInput.ReportConfigId = ReportId;

            //HeatMapBuilderSetupInput heatMapBuilderSetupInput = new HeatMapBuilderSetupInput(clientId);
            //heatMapBuilderSetupInput.ReportId = ReportId;
            //heatMapBuilderSetupInput.HeatMapReportStatus = ReportType;
            //heatMapBuilderSetupInput.reportFilterInput = new ReportFilterInput();

            return View("HeatMapFilterSetUp", heatMapBuilderSetupInput);
        }





        public ActionResult GetHeatMapThreshold(int clientId = 186, int ReportId = 27, string ReportType = "Add")
        {

            HeatMapBuilderSetupInput heatMapBuilderSetupInput = new HeatMapBuilderSetupInput(clientId);
            heatMapBuilderSetupInput.ReportId = ReportId;
            heatMapBuilderSetupInput.HeatMapReportStatus = ReportType;
            heatMapBuilderSetupInput.reportThresholdInput = new ReportThresholdInput();
            heatMapBuilderSetupInput.reportThresholdInput.ReportConfigId = ReportId;

            if (TempData["Success"] != null)
            {
                ModelState.AddModelError("Success", "Data has been saved successfully");
            }



            return View("HeatMapThresholdSetup", heatMapBuilderSetupInput);
        }


        public ActionResult GetDataForSelectedColumnForFilter(int clientId = 186, int ReportId = 27, int SelectedColumnId = 0, string ReportType = "Add")
        {

            HeatMapBuilderSetupInput heatMapBuilderSetupInput = new HeatMapBuilderSetupInput(clientId);
            heatMapBuilderSetupInput.ReportId = ReportId;
            heatMapBuilderSetupInput.HeatMapReportStatus = ReportType;
            heatMapBuilderSetupInput.reportFilterInput = new ReportFilterInput();
            heatMapBuilderSetupInput.reportFilterInput.SelectedColId = SelectedColumnId;
            heatMapBuilderSetupInput.reportFilterInput.ReportConfigId = ReportId;
            return View("HeatMapFilterSetUp", heatMapBuilderSetupInput);
        }

        [HttpPost]
        public ActionResult SaveHeatMapFilers(FormCollection form, HeatMapBuilderSetupInput input, int clientId = 186, int ReportId = 27, int SelectedColumnId = 0, string ReportType = "Add")
        {


            string selectedColumn = "";

            //Check for filter column selection
            string[] keys = form.AllKeys;
            if (keys.Contains("radiochk"))
            {
                selectedColumn = form.GetValue("radiochk").AttemptedValue;
            }



            if (selectedColumn != "")
            {

                //Delete Existing Data Filters From ReportFilter Table before inserting the records for the selected Filter

                ReportDataSet rdSet = ServiceReportDataSet.Get(Convert.ToInt32(selectedColumn));
                string SelectedColumnName = rdSet.SelectedColumn;

                IList<ReportFilter> rptFilterData = ServiceReportFilter.GetAll().Where(c => c.ReportConfigId == ReportId && c.FilterColumn.ToLower() == SelectedColumnName.ToLower()).ToList();
                foreach (ReportFilter item in rptFilterData)
                {
                    ServiceReportFilter.Delete(item.Id);
                }





                string[] selectedColumnValues = form.GetValues("chkSelectedData");
                if (selectedColumnValues != null)
                {
                    foreach (string colValue in selectedColumnValues)
                    {
                        //Insert a record in ReportFilter Table
                        ReportFilterInput rptFilterInput = new ReportFilterInput();
                        rptFilterInput.ReportConfigId = ReportId;
                        rptFilterInput.FilterColumn = SelectedColumnName;
                        rptFilterInput.FilterValue = colValue;
                        //need to be analyzed
                        //rptFilterInput.FilterId = 0;

                        var reportFilterId = ServiceReportFilter.Create(MapperReportFilter.MapToEntity(rptFilterInput, entityReportFilter));


                    }
                }
            }

            HeatMapBuilderSetupInput heatMapBuilderSetupInput = new HeatMapBuilderSetupInput(clientId);
            heatMapBuilderSetupInput.ReportId = ReportId;
            heatMapBuilderSetupInput.HeatMapReportStatus = ReportType;
            heatMapBuilderSetupInput.reportFilterInput = new ReportFilterInput();
            heatMapBuilderSetupInput.reportFilterInput.ReportConfigId = ReportId;

            string btnClickVal = form.GetValue("btnClick").AttemptedValue;

            if (btnClickVal == "AddFilter")
            {
                if (selectedColumn != "")
                    ModelState.AddModelError("Success", "Filter Data has been saved successfully");
                return View("HeatMapFilterSetUp", heatMapBuilderSetupInput);
            }
            else
            {
                return RedirectToAction("GetHeatMapAlgorithmSetup", new { clientId = clientId, ReportId = ReportId, ReportType = ReportType });

            }

        }

        public ActionResult GetHeatMapAlgorithmSetup(int clientId = 186, int ReportId = 27, string ReportType = "Add", string algorId = "")
        {

            HeatMapBuilderSetupInput heatMapBuilderSetupInput = new HeatMapBuilderSetupInput(clientId);
            heatMapBuilderSetupInput.ReportId = ReportId;
            heatMapBuilderSetupInput.HeatMapReportStatus = ReportType;
            heatMapBuilderSetupInput.reportAlgorithmInput = new ReportAlgorithmInput();
            heatMapBuilderSetupInput.reportAlgorithmInput.NewAlgorithmSolution = algorId;
            if (ReportId > 0)
            {
                entityReportAlgorithm = ServiceReportAlgorithm.GetAll().Where(c => c.ReportConfigId == ReportId).FirstOrDefault();
                if (entityReportAlgorithm != null)
                {
                    heatMapBuilderSetupInput.reportAlgorithmInput = MapperReportAlgorithm.MapToInput(entityReportAlgorithm);
                    entityReportAlgorithmMD = ServiceReportAlgorithmMD.GetAll().Where(c => c.AlgorithmId == entityReportAlgorithm.Id).FirstOrDefault();
                    heatMapBuilderSetupInput.reportAlgorithmMDInput = MapperReportAlgorithmMD.MapToInput(entityReportAlgorithmMD);
                    if (algorId == "")
                    {
                        heatMapBuilderSetupInput.reportAlgorithmInput.NewAlgorithmSolution = heatMapBuilderSetupInput.reportAlgorithmInput.AlgorithmSolution;
                    }
                    else
                    {
                        heatMapBuilderSetupInput.reportAlgorithmInput.NewAlgorithmSolution = algorId;
                    }
                }

                IEnumerable<ReportConfigurationMD> entityConfigList = ServiceReportConfigurationMD.GetAll().Where(c => c.ReportConfigId == ReportId);
                if (entityConfigList.Count() > 0)
                {
                    heatMapBuilderSetupInput.reportAlgorithmInput.reportConfigurationMDInput = MapperReportConfigurationMD.MapToInput(entityConfigList.First());
                }
                foreach (ReportConfigurationMD item in entityConfigList)
                {
                    switch (item.Key)
                    {
                        case "XAxis1":
                            heatMapBuilderSetupInput.reportAlgorithmInput.reportConfigurationMDInput.Xaxis1 = item.Value;
                            break;
                        case "XAxis2":
                            heatMapBuilderSetupInput.reportAlgorithmInput.reportConfigurationMDInput.Xaxis2 = item.Value;
                            break;
                        case "YAxis1":
                            heatMapBuilderSetupInput.reportAlgorithmInput.reportConfigurationMDInput.Yaxis1 = item.Value;
                            break;
                        case "YAxis2":
                            heatMapBuilderSetupInput.reportAlgorithmInput.reportConfigurationMDInput.Yaxis2 = item.Value;
                            break;
                    }

                }


                entityReportGraphConfiguration = ServiceReportGraphConfiguration.GetAll().Where(c => c.ReportConfigId == ReportId).FirstOrDefault();
                if (entityReportGraphConfiguration != null)
                {
                    heatMapBuilderSetupInput.reportAlgorithmInput.reportGraphConfigurationInput = MapperReportGraphConfiguration.MapToInput(entityReportGraphConfiguration);
                }

                //heatMapBuilderSetupInput.reportAlgorithmMDInput = MapperReportAlgorithmMD.MapToInput(entityReportAlgorithmMD);
                //heatMapBuilderSetupInput.reportAlgorithmInput.UsedColumnOne = heatMapBuilderSetupInput.reportAlgorithmMDInput.UsedColumnOne;
                //heatMapBuilderSetupInput.reportAlgorithmInput.UsedColumnTwo = heatMapBuilderSetupInput.reportAlgorithmMDInput.UsedColumnTwo;
            }

            return View("HeatMapAlgorithmSetUp", heatMapBuilderSetupInput);
        }



    }

}
