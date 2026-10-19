using Data;
using NTTData.AMOPortal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.UI;
using Domain.Entities.IncidentNavigator;
using WebUI.Areas.IncidentNavigator.Models;
using WebUI.Areas.IncidentNavigator;
using PagedList;
using PagedList.Mvc;
using WebUI.Dto;

namespace WebUI.Areas.IncidentNavigator.Controllers
{
    public class KnownErrorDBController : Controller
    {

        //
        // GET: /IncidentNavigator/KnownErrorDB/
        //[HttpPost]
        public ActionResult Index(int? page, string searchCriteria)
        {
            Db db = new Db();
            KnownErrorDBModel c = new KnownErrorDBModel();
         //   NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            c.kedblist = IncidentNavigatorHelper.GetKEDBListDetails(client.ClientId,searchCriteria,page);
            c.incidentlist = new List<IncidentListModel>().ToPagedList(1, 3);
            c.incidentpopuplist = new List<IncidentPopUpList>().ToPagedList(1, 3);
            return View(c);
        }

        public PartialViewResult GetKEDBList(int? page, string searchCriteria)
        {
          //  NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var kedbList =  IncidentNavigatorHelper.GetKEDBListDetails(client.ClientId, searchCriteria,page);
            return PartialView("_KEDBList",kedbList);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddKEDB(KnownErrorDBModel KEDBinput)
        {

          //  NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            KnownErrorDBModel kedbip = new KnownErrorDBModel();
            KEDBinput.ClientId = client.ClientId;
            KnownErrorDBModel c = new KnownErrorDBModel();
            string strSuccessmsg = "";
            bool res;
            ModelState.Clear();
                if (KEDBinput.Id <= 0)
            {
                 res = IncidentNavigatorHelper.insertkedbinfo(KEDBinput);
                strSuccessmsg = "Successfully Added";
            }
            else
            {
                //update logic
                 res = IncidentNavigatorHelper.updatekedbinfo(KEDBinput);
                c.incidentlist = GetKEDBIncident(KEDBinput.Id).ToPagedList(1, 3);
                strSuccessmsg = "Successfully Updated";
            }
            if(res)
            {
                 TempData.Add("ValidationSuccessMsg", strSuccessmsg);
            }
            else
            {
                ModelState.AddModelError("SaveErrMsg", "Error while saving KEDB.");
            }

            c.kedblist = IncidentNavigatorHelper.GetKEDBListDetails(client.ClientId, KEDBinput.Search,1);
            c.incidentlist = new List<IncidentListModel>().ToPagedList(1, 3);
            c.incidentpopuplist = new List<IncidentPopUpList>().ToPagedList(1, 3);
            return View("Index", c);

        }



        //joining incident and kedbincident tables and retrieve incidentid,title,description
        public IList<IncidentListModel> GetKEDBIncident(int kedbId)
        {

            Db db = new Db();
            var incidentlist = (from k in db.KEDBIncident
                                join i in db.Incidents on k.IncidentID equals i.Id
                                where (k.KEDBID == kedbId)
                                select new IncidentListModel
                                {
                                    Description = i.Description,
                                    Title = i.Title,
                                    IncidentID = i.Id,
                                    KEDBId = k.KEDBID,
                                    Id = k.Id
                                }
                                ).ToList();

                              

            return incidentlist;
        }


        [HttpPost]
        public ActionResult EditKedb(int kedbid)
        {

            KnownErrorDBModel model = new KnownErrorDBModel();
            if (kedbid.ToString() != null)
            {

                var dt = IncidentNavigatorHelper.EditKEDBDetails(kedbid);
                dt.incidentlist = GetKEDBIncident(kedbid).ToPagedList(1, 3);

                model = dt;

            }
            return PartialView("_AddEditKEDB", model);
        }

        [HttpPost]
        public PartialViewResult LinkIncidents(string selectedINs, int KEDBId)
        {

            var res = IncidentNavigatorHelper.SaveKEDBIncidents(selectedINs,KEDBId);
            var incidentlist = GetKEDBIncident(KEDBId);
            return PartialView("_IncidentList", incidentlist.ToPagedList(1,3));
        }

        [HttpGet]
        public PartialViewResult GetIncidents(int KEDBId, int? page)
        {

            var res = GetKEDBIncident(KEDBId).ToPagedList(page ?? 1, 3);
            //var incidentlist = GetKEDBIncident(KEDBId).ToPagedList(1, 3);
            return PartialView("_IncidentList", res);
        }


        public PartialViewResult DeleteKedbIncidents(int kedbId, int kedbincidentId)
        {
            var res = IncidentNavigatorHelper.DeleteKedbIncidents(kedbincidentId);
            var dt = IncidentNavigatorHelper.EditKEDBDetails(kedbId);
            dt.incidentlist = GetKEDBIncident(kedbId).ToPagedList(1, 3);
            //Set default emp records  
            return PartialView("_AddEditKEDB", dt);
        }

        [HttpPost]
        public JsonResult IsKEDBIdExist(string KEDID)
        {

            return Json(IsKEDBAvailable(KEDID));

        }

        private bool IsKEDBAvailable(string KEDID)
        {
           Db db = new Db();
           return  !db.KEDB.Any(k => k.KEDID == KEDID);
            
        }
        

        public FileResult DownloadKEDBFile(int kedbid)
        {
            KnownErrorDBModel model = new KnownErrorDBModel();
            if (kedbid.ToString() != null)
            {

                var dt = IncidentNavigatorHelper.EditKEDBDetails(kedbid);
                return File(dt.KEDBFile, System.Net.Mime.MediaTypeNames.Application.Octet,dt.KEDBFilename );

            }
            else
            return null;
        }

        public PartialViewResult GetIncidentPopUp(string searchCriteria, int? page)
        {
           // NTTData.AMOPortal.Domain.Entities.Client client = (NTTData.AMOPortal.Domain.Entities.Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int clientid = client.ClientId;
            if (!string.IsNullOrEmpty(searchCriteria))
            { 
            Db db = new Db();
            searchCriteria = searchCriteria.Trim();
                var incidentlist = (from i in db.Incidents
                                    where (i.ClientId == clientid && (i.Description.Contains(searchCriteria) || i.Title.Contains(searchCriteria)))
                                    select new IncidentPopUpList { IncidentID = i.Id, Description = i.Description,
                                        Title = i.Title, searchcriteria = searchCriteria }
                                    ).OrderBy(o=>o.IncidentID).ToPagedList((page ?? 1),3);

                return PartialView("_IncidentPopUp", incidentlist);

            }
          
        else
        {
            var ilist = new List<IncidentPopUpList>().ToPagedList(1,3);
            return PartialView("_IncidentPopUp", ilist.ToPagedList(page ?? 1, 3));
        }
        }

    }

}




