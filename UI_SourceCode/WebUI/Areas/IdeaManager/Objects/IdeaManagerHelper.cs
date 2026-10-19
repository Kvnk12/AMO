using System;
using System.Collections.Generic;
using System.Linq;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using NTTData.AMOPortal.WebUI.Dto;
using System.Net.Mail;
using System.Web.Configuration;
using Data;
using WebUI.Areas.IdeaManager.Dto;
using WebUI.Areas.IdeaManager.Models;
using NTTData.AMOPortal.WebUI.Mapper;
using Domain.Entities;
using System.Net;
using System.Web;
using System.Text.RegularExpressions;
using Domain.Abstract;
using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
//using NttData.AMOPortal.Core
namespace WebUI.Areas.IdeaManager.Objects
{
    public class IdeaManagerHelper
    {

        public static DataTable IMTrendingReportList(int ClientId)
        {

            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new Data.UnitOfWorks(contextFactory.GetContext());
                dt = unitOfWork.GetDataFromStoredProcedure("IMIdeaTrendingReport", paramName, paramValue);
            }
            return dt;
        }
        public static List<IdeaDashboardModel> GetIdeaList(int ClientId)
        {

            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            List<IdeaDashboardModel> idealist = new List<IdeaDashboardModel>();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new Data.UnitOfWorks(contextFactory.GetContext());
                dt = unitOfWork.GetDataFromStoredProcedure("IMIdeaTrendingReport", paramName, paramValue);
                foreach (DataRow dr in dt.Rows)
                {
                    IdeaDashboardModel idea = new IdeaDashboardModel();

                    idea.Id = (int)dr["Id"];
                    idea.ClientId = ClientId;
                    if ((DateTime)dr["ActualEndDate"] > new DateTime(1900, 01, 02))
                        idea.ActualEndDate = (DateTime)dr["ActualEndDate"];
                    if ((DateTime)dr["ActualEndDate"] > new DateTime(1900, 01, 02))
                        idea.ActualStartDate = (DateTime)dr["ActualStartDate"];
                    idea.ApprovedBy = (string)dr["Approved By"];
                    if ((DateTime)dr["Approved Date"] > new DateTime(1900, 01, 02))
                        idea.ApprovedDate = (DateTime)dr["Approved Date"];
                    if ((DateTime)dr["CompletedDt"] > new DateTime(1900, 01, 02))
                        idea.CompletedDt = (DateTime)dr["CompletedDt"];
                    if ((DateTime)dr["CompletedDt"] > new DateTime(1900, 01, 02))
                        idea.CompletedDt = (DateTime)dr["CompletedDt"];
                    idea.Complexity = (string)dr["Complexity"];
                    idea.CreatedBy = (string)dr["Created By"];
                    if ((DateTime)dr["Created Month-Year"] > new DateTime(1900, 01, 02))
                        idea.CreatedDate = (DateTime)dr["Created Month-Year"];
                    if ((DateTime)dr["LastUpdatedDate"] > new DateTime(1900, 01, 02))
                        idea.LastUpdatedDate = (DateTime)dr["LastUpdatedDate"];
                    idea.IdeaNumber = (string)dr["IdeaNumber"];
                    idea.IdeaSource = (string)dr["Idea Source"];
                    idea.IdeaType = (string)dr["Idea Type"];
                    idea.ImplementationManager = (string)dr["Implementation Manager"];
                    idea.ImplementationPriority = (int)dr["Implementation Priority"];
                    idea.ImplementationProgress = (decimal)dr["ImplementationProgress"];
                    idea.ImplementationStatus = (string)dr["Implementation Status"];
                    idea.InternalInitiative = (bool)dr["Internal Initiative"];
                    idea.ItemName = (string)dr["Idea Name"];
                    idea.LOBDepartment = (string)dr["LOB Department"];
                    if ((DateTime)dr["PlannedEndDate"] > new DateTime(1900, 01, 02))
                        idea.PlannedEndDate = (DateTime)dr["PlannedEndDate"];
                    if ((DateTime)dr["PlannedStartDate"] > new DateTime(1900, 01, 02))
                        idea.PlannedStartDate = (DateTime)dr["PlannedStartDate"];
                    idea.Priority = (int)dr["Priority"];
                    idea.Quantification = (decimal)dr["Quantification"];
                    idea.Status = (string)dr["Status"];
                    if ((DateTime)dr["SubmittedForReviewDt"] > new DateTime(1900, 01, 02))
                        idea.SubmittedForReviewDt = (DateTime)dr["SubmittedForReviewDt"];
                    idea.Team = (string)dr["Team"];


                    idealist.Add(idea);
                }

            }
            return idealist;
        }

        public static List<IdeaDashboardModel> New_GetIdeas(int ClientId)
        {
            Db db = new Db();
            List<IdeaDashboardModel> idealist = new List<IdeaDashboardModel>();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new Data.UnitOfWorks(contextFactory.GetContext());
                //var list = unitOfWork.IMPostIdea.Get().Where(w => w.ClientId == ClientId).Select
                //    (s => new IdeaDashboardModel {​​​​​​​ 
                //            Id=s.Id,
                //            IdeaNumber=s.IdeaNumber,
                //            ItemName=s.ItemName
                //        }​​​​​​​);
                var list = (from a in db.IMPostIdea
                            where a.ClientId == ClientId
                            select new IdeaDashboardModel
                            {
                                Id = a.Id,
                                IdeaNumber = a.IdeaNumber,
                                ItemName = a.ItemName
                            });
                foreach (var a in list)
                {
                    IdeaDashboardModel idea = new IdeaDashboardModel();

                    idea.Id = a.Id;
                    idea.ClientId = ClientId;
                    idea.IdeaNumber = a.IdeaNumber;
                    idea.ItemName = a.ItemName;
                    idealist.Add(idea);
                }
            }
            return idealist;
        }
        public static List<IdeaDashboardModel> New_GetIdeaList(int ClientId)
        {

            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            List<IdeaDashboardModel> idealist = new List<IdeaDashboardModel>();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new Data.UnitOfWorks(contextFactory.GetContext());
                dt = unitOfWork.GetDataFromStoredProcedure("IMAllIdeaReport", paramName, paramValue);
                foreach (DataRow dr in dt.Rows)
                {
                    IdeaDashboardModel idea = new IdeaDashboardModel();

                    idea.Id = (int)dr["Id"];
                    idea.ClientId = ClientId;
                    if ((DateTime)dr["ActualEndDate"] > new DateTime(1900, 01, 02))
                        idea.ActualEndDate = (DateTime)dr["ActualEndDate"];
                    if ((DateTime)dr["ActualStartDate"] > new DateTime(1900, 01, 02))
                        idea.ActualStartDate = (DateTime)dr["ActualStartDate"];
                    idea.ApprovedBy = (string)dr["Approved By"];
                    if ((DateTime)dr["Approved Date"] > new DateTime(1900, 01, 02))
                        idea.ApprovedDate = (DateTime)dr["Approved Date"];
                    if ((DateTime)dr["CompletedDt"] > new DateTime(1900, 01, 02))
                        idea.CompletedDt = (DateTime)dr["CompletedDt"];
                    //if ((DateTime)dr["CompletedDt"] > new DateTime(1900, 01, 02))
                    //    idea.CompletedDt = (DateTime)dr["CompletedDt"];
                    idea.Complexity = (string)dr["Complexity"];
                    idea.CreatedBy = (string)dr["Created By"];
                    idea.ProposedBy = (string)dr["Proposed By"];
                    idea.CreatedBy = idea.ProposedBy != "-" ? idea.CreatedBy + "/ " + idea.ProposedBy : idea.CreatedBy;
                    if ((DateTime)dr["Created Month-Year"] > new DateTime(1900, 01, 02))
                        idea.CreatedDate = (DateTime)dr["Created Month-Year"];
                    if ((DateTime)dr["LastUpdatedDate"] > new DateTime(1900, 01, 02))
                        idea.LastUpdatedDate = (DateTime)dr["LastUpdatedDate"];
                    idea.IdeaNumber = (string)dr["IdeaNumber"];
                    idea.IdeaSource = (string)dr["Idea Source"];
                    idea.IdeaType = (string)dr["Idea Type"];
                    idea.ImplementationManager = (string)dr["Implementation Manager"];
                    idea.ImplementationPriority = (int)dr["Implementation Priority"];
                    idea.ImplementationProgress = (decimal)dr["ImplementationProgress"];
                    idea.ImplementationStatus = (string)dr["Implementation Status"];
                    idea.InternalInitiative = (bool)dr["Internal Initiative"];
                    idea.ItemName = (string)dr["Idea Name"];
                    idea.LOBDepartment = (string)dr["LOB Department"];
                    if ((DateTime)dr["PlannedEndDate"] > new DateTime(1900, 01, 02))
                        idea.PlannedEndDate = (DateTime)dr["PlannedEndDate"];
                    if ((DateTime)dr["PlannedStartDate"] > new DateTime(1900, 01, 02))
                        idea.PlannedStartDate = (DateTime)dr["PlannedStartDate"];
                    idea.Priority = (int)dr["Priority"];
                    idea.Quantification = (decimal)dr["Quantification"];
                    idea.Status = (string)dr["Status"];
                    if ((DateTime)dr["SubmittedForReviewDt"] > new DateTime(1900, 01, 02))
                        idea.SubmittedForReviewDt = (DateTime)dr["SubmittedForReviewDt"];
                    idea.Team = (string)dr["Team"];

                    //description and additional fields
                    idea.ITSavings = dr["ITSavings"] is DBNull ? "" : (string)dr["ITSavings"];
                    idea.OfflineApprover = dr["OfflineApprover"] is DBNull ? "" : (string)dr["OfflineApprover"];
                    idea.BusinessBenefits = dr["BusinessBenefits"] is DBNull ? "" : (string)dr["BusinessBenefits"];
                    idea.RiskMitigation = dr["RiskMitigation"] is DBNull ? "" : (string)dr["RiskMitigation"];
                    idea.Metrics = dr["Metrics"] is DBNull ? "" : (string)dr["Metrics"];
                    idea.Comments = dr["Comments"] is DBNull ? "" : (string)dr["Comments"];
                    idea.CompletedNextSteps = dr["CompletedNextSteps"] is DBNull ? "" : (string)dr["CompletedNextSteps"];
                    idea.Classification = dr["Classification"] is DBNull ? "" : (string)dr["Classification"];
                    idea.SystemImpactedService = dr["SystemImpactedService"] is DBNull ? "" : (string)dr["SystemImpactedService"];



                    var TempSolution = dr["Solution"] is DBNull ? "" : (string)dr["Solution"];
                    if (TempSolution != "")
                    {
                        idea.Solution = HttpUtility.HtmlDecode(TempSolution);
                        var temp = Regex.Replace(idea.Solution, "<br />", Environment.NewLine);
                        temp = Regex.Replace(temp, "</p>", Environment.NewLine);
                        idea.Solution = Regex.Replace(temp, "<.*?>", "");
                    }
                    var TempAnalysis = dr["Analysis"] is DBNull ? "" : (string)dr["Analysis"];
                    if (TempAnalysis != "")
                    {
                        idea.Analysis = HttpUtility.HtmlDecode(TempAnalysis);
                        var temp = Regex.Replace(idea.Analysis, "<br />", Environment.NewLine);
                        temp = temp.Replace("</p>", Environment.NewLine);
                        idea.Analysis = Regex.Replace(temp, "<.*?>", "");
                    }
                    var TempImpComment = dr["StatusComment"] is DBNull ? "" : (string)dr["StatusComment"];
                    if (TempImpComment != "")
                    {
                        idea.ImpComments = HttpUtility.HtmlDecode(TempImpComment);
                        var temp = Regex.Replace(idea.ImpComments, "<br />", Environment.NewLine);
                        temp = temp.Replace("</p>", Environment.NewLine);
                        idea.ImpComments = Regex.Replace(temp, "<.*?>", "");
                    }

                    var TempProblem = dr["Problem"] is DBNull ? "" : (string)dr["Problem"];
                    if (TempProblem != "")
                    {
                        idea.Problem = HttpUtility.HtmlDecode(TempProblem);
                        var temp = Regex.Replace(idea.Problem, "<br />", Environment.NewLine);
                        temp = temp.Replace("</p>", Environment.NewLine);
                        idea.Problem = Regex.Replace(temp, "<.*?>", "");

                    }
                    var TempGoal = dr["Goal"] is DBNull ? "" : (string)dr["Goal"];
                    if (TempGoal != null)
                    {
                        idea.Goal = HttpUtility.HtmlDecode(TempGoal);
                        var temp = Regex.Replace(idea.Goal, "<br />", Environment.NewLine);
                        temp = temp.Replace("</p>", Environment.NewLine);
                        idea.Goal = Regex.Replace(temp, "<.*?>", "");
                    }





                    idealist.Add(idea);
                }

            }
            return idealist;
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

                    idea.IdeaId = (int)dr["Id"];
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
        public static void UpdateIdeaOwner(int Id,int? newownerid,int uid,int ClientId)
        {
                var contextFactory = new DbContextFactory();            
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var updatetitem = unitOfWork.IMPostIdea.GetByID(Id);
            var existingownerid = updatetitem.CreatedBy;
                updatetitem.CreatedBy = newownerid;
                updatetitem.IsReassigned = true;
                updatetitem.LastUpdatedDate = DateTime.Now;
                updatetitem.UpdatedBy = uid;
                unitOfWork.IMPostIdea.Update(updatetitem);
                unitOfWork.Save();
            var rs = new IMIdeaReassign_log();
            rs.IdeaId = Id;
            rs.ClientId = ClientId;
            rs.AssignedByUser = uid;
            rs.AssignedDateTime = DateTime.Now;
            rs.Owner = existingownerid.Value;
            rs.NewOwner = newownerid.Value;
            unitOfWork.IMIdeaReassign_log.Insert(rs);
            unitOfWork.Save();
            int insertid = rs.Id;

        }
        public static void saveAssignmentDetail(IMIdeaReassignModel dtl)
        {
            int id = 0;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var rs = new IMIdeaReassign_log();
                    rs.IdeaId = dtl.IdeaId;
                    rs.ClientId = dtl.ClientId;
                    rs.AssignedByUser = dtl.AssignedByUser;
                    rs.AssignedDateTime = DateTime.Now;
                    rs.Owner = dtl.Owner;
                    rs.NewOwner = dtl.NewOwner;
                    unitOfWork.IMIdeaReassign_log.Insert(rs);
                    unitOfWork.Save();
                    id = rs.Id;

                }
            }
            catch (Exception)
            {

            }
        }
        public static DataTable IMTrendingReportList2(int ClientId)
        {

            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new Data.UnitOfWorks(contextFactory.GetContext());
                dt = unitOfWork.GetDataFromStoredProcedure("IMChallengeTrendingReport", paramName, paramValue);
            }
            return dt;
        }
        public static IList<IMClassification> GetClassification()
        {


            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<IMClassification>(context);
                return varRepo.GetAll().ToList();
            }


        }
        public static IList<IMClassification> _getClassification(int Clientid)
        {


            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<IMClassification>(context);
                return varRepo.GetAll().Where(c => c.ClientId == Clientid).ToList();
            }


        }

        public string _getStatusName(int Stid)
        {
            var x = "";
            if (Stid == 1)
            {
                x = "Created";
            }
            else if (Stid == 2)
            {
                x = "Submitted";
            }
            else if (Stid == 3)
            {
                x = "Approved";
            }
            else if (Stid == 4)
            {
                x = "Declined";
            }
            else if (Stid == 5)
            {
                x = "Closed";
            }
            return x;
            //using (var contextFactory = new DbContextFactory())
            //   {

            //     var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            //    var n = unitOfWork.IMItemStatus.GetByID(Stid);
            //     return n.Status;
            //   }      


        }


        //public static IList<IMCategory> GetCategory()
        public static IList<IMCategory> GetCategory(int classificationid)
        {


            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<IMCategory>(context);
                return varRepo.Where(w => w.ClassificationId == classificationid).ToList();
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


        public static IEnumerable<SelectListItem> IMItemPriorityList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.IMItemPriority> priority = new Repo<NTTData.AMOPortal.Domain.Entities.IMItemPriority>(Context);
                List<NTTData.AMOPortal.Domain.Entities.IMItemPriority> priorityList = priority.GetAll().Where(i => i.IsDeleted == false).OrderBy(m => m.PriorityId).ToList();
                var Items = priorityList.Select(CList => new SelectListItem
                {
                    Value = CList.Id.ToString(),
                    Text = CList.PriorityId.ToString()
                });
                return Items;

            }
        }


        //public static IEnumerable<SelectListItem> IMIdeaTypeList
        //    {
        //    get
        //        {
        //        return (GetIdeaTypes().Select(clist => new SelectListItem
        //        {
        //            Value = clist.Id.ToString(),
        //            Text = clist.IdeaType.ToString(),
        //        }));
        //        }
        //    }


        public static IEnumerable<SelectListItem> IMIdeaSourceList
        {
            get
            {
                return (GetIdeaSources().Select(clist => new SelectListItem
                {
                    Value = clist.Id.ToString(),
                    Text = clist.IdeaSource.ToString(),
                }));
            }
        }

        public static IEnumerable<SelectListItem> IMLOBList
        {
            get
            {
                return (GetLOBSources().Select(clist => new SelectListItem
                {
                    Value = clist.Id.ToString(),
                    Text = clist.LOB.ToString(),
                }));
            }
        }



        public static IEnumerable<SelectListItem> IMItemStatusList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.IMItemStatus> priority = new Repo<NTTData.AMOPortal.Domain.Entities.IMItemStatus>(Context);
                List<NTTData.AMOPortal.Domain.Entities.IMItemStatus> priorityList = priority.GetAll().Where(i => i.IsDeleted == false).OrderBy(m => m.Status).ToList();
                var Items = priorityList.Select(CList => new SelectListItem
                {
                    Value = CList.Id.ToString(),
                    Text = CList.Status.ToString()
                });
                return Items;

            }
        }



        public static IList<IMChallengeCategoryMap> GetMapItemForChallengeItemId(int p)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<IMChallengeCategoryMap>(context);
                return varRepo.Where(c => c.ItemId == p).ToList();
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

        internal static IList<IMPostIdea> GetPostItems(int p)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<IMPostIdea>(context);
                return varRepo.Where(c => c.ClientId == p).ToList();
            }
        }



        public static IList<IMPostIdea> GetPostIdeaItems(int p)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<IMPostIdea>(context);
                return varRepo.GetAll().Where(c => c.ClientId == p).ToList();
            }
        }

        //for Challenges(Suggest  Ideas) Filter:Only Approved Ideas
        public static IList<IMPostIdea> _get_Approved_IdeaItems(int p)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<IMPostIdea>(context);
                //Status 1- created ; 2 - Submitted; 3 - Approved; 4- Declined; 5- Closed;
                return varRepo.GetAll().Where(c => c.ClientId == p && c.StatusId == 3).ToList();
            }
        }




        public static IList<IMPostIdea> GetSuggestIdeas(int p, int challengeId)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<IMPostIdea>(context);
                return varRepo.GetAll().Where(c => c.ClientId == p).ToList();
            }
        }


        public static IList<vwIMSuggestIdeas> GetSuggestIdeasForChallengeId(int p, int challengeId)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<vwIMSuggestIdeas>(context);
                return varRepo.GetAll().Where(c => c.ClientId == p && c.ChallengeId == challengeId).ToList();
            }
        }



        public static IList<IMPostChallenge> GetPostChallengeItems(int p)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<IMPostChallenge>(context);
                return varRepo.GetAll().Where(c => c.ClientId == p).ToList();
            }
        }


        public static IList<IMChallengeComments> GetChallengeComments(int p)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<IMChallengeComments>(context);
                return varRepo.GetAll().Where(c => c.ItemId == p).ToList().OrderByDescending(c => c.CreatedDate).ToList();
            }
        }



        // public static IList<IMIdeaType> GetIdeaTypes()
        public static IList<IMIdeaType> GetIdeaTypes(int ClientId)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<IMIdeaType>(context);
                //return varRepo.GetAll().ToList();
                return varRepo.GetAll().Where(c => c.ClientId == ClientId).ToList();
            }
            //getClientId cv = new getClientId()
            //var id = 
        }
        public static IEnumerable<SelectListItem> _getIdeaTypes(int Clientid)
        {
            var xc = GetIdeaTypes(Clientid).Select(clist => new SelectListItem
            {
                Value = clist.Id.ToString(),
                Text = clist.IdeaType.ToString(),
            });
            return xc;
        }
        //with client ID
        public static IList<IMIdeaType> GetIdeaTypes1(int ClientId)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<IMIdeaType>(context);
                return varRepo.GetAll().Where(c => c.ClientId == ClientId).ToList();
            }
        }
        public static IList<IMIdeaSource> GetIdeaSources()
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<IMIdeaSource>(context);
                return varRepo.GetAll().ToList();
            }
        }
        //with Client Id
        public static IList<IMIdeaSource> GetIdeaSources1(int ClientId)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<IMIdeaSource>(context);
                return varRepo.GetAll().Where(c => c.ClientId == ClientId).ToList();
            }
        }

        public static IEnumerable<SelectListItem> _getIdeaSources(int Clientid)
        {
            var xc = GetIdeaSources1(Clientid).Select(clist => new SelectListItem
            {
                Value = clist.Id.ToString(),
                Text = clist.IdeaSource.ToString(),
            });
            return xc;
        }

        public static IList<IMLOB> GetLOBSources()
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<IMLOB>(context);
                return varRepo.GetAll().ToList();
            }
        }
        //with clientId
        public static IList<IMLOB> GetLOBSources1(int ClientId)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<IMLOB>(context);
                return varRepo.GetAll().Where(c => c.ClientId == ClientId).ToList();
            }
        }


        public static IEnumerable<SelectListItem> _getLOBSources(int Clientid)
        {
            var xc = GetLOBSources1(Clientid).Select(clist => new SelectListItem
            {
                Value = clist.Id.ToString(),
                Text = clist.LOB.ToString(),
            });
            return xc;
        }

        public static IList<IMItemComments> GetItemComments(int p)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<IMItemComments>(context);
                return varRepo.GetAll().Where(c => c.ItemId == p).ToList().OrderByDescending(c => c.CreatedDate).ToList();
            }
        }




        public static IList<IMChallengeITBenefits> GetChallengeITBenefits(int p)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<IMChallengeITBenefits>(context);
                return varRepo.GetAll().Where(c => c.ItemId == p).ToList().OrderByDescending(c => c.CreatedDate).ToList();
            }
        }


        //public static IList<vwChallengePriorityHistory> GetPriorityHistoryDetails(int p)
        //    {
        //    using (var context = new DbContextFactory())
        //        {
        //        var varRepo = new Repo<vwChallengePriorityHistory>(context);
        //        return varRepo.GetAll().Where(c => c.ChallengeId == p).ToList().OrderByDescending(c => c.CreatedDate).ToList();
        //        }
        //    }

        public static IList<IMChallengePriorityHistory> GetPriorityHistoryDetails(int p)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<IMChallengePriorityHistory>(context);
                return varRepo.GetAll().Where(c => c.ChallengeId == p).ToList().OrderByDescending(c => c.CreatedDate).ToList();
            }
        }

        public static IList<vwIdeaPriorityHistory> GetIdeaPriorityHistoryDetails(int p)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<vwIdeaPriorityHistory>(context);
                var getidea = varRepo.GetAll().Where(c => c.IdeaId == p).ToList().OrderByDescending(c => c.CreatedDate).ToList();
                return getidea;
            }
        }


        //public static IList<vwChallengeStatusHistory> GetStatusHistoryDetails(int p)
        //    {
        //    using (var context = new DbContextFactory())
        //        {
        //        var varRepo = new Repo<vwChallengeStatusHistory>(context);
        //        return varRepo.GetAll().Where(c => c.ChallengeId == p).ToList().OrderByDescending(c => c.CreatedDate).ToList();
        //        }
        //    }



        public static IList<vwIdeaStatusHistory> GetIdeaStatusHistoryDetails(int p)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<vwIdeaStatusHistory>(context);
                return varRepo.GetAll().Where(c => c.IdeaId == p).ToList().OrderByDescending(c => c.CreatedDate).ToList();
            }
        }



        public static IList<IMItemITBenefits> GetItemITBenefits(int p)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<IMItemITBenefits>(context);
                return varRepo.GetAll().Where(c => c.ItemId == p).ToList().OrderBy(c => c.CreatedDate).ToList();
            }
        }

        public static IList<IMChallengeBusinessBenefits> GetChallengeBusinessBenefits(int p)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<IMChallengeBusinessBenefits>(context);
                return varRepo.GetAll().Where(c => c.ItemId == p).ToList().OrderBy(c => c.CreatedDate).ToList();
            }
        }


        public static IList<IMItemBusinessBenefits> GetItemBusinessBenefits(int p)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<IMItemBusinessBenefits>(context);
                return varRepo.GetAll().Where(c => c.ItemId == p).ToList().OrderBy(c => c.CreatedDate).ToList();
            }
        }

        public static IList<IMIdea> GetIdea()
        {

            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<IMIdea>(context);
                return varRepo.GetAll().ToList();
            }


        }


        //public static IList<IMCategory2> GetCategory2()
        //    {


        //    using (var context = new DbContextFactory())
        //        {
        //        var varRepo = new Repo<IMCategory2>(context);
        //        return varRepo.GetAll().ToList();
        //        }


        //    }

        internal static IList<IMPostIdea> GetPostIdeas(int p)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<IMPostIdea>(context);
                return varRepo.GetAll().Where(c => c.ClientId == p).ToList();
            }
        }


        public static IList<vwIMaccountwithcat> GetCatAndIdea()
        {


            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<vwIMaccountwithcat>(context);
                return varRepo.GetAll().ToList();
            }


        }

        //public static IList<IMCategory2Map> GetMapIdeaForIdeaId(int p)
        //    {
        //    using (var context = new DbContextFactory())
        //        {
        //        var varRepo = new Repo<IMCategory2Map>(context);
        //        return varRepo.GetAll().Where(c => c.IdeaId == p).ToList();
        //        }
        //    }






        public static IList<IMMetricTarget> GetMetricTargetDetails(int Id)
        {
            IList<MetricTargetModel> metrictargetdetails = new List<MetricTargetModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IList<IMMetricTarget> MetricTargetDetail = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                if (Id != 0)
                    MetricTargetDetail = unitOfWork.IMMetricTarget.Get(c => c.ItemId == Id).ToList();

                //if (MetricTargetDetail != null)
                //{
                //    foreach (var _metrictargetdetail in MetricTargetDetail)
                //    {
                //        //IMPostChallengeInput MetricTargetModel = new IMPostChallengeInput();
                //        MetricTargetModel MetricTargetModel = new MetricTargetModel();
                //        MetricTargetModel.Id = _metrictargetdetail.Id;
                //        MetricTargetModel.MetricTarget = _metrictargetdetail.MetricTarget;
                //        //MetricTargetModel.UpdatedDate = DateTime.Now;
                //        //var c = unitOfWork.UserProfile.GetByID(_metrictargetdetail.CreatedBy);
                //        MetricTargetDetails.Add(MetricTargetModel);
                //    }
                //}
                return MetricTargetDetail;
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

        public static IList<IMRoleModel> GetRoleDetails(int ClientId, int uid)
        {

            IList<IMRoleModel> Role = new List<IMRoleModel>();
            var entityMapper = new Mapper<IMRoleModel, IMRole>();

            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<IMRole> RoleDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (ClientId != 0)
                    RoleDetails = unitOfWork.IMRole.Get(cid => cid.ClientId == ClientId).ToList();

                if (RoleDetails != null)
                {
                    foreach (var _roledetail in RoleDetails)
                    {
                        IMRoleModel p = new IMRoleModel();
                        p.Id = _roledetail.Id;
                        p.ClientId = _roledetail.ClientId;
                        p.RoleName = _roledetail.RoleName;
                        p.CreatedBy = _roledetail.CreatedBy;
                        p.UpdatedBy = _roledetail.UpdatedBy;
                        p.Createddate = _roledetail.Createddate;
                        p.Updateddate = _roledetail.Updateddate;
                        Role.Add(p);
                    }
                }
                return Role;
            }

        }


        public static int CreateRole(IMRoleModel IMrolemodel, int Id)
        {
            int id = 0;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var rol = new IMRole();
                    rol.ClientId = IMrolemodel.ClientId;
                    rol.RoleName = IMrolemodel.RoleName;
                    rol.Createddate = DateTime.Now;
                    rol.Updateddate = DateTime.Now;
                    rol.CreatedBy = IMrolemodel.CreatedBy;
                    rol.UpdatedBy = IMrolemodel.UpdatedBy;
                    unitOfWork.IMRole.Insert(rol);
                    unitOfWork.Save();
                    id = rol.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool UpdateRole(IMRoleModel IMrolemodel)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.IMRole.GetByID(IMrolemodel.Id);
                    updatetitem.ClientId = IMrolemodel.ClientId;
                    updatetitem.RoleName = IMrolemodel.RoleName;
                    updatetitem.UpdatedBy = IMrolemodel.UpdatedBy;
                    updatetitem.Updateddate = DateTime.Now;
                    unitOfWork.IMRole.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveRole(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.IMRole.Delete(Id);
                unitOfWork.Save();

                //TempData["SLACreatedDeleted"] = "Deleted successfully.";
                return true;
            }
        }




        public static IList<IMResourceResponseModel> GetResourceDetails(int ClientId, int uid)
        {

            IList<IMResourceResponseModel> Resource = new List<IMResourceResponseModel>();
            var entityMapper = new Mapper<IMResourceResponseModel, IMResource>();

            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<IMResource> ResourceDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (ClientId != 0)
                    ResourceDetails = unitOfWork.IMResource.Get(cid => cid.ClientId == ClientId).ToList();

                if (ResourceDetails != null)
                {
                    foreach (var _resourcedetail in ResourceDetails)
                    {
                        IMResourceResponseModel p = new IMResourceResponseModel();
                        p.Id = _resourcedetail.Id;
                        p.ClientId = _resourcedetail.ClientId;
                        p.ResourceName = _resourcedetail.ResourceName;
                        p.CreatedBy = _resourcedetail.CreatedBy;
                        p.UpdatedBy = _resourcedetail.UpdatedBy;
                        p.Createddate = _resourcedetail.Createddate;
                        p.Updateddate = _resourcedetail.Updateddate;
                        Resource.Add(p);
                    }
                }
                return Resource;
            }

        }
        public static int CreateResource(IMResourceResponseModel IMresourcemodel, int Id)
        {
            int id = 0;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var res = new IMResource();
                    //rol.Id = Id;
                    res.ClientId = IMresourcemodel.ClientId;
                    res.ResourceName = IMresourcemodel.ResourceName;
                    res.Createddate = DateTime.Now;
                    res.Updateddate = DateTime.Now;
                    res.CreatedBy = IMresourcemodel.CreatedBy;
                    res.UpdatedBy = IMresourcemodel.UpdatedBy;
                    unitOfWork.IMResource.Insert(res);
                    unitOfWork.Save();
                    id = res.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool UpdateResource(IMResourceResponseModel IMresourcemodel)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.IMResource.GetByID(IMresourcemodel.Id);
                    updatetitem.ClientId = IMresourcemodel.ClientId;
                    updatetitem.ResourceName = IMresourcemodel.ResourceName;
                    updatetitem.UpdatedBy = IMresourcemodel.UpdatedBy;
                    updatetitem.Updateddate = DateTime.Now;
                    unitOfWork.IMResource.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveResource(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.IMResource.Delete(Id);
                unitOfWork.Save();

                //TempData["SLACreatedDeleted"] = "Deleted successfully.";
                return true;
            }
        }




        public static IList<IMProgressTemplateModel> GetProgessDetails(int phaseId)
        {
            IList<IMProgressTemplateModel> progresstemplate = new List<IMProgressTemplateModel>();
            var entityMapper = new Mapper<IMProgressTemplateModel, IMProgressTemplate>();

            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<IMProgressTemplate> ProgressDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                if (phaseId != 0)
                    ProgressDetails = unitOfWork.IMProgressTemplate.Get(cid => cid.Phase == phaseId).ToList();

                if (ProgressDetails != null)
                {
                    foreach (var _progdetail in ProgressDetails)
                    {
                        IMProgressTemplateModel pro = new IMProgressTemplateModel();
                        pro.ClientId = _progdetail.ClientId;
                        pro.ProgressId = _progdetail.Id;
                        pro.PhaseId = _progdetail.Phase;
                        pro.Progress = _progdetail.Progress;
                        progresstemplate.Add(pro);
                    }
                }
                return progresstemplate;
            }
        }


        public static int CreateProgess(IMProgressTemplateModel ImProgresstemplatemModel, int PId)
        {
            int id = 0;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var ipt = new IMProgressTemplate();
                    ipt.ClientId = ImProgresstemplatemModel.ClientId;
                    ipt.Phase = PId;
                    ipt.Progress = ImProgresstemplatemModel.Progress;
                    ipt.CreatedBy = ImProgresstemplatemModel.CreatedBy;
                    ipt.Createddate = DateTime.Now;
                    ipt.UpdatedBy = ImProgresstemplatemModel.UpdatedBy;
                    ipt.Updateddate = DateTime.Now;
                    unitOfWork.IMProgressTemplate.Insert(ipt);
                    unitOfWork.Save();
                    id = ipt.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }



        public static bool UpdateProgress(IMProgressTemplateModel ImProgresstemplatemModel)
        {
            try
            {
                //var entityMapper = new Mapper<IMProgressTemplate, IMProgressTemplateModel>();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var progressdet = unitOfWork.IMProgressTemplate.GetByID(ImProgresstemplatemModel.ProgressId);

                    progressdet.Progress = ImProgresstemplatemModel.Progress;
                    progressdet.UpdatedBy = ImProgresstemplatemModel.UpdatedBy;
                    progressdet.Updateddate = DateTime.Now;

                    unitOfWork.IMProgressTemplate.Update(progressdet);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static bool RemoveProgress(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.IMProgressTemplate.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }

        /*-------------------------new meta data -------------------------------------*/

        public static IList<IMComplexityModel> GetComplexityDetails(int ClientId, int uid)
        {

            IList<IMComplexityModel> Complex = new List<IMComplexityModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<IMComplexity> ComplexityDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (ClientId != 0)
                    ComplexityDetails = unitOfWork.IMComplexity.Get(cid => cid.ClientId == ClientId).ToList();

                if (ComplexityDetails != null)
                {
                    foreach (var _complexitydetail in ComplexityDetails)
                    {
                        IMComplexityModel cs = new IMComplexityModel();
                        cs.Id = _complexitydetail.Id;
                        cs.ClientId = _complexitydetail.ClientId;
                        cs.Complexity = _complexitydetail.Complexity;
                        cs.CreatedBy = _complexitydetail.CreatedBy;
                        cs.UpdatedBy = _complexitydetail.UpdatedBy;
                        cs.CreatedDt = _complexitydetail.CreatedDt;
                        cs.UpdatedDt = _complexitydetail.UpdatedDt;
                        Complex.Add(cs);
                    }
                }
                return Complex;
            }

        }
        public static int CreateComplexity(IMComplexityModel imcomplexityModel, int Id)
        {
            int id = 0;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var com = new IMComplexity();
                    com.ClientId = imcomplexityModel.ClientId;
                    com.Complexity = imcomplexityModel.Complexity;
                    com.CreatedDt = DateTime.Now;
                    com.CreatedBy = imcomplexityModel.CreatedBy;
                    unitOfWork.IMComplexity.Insert(com);
                    unitOfWork.Save();
                    id = com.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool UpdateComplexity(IMComplexityModel imcomplexityModel)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.IMComplexity.GetByID(imcomplexityModel.Id);
                    updatetitem.ClientId = imcomplexityModel.ClientId;
                    updatetitem.Complexity = imcomplexityModel.Complexity;
                    updatetitem.UpdatedBy = imcomplexityModel.UpdatedBy;
                    updatetitem.UpdatedDt = DateTime.Now;
                    unitOfWork.IMComplexity.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveComplexity(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.IMComplexity.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }

        public static IList<IMIdeaTypeModel> GetIdeaTypeDetails(int ClientId, int uid)
        {

            IList<IMIdeaTypeModel> IdeaType = new List<IMIdeaTypeModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<IMIdeaType> IdeaTypeDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (ClientId != 0)
                    IdeaTypeDetails = unitOfWork.IMIdeaType.Get(cid => cid.ClientId == ClientId).ToList();

                if (IdeaTypeDetails != null)
                {
                    foreach (var _ideatypedetail in IdeaTypeDetails)
                    {
                        IMIdeaTypeModel cs = new IMIdeaTypeModel();
                        cs.Id = _ideatypedetail.Id;
                        cs.ClientId = _ideatypedetail.ClientId;
                        cs.IdeaType = _ideatypedetail.IdeaType;
                        cs.CreatedBy = _ideatypedetail.CreatedBy;
                        cs.UpdatedBy = _ideatypedetail.UpdatedBy;
                        cs.CreatedDate = _ideatypedetail.CreatedDate;
                        cs.UpdatedBy = _ideatypedetail.UpdatedBy;
                        IdeaType.Add(cs);
                    }
                }
                return IdeaType;
            }

        }
        public static int CreateIdeaType(IMIdeaTypeModel imideaTypeModel, int Id)
        {
            int id = 0;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var ideatype = new IMIdeaType();
                    ideatype.ClientId = imideaTypeModel.ClientId;
                    ideatype.IdeaType = imideaTypeModel.IdeaType;
                    ideatype.CreatedDate = DateTime.Now;
                    ideatype.CreatedBy = imideaTypeModel.CreatedBy;
                    unitOfWork.IMIdeaType.Insert(ideatype);
                    unitOfWork.Save();
                    id = ideatype.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool UpdateIdeaType(IMIdeaTypeModel imideaTypeModel)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.IMIdeaType.GetByID(imideaTypeModel.Id);
                    updatetitem.ClientId = imideaTypeModel.ClientId;
                    updatetitem.IdeaType = imideaTypeModel.IdeaType;
                    updatetitem.UpdatedBy = imideaTypeModel.UpdatedBy;
                    updatetitem.LastUpdatedDate = DateTime.Now;
                    unitOfWork.IMIdeaType.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveIdeaType(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.IMIdeaType.Delete(Id);
                unitOfWork.Save();

                //TempData["SLACreatedDeleted"] = "Deleted successfully.";
                return true;
            }
        }


        public static IList<IMIdeaSourceModel> GetIdeaSourceDetails(int ClientId, int uid)
        {

            IList<IMIdeaSourceModel> IdeaSource = new List<IMIdeaSourceModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<IMIdeaSource> IdeaSourceDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (ClientId != 0)
                    IdeaSourceDetails = unitOfWork.IMIdeaSource.Get(cid => cid.ClientId == ClientId).ToList();

                if (IdeaSourceDetails != null)
                {
                    foreach (var _ideasourcedetail in IdeaSourceDetails)
                    {
                        IMIdeaSourceModel cs = new IMIdeaSourceModel();
                        cs.Id = _ideasourcedetail.Id;
                        cs.ClientId = _ideasourcedetail.ClientId;
                        cs.IdeaSource = _ideasourcedetail.IdeaSource;
                        cs.CreatedBy = _ideasourcedetail.CreatedBy;
                        cs.UpdatedBy = _ideasourcedetail.UpdatedBy;
                        cs.CreatedDate = _ideasourcedetail.CreatedDate;
                        cs.UpdatedBy = _ideasourcedetail.UpdatedBy;
                        IdeaSource.Add(cs);
                    }
                }
                return IdeaSource;
            }

        }
        public static int CreateIdeaSource(IMIdeaSourceModel imideaSourceModel, int Id)
        {
            int id = 0;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var ideasource = new IMIdeaSource();
                    ideasource.ClientId = imideaSourceModel.ClientId;
                    ideasource.IdeaSource = imideaSourceModel.IdeaSource;
                    ideasource.CreatedDate = DateTime.Now;
                    ideasource.CreatedBy = imideaSourceModel.CreatedBy;
                    unitOfWork.IMIdeaSource.Insert(ideasource);
                    unitOfWork.Save();
                    id = ideasource.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool UpdateIdeaSource(IMIdeaSourceModel imideaSourceModel)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.IMIdeaSource.GetByID(imideaSourceModel.Id);
                    updatetitem.ClientId = imideaSourceModel.ClientId;
                    updatetitem.IdeaSource = imideaSourceModel.IdeaSource;
                    updatetitem.UpdatedBy = imideaSourceModel.UpdatedBy;
                    updatetitem.LastUpdatedDate = DateTime.Now;
                    unitOfWork.IMIdeaSource.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveIdeaSource(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.IMIdeaSource.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }

        public static IList<IMLOBModel> GetLOBDetails(int ClientId, int uid)
        {

            IList<IMLOBModel> LOB = new List<IMLOBModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<IMLOB> LOBDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (ClientId != 0)
                    LOBDetails = unitOfWork.IMLOB.Get(cid => cid.ClientId == ClientId).ToList();

                if (LOBDetails != null)
                {
                    foreach (var _lobdetail in LOBDetails)
                    {
                        IMLOBModel cs = new IMLOBModel();
                        cs.Id = _lobdetail.Id;
                        cs.ClientId = _lobdetail.ClientId;
                        cs.LOB = _lobdetail.LOB;
                        cs.CreatedBy = _lobdetail.CreatedBy;
                        cs.UpdatedBy = _lobdetail.UpdatedBy;
                        cs.CreatedDate = _lobdetail.CreatedDate;
                        cs.UpdatedBy = _lobdetail.UpdatedBy;
                        LOB.Add(cs);
                    }
                }
                return LOB;
            }

        }
        public static int CreateLOB(IMLOBModel imlOBModel, int Id)
        {
            int id = 0;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var Lob = new IMLOB();
                    Lob.ClientId = imlOBModel.ClientId;
                    Lob.LOB = imlOBModel.LOB;
                    Lob.CreatedDate = DateTime.Now;
                    Lob.CreatedBy = imlOBModel.CreatedBy;
                    unitOfWork.IMLOB.Insert(Lob);
                    unitOfWork.Save();
                    id = Lob.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool UpdateLOB(IMLOBModel imlOBModel)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.IMLOB.GetByID(imlOBModel.Id);
                    updatetitem.ClientId = imlOBModel.ClientId;
                    updatetitem.LOB = imlOBModel.LOB;
                    updatetitem.UpdatedBy = imlOBModel.UpdatedBy;
                    updatetitem.LastUpdatedDate = DateTime.Now;
                    unitOfWork.IMLOB.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveLOB(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.IMLOB.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }

        public static IList<IMIdeaSourceMaster> GetIdeaSourceMasterDetails(int ClientId, int uid)
        {

            IList<IMIdeaSourceMaster> ideasrc = new List<IMIdeaSourceMaster>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<IMIdeaSourceMaster> srcdtl = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (ClientId != 0)
                    srcdtl = unitOfWork.IMIdeaSourceMaster.Get(cid => cid.ClientId == ClientId || cid.ClientId == 0).ToList();

                if (srcdtl != null)
                {
                    foreach (var _lobdetail in srcdtl)
                    {
                        IMIdeaSourceMaster cs = new IMIdeaSourceMaster();
                        cs.Id = _lobdetail.Id;
                        cs.ClientId = _lobdetail.ClientId;
                        cs.IdeaSource = _lobdetail.IdeaSource;
                        cs.CreatedBy = _lobdetail.CreatedBy;
                        cs.UpdatedBy = _lobdetail.UpdatedBy;
                        cs.CreatedDate = _lobdetail.CreatedDate;
                        cs.UpdatedBy = _lobdetail.UpdatedBy;
                        cs.isDefault = _lobdetail.isDefault;
                        ideasrc.Add(cs);
                    }
                }
                return ideasrc;
            }

        }
        public static int CreateIdeaSourceMaster(IMIdeaSourceMasterModel imideasrcBModel, int Id)
        {
            int id = 0;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var src = new IMIdeaSourceMaster();
                    src.ClientId = imideasrcBModel.ClientId;
                    src.IdeaSource = imideasrcBModel.IdeaSource;
                    src.CreatedDate = DateTime.Now;
                    src.IsDeleted = false;
                    src.CreatedBy = imideasrcBModel.CreatedBy;
                    src.isDefault = false;
                    unitOfWork.IMIdeaSourceMaster.Insert(src);
                    unitOfWork.Save();
                    id = src.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool UpdateIdeaSourceMaster(IMIdeaSourceMasterModel imsrcModel)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.IMIdeaSourceMaster.GetByID(imsrcModel.Id);
                    updatetitem.ClientId = imsrcModel.ClientId;
                    updatetitem.IdeaSource = imsrcModel.IdeaSource;
                    updatetitem.UpdatedBy = imsrcModel.UpdatedBy;
                    updatetitem.UpdatedDate = DateTime.Now;
                    unitOfWork.IMIdeaSourceMaster.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveIdeaSourceMaster(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.IMIdeaSourceMaster.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }




        public static IList<IMClassificationModel> GetClassificationDetails(int ClientId, int uid)
        {
            IList<IMClassificationModel> Classification = new List<IMClassificationModel>();

            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<IMClassification> ClassificationDetail = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                if (ClientId != 0)
                    ClassificationDetail = unitOfWork.IMClassification.Get(cid => cid.ClientId == ClientId).ToList();

                if (ClassificationDetail != null)
                {
                    foreach (var _classificationdetail in ClassificationDetail)
                    {
                        IMClassificationModel IMClassificationModel = new IMClassificationModel();
                        IMClassificationModel.CalId = _classificationdetail.Id;
                        IMClassificationModel.Classification = _classificationdetail.Classification;
                        IMClassificationModel.ClientId = _classificationdetail.ClientId;
                        IMClassificationModel.CreatedBy = _classificationdetail.CreatedBy;
                        IMClassificationModel.CreatedDate = _classificationdetail.CreatedDate;
                        IMClassificationModel.UpdatedBy = _classificationdetail.UpdatedBy;
                        IMClassificationModel.LastUpdatedDate = _classificationdetail.LastUpdatedDate;
                        Classification.Add(IMClassificationModel);
                    }
                }
                return Classification;
            }

        }


        public static int CreateClassification(IMClassificationModel imclassificationModel)
        {
            int id = 0;
            try
            {

                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    IMClassification ClassificationModel = new IMClassification();
                    ClassificationModel.Classification = imclassificationModel.Classification;
                    ClassificationModel.ClientId = imclassificationModel.ClientId;
                    ClassificationModel.CreatedBy = imclassificationModel.CreatedBy;
                    ClassificationModel.CreatedDate = imclassificationModel.CreatedDate;
                    unitOfWork.IMClassification.Insert(ClassificationModel);
                    unitOfWork.Save();
                    id = ClassificationModel.Id;

                }
            }
            catch (Exception)
            {
                return 0;
            }
            return id;
        }

        public static bool UpdateClassification(IMClassificationModel imclassificationModel)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var classificationinfo = unitOfWork.IMClassification.GetByID(imclassificationModel.CalId);
                    classificationinfo.Classification = imclassificationModel.Classification;
                    classificationinfo.UpdatedBy = imclassificationModel.UpdatedBy;
                    classificationinfo.LastUpdatedDate = imclassificationModel.LastUpdatedDate;
                    unitOfWork.IMClassification.Update(classificationinfo);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveClassification(int ClassId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.IMClassification.Delete(ClassId);
                unitOfWork.Save();
                return true;
            }
        }


        public static IList<IMCategoryModel> GetCategoryDetails(int CattId = 0)
        {
            IList<IMCategoryModel> categoryy = new List<IMCategoryModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<IMCategory> IMCategoryDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                if (CattId != 0)
                    IMCategoryDetails = unitOfWork.IMCategory.Get(cid => cid.ClassificationId == CattId).ToList();

                if (IMCategoryDetails != null)
                {
                    foreach (var _categorydetail in IMCategoryDetails)
                    {
                        IMCategoryModel categoryModel = new IMCategoryModel();
                        categoryModel.Category = _categorydetail.Category;
                        categoryModel.CaId = _categorydetail.Id;
                        categoryModel.clientid = _categorydetail.clientid;
                        categoryy.Add(categoryModel);
                    }
                }
                return categoryy;
            }

        }


        public static int CreateCategory(IMCategoryModel imcategoryModel)
        {
            int id = 0;
            try
            {

                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    IMCategory CategoryInfo = new IMCategory();
                    CategoryInfo.clientid = imcategoryModel.clientid;
                    CategoryInfo.ClassificationId = imcategoryModel.ClassificationId;
                    CategoryInfo.Category = imcategoryModel.Category;
                    CategoryInfo.CreatedBy = imcategoryModel.CreatedBy;
                    CategoryInfo.CreatedDate = imcategoryModel.CreatedDate;
                    unitOfWork.IMCategory.Insert(CategoryInfo);
                    unitOfWork.Save();
                    id = CategoryInfo.Id;

                }
            }
            catch (Exception)
            {
                return 0;
            }
            return id;
        }


        public static bool UpdateCategory(IMCategoryModel imcategoryModel)
        {
            try
            {

                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var categoryinfo = unitOfWork.IMCategory.GetByID(imcategoryModel.CaId);
                    // entityMapper.MapToEntity(Portfolioinput, appPortfolioinfo);                    
                    categoryinfo.Category = imcategoryModel.Category;
                    categoryinfo.UpdatedBy = imcategoryModel.UpdatedBy;
                    categoryinfo.LastUpdatedDate = imcategoryModel.LastUpdatedDate;
                    unitOfWork.IMCategory.Update(categoryinfo);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveCategory(int catId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.IMCategory.Delete(catId);
                unitOfWork.Save();
                return true;
            }
        }

        public static string[] GetIdeaApprovers(int IdeaId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var listApprovers = unitOfWork.IMIdeaApprover.Get().Where(g => g.IdeaId == IdeaId);

            var strLst = listApprovers.Select(s => s.ApproverId.ToString()).ToArray();
            return strLst;
        }



        //-------------------------------------------Configuration Type--------------------------------------------//


        public static IList<IMConfigTypeModel> GetConfigurationtypeDetails(int ClientId, int uid)
        {
            IList<IMConfigTypeModel> Configuration = new List<IMConfigTypeModel>();

            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<IMConfigType> ConfigurationDetail = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                if (ClientId != 0)
                    ConfigurationDetail = unitOfWork.IMConfigType.Get(cid => cid.ClientId == ClientId).ToList();

                if (ConfigurationDetail != null)
                {
                    foreach (var _configurationdetail in ConfigurationDetail)
                    {
                        IMConfigTypeModel IMConfigTypeModel = new IMConfigTypeModel();
                        IMConfigTypeModel.CTId = _configurationdetail.Id;
                        IMConfigTypeModel.ConfigType = _configurationdetail.ConfigType;
                        IMConfigTypeModel.ClientId = _configurationdetail.ClientId;
                        IMConfigTypeModel.CreatedBy = _configurationdetail.CreatedBy;
                        IMConfigTypeModel.CreatedDt = _configurationdetail.CreatedDt;
                        IMConfigTypeModel.UpdatedBy = _configurationdetail.UpdatedBy;
                        IMConfigTypeModel.UpdatedDt = _configurationdetail.UpdatedDt;
                        Configuration.Add(IMConfigTypeModel);
                    }
                }
                return Configuration;
            }

        }


        public static int CreateConfigurationtype(IMConfigTypeModel imconfigTypeModel)
        {
            int id = 0;
            try
            {

                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    IMConfigType imconfigType = new IMConfigType();
                    imconfigType.ConfigType = imconfigTypeModel.ConfigType;
                    imconfigType.ClientId = imconfigTypeModel.ClientId;
                    imconfigType.CreatedBy = imconfigTypeModel.CreatedBy;
                    imconfigType.CreatedDt = imconfigTypeModel.CreatedDt;
                    unitOfWork.IMConfigType.Insert(imconfigType);
                    unitOfWork.Save();
                    id = imconfigType.Id;

                }
            }
            catch (Exception)
            {
                return 0;
            }
            return id;
        }

        public static bool UpdateConfigurationtype(IMConfigTypeModel imconfigTypeModel)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var ConfigTypeInfo = unitOfWork.IMConfigType.GetByID(imconfigTypeModel.CTId);
                    ConfigTypeInfo.ConfigType = imconfigTypeModel.ConfigType;
                    ConfigTypeInfo.UpdatedBy = imconfigTypeModel.UpdatedBy;
                    ConfigTypeInfo.UpdatedDt = imconfigTypeModel.UpdatedDt;
                    unitOfWork.IMConfigType.Update(ConfigTypeInfo);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveConfigurationtype(int CtypeId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.IMConfigType.Delete(CtypeId);
                unitOfWork.Save();
                return true;
            }
        }
        //---------------------------------------Configuration Item-----------------------------------------------//


        public static IList<IMConfigItemModel> GetConfigurationitemDetails(int ConId = 0)
        {
            IList<IMConfigItemModel> configitems = new List<IMConfigItemModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<IMConfigItem> IMConfigItemDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                if (ConId != 0)
                    IMConfigItemDetails = unitOfWork.IMConfigItem.Get(cid => cid.ConfigTypeId == ConId).ToList();

                if (IMConfigItemDetails != null)
                {
                    foreach (var _citemdetail in IMConfigItemDetails)
                    {
                        IMConfigItemModel CitemModel = new IMConfigItemModel();
                        CitemModel.ConfigItem = _citemdetail.ConfigItem;
                        CitemModel.CIId = _citemdetail.Id;
                        CitemModel.ClientId = _citemdetail.ClientId;
                        configitems.Add(CitemModel);
                    }
                }
                return configitems;
            }

        }


        public static int CreateConfigurationitem(IMConfigItemModel imconfigItemModel)
        {
            int id = 0;
            try
            {

                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    IMConfigItem configitemInfo = new IMConfigItem();
                    configitemInfo.ClientId = imconfigItemModel.ClientId;
                    configitemInfo.ConfigTypeId = imconfigItemModel.ConfigTypeId;
                    configitemInfo.ConfigItem = imconfigItemModel.ConfigItem;
                    configitemInfo.CreatedBy = imconfigItemModel.CreatedBy;
                    configitemInfo.CreatedDt = imconfigItemModel.CreatedDt;
                    unitOfWork.IMConfigItem.Insert(configitemInfo);
                    unitOfWork.Save();
                    id = configitemInfo.Id;

                }
            }
            catch (Exception)
            {
                return 0;
            }
            return id;
        }


        public static bool UpdateConfigurationitem(IMConfigItemModel imconfigItemModel)
        {
            try
            {

                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var configIteminfo = unitOfWork.IMConfigItem.GetByID(imconfigItemModel.CIId);
                    configIteminfo.ConfigItem = imconfigItemModel.ConfigItem;
                    configIteminfo.UpdatedBy = imconfigItemModel.UpdatedBy;
                    configIteminfo.UpdatedDt = imconfigItemModel.UpdatedDt;
                    unitOfWork.IMConfigItem.Update(configIteminfo);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveConfigurationitem(int CitemId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.IMConfigItem.Delete(CitemId);
                unitOfWork.Save();
                return true;
            }
        }

        public static bool RemoveIdea(int Id)
        {
            try
            {
                var paramName = new string[] { "IdeaId" };
                var paramValue = new object[] { Id };
                // DataTable dt = new DataTable();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new Data.UnitOfWorks(contextFactory.GetContext());
                    var dt = unitOfWork.GetDataFromStoredProcedure("DeleteIMPostIdea", paramName, paramValue);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }


        }

        public static IList<IMChartDashboardModel> IMDashboardChartData(string Charttype, int ClientId, DateTime? StartDate, DateTime? EndDate, string IdeaSource)
        {
            IList<IMChartDashboardModel> Modellist = new List<IMChartDashboardModel>();
            var paramName = new string[] { "Charttype", "ClientId", "StartDate", "EndDate", "IdeaSource" };
            var paramValue = new object[] { Charttype, ClientId, StartDate, EndDate, IdeaSource };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("IMDashboardGetChartData", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new IMChartDashboardModel();


                    coldet.IdeaCount = Convert.ToInt32(dr["IdeaCount"]);

                    try
                    {
                        if (!(dr["Category"] is DBNull))
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
                        else
                        {
                            coldet.LongCategory = "Unknown";
                            coldet.DrpCategory = "Unknown";
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    Modellist.Add(coldet);
                }
            }
            return Modellist;
        }
        public static IList<IMChartDashboardModel> IMDashboardTrendChartData(int ClientId, string Charttype, DateTime StartDate, DateTime EndDate, string IdeaSource)
        {
            IList<IMChartDashboardModel> Modellist = new List<IMChartDashboardModel>();
            var paramName = new string[] { "ClientId", "StartDate", "EndDate", "Dim", "Ideasource" };
            var paramValue = new object[] { ClientId, StartDate, EndDate, Charttype, IdeaSource };

            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("SP_IMDashboard1IdeaCount_New", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new IMChartDashboardModel();

                    if (dr["IdeaCount"] is DBNull)
                        coldet.IdeaCount = 0;
                    else
                        coldet.IdeaCount = Convert.ToInt32(dr["IdeaCount"]);

                    try
                    {
                        var temp = 0;
                        if (Charttype == "Day")
                        {
                            if (dr["Dim_Value"] is DBNull)
                                coldet.DrpCategory = "0";
                            else
                                coldet.DrpCategory = dr["Dim_Value"].ToString();
                            temp = coldet.DrpCategory.IndexOf(" ");
                            coldet.DrpCategory = coldet.DrpCategory.Substring(0, temp);
                        }
                        else
                        {
                            if (dr["Dim_Value"] is DBNull)
                                coldet.DrpCategory = "0";
                            else
                                coldet.DrpCategory = dr["Dim_Value"].ToString();
                        }
                        if (dr["ResolvedCount"] is DBNull)
                            coldet.ResolvedCount = 0;
                        else
                            coldet.ResolvedCount = Convert.ToInt32(dr["ResolvedCount"]);
                    }
                    catch { }
                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }
        public static IList<IMChartDashboardModel> IMDashboard2TrendChartData(int ClientId, string drpval, string dpnVal, DateTime StartDate, DateTime EndDate, string IdeaSource)
        {
            IList<IMChartDashboardModel> Modellist = new List<IMChartDashboardModel>();
            var paramName = new string[] { "ClientId", "StartDate", "EndDate", "Dim", "dpnVal", "IdeaSource" };
            var paramValue = new object[] { ClientId, StartDate, EndDate, drpval, dpnVal, IdeaSource };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("SP_IMDashboard2IdeaCount", paramName, paramValue);
                if (dpnVal.Length > 0)
                {
                    foreach (DataRow dr in res.Rows)
                    {
                        var coldet = new IMChartDashboardModel();
                        coldet.IdeaCount = Convert.ToInt32(dr["IdeaCount"]);

                        try
                        {

                            coldet.DrpCategory = dr["Category"].ToString();
                            coldet.Dim = dr["Dim_Value"].ToString(); // dr["Category"].ToString();

                        }
                        catch { }
                        Modellist.Add(coldet);
                    }
                }
                else
                {
                    foreach (DataRow dr in res.Rows)
                    {
                        var coldet = new IMChartDashboardModel();
                        coldet.IdeaCount = Convert.ToInt32(dr["IdeaCount"]);

                        try
                        {
                            if (dr["cum_val"] is DBNull)
                                coldet.cumm_value = 0;
                            else
                                coldet.cumm_value = Convert.ToDecimal(dr["cum_val"]);
                            coldet.Dim = dr["Dim_Value"].ToString(); ;// dr["Category"].ToString();
                        }
                        catch { }
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

        public static IList<IMChartDashboardModel> calculatePercent(IList<IMChartDashboardModel> model)
        {
            //IList<DashboardTrendModel> Modellist = new List<DashboardTrendModel>();
            // Modellist = model;
            var sum = 0;
            foreach (var item in model)
            {
                sum = sum + item.IdeaCount;
            }
            if (sum != 0)
            {
                foreach (var item in model)
                {
                    item.Percent = Math.Round((decimal)(item.IdeaCount * 100) / sum, 2);
                    item.AverageVal = Math.Round((decimal)(item.IdeaCount) / model.Count, 2);
                }
            }

            return model;
        }

        public static IList<IMChartDashboardModel> calculateTrend(IList<IMChartDashboardModel> model)
        {
            int cnt = model.Count();
            if (cnt < 2)
                return model;
            var litem = model.ElementAt(cnt - 1);
            long LIdeaCount = litem.IdeaCount;
            long fIdeaCount = model.ElementAt(0).IdeaCount;
            var slope = (LIdeaCount - fIdeaCount) / (cnt - 1);
            var offset = fIdeaCount - (slope * 1);
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
        public static IList<IdeaDashboardModel> IMDashboardDrilldownData(string Charttype, int ClientId, DateTime? StartDate, DateTime? EndDate, string Category, string IdeaSource)
        {
            IList<IdeaDashboardModel> Modellist = new List<IdeaDashboardModel>();
            var paramName = new string[] { "Charttype", "ClientId", "StartDate", "EndDate", "CategoryValue", "IdeaSource" };
            var paramValue = new object[] { Charttype, ClientId, StartDate, EndDate, Category, IdeaSource };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("IMDashboardGetChartData_drilldown", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new IdeaDashboardModel();
                    coldet.Id = Convert.ToInt32(dr["Id"]);
                    if (!(dr["IdeaNumber"] is DBNull))
                        coldet.IdeaNumber = dr["IdeaNumber"].ToString();
                    else
                        coldet.IdeaNumber = "--NA--";
                    coldet.ItemName = dr["IdeaName"].ToString();
                    if (!(dr["Description"] is DBNull))
                        coldet.Problem = dr["Description"].ToString();
                    else
                        coldet.Problem = "--NA--";
                    if (!(dr["Goal"] is DBNull))
                        coldet.Goal = dr["Goal"].ToString();
                    else
                        coldet.Goal = "--NA--";
                    if (!(dr["PriorityId"] is DBNull))
                        coldet.Priority = Convert.ToInt32(dr["PriorityId"]);

                    if (!(dr["IdeaType"] is DBNull))
                        coldet.IdeaType = dr["IdeaType"].ToString();
                    else
                        coldet.IdeaType = "--NA--";
                    coldet.Status = dr["Status"].ToString();
                    if (!(dr["LOB"] is DBNull))
                        coldet.LOBDepartment = dr["LOB"].ToString();
                    else
                        coldet.LOBDepartment = "--NA--";
                    if (!(dr["Complexity"] is DBNull))
                        coldet.Complexity = dr["Complexity"].ToString();
                    else
                        coldet.Complexity = "--NA--";
                    if (!(dr["Team"] is DBNull))
                        coldet.Team = dr["Team"].ToString();
                    else
                        coldet.Team = "--NA--";
                    if (!(dr["SystemImpactedService"] is DBNull))
                        coldet.SystemImpactedService = dr["SystemImpactedService"].ToString();
                    else
                        coldet.SystemImpactedService = "--NA--";
                    coldet.CreatedBy = dr["CreatedBy"].ToString();
                    if (!(dr["UpdatedBy"] is DBNull))
                        coldet.UpdatedBy = dr["UpdatedBy"].ToString();
                    else
                        coldet.UpdatedBy = "--NA--";
                    if (!(dr["ApprovedBy"] is DBNull))
                        coldet.ApprovedBy = dr["ApprovedBy"].ToString();
                    else
                        coldet.ApprovedBy = "--NA--";
                    if (!(dr["ImpManager"] is DBNull))
                        coldet.ImplementationManager = dr["ImpManager"].ToString();
                    if (!(dr["CreatedDate"] is DBNull))
                        coldet.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                    if (!(dr["UpdatedDate"] is DBNull))
                        coldet.LastUpdatedDate = Convert.ToDateTime(dr["UpdatedDate"]);
                    if (!(dr["ApprovedDate"] is DBNull))
                        coldet.ApprovedDate = Convert.ToDateTime(dr["ApprovedDate"]);
                    if (!(dr["CompletedDate"] is DBNull))
                        coldet.CompletedDt = Convert.ToDateTime(dr["CompletedDate"]);
                    coldet.ClientId = Convert.ToInt32(dr["ClientId"]);
                    if (!(dr["Quantification"] is DBNull))
                        coldet.Quantification = Convert.ToDecimal(dr["Quantification"]);
                    coldet.InternalInitiative = (bool)dr["InternalInitiative"];
                    if (!(dr["PlannedStartDate"] is DBNull))
                        coldet.PlannedStartDate = Convert.ToDateTime(dr["PlannedStartDate"]);
                    if (!(dr["PlannedEndDate"] is DBNull))
                        coldet.PlannedEndDate = Convert.ToDateTime(dr["PlannedEndDate"]);
                    if (!(dr["ActualStartDate"] is DBNull))
                        coldet.ActualStartDate = Convert.ToDateTime(dr["ActualStartDate"]);
                    if (!(dr["ActualEndDate"] is DBNull))
                        coldet.ActualEndDate = Convert.ToDateTime(dr["ActualEndDate"]);
                    if (!(dr["ImplementationPriority"] is DBNull))
                        coldet.ImplementationPriority = Convert.ToInt32(dr["ImplementationPriority"]);
                    if (!(dr["ImplementationStatus"] is DBNull))
                        coldet.ImplementationStatus = dr["ImplementationStatus"].ToString();
                    else
                        coldet.ImplementationStatus = "--NA--";
                    if (!(dr["ImplementationProgress"] is DBNull))
                        coldet.ImplementationProgress = Convert.ToDecimal(dr["ImplementationProgress"]);
                    if (!(dr["ProposedBy"] is DBNull))
                        coldet.ProposedBy = dr["ProposedBy"].ToString();
                    else
                        coldet.ProposedBy = "--NA--";

                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }
        public static IList<IdeaDashboardModel> IMDashboard2DrilldownData(int ClientId, string drpval, string dpnVal, DateTime StartDate, DateTime EndDate, string Category, string Categoryseries, string IdeaSource)
        {
            IList<IdeaDashboardModel> Modellist = new List<IdeaDashboardModel>();
            var paramName = new string[] { "ClientId", "StartDate", "EndDate", "Dim", "dpnVal", "categoryValue", "CategorySeriesValue", "IdeaSource" };
            var paramValue = new object[] { ClientId, StartDate, EndDate, drpval, dpnVal, Category, Categoryseries, IdeaSource };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("SP_IMDashboard2IdeaCount_drilldown", paramName, paramValue);

                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new IdeaDashboardModel();
                    coldet.Id = Convert.ToInt32(dr["Id"]);
                    if (!(dr["IdeaNumber"] is DBNull))
                        coldet.IdeaNumber = dr["IdeaNumber"].ToString();
                    else
                        coldet.IdeaNumber = "--NA--";
                    coldet.ItemName = dr["IdeaName"].ToString();
                    if (!(dr["Description"] is DBNull))
                        coldet.Problem = dr["Description"].ToString();
                    else
                        coldet.Problem = "--NA--";
                    if (!(dr["Goal"] is DBNull))
                        coldet.Goal = dr["Goal"].ToString();
                    else
                        coldet.Goal = "--NA--";
                    if (!(dr["PriorityId"] is DBNull))
                        coldet.Priority = Convert.ToInt32(dr["PriorityId"]);

                    if (!(dr["IdeaType"] is DBNull))
                        coldet.IdeaType = dr["IdeaType"].ToString();
                    else
                        coldet.IdeaType = "--NA--";
                    coldet.Status = dr["Status"].ToString();
                    if (!(dr["LOB"] is DBNull))
                        coldet.LOBDepartment = dr["LOB"].ToString();
                    else
                        coldet.LOBDepartment = "--NA--";
                    if (!(dr["Complexity"] is DBNull))
                        coldet.Complexity = dr["Complexity"].ToString();
                    else
                        coldet.Complexity = "--NA--";
                    if (!(dr["Team"] is DBNull))
                        coldet.Team = dr["Team"].ToString();
                    else
                        coldet.Team = "--NA--";
                    if (!(dr["SystemImpactedService"] is DBNull))
                        coldet.SystemImpactedService = dr["SystemImpactedService"].ToString();
                    else
                        coldet.SystemImpactedService = "--NA--";
                    coldet.CreatedBy = dr["CreatedBy"].ToString();
                    if (!(dr["UpdatedBy"] is DBNull))
                        coldet.UpdatedBy = dr["UpdatedBy"].ToString();
                    else
                        coldet.UpdatedBy = "--NA--";
                    if (!(dr["ApprovedBy"] is DBNull))
                        coldet.ApprovedBy = dr["ApprovedBy"].ToString();
                    else
                        coldet.ApprovedBy = "--NA--";
                    if (!(dr["ImpManager"] is DBNull))
                        coldet.ImplementationManager = dr["ImpManager"].ToString();
                    if (!(dr["CreatedDate"] is DBNull))
                        coldet.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                    if (!(dr["UpdatedDate"] is DBNull))
                        coldet.LastUpdatedDate = Convert.ToDateTime(dr["UpdatedDate"]);
                    if (!(dr["ApprovedDate"] is DBNull))
                        coldet.ApprovedDate = Convert.ToDateTime(dr["ApprovedDate"]);
                    if (!(dr["CompletedDate"] is DBNull))
                        coldet.CompletedDt = Convert.ToDateTime(dr["CompletedDate"]);
                    coldet.ClientId = Convert.ToInt32(dr["ClientId"]);
                    if (!(dr["Quantification"] is DBNull))
                        coldet.Quantification = Convert.ToDecimal(dr["Quantification"]);
                    coldet.InternalInitiative = (bool)dr["InternalInitiative"];
                    if (!(dr["PlannedStartDate"] is DBNull))
                        coldet.PlannedStartDate = Convert.ToDateTime(dr["PlannedStartDate"]);
                    if (!(dr["PlannedEndDate"] is DBNull))
                        coldet.PlannedEndDate = Convert.ToDateTime(dr["PlannedEndDate"]);
                    if (!(dr["ActualStartDate"] is DBNull))
                        coldet.ActualStartDate = Convert.ToDateTime(dr["ActualStartDate"]);
                    if (!(dr["ActualEndDate"] is DBNull))
                        coldet.ActualEndDate = Convert.ToDateTime(dr["ActualEndDate"]);
                    if (!(dr["ImplementationPriority"] is DBNull))
                        coldet.ImplementationPriority = Convert.ToInt32(dr["ImplementationPriority"]);
                    if (!(dr["ImplementationStatus"] is DBNull))
                        coldet.ImplementationStatus = dr["ImplementationStatus"].ToString();
                    else
                        coldet.ImplementationStatus = "--NA--";
                    if (!(dr["ImplementationProgress"] is DBNull))
                        coldet.ImplementationProgress = Convert.ToDecimal(dr["ImplementationProgress"]);
                    if (!(dr["ProposedBy"] is DBNull))
                        coldet.ProposedBy = dr["ProposedBy"].ToString();
                    else
                        coldet.ProposedBy = "--NA--";

                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }
        public static IList<IdeaDashboardModel> IMTrendDrillDownData(int ClientId, string Charttype, DateTime StartDate, DateTime EndDate, string category, string InflowOutFlowType, string IdeaSource)
        {
            IList<IdeaDashboardModel> Modellist = new List<IdeaDashboardModel>();
            var paramName = new string[] { "ClientId", "StartDate", "EndDate", "Dim", "category", "InflowOutFlowType", "IdeaSource" };
            var paramValue = new object[] { ClientId, StartDate, EndDate, Charttype, category, InflowOutFlowType, IdeaSource };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("SP_IMDashboard1IdeaCount_DrillDown_New", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new IdeaDashboardModel();
                    coldet.Id = Convert.ToInt32(dr["Id"]);
                    if (!(dr["IdeaNumber"] is DBNull))
                        coldet.IdeaNumber = dr["IdeaNumber"].ToString();
                    else
                        coldet.IdeaNumber = "--NA--";
                    coldet.ItemName = dr["IdeaName"].ToString();
                    if (!(dr["Description"] is DBNull))
                        coldet.Problem = dr["Description"].ToString();
                    else
                        coldet.Problem = "--NA--";
                    if (!(dr["Goal"] is DBNull))
                        coldet.Goal = dr["Goal"].ToString();
                    else
                        coldet.Goal = "--NA--";
                    if (!(dr["PriorityId"] is DBNull))
                        coldet.Priority = Convert.ToInt32(dr["PriorityId"]);

                    if (!(dr["IdeaType"] is DBNull))
                        coldet.IdeaType = dr["IdeaType"].ToString();
                    else
                        coldet.IdeaType = "--NA--";
                    coldet.Status = dr["Status"].ToString();
                    if (!(dr["LOB"] is DBNull))
                        coldet.LOBDepartment = dr["LOB"].ToString();
                    else
                        coldet.LOBDepartment = "--NA--";
                    if (!(dr["Complexity"] is DBNull))
                        coldet.Complexity = dr["Complexity"].ToString();
                    else
                        coldet.Complexity = "--NA--";
                    if (!(dr["Team"] is DBNull))
                        coldet.Team = dr["Team"].ToString();
                    else
                        coldet.Team = "--NA--";
                    if (!(dr["SystemImpactedService"] is DBNull))
                        coldet.SystemImpactedService = dr["SystemImpactedService"].ToString();
                    else
                        coldet.SystemImpactedService = "--NA--";
                    coldet.CreatedBy = dr["CreatedBy"].ToString();
                    if (!(dr["UpdatedBy"] is DBNull))
                        coldet.UpdatedBy = dr["UpdatedBy"].ToString();
                    else
                        coldet.UpdatedBy = "--NA--";
                    if (!(dr["ApprovedBy"] is DBNull))
                        coldet.ApprovedBy = dr["ApprovedBy"].ToString();
                    else
                        coldet.ApprovedBy = "--NA--";
                    if (!(dr["ImpManager"] is DBNull))
                        coldet.ImplementationManager = dr["ImpManager"].ToString();
                    if (!(dr["CreatedDate"] is DBNull))
                        coldet.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                    if (!(dr["UpdatedDate"] is DBNull))
                        coldet.LastUpdatedDate = Convert.ToDateTime(dr["UpdatedDate"]);
                    if (!(dr["ApprovedDate"] is DBNull))
                        coldet.ApprovedDate = Convert.ToDateTime(dr["ApprovedDate"]);
                    if (!(dr["CompletedDate"] is DBNull))
                        coldet.CompletedDt = Convert.ToDateTime(dr["CompletedDate"]);
                    coldet.ClientId = Convert.ToInt32(dr["ClientId"]);
                    if (!(dr["Quantification"] is DBNull))
                        coldet.Quantification = Convert.ToDecimal(dr["Quantification"]);
                    coldet.InternalInitiative = (bool)dr["InternalInitiative"];
                    if (!(dr["PlannedStartDate"] is DBNull))
                        coldet.PlannedStartDate = Convert.ToDateTime(dr["PlannedStartDate"]);
                    if (!(dr["PlannedEndDate"] is DBNull))
                        coldet.PlannedEndDate = Convert.ToDateTime(dr["PlannedEndDate"]);
                    if (!(dr["ActualStartDate"] is DBNull))
                        coldet.ActualStartDate = Convert.ToDateTime(dr["ActualStartDate"]);
                    if (!(dr["ActualEndDate"] is DBNull))
                        coldet.ActualEndDate = Convert.ToDateTime(dr["ActualEndDate"]);
                    if (!(dr["ImplementationPriority"] is DBNull))
                        coldet.ImplementationPriority = Convert.ToInt32(dr["ImplementationPriority"]);
                    if (!(dr["ImplementationStatus"] is DBNull))
                        coldet.ImplementationStatus = dr["ImplementationStatus"].ToString();
                    else
                        coldet.ImplementationStatus = "--NA--";
                    if (!(dr["ImplementationProgress"] is DBNull))
                        coldet.ImplementationProgress = Convert.ToDecimal(dr["ImplementationProgress"]);
                    if (!(dr["ProposedBy"] is DBNull))
                        coldet.ProposedBy = dr["ProposedBy"].ToString();
                    else
                        coldet.ProposedBy = "--NA--";

                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }

    }
}