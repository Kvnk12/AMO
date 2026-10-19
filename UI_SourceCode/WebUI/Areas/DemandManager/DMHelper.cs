using Data;
using Domain.Entities;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Areas.DemandManager.Dto;
using WebUI.Areas.DemandManager.Models;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using System.Web.Configuration;
using System.Net.Mail;
using WebUI.Areas.DemandManager.Controllers;
using System.Security.Policy;
using System.Data;
using System.Reflection;
using System.Net;
using WebUI.App_Start;
using WebUI.Dto;
using DocumentFormat.OpenXml.Wordprocessing;

namespace WebUI.Areas.DemandManager
{
    public class DMHelper
    {


        /// <summary>
        /// View All Demands
        /// </summary>
        /// <param name="ClientId"> Display All Demands</param>
        /// <param name="IsCusUser">Display Demands where InternalInitiative is Unchecked.</param>
        /// <returns>IEnumerable PreApprovalModel</returns>
        public static IEnumerable<DemandPreApprovalModel> PreApproval(int ClientId, bool IsCusUser)
        {
            Db db = new Db();
            //True- Display Demands where InternalInitiative is Unchecked.
            if (IsCusUser)
            {
                var Demandlist = (from im in db.DMPostDemand
                                join UP in db.UserProfiles on im.CreatedBy equals UP.Id
                                join S in db.IMStatus on im.StatusId equals S.Id into TempStatus
                                where im.ClientId == ClientId && (im.StatusId == (int)DMStatus.SubmitForRevision || im.StatusId == (int)DMStatus.Revised || im.StatusId == (int)DMStatus.Created)
                                && im.InternalInitiative == false && im.IsDeleted == false
                                from status in TempStatus.DefaultIfEmpty()
                                select new
                                {
                                    DemandId = im.Id,
                                    DemandNumber = im.DemandNumber,
                                    DemandName = im.ItemName,
                                    InternalInitiative = im.InternalInitiative,
                                    Status = status.Status,
                                    CreatedBy = UP.FirstName + " " + UP.LastName,
                                    CreatedDt = im.CreatedDate,
                                    StatusChange = im.StatusDt,
                                    LastModified = im.LastUpdatedDate
                                }).Distinct().ToList();
                var DemandListTomodel = Demandlist.Select(e => new DemandPreApprovalModel
                {
                    DemandId = e.DemandId,
                    DemandNumber = e.DemandNumber,
                    DemandName = e.DemandName,
                    InternalInitiative = e.InternalInitiative,
                    Status = e.Status,
                    CreatedBy = e.CreatedBy,
                    CreatedDt = e.CreatedDt,
                    StatusChangeDt = e.StatusChange,
                    LastModifiedDt = e.LastModified
                });

                return DemandListTomodel;
            }
            //Display All Demands
            else
            {
                var Demandlist = (from im in db.DMPostDemand
                                join UP in db.UserProfiles on im.CreatedBy equals UP.Id
                                join S in db.IMStatus on im.StatusId equals S.Id into TempStatus
                                where im.ClientId == ClientId && (im.StatusId == (int)DMStatus.SubmitForRevision || im.StatusId == (int)DMStatus.Revised || im.StatusId == (int)DMStatus.Created)
                                        && im.IsDeleted == false
                                from status in TempStatus.DefaultIfEmpty()
                                select new
                                {
                                    DemandId = im.Id,
                                    DemandNumber = im.DemandNumber,
                                    DemandName = im.ItemName,
                                    InternalInitiative = im.InternalInitiative,
                                    Status = status.Status,
                                    CreatedBy = UP.FirstName + " " + UP.LastName,
                                    CreatedDt = im.CreatedDate,
                                    StatusChange = im.StatusDt,
                                    LastModified = im.LastUpdatedDate
                                }).Distinct().ToList();
                var DemandListTomodel = Demandlist.Select(e => new DemandPreApprovalModel
                {
                    DemandId = e.DemandId,
                    DemandNumber = e.DemandNumber,
                    DemandName = e.DemandName,
                    InternalInitiative = e.InternalInitiative,
                    Status = e.Status,
                    CreatedBy = e.CreatedBy,
                    CreatedDt = e.CreatedDt,
                    StatusChangeDt = e.StatusChange,
                    LastModifiedDt = e.LastModified
                });

                return DemandListTomodel;
            }

        }

        /// <summary>
        /// View My Demands
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="UserId">Display Demands For Creator</param>
        /// <returns>IEnumerable PreApprovalModel </returns>
        public static IEnumerable<DemandPreApprovalModel> PreApproval(int ClientId, int UserId)
        {
            Db db = new Db();
            var Demandlist = (from im in db.DMPostDemand
                            join UP in db.UserProfiles on im.CreatedBy equals UP.Id
                            join S in db.IMStatus on im.StatusId equals S.Id into TempStatus
                            where im.ClientId == ClientId && im.CreatedBy == UserId && (im.StatusId == (int)DMStatus.SubmitForRevision || im.StatusId == (int)DMStatus.Revised || im.StatusId == (int)DMStatus.Created)
                                 && im.IsDeleted == false
                            from status in TempStatus.DefaultIfEmpty()
                            select new
                            {
                                DemandId = im.Id,
                                DemandNumber = im.DemandNumber,
                                DemandName = im.ItemName,
                                InternalInitiative = im.InternalInitiative,
                                Status = status.Status,
                                CreatedBy = UP.FirstName + " " + UP.LastName,
                                CreatedDt = im.CreatedDate,
                                StatusChange = im.StatusDt,
                                LastModified = im.LastUpdatedDate
                            }).ToList();
            var DemandListTomodel = Demandlist.Select(e => new DemandPreApprovalModel
            {
                DemandId = e.DemandId,
                DemandNumber = e.DemandNumber,
                DemandName = e.DemandName,
                InternalInitiative = e.InternalInitiative,
                Status = e.Status,
                CreatedBy = e.CreatedBy,
                CreatedDt = e.CreatedDt,
                StatusChangeDt = e.StatusChange,
                LastModifiedDt = e.LastModified
            });

            return DemandListTomodel;
        }


        /// <summary>
        /// View All SubmittedForReview Demands
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="IsCusUser">Display Demands where InternalInitiative is Unchecked.</param>
        /// <returns></returns>
        public static IEnumerable<SubmitForReviewModel> SubmitForReview(int ClientId, bool IsCusUser)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();
            //True- Display Demands where InternalInitiative is Unchecked.
            if (IsCusUser)
            {
                var Demandlist = (from im in db.DMPostDemand
                                join UP in db.UserProfiles on im.CreatedBy equals UP.Id
                                join S in db.IMStatus on im.StatusId equals S.Id into TempStatus
                                join lob in db.DMLOB on im.LOBId equals lob.Id into TempLob
                                where im.ClientId == ClientId && (im.StatusId == (int)DMStatus.SubmitForReview || im.StatusId == (int)DMStatus.SubmitForApproval)
                                    && im.InternalInitiative == false && im.IsDeleted == false
                                from status in TempStatus.DefaultIfEmpty()
                                from Lob in TempLob.DefaultIfEmpty()
                                select new
                                {
                                    DemandId = im.Id,
                                    DemandName = im.ItemName,
                                    DemandNumber = im.DemandNumber,
                                    DemandSource = im.DemandSource,
                                    InternalInitiative = im.InternalInitiative,
                                    LOB = Lob.DemandLOB,
                                    Status = status.Status,
                                    value = im.QtyFication,
                                    CreatedBy = UP.FirstName + " " + UP.LastName,
                                    //Assigned Approvers -
                                    CreatedDt = im.CreatedDate,
                                    StatusChange = im.StatusDt,
                                    LastModified = im.LastUpdatedDate
                                }).Distinct().ToList();
                var DemandListTomodel = Demandlist.Select(e => new SubmitForReviewModel
                {
                    DemandId = e.DemandId,
                    DemandName = e.DemandName,
                    DemandNumber = e.DemandNumber,
                    DemandSource = e.DemandSource,
                    InternalInitiative = e.InternalInitiative,
                    LOB = e.LOB,
                    Status = e.Status,
                    Priority = "",
                    PrevPriority = "",
                    Value = e.value.ToString(),
                    CreatedBy = e.CreatedBy,
                    AssignedApprover = "",
                    CreatedDt = e.CreatedDt,
                    StatusChangeDt = e.StatusChange,
                    LastModifiedDt = e.LastModified
                }).ToList();

                //To get Priority and Approvers
                foreach (var i in DemandListTomodel)
                {
                    var approversList = unitOfWork.DMDemandApprover.Get().Where(f => f.DemandId == i.DemandId).ToList();
                    if (approversList != null)
                    {
                        if (approversList.Where(e => e.ApprovedDate != null).Any())
                            i.Status = "Pending for Approval";
                    }
                    var approvers = approversList.Select(f => f.ApproverId).ToList();
                    var priority = unitOfWork.DMPriorityHistory.Get().Where(f => f.DemandId == i.DemandId).OrderByDescending(r => r.Id).FirstOrDefault();
                    if (priority != null)
                    {
                        i.Priority = priority.OldPriorityId.ToString();
                        i.PrevPriority = priority.NewPriorityId.ToString();
                    }
                    foreach (var ap in approvers)
                    {
                        var name = unitOfWork.UserProfile.Get().Where(f => f.Id == ap).FirstOrDefault();
                        if (name != null)
                        {
                            i.AssignedApprover = i.AssignedApprover + name.FirstName + " " + name.LastName + ", ";
                        }
                    }
                }
                return DemandListTomodel;
            }
            else
            {
                var Demandlist = (from im in db.DMPostDemand
                                  join UP in db.UserProfiles on im.CreatedBy equals UP.Id
                                join S in db.IMStatus on im.StatusId equals S.Id into TempStatus
                                join lob in db.DMLOB on im.LOBId equals lob.Id into TempLob
                                where im.ClientId == ClientId && (im.StatusId == (int)DMStatus.SubmitForReview || im.StatusId == (int)DMStatus.SubmitForApproval)
                                     && im.IsDeleted == false
                                from status in TempStatus.DefaultIfEmpty()
                                from Lob in TempLob.DefaultIfEmpty()
                                select new
                                {
                                    DemandId = im.Id,
                                    DemandName = im.ItemName,
                                    DemandNumber = im.DemandNumber,
                                    DemandSource = im.DemandSource,
                                    InternalInitiative = im.InternalInitiative,
                                    LOB = Lob.DemandLOB,
                                    Status = status.Status,
                                    value = im.QtyFication,
                                    CreatedBy = UP.FirstName + " " + UP.LastName,
                                    //Assigned Approvers -
                                    CreatedDt = im.CreatedDate,
                                    StatusChange = im.StatusDt,
                                    LastModified = im.LastUpdatedDate
                                }).Distinct().ToList();
                var DemandListTomodel = Demandlist.Select(e => new SubmitForReviewModel
                {
                    DemandId = e.DemandId,
                    DemandName = e.DemandName,
                    DemandNumber = e.DemandNumber,
                    DemandSource = e.DemandSource,
                    InternalInitiative = e.InternalInitiative,
                    LOB = e.LOB,
                    Status = e.Status,
                    Priority = "",
                    PrevPriority = "",
                    Value = e.value.ToString(),
                    CreatedBy = e.CreatedBy,
                    AssignedApprover = "",
                    CreatedDt = e.CreatedDt,
                    StatusChangeDt = e.StatusChange,
                    LastModifiedDt = e.LastModified
                }).ToList();

                //To get Priority and Approvers
                foreach (var i in DemandListTomodel)
                {
                    var approversList = unitOfWork.DMDemandApprover.Get().Where(f => f.DemandId == i.DemandId).ToList();
                    if (approversList != null)
                    {
                        if (approversList.Where(e => e.ApprovedDate != null).Any())
                            i.Status = "Pending for Approval";
                    }
                    var approvers = approversList.Select(f => f.ApproverId).ToList();
                    var priority = unitOfWork.DMPriorityHistory.Get().Where(f => f.DemandId == i.DemandId).OrderByDescending(r => r.Id).FirstOrDefault();
                    if (priority != null)
                    {
                        i.Priority = priority.OldPriorityId.ToString();
                        i.PrevPriority = priority.NewPriorityId.ToString();
                    }
                    foreach (var ap in approvers)
                    {
                        var name = unitOfWork.UserProfile.Get().Where(f => f.Id == ap).FirstOrDefault();
                        if (name != null)
                        {
                            i.AssignedApprover = i.AssignedApprover + name.FirstName + " " + name.LastName + ", ";
                        }
                    }
                }
                return DemandListTomodel;
            }

        }


        /// <summary>
        /// View my Demands
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="UserId"></param>
        /// <param name="IsAdmin"> If YES : DisplayDemands for Approver, Else : Display Demands for Creator. </param>
        /// <returns></returns>
        public static IEnumerable<SubmitForReviewModel> SubmitForReview(int ClientId, int UserId, bool IsAdmin)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();
            //If Loggedin User is Approver [True]
            if (IsAdmin)
            {
                var Demandlist = (from im in db.DMPostDemand
                                join UP in db.UserProfiles on im.CreatedBy equals UP.Id
                                  join IMApp in db.DMDemandApprover on im.Id equals IMApp.DemandId
                                join S in db.IMStatus on im.StatusId equals S.Id into TempStatus
                                join lob in db.DMLOB on im.LOBId equals lob.Id into TempLob
                                where im.ClientId == ClientId && (IMApp.ApproverId == UserId || im.CreatedBy == UserId) && (im.StatusId == (int)DMStatus.SubmitForReview || im.StatusId == (int)DMStatus.SubmitForApproval)
                                     && im.IsDeleted == false
                                from status in TempStatus.DefaultIfEmpty()
                                from Lob in TempLob.DefaultIfEmpty()
                                select new
                                {
                                    DemandId = im.Id,
                                    DemandName = im.ItemName,
                                    DemandNumber = im.DemandNumber,
                                    DemandSource = im.DemandSource,
                                    InternalInitiative = im.InternalInitiative,
                                    LOB = Lob.DemandLOB,
                                    Status = status.Status,
                                    value = im.QtyFication,
                                    CreatedBy = UP.FirstName + " " + UP.LastName,
                                    //Assigned Approvers -
                                    CreatedDt = im.CreatedDate,
                                    StatusChange = im.StatusDt,
                                    LastModified = im.LastUpdatedDate
                                }).Distinct().ToList();
                var DemandListTomodel = Demandlist.Select(e => new SubmitForReviewModel
                {
                    DemandId = e.DemandId,
                    DemandName = e.DemandName,
                    DemandNumber = e.DemandNumber,
                    DemandSource = e.DemandSource,
                    InternalInitiative = e.InternalInitiative,
                    LOB = e.LOB,
                    Status = e.Status,
                    Priority = "",
                    PrevPriority = "",
                    Value = e.value.ToString(),
                    CreatedBy = e.CreatedBy,
                    AssignedApprover = "",
                    CreatedDt = e.CreatedDt,
                    StatusChangeDt = e.StatusChange,
                    LastModifiedDt = e.LastModified
                }).ToList();

                //To get Priority and Approvers
                foreach (var i in DemandListTomodel)
                {
                    var approversList = unitOfWork.DMDemandApprover.Get().Where(f => f.DemandId == i.DemandId).ToList();
                    if (approversList != null)
                    {
                        if (approversList.Where(e => e.ApprovedDate != null).Any())
                            i.Status = "Pending for Approval";
                    }
                    var approvers = approversList.Select(f => f.ApproverId).ToList();
                    var priority = unitOfWork.DMPriorityHistory.Get().Where(f => f.DemandId == i.DemandId).OrderByDescending(r => r.Id).FirstOrDefault();
                    if (priority != null)
                    {
                        i.Priority = priority.OldPriorityId.ToString();
                        i.PrevPriority = priority.NewPriorityId.ToString();
                    }
                    foreach (var ap in approvers)
                    {
                        var name = unitOfWork.UserProfile.Get().Where(f => f.Id == ap).FirstOrDefault();
                        if (name != null)
                        {
                            i.AssignedApprover = i.AssignedApprover + name.FirstName + " " + name.LastName + ", ";
                        }
                    }
                }
                return DemandListTomodel;
            }
            //No- Normal User  [False]
            else
            {
                var Demandlist = (from im in db.DMPostDemand
                                  join UP in db.UserProfiles on im.CreatedBy equals UP.Id
                                join S in db.IMStatus on im.StatusId equals S.Id into TempStatus
                                join lob in db.DMLOB on im.LOBId equals lob.Id into TempLob
                                where im.ClientId == ClientId && im.CreatedBy == UserId && (im.StatusId == (int)DMStatus.SubmitForReview || im.StatusId == (int)DMStatus.SubmitForApproval)
                                     && im.IsDeleted == false
                                from status in TempStatus.DefaultIfEmpty()
                                from Lob in TempLob.DefaultIfEmpty()
                                select new
                                {
                                    DemandId = im.Id,
                                    DemandName = im.ItemName,
                                    DemandNumber = im.DemandNumber,
                                    DemandSource = im.DemandSource,
                                    InternalInitiative = im.InternalInitiative,
                                    LOB = Lob.DemandLOB,
                                    Status = status.Status,
                                    value = im.QtyFication,
                                    CreatedBy = UP.FirstName + " " + UP.LastName,
                                    //Assigned Approvers -
                                    CreatedDt = im.CreatedDate,
                                    StatusChange = im.StatusDt,
                                    LastModified = im.LastUpdatedDate
                                }).Distinct().ToList();
                var DemandListTomodel = Demandlist.Select(e => new SubmitForReviewModel
                {
                    DemandId = e.DemandId,
                    DemandName = e.DemandName,
                    DemandNumber = e.DemandNumber,
                    DemandSource = e.DemandSource,
                    InternalInitiative = e.InternalInitiative,
                    LOB = e.LOB,
                    Status = e.Status,
                    Priority = "",
                    PrevPriority = "",
                    Value = e.value.ToString(),
                    CreatedBy = e.CreatedBy,
                    AssignedApprover = "",
                    CreatedDt = e.CreatedDt,
                    StatusChangeDt = e.StatusChange,
                    LastModifiedDt = e.LastModified
                }).ToList();

                //To get Priority and Approvers
                foreach (var i in DemandListTomodel)
                {
                    var approversList = unitOfWork.DMDemandApprover.Get().Where(f => f.DemandId == i.DemandId).ToList();
                    if (approversList != null)
                    {
                        if (approversList.Where(e => e.ApprovedDate != null).Any())
                            i.Status = "Pending for Approval";
                    }
                    var approvers = approversList.Select(f => f.ApproverId).ToList();
                    var priority = unitOfWork.DMPriorityHistory.Get().Where(f => f.DemandId == i.DemandId).OrderByDescending(r => r.Id).FirstOrDefault();
                    if (priority != null)
                    {
                        i.Priority = priority.OldPriorityId == null ? "" : priority.OldPriorityId.ToString();
                        i.PrevPriority = priority.NewPriorityId == null ? "" : priority.NewPriorityId.ToString();
                    }
                    foreach (var ap in approvers)
                    {
                        var name = unitOfWork.UserProfile.Get().Where(f => f.Id == ap).FirstOrDefault();
                        if (name != null)
                        {
                            i.AssignedApprover = i.AssignedApprover + name.FirstName + " " + name.LastName + ", ";
                        }
                    }
                }
                return DemandListTomodel;
            }

        }





        /// <summary>
        /// View All Demands
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="IsCusUser">Display Demands where InternalInitiative is Unchecked.</param>
        /// <returns></returns>
        public static IEnumerable<DemandApprovedModel> Approval(int ClientId, bool IsCusUser)
        {
            Db db = new Db();
            //True- Display Demands where InternalInitiative is Unchecked.
            if (IsCusUser)
            {
                var Demandlist = (from im in db.DMPostDemand
                                  join UP in db.UserProfiles on im.CreatedBy equals UP.Id into TempCreatedBy
                                join UP2 in db.UserProfiles on im.ApprovedBy equals UP2.Id into TempApprovedBy
                                join UP3 in db.UserProfiles on im.ImplementationManager equals UP3.Id into TempImpManager
                                join lob in db.DMLOB on im.LOBId equals lob.Id into TempLob
                                join S in db.IMStatus on im.StatusId equals S.Id into TempStatus
                                join impS in db.IMStatus on im.ImpStatusId equals impS.Id into TempImpStatus
                                where im.ClientId == ClientId && ( (im.ImpStatusId==null && im.StatusId == (int)DMStatus.Hold) || im.ImpStatusId == (int)DMStatus.SubmitForPlanning || im.StatusId == (int)DMStatus.Approved || im.StatusId == (int)DMStatus.Declined || im.StatusId == (int)DMStatus.Cancelled)
                                    && im.InternalInitiative == false && im.IsDeleted == false
                                from status in TempStatus.DefaultIfEmpty()
                                from createdBy in TempCreatedBy.DefaultIfEmpty()
                                from approvedBy in TempApprovedBy.DefaultIfEmpty()
                                from impManager in TempImpManager.DefaultIfEmpty()
                                from Imps in TempImpStatus.DefaultIfEmpty()
                                from Lob in TempLob.DefaultIfEmpty()
                                select new
                                {
                                    DemandId = im.Id,
                                    DemandName = im.ItemName,
                                    DemandNumber = im.DemandNumber,
                                    DemandSource = im.DemandSource,
                                    InternalInitiative = im.InternalInitiative,
                                    LOB = Lob.DemandLOB,
                                    ImpStatus = Imps.Status,
                                    Status = status.Status,
                                    Priority = im.PriorityId,
                                    value = im.QtyFication,
                                    CreatedBy = createdBy.FirstName + " " + createdBy.LastName,
                                    ApprovedBy = approvedBy.FirstName + " " + approvedBy.LastName,
                                    ImpManager = impManager.FirstName + " " + impManager.LastName,
                                    CreatedDt = im.CreatedDate,
                                    StatusChange = im.StatusDt
                                }).Distinct().ToList();
                var DemandListTomodel = Demandlist.Select(e => new DemandApprovedModel
                {
                    DemandId = e.DemandId,
                    DemandName = e.DemandName,
                    DemandNumber = e.DemandNumber,
                    DemandSource = e.DemandSource,
                    ApproverBy = e.ApprovedBy,
                    CreatedBy = e.CreatedBy,
                    CreatedDt = e.CreatedDt,
                    ImplementationManager = e.ImpManager,
                    ImpStatus = e.ImpStatus,
                    InternalInitiative = e.InternalInitiative,
                    LOB = e.LOB,
                    Priority = e.Priority.ToString(),
                    Status = e.Status,
                    StatusChangeDt = e.StatusChange,
                    Value = e.value.ToString()

                });

                return DemandListTomodel;
            }
            else
            {
                var Demandlist = (from im in db.DMPostDemand
                                  join UP in db.UserProfiles on im.CreatedBy equals UP.Id into TempCreatedBy
                                join UP2 in db.UserProfiles on im.ApprovedBy equals UP2.Id into TempApprovedBy
                                join UP3 in db.UserProfiles on im.ImplementationManager equals UP3.Id into TempImpManager
                                join lob in db.DMLOB on im.LOBId equals lob.Id into TempLob
                                join S in db.IMStatus on im.StatusId equals S.Id into TempStatus
                                join impS in db.IMStatus on im.ImpStatusId equals impS.Id into TempImpStatus
                                where im.ClientId == ClientId && ((im.ImpStatusId == null && im.StatusId == (int)DMStatus.Hold) || im.ImpStatusId == (int)DMStatus.SubmitForPlanning || im.StatusId == (int)DMStatus.Approved || im.StatusId == (int)DMStatus.Declined || im.StatusId == (int)DMStatus.Cancelled)
                                     && im.IsDeleted == false
                                from status in TempStatus.DefaultIfEmpty()
                                from createdBy in TempCreatedBy.DefaultIfEmpty()
                                from approvedBy in TempApprovedBy.DefaultIfEmpty()
                                from impManager in TempImpManager.DefaultIfEmpty()
                                from Imps in TempImpStatus.DefaultIfEmpty()
                                from Lob in TempLob.DefaultIfEmpty()
                                select new
                                {
                                    DemandId = im.Id,
                                    DemandName = im.ItemName,
                                    DemandNumber = im.DemandNumber,
                                    DemandSource = im.DemandSource,
                                    InternalInitiative = im.InternalInitiative,
                                    LOB = Lob.DemandLOB,
                                    ImpStatus = Imps.Status,
                                    Status = status.Status,
                                    Priority = im.PriorityId,
                                    value = im.QtyFication,
                                    CreatedBy = createdBy.FirstName + " " + createdBy.LastName,
                                    ApprovedBy = approvedBy.FirstName + " " + approvedBy.LastName,
                                    ImpManager = impManager.FirstName + " " + impManager.LastName,
                                    CreatedDt = im.CreatedDate,
                                    StatusChange = im.StatusDt
                                }).Distinct().ToList();
                var DemandListTomodel = Demandlist.Select(e => new DemandApprovedModel
                {
                    DemandId = e.DemandId,
                    DemandName = e.DemandName,
                    DemandNumber = e.DemandNumber,
                    ApproverBy = e.ApprovedBy,
                    CreatedBy = e.CreatedBy,
                    CreatedDt = e.CreatedDt,
                    DemandSource = e.DemandSource,
                    ImplementationManager = e.ImpManager,
                    ImpStatus = e.ImpStatus,
                    InternalInitiative = e.InternalInitiative,
                    LOB = e.LOB,
                    Priority = e.Priority.ToString(),
                    Status = e.Status,
                    StatusChangeDt = e.StatusChange,
                    Value = e.value.ToString()

                });

                return DemandListTomodel;
            }

        }


        /// <summary>
        /// View My Demands
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="UserId">Created By</param>
        /// <param name="IsAdmin"> If YES : DisplayDemands for Approver, Else : Display Demands for Creator.</param>
        /// <returns></returns>
        public static IEnumerable<DemandApprovedModel> Approval(int ClientId, int UserId, bool IsAdmin)
        {
            Db db = new Db();
            //If Logged in user is ADMIN/SysAdmin
            if (IsAdmin)
            {
                var Demandlist = (from im in db.DMPostDemand
                                join UP in db.UserProfiles on im.CreatedBy equals UP.Id into TempCreatedBy
                                join UP2 in db.UserProfiles on im.ApprovedBy equals UP2.Id into TempApprovedBy
                                join UP3 in db.UserProfiles on im.ImplementationManager equals UP3.Id into TempImpManager
                                join iMApp in db.DMDemandApprover on im.ApprovedBy equals iMApp.ApproverId into TempDemandApprover
                                  join lob in db.DMLOB on im.LOBId equals lob.Id into TempLob
                                join S in db.IMStatus on im.StatusId equals S.Id into TempStatus
                                join impS in db.IMStatus on im.ImpStatusId equals impS.Id into TempImpStatus
                                where im.ClientId == ClientId && (im.ApprovedBy == UserId || im.CreatedBy == UserId) && ((im.ImpStatusId == null && im.StatusId == (int)DMStatus.Hold) || im.ImpStatusId == (int)DMStatus.SubmitForPlanning || im.StatusId == (int)DMStatus.Approved || im.StatusId == (int)DMStatus.Declined || im.StatusId == (int)DMStatus.Cancelled)
                                     && im.IsDeleted == false
                                from IMApp in TempDemandApprover.DefaultIfEmpty()
                                from status in TempStatus.DefaultIfEmpty()
                                from createdBy in TempCreatedBy.DefaultIfEmpty()
                                from approvedBy in TempApprovedBy.DefaultIfEmpty()
                                from impManager in TempImpManager.DefaultIfEmpty()
                                from Imps in TempImpStatus.DefaultIfEmpty()
                                from Lob in TempLob.DefaultIfEmpty()
                                select new
                                {
                                    DemandId = im.Id,
                                    DemandName = im.ItemName,
                                    DemandNumber = im.DemandNumber,
                                    DemandSource = im.DemandSource,
                                    InternalInitiative = im.InternalInitiative,
                                    LOB = Lob.DemandLOB,
                                    ImpStatus = Imps.Status,
                                    Status = status.Status,
                                    Priority = im.PriorityId,
                                    value = im.QtyFication,
                                    CreatedBy = createdBy.FirstName + " " + createdBy.LastName,
                                    ApprovedBy = approvedBy.FirstName + " " + approvedBy.LastName,
                                    ImpManager = impManager.FirstName + " " + impManager.LastName,
                                    CreatedDt = im.CreatedDate,
                                    StatusChange = im.StatusDt
                                }).Distinct().ToList();
                var DemandListTomodel = Demandlist.Select(e => new DemandApprovedModel
                {
                    DemandId = e.DemandId,
                    DemandName = e.DemandName,
                    DemandNumber = e.DemandNumber,
                    ApproverBy = e.ApprovedBy,
                    CreatedBy = e.CreatedBy,
                    CreatedDt = e.CreatedDt,
                    DemandSource = e.DemandSource,
                    ImplementationManager = e.ImpManager,
                    ImpStatus = e.ImpStatus,
                    InternalInitiative = e.InternalInitiative,
                    LOB = e.LOB,
                    Priority = e.Priority.ToString(),
                    Status = e.Status,
                    StatusChangeDt = e.StatusChange,
                    Value = e.value.ToString()

                });

                return DemandListTomodel;
            }
            //LoggedIn User is not Admin
            else
            {
                var Demandlist = (from im in db.DMPostDemand
                                  join UP in db.UserProfiles on im.CreatedBy equals UP.Id into TempCreatedBy
                                join UP2 in db.UserProfiles on im.ApprovedBy equals UP2.Id into TempApprovedBy
                                join UP3 in db.UserProfiles on im.ImplementationManager equals UP3.Id into TempImpManager
                                join lob in db.DMLOB on im.LOBId equals lob.Id into TempLob
                                join S in db.IMStatus on im.StatusId equals S.Id into TempStatus
                                join impS in db.IMStatus on im.ImpStatusId equals impS.Id into TempImpStatus
                                where im.ClientId == ClientId && im.CreatedBy == UserId && (im.StatusId == (int)DMStatus.Hold || im.ImpStatusId == (int)DMStatus.SubmitForPlanning || im.StatusId == (int)DMStatus.Approved || im.StatusId == (int)DMStatus.Declined || im.StatusId == (int)DMStatus.Cancelled)
                                     && im.IsDeleted == false
                                from status in TempStatus.DefaultIfEmpty()
                                from createdBy in TempCreatedBy.DefaultIfEmpty()
                                from approvedBy in TempApprovedBy.DefaultIfEmpty()
                                from impManager in TempImpManager.DefaultIfEmpty()
                                from Imps in TempImpStatus.DefaultIfEmpty()
                                from Lob in TempLob.DefaultIfEmpty()
                                select new
                                {
                                    DemandId = im.Id,
                                    DemandName = im.ItemName,
                                    DemandNumber = im.DemandNumber,
                                    DemandSource = im.DemandSource,
                                    InternalInitiative = im.InternalInitiative,
                                    LOB = Lob.DemandLOB,
                                    ImpStatus = Imps.Status,
                                    Status = status.Status,
                                    Priority = im.PriorityId,
                                    value = im.QtyFication,
                                    CreatedBy = createdBy.FirstName + " " + createdBy.LastName,
                                    ApprovedBy = approvedBy.FirstName + " " + approvedBy.LastName,
                                    ImpManager = impManager.FirstName + " " + impManager.LastName,
                                    CreatedDt = im.CreatedDate,
                                    StatusChange = im.StatusDt
                                }).Distinct().ToList();
                var DemandListTomodel = Demandlist.Select(e => new DemandApprovedModel
                {
                    DemandId = e.DemandId,
                    DemandName = e.DemandName,
                    DemandNumber = e.DemandNumber,
                    ApproverBy = e.ApprovedBy,
                    CreatedBy = e.CreatedBy,
                    CreatedDt = e.CreatedDt,
                    DemandSource = e.DemandSource,
                    ImplementationManager = e.ImpManager,
                    ImpStatus = e.ImpStatus,
                    InternalInitiative = e.InternalInitiative,
                    LOB = e.LOB,
                    Priority = e.Priority.ToString(),
                    Status = e.Status,
                    StatusChangeDt = e.StatusChange,
                    Value = e.value.ToString()

                });

                return DemandListTomodel;
            }
        }


        /// <summary>
        /// View All Demands - [Implemented Demands]
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="IsCusUser">Display Demands where InternalInitiative is Unchecked.</param>
        /// <returns></returns>
        public static IEnumerable<DemandImplementationModel> Implementation(int ClientId, bool IsCusUser)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();
            //True- Display Demands where InternalInitiative is Unchecked.
            if (IsCusUser)
            {
                var Demandlist = (from im in db.DMPostDemand
                                join UP3 in db.UserProfiles on im.ImplementationManager equals UP3.Id into TempImpManager
                                join impS in db.IMStatus on im.ImpStatusId equals impS.Id into TempImpStatus
                                where im.ClientId == ClientId &&
                                    (im.ImpStatusId == (int)DMStatus.Hold || im.ImpStatusId == (int)DMStatus.SubmitForPlanning || im.ImpStatusId == (int)DMStatus.Planned || im.ImpStatusId == (int)DMStatus.Initiated)
                                    && im.InternalInitiative == false && im.IsDeleted == false && im.StatusId != (int)DMStatus.Cancelled
                                from impManager in TempImpManager.DefaultIfEmpty()
                                from Imps in TempImpStatus.DefaultIfEmpty()
                                select new
                                {
                                    DemandId = im.Id,
                                    DemandName = im.ItemName,
                                    DemandNumber = im.DemandNumber,
                                    InternalInitiative = im.InternalInitiative,
                                    ImpStatus = Imps.Status,
                                    Priority = im.PriorityId,
                                    ImpManager = impManager.FirstName + " " + impManager.LastName,
                                    LastReviewDate = im.LastReviewDt,
                                    SubmitPlanning = im.ApprovedDt,
                                    planStDt = im.PlannedStartDate,
                                    planEdDt = im.PlannedEndDate,
                                    actualStDt = im.CreatedDate,
                                    actualEdDt = im.ActualEndDate,
                                    UpdatedDt = im.LastUpdatedDate,
                                    CreatedDt=im.CreatedDate,
                                    ActualProgress = im.ImplementationProgress
                                }).Distinct().ToList();
                var DemandListTomodel = Demandlist.Select(e => new DemandImplementationModel
                {
                    DemandId = e.DemandId,
                    DemandName = e.DemandName,
                    DemandNumber = e.DemandNumber,
                    InternalInitiative = e.InternalInitiative,
                    Priority = e.Priority.ToString(),
                    PrevImpPriority = "",
                    ImpPriority = "",
                    ImplementationManager = e.ImpManager,
                    ImpStatus = e.ImpStatus,
                    LastReviewDt = e.LastReviewDate,
                    SubmitForPlanningDt = e.SubmitPlanning,
                    PlannedEndDt = e.planEdDt,
                    PlannedStartDt = e.planStDt,
                    ActualEndDt = e.CreatedDt,
                    ActualStartDt = e.actualStDt,
                    ActualProgress = e.ActualProgress == null ? "0" : e.ActualProgress.ToString(),
                    LastModifiedDt = e.UpdatedDt,
                    CreatedDt = e.CreatedDt
                }).ToList();
                //To get Priority and Approvers
                foreach (var i in DemandListTomodel)
                {
                    var priority = unitOfWork.DMImpPriorityHistory.Get().Where(f => f.DemandId == i.DemandId).OrderByDescending(r => r.Id).FirstOrDefault();
                    if (priority != null)
                    {

                        i.PrevImpPriority = priority.OldImpPriority == null ? "0" : priority.OldImpPriority.ToString();
                        i.ImpPriority = priority.NewImpPriority == null ? "0" : priority.NewImpPriority.ToString();
                    }

                    
                }

                return DemandListTomodel.AsEnumerable();
            }
            else
            {
                var Demandlist = (from im in db.DMPostDemand
                                  join UP3 in db.UserProfiles on im.ImplementationManager equals UP3.Id into TempImpManager
                                join impS in db.IMStatus on im.ImpStatusId equals impS.Id into TempImpStatus
                                where im.ClientId == ClientId &&
                                    (im.ImpStatusId == (int)DMStatus.Hold || im.ImpStatusId == (int)DMStatus.SubmitForPlanning || im.ImpStatusId == (int)DMStatus.Planned || im.ImpStatusId == (int)DMStatus.Initiated)
                                     && im.IsDeleted == false && im.StatusId != (int)DMStatus.Cancelled
                                from impManager in TempImpManager.DefaultIfEmpty()
                                from Imps in TempImpStatus.DefaultIfEmpty()
                                select new
                                {
                                    DemandId = im.Id,
                                    DemandName = im.ItemName,
                                    DemandNumber = im.DemandNumber,
                                    InternalInitiative = im.InternalInitiative,
                                    ImpStatus = Imps.Status,
                                    Priority = im.PriorityId,
                                    ImpManager = impManager.FirstName + " " + impManager.LastName,
                                    LastReviewDate = im.LastReviewDt,
                                    SubmitPlanning = im.ApprovedDt,
                                    planStDt = im.PlannedStartDate,
                                    planEdDt = im.PlannedEndDate,
                                    actualStDt = im.CreatedDate,
                                    actualEdDt = im.ActualEndDate,
                                    UpdatedDt = im.LastUpdatedDate,
                                    CreatedDt = im.CreatedDate,
                                    ActualProgress = im.ImplementationProgress
                                }).Distinct().ToList();
                var DemandListTomodel = Demandlist.Select(e => new DemandImplementationModel
                {
                    DemandId = e.DemandId,
                    DemandName = e.DemandName,
                    DemandNumber = e.DemandNumber,
                    InternalInitiative = e.InternalInitiative,
                    Priority = e.Priority.ToString(),
                    PrevImpPriority = "",
                    ImpPriority = "",
                    ImplementationManager = e.ImpManager,
                    ImpStatus = e.ImpStatus,
                    LastReviewDt = e.LastReviewDate,
                    SubmitForPlanningDt = e.SubmitPlanning,
                    PlannedEndDt = e.planEdDt,
                    PlannedStartDt = e.planStDt,
                    ActualEndDt = e.CreatedDt,
                    ActualStartDt = e.actualStDt,
                    CreatedDt = e.CreatedDt,
                    ActualProgress = e.ActualProgress == null ? "0" : e.ActualProgress.ToString(),
                    LastModifiedDt = e.UpdatedDt
                }).ToList();
                //To get Priority and Approvers
                foreach (var i in DemandListTomodel)
                {
                    var priority = unitOfWork.DMImpPriorityHistory.Get().Where(f => f.DemandId == i.DemandId).OrderByDescending(r => r.Id).FirstOrDefault();
                    if (priority != null)
                    {
                        i.PrevImpPriority = priority.OldImpPriority == null ? "0" : priority.OldImpPriority.ToString();
                        i.ImpPriority = priority.NewImpPriority == null ? "0" : priority.NewImpPriority.ToString();
                    }

                    //i.PrevImpPriority = priority.OldImpPriority.ToString();
                    //i.ImpPriority = priority.NewImpPriority.ToString();
                }

                return DemandListTomodel.AsEnumerable();
            }
        }


        /// <summary>
        /// View My Demand - [Implemented DemandS]
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="UserId"></param>
        /// <param name="IsAdmin"> If YES : DisplayDemands for ImplementationManager, Else : Display DemandS for Creator.</param>
        /// <returns></returns>
        public static IEnumerable<DemandImplementationModel> Implementation(int ClientId, int UserId, bool IsAdmin)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();
            //ImplementationAdmin
            if (IsAdmin)
            {
                var Demandlist = (from im in db.DMPostDemand
                                  join UP3 in db.UserProfiles on im.ImplementationManager equals UP3.Id into TempImpManager
                                join impS in db.IMStatus on im.ImpStatusId equals impS.Id into TempImpStatus
                                join imcomp in db.DMCompletedAndNextSteps.Where(w => w.ImpManagers == UserId) on im.Id equals imcomp.DemandId into TempCompletedSteps
                                from impManager in TempImpManager.DefaultIfEmpty()
                                from Imps in TempImpStatus.DefaultIfEmpty()
                                from imcomp in TempCompletedSteps.DefaultIfEmpty()
                                where im.ClientId == ClientId && (im.ImplementationManager == UserId || im.CreatedBy == UserId || im.ApprovedBy == UserId || imcomp.ImpManagers== UserId)
                                && (im.ImpStatusId == (int)DMStatus.Hold || im.ImpStatusId == (int)DMStatus.SubmitForPlanning || im.ImpStatusId == (int)DMStatus.Planned || im.ImpStatusId == (int)DMStatus.Initiated)
                                     && im.IsDeleted == false && im.StatusId != (int)DMStatus.Cancelled

                                select new
                                {
                                    DemandId = im.Id,
                                    DemandName = im.ItemName,
                                    DemandNumber = im.DemandNumber,
                                    InternalInitiative = im.InternalInitiative,
                                    ImpStatus = Imps.Status,
                                    Priority = im.PriorityId,
                                    /*
                                     ImpPriority - 
                                     PrevImpPriority -
                                     */
                                    ImpManager = impManager.FirstName + " " + impManager.LastName,
                                    LastReviewDate = im.LastReviewDt,
                                    SubmitPlanning = im.ApprovedDt,
                                    planStDt = im.PlannedStartDate,
                                    planEdDt = im.PlannedEndDate,
                                    actualStDt = im.CreatedDate,
                                    actualEdDt = im.ActualEndDate,
                                    UpdatedDt = im.LastUpdatedDate,
                                    ActualProgress = im.ImplementationProgress,
                                    CreatedDt = im.CreatedDate
                                }).Distinct().ToList();
                var DemandListTomodel = Demandlist.Select(e => new DemandImplementationModel
                {
                    DemandId = e.DemandId,
                    DemandName = e.DemandName,
                    DemandNumber = e.DemandNumber,
                    InternalInitiative = e.InternalInitiative,
                    Priority = e.Priority.ToString(),
                    PrevImpPriority = "",
                    ImpPriority = "",
                    ImplementationManager = e.ImpManager,
                    ImpStatus = e.ImpStatus,
                    LastReviewDt = e.LastReviewDate,
                    SubmitForPlanningDt = e.SubmitPlanning,
                    PlannedEndDt = e.planEdDt,
                    PlannedStartDt = e.planStDt,
                    ActualEndDt = e.CreatedDt,
                    ActualStartDt = e.actualStDt,
                    ActualProgress = e.ActualProgress == null ? "0" : e.ActualProgress.ToString(),
                    LastModifiedDt = e.UpdatedDt,
                    CreatedDt = e.CreatedDt

                }).ToList();
                //To get Priority and Approvers
                foreach (var i in DemandListTomodel)
                {
                    var priority = unitOfWork.DMImpPriorityHistory.Get().Where(f => f.DemandId == i.DemandId).OrderByDescending(r => r.Id).FirstOrDefault();
                    if (priority != null)
                    {
                        i.PrevImpPriority = priority.OldImpPriority == null ? "0" : priority.OldImpPriority.ToString();
                        i.ImpPriority = priority.NewImpPriority == null ? "0" : priority.NewImpPriority.ToString();
                    }

                    //i.PrevImpPriority = priority.OldImpPriority.ToString();
                    //i.ImpPriority = priority.NewImpPriority.ToString();
                }

                return DemandListTomodel.AsEnumerable();
            }
            //User
            else
            {
                var Demandlist = (from im in db.DMPostDemand
                                join UP3 in db.UserProfiles on im.ImplementationManager equals UP3.Id into TempImpManager
                                join impS in db.IMStatus on im.ImpStatusId equals impS.Id into TempImpStatus
                                where im.ClientId == ClientId && im.CreatedBy == UserId &&
                                    (im.ImpStatusId == (int)DMStatus.Hold || im.ImpStatusId == (int)DMStatus.SubmitForPlanning || im.ImpStatusId == (int)DMStatus.Planned || im.ImpStatusId == (int)DMStatus.Initiated)
                                     && im.IsDeleted == false && im.StatusId != (int)DMStatus.Cancelled
                                from impManager in TempImpManager.DefaultIfEmpty()
                                from Imps in TempImpStatus.DefaultIfEmpty()
                                select new
                                {
                                    DemandId = im.Id,
                                    DemandName = im.ItemName,
                                    DemandNumber = im.DemandNumber,
                                    InternalInitiative = im.InternalInitiative,
                                    ImpStatus = Imps.Status,
                                    Priority = im.PriorityId,
                                    ImpManager = impManager.FirstName + " " + impManager.LastName,
                                    LastReviewDate = im.LastReviewDt,
                                    SubmitPlanning = im.ApprovedDt,
                                    planStDt = im.PlannedStartDate,
                                    planEdDt = im.PlannedEndDate,
                                    actualStDt = im.CreatedDate,
                                    actualEdDt = im.ActualEndDate,
                                    UpdatedDt = im.LastUpdatedDate,
                                    ActualProgress = im.ImplementationProgress,
                                    CreatedDt = im.CreatedDate
                                }).Distinct().ToList();
                var DemandListTomodel = Demandlist.Select(e => new DemandImplementationModel
                {
                    DemandId = e.DemandId,
                    DemandName = e.DemandName,
                    DemandNumber = e.DemandNumber,
                    InternalInitiative = e.InternalInitiative,
                    Priority = e.Priority.ToString(),
                    PrevImpPriority = "",
                    ImpPriority = "",
                    ImplementationManager = e.ImpManager,
                    ImpStatus = e.ImpStatus,
                    LastReviewDt = e.LastReviewDate,
                    SubmitForPlanningDt = e.SubmitPlanning,
                    PlannedEndDt = e.planEdDt,
                    PlannedStartDt = e.planStDt,
                    ActualEndDt = e.CreatedDt,
                    ActualStartDt = e.actualStDt,
                    ActualProgress = e.ActualProgress == null ? "0" : e.ActualProgress.ToString(),
                    LastModifiedDt = e.UpdatedDt,
                    CreatedDt = e.CreatedDt
                }).ToList();
                //To get Priority and Approvers
                foreach (var i in DemandListTomodel)
                {
                    var priority = unitOfWork.DMImpPriorityHistory.Get().Where(f => f.DemandId == i.DemandId).OrderByDescending(r => r.Id).FirstOrDefault();
                    if (priority != null)
                    {
                        i.PrevImpPriority = priority.OldImpPriority == null ? "0" : priority.OldImpPriority.ToString();
                        i.ImpPriority = priority.NewImpPriority == null ? "0" : priority.NewImpPriority.ToString();
                    }
                    //i.PrevImpPriority = priority.OldImpPriority.ToString();
                    //i.ImpPriority = priority.NewImpPriority.ToString();
                }


                return DemandListTomodel.AsEnumerable();
            }
        }



        /// <summary>
        /// View All Demand - [Completed Demand]
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="IsCusUser">Display Demands where InternalInitiative is Unchecked.</param>
        /// <returns></returns>
        public static IEnumerable<DemandCompletionModel> Completion(int ClientId, bool IsCusUser)
        {
            Db db = new Db();
            //True- Display Demands where InternalInitiative is Unchecked.
            if (IsCusUser)
            {
                var Demandlist = (from im in db.DMPostDemand
                                  join UP in db.UserProfiles on im.CreatedBy equals UP.Id into TempCreatedBy
                                join UP2 in db.UserProfiles on im.ApprovedBy equals UP2.Id into TempApprovedBy
                                join UP3 in db.UserProfiles on im.ImplementationManager equals UP3.Id into TempImpManager
                                where im.ClientId == ClientId && (im.ImpStatusId == (int)DMStatus.Implemented)
                                    && im.InternalInitiative == false && im.IsDeleted == false
                                from createdBy in TempCreatedBy.DefaultIfEmpty()
                                from approvedBy in TempApprovedBy.DefaultIfEmpty()
                                from impManager in TempImpManager.DefaultIfEmpty()
                                select new
                                {
                                    DemandId = im.Id,
                                    DemandName = im.ItemName,
                                    DemandNumber = im.DemandNumber,
                                    DemandSource = im.DemandSource,
                                    InternalInitiative = im.InternalInitiative,
                                    Priority = im.PriorityId,
                                    value = im.QtyFication,
                                    CreatedBy = createdBy.FirstName + " " + createdBy.LastName,
                                    ApprovedBy = approvedBy.FirstName + " " + approvedBy.LastName,
                                    ImpManager = impManager.FirstName + " " + impManager.LastName,
                                    PlannedEndDt = im.PlannedEndDate,
                                    PlannedStartDt = im.PlannedStartDate,
                                    ActualEndDt = im.ActualEndDate,
                                    ActualStartDt = im.ActualStartDate,
                                    ActualProgress = im.ImplementationProgress,
                                    /*
                                        CompletedOnSchedule - 
                                     */
                                    UpdatedDt = im.LastUpdatedDate

                                }).Distinct().ToList();
                var DemandListTomodel = Demandlist.Select(e => new DemandCompletionModel
                {
                    DemandId = e.DemandId,
                    DemandName = e.DemandName,
                    DemandNumber = e.DemandNumber,
                    ApprovedBy = e.ApprovedBy,
                    CreatedBy = e.CreatedBy,
                    LastModifiedDt = e.UpdatedDt,
                    DemandSource = e.DemandSource,
                    ImplementationManager = e.ImpManager,
                    InternalInitiative = e.InternalInitiative,
                    Priority = e.Priority.ToString(),
                    Value = e.value.ToString(),
                    PlannedEndDt = e.PlannedEndDt,
                    PlannedStartDt = e.PlannedStartDt,
                    ActualEndDt = e.ActualEndDt,
                    ActualStartDt = e.ActualStartDt,
                    ActualProgress = e.ActualProgress == null ? "0" : e.ActualProgress.ToString(),
                    //Need Calculation
                    CompletedOnSchedule = ""

                });

                return DemandListTomodel;
            }
            else
            {
                var Demandlist = (from im in db.DMPostDemand
                                  join UP in db.UserProfiles on im.CreatedBy equals UP.Id into TempCreatedBy
                                join UP2 in db.UserProfiles on im.ApprovedBy equals UP2.Id into TempApprovedBy
                                join UP3 in db.UserProfiles on im.ImplementationManager equals UP3.Id into TempImpManager
                                where im.ClientId == ClientId && (im.ImpStatusId == (int)DMStatus.Implemented) && im.IsDeleted == false
                                from createdBy in TempCreatedBy.DefaultIfEmpty()
                                from approvedBy in TempApprovedBy.DefaultIfEmpty()
                                from impManager in TempImpManager.DefaultIfEmpty()
                                select new
                                {
                                    DemandId = im.Id,
                                    DemandName = im.ItemName,
                                    DemandNumber = im.DemandNumber,
                                    DemandSource = im.DemandSource,
                                    InternalInitiative = im.InternalInitiative,
                                    Priority = im.PriorityId,
                                    value = im.QtyFication,
                                    CreatedBy = createdBy.FirstName + " " + createdBy.LastName,
                                    ApprovedBy = approvedBy.FirstName + " " + approvedBy.LastName,
                                    ImpManager = impManager.FirstName + " " + impManager.LastName,
                                    PlannedEndDt = im.PlannedEndDate,
                                    PlannedStartDt = im.PlannedStartDate,
                                    ActualEndDt = im.ActualEndDate,
                                    ActualStartDt = im.ActualStartDate,
                                    ActualProgress = im.ImplementationProgress,
                                    /*
                                        CompletedOnSchedule - 
                                     */
                                    UpdatedDt = im.LastUpdatedDate

                                }).Distinct().ToList();
                var DemandListTomodel = Demandlist.Select(e => new DemandCompletionModel
                {
                    DemandId = e.DemandId,
                    DemandName = e.DemandName,
                    DemandNumber = e.DemandNumber,
                    ApprovedBy = e.ApprovedBy,
                    CreatedBy = e.CreatedBy,
                    LastModifiedDt = e.UpdatedDt,
                    DemandSource = e.DemandSource,
                    ImplementationManager = e.ImpManager,
                    InternalInitiative = e.InternalInitiative,
                    Priority = e.Priority.ToString(),
                    Value = e.value.ToString(),
                    PlannedEndDt = e.PlannedEndDt,
                    PlannedStartDt = e.PlannedStartDt,
                    ActualEndDt = e.ActualEndDt,
                    ActualStartDt = e.ActualStartDt,
                    ActualProgress = e.ActualProgress == null ? "0" : e.ActualProgress.ToString(),
                    //Need Calculation
                    CompletedOnSchedule = ""

                });

                return DemandListTomodel;
            }
        }

        /// <summary>
        /// View My Demand - [Completed Demand]
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="UserId"></param>
        /// <param name="IsAdmin"></param>
        /// <returns></returns>
        public static IEnumerable<DemandCompletionModel> Completion(int ClientId, int UserId, bool IsAdmin)
        {
            Db db = new Db();
            //If Admin
            if (IsAdmin)
            {
                var Demandlist = (from im in db.DMPostDemand
                                  join UP in db.UserProfiles on im.CreatedBy equals UP.Id into TempCreatedBy
                                join UP2 in db.UserProfiles on im.ApprovedBy equals UP2.Id into TempApprovedBy
                                join UP3 in db.UserProfiles on im.ImplementationManager equals UP3.Id into TempImpManager
                                where im.ClientId == ClientId && (im.CreatedBy == UserId || im.ApprovedBy == UserId || im.ImplementationManager == UserId) && (im.ImpStatusId == (int)DMStatus.Implemented)
                                     && im.IsDeleted == false
                                from createdBy in TempCreatedBy.DefaultIfEmpty()
                                from approvedBy in TempApprovedBy.DefaultIfEmpty()
                                from impManager in TempImpManager.DefaultIfEmpty()
                                select new
                                {
                                    DemandId = im.Id,
                                    DemandName = im.ItemName,
                                    DemandNumber = im.DemandNumber,
                                    DemandSource = im.DemandSource,
                                    InternalInitiative = im.InternalInitiative,
                                    Priority = im.PriorityId,
                                    value = im.QtyFication,
                                    CreatedBy = createdBy.FirstName + " " + createdBy.LastName,
                                    ApprovedBy = approvedBy.FirstName + " " + approvedBy.LastName,
                                    ImpManager = impManager.FirstName + " " + impManager.LastName,
                                    PlannedEndDt = im.PlannedEndDate,
                                    PlannedStartDt = im.PlannedStartDate,
                                    ActualEndDt = im.ActualEndDate,
                                    ActualStartDt = im.ActualStartDate,
                                    ActualProgress = im.ImplementationProgress,
                                    /*
                                        CompletedOnSchedule - 
                                     */
                                    UpdatedDt = im.LastUpdatedDate

                                }).Distinct().ToList();
                var DemandListTomodel = Demandlist.Select(e => new DemandCompletionModel
                {
                    DemandId = e.DemandId,
                    DemandName = e.DemandName,
                    DemandNumber = e.DemandNumber,
                    ApprovedBy = e.ApprovedBy,
                    CreatedBy = e.CreatedBy,
                    LastModifiedDt = e.UpdatedDt,
                    DemandSource = e.DemandSource,
                    ImplementationManager = e.ImpManager,
                    InternalInitiative = e.InternalInitiative,
                    Priority = e.Priority.ToString(),
                    Value = e.value.ToString(),
                    PlannedEndDt = e.PlannedEndDt,
                    PlannedStartDt = e.PlannedStartDt,
                    ActualEndDt = e.ActualEndDt,
                    ActualStartDt = e.ActualStartDt,
                    ActualProgress = e.ActualProgress == null ? "0" : e.ActualProgress.ToString(),
                    //Need Calculation
                    CompletedOnSchedule = ""

                });

                return DemandListTomodel;
            }
            //normal User
            else
            {
                var Demandlist = (from im in db.DMPostDemand
                                  join UP in db.UserProfiles on im.CreatedBy equals UP.Id into TempCreatedBy
                                join UP2 in db.UserProfiles on im.ApprovedBy equals UP2.Id into TempApprovedBy
                                join UP3 in db.UserProfiles on im.ImplementationManager equals UP3.Id into TempImpManager
                                where im.ClientId == ClientId && im.CreatedBy == UserId && (im.ImpStatusId == (int)DMStatus.Implemented) && im.IsDeleted == false
                                from createdBy in TempCreatedBy.DefaultIfEmpty()
                                from approvedBy in TempApprovedBy.DefaultIfEmpty()
                                from impManager in TempImpManager.DefaultIfEmpty()
                                select new
                                {
                                    DemandId = im.Id,
                                    DemandName = im.ItemName,
                                    DemandNumber = im.DemandNumber,
                                    DemandSource = im.DemandSource,
                                    InternalInitiative = im.InternalInitiative,
                                    Priority = im.PriorityId,
                                    value = im.QtyFication,
                                    CreatedBy = createdBy.FirstName + " " + createdBy.LastName,
                                    ApprovedBy = approvedBy.FirstName + " " + approvedBy.LastName,
                                    ImpManager = impManager.FirstName + " " + impManager.LastName,
                                    PlannedEndDt = im.PlannedEndDate,
                                    PlannedStartDt = im.PlannedStartDate,
                                    ActualEndDt = im.ActualEndDate,
                                    ActualStartDt = im.ActualStartDate,
                                    ActualProgress = im.ImplementationProgress,
                                    /*
                                        CompletedOnSchedule - 
                                     */
                                    UpdatedDt = im.LastUpdatedDate

                                }).Distinct().ToList();
                var DemandListTomodel = Demandlist.Select(e => new DemandCompletionModel
                {
                    DemandId = e.DemandId,
                    DemandName = e.DemandName,
                    DemandNumber = e.DemandNumber,
                    ApprovedBy = e.ApprovedBy,
                    CreatedBy = e.CreatedBy,
                    LastModifiedDt = e.UpdatedDt,
                    DemandSource = e.DemandSource,
                    ImplementationManager = e.ImpManager,
                    InternalInitiative = e.InternalInitiative,
                    Priority = e.Priority.ToString(),
                    Value = e.value.ToString(),
                    PlannedEndDt = e.PlannedEndDt,
                    PlannedStartDt = e.PlannedStartDt,
                    ActualEndDt = e.ActualEndDt,
                    ActualStartDt = e.ActualStartDt,
                    ActualProgress = e.ActualProgress == null ? "0" : e.ActualProgress.ToString(),
                    //Need Calculation
                    CompletedOnSchedule = ""

                });

                return DemandListTomodel;
            }
        }
        //----------------------------------------------Search Demands---------------------------------------------//

        public static IEnumerable<DemandSearchModel> DemandSearch(string stringLike, int ClientId, bool IsCusUser)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();
            if (IsCusUser)
            {
                var tempList = (from Demand in db.DMPostDemand
                                join status in db.IMStatus on Demand.StatusId equals status.Id
                                join impStatus in db.IMStatus on Demand.ImpStatusId equals impStatus.Id into tempImpStatus
                                from ImpStatus in tempImpStatus.DefaultIfEmpty()
                                where (Demand.ItemName.Contains(stringLike) || Demand.Description.Contains(stringLike) || Demand.DemandNumber.Contains(stringLike)) && Demand.ClientId == ClientId && Demand.InternalInitiative == false
                                select new
                                {
                                    Id = Demand.Id,
                                    DemandNumber = Demand.DemandNumber,
                                    Name = Demand.ItemName,
                                    Status = status.Status,
                                    ImpStatus = ImpStatus.Status
                                }
                ).Distinct().ToList();
                var searchDemandList = tempList.Select(e => new DemandSearchModel
                {
                    DemandId = e.Id,
                    DemandNumber = e.DemandNumber,
                    DemandName = e.Name,
                    Status = (String.IsNullOrEmpty(e.ImpStatus) || e.Status.ToLower() == "closed") ? e.Status : e.ImpStatus
                });
                return searchDemandList;
            }
            else
            {
                var tempList = (from Demand in db.DMPostDemand
                                join status in db.IMStatus on Demand.StatusId equals status.Id
                                join impStatus in db.IMStatus on Demand.ImpStatusId equals impStatus.Id into tempImpStatus
                                from ImpStatus in tempImpStatus.DefaultIfEmpty()
                                where (Demand.ItemName.Contains(stringLike) || Demand.Description.Contains(stringLike) || Demand.DemandNumber.Contains(stringLike)) && Demand.ClientId == ClientId
                                select new
                                {
                                    Id = Demand.Id,
                                    DemandNumber = Demand.DemandNumber,
                                    Name = Demand.ItemName,
                                    Status = status.Status,
                                    ImpStatus = ImpStatus.Status
                                }
                ).Distinct().ToList();
                var searchDemandList = tempList.Select(e => new DemandSearchModel
                {
                    DemandId = e.Id,
                    DemandNumber = e.DemandNumber,
                    DemandName = e.Name,
                    Status = (String.IsNullOrEmpty(e.ImpStatus) || e.Status.ToLower() == "closed") ? e.Status : e.ImpStatus
                });
                return searchDemandList;
            }
        }


        //------------------------------------------------------------------------------------------------------------------------------------------//
        //----------------------------------------------------------- Post Demand Functions -----------------------------------------------------------//
        //------------------------------------------------------------------------------------------------------------------------------------------//


        //Home Tab
        public static DemandViewModel saveDemand(DemandViewModel IVM, int uid)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            if (IVM.DemandId == 0)
            {
                //---------DemandDetails--------------//
                DMPostDemand Demand = new DMPostDemand();
                Demand.ItemName = IVM.DemandName.Trim();
                Demand.DemandSource = IVM.DemandSourceId;
                Demand.InternalInitiative = IVM.InternalInit;
                Demand.Description = IVM.Description;
                Demand.ImprovementTheme = IVM.ImprovementTheme;
                Demand.Approved_Arch = IVM.Approved_Arch;
                if (IVM.DemandtypeId != 0)
                {
                    Demand.DemandTypeId = IVM.DemandtypeId;
                }
                if (IVM.LOBId != 0)
                {
                    Demand.LOBId = IVM.LOBId;
                }
                //Other Demand Details
                Demand.CreatedBy = uid;
                Demand.CreatedDate = DateTime.Now;//Need to do Offset.
                Demand.ActualStartDate=DateTime.Now;
                Demand.PlannedStartDate = DateTime.Now;
                Demand.LastUpdatedDate = DateTime.Now;//Need to do Offset.
                Demand.LastReviewDt = DateTime.Now;
                Demand.StatusId = (int)DMStatus.Created;
                Demand.ClientId = IVM.ClientId;
                //Demand.UpdatedBy = uid;
                Demand.StatusDt = DateTime.Now;
                Demand.MultipleApprovals = IVM.MultipleApprovals;
                Demand.DemandPlatformLead = IVM.DemandPlatformLead;
                unitOfWork.DMPostDemand.Insert(Demand);
                unitOfWork.Save();

                DMStatusHistory DMStatusHistory = new DMStatusHistory()
                {
                    DemandId = Demand.Id,
                    NewStatusId = Demand.StatusId,
                    ClientId = IVM.ClientId,
                    IsDeleted = false,
                    CreatedBy = uid,
                    UpdatedBy = uid,
                    LastUpdatedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                unitOfWork.DMStatusHistory.Insert(DMStatusHistory);

                unitOfWork.Save();
                IVM.DemandId = Demand.Id;
                return IVM;
            }
            //If it is Edit
            else
            {
                var Demand = unitOfWork.DMPostDemand.GetByID(IVM.DemandId);
                var TempStatusId = Demand.StatusId;
                Demand.ItemName = IVM.DemandName.Trim();
                Demand.DemandSource = IVM.DemandSourceId;
                Demand.InternalInitiative = IVM.InternalInit;
                Demand.Description = IVM.Description.Trim();
                Demand.ImprovementTheme = IVM.ImprovementTheme;
                Demand.Approved_Arch = IVM.Approved_Arch;
                if (IVM.DemandtypeId != 0)
                {
                    Demand.DemandTypeId = IVM.DemandtypeId;
                }
                if (IVM.LOBId != 0)
                {
                    Demand.LOBId = IVM.LOBId;
                }
                //Other Demand Details
                Demand.UpdatedBy = uid;
                Demand.LastUpdatedDate = DateTime.Now;//Need to do Offset.
                Demand.LastReviewDt = DateTime.Now;

                if (Demand.StatusId == (int)DMStatus.SubmitForRevision)
                {
                    Demand.StatusId = (int)DMStatus.Revised;
                    Demand.StatusDt = DateTime.Now;
                }
                Demand.DemandPlatformLead = IVM.DemandPlatformLead;

                unitOfWork.DMPostDemand.Update(Demand);
                DMStatusHistory DMStatusHistory = new DMStatusHistory()
                {
                    DemandId = IVM.DemandId,
                    OldStatusId = TempStatusId,
                    NewStatusId = Demand.StatusId,
                    ClientId= IVM.ClientId,
                    IsDeleted = false,
                    CreatedBy = uid,
                    UpdatedBy = uid,
                    LastUpdatedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                unitOfWork.DMStatusHistory.Insert(DMStatusHistory);
                unitOfWork.Save();
                return IVM;
            }
        }

        public static void saveImpDetails(DemandViewModel IVM, int uid, string ClientName, string DemandLink)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

           
            var ExistDemand = unitOfWork.DMPostDemand.GetByID(IVM.DemandId);
            IVM.ActualStartDt=ExistDemand.CreatedDate;

            if (ExistDemand != null)
            {

                var TempOldStatus = ExistDemand.ImpStatusId;
                var TempOldPriority = ExistDemand.PriorityId;
                var TempImpStatusComment = ExistDemand.StatusComment;
                //When Demand is just approved and Implementation  Manager is not Selected.
                if (ExistDemand.StatusId == (int)DMStatus.Approved && ExistDemand.ImpStatusId == null && IVM.ImpDemandStatusID == null)
                {
                    ExistDemand.ImpPriorityId = IVM.PriorityId;
                    ExistDemand.PriorityId = IVM.PriorityId;
                    if (IVM.ImpPriority != TempOldPriority)
                    {
                        DMImpPriorityHistory DMImpPriorityHistory = new DMImpPriorityHistory()
                        {
                            DemandId = IVM.DemandId,
                            OldImpPriority = TempOldPriority,
                            NewImpPriority = IVM.ImpPriority,
                            ClientId = IVM.ClientId,
                            UpdatedBy = uid,
                            UpdatedDt = DateTime.Now
                        };
                        unitOfWork.DMImpPriorityHistory.Insert(DMImpPriorityHistory);
                    }
                    if (IVM.ImpManager != null && ExistDemand.ImplementationManager != IVM.ImpManager)
                    {
                        ExistDemand.ImplementationManager = IVM.ImpManager;
                        var up = unitOfWork.UserProfile.GetByID(IVM.ImpManager);
                        if (up != null)
                        {
                            new PostDemandController().SendDemandNotification(up.Email, up.FirstName + " " + up.LastName, ExistDemand.ItemName, DMStatus.SubmitForPlanning, ClientName, DemandLink);
                        }
                        ExistDemand.ImpStatusId = (int)DMStatus.SubmitForPlanning;
                        ExistDemand.ImpStatusDt = DateTime.Now;
                        DMStatusHistory DMStatusHistory = new DMStatusHistory()
                        {
                            DemandId = IVM.DemandId,
                            OldStatusId = ExistDemand.StatusId,
                            NewStatusId = ExistDemand.ImpStatusId,
                            Comments = IVM.StatusComment,
                            ClientId = IVM.ClientId,
                            IsDeleted = false,
                            CreatedBy = uid,
                            UpdatedBy = uid,
                            LastUpdatedDate = DateTime.Now,
                            CreatedDate = DateTime.Now
                        };
                        unitOfWork.DMStatusHistory.Insert(DMStatusHistory);
                    }
                    ExistDemand.LastUpdatedDate = DateTime.Now;
                    ExistDemand.LastReviewDt = DateTime.Now;
                    IVM.LastReviewDt = DateTime.Now;
                    ExistDemand.UpdatedBy = uid;
                    ExistDemand.ComplexityId = IVM.ComplexityId;
                    ExistDemand.DocumentTypeId = IVM.DocumentTypeId;
                    ExistDemand.DocumentNo = IVM.DocumentNo;
                    ExistDemand.EstimatedCost = IVM.EstimatedCost;
                    ExistDemand.ClientId = IVM.ClientId;
                    if (IVM.StatusComment != null)
                    {
                        ExistDemand.StatusComment = IVM.StatusComment;
                    }
                    unitOfWork.DMPostDemand.Update(ExistDemand);
                    // unitOfWork.Save();
                }
                //When Updating data with no Implementation Status selected.
                else if (ExistDemand.ImpStatusId == (int)DMStatus.SubmitForPlanning && IVM.ImpDemandStatusID == null)
                {
                    ExistDemand.ImpPriorityId = IVM.PriorityId;
                    ExistDemand.PriorityId = IVM.PriorityId;

                    if (IVM.ImpPriority != TempOldPriority)
                    {
                        DMImpPriorityHistory DMImpPriorityHistory = new DMImpPriorityHistory()
                        {
                            DemandId = IVM.DemandId,
                            OldImpPriority = TempOldPriority,
                            NewImpPriority = IVM.PriorityId,
                            ClientId = IVM.ClientId,
                            UpdatedBy = uid,
                            UpdatedDt = DateTime.Now
                        };
                        unitOfWork.DMImpPriorityHistory.Insert(DMImpPriorityHistory);
                    }

                    if (IVM.ImpManager != null && ExistDemand.ImplementationManager != IVM.ImpManager)
                    {
                        ExistDemand.ImplementationManager = IVM.ImpManager;
                        var up = unitOfWork.UserProfile.GetByID(IVM.ImpManager);
                        if (up != null)
                        {
                            new PostDemandController().SendDemandNotification(up.Email, up.FirstName + " " + up.LastName, ExistDemand.ItemName, (DMStatus)ExistDemand.ImpStatusId, ClientName, DemandLink);
                        }
                    }
                    ExistDemand.LastUpdatedDate = DateTime.Now;
                    ExistDemand.LastReviewDt = DateTime.Now;
                    IVM.LastReviewDt = DateTime.Now;
                    ExistDemand.UpdatedBy = uid;
                    ExistDemand.ComplexityId = IVM.ComplexityId;
                    ExistDemand.DocumentTypeId = IVM.DocumentTypeId;
                    ExistDemand.DocumentNo = IVM.DocumentNo;
                    ExistDemand.EstimatedCost = IVM.EstimatedCost;
                    if (IVM.StatusComment != null)
                    {
                        ExistDemand.StatusComment = IVM.StatusComment;
                    }
                    unitOfWork.DMPostDemand.Update(ExistDemand);
                    //unitOfWork.Save();
                }
                //With Status selected.
                else
                {
                    if (IVM.ImpDemandStatusID == (int)DMStatus.Implemented)
                    {
                        ExistDemand.CompletedDt = DateTime.Now;
                        ExistDemand.ImpStatusId = IVM.ImpDemandStatusID;
                        ExistDemand.ActualEndDate = DateTime.Now;
                        IVM.ActualEndDt = DateTime.Now;
                    }
                    else if (IVM.ImpDemandStatusID == (int)DMStatus.Cancelled)
                    {
                        ExistDemand.StatusId = IVM.ImpDemandStatusID.Value;
                        ExistDemand.StatusDt = DateTime.Now;
                        ExistDemand.ActualEndDate = null;

                    }
                    else
                    {
                        ExistDemand.ImpStatusId = IVM.ImpDemandStatusID;
                        ExistDemand.ImpStatusDt = DateTime.Now;
                        ExistDemand.ActualEndDate = null;
                    }
                    if (ExistDemand.ImpStatusId == (int)DMStatus.Hold || ExistDemand.StatusId == (int)DMStatus.Cancelled)
                    {
                        //send Email Notification
                        var up = unitOfWork.UserProfile.GetByID(ExistDemand.ApprovedBy);
                        if (up != null)
                        {
                            if (ExistDemand.StatusId == (int)DMStatus.Cancelled)
                            {
                                new PostDemandController().SendDemandNotification(up.Email, up.FirstName + " " + up.LastName, ExistDemand.ItemName, (DMStatus)ExistDemand.StatusId, ClientName, DemandLink);
                            }
                            else
                            {
                                new PostDemandController().SendDemandNotification(up.Email, up.FirstName + " " + up.LastName, ExistDemand.ItemName, (DMStatus)ExistDemand.ImpStatusId, ClientName, DemandLink);
                            }
                        }
                    }

                    ExistDemand.ImpPriorityId = IVM.PriorityId;
                    ExistDemand.PriorityId = IVM.PriorityId;

                    //--HIstory Table..
                    if (IVM.ImpPriority != TempOldPriority)
                    {
                        DMImpPriorityHistory DMImpPriorityHistory = new DMImpPriorityHistory()
                        {
                            DemandId = IVM.DemandId,
                            OldImpPriority = TempOldPriority,
                            NewImpPriority = IVM.PriorityId,
                            ClientId = IVM.ClientId,
                            UpdatedBy = uid,
                            UpdatedDt = DateTime.Now
                        };
                        unitOfWork.DMImpPriorityHistory.Insert(DMImpPriorityHistory);
                    }

                    if (IVM.ImpManager != null && ExistDemand.ImplementationManager != IVM.ImpManager)
                    {
                        ExistDemand.ImplementationManager = IVM.ImpManager;
                        var up = unitOfWork.UserProfile.GetByID(IVM.ImpManager);
                        if (up != null)
                        {
                            new PostDemandController().SendDemandNotification(up.Email, up.FirstName + " " + up.LastName, ExistDemand.ItemName, (DMStatus)ExistDemand.ImpStatusId, ClientName, DemandLink);
                        }
                    }
                    IVM.LastReviewDt = DateTime.Now;
                    ExistDemand.PlannedStartDate = IVM.ActualStartDt;
                    ExistDemand.PlannedEndDate = IVM.PlannedEndDt;
                    //ExistDemand.ActualEndDate = IVM.ActualEndDt;
                    ExistDemand.ImplementationProgress = (IVM.ImpDemandStatusID == 4) ? 100 : IVM.ActualProgress;
                    ExistDemand.ActualStartDate = IVM.ActualStartDt;
                    ExistDemand.ComplexityId = IVM.ComplexityId;
                    ExistDemand.DocumentTypeId = IVM.DocumentTypeId;
                    ExistDemand.DocumentNo = IVM.DocumentNo;
                    ExistDemand.EstimatedCost = IVM.EstimatedCost;
                    ExistDemand.LastUpdatedDate = DateTime.Now;
                    ExistDemand.LastReviewDt = DateTime.Now;

                    ExistDemand.UpdatedBy = uid;
                    if (IVM.StatusComment != null)
                    {
                        ExistDemand.StatusComment = IVM.StatusComment;
                    }
                    unitOfWork.DMPostDemand.Update(ExistDemand);

                    //Status History and Priority history
                    if (IVM.ImpDemandStatusID != TempOldStatus)
                    {
                        DMStatusHistory DMStatusHistory = new DMStatusHistory()
                        {
                            DemandId = IVM.DemandId,
                            OldStatusId = TempOldStatus,
                            NewStatusId = IVM.ImpDemandStatusID,
                            Comments = IVM.StatusComment,
                            ClientId = IVM.ClientId,
                            IsDeleted = false,
                            CreatedBy = uid,
                            UpdatedBy = uid,
                            LastUpdatedDate = DateTime.Now,
                            CreatedDate = DateTime.Now
                        };
                        unitOfWork.DMStatusHistory.Insert(DMStatusHistory);
                    }
                    else if (IVM.StatusComment != TempImpStatusComment)
                    {
                        DMStatusHistory DMStatusHistory = new DMStatusHistory()
                        {
                            // Id= checkComment.Id,
                            DemandId = IVM.DemandId,
                            OldStatusId = TempOldStatus,
                            NewStatusId = IVM.ImpDemandStatusID,
                            Comments = IVM.StatusComment,
                            ClientId = IVM.ClientId,
                            IsDeleted = false,
                            CreatedBy = uid,
                            UpdatedBy = uid,
                            LastUpdatedDate = DateTime.Now,
                        };
                        unitOfWork.DMStatusHistory.Insert(DMStatusHistory);
                    }
                    unitOfWork.Save();
                }
            }
        }

        public static DemandViewModel getDemandDetails(int DemandId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            DemandViewModel cv = new DemandViewModel();
            var iDeatils = unitOfWork.DMPostDemand.GetByID(DemandId);
            var analysis = unitOfWork.DMAnalysisRootCause.Get(e => e.DemandId == DemandId).FirstOrDefault();
            var solution = unitOfWork.DMSolution.Get(e => e.DemandId == DemandId).FirstOrDefault();
            if (analysis != null && analysis.Analysis.Trim() != "")
            {
                cv.Analysis = analysis.Analysis;
            }
            if (solution != null && solution.Solution.Trim() != "")
            {
                cv.Solution = solution.Solution;
            }
            cv.DemandName = iDeatils.ItemName;
            cv.DemandId = iDeatils.Id;
            cv.DemandNumber = iDeatils.DemandNumber;
            cv.DemandPlatformLead = iDeatils.DemandPlatformLead;
            cv.DemandStatusID = iDeatils.StatusId;
            cv.ImpDemandStatusID = iDeatils.ImpStatusId;
            cv.IsDeleted = iDeatils.IsDeleted;
            var temp = (DMStatus)iDeatils.StatusId; 
            cv.DemandStatus = temp.ToString();

            if (iDeatils.ImpStatusId != null)
            {
                var temp1 = (DMStatus)iDeatils.ImpStatusId.Value;
                cv.DemandStatus = temp1.ToString();
            }
            if (iDeatils.StatusId == (int)DMStatus.Cancelled)
            {
                var temp1 = (DMStatus)iDeatils.StatusId;
                cv.DemandStatus = temp1.ToString(); /*"Cancelled";*/
                cv.ImpDemandStatusID = iDeatils.StatusId;
            }
            cv.DemandtypeId = iDeatils.DemandTypeId;
            cv.DemandSourceId = iDeatils.DemandSource;
            cv.LOBId = iDeatils.LOBId;
            cv.InternalInit = iDeatils.InternalInitiative;
            cv.Description = iDeatils.Description;
            cv.ImprovementTheme = iDeatils.ImprovementTheme;
            cv.Approved_Arch = iDeatils.Approved_Arch;

            //Imp Priority
            cv.ImpPriority = iDeatils.ImpPriorityId;
            cv.PriorityId = iDeatils.PriorityId;

            cv.ImpManager = iDeatils.ImplementationManager;

            //need to add Analysis and Solution
            cv.LastReviewDt = iDeatils.LastReviewDt;
            cv.PlannedStartDt = iDeatils.ActualStartDate;
            cv.PlannedEndDt = iDeatils.PlannedEndDate;
            cv.ActualStartDt = iDeatils.ActualStartDate;
            cv.ActualProgress = iDeatils.ImplementationProgress;
            cv.ActualEndDt = iDeatils.ActualEndDate;
            //to read overall implementation status
            cv.StatusComment = iDeatils.StatusComment;
            cv.ComplexityId = iDeatils.ComplexityId;
            cv.EstimatedCost = iDeatils.EstimatedCost;
            cv.DocumentNo = iDeatils.DocumentNo;
            cv.DocumentTypeId = iDeatils.DocumentTypeId;
            cv.CreatedBy = iDeatils.CreatedBy;
            cv.ApprovedBy = iDeatils.ApprovedBy;
            if(iDeatils.StatusId==(int)DMStatus.SubmitForReview || iDeatils.StatusId == (int)DMStatus.SubmitForApproval)
            {
                var approverList = unitOfWork.DMDemandApprover.Get(g => g.DemandId == DemandId);
                if (approverList.Where(g => g.ApprovedDate != null).Any())
                    cv.DemandStatus = "Pending for Approval";
            }
                return cv;
        }

        /// <summary>
        /// Delete Demands
        /// </summary>
        /// <param name="DemandIDs"></param>
        /// <param name="ClientId"></param>
        /// <param name="uid"></param>
        public static void deleteDemands(string DemandIDs, int ClientId, int uid)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

            string[] SelectedDemandIds = DemandIDs.Split(',');
            foreach (var i in SelectedDemandIds)
            {
                var _DemandId = Convert.ToInt32(i);
                var checkStatus = unitOfWork.DMPostDemand.Get().Where(e => e.Id == _DemandId && e.CreatedBy == uid && e.StatusId == (int)DMStatus.Created).Any();
                if (checkStatus)
                {
                    var Demand = unitOfWork.DMPostDemand.GetByID(_DemandId);
                    if (!Demand.IsDeleted)
                    {
                        Demand.IsDeleted = true;
                        Demand.LastUpdatedDate = DateTime.Now;
                        Demand.LastReviewDt = DateTime.Now;

                        Demand.UpdatedBy = uid;
                        unitOfWork.DMPostDemand.Update(Demand);
                        unitOfWork.Save();
                    }
                }
            }
        }

        //---------------------------------------------------------------STATUS Change---------------------------------------------------------------//

        public static void submitforreview(string DemandIDs, string ClientName, int uid, string DemandLink)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

            string[] SelectedDemandIds = DemandIDs.Split(',');
            foreach (var i in SelectedDemandIds)
            {
                var _DemandId = Convert.ToInt32(i);
                // -->  Removed: Demand with created Status without Approver cannot be Submitted for Review
                var ExistDemand = unitOfWork.DMPostDemand.GetByID(_DemandId);
                var checkStatus = unitOfWork.DMPostDemand.Get().Where(e => e.Id == _DemandId && (e.StatusId == (int)DMStatus.Revised || e.StatusId == (int)DMStatus.Created)).Any();
                var checkApproverExists = unitOfWork.DMDemandApprover.Get(e => e.DemandId == _DemandId).Any();
                if (checkStatus && checkApproverExists)
                {
                    var Demand = unitOfWork.DMPostDemand.GetByID(_DemandId);
                    var oldStatus = Demand.StatusId;
                    Demand.StatusId = (int)DMStatus.SubmitForReview;
                    Demand.StatusDt = DateTime.Now;
                    unitOfWork.DMPostDemand.Update(Demand);

                    DMStatusHistory statusHistory = new DMStatusHistory();
                    statusHistory.DemandId = Demand.Id;
                    statusHistory.LastUpdatedDate = DateTime.Now;
                    statusHistory.UpdatedBy = uid;
                    statusHistory.OldStatusId = oldStatus;
                    statusHistory.NewStatusId = Demand.StatusId;
                    unitOfWork.DMStatusHistory.Insert(statusHistory);

                    unitOfWork.Save();
                    var Approvers = unitOfWork.DMDemandApprover.Get().Where(e => e.DemandId == _DemandId);
                    if (Approvers.Count() != 0)
                    {
                        DemandLink = DemandLink.Trim() + "&DemandId=" + _DemandId.ToString();
                        foreach (var appr in Approvers)
                        {
                            var up = unitOfWork.UserProfile.GetByID(appr.ApproverId);
                            if (up != null)
                            {
                                new PostDemandController().SendDemandNotification(up.Email, up.FirstName + " " + up.LastName, ExistDemand.ItemName, DMStatus.SubmitForReview, ClientName, DemandLink);
                            }
                        }
                    }
                }
            }
        }



        public static void submitforapproval(string DemandIDs, int ClientId, int uid)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

            string[] SelectedDemandIds = DemandIDs.Split(',');
            foreach (var i in SelectedDemandIds)
            {
                var _DemandId = Convert.ToInt32(i);
                var checkStatus = unitOfWork.DMPostDemand.Get().Where(e => e.Id == _DemandId && e.StatusId == (int)DMStatus.SubmitForReview).Any();
                if (checkStatus)
                {
                    var Demand = unitOfWork.DMPostDemand.GetByID(_DemandId);
                    var oldStatus = Demand.StatusId;
                    Demand.StatusId = (int)DMStatus.SubmitForApproval;
                    Demand.StatusDt = DateTime.Now;
                    unitOfWork.DMPostDemand.Update(Demand);

                    DMStatusHistory statusHistory = new DMStatusHistory();
                    statusHistory.DemandId = Demand.Id;
                    statusHistory.LastUpdatedDate = DateTime.Now;
                    statusHistory.UpdatedBy = uid;
                    statusHistory.ClientId = ClientId;
                    statusHistory.OldStatusId = oldStatus;
                    statusHistory.NewStatusId = Demand.StatusId;
                    unitOfWork.DMStatusHistory.Insert(statusHistory);

                    unitOfWork.Save();
                }
            }
        }

        //---------------------------------------------------------------Different Tabs---------------------------------------------------------------//


        public static IList<DMItemBusinessBenefitsInput> GetBusinessBenefitsDetails(int Id)
        {

            IList<DMItemBusinessBenefitsInput> BusinessBenefit = new List<DMItemBusinessBenefitsInput>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<DMItemBusinessBenefits> BusinessBenefitDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (Id != 0)
                    BusinessBenefitDetails = unitOfWork.DMItemBusinessBenefits.Get(cid => cid.ItemId == Id).ToList();

                if (BusinessBenefitDetails != null)
                {
                    foreach (var _businessdetail in BusinessBenefitDetails)
                    {
                        DMItemBusinessBenefitsInput bb = new DMItemBusinessBenefitsInput();
                        bb.Id = _businessdetail.Id;
                        bb.ItemId = _businessdetail.ItemId;
                        bb.Name = _businessdetail.Name;
                        bb.Detail = _businessdetail.Detail;
                        bb.BenefitType = String.IsNullOrWhiteSpace(_businessdetail.BenefitType) ? "" : _businessdetail.BenefitType;
                        bb.IncidentCount = _businessdetail.IncidentCount;
                        bb.Value = _businessdetail.Value;
                        bb.FTEHours = _businessdetail.FTEHours;
                        bb.CreatedBy = _businessdetail.CreatedBy;
                        bb.UpdatedBy = _businessdetail.UpdatedBy;
                        bb.CreatedDate = _businessdetail.CreatedDate;
                        bb.LastUpdatedDate = _businessdetail.LastUpdatedDate;
                        BusinessBenefit.Add(bb);
                    }
                }
                return BusinessBenefit;
            }

        }
        public static int CreateBusinessBenefits(DMItemBusinessBenefitsInput dmItemBusinessBenefits,int clientid, int Id)
        {
            int id = 0;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var businessDetails = new DMItemBusinessBenefits();
                    businessDetails.ItemId = Id;
                    businessDetails.Name = dmItemBusinessBenefits.Name;
                    businessDetails.Detail = dmItemBusinessBenefits.Detail;
                    businessDetails.ClientId = clientid;
                    businessDetails.BenefitType = dmItemBusinessBenefits.BenefitType;
                    businessDetails.IncidentCount = dmItemBusinessBenefits.IncidentCount;
                    businessDetails.Value = dmItemBusinessBenefits.Value;
                    businessDetails.FTEHours = dmItemBusinessBenefits.FTEHours;
                    businessDetails.CreatedDate = DateTime.Now;
                    businessDetails.LastUpdatedDate = DateTime.Now;
                    businessDetails.CreatedBy = dmItemBusinessBenefits.CreatedBy;
                    businessDetails.UpdatedBy = businessDetails.CreatedBy;
                    unitOfWork.DMItemBusinessBenefits.Insert(businessDetails);
                    unitOfWork.Save();
                    id = businessDetails.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool UpdateBusinessBenefits(DMItemBusinessBenefitsInput dmItemBusinessBenefits)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.DMItemBusinessBenefits.GetByID(dmItemBusinessBenefits.Id);
                    updatetitem.Name = dmItemBusinessBenefits.Name;
                    updatetitem.Detail = dmItemBusinessBenefits.Detail;
                    updatetitem.BenefitType = dmItemBusinessBenefits.BenefitType;
                    updatetitem.IncidentCount = dmItemBusinessBenefits.IncidentCount;
                    updatetitem.Value = dmItemBusinessBenefits.Value;
                    updatetitem.FTEHours = dmItemBusinessBenefits.FTEHours;
                    updatetitem.UpdatedBy = dmItemBusinessBenefits.UpdatedBy;
                    updatetitem.LastUpdatedDate = DateTime.Now;
                    unitOfWork.DMItemBusinessBenefits.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveBusinessBenefits(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.DMItemBusinessBenefits.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }

        /*---------------------------IT SAVINGS-----------------------------------*/


        public static IList<DMItemITBenefitsInput> GetITSavingsDetails(int Id)
        {

            IList<DMItemITBenefitsInput> ITSavings = new List<DMItemITBenefitsInput>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<DMItemITBenefits> ITSavingsDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (Id != 0)
                    ITSavingsDetails = unitOfWork.DMItemITBenefits.Get(cid => cid.ItemId == Id).ToList();

                if (ITSavingsDetails != null)
                {
                    foreach (var _itsavingsdetail in ITSavingsDetails)
                    {
                        DMItemITBenefitsInput ib = new DMItemITBenefitsInput();
                        ib.Id = _itsavingsdetail.Id;
                        ib.ItemId = _itsavingsdetail.ItemId;
                        ib.Name = _itsavingsdetail.Name;
                        ib.Detail = _itsavingsdetail.Detail;
                        ib.BenefitType = String.IsNullOrWhiteSpace(_itsavingsdetail.BenefitType) ? "" : _itsavingsdetail.BenefitType;
                        ib.Value = _itsavingsdetail.Value;
                        ib.FTEHours = _itsavingsdetail.FTEHours;
                        ib.IncidentCount = _itsavingsdetail.IncidentCount;
                        ib.CreatedBy = _itsavingsdetail.CreatedBy;
                        ib.UpdatedBy = _itsavingsdetail.UpdatedBy;
                        ib.CreatedDate = _itsavingsdetail.CreatedDate;
                        ib.LastUpdatedDate = _itsavingsdetail.LastUpdatedDate;
                        ITSavings.Add(ib);
                    }
                }
                return ITSavings;
            }

        }
        public static int CreateITSavings(DMItemITBenefitsInput dmItemITBenefitsInput,int clientId, int Id)
        {
            int id = 0;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var itsavingsDetails = new DMItemITBenefits();
                    itsavingsDetails.ItemId = Id;
                    itsavingsDetails.Name = dmItemITBenefitsInput.Name;
                    itsavingsDetails.Detail = dmItemITBenefitsInput.Detail;
                    itsavingsDetails.ClientId = clientId;
                    itsavingsDetails.BenefitType = dmItemITBenefitsInput.BenefitType;
                    itsavingsDetails.Value = dmItemITBenefitsInput.Value;
                    itsavingsDetails.FTEHours = dmItemITBenefitsInput.FTEHours;
                    itsavingsDetails.IncidentCount = dmItemITBenefitsInput.IncidentCount;
                    itsavingsDetails.CreatedDate = DateTime.Now;
                    itsavingsDetails.CreatedBy = dmItemITBenefitsInput.CreatedBy;
                    itsavingsDetails.LastUpdatedDate = DateTime.Now;
                    itsavingsDetails.UpdatedBy = itsavingsDetails.CreatedBy;
                    unitOfWork.DMItemITBenefits.Insert(itsavingsDetails);
                    unitOfWork.Save();
                    id = itsavingsDetails.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool UpdateITSavings(DMItemITBenefitsInput dmItemITBenefitsInput)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.DMItemITBenefits.GetByID(dmItemITBenefitsInput.Id);
                    updatetitem.Name = dmItemITBenefitsInput.Name;
                    updatetitem.Detail = dmItemITBenefitsInput.Detail;
                    updatetitem.BenefitType = dmItemITBenefitsInput.BenefitType;
                    updatetitem.Value = dmItemITBenefitsInput.Value;
                    updatetitem.FTEHours = dmItemITBenefitsInput.FTEHours;
                    updatetitem.IncidentCount = dmItemITBenefitsInput.IncidentCount;
                    updatetitem.UpdatedBy = dmItemITBenefitsInput.UpdatedBy;
                    updatetitem.LastUpdatedDate = DateTime.Now;
                    unitOfWork.DMItemITBenefits.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveITSavings(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.DMItemITBenefits.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }


        /*------------------------------COMMENTS-----------------------------------*/



        public static IList<DMItemCommentsInput> GetCommentsDetails(int Id)
        {
            IList<DMItemCommentsInput> Commentsss = new List<DMItemCommentsInput>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<DMItemComments> CommentsDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (Id != 0)
                    CommentsDetails = unitOfWork.DMItemComments.Get(cid => cid.ItemId == Id).ToList();

                if (CommentsDetails != null)
                {
                    foreach (var _commentsdetail in CommentsDetails)
                    {
                        DMItemCommentsInput cc = new DMItemCommentsInput();
                        cc.Id = _commentsdetail.Id;
                        cc.ItemId = _commentsdetail.ItemId;
                        cc.Comments = _commentsdetail.Comments;
                        cc.CreatedBy = _commentsdetail.CreatedBy;
                        cc.UpdatedBy = _commentsdetail.UpdatedBy;
                        cc.CreatedDate = _commentsdetail.CreatedDate;
                        cc.LastUpdatedDate = _commentsdetail.LastUpdatedDate;

                        var c = unitOfWork.UserProfile.GetByID(cc.CreatedBy);
                        if (c != null)
                        {
                            string ss = c.FirstName + " " + c.LastName;
                            cc.CreatedByName = ss;
                        }
                        Commentsss.Add(cc);
                    }
                }
                return Commentsss;
            }

        }
        public static int CreateComments(DMItemCommentsInput dmItemCommentsInput,int clientId, int Id)
        {
            int id = 0;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var commentsDetails = new DMItemComments();
                    commentsDetails.ItemId = Id;
                    commentsDetails.Comments = dmItemCommentsInput.Comments;
                    commentsDetails.CreatedDate = DateTime.Now;
                    commentsDetails.ClientId = clientId;
                    commentsDetails.CreatedBy = dmItemCommentsInput.CreatedBy;
                    commentsDetails.LastUpdatedDate = DateTime.Now;
                    commentsDetails.UpdatedBy = commentsDetails.CreatedBy;
                    unitOfWork.DMItemComments.Insert(commentsDetails);
                    unitOfWork.Save();
                    id = commentsDetails.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool UpdateComments(DMItemCommentsInput dmItemCommentsInput)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.DMItemComments.GetByID(dmItemCommentsInput.Id);
                    updatetitem.Comments = dmItemCommentsInput.Comments;
                    updatetitem.UpdatedBy = dmItemCommentsInput.UpdatedBy;
                    updatetitem.LastUpdatedDate = DateTime.Now;
                    unitOfWork.DMItemComments.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveCommentss(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.DMItemComments.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }


        //--------------------------------------------Analysis and Solutions-----------------------------------------------//

        public static void saveAnalysis(int DemandId,int clientId, int uid, string EncodeString)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var AnalysisExists = unitOfWork.DMAnalysisRootCause.Get(g => g.DemandId == DemandId).FirstOrDefault();
            if (AnalysisExists != null)
            {
                AnalysisExists.Analysis = EncodeString;
                AnalysisExists.UpdatedBy = uid;
                AnalysisExists.ClientId = clientId;
                AnalysisExists.UpdatedDt = DateTime.Now;
                unitOfWork.DMAnalysisRootCause.Update(AnalysisExists);
            }
            else
            {
                DMAnalysisRootCause inrc = new DMAnalysisRootCause
                {
                    DemandId = DemandId,
                    CreatedBy = uid,
                    Analysis = EncodeString,
                    ClientId=clientId,
                    CreatedDt = DateTime.Now,
                    UpdatedBy = uid,
                    UpdatedDt = DateTime.Now

                };
                unitOfWork.DMAnalysisRootCause.Insert(inrc);
            }
            unitOfWork.Save();
        }

        public static void saveSolution(int DemandId,int clientId, int uid, string EncodeString)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var SolutionExists = unitOfWork.DMSolution.Get(g => g.DemandId == DemandId).FirstOrDefault();
            if (SolutionExists != null)
            {
                SolutionExists.Solution = EncodeString;
                SolutionExists.UpdatedBy = uid;
                SolutionExists.ClientId = clientId;
                SolutionExists.UpdatedDt = DateTime.Now;
                unitOfWork.DMSolution.Update(SolutionExists);
            }
            else
            {
                DMSolution inrc = new DMSolution
                {
                    DemandId = DemandId,
                    CreatedBy = uid,
                    ClientId=clientId,
                    Solution = EncodeString,
                    CreatedDt = DateTime.Now,
                    UpdatedBy = uid,
                    UpdatedDt = DateTime.Now

                };
                unitOfWork.DMSolution.Insert(inrc);
            }
            unitOfWork.Save();
        }

        //--------------------------------------LOGS_---------------------------------------//
        public static IList<DemandPriorityHistoryModel> GetPriorityHistoryDet(int Id)
        {

            IList<DemandPriorityHistoryModel> PriorityHistory = new List<DemandPriorityHistoryModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<DMPriorityHistory> PriorityHistoryDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (Id != 0)
                    PriorityHistoryDetails = unitOfWork.DMPriorityHistory.Get(cid => cid.DemandId == Id).ToList();

                if (PriorityHistoryDetails != null)
                {
                    foreach (var _priorityhistorydetail in PriorityHistoryDetails)
                    {
                        DemandPriorityHistoryModel ph = new DemandPriorityHistoryModel();
                        ph.Id = _priorityhistorydetail.Id;
                        ph.DemandId = _priorityhistorydetail.DemandId;
                        ph.OldPriorityId = _priorityhistorydetail.OldPriorityId;
                        ph.NewPriorityId = _priorityhistorydetail.NewPriorityId;
                        ph.Comments = _priorityhistorydetail.Comments;
                        ph.UpdatedBy = _priorityhistorydetail.UpdatedBy;
                        ph.LastUpdatedDate = _priorityhistorydetail.LastUpdatedDate;
                        var c = unitOfWork.UserProfile.GetByID(ph.UpdatedBy);
                        if (c != null)
                        {
                            string ss = c.FirstName + " " + c.LastName;
                            ph.PostedBy = ss;
                        }


                        PriorityHistory.Add(ph);
                    }
                }
                return PriorityHistory;
            }

        }

        public static IList<DMImpPriorityHistoryModel> GetImpPriorityHistoryDetails(int Id)
        {

            IList<DMImpPriorityHistoryModel> DMPPriorityHistory = new List<DMImpPriorityHistoryModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<DMImpPriorityHistory> DMPPriorityHistoryDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (Id != 0)
                    DMPPriorityHistoryDetails = unitOfWork.DMImpPriorityHistory.Get(cid => cid.DemandId == Id).ToList();

                if (DMPPriorityHistoryDetails != null)
                {
                    foreach (var _imppriorityhistorydetail in DMPPriorityHistoryDetails)
                    {
                        DMImpPriorityHistoryModel iph = new DMImpPriorityHistoryModel();
                        iph.Id = _imppriorityhistorydetail.Id;
                        iph.DemandId = _imppriorityhistorydetail.DemandId;
                        iph.OldImpPriority = _imppriorityhistorydetail.OldImpPriority;
                        iph.NewImpPriority = _imppriorityhistorydetail.NewImpPriority;
                        iph.Comments = _imppriorityhistorydetail.Comments;
                        iph.UpdatedDt = _imppriorityhistorydetail.UpdatedDt;
                        iph.UpdatedBy = _imppriorityhistorydetail.UpdatedBy;
                        var c = unitOfWork.UserProfile.GetByID(iph.UpdatedBy);
                        if (c != null)
                        {
                            string ss = c.FirstName + " " + c.LastName;
                            iph.PostedBy = ss;
                        }
                        DMPPriorityHistory.Add(iph);
                    }
                }
                return DMPPriorityHistory;
            }

        }

        public static IList<DemandStatusHistoryModel> GetStatusHistoryDetails(int Id)
        {
            Db db = new Db();
            var statuslist = (from SH in db.DMStatusHistory
                              join user in db.UserProfiles on SH.UpdatedBy equals user.Id
                              join S in db.IMStatus on SH.OldStatusId equals S.Id into TempStatus
                              join S1 in db.IMStatus on SH.NewStatusId equals S1.Id into TempStatus1
                              where SH.DemandId == Id
                              from old in TempStatus.DefaultIfEmpty()
                              from neew in TempStatus1.DefaultIfEmpty()
                              select new DemandStatusHistoryModel
                              {
                                  OldStatus = old.Status,
                                  NewStatus = neew.Status,
                                  Comments = SH.Comments,
                                  PostedBy = user.FirstName + " " + user.LastName,
                                  LastUpdatedDate = SH.LastUpdatedDate
                              });
            return statuslist.ToList();
        }


        //--------------------------------------Completed And Next Steps---------------------------------------//
        public static List<DMCompletedAndNextSteps> getCompleted(int Demandid, int clientid)
        {
            Db db = new Db();
           
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var impdemand = unitOfWork.DMCompletedAndNextSteps.Get(cid => cid.DemandId == Demandid).ToList();
            return impdemand;

            }

        public static List<int?> getDemandTask(int Demandid, int clientid)
        {
            Db db = new Db();
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var taskid = (from a in db.DMTaskAndActivities 
                          where a.DemandId==Demandid && a.ClientId==clientid
                          where a.DemandId==Demandid && a.ClientId==clientid
                          select a.TaskId).ToList();
            return taskid;
        }

        public static IList<DMTaskAndActivitiesModel> GetTaskAndActivitiesDetails(int clientId,int Id)
        {
            IList<DMTaskAndActivitiesModel> taskAndActivities = new List<DMTaskAndActivitiesModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<DMTaskAndActivities> taskAndActivitiesDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                Db db = new Db();
                if (Id != 0)
                    taskAndActivitiesDetails = unitOfWork.DMTaskAndActivities.Get(cid => cid.DemandId == Id).ToList();

                if (taskAndActivitiesDetails != null)
                {
                    foreach (var _taskAndActivitiesdetail in taskAndActivitiesDetails)
                    {
                        DMTaskAndActivitiesModel cans = new DMTaskAndActivitiesModel();
                        cans.Id = _taskAndActivitiesdetail.Id;
                        cans.DemandId = _taskAndActivitiesdetail.DemandId;
                        cans.TaskId = _taskAndActivitiesdetail.TaskId.ToString();
                        cans.Task = (from a in db.DMTask
                                     where a.Id == _taskAndActivitiesdetail.TaskId && a.ClientId == clientId
                                     select a.DemandTask).FirstOrDefault().ToString();
                        cans.StartDate = _taskAndActivitiesdetail.StartDate;
                        cans.EndDate = _taskAndActivitiesdetail.EndDate;
                        cans.Description = _taskAndActivitiesdetail.Description;
                        cans.AssignedTo = _taskAndActivitiesdetail.AssignedTo;
                        cans.StageId = _taskAndActivitiesdetail.StageId.ToString();
                        var stage= (from a in db.DMStage
                                    where a.Id == _taskAndActivitiesdetail.StageId && a.ClientId == clientId
                                    select a.DemandStage).FirstOrDefault();
                        if(stage!=null)
                        {
                            cans.Stage = stage;

                        }
                        cans.Comments = _taskAndActivitiesdetail.Comments;
                        cans.UpdatedBy = _taskAndActivitiesdetail.UpdatedBy;
                        cans.UpdatedDate = _taskAndActivitiesdetail.UpdatedDate;
                        taskAndActivities.Add(cans);
                    }
                }
                return taskAndActivities;
            }

        }

        public static int CreateTaskAndActivities(DMTaskAndActivitiesModel DmTaskAndActivitiesModel, int clientId, int Id)
        {
            int id = 0;
            try
            {
                 using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var cns = new DMTaskAndActivities();
                    cns.DemandId = Id;
                    cns.TaskId = Convert.ToInt16(DmTaskAndActivitiesModel.TaskId);
                    cns.StartDate = DmTaskAndActivitiesModel.StartDate;
                    cns.EndDate = DmTaskAndActivitiesModel.EndDate;
                    cns.Description = DmTaskAndActivitiesModel.Description;
                    cns.AssignedTo = DmTaskAndActivitiesModel.AssignedTo;
                    cns.StageId = Convert.ToInt32(DmTaskAndActivitiesModel.StageId);
                    cns.Comments = DmTaskAndActivitiesModel.Comments;
                    cns.CreatedBy = DmTaskAndActivitiesModel.CreatedBy;
                    cns.CreatedDate = DateTime.Now ;
                    cns.ClientId = clientId;
                    cns.UserTaskToken = DmTaskAndActivitiesModel.UserTaskToken;
                    unitOfWork.DMTaskAndActivities.Insert(cns);
                    id = cns.Id;


                    DMTaskActivitiesHistory DMTaskActivitiesHistory = new DMTaskActivitiesHistory()
                     {
                             DemandId = Id,
                             ClientId = clientId,
                             TaskId = Convert.ToInt16(DmTaskAndActivitiesModel.TaskId),
                             StageId= Convert.ToInt32(DmTaskAndActivitiesModel.StageId),
                             Comments = DmTaskAndActivitiesModel.Comments,
                             PostedBy= DmTaskAndActivitiesModel.CreatedBy,
                             PostedOn = DateTime.Now,
                             AssignedTo=DmTaskAndActivitiesModel.AssignedTo
                    };
                    unitOfWork.DMTaskActivitiesHistory.Insert(DMTaskActivitiesHistory);
                    unitOfWork.Save();

                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool UpdateTaskAndActivities(DMTaskAndActivitiesModel DmTaskAndActivitiesModel,int clientId)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {

                    Db db = new Db();
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var ExistTaskActivity = (from a in db.DMTaskAndActivities
                                             where a.Id == DmTaskAndActivitiesModel.Id
                                             select a.StageId).FirstOrDefault(); 

                    var updatetitem = unitOfWork.DMTaskAndActivities.GetByID(DmTaskAndActivitiesModel.Id);
                    var existassignedto = updatetitem.AssignedTo;
                    
                    updatetitem.UpdatedDate = DateTime.Now;
                    updatetitem.StartDate = DmTaskAndActivitiesModel.StartDate;
                    updatetitem.EndDate = DmTaskAndActivitiesModel.EndDate;
                    updatetitem.Description = DmTaskAndActivitiesModel.Description;
                    updatetitem.AssignedTo = DmTaskAndActivitiesModel.AssignedTo;
                    updatetitem.StageId = Convert.ToInt32(DmTaskAndActivitiesModel.StageId);
                    updatetitem.Comments = DmTaskAndActivitiesModel.Comments;
                    updatetitem.UpdatedBy = DmTaskAndActivitiesModel.UpdatedBy;
                    updatetitem.UpdatedDate = DmTaskAndActivitiesModel.UpdatedDate;
                    updatetitem.UserTaskToken = DmTaskAndActivitiesModel.UserTaskToken;
                    unitOfWork.DMTaskAndActivities.Update(updatetitem);

                    if(ExistTaskActivity!= Convert.ToInt32(DmTaskAndActivitiesModel.StageId))
                    {
                        DMTaskActivitiesHistory DMTaskActivitiesHistory = new DMTaskActivitiesHistory()
                        {
                            DemandId = DmTaskAndActivitiesModel.DemandId,
                            ClientId = clientId,
                            TaskId = Convert.ToInt16(DmTaskAndActivitiesModel.TaskId),
                            StageId = Convert.ToInt32(DmTaskAndActivitiesModel.StageId),
                            Comments = DmTaskAndActivitiesModel.Comments,
                            PostedBy = DmTaskAndActivitiesModel.UpdatedBy,
                            PostedOn = DateTime.Now,
                            AssignedTo = DmTaskAndActivitiesModel.AssignedTo
                        };
                        unitOfWork.DMTaskActivitiesHistory.Insert(DMTaskActivitiesHistory);
                    }
                    
                    unitOfWork.Save();
                    if (existassignedto == DmTaskAndActivitiesModel.AssignedTo)
                    {
                        return false;
                    }
                    return true ;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveTaskAndActivities(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.DMTaskAndActivities.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }
        public static IList<DMTaskActivitiesHistoryModel> GetTaskActivitiesHistoryDetails(int Id)
        {
            Db db = new Db();
            var tasklist = (from th in db.DMTaskActivitiesHistory
                            join user in db.UserProfiles on th.PostedBy equals user.Id
                            join task in db.DMTask on th.TaskId equals task.Id
                            join stage in db.DMStage on th.StageId equals stage.Id
                            where th.DemandId == Id
                            select new DMTaskActivitiesHistoryModel
                            {
                                  Task = task.DemandTask,
                                  Stage = stage.DemandStage,
                                  Comments = th.Comments,
                                  PostedBy = user.FirstName + " " + user.LastName,
                                  PostedOn = th.PostedOn,
                                  AssignedTo=th.AssignedTo
                            });
            return tasklist.ToList();
        }


        public static List<string> GetImpName(int ImpManager)
        {
            Db db = new Db();
            List<string> list1 = (from imc in db.UserProfiles
                                  where imc.Id == ImpManager
                                  select imc.FirstName + " " + imc.LastName).ToList();

            return list1;
        }

        public static List<SelectListItem> ListOfAdminsSysAdmins(int clientId)
        {
            //Client client = (Client)Session["Client"];
            Db db = new Db();
            var listOfAdmins = (from UA in db.UserApplications
                                join UP in db.UserProfiles on UA.UserId equals UP.Id
                                join ASA in db.AppStore on UA.ApplicationId equals ASA.Id
                                join UR in db.UserRoles on UA.RoleId equals UR.Id
                                where ASA.ApplicationName.ToLower() == "Demand manager" && (UR.RoleName == "admin" || UR.RoleName == "sysadmin") && UA.ClientId == clientId
                                select UP
                                            ).Distinct().ToList();
            var AdminsWithName = listOfAdmins.Select(g =>
                new SelectListItem
                {
                    Text = g.FirstName + " " + g.LastName,
                    Value = g.Id.ToString()
                }
                ).ToList();
            //return Json(new { Result = "OK", Records = AdminsWithName });
            return AdminsWithName;
        }

        //------------------------------Classification and Category (PostDemand)[Tab-2]-----------------------------------------------------//
        public static IList<DMClassification> _getClassification(int Clientid)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<DMClassification>(context);
                return varRepo.GetAll().Where(c => c.ClientId == Clientid).ToList();
            }
        }
        //public static IList<vwDMclasswithcat> GetCatAndClassification(int clientid)
        //{
        //    using (var context = new DbContextFactory())
        //    {
        //        var varRepo = new Repo<vwDMclasswithcat>(context);
        //        return varRepo.Where(w => w.clientid == clientid).ToList();
        //    }
        //}
        public static IList<DMItemCategoryMap> GetMapItemForItemId(int p)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<DMItemCategoryMap>(context);
                return varRepo.Where(c => c.ItemId == p).ToList();
            }
        }

        public static IEnumerable<TreeViewItemModel> GetClassCat(int ClientId, int DemandId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            List<TreeViewItemModel> cc = new List<TreeViewItemModel>();
            //Existing Category for Demands
            var ExistCategoryIds = new List<int>();
            if (DemandId != 0)
            {
                ExistCategoryIds = unitOfWork.DMItemCategoryMap.Get(e => e.ItemId == DemandId).Select(f => f.CategoryId).ToList();
            }

            //getting Classification
            var Class = unitOfWork.DMClassification.Get().Where(r => r.ClientId == ClientId);
            if (Class != null)
            {
                foreach (var C in Class)
                {
                    TreeViewItemModel cls = new TreeViewItemModel();
                    //cls.Id = C.Id.ToString();
                    cls.Text = C.Classification;
                    var Categ = unitOfWork.DMCategory.Get().Where(e => e.ClassificationId == C.Id);
                    if (Categ.Count() != 0)
                    {
                        cls.HasChildren = true;
                        //Inserting Categories
                        List<TreeViewItemModel> category = new List<TreeViewItemModel>();
                        foreach (var ca in Categ)
                        {
                            TreeViewItemModel cate = new TreeViewItemModel();
                            cate.Id = ca.Id.ToString();
                            cate.Text = ca.Category;
                            if (DemandId != 0)
                            {
                                var check = ExistCategoryIds.Contains(ca.Id);
                                if (check)
                                {
                                    cate.Checked = true;
                                    cate.Selected = true;
                                }
                            }

                            category.Add(cate);
                        }
                        cls.Items = category;
                        cls.Expanded = true;

                    }
                    cc.Add(cls);
                }
            }

            return cc;
        }
        //Get ConfigType, ConfigItem
        public static IEnumerable<TreeViewItemModel> GetConfigType(int ClientId, int DemandId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            List<TreeViewItemModel> cc = new List<TreeViewItemModel>();
            //Existing ConfigItems of Demands
            var ExistConfigItems = new List<int>();
            if (DemandId != 0)
            {
                ExistConfigItems = unitOfWork.DMConfigItemsMap.Get(e => e.DemandId == DemandId).Select(f => f.ConfigItemId).ToList();
            }

            //getting ConFigType
            var CType = unitOfWork.DMConfigType.Get().Where(r => r.ClientId == ClientId);
            if (CType != null)
            {
                foreach (var C in CType)
                {
                    TreeViewItemModel ctype = new TreeViewItemModel();
                    //ctype.Id = C.Id.ToString();
                    ctype.Text = C.ConfigType;
                    var configItems = unitOfWork.DMConfigItem.Get().Where(e => e.ConfigTypeId == C.Id);
                    if (configItems.Count() != 0)
                    {
                        ctype.HasChildren = true;
                        //Inserting Categories
                        List<TreeViewItemModel> ConfigIt = new List<TreeViewItemModel>();
                        foreach (var Ci in configItems)
                        {
                            TreeViewItemModel ConfItem = new TreeViewItemModel();
                            ConfItem.Id = Ci.Id.ToString();
                            ConfItem.Text = Ci.ConfigItem;
                            if (DemandId != 0)
                            {
                                var check = ExistConfigItems.Contains(Ci.Id);
                                if (check)
                                {
                                    ConfItem.Checked = true;
                                    ConfItem.Selected = true;
                                }
                            }

                            ConfigIt.Add(ConfItem);
                        }
                        ctype.Items = ConfigIt;
                        ctype.Expanded = true;

                    }
                    cc.Add(ctype);
                }

            }
            return cc;
        }

        //helper method to get list of users with Implementation Manager Role
        public static IEnumerable<SelectListItem> GetimpManagers(int clientid)
        {
            Db db = new Db();
            var listOfAdmins = (from UA in db.UserApplications
                                join UP in db.UserProfiles on UA.UserId equals UP.Id
                                join ASA in db.AppStore on UA.ApplicationId equals ASA.Id
                                join UR in db.UserRoles on UA.RoleId equals UR.Id
                                where ASA.ApplicationName.ToLower() == "Demand manager" && (UR.RoleName == "implementationmanager" || UR.RoleName == "admin") && UA.ClientId == clientid
                                select UP).Distinct().OrderBy(x => x.FirstName).ToList();

            var AdminsWithName = listOfAdmins.Select(g =>
                new SelectListItem
                {
                    Text = g.FirstName + " " + g.LastName,
                    Value = g.Id.ToString()
                }).ToList();

            return AdminsWithName;
        }

      
        

        //--- Classification
        public static void AddClassificationCategory(int DemandId, int clientId,string CateIds, int uid)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            string[] SelectedCateIds = CateIds.Split(',');
            foreach (var catId in SelectedCateIds)
            {
                int temp;
                var IsNumber = int.TryParse(catId, out temp);
                if (IsNumber == true)
                {
                    DMItemCategoryMap CatMap = new DMItemCategoryMap();
                    CatMap.CategoryId = Convert.ToInt32(catId);
                    CatMap.ItemId = DemandId;
                    CatMap.ClientId = clientId;
                    CatMap.CreatedDate = DateTime.Now;
                    CatMap.LastUpdatedDate = DateTime.Now;
                    CatMap.UpdatedBy = uid;
                    unitOfWork.DMItemCategoryMap.Insert(CatMap);
                    unitOfWork.Save();
                }
            }
           
        }

        public static void DeleteClassificationCategory(int DemandId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var list = unitOfWork.DMItemCategoryMap.Get(e => e.ItemId == DemandId);
            if (list != null)
            {
                foreach (var cateory in list)
                {
                    unitOfWork.DMItemCategoryMap.Delete(cateory.Id);
                }
                unitOfWork.Save();
            }
        }


        //--Configuration
        public static void AddConfigItem(int DemandId, int clientId,string ConfigIds, int uid)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            string[] SelectedConfigIds = ConfigIds.Split(',');
            foreach (var configId in SelectedConfigIds)
            {
                int temp;
                var IsNumber = int.TryParse(configId, out temp);
                if (IsNumber == true)
                {
                    DMConfigItemsMap ConfigMap = new DMConfigItemsMap();
                    //ConfigMap.ConfigTypeId = Convert.ToInt32(configId);
                    ConfigMap.ConfigItemId = Convert.ToInt32(configId);
                    ConfigMap.DemandId = DemandId;
                    ConfigMap.ClientId = clientId;
                    ConfigMap.CreatedDt = DateTime.Now;
                    ConfigMap.UpdatedDt = DateTime.Now;
                    ConfigMap.CreatedBy = uid;
                    ConfigMap.UpdatedBy = uid;
                    unitOfWork.DMConfigItemsMap.Insert(ConfigMap);
                }
            }
            unitOfWork.Save();
        }

        public static void DeleteConfigItem(int DemandId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var list = unitOfWork.DMConfigItemsMap.Get(e => e.DemandId == DemandId);
            if (list != null)
            {
                foreach (var config in list)
                {
                    unitOfWork.DMConfigItemsMap.Delete(config.Id);
                }
                unitOfWork.Save();
            }
        }

        //TogetuserName
        public static string GetUserName(int uid)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var b = unitOfWork.UserProfile.GetByID(uid);
            var name = b.FirstName + " " + b.LastName;
            return name;
        }

        public static string[] getApprovers(int DemandId)
        {
            Db db = new Db();
            var list = (from UP in db.UserProfiles
                        join IMAP in db.DMDemandApprover on UP.Id equals IMAP.ApproverId
                        where IMAP.DemandId == DemandId
                        select UP.FirstName + " " + UP.LastName
                ).DefaultIfEmpty().ToArray();
            return list;
        }

        //-------------Mail--------------



        /// <summary>
        /// To change Demand Status to Revised when any data is modified.
        /// </summary>
        /// <param name="DemandId"></param>
        /// <param name="uid">UserId</param>
        public static void Demand_Revised(int DemandId,int clientId, int uid)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var ExistDemand = unitOfWork.DMPostDemand.GetByID(DemandId);
            if (ExistDemand != null)
            {
                if (ExistDemand.StatusId == (int)DMStatus.SubmitForRevision)
                {
                    ExistDemand.StatusId = (int)DMStatus.Revised;
                    ExistDemand.StatusDt = DateTime.Now;
                    ExistDemand.LastUpdatedDate = DateTime.Now;
                    ExistDemand.LastReviewDt = DateTime.Now;

                    ExistDemand.UpdatedBy = uid;
                    unitOfWork.DMPostDemand.Update(ExistDemand);

                    DMStatusHistory DMStatusHistory = new DMStatusHistory()
                    {
                        DemandId = DemandId,
                        OldStatusId = (int)DMStatus.SubmitForRevision,
                        NewStatusId = (int)DMStatus.Revised,
                        IsDeleted = false,
                        ClientId=clientId,
                        CreatedBy = uid,
                        UpdatedBy = uid,
                        LastUpdatedDate = DateTime.Now,
                        CreatedDate = DateTime.Now
                    };
                    unitOfWork.DMStatusHistory.Insert(DMStatusHistory);
                    unitOfWork.Save();
                }
            }
        }

        //For Exporting ---Server---
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //Removing the DemandId Column
            dataTable.Columns.Remove("DemandId");

            //put a breakpoint here and check datatable
            return dataTable;
        }



        public static DataSet exportDataToDataSet(int ClientId, int UserId, bool IsAdmin, int AllDemands = 1)
        {
            DataSet Ds = new DataSet();
            //My Demands
            if (AllDemands == 1)
            {
                //My Demands

                //Pre- Approval
                var preApprovalList = PreApproval(ClientId, UserId).ToList();
                var dt1 = ToDataTable(preApprovalList);
                Ds.Tables.Add(dt1);

                //Submitted for Review
                var submittedReviewList = SubmitForReview(ClientId, UserId, IsAdmin).ToList();
                var dt2 = ToDataTable(submittedReviewList);
                Ds.Tables.Add(dt2);

                //Approve/ Decline/ Cancel
                var ApproveDeclineList = Approval(ClientId, UserId, IsAdmin).ToList();
                var dt3 = ToDataTable(ApproveDeclineList);
                Ds.Tables.Add(dt3);

                //Implementation 
                var ImplementationList = Implementation(ClientId, UserId, IsAdmin).ToList();
                var dt4 = ToDataTable(ImplementationList);
                Ds.Tables.Add(dt4);

                //Completed Demands
                var CompletedList = Completion(ClientId, UserId, IsAdmin).ToList();
                var dt5 = ToDataTable(CompletedList);
                Ds.Tables.Add(dt5);


                Ds.Tables[0].TableName = "Pre-Approval Demands";
                Ds.Tables[1].TableName = "Submitted For Review Demands";
                Ds.Tables[2].TableName = "Approve-Decline-Cancelled Demands";
                Ds.Tables[3].TableName = "Under Implementation Demands ";
                Ds.Tables[4].TableName = "Completed Demands";
                return Ds;
            }
            //else - All Demands
            else
            {
                //All Demands

                //Pre- Approval
                var preApprovalList = PreApproval(ClientId, false).ToList();
                var dt1 = ToDataTable(preApprovalList);
                Ds.Tables.Add(dt1);

                //Submitted for Review
                var submittedReviewList = SubmitForReview(ClientId, false).ToList();
                var dt2 = ToDataTable(submittedReviewList);
                Ds.Tables.Add(dt2);

                //Approve/ Decline/ Cancel
                var ApproveDeclineList = Approval(ClientId, false).ToList();
                var dt3 = ToDataTable(ApproveDeclineList);
                Ds.Tables.Add(dt3);

                //Implementation 
                var ImplementationList = Implementation(ClientId, false).ToList();
                var dt4 = ToDataTable(ImplementationList);
                Ds.Tables.Add(dt4);

                //Completed Demands
                var CompletedList = Completion(ClientId, false).ToList();
                var dt5 = ToDataTable(CompletedList);
                Ds.Tables.Add(dt5);

                Ds.Tables[0].TableName = "Pre-Approval Demands";
                Ds.Tables[1].TableName = "Submitted For Review Demands";
                Ds.Tables[2].TableName = "Approve-Decline-Cancelled Demands";
                Ds.Tables[3].TableName = "Under Implementation Demands ";
                Ds.Tables[4].TableName = "Completed Demands";

                return Ds;
            }
        }


        public static IEnumerable<SelectListItem> getLOB(int ClientID)
        {
            //var ClientID = (Client)Session["Client"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.DMLOB.Get(x => x.ClientId == ClientID).ToList();
            var item = applicationList.OrderBy(x => x.Id).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.DemandLOB.ToString() });
            return item;

        }

        public static List<DemandApproverModel> getApproversForView(int DemandId)
        {
            Db db = new Db();
            var list = (from UP in db.UserProfiles
                        join IMAP in db.DMDemandApprover on UP.Id equals IMAP.ApproverId
                        where IMAP.DemandId == DemandId
                        select new
                        {
                            fName = UP.FirstName,
                            lName = UP.LastName,
                            Date = IMAP.ApprovedDate
                        }
                ).ToList();
            //.DefaultIfEmpty()
            var asd = list.Select(v => new DemandApproverModel
            {
                FirstName = v.fName,
                LastName = v.lName,
                ApprovedDate = v.Date
            }).ToList();
            return asd;
        }

        //-----------------------------------Demand Authorize---------------------------------//
        /// <summary>
        /// To authorize the Demand part of Logged in Client in Session
        /// </summary>
        /// <param name="ClientID"></param>
        /// <param name="DemandID"></param>
        /// <returns></returns>
        public static bool DemandAuthorize(int ClientID, int DemandID)
        {
            bool authorize = false;
            if (DemandID != 0)
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    authorize = unitOfWork.DMPostDemand.Get().Where(f => f.ClientId == ClientID && f.Id == DemandID).Any();
                }

                return authorize;
            }
            else
            {
                return false;
            }
        }

        //---------------------------------Demands classification-------------------------------//
        public static List<DemandViewModel> GetDetailClassification(int DemandId)
        {
            Db db = new Db();
            var list = (from a in db.DMItemCategoryMap
                        join b in db.DMCategory on a.CategoryId equals b.Id
                        join c in db.DMClassification
                        on b.ClassificationId equals c.Id
                        where a.ItemId == DemandId
                        select new
                        {
                            c.Classification,
                            b.Category
                        }).ToList();
            var q = list.Select(v => new DemandViewModel
            {
                Classification = v.Classification,
                category = v.Category
            }).ToList();

            return q;
        }


        public static List<DemandViewModel> GetDetailConfigItem(int DemandId)
        {
            Db db = new Db();

            var list = (from a in db.DMConfigItemsMap
                    join b in db.DMConfigItem on a.ConfigItemId equals b.Id
                    join c in db.DMConfigType on b.ConfigTypeId equals c.Id
                    where a.DemandId == DemandId
                        select new
                    {
                        c.ConfigType,
                        b.ConfigItem
                    }).ToList();
            var q = list.Select(v => new DemandViewModel
            {
                ConfigItem = v.ConfigItem,
                ConfigType = v.ConfigType
            }).ToList();

            return q;
        }
        
        public static IEnumerable<SelectListItem> GetListOfStage(int clientid)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var tempObj = unitOfWork.DMStage.Get(a=>a.ClientId==clientid);
            var returnObj = tempObj.Select(f => new SelectListItem
            {
                Text = f.DemandStage,
                Value = f.Id.ToString()
            }).Distinct();
            return returnObj;
            //Db db = new Db();
            //var listOfStages = (from a in db.DMStage
            //                    where a.ClientId == clientid
            //                    select new
            //                    {
            //                       StageId = a.Id,
            //                       Stage = a.DemandStage
            //                    }).ToList();

            //var StageList = listOfStages.Select(g =>
            //    new SelectListItem
            //    {
            //        Text = g.Stage.ToString(),
            //        Value = g.StageId.ToString()
            //    }).ToList();

            //return StageList;
        }
        public static IEnumerable<SelectListItem> GetListOfTask(int clientid)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var tempObj = unitOfWork.DMTask.Get(a => a.ClientId == clientid);
            var returnObj = tempObj.Select(f => new SelectListItem
            {
                Text = f.DemandTask,
                Value = f.Id.ToString()
            }).Distinct();
            return returnObj;

            //Db db = new Db();
            //var listOfTask = (from a in db.DMTask where a.ClientId==clientid
            //                  select new
            //                    {
            //                        TaskId = a.Id,
            //                        Task = a.DemandTask
            //                    }).ToList();

            //var TaskList = listOfTask.Select(g =>
            //    new SelectListItem
            //    {

            //        Text = g.Task.ToString(),
            //        Value = g.TaskId.ToString()
            //    }).ToList();

            //return TaskList;
        }
        public static string getTaskName(int id)
        {
            Db db = new Db();
            var task = (from a in db.DMTask
                        where a.Id == id
                        select a.DemandTask).FirstOrDefault().ToString();
            return task;
        }
        public static string getStageName(int id)
        {
            Db db = new Db();
            var stage = (from a in db.DMStage
                        where a.Id == id
                        select a.DemandStage).FirstOrDefault().ToString();
            return stage;
        }
        
        public static IEnumerable<SelectListItem> getComplexity(int clientId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var tempObj = unitOfWork.DMComplexity.Get(a => a.ClientId == clientId);
            var returnObj = tempObj.Select(f => new SelectListItem
            {
                Text = f.Complexity,
                Value = f.Id.ToString()
            }).Distinct();
            return returnObj;
        }
        public static IEnumerable<SelectListItem> getDocumentType(int clientId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var tempObj = unitOfWork.DMDocumentType.Get(a => a.ClientId == clientId);
            var returnObj = tempObj.Select(f => new SelectListItem
            {
                Text = f.DocumentType,
                Value = f.Id.ToString()
            }).Distinct();
            return returnObj;
        }
    }
}