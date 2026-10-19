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

namespace NTTData.AMOPortal.WebUI.Controllers
{
    public class IncidentController : Cruder<Incident, IncidentInput>
    {
        public IncidentController(ICrudService<Incident> service, IMapper<Incident, IncidentInput> v)
            : base(service, v)
        {
        }

        public ActionResult LoadDataSet(IncidentInput input)
        {
            return View(input);
        }

        public override ActionResult Index()
        {
            IncidentInput input = new IncidentInput();
            //List<Incident> Ilist = service.GetAll().Take(50).ToList();
            //input.incidentLists = Ilist;

            return View(input);
        }
        public ActionResult DelIncident(int id)
        {
            List<Incident> AllList = service.GetAll().ToList();
            var incident = AllList.Where(a => a.Id == id).FirstOrDefault();
            int clientId = incident.ClientId;
            service.Delete(id);


            IncidentInput mrinput = new IncidentInput();
            mrinput.ClientId = clientId;
            ModelState.AddModelError("CreateMsg", "Incident Request record deleted Successfully");
            ViewBag.pagedItems = mrinput.GetPagedList(1);
            return View("ViewIncidentbyClient", mrinput);


            //return RedirectToAction("ViewIncidentbyClient", new { ClientId = clientId });
        }

        //[HttpPost]
        //public ActionResult ViewIncidentbyClient(int clientid)
        //{
        //    IncidentInput input = new IncidentInput();
        //    input.ClientId = clientid;
        //    return View(input);
        //}

        public ActionResult ViewIncident(int ClientId, int PageNumber = 1)
        {
            IncidentInput input = new IncidentInput();
            input.ClientId = ClientId;
            //int? clientid = input.ClientId;
            //List<Incident> Ilist = new List<Incident>();
            //if (input.ClientId != null)
            //{
            //    List<Incident> AllList = service.GetAll().ToList();
            //    Ilist = AllList.Where(a => a.ClientId == input.ClientId).ToList();
            //}
            //else
            //{
            //    Ilist = service.GetAll().Take(50).ToList();

            //}
            //input.incidentLists = Ilist;
            //input.ClientId = clientid;

            ViewBag.pagedItems = input.GetPagedList(PageNumber);
            return View("ViewIncidentbyClient", input);
        }


        public ActionResult ViewIncidentbyClient()
        {
            IncidentInput input = new IncidentInput();
            ModelState.AddModelError("CreateMsg", "Please select a Client");
            //int? clientid = input.ClientId;
            //List<Incident> Ilist = new List<Incident>();
            //if (input.ClientId != null)
            //{
            //    List<Incident> AllList = service.GetAll().ToList();
            //    Ilist = AllList.Where(a => a.ClientId == input.ClientId).ToList();
            //}
            //else
            //{
            //    Ilist = service.GetAll().Take(50).ToList();

            //}
            //input.incidentLists = Ilist;
            //input.ClientId = clientid;
            return View("ViewIncidentbyClient", input);
        }

        public ActionResult SLADashboard()
        {

            var input = new IncidentInput();
            Client client = (Client)Session["Client"];

            input.ClientId = client.Id;
            input.SelectedYear = new string[] { };
            input.SelectedMonth = new string[] { };
            input.SelectedPriority = new string[] { };



            return View(input);
        }


        public override ActionResult Edit(int id)
        {
            Incident incident = new Incident();
            incident = service.Get(id);

            IncidentInput incidentInput = new IncidentInput();
            incidentInput = AutoMapper.Mapper.Map<Incident, IncidentInput>(incident);
            return View("Edit", incidentInput);
        }

        [HttpPost]
        public override ActionResult Edit(IncidentInput input)
        {
            if (!ModelState.IsValid)
                return View("Edit", input);
            //var e = editMapper.MapToEntity(input, service.Get(input.Id));
            var e = AutoMapper.Mapper.Map<IncidentInput, Incident>(input, service.Get(input.Id));
            service.Save();
            ModelState.AddModelError("CreateMsg", "Incident Edited successfully");
            return View(input);

        }

        public ActionResult CreateIncident(int cid)
        {
            IncidentInput incidentInput = new IncidentInput();
            incidentInput.ClientId = cid;
            return View(incidentInput);
        }

        [HttpPost]
        public ActionResult CreateIncident(IncidentInput input)
        {
            if (!ModelState.IsValid)
                return View("CreateIncident", input);
            Incident incident = new Incident();
            //var id = service.Create(createMapper.MapToEntity(input, incident));
            var id = service.Create(AutoMapper.Mapper.Map<IncidentInput, Incident>(input));

            var e = service.Get(id);
            ModelState.AddModelError("CreateMsg", "Incident is created successfully");

            return View(input);

        }


        [HttpPost]
        public ActionResult SLADashboard(IncidentInput input)
        {
            var slaName = input.SlALable;
            List<Incident> AllList = service.GetAll().ToList();
            var metric = input.MetricsSpecList.Where(a => a.ClientId == input.ClientId && a.MetricName == "Application Downtime Summary").FirstOrDefault();
            if (metric != null)
            {
                //input.TargetValue = metric.MetricTarget.ToString();



                input.TargetValue = string.Format("{0}:{1}:{2}", TimeSpan.FromMinutes(metric.MetricTarget).Days * 24 + TimeSpan.FromMinutes(metric.MetricTarget).Hours,
                    TimeSpan.FromMinutes(metric.MetricTarget).Minutes, TimeSpan.FromMinutes(metric.MetricTarget).Seconds);
            }
            Double newt = metric.MetricTarget;

            List<Incident> FilterList = AllList.Where(x => x.ClientId == input.ClientId).ToList();
            //IEnumerable<Incident> results = from foo in FilterList 
            //      where input.SelectedPriority.Contains(foo.Priority)
            //      select foo;

            //IEnumerable<Incident> results1 = from r in results
            //                                 where input.SelectedYear.Contains(r.CreatedYear)
            //                                 select r;

            //IEnumerable<Incident> results2 = from r2 in results1
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
            IEnumerable<Incident> results2 = from r2 in FilterList
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
