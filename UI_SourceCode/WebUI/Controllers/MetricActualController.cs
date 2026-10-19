using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.Domain.Service;
using NTTData.AMOPortal.WebUI.Dto;
using NTTData.AMOPortal.WebUI.Mapper;

namespace NTTData.AMOPortal.WebUI.Controllers
{
   /// <summary>
    /// Metric specification controller.
    /// </summary>
    public class MetricActualController : Cruder<MetricActual, MetricActualInput>
    {


        public MetricActualController(ICrudService<MetricActual> service, IMapper<MetricActual, MetricActualInput> v)
            : base(service, v)
        {
        }

        public ActionResult LoadActual(string MetricName, int Id)
        {
            MetricActualInput input = new MetricActualInput();

            input.MetricName = MetricName;
            input.MetricId = Id;
            return View(input);
        }

      

        /// <summary>
        /// Add a new record or updat the existing record for multiple application.
        /// </summary>
        /// <param name="metricSpecificationInput">Metric specification view model.</param>
        /// <returns>Redirects to metic specification action.</returns>
       
        [HttpPost]
        public ActionResult CreateNew(MetricActualInput input)
        {
          //  bool alreadyExist = false;

            // Get the list of all metric specification information.
            List<MetricActual> MetricActualList = service.GetAll().ToList();
            
            //if checkbox for all applications is not selected, enter the single application record
           
                if (!ModelState.IsValid)
                    return View(input);
                // Get the availabe record from the list for the given application and metric id if the record already availabe in the database.

                List<MetricActual> ExistingList = MetricActualList.Where(a => a.ApplicationId == input.ApplicationId && a.MetricId == input.MetricId).ToList();
                if (ExistingList.Count == 0)
                {
                    MetricActual m = new MetricActual();
                    var id = service.Create(createMapper.MapToEntity(input, m));
                    var e = service.Get(id);
                    
                }
                else
                {
                    input.Id = ExistingList.FirstOrDefault().Id;
                    var e = editMapper.MapToEntity(input, service.Get(input.Id));
                    service.Save();
                    
                }
           
                return RedirectToAction("Create");
            }

      


     

        public ActionResult ActualSetup(MetricActualInput input)
        {
            int? clientId = input.ClientId;

            input.ClientId = clientId;
            return View(input);
        }

       
        public ActionResult EditMetricActual(MetricActualInput input)
        {
            // Keep the application and metric id in temp variable to update back into metric specification view model.
            int metricId = input.MetricId;
            int applicationId = input.ApplicationId;
            int? clientId = input.ClientId;

            // Get the list of all metric specification information.
            List<MetricActual> MetricActualList = service.GetAll().ToList();

            // Get the availabe record from the list for the given application and metric id if the record already availabe in the database.
            var MetricActualInformation = MetricActualList.Where(a => a.ApplicationId == input.ApplicationId && a.MetricId == input.MetricId).ToList();
            // Convert enumerable metric specification to metrivc specification list.
            List<MetricActual> metricActual = MetricActualInformation.ToList();

            if (metricActual != null && metricActual.Count == 1)
            {
                // Map from entity to input model.
                input = createMapper.MapToInput(metricActual[0]);

                // Record found for given application and metric id.s
                input.IsRecordFound = true;
            }
            else
            {
                // Map from empty entity to input model.
                input = createMapper.MapToInput(new MetricActual());

                input.MetricId = metricId;
                input.ApplicationId = applicationId;
                input.ClientId = clientId;
                // Get the list of all metric specification information.
               
                // Record not found for given application and metric id.s
                input.IsRecordFound = false;
            }

            var metric = input.MetricsList.Where(a => a.Id == input.MetricId).ToList();
            //// Get the metric record from the list for the given metric id.

            // Map the value to input model.
            if (metric != null && metric.Count == 1)
            {
                input.MetricName = metric[0].MetricName;
                var Munit = from M in input.MetricUnitsList
                            where M.Id == metric[0].MetricUnitId
                            select M;
                input.MetricUnit = Munit.SingleOrDefault().UnitName;
                //input.Metrices.MetricUnitId = metric[0].MetricUnitId;
            }

            // Return to metric specific view with updated view model.
            return View(input);
        }


    }
}



