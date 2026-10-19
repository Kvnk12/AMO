using System;
using Domain.Entities;
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
using NTTData.AMOPortal.WebUI.Dto;
using NTTData.AMOPortal.Data;
using Data;
using WebUI.Dto;
using log4net;

namespace WebUI.Areas.IncidentNavigator.Controllers
{
    public class TicketsController : Controller
    {
        //
        // GET: /IncidentNavigator/KeywordSearch/

        private static readonly ILog Log = LogManager.GetLogger(typeof(TicketsController));
        private static bool bRunCluster;

        public ActionResult SLATicket()
        {
            bRunCluster = false;
            return View();
        }

        public ActionResult PendingTicket()
        {


            return View();
        }

        ////[AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetTickets([DataSourceRequest]DataSourceRequest request , DateTime StartDate, DateTime EndDate, string DateType)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            try
            { 

           // var client = (Client)Session["Client"];
            IList<SLAMaintenenceModel> SLAMaintenencList = new List<SLAMaintenenceModel>();
           
               // IEnumerable<Incident> IncidentDetails = null;
                //var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
               // var flt = request.Filters.Count;
                
                if (client.ClientId != 0)
               

                  SLAMaintenencList =  IncidentNavigatorHelper.getSlaTickets(client.ClientId, StartDate, EndDate, DateType);



            var jsonResult = Json(SLAMaintenencList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
                    catch (Exception ex)
                {
                    Log.Error("TicketsController GetTickets , Client ID is " + client.ClientId + " Exception: " + ex);
                }
            return Json(null);


        }


        public ActionResult UpdateTickets([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<SLAMaintenenceModel> SLAMaintenenceinput)
        {
           // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            try {
            int uid = (int)Session["UserId"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            if (SLAMaintenenceinput != null)
            {                
                foreach (var p in SLAMaintenenceinput)
                {
                    var updatetitem = unitOfWork.Incident.GetByID(p.Id);
                    /*
                    if ((updatetitem.ResponseExceptionIndicator == "Y" ? "Y" : "N") != (p.ResponseExceptionIndicator == true ? "Y" : "N") && updatetitem.ResponseExceptionReason == p.ResponseExceptionReason)
                    {
                        return JavaScript("alert('please enter Reason for  waive');");
                    }

                    if ((updatetitem.ResolutionExceptionIndicator == "Y" ? "Y" : "N") != (p.ResolutionExceptionIndicator == true ? "Y" : "N") && updatetitem.ResolutionExceptionReason == p.ResolutionExceptionReason)
                    {

                        return JavaScript("alert('please enter Reason for  waive');");
                    }
                    if ((updatetitem.ResponseTimeMetSLA) != (p.ResponseTimeMetSLA) && updatetitem.ResponseExceptionReason == p.ResponseExceptionReason)
                    {

                        return JavaScript("alert('please enter Reason for  waive');");
                    }
                    if ((updatetitem.ResolutionTimeMetSLA) != (p.ResolutionTimeMetSLA) && updatetitem.ResolutionExceptionReason == p.ResolutionExceptionReason)
                    {

                        return JavaScript("alert('please enter Reason for  waive');");
                    }*/
                    var cns = new AMSCustomAuditLog();
                            if (updatetitem.ResponseExceptionIndicator != (p.ResponseExceptionIndicator == true ? "Y" : "N"))
                            {
                                cns.UpdatedBy = uid;
                                cns.UpdatedTime = DateTime.Now;
                                cns.TableName = "Incident";
                                cns.Uniqueid = p.Id;
                                cns.ColumnName = "ResponseExceptionIndicator";
                                cns.ColumnValue = p.ResponseExceptionIndicator == true ? "Y" : "N";
                                cns.PreviousValue = updatetitem.ResponseExceptionIndicator;
                                cns.ClientId = client.ClientId;
                               unitOfWork.AMSCustomAuditLog.Insert(cns);
                               unitOfWork.Save();
                        //INUpdateQuery.Query = INUpdateQuery.Query + " ResponseExceptionIndicator = '" + (p.ResponseExceptionIndicator == true ? "Y" : "N") + "',";
                            }
                    if (updatetitem.ResolutionTimeMetSLA != (p.ResolutionTimeMetSLA ))
                    {
                        cns.UpdatedBy = uid;
                        cns.UpdatedTime = DateTime.Now;
                        cns.TableName = "Incident";
                        cns.Uniqueid = p.Id;
                        cns.ColumnName = "ResponseExceptionIndicator";
                        cns.ColumnValue = p.ResolutionTimeMetSLA ;
                        cns.PreviousValue = updatetitem.ResolutionTimeMetSLA;
                        cns.ClientId = client.ClientId;
                        unitOfWork.AMSCustomAuditLog.Insert(cns);
                        unitOfWork.Save();
                    }
                    if (updatetitem.ResponseTimeMetSLA != (p.ResponseTimeMetSLA))
                    {
                        cns.UpdatedBy = uid;
                        cns.UpdatedTime = DateTime.Now;
                        cns.TableName = "Incident";
                        cns.Uniqueid = p.Id;
                        cns.ColumnName = "ResponseExceptionIndicator";
                        cns.ColumnValue = p.ResponseTimeMetSLA;
                        cns.PreviousValue = updatetitem.ResponseTimeMetSLA;
                        cns.ClientId = client.ClientId;
                        unitOfWork.AMSCustomAuditLog.Insert(cns);
                        unitOfWork.Save();
                    }
                    if (updatetitem.ResponseExceptionReason != p.ResponseExceptionReason)
                            {
                                cns.UpdatedBy = uid;
                                cns.UpdatedTime = DateTime.Now;
                                cns.TableName = "Incident";
                                cns.Uniqueid = p.Id;
                                cns.ColumnName = "ResponseExceptionReason";
                                cns.ColumnValue = p.ResponseExceptionReason;
                                cns.PreviousValue = updatetitem.ResponseExceptionReason;
                                cns.ClientId = client.ClientId;
                               unitOfWork.AMSCustomAuditLog.Insert(cns);
                                unitOfWork.Save();
                      //  INUpdateQuery.Query = INUpdateQuery.Query + "ResponseExceptionReason = '" + p.ResponseExceptionReason + "',";
                            }
                            if (updatetitem.ResolutionExceptionReason != p.ResolutionExceptionReason)
                            {
                                cns.UpdatedBy = uid;
                                cns.UpdatedTime = DateTime.Now;
                                cns.TableName = "Incident";
                                cns.Uniqueid = p.Id;
                                cns.ColumnName = "ResolutionExceptionReason";
                                cns.ColumnValue = p.ResolutionExceptionReason;
                                cns.ClientId = client.ClientId;
                                cns.PreviousValue = updatetitem.ResolutionExceptionReason;
                                unitOfWork.AMSCustomAuditLog.Insert(cns);
                                unitOfWork.Save();
                      //  INUpdateQuery.Query = INUpdateQuery.Query + "ResolutionExceptionReason = '" + p.ResolutionExceptionReason + "',";
                            }
                            if (updatetitem.ResolutionExceptionIndicator != (p.ResolutionExceptionIndicator == true ? "Y" : "N"))
                            {
                                cns.UpdatedBy = uid;
                                cns.UpdatedTime = DateTime.Now;
                                cns.TableName = "Incident";
                                cns.Uniqueid = p.Id;
                                cns.ColumnName = "ResolutionExceptionIndicator";
                                cns.ColumnValue = p.ResolutionExceptionIndicator == true ? "Y" : "N";
                                cns.ClientId = client.ClientId;
                                cns.PreviousValue = updatetitem.ResolutionExceptionIndicator;
                                unitOfWork.AMSCustomAuditLog.Insert(cns);
                               unitOfWork.Save();
                      //  INUpdateQuery.Query = INUpdateQuery.Query + "ResolutionExceptionIndicator ='" + (p.ResolutionExceptionIndicator == true ? "Y" : "N") + "',";
                            }
                            if (updatetitem.Waivedindicator != (p.WaiveSLAReport == true ? "Y" : "N"))
                            {
                                cns.UpdatedBy = uid;
                                cns.UpdatedTime = DateTime.Now;
                                cns.TableName = "Incident";
                                cns.Uniqueid = p.Id;
                                cns.ColumnName = "Waivedindicator";
                                cns.ColumnValue = p.WaiveSLAReport == true ? "Y" : "N";
                                cns.ClientId = client.ClientId;
                                cns.PreviousValue = updatetitem.Waivedindicator;
                                unitOfWork.AMSCustomAuditLog.Insert(cns);
                                unitOfWork.Save();
                       // INUpdateQuery.Query = INUpdateQuery.Query + "Waivedindicator ='" + (p.WaiveSLAReport == true ? "Y" : "N") + "',";
                            }
                            if (updatetitem.WaiveReason != p.WaiveReason)
                            {
                                cns.UpdatedBy = uid;
                                cns.UpdatedTime = DateTime.Now;
                                cns.TableName = "Incident";
                                cns.Uniqueid = p.Id;
                                cns.ColumnName = "WaiveReason";
                                cns.ColumnValue = p.WaiveReason;
                                cns.ClientId = client.ClientId;
                                cns.PreviousValue = updatetitem.WaiveReason;
                                unitOfWork.AMSCustomAuditLog.Insert(cns);
                                unitOfWork.Save();
                      //  INUpdateQuery.Query = INUpdateQuery.Query + "WaiveReason ='" + p.WaiveReason + "',";
                            }

                            updatetitem.ResolutionTimeMetSLA = p.ResolutionTimeMetSLA;
                            updatetitem.ResponseTimeMetSLA = p.ResponseTimeMetSLA;
                            updatetitem.Waivedindicator = p.WaiveSLAReport == true ? "Y" : "N";
                            updatetitem.WaiveReason = p.WaiveReason;
                            updatetitem.ResponseExceptionIndicator = p.ResponseExceptionIndicator == true ? "Y" : "N";
                            updatetitem.ResponseExceptionReason = p.ResponseExceptionReason;
                            updatetitem.ResolutionExceptionReason = p.ResolutionExceptionReason;
                            updatetitem.ResolutionExceptionIndicator = p.ResolutionExceptionIndicator == true ? "Y" : "N";
                            unitOfWork.Incident.Update(updatetitem);
                            unitOfWork.Save();
                    p.ResolutionTimeMetSLA = (p.ResolutionTimeMetSLA==null)? p.ResolutionTimeMetSLA: p.ResolutionTimeMetSLA.Replace("'", "''");
                    p.ResponseTimeMetSLA = (p.ResponseTimeMetSLA == null) ? p.ResponseTimeMetSLA : p.ResponseTimeMetSLA.Replace("'", "''");
                    p.WaiveReason = (p.WaiveReason == null) ? p.WaiveReason : p.WaiveReason.Replace("'", "''");
                    p.ResponseExceptionReason = (p.ResponseExceptionReason == null) ? p.ResponseExceptionReason : p.ResponseExceptionReason.Replace("'", "''");
                    p.ResolutionExceptionReason = (p.ResolutionExceptionReason == null) ? p.ResolutionExceptionReason : p.ResolutionExceptionReason.Replace("'", "''");
                    var temp = unitOfWork.SSIS_INUpdateQuery.Get(G => G.ClientId == ClientId && G.TicketId == p.TicketID).FirstOrDefault();
                                    
                    if (temp== null)
                    {
                        var INUpdateQuery = new SSIS_INUpdateQuery();
                        INUpdateQuery.ClientId = ClientId;
                        INUpdateQuery.Type = "SSISUPDATE";
                        INUpdateQuery.TicketId = p.TicketID;
                        INUpdateQuery.Query = "UPDATE Incident set ResponseExceptionIndicator='" + (p.ResponseExceptionIndicator == true ? "Y" : "N") +
                         "',ResponseExceptionReason='" + p.ResponseExceptionReason +
                         "',ResponseTimeMetSLA='" + p.ResponseTimeMetSLA+
                         "',ResolutionExceptionIndicator='" + (p.ResolutionExceptionIndicator == true ? "Y" : "N") +
                         "',ResolutionExceptionReason='" + p.ResolutionExceptionReason +
                         "',ResolutionTimeMetSLA='" + p.ResolutionTimeMetSLA+
                         "',Waivedindicator='" + (p.WaiveSLAReport == true ? "Y" : "N") +
                         "',WaiveReason='" + p.WaiveReason +
                         //"',clientid = " + ClientId +
                         "' where clientid = " + ClientId + " AND IncidentId = '" + p.TicketID +"'";
                        INUpdateQuery.CreatedBy = uid;
                        INUpdateQuery.CreatedTime = DateTime.Now;
                        unitOfWork.SSIS_INUpdateQuery.Insert(INUpdateQuery);
                       
                    }
                    else
                    {
                        temp.ClientId = ClientId;
                        temp.Type = "SSISUPDATE";
                        temp.TicketId = p.TicketID;
                        temp.Query = "UPDATE Incident set ResponseExceptionIndicator='" + (p.ResponseExceptionIndicator == true ? "Y" : "N") +
                         "',ResponseExceptionReason='" + p.ResponseExceptionReason +
                          "',ResponseTimeMetSLA='" + p.ResponseTimeMetSLA +
                         "',ResolutionExceptionIndicator='" + (p.ResolutionExceptionIndicator == true ? "Y" : "N") +
                         "',ResolutionExceptionReason='" + p.ResolutionExceptionReason +
                          "',ResolutionTimeMetSLA='" + p.ResolutionTimeMetSLA +
                         "',Waivedindicator='" + (p.WaiveSLAReport == true ? "Y" : "N") +
                         "',WaiveReason='" + p.WaiveReason +
                         //"',clientid = " + ClientId +
                         "' where clientid = " + ClientId + " AND IncidentId = '" + p.TicketID+"'";
                        temp.UpdatedBy = uid;
                        temp.UpdatedTime = DateTime.Now;
                        unitOfWork.SSIS_INUpdateQuery.Update(temp);
                    }                    
                    unitOfWork.Save();
            }
            }
            
            return Json(SLAMaintenenceinput.ToDataSourceResult(request, ModelState));
        }
                    catch (Exception ex)
                {
                    Log.Error("TicketsController UpdateTickets , Client ID is " + client.ClientId + " Exception: " + ex);
                }
            return Json(null);

        }


        [HttpPost]
        public ActionResult Export_Tickets_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);


            return File(fileContents, contentType, fileName);
        }

        public ActionResult GetPendingTickets([DataSourceRequest] DataSourceRequest request, DateTime StartDate, DateTime EndDate, string DateType)
        {
            // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            //client.ClientId = 2483;
            IList<SLAMaintenenceModel> SLAMaintenencList = new List<SLAMaintenenceModel>();

            // IEnumerable<Incident> IncidentDetails = null;
            //var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            // var flt = request.Filters.Count;

            if (client.ClientId != 0)
            {

                try
                {
                    SLAMaintenencList = IncidentNavigatorHelper.getPendingTickets(client.ClientId, StartDate, EndDate, DateType);
                    var jsonResult = Json(SLAMaintenencList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = int.MaxValue;
                    return jsonResult;
                }
                catch (Exception ex)
                {
                    Log.Error("TicketsController GetPendingTickets , Client ID is " + client.ClientId+" Exception: " + ex);
                }
            }
            return Json(null);
        }


public ActionResult UpdatePendingTickets([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<SLAMaintenenceModel> SLAMaintenenceinput)
        {
            // var client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            int uid = (int)Session["UserId"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            if (SLAMaintenenceinput != null)
            {
                try 
                { 

                    foreach (var p in SLAMaintenenceinput)
                    {
                        var updatetitem = unitOfWork.Incident.GetByID(p.Id);

                        var cns = new AMSCustomAuditLog();
                        if(updatetitem == null)
                        {
                             if(!p.WaiveSLAReport)
                            {
                                ExcludeOrIncludePendingTickets(p.WaiveSLAReport, ClientId, p.Id, client.UserName, p.WaiveReason);
                            }
                        }
                        else
                        if  (updatetitem.Waivedindicator != (p.WaiveSLAReport == true ? "Y" : "N"))
                        {
                            ExcludeOrIncludePendingTickets(p.WaiveSLAReport,  ClientId, p.Id, client.UserName, p.WaiveReason);
                        }
                    }
                }
                    catch (Exception ex)
                {
                    Log.Error("TicketsController UpdatePendingTickets , Client ID is " + client.ClientId + " Exception: " + ex);
                }
        }

  
            return Json(SLAMaintenenceinput.ToDataSourceResult(request, ModelState));
        }
        
        public void ExcludeOrIncludePendingTickets(bool bExclude, int ClientId, int Id, string Excludedby, string ExcludedReason )
        {
            try { 
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            if (bExclude)
            {
                var paramName = new string[] { "ClientId", "Id", "Excludedby", "ExcludedReason" };
                var paramValue = new object[] { ClientId, Id, Excludedby,  ExcludedReason };
                unitOfWork.ExecProcedure("PendingTicketExclude", paramName, paramValue);
            }
            else
            {
                var paramName = new string[] { "ClientId", "Id" };
                var paramValue = new object[] { ClientId, Id };
                unitOfWork.ExecProcedure("PendingTicketInclude", paramName, paramValue);
                bRunCluster = true;
            }
        }
                    catch (Exception ex)
                {
                    Log.Error("TicketsController ExcludeOrIncludePendingTickets , Client ID is " + ClientId + " Exception: " + ex);
                }

}
[HttpPost]
        public ActionResult Export_PendingTickets_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);


            return File(fileContents, contentType, fileName);
        }

        [HttpPost]
        public ActionResult ExcecuteCreateJobSp()
        {
            var strSuccessmsg = "No need to run cluster jobs as no tickets included";
            int uid = (int)Session["UserId"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];

            try
            { 
            if (!bRunCluster)
                return Json(strSuccessmsg);
              // no need to run as no tickets included
            
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var paramName = new string[] { "ClientId" };
                var paramValue = new object[] { client.ClientId };
                var dt = unitOfWork.GetDataFromStoredProcedure("PY_PythonJobforClassificationTopic ", paramName, paramValue);
                INClusterActivityLog log1 = new INClusterActivityLog();
                log1.ActivityName = "Python ClassificationTopic Job Creation";
                log1.ProgramName = "ClientProfileManager->ConfigurationController->ExcecuteCreateJobSp";
                log1.ClientId = client.ClientId;
                log1.CreatedBy = uid;
                log1.CreatedDt = DateTime.Now;
                unitOfWork.INClusterActivityLog.Insert(log1);
                //unitOfWork.Save();

                //to start deleteCluster
                var paramName1 = new string[] { "job_name " };
                string jonname = "PY_TopicJob" + client.ClientId.ToString().Trim();
                var paramValue1 = new object[] { jonname };
                var dt1 = unitOfWork.GetDataFromStoredProcedure("msdb.dbo.sp_start_job", paramName1, paramValue1);
                INClusterActivityLog log12 = new INClusterActivityLog();
                log12.ActivityName = "Cluster Data Detele Job";
                log12.ProgramName = "ClientProfileManager->ConfigurationController->ExcecuteCreateJobSp";
                log12.ClientId = client.ClientId;
                log12.CreatedBy = uid;
                log12.CreatedDt = DateTime.Now;
                unitOfWork.INClusterActivityLog.Insert(log12);
                //unitOfWork.Save();

                //to start ClusterJob
                var paramName2 = new string[] { "job_name " };
                string jonname2 = "PY_StartClassificationJob" + client.ClientId.ToString().Trim();
                var paramValue2 = new object[] { jonname2 };
                var dt2 = unitOfWork.GetDataFromStoredProcedure("msdb.dbo.sp_start_job", paramName2, paramValue2);
                INClusterActivityLog log123 = new INClusterActivityLog();
                log123.ActivityName = "Cluster Data Start Job";
                log123.ProgramName = "ClientProfileManager->ConfigurationController->ExcecuteCreateJobSp";
                log123.ClientId = client.ClientId;
                log123.CreatedBy = uid;
                log123.CreatedDt = DateTime.Now;
                unitOfWork.INClusterActivityLog.Insert(log123);
                unitOfWork.Save();

                strSuccessmsg = "Successfully invoked the ‘Cluster classification’ job, which is in progress. Please check the re-clustered incident data after some time.";
                TempData.Add("ValidationSuccessMsg", strSuccessmsg);
                bRunCluster = false;

                return Json(strSuccessmsg);

            }
        }
                    catch (Exception ex)
                {
                    Log.Error("TicketsController ExcecuteCreateJobSp , Client ID is " + client.ClientId + " Exception: " + ex);
                }
            return Json(null);

        }

    }
}
















