using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.ClientProfileManager.Dto;
using WebUI.Areas.ClientProfileManager;
using Kendo.Mvc.Extensions;
using NTTData.AMOPortal.Domain.Entities;
using System.Text.RegularExpressions;
using System.Collections;
using System.Web.Helpers;
using System.Web.Configuration;
using NTTData.AMOPortal.Domain.Abstract;
using NTTData.AMOPortal.WebUI.Filters;

namespace WebUI.Areas.ClientProfileManager.Controllers
{
    public class PIDataMaskController : Controller
    {
        //
        // GET: /ClientProfileManager/PIDataMask/
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        public ActionResult Index()
        {
            return View();
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Read(DataSourceRequest request)
        {
            //Client client = (Client)Session["Client"];
            //var datalist = DataMaskingHelper.GetAllMaskDetails(client.Id);
            var datalist = DataMaskingHelper.GetAllMaskDetails();
            return Json(datalist.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(DataSourceRequest request, PIDataMaskListModel datalist)
        {
            if (datalist != null)
            {
                var v = DataMaskingHelper.DeleteData(datalist.Id);
            }
            return Json(new[] { datalist }.ToDataSourceResult(request, ModelState));
        }

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        [HttpPost]
        public ActionResult maskuniversal(string input, string inputPattern)
        {
            string Result = "";
            //try
            //{
            string Input = input;
            string InputPattern = @inputPattern;
            // string EmailPattern = @"(?<=[\w]{1})[\w-\._\+%]*(?=[\w]{1}@);
            // string PhonePattern = @"(?<=[\d]{3})[\d]*(?=[\d]{3})";
            Result = Regex.Replace(Input, InputPattern, "****");
            //}
            //catch(ArgumentException e)
            //{
            //    Result = "Invalid Regular Expression, Cannot perform the Function";
            //}
            return Json(new[] { Result, input });
        }

        // DONOT Delete (For Reference)

        //            Email
        //      axxxxc@abc.com
        //      @"(?<=[\w]{1})[\w-\._\+%]*(?=[\w]{1}@)"


        //   PhoneNo. 10 digits(no spaces or special symbols in between)
        //  Format +918xxxxxx789
        //      @"(?<=[\d]{3})[\d]*(?=[\d]{3})" 

        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add(DataSourceRequest request, PIDataMaskListModel datalist)
        {
            if (datalist != null && ModelState.IsValid)
            {
                try
                {
                    //var uru = Regex.Match(datalist.DataMaskExample, @datalist.DataMaskPattern);
                    //if (uru.Success)
                    var uru = new Regex(@datalist.DataMaskPattern);
                    if (uru.IsMatch(datalist.DataMaskExample))
                    {
                        var cTime = DateTime.Now;
                        int usid = (int)Session["UserId"];
                      // Client client = (Client)Session["Client"];
                        datalist.CreatedBy = usid;
                        datalist.CreatedDate = cTime;
                       // datalist.ClientId = client.Id;
                        var res = DataMaskingHelper.AddData(datalist);
                        datalist.Id = res;
                    }
                    else
                    {
                        ModelState.AddModelError("same", "This RegEx is not correct for the given input!");
                    }
                }
                catch (ArgumentException)
                {
                    ModelState.AddModelError("Exception", "This Regular Exception is not valid!");
                }
            }
            return Json(new[] { datalist }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update(DataSourceRequest request, PIDataMaskListModel datalist)
        {
            if (datalist != null && ModelState.IsValid)
            {
                try
                {
                    //var uru = Regex.Match(datalist.DataMaskExample, @datalist.DataMaskPattern);
                    //if (uru.Success)
                    var uru = new Regex(@datalist.DataMaskPattern);
                    if (uru.IsMatch(datalist.DataMaskExample))
                    {
                        var cTime = DateTime.Now;
                        int usid = (int)Session["UserId"];
                        datalist.UpdatedBy = usid;
                        datalist.UpdatedDate = cTime;
                        var r = DataMaskingHelper.UpdateData(datalist);
                    }
                    else
                    {
                        ModelState.AddModelError("same", "This RegEx is not correct for given input!");
                    }
                }
                catch (ArgumentException)
                {
                    ModelState.AddModelError("Exception", "This Regular Exception is not valid!");
                }

            }
            return Json(new[] { datalist }.ToDataSourceResult(request, ModelState));
        }


        [ActionTrack]
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Client Profile Manager" }, null)]
        public FileResult Download()
        {
                byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Areas/ClientProfileManager/Files/NttDataMaskingTool.zip"));
                string fileName = "NttDataMaskingTool.zip";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }



    }
}
