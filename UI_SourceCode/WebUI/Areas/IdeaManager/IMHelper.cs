using Data;
using Domain.Entities;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Areas.IdeaManager.Dto;
using WebUI.Areas.IdeaManager.Models;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using System.Web.Configuration;
using System.Net.Mail;
using WebUI.Areas.IdeaManager.Controllers;
using System.Security.Policy;
using System.Data;
using System.Reflection;
using System.Net;
using WebUI.App_Start;
using WebUI.Dto;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities.A3MM;

namespace WebUI.Areas.IdeaManager
{
    public class IMHelper
    {


        /// <summary>
        /// View All Ideas
        /// </summary>
        /// <param name="ClientId"> Display All Ideas</param>
        /// <param name="IsCusUser">Display ideas where InternalInitiative is Unchecked.</param>
        /// <returns>IEnumerable PreApprovalModel</returns>
        public static IEnumerable<PreApprovalModel> PreApproval(int ClientId, bool IsCusUser)
        {
            Db db = new Db();
            //True- Display ideas where InternalInitiative is Unchecked.
            if (IsCusUser)
            {
                var Idealist = (from im in db.IMPostIdea
                                join UP in db.UserProfiles on im.CreatedBy equals UP.Id
                                join T in db.IMTeam on im.TeamId equals T.Id into TempTeam
                                join S in db.IMStatus on im.StatusId equals S.Id into TempStatus
                                where im.ClientId == ClientId && (im.StatusId == (int)IMStatus.SubmitForRevision || im.StatusId == (int)IMStatus.Revised || im.StatusId == (int)IMStatus.Created)
                                && im.InternalInitiative == false && im.IsDeleted == false
                                from team in TempTeam.DefaultIfEmpty()
                                from status in TempStatus.DefaultIfEmpty()
                                select new
                                {
                                    IdeaId = im.Id,
                                    IdeaNumber = im.IdeaNumber,
                                    IdeaName = im.ItemName,
                                    InternalInitiative = im.InternalInitiative,
                                    Team = team.Team,
                                    Status = status.Status,
                                    CreatedBy = UP.FirstName + " " + UP.LastName,
                                    CreatedDt = im.CreatedDate,
                                    StatusChange = im.StatusDt,
                                    LastModified = im.LastUpdatedDate,
                                    ProposedBy = im.ProposedBy
                                }).Distinct().ToList();
                var IdeaListTomodel = Idealist.Select(e => new PreApprovalModel
                {
                    IdeaId = e.IdeaId,
                    IdeaNumber = e.IdeaNumber,
                    IdeaName = e.IdeaName,
                    InternalInitiative = e.InternalInitiative,
                    Team = e.Team,
                    Status = e.Status,
                    CreatedBy = e.ProposedBy != null ? e.CreatedBy + "/ " + e.ProposedBy : e.CreatedBy,
                    CreatedDt = e.CreatedDt,
                    StatusChangeDt = e.StatusChange,
                    LastModifiedDt = e.LastModified
                });

                return IdeaListTomodel;
            }
            //Display All Ideas
            else
            {
                var Idealist = (from im in db.IMPostIdea
                                join UP in db.UserProfiles on im.CreatedBy equals UP.Id
                                join T in db.IMTeam on im.TeamId equals T.Id into TempTeam
                                join S in db.IMStatus on im.StatusId equals S.Id into TempStatus
                                where im.ClientId == ClientId && (im.StatusId == (int)IMStatus.SubmitForRevision || im.StatusId == (int)IMStatus.Revised || im.StatusId == (int)IMStatus.Created)
                                        && im.IsDeleted == false
                                from team in TempTeam.DefaultIfEmpty()
                                from status in TempStatus.DefaultIfEmpty()
                                select new
                                {
                                    IdeaId = im.Id,
                                    IdeaNumber = im.IdeaNumber,
                                    IdeaName = im.ItemName,
                                    InternalInitiative = im.InternalInitiative,
                                    Team = team.Team,
                                    Status = status.Status,
                                    CreatedBy = UP.FirstName + " " + UP.LastName,
                                    CreatedDt = im.CreatedDate,
                                    StatusChange = im.StatusDt,
                                    LastModified = im.LastUpdatedDate,
                                    ProposedBy = im.ProposedBy
                                }).Distinct().ToList();
                var IdeaListTomodel = Idealist.Select(e => new PreApprovalModel
                {
                    IdeaId = e.IdeaId,
                    IdeaNumber = e.IdeaNumber,
                    IdeaName = e.IdeaName,
                    InternalInitiative = e.InternalInitiative,
                    Team = e.Team,
                    Status = e.Status,
                    CreatedBy = e.ProposedBy != null ? e.CreatedBy + "/ " + e.ProposedBy : e.CreatedBy,
                    CreatedDt = e.CreatedDt,
                    StatusChangeDt = e.StatusChange,
                    LastModifiedDt = e.LastModified
                });

                return IdeaListTomodel;
            }

        }

        /// <summary>
        /// View My Ideas
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="UserId">Display Ideas For Creator</param>
        /// <returns>IEnumerable PreApprovalModel </returns>
        public static IEnumerable<PreApprovalModel> PreApproval(int ClientId, int UserId)
        {
            Db db = new Db();
            var Idealist = (from im in db.IMPostIdea
                            join UP in db.UserProfiles on im.CreatedBy equals UP.Id
                            join T in db.IMTeam on im.TeamId equals T.Id into TempTeam
                            join S in db.IMStatus on im.StatusId equals S.Id into TempStatus
                            where im.ClientId == ClientId && im.CreatedBy == UserId && (im.StatusId == (int)IMStatus.SubmitForRevision || im.StatusId == (int)IMStatus.Revised || im.StatusId == (int)IMStatus.Created)
                                 && im.IsDeleted == false
                            from team in TempTeam.DefaultIfEmpty()
                            from status in TempStatus.DefaultIfEmpty()
                            select new
                            {
                                IdeaId = im.Id,
                                IdeaNumber = im.IdeaNumber,
                                IdeaName = im.ItemName,
                                InternalInitiative = im.InternalInitiative,
                                Team = team.Team,
                                Status = status.Status,
                                CreatedBy = UP.FirstName + " " + UP.LastName,
                                CreatedDt = im.CreatedDate,
                                StatusChange = im.StatusDt,
                                LastModified = im.LastUpdatedDate,
                                ProposedBy = im.ProposedBy
                            }).ToList();
            var IdeaListTomodel = Idealist.Select(e => new PreApprovalModel
            {
                IdeaId = e.IdeaId,
                IdeaNumber = e.IdeaNumber,
                IdeaName = e.IdeaName,
                InternalInitiative = e.InternalInitiative,
                Team = e.Team,
                Status = e.Status,
                CreatedBy = e.ProposedBy != null ? e.CreatedBy + "/ " + e.ProposedBy : e.CreatedBy,
                CreatedDt = e.CreatedDt,
                StatusChangeDt = e.StatusChange,
                LastModifiedDt = e.LastModified
            });

            return IdeaListTomodel;
        }


        /// <summary>
        /// View All SubmittedForReview Ideas
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="IsCusUser">Display ideas where InternalInitiative is Unchecked.</param>
        /// <returns></returns>
        public static IEnumerable<SubmitForReviewModel> SubmitForReview(int ClientId, bool IsCusUser)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();
            //True- Display ideas where InternalInitiative is Unchecked.
            if (IsCusUser)
            {
                var Idealist = (from im in db.IMPostIdea
                                join UP in db.UserProfiles on im.CreatedBy equals UP.Id
                                //join IS in db.IMIdeaSource on im.IdeaSourceId equals IS.Id
                                join T in db.IMTeam on im.TeamId equals T.Id into TempTeam
                                join S in db.IMStatus on im.StatusId equals S.Id into TempStatus
                                join lob in db.IMLOBSource on im.LOBId equals lob.Id into TempLob
                                join Com in db.IMComplexity on im.ComplexityId equals Com.Id into TempComplexity
                                where im.ClientId == ClientId && (im.StatusId == (int)IMStatus.SubmitForReview || im.StatusId == (int)IMStatus.SubmitForApproval)
                                    && im.InternalInitiative == false && im.IsDeleted == false
                                from team in TempTeam.DefaultIfEmpty()
                                from status in TempStatus.DefaultIfEmpty()
                                from Complexity in TempComplexity.DefaultIfEmpty()
                                from Lob in TempLob.DefaultIfEmpty()
                                select new
                                {
                                    IdeaId = im.Id,
                                    IdeaName = im.ItemName,
                                    IdeaNumber = im.IdeaNumber,
                                    IdeaSource = im.IdeaSource,
                                    InternalInitiative = im.InternalInitiative,
                                    Team = team.Team,
                                    LOB = Lob.LOB,
                                    Status = status.Status,
                                    /*Priority-
                                      PrevPriority-     */
                                    complexity = Complexity.Complexity,
                                    value = im.QtyFication,
                                    CreatedBy = UP.FirstName + " " + UP.LastName,
                                    //Assigned Approvers -
                                    CreatedDt = im.CreatedDate,
                                    StatusChange = im.StatusDt,
                                    LastModified = im.LastUpdatedDate,
                                    ProposedBy = im.ProposedBy
                                }).Distinct().ToList();
                var IdeaListTomodel = Idealist.Select(e => new SubmitForReviewModel
                {
                    IdeaId = e.IdeaId,
                    IdeaName = e.IdeaName,
                    IdeaNumber = e.IdeaNumber,
                    IdeaSource = e.IdeaSource,
                    InternalInitiative = e.InternalInitiative,
                    Team = e.Team,
                    LOB = e.LOB,
                    Status = e.Status,
                    Priority = "",
                    PrevPriority = "",
                    Complexity = e.complexity,
                    Value = e.value.ToString(),
                    CreatedBy = e.ProposedBy != null ? e.CreatedBy + "/ " + e.ProposedBy : e.CreatedBy,
                    AssignedApprover = "",
                    CreatedDt = e.CreatedDt,
                    StatusChangeDt = e.StatusChange,
                    LastModifiedDt = e.LastModified
                }).ToList();

                //To get Priority and Approvers
                foreach (var i in IdeaListTomodel)
                {
                    var approversList = unitOfWork.IMIdeaApprover.Get().Where(f => f.IdeaId == i.IdeaId).ToList();
                    if (approversList != null)
                    {
                        if (approversList.Where(e => e.ApprovedDate != null).Any())
                            i.Status = "Pending for Approval";
                    }
                    var approvers = approversList.Select(f => f.ApproverId).ToList();
                    var priority = unitOfWork.IMIdeaPriorityHistory.Get().Where(f => f.IdeaId == i.IdeaId).OrderByDescending(r => r.Id).FirstOrDefault();
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
                return IdeaListTomodel;
            }
            else
            {
                var Idealist = (from im in db.IMPostIdea
                                join UP in db.UserProfiles on im.CreatedBy equals UP.Id
                                //join IS in db.IMIdeaSource on im.IdeaSourceId equals IS.Id
                                join T in db.IMTeam on im.TeamId equals T.Id into TempTeam
                                join S in db.IMStatus on im.StatusId equals S.Id into TempStatus
                                join lob in db.IMLOBSource on im.LOBId equals lob.Id into TempLob
                                join Com in db.IMComplexity on im.ComplexityId equals Com.Id into TempComplexity
                                where im.ClientId == ClientId && (im.StatusId == (int)IMStatus.SubmitForReview || im.StatusId == (int)IMStatus.SubmitForApproval)
                                     && im.IsDeleted == false
                                from team in TempTeam.DefaultIfEmpty()
                                from status in TempStatus.DefaultIfEmpty()
                                from Complexity in TempComplexity.DefaultIfEmpty()
                                from Lob in TempLob.DefaultIfEmpty()
                                select new
                                {
                                    IdeaId = im.Id,
                                    IdeaName = im.ItemName,
                                    IdeaNumber = im.IdeaNumber,
                                    IdeaSource = im.IdeaSource,
                                    InternalInitiative = im.InternalInitiative,
                                    Team = team.Team,
                                    LOB = Lob.LOB,
                                    Status = status.Status,
                                    /*Priority-
                                      PrevPriority-     */
                                    complexity = Complexity.Complexity,
                                    value = im.QtyFication,
                                    CreatedBy = UP.FirstName + " " + UP.LastName,
                                    //Assigned Approvers -
                                    CreatedDt = im.CreatedDate,
                                    StatusChange = im.StatusDt,
                                    LastModified = im.LastUpdatedDate,
                                    ProposedBy = im.ProposedBy
                                }).Distinct().ToList();
                var IdeaListTomodel = Idealist.Select(e => new SubmitForReviewModel
                {
                    IdeaId = e.IdeaId,
                    IdeaName = e.IdeaName,
                    IdeaNumber = e.IdeaNumber,
                    IdeaSource = e.IdeaSource,
                    InternalInitiative = e.InternalInitiative,
                    Team = e.Team,
                    LOB = e.LOB,
                    Status = e.Status,
                    Priority = "",
                    PrevPriority = "",
                    Complexity = e.complexity,
                    Value = e.value.ToString(),
                    CreatedBy = e.ProposedBy != null ? e.CreatedBy + "/ " + e.ProposedBy : e.CreatedBy,
                    AssignedApprover = "",
                    CreatedDt = e.CreatedDt,
                    StatusChangeDt = e.StatusChange,
                    LastModifiedDt = e.LastModified
                }).ToList();

                //To get Priority and Approvers
                foreach (var i in IdeaListTomodel)
                {
                    var approversList = unitOfWork.IMIdeaApprover.Get().Where(f => f.IdeaId == i.IdeaId).ToList();
                    if (approversList != null)
                    {
                        if (approversList.Where(e => e.ApprovedDate != null).Any())
                            i.Status = "Pending for Approval";
                    }
                    var approvers = approversList.Select(f => f.ApproverId).ToList();
                    var priority = unitOfWork.IMIdeaPriorityHistory.Get().Where(f => f.IdeaId == i.IdeaId).OrderByDescending(r => r.Id).FirstOrDefault();
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
                return IdeaListTomodel;
            }

        }


        /// <summary>
        /// View my Ideas
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="UserId"></param>
        /// <param name="IsAdmin"> If YES : DisplayIdeas for Approver, Else : Display Ideas for Creator. </param>
        /// <returns></returns>
        public static IEnumerable<SubmitForReviewModel> SubmitForReview(int ClientId, int UserId, bool IsAdmin)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();
            //If Loggedin User is Approver [True]
            if (IsAdmin)
            {
                var Idealist = (from im in db.IMPostIdea
                                join UP in db.UserProfiles on im.CreatedBy equals UP.Id
                                //join IS in db.IMIdeaSource on im.IdeaSourceId equals IS.Id
                                join IMApp in db.IMIdeaApprover on im.Id equals IMApp.IdeaId
                                join T in db.IMTeam on im.TeamId equals T.Id into TempTeam
                                join S in db.IMStatus on im.StatusId equals S.Id into TempStatus
                                join lob in db.IMLOBSource on im.LOBId equals lob.Id into TempLob
                                join Com in db.IMComplexity on im.ComplexityId equals Com.Id into TempComplexity
                                where im.ClientId == ClientId && (IMApp.ApproverId == UserId || im.CreatedBy == UserId) && (im.StatusId == (int)IMStatus.SubmitForReview || im.StatusId == (int)IMStatus.SubmitForApproval)
                                     && im.IsDeleted == false
                                from team in TempTeam.DefaultIfEmpty()
                                from status in TempStatus.DefaultIfEmpty()
                                from Complexity in TempComplexity.DefaultIfEmpty()
                                from Lob in TempLob.DefaultIfEmpty()
                                select new
                                {
                                    IdeaId = im.Id,
                                    IdeaName = im.ItemName,
                                    IdeaNumber = im.IdeaNumber,
                                    IdeaSource = im.IdeaSource,
                                    InternalInitiative = im.InternalInitiative,
                                    Team = team.Team,
                                    LOB = Lob.LOB,
                                    Status = status.Status,
                                    /*Priority-
                                      PrevPriority-     */
                                    complexity = Complexity.Complexity,
                                    value = im.QtyFication,
                                    CreatedBy = UP.FirstName + " " + UP.LastName,
                                    //Assigned Approvers -
                                    CreatedDt = im.CreatedDate,
                                    StatusChange = im.StatusDt,
                                    LastModified = im.LastUpdatedDate,
                                    ProposedBy = im.ProposedBy
                                }).Distinct().ToList();
                var IdeaListTomodel = Idealist.Select(e => new SubmitForReviewModel
                {
                    IdeaId = e.IdeaId,
                    IdeaName = e.IdeaName,
                    IdeaNumber = e.IdeaNumber,
                    IdeaSource = e.IdeaSource,
                    InternalInitiative = e.InternalInitiative,
                    Team = e.Team,
                    LOB = e.LOB,
                    Status = e.Status,
                    Priority = "",
                    PrevPriority = "",
                    Complexity = e.complexity,
                    Value = e.value.ToString(),
                    CreatedBy = e.ProposedBy != null ? e.CreatedBy + "/ " + e.ProposedBy : e.CreatedBy,
                    AssignedApprover = "",
                    CreatedDt = e.CreatedDt,
                    StatusChangeDt = e.StatusChange,
                    LastModifiedDt = e.LastModified
                }).ToList();

                //To get Priority and Approvers
                foreach (var i in IdeaListTomodel)
                {
                    var approversList = unitOfWork.IMIdeaApprover.Get().Where(f => f.IdeaId == i.IdeaId).ToList();
                    if (approversList != null)
                    {
                        if (approversList.Where(e => e.ApprovedDate != null).Any())
                            i.Status = "Pending for Approval";
                    }
                    var approvers = approversList.Select(f => f.ApproverId).ToList();
                    var priority = unitOfWork.IMIdeaPriorityHistory.Get().Where(f => f.IdeaId == i.IdeaId).OrderByDescending(r => r.Id).FirstOrDefault();
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
                return IdeaListTomodel;
            }
            //No- Normal User  [False]
            else
            {
                var Idealist = (from im in db.IMPostIdea
                                join UP in db.UserProfiles on im.CreatedBy equals UP.Id
                                //join IS in db.IMIdeaSource on im.IdeaSourceId equals IS.Id
                                join T in db.IMTeam on im.TeamId equals T.Id into TempTeam
                                join S in db.IMStatus on im.StatusId equals S.Id into TempStatus
                                join lob in db.IMLOBSource on im.LOBId equals lob.Id into TempLob
                                join Com in db.IMComplexity on im.ComplexityId equals Com.Id into TempComplexity
                                where im.ClientId == ClientId && im.CreatedBy == UserId && (im.StatusId == (int)IMStatus.SubmitForReview || im.StatusId == (int)IMStatus.SubmitForApproval)
                                     && im.IsDeleted == false
                                from team in TempTeam.DefaultIfEmpty()
                                from status in TempStatus.DefaultIfEmpty()
                                from Complexity in TempComplexity.DefaultIfEmpty()
                                from Lob in TempLob.DefaultIfEmpty()
                                select new
                                {
                                    IdeaId = im.Id,
                                    IdeaName = im.ItemName,
                                    IdeaNumber = im.IdeaNumber,
                                    IdeaSource = im.IdeaSource,
                                    InternalInitiative = im.InternalInitiative,
                                    Team = team.Team,
                                    LOB = Lob.LOB,
                                    Status = status.Status,
                                    /*Priority-
                                      PrevPriority-     */
                                    complexity = Complexity.Complexity,
                                    value = im.QtyFication,
                                    CreatedBy = UP.FirstName + " " + UP.LastName,
                                    //Assigned Approvers -
                                    CreatedDt = im.CreatedDate,
                                    StatusChange = im.StatusDt,
                                    LastModified = im.LastUpdatedDate,
                                    ProposedBy = im.ProposedBy
                                }).Distinct().ToList();
                var IdeaListTomodel = Idealist.Select(e => new SubmitForReviewModel
                {
                    IdeaId = e.IdeaId,
                    IdeaName = e.IdeaName,
                    IdeaNumber = e.IdeaNumber,
                    IdeaSource = e.IdeaSource,
                    InternalInitiative = e.InternalInitiative,
                    Team = e.Team,
                    LOB = e.LOB,
                    Status = e.Status,
                    Priority = "",
                    PrevPriority = "",
                    Complexity = e.complexity,
                    Value = e.value.ToString(),
                    CreatedBy = e.ProposedBy != null ? e.CreatedBy + "/ " + e.ProposedBy : e.CreatedBy,
                    AssignedApprover = "",
                    CreatedDt = e.CreatedDt,
                    StatusChangeDt = e.StatusChange,
                    LastModifiedDt = e.LastModified
                }).ToList();

                //To get Priority and Approvers
                foreach (var i in IdeaListTomodel)
                {
                    var approversList = unitOfWork.IMIdeaApprover.Get().Where(f => f.IdeaId == i.IdeaId).ToList();
                    if (approversList != null)
                    {
                        if (approversList.Where(e => e.ApprovedDate != null).Any())
                            i.Status = "Pending for Approval";
                    }
                    var approvers = approversList.Select(f => f.ApproverId).ToList();
                    var priority = unitOfWork.IMIdeaPriorityHistory.Get().Where(f => f.IdeaId == i.IdeaId).OrderByDescending(r => r.Id).FirstOrDefault();
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
                return IdeaListTomodel;
            }

        }





        /// <summary>
        /// View All ideas
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="IsCusUser">Display ideas where InternalInitiative is Unchecked.</param>
        /// <returns></returns>
        public static IEnumerable<ApprovedModel> Approval(int ClientId, bool IsCusUser)
        {
            Db db = new Db();
            //True- Display ideas where InternalInitiative is Unchecked.
            if (IsCusUser)
            {
                var Idealist = (from im in db.IMPostIdea
                                join UP in db.UserProfiles on im.CreatedBy equals UP.Id into TempCreatedBy
                                join UP2 in db.UserProfiles on im.ApprovedBy equals UP2.Id into TempApprovedBy
                                join UP3 in db.UserProfiles on im.ImplementationManager equals UP3.Id into TempImpManager
                                join lob in db.IMLOBSource on im.LOBId equals lob.Id into TempLob
                                //join IS in db.IMIdeaSource on im.IdeaSourceId equals IS.Id
                                join T in db.IMTeam on im.TeamId equals T.Id into TempTeam
                                join S in db.IMStatus on im.StatusId equals S.Id into TempStatus
                                join Com in db.IMComplexity on im.ComplexityId equals Com.Id into TempComplexity
                                join impS in db.IMStatus on im.ImpStatusId equals impS.Id into TempImpStatus
                                where im.ClientId == ClientId && ((im.ImpStatusId == null && im.StatusId == (int)IMStatus.Hold) || im.ImpStatusId == (int)IMStatus.SubmitForPlanning || im.StatusId == (int)IMStatus.Approved || im.StatusId == (int)IMStatus.Declined || im.StatusId == (int)IMStatus.Cancelled)
                                    && im.InternalInitiative == false && im.IsDeleted == false
                                from team in TempTeam.DefaultIfEmpty()
                                from status in TempStatus.DefaultIfEmpty()
                                from Complexity in TempComplexity.DefaultIfEmpty()
                                from createdBy in TempCreatedBy.DefaultIfEmpty()
                                from approvedBy in TempApprovedBy.DefaultIfEmpty()
                                from impManager in TempImpManager.DefaultIfEmpty()
                                from Imps in TempImpStatus.DefaultIfEmpty()
                                from Lob in TempLob.DefaultIfEmpty()
                                select new
                                {
                                    IdeaId = im.Id,
                                    IdeaName = im.ItemName,
                                    IdeaNumber = im.IdeaNumber,
                                    IdeaSource = im.IdeaSource,
                                    InternalInitiative = im.InternalInitiative,
                                    Team = team.Team,
                                    LOB = Lob.LOB,
                                    ImpStatus = Imps.Status,
                                    Status = status.Status,
                                    Priority = im.PriorityId,
                                    complexity = Complexity.Complexity,
                                    value = im.QtyFication,
                                    CreatedBy = createdBy.FirstName + " " + createdBy.LastName,
                                    ApprovedBy = approvedBy.FirstName + " " + approvedBy.LastName,
                                    ImpManager = impManager.FirstName + " " + impManager.LastName,
                                    CreatedDt = im.CreatedDate,
                                    StatusChange = im.StatusDt,
                                    ProposedBy = im.ProposedBy
                                }).Distinct().ToList();
                var IdeaListTomodel = Idealist.Select(e => new ApprovedModel
                {
                    IdeaId = e.IdeaId,
                    IdeaName = e.IdeaName,
                    IdeaNumber = e.IdeaNumber,
                    ApproverBy = e.ApprovedBy,
                    Complexity = e.complexity,
                    CreatedBy = e.ProposedBy != null ? e.CreatedBy + "/ " + e.ProposedBy : e.CreatedBy,
                    CreatedDt = e.CreatedDt,
                    IdeaSource = e.IdeaSource,
                    ImplementationManager = e.ImpManager,
                    ImpStatus = e.ImpStatus,
                    InternalInitiative = e.InternalInitiative,
                    LOB = e.LOB,
                    Priority = e.Priority.ToString(),
                    Status = e.Status,
                    StatusChangeDt = e.StatusChange,
                    Team = e.Team,
                    Value = e.value.ToString()

                });

                return IdeaListTomodel;
            }
            else
            {
                var Idealist = (from im in db.IMPostIdea
                                join UP in db.UserProfiles on im.CreatedBy equals UP.Id into TempCreatedBy
                                join UP2 in db.UserProfiles on im.ApprovedBy equals UP2.Id into TempApprovedBy
                                join UP3 in db.UserProfiles on im.ImplementationManager equals UP3.Id into TempImpManager
                                join lob in db.IMLOBSource on im.LOBId equals lob.Id into TempLob
                                //join IS in db.IMIdeaSource on im.IdeaSourceId equals IS.Id
                                join T in db.IMTeam on im.TeamId equals T.Id into TempTeam
                                join S in db.IMStatus on im.StatusId equals S.Id into TempStatus
                                join Com in db.IMComplexity on im.ComplexityId equals Com.Id into TempComplexity
                                join impS in db.IMStatus on im.ImpStatusId equals impS.Id into TempImpStatus
                                where im.ClientId == ClientId && ((im.ImpStatusId == null && im.StatusId == (int)IMStatus.Hold) || im.ImpStatusId == (int)IMStatus.SubmitForPlanning || im.StatusId == (int)IMStatus.Approved || im.StatusId == (int)IMStatus.Declined || im.StatusId == (int)IMStatus.Cancelled)
                                     && im.IsDeleted == false
                                from team in TempTeam.DefaultIfEmpty()
                                from status in TempStatus.DefaultIfEmpty()
                                from Complexity in TempComplexity.DefaultIfEmpty()
                                from createdBy in TempCreatedBy.DefaultIfEmpty()
                                from approvedBy in TempApprovedBy.DefaultIfEmpty()
                                from impManager in TempImpManager.DefaultIfEmpty()
                                from Imps in TempImpStatus.DefaultIfEmpty()
                                from Lob in TempLob.DefaultIfEmpty()
                                select new
                                {
                                    IdeaId = im.Id,
                                    IdeaName = im.ItemName,
                                    IdeaNumber = im.IdeaNumber,
                                    IdeaSource = im.IdeaSource,
                                    InternalInitiative = im.InternalInitiative,
                                    Team = team.Team,
                                    LOB = Lob.LOB,
                                    ImpStatus = Imps.Status,
                                    Status = status.Status,
                                    Priority = im.PriorityId,
                                    complexity = Complexity.Complexity,
                                    value = im.QtyFication,
                                    CreatedBy = createdBy.FirstName + " " + createdBy.LastName,
                                    ApprovedBy = approvedBy.FirstName + " " + approvedBy.LastName,
                                    ImpManager = impManager.FirstName + " " + impManager.LastName,
                                    CreatedDt = im.CreatedDate,
                                    StatusChange = im.StatusDt,
                                    ProposedBy = im.ProposedBy
                                }).Distinct().ToList();
                var IdeaListTomodel = Idealist.Select(e => new ApprovedModel
                {
                    IdeaId = e.IdeaId,
                    IdeaName = e.IdeaName,
                    IdeaNumber = e.IdeaNumber,
                    ApproverBy = e.ApprovedBy,
                    Complexity = e.complexity,
                    CreatedBy = e.ProposedBy != null ? e.CreatedBy + "/ " + e.ProposedBy : e.CreatedBy,
                    CreatedDt = e.CreatedDt,
                    IdeaSource = e.IdeaSource,
                    ImplementationManager = e.ImpManager,
                    ImpStatus = e.ImpStatus,
                    InternalInitiative = e.InternalInitiative,
                    LOB = e.LOB,
                    Priority = e.Priority.ToString(),
                    Status = e.Status,
                    StatusChangeDt = e.StatusChange,
                    Team = e.Team,
                    Value = e.value.ToString()

                });

                return IdeaListTomodel;
            }

        }


        /// <summary>
        /// View My Ideas
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="UserId">Created By</param>
        /// <param name="IsAdmin"> If YES : DisplayIdeas for Approver, Else : Display Ideas for Creator.</param>
        /// <returns></returns>
        public static IEnumerable<ApprovedModel> Approval(int ClientId, int UserId, bool IsAdmin)
        {
            Db db = new Db();
            //If Logged in user is ADMIN/SysAdmin
            if (IsAdmin)
            {
                var Idealist = (from im in db.IMPostIdea
                                join UP in db.UserProfiles on im.CreatedBy equals UP.Id into TempCreatedBy
                                join UP2 in db.UserProfiles on im.ApprovedBy equals UP2.Id into TempApprovedBy
                                join UP3 in db.UserProfiles on im.ImplementationManager equals UP3.Id into TempImpManager
                                join iMApp in db.IMIdeaApprover on im.ApprovedBy equals iMApp.ApproverId into TempIdeaApprover
                                join lob in db.IMLOBSource on im.LOBId equals lob.Id into TempLob
                                //join IS in db.IMIdeaSource on im.IdeaSourceId equals IS.Id
                                join T in db.IMTeam on im.TeamId equals T.Id into TempTeam
                                join S in db.IMStatus on im.StatusId equals S.Id into TempStatus
                                join Com in db.IMComplexity on im.ComplexityId equals Com.Id into TempComplexity
                                join impS in db.IMStatus on im.ImpStatusId equals impS.Id into TempImpStatus
                                where im.ClientId == ClientId && (im.ApprovedBy == UserId || im.CreatedBy == UserId) && ((im.ImpStatusId == null && im.StatusId == (int)IMStatus.Hold) || im.ImpStatusId == (int)IMStatus.SubmitForPlanning || im.StatusId == (int)IMStatus.Approved || im.StatusId == (int)IMStatus.Declined || im.StatusId == (int)IMStatus.Cancelled)
                                     && im.IsDeleted == false
                                from IMApp in TempIdeaApprover.DefaultIfEmpty()
                                from team in TempTeam.DefaultIfEmpty()
                                from status in TempStatus.DefaultIfEmpty()
                                from Complexity in TempComplexity.DefaultIfEmpty()
                                from createdBy in TempCreatedBy.DefaultIfEmpty()
                                from approvedBy in TempApprovedBy.DefaultIfEmpty()
                                from impManager in TempImpManager.DefaultIfEmpty()
                                from Imps in TempImpStatus.DefaultIfEmpty()
                                from Lob in TempLob.DefaultIfEmpty()
                                select new
                                {
                                    IdeaId = im.Id,
                                    IdeaName = im.ItemName,
                                    IdeaNumber = im.IdeaNumber,
                                    IdeaSource = im.IdeaSource,
                                    InternalInitiative = im.InternalInitiative,
                                    Team = team.Team,
                                    LOB = Lob.LOB,
                                    ImpStatus = Imps.Status,
                                    Status = status.Status,
                                    Priority = im.PriorityId,
                                    complexity = Complexity.Complexity,
                                    value = im.QtyFication,
                                    CreatedBy = createdBy.FirstName + " " + createdBy.LastName,
                                    ApprovedBy = approvedBy.FirstName + " " + approvedBy.LastName,
                                    ImpManager = impManager.FirstName + " " + impManager.LastName,
                                    CreatedDt = im.CreatedDate,
                                    StatusChange = im.StatusDt,
                                    ProposedBy = im.ProposedBy
                                }).Distinct().ToList();
                var IdeaListTomodel = Idealist.Select(e => new ApprovedModel
                {
                    IdeaId = e.IdeaId,
                    IdeaName = e.IdeaName,
                    IdeaNumber = e.IdeaNumber,
                    ApproverBy = e.ApprovedBy,
                    Complexity = e.complexity,
                    CreatedBy = e.ProposedBy != null ? e.CreatedBy + "/ " + e.ProposedBy : e.CreatedBy,
                    CreatedDt = e.CreatedDt,
                    IdeaSource = e.IdeaSource,
                    ImplementationManager = e.ImpManager,
                    ImpStatus = e.ImpStatus,
                    InternalInitiative = e.InternalInitiative,
                    LOB = e.LOB,
                    Priority = e.Priority.ToString(),
                    Status = e.Status,
                    StatusChangeDt = e.StatusChange,
                    Team = e.Team,
                    Value = e.value.ToString()

                });

                return IdeaListTomodel;
            }
            //LoggedIn User is not Admin
            else
            {
                var Idealist = (from im in db.IMPostIdea
                                join UP in db.UserProfiles on im.CreatedBy equals UP.Id into TempCreatedBy
                                join UP2 in db.UserProfiles on im.ApprovedBy equals UP2.Id into TempApprovedBy
                                join UP3 in db.UserProfiles on im.ImplementationManager equals UP3.Id into TempImpManager
                                join lob in db.IMLOBSource on im.LOBId equals lob.Id into TempLob
                                //join IS in db.IMIdeaSource on im.IdeaSourceId equals IS.Id
                                join T in db.IMTeam on im.TeamId equals T.Id into TempTeam
                                join S in db.IMStatus on im.StatusId equals S.Id into TempStatus
                                join Com in db.IMComplexity on im.ComplexityId equals Com.Id into TempComplexity
                                join impS in db.IMStatus on im.ImpStatusId equals impS.Id into TempImpStatus
                                where im.ClientId == ClientId && im.CreatedBy == UserId && (im.StatusId == (int)IMStatus.Hold || im.ImpStatusId == (int)IMStatus.SubmitForPlanning || im.StatusId == (int)IMStatus.Approved || im.StatusId == (int)IMStatus.Declined || im.StatusId == (int)IMStatus.Cancelled)
                                     && im.IsDeleted == false
                                from team in TempTeam.DefaultIfEmpty()
                                from status in TempStatus.DefaultIfEmpty()
                                from Complexity in TempComplexity.DefaultIfEmpty()
                                from createdBy in TempCreatedBy.DefaultIfEmpty()
                                from approvedBy in TempApprovedBy.DefaultIfEmpty()
                                from impManager in TempImpManager.DefaultIfEmpty()
                                from Imps in TempImpStatus.DefaultIfEmpty()
                                from Lob in TempLob.DefaultIfEmpty()
                                select new
                                {
                                    IdeaId = im.Id,
                                    IdeaName = im.ItemName,
                                    IdeaNumber = im.IdeaNumber,
                                    IdeaSource = im.IdeaSource,
                                    InternalInitiative = im.InternalInitiative,
                                    Team = team.Team,
                                    LOB = Lob.LOB,
                                    ImpStatus = Imps.Status,
                                    Status = status.Status,
                                    Priority = im.PriorityId,
                                    complexity = Complexity.Complexity,
                                    value = im.QtyFication,
                                    CreatedBy = createdBy.FirstName + " " + createdBy.LastName,
                                    ApprovedBy = approvedBy.FirstName + " " + approvedBy.LastName,
                                    ImpManager = impManager.FirstName + " " + impManager.LastName,
                                    CreatedDt = im.CreatedDate,
                                    StatusChange = im.StatusDt,
                                    ProposedBy = im.ProposedBy
                                }).Distinct().ToList();
                var IdeaListTomodel = Idealist.Select(e => new ApprovedModel
                {
                    IdeaId = e.IdeaId,
                    IdeaName = e.IdeaName,
                    IdeaNumber = e.IdeaNumber,
                    ApproverBy = e.ApprovedBy,
                    Complexity = e.complexity,
                    CreatedBy = e.ProposedBy != null ? e.CreatedBy + "/ " + e.ProposedBy : e.CreatedBy,
                    CreatedDt = e.CreatedDt,
                    IdeaSource = e.IdeaSource,
                    ImplementationManager = e.ImpManager,
                    ImpStatus = e.ImpStatus,
                    InternalInitiative = e.InternalInitiative,
                    LOB = e.LOB,
                    Priority = e.Priority.ToString(),
                    Status = e.Status,
                    StatusChangeDt = e.StatusChange,
                    Team = e.Team,
                    Value = e.value.ToString()

                });

                return IdeaListTomodel;
            }
        }


        /// <summary>
        /// View All Ideas - [Implemented Ideas]
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="IsCusUser">Display ideas where InternalInitiative is Unchecked.</param>
        /// <returns></returns>
        public static IEnumerable<ImplementationModel> Implementation(int ClientId, bool IsCusUser)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();
            //True- Display ideas where InternalInitiative is Unchecked.
            if (IsCusUser)
            {
                var Idealist = (from im in db.IMPostIdea
                                join UP3 in db.UserProfiles on im.ImplementationManager equals UP3.Id into TempImpManager
                                join T in db.IMTeam on im.TeamId equals T.Id into TempTeam
                                join Com in db.IMComplexity on im.ComplexityId equals Com.Id into TempComplexity
                                join impS in db.IMStatus on im.ImpStatusId equals impS.Id into TempImpStatus
                                where im.ClientId == ClientId &&
                                    (im.ImpStatusId == (int)IMStatus.Hold || im.ImpStatusId == (int)IMStatus.SubmitForPlanning || im.ImpStatusId == (int)IMStatus.Planned || im.ImpStatusId == (int)IMStatus.Initiated)
                                    && im.InternalInitiative == false && im.IsDeleted == false && im.StatusId != (int)IMStatus.Cancelled
                                from team in TempTeam.DefaultIfEmpty()
                                from Complexity in TempComplexity.DefaultIfEmpty()
                                from impManager in TempImpManager.DefaultIfEmpty()
                                from Imps in TempImpStatus.DefaultIfEmpty()
                                select new
                                {
                                    IdeaId = im.Id,
                                    IdeaName = im.ItemName,
                                    IdeaNumber = im.IdeaNumber,
                                    InternalInitiative = im.InternalInitiative,
                                    Team = team.Team,
                                    ImpStatus = Imps.Status,
                                    Priority = im.PriorityId,
                                    /*
                                        ImpPriority - 
                                        PrevImpPriority -
                                    */
                                    complexity = Complexity.Complexity,
                                    ImpManager = impManager.FirstName + " " + impManager.LastName,
                                    LastReviewDate = im.LastReviewDt,
                                    SubmitPlanning = im.ApprovedDt,
                                    planStDt = im.PlannedStartDate,
                                    planEdDt = im.PlannedEndDate,
                                    actualStDt = im.ActualStartDate,
                                    actualEdDt = im.ActualEndDate,
                                    UpdatedDt = im.LastUpdatedDate,
                                    ActualProgress = im.ImplementationProgress
                                }).Distinct().ToList();
                var IdeaListTomodel = Idealist.Select(e => new ImplementationModel
                {
                    IdeaId = e.IdeaId,
                    IdeaName = e.IdeaName,
                    IdeaNumber = e.IdeaNumber,
                    InternalInitiative = e.InternalInitiative,
                    Priority = e.Priority.ToString(),
                    PrevImpPriority = "",
                    ImpPriority = "",
                    Complexity = e.complexity,
                    ImplementationManager = e.ImpManager,
                    ImpStatus = e.ImpStatus,
                    Team = e.Team,
                    LastReviewDt = e.LastReviewDate,
                    SubmitForPlanningDt = e.SubmitPlanning,
                    PlannedEndDt = e.planEdDt,
                    PlannedStartDt = e.planStDt,
                    ActualEndDt = e.actualEdDt,
                    ActualStartDt = e.actualStDt,
                    ActualProgress = e.ActualProgress == null ? "0" : e.ActualProgress.ToString(),


                    /*Calculation Fields
                     ImplementationHealth - 
                     TargetProgress - 
                     ActualProgress - 
                     */
                    LastModifiedDt = e.UpdatedDt
                }).ToList();
                //To get Priority and Approvers
                foreach (var i in IdeaListTomodel)
                {
                    var priority = unitOfWork.IMImpPriorityHistory.Get().Where(f => f.IdeaId == i.IdeaId).OrderByDescending(r => r.Id).FirstOrDefault();
                    if (priority != null)
                    {

                        i.PrevImpPriority = priority.OldImpPriority == null ? "0" : priority.OldImpPriority.ToString();
                        i.ImpPriority = priority.NewImpPriority == null ? "0" : priority.NewImpPriority.ToString();
                    }


                }

                return IdeaListTomodel.AsEnumerable();
            }
            else
            {
                var Idealist = (from im in db.IMPostIdea
                                join UP3 in db.UserProfiles on im.ImplementationManager equals UP3.Id into TempImpManager
                                join T in db.IMTeam on im.TeamId equals T.Id into TempTeam
                                join Com in db.IMComplexity on im.ComplexityId equals Com.Id into TempComplexity
                                join impS in db.IMStatus on im.ImpStatusId equals impS.Id into TempImpStatus
                                where im.ClientId == ClientId &&
                                    (im.ImpStatusId == (int)IMStatus.Hold || im.ImpStatusId == (int)IMStatus.SubmitForPlanning || im.ImpStatusId == (int)IMStatus.Planned || im.ImpStatusId == (int)IMStatus.Initiated)
                                     && im.IsDeleted == false && im.StatusId != (int)IMStatus.Cancelled
                                from team in TempTeam.DefaultIfEmpty()
                                from Complexity in TempComplexity.DefaultIfEmpty()
                                from impManager in TempImpManager.DefaultIfEmpty()
                                from Imps in TempImpStatus.DefaultIfEmpty()
                                select new
                                {
                                    IdeaId = im.Id,
                                    IdeaName = im.ItemName,
                                    IdeaNumber = im.IdeaNumber,
                                    InternalInitiative = im.InternalInitiative,
                                    Team = team.Team,
                                    ImpStatus = Imps.Status,
                                    Priority = im.PriorityId,
                                    /*
                                        ImpPriority - 
                                        PrevImpPriority -
                                    */
                                    complexity = Complexity.Complexity,
                                    ImpManager = impManager.FirstName + " " + impManager.LastName,
                                    LastReviewDate = im.LastReviewDt,
                                    SubmitPlanning = im.ApprovedDt,
                                    planStDt = im.PlannedStartDate,
                                    planEdDt = im.PlannedEndDate,
                                    actualStDt = im.ActualStartDate,
                                    actualEdDt = im.ActualEndDate,
                                    UpdatedDt = im.LastUpdatedDate,
                                    ActualProgress = im.ImplementationProgress
                                }).Distinct().ToList();
                var IdeaListTomodel = Idealist.Select(e => new ImplementationModel
                {
                    IdeaId = e.IdeaId,
                    IdeaName = e.IdeaName,
                    IdeaNumber = e.IdeaNumber,
                    InternalInitiative = e.InternalInitiative,
                    Priority = e.Priority.ToString(),
                    PrevImpPriority = "",
                    ImpPriority = "",
                    Complexity = e.complexity,
                    ImplementationManager = e.ImpManager,
                    ImpStatus = e.ImpStatus,
                    Team = e.Team,
                    LastReviewDt = e.LastReviewDate,
                    SubmitForPlanningDt = e.SubmitPlanning,
                    PlannedEndDt = e.planEdDt,
                    PlannedStartDt = e.planStDt,
                    ActualEndDt = e.actualEdDt,
                    ActualStartDt = e.actualStDt,
                    ActualProgress = e.ActualProgress == null ? "0" : e.ActualProgress.ToString(),


                    /*Calculation Fields
                     ImplementationHealth - 
                     TargetProgress - 
                     ActualProgress - 
                     */
                    LastModifiedDt = e.UpdatedDt
                }).ToList();
                //To get Priority and Approvers
                foreach (var i in IdeaListTomodel)
                {
                    var priority = unitOfWork.IMImpPriorityHistory.Get().Where(f => f.IdeaId == i.IdeaId).OrderByDescending(r => r.Id).FirstOrDefault();
                    if (priority != null)
                    {
                        i.PrevImpPriority = priority.OldImpPriority == null ? "0" : priority.OldImpPriority.ToString();
                        i.ImpPriority = priority.NewImpPriority == null ? "0" : priority.NewImpPriority.ToString();
                    }

                    //i.PrevImpPriority = priority.OldImpPriority.ToString();
                    //i.ImpPriority = priority.NewImpPriority.ToString();
                }

                return IdeaListTomodel.AsEnumerable();
            }
        }


        /// <summary>
        /// View My Idea - [Implemented Ideas]
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="UserId"></param>
        /// <param name="IsAdmin"> If YES : DisplayIdeas for ImplementationManager, Else : Display Ideas for Creator.</param>
        /// <returns></returns>
        public static IEnumerable<ImplementationModel> Implementation(int ClientId, int UserId, bool IsAdmin)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();
            //ImplementationAdmin
            if (IsAdmin)
            {
                var Idealist = (from im in db.IMPostIdea
                                join UP3 in db.UserProfiles on im.ImplementationManager equals UP3.Id into TempImpManager
                                join T in db.IMTeam on im.TeamId equals T.Id into TempTeam
                                join Com in db.IMComplexity on im.ComplexityId equals Com.Id into TempComplexity
                                join impS in db.IMStatus on im.ImpStatusId equals impS.Id into TempImpStatus
                                join imcomp in db.IMCompletedAndNextSteps.Where(w => w.ImpManagers == UserId) on im.Id equals imcomp.IdeaId into TempCompletedSteps

                                from team in TempTeam.DefaultIfEmpty()
                                from Complexity in TempComplexity.DefaultIfEmpty()
                                from impManager in TempImpManager.DefaultIfEmpty()
                                from Imps in TempImpStatus.DefaultIfEmpty()
                                from imcomp in TempCompletedSteps.DefaultIfEmpty()
                                where im.ClientId == ClientId && (im.ImplementationManager == UserId || im.CreatedBy == UserId || im.ApprovedBy == UserId || imcomp.ImpManagers == UserId)
                                && (im.ImpStatusId == (int)IMStatus.Hold || im.ImpStatusId == (int)IMStatus.SubmitForPlanning || im.ImpStatusId == (int)IMStatus.Planned || im.ImpStatusId == (int)IMStatus.Initiated)
                                     && im.IsDeleted == false && im.StatusId != (int)IMStatus.Cancelled

                                select new
                                {
                                    IdeaId = im.Id,
                                    IdeaName = im.ItemName,
                                    IdeaNumber = im.IdeaNumber,
                                    InternalInitiative = im.InternalInitiative,
                                    Team = team.Team,
                                    ImpStatus = Imps.Status,
                                    Priority = im.PriorityId,
                                    /*
                                     ImpPriority - 
                                     PrevImpPriority -
                                     */
                                    complexity = Complexity.Complexity,
                                    ImpManager = impManager.FirstName + " " + impManager.LastName,
                                    LastReviewDate = im.LastReviewDt,
                                    SubmitPlanning = im.ApprovedDt,
                                    planStDt = im.PlannedStartDate,
                                    planEdDt = im.PlannedEndDate,
                                    actualStDt = im.ActualStartDate,
                                    actualEdDt = im.ActualEndDate,
                                    UpdatedDt = im.LastUpdatedDate,
                                    ActualProgress = im.ImplementationProgress
                                }).Distinct().ToList();
                var IdeaListTomodel = Idealist.Select(e => new ImplementationModel
                {
                    IdeaId = e.IdeaId,
                    IdeaName = e.IdeaName,
                    IdeaNumber = e.IdeaNumber,
                    InternalInitiative = e.InternalInitiative,
                    Priority = e.Priority.ToString(),
                    PrevImpPriority = "",
                    ImpPriority = "",
                    Complexity = e.complexity,
                    ImplementationManager = e.ImpManager,
                    ImpStatus = e.ImpStatus,
                    Team = e.Team,
                    LastReviewDt = e.LastReviewDate,
                    SubmitForPlanningDt = e.SubmitPlanning,
                    PlannedEndDt = e.planEdDt,
                    PlannedStartDt = e.planStDt,
                    ActualEndDt = e.actualEdDt,
                    ActualStartDt = e.actualStDt,
                    ActualProgress = e.ActualProgress == null ? "0" : e.ActualProgress.ToString(),


                    /*Calculation Fields
                     ImplementationHealth - 
                     TargetProgress - 
                     ActualProgress - 
                     */
                    LastModifiedDt = e.UpdatedDt
                }).ToList();
                //To get Priority and Approvers
                foreach (var i in IdeaListTomodel)
                {
                    var priority = unitOfWork.IMImpPriorityHistory.Get().Where(f => f.IdeaId == i.IdeaId).OrderByDescending(r => r.Id).FirstOrDefault();
                    if (priority != null)
                    {
                        i.PrevImpPriority = priority.OldImpPriority == null ? "0" : priority.OldImpPriority.ToString();
                        i.ImpPriority = priority.NewImpPriority == null ? "0" : priority.NewImpPriority.ToString();
                    }

                    //i.PrevImpPriority = priority.OldImpPriority.ToString();
                    //i.ImpPriority = priority.NewImpPriority.ToString();
                }

                return IdeaListTomodel.AsEnumerable();
            }
            //User
            else
            {
                var Idealist = (from im in db.IMPostIdea
                                join UP3 in db.UserProfiles on im.ImplementationManager equals UP3.Id into TempImpManager
                                join T in db.IMTeam on im.TeamId equals T.Id into TempTeam
                                join Com in db.IMComplexity on im.ComplexityId equals Com.Id into TempComplexity
                                join impS in db.IMStatus on im.ImpStatusId equals impS.Id into TempImpStatus
                                where im.ClientId == ClientId && im.CreatedBy == UserId &&
                                    (im.ImpStatusId == (int)IMStatus.Hold || im.ImpStatusId == (int)IMStatus.SubmitForPlanning || im.ImpStatusId == (int)IMStatus.Planned || im.ImpStatusId == (int)IMStatus.Initiated)
                                     && im.IsDeleted == false && im.StatusId != (int)IMStatus.Cancelled
                                from team in TempTeam.DefaultIfEmpty()
                                from Complexity in TempComplexity.DefaultIfEmpty()
                                from impManager in TempImpManager.DefaultIfEmpty()
                                from Imps in TempImpStatus.DefaultIfEmpty()
                                select new
                                {
                                    IdeaId = im.Id,
                                    IdeaName = im.ItemName,
                                    IdeaNumber = im.IdeaNumber,
                                    InternalInitiative = im.InternalInitiative,
                                    Team = team.Team,
                                    ImpStatus = Imps.Status,
                                    Priority = im.PriorityId,
                                    /*
                                     ImpPriority - 
                                     PrevImpPriority -
                                     */
                                    complexity = Complexity.Complexity,
                                    ImpManager = impManager.FirstName + " " + impManager.LastName,
                                    LastReviewDate = im.LastReviewDt,
                                    SubmitPlanning = im.ApprovedDt,
                                    planStDt = im.PlannedStartDate,
                                    planEdDt = im.PlannedEndDate,
                                    actualStDt = im.ActualStartDate,
                                    actualEdDt = im.ActualEndDate,
                                    UpdatedDt = im.LastUpdatedDate,
                                    ActualProgress = im.ImplementationProgress
                                }).Distinct().ToList();
                var IdeaListTomodel = Idealist.Select(e => new ImplementationModel
                {
                    IdeaId = e.IdeaId,
                    IdeaName = e.IdeaName,
                    IdeaNumber = e.IdeaNumber,
                    InternalInitiative = e.InternalInitiative,
                    Priority = e.Priority.ToString(),
                    PrevImpPriority = "",
                    ImpPriority = "",
                    Complexity = e.complexity,
                    ImplementationManager = e.ImpManager,
                    ImpStatus = e.ImpStatus,
                    Team = e.Team,
                    LastReviewDt = e.LastReviewDate,
                    SubmitForPlanningDt = e.SubmitPlanning,
                    PlannedEndDt = e.planEdDt,
                    PlannedStartDt = e.planStDt,
                    ActualEndDt = e.actualEdDt,
                    ActualStartDt = e.actualStDt,
                    ActualProgress = e.ActualProgress == null ? "0" : e.ActualProgress.ToString(),


                    /*Calculation Fields
                     ImplementationHealth - 
                     TargetProgress - 
                     ActualProgress - 
                     */
                    LastModifiedDt = e.UpdatedDt
                }).ToList();
                //To get Priority and Approvers
                foreach (var i in IdeaListTomodel)
                {
                    var priority = unitOfWork.IMImpPriorityHistory.Get().Where(f => f.IdeaId == i.IdeaId).OrderByDescending(r => r.Id).FirstOrDefault();
                    if (priority != null)
                    {
                        i.PrevImpPriority = priority.OldImpPriority == null ? "0" : priority.OldImpPriority.ToString();
                        i.ImpPriority = priority.NewImpPriority == null ? "0" : priority.NewImpPriority.ToString();
                    }

                    //i.PrevImpPriority = priority.OldImpPriority.ToString();
                    //i.ImpPriority = priority.NewImpPriority.ToString();
                }


                return IdeaListTomodel.AsEnumerable();
            }
        }



        /// <summary>
        /// View All Ideas - [Completed Ideas]
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="IsCusUser">Display ideas where InternalInitiative is Unchecked.</param>
        /// <returns></returns>
        public static IEnumerable<CompletionModel> Completion(int ClientId, bool IsCusUser)
        {
            Db db = new Db();
            //True- Display ideas where InternalInitiative is Unchecked.
            if (IsCusUser)
            {
                var Idealist = (from im in db.IMPostIdea
                                join UP in db.UserProfiles on im.CreatedBy equals UP.Id into TempCreatedBy
                                join UP2 in db.UserProfiles on im.ApprovedBy equals UP2.Id into TempApprovedBy
                                join UP3 in db.UserProfiles on im.ImplementationManager equals UP3.Id into TempImpManager
                                //join IS in db.IMIdeaSource on im.IdeaSourceId equals IS.Id
                                join T in db.IMTeam on im.TeamId equals T.Id into TempTeam
                                join Com in db.IMComplexity on im.ComplexityId equals Com.Id into TempComplexity
                                where im.ClientId == ClientId && (im.ImpStatusId == (int)IMStatus.Implemented)
                                    && im.InternalInitiative == false && im.IsDeleted == false
                                from team in TempTeam.DefaultIfEmpty()
                                from Complexity in TempComplexity.DefaultIfEmpty()
                                from createdBy in TempCreatedBy.DefaultIfEmpty()
                                from approvedBy in TempApprovedBy.DefaultIfEmpty()
                                from impManager in TempImpManager.DefaultIfEmpty()
                                select new
                                {
                                    IdeaId = im.Id,
                                    IdeaName = im.ItemName,
                                    IdeaNumber = im.IdeaNumber,
                                    IdeaSource = im.IdeaSource,
                                    InternalInitiative = im.InternalInitiative,
                                    Team = team.Team,
                                    Priority = im.PriorityId,
                                    complexity = Complexity.Complexity,
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
                                    UpdatedDt = im.LastUpdatedDate,
                                    ProposedBy = im.ProposedBy

                                }).Distinct().ToList();
                var IdeaListTomodel = Idealist.Select(e => new CompletionModel
                {
                    IdeaId = e.IdeaId,
                    IdeaName = e.IdeaName,
                    IdeaNumber = e.IdeaNumber,
                    ApprovedBy = e.ApprovedBy,
                    Complexity = e.complexity,
                    CreatedBy = e.ProposedBy != null ? e.CreatedBy + "/ " + e.ProposedBy : e.CreatedBy,
                    LastModifiedDt = e.UpdatedDt,
                    IdeaSource = e.IdeaSource,
                    ImplementationManager = e.ImpManager,
                    InternalInitiative = e.InternalInitiative,
                    Priority = e.Priority.ToString(),
                    Team = e.Team,
                    Value = e.value.ToString(),
                    PlannedEndDt = e.PlannedEndDt,
                    PlannedStartDt = e.PlannedStartDt,
                    ActualEndDt = e.ActualEndDt,
                    ActualStartDt = e.ActualStartDt,
                    ActualProgress = e.ActualProgress == null ? "0" : e.ActualProgress.ToString(),
                    //Need Calculation
                    CompletedOnSchedule = ""

                });

                return IdeaListTomodel;
            }
            else
            {
                var Idealist = (from im in db.IMPostIdea
                                join UP in db.UserProfiles on im.CreatedBy equals UP.Id into TempCreatedBy
                                join UP2 in db.UserProfiles on im.ApprovedBy equals UP2.Id into TempApprovedBy
                                join UP3 in db.UserProfiles on im.ImplementationManager equals UP3.Id into TempImpManager
                                //join IS in db.IMIdeaSource on im.IdeaSourceId equals IS.Id
                                join T in db.IMTeam on im.TeamId equals T.Id into TempTeam
                                join Com in db.IMComplexity on im.ComplexityId equals Com.Id into TempComplexity
                                where im.ClientId == ClientId && (im.ImpStatusId == (int)IMStatus.Implemented) && im.IsDeleted == false
                                from team in TempTeam.DefaultIfEmpty()
                                from Complexity in TempComplexity.DefaultIfEmpty()
                                from createdBy in TempCreatedBy.DefaultIfEmpty()
                                from approvedBy in TempApprovedBy.DefaultIfEmpty()
                                from impManager in TempImpManager.DefaultIfEmpty()
                                select new
                                {
                                    IdeaId = im.Id,
                                    IdeaName = im.ItemName,
                                    IdeaNumber = im.IdeaNumber,
                                    IdeaSource = im.IdeaSource,
                                    InternalInitiative = im.InternalInitiative,
                                    Team = team.Team,
                                    Priority = im.PriorityId,
                                    complexity = Complexity.Complexity,
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
                                    UpdatedDt = im.LastUpdatedDate,
                                    ProposedBy = im.ProposedBy

                                }).Distinct().ToList();
                var IdeaListTomodel = Idealist.Select(e => new CompletionModel
                {
                    IdeaId = e.IdeaId,
                    IdeaName = e.IdeaName,
                    IdeaNumber = e.IdeaNumber,
                    ApprovedBy = e.ApprovedBy,
                    Complexity = e.complexity,
                    CreatedBy = e.ProposedBy != null ? e.CreatedBy + "/ " + e.ProposedBy : e.CreatedBy,
                    LastModifiedDt = e.UpdatedDt,
                    IdeaSource = e.IdeaSource,
                    ImplementationManager = e.ImpManager,
                    InternalInitiative = e.InternalInitiative,
                    Priority = e.Priority.ToString(),
                    Team = e.Team,
                    Value = e.value.ToString(),
                    PlannedEndDt = e.PlannedEndDt,
                    PlannedStartDt = e.PlannedStartDt,
                    ActualEndDt = e.ActualEndDt,
                    ActualStartDt = e.ActualStartDt,
                    ActualProgress = e.ActualProgress == null ? "0" : e.ActualProgress.ToString(),
                    //Need Calculation
                    CompletedOnSchedule = ""

                });

                return IdeaListTomodel;
            }
        }

        /// <summary>
        /// View My Idea - [Completed Ideas]
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="UserId"></param>
        /// <param name="IsAdmin"></param>
        /// <returns></returns>
        public static IEnumerable<CompletionModel> Completion(int ClientId, int UserId, bool IsAdmin)
        {
            Db db = new Db();
            //If Admin
            if (IsAdmin)
            {
                var Idealist = (from im in db.IMPostIdea
                                join UP in db.UserProfiles on im.CreatedBy equals UP.Id into TempCreatedBy
                                join UP2 in db.UserProfiles on im.ApprovedBy equals UP2.Id into TempApprovedBy
                                join UP3 in db.UserProfiles on im.ImplementationManager equals UP3.Id into TempImpManager
                                //join IS in db.IMIdeaSource on im.IdeaSourceId equals IS.Id
                                join T in db.IMTeam on im.TeamId equals T.Id into TempTeam
                                join Com in db.IMComplexity on im.ComplexityId equals Com.Id into TempComplexity
                                where im.ClientId == ClientId && (im.CreatedBy == UserId || im.ApprovedBy == UserId || im.ImplementationManager == UserId) && (im.ImpStatusId == (int)IMStatus.Implemented)
                                     && im.IsDeleted == false
                                from team in TempTeam.DefaultIfEmpty()
                                from Complexity in TempComplexity.DefaultIfEmpty()
                                from createdBy in TempCreatedBy.DefaultIfEmpty()
                                from approvedBy in TempApprovedBy.DefaultIfEmpty()
                                from impManager in TempImpManager.DefaultIfEmpty()
                                select new
                                {
                                    IdeaId = im.Id,
                                    IdeaName = im.ItemName,
                                    IdeaNumber = im.IdeaNumber,
                                    IdeaSource = im.IdeaSource,
                                    InternalInitiative = im.InternalInitiative,
                                    Team = team.Team,
                                    Priority = im.PriorityId,
                                    complexity = Complexity.Complexity,
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
                                    UpdatedDt = im.LastUpdatedDate,
                                    ProposedBy = im.ProposedBy

                                }).Distinct().ToList();
                var IdeaListTomodel = Idealist.Select(e => new CompletionModel
                {
                    IdeaId = e.IdeaId,
                    IdeaName = e.IdeaName,
                    IdeaNumber = e.IdeaNumber,
                    ApprovedBy = e.ApprovedBy,
                    Complexity = e.complexity,
                    CreatedBy = e.ProposedBy != null ? e.CreatedBy + "/ s" + e.ProposedBy : e.CreatedBy,
                    LastModifiedDt = e.UpdatedDt,
                    IdeaSource = e.IdeaSource,
                    ImplementationManager = e.ImpManager,
                    InternalInitiative = e.InternalInitiative,
                    Priority = e.Priority.ToString(),
                    Team = e.Team,
                    Value = e.value.ToString(),
                    PlannedEndDt = e.PlannedEndDt,
                    PlannedStartDt = e.PlannedStartDt,
                    ActualEndDt = e.ActualEndDt,
                    ActualStartDt = e.ActualStartDt,
                    ActualProgress = e.ActualProgress == null ? "0" : e.ActualProgress.ToString(),
                    //Need Calculation
                    CompletedOnSchedule = ""

                });

                return IdeaListTomodel;
            }
            //normal User
            else
            {
                var Idealist = (from im in db.IMPostIdea
                                join UP in db.UserProfiles on im.CreatedBy equals UP.Id into TempCreatedBy
                                join UP2 in db.UserProfiles on im.ApprovedBy equals UP2.Id into TempApprovedBy
                                join UP3 in db.UserProfiles on im.ImplementationManager equals UP3.Id into TempImpManager
                                //join IS in db.IMIdeaSource on im.IdeaSourceId equals IS.Id
                                join T in db.IMTeam on im.TeamId equals T.Id into TempTeam
                                join Com in db.IMComplexity on im.ComplexityId equals Com.Id into TempComplexity
                                where im.ClientId == ClientId && im.CreatedBy == UserId && (im.ImpStatusId == (int)IMStatus.Implemented) && im.IsDeleted == false
                                from team in TempTeam.DefaultIfEmpty()
                                from Complexity in TempComplexity.DefaultIfEmpty()
                                from createdBy in TempCreatedBy.DefaultIfEmpty()
                                from approvedBy in TempApprovedBy.DefaultIfEmpty()
                                from impManager in TempImpManager.DefaultIfEmpty()
                                select new
                                {
                                    IdeaId = im.Id,
                                    IdeaName = im.ItemName,
                                    IdeaNumber = im.IdeaNumber,
                                    IdeaSource = im.IdeaSource,
                                    InternalInitiative = im.InternalInitiative,
                                    Team = team.Team,
                                    Priority = im.PriorityId,
                                    complexity = Complexity.Complexity,
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
                                    UpdatedDt = im.LastUpdatedDate,
                                    ProposedBy = im.ProposedBy

                                }).Distinct().ToList();
                var IdeaListTomodel = Idealist.Select(e => new CompletionModel
                {
                    IdeaId = e.IdeaId,
                    IdeaName = e.IdeaName,
                    IdeaNumber = e.IdeaNumber,
                    ApprovedBy = e.ApprovedBy,
                    Complexity = e.complexity,
                    CreatedBy = e.ProposedBy != null ? e.CreatedBy + "/ " + e.ProposedBy : e.CreatedBy,
                    LastModifiedDt = e.UpdatedDt,
                    IdeaSource = e.IdeaSource,
                    ImplementationManager = e.ImpManager,
                    InternalInitiative = e.InternalInitiative,
                    Priority = e.Priority.ToString(),
                    Team = e.Team,
                    Value = e.value.ToString(),
                    PlannedEndDt = e.PlannedEndDt,
                    PlannedStartDt = e.PlannedStartDt,
                    ActualEndDt = e.ActualEndDt,
                    ActualStartDt = e.ActualStartDt,
                    ActualProgress = e.ActualProgress == null ? "0" : e.ActualProgress.ToString(),
                    //Need Calculation
                    CompletedOnSchedule = ""

                });

                return IdeaListTomodel;
            }
        }
        //----------------------------------------------Search Ideas---------------------------------------------//

        public static IEnumerable<IdeaSearchModel> ideaSearch(string stringLike, int ClientId, bool IsCusUser)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();
            if (IsCusUser)
            {
                var tempList = (from Idea in db.IMPostIdea
                                join status in db.IMStatus on Idea.StatusId equals status.Id
                                join impStatus in db.IMStatus on Idea.ImpStatusId equals impStatus.Id into tempImpStatus
                                from ImpStatus in tempImpStatus.DefaultIfEmpty()
                                where (Idea.ItemName.Contains(stringLike) || Idea.Description.Contains(stringLike) || Idea.IdeaNumber.Contains(stringLike)) && Idea.ClientId == ClientId && Idea.InternalInitiative == false && Idea.IsDeleted == false
                                select new
                                {
                                    Id = Idea.Id,
                                    IdeaNumber = Idea.IdeaNumber,
                                    Name = Idea.ItemName,
                                    Status = status.Status,
                                    ImpStatus = ImpStatus.Status
                                }
                ).Distinct().ToList();
                var searchIdeaList = tempList.Select(e => new IdeaSearchModel
                {
                    IdeaId = e.Id,
                    IdeaNumber = e.IdeaNumber,
                    IdeaName = e.Name,
                    Status = (String.IsNullOrEmpty(e.ImpStatus) || e.Status.ToLower() == "closed") ? e.Status : e.ImpStatus
                });
                return searchIdeaList;
            }
            else
            {
                var tempList = (from Idea in db.IMPostIdea
                                join status in db.IMStatus on Idea.StatusId equals status.Id
                                join impStatus in db.IMStatus on Idea.ImpStatusId equals impStatus.Id into tempImpStatus
                                from ImpStatus in tempImpStatus.DefaultIfEmpty()
                                where (Idea.ItemName.Contains(stringLike) || Idea.Description.Contains(stringLike) || Idea.IdeaNumber.Contains(stringLike)) && Idea.ClientId == ClientId && Idea.IsDeleted == false
                                select new
                                {
                                    Id = Idea.Id,
                                    IdeaNumber = Idea.IdeaNumber,
                                    Name = Idea.ItemName,
                                    Status = status.Status,
                                    ImpStatus = ImpStatus.Status
                                }
                ).Distinct().ToList();
                var searchIdeaList = tempList.Select(e => new IdeaSearchModel
                {
                    IdeaId = e.Id,
                    IdeaNumber = e.IdeaNumber,
                    IdeaName = e.Name,
                    Status = (String.IsNullOrEmpty(e.ImpStatus) || e.Status.ToLower() == "closed") ? e.Status : e.ImpStatus
                });
                return searchIdeaList;
            }
        }


        //------------------------------------------------------------------------------------------------------------------------------------------//
        //----------------------------------------------------------- Post Idea Functions -----------------------------------------------------------//
        //------------------------------------------------------------------------------------------------------------------------------------------//


        //Home Tab
        //public static int saveIdea(string IdeaName, int IdeaSourceId, bool InternalInit, int TeamId, string Description, int clientId, int uid, decimal Value = 0, int IdeaTypeId = 0, int LobId = 0, int ComplexId = 0)
        public static IdeaViewModel saveIdea(IdeaViewModel IVM, int uid, string IdeaLink, string ClientName)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            Db db = new Db();
            if (IVM.IdeaId == 0)
            {
                //---------IdeaDetails--------------//
                IMPostIdea idea = new IMPostIdea();
                idea.ItemName = IVM.IdeaName.Trim();
                idea.IdeaSource = IVM.IdeaSourceId;
                idea.InternalInitiative = IVM.InternalInit;
                idea.TeamId = IVM.TeamId;
                idea.Description = IVM.Description;
                idea.ImprovementTheme = IVM.ImprovementTheme;
                if (IVM.QtyFication != 0)
                {
                    idea.QtyFication = IVM.QtyFication;
                }
                if (IVM.IdeatypeId != 0)
                {
                    idea.IdeaTypeId = IVM.IdeatypeId;
                }
                if (IVM.Complexity != 0)
                {
                    idea.ComplexityId = IVM.Complexity;
                }
                if (IVM.LOBId != 0)
                {
                    idea.LOBId = IVM.LOBId;
                }
                if (IVM.SystemImpactedId != 0)
                {
                    idea.SystemImpactedId = IVM.SystemImpactedId;
                }
                //Other idea Details
                idea.ProposedBy = IVM.ProposedBy;
                idea.CreatedBy = uid;
                idea.CreatedDate = DateTime.Now;//Need to do Offset.
                idea.LastUpdatedDate = DateTime.Now;//Need to do Offset.
                idea.StatusId = (int)IMStatus.Created;
                idea.ClientId = IVM.ClientId;
                //idea.UpdatedBy = uid;
                idea.StatusDt = DateTime.Now;
                idea.MultipleApprovals = IVM.MultipleApprovals;
                unitOfWork.IMPostIdea.Insert(idea);
                unitOfWork.Save();

                IMIdeaStatusHistory IMIdeaStatusHistory = new IMIdeaStatusHistory()
                {
                    IdeaId = idea.Id,
                    NewStatusId = idea.StatusId,
                    IsDeleted = false,
                    CreatedBy = uid,
                    UpdatedBy = uid,
                    LastUpdatedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                unitOfWork.IMIdeaStatusHistory.Insert(IMIdeaStatusHistory);

                unitOfWork.Save();
                IVM.IdeaId = idea.Id;

                //if (IVM.IMAutoApprovalSelection)
                //{
                //    var listOfAdmins = unitOfWork.IMAutoApprovers.Get().Where(e => e.ClientId == IVM.ClientId).ToList();
                //    var temp = "";
                //    foreach (var item in listOfAdmins)
                //    {
                //        temp += item.ApproverId + ",";
                //    }
                //    var adminlistlength = temp.Length - 1;
                //    var adminlist = temp.Substring(0, adminlistlength);
                //    string[] SelectedAdminIds = adminlist.Split(',');
                //    var OldStatus = (int)IMStatus.Created;
                //    idea.StatusId = (int)IMStatus.SubmitForReview;
                //    idea.StatusDt = DateTime.Now;
                //    idea.LastUpdatedDate = DateTime.Now;
                //    idea.UpdatedBy = uid;
                //    unitOfWork.IMPostIdea.Update(idea);
                //    IMIdeaStatusHistory cv = new IMIdeaStatusHistory();
                //    cv.Comments = "";
                //    cv.IdeaId =idea.Id ;
                //    cv.OldStatusId = OldStatus;
                //    cv.NewStatusId = idea.StatusId;
                //    cv.CreatedBy = uid;
                //    cv.UpdatedBy = uid;
                //    cv.CreatedDate = DateTime.Now;
                //    cv.LastUpdatedDate = DateTime.Now;
                //    unitOfWork.IMIdeaStatusHistory.Insert(cv);
                //    var deleteApprovers = from details in db.IMIdeaApprover
                //                          where details.IdeaId == idea.Id
                //                          select details;

                //    foreach (var detail in deleteApprovers)
                //    {
                //        unitOfWork.IMIdeaApprover.Delete(detail.Id);

                //    }
                //    foreach (var item in SelectedAdminIds)
                //    {
                //        if (item != " " || item != "," || item != "")
                //        {
                //            var ApproverId = Convert.ToInt32(item);
                //            IMIdeaApprover ideaApprover = new IMIdeaApprover();
                //            ideaApprover.IdeaId = Convert.ToInt32(idea.Id);
                //            ideaApprover.ApproverId = ApproverId;
                //            ideaApprover.Createdby = uid;
                //            ideaApprover.CreatedDt = DateTime.Now;
                //            unitOfWork.IMIdeaApprover.Insert(ideaApprover);
                //            var u = unitOfWork.UserProfile.Get(g => g.Id == ideaApprover.ApproverId).FirstOrDefault();
                //            new PostIdeaController().SendIdeaNotification(u.Email, u.FirstName + " " + u.LastName, idea.ItemName, IMStatus.SubmitForReview, ClientName, IdeaLink);

                //        }
                //    }
                //    }
                //unitOfWork.Save();
                return IVM;
            }
            //If it is Edit
            else
            {
                var idea = unitOfWork.IMPostIdea.GetByID(IVM.IdeaId);
                var TempStatusId = idea.StatusId;
                idea.ItemName = IVM.IdeaName.Trim();
                idea.IdeaSource = IVM.IdeaSourceId;
                idea.InternalInitiative = IVM.InternalInit;
                idea.TeamId = IVM.TeamId;
                idea.Description = IVM.Description.Trim();
                idea.ImprovementTheme = IVM.ImprovementTheme;
                if (IVM.QtyFication != 0)
                {
                    idea.QtyFication = IVM.QtyFication;
                }
                if (IVM.IdeatypeId != 0)
                {
                    idea.IdeaTypeId = IVM.IdeatypeId;
                }
                if (IVM.Complexity != 0)
                {
                    idea.ComplexityId = IVM.Complexity;
                }
                if (IVM.LOBId != 0)
                {
                    idea.LOBId = IVM.LOBId;
                }
                if (IVM.SystemImpactedId != 0)
                {
                    idea.SystemImpactedId = IVM.SystemImpactedId;
                }
                //Other idea Details
                idea.ProposedBy = IVM.ProposedBy;
                idea.UpdatedBy = uid;
                idea.LastUpdatedDate = DateTime.Now;//Need to do Offset.
                if (idea.StatusId == (int)IMStatus.SubmitForRevision)
                {
                    idea.StatusId = (int)IMStatus.Revised;
                    idea.StatusDt = DateTime.Now;
                }
                unitOfWork.IMPostIdea.Update(idea);
                IMIdeaStatusHistory IMIdeaStatusHistory = new IMIdeaStatusHistory()
                {
                    IdeaId = IVM.IdeaId,
                    OldStatusId = TempStatusId,
                    NewStatusId = idea.StatusId,
                    IsDeleted = false,
                    CreatedBy = uid,
                    UpdatedBy = uid,
                    LastUpdatedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                unitOfWork.IMIdeaStatusHistory.Insert(IMIdeaStatusHistory);


                unitOfWork.Save();
                return IVM;
            }
        }


        //public static void saveImpDetails(int IdeaId, int StatusId, int impPriority, int impManager,DateTime? LastReviewDt, DateTime? PSDt, DateTime? PEDt, DateTime? ASDt, DateTime? AEDt)
        public static void saveImpDetails(IdeaViewModel IVM, int uid, string ClientName, string IdeaLink)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());


            var ExistIdea = unitOfWork.IMPostIdea.GetByID(IVM.IdeaId);
            if (ExistIdea != null)
            {

                var TempOldStatus = ExistIdea.ImpStatusId;
                var TempOldPriority = ExistIdea.ImpPriorityId;
                var TempImpStatusComment = ExistIdea.StatusComment;
                //When Idea is just approved and Implementation  Manager is not Selected.
                if (ExistIdea.StatusId == (int)IMStatus.Approved && ExistIdea.ImpStatusId == null && IVM.ImpIdeaStatusID == null)
                {
                    ExistIdea.ImpPriorityId = IVM.ImpPriority;
                    if (IVM.ImpPriority != TempOldPriority)
                    {
                        IMImpPriorityHistory IMImpPriorityHistory = new IMImpPriorityHistory()
                        {
                            IdeaId = IVM.IdeaId,
                            OldImpPriority = TempOldPriority,
                            NewImpPriority = IVM.ImpPriority,
                            UpdatedBy = uid,
                            UpdatedDt = DateTime.Now
                        };
                        unitOfWork.IMImpPriorityHistory.Insert(IMImpPriorityHistory);
                    }
                    if (IVM.ImpManager != null && ExistIdea.ImplementationManager != IVM.ImpManager)
                    {
                        ExistIdea.ImplementationManager = IVM.ImpManager;
                        var up = unitOfWork.UserProfile.GetByID(IVM.ImpManager);
                        if (up != null)
                        {
                            new PostIdeaController().SendIdeaNotification(up.Email, up.FirstName + " " + up.LastName, ExistIdea.ItemName, IMStatus.SubmitForPlanning, ClientName, IdeaLink);
                        }
                        ExistIdea.ImpStatusId = (int)IMStatus.SubmitForPlanning;
                        ExistIdea.ImpStatusDt = DateTime.Now;
                        IMIdeaStatusHistory IMIdeaStatusHistory = new IMIdeaStatusHistory()
                        {
                            IdeaId = IVM.IdeaId,
                            OldStatusId = ExistIdea.StatusId,
                            NewStatusId = ExistIdea.ImpStatusId,
                            Comments = IVM.StatusComment,
                            IsDeleted = false,
                            CreatedBy = uid,
                            UpdatedBy = uid,
                            LastUpdatedDate = DateTime.Now,
                            CreatedDate = DateTime.Now
                        };
                        unitOfWork.IMIdeaStatusHistory.Insert(IMIdeaStatusHistory);
                    }
                    ExistIdea.LastReviewDt = IVM.LastReviewDt;
                    ExistIdea.LastUpdatedDate = DateTime.Now;
                    ExistIdea.UpdatedBy = uid;
                    if (IVM.StatusComment != null)
                    {
                        ExistIdea.StatusComment = IVM.StatusComment;
                    }
                    unitOfWork.IMPostIdea.Update(ExistIdea);
                    // unitOfWork.Save();
                }
                //When Updating data with no Implementation Status selected.
                else if (ExistIdea.ImpStatusId == (int)IMStatus.SubmitForPlanning && IVM.ImpIdeaStatusID == null)
                {
                    ExistIdea.ImpPriorityId = IVM.ImpPriority;
                    if (IVM.ImpPriority != TempOldPriority)
                    {
                        IMImpPriorityHistory IMImpPriorityHistory = new IMImpPriorityHistory()
                        {
                            IdeaId = IVM.IdeaId,
                            OldImpPriority = TempOldPriority,
                            NewImpPriority = IVM.ImpPriority,
                            UpdatedBy = uid,
                            UpdatedDt = DateTime.Now
                        };
                        unitOfWork.IMImpPriorityHistory.Insert(IMImpPriorityHistory);
                    }

                    if (IVM.ImpManager != null && ExistIdea.ImplementationManager != IVM.ImpManager)
                    {
                        ExistIdea.ImplementationManager = IVM.ImpManager;
                        var up = unitOfWork.UserProfile.GetByID(IVM.ImpManager);
                        if (up != null)
                        {
                            new PostIdeaController().SendIdeaNotification(up.Email, up.FirstName + " " + up.LastName, ExistIdea.ItemName, (IMStatus)ExistIdea.ImpStatusId, ClientName, IdeaLink);
                        }
                    }
                    ExistIdea.LastReviewDt = IVM.LastReviewDt;
                    ExistIdea.LastUpdatedDate = DateTime.Now;
                    ExistIdea.UpdatedBy = uid;
                    if (IVM.StatusComment != null)
                    {
                        ExistIdea.StatusComment = IVM.StatusComment;
                    }
                    unitOfWork.IMPostIdea.Update(ExistIdea);
                    //unitOfWork.Save();
                }
                //With Status selected.
                else
                {
                    if (IVM.ImpIdeaStatusID == (int)IMStatus.Implemented)
                    {
                        ExistIdea.CompletedDt = DateTime.Now;
                    }

                    if (IVM.ImpIdeaStatusID == (int)IMStatus.Cancelled)
                    {
                        ExistIdea.StatusId = IVM.ImpIdeaStatusID.Value;
                        ExistIdea.StatusDt = DateTime.Now;
                    }
                    else
                    {
                        ExistIdea.ImpStatusId = IVM.ImpIdeaStatusID;
                        ExistIdea.ImpStatusDt = DateTime.Now;
                    }
                    if (ExistIdea.ImpStatusId == (int)IMStatus.Hold || ExistIdea.StatusId == (int)IMStatus.Cancelled)
                    {
                        //send Email Notification
                        var up = unitOfWork.UserProfile.GetByID(ExistIdea.ApprovedBy);
                        if (up != null)
                        {
                            if (ExistIdea.StatusId == (int)IMStatus.Cancelled)
                            {
                                new PostIdeaController().SendIdeaNotification(up.Email, up.FirstName + " " + up.LastName, ExistIdea.ItemName, (IMStatus)ExistIdea.StatusId, ClientName, IdeaLink);
                            }
                            else
                            {
                                new PostIdeaController().SendIdeaNotification(up.Email, up.FirstName + " " + up.LastName, ExistIdea.ItemName, (IMStatus)ExistIdea.ImpStatusId, ClientName, IdeaLink);
                            }
                        }
                    }

                    ExistIdea.ImpPriorityId = IVM.ImpPriority;
                    //--HIstory Table..
                    if (IVM.ImpPriority != TempOldPriority)
                    {
                        IMImpPriorityHistory IMImpPriorityHistory = new IMImpPriorityHistory()
                        {
                            IdeaId = IVM.IdeaId,
                            OldImpPriority = TempOldPriority,
                            NewImpPriority = IVM.ImpPriority,
                            UpdatedBy = uid,
                            UpdatedDt = DateTime.Now
                        };
                        unitOfWork.IMImpPriorityHistory.Insert(IMImpPriorityHistory);
                    }

                    if (IVM.ImpManager != null && ExistIdea.ImplementationManager != IVM.ImpManager)
                    {
                        ExistIdea.ImplementationManager = IVM.ImpManager;
                        var up = unitOfWork.UserProfile.GetByID(IVM.ImpManager);
                        if (up != null)
                        {
                            new PostIdeaController().SendIdeaNotification(up.Email, up.FirstName + " " + up.LastName, ExistIdea.ItemName, (IMStatus)ExistIdea.ImpStatusId, ClientName, IdeaLink);
                        }
                    }
                    ExistIdea.LastReviewDt = IVM.LastReviewDt;
                    ExistIdea.PlannedStartDate = IVM.PlannedStartDt;
                    ExistIdea.PlannedEndDate = IVM.PlannedEndDt;
                    ExistIdea.ActualEndDate = IVM.ActualEndDt;
                    ExistIdea.ImplementationProgress = (IVM.ImpIdeaStatusID == 4) ? 100 : IVM.ActualProgress;
                    ExistIdea.ActualStartDate = IVM.ActualStartDt;
                    ExistIdea.LastUpdatedDate = DateTime.Now;
                    ExistIdea.UpdatedBy = uid;
                    if (IVM.StatusComment != null)
                    {
                        ExistIdea.StatusComment = IVM.StatusComment;
                    }
                    unitOfWork.IMPostIdea.Update(ExistIdea);

                    //Status History and Priority history
                    if (IVM.ImpIdeaStatusID != TempOldStatus)
                    {
                        IMIdeaStatusHistory IMIdeaStatusHistory = new IMIdeaStatusHistory()
                        {
                            IdeaId = IVM.IdeaId,
                            OldStatusId = TempOldStatus,
                            NewStatusId = IVM.ImpIdeaStatusID,
                            Comments = IVM.StatusComment,
                            IsDeleted = false,
                            CreatedBy = uid,
                            UpdatedBy = uid,
                            LastUpdatedDate = DateTime.Now,
                            CreatedDate = DateTime.Now
                        };
                        unitOfWork.IMIdeaStatusHistory.Insert(IMIdeaStatusHistory);
                    }
                    else if (IVM.StatusComment != TempImpStatusComment)
                    {
                        IMIdeaStatusHistory IMIdeaStatusHistory = new IMIdeaStatusHistory()
                        {
                            // Id= checkComment.Id,
                            IdeaId = IVM.IdeaId,
                            OldStatusId = TempOldStatus,
                            NewStatusId = IVM.ImpIdeaStatusID,
                            Comments = IVM.StatusComment,
                            IsDeleted = false,
                            CreatedBy = uid,
                            UpdatedBy = uid,
                            LastUpdatedDate = DateTime.Now,


                        };
                        unitOfWork.IMIdeaStatusHistory.Insert(IMIdeaStatusHistory);
                    }

                    unitOfWork.Save();
                }

                if (ExistIdea.ActionitemId != null || ExistIdea.ActionitemId > 0)
                {
                    var updateString = "";
                    ActionItemTracker ait1 = unitOfWork.ActionItemTracker.GetByID(ExistIdea.ActionitemId);
                    if (IVM.PlannedEndDt != null)
                    {
                        if (ait1.PlannedCompletionDate == null)
                        {
                            updateString = updateString + ", Planned Completion Date - " + IVM.PlannedEndDt.Value.AddDays(1).ToString("MM/dd/yyyy") + "  ";

                        }
                        else
                        {
                            var plannedCompletionDateString = IVM.PlannedEndDt.Value.AddDays(1).ToString("MM/dd/yyyy");
                            updateString = updateString + ", Planned Completion Date changed from " + IVM.PlannedEndDt.Value.AddDays(1).ToString("MM/dd/yyyy") + " to " + plannedCompletionDateString + "  ";
                        }
                    }
                    ait1.PlannedCompletionDate = IVM.PlannedEndDt;
                    var tempstatus = ait1.Status;
                    if (IVM.ImpIdeaStatusID == 17)
                    {
                        ait1.Status = "In Progress";
                    }
                    if (IVM.ImpIdeaStatusID == 5)
                    {
                        ait1.Status = "Cancelled";
                    }
                    if (IVM.ImpIdeaStatusID == 18)
                    {
                        ait1.Status = "Hold";
                    }
                    if (IVM.ImpIdeaStatusID == 4)
                    {
                        ait1.Status = "Completed";

                    }
                    if (tempstatus != ait1.Status)
                    {
                        updateString = updateString + ", Status - " + ait1.Status + "  ";
                    }


                    if (IVM.StatusComment != null)
                    {
                        if (ait1.Comments != null)
                        {
                            updateString = updateString + ", Comments updated from " + ait1.Comments + " to " + IVM.StatusComment + "   ";
                        }
                        else
                        {
                            updateString = updateString + ", Comments - " + IVM.StatusComment + "   ";

                        }
                    }
                    ait1.Comments = IVM.StatusComment;
                    if (updateString != "")
                    {
                        ActionItemTrackerLog alog = new ActionItemTrackerLog();
                        alog.ActionItemID = ait1.Id;
                        alog.Updates = updateString;
                        alog.ClientID = ait1.ClientID;
                        alog.updateDate = DateTime.Now;
                        unitOfWork.ActionItemTrackerLog.Insert(alog);
                    }
                    unitOfWork.ActionItemTracker.Update(ait1);
                    unitOfWork.Save();

                }
            }
        }

        public static IdeaViewModel getIdeaDetails(int IdeaId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            IdeaViewModel cv = new IdeaViewModel();
            var iDeatils = unitOfWork.IMPostIdea.GetByID(IdeaId);
            var analysis = unitOfWork.IMAnalysisRootCause.Get(e => e.IdeaId == IdeaId).FirstOrDefault();
            var solution = unitOfWork.IMSolution.Get(e => e.IdeaId == IdeaId).FirstOrDefault();
            if (analysis != null && analysis.Analysis.Trim() != "")
            {
                cv.Analysis = analysis.Analysis;
            }
            if (solution != null && solution.Solution.Trim() != "")
            {
                cv.Solution = solution.Solution;
            }
            cv.IdeaName = iDeatils.ItemName;
            cv.IdeaId = iDeatils.Id;
            cv.IdeaNumber = iDeatils.IdeaNumber;
            cv.IdeaStatusID = iDeatils.StatusId;
            cv.ImpIdeaStatusID = iDeatils.ImpStatusId;
            cv.IsDeleted = iDeatils.IsDeleted;
            var temp = (IMStatus)iDeatils.StatusId;
            cv.IdeaStatus = temp.ToString();

            if (iDeatils.ImpStatusId != null)
            {
                var temp1 = (IMStatus)iDeatils.ImpStatusId.Value;
                cv.IdeaStatus = temp1.ToString();
            }
            if (iDeatils.StatusId == (int)IMStatus.Cancelled)
            {
                var temp1 = (IMStatus)iDeatils.StatusId;
                cv.IdeaStatus = temp1.ToString(); /*"Cancelled";*/
                cv.ImpIdeaStatusID = iDeatils.StatusId;
            }
            cv.QtyFication = iDeatils.QtyFication;
            cv.SystemImpactedId = iDeatils.SystemImpactedId;
            cv.ProposedBy = iDeatils.ProposedBy;
            var up = unitOfWork.UserProfile.GetByID(iDeatils.CreatedBy);
            var createdby = " ";
            if (up != null)
                createdby = up.FirstName + " " + up.LastName;
            cv.CreatedByName = iDeatils.ProposedBy != null ? createdby + "/ " + iDeatils.ProposedBy : createdby;

            cv.IdeatypeId = iDeatils.IdeaTypeId;
            cv.IdeaSourceId = iDeatils.IdeaSource;
            cv.LOBId = iDeatils.LOBId;
            cv.Complexity = iDeatils.ComplexityId;
            cv.InternalInit = iDeatils.InternalInitiative;
            cv.TeamId = iDeatils.TeamId;
            cv.Description = iDeatils.Description;
            cv.ImprovementTheme = iDeatils.ImprovementTheme;
            //Imp Priority
            cv.ImpPriority = iDeatils.ImpPriorityId;
            cv.ImpManager = iDeatils.ImplementationManager;

            //need to add Analysis and Solution
            cv.LastReviewDt = iDeatils.LastReviewDt;
            cv.PlannedStartDt = iDeatils.PlannedStartDate;
            cv.PlannedEndDt = iDeatils.PlannedEndDate;
            cv.ActualStartDt = iDeatils.ActualStartDate;
            cv.ActualProgress = iDeatils.ImplementationProgress;
            cv.ActualEndDt = iDeatils.ActualEndDate;
            //to read overall implementation status
            cv.StatusComment = iDeatils.StatusComment;
            //calc
            ////cv.ActualProgress = "";
            ////cv.TargetProgress = "";
            ////cv.ImplementationHealth = "";

            cv.CreatedBy = iDeatils.CreatedBy;
            cv.ApprovedBy = iDeatils.ApprovedBy;
            if (iDeatils.StatusId == (int)IMStatus.SubmitForReview || iDeatils.StatusId == (int)IMStatus.SubmitForApproval)
            {
                var approverList = unitOfWork.IMIdeaApprover.Get(g => g.IdeaId == IdeaId);
                if (approverList.Where(g => g.ApprovedDate != null).Any())
                    cv.IdeaStatus = "Pending for Approval";
            }


            return cv;
        }


        /// <summary>
        /// Delete Ideas
        /// </summary>
        /// <param name="IdeaIDs"></param>
        /// <param name="ClientId"></param>
        /// <param name="uid"></param>
        public static void deleteideas(string IdeaIDs, int ClientId, int uid)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

            string[] SelectedIdeaIds = IdeaIDs.Split(',');
            foreach (var i in SelectedIdeaIds)
            {
                var _ideaId = Convert.ToInt32(i);
                var checkStatus = unitOfWork.IMPostIdea.Get().Where(e => e.Id == _ideaId && e.CreatedBy == uid && e.StatusId == (int)IMStatus.Created).Any();
                if (checkStatus)
                {
                    var idea = unitOfWork.IMPostIdea.GetByID(_ideaId);
                    if (!idea.IsDeleted)
                    {
                        idea.IsDeleted = true;
                        idea.LastUpdatedDate = DateTime.Now;
                        idea.UpdatedBy = uid;
                        unitOfWork.IMPostIdea.Update(idea);
                        unitOfWork.Save();
                    }
                }
            }
        }


        //---------------------------------------------------------------STATUS Change---------------------------------------------------------------//


        public static void submitforreview(string IdeaIDs, string ClientName, int uid, string IdeaLink)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

            string[] SelectedIdeaIds = IdeaIDs.Split(',');
            foreach (var i in SelectedIdeaIds)
            {
                var _ideaId = Convert.ToInt32(i);
                // (e.StatusId == (int)IMStatus.SubmitForRevision || e.StatusId == (int)IMStatus.Created || ) --- [Removed]
                // -->  Removed: Idea with created Status without Approver cannot be Submitted for Review
                var ExistIdea = unitOfWork.IMPostIdea.GetByID(_ideaId);
                var checkStatus = unitOfWork.IMPostIdea.Get().Where(e => e.Id == _ideaId && (e.StatusId == (int)IMStatus.Revised || e.StatusId == (int)IMStatus.Created)).Any();
                var checkApproverExists = unitOfWork.IMIdeaApprover.Get(e => e.IdeaId == _ideaId).Any();
                if (checkStatus && checkApproverExists)
                {
                    var idea = unitOfWork.IMPostIdea.GetByID(_ideaId);
                    var oldStatus = idea.StatusId;
                    idea.StatusId = (int)IMStatus.SubmitForReview;
                    idea.StatusDt = DateTime.Now;
                    unitOfWork.IMPostIdea.Update(idea);

                    IMIdeaStatusHistory statusHistory = new IMIdeaStatusHistory();
                    statusHistory.IdeaId = idea.Id;
                    statusHistory.LastUpdatedDate = DateTime.Now;
                    statusHistory.UpdatedBy = uid;
                    statusHistory.OldStatusId = oldStatus;
                    statusHistory.NewStatusId = idea.StatusId;
                    unitOfWork.IMIdeaStatusHistory.Insert(statusHistory);

                    unitOfWork.Save();
                    var Approvers = unitOfWork.IMIdeaApprover.Get().Where(e => e.IdeaId == _ideaId);
                    if (Approvers.Count() != 0)
                    {
                        IdeaLink = IdeaLink.Trim() + "&IdeaId=" + _ideaId.ToString();
                        foreach (var appr in Approvers)
                        {
                            var up = unitOfWork.UserProfile.GetByID(appr.ApproverId);
                            if (up != null)
                            {
                                new PostIdeaController().SendIdeaNotification(up.Email, up.FirstName + " " + up.LastName, ExistIdea.ItemName, IMStatus.SubmitForReview, ClientName, IdeaLink);
                            }
                        }
                    }
                }
            }
        }



        public static void submitforapproval(string IdeaIDs, int ClientId, int uid)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

            string[] SelectedIdeaIds = IdeaIDs.Split(',');
            foreach (var i in SelectedIdeaIds)
            {
                var _ideaId = Convert.ToInt32(i);
                var checkStatus = unitOfWork.IMPostIdea.Get().Where(e => e.Id == _ideaId && e.StatusId == (int)IMStatus.SubmitForReview).Any();
                if (checkStatus)
                {
                    var idea = unitOfWork.IMPostIdea.GetByID(_ideaId);
                    var oldStatus = idea.StatusId;
                    idea.StatusId = (int)IMStatus.SubmitForApproval;
                    idea.StatusDt = DateTime.Now;
                    unitOfWork.IMPostIdea.Update(idea);

                    IMIdeaStatusHistory statusHistory = new IMIdeaStatusHistory();
                    statusHistory.IdeaId = idea.Id;
                    statusHistory.LastUpdatedDate = DateTime.Now;
                    statusHistory.UpdatedBy = uid;
                    statusHistory.OldStatusId = oldStatus;
                    statusHistory.NewStatusId = idea.StatusId;
                    unitOfWork.IMIdeaStatusHistory.Insert(statusHistory);

                    unitOfWork.Save();
                }
            }
        }









        //---------------------------------------------------------------Different Tabs---------------------------------------------------------------//


        public static IList<IMItemBusinessBenefitsInput> GetBusinessBenefitsDetails(int Id)
        {

            IList<IMItemBusinessBenefitsInput> BusinessBenefit = new List<IMItemBusinessBenefitsInput>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<IMItemBusinessBenefits> BusinessBenefitDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (Id != 0)
                    BusinessBenefitDetails = unitOfWork.IMItemBusinessBenefits.Get(cid => cid.ItemId == Id).ToList();

                if (BusinessBenefitDetails != null)
                {
                    foreach (var _businessdetail in BusinessBenefitDetails)
                    {
                        IMItemBusinessBenefitsInput bb = new IMItemBusinessBenefitsInput();
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
        public static int CreateBusinessBenefits(IMItemBusinessBenefitsInput imItemBusinessBenefits, int Id)
        {
            int id = 0;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var businessDetails = new IMItemBusinessBenefits();
                    businessDetails.ItemId = Id;
                    businessDetails.Name = imItemBusinessBenefits.Name;
                    businessDetails.Detail = imItemBusinessBenefits.Detail;
                    businessDetails.BenefitType = imItemBusinessBenefits.BenefitType;
                    businessDetails.IncidentCount = imItemBusinessBenefits.IncidentCount;
                    businessDetails.Value = imItemBusinessBenefits.Value;
                    businessDetails.FTEHours = imItemBusinessBenefits.FTEHours;
                    businessDetails.CreatedDate = DateTime.Now;
                    businessDetails.LastUpdatedDate = DateTime.Now;
                    businessDetails.CreatedBy = imItemBusinessBenefits.CreatedBy;
                    businessDetails.UpdatedBy = businessDetails.CreatedBy;
                    unitOfWork.IMItemBusinessBenefits.Insert(businessDetails);
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

        public static bool UpdateBusinessBenefits(IMItemBusinessBenefitsInput imItemBusinessBenefits)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.IMItemBusinessBenefits.GetByID(imItemBusinessBenefits.Id);
                    updatetitem.Name = imItemBusinessBenefits.Name;
                    updatetitem.Detail = imItemBusinessBenefits.Detail;
                    updatetitem.BenefitType = imItemBusinessBenefits.BenefitType;
                    updatetitem.IncidentCount = imItemBusinessBenefits.IncidentCount;
                    updatetitem.Value = imItemBusinessBenefits.Value;
                    updatetitem.FTEHours = imItemBusinessBenefits.FTEHours;
                    updatetitem.UpdatedBy = imItemBusinessBenefits.UpdatedBy;
                    updatetitem.LastUpdatedDate = DateTime.Now;
                    unitOfWork.IMItemBusinessBenefits.Update(updatetitem);
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
                unitOfWork.IMItemBusinessBenefits.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }

        /*---------------------------IT SAVINGS-----------------------------------*/


        public static IList<IMItemITBenefitsInput> GetITSavingsDetails(int Id)
        {

            IList<IMItemITBenefitsInput> ITSavings = new List<IMItemITBenefitsInput>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<IMItemITBenefits> ITSavingsDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (Id != 0)
                    ITSavingsDetails = unitOfWork.IMItemITBenefits.Get(cid => cid.ItemId == Id).ToList();

                if (ITSavingsDetails != null)
                {
                    foreach (var _itsavingsdetail in ITSavingsDetails)
                    {
                        IMItemITBenefitsInput ib = new IMItemITBenefitsInput();
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
        public static int CreateITSavings(IMItemITBenefitsInput imItemITBenefitsInput, int Id)
        {
            int id = 0;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var itsavingsDetails = new IMItemITBenefits();
                    itsavingsDetails.ItemId = Id;
                    itsavingsDetails.Name = imItemITBenefitsInput.Name;
                    itsavingsDetails.Detail = imItemITBenefitsInput.Detail;
                    itsavingsDetails.BenefitType = imItemITBenefitsInput.BenefitType;
                    itsavingsDetails.Value = imItemITBenefitsInput.Value;
                    itsavingsDetails.FTEHours = imItemITBenefitsInput.FTEHours;
                    itsavingsDetails.IncidentCount = imItemITBenefitsInput.IncidentCount;
                    itsavingsDetails.CreatedDate = DateTime.Now;
                    itsavingsDetails.CreatedBy = imItemITBenefitsInput.CreatedBy;
                    itsavingsDetails.LastUpdatedDate = DateTime.Now;
                    itsavingsDetails.UpdatedBy = itsavingsDetails.CreatedBy;
                    unitOfWork.IMItemITBenefits.Insert(itsavingsDetails);
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

        public static bool UpdateITSavings(IMItemITBenefitsInput imItemITBenefitsInput)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.IMItemITBenefits.GetByID(imItemITBenefitsInput.Id);
                    updatetitem.Name = imItemITBenefitsInput.Name;
                    updatetitem.Detail = imItemITBenefitsInput.Detail;
                    updatetitem.BenefitType = imItemITBenefitsInput.BenefitType;
                    updatetitem.Value = imItemITBenefitsInput.Value;
                    updatetitem.FTEHours = imItemITBenefitsInput.FTEHours;
                    updatetitem.IncidentCount = imItemITBenefitsInput.IncidentCount;
                    updatetitem.UpdatedBy = imItemITBenefitsInput.UpdatedBy;
                    updatetitem.LastUpdatedDate = DateTime.Now;
                    unitOfWork.IMItemITBenefits.Update(updatetitem);
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
                unitOfWork.IMItemITBenefits.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }


        /*---------------------------Risk And Mitigation-----------------------------------*/


        public static IList<IMRiskAndMitigationModel> GetRiskAndMitigationDetails(int Id)
        {

            IList<IMRiskAndMitigationModel> RiskAndMitigation = new List<IMRiskAndMitigationModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<IMRiskAndMitigation> RiskAndMitigationDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (Id != 0)
                    RiskAndMitigationDetails = unitOfWork.IMRiskAndMitigation.Get(cid => cid.IdeaId == Id).ToList();

                if (RiskAndMitigationDetails != null)
                {
                    foreach (var _riskmitigationdetail in RiskAndMitigationDetails)
                    {
                        IMRiskAndMitigationModel ib = new IMRiskAndMitigationModel();
                        ib.Id = _riskmitigationdetail.Id;
                        ib.IdeaId = _riskmitigationdetail.IdeaId;
                        ib.CreatedDt = _riskmitigationdetail.CreatedDt;
                        ib.Risk = _riskmitigationdetail.Risk;
                        ib.MitigationPlan = _riskmitigationdetail.MitigationPlan;
                        ib.Comments = _riskmitigationdetail.Comments;
                        ib.CreatedBy = _riskmitigationdetail.CreatedBy;
                        ib.UpdatedBy = _riskmitigationdetail.UpdatedBy;
                        ib.UpdatedDt = _riskmitigationdetail.UpdatedDt;
                        RiskAndMitigation.Add(ib);
                    }
                }
                return RiskAndMitigation;
            }

        }
        public static int CreateRiskAndMitigation(IMRiskAndMitigationModel imRiskAndMitigationModel, int Id)
        {
            int id = 0;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var riskmitigationDetails = new IMRiskAndMitigation();
                    riskmitigationDetails.IdeaId = Id;
                    riskmitigationDetails.CreatedDt = DateTime.Now;
                    riskmitigationDetails.UpdatedDt = DateTime.Now;
                    riskmitigationDetails.Risk = imRiskAndMitigationModel.Risk;
                    riskmitigationDetails.MitigationPlan = imRiskAndMitigationModel.MitigationPlan;
                    riskmitigationDetails.Comments = imRiskAndMitigationModel.Comments;
                    riskmitigationDetails.CreatedBy = imRiskAndMitigationModel.CreatedBy;
                    riskmitigationDetails.UpdatedBy = riskmitigationDetails.CreatedBy;
                    unitOfWork.IMRiskAndMitigation.Insert(riskmitigationDetails);
                    unitOfWork.Save();
                    id = riskmitigationDetails.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool UpdateRiskAndMitigation(IMRiskAndMitigationModel imRiskAndMitigationModel)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.IMRiskAndMitigation.GetByID(imRiskAndMitigationModel.Id);
                    updatetitem.Risk = imRiskAndMitigationModel.Risk;
                    updatetitem.MitigationPlan = imRiskAndMitigationModel.MitigationPlan;
                    updatetitem.Comments = imRiskAndMitigationModel.Comments;
                    updatetitem.UpdatedBy = imRiskAndMitigationModel.UpdatedBy;
                    updatetitem.UpdatedDt = DateTime.Now;
                    unitOfWork.IMRiskAndMitigation.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveRiskAndMitigation(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.IMRiskAndMitigation.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }


        /*------------------------------COMMENTS-----------------------------------*/



        public static IList<IMItemCommentsInput> GetCommentsDetails(int Id)
        {
            IList<IMItemCommentsInput> Commentsss = new List<IMItemCommentsInput>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<IMItemComments> CommentsDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (Id != 0)
                    CommentsDetails = unitOfWork.IMItemComments.Get(cid => cid.ItemId == Id).ToList();

                if (CommentsDetails != null)
                {
                    foreach (var _commentsdetail in CommentsDetails)
                    {
                        IMItemCommentsInput cc = new IMItemCommentsInput();
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
        public static int CreateComments(IMItemCommentsInput imItemCommentsInput, int Id)
        {
            int id = 0;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var commentsDetails = new IMItemComments();
                    commentsDetails.ItemId = Id;
                    commentsDetails.Comments = imItemCommentsInput.Comments;
                    commentsDetails.CreatedDate = DateTime.Now;
                    commentsDetails.CreatedBy = imItemCommentsInput.CreatedBy;
                    commentsDetails.LastUpdatedDate = DateTime.Now;
                    commentsDetails.UpdatedBy = commentsDetails.CreatedBy;
                    unitOfWork.IMItemComments.Insert(commentsDetails);
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

        public static bool UpdateComments(IMItemCommentsInput imItemCommentsInput)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.IMItemComments.GetByID(imItemCommentsInput.Id);
                    updatetitem.Comments = imItemCommentsInput.Comments;
                    updatetitem.UpdatedBy = imItemCommentsInput.UpdatedBy;
                    updatetitem.LastUpdatedDate = DateTime.Now;
                    unitOfWork.IMItemComments.Update(updatetitem);
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
                unitOfWork.IMItemComments.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }

        //------------------------------Offline Approver-----------------------------------//



        public static IList<OfflineApproverModel> GetOfflineApproverDetails(int Id)
        {
            NTTData.AMOPortal.Data.Db db = new NTTData.AMOPortal.Data.Db();
            IList<OfflineApproverModel> offlapprmodel = new List<OfflineApproverModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<IMOfflineApprover> offlAppr = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (Id != 0)
                    offlAppr = unitOfWork.IMOfflineApprover.Get(cid => cid.IdeaId == Id).ToList();
         
                if (offlAppr != null)
                {
                    foreach (var apprdtl in offlAppr)
                    {
                        OfflineApproverModel cc = new OfflineApproverModel();
                        cc.Id = apprdtl.Id;
                        List<ApproverAttachment> ApproverAttachment = (from x in db.ApprovalAttachments
                                                                       where x.OffapprovalId == apprdtl.Id
                                                                       select new ApproverAttachment
                                                                       {
                                                                           Id = x.Id,
                                                                           filename = x.FileName,
                                                                           fileCont = x.AttachFile
                                                                       }).ToList();
                        cc.OffApprovalAttachment = ApproverAttachment;
                        cc.IdeaId = apprdtl.IdeaId;
                        cc.ApproverName = apprdtl.ApproverName;
                        cc.Comments = apprdtl.Comments;
                        cc.CreatedBy = apprdtl.CreatedBy;
                        cc.UpdatedBy = apprdtl.UpdatedBy;
                        cc.CreatedDate = apprdtl.CreatedDate;
                        cc.ApprovedOn = apprdtl.ApprovedOn;
                        cc.LastUpdatedDate = apprdtl.LastUpdatedDate;
                        offlapprmodel.Add(cc);
                    }
                }
                return offlapprmodel;
            }

        }
        public static int CreateOfflineApprdetail(OfflineApproverModel offlappr, int Id)
        {
            int id = 0;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var commentsDetails = new IMOfflineApprover();
                    commentsDetails.IdeaId = Id;
                    commentsDetails.ApproverName = offlappr.ApproverName;
                    commentsDetails.Comments = offlappr.Comments;
                    commentsDetails.CreatedDate = offlappr.CreatedDate;
                    commentsDetails.CreatedBy = offlappr.CreatedBy;
                    commentsDetails.LastUpdatedDate = DateTime.Now;
                    commentsDetails.UpdatedBy = offlappr.CreatedBy;
                    unitOfWork.IMOfflineApprover.Insert(commentsDetails);
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

        public static bool UpdateOfflineappr(OfflineApproverModel offlAppr)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.IMOfflineApprover.GetByID(offlAppr.Id);
                    updatetitem.Comments = offlAppr.Comments;
                    updatetitem.ApproverName = offlAppr.ApproverName;
                    updatetitem.CreatedDate = offlAppr.CreatedDate;
                    updatetitem.ApprovedOn = offlAppr.ApprovedOn;
                    updatetitem.UpdatedBy = offlAppr.UpdatedBy;
                    updatetitem.LastUpdatedDate = DateTime.Now;
                    unitOfWork.IMOfflineApprover.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool UpdateNewappr(IMIdeaReassignModel offlAppr)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.IMIdeaReassign_log.GetByID(offlAppr.Id);

                    updatetitem.NewOwner = Convert.ToInt32(offlAppr.ImpManagers);


                    unitOfWork.IMIdeaReassign_log.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveoffApprover(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.IMOfflineApprover.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }


        //---------------------------METRIC------------------------------------------//

        public static IList<MetricTargetModel> GetMetricTargetDetails(int Id)
        {
            IList<MetricTargetModel> metrictargetdetails = new List<MetricTargetModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IList<IMMetricTarget> MetricTargetDetail = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                if (Id != 0)
                    MetricTargetDetail = unitOfWork.IMMetricTarget.Get(c => c.ItemId == Id).ToList();

                if (MetricTargetDetail != null)
                {
                    foreach (var _metrictargetdetail in MetricTargetDetail)
                    {
                        MetricTargetModel MetricTargetModel = new MetricTargetModel();
                        MetricTargetModel.Id = _metrictargetdetail.Id;
                        MetricTargetModel.ItemId = _metrictargetdetail.ItemId;
                        MetricTargetModel.MetricTarget = _metrictargetdetail.MetricTarget;
                        MetricTargetModel.CreatedBy = _metrictargetdetail.CreatedBy;
                        var c = unitOfWork.UserProfile.GetByID(MetricTargetModel.CreatedBy);
                        if (c != null)
                        {
                            string ss = c.FirstName + " " + c.LastName;
                            MetricTargetModel.PostedBy = ss;
                        }
                        MetricTargetModel.CreatedDate = _metrictargetdetail.CreatedDate;
                        metrictargetdetails.Add(MetricTargetModel);
                    }
                }
                return metrictargetdetails;
            }
        }


        public static int CreateMetricTarget(MetricTargetModel metrictargetmodel, int Id)
        {
            int id = 0;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var surQ = new IMMetricTarget();
                    surQ.ItemId = Id;
                    surQ.MetricTarget = metrictargetmodel.MetricTarget;
                    surQ.CreatedDate = DateTime.Now;
                    surQ.UpdatedDate = DateTime.Now;
                    surQ.CreatedBy = metrictargetmodel.CreatedBy;
                    surQ.UpdatedBy = metrictargetmodel.UpdatedBy;
                    unitOfWork.IMMetricTarget.Insert(surQ);
                    unitOfWork.Save();
                    id = surQ.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }



        public static bool UpdateMetricTarget(MetricTargetModel metrictargetmodel)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.IMMetricTarget.GetByID(metrictargetmodel.Id);
                    updatetitem.MetricTarget = metrictargetmodel.MetricTarget;
                    updatetitem.UpdatedBy = metrictargetmodel.UpdatedBy;
                    updatetitem.UpdatedDate = DateTime.Now;
                    unitOfWork.IMMetricTarget.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveMetricTarget(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.IMMetricTarget.Delete(Id);
                unitOfWork.Save();

                //TempData["SLACreatedDeleted"] = "Deleted successfully.";
                return true;
            }
        }

        //--------------------------------------------Analysis and Solutions-----------------------------------------------//

        public static void saveAnalysis(int IdeaId, int uid, string EncodeString)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var AnalysisExists = unitOfWork.IMAnalysisRootCause.Get(g => g.IdeaId == IdeaId).FirstOrDefault();
            if (AnalysisExists != null)
            {
                AnalysisExists.Analysis = EncodeString;
                AnalysisExists.UpdatedBy = uid;
                AnalysisExists.UpdatedDt = DateTime.Now;
                unitOfWork.IMAnalysisRootCause.Update(AnalysisExists);
            }
            else
            {
                IMAnalysisRootCause inrc = new IMAnalysisRootCause
                {
                    IdeaId = IdeaId,
                    CreatedBy = uid,
                    Analysis = EncodeString,
                    CreatedDt = DateTime.Now,
                    UpdatedBy = uid,
                    UpdatedDt = DateTime.Now

                };
                unitOfWork.IMAnalysisRootCause.Insert(inrc);
            }
            unitOfWork.Save();
        }

        public static void saveSolution(int IdeaId, int uid, string EncodeString)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var SolutionExists = unitOfWork.IMSolution.Get(g => g.IdeaId == IdeaId).FirstOrDefault();
            if (SolutionExists != null)
            {
                SolutionExists.Solution = EncodeString;
                SolutionExists.UpdatedBy = uid;
                SolutionExists.UpdatedDt = DateTime.Now;
                unitOfWork.IMSolution.Update(SolutionExists);
            }
            else
            {
                IMSolution inrc = new IMSolution
                {
                    IdeaId = IdeaId,
                    CreatedBy = uid,
                    Solution = EncodeString,
                    CreatedDt = DateTime.Now,
                    UpdatedBy = uid,
                    UpdatedDt = DateTime.Now

                };
                unitOfWork.IMSolution.Insert(inrc);
            }
            unitOfWork.Save();
        }

        //--------------------------------------LOGS_---------------------------------------//


        public static IList<IMIdeaPriorityHistoryModel> GetPriorityHistoryDet(int Id)
        {

            IList<IMIdeaPriorityHistoryModel> PriorityHistory = new List<IMIdeaPriorityHistoryModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<IMIdeaPriorityHistory> PriorityHistoryDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (Id != 0)
                    PriorityHistoryDetails = unitOfWork.IMIdeaPriorityHistory.Get(cid => cid.IdeaId == Id).ToList();

                if (PriorityHistoryDetails != null)
                {
                    foreach (var _priorityhistorydetail in PriorityHistoryDetails)
                    {
                        IMIdeaPriorityHistoryModel ph = new IMIdeaPriorityHistoryModel();
                        ph.Id = _priorityhistorydetail.Id;
                        ph.IdeaId = _priorityhistorydetail.IdeaId;
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

        public static IList<IMImpPriorityHistoryModel> GetImpPriorityHistoryDetails(int Id)
        {

            IList<IMImpPriorityHistoryModel> IMPPriorityHistory = new List<IMImpPriorityHistoryModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<IMImpPriorityHistory> IMPPriorityHistoryDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (Id != 0)
                    IMPPriorityHistoryDetails = unitOfWork.IMImpPriorityHistory.Get(cid => cid.IdeaId == Id).ToList();

                if (IMPPriorityHistoryDetails != null)
                {
                    foreach (var _imppriorityhistorydetail in IMPPriorityHistoryDetails)
                    {
                        IMImpPriorityHistoryModel iph = new IMImpPriorityHistoryModel();
                        iph.Id = _imppriorityhistorydetail.Id;
                        iph.IdeaId = _imppriorityhistorydetail.IdeaId;
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
                        IMPPriorityHistory.Add(iph);
                    }
                }
                return IMPPriorityHistory;
            }

        }

         public static IList<IMIdeaReassignModel> GetReassignHistoryDetails(int Id)
        {

            IList<IMIdeaReassignModel> IMPPriorityHistory = new List<IMIdeaReassignModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<IMIdeaReassign_log> IMPPriorityHistoryDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (Id != 0)
                    IMPPriorityHistoryDetails = unitOfWork.IMIdeaReassign_log.Get(cid => cid.IdeaId == Id).ToList();

                if (IMPPriorityHistoryDetails != null)
                {
                    foreach (var _imppriorityhistorydetail in IMPPriorityHistoryDetails)
                    {
                        IMIdeaReassignModel iph = new IMIdeaReassignModel();
                        iph.Id = _imppriorityhistorydetail.Id;
                        iph.IdeaId = _imppriorityhistorydetail.IdeaId;
                        iph.NewOwner= _imppriorityhistorydetail.NewOwner;                       
                        iph.AssignedDateTime = _imppriorityhistorydetail.AssignedDateTime;                       
                        var c = unitOfWork.UserProfile.GetByID(iph.NewOwner);
                        if (c != null)
                        {
                            string ss = c.FirstName + " " + c.LastName;
                            iph.AssignedToUserName = ss;
                        }
                        iph.Owner = _imppriorityhistorydetail.Owner;
                        var c1 = unitOfWork.UserProfile.GetByID(iph.Owner);
                        if (c1 != null)
                        {
                            string ss = c1.FirstName + " " + c1.LastName;
                            iph.CreatedByName = ss;
                        }
                        iph.Status = "Ressigned";
                        IMPPriorityHistory.Add(iph);
                    }
                }
                return IMPPriorityHistory;
            }

        }


        public static IList<IMIdeaStatusHistoryModel> GetStatusHistoryDetails(int Id)
        {
            Db db = new Db();
            var statuslist = (from SH in db.IMIdeaStatusHistory
                              join user in db.UserProfiles on SH.UpdatedBy equals user.Id
                              join S in db.IMStatus on SH.OldStatusId equals S.Id into TempStatus
                              join S1 in db.IMStatus on SH.NewStatusId equals S1.Id into TempStatus1
                              where SH.IdeaId == Id
                              from old in TempStatus.DefaultIfEmpty()
                              from neew in TempStatus1.DefaultIfEmpty()
                              select new IMIdeaStatusHistoryModel
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



        public static List<IMCompletedAndNextSteps> getCompleted(int ideaid, int clientid)
        {
            Db db = new Db();
            //List<IMPostIdeaModel> list1 = (from im in db.IMPostIdea
            //                               Rjoin up in db.UserProfiles on im.ImplementationManager equals up.Id
            //                               where im.Id == ideadid && im.ClientId == clientid
            //                               select im).ToList();

            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var impidea = unitOfWork.IMCompletedAndNextSteps.Get(cid => cid.IdeaId == ideaid).ToList();
            return impidea;

        }

        public static int? getPost(int ideaid, int clientid)
        {
            Db db = new Db();
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var impidea = unitOfWork.IMPostIdea.Get(cid => cid.Id == ideaid && cid.ClientId == clientid).FirstOrDefault().ImplementationManager;
            return impidea;
        }

        //public static IList<IMPostIdeaModel> getPost1(int ideaid , int clientid)
        //{
        //    IList<IMPostIdeaModel> impost = new List<IMPostIdeaModel>();
        //    using (var contextFactory = new DbContextFactory())
        //    {
        //        IEnumerable<IMPostIdea> postStepDetails = null;
        //        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

        //        pos
        //    }
        //}

        public static IList<IMCompletedAndNextStepsModel> GetCompletedAndNextStepsDetails(int Id)
        {

            IList<IMCompletedAndNextStepsModel> completedAndNextSteps = new List<IMCompletedAndNextStepsModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<IMCompletedAndNextSteps> completedAndNextStepsDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (Id != 0)
                    completedAndNextStepsDetails = unitOfWork.IMCompletedAndNextSteps.Get(cid => cid.IdeaId == Id).ToList();

                if (completedAndNextStepsDetails != null)
                {
                    foreach (var _completedandnextstepsdetail in completedAndNextStepsDetails)
                    {
                        IMCompletedAndNextStepsModel cans = new IMCompletedAndNextStepsModel();
                        cans.Id = _completedandnextstepsdetail.Id;
                        cans.IdeaId = _completedandnextstepsdetail.IdeaId;
                        cans.UpdatedDt = _completedandnextstepsdetail.UpdatedDt;
                        cans.CompletedSteps = _completedandnextstepsdetail.CompletedSteps;
                        cans.ImpManagers = _completedandnextstepsdetail.ImpManagers != null ? (_completedandnextstepsdetail.ImpManagers).ToString() : 0.ToString();
                        cans.NextSteps = _completedandnextstepsdetail.NextSteps;
                        cans.Comments = _completedandnextstepsdetail.Comments;
                        cans.UpdatedBy = _completedandnextstepsdetail.UpdatedBy;
                        completedAndNextSteps.Add(cans);
                    }
                }
                return completedAndNextSteps;
            }

        }


        //public static List<IMCompletedAndNextStepsModel> getComp (int ideaid , int clientid)
        //{
        //    Db db = new Db();

        //}

        public static int CreateCompletedAndNextSteps(IMCompletedAndNextStepsModel imcompletedAndNextStepsModel, int Id)
        {
            int id = 0;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var cns = new IMCompletedAndNextSteps();
                    cns.IdeaId = Id;
                    cns.UpdatedDt = DateTime.Now;
                    cns.CompletedSteps = imcompletedAndNextStepsModel.CompletedSteps;
                    cns.ImpManagers = Convert.ToInt32(imcompletedAndNextStepsModel.ImpManagers);
                    cns.NextSteps = imcompletedAndNextStepsModel.NextSteps;
                    cns.Comments = imcompletedAndNextStepsModel.Comments;
                    cns.UpdatedBy = imcompletedAndNextStepsModel.UpdatedBy;
                    unitOfWork.IMCompletedAndNextSteps.Insert(cns);
                    unitOfWork.Save();
                    id = cns.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }



        public static bool UpdateCompletedAndNextSteps(IMCompletedAndNextStepsModel imcompletedAndNextStepsModel)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.IMCompletedAndNextSteps.GetByID(imcompletedAndNextStepsModel.Id);
                    updatetitem.UpdatedDt = DateTime.Now;
                    updatetitem.CompletedSteps = imcompletedAndNextStepsModel.CompletedSteps;
                    updatetitem.ImpManagers = Convert.ToInt32(imcompletedAndNextStepsModel.ImpManagers);
                    updatetitem.NextSteps = imcompletedAndNextStepsModel.NextSteps;
                    updatetitem.Comments = imcompletedAndNextStepsModel.Comments;
                    updatetitem.UpdatedBy = imcompletedAndNextStepsModel.UpdatedBy;
                    unitOfWork.IMCompletedAndNextSteps.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveCompletedAndNextSteps(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.IMCompletedAndNextSteps.Delete(Id);
                unitOfWork.Save();
                return true;
            }
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
                                where ASA.ApplicationName.ToLower() == "idea manager" && (UR.RoleName == "admin" || UR.RoleName == "sysadmin") && UA.ClientId == clientId
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

        //------------------------------Classification and Category (PostIdea)[Tab-2]-----------------------------------------------------//
        public static IList<IMClassification> _getClassification(int Clientid)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<IMClassification>(context);
                return varRepo.GetAll().Where(c => c.ClientId == Clientid).ToList();
            }
        }
        public static IList<vwIMclasswithcat> GetCatAndClassification(int clientid)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<vwIMclasswithcat>(context);
                return varRepo.Where(w => w.clientid == clientid).ToList();
            }
        }
        public static IList<IMItemCategoryMap> GetMapItemForItemId(int p)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<IMItemCategoryMap>(context);
                return varRepo.Where(c => c.ItemId == p).ToList();
            }
        }

        public static IEnumerable<TreeViewItemModel> GetClassCat(int ClientId, int IdeaId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            List<TreeViewItemModel> cc = new List<TreeViewItemModel>();
            //Existing Category for ideas
            var ExistCategoryIds = new List<int>();
            if (IdeaId != 0)
            {
                ExistCategoryIds = unitOfWork.IMItemCategoryMap.Get(e => e.ItemId == IdeaId).Select(f => f.CategoryId).ToList();
            }

            //getting Classification
            var Class = unitOfWork.IMClassification.Get().Where(r => r.ClientId == ClientId);
            if (Class != null)
            {
                foreach (var C in Class)
                {
                    TreeViewItemModel cls = new TreeViewItemModel();
                    //cls.Id = C.Id.ToString();
                    cls.Text = C.Classification;
                    var Categ = unitOfWork.IMCategory.Get().Where(e => e.ClassificationId == C.Id);
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
                            if (IdeaId != 0)
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
        public static IEnumerable<TreeViewItemModel> GetConfigType(int ClientId, int IdeaId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            List<TreeViewItemModel> cc = new List<TreeViewItemModel>();
            //Existing ConfigItems of ideas
            var ExistConfigItems = new List<int>();
            if (IdeaId != 0)
            {
                ExistConfigItems = unitOfWork.IMConfigItemsMap.Get(e => e.IdeaId == IdeaId).Select(f => f.ConfigItemId).ToList();
            }

            //getting ConFigType
            var CType = unitOfWork.IMConfigType.Get().Where(r => r.ClientId == ClientId);
            if (CType != null)
            {
                foreach (var C in CType)
                {
                    TreeViewItemModel ctype = new TreeViewItemModel();
                    //ctype.Id = C.Id.ToString();
                    ctype.Text = C.ConfigType;
                    var configItems = unitOfWork.IMConfigItem.Get().Where(e => e.ConfigTypeId == C.Id);
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
                            if (IdeaId != 0)
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
                                where ASA.ApplicationName.ToLower() == "idea manager" && (UR.RoleName == "implementationmanager" || UR.RoleName == "admin") && UA.ClientId == clientid
                                select UP).Distinct().OrderBy(x => x.FirstName).ToList();

            var AdminsWithName = listOfAdmins.Select(g =>
                new SelectListItem
                {
                    Text = g.FirstName + " " + g.LastName,
                    Value = g.Id.ToString()
                }).ToList();

            return AdminsWithName;
        }
        public static IEnumerable<SelectListItem> GetAllIMUser(int clientid)
        {
            Db db = new Db();
            var listOfAdmins = (from UA in db.UserApplications
                                join UP in db.UserProfiles on UA.UserId equals UP.Id
                                join ASA in db.AppStore on UA.ApplicationId equals ASA.Id
                                join UR in db.UserRoles on UA.RoleId equals UR.Id
                                where ASA.ApplicationName.ToLower() == "idea manager" && /*(UR.RoleName == "implementationmanager" || UR.RoleName == "admin") &&*/ UA.ClientId == clientid
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
        public static void AddClassificationCategory(int IdeaId, string CateIds, int uid)
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
                    IMItemCategoryMap CatMap = new IMItemCategoryMap();
                    CatMap.CategoryId = Convert.ToInt32(catId);
                    CatMap.ItemId = IdeaId;
                    CatMap.CreatedDate = DateTime.Now;
                    CatMap.LastUpdatedDate = DateTime.Now;
                    CatMap.UpdatedBy = uid;
                    unitOfWork.IMItemCategoryMap.Insert(CatMap);
                }
            }
            unitOfWork.Save();
        }

        public static void DeleteClassificationCategory(int IdeaId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var list = unitOfWork.IMItemCategoryMap.Get(e => e.ItemId == IdeaId);
            if (list != null)
            {
                foreach (var cateory in list)
                {
                    unitOfWork.IMItemCategoryMap.Delete(cateory.Id);
                }
                unitOfWork.Save();
            }
        }


        //--Configuration
        public static void AddConfigItem(int IdeaId, string ConfigIds, int uid)
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
                    IMConfigItemsMap ConfigMap = new IMConfigItemsMap();
                    //ConfigMap.ConfigTypeId = Convert.ToInt32(configId);
                    ConfigMap.ConfigItemId = Convert.ToInt32(configId);
                    ConfigMap.IdeaId = IdeaId;
                    ConfigMap.CreatedDt = DateTime.Now;
                    ConfigMap.UpdatedDt = DateTime.Now;
                    ConfigMap.CreatedBy = uid;
                    ConfigMap.UpdatedBy = uid;
                    unitOfWork.IMConfigItemsMap.Insert(ConfigMap);
                }
            }
            unitOfWork.Save();
        }

        public static void DeleteConfigItem(int IdeaId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var list = unitOfWork.IMConfigItemsMap.Get(e => e.IdeaId == IdeaId);
            if (list != null)
            {
                foreach (var config in list)
                {
                    unitOfWork.IMConfigItemsMap.Delete(config.Id);
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

        public static string[] getApprovers(int IdeaId)
        {
            Db db = new Db();
            var list = (from UP in db.UserProfiles
                        join IMAP in db.IMIdeaApprover on UP.Id equals IMAP.ApproverId
                        where IMAP.IdeaId == IdeaId
                        select UP.FirstName + " " + UP.LastName
                ).DefaultIfEmpty().ToArray();
            return list;
        }

        //-------------Mail--------------



        /// <summary>
        /// To change Idea Status to Revised when any data is modified.
        /// </summary>
        /// <param name="IdeaId"></param>
        /// <param name="uid">UserId</param>
        public static void Idea_Revised(int IdeaId, int uid)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var ExistIdea = unitOfWork.IMPostIdea.GetByID(IdeaId);
            if (ExistIdea != null)
            {
                if (ExistIdea.StatusId == (int)IMStatus.SubmitForRevision)
                {
                    ExistIdea.StatusId = (int)IMStatus.Revised;
                    ExistIdea.StatusDt = DateTime.Now;
                    ExistIdea.LastUpdatedDate = DateTime.Now;
                    ExistIdea.UpdatedBy = uid;
                    unitOfWork.IMPostIdea.Update(ExistIdea);

                    IMIdeaStatusHistory IMIdeaStatusHistory = new IMIdeaStatusHistory()
                    {
                        IdeaId = IdeaId,
                        OldStatusId = (int)IMStatus.SubmitForRevision,
                        NewStatusId = (int)IMStatus.Revised,
                        IsDeleted = false,
                        CreatedBy = uid,
                        UpdatedBy = uid,
                        LastUpdatedDate = DateTime.Now,
                        CreatedDate = DateTime.Now
                    };
                    unitOfWork.IMIdeaStatusHistory.Insert(IMIdeaStatusHistory);
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
            //Removing the IdeaId Column
            dataTable.Columns.Remove("IdeaId");

            //put a breakpoint here and check datatable
            return dataTable;
        }



        public static DataSet exportDataToDataSet(int ClientId, int UserId, bool IsAdmin, int AllIdeas = 1)
        {
            DataSet Ds = new DataSet();
            //My Ideas
            if (AllIdeas == 1)
            {
                //My Ideas

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

                //Completed Ideas
                var CompletedList = Completion(ClientId, UserId, IsAdmin).ToList();
                var dt5 = ToDataTable(CompletedList);
                Ds.Tables.Add(dt5);


                Ds.Tables[0].TableName = "Pre-Approval Ideas";
                Ds.Tables[1].TableName = "Submitted For Review Ideas";
                Ds.Tables[2].TableName = "Approve-Decline-Cancelled Ideas";
                Ds.Tables[3].TableName = "Under Implementation Ideas ";
                Ds.Tables[4].TableName = "Completed Ideas";
                return Ds;
            }
            //else - All Ideas
            else
            {
                //All Ideas

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

                //Completed Ideas
                var CompletedList = Completion(ClientId, false).ToList();
                var dt5 = ToDataTable(CompletedList);
                Ds.Tables.Add(dt5);

                Ds.Tables[0].TableName = "Pre-Approval Ideas";
                Ds.Tables[1].TableName = "Submitted For Review Ideas";
                Ds.Tables[2].TableName = "Approve-Decline-Cancelled Ideas";
                Ds.Tables[3].TableName = "Under Implementation Ideas ";
                Ds.Tables[4].TableName = "Completed Ideas";

                return Ds;
            }
        }


        public static IEnumerable<SelectListItem> getLOB(int ClientID)
        {
            //var ClientID = (Client)Session["Client"];
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var applicationList = unitOfWork.IMLOB.Get(x => x.ClientId == ClientID).ToList();
            var item = applicationList.OrderBy(x => x.Id).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.LOB.ToString() });
            return item;

        }

        public static List<ApproverModel> getApproversForView(int IdeaId)
        {
            Db db = new Db();
            var list = (from UP in db.UserProfiles
                        join IMAP in db.IMIdeaApprover on UP.Id equals IMAP.ApproverId
                        where IMAP.IdeaId == IdeaId
                        select new
                        {
                            fName = UP.FirstName,
                            lName = UP.LastName,
                            Date = IMAP.ApprovedDate
                        }
                ).ToList();
            //.DefaultIfEmpty()
            var asd = list.Select(v => new ApproverModel
            {
                FirstName = v.fName,
                LastName = v.lName,
                ApprovedDate = v.Date
            }).ToList();
            return asd;
        }

        //-----------------------------------Idea Authorize---------------------------------//
        /// <summary>
        /// To authorize the Idea part of Logged in Client in Session
        /// </summary>
        /// <param name="ClientID"></param>
        /// <param name="IdeaID"></param>
        /// <returns></returns>
        public static bool IdeaAuthorize(int ClientID, int IdeaID)
        {
            bool authorize = false;
            if (IdeaID != 0)
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    authorize = unitOfWork.IMPostIdea.Get().Where(f => f.ClientId == ClientID && f.Id == IdeaID).Any();
                }

                return authorize;
            }
            else
            {
                return false;
            }
        }

        //---------------------------------Ideas classification-------------------------------//
        public static List<IdeaViewModel> GetDetailClassification(int IdeaId)
        {
            Db db = new Db();
            var list = (from a in db.IMItemCategoryMap
                        join b in db.IMCategory on a.CategoryId equals b.Id
                        join c in db.IMClassification
                        on b.ClassificationId equals c.Id
                        where a.ItemId == IdeaId
                        select new
                        {
                            c.Classification,
                            b.Category
                        }).ToList();
            var q = list.Select(v => new IdeaViewModel
            {
                Classification = v.Classification,
                category = v.Category
            }).ToList();

            return q;
        }


        public static List<IdeaViewModel> GetDetailConfigItem(int IdeaId)
        {
            Db db = new Db();

            var list = (from a in db.IMConfigItemsMap
                        join b in db.IMConfigItem on a.ConfigItemId equals b.Id
                        join c in db.IMConfigType on b.ConfigTypeId equals c.Id
                        where a.IdeaId == IdeaId
                        select new
                        {
                            c.ConfigType,
                            b.ConfigItem
                        }).ToList();
            var q = list.Select(v => new IdeaViewModel
            {
                ConfigItem = v.ConfigItem,
                ConfigType = v.ConfigType
            }).ToList();

            return q;
        }
        public static bool isAutoApproved(int ClientId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var approversList = unitOfWork.Clients.Get().Where(f => f.Id == ClientId).FirstOrDefault().IMAutoApprovalSelection;
                return approversList;
            }
        }
        public static IList<IMAutoApproverModel> ListOfMetadataAdmins(int ClientId)
        {
            Db db = new Db();
            var listOfAdmins = (from UA in db.UserApplications
                                join UP in db.UserProfiles on UA.UserId equals UP.Id
                                join ASA in db.AppStore on UA.ApplicationId equals ASA.Id
                                join UR in db.UserRoles on UA.RoleId equals UR.Id
                                join APPRS in db.IMAutoApprovers.Where(w => w.ClientId == ClientId) on UP.Id equals APPRS.ApproverId into tmpApprs
                                from aprs in tmpApprs.DefaultIfEmpty()
                                where ASA.ApplicationName.ToLower() == "idea manager" && (UR.RoleName == "admin" || UR.RoleName == "sysadmin") && UA.ClientId == ClientId
                                select new IMAutoApproverModel
                                {
                                    Id = UP.Id,
                                    IdeaApproverId = aprs.Id,
                                    ApproverName = UP.FirstName + " " + UP.LastName
                                }).Distinct().ToList();
            return listOfAdmins;


        }


        //------------------------------------System Impacted service helper--------------------------------//
        public static IList<IMSystemImpactedServiceModal> GetSysImpactedDetails(int ClientId, int uid)
        {

            IList<IMSystemImpactedServiceModal> Teams = new List<IMSystemImpactedServiceModal>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<IMSystemImpactedService> SysImpactedDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (ClientId != 0)
                    SysImpactedDetails = unitOfWork.IMSystemImpactedService.Get(cid => cid.ClientId == ClientId).ToList();

                if (SysImpactedDetails != null)
                {
                    foreach (var _detail in SysImpactedDetails)
                    {
                        IMSystemImpactedServiceModal cs = new IMSystemImpactedServiceModal();
                        cs.Id = _detail.Id;
                        cs.ClientId = _detail.ClientId;
                        cs.SystemImpactedService = _detail.SystemImpactedService;

                        Teams.Add(cs);
                    }
                }
                return Teams;
            }

        }
        public static int CreateSysImpacted(IMSystemImpactedServiceModal imTeamModel)
        {
            int id = 0;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var tea = new IMSystemImpactedService();
                    tea.ClientId = imTeamModel.ClientId;
                    tea.SystemImpactedService = imTeamModel.SystemImpactedService;
                    unitOfWork.IMSystemImpactedService.Insert(tea);
                    unitOfWork.Save();
                    id = tea.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool UpdateSysImpacted(IMSystemImpactedServiceModal imTeamModel)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.IMSystemImpactedService.GetByID(imTeamModel.Id);
                    updatetitem.ClientId = imTeamModel.ClientId;
                    updatetitem.SystemImpactedService = imTeamModel.SystemImpactedService;
                    unitOfWork.IMSystemImpactedService.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveSysImpacted(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.IMSystemImpactedService.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }


        public static IList<IMIdeaReassignModel> GetIdeaReassignList(int ClientId)
        {

            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            IList<IMIdeaReassignModel> idealist = new List<IMIdeaReassignModel>();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new Data.UnitOfWorks(contextFactory.GetContext());
                dt = unitOfWork.GetDataFromStoredProcedure("IMAllIdeaReport_Reassign", paramName, paramValue);
                foreach (DataRow dr in dt.Rows)
                {
                    IMIdeaReassignModel idea = new IMIdeaReassignModel();

                    //  idea.IdeaId = (int)dr["Id"];
                    idea.ClientId = ClientId;
                    idea.IdeaNumber = (string)dr["IdeaNumber"];
                    idea.ItemName = (string)dr["Idea Name"];
                    idea.Status = (string)dr["Status"];
                    // idea.AssignedByUser = (int)dr["Owner"];
                    var creator = (string)dr["Created By"];
                    idea.ProposedBy = (string)dr["Proposed By"];
                    idea.CreatedByName = idea.ProposedBy != "-" ? creator + "/ " + idea.ProposedBy : creator;



                    idealist.Add(idea);
                }

            }
            return idealist;
        }


    }
}