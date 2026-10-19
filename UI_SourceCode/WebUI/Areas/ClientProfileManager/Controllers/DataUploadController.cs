using Data;
using Domain.Entities.ClientProfileManager;
using NTTData.AMOPortal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.ClientProfileManager.ViewModel;
using WebUI.Areas.ClientProfileManager.Dto;
using System.Net.Mime;
using System.Text;
using System.IO;
using System.Web.UI;
using NTTData.AMOPortal.WebUI.Filters;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using WebUI.App_Start;
using WebUI.Dto;

namespace WebUI.Areas.ClientProfileManager.Controllers
{
    public class DataUploadController : Controller
    {
        //
        // GET: /ClientProfileManager/DataUpload/

        [CustomAuthorize( new string[]{"sysadmin","admin","superuser"}, new string[] {"Client Profile Manager" },null )]
        public ActionResult Index()
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                // NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                int ClientId = client.ClientId;
                jobDetailInput jobDetail = new jobDetailInput();
                jobDetail.ClientId = ClientId;
                jobDetail.jobDetailList = ClientProfileManagerHelper.GetJobDetail(ClientId);
                return View(jobDetail);
            }
         }
     

        //
        // GET: /ClientProfileManager/DataUpload/Create
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        public ActionResult JobScheduling()
        {
            scheduleJobInput scheduleinput = new scheduleJobInput();
            //NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            scheduleinput.ClientId = ClientId;
            scheduleinput.ClientName = client.ClientName;
            return View("ScheduleJobs", scheduleinput);
        }

        /// <summary>  
        /// This method will return PartialView with schedul job data  
        /// </summary>  
        /// <param name="client"></param>
        /// <param name="appid"></param>
        /// <returns></returns>  
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        public PartialViewResult GetScheduleJobs(string client, int appid)
        {

            string clilentname= client.Replace(" ", "");
            if (clilentname != "")
            {
                var jobs = ClientProfileManagerHelper.GetScheduleJobs(clilentname, appid);
                //Set default emp records  
                return PartialView("_ScheduleJobList", jobs);
            }
            else
            {
                return PartialView("_ScheduleJobList", null);
            }
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        [HttpPost]
        public PartialViewResult CreateJob(scheduleJobInput jobinput)
        {
            //string str = DateTime.Now.ToLongTimeString();
            string returnVal = "";
            ////if (string.IsNullOrEmpty(jobinput.time))
            ////    jobinput.time = "120000";
            ////else
            ////    jobinput.time = jobinput.time.Replace(":", "");
            TempData.Clear();
            if (string.IsNullOrEmpty(jobinput.ScheduleType))
                ModelState.AddModelError("CreateValidationErrMsg", "Please select schedule type.");
            if (jobinput.ScheduleType == "MONTHLY" && string.IsNullOrEmpty(jobinput.MonthDay))
                ModelState.AddModelError("CreateValidationErrMsg", "Please select month day.");
            if (jobinput.ScheduleType == "WEEKLY" && string.IsNullOrEmpty(jobinput.WeekDay))
                ModelState.AddModelError("CreateValidationErrMsg", "Please select week day.");
            IList<ScheduleJobsViewModel> jobs = new List<ScheduleJobsViewModel>();
            if (ModelState.IsValid)
            {
                jobs = ClientProfileManagerHelper.CreateScheduleJobs(jobinput, out returnVal);
                //Set default emp records  
                if (returnVal == "Success")
                    TempData.Add("JobCreationSuccess", "Schedule job got created successfully.");
                else
                    ModelState.AddModelError("CreateErrMsg", returnVal);
            }
            return PartialView("_ScheduleJobList", jobs);
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        public PartialViewResult DeleteJob(string JobName, string ApplicationId, string TemplateIdentifier)
        {
            int appid = 0;
            int.TryParse(ApplicationId, out appid);

            var jobs = ClientProfileManagerHelper.DeleteJob(TemplateIdentifier, appid, JobName);
            //Set default emp records  

            return PartialView("_ScheduleJobList", jobs);
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        [HttpGet]
        public ActionResult Acknowledge(int id)
        {
            try
            {
                // TODO: Add delete logic here
                int? clientid = null;
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    JobDetail jd = unitOfWork.JobDetails.Get(jid => jid.Id == id).First();
                    jd.IsChecked = true;
                    clientid = jd.ClientId;
                    unitOfWork.JobDetails.Update(jd);// (id) (jobDetails);
                    unitOfWork.Save();
                }
                return RedirectToAction("Index", new { Clientid = clientid });
            }
            catch
            {
                return View();
            }
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        [HttpGet]
        public ActionResult RejectedRecords(int jobid)
        {
            try
            {
                ViewData["JobId"] = jobid;
                var dt = ClientProfileManagerHelper.GetRejectedRecords(jobid);
              //  ExportViewModel model = new ExportViewModel();
              //  model.RejectedRecords = dt;
                return View("RejectedRecords", dt);
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                return View();
            }
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

 

        public ActionResult DataFileUpload()
        {
            ////NTTData.AMOPortal.WebUI.Dto.HomeInput menuItemsInput = (NTTData.AMOPortal.WebUI.Dto.HomeInput)Session["MenuItemsInput"];
            ////if (menuItemsInput.UserRole.ToLower() != "sysadmin" && menuItemsInput.UserRole.ToLower() != "admin")
            ////{
            ////    return Content("Not authorised to access data mapping module");
            ////}
            FileUploadViewModel fileData = new FileUploadViewModel();
            //NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            fileData.ClientId = ClientId;
            return View("DataFileUpload", fileData);
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        [HttpPost]
        public ActionResult UploadFile(FileUploadViewModel fileUploadViewModel, string btnUpload)
        {
            ModelState.Clear();
            try
            {
                //NTTData.AMOPortal.WebUI.Dto.HomeInput menuItemsInput = (NTTData.AMOPortal.WebUI.Dto.HomeInput)Session["MenuItemsInput"];
                //if (menuItemsInput.UserRole.ToLower() != "sysadmin" && menuItemsInput.UserRole.ToLower() != "admin")
                //{
                //    return Content("Not authorised to access data mapping module");
                //}

                ////if (fileUploadViewModel.ClientId == 0)
                ////{
                ////    ModelState.AddModelError("ClientIdErr", "Please select the client.");
                ////}
                //NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                int ClientId = client.ClientId;

                if (fileUploadViewModel.httpPostedFileData == null)
                {
                    ModelState.AddModelError("FileErr", "Please select the data file.");
                }

                if (!ModelState.IsValid)
                {
                    return View("DataFileUpload", fileUploadViewModel);
                }
                fileUploadViewModel.ClientId = ClientId;
                if (btnUpload.ToLower() == "execute")
                    fileUploadViewModel.ScheduleJob = true;
                else
                    fileUploadViewModel.ScheduleJob = false;

                var result = false;
                try
                {
                    result = ClientProfileManagerHelper.UploadDataFile(fileUploadViewModel);

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("UploadErrMsg", "Error occured. Please contact AM-Optimization team." + ex.Message);
                    return View("DataFileUpload", fileUploadViewModel);
                }
                if (!result)
                {
                    ModelState.AddModelError("FileUploadErrMsg", "Please check the file name; please rename the file to appropriate Template identifier name. Please go to data mapping page to find correct Template identifier.");
                }
                else
                {
                    if (btnUpload.ToLower() == "execute")
                        TempData["SuccessFileUploadMsg"] = "Data file got uploaded successfully. File would be processed and email notification would be sent.";
                    else
                        TempData["SuccessFileUploadMsg"] = "Data file got uploaded successfully. File would be processed as per the schedule.";
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("FinalUploadErrMsg", "Error occured. Please contact AM-Optimization team." + ex.Message);
                return View("DataFileUpload", fileUploadViewModel);
            }
            return View("DataFileUpload", fileUploadViewModel);
        }

        //[CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        //[HttpPost]
        //public ActionResult FilterByDate(jobDetailInput jobdetail)
        //{
        //    using (var contextFactory = new DbContextFactory())
        //    {
        //        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
        //        //NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
        //        ClientDetail client = (ClientDetail)Session["SessionClient"];
        //        jobDetailInput JDVM = new jobDetailInput();
        //        JDVM = jobdetail;
        //        JDVM.ClientId = client.ClientId;
        //        DateTime toDate=jobdetail.toDt.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
        //        JDVM.jobDetailList = ClientProfileManagerHelper.GetJobDetailByDate(jobdetail.fromDt, toDate, jobdetail.ClientId);
        //        return View("index", JDVM);
        //    }

        //}

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        public ActionResult ExporttoExcel()
        {
            return View(new ExportViewModel());
        }

        // [HttpPost]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        public void ExportRejRecords(int JobId)
        {
            var dt = ClientProfileManagerHelper.GetRejectedRecords(JobId);

            var grid = new System.Web.UI.WebControls.GridView();

            grid.DataSource = dt;
            grid.DataBind();

            Response.ClearContent();
            string Filename = "RejectedRecords_" + JobId.ToString() + ".xlsx";
            Response.AddHeader("content-disposition", "attachment; filename=" + Filename);
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();



        }

        protected string RenderRazorViewToString(string viewName, object model)
        {
            if (model != null)
            {
                ViewData.Model = model;
            }
            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        [HttpPost]
        public FileStreamResult ExportRejectedRecords(int jobid)
        {
           // int jobid = 0;

            //if (ViewData["JobId"]!=null)
            // jobid = Convert.ToInt32(ViewData["JobId"]);
            string fileName = "RejectedRecords_"+ jobid.ToString() + ".xlsx";
            var dt = ClientProfileManagerHelper.GetRejectedRecords(jobid);
            System.IO.Stream stream = Helper.GetXLFileStream(dt);
             stream.Position = 0;
            string mimeType = "application/vnd.ms-excel";
            var fileStreamResult = new FileStreamResult(stream, mimeType);
            fileStreamResult.FileDownloadName = fileName;
            fileStreamResult.FileStream.Seek(0, SeekOrigin.Begin);
            return fileStreamResult;
        }




        [HttpGet]
        public JsonResult TemplateIdentifier(int appid)
        {
            //NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            Db db = new Db();
            var identifier=(from app in db.StoreApplication
                           join maptmp in db.MappingTemplate on app.MappingId equals maptmp.Id
                           where app.ApplicationId == appid && maptmp.ClientId== client.ClientId && maptmp.AcknowledgeBy!=null
                            select new
                           {
                               Id = maptmp.Id,
                               DataTemplateId = maptmp.GlobalMappingIdentifier,
                              // ack= maptmp.AcknowledgeBy//.HasValue
                            }
                          ).ToList();

            //DbContextFactory Context = new DbContextFactory();
            //Repo<NTTData.AMOPortal.Domain.Entities.StoreApplication> accounts = new Data.Repo<Domain.Entities.StoreApplication>(Context);
            //List<NTTData.AMOPortal.Domain.Entities.ClientAccountContract> list = accounts.GetAll().Where(a => a.ClientId == clientid && a.AccountId == accountid).OrderBy(o => o.ContractName).ToList();
            var Items = identifier.Select(Field => new SelectListItem
            {
                Value = Field.DataTemplateId.ToString(),
                Text = Field.DataTemplateId.ToString()
            });

            return Json(Items, JsonRequestBehavior.AllowGet);
        }


    }
}
