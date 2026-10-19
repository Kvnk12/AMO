using NTTData.AMOPortal.Domain.Abstract;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.Domain.Service;
using NTTData.AMOPortal.WebUI.Dto;
using NTTData.AMOPortal.WebUI.Mapper;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Service;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.WebUI.Filters;

namespace WebUI.Areas.DemandManager.Controllers
{
    public class TrendingReportDemandController : Controller
    {
        //[CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Demand Manager" }, null)]
        //[ActionTrack]
        //public ActionResult Index()
        //{

        //    //ClusterManagementInput c = new ClusterManagementInput();
        //    //c.ClientId = ClientId;

        //    return View();

        //}
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Demand Manager" }, null)]     
        [ActionTrack]
        public ActionResult ChallengeIndex()
        {

            //ClusterManagementInput c = new ClusterManagementInput();
            //c.ClientId = ClientId;

            return View("ChallengeIndex");

        }
    }
}

