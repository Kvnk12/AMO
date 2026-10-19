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

namespace NTTData.AMOPortal.WebUI.Controllers
{
    //public class MetricController : Controller
    //{
    public class MetricController : Cruder<Metric, MetricInput> 
    {
        public MetricController(ICrudService<Metric> service, IMapper<Metric, MetricInput> v)
            : base(service, v)
        {
         //
        // GET: /Metric/

        //public ActionResult Index()
        //{
        //    return View();
        //}
              /// <summary>
        /// Method to display Company details for updation
        /// </summary>
        /// <param name="CompanyID">int</param>
        /// <returns>Company View</returns>
        //public ActionResult ManageMetricEdit(int MetricID)
        //{
        //    Metric metric = new Metric();
          
        //    if (MetricID.HasValue)
        //    {
        //        using (Repo<Metric> soContext = new Repo<Metric>())
        //        {
        //            metric = soContext.Get(MetricID);
        //        }
        //    }
        //    return View(metric);
        //}
        ///// <summary>
        ///// Method to save Company details created/updated by user
        ///// </summary>
        ///// <param name="company">Company</param>
        ///// <returns>Company View</returns>
        //[HttpPost]
        //public ActionResult ManageMetricEdit(Metric metric)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        using (Repo<Metric> soContext = new Repo<Metric>())
        //        {
        //            if (metric.Id == 0)
        //            {
        //                soContext.Insert(metric);
                       
        //            }
        //            else
        //            {
                        
        //                soContext.Update(metric);
        //            }
        //            soContext.Save();
        //        }
        //    }
        //    return RedirectToAction("ManageCompany");
        //}
        }
    }
       

    
}
