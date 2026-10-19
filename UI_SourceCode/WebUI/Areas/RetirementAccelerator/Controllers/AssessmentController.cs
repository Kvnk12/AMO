using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.RetirementAccelerator.Models;
using Domain.Entities.RetirementAccelerator;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using static System.Net.WebRequestMethods;
using System.Net.Mime;
using WebUI.Dto;

namespace WebUI.Areas.RetirementAccelerator.Controllers
{
    public class AssessmentController : Controller
    {
        //
        // GET: /RetirementAccelerator/Assesment/

        public ActionResult Assess()
        {
            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];

            return View("Index");
        }

        ////public ActionResult RomEstimator()
        ////{
        ////    Client client = (Client)Session["Client"];

        ////    return View("Index");
        ////}

        public ActionResult AssessRead([DataSourceRequest] DataSourceRequest request, int ApplicationId = 0)
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            return Json(RAHelper.FetchAssessDetails(ApplicationId, client.ClientId,uid).ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AssessSave([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<AssessmentModel> Question)
        {
            int uid = (int)Session["UserId"];
            if (Question != null && ModelState.IsValid)
            {
                foreach (var Q in Question)
                {
                    RAHelper.AssessUpdate(Q,uid);
                }
            }
            return Json(Question.ToDataSourceResult(request, ModelState));
        }

        public ActionResult EstimateRead([DataSourceRequest] DataSourceRequest request, int AppId = 0)
        {
            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            return Json(RAHelper.FetchEstimateDetails(AppId, client.ClientId, uid).ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EstimateSave([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ArchivalDecommissionModel> Answers)
        {
            int uid = (int)Session["UserId"];
            if (Answers != null && ModelState.IsValid)
            {
                foreach (var ANS in Answers)
                {
                    RAHelper.EstimateUpdate(ANS, uid);
                }
            }
            return Json(Answers.ToDataSourceResult(request, ModelState));
        }


        //New method for saving ratings out of grid saving Functionality
        [HttpPost]
        public ActionResult AssessAjaxSave(RatingJSObject rating)
        {
            int uid = (int)Session["UserId"];
            if (rating != null && rating.RatingsList!=null && ModelState.IsValid)
            {
                foreach (var Q in rating.RatingsList)
                {
                    if(Q.RowId!=null && Q.RowId != 0)
                    {
                        AssessmentModel model = new AssessmentModel();
                        model.Id = Q.RowId.Value;
                        model.OriginalRating = Q.OriginalRating;
                        model.FinalRating = Q.FinalRating;
                        model.Comments = Q.Comments;
                        RAHelper.AssessUpdate(model, uid);
                        
                    }
                }
            }
            return Json(rating);
        }



        public ActionResult GetApplications()
        {
            // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var ApplicationList = RAHelper.getApplications(client.ClientId);
            return Json(ApplicationList, JsonRequestBehavior.AllowGet);
        }

        //-------------------------------------------------------------------Notes------------------------------------------------------------------//
        [HttpPost]
        public ActionResult UpdateNotes(int Id = 0, string Notes = "")
        {
            //  Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var EncodeString = Server.HtmlDecode(Notes);
            if (Id != 0)
            {
                RAHelper.saveNotes(Id, EncodeString.Trim());
            }
            return Json(new { EncodeString }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult getNotes(int AppId = 0)
        {
            //var client = (Client)Session["Client"];
            //int uid = (int)Session["UserId"];
            if (AppId != 0)
            {
                string model = RAHelper.getNotesDetails(AppId);
                return Json(model);
            }
            else
            {
                return Json(new { Failure = "Select any Application" });
            }
        }



        //----------------------------------------------Disposition Recommendation--------------------------------------------//
        public ActionResult Disposition()
        {
            return View("_Disposition");
        }

        [HttpPost]
        public ActionResult getDispositionDetails(int AppId = 0)
        {
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            if (AppId != 0)
            {
                var model = RAHelper.getDispDetails(AppId);
                return Json(model);
            }
            else
            {
                return Json(new {Failure="Select any Application" });
            }   
        }

        [HttpPost]
        public ActionResult saveDispositionDetails(AssessDispRecomModel model)
        {
            int uid = (int)Session["UserId"];
            if (model.RAApplicationId != 0)
            {
                RAHelper.SaveDispDetails(model,uid);
                return Json(new { Success = "Data saved successfully!!" });
            }
            else
            {
                return Json(new { Failure = "Select any Application" });
            }

        }
        public ActionResult RomEstimator()
        {
            return View("ROMEstimator");
        }

        //---------------------------------------------------ROM Estimator--------------------------------------------------------//

        //----------------Project Management --------------------//

        public ActionResult ProjManageRead([DataSourceRequest] DataSourceRequest request, int ApplicationId = 0)
        {
            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            return Json(RAHelper.FetchProjManageDetails(ApplicationId, client.ClientId, uid).ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ProjManageSave([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ProjectManagementModel> project)
        {
            int uid = (int)Session["UserId"];
            if (project != null && ModelState.IsValid)
            {
                foreach (var p in project)
                {
                    RAHelper.ProjManageUpdate(p, uid);
                }
            }
            return Json(project.ToDataSourceResult(request, ModelState));
        }

        //----------------Support Cost --------------------//

        public ActionResult SupportCostRead([DataSourceRequest] DataSourceRequest request, int ApplicationId = 0)
        {
            // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            return Json(RAHelper.FetchSupportCostDetails(ApplicationId, client.ClientId, uid).ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SupportCostSave([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<SupportCostModel> Cost)
        {
            int uid = (int)Session["UserId"];
            if (Cost != null && ModelState.IsValid)
            {
                foreach (var Q in Cost)
                {
                    RAHelper.SupportCostUpdate(Q, uid);
                }
            }
            return Json(Cost.ToDataSourceResult(request, ModelState));
        }

        //----------------Execution Cost --------------------//
        [HttpPost]
        public ActionResult ExecutionCostRead([DataSourceRequest] DataSourceRequest request, int ApplicationId = 0)
        {
          //  Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            return Json(RAHelper.FetchExecutionCostDetails(ApplicationId, client.ClientId, uid).ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ExecutionCostSave([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ExecutionCostModel> Cost)
        {
            int uid = (int)Session["UserId"];
            if (Cost != null && ModelState.IsValid)
            {
                foreach (var Q in Cost)
                {
                    RAHelper.ExecutionCostUpdate(Q, uid);
                }
            }
            return Json(Cost.ToDataSourceResult(request, ModelState));
        }


        //public ActionResult ExecutionFileUpload(IEnumerable<HttpPostedFileBase> files, int RowId=0)
        //{
        //    Client client = (Client)Session["Client"];
        //    int uid = (int)Session["UserId"];
        //    if (files != null && RowId!=0)
        //    {
        //        foreach(var file in files)
        //        {
        //            RAHelper.executionFileSave(file,RowId,uid);
        //        }
        //    }


        //    return Content("");
        //}

        public ActionResult OngoingArchivalRead([DataSourceRequest] DataSourceRequest request, int ApplicationId = 0)
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            return Json(RAHelper.FetchOngoingArchivalDetails(ApplicationId, client.ClientId, uid).ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult OngoingArchivalSave([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<OngoingArchivalSupportModel> Cost)
        {
            int uid = (int)Session["UserId"];
            if (Cost != null && ModelState.IsValid)
            {
                foreach (var Q in Cost)
                {
                    RAHelper.OngoingArchivalUpdate(Q, uid);
                }
            }
            return Json(Cost.ToDataSourceResult(request, ModelState));
        }


        [HttpPost]
        public JsonResult NewFileUpload()
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            var RowId = Convert.ToInt32(Request.Form["dbrowid"]);
            var whichTable = Convert.ToInt32(Request.Form["gridname"]);
            if (Request.Files[0] != null && RowId != 0)
            {
                HttpPostedFileBase file = Request.Files[0];
                //foreach (HttpPostedFileBase file in Request.Files)
                //{
                      RAHelper.FileSave(file, RowId, uid, whichTable);
                      //RAHelper.executionFileSave(file, RowId, uid,whichTable);
                //}
            }


            return Json("");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult FileDestroy(int rowId = 0, int table = 0)
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            if (rowId != 0 && table != 0)
            {
                RAHelper.FileRemove(rowId, uid, table);
            }

            return Content("");
        }

        public ActionResult downloadFile(int rowId=0, int table=0)
        {
            if(rowId!=0 && table != 0)
            {
                var FileModel = RAHelper.filedownload(rowId, table);
                return File(FileModel.file, MediaTypeNames.Application.Octet, FileModel.fileName);
            }
            else{
                return RedirectToAction("GetFileError");
            }
        }

        public ActionResult GetFileError()
        {
            return Content("<h4>Sorry, no file found!!</h4>");
        }

    }
}
