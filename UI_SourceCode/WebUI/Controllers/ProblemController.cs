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
    public class ProblemController : Cruder<Problem, ProblemInput>
    {
        public ProblemController(ICrudService<Problem> service, IMapper<Problem, ProblemInput> v)
            : base(service, v)
        {
        }

        public ActionResult LoadDataSet(ProblemInput input)
        {
            return View(input);
        }

        public override ActionResult Index()
        {
            ProblemInput input = new ProblemInput();

            return View(input);
        }
        public ActionResult DelProblem(int id)
        {
            List<Problem> AllList = service.GetAll().ToList();
            var Problem = AllList.Where(a => a.Id == id).FirstOrDefault();
            int clientId = Problem.ClientId;
            service.Delete(id);

            ProblemInput mrinput = new ProblemInput();
            mrinput.ClientId = clientId;
            ModelState.AddModelError("CreateMsg", "Problem Request record deleted Successfully");
            ViewBag.pagedItems = mrinput.GetPagedList(1);
            return View("ViewProblembyClient", mrinput);
        }


        public ActionResult ViewProblem(int ClientId, int PageNumber = 1)
        {
            ProblemInput input = new ProblemInput();
            input.ClientId = ClientId;
            ViewBag.pagedItems = input.GetPagedList(PageNumber);
            return View("ViewProblembyClient", input);
        }


        public ActionResult ViewProblembyClient()
        {
            ProblemInput input = new ProblemInput();
            ModelState.AddModelError("CreateMsg", "Please select a Client");
            return View("ViewProblembyClient", input);
        }

        public ActionResult SLADashboard(int ClientId = 0)
        {

            var input = new ProblemInput();

            input.ClientId = ClientId;
            input.SelectedYear = new string[] { };
            input.SelectedMonth = new string[] { };
            input.SelectedPriority = new string[] { };



            return View(input);
        }


        public override ActionResult Edit(int id)
        {
            Problem Problem = new Problem();
            Problem = service.Get(id);

            ProblemInput ProblemInput = new ProblemInput();
            ProblemInput = AutoMapper.Mapper.Map<Problem, ProblemInput>(Problem);
            return View("Edit", ProblemInput);
        }

        [HttpPost]
        public override ActionResult Edit(ProblemInput input)
        {
            if (!ModelState.IsValid)
                return View("Edit", input);
            var e = AutoMapper.Mapper.Map<ProblemInput, Problem>(input, service.Get(input.Id));
            service.Save();
            ModelState.AddModelError("CreateMsg", "Problem request Edited successfully");
            return View(input);

        }

        public ActionResult CreateProblem(int id)
        {
            ProblemInput ProblemInput = new ProblemInput();
            ProblemInput.ClientId = id;
            return View(ProblemInput);
        }

        [HttpPost]
        public ActionResult CreateProblem(ProblemInput input)
        {
            if (!ModelState.IsValid)
                return View("CreateProblem", input);
            Problem Problem = new Problem();
            var id = service.Create(AutoMapper.Mapper.Map<ProblemInput, Problem>(input));

            var e = service.Get(id);
            ModelState.AddModelError("CreateMsg", "Problem request is created successfully");

            return View(input);

        }


        [HttpPost]
        public ActionResult SLADashboard(ProblemInput input)
        {
            var slaName = input.SlALable;
            List<Problem> AllList = service.GetAll().ToList();
            var metric = input.MetricsSpecList.Where(a => a.ClientId == input.ClientId && a.MetricName == "Application Downtime Summary").FirstOrDefault();
            if (metric != null)
            {
                



                input.TargetValue = string.Format("{0}:{1}:{2}", TimeSpan.FromMinutes(metric.MetricTarget).Days * 24 + TimeSpan.FromMinutes(metric.MetricTarget).Hours,
                    TimeSpan.FromMinutes(metric.MetricTarget).Minutes, TimeSpan.FromMinutes(metric.MetricTarget).Seconds);
            }
            Double newt = metric.MetricTarget;

            List<Problem> FilterList = AllList.Where(x => x.ClientId == input.ClientId).ToList();
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
            IEnumerable<Problem> results2 = from r2 in FilterList
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
    }).ToList();
            input.GetResult = result.Where(x => !x.timev.StartsWith("-")).ToList();
            input.SlALable = slaName;
            input.metricId = metric.MetricId;
            return View(input);
        }

    }
}
