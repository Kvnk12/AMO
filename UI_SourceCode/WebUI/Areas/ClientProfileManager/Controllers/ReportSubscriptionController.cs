using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Dto;
using WebUI.Areas.IncidentNavigator.Models;
using WebUI.Areas.ClientProfileManager.ViewModel;
using Kendo.Mvc.UI;
using WebUI.Areas.IncidentNavigator.Dto;
using Kendo.Mvc.Extensions;
using NTTData.AMOPortal.WebUI.Filters;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Dto;
using System.Collections.Specialized;
using Domain.Entities;
using System.Web.Configuration;
using Data;
using NTTData.AMOPortal.Data;
using System.Data;
using System.IO;
using Domain.Entities.IncidentNavigator;


namespace WebUI.Areas.ClientProfileManager.Controllers
{
    public class ReportSubscriptionController : Controller
    {
        //
        // GET: /ClientProfileManager/ReportSubscription/

      

        public ActionResult RptSubscriptionList(string reportname)
        {
            var UserId = (int)Session["UserId"];
            if (App_Start.Helper.HasRole((int)App_Start.Helper.AMOApplicationName.IncidentNavigator, (int)App_Start.Helper.AMOUserRoles.Sysadmin)) {
                ViewData["useridrole"] = "true";
            }
            else
            {
                ViewData["useridrole"] = "false";
            }
            
            ViewData["useridInt"] = Convert.ToInt32(UserId);

            if (reportname == null) {
                ViewData["reportName"] = "noreport";
                    }
            else
            {
                ViewData["reportName"] = reportname.ToString();
            }
            return View("RptSubscriptionList");
        }

        public ActionResult RPTJobDetailList(string reportname1)
        {
            if (reportname1 == null)
            {
                ViewData["reportName"] = "noreport";
            }
            else
            {
                ViewData["reportName"] = reportname1.ToString();
            }

            return View("RPTJobDetailList");
        }

        public string getEmailID(int usid)
        {
            
            using (var contextFactory = new DbContextFactory())
            {
                
                Db db = new Db();
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var item = (from a in db.UserProfiles
                            where a.Id == usid
                            select a.Email).FirstOrDefault().ToString();
                return item;
            }
        }

        [HttpPost]
        public ActionResult GetReportSubscriptions([DataSourceRequest] DataSourceRequest request , string reportBy)
        {
            HomeInput MenuItemsInput = (HomeInput)Session["MenuItemsInput"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            //List<int?> toolid = new List<int?>();
            //foreach (var a in MenuItemsInput.UserApplications)
            //{
            //    if (client.ClientId == a.ClientId)
            //    {
            //        toolid.Add(a.ApplicationId);
            //    }
            //}
            var rec = ClientProfileManagerHelper.GetRptSubscription(client.ClientId,reportBy);
            
            return Json(rec.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }
        

        [HttpPost]
        public ActionResult GetReportJobDetail([DataSourceRequest] DataSourceRequest request , string reportBy1)
       {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var rec = ClientProfileManagerHelper.GetRptJobDetailList(client.ClientId, reportBy1);
            //return Json(rec, JsonRequestBehavior.AllowGet);
            return Json(rec.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);

        }
        public FileResult DownloadRptJOB(int Id)
        {
            Db db = new Db();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var jobDet = unitOfWork.RPTJobDetail.Get(a => a.Id == Id).FirstOrDefault();

                var rptDet = (from a in db.RPTSubscription
                              join b in db.RPTJobDetail on a.Id equals b.RPTSubscriptionId
                              where a.Id == jobDet.RPTSubscriptionId
                              select a).FirstOrDefault();
                
                    var fileName = rptDet.TelerikReportName + "_" + rptDet.ScheduleTime + "." + rptDet.ReportFormat;

                    fileName.ToString();

                    if (jobDet.ReportDocument != null)
                    {
                        return File(jobDet.ReportDocument, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                    }
                
            }
            return null;
        }

        public ActionResult DownloadRptAnalysJOB(int Id)
        {
            Db db = new Db();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var jobDet = unitOfWork.RPTJobDetail.Get(a => a.Id == Id).FirstOrDefault();

                var rptDet = (from a in db.RPTSubscription
                              join b in db.RPTJobDetail on a.Id equals b.RPTSubscriptionId
                              where a.Id == jobDet.RPTSubscriptionId
                              select a).FirstOrDefault();

                System.Diagnostics.Process.Start(jobDet.RPTJobUrl);
                //return null;
                return Json(jobDet.RPTJobUrl, JsonRequestBehavior.AllowGet);
              // return Redirect(jobDet.RPTJobUrl);
            }
           
        }
        [HttpPost]
        public ActionResult ArchiveRptJOB(int Id)
        {
            var res = ClientProfileManagerHelper.ArchiveRptDetails(Id);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetReportDetailsForEdit(int Ids)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ID = Convert.ToInt32(Ids);
            var rec = ClientProfileManagerHelper.GetReportDetails(ID);
            return Json(rec, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditReportSubcriptionDetails(RTPSubcriptionModel RPT)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            RPT.UpdatedBy = usid;
            RPT.UpdatedDate = DateTime.Now;
            RPT.Recipients = RPT.Recipients.Replace(',', ';');
            //RPT.Status = "Draft";
            //IST CODE
            //if (RPT.Timezone == "IST")
            //{
            //    if (Convert.ToInt32(RPT.selectMinutes) >= 00 && Convert.ToInt32(RPT.selectMinutes) < 30)
            //    {
            //        RPT.selectHours = (Convert.ToInt32(RPT.selectHours) - 11 + 24).ToString(); 
            //        RPT.selectMinutes = (Convert.ToInt32(RPT.selectMinutes) - 30 + 60).ToString(); 
            //        if ((Convert.ToInt32(RPT.selectHours) >= 11))
            //        {
            //            RPT.selectHours = (Convert.ToInt32(RPT.selectHours) - 24).ToString();
            //        }


            //    }
            //    else if (Convert.ToInt32(RPT.selectMinutes) >= 30 && Convert.ToInt32(RPT.selectMinutes) <= 59)
            //    {
            //        RPT.selectHours = (Convert.ToInt32(RPT.selectHours) - 10 + 24).ToString();
            //        RPT.selectMinutes = (Convert.ToInt32(RPT.selectMinutes) - 30).ToString();
            //        if (Convert.ToInt32(RPT.selectMinutes) == 0)
            //        {
            //            RPT.selectMinutes = "00";
            //        }
            //        if ((Convert.ToInt32(RPT.selectHours) >= 10))
            //        {
            //            RPT.selectHours = (Convert.ToInt32(RPT.selectHours) - 24).ToString();
            //        }
            //    }
            //}

            if (RPT.Timezone == "PST")
            {
                RPT.selectHours = (Convert.ToInt32(RPT.selectHours) + 3).ToString();
                if (Convert.ToInt32(RPT.selectHours)>=24)
                {
                    RPT.selectHours = (Convert.ToInt32(RPT.selectHours) - 24).ToString();
                }
            }

            if (RPT.Timezone == "CST")
            {
                if (RPT.selectHours == "23")
                {
                    RPT.selectHours = "0";
                }
                else
                {
                    RPT.selectHours = (Convert.ToInt32(RPT.selectHours) + 1).ToString();
                }
            }
            RPT.ScheduleTime = (RPT.selectHours + ":" + RPT.selectMinutes + ":" + "00").ToString(); //if pm selecthours + 12
            var res = ClientProfileManagerHelper.UpdateReportSubscription(RPT);
            return Json(RPT, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult SaveEditReportSubcriptionDetails(RTPSubcriptionModel RPT)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int usid = (int)Session["UserId"];
            RPT.CreatedBy = usid;
            RPT.CreatedDate = DateTime.Now;
            RPT.Status = "Draft";
            RPT.ClientID = client.ClientId;
            RPT.AppID = (int)App_Start.Helper.AMOApplicationName.IncidentNavigator;
            //RPT.Recipients = RPT.Recipients.Replace(',', ';');
            //CODE FOR IST
            //if (RPT.Timezone == "IST")
            //{
            //    if (Convert.ToInt32(RPT.selectMinutes) >= 00 && Convert.ToInt32(RPT.selectMinutes) < 30)
            //    {
            //        RPT.selectHours = (Convert.ToInt32(RPT.selectHours) - 11 + 24).ToString();
            //        RPT.selectMinutes = (Convert.ToInt32(RPT.selectMinutes) - 30 + 60).ToString();
            //        if ((Convert.ToInt32(RPT.selectHours) >= 11)){
            //            RPT.selectHours = (Convert.ToInt32(RPT.selectHours) - 24).ToString();
            //        }


            //    }
            //    else if (Convert.ToInt32(RPT.selectMinutes) >= 30 && Convert.ToInt32(RPT.selectMinutes) <= 59)
            //    {
            //        RPT.selectHours = (Convert.ToInt32(RPT.selectHours) - 10 + 24).ToString();
            //        RPT.selectMinutes = (Convert.ToInt32(RPT.selectMinutes) - 30).ToString();
            //        if (Convert.ToInt32(RPT.selectMinutes) == 0)
            //        {
            //            RPT.selectMinutes = "00";
            //        }
            //        if ((Convert.ToInt32(RPT.selectHours) >= 10))
            //        {
            //            RPT.selectHours = (Convert.ToInt32(RPT.selectHours) - 24).ToString();
            //        }
            //    }
            //}
            if (RPT.Timezone == "PST")
            {
                RPT.selectHours = (Convert.ToInt32(RPT.selectHours) + 3).ToString();
                if (Convert.ToInt32(RPT.selectHours) >= 24)
                {
                    RPT.selectHours = (Convert.ToInt32(RPT.selectHours) - 24).ToString();
                }
            }

            if (RPT.Timezone == "CST")
            {
                if(RPT.selectHours == "23")
                {
                    RPT.selectHours = "0";
                }
                else
                {
                    RPT.selectHours = (Convert.ToInt32(RPT.selectHours) + 1).ToString();
                }               
            }

            RPT.ScheduleTime = (RPT.selectHours + ":" + RPT.selectMinutes + ":" + "00").ToString(); //if pm selecthours + 12
            
            var res = ClientProfileManagerHelper.CreateReportSubscription(RPT);
            return Json(res, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult DeleteReportSubscription(int Id)
        {
            //var res = ClientProfileManagerHelper.DeleteReportSub(Id);
            //return Json(res, JsonRequestBehavior.AllowGet);
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var rptObject = unitOfWork.RPTSubscription.GetByID(Id);
                rptObject.Status = "Deleted";
                unitOfWork.RPTSubscription.Update(rptObject);
                unitOfWork.Save();
                return Json(rptObject, JsonRequestBehavior.AllowGet);

            }

        }

        [HttpPost]
        public ActionResult CopyReportSubscription(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = ClientProfileManagerHelper.CopyReportSub(Id);
                var rptObject = unitOfWork.RPTSubscription.GetByID(Id);
                return Json(rptObject, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult approveReportSubscription(int Id)
        {
            Db db = new Db();
           
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var rptObject = unitOfWork.RPTSubscription.GetByID(Id);
                    rptObject.Status = "Approved";
                    unitOfWork.RPTSubscription.Update(rptObject);
                    unitOfWork.Save();
                    return Json(rptObject, JsonRequestBehavior.AllowGet);

                }
            
            
         }

     

            
    }
}
