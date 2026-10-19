using Data;
using System.Data;
using Domain.Abstract;
using WebUI.Areas.ClientProfileManager.Dto;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Areas.IncidentNavigator.Dto;
using NTTData.AMOPortal.WebUI.Mapper;
using WebUI.Dto;
using WebUI.Models;
using NTTData.AMOPortal.Domain.Service;
using System.IO;
using System.Text;
using WebUI.Areas.IncidentNavigator.Models;

using Domain.Entities.IncidentNavigator;
using PagedList;
using PagedList.Mvc;
using System.Web.Mvc;
using WebUI.Areas.IncidentNavigator.AspxReport;
using System.Reflection;
using WebUI.Areas.ClientProfileManager.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Text.RegularExpressions;
using log4net;

namespace WebUI.Areas.IncidentNavigator
{
    public class IncidentNavigatorHelper
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(IncidentNavigatorHelper));

        public static IList<ClusterlistModel> INGetDistinctClusters(int ClientId, string TicketType, string ConfigItemName)
        {
            IList<ClusterlistModel> Clusterlist = new List<ClusterlistModel>();
            var paramName = new string[] { "ClientId", "TicketType", "ConfigItemName" };
            var paramValue = new object[] { ClientId, TicketType, ConfigItemName };


            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("INGetDistinctClusters", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new ClusterlistModel();


                    coldet.ClusterLablefromCluster = dr["ClusterLablefromCluster"].ToString();
                    coldet.imagePerCluster = dr["imagePerCluster"].ToString();

                    try
                    {
                        coldet.ClusterIDfromCluster = Convert.ToInt32(dr["ClusterIDfromCluster"]);
                    }

                    catch (Exception ex)
                    {
                        Log.Error("IncidentNavigatorHelper error 'INGetDistinctClusters' ", ex);
                    }
                    Clusterlist.Add(coldet);
                }

            }
            return Clusterlist;
        }
        public static IList<ClusterlistModel> INGetDistinctClustersForWordCloud(int ClientId)
        {
            IList<ClusterlistModel> Clusterlist = new List<ClusterlistModel>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };


            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("INGetDistinctClusters_Test", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new ClusterlistModel();


                    coldet.ClusterLablefromCluster = dr["ClusterLablefromCluster"].ToString();
                    coldet.imagePerCluster = dr["imagePerCluster"].ToString();

                    try
                    {
                        coldet.ClusterIDfromCluster = Convert.ToInt32(dr["ClusterIDfromCluster"]);
                    }

                    catch (Exception ex)
                    {
                        Log.Error("IncidentNavigatorHelper error 'INGetDistinctClustersForWordCloud' ", ex);
                    }
                    Clusterlist.Add(coldet);
                }

            }
            return Clusterlist;
        }
        public static IEnumerable<IncidentsClusterlistModel> INGetIncidentClusterlistforWordCloud(int ClientId, int ClusterId)
        {
            IList<IncidentsClusterlistModel> IncidentClusterlist = new List<IncidentsClusterlistModel>();
            var paramName = new string[] { "ClientId", "ClusterId" };
            var paramValue = new object[] { ClientId, ClusterId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("INGetIncidentClusterlist_test", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new IncidentsClusterlistModel();


                    coldet.ClusterLablefromCluster = dr["ClusterLablefromCluster"].ToString();
                    coldet.Incidentid = dr["Incidentid"].ToString();

                    coldet.AffectedItemName = dr["AffectedItemName"].ToString();
                    coldet.ClosureCategory = dr["ClosureCategory"].ToString();
                    coldet.DescriptionfromIncident = dr["DescriptionfromIncident"].ToString();
                    coldet.elapsedtime = dr["elapsedtime"].ToString();
                    coldet.Priority = dr["Priority"].ToString();
                    coldet.Severity = dr["Severity"].ToString();
                    coldet.Assignedtosupportgroup = dr["Assignedtosupportgroup"].ToString();
                    coldet.Type = dr["Type"].ToString();
                    coldet.ConfigItemName = dr["ConfigItemName"].ToString();
                    if (!(dr["ClosedDateTime"] is DBNull))
                        coldet.ClosedDateTime = Convert.ToDateTime(dr["ClosedDateTime"]);
                    if (!(dr["CreatedDateTime"] is DBNull))
                        coldet.CreatedDateTime = Convert.ToDateTime(dr["CreatedDateTime"]);
                    if (!(dr["ResolutionDateTime"] is DBNull))
                        coldet.ResolutionDateTime = Convert.ToDateTime(dr["ResolutionDateTime"]);
                    if (!(dr["ResolutionDescription"] is DBNull))
                        coldet.ResolutionDescription = dr["ResolutionDescription"].ToString();
                    //coldet.imagePerCluster = dr["imagePerCluster"].ToString();
                    try
                    {
                        coldet.PrimaryKeyIncidentId = Convert.ToInt32(dr["PrimaryKeyIncidentId"]);
                        coldet.ClusterIDfromCluster = Convert.ToInt32(dr["ClusterIDfromCluster"]);
                        coldet.Resolutiontime = Convert.ToInt32(dr["Resolutiontime"]);
                    }

                    catch (Exception ex)
                    {
                        Log.Error("IncidentNavigatorHelper error 'INGetIncidentClusterlistforWordCloud' ", ex);
                    }
                    IncidentClusterlist.Add(coldet);
                }
                return IncidentClusterlist;
            }
        }

        public static IEnumerable<IncidentsClusterlistModel> INGetIncidentClusterlist(int ClientId, string TicketType, string ConfigItemName, int ClusterId)
        {
            IList<IncidentsClusterlistModel> IncidentClusterlist = new List<IncidentsClusterlistModel>();
            var paramName = new string[] { "ClientId", "TicketType", "ConfigItemName", "ClusterId" };
            var paramValue = new object[] { ClientId, TicketType, ConfigItemName, ClusterId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("INGetIncidentClusterlist", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new IncidentsClusterlistModel();


                    coldet.ClusterLablefromCluster = dr["ClusterLablefromCluster"].ToString();
                    coldet.Incidentid = dr["Incidentid"].ToString();

                    coldet.AffectedItemName = dr["AffectedItemName"].ToString();
                    coldet.ClosureCategory = dr["ClosureCategory"].ToString();
                    coldet.DescriptionfromIncident = dr["DescriptionfromIncident"].ToString();
                    coldet.elapsedtime = dr["elapsedtime"].ToString();
                    coldet.Priority = dr["Priority"].ToString();
                    coldet.Severity = dr["Severity"].ToString();
                    coldet.Assignedtosupportgroup = dr["Assignedtosupportgroup"].ToString();
                    //coldet.imagePerCluster = dr["imagePerCluster"].ToString();
                    try
                    {

                        coldet.ClusterIDfromCluster = Convert.ToInt32(dr["ClusterIDfromCluster"]);
                        coldet.Resolutiontime = Convert.ToInt32(dr["Resolutiontime"]);
                        coldet.PrimaryKeyIncidentId = Convert.ToInt32(dr["PrimaryKeyIncidentId"]);
                    }

                    catch (Exception ex)
                    {
                        Log.Error("IncidentNavigatorHelper error 'INGetIncidentClusterlist' ", ex);
                    }
                    IncidentClusterlist.Add(coldet);
                }
                return IncidentClusterlist;
            }
        }

        public static IList<ClusterReportInput> INGetClusterReportlist(int ClientId)
        {
            IList<ClusterReportInput> Clusterlist = new List<ClusterReportInput>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };


            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("INGetClusterAvgReport", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new ClusterReportInput();


                    coldet.Configitemname = dr["Configitemname"].ToString();
                    coldet.ClusterLabel = dr["ClusterLabel"].ToString();

                    coldet.Type = dr["Type"].ToString();

                    try
                    {
                        coldet.TotalIncidents = Convert.ToInt32(dr["TotalIncidents"]);
                        coldet.AvgResolutionTime = Convert.ToDecimal(dr["AvgResolutionTime"]);
                        coldet.AvgCostPerTicket = Convert.ToDecimal(dr["AvgSLACostPerTicket"]);

                    }

                    catch (Exception ex)
                    {
                        Log.Error("IncidentNavigatorHelper error 'INGetClusterReportlist' ", ex);
                    }
                    Clusterlist.Add(coldet);
                }

            }
            return Clusterlist;
        }

        public static List<INTrendingReportEntity> INTrendingReportListRange(int ClientId, DateTime fromDate, DateTime toDate, string dateType)
        {

            //  IList<TrendingReportInput> Tredingdata = new List<TrendingReportInput>();
            TimeSpan ts = new TimeSpan(23, 59, 59);
            DateTime newDate = toDate.Add(ts);
            toDate = newDate;
            List<INTrendingReportEntity> data = new List<INTrendingReportEntity>();

            switch (dateType)
            {
                case "CreatedDateTime":
                    {
                        data = getCreatedDateResult(ClientId, fromDate, toDate);
                        break;
                    }

                case "ClosedDateTime":
                    {
                        data = getClosedDateResult(ClientId, fromDate, toDate);
                        break;
                    }

                case "LastUpdatedDateTime":
                    {
                        data = getLastUpdatedDateResult(ClientId, fromDate, toDate);
                        break;
                    }

                case "ResolutionDateTime":
                    {
                        data = getResolutionDateResult(ClientId, fromDate, toDate);
                        break;
                    }

            }

            return data;
        }
        public static List<INTrendingReportEntity> getCreatedDateResult(int ClientId, DateTime StartDate, DateTime EndDate)
        {
            Db db = new Db();
            var _dt = (from t in db.Incidents
                       join acc in db.ClientAccountSetup on t.AccountId equals acc.Id
                       join contract in db.ClientAccountContract on t.ContractId equals contract.Id
                       where t.ClientId == ClientId
                      && (t.CreatedDateTime >= StartDate && t.CreatedDateTime <= EndDate)

                       select new INTrendingReportEntity
                       {
                           ID = t.Id,
                           Clientid = t.ClientId,
                           Incidentid = t.IncidentId,
                           TicketId = t.IncidentId,
                           Createddatetime = t.CreatedDateTime,
                           Status = t.Status,
                           Title = t.Title,
                           Priority = t.Priority,
                           Urgency = t.Urgency,
                           Type = t.Type,
                           Outagetype = t.OutageType,
                           Divisionimpacted = t.DivisionImpacted,
                           Businessunitimpacted = t.BusinessUnitImpacted,
                           Servicecategory = t.ServiceCategory,
                           Servicesubcategory = t.ServiceSubCategory,
                           Servicearea = t.ServiceArea,
                           Regionimpacted = t.RegionImpacted,
                           Locationsimpacted = t.LocationsImpacted,
                           Customerdept = t.CustomerDept,
                           Customerlastname = t.CustomerLastname,
                           Customerfirstname = t.CustomerFirstname,
                           Customertitle = t.CustomerTitle,
                           Customersatisfaction = t.CustomerSatisfaction,
                           Impacttype = t.ImpactType,
                           Configitemid = t.ConfigItemId,
                           Configtype = t.ConfigType,
                           Configitemname = t.ConfigItemName,
                           Environmentimpacted = t.EnvironmentImpacted,
                           Nodename = t.NodeName,
                           Rootcausetype = t.RootCauseType,
                           Rootcausedescription = t.RootcauseDescription,
                           Resolutiontype = t.ResolutionType,
                           Solutionid = t.SolutionId,
                           Deploymentrelease = t.DeploymentRelease,
                           Relatedchangeids = t.RelatedChangeIds,
                           Relatedids = t.RelatedIds,
                           Relatedproblemids = t.RelatedProblemids,
                           Helpdesk = t.Helpdesk,
                           Assignedtoperson = t.AssignedToPerson,
                           Assignedtosupportgroup = t.AssignedToSupportGroup,
                           Createdbygroup = t.CreatedByGroup,
                           Createdbyperson = t.CreatedByPerson,
                           Createdtimezone = t.CreatedTimezone,
                           Createdtime = t.CreatedTime,
                           Createdweekday = t.CreatedWeekday,
                           Createdweeknum = t.CreatedWeeknum,
                           Createdmonth = t.CreatedMonth,
                           Createdyear = t.CreatedYear,
                           Acknowlededbygroup = t.AcknowlededByGroup,
                           Acknowlededbyperson = t.AcknowlededByPerson,
                           Acknowledgeddatetime = t.AcknowledgedDateTime,
                           Acknowledgedtimezone = t.AcknowledgedTimeZone,
                           Acknowledgedtime = t.AcknowledgedTime,
                           Acknowledgedweekday = t.Acknowledgedweekday,
                           Acknowledgedweeknum = t.Acknowledgedweeknum,
                           Acknowledgedmonth = t.Acknowledgedmonth,
                           Acknowledgedyear = t.Acknowledgedyear,
                           Resolvedbygroup = t.ResolvedByGroup,
                           Resolvedbyperson = t.Resolvedbyperson,
                           Resolvedbyvendor = t.ResolvedByVendor,
                           Resolutionefforthours = t.ResolutionEfforthours,
                           Resolutiondatetime = t.ResolutionDateTime,
                           Resolvedtimezone = t.ResolvedTimezone,
                           Resolutiontime = t.ResolutionTime,
                           Resolutionweekday = t.ResolutionWeekday,
                           Resolutionweeknum = t.ResolutionWeeknum,
                           Servicerestoredbygroup = t.ServiceRestoredByGroup,
                           Servicerestoredbyperson = t.ServiceRestoredbyperson,
                           Servicerestoreddatetime = t.ServiceRestoredDateTime,
                           Servicerestoredtimezone = t.ServiceRestoredTimeZone,
                           Servicerestoredtime = t.ServiceRestoredTime,
                           Servicerestoredweekday = t.ServiceRestoredWeekday,
                           Servicerestoredweeknum = t.ServiceRestoredWeeknum,
                           Servicerestoredmonth = t.ServiceRestoredMonth,
                           Servicerestoredyear = t.ServiceRestoredYear,
                           Lastupdatedbyperson = t.LastUpdatedByPerson,
                           Lastupdateddatetime = t.LastUpdatedDateTime,
                           Datasource = t.DataSource,
                           Datasourcenotes = t.DataSourceNotes,
                           Assignedbygroup = t.AssignedByGroup,
                           Assignedbyperson = t.AssignedByPerson,
                           Assigneddatetime = t.AssignedDateTime,
                           Esitmatedresponsetimebygroup = "NA",
                           Esitmatedresponsedatetime = t.EsitmatedResponseDateTime,
                           Resolutionyear = t.ResolutionYear,
                           Resolutionmonth = t.ResolutionMonth,
                           Waivedindicator = t.Waivedindicator,
                           Esitmatedresolutiondate = t.EsitmatedResolutionDate,
                           Totalsuspenddurationassignedtoack = t.TotalSuspendDurationAssignedtoack,
                           Totalsuspenddurationacktoer = t.TotalSuspendDurationacktoer,
                           Totalsuspenddurationertoresolution = t.TotalSuspendDurationertoreSolution,
                           Resolvedindicator = t.Resolvedindicator,
                           IsNTTGroup = t.IsNTTGroup,
                           SupportGroupType = t.SupportGroupType,
                           InProgressdatetime = t.InProgressDateTime,
                           Closeddatetime = t.ClosedDateTime,
                           ScheduledstartdateTime = t.ScheduledStartDateTime,
                           ScheduledenddateTime = t.ScheduledEndDateTime,
                           Scheduledchangecount = t.ScheduledChangeCount,
                           Company = t.Company,
                           Site = t.Site,
                           Assigneecompany = t.AssigneeCompany,
                           Assigneeorganization = t.AssigneeOrganization,
                           Estimatedworkeffort = t.EstimatedWorkEffort,
                           Actualworkeffort = t.ActualWorkEffort,
                           ActualstartdateTime = t.ActualStartDateTime,
                           Recyclecount = t.Recyclecount,
                           Statusreason = t.StatusReason,
                           ConfigItemCategory = t.ConfigItemCategory,
                           ResponseTime = t.ResponseTime,
                           ResponseTimeMetSLA = t.ResponseTimeMetSLA,
                           ResolutionTimeMetSLA = t.ResolutionTimeMetSLA,
                           EstimateResponseTime = t.EstimateResponseTime,
                           ClosureCategory = t.ClosureCategory,
                           EstimatedResponseTimeMetSLA = t.EstimatedResponseTimeMetSLA,
                           EstResptarget = t.EstResptarget,
                           Reslntargettime = t.ReslntargetTime,
                           ResolutionExceptionIndicator = t.ResolutionExceptionIndicator,
                           ResolutionExceptionReason = t.ResolutionExceptionReason,
                           ResponseExceptionIndicator = t.ResponseExceptionIndicator,
                           ResponseExceptionReason = t.ResponseExceptionReason,
                           Resptargettime = t.ResptargetTime,
                           Severity = t.Severity,
                           WorkGroupLevel = t.WorkGroupLevel,
                           AffectedItemName = t.AffectedItemName,
                           SLACostPerTicket = t.SLACostPerTicket,
                           INResolutiontime = t.INResolutionTime,
                           INCostPerTicket = t.INCostPerTicket,
                           //  AccountName = string.IsNullOrEmpty(dr.AccountName) ? "NA" : dr.AccountName,
                           //  ContractName = string.IsNullOrEmpty(dr.ContractName) ? "NA" : dr.ContractName
                           AccountName = acc.AccountName,
                           ContractName = contract.ContractName,
                           //newly added by DP
                           Active = t.Active,
                           Activityduedatetime = t.Activityduedatetime,
                           ActualenddateTime = t.ActualenddateTime,
                           Additionalassigneelist = t.Additionalassigneelist,
                           Approval = t.Approval,
                           Approvalhistory = t.Approvalhistory,
                           Approvalsetdatetime = t.Approvalsetdatetime,
                           Businessduration = t.Businessduration,
                           Closenotes = t.Closenotes,
                           Closedby = t.Closedby,
                           Commentsworknotes = t.Commentsworknotes,
                           Correlationdisplay = t.Correlationdisplay,
                           CorrelationID = t.CorrelationID,
                           Deliveryplan = t.Deliveryplan,
                           Deliverytask = t.Deliverytask,
                           Domain = t.Domain,
                           Domainpath = t.Domainpath,
                           Escalation = t.Escalation,
                           Followupdatetime = t.Followupdatetime,
                           Grouplist = t.Grouplist,
                           Knowledge = t.Knowledge,
                           Location = t.Location,
                           MadeSLA = t.MadeSLA,
                           Openeddatetime = t.Openeddatetime,
                           Openedby = t.Openedby,
                           Order = t.Order,
                           ParentticketID = t.ParentticketID,
                           Reassignmentcount = t.Reassignmentcount,
                           SLAduedatetime = t.SLAduedatetime,
                           Tags = t.Tags,
                           Timeworked = t.Timeworked,
                           Updatescount = t.Updatescount,
                           Uponapprovalstep = t.Uponapprovalstep,
                           Uponrejectstep = t.Uponrejectstep,
                           Userinput = t.Userinput,
                           Watchlist = t.Watchlist,
                           Worknotes = t.Worknotes,
                           Worknoteslist = t.Worknoteslist,
                           Businessresolvetime = t.Businessresolvetime,
                           Caller = t.Caller,
                           Causedbychange = t.Causedbychange,
                           ChildIncidentscount = t.ChildIncidentscount,
                           Incidentstate = t.Incidentstate,
                           Lastreopenedatdatetime = t.Lastreopenedatdatetime,
                           Lastreopenedby = t.Lastreopenedby,
                           Notify = t.Notify,
                           ParentIncidentID = t.ParentIncidentID,
                           TotalResolvetime = t.TotalResolvetime,
                           Knownerror = t.Knownerror,
                           Majorproblem = t.Majorproblem,
                           Problemstate = t.Problemstate,
                           RelatedIncidentscount = t.RelatedIncidentscount,
                           Workaround = t.Workaround,
                           Backoutplan = t.Backoutplan,
                           CABdatetime = t.CABdatetime,
                           CABdelegate = t.CABdelegate,
                           CABrecommendation = t.CABrecommendation,
                           CABrequired = t.CABrequired,
                           Changeplan = t.Changeplan,
                           Closecode = t.Closecode,
                           Conflictlastrundatetime = t.Conflictlastrundatetime,
                           Conflictstatus = t.Conflictstatus,
                           Implementationplan = t.Implementationplan,
                           Onhold = t.Onhold,
                           OnHoldChangeTasks = t.OnHoldChangeTasks,
                           Outsidemaintenanceschedule = t.Outsidemaintenanceschedule,
                           Phase = t.Phase,
                           Phasestate = t.Phasestate,
                           Productionsystem = t.Productionsystem,
                           Reason = t.Reason,
                           Requestedby = t.Requestedby,
                           Requestedbydatetime = t.Requestedbydatetime,
                           Reviewcomments = t.Reviewcomments,
                           Reviewdatetime = t.Reviewdatetime,
                           Reviewstatus = t.Reviewstatus,
                           Risk = t.Risk,
                           Changerequestscope = t.Changerequestscope,
                           Changerequesttype = t.Changerequesttype,
                           Tower = t.Tower,
                           ClientPriority = t.clientpriority
                       }).ToList();
            return _dt;

        }
        public static List<INTrendingReportEntity> getClosedDateResult(int ClientId, DateTime StartDate, DateTime EndDate)
        {
            Db db = new Db();
            var _dt = (from t in db.Incidents
                       join acc in db.ClientAccountSetup on t.AccountId equals acc.Id
                       join contract in db.ClientAccountContract on t.ContractId equals contract.Id
                       where t.ClientId == ClientId
                      && (t.ClosedDateTime >= StartDate && t.ClosedDateTime <= EndDate)

                       select new INTrendingReportEntity
                       {
                           ID = t.Id,
                           Clientid = t.ClientId,
                           Incidentid = t.IncidentId,
                           TicketId = t.IncidentId,
                           Createddatetime = t.CreatedDateTime,
                           Status = t.Status,
                           Title = t.Title,
                           Priority = t.Priority,
                           Urgency = t.Urgency,
                           Type = t.Type,
                           Outagetype = t.OutageType,
                           Divisionimpacted = t.DivisionImpacted,
                           Businessunitimpacted = t.BusinessUnitImpacted,
                           Servicecategory = t.ServiceCategory,
                           Servicesubcategory = t.ServiceSubCategory,
                           Servicearea = t.ServiceArea,
                           Regionimpacted = t.RegionImpacted,
                           Locationsimpacted = t.LocationsImpacted,
                           Customerdept = t.CustomerDept,
                           Customerlastname = t.CustomerLastname,
                           Customerfirstname = t.CustomerFirstname,
                           Customertitle = t.CustomerTitle,
                           Customersatisfaction = t.CustomerSatisfaction,
                           Impacttype = t.ImpactType,
                           Configitemid = t.ConfigItemId,
                           Configtype = t.ConfigType,
                           Configitemname = t.ConfigItemName,
                           Environmentimpacted = t.EnvironmentImpacted,
                           Nodename = t.NodeName,
                           Rootcausetype = t.RootCauseType,
                           Rootcausedescription = t.RootcauseDescription,
                           Resolutiontype = t.ResolutionType,
                           Solutionid = t.SolutionId,
                           Deploymentrelease = t.DeploymentRelease,
                           Relatedchangeids = t.RelatedChangeIds,
                           Relatedids = t.RelatedIds,
                           Relatedproblemids = t.RelatedProblemids,
                           Helpdesk = t.Helpdesk,
                           Assignedtoperson = t.AssignedToPerson,
                           Assignedtosupportgroup = t.AssignedToSupportGroup,
                           Createdbygroup = t.CreatedByGroup,
                           Createdbyperson = t.CreatedByPerson,
                           Createdtimezone = t.CreatedTimezone,
                           Createdtime = t.CreatedTime,
                           Createdweekday = t.CreatedWeekday,
                           Createdweeknum = t.CreatedWeeknum,
                           Createdmonth = t.CreatedMonth,
                           Createdyear = t.CreatedYear,
                           Acknowlededbygroup = t.AcknowlededByGroup,
                           Acknowlededbyperson = t.AcknowlededByPerson,
                           Acknowledgeddatetime = t.AcknowledgedDateTime,
                           Acknowledgedtimezone = t.AcknowledgedTimeZone,
                           Acknowledgedtime = t.AcknowledgedTime,
                           Acknowledgedweekday = t.Acknowledgedweekday,
                           Acknowledgedweeknum = t.Acknowledgedweeknum,
                           Acknowledgedmonth = t.Acknowledgedmonth,
                           Acknowledgedyear = t.Acknowledgedyear,
                           Resolvedbygroup = t.ResolvedByGroup,
                           Resolvedbyperson = t.Resolvedbyperson,
                           Resolvedbyvendor = t.ResolvedByVendor,
                           Resolutionefforthours = t.ResolutionEfforthours,
                           Resolutiondatetime = t.ResolutionDateTime,
                           Resolvedtimezone = t.ResolvedTimezone,
                           Resolutiontime = t.ResolutionTime,
                           Resolutionweekday = t.ResolutionWeekday,
                           Resolutionweeknum = t.ResolutionWeeknum,
                           Servicerestoredbygroup = t.ServiceRestoredByGroup,
                           Servicerestoredbyperson = t.ServiceRestoredbyperson,
                           Servicerestoreddatetime = t.ServiceRestoredDateTime,
                           Servicerestoredtimezone = t.ServiceRestoredTimeZone,
                           Servicerestoredtime = t.ServiceRestoredTime,
                           Servicerestoredweekday = t.ServiceRestoredWeekday,
                           Servicerestoredweeknum = t.ServiceRestoredWeeknum,
                           Servicerestoredmonth = t.ServiceRestoredMonth,
                           Servicerestoredyear = t.ServiceRestoredYear,
                           Lastupdatedbyperson = t.LastUpdatedByPerson,
                           Lastupdateddatetime = t.LastUpdatedDateTime,
                           Datasource = t.DataSource,
                           Datasourcenotes = t.DataSourceNotes,
                           Assignedbygroup = t.AssignedByGroup,
                           Assignedbyperson = t.AssignedByPerson,
                           Assigneddatetime = t.AssignedDateTime,
                           Esitmatedresponsetimebygroup = "NA",
                           Esitmatedresponsedatetime = t.EsitmatedResponseDateTime,
                           Resolutionyear = t.ResolutionYear,
                           Resolutionmonth = t.ResolutionMonth,
                           Waivedindicator = t.Waivedindicator,
                           Esitmatedresolutiondate = t.EsitmatedResolutionDate,
                           Totalsuspenddurationassignedtoack = t.TotalSuspendDurationAssignedtoack,
                           Totalsuspenddurationacktoer = t.TotalSuspendDurationacktoer,
                           Totalsuspenddurationertoresolution = t.TotalSuspendDurationertoreSolution,
                           Resolvedindicator = t.Resolvedindicator,
                           IsNTTGroup = t.IsNTTGroup,
                           SupportGroupType = t.SupportGroupType,
                           InProgressdatetime = t.InProgressDateTime,
                           Closeddatetime = t.ClosedDateTime,
                           ScheduledstartdateTime = t.ScheduledStartDateTime,
                           ScheduledenddateTime = t.ScheduledEndDateTime,
                           Scheduledchangecount = t.ScheduledChangeCount,
                           Company = t.Company,
                           Site = t.Site,
                           Assigneecompany = t.AssigneeCompany,
                           Assigneeorganization = t.AssigneeOrganization,
                           Estimatedworkeffort = t.EstimatedWorkEffort,
                           Actualworkeffort = t.ActualWorkEffort,
                           ActualstartdateTime = t.ActualStartDateTime,
                           Recyclecount = t.Recyclecount,
                           Statusreason = t.StatusReason,
                           ConfigItemCategory = t.ConfigItemCategory,
                           ResponseTime = t.ResponseTime,
                           ResponseTimeMetSLA = t.ResponseTimeMetSLA,
                           ResolutionTimeMetSLA = t.ResolutionTimeMetSLA,
                           EstimateResponseTime = t.EstimateResponseTime,
                           ClosureCategory = t.ClosureCategory,
                           EstimatedResponseTimeMetSLA = t.EstimatedResponseTimeMetSLA,
                           EstResptarget = t.EstResptarget,
                           Reslntargettime = t.ReslntargetTime,
                           ResolutionExceptionIndicator = t.ResolutionExceptionIndicator,
                           ResolutionExceptionReason = t.ResolutionExceptionReason,
                           ResponseExceptionIndicator = t.ResponseExceptionIndicator,
                           ResponseExceptionReason = t.ResponseExceptionReason,
                           Resptargettime = t.ResptargetTime,
                           Severity = t.Severity,
                           WorkGroupLevel = t.WorkGroupLevel,
                           AffectedItemName = t.AffectedItemName,
                           SLACostPerTicket = t.SLACostPerTicket,
                           INResolutiontime = t.INResolutionTime,
                           INCostPerTicket = t.INCostPerTicket,
                           //  AccountName = string.IsNullOrEmpty(dr.AccountName) ? "NA" : dr.AccountName,
                           //  ContractName = string.IsNullOrEmpty(dr.ContractName) ? "NA" : dr.ContractName
                           AccountName = acc.AccountName,
                           ContractName = contract.ContractName,
                           //newly added by DP
                           Active = t.Active,
                           Activityduedatetime = t.Activityduedatetime,
                           ActualenddateTime = t.ActualenddateTime,
                           Additionalassigneelist = t.Additionalassigneelist,
                           Approval = t.Approval,
                           Approvalhistory = t.Approvalhistory,
                           Approvalsetdatetime = t.Approvalsetdatetime,
                           Businessduration = t.Businessduration,
                           Closenotes = t.Closenotes,
                           Closedby = t.Closedby,
                           Commentsworknotes = t.Commentsworknotes,
                           Correlationdisplay = t.Correlationdisplay,
                           CorrelationID = t.CorrelationID,
                           Deliveryplan = t.Deliveryplan,
                           Deliverytask = t.Deliverytask,
                           Domain = t.Domain,
                           Domainpath = t.Domainpath,
                           Escalation = t.Escalation,
                           Followupdatetime = t.Followupdatetime,
                           Grouplist = t.Grouplist,
                           Knowledge = t.Knowledge,
                           Location = t.Location,
                           MadeSLA = t.MadeSLA,
                           Openeddatetime = t.Openeddatetime,
                           Openedby = t.Openedby,
                           Order = t.Order,
                           ParentticketID = t.ParentticketID,
                           Reassignmentcount = t.Reassignmentcount,
                           SLAduedatetime = t.SLAduedatetime,
                           Tags = t.Tags,
                           Timeworked = t.Timeworked,
                           Updatescount = t.Updatescount,
                           Uponapprovalstep = t.Uponapprovalstep,
                           Uponrejectstep = t.Uponrejectstep,
                           Userinput = t.Userinput,
                           Watchlist = t.Watchlist,
                           Worknotes = t.Worknotes,
                           Worknoteslist = t.Worknoteslist,
                           Businessresolvetime = t.Businessresolvetime,
                           Caller = t.Caller,
                           Causedbychange = t.Causedbychange,
                           ChildIncidentscount = t.ChildIncidentscount,
                           Incidentstate = t.Incidentstate,
                           Lastreopenedatdatetime = t.Lastreopenedatdatetime,
                           Lastreopenedby = t.Lastreopenedby,
                           Notify = t.Notify,
                           ParentIncidentID = t.ParentIncidentID,
                           TotalResolvetime = t.TotalResolvetime,
                           Knownerror = t.Knownerror,
                           Majorproblem = t.Majorproblem,
                           Problemstate = t.Problemstate,
                           RelatedIncidentscount = t.RelatedIncidentscount,
                           Workaround = t.Workaround,
                           Backoutplan = t.Backoutplan,
                           CABdatetime = t.CABdatetime,
                           CABdelegate = t.CABdelegate,
                           CABrecommendation = t.CABrecommendation,
                           CABrequired = t.CABrequired,
                           Changeplan = t.Changeplan,
                           Closecode = t.Closecode,
                           Conflictlastrundatetime = t.Conflictlastrundatetime,
                           Conflictstatus = t.Conflictstatus,
                           Implementationplan = t.Implementationplan,
                           Onhold = t.Onhold,
                           OnHoldChangeTasks = t.OnHoldChangeTasks,
                           Outsidemaintenanceschedule = t.Outsidemaintenanceschedule,
                           Phase = t.Phase,
                           Phasestate = t.Phasestate,
                           Productionsystem = t.Productionsystem,
                           Reason = t.Reason,
                           Requestedby = t.Requestedby,
                           Requestedbydatetime = t.Requestedbydatetime,
                           Reviewcomments = t.Reviewcomments,
                           Reviewdatetime = t.Reviewdatetime,
                           Reviewstatus = t.Reviewstatus,
                           Risk = t.Risk,
                           Changerequestscope = t.Changerequestscope,
                           Changerequesttype = t.Changerequesttype,
                           Tower = t.Tower,
                           ClientPriority = t.clientpriority
                       }).ToList();
            return _dt;
        }
        public static List<INTrendingReportEntity> getLastUpdatedDateResult(int ClientId, DateTime StartDate, DateTime EndDate)
        {
            Db db = new Db();
            var _dt = (from t in db.Incidents
                       join acc in db.ClientAccountSetup on t.AccountId equals acc.Id
                       join contract in db.ClientAccountContract on t.ContractId equals contract.Id
                       where t.ClientId == ClientId
                      && (t.LastUpdatedDateTime >= StartDate && t.LastUpdatedDateTime <= EndDate)

                       select new INTrendingReportEntity
                       {
                           ID = t.Id,
                           Clientid = t.ClientId,
                           Incidentid = t.IncidentId,
                           TicketId = t.IncidentId,
                           Createddatetime = t.CreatedDateTime,
                           Status = t.Status,
                           Title = t.Title,
                           Priority = t.Priority,
                           Urgency = t.Urgency,
                           Type = t.Type,
                           Outagetype = t.OutageType,
                           Divisionimpacted = t.DivisionImpacted,
                           Businessunitimpacted = t.BusinessUnitImpacted,
                           Servicecategory = t.ServiceCategory,
                           Servicesubcategory = t.ServiceSubCategory,
                           Servicearea = t.ServiceArea,
                           Regionimpacted = t.RegionImpacted,
                           Locationsimpacted = t.LocationsImpacted,
                           Customerdept = t.CustomerDept,
                           Customerlastname = t.CustomerLastname,
                           Customerfirstname = t.CustomerFirstname,
                           Customertitle = t.CustomerTitle,
                           Customersatisfaction = t.CustomerSatisfaction,
                           Impacttype = t.ImpactType,
                           Configitemid = t.ConfigItemId,
                           Configtype = t.ConfigType,
                           Configitemname = t.ConfigItemName,
                           Environmentimpacted = t.EnvironmentImpacted,
                           Nodename = t.NodeName,
                           Rootcausetype = t.RootCauseType,
                           Rootcausedescription = t.RootcauseDescription,
                           Resolutiontype = t.ResolutionType,
                           Solutionid = t.SolutionId,
                           Deploymentrelease = t.DeploymentRelease,
                           Relatedchangeids = t.RelatedChangeIds,
                           Relatedids = t.RelatedIds,
                           Relatedproblemids = t.RelatedProblemids,
                           Helpdesk = t.Helpdesk,
                           Assignedtoperson = t.AssignedToPerson,
                           Assignedtosupportgroup = t.AssignedToSupportGroup,
                           Createdbygroup = t.CreatedByGroup,
                           Createdbyperson = t.CreatedByPerson,
                           Createdtimezone = t.CreatedTimezone,
                           Createdtime = t.CreatedTime,
                           Createdweekday = t.CreatedWeekday,
                           Createdweeknum = t.CreatedWeeknum,
                           Createdmonth = t.CreatedMonth,
                           Createdyear = t.CreatedYear,
                           Acknowlededbygroup = t.AcknowlededByGroup,
                           Acknowlededbyperson = t.AcknowlededByPerson,
                           Acknowledgeddatetime = t.AcknowledgedDateTime,
                           Acknowledgedtimezone = t.AcknowledgedTimeZone,
                           Acknowledgedtime = t.AcknowledgedTime,
                           Acknowledgedweekday = t.Acknowledgedweekday,
                           Acknowledgedweeknum = t.Acknowledgedweeknum,
                           Acknowledgedmonth = t.Acknowledgedmonth,
                           Acknowledgedyear = t.Acknowledgedyear,
                           Resolvedbygroup = t.ResolvedByGroup,
                           Resolvedbyperson = t.Resolvedbyperson,
                           Resolvedbyvendor = t.ResolvedByVendor,
                           Resolutionefforthours = t.ResolutionEfforthours,
                           Resolutiondatetime = t.ResolutionDateTime,
                           Resolvedtimezone = t.ResolvedTimezone,
                           Resolutiontime = t.ResolutionTime,
                           Resolutionweekday = t.ResolutionWeekday,
                           Resolutionweeknum = t.ResolutionWeeknum,
                           Servicerestoredbygroup = t.ServiceRestoredByGroup,
                           Servicerestoredbyperson = t.ServiceRestoredbyperson,
                           Servicerestoreddatetime = t.ServiceRestoredDateTime,
                           Servicerestoredtimezone = t.ServiceRestoredTimeZone,
                           Servicerestoredtime = t.ServiceRestoredTime,
                           Servicerestoredweekday = t.ServiceRestoredWeekday,
                           Servicerestoredweeknum = t.ServiceRestoredWeeknum,
                           Servicerestoredmonth = t.ServiceRestoredMonth,
                           Servicerestoredyear = t.ServiceRestoredYear,
                           Lastupdatedbyperson = t.LastUpdatedByPerson,
                           Lastupdateddatetime = t.LastUpdatedDateTime,
                           Datasource = t.DataSource,
                           Datasourcenotes = t.DataSourceNotes,
                           Assignedbygroup = t.AssignedByGroup,
                           Assignedbyperson = t.AssignedByPerson,
                           Assigneddatetime = t.AssignedDateTime,
                           Esitmatedresponsetimebygroup = "NA",
                           Esitmatedresponsedatetime = t.EsitmatedResponseDateTime,
                           Resolutionyear = t.ResolutionYear,
                           Resolutionmonth = t.ResolutionMonth,
                           Waivedindicator = t.Waivedindicator,
                           Esitmatedresolutiondate = t.EsitmatedResolutionDate,
                           Totalsuspenddurationassignedtoack = t.TotalSuspendDurationAssignedtoack,
                           Totalsuspenddurationacktoer = t.TotalSuspendDurationacktoer,
                           Totalsuspenddurationertoresolution = t.TotalSuspendDurationertoreSolution,
                           Resolvedindicator = t.Resolvedindicator,
                           IsNTTGroup = t.IsNTTGroup,
                           SupportGroupType = t.SupportGroupType,
                           InProgressdatetime = t.InProgressDateTime,
                           Closeddatetime = t.ClosedDateTime,
                           ScheduledstartdateTime = t.ScheduledStartDateTime,
                           ScheduledenddateTime = t.ScheduledEndDateTime,
                           Scheduledchangecount = t.ScheduledChangeCount,
                           Company = t.Company,
                           Site = t.Site,
                           Assigneecompany = t.AssigneeCompany,
                           Assigneeorganization = t.AssigneeOrganization,
                           Estimatedworkeffort = t.EstimatedWorkEffort,
                           Actualworkeffort = t.ActualWorkEffort,
                           ActualstartdateTime = t.ActualStartDateTime,
                           Recyclecount = t.Recyclecount,
                           Statusreason = t.StatusReason,
                           ConfigItemCategory = t.ConfigItemCategory,
                           ResponseTime = t.ResponseTime,
                           ResponseTimeMetSLA = t.ResponseTimeMetSLA,
                           ResolutionTimeMetSLA = t.ResolutionTimeMetSLA,
                           EstimateResponseTime = t.EstimateResponseTime,
                           ClosureCategory = t.ClosureCategory,
                           EstimatedResponseTimeMetSLA = t.EstimatedResponseTimeMetSLA,
                           EstResptarget = t.EstResptarget,
                           Reslntargettime = t.ReslntargetTime,
                           ResolutionExceptionIndicator = t.ResolutionExceptionIndicator,
                           ResolutionExceptionReason = t.ResolutionExceptionReason,
                           ResponseExceptionIndicator = t.ResponseExceptionIndicator,
                           ResponseExceptionReason = t.ResponseExceptionReason,
                           Resptargettime = t.ResptargetTime,
                           Severity = t.Severity,
                           WorkGroupLevel = t.WorkGroupLevel,
                           AffectedItemName = t.AffectedItemName,
                           SLACostPerTicket = t.SLACostPerTicket,
                           INResolutiontime = t.INResolutionTime,
                           INCostPerTicket = t.INCostPerTicket,
                           //  AccountName = string.IsNullOrEmpty(dr.AccountName) ? "NA" : dr.AccountName,
                           //  ContractName = string.IsNullOrEmpty(dr.ContractName) ? "NA" : dr.ContractName
                           AccountName = acc.AccountName,
                           ContractName = contract.ContractName,
                           //newly added by DP
                           Active = t.Active,
                           Activityduedatetime = t.Activityduedatetime,
                           ActualenddateTime = t.ActualenddateTime,
                           Additionalassigneelist = t.Additionalassigneelist,
                           Approval = t.Approval,
                           Approvalhistory = t.Approvalhistory,
                           Approvalsetdatetime = t.Approvalsetdatetime,
                           Businessduration = t.Businessduration,
                           Closenotes = t.Closenotes,
                           Closedby = t.Closedby,
                           Commentsworknotes = t.Commentsworknotes,
                           Correlationdisplay = t.Correlationdisplay,
                           CorrelationID = t.CorrelationID,
                           Deliveryplan = t.Deliveryplan,
                           Deliverytask = t.Deliverytask,
                           Domain = t.Domain,
                           Domainpath = t.Domainpath,
                           Escalation = t.Escalation,
                           Followupdatetime = t.Followupdatetime,
                           Grouplist = t.Grouplist,
                           Knowledge = t.Knowledge,
                           Location = t.Location,
                           MadeSLA = t.MadeSLA,
                           Openeddatetime = t.Openeddatetime,
                           Openedby = t.Openedby,
                           Order = t.Order,
                           ParentticketID = t.ParentticketID,
                           Reassignmentcount = t.Reassignmentcount,
                           SLAduedatetime = t.SLAduedatetime,
                           Tags = t.Tags,
                           Timeworked = t.Timeworked,
                           Updatescount = t.Updatescount,
                           Uponapprovalstep = t.Uponapprovalstep,
                           Uponrejectstep = t.Uponrejectstep,
                           Userinput = t.Userinput,
                           Watchlist = t.Watchlist,
                           Worknotes = t.Worknotes,
                           Worknoteslist = t.Worknoteslist,
                           Businessresolvetime = t.Businessresolvetime,
                           Caller = t.Caller,
                           Causedbychange = t.Causedbychange,
                           ChildIncidentscount = t.ChildIncidentscount,
                           Incidentstate = t.Incidentstate,
                           Lastreopenedatdatetime = t.Lastreopenedatdatetime,
                           Lastreopenedby = t.Lastreopenedby,
                           Notify = t.Notify,
                           ParentIncidentID = t.ParentIncidentID,
                           TotalResolvetime = t.TotalResolvetime,
                           Knownerror = t.Knownerror,
                           Majorproblem = t.Majorproblem,
                           Problemstate = t.Problemstate,
                           RelatedIncidentscount = t.RelatedIncidentscount,
                           Workaround = t.Workaround,
                           Backoutplan = t.Backoutplan,
                           CABdatetime = t.CABdatetime,
                           CABdelegate = t.CABdelegate,
                           CABrecommendation = t.CABrecommendation,
                           CABrequired = t.CABrequired,
                           Changeplan = t.Changeplan,
                           Closecode = t.Closecode,
                           Conflictlastrundatetime = t.Conflictlastrundatetime,
                           Conflictstatus = t.Conflictstatus,
                           Implementationplan = t.Implementationplan,
                           Onhold = t.Onhold,
                           OnHoldChangeTasks = t.OnHoldChangeTasks,
                           Outsidemaintenanceschedule = t.Outsidemaintenanceschedule,
                           Phase = t.Phase,
                           Phasestate = t.Phasestate,
                           Productionsystem = t.Productionsystem,
                           Reason = t.Reason,
                           Requestedby = t.Requestedby,
                           Requestedbydatetime = t.Requestedbydatetime,
                           Reviewcomments = t.Reviewcomments,
                           Reviewdatetime = t.Reviewdatetime,
                           Reviewstatus = t.Reviewstatus,
                           Risk = t.Risk,
                           Changerequestscope = t.Changerequestscope,
                           Changerequesttype = t.Changerequesttype,
                           Tower = t.Tower,
                           ClientPriority = t.clientpriority
                       }).ToList();
            return _dt;

        }
        public static List<INTrendingReportEntity> getResolutionDateResult(int ClientId, DateTime StartDate, DateTime EndDate)
        {
            Db db = new Db();
            var _dt = (from t in db.Incidents
                       join acc in db.ClientAccountSetup on t.AccountId equals acc.Id
                       join contract in db.ClientAccountContract on t.ContractId equals contract.Id
                       where t.ClientId == ClientId
                      && (t.ResolutionDateTime >= StartDate && t.ResolutionDateTime <= EndDate)

                       select new INTrendingReportEntity
                       {
                           ID = t.Id,
                           Clientid = t.ClientId,
                           Incidentid = t.IncidentId,
                           TicketId = t.IncidentId,
                           Createddatetime = t.CreatedDateTime,
                           Status = t.Status,
                           Title = t.Title,
                           Priority = t.Priority,
                           Urgency = t.Urgency,
                           Type = t.Type,
                           Outagetype = t.OutageType,
                           Divisionimpacted = t.DivisionImpacted,
                           Businessunitimpacted = t.BusinessUnitImpacted,
                           Servicecategory = t.ServiceCategory,
                           Servicesubcategory = t.ServiceSubCategory,
                           Servicearea = t.ServiceArea,
                           Regionimpacted = t.RegionImpacted,
                           Locationsimpacted = t.LocationsImpacted,
                           Customerdept = t.CustomerDept,
                           Customerlastname = t.CustomerLastname,
                           Customerfirstname = t.CustomerFirstname,
                           Customertitle = t.CustomerTitle,
                           Customersatisfaction = t.CustomerSatisfaction,
                           Impacttype = t.ImpactType,
                           Configitemid = t.ConfigItemId,
                           Configtype = t.ConfigType,
                           Configitemname = t.ConfigItemName,
                           Environmentimpacted = t.EnvironmentImpacted,
                           Nodename = t.NodeName,
                           Rootcausetype = t.RootCauseType,
                           Rootcausedescription = t.RootcauseDescription,
                           Resolutiontype = t.ResolutionType,
                           Solutionid = t.SolutionId,
                           Deploymentrelease = t.DeploymentRelease,
                           Relatedchangeids = t.RelatedChangeIds,
                           Relatedids = t.RelatedIds,
                           Relatedproblemids = t.RelatedProblemids,
                           Helpdesk = t.Helpdesk,
                           Assignedtoperson = t.AssignedToPerson,
                           Assignedtosupportgroup = t.AssignedToSupportGroup,
                           Createdbygroup = t.CreatedByGroup,
                           Createdbyperson = t.CreatedByPerson,
                           Createdtimezone = t.CreatedTimezone,
                           Createdtime = t.CreatedTime,
                           Createdweekday = t.CreatedWeekday,
                           Createdweeknum = t.CreatedWeeknum,
                           Createdmonth = t.CreatedMonth,
                           Createdyear = t.CreatedYear,
                           Acknowlededbygroup = t.AcknowlededByGroup,
                           Acknowlededbyperson = t.AcknowlededByPerson,
                           Acknowledgeddatetime = t.AcknowledgedDateTime,
                           Acknowledgedtimezone = t.AcknowledgedTimeZone,
                           Acknowledgedtime = t.AcknowledgedTime,
                           Acknowledgedweekday = t.Acknowledgedweekday,
                           Acknowledgedweeknum = t.Acknowledgedweeknum,
                           Acknowledgedmonth = t.Acknowledgedmonth,
                           Acknowledgedyear = t.Acknowledgedyear,
                           Resolvedbygroup = t.ResolvedByGroup,
                           Resolvedbyperson = t.Resolvedbyperson,
                           Resolvedbyvendor = t.ResolvedByVendor,
                           Resolutionefforthours = t.ResolutionEfforthours,
                           Resolutiondatetime = t.ResolutionDateTime,
                           Resolvedtimezone = t.ResolvedTimezone,
                           Resolutiontime = t.ResolutionTime,
                           Resolutionweekday = t.ResolutionWeekday,
                           Resolutionweeknum = t.ResolutionWeeknum,
                           Servicerestoredbygroup = t.ServiceRestoredByGroup,
                           Servicerestoredbyperson = t.ServiceRestoredbyperson,
                           Servicerestoreddatetime = t.ServiceRestoredDateTime,
                           Servicerestoredtimezone = t.ServiceRestoredTimeZone,
                           Servicerestoredtime = t.ServiceRestoredTime,
                           Servicerestoredweekday = t.ServiceRestoredWeekday,
                           Servicerestoredweeknum = t.ServiceRestoredWeeknum,
                           Servicerestoredmonth = t.ServiceRestoredMonth,
                           Servicerestoredyear = t.ServiceRestoredYear,
                           Lastupdatedbyperson = t.LastUpdatedByPerson,
                           Lastupdateddatetime = t.LastUpdatedDateTime,
                           Datasource = t.DataSource,
                           Datasourcenotes = t.DataSourceNotes,
                           Assignedbygroup = t.AssignedByGroup,
                           Assignedbyperson = t.AssignedByPerson,
                           Assigneddatetime = t.AssignedDateTime,
                           Esitmatedresponsetimebygroup = "NA",
                           Esitmatedresponsedatetime = t.EsitmatedResponseDateTime,
                           Resolutionyear = t.ResolutionYear,
                           Resolutionmonth = t.ResolutionMonth,
                           Waivedindicator = t.Waivedindicator,
                           Esitmatedresolutiondate = t.EsitmatedResolutionDate,
                           Totalsuspenddurationassignedtoack = t.TotalSuspendDurationAssignedtoack,
                           Totalsuspenddurationacktoer = t.TotalSuspendDurationacktoer,
                           Totalsuspenddurationertoresolution = t.TotalSuspendDurationertoreSolution,
                           Resolvedindicator = t.Resolvedindicator,
                           IsNTTGroup = t.IsNTTGroup,
                           SupportGroupType = t.SupportGroupType,
                           InProgressdatetime = t.InProgressDateTime,
                           Closeddatetime = t.ClosedDateTime,
                           ScheduledstartdateTime = t.ScheduledStartDateTime,
                           ScheduledenddateTime = t.ScheduledEndDateTime,
                           Scheduledchangecount = t.ScheduledChangeCount,
                           Company = t.Company,
                           Site = t.Site,
                           Assigneecompany = t.AssigneeCompany,
                           Assigneeorganization = t.AssigneeOrganization,
                           Estimatedworkeffort = t.EstimatedWorkEffort,
                           Actualworkeffort = t.ActualWorkEffort,
                           ActualstartdateTime = t.ActualStartDateTime,
                           Recyclecount = t.Recyclecount,
                           Statusreason = t.StatusReason,
                           ConfigItemCategory = t.ConfigItemCategory,
                           ResponseTime = t.ResponseTime,
                           ResponseTimeMetSLA = t.ResponseTimeMetSLA,
                           ResolutionTimeMetSLA = t.ResolutionTimeMetSLA,
                           EstimateResponseTime = t.EstimateResponseTime,
                           ClosureCategory = t.ClosureCategory,
                           EstimatedResponseTimeMetSLA = t.EstimatedResponseTimeMetSLA,
                           EstResptarget = t.EstResptarget,
                           Reslntargettime = t.ReslntargetTime,
                           ResolutionExceptionIndicator = t.ResolutionExceptionIndicator,
                           ResolutionExceptionReason = t.ResolutionExceptionReason,
                           ResponseExceptionIndicator = t.ResponseExceptionIndicator,
                           ResponseExceptionReason = t.ResponseExceptionReason,
                           Resptargettime = t.ResptargetTime,
                           Severity = t.Severity,
                           WorkGroupLevel = t.WorkGroupLevel,
                           AffectedItemName = t.AffectedItemName,
                           SLACostPerTicket = t.SLACostPerTicket,
                           INResolutiontime = t.INResolutionTime,
                           INCostPerTicket = t.INCostPerTicket,
                           //  AccountName = string.IsNullOrEmpty(dr.AccountName) ? "NA" : dr.AccountName,
                           //  ContractName = string.IsNullOrEmpty(dr.ContractName) ? "NA" : dr.ContractName
                           AccountName = acc.AccountName,
                           ContractName = contract.ContractName,
                           //newly added by DP
                           Active = t.Active,
                           Activityduedatetime = t.Activityduedatetime,
                           ActualenddateTime = t.ActualenddateTime,
                           Additionalassigneelist = t.Additionalassigneelist,
                           Approval = t.Approval,
                           Approvalhistory = t.Approvalhistory,
                           Approvalsetdatetime = t.Approvalsetdatetime,
                           Businessduration = t.Businessduration,
                           Closenotes = t.Closenotes,
                           Closedby = t.Closedby,
                           Commentsworknotes = t.Commentsworknotes,
                           Correlationdisplay = t.Correlationdisplay,
                           CorrelationID = t.CorrelationID,
                           Deliveryplan = t.Deliveryplan,
                           Deliverytask = t.Deliverytask,
                           Domain = t.Domain,
                           Domainpath = t.Domainpath,
                           Escalation = t.Escalation,
                           Followupdatetime = t.Followupdatetime,
                           Grouplist = t.Grouplist,
                           Knowledge = t.Knowledge,
                           Location = t.Location,
                           MadeSLA = t.MadeSLA,
                           Openeddatetime = t.Openeddatetime,
                           Openedby = t.Openedby,
                           Order = t.Order,
                           ParentticketID = t.ParentticketID,
                           Reassignmentcount = t.Reassignmentcount,
                           SLAduedatetime = t.SLAduedatetime,
                           Tags = t.Tags,
                           Timeworked = t.Timeworked,
                           Updatescount = t.Updatescount,
                           Uponapprovalstep = t.Uponapprovalstep,
                           Uponrejectstep = t.Uponrejectstep,
                           Userinput = t.Userinput,
                           Watchlist = t.Watchlist,
                           Worknotes = t.Worknotes,
                           Worknoteslist = t.Worknoteslist,
                           Businessresolvetime = t.Businessresolvetime,
                           Caller = t.Caller,
                           Causedbychange = t.Causedbychange,
                           ChildIncidentscount = t.ChildIncidentscount,
                           Incidentstate = t.Incidentstate,
                           Lastreopenedatdatetime = t.Lastreopenedatdatetime,
                           Lastreopenedby = t.Lastreopenedby,
                           Notify = t.Notify,
                           ParentIncidentID = t.ParentIncidentID,
                           TotalResolvetime = t.TotalResolvetime,
                           Knownerror = t.Knownerror,
                           Majorproblem = t.Majorproblem,
                           Problemstate = t.Problemstate,
                           RelatedIncidentscount = t.RelatedIncidentscount,
                           Workaround = t.Workaround,
                           Backoutplan = t.Backoutplan,
                           CABdatetime = t.CABdatetime,
                           CABdelegate = t.CABdelegate,
                           CABrecommendation = t.CABrecommendation,
                           CABrequired = t.CABrequired,
                           Changeplan = t.Changeplan,
                           Closecode = t.Closecode,
                           Conflictlastrundatetime = t.Conflictlastrundatetime,
                           Conflictstatus = t.Conflictstatus,
                           Implementationplan = t.Implementationplan,
                           Onhold = t.Onhold,
                           OnHoldChangeTasks = t.OnHoldChangeTasks,
                           Outsidemaintenanceschedule = t.Outsidemaintenanceschedule,
                           Phase = t.Phase,
                           Phasestate = t.Phasestate,
                           Productionsystem = t.Productionsystem,
                           Reason = t.Reason,
                           Requestedby = t.Requestedby,
                           Requestedbydatetime = t.Requestedbydatetime,
                           Reviewcomments = t.Reviewcomments,
                           Reviewdatetime = t.Reviewdatetime,
                           Reviewstatus = t.Reviewstatus,
                           Risk = t.Risk,
                           Changerequestscope = t.Changerequestscope,
                           Changerequesttype = t.Changerequesttype,
                           Tower = t.Tower,
                           ClientPriority = t.clientpriority
                       }).ToList();
            return _dt;

        }
        public static IList<SLAMaintenenceModel> getSlaTickets(int ClientId, DateTime StartDate, DateTime EndDate, string DateType)
        {
            IList<SLAMaintenenceModel> Tredingdata = new List<SLAMaintenenceModel>();
            var paramName = new string[] { "ClientId", "Datetype", "StartDate", "EndDate" };
            var paramValue = new object[] { ClientId, DateType, StartDate, EndDate };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("SLATicketDataMaintenance", paramName, paramValue);

                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new SLAMaintenenceModel();
                    coldet.Id = Convert.ToInt32(dr["ID"]);
                    if (!(dr["AssignedDateTime"] is DBNull))
                        coldet.AssignedDateTime = Convert.ToDateTime(dr["AssignedDateTime"]);
                    if (!(dr["ClosedDateTime"] is DBNull))
                        coldet.ClosedDateTime = Convert.ToDateTime(dr["ClosedDateTime"]);
                    if (!(dr["CreatedDateTime"] is DBNull))
                        coldet.CreatedDateTime = Convert.ToDateTime(dr["CreatedDateTime"]);
                    if (!(dr["LastUpdatedDateTime"] is DBNull))
                        coldet.LastUpdatedDateTime = Convert.ToDateTime(dr["LastUpdatedDateTime"]);
                    coldet.Description = dr["Description"].ToString();
                    coldet.Priority = dr["Priority"].ToString();
                    if (!(dr["ResolutionDateTime"] is DBNull))
                        coldet.ResolutionDateTime = Convert.ToDateTime(dr["ResolutionDateTime"]);
                    if (!(dr["ReslntargetTime"] is DBNull))
                        coldet.ResolutionTarget = dr["ReslntargetTime"].ToString();
                    if (!(dr["ResolutionTime"] is DBNull))
                        coldet.ResolutionTime = Convert.ToInt32(dr["ResolutionTime"]);
                    if (!(dr["ResolutionTimeMetSLA"] is DBNull))
                        coldet.ResolutionTimeMetSLA = dr["ResolutionTimeMetSLA"].ToString();
                    if (!(dr["ResptargetTime"] is DBNull))
                        coldet.ResponseTarget = dr["ResptargetTime"].ToString();
                    if (!(dr["ResponseTime"] is DBNull))
                        coldet.ResponseTime = Convert.ToInt32(dr["ResponseTime"]);
                    if (!(dr["ResolutionTimeMetSLA"] is DBNull))
                        coldet.ResponseTimeMetSLA = dr["ResolutionTimeMetSLA"].ToString();
                    if (!(dr["Status"] is DBNull))
                        coldet.Status = dr["Status"].ToString();
                    if (!(dr["IncidentId"] is DBNull))
                        coldet.TicketID = dr["IncidentId"].ToString();
                    if (!(dr["Tower"] is DBNull))
                        coldet.Track = dr["Tower"].ToString();
                    if (!(dr["Type"] is DBNull))
                        coldet.Type = dr["Type"].ToString();
                    if (!(dr["WaiveReason"] is DBNull))
                        coldet.WaiveReason = dr["WaiveReason"].ToString();
                    if (!(dr["Waivedindicator"] is DBNull))
                        coldet.WaiveSLAReport = dr["Waivedindicator"].ToString() == "Y" ? true : false;
                    if (!(dr["AssignedToSupportGroup"] is DBNull))
                        coldet.AssignedToSupportGroup = dr["AssignedToSupportGroup"].ToString();
                    if (!(dr["ResolutionExceptionIndicator"] is DBNull))
                        coldet.ResolutionExceptionIndicator = dr["ResolutionExceptionIndicator"].ToString() == "Y" ? true : false;
                    if (!(dr["ResponseExceptionReason"] is DBNull))
                        coldet.ResponseExceptionReason = dr["ResponseExceptionReason"].ToString();
                    if (!(dr["ResponseExceptionIndicator"] is DBNull))
                        coldet.ResponseExceptionIndicator = dr["ResponseExceptionIndicator"].ToString() == "Y" ? true : false;
                    if (!(dr["ResolutionExceptionReason"] is DBNull))
                        coldet.ResolutionExceptionReason = dr["ResolutionExceptionReason"].ToString();
                    Tredingdata.Add(coldet);
                }
            }
            return Tredingdata;
        }

        public static IList<SLAMaintenenceModel> getPendingTickets(int ClientId, DateTime StartDate, DateTime EndDate, string DateType)
        {
            IList<SLAMaintenenceModel> Tredingdata = new List<SLAMaintenenceModel>();
            var paramName = new string[] { "ClientId", "Datetype", "StartDate", "EndDate" };
            var paramValue = new object[] { ClientId, DateType, StartDate, EndDate };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("PendingTicketMaintenance", paramName, paramValue);

                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new SLAMaintenenceModel();
                    coldet.Id = Convert.ToInt32(dr["ID"]);
                    if (!(dr["AssignedDateTime"] is DBNull))
                        coldet.AssignedDateTime = Convert.ToDateTime(dr["AssignedDateTime"]);
                    if (!(dr["ClosedDateTime"] is DBNull))
                        coldet.ClosedDateTime = Convert.ToDateTime(dr["ClosedDateTime"]);
                    if (!(dr["CreatedDateTime"] is DBNull))
                        coldet.CreatedDateTime = Convert.ToDateTime(dr["CreatedDateTime"]);
                    if (!(dr["LastUpdatedDateTime"] is DBNull))
                        coldet.LastUpdatedDateTime = Convert.ToDateTime(dr["LastUpdatedDateTime"]);
                    coldet.Description = dr["Description"].ToString();
                    coldet.Priority = dr["Priority"].ToString();
                    if (!(dr["ResolutionDateTime"] is DBNull))
                        coldet.ResolutionDateTime = Convert.ToDateTime(dr["ResolutionDateTime"]);
                    if (!(dr["ReslntargetTime"] is DBNull))
                        coldet.ResolutionTarget = dr["ReslntargetTime"].ToString();
                    if (!(dr["ResolutionTime"] is DBNull))
                        coldet.ResolutionTime = Convert.ToInt32(dr["ResolutionTime"]);
                    if (!(dr["ResolutionTimeMetSLA"] is DBNull))
                        coldet.ResolutionTimeMetSLA = dr["ResolutionTimeMetSLA"].ToString();
                    if (!(dr["ResptargetTime"] is DBNull))
                        coldet.ResponseTarget = dr["ResptargetTime"].ToString();
                    if (!(dr["ResponseTime"] is DBNull))
                        coldet.ResponseTime = Convert.ToInt32(dr["ResponseTime"]);
                    if (!(dr["ResolutionTimeMetSLA"] is DBNull))
                        coldet.ResponseTimeMetSLA = dr["ResolutionTimeMetSLA"].ToString();
                    if (!(dr["Status"] is DBNull))
                        coldet.Status = dr["Status"].ToString();
                    if (!(dr["IncidentId"] is DBNull))
                        coldet.TicketID = dr["IncidentId"].ToString();
                    if (!(dr["Tower"] is DBNull))
                        coldet.Track = dr["Tower"].ToString();
                    if (!(dr["Type"] is DBNull))
                        coldet.Type = dr["Type"].ToString();
                    if (!(dr["ExcludeReason"] is DBNull))
                        coldet.WaiveReason = dr["ExcludeReason"].ToString();
                    if (!(dr["ExcludeIndicator"] is DBNull))
                        coldet.WaiveSLAReport = dr["ExcludeIndicator"].ToString() == "Y" ? true : false;
                    if (!(dr["AssignedToSupportGroup"] is DBNull))
                        coldet.AssignedToSupportGroup = dr["AssignedToSupportGroup"].ToString();
                    Tredingdata.Add(coldet);
                }
            }
            return Tredingdata;
        }
        public static DataTable INBackLogAnalysis(int ClientId, DateTime fromDate, DateTime toDate, string columngroup, bool bCumulativeflag, string TowerLst, string LocationLst, string TypeLst, string ServiceareaLst, string ClientPriorityLst, string strAssignedtosupportgroup, string strColumnFltrs)
        {

            // IList<TrendingReportInput> Tredingdata = new List<TrendingReportInput>();

            DateTime paramFromDate = System.DateTime.MaxValue;
            DateTime paramToDate = System.DateTime.MaxValue;
            paramFromDate = fromDate;
            paramToDate = toDate;

            DataTable dt = new DataTable();
            DataTable retTable = new DataTable();
            retTable.Columns.Add("Date");
            retTable.Columns.Add("Location");
            retTable.Columns.Add("Type");
            retTable.Columns.Add("Month");
            retTable.Columns.Add("Year");
            retTable.Columns.Add("Quarter");
            retTable.Columns.Add("Tower");
            retTable.Columns.Add("Week");
            retTable.Columns.Add("Priority"); //
            retTable.Columns.Add("Assignedtosupportgroup");
            retTable.Columns.Add("Service Area");

            retTable.Columns.Add("Closed Tickets", Type.GetType("System.Int32"));
            retTable.Columns.Add("Created Tickets", Type.GetType("System.Int32"));
            retTable.Columns.Add("Resolved Tickets", Type.GetType("System.Int32"));
            retTable.Columns.Add("Resolved Backlog", Type.GetType("System.Int32"));
            retTable.Columns.Add("Closed Backlog", Type.GetType("System.Int32"));
            retTable.Columns.Add("Cumulative Resolved Backlog", Type.GetType("System.Int32"));
            retTable.Columns.Add("Cumulative Closed Backlog", Type.GetType("System.Int32"));
            paramFromDate = fromDate;
            paramToDate = toDate;


            string spName = "SP_FetchDataFromCube";
            //if (bCumulativeflag)
            {
                var paramName = new string[] { "ClientId", "StartDate", "EndDate", "Dim", "Tower", "Location", "Type", "Servicearea", "ClientPriority", "Assignedtosupportgroup", "Filter" };
                var paramValue = new object[] { ClientId, fromDate, toDate, columngroup, TowerLst, LocationLst, TypeLst, ServiceareaLst, ClientPriorityLst, strAssignedtosupportgroup, strColumnFltrs };



                spName = "SP_FetchTicketCountFilter_New";
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    dt = unitOfWork.GetDataFromStoredProcedure(spName, paramName, paramValue);

                }


                foreach (DataRow dr in dt.Rows)
                {
                    DataRow retRow = retTable.NewRow();
                    retRow["Date"] = Convert.ToDateTime(dr["Date"]).ToShortDateString();// dr["[IncidentCount].[Date].[Date].[MEMBER_CAPTION]"];
                    retRow["Type"] = dr["Type"];
                    retRow["Location"] = dr["Location"];
                    retRow["Month"] = dr["Month"];
                    retRow["Year"] = dr["Year"];
                    retRow["Quarter"] = dr["Quarter"];
                    retRow["Tower"] = dr["Tower"];
                    retRow["Week"] = dr["Week"];
                    retRow["Service Area"] = dr["Servicearea"];
                    retRow["Priority"] = dr["ClientPriority"];
                    retRow["Assignedtosupportgroup"] = dr["Assignedtosupportgroup"];

                    retRow["Closed Tickets"] = Convert.ToInt32(dr["ClosedTicket"]);
                    retRow["Created Tickets"] = Convert.ToInt32(dr["CreatedTicket"]);
                    retRow["Resolved Tickets"] = Convert.ToInt32(dr["ResolvedTicket"]);
                    retRow["Resolved Backlog"] = Convert.ToInt32(dr["ResolvedBacklogTicket"]);
                    retRow["Closed Backlog"] = Convert.ToInt32(dr["ClosedBacklogTicket"]);
                    retRow["Cumulative Resolved Backlog"] = Convert.ToInt32(dr["CumulativeResolvedBacklog"]);
                    retRow["Cumulative Closed Backlog"] = Convert.ToInt32(dr["CumulativeClosedBacklog"]);
                    retTable.Rows.Add(retRow);

                }

            }
            //else
            //{
            //    var paramName = new string[] { "ClientId" };
            //    var paramValue = new object[] { ClientId };

            //    spName = "SP_FetchDataFromCube_New";
            //    using (var contextFactory = new DbContextFactory())
            //    {
            //        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            //        dt = unitOfWork.GetDataFromStoredProcedure(spName, paramName, paramValue);

            //    }


            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        DataRow retRow = retTable.NewRow();
            //        retRow["Date"] = Convert.ToDateTime(dr["[IncidentCount].[Date].[Date].[MEMBER_CAPTION]"]).ToShortDateString();
            //        DateTime RecDate = Convert.ToDateTime(retRow["Date"]);

            //        if (RecDate >= fromDate && RecDate <= toDate)
            //        {
            //            retRow["Type"] = dr["[IncidentCount].[Type].[Type].[MEMBER_CAPTION]"];
            //            retRow["Location"] = dr["[IncidentCount].[Location].[Location].[MEMBER_CAPTION]"];
            //            retRow["Month"] = dr["[IncidentCount].[Month].[Month].[MEMBER_CAPTION]"];
            //            // retRow["Date"] = dr["[IncidentCount].[Date].[Date].[MEMBER_CAPTION]"];
            //            retRow["Year"] = dr["[IncidentCount].[Year].[Year].[MEMBER_CAPTION]"];
            //            retRow["Quarter"] = dr["[IncidentCount].[Quarter].[Quarter].[MEMBER_CAPTION]"];
            //            retRow["Tower"] = dr["[IncidentCount].[Tower].[Tower].[MEMBER_CAPTION]"];
            //            retRow["Week"] = dr["[IncidentCount].[Week].[Week].[MEMBER_CAPTION]"];
            //            retRow["Service Area"] = dr["[IncidentCount].[Servicearea].[Servicearea].[MEMBER_CAPTION]"];
            //            retRow["Priority"] = dr["[IncidentCount].[Client Priority].[Client Priority].[MEMBER_CAPTION]"]; ;
            //            retRow["Assignedtosupportgroup"] = dr["[IncidentCount].[Assignedtosupportgroup].[Assignedtosupportgroup].[MEMBER_CAPTION]"];
            //            retRow["Closed Tickets"] = Convert.ToInt32(dr["[Measures].[Closed Ticket]"]);
            //            retRow["Created Tickets"] = Convert.ToInt32(dr["[Measures].[Created Ticket]"]);
            //            retRow["Resolved Tickets"] = Convert.ToInt32(dr["[Measures].[Resolved Ticket]"]);
            //            retRow["Resolved Backlog"] = Convert.ToInt32(dr["[Measures].[Resolved Backlog Ticket]"]);
            //            retRow["Closed Backlog"] = Convert.ToInt32(dr["[Measures].[Closed Backlog Ticket]"]);

            //            retRow["Cumulative Resolved Backlog"] = 0;//.[CumulativeResolvedbacklog]"]);
            //            retRow["Cumulative Closed Backlog"] = 0;//.[CumulativeClosedbacklog]"]);

            //            // retRow["Cumulative Backlog"] = Convert.ToInt32(dr["[Measures].[Backlog Ticket]"]);
            //            retTable.Rows.Add(retRow);
            //        }
            //    }
            //}
            return retTable;

        }
        public static List<INTrendingReportEntity> INTrendingReportList(int ClientId)
        {
            IList<TrendingReportInput> Tredingdata = new List<TrendingReportInput>();
            ////var paramName = new string[] { "ClientId" };
            ////var paramValue = new object[] { ClientId };
            Db db = new Db();
            var _dt = (from t in db.Incidents
                       join acc in db.ClientAccountSetup on t.AccountId equals acc.Id
                       join contract in db.ClientAccountContract on t.ContractId equals contract.Id
                       where t.ClientId == ClientId
                       select new INTrendingReportEntity
                       {
                           ID = t.Id,
                           Clientid = t.ClientId,
                           Incidentid = t.IncidentId,
                           TicketId = t.IncidentId,
                           Createddatetime = t.CreatedDateTime,
                           Status = t.Status,
                           Title = t.Title,
                           Priority = t.Priority,
                           Urgency = t.Urgency,
                           Type = t.Type,
                           Outagetype = t.OutageType,
                           Divisionimpacted = t.DivisionImpacted,
                           Businessunitimpacted = t.BusinessUnitImpacted,
                           Servicecategory = t.ServiceCategory,
                           Servicesubcategory = t.ServiceSubCategory,
                           Servicearea = t.ServiceArea,
                           Regionimpacted = t.RegionImpacted,
                           Locationsimpacted = t.LocationsImpacted,
                           Customerdept = t.CustomerDept,
                           Customerlastname = t.CustomerLastname,
                           Customerfirstname = t.CustomerFirstname,
                           Customertitle = t.CustomerTitle,
                           Customersatisfaction = t.CustomerSatisfaction,
                           Impacttype = t.ImpactType,
                           Configitemid = t.ConfigItemId,
                           Configtype = t.ConfigType,
                           Configitemname = t.ConfigItemName,
                           Environmentimpacted = t.EnvironmentImpacted,
                           Nodename = t.NodeName,
                           Rootcausetype = t.RootCauseType,
                           Rootcausedescription = t.RootcauseDescription,
                           Resolutiontype = t.ResolutionType,
                           Solutionid = t.SolutionId,
                           Deploymentrelease = t.DeploymentRelease,
                           Relatedchangeids = t.RelatedChangeIds,
                           Relatedids = t.RelatedIds,
                           Relatedproblemids = t.RelatedProblemids,
                           Helpdesk = t.Helpdesk,
                           Assignedtoperson = t.AssignedToPerson,
                           Assignedtosupportgroup = t.AssignedToSupportGroup,
                           Createdbygroup = t.CreatedByGroup,
                           Createdbyperson = t.CreatedByPerson,
                           Createdtimezone = t.CreatedTimezone,
                           Createdtime = t.CreatedTime,
                           Createdweekday = t.CreatedWeekday,
                           Createdweeknum = t.CreatedWeeknum,
                           Createdmonth = t.CreatedMonth,
                           Createdyear = t.CreatedYear,
                           Acknowlededbygroup = t.AcknowlededByGroup,
                           Acknowlededbyperson = t.AcknowlededByPerson,
                           Acknowledgeddatetime = t.AcknowledgedDateTime,
                           Acknowledgedtimezone = t.AcknowledgedTimeZone,
                           Acknowledgedtime = t.AcknowledgedTime,
                           Acknowledgedweekday = t.Acknowledgedweekday,
                           Acknowledgedweeknum = t.Acknowledgedweeknum,
                           Acknowledgedmonth = t.Acknowledgedmonth,
                           Acknowledgedyear = t.Acknowledgedyear,
                           Resolvedbygroup = t.ResolvedByGroup,
                           Resolvedbyperson = t.Resolvedbyperson,
                           Resolvedbyvendor = t.ResolvedByVendor,
                           Resolutionefforthours = t.ResolutionEfforthours,
                           Resolutiondatetime = t.ResolutionDateTime,
                           Resolvedtimezone = t.ResolvedTimezone,
                           Resolutiontime = t.ResolutionTime,
                           Resolutionweekday = t.ResolutionWeekday,
                           Resolutionweeknum = t.ResolutionWeeknum,
                           Servicerestoredbygroup = t.ServiceRestoredByGroup,
                           Servicerestoredbyperson = t.ServiceRestoredbyperson,
                           Servicerestoreddatetime = t.ServiceRestoredDateTime,
                           Servicerestoredtimezone = t.ServiceRestoredTimeZone,
                           Servicerestoredtime = t.ServiceRestoredTime,
                           Servicerestoredweekday = t.ServiceRestoredWeekday,
                           Servicerestoredweeknum = t.ServiceRestoredWeeknum,
                           Servicerestoredmonth = t.ServiceRestoredMonth,
                           Servicerestoredyear = t.ServiceRestoredYear,
                           Lastupdatedbyperson = t.LastUpdatedByPerson,
                           Lastupdateddatetime = t.LastUpdatedDateTime,
                           Datasource = t.DataSource,
                           Datasourcenotes = t.DataSourceNotes,
                           Assignedbygroup = t.AssignedByGroup,
                           Assignedbyperson = t.AssignedByPerson,
                           Assigneddatetime = t.AssignedDateTime,
                           Esitmatedresponsetimebygroup = "NA",
                           Esitmatedresponsedatetime = t.EsitmatedResponseDateTime,
                           Resolutionyear = t.ResolutionYear,
                           Resolutionmonth = t.ResolutionMonth,
                           Waivedindicator = t.Waivedindicator,
                           Esitmatedresolutiondate = t.EsitmatedResolutionDate,
                           Totalsuspenddurationassignedtoack = t.TotalSuspendDurationAssignedtoack,
                           Totalsuspenddurationacktoer = t.TotalSuspendDurationacktoer,
                           Totalsuspenddurationertoresolution = t.TotalSuspendDurationertoreSolution,
                           Resolvedindicator = t.Resolvedindicator,
                           IsNTTGroup = t.IsNTTGroup,
                           SupportGroupType = t.SupportGroupType,
                           InProgressdatetime = t.InProgressDateTime,
                           Closeddatetime = t.ClosedDateTime,
                           ScheduledstartdateTime = t.ScheduledStartDateTime,
                           ScheduledenddateTime = t.ScheduledEndDateTime,
                           Scheduledchangecount = t.ScheduledChangeCount,
                           Company = t.Company,
                           Site = t.Site,
                           Assigneecompany = t.AssigneeCompany,
                           Assigneeorganization = t.AssigneeOrganization,
                           Estimatedworkeffort = t.EstimatedWorkEffort,
                           Actualworkeffort = t.ActualWorkEffort,
                           ActualstartdateTime = t.ActualStartDateTime,
                           Recyclecount = t.Recyclecount,
                           Statusreason = t.StatusReason,
                           ConfigItemCategory = t.ConfigItemCategory,
                           ResponseTime = t.ResponseTime,
                           ResponseTimeMetSLA = t.ResponseTimeMetSLA,
                           ResolutionTimeMetSLA = t.ResolutionTimeMetSLA,
                           EstimateResponseTime = t.EstimateResponseTime,
                           ClosureCategory = t.ClosureCategory,
                           EstimatedResponseTimeMetSLA = t.EstimatedResponseTimeMetSLA,
                           EstResptarget = t.EstResptarget,
                           Reslntargettime = t.ReslntargetTime,
                           ResolutionExceptionIndicator = t.ResolutionExceptionIndicator,
                           ResolutionExceptionReason = t.ResolutionExceptionReason,
                           ResponseExceptionIndicator = t.ResponseExceptionIndicator,
                           ResponseExceptionReason = t.ResponseExceptionReason,
                           Resptargettime = t.ResptargetTime,
                           Severity = t.Severity,
                           WorkGroupLevel = t.WorkGroupLevel,
                           AffectedItemName = t.AffectedItemName,
                           SLACostPerTicket = t.SLACostPerTicket,
                           INResolutiontime = t.INResolutionTime,
                           INCostPerTicket = t.INCostPerTicket,
                           //  AccountName = string.IsNullOrEmpty(dr.AccountName) ? "NA" : dr.AccountName,
                           //  ContractName = string.IsNullOrEmpty(dr.ContractName) ? "NA" : dr.ContractName
                           AccountName = acc.AccountName,
                           ContractName = contract.ContractName,
                           //newly added by DP
                           Active = t.Active,
                           Activityduedatetime = t.Activityduedatetime,
                           ActualenddateTime = t.ActualenddateTime,
                           Additionalassigneelist = t.Additionalassigneelist,
                           Approval = t.Approval,
                           Approvalhistory = t.Approvalhistory,
                           Approvalsetdatetime = t.Approvalsetdatetime,
                           Businessduration = t.Businessduration,
                           Closenotes = t.Closenotes,
                           Closedby = t.Closedby,
                           Commentsworknotes = t.Commentsworknotes,
                           Correlationdisplay = t.Correlationdisplay,
                           CorrelationID = t.CorrelationID,
                           Deliveryplan = t.Deliveryplan,
                           Deliverytask = t.Deliverytask,
                           Domain = t.Domain,
                           Domainpath = t.Domainpath,
                           Escalation = t.Escalation,
                           Followupdatetime = t.Followupdatetime,
                           Grouplist = t.Grouplist,
                           Knowledge = t.Knowledge,
                           Location = t.Location,
                           MadeSLA = t.MadeSLA,
                           Openeddatetime = t.Openeddatetime,
                           Openedby = t.Openedby,
                           Order = t.Order,
                           ParentticketID = t.ParentticketID,
                           Reassignmentcount = t.Reassignmentcount,
                           SLAduedatetime = t.SLAduedatetime,
                           Tags = t.Tags,
                           Timeworked = t.Timeworked,
                           Updatescount = t.Updatescount,
                           Uponapprovalstep = t.Uponapprovalstep,
                           Uponrejectstep = t.Uponrejectstep,
                           Userinput = t.Userinput,
                           Watchlist = t.Watchlist,
                           Worknotes = t.Worknotes,
                           Worknoteslist = t.Worknoteslist,
                           Businessresolvetime = t.Businessresolvetime,
                           Caller = t.Caller,
                           Causedbychange = t.Causedbychange,
                           ChildIncidentscount = t.ChildIncidentscount,
                           Incidentstate = t.Incidentstate,
                           Lastreopenedatdatetime = t.Lastreopenedatdatetime,
                           Lastreopenedby = t.Lastreopenedby,
                           Notify = t.Notify,
                           ParentIncidentID = t.ParentIncidentID,
                           TotalResolvetime = t.TotalResolvetime,
                           Knownerror = t.Knownerror,
                           Majorproblem = t.Majorproblem,
                           Problemstate = t.Problemstate,
                           RelatedIncidentscount = t.RelatedIncidentscount,
                           Workaround = t.Workaround,
                           Backoutplan = t.Backoutplan,
                           CABdatetime = t.CABdatetime,
                           CABdelegate = t.CABdelegate,
                           CABrecommendation = t.CABrecommendation,
                           CABrequired = t.CABrequired,
                           Changeplan = t.Changeplan,
                           Closecode = t.Closecode,
                           Conflictlastrundatetime = t.Conflictlastrundatetime,
                           Conflictstatus = t.Conflictstatus,
                           Implementationplan = t.Implementationplan,
                           Onhold = t.Onhold,
                           OnHoldChangeTasks = t.OnHoldChangeTasks,
                           Outsidemaintenanceschedule = t.Outsidemaintenanceschedule,
                           Phase = t.Phase,
                           Phasestate = t.Phasestate,
                           Productionsystem = t.Productionsystem,
                           Reason = t.Reason,
                           Requestedby = t.Requestedby,
                           Requestedbydatetime = t.Requestedbydatetime,
                           Reviewcomments = t.Reviewcomments,
                           Reviewdatetime = t.Reviewdatetime,
                           Reviewstatus = t.Reviewstatus,
                           Risk = t.Risk,
                           Changerequestscope = t.Changerequestscope,
                           Changerequesttype = t.Changerequesttype,
                           Tower = t.Tower

                       }).ToList();
            return _dt;
            ////    var dt = _dt.Select(dr => new INTrendingReportEntity
            ////    {
            ////        ID = dr.Incident.Id,
            ////        Clientid = dr.Incident.ClientId,
            ////        Incidentid = dr.Incident.IncidentId,
            ////        TicketId = String.IsNullOrEmpty(dr.Incident.IncidentId) ? "NA" : dr.Incident.IncidentId,
            ////        Createddatetime = dr.Incident.CreatedDateTime == null ? DateTime.MinValue : dr.Incident.CreatedDateTime.Value,
            ////        Status = String.IsNullOrEmpty(dr.Incident.Status) ? "NA" : dr.Incident.Status,
            ////        Title = String.IsNullOrEmpty(dr.Incident.Title) ? "NA" : dr.Incident.Title,
            ////        Priority = String.IsNullOrEmpty(dr.Incident.Priority) ? "NA" : dr.Incident.Priority,
            ////        Urgency = String.IsNullOrEmpty(dr.Incident.Urgency) ? "NA" : dr.Incident.Urgency,
            ////        Type = String.IsNullOrEmpty(dr.Incident.Type) ? "NA" : dr.Incident.Type,
            ////        Outagetype = String.IsNullOrEmpty(dr.Incident.OutageType) ? "NA" : dr.Incident.OutageType,
            ////        Divisionimpacted = String.IsNullOrEmpty(dr.Incident.DivisionImpacted) ? "NA" : dr.Incident.DivisionImpacted,
            ////        Businessunitimpacted = String.IsNullOrEmpty(dr.Incident.BusinessUnitImpacted) ? "NA" : dr.Incident.BusinessUnitImpacted,
            ////        Servicecategory = String.IsNullOrEmpty(dr.Incident.ServiceCategory) ? "NA" : dr.Incident.ServiceCategory,
            ////        Servicesubcategory = String.IsNullOrEmpty(dr.Incident.ServiceSubCategory) ? "NA" : dr.Incident.ServiceSubCategory,
            ////        Servicearea = String.IsNullOrEmpty(dr.Incident.ServiceArea) ? "NA" : dr.Incident.ServiceArea,
            ////        Regionimpacted = String.IsNullOrEmpty(dr.Incident.RegionImpacted) ? "NA" : dr.Incident.RegionImpacted,
            ////        Locationsimpacted = String.IsNullOrEmpty(dr.Incident.LocationsImpacted) ? "NA" : dr.Incident.LocationsImpacted,
            ////        Customerdept = String.IsNullOrEmpty(dr.Incident.CustomerDept) ? "NA" : dr.Incident.CustomerDept,
            ////        Customerlastname = String.IsNullOrEmpty(dr.Incident.CustomerLastname) ? "NA" : dr.Incident.CustomerLastname,
            ////        Customerfirstname = String.IsNullOrEmpty(dr.Incident.CustomerFirstname) ? "NA" : dr.Incident.CustomerFirstname,
            ////        Customertitle = String.IsNullOrEmpty(dr.Incident.CustomerTitle) ? "NA" : dr.Incident.CustomerTitle,
            ////        Customersatisfaction = String.IsNullOrEmpty(dr.Incident.CustomerSatisfaction) ? "NA" : dr.Incident.CustomerSatisfaction,
            ////        Impacttype = String.IsNullOrEmpty(dr.Incident.ImpactType) ? "NA" : dr.Incident.ImpactType,
            ////        Configitemid = String.IsNullOrEmpty(dr.Incident.ConfigItemId) ? "NA" : dr.Incident.ConfigItemId,
            ////        Configtype = String.IsNullOrEmpty(dr.Incident.ConfigType) ? "NA" : dr.Incident.ConfigType,
            ////        Configitemname = String.IsNullOrEmpty(dr.Incident.ConfigItemName) ? "NA" : dr.Incident.ConfigItemName,
            ////        Environmentimpacted = String.IsNullOrEmpty(dr.Incident.EnvironmentImpacted) ? "NA" : dr.Incident.EnvironmentImpacted,
            ////        Nodename = String.IsNullOrEmpty(dr.Incident.NodeName) ? "NA" : dr.Incident.NodeName,
            ////        Rootcausetype = String.IsNullOrEmpty(dr.Incident.RootCauseType) ? "NA" : dr.Incident.RootCauseType,
            ////        Rootcausedescription = String.IsNullOrEmpty(dr.Incident.RootcauseDescription) ? "NA" : dr.Incident.RootcauseDescription,
            ////        Resolutiontype = String.IsNullOrEmpty(dr.Incident.ResolutionType) ? "NA" : dr.Incident.ResolutionType,
            ////        Solutionid = String.IsNullOrEmpty(dr.Incident.SolutionId) ? "NA" : dr.Incident.SolutionId,
            ////        Deploymentrelease = String.IsNullOrEmpty(dr.Incident.DeploymentRelease) ? "NA" : dr.Incident.DeploymentRelease,
            ////        Relatedchangeids = String.IsNullOrEmpty(dr.Incident.RelatedChangeIds) ? "NA" : dr.Incident.RelatedChangeIds,
            ////        Relatedids = String.IsNullOrEmpty(dr.Incident.RelatedIds) ? "NA" : dr.Incident.RelatedIds,
            ////        Relatedproblemids = String.IsNullOrEmpty(dr.Incident.RelatedProblemids) ? "NA" : dr.Incident.RelatedProblemids,
            ////        Helpdesk = String.IsNullOrEmpty(dr.Incident.Helpdesk) ? "NA" : dr.Incident.Helpdesk,
            ////        Assignedtoperson = String.IsNullOrEmpty(dr.Incident.AssignedToPerson) ? "NA" : dr.Incident.AssignedToPerson,
            ////        Assignedtosupportgroup = String.IsNullOrEmpty(dr.Incident.AssignedToSupportGroup) ? "NA" : dr.Incident.AssignedToSupportGroup,
            ////        Createdbygroup = String.IsNullOrEmpty(dr.Incident.CreatedByGroup) ? "NA" : dr.Incident.CreatedByGroup,
            ////        Createdbyperson = String.IsNullOrEmpty(dr.Incident.CreatedByPerson) ? "NA" : dr.Incident.CreatedByPerson,
            ////        Createdtimezone = String.IsNullOrEmpty(dr.Incident.CreatedTimezone) ? "NA" : dr.Incident.CreatedTimezone,
            ////        Createdtime = String.IsNullOrEmpty(dr.Incident.CreatedTime) ? "NA" : dr.Incident.CreatedTime,
            ////        Createdweekday = String.IsNullOrEmpty(dr.Incident.CreatedWeekday) ? "NA" : dr.Incident.CreatedWeekday,
            ////        Createdweeknum = String.IsNullOrEmpty(dr.Incident.CreatedWeeknum) ? "NA" : dr.Incident.CreatedWeeknum,
            ////        Createdmonth = String.IsNullOrEmpty(dr.Incident.CreatedMonth) ? "NA" : dr.Incident.CreatedMonth,
            ////        Createdyear = String.IsNullOrEmpty(dr.Incident.CreatedYear) ? "NA" : dr.Incident.CreatedYear,
            ////        Acknowlededbygroup = String.IsNullOrEmpty(dr.Incident.AcknowlededByGroup) ? "NA" : dr.Incident.AcknowlededByGroup,
            ////        Acknowlededbyperson = String.IsNullOrEmpty(dr.Incident.AcknowlededByPerson) ? "NA" : dr.Incident.AcknowlededByPerson,
            ////        Acknowledgeddatetime = dr.Incident.AcknowledgedDateTime == null ? DateTime.MinValue : dr.Incident.AcknowledgedDateTime.Value,
            ////        Acknowledgedtimezone = String.IsNullOrEmpty(dr.Incident.AcknowledgedTimeZone) ? "NA" : dr.Incident.AcknowledgedTimeZone,
            ////        Acknowledgedtime = String.IsNullOrEmpty(dr.Incident.AcknowledgedTime) ? "NA" : dr.Incident.AcknowledgedTime,
            ////        Acknowledgedweekday = String.IsNullOrEmpty(dr.Incident.Acknowledgedweekday) ? "NA" : dr.Incident.Acknowledgedweekday,
            ////        Acknowledgedweeknum = String.IsNullOrEmpty(dr.Incident.Acknowledgedweeknum) ? "NA" : dr.Incident.Acknowledgedweeknum,
            ////        Acknowledgedmonth = String.IsNullOrEmpty(dr.Incident.Acknowledgedmonth) ? "NA" : dr.Incident.Acknowledgedmonth,
            ////        Acknowledgedyear = String.IsNullOrEmpty(dr.Incident.Acknowledgedyear) ? "NA" : dr.Incident.Acknowledgedyear,
            ////        Resolvedbygroup = String.IsNullOrEmpty(dr.Incident.ResolvedByGroup) ? "NA" : dr.Incident.ResolvedByGroup,
            ////        Resolvedbyperson = String.IsNullOrEmpty(dr.Incident.Resolvedbyperson) ? "NA" : dr.Incident.Resolvedbyperson,
            ////        Resolvedbyvendor = String.IsNullOrEmpty(dr.Incident.ResolvedByVendor) ? "NA" : dr.Incident.ResolvedByVendor,
            ////        Resolutionefforthours = string.IsNullOrEmpty(dr.Incident.ResolutionEfforthours) ? "NA" : dr.Incident.ResolutionEfforthours,
            ////        Resolutiondatetime = dr.Incident.ResolutionDateTime == null ? DateTime.MinValue : dr.Incident.ResolutionDateTime,
            ////        Resolvedtimezone = string.IsNullOrEmpty(dr.Incident.ResolvedTimezone) ? "NA" : dr.Incident.ResolvedTimezone,
            ////        Resolutiontime = string.IsNullOrEmpty(dr.Incident.ResolutionTime.ToString()) ? 0 : Convert.ToInt32(dr.Incident.ResolutionTime),
            ////        Resolutionweekday = string.IsNullOrEmpty(dr.Incident.ResolutionWeekday) ? "NA" : dr.Incident.ResolutionWeekday,
            ////        Resolutionweeknum = string.IsNullOrEmpty(dr.Incident.ResolutionWeeknum) ? "NA" : dr.Incident.ResolutionWeeknum,
            ////        Servicerestoredbygroup = string.IsNullOrEmpty(dr.Incident.ServiceRestoredByGroup) ? "NA" : dr.Incident.ServiceRestoredByGroup,
            ////        Servicerestoredbyperson = string.IsNullOrEmpty(dr.Incident.ServiceRestoredbyperson) ? "NA" : dr.Incident.ServiceRestoredbyperson,
            ////        Servicerestoreddatetime = dr.Incident.ServiceRestoredDateTime == null ? DateTime.MinValue : dr.Incident.ServiceRestoredDateTime,
            ////        Servicerestoredtimezone = string.IsNullOrEmpty(dr.Incident.ServiceRestoredTimeZone) ? "NA" : dr.Incident.ServiceRestoredTimeZone,
            ////        Servicerestoredtime = string.IsNullOrEmpty(dr.Incident.ServiceRestoredTime) ? "NA" : dr.Incident.ServiceRestoredTime,
            ////        Servicerestoredweekday = string.IsNullOrEmpty(dr.Incident.ServiceRestoredWeekday) ? "NA" : dr.Incident.ServiceRestoredWeekday,
            ////        Servicerestoredweeknum = string.IsNullOrEmpty(dr.Incident.ServiceRestoredWeeknum) ? "NA" : dr.Incident.ServiceRestoredWeeknum,
            ////        Servicerestoredmonth = string.IsNullOrEmpty(dr.Incident.ServiceRestoredMonth) ? "NA" : dr.Incident.ServiceRestoredMonth,
            ////        Servicerestoredyear = string.IsNullOrEmpty(dr.Incident.ServiceRestoredYear) ? "NA" : dr.Incident.ServiceRestoredYear,
            ////        Lastupdatedbyperson = string.IsNullOrEmpty(dr.Incident.LastUpdatedByPerson) ? "NA" : dr.Incident.LastUpdatedByPerson,
            ////        Lastupdateddatetime = dr.Incident.LastUpdatedDateTime == null ? DateTime.MinValue : dr.Incident.LastUpdatedDateTime.Value,
            ////        Datasource = string.IsNullOrEmpty(dr.Incident.DataSource) ? "NA" : dr.Incident.DataSource,
            ////        Datasourcenotes = string.IsNullOrEmpty(dr.Incident.DataSourceNotes) ? "NA" : dr.Incident.DataSourceNotes,
            ////        Assignedbygroup = string.IsNullOrEmpty(dr.Incident.AssignedByGroup) ? "NA" : dr.Incident.AssignedByGroup,
            ////        Assignedbyperson = string.IsNullOrEmpty(dr.Incident.AssignedByPerson) ? "NA" : dr.Incident.AssignedByPerson,
            ////        Assigneddatetime = dr.Incident.AssignedDateTime == null ? DateTime.MinValue : dr.Incident.AssignedDateTime,
            ////        Esitmatedresponsetimebygroup = "NA", //sNullOrEmpty(dr.Esitmatedresponsetimebygroup)? "NA" : dr.Esitmatedresponsetimebygroup,
            ////        Esitmatedresponsedatetime = dr.Incident.EsitmatedResponseDateTime == null ? DateTime.MinValue : dr.Incident.EsitmatedResponseDateTime,
            ////        Resolutionyear = string.IsNullOrEmpty(dr.Incident.ResolutionYear) ? "NA" : dr.Incident.ResolutionYear,
            ////        Resolutionmonth = string.IsNullOrEmpty(dr.Incident.ResolutionMonth) ? "NA" : dr.Incident.ResolutionMonth,
            ////        Waivedindicator = string.IsNullOrEmpty(dr.Incident.Waivedindicator) ? "NA" : dr.Incident.Waivedindicator,
            ////        Esitmatedresolutiondate = dr.Incident.EsitmatedResolutionDate == null ? DateTime.MinValue : dr.Incident.EsitmatedResolutionDate,
            ////        Totalsuspenddurationassignedtoack = dr.Incident.TotalSuspendDurationAssignedtoack <= 0 ? 0 : dr.Incident.TotalSuspendDurationAssignedtoack.Value,
            ////        Totalsuspenddurationacktoer = dr.Incident.TotalSuspendDurationacktoer <= 0 ? 0 : dr.Incident.TotalSuspendDurationacktoer.Value,
            ////        Totalsuspenddurationertoresolution = dr.Incident.TotalSuspendDurationertoreSolution <= 0 ? 0 : dr.Incident.TotalSuspendDurationertoreSolution.Value,
            ////        Resolvedindicator = string.IsNullOrEmpty(dr.Incident.Resolvedindicator) ? "NA" : dr.Incident.Resolvedindicator,
            ////        IsNTTGroup = string.IsNullOrEmpty(dr.Incident.IsNTTGroup) ? "NA" : dr.Incident.IsNTTGroup,
            ////        SupportGroupType = string.IsNullOrEmpty(dr.Incident.SupportGroupType) ? "NA" : dr.Incident.SupportGroupType,
            ////        InProgressdatetime = dr.Incident.InProgressDateTime==null ? DateTime.MinValue : dr.Incident.InProgressDateTime,
            ////        Closeddatetime = dr.Incident.ClosedDateTime==null ? DateTime.MinValue : Convert.ToDateTime(dr.Incident.ClosedDateTime),
            ////        ScheduledstartdateTime = dr.Incident.ScheduledStartDateTime==null ? DateTime.MinValue : Convert.ToDateTime(dr.Incident.ScheduledStartDateTime),
            ////        ScheduledenddateTime = dr.Incident.ScheduledEndDateTime==null ? DateTime.MinValue : Convert.ToDateTime(dr.Incident.ScheduledEndDateTime),
            ////        Scheduledchangecount = dr.Incident.ScheduledChangeCount==null ? 0 : Convert.ToInt32(dr.Incident.ScheduledChangeCount),
            ////        Company = string.IsNullOrEmpty(dr.Incident.Company) ? "NA" : dr.Incident.Company,
            ////        Site = string.IsNullOrEmpty(dr.Incident.Site) ? "NA" : dr.Incident.Site,
            ////        Assigneecompany = string.IsNullOrEmpty(dr.Incident.AssigneeCompany) ? "NA" : dr.Incident.AssigneeCompany,
            ////        Assigneeorganization = string.IsNullOrEmpty(dr.Incident.AssigneeOrganization) ? "NA" : dr.Incident.AssigneeOrganization,
            ////        Estimatedworkeffort = dr.Incident.EstimatedWorkEffort==null ? 0 : Convert.ToInt32(dr.Incident.EstimatedWorkEffort),
            ////        Actualworkeffort = dr.Incident.ActualWorkEffort == null? 0 : Convert.ToDecimal(dr.Incident.ActualWorkEffort),
            ////        ActualstartdateTime = dr.Incident.ActualStartDateTime==null ? DateTime.MinValue : Convert.ToDateTime(dr.Incident.ActualStartDateTime),
            ////        Recyclecount = dr.Incident.Recyclecount==null ? 0 : Convert.ToInt32(dr.Incident.Recyclecount),
            ////        Statusreason = string.IsNullOrEmpty(dr.Incident.StatusReason) ? "NA" : dr.Incident.StatusReason,
            ////        ConfigItemCategory = string.IsNullOrEmpty(dr.Incident.ConfigItemCategory) ? "NA" : dr.Incident.ConfigItemCategory,
            ////        ResponseTime = dr.Incident.ResponseTime ==null ? 0 : Convert.ToInt32(dr.Incident.ResponseTime),
            ////        ResponseTimeMetSLA = string.IsNullOrEmpty(dr.Incident.ResponseTimeMetSLA) ? "NA" : dr.Incident.ResponseTimeMetSLA,
            ////        ResolutionTimeMetSLA = string.IsNullOrEmpty(dr.Incident.ResolutionTimeMetSLA) ? "NA" : dr.Incident.ResolutionTimeMetSLA,
            ////        EstimateResponseTime = dr.Incident.EstimateResponseTime==null ? 0 : Convert.ToInt32(dr.Incident.EstimateResponseTime),
            ////        ClosureCategory = string.IsNullOrEmpty(dr.Incident.ClosureCategory) ? "NA" : dr.Incident.ClosureCategory,
            ////        EstimatedResponseTimeMetSLA = string.IsNullOrEmpty(dr.Incident.EstimatedResponseTimeMetSLA) ? "NA" : dr.Incident.EstimatedResponseTimeMetSLA,
            ////        EstResptarget = string.IsNullOrEmpty(dr.Incident.EstResptarget) ? "NA" : dr.Incident.EstResptarget,
            ////        Reslntargettime = string.IsNullOrEmpty(dr.Incident.ReslntargetTime) ? "NA" : dr.Incident.ReslntargetTime,
            ////        ResolutionExceptionIndicator = string.IsNullOrEmpty(dr.Incident.ResolutionExceptionIndicator) ? "NA" : dr.Incident.ResolutionExceptionIndicator,
            ////        ResolutionExceptionReason = string.IsNullOrEmpty(dr.Incident.ResolutionExceptionReason) ? "NA" : dr.Incident.ResolutionExceptionReason,
            ////        ResponseExceptionIndicator = string.IsNullOrEmpty(dr.Incident.ResponseExceptionIndicator) ? "NA" : dr.Incident.ResponseExceptionIndicator,
            ////        ResponseExceptionReason = string.IsNullOrEmpty(dr.Incident.ResponseExceptionReason) ? "NA" : dr.Incident.ResponseExceptionReason,
            ////        Resptargettime = string.IsNullOrEmpty(dr.Incident.ResptargetTime) ? "NA" : dr.Incident.ResptargetTime,
            ////        Severity = string.IsNullOrEmpty(dr.Incident.Severity) ? "NA" : dr.Incident.Severity,
            ////        WorkGroupLevel = string.IsNullOrEmpty(dr.Incident.WorkGroupLevel) ? "NA" : dr.Incident.WorkGroupLevel,
            ////        AffectedItemName = string.IsNullOrEmpty(dr.Incident.AffectedItemName) ? "NA" : dr.Incident.AffectedItemName,
            ////        SLACostPerTicket = dr.Incident.SLACostPerTicket == null ? 0 : dr.Incident.SLACostPerTicket.Value,
            ////        INResolutiontime = dr.Incident.INResolutionTime ==null ? 0 : dr.Incident.INResolutionTime.Value,
            ////        INCostPerTicket = dr.Incident.INCostPerTicket ==null ? 0 : dr.Incident.INCostPerTicket.Value,
            ////        AccountName = string.IsNullOrEmpty(dr.AccountName) ? "NA" : dr.AccountName,
            ////        ContractName = string.IsNullOrEmpty(dr.ContractName) ? "NA" : dr.ContractName
            ////}).ToList();
            //   return dt;

        }



        public static DataTable INTrendingReportList(int ClientId, DateTime start, DateTime end)
        {
            IList<TrendingReportInput> Tredingdata = new List<TrendingReportInput>();
            var paramName = new string[] { "ClientId", "FromDate", "ToDate" };
            var paramValue = new object[] { ClientId, start, end };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                dt = unitOfWork.GetDataFromStoredProcedure("INTrendingReportDateRange", paramName, paramValue);
            }
            return dt;
        }


        public static DataTable INInflowOutflowAnalysis(int ClientId)
        {
            IList<TrendingReportInput> Tredingdata = new List<TrendingReportInput>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                dt = unitOfWork.GetDataFromStoredProcedure("InflowOutflowAnalysis", paramName, paramValue);
            }
            return dt;
        }

        public static DataTable INTrendingReportListWithCluster(int ClientId)
        {
            IList<TrendingReportInput> Tredingdata = new List<TrendingReportInput>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                dt = unitOfWork.GetDataFromStoredProcedure("INTrendingReportWithCluster", paramName, paramValue);
            }
            return dt;
        }
        public static IList<DashboardTrendModel> INNewDashboardTicketTrend(string Charttype, int ClientId, string TicketType, string Status, string Track, string Priority, string Datetype, DateTime StartDate, DateTime EndDate)
        {
            IList<DashboardTrendModel> Modellist = new List<DashboardTrendModel>();
            var paramName = new string[] { "Charttype", "ClientId", "TicketType", "Status", "Track", "Priority", "Datetype", "StartDate", "EndDate" };
            var paramValue = new object[] { Charttype, ClientId, TicketType, Status, Track, Priority, Datetype, StartDate, EndDate };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("INNewDashboardTicketTrend", paramName, paramValue);

                //var res1 = unitOfWork.GetDataFromStoredProcedure("INDashboardTicketTrendOutflow", paramName, paramValue);
                //res1.PrimaryKey = new DataColumn[] { res1.Columns["ResolutionyearMonth"] };

                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DashboardTrendModel();


                    coldet.yearMonth = dr["yearMonth"].ToString();

                    try
                    {

                        coldet.IncidentCount = Convert.ToInt32(dr["IncidentCount"]);
                        // coldet.ResolvedIncidentCount = Convert.ToInt32(rows[0]["ResolvedIncidentCount"]);
                    }

                    catch (Exception ex)
                    {
                        Log.Error("IncidentNavigatorHelper error 'INNewDashboardTicketTrend' ", ex);
                    }
                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }
        public static IList<INChartDashboardModel> INDashboardTrendChartData(int ClientId, string Charttype, string Datetype, DateTime StartDate, DateTime EndDate, string TicketType, string Status, string Track, string Priority, string contract)
        {
            IList<INChartDashboardModel> Modellist = new List<INChartDashboardModel>();
            var paramName = new string[] { "ClientId", "StartDate", "EndDate", "Dim", "Fieldname", "TicketType", "Status", "Track", "Priority", "contract" };
            var paramValue = new object[] { ClientId, StartDate, EndDate, Charttype, Datetype, TicketType, Status, Track, Priority, contract };

            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("SP_Dashboard1TicketCount_Prod", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new INChartDashboardModel();


                    coldet.IncidentCount = Convert.ToInt32(dr["TicketCount"]);

                    try
                    {
                        var temp = 0;
                        if (Charttype == "Day")
                        {
                            coldet.DrpCategory = dr["Dim_Value"].ToString();
                            temp = coldet.DrpCategory.IndexOf(" ");
                            coldet.DrpCategory = coldet.DrpCategory.Substring(0, temp);
                        }
                        else
                        {
                            coldet.DrpCategory = dr["Dim_Value"].ToString();
                        }
                        coldet.ResolvedCount = Convert.ToInt32(dr["ResolvedCount"]);
                    }
                    catch (Exception ex)
                    {
                        Log.Error("IncidentNavigatorHelper error 'INDashboardTrendChartData' ", ex);
                    }
                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }
        public static IList<INChartDashboardModel> INDashboard2TrendChartData(int ClientId, string drpval, string TicketType, string Status, string Track, string Priority, string dpnVal, string Datetype, DateTime StartDate, DateTime EndDate, string contract)
        {
            IList<INChartDashboardModel> Modellist = new List<INChartDashboardModel>();
            var paramName = new string[] { "ClientId", "StartDate", "EndDate", "Dim", "Fieldname", "TicketType", "Status", "Track", "Priority", "dpnVal", "contract" };
            var paramValue = new object[] { ClientId, StartDate, EndDate, drpval, Datetype, TicketType, Status, Track, Priority, dpnVal, contract };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("SP_Dashboard2TicketCount_prod", paramName, paramValue);
                if (dpnVal.Length > 0)
                {
                    foreach (DataRow dr in res.Rows)
                    {
                        var coldet = new INChartDashboardModel();
                        coldet.IncidentCount = Convert.ToInt32(dr["TicketCount"]);

                        try
                        {

                            coldet.DrpCategory = dr["Category"].ToString();
                            coldet.Dim = dr["Dim_Value"].ToString(); ;// dr["Category"].ToString();
                        }
                        catch (Exception ex)
                        {
                            Log.Error("IncidentNavigatorHelper error 'INDashboard2TrendChartData' ", ex);
                        }
                        Modellist.Add(coldet);
                    }
                }
                else
                {
                    foreach (DataRow dr in res.Rows)
                    {
                        var coldet = new INChartDashboardModel();
                        coldet.IncidentCount = Convert.ToInt32(dr["TicketCount"]);

                        try
                        {

                            coldet.Dim = dr["Dim_Value"].ToString(); ;// dr["Category"].ToString();
                        }
                        catch (Exception ex)
                        {
                            Log.Error("IncidentNavigatorHelper error 'INDashboard2TrendChartData' ", ex);
                        }
                        Modellist.Add(coldet);
                    }
                }

            }
            if (drpval == "Month" || drpval == "Date")
            {
                foreach (var item in Modellist)
                {
                    item.LongCategory = item.Dim;
                    item.Dim = item.Dim.Substring(4);

                }
            }
            return Modellist;
        }

        public static IList<DashboardTrendModel> INDashboardTicketTrend(int ClientId, string TicketType, string ConfigItemName, DateTime StartDate, DateTime EndDate)
        {
            IList<DashboardTrendModel> Modellist = new List<DashboardTrendModel>();
            var paramName = new string[] { "ClientId", "TicketType", "ConfigItemName", "StartDate", "EndDate" };
            var paramValue = new object[] { ClientId, TicketType, ConfigItemName, StartDate, EndDate };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("INDashboardTicketTrend", paramName, paramValue);

                var res1 = unitOfWork.GetDataFromStoredProcedure("INDashboardTicketTrendOutflow", paramName, paramValue);
                res1.PrimaryKey = new DataColumn[] { res1.Columns["ResolutionyearMonth"] };

                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DashboardTrendModel();


                    coldet.yearMonth = dr["yearMonth"].ToString();

                    // var  d = res1.Rows.Find("ResolutionyearMonth = " + coldet.yearMonth);
                    // DataRow drr = res1.AsEnumerable()
                    //.SingleOrDefault(r => r.Field<String>("ResolutionyearMonth") == coldet.yearMonth);
                    DataRow[] rows = res1.Select("ResolutionyearMonth = " + "'" + dr["yearMonth"].ToString() + "'");
                    //   DataRow[] result = res1.Select("ResolutionyearMonth" = dr["yearMonth"].ToString());
                    //  var drr = res1.AsEnumerable().Where(row => row["ResolutionyearMonth"].ToString() == coldet.yearMonth).FirstOrDefault();

                    try
                    {

                        coldet.IncidentCount = Convert.ToInt32(dr["IncidentCount"]);
                        coldet.ResolvedIncidentCount = Convert.ToInt32(rows[0]["ResolvedIncidentCount"]);
                    }

                    catch (Exception ex)
                    {
                        Log.Error("IncidentNavigatorHelper error 'INDashboardTicketTrend' ", ex);
                    }
                    Modellist.Add(coldet);
                }

            }

            //using (var contextFactory1 = new DbContextFactory())
            //{
            //    var unitOfWork = new UnitOfWorks(contextFactory1.GetContext());
            //    var res = unitOfWork.GetDataFromStoredProcedure("INDashboardTicketTrendOutflow", paramName, paramValue);
            //    foreach (DataRow dr in res.Rows)
            //    {
            //        if(Modellist.Any(c=>c.yearMonth == dr["ResolutionyearMonth"].ToString()))

            //        {
            //            DashboardTrendModel v = Modellist.Any(c => c.yearMonth == dr["ResolutionyearMonth"].ToString());

            //            try
            //            {

            //               v. = Convert.ToInt16(dr["ResolvedIncidentCount"]);
            //            }

            //            catch { }
            //        }





            //    }

            //}

            return Modellist;
        }


        public static IList<DashboardTrendModel> INDashboardTopWorkgroups(int ClientId, string TicketType, string ConfigItemName, DateTime StartDate, DateTime EndDate)
        {
            IList<DashboardTrendModel> Modellist = new List<DashboardTrendModel>();
            var paramName = new string[] { "ClientId", "TicketType", "ConfigItemName", "StartDate", "EndDate" };
            var paramValue = new object[] { ClientId, TicketType, ConfigItemName, StartDate, EndDate };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("INDashboardTopWorkgroups", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DashboardTrendModel();


                    coldet.Assignedtosupportgroup = dr["Assignedtosupportgroup"].ToString();



                    try
                    {

                        coldet.WorkgroupIncidentCount = Convert.ToInt32(dr["IncidentCount"]);
                    }

                    catch (Exception ex)
                    {
                        Log.Error("IncidentNavigatorHelper error 'INDashboardTopWorkgroups' ", ex);
                    }
                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }


        public static IList<DashboardTrendModel> INDashboardTopholdtimeUnresolved(int ClientId, string TicketType, string ConfigItemName, DateTime StartDate, DateTime EndDate)
        {
            IList<DashboardTrendModel> Modellist = new List<DashboardTrendModel>();
            var paramName = new string[] { "ClientId", "TicketType", "ConfigItemName", "StartDate", "EndDate" };
            var paramValue = new object[] { ClientId, TicketType, ConfigItemName, StartDate, EndDate };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("INDashboardTop5Holdtime", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DashboardTrendModel();


                    coldet.IncidentId = dr["IncidentId"].ToString();



                    try
                    {

                        coldet.Holdtime = Convert.ToInt32(dr["HoldTime"]);
                    }

                    catch (Exception ex)
                    {
                        Log.Error("IncidentNavigatorHelper error 'INDashboardTopholdtimeUnresolved' ", ex);
                    }
                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }

        public static IList<DashboardTrendModel> INDashboardSeverity(int ClientId, string TicketType, string ConfigItemName)
        {
            IList<DashboardTrendModel> Modellist = new List<DashboardTrendModel>();
            var paramName = new string[] { "ClientId", "TicketType", "ConfigItemName" };
            var paramValue = new object[] { ClientId, TicketType, ConfigItemName };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("INDashboardSeverity", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DashboardTrendModel();


                    coldet.Severity = dr["Severity"].ToString();



                    try
                    {

                        coldet.SeverityIncidentCount = Convert.ToInt32(dr["IncidentCount"]);
                    }

                    catch (Exception ex)
                    {
                        Log.Error("IncidentNavigatorHelper error 'INDashboardSeverity' ", ex);
                    }
                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }


        public static IList<DashboardTrendModel> INDashboardCluster(int ClientId, string TicketType, string ConfigItemName, DateTime StartDate, DateTime EndDate)
        {
            IList<DashboardTrendModel> Modellist = new List<DashboardTrendModel>();
            var paramName = new string[] { "ClientId", "TicketType", "ConfigItemName", "StartDate", "EndDate" };
            var paramValue = new object[] { ClientId, TicketType, ConfigItemName, StartDate, EndDate };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("INDashboardCluster", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DashboardTrendModel();


                    coldet.ClusterLabel = dr["ClusterLabel"].ToString();



                    try
                    {

                        coldet.TotalTickets = Convert.ToInt32(dr["TotalTickets"]);
                    }

                    catch (Exception ex)
                    {
                        Log.Error("IncidentNavigatorHelper error 'INDashboardCluster' ", ex);
                    }
                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }


        public static IList<DashboardTrendModel> INDashboardClusterDrilldown(int ClientId, string TicketType, string ConfigItemName, DateTime StartDate, DateTime EndDate, string Clusterlabel)
        {
            IList<DashboardTrendModel> Modellist = new List<DashboardTrendModel>();
            var paramName = new string[] { "ClientId", "TicketType", "ConfigItemName", "StartDate", "EndDate", "Clusterlabel" };
            var paramValue = new object[] { ClientId, TicketType, ConfigItemName, StartDate, EndDate, Clusterlabel };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("INDashboardClusterDrilldown", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DashboardTrendModel();

                    coldet.Description = dr["Description"].ToString();
                    coldet.Type = dr["Type"].ToString();
                    coldet.Configitemname = dr["Configitemname"].ToString();
                    coldet.Assignedtoperson = dr["Assignedtoperson"].ToString();
                    coldet.Assignedtosupportgroup = dr["Assignedtosupportgroup"].ToString();
                    coldet.Status = dr["Status"].ToString();
                    coldet.IncidentId = dr["IncidentId"].ToString();
                    coldet.Priority = dr["Priority"].ToString();
                    coldet.ClusterLabel = dr["ClusterLabel"].ToString();
                    coldet.TopWords = dr["TopWords"].ToString();


                    try
                    {

                        //  coldet.IncidentId = Convert.ToInt16(dr["IncidentId"]);
                    }

                    catch (Exception ex)
                    {
                        Log.Error("IncidentNavigatorHelper error 'INDashboardClusterDrilldown' ", ex);
                    }
                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }

        public static IList<DashboardTrendModel> INDashboardTicketTrendOutflowDrilldown(int ClientId, string TicketType, string ConfigItemName, DateTime StartDate, DateTime EndDate, String YearMonth)
        {
            IList<DashboardTrendModel> Modellist = new List<DashboardTrendModel>();
            var paramName = new string[] { "ClientId", "TicketType", "ConfigItemName", "StartDate", "EndDate", "YearMonth" };
            var paramValue = new object[] { ClientId, TicketType, ConfigItemName, StartDate, EndDate, YearMonth };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("INDashboardTicketTrendOutflowDrilldown", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DashboardTrendModel();

                    coldet.Description = dr["Description"].ToString();
                    coldet.Type = dr["Type"].ToString();
                    coldet.Configitemname = dr["Configitemname"].ToString();
                    coldet.Assignedtoperson = dr["Assignedtoperson"].ToString();
                    coldet.Assignedtosupportgroup = dr["Assignedtosupportgroup"].ToString();
                    coldet.Status = dr["Status"].ToString();
                    coldet.IncidentId = dr["IncidentId"].ToString();
                    coldet.Priority = dr["Priority"].ToString();


                    try
                    {

                        coldet.Createddatetime = Convert.ToDateTime(dr["Createddatetime"]);
                        coldet.Resolutiondatetime = Convert.ToDateTime(dr["Resolutiondatetime"]);
                    }

                    catch (Exception ex)
                    {
                        Log.Error("IncidentNavigatorHelper error 'INDashboardTicketTrendOutflowDrilldown' ", ex);
                    }
                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }


        public static IList<DashboardTrendModel> INDashboardTicketTrendInflowDrilldown(int ClientId, string TicketType, string ConfigItemName, DateTime StartDate, DateTime EndDate, string YearMonth)
        {
            IList<DashboardTrendModel> Modellist = new List<DashboardTrendModel>();
            var paramName = new string[] { "ClientId", "TicketType", "ConfigItemName", "StartDate", "EndDate", "YearMonth" };
            var paramValue = new object[] { ClientId, TicketType, ConfigItemName, StartDate, EndDate, YearMonth };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("INDashboardTicketTrendInflowDrilldown", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DashboardTrendModel();

                    coldet.Description = dr["Description"].ToString();
                    coldet.Type = dr["Type"].ToString();
                    coldet.Configitemname = dr["Configitemname"].ToString();
                    coldet.Assignedtoperson = dr["Assignedtoperson"].ToString();
                    coldet.Assignedtosupportgroup = dr["Assignedtosupportgroup"].ToString();
                    coldet.Status = dr["Status"].ToString();
                    coldet.IncidentId = dr["IncidentId"].ToString();
                    coldet.Priority = dr["Priority"].ToString();


                    try
                    {

                        coldet.Createddatetime = Convert.ToDateTime(dr["Createddatetime"]);
                        coldet.Resolutiondatetime = Convert.ToDateTime(dr["Resolutiondatetime"]);
                    }

                    catch (Exception ex)
                    {
                        Log.Error("IncidentNavigatorHelper error 'INDashboardTicketTrendInflowDrilldown' ", ex);
                    }
                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }

        public static IList<DashboardTrendModel> INDashboardTop5HoldtimeDrilldown(String IncidentId)
        {
            IList<DashboardTrendModel> Modellist = new List<DashboardTrendModel>();
            var paramName = new string[] { "IncidentId" };
            var paramValue = new object[] { IncidentId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("INDashboardTop5HoldtimeDrilldown", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DashboardTrendModel();

                    coldet.Description = dr["Description"].ToString();
                    coldet.Type = dr["Type"].ToString();
                    coldet.Configitemname = dr["Configitemname"].ToString();
                    coldet.Assignedtoperson = dr["Assignedtoperson"].ToString();
                    coldet.Assignedtosupportgroup = dr["Assignedtosupportgroup"].ToString();
                    coldet.Status = dr["Status"].ToString();
                    coldet.IncidentId = dr["IncidentId"].ToString();
                    coldet.Priority = dr["Priority"].ToString();


                    try
                    {

                        coldet.Createddatetime = Convert.ToDateTime(dr["Createddatetime"]);
                        coldet.Lastupdateddatetime = Convert.ToDateTime(dr["Lastupdateddatetime"]);
                        coldet.Totalsuspenddurationertoresolution = Convert.ToInt32(dr["Totalsuspenddurationertoresolution"]);
                    }

                    catch (Exception ex)
                    {
                        Log.Error("IncidentNavigatorHelper error 'INDashboardTop5HoldtimeDrilldown' ", ex);
                    }
                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }

        public static IList<DashboardTrendModel> INYearmonthForDashboard(int ClientId)
        {
            IList<DashboardTrendModel> Modellist = new List<DashboardTrendModel>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("INYearmonthForDashboard", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DashboardTrendModel();


                    coldet.ddlyearMonth = dr["yearMonth"].ToString();




                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }



        public static IList<DashboardTrendModel> INDashboardAvgResolutionCost(int ClientId, string TicketType, string ConfigItemName, DateTime StartDate, DateTime EndDate)
        {
            IList<DashboardTrendModel> Modellist = new List<DashboardTrendModel>();
            var paramName = new string[] { "ClientId", "TicketType", "ConfigItemName", "StartDate", "EndDate" };
            var paramValue = new object[] { ClientId, TicketType, ConfigItemName, StartDate, EndDate };
            DataTable res = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                //res = unitOfWork.GetDataFromStoredProcedure("INDashboardAvgResolutionCost", paramName, paramValue);

                var item = unitOfWork.INIncidentServicehours.Get(x => x.ClientId == ClientId).FirstOrDefault();
                if (item != null)
                {
                    if (item.HasSLAManager != true)
                    {
                        using (var db1 = new DbContextFactory())
                        {
                            var DB1 = new UnitOfWorks(db1.GetContext());
                            res = DB1.GetDataFromStoredProcedure("INDashboardAvgResolutionCost", paramName, paramValue);
                        }
                    }
                    else
                    {
                        using (var db2 = new DbContextFactory())
                        {
                            var DB2 = new UnitOfWorks(db2.GetContext());
                            res = DB2.GetDataFromStoredProcedure("INDashboardAvgResolutionCostUsingSLAdata", paramName, paramValue);
                        }
                    }
                }
                else
                {
                    using (var db3 = new DbContextFactory())
                    {
                        var DB3 = new UnitOfWorks(db3.GetContext());
                        res = DB3.GetDataFromStoredProcedure("INDashboardAvgResolutionCostUsingSLAdata", paramName, paramValue);
                    }
                }
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DashboardTrendModel();


                    coldet.ddlyearMonth = dr["yearMonth"].ToString();


                    try
                    {

                        coldet.AvgResolutionP1 = Convert.ToDecimal(dr["AvgINResolutiontimeP1"]);
                        coldet.AvgResolutionP2 = Convert.ToDecimal(dr["AvgINResolutiontimeP2"]);
                        coldet.AvgResolutionP3 = Convert.ToDecimal(dr["AvgINResolutiontimeP3"]);
                        coldet.AvgResolutionP4 = Convert.ToDecimal(dr["AvgINResolutiontimeP4"]);
                        coldet.AvgCostP1 = Convert.ToDecimal(dr["AvgINCostPerTicketP1"]);
                        coldet.AvgCostP2 = Convert.ToDecimal(dr["AvgINCostPerTicketP2"]);
                        coldet.AvgCostP3 = Convert.ToDecimal(dr["AvgINCostPerTicketP3"]);
                        coldet.AvgCostP4 = Convert.ToDecimal(dr["AvgINCostPerTicketP4"]);
                    }

                    catch (Exception ex)
                    {
                        Log.Error("IncidentNavigatorHelper error 'INDashboardAvgResolutionCost' ", ex);
                    }

                    Modellist.Add(coldet);
                }
                return Modellist;
            }


        }



        public static void ExecuteINResolutionCalc(int ClientId)
        {

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                var paramNameset1 = new string[] { "ClientId" };
                var paramValueset1 = new object[] { ClientId };
                unitOfWork.ExecProcedure("ResolutionTimeCalcNew", paramNameset1, paramValueset1);

            }
        }


        public static void ExecuteINBlendedRateCalc(int ClientId)
        {

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                var paramNameset1 = new string[] { "ClientId" };
                var paramValueset1 = new object[] { ClientId };
                unitOfWork.ExecProcedure("INResolutionTimeBlendedRateCalc", paramNameset1, paramValueset1);

            }
        }

        public static IList<ClusterAuditModel> INGetClusterAuditlist(int ClientId)
        {
            IList<ClusterAuditModel> Clusterlist = new List<ClusterAuditModel>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };


            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("INClusterAudit", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new ClusterAuditModel();


                    coldet.IncidentId = dr["IncidentId"].ToString();
                    coldet.ColumnName = dr["ColumnName"].ToString();
                    coldet.OldValue = dr["OldValue"].ToString();
                    coldet.NewValue = dr["NewValue"].ToString();
                    coldet.Action = dr["Action"].ToString();
                    coldet.UserName = dr["UserName"].ToString();
                    coldet.IPAddress = dr["IPAddress"].ToString();


                    try
                    {
                        coldet.Timestamp = Convert.ToDateTime(dr["Timestamp"]);
                        //coldet.AvgCostPerTicket = Convert.ToDecimal(dr["AvgSLACostPerTicket"]);
                        //coldet.TotalIncidents = Convert.ToInt16(dr["TotalIncidents"]);
                    }

                    catch (Exception ex)
                    {
                        Log.Error("IncidentNavigatorHelper error 'INGetClusterAuditlist' ", ex);
                    }
                    Clusterlist.Add(coldet);
                }

            }
            return Clusterlist;
        }


        public static IList<TrendingReportInput> INCubeSourceColumnList(int ClientId)
        {
            IList<TrendingReportInput> Modellist = new List<TrendingReportInput>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("INCubeSourceColumnList", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new TrendingReportInput();


                    coldet.ToolColumnName = dr["ToolColumnName"].ToString();
                    coldet.ClientColumnName = dr["ClientColumnName"].ToString();



                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }
        // kedb helper (to send data to kedb db)
        public static bool insertkedbinfo(KnownErrorDBModel KEDBinput)
        {
            var entityMapper = new Mapper<KEDB, KnownErrorDBModel>();
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                    var dnkedb = new KEDB();

                    dnkedb.KEDID = KEDBinput.KEDID;
                    dnkedb.ClientId = KEDBinput.ClientId;
                    dnkedb.Cause = KEDBinput.Cause;
                    dnkedb.ResolutionSteps = KEDBinput.ResolutionSteps;
                    dnkedb.Escalations = KEDBinput.Escalations;
                    dnkedb.SolutionApproved = KEDBinput.SolutionApproved; /*!Convert.ToBoolean(dr["isNullable"]);*/
                    dnkedb.Author = KEDBinput.Author;
                    dnkedb.CIName = KEDBinput.CIName;
                    dnkedb.BusinessFunction = KEDBinput.BusinessFunction;
                    dnkedb.TicketidClusters = KEDBinput.TicketidClusters;

                    if (KEDBinput.httpPostedTemplateFile != null)
                    {
                        var s = KEDBinput.httpPostedTemplateFile.FileName;
                        Stream filestream = KEDBinput.httpPostedTemplateFile.InputStream;
                        BinaryReader br = new BinaryReader(filestream);
                        Byte[] bytes = br.ReadBytes((Int32)filestream.Length);
                        dnkedb.KEDBFile = bytes;
                        dnkedb.FileName = s;
                    }
                    unitOfWork.KEDB.Insert(dnkedb);

                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("IncidentNavigatorHelper error 'insertkedbinfo' ", ex);
            }

                return false;
        }
        //updates kedb data
        public static bool updatekedbinfo(KnownErrorDBModel KEDBinput)
        {
            var entityMapper = new Mapper<KEDB, KnownErrorDBModel>();
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                    // var dnkedb = new KEDB();
                    //   KEDB dnkedb = new KEDB();

                    //  entityMapper.MapToEntity(KEDBinput, dnkedb);
                    var dnkedb = unitOfWork.KEDB.GetByID(KEDBinput.Id);
                    //  dnkedb.Id = KEDBinput.Id;
                    dnkedb.KEDID = KEDBinput.KEDID;
                    dnkedb.ClientId = KEDBinput.ClientId;
                    dnkedb.Cause = KEDBinput.Cause;
                    dnkedb.ResolutionSteps = KEDBinput.ResolutionSteps;
                    dnkedb.Escalations = KEDBinput.Escalations;
                    dnkedb.SolutionApproved = KEDBinput.SolutionApproved;
                    dnkedb.Author = KEDBinput.Author;
                    dnkedb.CIName = KEDBinput.CIName;
                    dnkedb.BusinessFunction = KEDBinput.BusinessFunction;
                    dnkedb.TicketidClusters = KEDBinput.TicketidClusters;

                    if (KEDBinput.httpPostedTemplateFile != null)
                    {


                        var s = KEDBinput.httpPostedTemplateFile.FileName;
                        Stream filestream = KEDBinput.httpPostedTemplateFile.InputStream;
                        BinaryReader br = new BinaryReader(filestream);
                        Byte[] bytes = br.ReadBytes((Int32)filestream.Length);
                        dnkedb.KEDBFile = bytes;
                        dnkedb.FileName = s;

                    }

                    unitOfWork.KEDB.Update(dnkedb);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("IncidentNavigatorHelper error 'updatekedbinfo' ", ex);
            }
                return false;
            
        }

        //retrieve data from kedb db
        public static IPagedList<KnownErrorDBModel> GetKEDBListDetails(int clientId, string searchCriteria, int? page)
        {
            if (!string.IsNullOrEmpty(searchCriteria))
            {

                Db db = new Db();
                var kedbList = (from k in db.KEDB
                                where k.ClientId == clientId &&
                                (k.BusinessFunction.Contains(searchCriteria)
                                || k.CIName.Contains(searchCriteria) || k.Cause.Contains(searchCriteria)
                                || k.ResolutionSteps.Contains(searchCriteria))
                                select new KnownErrorDBModel
                                {
                                    Id = k.Id,
                                    ClientId = k.ClientId,
                                    KEDID = k.KEDID,
                                    Cause = k.Cause,
                                    ResolutionSteps = k.ResolutionSteps,
                                    Escalations = k.Escalations,
                                    SolutionApproved = k.SolutionApproved,
                                    Author = k.Author,
                                    CIName = k.CIName,
                                    BusinessFunction = k.BusinessFunction,
                                    TicketidClusters = k.TicketidClusters,
                                    KEDBFile = k.KEDBFile,
                                    KEDBFilename = k.FileName,
                                    Filename = k.FileName,
                                    Search = searchCriteria
                                }).OrderBy(o => o.Id).ToPagedList(page ?? 1, 7);

                return kedbList;

            }
            else
            {
                return new List<KnownErrorDBModel>().ToPagedList(1, 7);
            }

        }



        //retrieve data from kedbincident
        public static IList<KEDBIncidentModel> GetKedbIncListDetails()
        {
            IList<KEDBIncidentModel> kiList = new List<KEDBIncidentModel>();

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                var Kdetails = unitOfWork.KEDBIncident.Get();

                foreach (var item in Kdetails)
                {
                    KEDBIncidentModel kedbincident = new KEDBIncidentModel();

                    kedbincident.KEDBID = item.KEDBID;
                    kedbincident.IncidentID = item.IncidentID;
                    kiList.Add(kedbincident);
                }
            }

            return kiList;
        }
        public static KnownErrorDBModel EditKEDBDetails(int id)
        {
            KnownErrorDBModel kedb = new KnownErrorDBModel();

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var Cdetails = unitOfWork.KEDB.GetByID(id);

                //KnownErrorDBModel kedb = new KnownErrorDBModel();
                kedb.Id = Cdetails.Id;
                kedb.ClientId = Cdetails.ClientId;
                kedb.KEDID = Cdetails.KEDID;
                kedb.Cause = Cdetails.Cause;
                kedb.ResolutionSteps = Cdetails.ResolutionSteps;
                kedb.Escalations = Cdetails.Escalations;
                kedb.SolutionApproved = Cdetails.SolutionApproved;
                kedb.Author = Cdetails.Author;
                kedb.CIName = Cdetails.CIName;
                kedb.BusinessFunction = Cdetails.BusinessFunction;
                kedb.TicketidClusters = Cdetails.TicketidClusters;
                kedb.KEDBFile = Cdetails.KEDBFile;
                kedb.KEDBFilename = Cdetails.FileName;
                kedb.Filename = Cdetails.FileName;
            }
            return kedb;
        }

        public static bool SaveKEDBIncidents(string selectedINs, int KEDBId)
        {
            if (string.IsNullOrEmpty(selectedINs))
            {
                return false;
            }
            string[] strINs = selectedINs.Split(new char[] { ',' });
            bool ret = false;
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                foreach (var strIN in strINs)
                {
                    int inci = Convert.ToInt32(strIN);
                    var addedIncident = unitOfWork.KEDBIncident.Get(G => G.IncidentID == inci && G.KEDBID == KEDBId).FirstOrDefault();
                    if (addedIncident != null)
                        continue;
                    var kedbincident = new KEDBIncident();
                    kedbincident.IncidentID = Convert.ToInt32(strIN);
                    kedbincident.KEDBID = KEDBId;
                    unitOfWork.KEDBIncident.Insert(kedbincident);
                }
                unitOfWork.Save();
                ret = true;
            }
            return ret;
        }


        public static bool DeleteKedbIncidents(int kedbincidentId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                //   var KEDBIncident = unitOfWork.KEDBIncident.Get().Where(w => w.KEDBID == kedbid && w.IncidentID == incidentId).FirstOrDefault();
                unitOfWork.KEDBIncident.Delete(kedbincidentId);
                unitOfWork.Save();
                return true;
            }
        }



        public static bool AddKeyWord(KeywordSearchModel keysearch)
        {
            //var entityMapper = new Mapper<KeywordSearch, KeywordSearchModel>();
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    if (!string.IsNullOrEmpty(keysearch.WordValue) && !unitOfWork.KeywordSearch.Get().Where(w => w.WordValue.ToLower() == keysearch.WordValue.ToLower()).Any())
                    {
                        KeywordSearch ks = new KeywordSearch();
                        ks.WordValue = keysearch.WordValue;
                        ks.isdeleted = false;
                        ks.CreatedDate = keysearch.CreatedDate;
                        ks.UpdatedDate = keysearch.UpdatedDate;
                        ks.WordDescription = keysearch.WordValue;
                        ks.ClientId = keysearch.ClientId;
                        ks.WordType = "custom";
                        ks.WordLanguage = keysearch.WordLanguage;
                        unitOfWork.KeywordSearch.Insert(ks);
                        unitOfWork.Save();
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("IncidentNavigatorHelper error 'AddKeyWord' ", ex);
            }
            
                return false;
            
        }

        public static string GetWordLang(int clientid)
        {
            string lang = null;
            //List<KeywordListInput> SearchRes = new List<KeywordListInput>();
            using (var contextFactory = new DbContextFactory())
            {

                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                string[] parmname = new string[] { "clientid" };
                var values = new object[] { clientid };
                var t = unitOfWork.GetDataFromStoredProcedure("GetWordLangPro", parmname, values);
                // var t = unitOfWork.INClusterConfigurationValue.Get().Where(e => e.ClientId == clientid && e.ParameterName.ToLower() == "language").FirstOrDefault();
                if (t != null)
                {
                    foreach (DataRow dr in t.Rows)
                    {
                        //  var sModel = new KeywordListInput();
                        // sModel.WordLanguage = dr["ParameterValue"].ToString();
                        lang = dr["ParameterValue"].ToString();

                    }
                }


            }
            return lang;

            //    string[] parmname = new string[] { "clientid" };
            //    var values = new object[] { clientid };
            //    var dtSearchRes = unitOfWork.GetDataFromStoredProcedure("GetWordLangPro", parmname, values);
            //    foreach (DataRow dr in dtSearchRes.Rows)
            //    {
            //        var sModel = new KeywordListInput();
            //        sModel.WordLanguage = dr["ParameterValue"].ToString();
            //        SearchRes.Add(sModel);
            //    }
            //}
            //return SearchRes;
        }


        public static IList<KeywordListInput> GetClientWordValue(int clientid)
        {
            List<KeywordListInput> SearchRes = new List<KeywordListInput>();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                string[] parmname = new string[] { "clientid" };
                var values = new object[] { clientid };
                var dtSearchRes = unitOfWork.GetDataFromStoredProcedure("GetWordListPro", parmname, values);
                foreach (DataRow dr in dtSearchRes.Rows)
                {
                    var sModel = new KeywordListInput();
                    sModel.keywords = dr["WordValue"].ToString();
                    sModel.WordType = dr["WordType"].ToString();
                    SearchRes.Add(sModel);
                }
                return SearchRes;
            }
        }
        public static IList<KeywordListInput> New_GetClientWordValue(int clientid)
        {
            List<KeywordListInput> SearchRes = new List<KeywordListInput>();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                string[] parmname = new string[] { "clientid" };
                var values = new object[] { clientid };
                var dtSearchRes = unitOfWork.GetDataFromStoredProcedure("INWordAnalysisDescForAllWords", parmname, values);
                foreach (DataRow dr in dtSearchRes.Rows)
                {
                    var sModel = new KeywordListInput();
                    sModel.keywords = dr["WordValue"].ToString();
                    sModel.WordType = dr["WordType"].ToString();
                    sModel.WordTotInc = dr["WordIncidentCount"].ToString();
                    //sModel.WordDtInc = dr["WorddateIncCount"].ToString();
                    sModel.TotalInc = dr["totalincident"].ToString();

                    SearchRes.Add(sModel);
                }
                return SearchRes;
            }
        }
        public static IList<KeywordListInput> GetClientSpecificWordValue(int clientid)
        {
            List<KeywordListInput> SearchRes = new List<KeywordListInput>();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                string[] parmname = new string[] { "clientid" };
                var values = new object[] { clientid };
                var dtSearchRes = unitOfWork.GetDataFromStoredProcedure("GetClusterWordList", parmname, values);
                foreach (DataRow dr in dtSearchRes.Rows)
                {
                    var sModel = new KeywordListInput();
                    sModel.keywords = dr["clusterword"].ToString();
                    SearchRes.Add(sModel);
                }
                return SearchRes;
            }
        }
        public static IList<INDictionaryList> GetClientExcludedWord(int clientid)
        {
            List<INDictionaryList> ExcWords = new List<INDictionaryList>();
            Db db = new Db();
            var exwords = (from a in db.INDictionary
                           where a.ClientId == clientid
                           select a).ToList();
            foreach (var a in exwords)
            {
                var user = (from b in db.UserProfiles
                            where b.Id == a.ExcludedBy
                            select new
                            {
                                fName = b.FirstName,
                                lName = b.LastName
                            }).FirstOrDefault();
                INDictionaryList wordList = new INDictionaryList();
                wordList.Id = a.Id;
                wordList.ClientId = a.ClientId.Value;
                wordList.Word = a.Word;
                if (user != null)
                {
                    wordList.ExcludedBy = user.fName + " " + user.lName;
                }
                else
                {
                    wordList.ExcludedBy = "NA";

                }
                if (a.ExcludedDate != null)
                {
                    wordList.ExcludedDate = a.ExcludedDate.Value.ToShortDateString();
                }
                else
                {
                    wordList.ExcludedDate = "NA";
                }
                ExcWords.Add(wordList);
            }
            return ExcWords;
        }
        public static void AddClientExcludedWord(string word,string delimiter, int uid, int clientid)
        {
            Db db = new Db();
            INClusterListModel mod = new INClusterListModel();
            mod.listofExcludedwords = IncidentNavigatorHelper.GetClientExcludedWord(clientid);
            List<string> wordList = new List<string>();
            foreach(var a in mod.listofExcludedwords)
            {
                wordList.Add(a.Word);
            }
            if (delimiter == "#" || delimiter == "," || delimiter == "|" || delimiter == ";")
            {
                List<string> words = new List<string>();
                words = word.Split(delimiter.ToCharArray()[0]).ToList();
                foreach(var w in words)
                {
                    if (wordList.Contains(w))
                    {
                        continue;
                    }
                    else
                    {
                        INDictionary excword = new INDictionary();
                        excword.Word = w;
                        excword.ClientId = clientid;
                        excword.ExcludedBy = uid;
                        excword.ExcludedDate = DateTime.Now;
                        db.INDictionary.Add(excword);
                        db.SaveChanges();
                    }
                    
                }

            }
            else
            {
                if (!wordList.Contains(word))
                {
                    INDictionary exword = new INDictionary();
                    exword.Word = word;
                    exword.ClientId = clientid;
                    exword.ExcludedBy = uid;
                    exword.ExcludedDate = DateTime.Now;
                    db.INDictionary.Add(exword);
                    db.SaveChanges();
                }
                
            }
            
        }
        public static void DeleteClientExcludedWord(int id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.INDictionary.Delete(id);
                unitOfWork.Save();
            }
        }

        public static List<AlphabetList> GetKeyWordList(int clientid)
        {
            List<AlphabetList> model = new List<AlphabetList>();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                string[] parmname = new string[] { "clientid" };
                var values = new object[] { clientid };
                var dtSearchRes = unitOfWork.GetDataFromStoredProcedure("INGetWordCountAndWordIncidents", parmname, values);
                foreach (DataRow dr in dtSearchRes.Rows)
                {
                    var sModel = new AlphabetList();
                    sModel.A_bet = dr["FirstLetter"].ToString();
                    sModel.NoAlphaWords = dr["words"].ToString();
                    sModel.NoTotAlphaINC = dr["wordsincident"].ToString();
                    model.Add(sModel);
                }

            }
            return model;
        }


        public static List<KeywordReportListModel> GetReportList(int clientid, string Wordvalues)
        {
            List<KeywordReportListModel> keyword = new List<KeywordReportListModel>();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                string[] parmname = new string[] { "clientid", "Wordvalues" };
                var values = new object[] { clientid, Wordvalues };
                var dtSearchRes = unitOfWork.GetDataFromStoredProcedure("INWordAnalysisDescForWords", parmname, values);
                foreach (DataRow dr in dtSearchRes.Rows)
                {
                    var krModel = new KeywordReportListModel();
                    krModel.Id = (int)dr["Id"];
                    krModel.WordType = (string)dr["WordType"];
                    krModel.TotalIncidentCount = (int)dr["totalincident"];
                    krModel.WordTotIncCount = (int)dr["WordIncidentCount"];
                    //   krModel.WordDtIncCount = (int)dr["WordateIncCount"];
                    krModel.WordIncidentPer = (decimal)dr["wordincidentper"];
                    krModel.WordValue = (string)dr["WordValue"];
                    keyword.Add(krModel);
                }
            }
            return keyword;
        }
        public static bool RemoveKeyword(string selectedINs)
        {
            if (string.IsNullOrEmpty(selectedINs))
            {
                return false;
            }
            string[] strINs = selectedINs.Split(new char[] { ',' });
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                foreach (var strIN in strINs)
                {

                    unitOfWork.KeywordSearch.Delete(Convert.ToInt32(strIN));
                }

                unitOfWork.Save();

                //TempData["SLACreatedDeleted"] = "Deleted successfully.";
                return true;
            }

        }

        public static IList<SLAMaintenenceModel> KeywordDrillDownData(int clientid, string WordValue)
        {
            IList<SLAMaintenenceModel> Modellist = new List<SLAMaintenenceModel>();
            var paramName = new string[] { "ClientId", "WordValue" };
            var paramValue = new object[] { clientid, WordValue };
            DataTable dt = new DataTable();
            DataTable res = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                res = unitOfWork.GetDataFromStoredProcedure("KeywordData_drilldown", paramName, paramValue);
            }
            foreach (DataRow dr in res.Rows)
            {
                var coldet = new SLAMaintenenceModel();
                coldet.Id = Convert.ToInt32(dr["ID"]);
                coldet.TicketID = dr["IncidentId"].ToString();
                if (!(dr["Type"] is DBNull))
                    coldet.Type = dr["Type"].ToString();
                coldet.Description = dr["Description"].ToString();
                coldet.Priority = dr["Priority"].ToString();
                if (!(dr["Status"] is DBNull))
                    coldet.Status = dr["Status"].ToString();
                if (!(dr["AssignedToSupportGroup"] is DBNull))
                    coldet.AssignedToSupportGroup = dr["AssignedToSupportGroup"].ToString();
                if (!(dr["AssignedDateTime"] is DBNull))
                    coldet.AssignedDateTime = Convert.ToDateTime(dr["AssignedDateTime"]);
                if (!(dr["ClosedDateTime"] is DBNull))
                    coldet.ClosedDateTime = Convert.ToDateTime(dr["ClosedDateTime"]);
                if (!(dr["CreatedDateTime"] is DBNull))
                    coldet.CreatedDateTime = Convert.ToDateTime(dr["CreatedDateTime"]);
                if (!(dr["LastUpdatedDateTime"] is DBNull))
                    coldet.LastUpdatedDateTime = Convert.ToDateTime(dr["LastUpdatedDateTime"]);
                if (!(dr["ResolutionDateTime"] is DBNull))
                    coldet.ResolutionDateTime = Convert.ToDateTime(dr["ResolutionDateTime"]);
                if (!(dr["Resolutiontype"] is DBNull))
                    coldet.Resolutiontype = dr["Resolutiontype"].ToString();
                if (!(dr["Tower"] is DBNull))
                    coldet.Track = dr["Tower"].ToString();
                Modellist.Add(coldet);
            }
            return Modellist;
        }

        public static List<KeywordConfigModel> GetKeywordCountByCI(int clientid, string WordValue)
        {
            List<KeywordConfigModel> keyword = new List<KeywordConfigModel>();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                string[] parmname = new string[] { "clientid", "WordValue" };
                var values = new object[] { clientid, WordValue };
                var dtSearchRes = unitOfWork.GetDataFromStoredProcedure("INGetWordAnalysisDescByCI ", parmname, values);
                int idval = 0;
                foreach (DataRow dr in dtSearchRes.Rows)
                {
                    idval++;
                    var krModel = new KeywordConfigModel();
                    krModel.ConfigId = idval;// (int)dr["Id"];
                    krModel.WordValueID = WordValue;
                    if (dr["Configitemname"] != null)
                    {
                        krModel.ByConfigItem = (string)dr["Configitemname"];
                    }
                    if (dr["CIWordIncidentCount"] != null)
                    {
                        krModel.CiWordIncidentCount = (int)dr["CIWordIncidentCount"];
                    }
                    if (dr["CIWordIncidentPer"] != null)
                    {
                        krModel.CiWordIncidentPer = (decimal)dr["CIWordIncidentPer"];
                    }

                    keyword.Add(krModel);
                }
            }
            return keyword;
        }

        public static List<KeywordIncidentModel> GetIncidents(int clientid, string WordValue1, string ByConfigItem)
        {
            List<KeywordIncidentModel> keyword = new List<KeywordIncidentModel>();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                string[] parmname = new string[] { "clientid", "WordValue", "ConfigitemName" };
                var values = new object[] { clientid, WordValue1, ByConfigItem };
                var dtSearchRes = unitOfWork.GetDataFromStoredProcedure("INGetIncidentDetails ", parmname, values);
                int idval = 0;
                foreach (DataRow dr in dtSearchRes.Rows)
                {
                    idval++;
                    var krModel = new KeywordIncidentModel();
                    krModel.Id = idval;// (int)dr["Id"];
                    if (dr["ID"] != null)
                    {
                        krModel.Incidentid = (string)dr["ID"];
                    }
                    if (dr["Title"] != null)
                    {
                        krModel.Title = (string)dr["Title"];
                    }
                    if (dr["Status"] != null)
                    {
                        krModel.Status = (string)dr["Status"];
                    }
                    if (dr["ConfigItem"] != null)
                    {
                        krModel.ConfigItem = (string)dr["ConfigItem"];
                    }
                    if (dr["CreatedDate"] != null)
                    {
                        krModel.CreatedDate = dr["CreatedDate"].ToString();
                    }
                    if (dr["ResolvedDate"] != null)
                    {
                        krModel.ResolvedDate = dr["ResolvedDate"].ToString();
                    }

                    keyword.Add(krModel);
                }
            }
            return keyword;
        }

        public static IEnumerable<SelectListItem> getINReportList()
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var Templist = unitOfWork.INReport.Get().ToList();
            var list = Templist.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.ReportName });
            return list;
        }

        public static IList<INClientReportModel> getAllReport(int ClientId)
        {

            IList<INClientReportModel> aprInterfaceModels = new List<INClientReportModel>();
            var entityMapper = new Mapper<INClientReportModel, INClientReport>();

            using (var contextFactory = new DbContextFactory())
            {
                // IEnumerable<INClientReport> INReportlist = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var db = new Db();
                var reportlist = (from crp in db.INClientReport
                                  join rp in db.INReport on crp.INReportId equals rp.Id
                                  where ClientId == crp.ClientId
                                  select new { Id = crp.Id, INReportId = rp.Id, ReportName = rp.ReportName, ClientId = crp.ClientId }).ToList();


                foreach (var app in reportlist)
                {
                    INClientReportModel pai = new INClientReportModel();
                    pai.Id = app.Id;
                    pai.ClientId = app.ClientId;
                    pai.INReportId = app.INReportId;
                    pai.ReportName = app.ReportName;
                    //pai.Reportinfo = new SelectListItem();
                    //pai.Reportinfo.Value = app.ApplicationInterfaced.ToString();
                    //pai.appinfo.Text = app.AppInterfacedText;

                    aprInterfaceModels.Add(pai);
                }

                return aprInterfaceModels;

            }
        }

        public static int AddReport(INClientReportModel reportModel, int ClientId, int uid)
        {
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var AnalysisExists = unitOfWork.INClientReport.Get(g => g.CreatedBy == uid && g.INReportId == reportModel.INReportId).FirstOrDefault();
                    if (AnalysisExists != null)
                    {
                        return 0;
                    }
                    else
                    {
                        var cns = new INClientReport();
                        cns.ClientId = ClientId;
                        cns.INReportId = reportModel.INReportId;
                        cns.CreatedDt = DateTime.Now;
                        cns.UpdatedDt = DateTime.Now;
                        cns.UpdatedBy = uid;
                        cns.CreatedBy = uid;
                        unitOfWork.INClientReport.Insert(cns);
                        unitOfWork.Save();
                        id = cns.Id;
                        return id;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("IncidentNavigatorHelper error 'AddReport' ", ex);
            }
            return 0;
        }

        public static bool DeleteReport(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.INClientReport.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }


        public static void saveObservation(int ClientId, string Range = "", int uid = 0, string Observation = "", int ReportId = 0, string Type = "")
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

            INReportObservation inrc = new INReportObservation();
            inrc.ClientId = ClientId;
            inrc.Observation = Observation;
            inrc.ObservationType = Type;
            inrc.ReportId = ReportId;
            inrc.DateRange = Range;
            inrc.userId = uid;
            inrc.CreatedDt = DateTime.Now;
            inrc.UpdatedBy = uid;
            inrc.UpdatedDt = DateTime.Now;
            unitOfWork.INReportObservation.Insert(inrc);
            unitOfWork.Save();
        }

        //public static string readObservation(string range,int uid)
        //{
        //    var contextFactory = new DbContextFactory();
        //    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
        //    INReportObservationModel model = new INReportObservationModel();

        //    var CommonDetails = unitOfWork.INReportObservation.Get(g => g.userId == uid&&g.DateRange== range).FirstOrDefault();
        //    if (CommonDetails != null)
        //    {
        //        //Common Details
        //        model.Observation = CommonDetails.Observation;
        //    }
        //    var c = model.Observation;
        //    return c;

        //}
        public static IList<INReportObservationModel> ReadObservation(int ClientId)
        {
            IList<INReportObservationModel> Observation = new List<INReportObservationModel>();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var db = new Db();
                var obserList = (from rpo in db.INReportObservation
                                 join rp in db.INReport on rpo.ReportId equals rp.Id
                                 join usr in db.UserProfiles on rpo.userId equals usr.Id
                                 where ClientId == rpo.ClientId && rpo.ObservationType == "Observation"
                                 select new { Id = rpo.Id, ReportName = rp.ReportName, DateRange = rpo.DateRange, Observation = rpo.Observation, CreatorName = usr.FirstName + " " + usr.LastName }).ToList();




                if (obserList != null)
                {
                    foreach (var _observation in obserList)
                    {
                        INReportObservationModel Obse = new INReportObservationModel();
                        Obse.Id = _observation.Id;
                        Obse.ReportName = _observation.ReportName;
                        Obse.CreatedByName = _observation.CreatorName;
                        Obse.DateRange = _observation.DateRange;
                        Obse.Observation = _observation.Observation;
                        Observation.Add(Obse);
                    }
                }
                return Observation;
            }
        }


        //---------------------------------------Metadata helpers-----------------------------------------//
        //------------------------------------------------------------------------------------------------//

        public static IEnumerable<IMTeamModel> readTrack(int ClientId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var disp = unitOfWork.IMTeam.Get(g => g.ClientId == ClientId);
            var returnModel = disp.Select(e => new IMTeamModel
            {
                Id = e.Id,
                Team = e.Team

            });
            // var list = temp.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Team });
            return returnModel;
        }
        public static List<AssignGroupModel> readAssignGroup(int Towerid)
        {
            Db db = new Db();
            var temp = db.INAssigneeGroup.Where(f => f.TowerId == Towerid).Select(e => new AssignGroupModel
            {
                ASId = e.Id,
                AssignGroup = e.AssignGroup,
                TowerId = e.TowerId
            }).ToList();
            return temp;
        }

        public static int addTrack(TowerModel model)
        {
            var returnId = 0;
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var entity = new INBusinessTower()
                {
                    ClientId = model.ClientId,
                    CreatedBy = model.CreatedBy,
                    UpdatedBy = model.UpdatedBy,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    TowerName = model.TowerName.Trim()
                };
                unitOfWork.INBusinessTower.Insert(entity);
                unitOfWork.Save();
                returnId = entity.Id;
            }
            return returnId;
        }

        public static int addAssignGroup(AssignGroupModel model)
        {
            var returnId = 0;
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var entity = new INAssigneeGroup()
                {
                    TowerId = model.TowerId.Value,
                    CreatedBy = model.CreatedBy,
                    UpdatedBy = model.UpdatedBy,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    AssignGroup = model.AssignGroup.Trim()
                };
                unitOfWork.INAssigneeGroup.Insert(entity);
                unitOfWork.Save();
                returnId = entity.Id;
            }
            return returnId;
        }


        public static void updateTrack(TowerModel model)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var entity = new INBusinessTower()
                {
                    UpdatedBy = model.UpdatedBy,
                    UpdatedDate = DateTime.Now,
                    TowerName = model.TowerName.Trim()
                };
                unitOfWork.INBusinessTower.Update(entity);
                unitOfWork.Save();
            }
        }

        public static void updateAssignGroup(AssignGroupModel model)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var entity = new INAssigneeGroup()
                {
                    UpdatedBy = model.UpdatedBy,
                    UpdatedDate = DateTime.Now,
                    AssignGroup = model.AssignGroup.Trim()
                };
                unitOfWork.INAssigneeGroup.Update(entity);
                unitOfWork.Save();
            }
        }


        public static void deleteTrack(int TrackId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var AssignIds = unitOfWork.INAssigneeGroup.Get(g => g.TowerId == TrackId);
                foreach (var i in AssignIds)
                {
                    unitOfWork.INAssigneeGroup.Delete(i.Id);
                }

                unitOfWork.INBusinessTower.Delete(TrackId);
                unitOfWork.Save();
            }
        }
        public static void deleteAssignGroup(int AssignGroupId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.INAssigneeGroup.Delete(AssignGroupId);
                unitOfWork.Save();
            }
        }

        public static IEnumerable<SelectListItem> getColumnList(int ClientId)
        {
            IList<TeamRuleModel> AppColumnDetail = new List<TeamRuleModel>();
            var paramName = new string[] { "tableName" };
            var paramValue = new object[] { "Incident" };

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                var res = unitOfWork.GetDataFromStoredProcedure("GetTableColumnNameswithFilterForTower", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new TeamRuleModel();
                    var FieldDatatpe = dr["DataType"].ToString();

                    //if (FieldDatatpe == "nvarchar" || FieldDatatpe == "varchar")
                    //{
                    //var FieldLength = Convert.ToInt32(dr["Size"]);
                    //if (FieldLength < 260)
                    //{
                    coldet.INFieldName = dr["AppField"].ToString();
                    AppColumnDetail.Add(coldet);
                    //}
                    // }

                }

            }
            var list = AppColumnDetail.Select(f => new SelectListItem { Value = f.INFieldName, Text = f.INFieldName });

            return list;
        }


        public static List<TeamRuleModel> getMappedColumnValuesList(int ClId)
        {
            List<TeamRuleModel> AppColumnDetail = new List<TeamRuleModel>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClId };

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                var res = unitOfWork.GetDataFromStoredProcedure("INSourceMappingColumnForChart", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new TeamRuleModel();
                    coldet.INFieldName = dr["ToolColumnName"].ToString();
                    // coldet.FieldValue = dr["ClientColumnName "].ToString();


                    if (!String.IsNullOrWhiteSpace(coldet.INFieldName))
                    {
                        AppColumnDetail.Add(coldet);
                    }
                }
            }
            // var list = AppColumnDetail.Select(f => new SelectListItem { Value = f.FieldValue, Text = f.FieldValue });
            return AppColumnDetail;
        }








        public static List<TeamRuleModel> getColumnValuesList(int ClId, string Columnid)
        {
            List<TeamRuleModel> AppColumnDetail = new List<TeamRuleModel>();
            var paramName = new string[] { "ClientId", "ColumnName" };
            var paramValue = new object[] { ClId, Columnid };

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                var res = unitOfWork.GetDataFromStoredProcedure("GetColumnValue", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new TeamRuleModel();
                    coldet.FieldValue = dr["AppValue"].ToString();
                    if (!String.IsNullOrWhiteSpace(coldet.FieldValue))
                    {
                        AppColumnDetail.Add(coldet);
                    }
                }
            }
            // var list = AppColumnDetail.Select(f => new SelectListItem { Value = f.FieldValue, Text = f.FieldValue });
            return AppColumnDetail;
        }





        public static IList<TeamRuleModel> getRuleList(int TrackId)
        {

            IList<TeamRuleModel> EstCategoryList = new List<TeamRuleModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<TeamRule> RowDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (TrackId != 0)
                    RowDetails = unitOfWork.TeamRule.Get(cid => cid.TeamId == TrackId).ToList();

                if (RowDetails != null)
                {
                    foreach (var _detail in RowDetails)
                    {
                        TeamRuleModel bb = new TeamRuleModel();
                        bb.TId = _detail.Id;
                        bb.INFieldName = _detail.INFieldName;
                        bb.FieldValue = _detail.FieldValue;
                        bb.Operator = _detail.Operator;
                        bb.GroupEndOperator = _detail.GroupEndOperator;
                        bb.GroupStartOperator = _detail.GroupStartOperator;
                        bb.Condition = _detail.Condition;
                        bb.ConstantValue = _detail.ConstantValue;
                        EstCategoryList.Add(bb);
                    }
                }
                return EstCategoryList;
            }

        }
        public static int addTeamRule(TeamRuleModel Input)
        {
            int ra_id = 0;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var _Details = new TeamRule();
                    _Details.TeamId = Input.TeamId;
                    _Details.INFieldName = Input.INFieldName;
                    _Details.FieldValue = Input.FieldValue;
                    _Details.Operator = Input.Operator;
                    _Details.GroupEndOperator = Input.GroupEndOperator;
                    _Details.GroupStartOperator = Input.GroupStartOperator;
                    _Details.Condition = Input.Condition;
                    _Details.ConstantValue = Input.ConstantValue;
                    unitOfWork.TeamRule.Insert(_Details);
                    unitOfWork.Save();
                    ra_id = _Details.Id;

                    return ra_id;
                }
            }
            catch (Exception ex)
            {
                Log.Error("IncidentNavigatorHelper error 'addTeamRule' ", ex);
            }
            return 0;
        }



        public static bool updateTeamRule(TeamRuleModel _Input)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.TeamRule.GetByID(_Input.TId);
                    updatetitem.INFieldName = _Input.INFieldName;
                    updatetitem.FieldValue = _Input.FieldValue;
                    updatetitem.Operator = _Input.Operator;
                    updatetitem.GroupEndOperator = _Input.GroupEndOperator;
                    updatetitem.GroupStartOperator = _Input.GroupStartOperator;
                    updatetitem.Condition = _Input.Condition;
                    updatetitem.ConstantValue = _Input.ConstantValue;
                    unitOfWork.TeamRule.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("IncidentNavigatorHelper error 'updateTeamRule' ", ex);
            }
            return false;
        }


        public static bool RemoveTeamRule(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.TeamRule.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }


        public static string GenerateNExecRule(int clientid, string strcommand, int uid)
        {

            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            if (strcommand == "SAVE")
            {
                var param1 = new string[] { "ClientId", "Type" };
                var paramVal = new object[] { clientid, "SSISUPDATE" };
                var result = unitOfWork.GetDataFromStoredProcedure("DeleteSSISUpdateQuery", param1, paramVal);
            }
            // var ssisupdatequery = new SSIS_INUpdateQuery { ClientId = clientid, Type = "SSISUPDATE", Query = updateStatement };
            // unitOfWork.SSIS_INUpdateQuery.Insert(ssisupdatequery);
            var db = new Db();
            var TeamRuledata = (from tm in db.IMTeam
                                join tr in db.TeamRule on tm.Id equals tr.TeamId
                                where tm.ClientId == clientid
                                select new { ruleid = tr.Id, Team = tm.Team, Column = tr.INFieldName, value = tr.FieldValue, Operator = tr.Operator, StartBraces = tr.GroupStartOperator, EndBraces = tr.GroupEndOperator, Condition = tr.Condition, ConstantValue = tr.ConstantValue }).OrderBy(e => e.Team).ToList();
            var TeamList = TeamRuledata.Select(s => s.Team).Distinct().ToList();
            string DisplayQuery = "";
            var updateStatement = "";
            //  var DisplayQuery = "";
            foreach (var item in TeamList)
            {
                updateStatement = "UPDATE Incident set Tower='" + item + "' where clientid=" + clientid.ToString() + " AND (";

                if (TeamRuledata.Count() > 0)
                {
                    var TeamRule = TeamRuledata.Where(w => w.Team == item).OrderBy(o => o.ruleid).ToList();
                    var count = TeamRule.Count();
                    int i = 0;
                    foreach (var dr in TeamRule)
                    {
                        i++;
                        if (i == count)
                        {
                            if (string.IsNullOrEmpty(dr.value) && string.IsNullOrEmpty(dr.ConstantValue))
                            {
                                //string tempCond = "";
                                if (dr.Condition == "=" || dr.Condition == "IS NULL")
                                    updateStatement = updateStatement = updateStatement + dr.StartBraces + " ( " + dr.Column + " IS NULL OR " + dr.Column + "='') " + dr.EndBraces;
                                else if (dr.Condition == "<>" || dr.Condition == "IS NOT NULL")
                                    updateStatement = updateStatement = updateStatement + dr.StartBraces + " ( " + dr.Column + " IS NOT NULL AND " + dr.Column + "<>'' ) " + dr.EndBraces;
                            }
                            else
                            {
                                if (dr.Condition == "=" || dr.Condition == "<>")
                                {
                                    if (!string.IsNullOrEmpty(dr.value))
                                        updateStatement = updateStatement + dr.StartBraces + dr.Column + dr.Condition + " '" + dr.value + "' " + dr.EndBraces;
                                    else
                                        updateStatement = updateStatement + dr.StartBraces + dr.Column + dr.Condition + dr.ConstantValue + dr.EndBraces;
                                }
                                else if (dr.Condition == "CONTAINS")
                                {
                                    if (!string.IsNullOrEmpty(dr.value))
                                    {
                                        updateStatement = updateStatement + dr.StartBraces + dr.Column + " LIKE '%" + dr.value + "%' " + dr.EndBraces;
                                    }
                                    else
                                    {
                                        updateStatement = updateStatement + dr.StartBraces + dr.Column + " LIKE '%" + dr.ConstantValue + "%' " + dr.EndBraces;
                                    }
                                }
                                else if (dr.Condition == "NOT CONTAINS")
                                {
                                    if (!string.IsNullOrEmpty(dr.value))
                                    {
                                        updateStatement = updateStatement + dr.StartBraces + dr.Column + " NOT LIKE '%" + dr.value + "%' " + dr.EndBraces;
                                    }
                                    else
                                    {
                                        updateStatement = updateStatement + dr.StartBraces + dr.Column + " NOT LIKE '%" + dr.ConstantValue + "%' " + dr.EndBraces;
                                    }
                                }
                                else if (dr.Condition == "IN")
                                {
                                    if (!string.IsNullOrEmpty(dr.value))
                                        updateStatement = updateStatement + dr.StartBraces + dr.Column + " IN ('" + dr.value + "') " + dr.EndBraces;
                                    else
                                        updateStatement = updateStatement + dr.StartBraces + dr.Column + " IN (" + dr.ConstantValue + ") " + dr.EndBraces;
                                }
                                else if (dr.Condition == "NOT IN")
                                {
                                    if (!string.IsNullOrEmpty(dr.value))
                                        updateStatement = updateStatement + dr.StartBraces + dr.Column + " NOT IN ('" + dr.value + "') " + dr.EndBraces;
                                    else
                                        updateStatement = updateStatement + dr.StartBraces + dr.Column + " NOT IN (" + dr.ConstantValue + ") " + dr.EndBraces;
                                }
                            }
                        }
                        else
                        {
                            if (dr.Condition == "=" || dr.Condition == "<>")
                                if (string.IsNullOrEmpty(dr.value) && string.IsNullOrEmpty(dr.ConstantValue) && dr.Condition == "=")
                                    updateStatement = updateStatement + dr.StartBraces + '(' + dr.Column + "='' OR " + dr.Column + " is null) " + dr.EndBraces + dr.Operator + " ";
                                else if (string.IsNullOrEmpty(dr.value) && string.IsNullOrEmpty(dr.ConstantValue) && dr.Condition == "<>")
                                    updateStatement = updateStatement + dr.StartBraces + '(' + dr.Column + "<>'' AND " + dr.Column + " is not null) " + dr.EndBraces + dr.Operator + " ";
                                else if (!string.IsNullOrEmpty(dr.value))
                                    updateStatement = updateStatement + dr.StartBraces + dr.Column + dr.Condition + " '" + dr.value + "' " + dr.EndBraces + dr.Operator + " ";
                                else
                                    updateStatement = updateStatement + dr.StartBraces + dr.Column + dr.Condition + dr.ConstantValue + dr.EndBraces + dr.Operator + " ";
                            else if (dr.Condition == "CONTAINS")
                                if (!string.IsNullOrEmpty(dr.value))
                                    updateStatement = updateStatement + dr.StartBraces + dr.Column + " LIKE '%" + dr.value + "%' " + dr.EndBraces + dr.Operator + " ";
                                else
                                    updateStatement = updateStatement + dr.StartBraces + dr.Column + " LIKE '%" + dr.ConstantValue + "%' " + dr.EndBraces + dr.Operator + " ";
                            else if (dr.Condition == "NOT CONTAINS")
                                if (!string.IsNullOrEmpty(dr.value))
                                    updateStatement = updateStatement + dr.StartBraces + dr.Column + " NOT LIKE '%" + dr.value + "%' " + dr.EndBraces + dr.Operator + " ";
                                else
                                    updateStatement = updateStatement + dr.StartBraces + dr.Column + " NOT LIKE '%" + dr.ConstantValue + "%' " + dr.EndBraces + dr.Operator + " ";

                            else if (dr.Condition == "IN")
                                if (!string.IsNullOrEmpty(dr.value))
                                    updateStatement = updateStatement + dr.StartBraces + dr.Column + " IN ('" + dr.value + "') " + dr.EndBraces + dr.Operator + " ";
                                else
                                    updateStatement = updateStatement + dr.StartBraces + dr.Column + " IN (" + dr.ConstantValue + ") " + dr.EndBraces + dr.Operator + " ";
                            else if (dr.Condition == "NOT IN")
                                if (!string.IsNullOrEmpty(dr.value))
                                    updateStatement = updateStatement + dr.StartBraces + dr.Column + " NOT IN ('" + dr.value + "') " + dr.EndBraces + dr.Operator + " ";
                                else
                                    updateStatement = updateStatement + dr.StartBraces + dr.Column + " NOT IN (" + dr.ConstantValue + ") " + dr.EndBraces + dr.Operator + " ";

                            //updateStatement = updateStatement + dr.StartBraces + dr.Column + " ='" + dr.value + "' " + dr.EndBraces + dr.Operator + " ";

                        }


                    }
                    updateStatement = updateStatement + " )";
                    if (strcommand == "SAVE")
                    {
                        var paramName = new string[] { "ClientId", "Updatestring" };
                        var paramValue = new object[] { clientid, updateStatement };
                        // var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                        var res = unitOfWork.GetDataFromStoredProcedure("GenerateTower", paramName, paramValue);
                        var updatequeryinSSIS = updateStatement + " and clientid=@clientid and jobid=@jobid and (select count(1) from SourceDataMapping where MappingId=(select mappingid from jobdetail where id=@jobid) and MappingCol='Track' and sField is not null)<=0";
                        var ssisupdatequery = new SSIS_INUpdateQuery { ClientId = clientid, Type = "SSISUPDATE", Query = updatequeryinSSIS, CreatedBy = uid, CreatedTime = DateTime.Now };
                        unitOfWork.SSIS_INUpdateQuery.Insert(ssisupdatequery);
                    }
                    DisplayQuery = DisplayQuery + "<br/>" + updateStatement;

                }
            }
            unitOfWork.Save();
            unitOfWork.Dispose();
            return DisplayQuery;

        }


        public static IList<INChartDashboardModel> INDashboardChartData(string Charttype, int ClientId, string TicketType, string Status, string Track, string Priority, string Datetype, DateTime? StartDate, DateTime? EndDate, string contract)
        {
            IList<INChartDashboardModel> Modellist = new List<INChartDashboardModel>();
            var paramName = new string[] { "Charttype", "ClientId", "TicketType", "Status", "Track", "Priority", "Datetype", "StartDate", "EndDate", "contract" };
            var paramValue = new object[] { Charttype, ClientId, TicketType, Status, Track, Priority, Datetype, StartDate, EndDate, contract };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("INDashboardGetChartData_prod", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new INChartDashboardModel();


                    coldet.IncidentCount = Convert.ToInt32(dr["IncidentCount"]);

                    try
                    {
                        coldet.LongCategory = dr["Category"].ToString().Trim();
                        var strLength = 0;
                        strLength = dr["Category"].ToString().Length;
                        if (strLength > 10)
                        {
                            coldet.DrpCategory = dr["Category"].ToString().Substring(0, 7) + "..";

                        }
                        else
                        {
                            coldet.DrpCategory = dr["Category"].ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("IncidentNavigatorHelper error 'INDashboardChartData' ", ex);
                    }


                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }

        public static DataTable GetIssueClusterReportDataNew(int ClientId,  int TextLength)
        {
            IList<Integration_ReportModel> Modellist = new List<Integration_ReportModel>();
            var paramName = new string[] { "ClientId",  "TextLength" };
            var paramValue = new object[] { ClientId,  TextLength };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                return unitOfWork.GetDataFromStoredProcedure("FetchTextReportByClient", paramName, paramValue);

            }
            return null;

        }
        public static IList<Integration_ReportModel> GetIssueClusterReportData(int ClientId, string TicketType, string Status, string Track, string Priority, string Datetype, DateTime? StartDate, DateTime? EndDate, string contract,string TextType,int TextLength)
        {
            IList<Integration_ReportModel> Modellist = new List<Integration_ReportModel>();
            var paramName = new string[] { "ClientId", "TicketType", "Status", "Track", "Priority", "Datetype", "StartDate", "EndDate", "contract", "TextType", "TextLength" };
            var paramValue = new object[] { ClientId, TicketType, Status, Track, Priority, Datetype, StartDate, EndDate, contract , TextType,  TextLength };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("FetchTextReport", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new Integration_ReportModel();


                    coldet.IncidentCount = Convert.ToInt32(dr["IdCount"]);

                    try
                    {
                        coldet.LongCategory = dr["dtext"].ToString().Trim();
                        

                    }
                    catch (Exception ex)
                    {
                        Log.Error("IncidentNavigatorHelper error 'GetIssueClusterReportData' ", ex);
                    }


                    Modellist.Add(coldet);
                }
             
            }
            return Modellist;
            
        }
        public static IList<Integration_ReportModel> INIntegration_ReportChartData(int ClientId, string TicketType, string Status, string Track, string Priority, string Datetype, DateTime? StartDate, DateTime? EndDate, string contract)
        {
            IList<Integration_ReportModel> Modellist = new List<Integration_ReportModel>();
            var paramName = new string[] { "ClientId", "TicketType", "Status", "Track", "Priority", "Datetype", "StartDate", "EndDate", "contract" };
            var paramValue = new object[] { ClientId, TicketType, Status, Track, Priority, Datetype, StartDate, EndDate, contract };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("FetchIntegrationReportData", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new Integration_ReportModel();


                    coldet.IncidentCount = Convert.ToInt32(dr["IdCount"]);

                    try
                    {
                        coldet.LongCategory = dr["GCategory"].ToString().Trim();
                        coldet.DrpCategory = dr["GCategory"].ToString().Trim();

                    }
                    catch (Exception ex)
                    {
                        Log.Error("IncidentNavigatorHelper error 'INIntegration_ReportChartData' ", ex);
                    }


                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }
        public static IList<Integration_ReportModel> INIntegration_ReportTokenChartData(int ClientId, string TicketType, string Status, string Track, string Priority, string Datetype, DateTime? StartDate, DateTime? EndDate, string contract, string Category)
        {
            IList<Integration_ReportModel> Modellist = new List<Integration_ReportModel>();
            var paramName = new string[] { "ClientId", "TicketType", "Status", "Track", "Priority", "Datetype", "StartDate", "EndDate", "contract", "Category" };
            var paramValue = new object[] { ClientId, TicketType, Status, Track, Priority, Datetype, StartDate, EndDate, contract, Category };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("FetchIntegrationReportTokenData", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new Integration_ReportModel();


                    coldet.IncidentCount = Convert.ToInt32(dr["IdCount"]);

                    try
                    {
                        coldet.Token = dr["GToken"].ToString().Trim();

                    }
                    catch (Exception ex)
                    {
                        Log.Error("IncidentNavigatorHelper error 'INIntegration_ReportTokenChartData' ", ex);
                    }


                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }

        public static IList<Integration_ReportModel> INIntegration_ReportTowerChartData(int ClientId, string TicketType, string Status, string Track, string Priority, string Datetype, DateTime? StartDate, DateTime? EndDate, string contract)
        {
            IList<Integration_ReportModel> Modellist = new List<Integration_ReportModel>();
            var paramName = new string[] { "ClientId", "TicketType", "Status", "Track", "Priority", "Datetype", "StartDate", "EndDate", "contract" };
            var paramValue = new object[] { ClientId, TicketType, Status, Track, Priority, Datetype, StartDate, EndDate, contract };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("FetchIntegrationReportDataByTower", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new Integration_ReportModel();


                    coldet.IncidentCount = Convert.ToInt32(dr["IdCount"]);

                    try
                    {
                        coldet.Tower = dr["Tower"].ToString().Trim();

                    }
                    catch (Exception ex)
                    {
                        Log.Error("IncidentNavigatorHelper error 'INIntegration_ReportTowerChartData' ", ex);
                    }


                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }

        public static IList<Integration_ReportModel> FetchIntegrationReportDataByLeverForTower(int ClientId, string TicketType, string Status, string Track, string Priority, string Datetype, DateTime? StartDate, DateTime? EndDate, string contract)
        {
            IList<Integration_ReportModel> Modellist = new List<Integration_ReportModel>();
            var paramName = new string[] { "ClientId", "TicketType", "Status", "Track", "Priority", "Datetype", "StartDate", "EndDate", "contract" };
            var paramValue = new object[] { ClientId, TicketType, Status, Track, Priority, Datetype, StartDate, EndDate, contract };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("FetchIntegrationReportDataByLeverForTower", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new Integration_ReportModel();


                    coldet.IncidentCount = Convert.ToInt32(dr["IdCount"]);

                    try
                    {
                        //coldet.Tower = dr["Tower"].ToString().Trim();
                        coldet.Lever = dr["Lever"].ToString().Trim();

                    }
                    catch (Exception ex)
                    {
                        Log.Error("IncidentNavigatorHelper error 'FetchIntegrationReportDataByLeverForTower' ", ex);
                    }


                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }
        public static IList<IntegrationReportModel> INIntegration_ReportTowerandLever_drilldown(int ClientId, string TicketType, string Status, string Track, string Lever, string Category, string Token, string Priority, string Datetype, DateTime? StartDate, DateTime? EndDate, string contract, string Tower)
        {

            IList<IntegrationReportModel> Modellist = new List<IntegrationReportModel>();
            var paramName = new string[] { "ClientId", "TicketType", "Status", "Track", "Lever", "Category", "Token", "Priority", "Datetype", "StartDate", "EndDate", "contract" };
            var paramValue = new object[] { ClientId, TicketType, Status, Track, Lever, Category, Token, Priority, Datetype, StartDate, EndDate, contract };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("FetchIntegrationReportDataByTowerandLever_drilldown_prod2", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new IntegrationReportModel();
                    coldet.Id = Convert.ToInt32(dr["ID"]);

                    try
                    {
                        if (!(dr["AssignedDateTime"] is DBNull))
                            coldet.AssignedDateTime = Convert.ToDateTime(dr["AssignedDateTime"]);
                        if (!(dr["ClosedDateTime"] is DBNull))
                            coldet.ClosedDateTime = Convert.ToDateTime(dr["ClosedDateTime"]);
                        if (!(dr["CreatedDateTime"] is DBNull))
                            coldet.CreatedDateTime = Convert.ToDateTime(dr["CreatedDateTime"]);
                        coldet.Description = dr["Description"].ToString();
                        if (!(dr["LastUpdatedDateTime"] is DBNull))
                            coldet.LastUpdatedDateTime = Convert.ToDateTime(dr["LastUpdatedDateTime"]);
                        coldet.Priority = dr["Priority"].ToString();
                        if (!(dr["ResolutionDateTime"] is DBNull))
                            coldet.ResolutionDateTime = Convert.ToDateTime(dr["ResolutionDateTime"]);
                        if (!(dr["ReslntargetTime"] is DBNull))
                            coldet.ResolutionTarget = dr["ReslntargetTime"].ToString();
                        if (!(dr["ResolutionTime"] is DBNull))
                            coldet.ResolutionTime = Convert.ToInt32(dr["ResolutionTime"]);
                        if (!(dr["ResolutionTimeMetSLA"] is DBNull))
                            coldet.ResolutionTimeMetSLA = dr["ResolutionTimeMetSLA"].ToString();
                        if (!(dr["ResptargetTime"] is DBNull))
                            coldet.ResponseTarget = dr["ResptargetTime"].ToString();
                        if (!(dr["ResponseTime"] is DBNull))
                            coldet.ResponseTime = Convert.ToInt32(dr["ResponseTime"]);
                        if (!(dr["ResponseTimeMetSLA"] is DBNull))
                            coldet.ResponseTimeMetSLA = dr["ResponseTimeMetSLA"].ToString();
                        if (!(dr["Status"] is DBNull))
                            coldet.Status = dr["Status"].ToString();
                        coldet.TicketID = dr["IncidentId"].ToString();
                        if (!(dr["Tower"] is DBNull))
                            coldet.Track = dr["Tower"].ToString();
                        if (!(dr["Type"] is DBNull))
                            coldet.Type = dr["Type"].ToString();
                        if (!(dr["WaiveReason"] is DBNull))
                            coldet.WaiveReason = dr["WaiveReason"].ToString();
                        coldet.WaiveSLAReport = dr["Waivedindicator"].ToString() == "N" ? false : true;
                        if (!(dr["AssignedToSupportGroup"] is DBNull))
                            coldet.AssignedToSupportGroup = dr["AssignedToSupportGroup"].ToString();
                        coldet.ResolutionExceptionIndicator = dr["ResolutionExceptionIndicator"].ToString() == "N" ? false : true;
                        if (!(dr["ResolutionExceptionReason"] is DBNull))
                            coldet.ResolutionExceptionReason = dr["ResolutionExceptionReason"].ToString();
                        coldet.ResponseExceptionIndicator = dr["ResponseExceptionIndicator"].ToString() == "N" ? false : true;
                        if (!(dr["ResponseExceptionReason"] is DBNull))
                            coldet.ResponseExceptionReason = dr["ResponseExceptionReason"].ToString();
                        if (!(dr["ShortDescription"] is DBNull))
                            coldet.ShortDescription = dr["ShortDescription"].ToString();
                        if (!(dr["Category"] is DBNull))
                            coldet.Category = dr["Category"].ToString();
                        if (!(dr["Lever"] is DBNull))
                            coldet.Lever = dr["Lever"].ToString();
                        if (!(dr["Token"] is DBNull))
                            coldet.Token = dr["Token"].ToString();
                    }
                    catch (Exception ex)
                    {
                        Log.Error("IncidentNavigatorHelper error 'INIntegration_ReportTowerandLever_drilldown' ", ex);
                    }


                    Modellist.Add(coldet);


                }

            }
            return Modellist;
        }
        public static IList<IntegrationReportModel> INIntegration_DescClusterRepor_drilldown(int ClientId, string TextType, int TextLength, string Status, string Track, string Lever, string Category, string Token, string Priority, string Datetype, DateTime? StartDate, DateTime? EndDate, string contract, string Tower)
        {

            IList<IntegrationReportModel> Modellist = new List<IntegrationReportModel>();
            var paramName = new string[] { "ClientId", "TextType","TextLength", "Status", "Track",  "Category",  "Priority", "Datetype", "StartDate", "EndDate", "contract" };
            var paramValue = new object[] { ClientId, TextType, TextLength, Status, Track,  Category,  Priority, Datetype, StartDate, EndDate, contract };
            

            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("FetchDescClusterReportDrilldown", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new IntegrationReportModel();
                    coldet.Id = Convert.ToInt32(dr["ID"]);

                    try
                    {
                        if (!(dr["AssignedDateTime"] is DBNull))
                            coldet.AssignedDateTime = Convert.ToDateTime(dr["AssignedDateTime"]);
                        if (!(dr["ClosedDateTime"] is DBNull))
                            coldet.ClosedDateTime = Convert.ToDateTime(dr["ClosedDateTime"]);
                        if (!(dr["CreatedDateTime"] is DBNull))
                            coldet.CreatedDateTime = Convert.ToDateTime(dr["CreatedDateTime"]);
                        coldet.Description = dr["Description"].ToString();
                        if (!(dr["LastUpdatedDateTime"] is DBNull))
                            coldet.LastUpdatedDateTime = Convert.ToDateTime(dr["LastUpdatedDateTime"]);
                        coldet.Priority = dr["PriorityVal"].ToString();
                        if (!(dr["ResolutionDateTime"] is DBNull))
                            coldet.ResolutionDateTime = Convert.ToDateTime(dr["ResolutionDateTime"]);
                        if (!(dr["ReslntargetTime"] is DBNull))
                            coldet.ResolutionTarget = dr["ReslntargetTime"].ToString();
                        if (!(dr["ResolutionTime"] is DBNull))
                            coldet.ResolutionTime = Convert.ToInt32(dr["ResolutionTime"]);
                        if (!(dr["ResolutionTimeMetSLA"] is DBNull))
                            coldet.ResolutionTimeMetSLA = dr["ResolutionTimeMetSLA"].ToString();
                        if (!(dr["ResptargetTime"] is DBNull))
                            coldet.ResponseTarget = dr["ResptargetTime"].ToString();
                        if (!(dr["ResponseTime"] is DBNull))
                            coldet.ResponseTime = Convert.ToInt32(dr["ResponseTime"]);
                        if (!(dr["ResponseTimeMetSLA"] is DBNull))
                            coldet.ResponseTimeMetSLA = dr["ResponseTimeMetSLA"].ToString();
                        if (!(dr["Status"] is DBNull))
                            coldet.Status = dr["Status"].ToString();
                        coldet.TicketID = dr["IncidentId"].ToString();
                        if (!(dr["Tower"] is DBNull))
                            coldet.Track = dr["Tower"].ToString();
                        if (!(dr["TypeValue"] is DBNull))
                            coldet.Type = dr["TypeValue"].ToString();
                        if (!(dr["WaiveReason"] is DBNull))
                            coldet.WaiveReason = dr["WaiveReason"].ToString();
                        coldet.WaiveSLAReport = dr["Waivedindicator"].ToString() == "N" ? false : true;
                        if (!(dr["AssignedtosupportgroupValue"] is DBNull))
                            coldet.AssignedToSupportGroup = dr["AssignedtosupportgroupValue"].ToString();
                        coldet.ResolutionExceptionIndicator = dr["ResolutionExceptionIndicator"].ToString() == "N" ? false : true;
                        if (!(dr["ResolutionExceptionReason"] is DBNull))
                            coldet.ResolutionExceptionReason = dr["ResolutionExceptionReason"].ToString();
                        coldet.ResponseExceptionIndicator = dr["ResponseExceptionIndicator"].ToString() == "N" ? false : true;
                        if (!(dr["ResponseExceptionReason"] is DBNull))
                            coldet.ResponseExceptionReason = dr["ResponseExceptionReason"].ToString();
                        if (!(dr["ShortDescription"] is DBNull))
                            coldet.ShortDescription = dr["ShortDescription"].ToString();
                        if (!(dr["ResolutionDescription"] is DBNull))
                            coldet.ResolutionDescription = dr["ResolutionDescription"].ToString();
                        if (!(dr["Category"] is DBNull))
                            coldet.Category = dr["Category"].ToString();
                        if (!(dr["Lever"] is DBNull))
                            coldet.Lever = dr["Lever"].ToString();
                        if (!(dr["Token"] is DBNull))
                            coldet.Token = dr["Token"].ToString();
                    }
                    catch (Exception ex)
                    {
                        Log.Error("IncidentNavigatorHelper error 'INIntegration_DescClusterRepor_drilldown' ", ex);
                    }


                    Modellist.Add(coldet);

                }

            }
            return Modellist;
        }

        public static IList<Integration_ReportModel> INIntegration_ReportTowerandLeverChartData(int ClientId, string TicketType, string Status, string Track, string Priority, string Datetype, DateTime? StartDate, DateTime? EndDate, string contract, string Tower)
        {
            IList<Integration_ReportModel> Modellist = new List<Integration_ReportModel>();
            var paramName = new string[] { "ClientId", "TicketType", "Status", "Track", "Priority", "Datetype", "StartDate", "EndDate", "contract" };
            var paramValue = new object[] { ClientId, TicketType, Status, Track, Priority, Datetype, StartDate, EndDate, contract };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("FetchIntegrationReportDataByTowerandLever", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new Integration_ReportModel();


                    coldet.IncidentCount = Convert.ToInt32(dr["IdCount"]);

                    try
                    {
                        coldet.Tower = dr["Tower"].ToString().Trim();
                        coldet.Lever = dr["Lever"].ToString().Trim();

                    }
                    catch (Exception ex)
                    {
                        Log.Error("IncidentNavigatorHelper error 'INIntegration_ReportTowerandLeverChartData' ", ex);
                    }


                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }

        public static IList<Integration_ReportModel> calculatePercent(IList<Integration_ReportModel> model)
        {
            //IList<DashboardTrendModel> Modellist = new List<DashboardTrendModel>();
            // Modellist = model;
            var sum = 0;
            foreach (var item in model)
            {
                sum = sum + item.IncidentCount;
            }
            if (sum != 0)
            {
                foreach (var item in model)
                {
                    // item.Percent =(decimal) (item.IncidentCount * 100) / sum;
                    item.Percent = Math.Round((decimal)(item.IncidentCount * 100) / sum, 2);
                    //  item.AverageVal = Math.Round((decimal)(item.IncidentCount) / model.Count, 2);
                }
            }

            return model;
        }
        public static IList<Integration_ReportModel> calculateColor(IList<Integration_ReportModel> model)
        {
            List<string> colorList = new List<string> { "#168CDB", "#FA9000", "#118614", "#E64C8E", "#53707C", "#C15E5F", "#DB502A", "#846113", "#12887F", "#7A33FA", "#BF8BDC" };
            var i = 0;
            foreach (var item in model)
            {
                if (i < colorList.Count)
                {
                    item.seriesColor = colorList[i];
                    i++;
                }
                else
                {
                    i = 0;
                }

            }

            return model;
        }

        public static IList<INChartDashboardModel> calculatePercent(IList<INChartDashboardModel> model)
        {
            //IList<DashboardTrendModel> Modellist = new List<DashboardTrendModel>();
            // Modellist = model;
            var sum = 0;
            foreach (var item in model)
            {
                sum = sum + item.IncidentCount;
            }
            if (sum != 0)
            {
                foreach (var item in model)
                {
                    // item.Percent =(decimal) (item.IncidentCount * 100) / sum;
                    item.Percent = Math.Round((decimal)(item.IncidentCount * 100) / sum, 2);
                    item.AverageVal = Math.Round((decimal)(item.IncidentCount) / model.Count, 2);
                }
            }

            return model;
        }

        public static IList<INChartDashboardModel> calculateTrend(IList<INChartDashboardModel> model)
        {
            int cnt = model.Count();
            if (cnt < 2)
                return model;
            var litem = model.ElementAt(cnt - 1);
            long LIncidentCount = litem.IncidentCount;
            long fIncidentCount = model.ElementAt(0).IncidentCount;
            var slope = (LIncidentCount - fIncidentCount) / (cnt - 1);
            var offset = fIncidentCount - (slope * 1);
            long LResolvedInc = litem.ResolvedCount;
            long FResolvedInc = model.ElementAt(0).ResolvedCount;
            var ResolvedSlope = (LResolvedInc - FResolvedInc) / (cnt - 1);
            var ResolvedOffset = FResolvedInc - (ResolvedSlope * 1);

            cnt = 1;
            foreach (var item in model)
            {

                item.TrendVal = (slope * cnt) + offset;
                item.ResolvedTrend = (ResolvedSlope * cnt) + ResolvedOffset;
                cnt++;
            }

            return model;
        }

        public static IEnumerable<SLAMaintenenceModel> getIncidentsCluster(int clientid)
        {
            Db db = new Db();
            IEnumerable<SLAMaintenenceModel> res = (from a in db.Incidents
                                                    join b in db.INIncidentClustered on a.Id equals b.INCIDENT_ID into inClustered
                                                    from inc in inClustered.DefaultIfEmpty()
                                                    join c in db.INCluster on inc.CLUSTERID equals c.ClusterID into cluster
                                                    from cc in cluster.DefaultIfEmpty()
                                                    where a.ClientId == clientid
                                                    //&& b.Clientid == clientid

                                                    select new SLAMaintenenceModel
                                                    {
                                                        Id = a.Id,
                                                        AssignedDateTime = a.AssignedDateTime,
                                                        ClosedDateTime = a.ClosedDateTime,
                                                        CreatedDateTime = a.CreatedDateTime,
                                                        Description = a.Description,
                                                        LastUpdatedDateTime = a.LastUpdatedDateTime,
                                                        Priority = a.Priority,
                                                        ResolutionDateTime = a.ResolutionDateTime,
                                                        ResolutionTarget = a.ReslntargetTime,
                                                        ResolutionTime = a.ResolutionTime,
                                                        ResolutionTimeMetSLA = a.ResolutionTimeMetSLA,
                                                        ResponseTarget = a.ResptargetTime,
                                                        ResponseTime = a.ResponseTime,
                                                        ResponseTimeMetSLA = a.ResponseTimeMetSLA,
                                                        Status = a.Status,
                                                        TicketID = a.IncidentId,
                                                        Track = a.Tower,
                                                        Type = a.Type,
                                                        WaiveReason = a.WaiveReason,
                                                        WaiveSLAReport = a.Waivedindicator == "N" ? false : true,
                                                        AssignedToSupportGroup = a.AssignedToSupportGroup,
                                                        ResolutionExceptionIndicator = a.ResolutionExceptionIndicator == "N" ? false : true,
                                                        ResolutionExceptionReason = a.ResolutionExceptionReason,
                                                        ResponseExceptionIndicator = a.ResponseExceptionIndicator == "N" ? false : true,
                                                        ResponseExceptionReason = a.ResponseExceptionReason,
                                                        ShortDescription = a.ShortDescription,
                                                        ClusterLabel = cc.ClusterLabel,
                                                        ClusterName = cc.TopWords

                                                    }).ToList();

            return res;

        }
        public static IEnumerable<SLAMaintenenceModel> Sp_getIncidentsCluster(int clientid)
        {
            IList<SLAMaintenenceModel> Tredingdata = new List<SLAMaintenenceModel>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { clientid };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("INTicketInventory", paramName, paramValue);

                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new SLAMaintenenceModel();
                    //coldet.Id = Convert.ToInt32(dr["ID"]);
                    if (!(dr["AssignedDateTime"] is DBNull))
                        coldet.AssignedDateTime = Convert.ToDateTime(dr["AssignedDateTime"]);
                    if (!(dr["ClosedDateTime"] is DBNull))
                        coldet.ClosedDateTime = Convert.ToDateTime(dr["ClosedDateTime"]);
                    if (!(dr["CreatedDateTime"] is DBNull))
                        coldet.CreatedDateTime = Convert.ToDateTime(dr["CreatedDateTime"]);
                    if (!(dr["LastUpdatedDateTime"] is DBNull))
                        coldet.LastUpdatedDateTime = Convert.ToDateTime(dr["LastUpdatedDateTime"]);
                    coldet.Description = dr["Description"].ToString();
                    coldet.ShortDescription = dr["ShortDescription"].ToString();
                    coldet.Priority = dr["Priority"].ToString();
                    if (!(dr["ResolutionDateTime"] is DBNull))
                        coldet.ResolutionDateTime = Convert.ToDateTime(dr["ResolutionDateTime"]);
                    if (!(dr["Resolutiontarget"] is DBNull))
                        coldet.ResolutionTarget = dr["Resolutiontarget"].ToString();
                    if (!(dr["ResolutionTime"] is DBNull))
                        coldet.ResolutionTime = Convert.ToInt32(dr["ResolutionTime"]);
                    if (!(dr["ResolutionTimeMetSLA"] is DBNull))
                        coldet.ResolutionTimeMetSLA = dr["ResolutionTimeMetSLA"].ToString();
                    if (!(dr["ResponseTarget"] is DBNull))
                        coldet.ResponseTarget = dr["ResponseTarget"].ToString();
                    if (!(dr["ResponseTime"] is DBNull))
                        coldet.ResponseTime = Convert.ToInt32(dr["ResponseTime"]);
                    if (!(dr["ResolutionTimeMetSLA"] is DBNull))
                        coldet.ResponseTimeMetSLA = dr["ResolutionTimeMetSLA"].ToString();
                    if (!(dr["Status"] is DBNull))
                        coldet.Status = dr["Status"].ToString();
                    if (!(dr["IncidentId"] is DBNull))
                        coldet.TicketID = dr["IncidentId"].ToString();
                    if (!(dr["Tower"] is DBNull))
                        coldet.Track = dr["Tower"].ToString();
                    if (!(dr["Type"] is DBNull))
                        coldet.Type = dr["Type"].ToString();
                    if (!(dr["WaiveReason"] is DBNull))
                        coldet.WaiveReason = dr["WaiveReason"].ToString();
                    if (!(dr["Waivedindicator"] is DBNull))
                        coldet.WaiveSLAReport = dr["Waivedindicator"].ToString() == "Y" ? true : false;
                    if (!(dr["AssignedToSupportGroup"] is DBNull))
                        coldet.AssignedToSupportGroup = dr["AssignedToSupportGroup"].ToString();
                    if (!(dr["ResolutionExceptionIndicator"] is DBNull))
                        coldet.ResolutionExceptionIndicator = dr["ResolutionExceptionIndicator"].ToString() == "Y" ? true : false;
                    if (!(dr["ResponseExceptionReason"] is DBNull))
                        coldet.ResponseExceptionReason = dr["ResponseExceptionReason"].ToString();
                    if (!(dr["ResponseExceptionIndicator"] is DBNull))
                        coldet.ResponseExceptionIndicator = dr["ResponseExceptionIndicator"].ToString() == "Y" ? true : false;
                    if (!(dr["ResolutionExceptionReason"] is DBNull))
                        coldet.ResolutionExceptionReason = dr["ResolutionExceptionReason"].ToString();
                    if (!(dr["ResolutionDescription"] is DBNull))
                        coldet.ResolutionDescription = dr["ResolutionDescription"].ToString();
                    if (!(dr["ClusterLabel"] is DBNull))
                        coldet.ClusterLabel = dr["ClusterLabel"].ToString();
                    if (!(dr["TopWords"] is DBNull))
                        coldet.ClusterName = dr["TopWords"].ToString();
                    Tredingdata.Add(coldet);
                }
            }
            return Tredingdata;
        }

        public static IEnumerable<SLAMaintenenceModel> getConsolidatedCluster(int clientid, string dateType, DateTime fromDate, DateTime toDate, string TypeArray
          , string ContractIDs, string StatusArray, string PriorityArray, string TowerArray, string ReportBy, string GroupArray, string WaivedindicatorOp1, string Account, string graphPriority,
            string graphDate, string param3, string param4, string param5, int count)
        {
            IList<SLAMaintenenceModel> Modellist = new List<SLAMaintenenceModel>();
            var paramName = new string[] { "ClientId", "groupbyparam ", "Datecolumn", "@StartDate", "@EndDate", "@ContractId", "@Towers", "@Statuses", "@Types", "@PriorityArray", "@Priority", "@Category" };
            var paramValue = new object[] { clientid, ReportBy, dateType, fromDate, toDate, ContractIDs, TowerArray, StatusArray, TypeArray, graphPriority, graphDate };
            if (count == 2)
            {
                paramValue = new object[] { clientid, ReportBy, dateType, fromDate, toDate, ContractIDs, TowerArray, StatusArray, TypeArray, "", graphPriority, graphDate };
            }
            //Report2 Graph
            else if (count == 3)
            {
                paramValue = new object[] { clientid, ReportBy, dateType, fromDate, toDate, ContractIDs, param3, StatusArray, TypeArray, "", graphPriority, graphDate };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }
            //Report1 Table
            else if (count == 4)
            {
                paramValue = new object[] { clientid, ReportBy, dateType, fromDate, toDate, ContractIDs, TowerArray, param3, TypeArray, "", graphPriority, graphDate };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }
            //Report2 Table
            else if (count == 5)
            {
                paramValue = new object[] { clientid, ReportBy, dateType, fromDate, toDate, ContractIDs, param5, param3, TypeArray, "", graphPriority, graphDate };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }
            //Report1 total
            else if (count == 6)
            {
                //Priority array needs to be passed as new paramter
                //new priority array column should be added for total and grand total param3 is status , graphdate is LUD
                paramValue = new object[] { clientid, ReportBy, dateType, fromDate, toDate, ContractIDs, TowerArray, param3, TypeArray, PriorityArray, graphPriority, graphDate };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }
            //Report1 grand total
            else if (count == 7)
            {
                //Priority array needs to be passed as new paramter
                paramValue = new object[] { clientid, ReportBy, dateType, fromDate, toDate, ContractIDs, TowerArray, StatusArray, TypeArray, PriorityArray, graphPriority, graphDate };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }
            //Report2 total
            else if (count == 8)
            {
                //Priority array needs to be passed as new paramter
                paramValue = new object[] { clientid, ReportBy, dateType, fromDate, toDate, ContractIDs, param5, param3, TypeArray, PriorityArray, graphPriority, graphDate };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }
            //Report2 Table grand total
            else if (count == 9)
            {
                //Priority array needs to be passed as new paramter
                paramValue = new object[] { clientid, ReportBy, dateType, fromDate, toDate, ContractIDs, param5, StatusArray, TypeArray, PriorityArray, graphPriority, graphDate };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }
            //Report1 Graph
            else
            {
                paramValue = new object[] { clientid, ReportBy, dateType, fromDate, toDate, ContractIDs, TowerArray, StatusArray, TypeArray, "", graphPriority, graphDate };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("[SUPPORT_TREND_TicketStatusWithDateColumnL_Drilldown]", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new SLAMaintenenceModel();
                    coldet.Id = Convert.ToInt32(dr["ID"]);
                    if (!(dr["AssignedDateTime"] is DBNull))
                    {
                        coldet.AssignedDateTime = Convert.ToDateTime(dr["AssignedDateTime"]);
                        coldet.AssignedDateTimestring = (Convert.ToDateTime(dr["AssignedDateTime"])).ToString();
                    }
                    if (!(dr["ClosedDateTime"] is DBNull))
                    {
                        coldet.ClosedDateTime = Convert.ToDateTime(dr["ClosedDateTime"]);
                        coldet.ClosedDateTimestring = (Convert.ToDateTime(dr["ClosedDateTime"])).ToString();
                    }
                    if (!(dr["CreatedDateTime"] is DBNull))
                    {
                        coldet.CreatedDateTime = Convert.ToDateTime(dr["CreatedDateTime"]);
                        coldet.CreatedDateTimestring = (Convert.ToDateTime(dr["CreatedDateTime"])).ToString();
                    }
                    coldet.Description = dr["Description"].ToString();
                    if (!(dr["LastUpdatedDateTime"] is DBNull))
                    {
                        coldet.LastUpdatedDateTime = Convert.ToDateTime(dr["LastUpdatedDateTime"]);
                        coldet.LastUpdatedDateTimestring = (Convert.ToDateTime(dr["LastUpdatedDateTime"])).ToString();
                    }
                    coldet.Priority = dr["Priority"].ToString();
                    if (!(dr["ResolutionDateTime"] is DBNull))
                    {
                        coldet.ResolutionDateTime = Convert.ToDateTime(dr["ResolutionDateTime"]);
                        coldet.ResolutionDateTimestring = (Convert.ToDateTime(dr["ResolutionDateTime"])).ToString();
                    }
                    if (!(dr["ReslntargetTime"] is DBNull))
                        coldet.ResolutionTarget = dr["ReslntargetTime"].ToString();
                    if (!(dr["ResolutionTime"] is DBNull))
                        coldet.ResolutionTime = Convert.ToInt32(dr["ResolutionTime"]);
                    if (!(dr["ResolutionTimeMetSLA"] is DBNull))
                        coldet.ResolutionTimeMetSLA = dr["ResolutionTimeMetSLA"].ToString();
                    if (!(dr["ResptargetTime"] is DBNull))
                        coldet.ResponseTarget = dr["ResptargetTime"].ToString();
                    if (!(dr["ResponseTime"] is DBNull))
                        coldet.ResponseTime = Convert.ToInt32(dr["ResponseTime"]);
                    if (!(dr["ResponseTimeMetSLA"] is DBNull))
                        coldet.ResponseTimeMetSLA = dr["ResponseTimeMetSLA"].ToString();
                    if (!(dr["Status"] is DBNull))
                        coldet.Status = dr["Status"].ToString();
                    coldet.TicketID = dr["IncidentId"].ToString();
                    if (!(dr["Tower"] is DBNull))
                        coldet.Track = dr["Tower"].ToString();
                    if (!(dr["Type"] is DBNull))
                        coldet.Type = dr["Type"].ToString();
                    if (!(dr["WaiveReason"] is DBNull))
                        coldet.WaiveReason = dr["WaiveReason"].ToString();
                    coldet.WaiveSLAReport = dr["Waivedindicator"].ToString() == "N" ? false : true;
                    if (!(dr["AssignedToSupportGroup"] is DBNull))
                        coldet.AssignedToSupportGroup = dr["AssignedToSupportGroup"].ToString();
                    coldet.ResolutionExceptionIndicator = dr["ResolutionExceptionIndicator"].ToString() == "N" ? false : true;
                    if (!(dr["ResolutionExceptionReason"] is DBNull))
                        coldet.ResolutionExceptionReason = dr["ResolutionExceptionReason"].ToString();
                    coldet.ResponseExceptionIndicator = dr["ResponseExceptionIndicator"].ToString() == "N" ? false : true;
                    if (!(dr["ResponseExceptionReason"] is DBNull))
                        coldet.ResponseExceptionReason = dr["ResponseExceptionReason"].ToString();
                    if (!(dr["ShortDescription"] is DBNull))
                        coldet.ShortDescription = dr["ShortDescription"].ToString();

                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }



        public static IEnumerable<SLAMaintenenceModel> getResolutionCluster(int clientid, string dateType, DateTime fromDate, DateTime toDate, string TypeArray
        , string ContractIDs, string StatusArray, string PriorityArray, string TowerArray, string ReportBy, string GroupArray, string WaivedindicatorOp1, string Account, string graphPriority,
          string tower, string typeval, int count, string waiveOption,string contractnameReport)
        {
            IList<SLAMaintenenceModel> Modellist = new List<SLAMaintenenceModel>();
            var paramName = new string[] { "ClientId", "Datecolumn", "@StartDate", "@EndDate", "@AccountId", "@ServiceArea", "@Priority", "@Towers", "@MetSlaType", "@ContractId", "@Statuses", "@Types", "@WaivedindicatorOption", "@Waived", "@contractnameReport" };
            var paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", PriorityArray, TowerArray, "", ContractIDs, StatusArray, TypeArray, waiveOption };
            if (count == 3)
            {
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", graphPriority, TowerArray, "", ContractIDs, StatusArray, typeval, waiveOption, "" , "" };
                //SLAKPI_IncidentResolutionTimeSLASummaryByContract_withDatecolumn_DrillDown 2471 ,'Closeddatetime','2020-10-10' ,'2020-11-10' ,1076,null,'P2','DATA','','','','
            }
            //Report2 Graph
            else if (count == 4)
            {
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", graphPriority, TowerArray, "Yes", ContractIDs, StatusArray, typeval, waiveOption, "" , "" };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }
            //Report1 Table
            else if (count == 5)
            {
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", graphPriority, TowerArray, "No", ContractIDs, StatusArray, typeval, waiveOption, "", "" };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }
            ////Report2 Table
            else if (count == 6)
            {
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", graphPriority, tower, "", ContractIDs, StatusArray, typeval, waiveOption, "", "" };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }
            //Report1 total
            else if (count == 7)
            {
                //Priority array needs to be passed as new paramter
                //new priority array column should be added for total and grand total param3 is status , graphdate is LUD
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", graphPriority, tower, "Yes", ContractIDs, StatusArray, typeval, waiveOption, "", "" };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }
            //Report1 grand total
            else if (count == 8)
            {
                //Priority array needs to be passed as new paramter
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", graphPriority, tower, "No", ContractIDs, StatusArray, typeval, waiveOption, "", "" };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }
            //--------------------------------------xBREACHEDSLAREPORTx------------------------------------------------------------
            else if (count == 9)
            {
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", graphPriority, tower, "", "", StatusArray, typeval, waiveOption, "", contractnameReport };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }

            else if (count == 10)
            {
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", PriorityArray, tower, "", "", StatusArray, typeval, waiveOption, "", contractnameReport };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }

            else if (count == 11)
            {
                Log.Error("case 11" + paramValue);
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", graphPriority, TowerArray, "", "", StatusArray, typeval, waiveOption, "" , contractnameReport };
                Log.Error("case 11" + paramValue);
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }

            else if (count == 12)
            {
                //Priority array needs to be passed as new paramter
                //new priority array column should be added for total and grand total param3 is status , graphdate is LUD
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", PriorityArray, TowerArray, "", "", StatusArray, typeval, waiveOption, "", contractnameReport };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }

            else if (count == 13)
            {
                //Priority array needs to be passed as new paramter
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", graphPriority, TowerArray, "No", "", StatusArray, typeval, waiveOption, "", contractnameReport };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }
            else if (count == 14)
            {
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", PriorityArray, TowerArray, "No", "", StatusArray, typeval, waiveOption, "", contractnameReport };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }

            else if (count == 15)
            {
                //Priority array needs to be passed as new paramter
                //new priority array column should be added for total and grand total param3 is status , graphdate is LUD
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", graphPriority, TowerArray, "", "", StatusArray, typeval, waiveOption, "Y" , contractnameReport };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }

            else if (count == 16)
            {
                //Priority array needs to be passed as new paramter
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", PriorityArray, TowerArray, "", "", StatusArray, typeval, waiveOption, "Y", contractnameReport };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }

            //Report1 Graph
            else
            {
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", PriorityArray, TowerArray, "", ContractIDs, StatusArray, TypeArray, waiveOption, "", contractnameReport };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }
            Log.Error("Param value =" + paramValue);
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("[SLAKPI_IncidentResolutionTimeSLASummaryByContract_withDatecolumn_DrillDown]", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new SLAMaintenenceModel();
                    coldet.Id = Convert.ToInt32(dr["ID"]);
                    if (!(dr["AssignedDateTime"] is DBNull))
                    {
                        coldet.AssignedDateTime = Convert.ToDateTime(dr["AssignedDateTime"]);
                        coldet.AssignedDateTimestring = (Convert.ToDateTime(dr["AssignedDateTime"])).ToString();
                    }
                    if (!(dr["ClosedDateTime"] is DBNull))
                    {
                        coldet.ClosedDateTime = Convert.ToDateTime(dr["ClosedDateTime"]);
                        coldet.ClosedDateTimestring = (Convert.ToDateTime(dr["ClosedDateTime"])).ToString();
                    }
                    if (!(dr["CreatedDateTime"] is DBNull))
                    {
                        coldet.CreatedDateTime = Convert.ToDateTime(dr["CreatedDateTime"]);
                        coldet.CreatedDateTimestring= (Convert.ToDateTime(dr["CreatedDateTime"])).ToString();
                    }
                    coldet.Description = dr["Description"].ToString();
                    if (!(dr["LastUpdatedDateTime"] is DBNull))
                    {
                        coldet.LastUpdatedDateTime = Convert.ToDateTime(dr["LastUpdatedDateTime"]);
                        coldet.LastUpdatedDateTimestring = (Convert.ToDateTime(dr["LastUpdatedDateTime"])).ToString();
                    }
                    coldet.Priority = dr["Priority"].ToString();
                    if (!(dr["ResolutionDateTime"] is DBNull))
                    {
                        coldet.ResolutionDateTime = Convert.ToDateTime(dr["ResolutionDateTime"]);
                        coldet.ResolutionDateTimestring = (Convert.ToDateTime(dr["ResolutionDateTime"])).ToString();
                    }
                    if (!(dr["ReslntargetTime"] is DBNull))
                        coldet.ResolutionTarget = dr["ReslntargetTime"].ToString();
                    if (!(dr["ResolutionTime"] is DBNull))
                        coldet.Resolutiontime1 = (dr["ResolutionTime"].ToString());
                    if (!(dr["ResolutionTimeMetSLA"] is DBNull))
                        coldet.ResolutionTimeMetSLA = dr["ResolutionTimeMetSLA"].ToString();
                    if (!(dr["ResptargetTime"] is DBNull))
                        coldet.ResponseTarget = dr["ResptargetTime"].ToString();
                    if (!(dr["ResponseTime"] is DBNull))
                        coldet.ResponseTime = Convert.ToInt32(dr["ResponseTime"]);
                    if (!(dr["ResponseTimeMetSLA"] is DBNull))
                        coldet.ResponseTimeMetSLA = dr["ResponseTimeMetSLA"].ToString();
                    if (!(dr["Status"] is DBNull))
                        coldet.Status = dr["Status"].ToString();
                    coldet.TicketID = dr["IncidentId"].ToString();
                    if (!(dr["Tower"] is DBNull))
                        coldet.Track = dr["Tower"].ToString();
                    if (!(dr["Type"] is DBNull))
                        coldet.Type = dr["Type"].ToString();
                    if (!(dr["WaiveReason"] is DBNull))
                        coldet.WaiveReason = dr["WaiveReason"].ToString();
                    coldet.WaiveSLAReport = dr["Waivedindicator"].ToString() == "N" ? false : true;
                    if (!(dr["AssignedToSupportGroup"] is DBNull))
                        coldet.AssignedToSupportGroup = dr["AssignedToSupportGroup"].ToString();
                    coldet.ResolutionExceptionIndicator = dr["ResolutionExceptionIndicator"].ToString() == "N" ? false : true;
                    if (!(dr["ResolutionExceptionReason"] is DBNull))
                        coldet.ResolutionExceptionReason = dr["ResolutionExceptionReason"].ToString();
                    coldet.ResponseExceptionIndicator = dr["ResponseExceptionIndicator"].ToString() == "N" ? false : true;
                    if (!(dr["ResponseExceptionReason"] is DBNull))
                        coldet.ResponseExceptionReason = dr["ResponseExceptionReason"].ToString();
                    if (!(dr["ShortDescription"] is DBNull))
                        coldet.ShortDescription = dr["ShortDescription"].ToString();
                    if (!(dr["Waivedindicator"] is DBNull))
                        coldet.Waivedindicator = dr["Waivedindicator"].ToString();
                    if (!(dr["Resolutiontype"] is DBNull))
                        coldet.Resolutiontype = dr["Resolutiontype"].ToString();
                    //if (!(dr["ResolutionExceptionreason"] is DBNull))
                    //    coldet.ResolutionExceptionReason = dr["ResolutionExceptionreason"].ToString();



                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }


        public static IEnumerable<SLAMaintenenceModel> getKPICluster(int clientid, string dateType, DateTime fromDate, DateTime toDate, string TypeArray
              , string ContractIDs, string StatusArray, string PriorityArray, string TowerArray, string ReportBy, string GroupArray, string WaivedindicatorOp1, string Account, string graphPriority,
                string ReportSelected, string graphDate, string waiveOption, string Agingtitle, int Recyclecount)
        {
            IList<SLAMaintenenceModel> Modellist = new List<SLAMaintenenceModel>();
            var paramName = new string[] { "ClientId", "@AccountId", "@ContractId", "Datecolumn", "groupbyparam", "@StartDate", "@EndDate", "@Priority", "@Statuses", "@Towers", "@Types", "groupbyDate", "@ServiceArea", "@WaivedindicatorOption" };
            var paramValue = new object[] { clientid, Account, ContractIDs, dateType, ReportBy, fromDate, toDate, graphPriority, StatusArray, TowerArray, TypeArray, graphDate, "", waiveOption };
            var SP_name = "MTTR_OverallTicketVolumeReportL2_drilldown";
            switch (ReportSelected)
            {
                case "MTTR":
                    {
                        paramValue = new object[] { clientid, Account, ContractIDs, dateType, ReportBy, fromDate, toDate, graphPriority, StatusArray, TowerArray, TypeArray, graphDate, "", "" };
                        //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
                        SP_name = "MTTR_OverallTicketVolumeReportL2_drilldown";
                        break;
                    }
                case "MTTA":
                    {
                        paramValue = new object[] { clientid, Account, ContractIDs, dateType, ReportBy, fromDate, toDate, graphPriority, StatusArray, TowerArray, TypeArray, graphDate, "", "" };
                        //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
                        SP_name = "MTTA_OverallTicketVolumeReportL2_drilldown";
                        break;
                    }
                case "Aging":
                    {
                        paramName = new string[] { "ClientId", "@AccountId", "@ContractId", "Datecolumn", "@StartDate", "@EndDate", "@Priority", "@Statuses", "@Towers", "@Types", "@Assignedtosupportgroup", "@Title" };
                        paramValue = new object[] { clientid, Account, ContractIDs, dateType, fromDate, toDate, graphPriority, StatusArray, TowerArray, TypeArray, GroupArray, Agingtitle };
                        SP_name = "RepAgingQryProcCurrClk_AsPerSvcCal_drilldown";
                        break;
                    }
                case "FirstTimeRight":
                    {
                        paramName = new string[] { "ClientId", "@AccountId", "@ContractId", "groupbyparam", "Datecolumn", "@StartDate", "@EndDate", "@Priority", "@Statuses", "@Towers", "@Types", "@Assignedtosupportgroup", "@Recyclecount", "groupbyDate" };
                        paramValue = new object[] { clientid, Account, ContractIDs, ReportBy, dateType, fromDate, toDate, graphPriority, StatusArray, TowerArray, TypeArray, GroupArray, Recyclecount, graphDate };
                        //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
                        SP_name = "SLAKPI_FIRSTTIMERIGHT_ReopenWithDateColumnL_drilldown";
                        break;
                    }
            }

            //Report1 Table

            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure(SP_name, paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new SLAMaintenenceModel();
                    coldet.Id = Convert.ToInt32(dr["ID"]);
                    if (!(dr["AssignedDateTime"] is DBNull))
                        coldet.AssignedDateTime = Convert.ToDateTime(dr["AssignedDateTime"]);
                    if (!(dr["ClosedDateTime"] is DBNull))
                        coldet.ClosedDateTime = Convert.ToDateTime(dr["ClosedDateTime"]);
                    if (!(dr["CreatedDateTime"] is DBNull))
                        coldet.CreatedDateTime = Convert.ToDateTime(dr["CreatedDateTime"]);
                    coldet.Description = dr["Description"].ToString();
                    if (!(dr["LastUpdatedDateTime"] is DBNull))
                        coldet.LastUpdatedDateTime = Convert.ToDateTime(dr["LastUpdatedDateTime"]);
                    coldet.Priority = dr["Priority"].ToString();
                    if (!(dr["ResolutionDateTime"] is DBNull))
                        coldet.ResolutionDateTime = Convert.ToDateTime(dr["ResolutionDateTime"]);
                    if (!(dr["ReslntargetTime"] is DBNull))
                        coldet.ResolutionTarget = dr["ReslntargetTime"].ToString();
                    if (!(dr["ResolutionTime"] is DBNull))
                        coldet.Resolutiontime1 = (dr["ResolutionTime"].ToString());
                    if (!(dr["ResolutionTimeMetSLA"] is DBNull))
                        coldet.ResolutionTimeMetSLA = dr["ResolutionTimeMetSLA"].ToString();
                    if (!(dr["ResptargetTime"] is DBNull))
                        coldet.ResponseTarget = dr["ResptargetTime"].ToString();
                    if (!(dr["ResponseTime"] is DBNull))
                        coldet.ResponseTime = Convert.ToInt32(dr["ResponseTime"]);
                    if (!(dr["ResponseTimeMetSLA"] is DBNull))
                        coldet.ResponseTimeMetSLA = dr["ResponseTimeMetSLA"].ToString();
                    if (!(dr["Status"] is DBNull))
                        coldet.Status = dr["Status"].ToString();
                    coldet.TicketID = dr["IncidentId"].ToString();
                    if (!(dr["Tower"] is DBNull))
                        coldet.Track = dr["Tower"].ToString();
                    if (!(dr["Type"] is DBNull))
                        coldet.Type = dr["Type"].ToString();
                    if (!(dr["WaiveReason"] is DBNull))
                        coldet.WaiveReason = dr["WaiveReason"].ToString();
                    coldet.WaiveSLAReport = dr["Waivedindicator"].ToString() == "N" ? false : true;
                    if (!(dr["AssignedToSupportGroup"] is DBNull))
                        coldet.AssignedToSupportGroup = dr["AssignedToSupportGroup"].ToString();
                    coldet.ResolutionExceptionIndicator = dr["ResolutionExceptionIndicator"].ToString() == "N" ? false : true;
                    if (!(dr["ResolutionExceptionReason"] is DBNull))
                        coldet.ResolutionExceptionReason = dr["ResolutionExceptionReason"].ToString();
                    coldet.ResponseExceptionIndicator = dr["ResponseExceptionIndicator"].ToString() == "N" ? false : true;
                    if (!(dr["ResponseExceptionReason"] is DBNull))
                        coldet.ResponseExceptionReason = dr["ResponseExceptionReason"].ToString();
                    if (!(dr["ShortDescription"] is DBNull))
                        coldet.ShortDescription = dr["ShortDescription"].ToString();
                    if (!(dr["Waivedindicator"] is DBNull))
                        coldet.Waivedindicator = dr["Waivedindicator"].ToString();
                    if (!(dr["Resolutiontype"] is DBNull))
                        coldet.Resolutiontype = dr["Resolutiontype"].ToString();

                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }

        public static IEnumerable<SLAMaintenenceModel> getKEDBCluster(int clientid, string KEDBNumber, int IssueTypeId, int SolutionCategoryId, int SolutionStatusId, string Tower)
        {
            IList<SLAMaintenenceModel> Modellist = new List<SLAMaintenenceModel>();


            var paramName = new string[] { "ClientId", "@KEDBNumber", "@IssueTypeId", "@SolutionCategoryId", "@SolutionStatusId", "@Tower" };
            var paramValue = new object[] { clientid, KEDBNumber, IssueTypeId, SolutionCategoryId, SolutionStatusId, Tower };
            var SP_name = "KEDB_Summary_drilldown";

            //Report1 Table

            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure(SP_name, paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new SLAMaintenenceModel();
                    coldet.Id = Convert.ToInt32(dr["ID"]);
                    if (!(dr["AssignedDateTime"] is DBNull))
                        coldet.AssignedDateTime = Convert.ToDateTime(dr["AssignedDateTime"]);
                    if (!(dr["ClosedDateTime"] is DBNull))
                        coldet.ClosedDateTime = Convert.ToDateTime(dr["ClosedDateTime"]);
                    if (!(dr["CreatedDateTime"] is DBNull))
                        coldet.CreatedDateTime = Convert.ToDateTime(dr["CreatedDateTime"]);
                    coldet.Description = dr["Description"].ToString();
                    if (!(dr["LastUpdatedDateTime"] is DBNull))
                        coldet.LastUpdatedDateTime = Convert.ToDateTime(dr["LastUpdatedDateTime"]);
                    coldet.Priority = dr["Priority"].ToString();
                    if (!(dr["ResolutionDateTime"] is DBNull))
                        coldet.ResolutionDateTime = Convert.ToDateTime(dr["ResolutionDateTime"]);
                    if (!(dr["ReslntargetTime"] is DBNull))
                        coldet.ResolutionTarget = dr["ReslntargetTime"].ToString();
                    if (!(dr["ResolutionTime"] is DBNull))
                        coldet.Resolutiontime1 = (dr["ResolutionTime"].ToString());
                    if (!(dr["ResolutionTimeMetSLA"] is DBNull))
                        coldet.ResolutionTimeMetSLA = dr["ResolutionTimeMetSLA"].ToString();
                    if (!(dr["ResptargetTime"] is DBNull))
                        coldet.ResponseTarget = dr["ResptargetTime"].ToString();
                    if (!(dr["ResponseTime"] is DBNull))
                        coldet.ResponseTime = Convert.ToInt32(dr["ResponseTime"]);
                    if (!(dr["ResponseTimeMetSLA"] is DBNull))
                        coldet.ResponseTimeMetSLA = dr["ResponseTimeMetSLA"].ToString();
                    if (!(dr["Status"] is DBNull))
                        coldet.Status = dr["Status"].ToString();
                    coldet.TicketID = dr["IncidentId"].ToString();
                    if (!(dr["Tower"] is DBNull))
                        coldet.Track = dr["Tower"].ToString();
                    if (!(dr["Type"] is DBNull))
                        coldet.Type = dr["Type"].ToString();
                    if (!(dr["WaiveReason"] is DBNull))
                        coldet.WaiveReason = dr["WaiveReason"].ToString();
                    coldet.WaiveSLAReport = dr["Waivedindicator"].ToString() == "N" ? false : true;
                    if (!(dr["AssignedToSupportGroup"] is DBNull))
                        coldet.AssignedToSupportGroup = dr["AssignedToSupportGroup"].ToString();
                    coldet.ResolutionExceptionIndicator = dr["ResolutionExceptionIndicator"].ToString() == "N" ? false : true;
                    if (!(dr["ResolutionExceptionReason"] is DBNull))
                        coldet.ResolutionExceptionReason = dr["ResolutionExceptionReason"].ToString();
                    coldet.ResponseExceptionIndicator = dr["ResponseExceptionIndicator"].ToString() == "N" ? false : true;
                    if (!(dr["ResponseExceptionReason"] is DBNull))
                        coldet.ResponseExceptionReason = dr["ResponseExceptionReason"].ToString();
                    if (!(dr["ShortDescription"] is DBNull))
                        coldet.ShortDescription = dr["ShortDescription"].ToString();
                    if (!(dr["Waivedindicator"] is DBNull))
                        coldet.Waivedindicator = dr["Waivedindicator"].ToString();
                    if (!(dr["Resolutiontype"] is DBNull))
                        coldet.Resolutiontype = dr["Resolutiontype"].ToString();

                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }
        public static IEnumerable<SLAMaintenenceModel> getClusterDetails(int clientid, string clusterlabel, string Configitemname, string ReportSelected)
        {
            IList<SLAMaintenenceModel> Modellist = new List<SLAMaintenenceModel>();
            var paramName = new string[] { "ClientId", "@clusterlabel", "@Configitemname" };
            var paramValue = new object[] { clientid, clusterlabel, Configitemname };
            var SP_name = "TicketKeywordsReport_drilldown";
            switch (ReportSelected)
            {
                case "TicketKeywordsReport":
                    {
                        SP_name = "TicketKeywordsReport_drilldown";
                        break;
                    }
                case "TicketClusterAnalysis":
                    {
                        paramName = new string[] { "ClientId", "@clusterlabel" };
                        paramValue = new object[] { clientid, clusterlabel, };
                        SP_name = "[TicketClusterAnalysis_drilldown]";
                        break;
                    }
            }

            //Report1 Table

            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure(SP_name, paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new SLAMaintenenceModel();
                    coldet.Id = Convert.ToInt32(dr["ID"]);
                    if (!(dr["AssignedDateTime"] is DBNull))
                        coldet.AssignedDateTime = Convert.ToDateTime(dr["AssignedDateTime"]);
                    if (!(dr["ClosedDateTime"] is DBNull))
                        coldet.ClosedDateTime = Convert.ToDateTime(dr["ClosedDateTime"]);
                    if (!(dr["CreatedDateTime"] is DBNull))
                        coldet.CreatedDateTime = Convert.ToDateTime(dr["CreatedDateTime"]);
                    coldet.Description = dr["Description"].ToString();
                    if (!(dr["LastUpdatedDateTime"] is DBNull))
                        coldet.LastUpdatedDateTime = Convert.ToDateTime(dr["LastUpdatedDateTime"]);
                    coldet.Priority = dr["Priority"].ToString();
                    if (!(dr["ResolutionDateTime"] is DBNull))
                        coldet.ResolutionDateTime = Convert.ToDateTime(dr["ResolutionDateTime"]);
                    if (!(dr["ReslntargetTime"] is DBNull))
                        coldet.ResolutionTarget = dr["ReslntargetTime"].ToString();
                    if (!(dr["ResolutionTime"] is DBNull))
                        coldet.Resolutiontime1 = (dr["ResolutionTime"].ToString());
                    if (!(dr["ResolutionTimeMetSLA"] is DBNull))
                        coldet.ResolutionTimeMetSLA = dr["ResolutionTimeMetSLA"].ToString();
                    if (!(dr["ResptargetTime"] is DBNull))
                        coldet.ResponseTarget = dr["ResptargetTime"].ToString();
                    if (!(dr["ResponseTime"] is DBNull))
                        coldet.ResponseTime = Convert.ToInt32(dr["ResponseTime"]);
                    if (!(dr["ResponseTimeMetSLA"] is DBNull))
                        coldet.ResponseTimeMetSLA = dr["ResponseTimeMetSLA"].ToString();
                    if (!(dr["Status"] is DBNull))
                        coldet.Status = dr["Status"].ToString();
                    coldet.TicketID = dr["IncidentId"].ToString();
                    if (!(dr["Tower"] is DBNull))
                        coldet.Track = dr["Tower"].ToString();
                    if (!(dr["Type"] is DBNull))
                        coldet.Type = dr["Type"].ToString();
                    if (!(dr["WaiveReason"] is DBNull))
                        coldet.WaiveReason = dr["WaiveReason"].ToString();
                    coldet.WaiveSLAReport = dr["Waivedindicator"].ToString() == "N" ? false : true;
                    if (!(dr["AssignedToSupportGroup"] is DBNull))
                        coldet.AssignedToSupportGroup = dr["AssignedToSupportGroup"].ToString();
                    coldet.ResolutionExceptionIndicator = dr["ResolutionExceptionIndicator"].ToString() == "N" ? false : true;
                    if (!(dr["ResolutionExceptionReason"] is DBNull))
                        coldet.ResolutionExceptionReason = dr["ResolutionExceptionReason"].ToString();
                    coldet.ResponseExceptionIndicator = dr["ResponseExceptionIndicator"].ToString() == "N" ? false : true;
                    if (!(dr["ResponseExceptionReason"] is DBNull))
                        coldet.ResponseExceptionReason = dr["ResponseExceptionReason"].ToString();
                    if (!(dr["ShortDescription"] is DBNull))
                        coldet.ShortDescription = dr["ShortDescription"].ToString();
                    if (!(dr["Waivedindicator"] is DBNull))
                        coldet.Waivedindicator = dr["Waivedindicator"].ToString();
                    if (!(dr["Resolutiontype"] is DBNull))
                        coldet.Resolutiontype = dr["Resolutiontype"].ToString();

                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }

        public static IEnumerable<SLAMaintenenceModel> getResponseCluster(int clientid, string dateType, DateTime fromDate, DateTime toDate, string TypeArray
  , string ContractIDs, string StatusArray, string PriorityArray, string TowerArray, string ReportBy, string GroupArray, string WaivedindicatorOp1, string Account, string graphPriority,
    string tower, string typeval, int count, string waiveOption, string contractnameReport)
        {
            IList<SLAMaintenenceModel> Modellist = new List<SLAMaintenenceModel>();
            var paramName = new string[] { "ClientId", "Datecolumn", "@StartDate", "@EndDate", "@AccountId", "@ServiceArea", "@Priority", "@Towers", "@MetSlaType", "@ContractId", "@Statuses", "@Types", "@WaivedindicatorOption", "@Waived", "@contractnameReport" };
            var paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", PriorityArray, TowerArray, "", ContractIDs, StatusArray, TypeArray, waiveOption };
            if (count == 3)
            {
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", graphPriority, TowerArray, "", ContractIDs, StatusArray, typeval, waiveOption, "","" };
                //SLAKPI_IncidentResolutionTimeSLASummaryByContract_withDatecolumn_DrillDown 2471 ,'Closeddatetime','2020-10-10' ,'2020-11-10' ,1076,null,'P2','DATA','','','','
            }
            //Report2 Graph
            else if (count == 4)
            {
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", graphPriority, TowerArray, "Yes", ContractIDs, StatusArray, typeval, waiveOption, "","" };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }
            //Report1 Table
            else if (count == 5)
            {
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", graphPriority, TowerArray, "No", ContractIDs, StatusArray, typeval, waiveOption, "","" };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }
            ////Report2 Table
            else if (count == 6)
            {
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", graphPriority, tower, "", ContractIDs, StatusArray, typeval, waiveOption, "","" };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }
            //Report1 total
            else if (count == 7)
            {
                //Priority array needs to be passed as new paramter
                //new priority array column should be added for total and grand total param3 is status , graphdate is LUD
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", graphPriority, tower, "Yes", ContractIDs, StatusArray, typeval, waiveOption, "","" };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }
            //Report1 grand total
            else if (count == 8)
            {
                //Priority array needs to be passed as new paramter
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", graphPriority, tower, "No", ContractIDs, StatusArray, typeval, waiveOption, "","" };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }
            //--------------------------------------xBREACHEDSLAREPORTx------------------------------------------------------------
            else if (count == 9)
            {
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", graphPriority, tower, "", ContractIDs, StatusArray, typeval, waiveOption, "" , contractnameReport };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }

            else if (count == 10)
            {
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", PriorityArray, tower, "", ContractIDs, StatusArray, typeval, waiveOption, "" , contractnameReport };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }

            else if (count == 11)
            {
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", graphPriority, TowerArray, "", ContractIDs, StatusArray, typeval, waiveOption, "", contractnameReport };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }

            else if (count == 12)
            {
                //Priority array needs to be passed as new paramter
                //new priority array column should be added for total and grand total param3 is status , graphdate is LUD
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", PriorityArray, TowerArray, "", ContractIDs, StatusArray, typeval, waiveOption, "", contractnameReport };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }

            else if (count == 13)
            {
                //Priority array needs to be passed as new paramter
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", graphPriority, TowerArray, "No", ContractIDs, StatusArray, typeval, waiveOption, "", contractnameReport };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }
            else if (count == 14)
            {
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", PriorityArray, TowerArray, "No", ContractIDs, StatusArray, typeval, waiveOption, "", contractnameReport };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }

            else if (count == 15)
            {
                //Priority array needs to be passed as new paramter
                //new priority array column should be added for total and grand total param3 is status , graphdate is LUD
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", graphPriority, TowerArray, "", ContractIDs, StatusArray, typeval, waiveOption, "Y" , contractnameReport };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }

            else if (count == 16)
            {
                //Priority array needs to be passed as new paramter
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", PriorityArray, TowerArray, "", ContractIDs, StatusArray, typeval, waiveOption, "Y", contractnameReport };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }

            //Report1 Graph
            else
            {
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", PriorityArray, TowerArray, "", ContractIDs, StatusArray, TypeArray, waiveOption, "",contractnameReport };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("[SLAKPI_IncidentResponseTimeSLASummaryByContractL_withDatecolumn_DrillDown]", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new SLAMaintenenceModel();
                    coldet.Id = Convert.ToInt32(dr["ID"]);
                    if (!(dr["AssignedDateTime"] is DBNull))
                    {
                        coldet.AssignedDateTime = Convert.ToDateTime(dr["AssignedDateTime"]);
                        coldet.AssignedDateTimestring = (Convert.ToDateTime(dr["AssignedDateTime"])).ToString();
                    }
                    if (!(dr["ClosedDateTime"] is DBNull))
                    {
                        coldet.ClosedDateTime = Convert.ToDateTime(dr["ClosedDateTime"]);
                        coldet.ClosedDateTimestring = (Convert.ToDateTime(dr["ClosedDateTime"])).ToString();
                    }
                    if (!(dr["CreatedDateTime"] is DBNull))
                    {
                        coldet.CreatedDateTime = Convert.ToDateTime(dr["CreatedDateTime"]);
                        coldet.CreatedDateTimestring = (Convert.ToDateTime(dr["CreatedDateTime"])).ToString();
                    }
                    coldet.Description = dr["Description"].ToString();
                    if (!(dr["LastUpdatedDateTime"] is DBNull))
                    {
                        coldet.LastUpdatedDateTime = Convert.ToDateTime(dr["LastUpdatedDateTime"]);
                        coldet.LastUpdatedDateTimestring = (Convert.ToDateTime(dr["LastUpdatedDateTime"])).ToString();
                    }
                    coldet.Priority = dr["Priority"].ToString();
                    if (!(dr["ResolutionDateTime"] is DBNull))
                    {
                        coldet.ResolutionDateTime = Convert.ToDateTime(dr["ResolutionDateTime"]);
                        coldet.ResolutionDateTimestring =( Convert.ToDateTime(dr["ResolutionDateTime"])).ToString();
                    }
                    if (!(dr["ReslntargetTime"] is DBNull))
                        coldet.ResolutionTarget = dr["ReslntargetTime"].ToString();
                    if (!(dr["ResolutionTime"] is DBNull))
                        coldet.Resolutiontime1 = (dr["ResolutionTime"].ToString());
                    if (!(dr["ResolutionTimeMetSLA"] is DBNull))
                        coldet.ResolutionTimeMetSLA = dr["ResolutionTimeMetSLA"].ToString();
                    if (!(dr["ResptargetTime"] is DBNull))
                        coldet.ResponseTarget = dr["ResptargetTime"].ToString();
                    if (!(dr["ResponseTime"] is DBNull))
                        coldet.ResponseTime = Convert.ToInt32(dr["ResponseTime"]);
                    if (!(dr["ResponseTimeMetSLA"] is DBNull))
                        coldet.ResponseTimeMetSLA = dr["ResponseTimeMetSLA"].ToString();
                    if (!(dr["Status"] is DBNull))
                        coldet.Status = dr["Status"].ToString();
                    coldet.TicketID = dr["IncidentId"].ToString();
                    if (!(dr["Tower"] is DBNull))
                        coldet.Track = dr["Tower"].ToString();
                    if (!(dr["Type"] is DBNull))
                        coldet.Type = dr["Type"].ToString();
                    if (!(dr["WaiveReason"] is DBNull))
                        coldet.WaiveReason = dr["WaiveReason"].ToString();
                    coldet.WaiveSLAReport = dr["Waivedindicator"].ToString() == "N" ? false : true;
                    if (!(dr["AssignedToSupportGroup"] is DBNull))
                        coldet.AssignedToSupportGroup = dr["AssignedToSupportGroup"].ToString();
                    coldet.ResolutionExceptionIndicator = dr["ResolutionExceptionIndicator"].ToString() == "N" ? false : true;
                    if (!(dr["ResolutionExceptionReason"] is DBNull))
                        coldet.ResolutionExceptionReason = dr["ResolutionExceptionReason"].ToString();
                    coldet.ResponseExceptionIndicator = dr["ResponseExceptionIndicator"].ToString() == "N" ? false : true;
                    if (!(dr["ResponseExceptionReason"] is DBNull))
                        coldet.ResponseExceptionReason = dr["ResponseExceptionReason"].ToString();
                    if (!(dr["ShortDescription"] is DBNull))
                        coldet.ShortDescription = dr["ShortDescription"].ToString();
                    if (!(dr["Waivedindicator"] is DBNull))
                        coldet.Waivedindicator = dr["Waivedindicator"].ToString();
                    if (!(dr["Resolutiontype"] is DBNull))
                        coldet.Resolutiontype = dr["Resolutiontype"].ToString();

                    //assigning response to resolution 
                    coldet.ResolutionTimeMetSLA = coldet.ResponseTimeMetSLA;
                    coldet.ResolutionExceptionIndicator = coldet.ResponseExceptionIndicator;
                    coldet.ResolutionExceptionReason = coldet.ResponseExceptionReason;
                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }


        public static IEnumerable<SLAMaintenenceModel> getResponseProdCluster(int clientid, string dateType, DateTime fromDate, DateTime toDate, string TypeArray
        , string ContractIDs, string StatusArray, string PriorityArray, string TowerArray, string ReportBy, string GroupArray, string WaivedindicatorOp1, string Account, string graphPriority,
          string tower, string typeval, int count, string graphDate, string waiveoption)
        {
            IList<SLAMaintenenceModel> Modellist = new List<SLAMaintenenceModel>();
            var paramName = new string[] { "ClientId", "Datecolumn", "@groupbyparam", "@StartDate", "@EndDate", "@AccountId", "@ServiceArea", "@Priority", "@Towers", "@MetSlaType", "@ContractId", "@Statuses", "@Types", "@Waived", "@Category", "@WaivedindicatorOption" };
            var paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", PriorityArray, TowerArray, "", ContractIDs, StatusArray, TypeArray, "", waiveoption };
            if (count == 17)
            {
                paramValue = new object[] { clientid, dateType, ReportBy, fromDate, toDate, Account, "", graphPriority, TowerArray, "", ContractIDs, StatusArray, typeval, "", graphDate, waiveoption };
                //SLAKPI_IncidentResolutionTimeSLASummaryByContract_withDatecolumn_DrillDown 2471 ,'Closeddatetime','2020-10-10' ,'2020-11-10' ,1076,null,'P2','DATA','','','','
            }

            //Total
            else if (count == 18)
            {
                paramValue = new object[] { clientid, dateType, ReportBy, fromDate, toDate, Account, "", graphPriority, TowerArray, "", ContractIDs, StatusArray, typeval, "", "", waiveoption };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }

            else if (count == 19)
            {
                paramValue = new object[] { clientid, dateType, ReportBy, fromDate, toDate, Account, "", graphPriority, tower, "", ContractIDs, StatusArray, typeval, "", graphDate, waiveoption };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }

            else if (count == 20)
            {
                paramValue = new object[] { clientid, dateType, ReportBy, fromDate, toDate, Account, "", graphPriority, tower, "", ContractIDs, StatusArray, typeval, "", "", waiveoption };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }

            //Report1 Graph

            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("[ProductionSummary_Response_Drilldown]", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new SLAMaintenenceModel();
                    coldet.Id = Convert.ToInt32(dr["ID"]);
                    if (!(dr["AssignedDateTime"] is DBNull))
                    {
                        coldet.AssignedDateTime = Convert.ToDateTime(dr["AssignedDateTime"]);
                        coldet.AssignedDateTimestring = (Convert.ToDateTime(dr["AssignedDateTime"])).ToString();
                    }

                    if (!(dr["ClosedDateTime"] is DBNull)) { 
                        coldet.ClosedDateTime = Convert.ToDateTime(dr["ClosedDateTime"]);
                    coldet.ClosedDateTimestring = (Convert.ToDateTime(dr["ClosedDateTime"])).ToString();
                }
                    if (!(dr["CreatedDateTime"] is DBNull))
                    {
                        coldet.CreatedDateTime = Convert.ToDateTime(dr["CreatedDateTime"]);
                        coldet.CreatedDateTimestring = (Convert.ToDateTime(dr["CreatedDateTime"])).ToString();
                    }
                    coldet.Description = dr["Description"].ToString();
                    if (!(dr["LastUpdatedDateTime"] is DBNull))
                    {
                        coldet.LastUpdatedDateTime = Convert.ToDateTime(dr["LastUpdatedDateTime"]);
                        coldet.LastUpdatedDateTimestring = (Convert.ToDateTime(dr["LastUpdatedDateTime"])).ToString();
                    }
                    coldet.Priority = dr["Priority"].ToString();
                    if (!(dr["ResolutionDateTime"] is DBNull))
                    {
                        coldet.ResolutionDateTime = Convert.ToDateTime(dr["ResolutionDateTime"]);
                        coldet.ResolutionDateTimestring = (Convert.ToDateTime(dr["ResolutionDateTime"])).ToString();
                    }
                    if (!(dr["ReslntargetTime"] is DBNull))
                        coldet.ResolutionTarget = dr["ReslntargetTime"].ToString();
                    if (!(dr["ResolutionTime"] is DBNull))
                        coldet.Resolutiontime1 = (dr["ResolutionTime"].ToString());
                    if (!(dr["ResolutionTimeMetSLA"] is DBNull))
                        coldet.ResolutionTimeMetSLA = dr["ResolutionTimeMetSLA"].ToString();
                    if (!(dr["ResptargetTime"] is DBNull))
                        coldet.ResponseTarget = dr["ResptargetTime"].ToString();
                    if (!(dr["ResponseTime"] is DBNull))
                        coldet.ResponseTime = Convert.ToInt32(dr["ResponseTime"]);
                    if (!(dr["ResponseTimeMetSLA"] is DBNull))
                        coldet.ResponseTimeMetSLA = dr["ResponseTimeMetSLA"].ToString();
                    if (!(dr["Status"] is DBNull))
                        coldet.Status = dr["Status"].ToString();
                    coldet.TicketID = dr["IncidentId"].ToString();
                    if (!(dr["Tower"] is DBNull))
                        coldet.Track = dr["Tower"].ToString();
                    if (!(dr["Type"] is DBNull))
                        coldet.Type = dr["Type"].ToString();
                    if (!(dr["WaiveReason"] is DBNull))
                        coldet.WaiveReason = dr["WaiveReason"].ToString();
                    coldet.WaiveSLAReport = dr["Waivedindicator"].ToString() == "N" ? false : true;
                    if (!(dr["AssignedToSupportGroup"] is DBNull))
                        coldet.AssignedToSupportGroup = dr["AssignedToSupportGroup"].ToString();
                    coldet.ResolutionExceptionIndicator = dr["ResolutionExceptionIndicator"].ToString() == "N" ? false : true;
                    if (!(dr["ResolutionExceptionReason"] is DBNull))
                        coldet.ResolutionExceptionReason = dr["ResolutionExceptionReason"].ToString();
                    coldet.ResponseExceptionIndicator = dr["ResponseExceptionIndicator"].ToString() == "N" ? false : true;
                    if (!(dr["ResponseExceptionReason"] is DBNull))
                        coldet.ResponseExceptionReason = dr["ResponseExceptionReason"].ToString();
                    if (!(dr["ShortDescription"] is DBNull))
                        coldet.ShortDescription = dr["ShortDescription"].ToString();
                    if (!(dr["Waivedindicator"] is DBNull))
                        coldet.Waivedindicator = dr["Waivedindicator"].ToString();
                    if (!(dr["Resolutiontype"] is DBNull))
                        coldet.Resolutiontype = dr["Resolutiontype"].ToString();

                    //assigning response to resolution 
                    coldet.ResolutionTimeMetSLA = coldet.ResponseTimeMetSLA;
                    coldet.ResolutionExceptionIndicator = coldet.ResponseExceptionIndicator;
                    coldet.ResolutionExceptionReason = coldet.ResponseExceptionReason;
                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }


        public static IEnumerable<SLAMaintenenceModel> getResolutionProdCluster(int clientid, string dateType, DateTime fromDate, DateTime toDate, string TypeArray
        , string ContractIDs, string StatusArray, string PriorityArray, string TowerArray, string ReportBy, string GroupArray, string WaivedindicatorOp1, string Account, string graphPriority,
          string tower, string typeval, int count, string graphDate, string waiveOption)
        {
            IList<SLAMaintenenceModel> Modellist = new List<SLAMaintenenceModel>();
            var paramName = new string[] { "ClientId", "Datecolumn", "@groupbyparam", "@StartDate", "@EndDate", "@AccountId", "@ServiceArea", "@Priority", "@Towers", "@MetSlaType", "@ContractId", "@Statuses", "@Types", "@Waived", "@Category", "@WaivedindicatorOption" };
            var paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", PriorityArray, TowerArray, "", ContractIDs, StatusArray, TypeArray, "", waiveOption };
            if (count == 17)
            {
                paramValue = new object[] { clientid, dateType, ReportBy, fromDate, toDate, Account, "", graphPriority, TowerArray, "", ContractIDs, StatusArray, typeval, "", graphDate, waiveOption };
                //SLAKPI_IncidentResolutionTimeSLASummaryByContract_withDatecolumn_DrillDown 2471 ,'Closeddatetime','2020-10-10' ,'2020-11-10' ,1076,null,'P2','DATA','','','','
            }

            //Total
            else if (count == 18)
            {
                paramValue = new object[] { clientid, dateType, ReportBy, fromDate, toDate, Account, "", graphPriority, TowerArray, "", ContractIDs, StatusArray, typeval, "", "", waiveOption };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }

            else if (count == 19)
            {
                paramValue = new object[] { clientid, dateType, ReportBy, fromDate, toDate, Account, "", graphPriority, tower, "", ContractIDs, StatusArray, typeval, "", graphDate, waiveOption };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }

            else if (count == 20)
            {
                paramValue = new object[] { clientid, dateType, ReportBy, fromDate, toDate, Account, "", graphPriority, tower, "", ContractIDs, StatusArray, typeval, "", "", waiveOption };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }

            //Report1 Graph

            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("[ProductionSummary_Resolution_Drilldown]", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new SLAMaintenenceModel();
                    coldet.Id = Convert.ToInt32(dr["ID"]);
                    if (!(dr["AssignedDateTime"] is DBNull))
                    {
                        coldet.AssignedDateTime = Convert.ToDateTime(dr["AssignedDateTime"]);
                        coldet.AssignedDateTimestring = (Convert.ToDateTime(dr["AssignedDateTime"])).ToString();
                    }

                    if (!(dr["ClosedDateTime"] is DBNull))
                    {
                        coldet.ClosedDateTime = Convert.ToDateTime(dr["ClosedDateTime"]);
                        coldet.ClosedDateTimestring = (Convert.ToDateTime(dr["ClosedDateTime"])).ToString();
                    }
                    if (!(dr["CreatedDateTime"] is DBNull))
                    {
                        coldet.CreatedDateTime = Convert.ToDateTime(dr["CreatedDateTime"]);
                        coldet.CreatedDateTimestring = (Convert.ToDateTime(dr["CreatedDateTime"])).ToString();
                    }
                    coldet.Description = dr["Description"].ToString();
                    if (!(dr["LastUpdatedDateTime"] is DBNull))
                    {
                        coldet.LastUpdatedDateTime = Convert.ToDateTime(dr["LastUpdatedDateTime"]);
                        coldet.LastUpdatedDateTimestring = (Convert.ToDateTime(dr["LastUpdatedDateTime"])).ToString();
                    }

                    coldet.Priority = dr["Priority"].ToString();
                    if (!(dr["ResolutionDateTime"] is DBNull))
                    {
                        coldet.ResolutionDateTime = Convert.ToDateTime(dr["ResolutionDateTime"]);
                        coldet.ResolutionDateTimestring = (Convert.ToDateTime(dr["ResolutionDateTime"])).ToString();
                    }
                    if (!(dr["ReslntargetTime"] is DBNull))
                        coldet.ResolutionTarget = dr["ReslntargetTime"].ToString();
                    if (!(dr["ResolutionTime"] is DBNull))
                        coldet.Resolutiontime1 = (dr["ResolutionTime"].ToString());
                    if (!(dr["ResolutionTimeMetSLA"] is DBNull))
                        coldet.ResolutionTimeMetSLA = dr["ResolutionTimeMetSLA"].ToString();
                    if (!(dr["ResptargetTime"] is DBNull))
                        coldet.ResponseTarget = dr["ResptargetTime"].ToString();
                    if (!(dr["ResponseTime"] is DBNull))
                        coldet.ResponseTime = Convert.ToInt32(dr["ResponseTime"]);
                    if (!(dr["ResponseTimeMetSLA"] is DBNull))
                        coldet.ResponseTimeMetSLA = dr["ResponseTimeMetSLA"].ToString();
                    if (!(dr["Status"] is DBNull))
                        coldet.Status = dr["Status"].ToString();
                    coldet.TicketID = dr["IncidentId"].ToString();
                    if (!(dr["Tower"] is DBNull))
                        coldet.Track = dr["Tower"].ToString();
                    if (!(dr["Type"] is DBNull))
                        coldet.Type = dr["Type"].ToString();
                    if (!(dr["WaiveReason"] is DBNull))
                        coldet.WaiveReason = dr["WaiveReason"].ToString();
                    coldet.WaiveSLAReport = dr["Waivedindicator"].ToString() == "N" ? false : true;
                    if (!(dr["AssignedToSupportGroup"] is DBNull))
                        coldet.AssignedToSupportGroup = dr["AssignedToSupportGroup"].ToString();
                    coldet.ResolutionExceptionIndicator = dr["ResolutionExceptionIndicator"].ToString() == "N" ? false : true;
                    if (!(dr["ResolutionExceptionReason"] is DBNull))
                        coldet.ResolutionExceptionReason = dr["ResolutionExceptionReason"].ToString();
                    coldet.ResponseExceptionIndicator = dr["ResponseExceptionIndicator"].ToString() == "N" ? false : true;
                    if (!(dr["ResponseExceptionReason"] is DBNull))
                        coldet.ResponseExceptionReason = dr["ResponseExceptionReason"].ToString();
                    if (!(dr["ShortDescription"] is DBNull))
                        coldet.ShortDescription = dr["ShortDescription"].ToString();
                    if (!(dr["Waivedindicator"] is DBNull))
                        coldet.Waivedindicator = dr["Waivedindicator"].ToString();
                    if (!(dr["Resolutiontype"] is DBNull))
                        coldet.Resolutiontype = dr["Resolutiontype"].ToString();

                    //assigning response to resolution 
                    //coldet.ResolutionTimeMetSLA = coldet.ResponseTimeMetSLA;
                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }


        public static IEnumerable<SLAMaintenenceModel> getVolumeCluster(int clientid, string dateType, DateTime fromDate, DateTime toDate, string TypeArray
           , string ContractIDs, string StatusArray, string PriorityArray, string TowerArray, string ReportBy, string GroupArray, string WaivedindicatorOp1, string Account, string param1,
            string param2, string param3, string param4, string param5, int count)
        {
            IList<SLAMaintenenceModel> Modellist = new List<SLAMaintenenceModel>();
            var paramName = new string[] { "ClientId", "Datecolumn", "@StartDate", "@EndDate", "@AccountId", "@ServiceArea", "@groupbyparam", "@Priority", "@Towers", "@MetSlaType", "@ContractId", "@Statuses", "@Types", "@Waived", "@AssignedToSupportGrpArray" };
            var paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", ReportBy, PriorityArray, TowerArray, "", ContractIDs, StatusArray, TypeArray, "", GroupArray };


            if (count == 1)
            {
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", ReportBy, PriorityArray, param5, "", ContractIDs, StatusArray, TypeArray, "", GroupArray };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }

            else if (count == 3)
            {
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", ReportBy, PriorityArray, param5, "", ContractIDs, StatusArray, param2, "", param1 };
                //SLAKPI_IncidentResolutionTimeSLASummaryByContract_withDatecolumn_DrillDown 2471 ,'Closeddatetime','2020-10-10' ,'2020-11-10' ,1076,null,'P2','DATA','','','','
            }

            //Report2 Graph
            else if (count == 4)
            {
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", ReportBy, param3, param5, "", ContractIDs, StatusArray, param2, "", param1 };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }
            //Report1 Table
            else if (count == 5)
            {
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", ReportBy, param3, param5, "", ContractIDs, param4, param2, "", param1 };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }
            ////Report2 Table
            else if (count == 6)
            {
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", ReportBy, param3, param5, "", ContractIDs, param4, TypeArray, "", param1 };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }
            //Report1 total
            else if (count == 7)
            {
                //Priority array needs to be passed as new paramter
                //new priority array column should be added for total and grand total param3 is status , graphdate is LUD
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", ReportBy, param3, param5, "", ContractIDs, StatusArray, TypeArray, "", param1 };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }
            //Report1 grand total
            else if (count == 8)
            {
                //Priority array needs to be passed as new paramter
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", ReportBy, PriorityArray, param5, "", ContractIDs, StatusArray, param2, "", param1 };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }
            //--------------------------------------xBREACHEDSLAREPORTx------------------------------------------------------------
            else if (count == 9)
            {
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", ReportBy, PriorityArray, param5, "", ContractIDs, StatusArray, TypeArray, "", param1 };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }

            else if (count == 10)
            {
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", ReportBy, PriorityArray, param5, "", ContractIDs, StatusArray, param2, "", GroupArray };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }



            //Report1 Graph
            else
            {
                paramValue = new object[] { clientid, dateType, fromDate, toDate, Account, "", ReportBy, PriorityArray, param5, "", ContractIDs, StatusArray, TypeArray, "", GroupArray };
                //var paramValue = new object[] { clientid, "Week", "Resolutiondatetime", "2020-7-1", "2020-7-31", "", "", "", "", "P1", "07/13/2020" };
            }
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("[SLAKPI_Volume_dynamicTicketVolumeWithDateColumnL_Drilldown]", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new SLAMaintenenceModel();
                    coldet.Id = Convert.ToInt32(dr["ID"]);
                    if (!(dr["AssignedDateTime"] is DBNull))
                        coldet.AssignedDateTime = Convert.ToDateTime(dr["AssignedDateTime"]);
                    if (!(dr["ClosedDateTime"] is DBNull))
                        coldet.ClosedDateTime = Convert.ToDateTime(dr["ClosedDateTime"]);
                    if (!(dr["CreatedDateTime"] is DBNull))
                        coldet.CreatedDateTime = Convert.ToDateTime(dr["CreatedDateTime"]);
                    coldet.Description = dr["Description"].ToString();
                    if (!(dr["LastUpdatedDateTime"] is DBNull))
                        coldet.LastUpdatedDateTime = Convert.ToDateTime(dr["LastUpdatedDateTime"]);
                    coldet.Priority = dr["Priority"].ToString();
                    if (!(dr["ResolutionDateTime"] is DBNull))
                        coldet.ResolutionDateTime = Convert.ToDateTime(dr["ResolutionDateTime"]);
                    if (!(dr["ReslntargetTime"] is DBNull))
                        coldet.ResolutionTarget = dr["ReslntargetTime"].ToString();
                    if (!(dr["ResolutionTime"] is DBNull))
                        coldet.Resolutiontime1 = (dr["ResolutionTime"].ToString());
                    if (!(dr["ResolutionTimeMetSLA"] is DBNull))
                        coldet.ResolutionTimeMetSLA = dr["ResolutionTimeMetSLA"].ToString();
                    if (!(dr["ResptargetTime"] is DBNull))
                        coldet.ResponseTarget = dr["ResptargetTime"].ToString();
                    if (!(dr["ResponseTime"] is DBNull))
                        coldet.ResponseTime = Convert.ToInt32(dr["ResponseTime"]);
                    if (!(dr["ResponseTimeMetSLA"] is DBNull))
                        coldet.ResponseTimeMetSLA = dr["ResponseTimeMetSLA"].ToString();
                    if (!(dr["Status"] is DBNull))
                        coldet.Status = dr["Status"].ToString();
                    coldet.TicketID = dr["IncidentId"].ToString();
                    if (!(dr["Tower"] is DBNull))
                        coldet.Track = dr["Tower"].ToString();
                    if (!(dr["Type"] is DBNull))
                        coldet.Type = dr["Type"].ToString();
                    if (!(dr["WaiveReason"] is DBNull))
                        coldet.WaiveReason = dr["WaiveReason"].ToString();
                    coldet.WaiveSLAReport = dr["Waivedindicator"].ToString() == "N" ? false : true;
                    if (!(dr["AssignedToSupportGroup"] is DBNull))
                        coldet.AssignedToSupportGroup = dr["AssignedToSupportGroup"].ToString();
                    coldet.ResolutionExceptionIndicator = dr["ResolutionExceptionIndicator"].ToString() == "N" ? false : true;
                    if (!(dr["ResolutionExceptionReason"] is DBNull))
                        coldet.ResolutionExceptionReason = dr["ResolutionExceptionReason"].ToString();
                    coldet.ResponseExceptionIndicator = dr["ResponseExceptionIndicator"].ToString() == "N" ? false : true;
                    if (!(dr["ResponseExceptionReason"] is DBNull))
                        coldet.ResponseExceptionReason = dr["ResponseExceptionReason"].ToString();
                    if (!(dr["ShortDescription"] is DBNull))
                        coldet.ShortDescription = dr["ShortDescription"].ToString();
                    if (!(dr["Waivedindicator"] is DBNull))
                        coldet.Waivedindicator = dr["Waivedindicator"].ToString();
                    if (!(dr["Resolutiontype"] is DBNull))
                        coldet.Resolutiontype = dr["Resolutiontype"].ToString();

                    //assigning response to resolution 
                    //coldet.ResolutionTimeMetSLA = coldet.ResponseTimeMetSLA;
                    //coldet.ResolutionExceptionIndicator = coldet.ResponseExceptionIndicator;
                    //coldet.ResolutionExceptionReason = coldet.ResponseExceptionReason;
                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }

        public static DataTable getCSATCluster(int clientid)
        {
            IList<CSATModel> csatList = new List<CSATModel>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { clientid };

            DataTable dt = new DataTable();

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                dt = unitOfWork.GetDataFromStoredProcedure("GetQuestionCSAT", paramName, paramValue);
                //foreach (DataRow dr in res.Rows)
                //{
                //    var coldet = new INContractRuleModel();
                //    var FieldDatatpe = dr["DataType"].ToString();

                //    if (FieldDatatpe == "nvarchar")
                //    {
                //        var FieldLength = Convert.ToInt32(dr["Size"]);
                //        if (FieldLength < 260)
                //        {
                //            coldet.INFieldName = dr["AppField"].ToString();
                //            AppColumnDetail.Add(coldet);
                //        }
                //    }

                //}
            }




            return dt;

        }
        public static IEnumerable<ClientAccountContract> readTrackContract(int ClientId)
        {

            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var disp = unitOfWork.ClientAccountContract.Get(g => g.ClientId == ClientId);
            var returnModel = disp.Select(e => new ClientAccountContract
            {
                Id = e.Id,
                ContractName = e.ContractName

            });

            return returnModel;
        }




        public static IList<INContractRuleModel> getRuleListContract(int Id)
        {

            IList<INContractRuleModel> EstCategoryList = new List<INContractRuleModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<INContractRule> RowDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());


                if (Id != 0)

                    RowDetails = unitOfWork.INContractRule.Get(cid => cid.ContractId == Id).ToList();

                if (RowDetails != null)
                {
                    foreach (var _detail in RowDetails)
                    {
                        INContractRuleModel bb = new INContractRuleModel();
                        bb.Id = _detail.Id;
                        bb.INFieldName = _detail.INFieldName;
                        bb.FieldValue = _detail.FieldValue;
                        bb.Operator = _detail.Operator;
                        bb.GroupEndOperator = _detail.GroupEndOperator;
                        bb.GroupStartOperator = _detail.GroupStartOperator;
                        bb.Condition = _detail.Condition;
                        bb.ConstantValue = _detail.ConstantValue;
                        EstCategoryList.Add(bb);
                    }
                }
                return EstCategoryList;
            }

        }

        public static int addTeamRuleContract(INContractRuleModel Input)
        {
            int ra_id = 0;
            Db db = new Db();

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var _Details = new INContractRule();
                    _Details.ContractId = Input.ContractId;
                    _Details.INFieldName = Input.INFieldName;
                    _Details.FieldValue = Input.FieldValue;
                    _Details.Operator = Input.Operator;
                    _Details.GroupEndOperator = Input.GroupEndOperator;
                    _Details.GroupStartOperator = Input.GroupStartOperator;
                    _Details.Condition = Input.Condition;
                    _Details.ConstantValue = Input.ConstantValue;
                    db.INContractRule.Add(_Details);
                    db.SaveChanges();
                    // unitOfWork.INContractRule.Insert(_Details);
                    //db.INContractRule.Add(_Details);
                    //unitOfWork.Save();
                    ra_id = _Details.ContractId;

                    return ra_id;
                }
            }
            catch (Exception ex)
            {
                Log.Error("IncidentNavigatorHelper error 'addTeamRuleContract' ", ex);
            }
            return 0;
        }

        public static bool updateTeamRuleContract(INContractRuleModel _Input)
        {
            Db db = new Db();
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    //   var updatetitem = unitOfWork.INContractRule.GetByID(_Input.Id);
                    //INContractRule updateitem = (from a in db.INContractRule
                    //                  where a.Id == _Input.Id
                    //                  select a).FirstOrDefault();
                    INContractRule updateitem = db.INContractRule.FirstOrDefault(x => x.Id == _Input.Id);


                    updateitem.INFieldName = _Input.INFieldName;
                    updateitem.FieldValue = _Input.FieldValue;
                    updateitem.Operator = _Input.Operator;
                    updateitem.GroupEndOperator = _Input.GroupEndOperator;
                    updateitem.GroupStartOperator = _Input.GroupStartOperator;
                    updateitem.Condition = _Input.Condition;
                    updateitem.ConstantValue = _Input.ConstantValue;
                    db.SaveChanges();


                    //  unitOfWork.INContractRule.Update(updateitem);

                    //  unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("IncidentNavigatorHelper error 'updateTeamRuleContract' ", ex);
            }
            return false;
        }

        public static bool RemoveTeamRuleContract(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.INContractRule.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }

        public static IEnumerable<SelectListItem> getColumnListContract(int ClientId)
        {
            IList<INContractRuleModel> AppColumnDetail = new List<INContractRuleModel>();
            var paramName = new string[] { "tableName" };
            var paramValue = new object[] { "Incident" };

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                var res = unitOfWork.GetDataFromStoredProcedure("GetTableColumnNameswithFilterForTower", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new INContractRuleModel();
                    var FieldDatatpe = dr["DataType"].ToString();

                    //if (FieldDatatpe == "nvarchar")
                    //{
                    //    var FieldLength = Convert.ToInt32(dr["Size"]);
                    //    if (FieldLength < 260)
                    //    {
                    coldet.INFieldName = dr["AppField"].ToString();
                    AppColumnDetail.Add(coldet);
                    //    }
                    //}

                }
            }
            var list = AppColumnDetail.Select(f => new SelectListItem { Value = f.INFieldName, Text = f.INFieldName });
            return list;
        }



        public static List<INContractRuleModel> getColumnValuesListContract(int ClId, string Columnid)
        {
            List<INContractRuleModel> AppColumnDetail = new List<INContractRuleModel>();
            var paramName = new string[] { "ClientId", "ColumnName" };
            var paramValue = new object[] { ClId, Columnid };

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                var res = unitOfWork.GetDataFromStoredProcedure("GetColumnValue", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new INContractRuleModel();
                    coldet.FieldValue = dr["AppValue"].ToString();
                    if (!String.IsNullOrWhiteSpace(coldet.FieldValue))
                    {
                        AppColumnDetail.Add(coldet);
                    }
                }
            }
            // var list = AppColumnDetail.Select(f => new SelectListItem { Value = f.FieldValue, Text = f.FieldValue });
            return AppColumnDetail;
        }
        public static string GenerateNExecRuleContract(int clientid, string strcommand, int uid)
        {

            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            if (strcommand == "SAVE")
            {
                var param1 = new string[] { "ClientId", "Type" };
                var paramVal = new object[] { clientid, "SSISUPDATECONTRACT" };
                var result = unitOfWork.GetDataFromStoredProcedure("DeleteSSISUpdateQuery", param1, paramVal);
            }
            // var ssisupdatequery = new SSIS_INUpdateQuery { ClientId = clientid, Type = "SSISUPDATE", Query = updateStatement };
            // unitOfWork.SSIS_INUpdateQuery.Insert(ssisupdatequery);
            var db = new Db();
            var ContractRuledata = (from tm in db.ClientAccountContract
                                    join tr in db.INContractRule on tm.Id equals tr.ContractId
                                    where tm.ClientId == clientid
                                    select new { ruleid = tr.Id, ContractId = tm.Id, AccountId = tm.AccountId, Column = tr.INFieldName, value = tr.FieldValue, Operator = tr.Operator, StartBraces = tr.GroupStartOperator, EndBraces = tr.GroupEndOperator, Condition = tr.Condition, ConstantValue = tr.ConstantValue }).OrderBy(e => e.ContractId).ToList();
            var ContractList = (from a in ContractRuledata
                                select new { ContractId = a.ContractId, AccountId = a.AccountId }).Distinct().OrderBy(a => a.ContractId);

            string DisplayQuery = "";
            var updateStatement = "";
            //  var DisplayQuery = "";
            foreach (var item in ContractList)
            {


                updateStatement = "UPDATE Incident set ContractId='" + item.ContractId + "', AccountId='" + item.AccountId + "' where clientid=" + clientid.ToString() + " AND (";

                if (ContractRuledata.Count() > 0)
                {
                    var ContractRule = ContractRuledata.Where(w => w.ContractId == item.ContractId).OrderBy(o => o.ruleid).ToList();
                    var count = ContractRule.Count();
                    int i = 0;
                    foreach (var dr in ContractRule)
                    {
                        i++;
                        if (i == count)
                        {
                            if (string.IsNullOrEmpty(dr.value) && string.IsNullOrEmpty(dr.ConstantValue))
                            {
                                //string tempCond = "";
                                if (dr.Condition == "=" || dr.Condition == "IS NULL")
                                    updateStatement = updateStatement = updateStatement + dr.StartBraces + " ( " + dr.Column + " IS NULL OR " + dr.Column + "='') " + dr.EndBraces;
                                else if (dr.Condition == "<>" || dr.Condition == "IS NOT NULL")
                                    updateStatement = updateStatement = updateStatement + dr.StartBraces + " ( " + dr.Column + " IS NOT NULL AND " + dr.Column + "<>'' ) " + dr.EndBraces;
                            }
                            else
                            {
                                if (dr.Condition == "=" || dr.Condition == "<>")
                                {
                                    if (!string.IsNullOrEmpty(dr.value))
                                        updateStatement = updateStatement + dr.StartBraces + dr.Column + dr.Condition + " '" + dr.value + "' " + dr.EndBraces;
                                    else
                                        updateStatement = updateStatement + dr.StartBraces + dr.Column + dr.Condition + dr.ConstantValue + dr.EndBraces;
                                }
                                else if (dr.Condition == "CONTAINS")
                                {
                                    if (!string.IsNullOrEmpty(dr.value))
                                    {
                                        updateStatement = updateStatement + dr.StartBraces + dr.Column + " LIKE '%" + dr.value + "%' " + dr.EndBraces;
                                    }
                                    else
                                    {
                                        updateStatement = updateStatement + dr.StartBraces + dr.Column + " LIKE '%" + dr.ConstantValue + "%' " + dr.EndBraces;
                                    }
                                }

                                else if (dr.Condition == "NOT CONTAINS")
                                {
                                    if (!string.IsNullOrEmpty(dr.value))
                                    {
                                        updateStatement = updateStatement + dr.StartBraces + dr.Column + " NOT LIKE '%" + dr.value + "%' " + dr.EndBraces;
                                    }
                                    else
                                    {
                                        updateStatement = updateStatement + dr.StartBraces + dr.Column + " NOT LIKE '%" + dr.ConstantValue + "%' " + dr.EndBraces;
                                    }
                                }

                                else if (dr.Condition == "IN")
                                {
                                    if (!string.IsNullOrEmpty(dr.value))
                                        updateStatement = updateStatement + dr.StartBraces + dr.Column + " IN ('" + dr.value + "') " + dr.EndBraces;
                                    else
                                        updateStatement = updateStatement + dr.StartBraces + dr.Column + " IN (" + dr.ConstantValue + ") " + dr.EndBraces;
                                }
                                else if (dr.Condition == "NOT IN")
                                {
                                    if (!string.IsNullOrEmpty(dr.value))
                                        updateStatement = updateStatement + dr.StartBraces + dr.Column + " NOT IN ('" + dr.value + "') " + dr.EndBraces;
                                    else
                                        updateStatement = updateStatement + dr.StartBraces + dr.Column + " NOT IN (" + dr.ConstantValue + ") " + dr.EndBraces;
                                }
                            }
                        }
                        else
                        {
                            if (dr.Condition == "=" || dr.Condition == "<>")
                                if (string.IsNullOrEmpty(dr.value) && string.IsNullOrEmpty(dr.ConstantValue) && dr.Condition == "=")
                                    updateStatement = updateStatement + dr.StartBraces + '(' + dr.Column + "='' OR " + dr.Column + " is null) " + dr.EndBraces + dr.Operator + " ";
                                else if (string.IsNullOrEmpty(dr.value) && string.IsNullOrEmpty(dr.ConstantValue) && dr.Condition == "<>")
                                    updateStatement = updateStatement + dr.StartBraces + '(' + dr.Column + "<>'' AND " + dr.Column + " is not null) " + dr.EndBraces + dr.Operator + " ";
                                else if (!string.IsNullOrEmpty(dr.value))
                                    updateStatement = updateStatement + dr.StartBraces + dr.Column + dr.Condition + " '" + dr.value + "' " + dr.EndBraces + dr.Operator + " ";
                                else
                                    updateStatement = updateStatement + dr.StartBraces + dr.Column + dr.Condition + dr.ConstantValue + dr.EndBraces + dr.Operator + " ";
                            else if (dr.Condition == "CONTAINS")
                                if (!string.IsNullOrEmpty(dr.value))
                                    updateStatement = updateStatement + dr.StartBraces + dr.Column + " LIKE '%" + dr.value + "%' " + dr.EndBraces + dr.Operator + " ";
                                else
                                    updateStatement = updateStatement + dr.StartBraces + dr.Column + " LIKE '%" + dr.ConstantValue + "%' " + dr.EndBraces + dr.Operator + " ";
                            else if (dr.Condition == "NOT CONTAINS")
                                if (!string.IsNullOrEmpty(dr.value))
                                    updateStatement = updateStatement + dr.StartBraces + dr.Column + " NOT LIKE '%" + dr.value + "%' " + dr.EndBraces + dr.Operator + " ";
                                else
                                    updateStatement = updateStatement + dr.StartBraces + dr.Column + " NOT LIKE '%" + dr.ConstantValue + "%' " + dr.EndBraces + dr.Operator + " ";

                            else if (dr.Condition == "IN")
                                if (!string.IsNullOrEmpty(dr.value))
                                    updateStatement = updateStatement + dr.StartBraces + dr.Column + " IN ('" + dr.value + "') " + dr.EndBraces + dr.Operator + " ";
                                else
                                    updateStatement = updateStatement + dr.StartBraces + dr.Column + " IN (" + dr.ConstantValue + ") " + dr.EndBraces + dr.Operator + " ";
                            else if (dr.Condition == "NOT IN")
                                if (!string.IsNullOrEmpty(dr.value))
                                    updateStatement = updateStatement + dr.StartBraces + dr.Column + " NOT IN ('" + dr.value + "') " + dr.EndBraces + dr.Operator + " ";
                                else
                                    updateStatement = updateStatement + dr.StartBraces + dr.Column + " NOT IN (" + dr.ConstantValue + ") " + dr.EndBraces + dr.Operator + " ";

                            //updateStatement = updateStatement + dr.StartBraces + dr.Column + " ='" + dr.value + "' " + dr.EndBraces + dr.Operator + " ";

                        }


                    }
                    updateStatement = updateStatement + " )";
                    if (strcommand == "SAVE")
                    {
                        var paramName = new string[] { "ClientId", "Updatestring" };
                        var paramValue = new object[] { clientid, updateStatement };
                        // var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                        var res = unitOfWork.GetDataFromStoredProcedure("GenerateTower", paramName, paramValue);
                        var updatequeryinSSIS = updateStatement + " and clientid=@clientid and jobid=@jobid and (select count(1) from SourceDataMapping where MappingId=(select mappingid from jobdetail where id=@jobid) and MappingCol='ContractId' and sField is not null)<=0";
                        var ssisupdatequery = new SSIS_INUpdateQuery { ClientId = clientid, Type = "SSISUPDATECONTRACT", Query = updatequeryinSSIS, CreatedBy = uid, CreatedTime = DateTime.Now };
                        unitOfWork.SSIS_INUpdateQuery.Insert(ssisupdatequery);
                    }
                    DisplayQuery = DisplayQuery + "<br/>" + updateStatement;

                }

            }
            unitOfWork.Save();
            unitOfWork.Dispose();
            return DisplayQuery;

        }


        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public static IEnumerable<INColumnValueModel> readTrackConfiguration(int ClientId)
        {
            Db db = new Db();
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var disp = unitOfWork.INColumnValue.Get(g => g.ClientId == ClientId);
            var returnModel = disp.Select(e => new INColumnValueModel
            {
                Id = e.Id,
                ColumnName = e.ColumnName,
                Value = e.Value
            });

            return returnModel;
        }


        public static IEnumerable<SelectListItem> GetListOfLogicalColumn(int clientid)
        {
            //-------------------------------
            //Db db = new Db();
            //List<string> colList= new List<string>();
            //using(var contextFactory = new DbContextFactory())
            //{
            //    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            //    var param1 = new string[] { };
            //    var paramVal = new object[] { };
            //    var result = unitOfWork.GetDataFromStoredProcedure("getIncidentColumnList", param1, paramVal);

            //    colList = result.AsEnumerable().Select(r => r.Field<string>("COLUMN_NAME")).ToList();
            //}

            //List<SelectListItem> li = new List<SelectListItem>();

            //foreach(var a in colList)
            //{
            //    li.Add(new SelectListItem
            //    {
            //        Text = a.ToString(),
            //        Value = a.ToString()
            //    });
            //}
            //return li;
            //----------------------------------

            IList<INContractRuleModel> AppColumnDetail = new List<INContractRuleModel>();
            var paramName = new string[] { "tableName" };
            var paramValue = new object[] { "Incident" };

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                var res = unitOfWork.GetDataFromStoredProcedure("GetTableColumnNameswithFilterForTower", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new INContractRuleModel();
                    var FieldDatatpe = dr["DataType"].ToString();

                    //if (FieldDatatpe == "nvarchar" || FieldDatatpe =="varchar")
                    //{
                    // var FieldLength = Convert.ToInt32(dr["Size"]);
                    //if (FieldLength < 260)
                    //{
                    coldet.INFieldName = dr["AppField"].ToString();
                    AppColumnDetail.Add(coldet);
                    //}
                    //}

                }
            }
            var list = AppColumnDetail.Select(f => new SelectListItem { Value = f.INFieldName, Text = f.INFieldName });



            return list;
        }


        public static IList<INColumnValueRuleModel> getRuleListLogical(int Id, int clientID)
        {

            IList<INColumnValueRuleModel> EstCategoryList = new List<INColumnValueRuleModel>();
            using (var contextFactory = new DbContextFactory())
            {
                Db db = new Db();
                IEnumerable<INColumnValueRule> RowDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());


                if (Id != 0)

                    //RowDetails = unitOfWork.INColumnValueRule.Get(cid => cid.INColumnValueId == Id).ToList();

                    RowDetails = (from a in db.INColumnValueRule
                                  where a.INColumnValueId == Id && a.ClientId == clientID
                                  select a).ToList();


                if (RowDetails != null)
                {
                    foreach (var _detail in RowDetails)
                    {
                        INColumnValueRuleModel bb = new INColumnValueRuleModel();
                        bb.Id = _detail.Id;
                        bb.INFieldName = _detail.INFieldName;
                        bb.FieldValue = _detail.FieldValue;
                        bb.Operator = _detail.Operator;
                        bb.GroupEndOperator = _detail.GroupEndOperator;
                        bb.GroupStartOperator = _detail.GroupStartOperator;
                        bb.Condition = _detail.Condition;
                        bb.ConstantValue = _detail.ConstantValue;
                        EstCategoryList.Add(bb);
                    }
                }
                return EstCategoryList;
            }

        }

        public static int addTeamRuleLogical(INColumnValueRuleModel Input, int clientID, int uid)
        {
            int ra_id = 0;
            Db db = new Db();

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var _Details = new INColumnValueRule();
                    _Details.INColumnValueId = Input.INColumnValueId;
                    _Details.INFieldName = Input.INFieldName;
                    _Details.FieldValue = Input.FieldValue;
                    _Details.Operator = Input.Operator;
                    _Details.GroupEndOperator = Input.GroupEndOperator;
                    _Details.GroupStartOperator = Input.GroupStartOperator;
                    _Details.Condition = Input.Condition;
                    _Details.ConstantValue = Input.ConstantValue;
                    _Details.ClientId = clientID;
                    _Details.Created_By = uid;
                    _Details.Created_Date = DateTime.Now;
                    db.INColumnValueRule.Add(_Details);
                    db.SaveChanges();
                    // unitOfWork.INContractRule.Insert(_Details);
                    //db.INContractRule.Add(_Details);
                    //unitOfWork.Save();
                    ra_id = _Details.INColumnValueId;

                    return ra_id;
                }
            }
            catch (Exception ex)
            {
                Log.Error("IncidentNavigatorHelper error 'addTeamRuleLogical' ", ex);
            }
            return 0;
        }

        public static bool updateTeamRuleLogical(INColumnValueRuleModel _Input, int uid)
        {
            Db db = new Db();
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    //   var updatetitem = unitOfWork.INContractRule.GetByID(_Input.Id);
                    //INContractRule updateitem = (from a in db.INContractRule
                    //                  where a.Id == _Input.Id
                    //                  select a).FirstOrDefault();
                    INColumnValueRule updateitem = db.INColumnValueRule.FirstOrDefault(x => x.Id == _Input.Id);


                    updateitem.INFieldName = _Input.INFieldName;
                    updateitem.FieldValue = _Input.FieldValue;
                    updateitem.Operator = _Input.Operator;
                    updateitem.GroupEndOperator = _Input.GroupEndOperator;
                    updateitem.GroupStartOperator = _Input.GroupStartOperator;
                    updateitem.Condition = _Input.Condition;
                    updateitem.ConstantValue = _Input.ConstantValue;
                    updateitem.Modified_By = uid;
                    updateitem.Modified_Date = DateTime.Now;
                    //db.SaveChanges();


                    unitOfWork.INColumnValueRule.Update(updateitem);

                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("IncidentNavigatorHelper error 'updateTeamRuleLogical' ", ex);
            }
            return false;
        }

        public static bool RemoveTeamRuleLogical(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.INColumnValueRule.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }


        public static IEnumerable<SelectListItem> getColumnListLogical(int ClientId)
        {
            IList<INColumnValueRuleModel> AppColumnDetail = new List<INColumnValueRuleModel>();
            var paramName = new string[] { "tableName" };
            var paramValue = new object[] { "Incident" };

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                var res = unitOfWork.GetDataFromStoredProcedure("GetTableColumnNameswithFilterForTower", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new INColumnValueRuleModel();
                    var FieldDatatpe = dr["DataType"].ToString();

                    //if (FieldDatatpe == "nvarchar")
                    //{
                    //    var FieldLength = Convert.ToInt32(dr["Size"]);
                    //    if (FieldLength < 260)
                    //    {
                    coldet.INFieldName = dr["AppField"].ToString();
                    AppColumnDetail.Add(coldet);
                    //    }
                    //}

                }
            }
            var list = AppColumnDetail.Select(f => new SelectListItem { Value = f.INFieldName, Text = f.INFieldName });
            return list;
        }

        public static string GenerateNExecRuleLogical(int clientid, string strcommand, int uid)
        {

            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            if (strcommand == "SAVE")
            {
                var param1 = new string[] { "ClientId", "Type" };
                var paramVal = new object[] { clientid, "SSISUPDATECOLUMN" };
                var result = unitOfWork.GetDataFromStoredProcedure("DeleteSSISUpdateQuery", param1, paramVal);
            }
            // var ssisupdatequery = new SSIS_INUpdateQuery { ClientId = clientid, Type = "SSISUPDATE", Query = updateStatement };
            // unitOfWork.SSIS_INUpdateQuery.Insert(ssisupdatequery);
            var db = new Db();
            var ColumnRuledata = (from tm in db.INColumnValue
                                  join tr in db.INColumnValueRule on tm.Id equals tr.INColumnValueId
                                  where tm.ClientId == clientid && tr.ClientId == clientid
                                  select new { ruleid = tr.Id, INColumnValueId = tm.Id, ColumnName = tm.ColumnName, ColumnValue = tm.Value, Column = tr.INFieldName, value = tr.FieldValue, Operator = tr.Operator, StartBraces = tr.GroupStartOperator, EndBraces = tr.GroupEndOperator, Condition = tr.Condition, ConstantValue = tr.ConstantValue }).OrderBy(e => e.INColumnValueId).ToList();
            var ColumnList = (from a in ColumnRuledata
                              select new { INColumnValueId = a.INColumnValueId, ColumnValue = a.ColumnValue, ColumnName = a.ColumnName }).Distinct().OrderBy(a => a.INColumnValueId);

            string DisplayQuery = "";
            var updateStatement = "";
            //  var DisplayQuery = "";
            foreach (var item in ColumnList)
            {


                updateStatement = "UPDATE Incident set " + item.ColumnName + "='" + item.ColumnValue + "' where clientid=" + clientid.ToString() + " AND (";

                if (ColumnRuledata.Count() > 0)
                {
                    var ColumnRule = ColumnRuledata.Where(w => w.INColumnValueId == item.INColumnValueId).OrderBy(o => o.ruleid).ToList();
                    var count = ColumnRule.Count();
                    int i = 0;
                    foreach (var dr in ColumnRule)
                    {
                        i++;
                        if (i == count)
                        {
                            if (string.IsNullOrEmpty(dr.value) && string.IsNullOrEmpty(dr.ConstantValue))
                            {
                                //string tempCond = "";
                                if (dr.Condition == "=" || dr.Condition == "IS NULL")
                                    updateStatement = updateStatement = updateStatement + dr.StartBraces + " ( " + dr.Column + " IS NULL OR " + dr.Column + "='') " + dr.EndBraces;
                                else if (dr.Condition == "<>" || dr.Condition == "IS NOT NULL")
                                    updateStatement = updateStatement = updateStatement + dr.StartBraces + " ( " + dr.Column + " IS NOT NULL AND " + dr.Column + "<>'' ) " + dr.EndBraces;
                            }
                            else
                            {
                                if (dr.Condition == "=" || dr.Condition == "<>")
                                {
                                    if (!string.IsNullOrEmpty(dr.value))
                                        updateStatement = updateStatement + dr.StartBraces + dr.Column + dr.Condition + " '" + dr.value + "' " + dr.EndBraces;
                                    else
                                        updateStatement = updateStatement + dr.StartBraces + dr.Column + dr.Condition + dr.ConstantValue + dr.EndBraces;
                                }
                                else if (dr.Condition == "CONTAINS")
                                {
                                    if (!string.IsNullOrEmpty(dr.value))
                                    {
                                        updateStatement = updateStatement + dr.StartBraces + dr.Column + " LIKE '%" + dr.value + "%' " + dr.EndBraces;
                                    }
                                    else
                                    {
                                        updateStatement = updateStatement + dr.StartBraces + dr.Column + " LIKE '%" + dr.ConstantValue + "%' " + dr.EndBraces;
                                    }
                                }
                                else if (dr.Condition == "NOT CONTAINS")
                                {
                                    if (!string.IsNullOrEmpty(dr.value))
                                    {
                                        updateStatement = updateStatement + dr.StartBraces + dr.Column + " NOT LIKE '%" + dr.value + "%' " + dr.EndBraces;
                                    }
                                    else
                                    {
                                        updateStatement = updateStatement + dr.StartBraces + dr.Column + " NOT LIKE '%" + dr.ConstantValue + "%' " + dr.EndBraces;
                                    }
                                }

                                else if (dr.Condition == "IN")
                                {
                                    if (!string.IsNullOrEmpty(dr.value))
                                        updateStatement = updateStatement + dr.StartBraces + dr.Column + " IN ('" + dr.value + "') " + dr.EndBraces;
                                    else
                                        updateStatement = updateStatement + dr.StartBraces + dr.Column + " IN (" + dr.ConstantValue + ") " + dr.EndBraces;
                                }
                                else if (dr.Condition == "NOT IN")
                                {
                                    if (!string.IsNullOrEmpty(dr.value))
                                        updateStatement = updateStatement + dr.StartBraces + dr.Column + " NOT IN ('" + dr.value + "') " + dr.EndBraces;
                                    else
                                        updateStatement = updateStatement + dr.StartBraces + dr.Column + " NOT IN (" + dr.ConstantValue + ") " + dr.EndBraces;
                                }
                            }
                        }
                        else
                        {
                            if (dr.Condition == "=" || dr.Condition == "<>")
                                if (string.IsNullOrEmpty(dr.value) && string.IsNullOrEmpty(dr.ConstantValue) && dr.Condition == "=")
                                    updateStatement = updateStatement + dr.StartBraces + '(' + dr.Column + "='' OR " + dr.Column + " is null) " + dr.EndBraces + dr.Operator + " ";
                                else if (string.IsNullOrEmpty(dr.value) && string.IsNullOrEmpty(dr.ConstantValue) && dr.Condition == "<>")
                                    updateStatement = updateStatement + dr.StartBraces + '(' + dr.Column + "<>'' AND " + dr.Column + " is not null) " + dr.EndBraces + dr.Operator + " ";
                                else if (!string.IsNullOrEmpty(dr.value))
                                    updateStatement = updateStatement + dr.StartBraces + dr.Column + dr.Condition + " '" + dr.value + "' " + dr.EndBraces + dr.Operator + " ";
                                else
                                    updateStatement = updateStatement + dr.StartBraces + dr.Column + dr.Condition + dr.ConstantValue + dr.EndBraces + dr.Operator + " ";
                            else if (dr.Condition == "CONTAINS")
                                if (!string.IsNullOrEmpty(dr.value))
                                    updateStatement = updateStatement + dr.StartBraces + dr.Column + " LIKE '%" + dr.value + "%' " + dr.EndBraces + dr.Operator + " ";
                                else
                                    updateStatement = updateStatement + dr.StartBraces + dr.Column + " LIKE '%" + dr.ConstantValue + "%' " + dr.EndBraces + dr.Operator + " ";
                            else if (dr.Condition == "NOT CONTAINS")
                                if (!string.IsNullOrEmpty(dr.value))
                                    updateStatement = updateStatement + dr.StartBraces + dr.Column + " NOT LIKE '%" + dr.value + "%' " + dr.EndBraces + dr.Operator + " ";
                                else
                                    updateStatement = updateStatement + dr.StartBraces + dr.Column + " NOT LIKE '%" + dr.ConstantValue + "%' " + dr.EndBraces + dr.Operator + " ";

                            else if (dr.Condition == "IN")
                                if (!string.IsNullOrEmpty(dr.value))
                                    updateStatement = updateStatement + dr.StartBraces + dr.Column + " IN ('" + dr.value + "') " + dr.EndBraces + dr.Operator + " ";
                                else
                                    updateStatement = updateStatement + dr.StartBraces + dr.Column + " IN (" + dr.ConstantValue + ") " + dr.EndBraces + dr.Operator + " ";
                            else if (dr.Condition == "NOT IN")
                                if (!string.IsNullOrEmpty(dr.value))
                                    updateStatement = updateStatement + dr.StartBraces + dr.Column + " NOT IN ('" + dr.value + "') " + dr.EndBraces + dr.Operator + " ";
                                else
                                    updateStatement = updateStatement + dr.StartBraces + dr.Column + " NOT IN (" + dr.ConstantValue + ") " + dr.EndBraces + dr.Operator + " ";

                            //updateStatement = updateStatement + dr.StartBraces + dr.Column + " ='" + dr.value + "' " + dr.EndBraces + dr.Operator + " ";

                        }


                    }
                    updateStatement = updateStatement + " )";
                    if (strcommand == "SAVE")
                    {
                        var paramName = new string[] { "ClientId", "Updatestring" };
                        var paramValue = new object[] { clientid, updateStatement };
                        // var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                        var res = unitOfWork.GetDataFromStoredProcedure("GenerateTower", paramName, paramValue);
                        var updatequeryinSSIS = updateStatement + " and clientid=@clientid and jobid=@jobid and (select count(1) from SourceDataMapping where MappingId=(select mappingid from jobdetail where id=@jobid) and MappingCol= '" + item.ColumnName + "' and sField is not null)<=0";
                        var ssisupdatequery = new SSIS_INUpdateQuery { ClientId = clientid, Type = "SSISUPDATECOLUMN", Query = updatequeryinSSIS, CreatedBy = uid, CreatedTime = DateTime.Now };
                        unitOfWork.SSIS_INUpdateQuery.Insert(ssisupdatequery);
                    }
                    DisplayQuery = DisplayQuery + "<br/>" + updateStatement;

                }

            }
            unitOfWork.Save();
            unitOfWork.Dispose();
            return DisplayQuery;

        }



        public static int CreateColumnValue(INColumnValueModel InColumnValuevar, int uid, int clientID)
        {
            int id = 0;
            //var entityMapper = new Mapper<ClientTicketInfo, TicketInfoViewModel>();
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                    var INColumnValue = new INColumnValue();

                    INColumnValue.ColumnName = InColumnValuevar.ColumnName;
                    INColumnValue.Value = InColumnValuevar.Value;
                    INColumnValue.Created_Date = DateTime.Now;
                    INColumnValue.Modified_Date = DateTime.Now;
                    INColumnValue.Created_By = uid;
                    INColumnValue.Modified_By = uid;
                    INColumnValue.ClientId = clientID;


                    unitOfWork.INColumnValue.Insert(INColumnValue);

                    unitOfWork.Save();
                    id = INColumnValue.Id;
                    return id;
                }
            }
            catch (Exception ex)
            {
                Log.Error("IncidentNavigatorHelper error 'CreateColumnValue' ", ex);
            }
            return 0;
        }

        public static bool UpdateTrackConfiguration(INColumnValueModel InColumnValuevar, int uid)
        {

            try
            {

                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.INColumnValue.GetByID(InColumnValuevar.Id);
                    updatetitem.ColumnName = InColumnValuevar.ColumnName;
                    updatetitem.Value = InColumnValuevar.Value;
                    updatetitem.Modified_Date = DateTime.Now;
                    updatetitem.Modified_By = uid;

                    unitOfWork.INColumnValue.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("IncidentNavigatorHelper error 'UpdateTrackConfiguration' ", ex);
            }
            return false;
        }

        public static bool RemoveTrackConfiguration(int Id, int ClientID)
        {
            Db db = new Db();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var resultID = (from a in db.INColumnValueRule
                                where a.INColumnValueId == Id && a.ClientId == ClientID
                                select a.Id).ToList();

                if (resultID.Count > 0)
                {
                    foreach (var i in resultID)
                    {
                        unitOfWork.INColumnValueRule.Delete(i);
                    }
                }

                unitOfWork.Save();


                unitOfWork.INColumnValue.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }
        public static IList<SLAMaintenenceModel> INDashboardDrilldownData(string Charttype, int ClientId, string TicketType, string Status, string Track, string Priority, string Datetype, DateTime? StartDate, DateTime? EndDate, string contract, string Category)
        {
            IList<SLAMaintenenceModel> Modellist = new List<SLAMaintenenceModel>();
            var paramName = new string[] { "Charttype", "ClientId", "TicketType", "Status", "Track", "Priority", "Datetype", "StartDate", "EndDate", "contract", "CategoryValue" };
            var paramValue = new object[] { Charttype, ClientId, TicketType, Status, Track, Priority, Datetype, StartDate, EndDate, contract, Category };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("INDashboardGetChartData_drilldown_prod", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new SLAMaintenenceModel();
                    coldet.Id = Convert.ToInt32(dr["ID"]);
                    coldet.TicketID = dr["IncidentId"].ToString();
                    if (!(dr["Type"] is DBNull))
                        coldet.Type = dr["Type"].ToString();
                    coldet.Description = dr["Description"].ToString();
                    coldet.Priority = dr["Priority"].ToString();
                    if (!(dr["Status"] is DBNull))
                        coldet.Status = dr["Status"].ToString();
                    if (!(dr["AssignedToSupportGroup"] is DBNull))
                        coldet.AssignedToSupportGroup = dr["AssignedToSupportGroup"].ToString();
                    if (!(dr["AssignedDateTime"] is DBNull))
                        coldet.AssignedDateTime = Convert.ToDateTime(dr["AssignedDateTime"]);
                    if (!(dr["ClosedDateTime"] is DBNull))
                        coldet.ClosedDateTime = Convert.ToDateTime(dr["ClosedDateTime"]);
                    if (!(dr["CreatedDateTime"] is DBNull))
                        coldet.CreatedDateTime = Convert.ToDateTime(dr["CreatedDateTime"]);
                    if (!(dr["LastUpdatedDateTime"] is DBNull))
                        coldet.LastUpdatedDateTime = Convert.ToDateTime(dr["LastUpdatedDateTime"]);
                    if (!(dr["ResolutionDateTime"] is DBNull))
                        coldet.ResolutionDateTime = Convert.ToDateTime(dr["ResolutionDateTime"]);
                    if (!(dr["Resolutiontype"] is DBNull))
                        coldet.Resolutiontype = dr["Resolutiontype"].ToString();
                    if (!(dr["Tower"] is DBNull))
                        coldet.Track = dr["Tower"].ToString();
                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }
        public static IList<SLAMaintenenceModel> INTrendDrillDownData(int ClientId, string Charttype, string Datetype, DateTime StartDate, DateTime EndDate, string TicketType, string Status, string Track, string Priority, string contract, string category, string InflowOutFlowType)
        {
            IList<SLAMaintenenceModel> Modellist = new List<SLAMaintenenceModel>();
            var paramName = new string[] { "ClientId", "StartDate", "EndDate", "Dim", "Fieldname", "TicketType", "Status", "Track", "Priority", "contract", "category", "InflowOutFlowType" };
            var paramValue = new object[] { ClientId, StartDate, EndDate, Charttype, Datetype, TicketType, Status, Track, Priority, contract, category, InflowOutFlowType };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("SP_Dashboard1TicketCount_DrillDown_prod", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new SLAMaintenenceModel();
                    coldet.Id = Convert.ToInt32(dr["ID"]);
                    coldet.TicketID = dr["IncidentId"].ToString();
                    if (!(dr["Type"] is DBNull))
                        coldet.Type = dr["Type"].ToString();
                    coldet.Description = dr["Description"].ToString();
                    coldet.Priority = dr["Priority"].ToString();
                    if (!(dr["Status"] is DBNull))
                        coldet.Status = dr["Status"].ToString();
                    if (!(dr["AssignedToSupportGroup"] is DBNull))
                        coldet.AssignedToSupportGroup = dr["AssignedToSupportGroup"].ToString();
                    if (!(dr["AssignedDateTime"] is DBNull))
                        coldet.AssignedDateTime = Convert.ToDateTime(dr["AssignedDateTime"]);
                    if (!(dr["ClosedDateTime"] is DBNull))
                        coldet.ClosedDateTime = Convert.ToDateTime(dr["ClosedDateTime"]);
                    if (!(dr["CreatedDateTime"] is DBNull))
                        coldet.CreatedDateTime = Convert.ToDateTime(dr["CreatedDateTime"]);
                    if (!(dr["LastUpdatedDateTime"] is DBNull))
                        coldet.LastUpdatedDateTime = Convert.ToDateTime(dr["LastUpdatedDateTime"]);
                    if (!(dr["ResolutionDateTime"] is DBNull))
                        coldet.ResolutionDateTime = Convert.ToDateTime(dr["ResolutionDateTime"]);
                    if (!(dr["Resolutiontype"] is DBNull))
                        coldet.Resolutiontype = dr["Resolutiontype"].ToString();
                    if (!(dr["Tower"] is DBNull))
                        coldet.Track = dr["Tower"].ToString();
                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }
        public static IList<SLAMaintenenceModel> INDashboard2DrilldownData(int ClientId, string drpval, string TicketType, string Status, string Track, string Priority, string dpnVal, string Datetype, DateTime StartDate, DateTime EndDate, string contract, string Category, string Categoryseries)
        {
            IList<SLAMaintenenceModel> Modellist = new List<SLAMaintenenceModel>();
            var paramName = new string[] { "ClientId", "StartDate", "EndDate", "Dim", "Fieldname", "TicketType", "Status", "Track", "Priority", "dpnVal", "contract", "categoryValue", "CategorySeriesValue" };
            var paramValue = new object[] { ClientId, StartDate, EndDate, drpval, Datetype, TicketType, Status, Track, Priority, dpnVal, contract, Category, Categoryseries };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("SP_Dashboard2TicketCount_DrillDown_prod", paramName, paramValue);
                //if (dpnVal.Length > 0)
                //{
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new SLAMaintenenceModel();
                    coldet.Id = Convert.ToInt32(dr["ID"]);
                    coldet.TicketID = dr["IncidentId"].ToString();
                    if (!(dr["Type"] is DBNull))
                        coldet.Type = dr["Type"].ToString();
                    coldet.Description = dr["Description"].ToString();
                    coldet.Priority = dr["Priority"].ToString();
                    if (!(dr["Status"] is DBNull))
                        coldet.Status = dr["Status"].ToString();
                    if (!(dr["AssignedToSupportGroup"] is DBNull))
                        coldet.AssignedToSupportGroup = dr["AssignedToSupportGroup"].ToString();
                    if (!(dr["AssignedDateTime"] is DBNull))
                        coldet.AssignedDateTime = Convert.ToDateTime(dr["AssignedDateTime"]);
                    if (!(dr["ClosedDateTime"] is DBNull))
                        coldet.ClosedDateTime = Convert.ToDateTime(dr["ClosedDateTime"]);
                    if (!(dr["CreatedDateTime"] is DBNull))
                        coldet.CreatedDateTime = Convert.ToDateTime(dr["CreatedDateTime"]);
                    if (!(dr["LastUpdatedDateTime"] is DBNull))
                        coldet.LastUpdatedDateTime = Convert.ToDateTime(dr["LastUpdatedDateTime"]);
                    if (!(dr["ResolutionDateTime"] is DBNull))
                        coldet.ResolutionDateTime = Convert.ToDateTime(dr["ResolutionDateTime"]);
                    if (!(dr["Resolutiontype"] is DBNull))
                        coldet.Resolutiontype = dr["Resolutiontype"].ToString();
                    if (!(dr["Tower"] is DBNull))
                        coldet.Track = dr["Tower"].ToString();
                    Modellist.Add(coldet);
                }
                //}
            }
            return Modellist;
        }
        public static List<CompReportModel> GetCompRecords(int clientid)
        {
            List<CompReportModel> rec = new List<CompReportModel>();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                string[] parmname = new string[] { "ClientId", "ReportDate" };
                var values = new object[] { clientid, DateTime.Now };
                var dtSearchRes = unitOfWork.GetDataFromStoredProcedure("GetComplianceReportByDate", parmname, values);
                foreach (DataRow dr in dtSearchRes.Rows)
                {
                    var cmModel = new CompReportModel();
                    cmModel.IncidentID = dr["incidentid"].ToString();
                    cmModel.Priority = dr["Priority"].ToString();
                    cmModel.Createddate = Convert.ToDateTime(dr["Createddatetime"]);
                    cmModel.Status = dr["Status"].ToString();
                    cmModel.AssignedTo = dr["Assignedtoperson"].ToString();
                    cmModel.AssignedGroup = dr["Assignedtosupportgroup"].ToString();
                    cmModel.SLAStaus = dr["SLAStatus"].ToString();
                    if (!(dr["ResolutionCalcX"] is DBNull))
                        cmModel.ResStartTime = Convert.ToDateTime(dr["ResolutionCalcX"]);
                    cmModel.SLATarget = dr["DisplayMetricTarget"].ToString();
                    cmModel.Tower = dr["Tower"].ToString();
                    cmModel.PendingTime = dr["Totalsuspenddurationertoresolution"].ToString();
                    cmModel.Age = dr["Ticketage"].ToString();
                    cmModel.statusicon = "";
                    if (cmModel.SLAStaus == "Breached")
                    {
                        cmModel.statusicon = "breached";
                    }
                    else if (cmModel.SLAStaus == "Warning")
                    {
                        cmModel.statusicon = "warning_SLA";
                    }
                    else if (cmModel.SLAStaus == "OnTarget")
                    {
                        cmModel.statusicon = "ontarget";
                    }
                    else if (cmModel.SLAStaus == "NA")
                    {
                        cmModel.statusicon = "notapplicable";
                    }
                    else
                    {
                        cmModel.statusicon = "";
                    }
                    if (!(dr["ResolutionDueOn"] is DBNull))
                        cmModel.ResDueOn = Convert.ToDateTime(dr["ResolutionDueOn"]);
                    rec.Add(cmModel);
                }
            }
            return rec;
        }

        public static IEnumerable<KEDBInstanceModel> getKEDB(int clientid, string whereclause)
        {
            IList<KEDBInstanceModel> kedbList = new List<KEDBInstanceModel>();

            string tempWhereclause = whereclause.Replace(" Starts with '", " LIKE '");
            tempWhereclause = tempWhereclause.Replace(" Is Equal To '", " = '");
            tempWhereclause = tempWhereclause.Replace(" Not Equal To '", " <> '");
            tempWhereclause = tempWhereclause.Replace(" Greater Than '", " > '");
            tempWhereclause = tempWhereclause.Replace(" Less Than '", " < '");
            tempWhereclause = tempWhereclause.Replace("TicketId", "I.IncidentID");
            tempWhereclause = tempWhereclause.Replace("Issue Description", "I.Description");
            tempWhereclause = tempWhereclause.Replace("Config Item Name", "I.ConfigItemName");
            tempWhereclause = tempWhereclause.Replace("KEDB_NO", "K.KEDBNumber");
            tempWhereclause = tempWhereclause.Replace("Opened On", "I.Createddatetime");
            tempWhereclause = tempWhereclause.Replace(" GMT+0530 (India Standard Time)", "");
            tempWhereclause = tempWhereclause.Replace("Sun", "").Replace("Mon", "").Replace("Tue", "").Replace("Wed", "").Replace("Thu", "").Replace("Fri", "").Replace("Sat", "");

            int startsCount = 0;
            foreach (Match m in Regex.Matches(tempWhereclause, "LIKE '"))
            {
                startsCount++;
            }

            int incrementPositionStarts = 0;
            int posOfEndQuoteStarts = 0;
            for (int i = 0; i < startsCount; i++)
            {
                int posOfLikePerStarts = tempWhereclause.IndexOf(" LIKE '", incrementPositionStarts) + 7;
                posOfEndQuoteStarts = tempWhereclause.IndexOf("'", posOfLikePerStarts);
                int tempLength = tempWhereclause.Length - posOfEndQuoteStarts; //position of end quote
                tempWhereclause = (tempWhereclause.Substring(0, posOfEndQuoteStarts)) + "%'" + (tempWhereclause.Substring(posOfEndQuoteStarts + 1, tempLength - 1));
                incrementPositionStarts = posOfEndQuoteStarts;
            }

            tempWhereclause = tempWhereclause.Replace(" Contains '", " LIKE '%");

            int wordCount = 0;
            foreach (Match m in Regex.Matches(tempWhereclause, "LIKE '%"))
            {
                wordCount++;
            }

            int incrementPosition = 0;
            int posOfEndQuote = 0;
            for (int i = 0; i < wordCount; i++)
            {
                int posOfLikePer = tempWhereclause.IndexOf(" LIKE '%", incrementPosition) + 7;
                posOfEndQuote = tempWhereclause.IndexOf("'", posOfLikePer);
                int tempLength = tempWhereclause.Length - posOfEndQuote; //position of end quote
                tempWhereclause = (tempWhereclause.Substring(0, posOfEndQuote)) + "%'" + (tempWhereclause.Substring(posOfEndQuote + 1, tempLength - 1));
                incrementPosition = posOfEndQuote;
            }




            DataTable dt = new DataTable();
            var paramName = new string[] { "ClientId", "Criteria" };
            var paramValue = new object[] { clientid, tempWhereclause };

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("[KEDBSEarch]", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new KEDBInstanceModel();
                    coldet.Id = Convert.ToInt32(dr["ID"]);
                    if (!(dr["KEDBID"] is DBNull))
                        coldet.KEDBID = Convert.ToInt32(dr["KEDBID"]);
                    if (!(dr["KEDBNumber"] is DBNull))
                        coldet.KEDBNumber = dr["KEDBNumber"].ToString();
                    if (!(dr["Description"] is DBNull))
                        coldet.Description = dr["Description"].ToString();
                    if (!(dr["ISSUETYPE"] is DBNull))
                        coldet.KedbIssueType = dr["ISSUETYPE"].ToString();
                    //coldet.Description = dr["Description"].ToString();
                    if (!(dr["Cause"] is DBNull))
                        coldet.Cause = dr["Cause"].ToString();
                    //coldet.Priority = dr["Priority"].ToString();
                    if (!(dr["Tower"] is DBNull))
                        coldet.Tower = dr["Tower"].ToString();
                    if (!(dr["Application"] is DBNull))
                        coldet.ConfigItemName = dr["Application"].ToString();
                    if (!(dr["SOLUTIONCATEGORY"] is DBNull))
                        coldet.SolutionCategory = (dr["SOLUTIONCATEGORY"].ToString());
                    if (!(dr["SolStatus"] is DBNull))
                        coldet.SolStatus = dr["SolStatus"].ToString();
                    if (!(dr["BusinessFunction"] is DBNull))
                        coldet.BusinessFunction = dr["BusinessFunction"].ToString();
                    if (!(dr["CIName"] is DBNull))
                        coldet.CIName = dr["CIName"].ToString();
                    if (!(dr["TicketId"] is DBNull))
                        coldet.TicketId = dr["TicketId"].ToString();
                    if (!(dr["isAutoIdentified"] is DBNull))
                        coldet.isAutoIdentifiedVal = dr["isAutoIdentified"].ToString();
                    else
                        coldet.isAutoIdentifiedVal = "-";
                    kedbList.Add(coldet);
                }

            }

            return kedbList;

        }

        public static IEnumerable<KEDBInstanceModel> getKEDB_Drilldown(int clientid, string Status_action, string IssueTypeP_action, string Tower_action)
        {
            IList<KEDBInstanceModel> kedbList = new List<KEDBInstanceModel>();

            DataTable dt = new DataTable();
            var paramName = new string[] { "ClientId", "Status", "Issue", "Tower" };
            var paramValue = new object[] { clientid, Status_action, IssueTypeP_action, Tower_action };

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("[KEDB_SolutionStatus_Drilldown]", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new KEDBInstanceModel();
                    //coldet.Id = Convert.ToInt32(dr["ID"]);
                    if (!(dr["KEDBID"] is DBNull))
                        coldet.KEDBID = Convert.ToInt32(dr["KEDBID"]);
                    if (!(dr["KEDBNumber"] is DBNull))
                        coldet.KEDBNumber = dr["KEDBNumber"].ToString();
                    if (!(dr["ISSUETYPE"] is DBNull))
                        coldet.KedbIssueType = dr["ISSUETYPE"].ToString();
                    if (!(dr["Tower"] is DBNull))
                        coldet.Tower = dr["Tower"].ToString();
                    if (!(dr["SOLUTIONCATEGORY"] is DBNull))
                        coldet.SolutionCategory = (dr["SOLUTIONCATEGORY"].ToString());
                    if (!(dr["SolStatus"] is DBNull))
                        coldet.SolStatus = dr["SolStatus"].ToString();
                    if (!(dr["KEDBDescription"] is DBNull))
                        coldet.Description = dr["KEDBDescription"].ToString();
                    if (!(dr["KnowledgeArticle"] is DBNull))
                        coldet.KnowledgeArticle = dr["KnowledgeArticle"].ToString();
                    if (!(dr["ResolutionSteps"] is DBNull))
                        coldet.ResolutionSteps = dr["ResolutionSteps"].ToString();
                    if (!(dr["FileName"] is DBNull))
                        coldet.FileName = dr["FileName"].ToString();

                    kedbList.Add(coldet);
                }

            }

            return kedbList;

        }


        public static IEnumerable<KEDBInstanceModel> getKEDBSummary_Drilldown(int clientid, string Tower, string KEDBNumber)
        {
            IList<KEDBInstanceModel> kedbList = new List<KEDBInstanceModel>();

            DataTable dt = new DataTable();
            var paramName = new string[] { "ClientId", "@Tower", "@KEDBNumber" };
            var paramValue = new object[] { clientid, Tower, KEDBNumber };

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("[KEDB_SummaryKEDB_Drilldown]", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new KEDBInstanceModel();
                    //coldet.Id = Convert.ToInt32(dr["ID"]);
                    if (!(dr["KEDBID"] is DBNull))
                        coldet.KEDBID = Convert.ToInt32(dr["KEDBID"]);
                    if (!(dr["KEDBNumber"] is DBNull))
                        coldet.KEDBNumber = dr["KEDBNumber"].ToString();
                    if (!(dr["ISSUETYPE"] is DBNull))
                        coldet.KedbIssueType = dr["ISSUETYPE"].ToString();
                    if (!(dr["Tower"] is DBNull))
                        coldet.Tower = dr["Tower"].ToString();
                    if (!(dr["SOLUTIONCATEGORY"] is DBNull))
                        coldet.SolutionCategory = (dr["SOLUTIONCATEGORY"].ToString());
                    if (!(dr["SolStatus"] is DBNull))
                        coldet.SolStatus = dr["SolStatus"].ToString();
                    if (!(dr["KEDBDescription"] is DBNull))
                        coldet.Description = dr["KEDBDescription"].ToString();
                    if (!(dr["KnowledgeArticle"] is DBNull))
                        coldet.KnowledgeArticle = dr["KnowledgeArticle"].ToString();
                    if (!(dr["ResolutionSteps"] is DBNull))
                        coldet.ResolutionSteps = dr["ResolutionSteps"].ToString();
                    if (!(dr["FileName"] is DBNull))
                        coldet.FileName = dr["FileName"].ToString();

                    kedbList.Add(coldet);
                }

            }

            return kedbList;

        }
        public static IEnumerable<KEDBInstanceModel> getKEDB_RF_Drilldown(int clientid, string KADoc_action, string IssueTypeP_action, string Tower_action, DateTime fromDate, DateTime toDate)
        {
            IList<KEDBInstanceModel> kedbList = new List<KEDBInstanceModel>();

            DataTable dt = new DataTable();

            var paramName = new string[] { "ClientId", "KADoc", "Issue", "Tower", "StartDate", "EndDate" };
            var paramValue = new object[] { clientid, KADoc_action, IssueTypeP_action, Tower_action, fromDate, toDate };

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("[KEDB_ResolutionFormat_Drilldown]", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new KEDBInstanceModel();
                    //coldet.Id = Convert.ToInt32(dr["ID"]);
                    if (!(dr["KEDBID"] is DBNull))
                        coldet.KEDBID = Convert.ToInt32(dr["KEDBID"]);
                    if (!(dr["KEDBNumber"] is DBNull))
                        coldet.KEDBNumber = dr["KEDBNumber"].ToString();
                    if (!(dr["ISSUETYPE"] is DBNull))
                        coldet.KedbIssueType = dr["ISSUETYPE"].ToString();
                    if (!(dr["Tower"] is DBNull))
                        coldet.Tower = dr["Tower"].ToString();
                    if (!(dr["SOLUTIONCATEGORY"] is DBNull))
                        coldet.SolutionCategory = (dr["SOLUTIONCATEGORY"].ToString());
                    if (!(dr["SolStatus"] is DBNull))
                        coldet.SolStatus = dr["SolStatus"].ToString();
                    if (!(dr["KEDBDescription"] is DBNull))
                        coldet.Description = dr["KEDBDescription"].ToString();
                    if (!(dr["KnowledgeArticle"] is DBNull))
                        coldet.KnowledgeArticle = dr["KnowledgeArticle"].ToString();
                    if (!(dr["ResolutionSteps"] is DBNull))
                        coldet.ResolutionSteps = dr["ResolutionSteps"].ToString();
                    if (!(dr["FileName"] is DBNull))
                        coldet.FileName = dr["FileName"].ToString();

                    kedbList.Add(coldet);
                }

            }

            return kedbList;

        }
        public static IEnumerable<KEDBInstanceModel> ViewKEDB(int clientid, int KEDBID)
        {
            IList<KEDBInstanceModel> kedbList = new List<KEDBInstanceModel>();

            DataTable dt = new DataTable();
            var paramName = new string[] { "ClientId", "KEDBID" };
            var paramValue = new object[] { clientid, KEDBID };

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("[ViewKEDB]", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new KEDBInstanceModel();
                    coldet.Id = Convert.ToInt32(dr["ID"]);
                    coldet.ClientId = Convert.ToInt32(dr["ClientID"]);
                    if (!(dr["IncidentID"] is DBNull))
                        coldet.IncidentId = Convert.ToInt32(dr["IncidentID"]);
                    if (!(dr["PointOfContact"] is DBNull))
                        coldet.PointOfContact = dr["PointOfContact"].ToString();
                    if (!(dr["KnowledgeArticle"] is DBNull))
                        coldet.KnowledgeArticle = dr["KnowledgeArticle"].ToString();
                    if (!(dr["Description"] is DBNull))
                        coldet.Description = dr["Description"].ToString();
                    //coldet.Description = dr["Description"].ToString();
                    if (!(dr["KEDBNumber"] is DBNull))
                        coldet.KEDBNumber = dr["KEDBNumber"].ToString();
                    //coldet.Priority = dr["Priority"].ToString();
                    if (!(dr["SolutionCategory"] is DBNull))
                        coldet.SolutionCategory = dr["SolutionCategory"].ToString();
                    if (!(dr["SolStatus"] is DBNull))
                        coldet.SolStatus = dr["SolStatus"].ToString();
                    if (!(dr["SolStatus"] is DBNull))
                        coldet.SolStatus = dr["SolStatus"].ToString();
                    if (!(dr["BusinessFunction"] is DBNull))
                        coldet.BusinessFunction = dr["BusinessFunction"].ToString();
                    if (!(dr["KedbIssueType"] is DBNull))
                        coldet.KedbIssueType = dr["KedbIssueType"].ToString();
                    if (!(dr["UpdatedBy"] is DBNull))
                        coldet.UpdatedBy = Convert.ToInt32(dr["UpdatedBy"]);
                    if (!(dr["UpdatedDate"] is DBNull))
                        coldet.UpdatedDate = Convert.ToDateTime(dr["UpdatedDate"]);
                    if (!(dr["ResolutionSteps"] is DBNull))
                        coldet.ResolutionSteps = dr["ResolutionSteps"].ToString();
                    if (!(dr["FileName"] is DBNull))
                        coldet.FileName = dr["FileName"].ToString();
                    kedbList.Add(coldet);
                }

            }

            return kedbList;

        }


        public static IEnumerable<KEDBInstanceModel> getKEDB(int clientid)
        {
            IList<KEDBInstanceModel> kedbList = new List<KEDBInstanceModel>();
            var paramName = new string[] { "ClientId", "Criteria" };
            var paramValue = new object[] { clientid, "" };

            DataTable dt = new DataTable();

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("[GetKedb]", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new KEDBInstanceModel();
                    coldet.Id = Convert.ToInt32(dr["ID"]);
                    if (!(dr["KEDBNumber"] is DBNull))
                        coldet.KEDBNumber = dr["KEDBNumber"].ToString();
                    if (!(dr["Description"] is DBNull))
                        coldet.Description = dr["Description"].ToString();
                    if (!(dr["ISSUETYPE"] is DBNull))
                        coldet.KedbIssueType = dr["ISSUETYPE"].ToString();
                    //coldet.Description = dr["Description"].ToString();
                    if (!(dr["Cause"] is DBNull))
                        coldet.Cause = dr["Cause"].ToString();
                    //coldet.Priority = dr["Priority"].ToString();
                    if (!(dr["Tower"] is DBNull))
                        coldet.Tower = dr["Tower"].ToString();
                    if (!(dr["Application"] is DBNull))
                        coldet.ConfigItemName = dr["Application"].ToString();
                    if (!(dr["SOLUTIONCATEGORY"] is DBNull))
                        coldet.SolutionCategory = (dr["SOLUTIONCATEGORY"].ToString());
                    if (!(dr["SolStatus"] is DBNull))
                        coldet.SolStatus = dr["SolStatus"].ToString();
                    if (!(dr["BusinessFunction"] is DBNull))
                        coldet.BusinessFunction = dr["BusinessFunction"].ToString();
                    if (!(dr["CIName"] is DBNull))
                        coldet.CIName = dr["CIName"].ToString();

                    kedbList.Add(coldet);
                }

            }

            return kedbList;

        }
        public static int GenerateKEDB(int ClientId)
        {
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var cns = new KEDBInstance();
                    cns.ClientId = ClientId;
                    cns.CreatedDate = DateTime.Now;
                    cns.UpdatedDate = DateTime.Now;
                    cns.IsDeleted = true;
                    unitOfWork.KEDBInstance.Insert(cns);
                    unitOfWork.Save();
                    id = cns.Id;
                    return id;
                }
            }
            catch (Exception ex)
            {
                Log.Error("IncidentNavigatorHelper error 'GenerateKEDB' ", ex);
            }
            return 0;
        }

        public static bool InsertKEDBIncidents(string kedbIds, int res, int uid)
        {
            if (string.IsNullOrEmpty(kedbIds))
            {
                return false;
            }
            string[] strINs = kedbIds.Split(new char[] { '-' });
            bool ret = false;
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                foreach (var strIN in strINs)
                {
                    int inci = Convert.ToInt32(strIN);
                    //var addedIncident = unitOfWork.KEDBIncident.Get(G => G.IncidentID == inci).FirstOrDefault();
                    //if (addedIncident != null)
                    //    continue;
                    var kedbincident = new KEDBIncident();
                    kedbincident.IncidentID = inci;
                    kedbincident.KEDBID = res;
                    kedbincident.CreatedDate = DateTime.Now;
                    kedbincident.UpdatedDate = DateTime.Now;
                    kedbincident.CreatedBy = uid;
                    kedbincident.UpdatedBy = uid;
                    kedbincident.IsDeleted = false;
                    kedbincident.isAutoIdentified = false;

                    unitOfWork.KEDBIncident.Insert(kedbincident);
                    unitOfWork.Save();
                }

                ret = true;
            }
            return ret;
        }

        public static bool DeleteKEDBNumber(int delId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                Db db = new Db();

                var n = unitOfWork.KEDBInstance.GetByID(delId);
                if (n is null || n.IsDeleted == false)
                {
                    return true;
                }
                else
                {
                    var deleteKEDBinc = from details in db.KEDBIncident
                                        where details.KEDBID == delId
                                        select details;

                    foreach (var detail in deleteKEDBinc)
                    {
                        unitOfWork.KEDBIncident.Delete(detail.Id);
                        unitOfWork.Save();
                    }
                    unitOfWork.KEDBInstance.Delete(delId);
                }

                unitOfWork.Save();
                return true;
            }
        }
        public static bool UpdateKEDBNumber(KEDBInstanceModel Km)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.KEDBInstance.GetByID(Km.Id);
                    updatetitem.UpdatedDate = DateTime.Now;
                    updatetitem.UpdatedBy = Km.UpdatedBy;
                    updatetitem.BusinessFunctionId = Km.BusinessFunctionId;
                    updatetitem.IssueTypeId = Km.IssueTypeId;
                    updatetitem.SolutionCategoryId = Km.SolutionCategoryId;
                    //setting SolutionStatus to Draft
                    updatetitem.SolutionStatusId = 1;
                    updatetitem.KnowledgeArticle = Km.KnowledgeArticle.Trim();
                    updatetitem.PointOfContact = Km.PointOfContact == " " ? null : Km.PointOfContact.Trim();
                    updatetitem.Description = Km.Description == " " ? null : Km.Description.Trim();
                    updatetitem.IsDeleted = false;
                    updatetitem.ResolutionSteps = Km.ResolutionSteps;
                    unitOfWork.KEDBInstance.Update(updatetitem);
                    unitOfWork.Save();
                    var incinsert = InsertKEDBIncidents(Km.Incid, Km.Id, (int)Km.UpdatedBy);
                    return true;
                }
            }

            catch (Exception ex)
            {
                Log.Error("IncidentNavigatorHelper error 'UpdateKEDBNumber' ", ex);
            }
            return false;
        }

        public static IEnumerable<KEDBInstanceModel> getKEDBTicket(int ClientId, int kedbid)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                Db db = new Db();
                var tempList = from KN in db.KEDBInstance
                               join KI in db.KEDBIncident on KN.Id equals KI.KEDBID
                               join i in db.Incidents on KI.IncidentID equals i.Id
                               where KN.ClientId == ClientId & KN.Id == kedbid & KI.IsDeleted != true
                               select new
                               {
                                   TicketId = i.IncidentId,
                                   Description = i.Description,
                                   isAutoIdentified = KI.isAutoIdentified
                               };
                var q = new List<KEDBInstanceModel>();
                foreach (var item in tempList)
                {
                    var kedb = new KEDBInstanceModel();
                    kedb.TicketId = item.TicketId;
                    kedb.Description = item.Description;
                    kedb.isAutoIdentifiedVal = item.isAutoIdentified.ToString();
                    q.Add(kedb);
                }
                return q;
            }
        }

        public static IEnumerable<KEDBInstanceModel> getKEDBTicketView(int ClientId, int kedbid)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                Db db = new Db();
                var tempList = from KN in db.KEDBInstance
                               join KI in db.KEDBIncident on KN.Id equals KI.KEDBID
                               where KN.ClientId == ClientId & KN.Id == kedbid
                               select new
                               {
                                   TicketId = KI.IncidentID,
                                   Description = KN.Description
                               };
                var q = tempList.Select(v => new KEDBInstanceModel
                {
                    Id = v.TicketId,
                    Description = v.Description
                }).ToList();


                return q;
            }
        }

        public static bool ADDKEDBIncidents(KEDBIncidentModel kedbREF, List<int> nums)
        {
            //if (string.IsNullOrEmpty(IDs))
            //{
            //    return false;
            //}

            if (nums.Count == 0)
            {
                return false;
            }

            bool ret = false;
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                foreach (var i in nums)
                {
                    //int inci = Convert.ToInt32(strIN);
                    //var addedIncident = unitOfWork.KEDBIncident.Get(G => G.IncidentID == inci).FirstOrDefault();
                    //if (addedIncident != null)
                    //    continue;
                    int inci = Convert.ToInt32(i);
                    var kedbincident = new KEDBIncident();
                    kedbincident.IncidentID = inci;
                    kedbincident.KEDBID = kedbREF.KEDBID;
                    kedbincident.CreatedDate = DateTime.Now;
                    kedbincident.UpdatedDate = DateTime.Now;
                    kedbincident.CreatedBy = kedbREF.CreatedBy;
                    kedbincident.UpdatedBy = kedbREF.UpdatedBy;
                    kedbincident.IsDeleted = false;
                    unitOfWork.KEDBIncident.Insert(kedbincident);
                    unitOfWork.Save();
                }

                ret = true;
            }
            return ret;
        }


        public static int checkKEDBIncident(int inID)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                Db db = new Db();
                var kedbIncident = (from a in db.KEDBIncident
                                    where a.IncidentID == inID
                                    & a.IsDeleted == false
                                    select a).FirstOrDefault();
                if (kedbIncident != null)
                {
                    return kedbIncident.IncidentID;
                }
            }
            return 0;
        }
        public static KEDBInstanceModel EditKEDBInsDetails(int KedbId, int cid)
        {
            Db db = new Db();
            KEDBInstanceModel kedbDetails = (from a in db.KEDBInstance
                                             join d in db.KEDBSolutionCategory.DefaultIfEmpty() on a.SolutionCategoryId equals d.Id into ps
                                             from d in ps.DefaultIfEmpty()
                                             join e in db.KEDBIssueType.DefaultIfEmpty() on a.IssueTypeId equals e.Id into ps1
                                             from e in ps1.DefaultIfEmpty()
                                             join f in db.KEDBSolutionStatus.DefaultIfEmpty() on a.SolutionStatusId equals f.Id into ps2
                                             from f in ps2.DefaultIfEmpty()
                                             join g in db.KEDBBusinessFunction.DefaultIfEmpty() on a.BusinessFunctionId equals g.Id into ps3
                                             from g in ps3.DefaultIfEmpty()
                                             where a.ClientId == cid && a.Id == KedbId
                                             select new KEDBInstanceModel
                                             {
                                                 Id = a.Id,
                                                 ClientId = a.ClientId,
                                                 PointOfContact = a.PointOfContact,
                                                 KnowledgeArticle = a.KnowledgeArticle,
                                                 Description = a.Description,
                                                 KEDBNumber = a.KEDBNumber,
                                                 SolutionCategoryId = d.Id,
                                                 SolutionStatusId = f.Id,
                                                 SolStatus = f.Status,
                                                 BusinessFunctionId = g.Id,
                                                 IssueTypeId = e.Id,
                                                 UpdatedDate = a.UpdatedDate,
                                                 UpdatedBy = a.UpdatedBy,
                                                 ResolutionSteps = a.ResolutionSteps,
                                                 FileName = a.FileName
                                             }
                                            ).FirstOrDefault();
            kedbDetails.UpdatedUser = (from b in db.UserProfiles
                                       where b.Id == kedbDetails.UpdatedBy
                                       select b.Email).FirstOrDefault();
            kedbDetails.UpdatedDateTime = kedbDetails.UpdatedDate.ToString("MM/dd/yyyy");

            return kedbDetails;
        }

        public static bool UpdateKEDBDetails(KEDBInstanceModel Km, int uid, int cid)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updateitem = unitOfWork.KEDBInstance.GetByID(Km.Id);
                    var prevStatus = updateitem.SolutionStatusId;
                    updateitem.UpdatedDate = DateTime.Now;
                    updateitem.UpdatedBy = Km.UpdatedBy;
                    updateitem.BusinessFunctionId = Km.BusinessFunctionId;
                    updateitem.IssueTypeId = Km.IssueTypeId;
                    updateitem.SolutionCategoryId = Km.SolutionCategoryId;
                    updateitem.SolutionStatusId = Km.SolutionStatusId;
                    updateitem.KnowledgeArticle = Km.KnowledgeArticle.Trim();
                    updateitem.PointOfContact = Km.PointOfContact.Trim();
                    updateitem.Description = Km.Description.Trim();
                    updateitem.ResolutionSteps = Km.ResolutionSteps.Trim();
                    if (Km.SolStatus.ToUpper() == "RETIRED")
                    {
                        updateitem.IsDeleted = true;
                        var updateincident = unitOfWork.KEDBIncident.Get(x => x.KEDBID == Km.Id);
                        foreach (var item in updateincident)
                        {
                            item.IsDeleted = true;
                            item.UpdatedBy = uid;
                            item.UpdatedDate = DateTime.Now;
                            unitOfWork.KEDBIncident.Update(item);
                        }
                    }
                    unitOfWork.KEDBInstance.Update(updateitem);
                    if (prevStatus != Km.SolutionStatusId)
                    {
                        var dnkedb = new KEDBStatusAudit();

                        dnkedb.KEDBID = Km.Id;
                        dnkedb.StatusId = Km.SolutionStatusId.Value;
                        dnkedb.PreviousStatusId = prevStatus;
                        dnkedb.CreatedBy = uid;
                        dnkedb.CreatedDate = DateTime.Now;
                        unitOfWork.KEDBStatusAudit.Insert(dnkedb);
                    }
                    unitOfWork.Save();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("IncidentNavigatorHelper error 'UpdateKEDBDetails' ", ex);
            }

            return false;
        }
        public static bool DelinkKEDBDetails(int kid, int tid, int uid, int cid)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                Db db = new Db();
                KEDBIncident kedbinc = (from a in db.KEDBIncident
                                        where a.IncidentID == tid && a.KEDBID == kid
                                        select a).FirstOrDefault();

                if (kedbinc != null)
                {
                    var updateitem = unitOfWork.KEDBIncident.GetByID(kedbinc.Id);
                    updateitem.UpdatedDate = DateTime.Now;
                    updateitem.UpdatedBy = uid;
                    updateitem.IsDeleted = true;
                    unitOfWork.KEDBIncident.Update(updateitem);
                    unitOfWork.Save();
                }
                return true;
            }

        }
        public static IList<KEDBIssueTypeModel> GetIssueTypeDetails(int ClientId, int uid)
        {
            IList<KEDBIssueTypeModel> IssueType = new List<KEDBIssueTypeModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<KEDBIssueTypes> IssueTypeDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (ClientId != 0)
                    IssueTypeDetails = unitOfWork.KEDBIssueTypes.Get(cid => cid.ClientId == ClientId).ToList();

                if (IssueTypeDetails != null)
                {
                    foreach (var _issuetypedetail in IssueTypeDetails)
                    {
                        KEDBIssueTypeModel cs = new KEDBIssueTypeModel();
                        cs.Id = _issuetypedetail.Id;
                        cs.ClientId = _issuetypedetail.ClientId;
                        cs.IssueType = _issuetypedetail.KEDBIssueType;

                        IssueType.Add(cs);
                    }
                }
                return IssueType;
            }

        }
        public static int CreateIssueType(KEDBIssueTypeModel issueTypeModel, int Id, int uid)
        {
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var issuetype = new KEDBIssueTypes();
                    issuetype.ClientId = issueTypeModel.ClientId;
                    issuetype.KEDBIssueType = issueTypeModel.IssueType;
                    issuetype.CreatedOn = DateTime.Now;
                    issuetype.CreatedBy = uid;
                    unitOfWork.KEDBIssueTypes.Insert(issuetype);
                    unitOfWork.Save();
                    id = issuetype.Id;
                    return id;
                }
            }
            catch (Exception ex)
            {
                Log.Error("IncidentNavigatorHelper error 'CreateIssueType' ", ex);
            }
            return 0;
        }

        public static bool UpdateIssueType(KEDBIssueTypeModel issueTypeModel, int uid)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.KEDBIssueTypes.GetByID(issueTypeModel.Id);
                    updatetitem.ClientId = issueTypeModel.ClientId;
                    updatetitem.KEDBIssueType = issueTypeModel.IssueType;
                    updatetitem.UpdatedBy = uid;
                    updatetitem.UpdatedOn = DateTime.Now;
                    unitOfWork.KEDBIssueTypes.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("IncidentNavigatorHelper error 'UpdateIssueType' ", ex);
            }
            return false;
        }

        public static bool RemoveIssueType(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.KEDBIssueTypes.Delete(Id);
                unitOfWork.Save();

                return true;
            }
        }

        public static IList<KEDBBusinessFunctionModel> GetBusFunctionDetails(int ClientId, int uid)
        {
            IList<KEDBBusinessFunctionModel> BusFunct = new List<KEDBBusinessFunctionModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<KEDBBusinessFunction> BusFunctDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (ClientId != 0)
                    BusFunctDetails = unitOfWork.KEDBBusinessFunction.Get(cid => cid.ClientId == ClientId).ToList();

                if (BusFunctDetails != null)
                {
                    foreach (var _busfunctdetail in BusFunctDetails)
                    {
                        KEDBBusinessFunctionModel cs = new KEDBBusinessFunctionModel();
                        cs.Id = _busfunctdetail.Id;
                        cs.ClientId = _busfunctdetail.ClientId;
                        cs.BusinessFunction = _busfunctdetail.BusinessFunction;

                        BusFunct.Add(cs);
                    }
                }
                return BusFunct;
            }

        }
        public static int CreateBusFunction(KEDBBusinessFunctionModel busFunctModel, int Id, int uid)
        {
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var busFunct = new KEDBBusinessFunction();
                    busFunct.ClientId = busFunctModel.ClientId;
                    busFunct.BusinessFunction = busFunctModel.BusinessFunction;
                    busFunct.CreatedOn = DateTime.Now;
                    busFunct.CreatedBy = uid;
                    unitOfWork.KEDBBusinessFunction.Insert(busFunct);
                    unitOfWork.Save();
                    id = busFunct.Id;
                    return id;
                }
            }
            catch (Exception ex)
            {
                Log.Error("IncidentNavigatorHelper error 'CreateBusFunction' ", ex);
            }
            return 0;
        }

        public static bool UpdateBusFunction(KEDBBusinessFunctionModel busSolModel, int uid)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.KEDBBusinessFunction.GetByID(busSolModel.Id);
                    updatetitem.ClientId = busSolModel.ClientId;
                    updatetitem.BusinessFunction = busSolModel.BusinessFunction;
                    updatetitem.UpdatedBy = uid;
                    updatetitem.UpdatedOn = DateTime.Now;
                    unitOfWork.KEDBBusinessFunction.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("IncidentNavigatorHelper error 'UpdateBusFunction' ", ex);
            }
            return false;
        }

        public static bool RemoveBusFunction(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.KEDBBusinessFunction.Delete(Id);
                unitOfWork.Save();

                return true;
            }
        }
        public static IList<KEDBSolutionCategoryModel> GetSolCateDetails(int ClientId, int uid)
        {
            IList<KEDBSolutionCategoryModel> SolCategory = new List<KEDBSolutionCategoryModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<KEDBSolutionCategory> SolCatDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (ClientId != 0)
                    SolCatDetails = unitOfWork.KEDBSolutionCategory.Get(cid => cid.ClientId == ClientId).ToList();

                if (SolCatDetails != null)
                {
                    foreach (var _solCatdetail in SolCatDetails)
                    {
                        KEDBSolutionCategoryModel cs = new KEDBSolutionCategoryModel();
                        cs.Id = _solCatdetail.Id;
                        cs.ClientId = _solCatdetail.ClientId;
                        cs.SolCategory = _solCatdetail.SolutionCategory;

                        SolCategory.Add(cs);
                    }
                }
                return SolCategory;
            }

        }
        public static int CreateSolCategory(KEDBSolutionCategoryModel solCateModel, int Id, int uid)
        {
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var solcat = new KEDBSolutionCategory();
                    solcat.ClientId = solCateModel.ClientId;
                    solcat.SolutionCategory = solCateModel.SolCategory;
                    solcat.CreatedOn = DateTime.Now;
                    solcat.CreatedBy = uid;
                    unitOfWork.KEDBSolutionCategory.Insert(solcat);
                    unitOfWork.Save();
                    id = solcat.Id;
                    return id;
                }
            }
            catch (Exception ex)
            {
                Log.Error("IncidentNavigatorHelper error 'CreateSolCategory' ", ex);
            }
            return 0;
        }

        public static bool UpdateSolCategory(KEDBSolutionCategoryModel solCatModel, int uid)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.KEDBSolutionCategory.GetByID(solCatModel.Id);
                    updatetitem.ClientId = solCatModel.ClientId;
                    updatetitem.SolutionCategory = solCatModel.SolCategory;
                    updatetitem.UpdatedBy = uid;
                    updatetitem.UpdatedOn = DateTime.Now;
                    unitOfWork.KEDBSolutionCategory.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("IncidentNavigatorHelper error 'UpdateSolCategory' ", ex);
            }
            return false;
        }

        public static bool RemoveSolCategory(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.KEDBSolutionCategory.Delete(Id);
                unitOfWork.Save();

                return true;
            }
        }
        public static bool AssignKEDBNumber(int sourceKedb, string Ids, int kedbID)
        {
            if (string.IsNullOrEmpty(Ids))
            {
                return false;
            }
            string[] strINs = Ids.Split(new char[] { ',' });
            bool ret = false;
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                foreach (var strIN in strINs)
                {
                    int inci = Convert.ToInt32(strIN);
                    //deleting of the existing record can be implemented
                    var addedIncident = unitOfWork.KEDBIncident.Get(G => G.IncidentID == inci && G.KEDBID == sourceKedb).FirstOrDefault();
                    addedIncident.KEDBID = kedbID;
                    unitOfWork.KEDBIncident.Update(addedIncident);
                    unitOfWork.Save();
                }
                ret = true;
            }
            return ret;
        }
        public static IEnumerable<KEDBInstanceModel> getKEDBTicketById(string Incids)
        {
            IList<KEDBInstanceModel> klist = new List<KEDBInstanceModel>();

            string[] strINs = Incids.Split(new char[] { '-' });
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                foreach (var strIN in strINs)
                {
                    int inci = Convert.ToInt32(strIN);
                    var addedIncident = unitOfWork.Incident.Get(G => G.Id == inci).FirstOrDefault();
                    var kedbincident = new KEDBInstanceModel();
                    kedbincident.Id = addedIncident.Id;
                    kedbincident.TicketId = addedIncident.IncidentId;
                    kedbincident.Description = addedIncident.Description;
                    klist.Add(kedbincident);
                }
            }
            return klist;
        }
        public static IEnumerable<KEDBInstanceModel> getAllKEDBDetail(int ClientId, int sourcekedbid)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                Db db = new Db();
                var tempList = from KN in db.KEDBInstance
                               join ss in db.KEDBSolutionStatus on KN.SolutionStatusId equals ss.Id
                               where KN.ClientId == ClientId & KN.Id != sourcekedbid & KN.IsDeleted == false & ss.Status.ToLower() != "retired"
                               select new KEDBInstanceModel
                               {
                                   Id = KN.Id,
                                   KEDBNumber = KN.KEDBNumber,
                                   Description = KN.Description
                               };

                return tempList;
            }
        }
        public static IEnumerable<KEDBAssIncModel> getKEDBwithIncident(int clientId, string Incids, int sourcekedbid)
        {
            IList<KEDBAssIncModel> klistmain = new List<KEDBAssIncModel>();
            var paramName = new string[] { "ClientId", "Incids", "sourcekedbid" };
            var paramValue = new object[] { clientId, Incids, sourcekedbid };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("GetKEDBSelectedIncident", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new KEDBAssIncModel();
                    coldet.IncId = Convert.ToInt32(dr["ID"]);
                    coldet.TicketId = dr["Incidentid"].ToString();
                    coldet.TicketDescription = dr["Description"].ToString();
                    coldet.isAutoIdentified = (bool)dr["isAutoIdentified"];
                    klistmain.Add(coldet);
                }
                if (klistmain.Count == 0)
                {
                    var kedbexist = unitOfWork.KEDBIncident.Get(G => G.KEDBID == sourcekedbid).Any();
                    if (kedbexist == false)
                    {
                        var addedIncident = unitOfWork.KEDBInstance.Get(G => G.Id == sourcekedbid).FirstOrDefault();
                        addedIncident.IsDeleted = true;
                        unitOfWork.KEDBInstance.Update(addedIncident);
                        unitOfWork.Save();
                    }

                }
                return klistmain;
            }


        }
        public static IEnumerable<KEDBInstanceModel> getKEDBSelTicket(int clientId, int sourcekedbid, string Incids)
        {
            IList<KEDBInstanceModel> klistmain = new List<KEDBInstanceModel>();
            var paramName = new string[] { "ClientId", "Incids", "sourcekedbid" };
            var paramValue = new object[] { clientId, Incids, sourcekedbid };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("GetKEDBSelectedIncident", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new KEDBInstanceModel();
                    coldet.TicketId = dr["Incidentid"].ToString();
                    coldet.Description = dr["Description"].ToString();
                    coldet.isAutoIdentifiedVal = dr["isAutoIdentified"].ToString();
                    klistmain.Add(coldet);
                }

                return klistmain;
            }


        }
        public static IEnumerable<KEDBInstanceModel> getSourceKEDBDetail(int clientId, int sourcekedbid)
        {
            IList<KEDBInstanceModel> klistmain = new List<KEDBInstanceModel>();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                //var sourcekedb = unitOfWork.KEDBInstance.GetByID(sourcekedbid);
                var sourcekedb = unitOfWork.KEDBInstance.Get().Where(f => f.Id == sourcekedbid && f.IsDeleted == false).ToList();
                var kedb = new KEDBInstanceModel();
                foreach (var item in sourcekedb)
                {
                    kedb.Id = item.Id;
                    kedb.KEDBNumber = item.KEDBNumber;
                    kedb.Description = item.Description;
                    klistmain.Add(kedb);
                }

            }
            return klistmain;
        }
        public static bool VerifyKEDB(int ClientID, string incids, int kedbid)
        {
            bool authorize = false;
            if (kedbid != 0)
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    authorize = unitOfWork.KEDBInstance.Get().Where(f => f.ClientId == ClientID && f.Id == kedbid).Any();
                    string[] strINs = incids.Split(new char[] { '-' });
                    foreach (var strIN in strINs)
                    {
                        int inci = Convert.ToInt32(strIN);
                        authorize = unitOfWork.Incident.Get(G => G.Id == inci && G.ClientId == ClientID).Any();
                    }
                }

                return authorize;
            }
            else
            {
                return false;
            }
        }
        public static bool SaveKEDBAutoAssigment(int KEDBID, int IncidentId, int uid, int ClientId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                Db db = new Db();
                KEDBIncident kedbinc = (from a in db.KEDBIncident
                                        where a.IncidentID == IncidentId && a.KEDBID == KEDBID
                                        select a).FirstOrDefault();

                if (kedbinc != null)
                {
                    var updateitem = unitOfWork.KEDBIncident.GetByID(kedbinc.Id);
                    updateitem.UpdatedDate = DateTime.Now;
                    updateitem.UpdatedBy = uid;
                    updateitem.isAutoIdentified = false;
                    unitOfWork.KEDBIncident.Update(updateitem);
                    unitOfWork.Save();
                }
                return true;
            }

        }
        public static bool SaveKEDBMultAutoAssigment(int KEDBID, string selectedIDs, int uid, int ClientId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                Db db = new Db();
                string[] strINs = selectedIDs.Split(new char[] { '-' });
                foreach (var strIN in strINs)
                {
                    int incid = Convert.ToInt32(strIN);
                    KEDBIncident kedbinc = (from a in db.KEDBIncident
                                            where a.IncidentID == incid && a.KEDBID == KEDBID
                                            select a).FirstOrDefault();

                    if (kedbinc != null)
                    {
                        var updateitem = unitOfWork.KEDBIncident.GetByID(kedbinc.Id);
                        updateitem.UpdatedDate = DateTime.Now;
                        updateitem.UpdatedBy = uid;
                        updateitem.isAutoIdentified = false;
                        unitOfWork.KEDBIncident.Update(updateitem);
                        unitOfWork.Save();
                    }
                }
                return true;
            }

        }

        public static IList<ConfigureAgingReportModel> GetConfigAgingQuery(int clientId)
        {

            IList<ConfigureAgingReportModel> configureAging = new List<ConfigureAgingReportModel>();
            using (var contextFactory = new DbContextFactory())
            {

                IEnumerable<ConfigAgingQuery> configureAgingDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                Db db = new Db();
                configureAgingDetails = (from a in db.ConfigAgingQuery
                                         where a.ClientID == clientId
                                         select a).ToList();


                if (configureAgingDetails != null)
                {
                    foreach (var _configAging in configureAgingDetails)
                    {
                        ConfigureAgingReportModel cans = new ConfigureAgingReportModel();
                        cans.ID = _configAging.ID;
                        //cans.ImprovementTypeId = _servImpType.ImprovementTypeId != null ? _servImpType.ImprovementTypeId.Value : 0;
                        cans.SeqNo = _configAging.SeqNo;
                        cans.Titlevalue = _configAging.Titlevalue;
                        cans.StartValue = _configAging.StartValue;
                        cans.EndValue = _configAging.EndValue;
                        cans.CreatedBy = _configAging.CreatedBy;
                        cans.UpdatedBy = _configAging.UpdatedBy;
                        cans.CreatedDt = _configAging.CreatedDt;
                        cans.UpdatedDt = _configAging.UpdatedDt;
                        cans.Datetype = _configAging.Datetype;
                        cans.ClientID = clientId;
                        cans.Count = configureAgingDetails.Count();
                        configureAging.Add(cans);

                    }
                }
                return configureAging;
            }

        }



        public static int AddNewConfigReportQuery(ConfigureAgingReportModel configAgingReport, int uid)
        {
            int id = 0;
            try
            {
                Db db = new Db();
                var seqNo = (from a in db.ConfigAgingQuery
                             orderby a.SeqNo descending
                             where a.ClientID == configAgingReport.ClientID
                             select a.SeqNo).FirstOrDefault();


                var countOfRecords = (from a in db.ConfigAgingQuery
                                      where a.ClientID == configAgingReport.ClientID
                                      select a).Count();

                if (countOfRecords != 15)
                {

                    if (seqNo == 0 || seqNo == null)
                    {
                        seqNo = 1;
                    }
                    else
                    {
                        seqNo = seqNo + 1;
                    }
                    if (Convert.ToInt32(configAgingReport.StartValue) >= 365)
                    {
                        configAgingReport.Titlevalue = ">" + configAgingReport.EndValue.ToString() + " Days";
                    }
                    if (Convert.ToInt32(configAgingReport.StartValue) < 365)
                    {
                        configAgingReport.Titlevalue = configAgingReport.EndValue.ToString() + " - " + configAgingReport.StartValue.ToString() + " Days";
                    }


                    using (var contextFactory = new DbContextFactory())
                    {
                        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                        var configItem = new ConfigAgingQuery();
                        configItem.ClientID = configAgingReport.ClientID;
                        configItem.Datetype = configAgingReport.Datetype;
                        configItem.StartValue = configAgingReport.StartValue;
                        configItem.EndValue = configAgingReport.EndValue;
                        configItem.SeqNo = seqNo;
                        configItem.Titlevalue = configAgingReport.Titlevalue;
                        configItem.CreatedBy = uid;
                        configItem.UpdatedBy = uid;
                        configItem.CreatedDt = DateTime.Now;
                        configItem.UpdatedDt = DateTime.Now;
                        unitOfWork.ConfigAgingQuery.Insert(configItem);
                        unitOfWork.Save();
                        id = configItem.ID;
                        return id;
                    }
                }
                else
                {
                    return 0;
                }
            }

            catch (Exception ex)
            {
                Log.Error("IncidentNavigatorHelper error 'AddNewConfigReportQuery' ", ex);
            }
            return 0;
        }




        public static bool UpdateConfigReportQuery(ConfigureAgingReportModel configReport, int uid)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.ConfigAgingQuery.GetByID(configReport.ID);
                    updatetitem.ClientID = configReport.ClientID;
                    updatetitem.StartValue = configReport.StartValue;
                    updatetitem.EndValue = configReport.EndValue;
                    updatetitem.Datetype = configReport.Datetype;
                    updatetitem.UpdatedBy = uid;
                    updatetitem.UpdatedDt = DateTime.Now;
                    if (Convert.ToInt32(configReport.StartValue) >= 365)
                    {
                        updatetitem.Titlevalue = ">" + configReport.EndValue.ToString() + " Days"; ;
                    }
                    if (Convert.ToInt32(configReport.StartValue) < 365)
                    {
                        updatetitem.Titlevalue = configReport.EndValue.ToString() + " - " + configReport.StartValue.ToString() + " Days";
                    }
                    unitOfWork.ConfigAgingQuery.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("IncidentNavigatorHelper error 'UpdateConfigReportQuery' ", ex);
            }
            return false;
        }


        public static bool RemoveConfigReportQuery(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.ConfigAgingQuery.Delete(Id);
                unitOfWork.Save();

                return true;
            }
        }

        public static bool deleteConfigAgingIN(int sequenceNo, int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                try
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    Db db = new Db();
                    //var n = unitOfWork.ConfigAgingQuery.GetByID(Id);
                    unitOfWork.ConfigAgingQuery.Delete(Id);
                    unitOfWork.Save();

                    return true;
                }

                catch (Exception ex)
                {
                    Log.Error("IncidentNavigatorHelper error 'deleteConfigAgingIN' ", ex);
                }
                return false;
            }
        }

        public static IEnumerable<string> getEmailUserPersonnel(int ClientId)
        {
            IEnumerable<string> userEmailList = new List<string>();
            List<string> userValues = new List<string>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("getEmailUserPersonnel", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var value = dr["Email"].ToString();
                    userValues.Add(value);
                }

                userEmailList = userValues;

                return userEmailList;
            }


        }



        public static IEnumerable<MetricSpecificationsModel> getSlaList(int clientid,int Id, string metricName)
        {
            IList<MetricSpecificationsModel> SlaList = new List<MetricSpecificationsModel>();



            DataTable dt = new DataTable();
            var paramName = new string[] { "ClientId","Id", "metricName" };
            var paramValue = new object[] { clientid,Id, metricName };

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("[GetSLaMetricDetails]", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    
                    var coldet = new MetricSpecificationsModel();

                    coldet.Id = Convert.ToInt32(dr["Id"]);
                    if (!(dr["AccountID"] is DBNull))
                        coldet.AccountId = Convert.ToInt32(dr["AccountID"]);
                    if (!(dr["ContractID"] is DBNull))
                        coldet.ContractId = Convert.ToInt32(dr["ContractID"]);
                    if (!(dr["AccountName"] is DBNull))
                        coldet.AccountName = dr["AccountName"].ToString();
                    if (!(dr["ContractName"] is DBNull))
                        coldet.ContractName = dr["ContractName"].ToString();
                    if (!(dr["Priority"] is DBNull))
                        coldet.Priority = Convert.ToInt32(dr["Priority"]);
                    if (!(dr["ClientPriority"] is DBNull))
                        coldet.ClientPriority = dr["ClientPriority"].ToString();
                    if (!(dr["MetricName"] is DBNull))
                        coldet.MetricName = dr["MetricName"].ToString();
                    if (!(dr["MetricUnit"] is DBNull))
                        coldet.MetricUnit = dr["MetricUnit"].ToString();
                    if (!(dr["MetricID"] is DBNull))
                        coldet.MetricId = Convert.ToInt32(dr["MetricID"]);
                    //coldet.Description = dr["Description"].ToString();
                    if (!(dr["MetricStatus"] is DBNull))
                        coldet.MetricStatus = dr["MetricStatus"].ToString();
                    //coldet.Priority = dr["Priority"].ToString();
                    if (!(dr["HasSLA"] is DBNull))
                        coldet.HasSLA = Convert.ToBoolean(dr["HasSLA"]);
                    if (!(dr["MetricStartDate"] is DBNull))
                        coldet.StartDate = Convert.ToDateTime(dr["MetricStartDate"]);
                    if (!(dr["MetricEnddate"] is DBNull))
                        coldet.EndDate = Convert.ToDateTime(dr["MetricEnddate"]);
                    if (!(dr["MetricBaseline"] is DBNull))
                        coldet.MetricBaseline = Convert.ToDouble(dr["MetricBaseline"]);
                    if (!(dr["MetricTarget"] is DBNull))
                        coldet.MetricTarget = Convert.ToDouble(dr["MetricTarget"]);
                    if (!(dr["MetricOwner"] is DBNull))
                        coldet.MetricOwner = dr["MetricOwner"].ToString();
                    if (!(dr["ClockSetup"] is DBNull))
                        coldet.ClockType = dr["ClockSetup"].ToString();
                    if (!(dr["SLAComplianceUnits"] is DBNull))
                        coldet.ComplianceUnits = dr["SLAComplianceUnits"].ToString();
                    if (!(dr["SLAComplianceBaseline"] is DBNull))
                        coldet.ComplianceBaseline = dr["SLAComplianceBaseline"].ToString();
                    if (!(dr["SLAComplianceTarget"] is DBNull))
                        coldet.ComplianceTarget = dr["SLAComplianceTarget"].ToString();
                    if (!(dr["ExceedsslaExceptations"] is DBNull))
                        coldet.ExceedsExpectation = dr["ExceedsslaExceptations"].ToString();
                    if (!(dr["MetExpectationhigherlimit"] is DBNull))
                        coldet.MetExpectationhigherlimit = dr["MetExpectationhigherlimit"].ToString();
                    if (!(dr["MetExpectationlowerlimit"] is DBNull))
                        coldet.MetExpectationlowerlimit = dr["MetExpectationlowerlimit"].ToString();
                    if (!(dr["BelowExpectation"] is DBNull))
                        coldet.BelowExpectation = dr["BelowExpectation"].ToString();
                    if (!(dr["Weightage"] is DBNull))
                        coldet.Weightage = Convert.ToInt32(dr["Weightage"]);
                    if (coldet.MetExpectationlowerlimit != null && coldet.MetExpectationhigherlimit != null)
                    {
                        coldet.MetSLAExpectationsBTW = coldet.MetExpectationlowerlimit + " and " + coldet.MetExpectationhigherlimit;
                    }
                    else
                    {
                        coldet.MetSLAExpectationsBTW = "";
                    }
                    SlaList.Add(coldet);
                }

            }

            return SlaList;

        }





        public static bool SaveSLASetup(MetricSpecificationsModel dt)
        {
            int Id = 0;
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                var mod = new MetricSpecification();
                mod.AccountId = dt.AccountId;
                mod.ClientId = dt.ClientId;
                mod.ContractId = dt.ContractId;
                mod.MetricId = dt.MetricId;
                mod.Priority = dt.Priority;
                mod.ClientPriority = dt.ClientPriority;
                mod.Weight = dt.Weightage;
                mod.MetricStatusId = dt.MetricStatusId;
                mod.StartDate = dt.StartDate;
                mod.EndDate = dt.EndDate;
                mod.MetricBaseline = dt.MetricBaseline;
                mod.MetricTarget = dt.MetricTarget;
                mod.MetricOwner = dt.MetricOwner;
                mod.ComplianceUnits = dt.ComplianceUnits;
                mod.ComplianceBaseline = dt.ComplianceBaseline;
                mod.ComplianceTarget = dt.ComplianceTarget;
                mod.ExceedsExpectation = dt.ExceedsExpectation;
                mod.MetExpectationlowerlimit = dt.MetExpectationlowerlimit;
                mod.MetExpectationhigherlimit = dt.MetExpectationhigherlimit;
                mod.BelowExpectation = dt.BelowExpectation;
                mod.HasSLA = dt.HasSLA;
                mod.ComplianceTarget = dt.ComplianceTarget;
                mod.ComplianceBaseline = dt.ComplianceBaseline;
                mod.ComplianceUnits = dt.ComplianceUnits;
                mod.CalculationData = dt.CalculationData;
                mod.Clocksetup = dt.Clocksetup;
                if (dt.booleanNewOrEdit=="Edit")
                {
                    mod.Id = dt.Id;
                    unitOfWork.MetricSpecification.Update(mod);
                }
                else
                {
                    unitOfWork.MetricSpecification.Insert(mod);
                }
                unitOfWork.Save();
                Id = mod.Id;
                return true;
            }



        }

        public static IEnumerable<MetricSpecificationsModel> ViewSLA(int clientid, int Id)
        {
            IList<MetricSpecificationsModel> SlaList = new List<MetricSpecificationsModel>();

            DataTable dt = new DataTable();
            var paramName = new string[] { "ClientId", "Id" };
            var paramValue = new object[] { clientid, Id };

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("[ViewSLA]", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new MetricSpecificationsModel();

                    coldet.Id = Convert.ToInt32(dr["Id"]);
                    if (!(dr["AccountName"] is DBNull))
                        coldet.AccountName = dr["AccountName"].ToString();
                    if (!(dr["ContractName"] is DBNull))
                        coldet.ContractName = dr["ContractName"].ToString();
                    if (!(dr["Priority"] is DBNull))
                        coldet.Priority = Convert.ToInt32(dr["Priority"]);
                    if (!(dr["MetricName"] is DBNull))
                        coldet.MetricName = dr["MetricName"].ToString();
                    if (!(dr["MetricUnit"] is DBNull))
                        coldet.MetricUnit = dr["MetricUnit"].ToString();
                    if (!(dr["MetricID"] is DBNull))
                        coldet.MetricId = Convert.ToInt32(dr["MetricID"]);
                    //coldet.Description = dr["Description"].ToString();
                    if (!(dr["MetricStatus"] is DBNull))
                        coldet.MetricStatus = dr["MetricStatus"].ToString();
                    //coldet.Priority = dr["Priority"].ToString();
                    if (!(dr["HasSLA"] is DBNull))
                        coldet.HasSLA = Convert.ToBoolean(dr["HasSLA"]);
                    if (!(dr["MetricStartDate"] is DBNull))
                        coldet.StartDate = Convert.ToDateTime(dr["MetricStartDate"]);
                    if (!(dr["MetricEnddate"] is DBNull))
                        coldet.EndDate = Convert.ToDateTime(dr["MetricEnddate"]);
                    if (!(dr["MetricBaseline"] is DBNull))
                        coldet.MetricBaseline = Convert.ToDouble(dr["MetricBaseline"]);
                    if (!(dr["MetricTarget"] is DBNull))
                        coldet.MetricTarget = Convert.ToDouble(dr["MetricTarget"]);
                    if (!(dr["MetricOwner"] is DBNull))
                        coldet.MetricOwner = dr["MetricOwner"].ToString();
                    if (!(dr["ClockSetup"] is DBNull))
                        coldet.ClockType = dr["ClockSetup"].ToString();
                    if (!(dr["SLAComplianceUnits"] is DBNull))
                        coldet.ComplianceUnits = dr["SLAComplianceUnits"].ToString();
                    if (!(dr["SLAComplianceBaseline"] is DBNull))
                        coldet.ComplianceBaseline = dr["SLAComplianceBaseline"].ToString();
                    if (!(dr["SLAComplianceTarget"] is DBNull))
                        coldet.ComplianceTarget = dr["SLAComplianceTarget"].ToString();
                    if (!(dr["ExceedsslaExceptations"] is DBNull))
                        coldet.ExceedsExpectation = dr["ExceedsslaExceptations"].ToString();
                    if (!(dr["MetExpectationhigherlimit"] is DBNull))
                        coldet.MetExpectationhigherlimit = dr["MetExpectationhigherlimit"].ToString();
                    if (!(dr["MetExpectationlowerlimit"] is DBNull))
                        coldet.MetExpectationlowerlimit = dr["MetExpectationlowerlimit"].ToString();
                    if (!(dr["BelowExpectation"] is DBNull))
                        coldet.BelowExpectation = dr["BelowExpectation"].ToString();
                    if (!(dr["Weightage"] is DBNull))
                        coldet.Weightage = Convert.ToInt32(dr["Weightage"]);
                    coldet.MetSLAExpectationsBTW = coldet.MetExpectationlowerlimit + " and " + coldet.MetExpectationhigherlimit;
                    SlaList.Add(coldet);
                }

            }

            return SlaList;

        }

    }
}

