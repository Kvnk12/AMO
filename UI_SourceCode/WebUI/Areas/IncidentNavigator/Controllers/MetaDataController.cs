using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using NTTData.AMOPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.IncidentNavigator.Models;
using WebUI.Dto;
using Data;
using System.Data;
using Domain.Abstract;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.WebUI.Dto;
using WebUI.Areas.IncidentNavigator.Dto;
using NTTData.AMOPortal.WebUI.Mapper;
using NTTData.AMOPortal.Domain.Service;
using System.IO;
using System.Text;
using Domain.Entities.IncidentNavigator;
using PagedList;
using PagedList.Mvc;
using WebUI.Areas.IncidentNavigator.AspxReport;
using System.Reflection;
using log4net;
using NTTData.AMOPortal.WebUI.Filters;

namespace WebUI.Areas.IncidentNavigator.Controllers
{
    public class MetaDataController : Controller
    {
        //
        // GET: /IncidentNavigator/MetaData/

        private static readonly ILog Log = LogManager.GetLogger(typeof(KEDBController));

        public ActionResult TowerIndex()
        {
            var Cl = (ClientDetail)Session["SessionClient"];
            ViewData["LogicalColumnList"] = GetColumnList(Cl.ClientId);
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);

            return View("TowerIndex");
        }
        [ActionTrack]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ReadTrack([DataSourceRequest]DataSourceRequest request)
        {
            //  var client = (Client)Session["Client"];
            try
            {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
            var model = IncidentNavigatorHelper.readTrack(client.ClientId);
            return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Error in ReadTrack MetaData controller", ex);
                throw ex;
            }
        }

        // [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ReadAssignGroup([DataSourceRequest] DataSourceRequest request, int TrackId = 0)
        {
            // var clid = (Client)Session["Client"];
            try
            {
                 ClientDetail clid = (ClientDetail)Session["SessionClient"];
                return Json(IncidentNavigatorHelper.getRuleList(TrackId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Error in ReadAssignGroup MetaData controller", ex);
                throw ex;
            }
        }


        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult CreateAssignGroup([DataSourceRequest]DataSourceRequest request, TeamRuleModel model,int TrId = 0)
        {
          //  var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            if (!string.IsNullOrEmpty(model.INFieldName)  && TrId!=0)
            {
                model.TeamId = TrId;
              
                var ii= IncidentNavigatorHelper.addTeamRule(model);
                model.TId=ii;
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult UpdateAssignGroup([DataSourceRequest]DataSourceRequest request, TeamRuleModel model)
        {
            var uid = (int)Session["UserId"];
            if (!string.IsNullOrEmpty(model.INFieldName) )
            {
         
                IncidentNavigatorHelper.updateTeamRule(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult DeleteAssignGroup([DataSourceRequest]DataSourceRequest request, TeamRuleModel model)
        {
            IncidentNavigatorHelper.RemoveTeamRule(model.TId);
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public IEnumerable<SelectListItem> GetColumnList(int ClientID)
        { 
            var list = IncidentNavigatorHelper.getColumnList(ClientID);
            return list;
        }
     //[HttpGet]
        public ActionResult GetColumnValueList(string text,string Columnid)//, string ColValue)
           {
            //var contextFactory = new DbContextFactory();
          
          //  var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int Id = client.ClientId;
            if (!String.IsNullOrWhiteSpace(Columnid))
            {
                
                var list = IncidentNavigatorHelper.getColumnValuesList(Id, Columnid).ToList();
                if (list.Count == 0)
                {
                    List<SelectListItem> list1 = new List<SelectListItem>();
                    list1.Add(new SelectListItem { Value = "", Text = "" });
                    return  Json(list1, JsonRequestBehavior.AllowGet);
                }
               
               if (string.IsNullOrEmpty(text))
                    return Json(list.Select(A => new { Value = A.FieldValue.ToString(), Text = A.FieldValue }).ToList(), JsonRequestBehavior.AllowGet);
               else
                    return Json(list.Select(A => new { Value = A.FieldValue, Text = A.FieldValue }).Where(w=>w.Value.Contains(text)).ToList(), JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        //-------------------------------------------------------------------Notes------------------------------------------------------------------//
        [HttpGet]
        [ActionTrack]
        public ActionResult GenerateRule()
        {
         //   Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            string retval = IncidentNavigatorHelper.GenerateNExecRule(client.ClientId,"SAVE",uid);

            return Json(new {}, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        [ActionTrack]
        public ActionResult ValidateRule()
        {
           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            var updatestring =  IncidentNavigatorHelper.GenerateNExecRule(client.ClientId,"No",uid);

            return Json(updatestring, JsonRequestBehavior.AllowGet);
        }


   //-----------------------------------------------------------------------xxxxx------------------------------------------------------------------------------------     
        public ActionResult ContractIndex()
        {

            var Cl = (ClientDetail)Session["SessionClient"];
            ViewData["LogicalColumnList"] = GetColumnList(Cl.ClientId);
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);

            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ReadTrackContract([DataSourceRequest] DataSourceRequest request)
        {
            //  var client = (Client)Session["Client"];
            try
            {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                var model = IncidentNavigatorHelper.readTrackContract(client.ClientId);
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Error in ReadTrackContract MetaData controller", ex);
                throw ex;
            }
        }

        // [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ReadAssignGroupContract([DataSourceRequest] DataSourceRequest request, int Id = 0)
        {
            // var clid = (Client)Session["Client"];
            try
            {


                ClientDetail clid = (ClientDetail)Session["SessionClient"];
                return Json(IncidentNavigatorHelper.getRuleListContract(Id).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Log.Error("Error in ReadAssignGroupContract MetaData controller", ex);
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult CreateAssignGroupContract([DataSourceRequest] DataSourceRequest request, INContractRuleModel model, int CId = 0)
        {
            //  var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            if (!string.IsNullOrEmpty(model.INFieldName) && CId != 0)
            {
                model.ContractId = CId;

                var ii = IncidentNavigatorHelper.addTeamRuleContract(model);
                model.Id = ii;
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult UpdateAssignGroupContract([DataSourceRequest] DataSourceRequest request, INContractRuleModel model)
        {
            var uid = (int)Session["UserId"];
            if (!string.IsNullOrEmpty(model.INFieldName))
            {

                IncidentNavigatorHelper.updateTeamRuleContract(model);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult DeleteAssignGroupContract([DataSourceRequest] DataSourceRequest request, INContractRuleModel model)
        {
            IncidentNavigatorHelper.RemoveTeamRuleContract(model.Id);
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }


        public IEnumerable<SelectListItem> GetColumnListContract(int ClientID)
        {
            var list = IncidentNavigatorHelper.getColumnListContract(ClientID);
            return list;
        }



        //[HttpGet]
        public ActionResult GetColumnValueListContract(string text, string Columnid)//, string ColValue)
        {
            //var contextFactory = new DbContextFactory();

            //  var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int Id = client.ClientId;
            if (!String.IsNullOrWhiteSpace(Columnid))
            {
                var list = IncidentNavigatorHelper.getColumnValuesListContract(Id, Columnid).ToList();
                // return list;
                if (string.IsNullOrEmpty(text))
                    return Json(list.Select(A => new { Value = A.FieldValue.ToString(), Text = A.FieldValue }).ToList(), JsonRequestBehavior.AllowGet);
                else
                    return Json(list.Select(A => new { Value = A.FieldValue, Text = A.FieldValue }).Where(w => w.Value.Contains(text)).ToList(), JsonRequestBehavior.AllowGet);
            }
            return null;
        }
                                                                                           
        //-------------------------------------------------------------------Notes------------------------------------------------------------------//
        [HttpGet]
        [ActionTrack]
        public ActionResult GenerateRuleContract()
        {
            //   Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            string retval = IncidentNavigatorHelper.GenerateNExecRuleContract(client.ClientId, "SAVE",uid);

            return Json(new { }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        [ActionTrack]
        public ActionResult ValidateRuleContract()
        {
            // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            var updatestring = IncidentNavigatorHelper.GenerateNExecRuleContract(client.ClientId, "No",uid);

            return Json(updatestring, JsonRequestBehavior.AllowGet);
        }



        //-----------------------------------------------------------------------xxxxx------------------------------------------------------------------------------------     


        public ActionResult LogicalIndex()
        {
            var Cl = (ClientDetail)Session["SessionClient"];
            ViewData["LogicalColumnList"] = GetColumnList(Cl.ClientId);
            var UserId = (int)Session["UserId"];
            ViewData["GetStringResult"] = getFullName(UserId);

            return View();
        }

        public IEnumerable<SelectListItem> GetListOfLogicalColumn(int clientid)
        {
            try
            {
                var ServTypenames = IncidentNavigatorHelper.GetListOfLogicalColumn(clientid);
                return ServTypenames;
            }
            catch (Exception ex)
            {
                Log.Error("Error in ReadAssignGroupContract MetaData controller", ex);
                throw ex;
            }

        }

        public ActionResult ReadTrackConfiguration([DataSourceRequest] DataSourceRequest request)
        {
            //  var client = (Client)Session["Client"];
            try
            {
                ClientDetail client = (ClientDetail)Session["SessionClient"];
                var model = IncidentNavigatorHelper.readTrackConfiguration(client.ClientId);
                return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
           
             catch (Exception ex)
            {
                Log.Error("Error in ReadAssignGroupContract MetaData controller", ex);
                throw ex;
            }

        }

        [ActionTrack]
        public ActionResult CreateTrackConfiguration([DataSourceRequest] DataSourceRequest request, INColumnValueModel InColumnValuevar)
            {
            Db db = new Db();
            var Cl = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            INColumnValue inc = new INColumnValue();
            var res = IncidentNavigatorHelper.CreateColumnValue(InColumnValuevar,uid,Cl.ClientId);
           

            if (res > 0)
            {
                InColumnValuevar.Id = res;
            }

            else
            {
                ModelState.AddModelError("SaveErr", "Error occured while saving data.");
            }


            return Json(new[] { InColumnValuevar }.ToDataSourceResult(request, ModelState));

        }

        
        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult UpdateTrackConfiguration([DataSourceRequest] DataSourceRequest request, INColumnValueModel InColumnValuevar)
        {
            var Cl = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            if (InColumnValuevar != null && ModelState.IsValid)
            {

                var res = IncidentNavigatorHelper.UpdateTrackConfiguration(InColumnValuevar, uid);
            }

            return Json(new[] { InColumnValuevar }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        [ActionTrack]
        public ActionResult RemoveTrackConfiguration([DataSourceRequest] DataSourceRequest request, INColumnValueModel InColumnValuevar, int Id=0)
        {
            Db db = new Db();
            var Cl = (ClientDetail)Session["SessionClient"];
            
          
            if (InColumnValuevar != null && Id > 0)
            {
                var v = IncidentNavigatorHelper.RemoveTrackConfiguration(Id,Cl.ClientId);
                
            }
            else
            {                           
                ModelState.AddModelError("Id", "Service Improvement has plan and progress updated, can’t be deleted");
                //return Json(new[] { InColumnValuevar }.ToDataSourceResult(request, ModelState));
            }

            return Json(new[] { InColumnValuevar }.ToDataSourceResult(request, ModelState));
        }


        public IEnumerable<SelectListItem> GetColumnListLogical(int ClientID)
        {
            var list = IncidentNavigatorHelper.getColumnListLogical(ClientID);
            return list;
        }

        public ActionResult ReadAssignGroupLogical([DataSourceRequest] DataSourceRequest request, int Id = 0)
        {
            // var clid = (Client)Session["Client"];
            try
            {
                ClientDetail cl = (ClientDetail)Session["SessionClient"];
                return Json(IncidentNavigatorHelper.getRuleListLogical(Id, cl.ClientId).AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            
            catch (Exception ex)
            {
                Log.Error("Error in ReadAssignGroupLogical MetaData controller", ex);
                throw ex;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult CreateAssignGroupLogical([DataSourceRequest] DataSourceRequest request, INColumnValueRuleModel model, int CId = 0)
        {
            //  var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            if (!string.IsNullOrEmpty(model.INFieldName) && CId != 0)
            {
                model.INColumnValueId = CId;

                var ii = IncidentNavigatorHelper.addTeamRuleLogical(model , client.ClientId , uid);
                model.Id = ii;
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult UpdateAssignGroupLogical([DataSourceRequest] DataSourceRequest request, INColumnValueRuleModel model)
        {
            var uid = (int)Session["UserId"];
            if (!string.IsNullOrEmpty(model.INFieldName))
            {

                IncidentNavigatorHelper.updateTeamRuleLogical(model,uid);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        [ActionTrack]
        public ActionResult DeleteAssignGroupLogical([DataSourceRequest] DataSourceRequest request, INColumnValueRuleModel model)
        {
            IncidentNavigatorHelper.RemoveTeamRuleLogical(model.Id);
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }


        [HttpGet]
        [ActionTrack]
        public ActionResult GenerateRuleConfiguration()
        {
            //   Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            string retval = IncidentNavigatorHelper.GenerateNExecRuleLogical(client.ClientId, "SAVE",uid);

            return Json(new { }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        [ActionTrack]
        public ActionResult ValidateRuleConfiguration()
        {
            // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            var uid = (int)Session["UserId"];
            var updatestring = IncidentNavigatorHelper.GenerateNExecRuleLogical(client.ClientId, "No",uid);

            return Json(updatestring, JsonRequestBehavior.AllowGet);
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
