using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Areas.IncidentNavigator.Models;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using WebUI.Areas.IncidentNavigator.Dto;
using Kendo.Mvc.Extensions;
using NTTData.AMOPortal.WebUI.Filters;
using NTTData.AMOPortal.Domain.Entities;
using WebUI.Dto;
using NTTData.AMOPortal.WebUI.Dto;
using log4net;
using System.IO;
using NTTData.AMOPortal.Data;
using System.Data;
using Data;

namespace WebUI.Areas.IncidentNavigator.Controllers
{
    public class KeywordSearchController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(KeywordSearchController));
        // GET: /IncidentNavigator/KeywordSearch/

        [ActionTrack]
        public ActionResult Index()
        {

            return RedirectToAction("ReadKey");
        }
        public ActionResult KeyWordSearch_new()
        {

            return RedirectToAction("New_ReadKey");
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        [ActionTrack]
        public ActionResult AddKey(KeywordSearchModel keysearch)
        {
                var currentTime = DateTime.Now;
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);

            // var cl = (Client)Session["Client"];
            ClientDetail cl = (ClientDetail)Session["SessionClient"];
                keysearch.CreatedDate = currentTime;
                keysearch.UpdatedDate = currentTime;
                keysearch.ClientId = cl.ClientId;
                var lang = IncidentNavigatorHelper.GetWordLang(cl.ClientId);
                if (lang == null)
                {
                    keysearch.WordLanguage = "en";

                }
                else
                    keysearch.WordLanguage = lang.ToString();

                var res = IncidentNavigatorHelper.AddKeyWord(keysearch);

                var res2 = IncidentNavigatorHelper.GetKeyWordList(cl.ClientId);
                keysearch.AlphaList = res2;
                // keysearch.listofkeywords = IncidentNavigatorHelper.GetKeyWordList(); 
                keysearch.listofkeywords = IncidentNavigatorHelper.GetClientWordValue(cl.ClientId);
                return View("Index", keysearch);
           
        }
        [ActionTrack]
        public ActionResult New_AddKey(KeywordSearchModel keysearch)
        {
          try {
                var UserId = (int)Session["UserId"];
                ViewData["GetStringResult"] = getFullName(UserId);

                var currentTime = DateTime.Now;
            // var cl = (Client)Session["Client"];
            ClientDetail cl = (ClientDetail)Session["SessionClient"];
            keysearch.CreatedDate = currentTime;
            keysearch.UpdatedDate = currentTime;
            keysearch.ClientId = cl.ClientId;
            var lang = IncidentNavigatorHelper.GetWordLang(cl.ClientId);
            if (lang == null)
            {
                keysearch.WordLanguage = "en";

            }
            else
                keysearch.WordLanguage = lang.ToString();
            var res = IncidentNavigatorHelper.AddKeyWord(keysearch);
            keysearch.StartDate = DateTime.Now.AddMonths(-1);
            keysearch.EndDate = DateTime.Now;

            var res2 = IncidentNavigatorHelper.GetKeyWordList(cl.ClientId);
            keysearch.AlphaList = res2;
            // keysearch.listofkeywords = IncidentNavigatorHelper.GetKeyWordList(); 
            IList<KeywordListInput> keylists= IncidentNavigatorHelper.New_GetClientWordValue(cl.ClientId);
            keysearch.listofkeywords = keylists.OrderBy(a => a.keywords).ToList();
            keysearch.listofAllkeywords = keylists.OrderByDescending(b => b.WordType).ToList();
            keysearch.TotalIncidents = (from a in keysearch.listofkeywords
                                select a.TotalInc).FirstOrDefault();
            return View("KeyWordSearch_new", keysearch);
          }
          catch (Exception ex)
          {
              Log.Error("Error in KeywordSearchController: New_AddKey ", ex);
              throw ex;
          }
        }

        public ActionResult ReadKey()
        {
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);

            KeywordSearchModel v = new KeywordSearchModel();
            // var cl = (Client)Session["Client"];
            ClientDetail cl = (ClientDetail)Session["SessionClient"];
            var res = IncidentNavigatorHelper.GetKeyWordList(cl.ClientId);
            v.AlphaList = res;
            // v.listofkeywords= IncidentNavigatorHelper.GetKeyWordList();
            v.listofkeywords = IncidentNavigatorHelper.GetClientWordValue(cl.ClientId);
            return View("Index", v);
        }
        
        public ActionResult New_ReadKey()
        {
          try {
                var UserId = (int)Session["UserId"];
                ViewData["GetStringResult"] = getFullName(UserId);

                KeywordSearchModel v = new KeywordSearchModel();
            
            ClientDetail cl = (ClientDetail)Session["SessionClient"];
            var res = IncidentNavigatorHelper.GetKeyWordList(cl.ClientId);//, DateFilter, v.StartDate, v.EndDate);
            v.AlphaList = res;
            // v.listofkeywords= IncidentNavigatorHelper.GetKeyWordList();
           // IList<KeywordListInput> keylist = IncidentNavigatorHelper.GetClientWordValue(cl.ClientId);
            IList<KeywordListInput> keylist = IncidentNavigatorHelper.New_GetClientWordValue(cl.ClientId);//, DateFilter, v.StartDate, v.EndDate);
            v.listofkeywords = keylist.OrderBy(p => p.keywords).ToList();
            v.listofAllkeywords = keylist.OrderByDescending(b => b.WordType).ToList();

            v.TotalIncidents = (from a in v.listofkeywords
                                select a.TotalInc).FirstOrDefault();

            return View("KeyWordSearch_new", v);
          }
          catch (Exception ex)
          {
              Log.Error("Error in KeywordSearchController: New_ReadKey ", ex);
              throw ex;
          }
        }
        [HttpPost]
        public ActionResult FilterKey(string DateFilter , DateTime StartDate , DateTime EndDate )
        {
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);

            KeywordSearchModel v = new KeywordSearchModel();
            ClientDetail cl = (ClientDetail)Session["SessionClient"];
            v.DateFilter = DateFilter;
            v.StartDate = StartDate;
            v.EndDate = EndDate;
            var res = IncidentNavigatorHelper.GetKeyWordList(cl.ClientId);
            v.AlphaList = res;
            // v.listofkeywords= IncidentNavigatorHelper.GetKeyWordList
            IList<KeywordListInput> keylist = IncidentNavigatorHelper.New_GetClientWordValue(cl.ClientId);
            v.listofkeywords = keylist.OrderBy(a => a.keywords).ToList();
            v.listofAllkeywords = keylist.OrderByDescending(b => b.WordType).ToList();

            v.TotalIncidents = (from a in v.listofkeywords
                                select a.TotalInc).FirstOrDefault();
            v.listofAllkeywords = IncidentNavigatorHelper.GetClientWordValue(cl.ClientId);
            return PartialView("_keywordSearch", v);
            
        }

        [ActionTrack]
        [HttpPost]
        public ActionResult DeleteKey(string selectedINs)
        {
           
            KeywordSearchModel v = new KeywordSearchModel();

            var res = IncidentNavigatorHelper.RemoveKeyword(selectedINs);

            return Content(res.ToString());
           
        }

        [HttpPost]
        public ActionResult KeywordReportList([DataSourceRequest] DataSourceRequest request, string selectedINs)
        {
            //  NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            //List<KeywordReportListModel> jk = new List<KeywordReportListModel>();
            // jk.Add(new KeywordReportListModel { Id = 1, TotalIncidentCount=222,WordIncidentCount= 45, WordIncidentPer=356, WordValue= "fgdsg" });
            var res = IncidentNavigatorHelper.GetReportList(client.ClientId, selectedINs);

            return Json(res.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
            
        }
        [HttpPost]
        public ActionResult New_KeywordReportList([DataSourceRequest] DataSourceRequest request, string selectedINs, string selectedWords, string DateFilter, DateTime? StartDate=null, DateTime? EndDate=null)
        {
          try {
                Log.Debug("KeywordReportList Grid Selection : "+ selectedWords+" "+ selectedINs);
            //  NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            //List<KeywordReportListModel> jk = new List<KeywordReportListModel>();
            // jk.Add(new KeywordReportListModel { Id = 1, TotalIncidentCount=222,WordIncidentCount= 45, WordIncidentPer=356, WordValue= "fgdsg" });
            if (StartDate == null)
            {
                StartDate = DateTime.Now.AddMonths(-1);
            }
            
            if (EndDate == null)
            {
                EndDate = DateTime.Now;
            }
            
            var res = IncidentNavigatorHelper.GetReportList(client.ClientId,  selectedWords);
            var result = res.OrderBy(e => e.WordValue);
            return Json(result.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
          }
          catch (Exception ex)
          {
              Log.Error("Error in KeywordSearchController: New_KeywordReportList ", ex);
              throw ex;
          }
        }

        [ActionTrack]
        [HttpPost]
        public ActionResult GetKeywordCount([DataSourceRequest] DataSourceRequest request, string WordValue)
        {
            //  NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var res1 = IncidentNavigatorHelper.GetKeywordCountByCI(client.ClientId, WordValue);
            return Json(res1.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
            
        }

        [ActionTrack]
        [HttpPost]
        public ActionResult GetIncidents([DataSourceRequest] DataSourceRequest request, string WordValue1, string ByConfigItem)
        { 
            //  NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            //var res1 = IncidentNavigatorHelper.GetKeywordCountByCI(client.Id, WordValue, ByConfigItem);
            //KeywordIncidentModel ks = new KeywordIncidentModel();
            //List<KeywordIncidentModel> ss = new List<KeywordIncidentModel>();
            //ks.Incidentid = 1.ToString();
            //ks.Title = "abc";
            //ks.Status = "xyz";
            //ks.ConfigItem = "jack";
            //ks.CreatedDate = DateTime.Now;
            //ks.ResolvedDate = DateTime.Now;
            //ss.Add(ks);
            var ss = IncidentNavigatorHelper.GetIncidents(client.ClientId, WordValue1, ByConfigItem);
            return Json(ss.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);

                //return Json(res1.ToDataSourceResult(request, ModelState));
            
        }

        [ActionTrack]
        [HttpPost]
        public ActionResult GetKeywordDrilldownGrid([DataSourceRequest] DataSourceRequest request, string WordValue)
        {
          try {
                Log.Debug("KeywordSearch Drilldown Selected word: "+ WordValue);
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            List<SLAMaintenenceModel> model = IncidentNavigatorHelper.KeywordDrillDownData(ClientId, WordValue).ToList();
            var result= Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            result.MaxJsonLength = int.MaxValue;
            var jsonResult = Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
          }
          catch (Exception ex)
          {
              Log.Error("Error in KeywordSearchController: GetKeywordDrilldownGrid ", ex);
              throw ex;
          }
        }

        public ActionResult ExportTicketData(string selectedWordList)
        {
            try
            {
                // var client = (Client)Session["Client"];
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                int ClientId = client.ClientId;
                string fileName = client.ClientName.Replace(" ", "_") + "_WordsTicketData" + ".xlsx";
                System.IO.Stream stream = App_Start.Helper.GetXLFileStream(GetTicketData(ClientId, selectedWordList));
                stream.Position = 0;
                string mimeType = "application/vnd.ms-excel";
                var fileStreamResult = new FileStreamResult(stream, mimeType);
                fileStreamResult.FileDownloadName = fileName;
                fileStreamResult.FileStream.Seek(0, SeekOrigin.Begin);
                return fileStreamResult;
            }
            catch
            {
                return null;
            }
        }

        private System.Data.DataTable GetTicketData(int ClientId, string selectedWordList)
        {
            var contextFactory = new DbContextFactory();
            DataTable returntable = new DataTable();
            using (var unitOfWork = new UnitOfWorks(contextFactory.GetContext()))
            {
                var paramName = new string[] { "ClientId", "WordValues" };
                var paramValue = new object[] { ClientId, selectedWordList };
                returntable = unitOfWork.GetDataFromStoredProcedure("Keywords_TicketData", paramName, paramValue);

            }
            return returntable;
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
