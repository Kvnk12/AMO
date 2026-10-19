using Data;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.WebUI.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using WebUI.Areas.TransformationAccelerator.Model;
using WebUI.Dto;
using WebUI.Areas.IncidentNavigator;

namespace WebUI.Areas.TransformationAccelerator.Controllers
{
    public class ConfigureTAReportController : Controller
    {
        //
        // GET: /TransformationAccelerator/ConfigureTAReport/
        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Transformation Accelerator" }, null)]
        public ActionResult Index()
        {
            return View();
        }
        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Transformation Accelerator" }, null)]
        public ActionResult TransformationAcceleratorReports()
        {
            //TAReportObservationModel mod = prepareCommonParameters();
            //WebConfigurationManager.AppSettings["TelerikReportServer"];
            return View();

        }
        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Transformation Accelerator" }, null)]
        public ActionResult TAReports()
        {
            TAReportObservationModel mod = prepareCommonParameters();
            return View(mod);

        }
        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Transformation Accelerator" }, null)]
        public ActionResult TAReports_ProgressAgainstPlan()
        {
            TAReportObservationModel mod = prepareCommonParameters();

            ClientDetail client = (ClientDetail)Session["SessionClient"];
            IEnumerable<string> userEmailList = IncidentNavigatorHelper.getEmailUserPersonnel(client.ClientId);

            ViewBag.userEmailList = userEmailList;
            return View(mod);

        }
        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Transformation Accelerator" }, null)]
        public ActionResult TAReports_DetailedProgress()
        {
            TAReportObservationModel mod = prepareCommonParameters();
            GetYearsLst(mod, mod.ClientId);
            return View(mod);

        }
        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Transformation Accelerator" }, null)]
        public ActionResult TAReports_summary()
        {
            TAReportObservationModel mod = prepareCommonParameters();
            mod.ProgressVal = GetProgressVal(mod.ClientId, mod.ReportYear, mod.ReportQtr);
            GetYearsLst(mod, mod.ClientId);

            ClientDetail client = (ClientDetail)Session["SessionClient"];
            IEnumerable<string> userEmailList = IncidentNavigatorHelper.getEmailUserPersonnel(client.ClientId);

            ViewBag.userEmailList = userEmailList;
            return View(mod);

        }
        private TAReportObservationModel prepareCommonParameters()
        {
            //  Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            TAReportObservationModel mod = new TAReportObservationModel();
            mod.ClientName = client.ClientName;
            mod.UserName = IdeaManager.IMHelper.GetUserName(uid);
            mod.ClientId = client.ClientId;
            mod.UserId = uid;
            mod.ToYear = DateTime.Today.Date;
            mod.FromYear = DateTime.Today.Date.AddMonths(-1);
            mod.DateRange = mod.FromYear.Value.ToString("MM/dd/yyyy") + " To " + mod.ToYear.Value.ToString("MM/dd/yyyy");
            mod.ClusterPath = WebConfigurationManager.AppSettings["ClusterPath"];
            mod.CreatedDt = DateTime.Now;
            mod.ReportYear = DateTime.Today.Year;
            mod.ReportQtr = (DateTime.Today.Month -1)/3 +1;

            //mod.ProgressVal = GetProgressVal(mod.ClientId, mod.ReportYear, mod.ReportQtr);
           // GetFilterInfo(mod, client.ClientId);
           
            return mod;
        }
        public int GetProgressVal(int Clientid,int Year,int Qtr)
        {
            int ProgressVal =0;
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var paramNameset1 = new string[] { "ClientId", "@Year","@Qtr"};
                var paramValueset1 = new object[] { Clientid ,Year,Qtr};
                DataTable dt = new DataTable();
                dt = unitOfWork.GetDataFromStoredProcedure("GetTA_ProgressValQry", paramNameset1, paramValueset1);
                DataRow dr =  dt.Rows[0];
                string st = dr.ItemArray.ElementAt(0).ToString();
                if (st.Length > 0)
                {
                    ProgressVal = int.Parse(st.Substring(0, st.IndexOf(".")));
                }
                

            }
            return ProgressVal;
        }
        public int GetYearsLst(TAReportObservationModel mod,int Clientid)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var paramNameset1 = new string[] { "ClientId"};
                var paramValueset1 = new object[] { Clientid};
                DataTable dt = new DataTable();
                dt = unitOfWork.GetDataFromStoredProcedure("GetTA_SummaryYearsQry", paramNameset1, paramValueset1);
                List<SelectListItem> ls;

                int i = 0;
                ls = new List<SelectListItem>();

                foreach (DataRow row in dt.Rows)
                {
                    ls.Add(new SelectListItem() { Text = row.ItemArray[0].ToString(), Value = row.ItemArray[0].ToString() });
                    i++;
                }
                mod.YearsList = ls;
                
            }
            return 0;
        }
        
        public static void GetFilterInfo(TAReportObservationModel mod, int ClientId)
        {
                return;
            }

        }


   
}
