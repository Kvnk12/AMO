using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.WebUI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.IncidentNavigator.Controllers
{
    public class ClusterAuditController : Controller
    {
        //
        // GET: /IncidentNavigator/ClusterAudit/

        [ActionTrack]
        public ActionResult Index()
        {
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);

            return View();
        }
        public string getFullName(int userId)
        {
            Db db = new Db();
            var fullName = (from b in db.UserProfiles
                            where b.Id == userId
                            select b.FirstName + " " + b.LastName).FirstOrDefault();

            return fullName;
        }


    }
}
