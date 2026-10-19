using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.IncidentNavigator.AspxReport;
using WebUI.Areas.IncidentNavigator.Models;

namespace WebUI.Areas.IncidentNavigator.Controllers
{
    public class INAnalysisController : Controller
    {
        //
        // GET: /IncidentNavigator/INAnalysis/

        public ActionResult Index()
        {
            return View();
        }
             
       
        public ActionResult Incident_Read([DataSourceRequest]DataSourceRequest request)
        {
            List<INAnalysisModel> replst = new List<INAnalysisModel>();
            var objclient = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            JsonResult  json= Json(IncidentNavigatorHelper.INTrendingReportList(objclient.Id).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

    }
}
