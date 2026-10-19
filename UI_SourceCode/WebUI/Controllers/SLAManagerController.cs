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

namespace NTTData.AMOPortal.WebUI.Controllers
{
    public class SLAManagerController : Cruder<Metric, SLAMetricInput>
    {
        //Entities
        private Metric entityMetric;
        private ChartIndicator entityChart;
        private ObjectiveType entityObjT;
        private ContextualElement entityContext;
        private Calculation entityCalc;
        private CalculationDataElement entityCalcData;
        private Rules entityRules;
        private PrimaryAudience entityPYAud;
        public MetricUnit entityMetUnit;

        //Service
        private CrudService<Metric> Metricservice;
        private CrudService<ChartIndicator> Chartservice;
        private CrudService<ObjectiveType> ObjTypeService;
        private CrudService<ContextualElement> ContextService;
        private CrudService<Calculation> CalcService;
        private CrudService<CalculationDataElement> CalcdataService;
        private CrudService<Rules> RuleService;
        private CrudService<PrimaryAudience> PYAudService;
        private CrudService<MetricUnit> MetUnitService;

        //Mapper
        private Mapper<Metric, SLAMetricInput> MetriccreateMapper;
        private Mapper<ChartIndicator, SLAChartIndInput> ChartcreateMapper;
        private Mapper<ObjectiveType, SLAObjTypeInput> ObjTcreateMapper;
        private Mapper<ContextualElement, SLAContextInput> ContextcreateMapper;
        private Mapper<Calculation, SLACalcInput> CalccreateMapper;
        private Mapper<CalculationDataElement, SLACalcDataInput> CalcDatacreateMapper;
        private Mapper<Rules, SLARulesInput> RulescreateMapper;
        private Mapper<PrimaryAudience, SLAPYAudInput> PYAudcreateMapper;
        private Mapper<MetricUnit, SLAMetricUnit> MetUnitMapper;

        //Repo
        private Repo<ChartIndicator> repoChart;
        private Repo<Metric> repoMetric;
        private Repo<ObjectiveType> repoObjT;
        private Repo<ContextualElement> repoContext;
        private Repo<Calculation> repoCalc;
        private Repo<CalculationDataElement> repoCalcData;
        private Repo<Rules> repoRules;
        private Repo<PrimaryAudience> repoPYAud;
        private Repo<MetricUnit> repoMetUnit;


        public SLAManagerController(ICrudService<Metric> service, IMapper<Metric, SLAMetricInput> v)
            : base(service, v)
        {

        }

        public ActionResult MetricLibraryView()
        {
            SLAMetricInput input = new SLAMetricInput();
            return View("Create", input);
        }

        [HttpPost]
        public ActionResult MetricLibraryView(SLAMetricInput input)
        {

            DbContextFactory Dbcon = new DbContextFactory();
            repoMetric = new Repo<Metric>(Dbcon);
            repoChart = new Repo<ChartIndicator>(Dbcon);
            repoObjT = new Repo<ObjectiveType>(Dbcon);
            repoContext = new Repo<ContextualElement>(Dbcon);
            repoCalc = new Repo<Calculation>(Dbcon);
            repoCalcData = new Repo<CalculationDataElement>(Dbcon);
            repoRules = new Repo<Rules>(Dbcon);
            repoPYAud = new Repo<PrimaryAudience>(Dbcon);
            repoMetUnit = new Repo<MetricUnit>(Dbcon);

            MetriccreateMapper = new Mapper<Metric, SLAMetricInput>();
            ChartcreateMapper = new Mapper<ChartIndicator, SLAChartIndInput>();
            ObjTcreateMapper = new Mapper<ObjectiveType, SLAObjTypeInput>();
            ContextcreateMapper = new Mapper<ContextualElement, SLAContextInput>();
            CalccreateMapper = new Mapper<Calculation, SLACalcInput>();
            CalcDatacreateMapper = new Mapper<CalculationDataElement, SLACalcDataInput>();
            RulescreateMapper = new Mapper<Rules, SLARulesInput>();
            PYAudcreateMapper = new Mapper<PrimaryAudience, SLAPYAudInput>();
            MetUnitMapper = new Mapper<MetricUnit, SLAMetricUnit>();

            entityMetric = new Metric();
            entityChart = new ChartIndicator();
            entityObjT = new ObjectiveType();
            entityContext = new ContextualElement();
            entityCalc = new Calculation();
            entityCalcData = new CalculationDataElement();
            entityRules = new Rules();
            entityPYAud = new PrimaryAudience();
            entityMetUnit = new MetricUnit();

            Metricservice = new CrudService<Metric>(repoMetric);
            Chartservice = new CrudService<ChartIndicator>(repoChart);
            ObjTypeService = new CrudService<ObjectiveType>(repoObjT);
            CalcService = new CrudService<Calculation>(repoCalc);
            ContextService = new CrudService<ContextualElement>(repoContext);
            CalcdataService = new CrudService<CalculationDataElement>(repoCalcData);
            RuleService = new CrudService<Rules>(repoRules);
            PYAudService = new CrudService<PrimaryAudience>(repoPYAud);
            MetUnitService = new CrudService<MetricUnit>(repoMetUnit);

            if (!ModelState.IsValid)
                return View(input);

            var chartId = Chartservice.Create(ChartcreateMapper.MapToEntity(input.ChartIndicatorIds, entityChart));
            input.ChartIndicatorId = chartId;

            var objTId = ObjTypeService.Create(ObjTcreateMapper.MapToEntity(input.ObjectiveTypeIds, entityObjT));
            input.ObjectiveTypeId = objTId;

            var calcId = CalcService.Create(CalccreateMapper.MapToEntity(input.CalculationIds, entityCalc));
            input.CalculationId = calcId;

            var contextId = ContextService.Create(ContextcreateMapper.MapToEntity(input.ContextIds, entityContext));
            input.ContextId = contextId;

            var calcdataId = CalcdataService.Create(CalcDatacreateMapper.MapToEntity(input.CalculationDataIds, entityCalcData));
            input.CalculationDataId = calcdataId;

            var ruleId = RuleService.Create(RulescreateMapper.MapToEntity(input.RulesIds, entityRules));
            input.RulesId = ruleId;

            var pyaudId = PYAudService.Create(PYAudcreateMapper.MapToEntity(input.PrimaryAudienceIds, entityPYAud));
            input.PrimaryAudienceId = pyaudId;

            var metunit = MetUnitService.Create(MetUnitMapper.MapToEntity(input.MetricUnitIds, entityMetUnit));
            input.MetricUnitId = metunit;

            input.CreatedBy = User.Identity.Name;
            input.UpdatedBy = User.Identity.Name;

            var metricId = Metricservice.Create(MetriccreateMapper.MapToEntity(input, entityMetric));
            var e = Metricservice.Get(pyaudId);

            ModelState.AddModelError("CreateMsg", "SLA Metric Created Successfully.");
            return View("Create", input);

        }

        public override ActionResult Edit(int id)
        {
            DbContextFactory Dbcon = new DbContextFactory();
            repoMetric = new Repo<Metric>(Dbcon);
            repoChart = new Repo<ChartIndicator>(Dbcon);
            repoObjT = new Repo<ObjectiveType>(Dbcon);
            repoContext = new Repo<ContextualElement>(Dbcon);
            repoCalc = new Repo<Calculation>(Dbcon);
            repoCalcData = new Repo<CalculationDataElement>(Dbcon);
            repoRules = new Repo<Rules>(Dbcon);
            repoPYAud = new Repo<PrimaryAudience>(Dbcon);
            repoMetUnit = new Repo<MetricUnit>(Dbcon);

            MetriccreateMapper = new Mapper<Metric, SLAMetricInput>();
            ChartcreateMapper = new Mapper<ChartIndicator, SLAChartIndInput>();
            ObjTcreateMapper = new Mapper<ObjectiveType, SLAObjTypeInput>();
            ContextcreateMapper = new Mapper<ContextualElement, SLAContextInput>();
            CalccreateMapper = new Mapper<Calculation, SLACalcInput>();
            CalcDatacreateMapper = new Mapper<CalculationDataElement, SLACalcDataInput>();
            RulescreateMapper = new Mapper<Rules, SLARulesInput>();
            PYAudcreateMapper = new Mapper<PrimaryAudience, SLAPYAudInput>();
            MetUnitMapper = new Mapper<MetricUnit, SLAMetricUnit>();

            entityMetric = new Metric();
            entityChart = new ChartIndicator();
            entityObjT = new ObjectiveType();
            entityContext = new ContextualElement();
            entityCalc = new Calculation();
            entityCalcData = new CalculationDataElement();
            entityRules = new Rules();
            entityPYAud = new PrimaryAudience();
            entityMetUnit = new MetricUnit();

            Metricservice = new CrudService<Metric>(repoMetric);
            Chartservice = new CrudService<ChartIndicator>(repoChart);
            ObjTypeService = new CrudService<ObjectiveType>(repoObjT);
            CalcService = new CrudService<Calculation>(repoCalc);
            ContextService = new CrudService<ContextualElement>(repoContext);
            CalcdataService = new CrudService<CalculationDataElement>(repoCalcData);
            RuleService = new CrudService<Rules>(repoRules);
            PYAudService = new CrudService<PrimaryAudience>(repoPYAud);
            MetUnitService = new CrudService<MetricUnit>(repoMetUnit);

            var eMetric = MetriccreateMapper.MapToInput(Metricservice.Get(id));
            eMetric.ChartIndicatorIds = ChartcreateMapper.MapToInput(Chartservice.Get(eMetric.ChartIndicatorId));
            eMetric.ObjectiveTypeIds = ObjTcreateMapper.MapToInput(ObjTypeService.Get(eMetric.ObjectiveTypeId));
            eMetric.CalculationIds = CalccreateMapper.MapToInput(CalcService.Get(eMetric.CalculationId));
            eMetric.CalculationDataIds = CalcDatacreateMapper.MapToInput(CalcdataService.Get(eMetric.CalculationDataId));
            eMetric.RulesIds = RulescreateMapper.MapToInput(RuleService.Get(eMetric.RulesId));
            eMetric.ContextIds = ContextcreateMapper.MapToInput(ContextService.Get(eMetric.ContextId));
            eMetric.PrimaryAudienceIds = PYAudcreateMapper.MapToInput(PYAudService.Get(eMetric.PrimaryAudienceId));
            eMetric.MetricUnitIds = MetUnitMapper.MapToInput(MetUnitService.Get(eMetric.MetricUnitId));

            return View("Edit", eMetric);
        }

        [HttpPost]
        public override ActionResult Edit(SLAMetricInput input)
        {
            DbContextFactory Dbcon = new DbContextFactory();
            repoMetric = new Repo<Metric>(Dbcon);
            repoChart = new Repo<ChartIndicator>(Dbcon);
            repoObjT = new Repo<ObjectiveType>(Dbcon);
            repoContext = new Repo<ContextualElement>(Dbcon);
            repoCalc = new Repo<Calculation>(Dbcon);
            repoCalcData = new Repo<CalculationDataElement>(Dbcon);
            repoRules = new Repo<Rules>(Dbcon);
            repoPYAud = new Repo<PrimaryAudience>(Dbcon);
            repoMetUnit = new Repo<MetricUnit>(Dbcon);

            MetriccreateMapper = new Mapper<Metric, SLAMetricInput>();
            ChartcreateMapper = new Mapper<ChartIndicator, SLAChartIndInput>();
            ObjTcreateMapper = new Mapper<ObjectiveType, SLAObjTypeInput>();
            ContextcreateMapper = new Mapper<ContextualElement, SLAContextInput>();
            CalccreateMapper = new Mapper<Calculation, SLACalcInput>();
            CalcDatacreateMapper = new Mapper<CalculationDataElement, SLACalcDataInput>();
            RulescreateMapper = new Mapper<Rules, SLARulesInput>();
            PYAudcreateMapper = new Mapper<PrimaryAudience, SLAPYAudInput>();
            MetUnitMapper = new Mapper<MetricUnit, SLAMetricUnit>();

            entityMetric = new Metric();
            entityChart = new ChartIndicator();
            entityObjT = new ObjectiveType();
            entityContext = new ContextualElement();
            entityCalc = new Calculation();
            entityCalcData = new CalculationDataElement();
            entityRules = new Rules();
            entityPYAud = new PrimaryAudience();
            entityMetUnit = new MetricUnit();

            Metricservice = new CrudService<Metric>(repoMetric);
            Chartservice = new CrudService<ChartIndicator>(repoChart);
            ObjTypeService = new CrudService<ObjectiveType>(repoObjT);
            CalcService = new CrudService<Calculation>(repoCalc);
            ContextService = new CrudService<ContextualElement>(repoContext);
            CalcdataService = new CrudService<CalculationDataElement>(repoCalcData);
            RuleService = new CrudService<Rules>(repoRules);
            PYAudService = new CrudService<PrimaryAudience>(repoPYAud);
            MetUnitService = new CrudService<MetricUnit>(repoMetUnit);

            if (!ModelState.IsValid)
                return View(input);

            var e = Metricservice.Get(input.Id);

            input.ChartIndicatorId = e.ChartIndicatorId;
            input.ChartIndicatorIds.Id = e.ChartIndicatorId;
            ChartcreateMapper.MapToEntity(input.ChartIndicatorIds, Chartservice.Get(e.ChartIndicatorId));
            Chartservice.Save();

            input.ObjectiveTypeId = e.ObjectiveTypeId;
            input.ObjectiveTypeIds.Id = e.ObjectiveTypeId;
            ObjTcreateMapper.MapToEntity(input.ObjectiveTypeIds, ObjTypeService.Get(e.ObjectiveTypeId));
            ObjTypeService.Save();

            input.CalculationId = e.CalculationId;
            input.CalculationIds.Id = e.CalculationId;
            CalccreateMapper.MapToEntity(input.CalculationIds, CalcService.Get(e.CalculationId));
            CalcService.Save();

            input.ContextId = e.ContextId;
            input.ContextIds.Id = e.ContextId;
            ContextcreateMapper.MapToEntity(input.ContextIds, ContextService.Get(e.ContextId));
            ContextService.Save();

            input.CalculationDataId = e.CalculationDataId;
            input.CalculationDataIds.Id = e.CalculationDataId;
            CalcDatacreateMapper.MapToEntity(input.CalculationDataIds, CalcdataService.Get(e.CalculationDataId));
            CalcdataService.Save();

            input.RulesId = e.RulesId;
            input.RulesIds.Id = e.RulesId;
            RulescreateMapper.MapToEntity(input.RulesIds, RuleService.Get(e.RulesId));
            RuleService.Save();

            input.PrimaryAudienceId = e.PrimaryAudienceId;
            input.PrimaryAudienceIds.Id = e.PrimaryAudienceId;
            PYAudcreateMapper.MapToEntity(input.PrimaryAudienceIds, PYAudService.Get(e.PrimaryAudienceId));
            PYAudService.Save();

            input.MetricUnitId = e.MetricUnitId;
            input.MetricUnitIds.Id = e.MetricUnitId;
            MetUnitMapper.MapToEntity(input.MetricUnitIds, MetUnitService.Get(e.MetricUnitId));
            MetUnitService.Save();

            input.CreatedBy = e.CreatedBy;
            input.UpdatedBy = User.Identity.Name;
            MetriccreateMapper.MapToEntity(input, Metricservice.Get(e.Id));
            Metricservice.Save();

            ModelState.AddModelError("EditMsg", "SLA Metric Edited Successfully.");

            return View(input);
        }

        public ActionResult DelMetric(int id)
        {
            DbContextFactory Dbcon = new DbContextFactory();
            repoMetric = new Repo<Metric>(Dbcon);
            repoChart = new Repo<ChartIndicator>(Dbcon);
            repoObjT = new Repo<ObjectiveType>(Dbcon);
            repoContext = new Repo<ContextualElement>(Dbcon);
            repoCalc = new Repo<Calculation>(Dbcon);
            repoCalcData = new Repo<CalculationDataElement>(Dbcon);
            repoRules = new Repo<Rules>(Dbcon);
            repoPYAud = new Repo<PrimaryAudience>(Dbcon);
            repoMetUnit = new Repo<MetricUnit>(Dbcon);

            MetriccreateMapper = new Mapper<Metric, SLAMetricInput>();
            ChartcreateMapper = new Mapper<ChartIndicator, SLAChartIndInput>();
            ObjTcreateMapper = new Mapper<ObjectiveType, SLAObjTypeInput>();
            ContextcreateMapper = new Mapper<ContextualElement, SLAContextInput>();
            CalccreateMapper = new Mapper<Calculation, SLACalcInput>();
            CalcDatacreateMapper = new Mapper<CalculationDataElement, SLACalcDataInput>();
            RulescreateMapper = new Mapper<Rules, SLARulesInput>();
            PYAudcreateMapper = new Mapper<PrimaryAudience, SLAPYAudInput>();
            MetUnitMapper = new Mapper<MetricUnit, SLAMetricUnit>();

            entityMetric = new Metric();
            entityChart = new ChartIndicator();
            entityObjT = new ObjectiveType();
            entityContext = new ContextualElement();
            entityCalc = new Calculation();
            entityCalcData = new CalculationDataElement();
            entityRules = new Rules();
            entityPYAud = new PrimaryAudience();
            entityMetUnit = new MetricUnit();

            Metricservice = new CrudService<Metric>(repoMetric);
            Chartservice = new CrudService<ChartIndicator>(repoChart);
            ObjTypeService = new CrudService<ObjectiveType>(repoObjT);
            CalcService = new CrudService<Calculation>(repoCalc);
            ContextService = new CrudService<ContextualElement>(repoContext);
            CalcdataService = new CrudService<CalculationDataElement>(repoCalcData);
            RuleService = new CrudService<Rules>(repoRules);
            PYAudService = new CrudService<PrimaryAudience>(repoPYAud);
            MetUnitService = new CrudService<MetricUnit>(repoMetUnit);

            var e = Metricservice.Get(id);

            //Repo<MetricActual> lMetAct = new Data.Repo<NTTData.AMOPortal.Domain.Entities.MetricActual>(Dbcon);
            //List<MetricActual> lMetricActual = lMetAct.GetAll().ToList();
            //var qMetA = from p in lMetricActual
            //            where p.MetricId == e.Id
            //            select p;
            //Repo<MetricSpecification> lMetSpec = new Data.Repo<NTTData.AMOPortal.Domain.Entities.MetricSpecification>(Dbcon);
            //List<MetricSpecification> lMetricSpec = lMetSpec.GetAll().ToList();
            //var qMetS = from p in lMetricSpec
            //            where p.MetricId == e.Id
            //            select p;
            SLAMetricInput input = new SLAMetricInput();
            //if (qMetA.Count().Equals(0) && qMetS.Count().Equals(0))
            //{
            try
            {
                Metricservice.Delete(id);
                Chartservice.Delete(e.ChartIndicatorId);
                ObjTypeService.Delete(e.ObjectiveTypeId);
                CalcService.Delete(e.CalculationId);
                CalcdataService.Delete(e.CalculationDataId);
                ContextService.Delete(e.ContextId);
                RuleService.Delete(e.RulesId);
                PYAudService.Delete(e.PrimaryAudienceId);
                MetUnitService.Delete(e.MetricUnitId);

                ModelState.AddModelError("DelMsg", "SLA Metric Deleted Successfully.");
                //}
                //else
                //{
                //    ModelState.AddModelError("DelMsg", "Metric cannot be deleted due to associated SetUp in SLA SetUp or Metric Actual");
                //}
            }
            catch
            {
                ModelState.AddModelError("DelMsg", "'" + e.MetricName + "' is mapped to a Client and cannot be Deleted");
            }
            return View("Create", input);
        }

        [HttpGet]
        public JsonResult BindService(int sercatId)
        {
            DbContextFactory Dbcon = new DbContextFactory();
            Db db = new Db();
            var SerNames = from r in db.Services
                           where r.ServiceCategoryId == sercatId
                           select new
                           {
                               SerId = r.Id,
                               SerName = r.Name
                           };
            return Json(SerNames, JsonRequestBehavior.AllowGet);
        }


    }
}