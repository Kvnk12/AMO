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
using AutoMapper;
using WebUI.Dto;

namespace NTTData.AMOPortal.WebUI.Controllers
{
    public class MaintenanceRequestController : Cruder<MaintenanceRequest, MaintenanceRequestInput>
    {
        public MaintenanceRequestController(ICrudService<MaintenanceRequest> service, IMapper<MaintenanceRequest, MaintenanceRequestInput> v)
            : base(service, v)
        {
        }

        public ActionResult LoadDataSet(MaintenanceRequestInput input)
        {
            return View(input);
        }

        public override ActionResult Index()
        {
            MaintenanceRequestInput input = new MaintenanceRequestInput();
            //List<MaintenanceRequest> Ilist = service.GetAll().Take(50).ToList();
            //input.MaintenanceRequestLists = Ilist;

            return View(input);
        }
        public ActionResult DelMaintenanceRequest(int id)
        {
            List<MaintenanceRequest> AllList = service.GetAll().ToList();
            var MaintenanceRequest = AllList.Where(a => a.Id == id).FirstOrDefault();
            int clientId = MaintenanceRequest.ClientId;
            service.Delete(id);

            MaintenanceRequestInput mrinput = new MaintenanceRequestInput();
            mrinput.ClientId = clientId;
            ModelState.AddModelError("CreateMsg", "Maintenance Request record deleted Successfully");
            ViewBag.pagedItems = mrinput.GetPagedList(1);
            return View("ViewMaintenanceRequestbyClient", mrinput);
        }

        //[HttpPost]
        //public ActionResult ViewMaintenanceRequestbyClient(int clientid)
        //{
        //    MaintenanceRequestInput input = new MaintenanceRequestInput();
        //    input.ClientId = clientid;
        //    return View(input);
        //}

        public ActionResult ViewMaintenanceRequest(int ClientId, int PageNumber=1)
        {
            MaintenanceRequestInput input = new MaintenanceRequestInput();
            input.ClientId = ClientId;
            //int? clientid = input.ClientId;
            //List<MaintenanceRequest> Ilist = new List<MaintenanceRequest>();
            //if (input.ClientId != null)
            //{
            //    List<MaintenanceRequest> AllList = service.GetAll().ToList();
            //    Ilist = AllList.Where(a => a.ClientId == input.ClientId).ToList();
            //}
            //else
            //{
            //    Ilist = service.GetAll().Take(50).ToList();

            //}
            //input.MaintenanceRequestLists = Ilist;
            //input.ClientId = clientid;

            ViewBag.pagedItems = input.GetPagedList(PageNumber);
            return View("ViewMaintenanceRequestbyClient", input);
        }


        public ActionResult ViewMaintenanceRequestbyClient()
        {
            MaintenanceRequestInput input = new MaintenanceRequestInput();
            ModelState.AddModelError("CreateMsg", "Please select a Client");
            //int? clientid = input.ClientId;
            //List<MaintenanceRequest> Ilist = new List<MaintenanceRequest>();
            //if (input.ClientId != null)
            //{
            //    List<MaintenanceRequest> AllList = service.GetAll().ToList();
            //    Ilist = AllList.Where(a => a.ClientId == input.ClientId).ToList();
            //}
            //else
            //{
            //    Ilist = service.GetAll().Take(50).ToList();

            //}
            //input.MaintenanceRequestLists = Ilist;
            //input.ClientId = clientid;
            return View("ViewMaintenanceRequestbyClient", input);
        }

        public ActionResult SLADashboard(int ClientId = 0)
        {

            var input = new MaintenanceRequestInput();

            input.ClientId = ClientId;
            input.SelectedYear = new string[] { };
            input.SelectedMonth = new string[] { };
            input.SelectedPriority = new string[] { };



            return View(input);
        }


        public override ActionResult Edit(int id)
        {
            MaintenanceRequest MaintenanceRequest = new MaintenanceRequest();
            MaintenanceRequest = service.Get(id);

            MaintenanceRequestInput MaintenanceRequestInput = new MaintenanceRequestInput();
            MaintenanceRequestInput = AutoMapper.Mapper.Map<MaintenanceRequest, MaintenanceRequestInput>(MaintenanceRequest);
            return View("Edit", MaintenanceRequestInput);
        }

        [HttpPost]
        public override ActionResult Edit(MaintenanceRequestInput input)
        {
            if (!ModelState.IsValid)
                return View("Edit", input);
            //var e = editMapper.MapToEntity(input, service.Get(input.Id));
            var e = AutoMapper.Mapper.Map<MaintenanceRequestInput, MaintenanceRequest>(input, service.Get(input.Id));
            service.Save();
            ModelState.AddModelError("CreateMsg", "MaintenanceRequest Edited successfully");
            return View(input);

        }

        public ActionResult CreateMaintenanceRequest(int id)
        {
            MaintenanceRequestInput MaintenanceRequestInput = new MaintenanceRequestInput();
            MaintenanceRequestInput.ClientId = id;
            return View(MaintenanceRequestInput);
        }

        [HttpPost]
        public ActionResult CreateMaintenanceRequest(MaintenanceRequestInput input)
        {
            if (!ModelState.IsValid)
                return View("CreateMaintenanceRequest", input);
            MaintenanceRequest MaintenanceRequest = new MaintenanceRequest();
            //var id = service.Create(createMapper.MapToEntity(input, MaintenanceRequest));
            var id = service.Create(AutoMapper.Mapper.Map<MaintenanceRequestInput, MaintenanceRequest>(input));

            var e = service.Get(id);
            ModelState.AddModelError("CreateMsg", "MaintenanceRequest is created successfully");

            return View(input);

        }


        [HttpPost]
        public ActionResult SLADashboard(MaintenanceRequestInput input)
        {
            var slaName = input.SlALable;
            List<MaintenanceRequest> AllList = service.GetAll().ToList();
            var metric = input.MetricsSpecList.Where(a => a.ClientId == input.ClientId && a.MetricName == "Application Downtime Summary").FirstOrDefault();
            if (metric != null)
            {
                //input.TargetValue = metric.MetricTarget.ToString();



                input.TargetValue = string.Format("{0}:{1}:{2}", TimeSpan.FromMinutes(metric.MetricTarget).Days * 24 + TimeSpan.FromMinutes(metric.MetricTarget).Hours,
                    TimeSpan.FromMinutes(metric.MetricTarget).Minutes, TimeSpan.FromMinutes(metric.MetricTarget).Seconds);
            }
            Double newt = metric.MetricTarget;

            List<MaintenanceRequest> FilterList = AllList.Where(x => x.ClientId == input.ClientId).ToList();
            //IEnumerable<MaintenanceRequest> results = from foo in FilterList 
            //      where input.SelectedPriority.Contains(foo.Priority)
            //      select foo;

            //IEnumerable<MaintenanceRequest> results1 = from r in results
            //                                 where input.SelectedYear.Contains(r.CreatedYear)
            //                                 select r;

            //IEnumerable<MaintenanceRequest> results2 = from r2 in results1
            //                                 where input.SelectedMonth.Contains(r2.CreatedMonth)
            //                                 select r2;
            if (input.SelectedPriority == null)
            {
                input.SelectedPriority = new string[] { };
            }
            if (input.SelectedYear == null)
            {
                input.SelectedYear = new string[] { };
            }
            if (input.SelectedMonth == null)
            {
                input.SelectedMonth = new string[] { };
            }
            IEnumerable<MaintenanceRequest> results2 = from r2 in FilterList
                                             //where input.SelectedMonth.Contains(r2.Priority) || input.SelectedYear.Contains(r2.CreatedYear) || input.SelectedMonth.Contains(r2.CreatedMonth)
                                             where ((input.SelectedPriority.ToList().Count == 0 || input.SelectedPriority.Contains("") || input.SelectedPriority.Contains(r2.Priority)) &&
                                             (input.SelectedYear.ToList().Count == 0 || input.SelectedYear.Contains("") || input.SelectedYear.Contains(r2.CreatedYear)) &&
                                             (input.SelectedMonth.ToList().Count == 0 || input.SelectedMonth.Contains("") || input.SelectedMonth.Contains(r2.CreatedMonth)))
                                             select r2;


            List<ResultLine> result = results2.ToList()
    .GroupBy(l => l.ConfigItemName)
    .Select(cl => new ResultLine
    {
        AppName = cl.First().ConfigItemName,
        count = cl.Count().ToString(),

        timev = cl.Sum(x => Convert.ToDateTime(x.ServiceRestoredDateTime).Subtract(Convert.ToDateTime(x.CreatedDateTime)).TotalSeconds).ToString(),

        time = string.Format("{0}:{1}:{2}", (TimeSpan.FromSeconds(cl.Sum(x => Convert.ToDateTime(x.ServiceRestoredDateTime).Subtract(Convert.ToDateTime(x.CreatedDateTime)).TotalSeconds)).Days * 24) + (TimeSpan.FromSeconds(cl.Sum(x => Convert.ToDateTime(x.ServiceRestoredDateTime).Subtract(Convert.ToDateTime(x.CreatedDateTime)).TotalSeconds)).Hours),
      TimeSpan.FromSeconds(cl.Sum(x => Convert.ToDateTime(x.ServiceRestoredDateTime).Subtract(Convert.ToDateTime(x.CreatedDateTime)).TotalSeconds)).Minutes,
      TimeSpan.FromSeconds(cl.Sum(x => Convert.ToDateTime(x.ServiceRestoredDateTime).Subtract(Convert.ToDateTime(x.CreatedDateTime)).TotalSeconds)).Seconds),
        IsMissed = Convert.ToInt64(cl.Sum(x => Convert.ToDateTime(x.ServiceRestoredDateTime).Subtract(Convert.ToDateTime(x.CreatedDateTime)).TotalSeconds) - TimeSpan.FromMinutes(metric.MetricTarget).TotalSeconds)
        // IsMissed = Convert.ToInt32(((TimeSpan.FromSeconds(cl.Sum(x => Convert.ToDateTime(x.ServiceRestoredDateTime).Subtract(Convert.ToDateTime(x.CreatedDateTime)).TotalSeconds)).Days * 24) + (TimeSpan.FromSeconds(cl.Sum(x => Convert.ToDateTime(x.ServiceRestoredDateTime).Subtract(Convert.ToDateTime(x.CreatedDateTime)).TotalSeconds)).Hours)) - (TimeSpan.FromMinutes(newt).Hours))
        //(cl.Sum(x => Convert.ToDateTime(x.ServiceRestoredDateTime).Subtract(Convert.ToDateTime(x.CreatedDateTime)).TotalSeconds)).Days * 24) + cl.Sum(x => Convert.ToDateTime(x.ServiceRestoredDateTime).Subtract(Convert.ToDateTime(x.CreatedDateTime)).TotalSeconds).Hours)-newt)
    }).ToList();
            input.GetResult = result.Where(x => !x.timev.StartsWith("-")).ToList();
            input.SlALable = slaName;
            input.metricId = metric.MetricId;
            return View(input);
        }

    }
}
