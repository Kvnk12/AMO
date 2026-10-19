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
using WebUI.Areas.DemandManager.Dto;
using WebUI.Areas.DemandManager.Models;
using NTTData.AMOPortal.WebUI.Mapper;
using Domain.Entities;
using System.Net;
using System.Web;
using System.Text.RegularExpressions;

//using NttData.AMOPortal.Core
namespace WebUI.Areas.DemandManager.Object
{
    public class DemandManagerHelper
    {
        public static DataTable DMTrendingReportList(int ClientId)
        {
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new Data.UnitOfWorks(contextFactory.GetContext());
                dt = unitOfWork.GetDataFromStoredProcedure("DMDemandTrendingReport", paramName, paramValue);
            }
            return dt;
        }
        public static List<DemandDashboardModel> GetDemandList(int ClientId)
        {

            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            List<DemandDashboardModel> Demandlist = new List<DemandDashboardModel>();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new Data.UnitOfWorks(contextFactory.GetContext());
                dt = unitOfWork.GetDataFromStoredProcedure("DMDemandTrendingReport", paramName, paramValue);
                foreach(DataRow dr in dt.Rows)
                {
                    DemandDashboardModel Demand = new DemandDashboardModel();

                    Demand.Id = (int)dr["Id"];
                    Demand.ClientId = ClientId;
                    if ((DateTime)dr["ActualEndDate"] > new DateTime(1900, 01, 02))
                        Demand.ActualEndDate = (DateTime)dr["ActualEndDate"];
                    if ((DateTime)dr["ActualEndDate"] > new DateTime(1900, 01, 02))
                        Demand.ActualStartDate = (DateTime)dr["ActualStartDate"];
                    Demand.ApprovedBy = (string)dr["Approved By"];
                    if ((DateTime)dr["Approved Date"] > new DateTime(1900, 01, 02))
                        Demand.ApprovedDate = (DateTime)dr["Approved Date"];
                    if ((DateTime)dr["CompletedDt"] > new DateTime(1900, 01, 02))
                        Demand.CompletedDt = (DateTime)dr["CompletedDt"];
                    if ((DateTime)dr["CompletedDt"] > new DateTime(1900, 01, 02))
                        Demand.CompletedDt = (DateTime)dr["CompletedDt"];
                    Demand.CreatedBy = (string)dr["Created By"];
                    if ((DateTime)dr["Created Month-Year"] > new DateTime(1900, 01, 02))
                        Demand.CreatedDate = (DateTime)dr["Created Month-Year"];
                    if ((DateTime)dr["LastUpdatedDate"] > new DateTime(1900, 01, 02))
                        Demand.LastUpdatedDate = (DateTime)dr["LastUpdatedDate"];
                    Demand.DemandNumber = (string)dr["DemandNumber"];
                    Demand.DemandSource = (string)dr["Demand Source"];
                    Demand.DemandType = (string)dr["Demand Type"];
                    Demand.ImplementationManager = (string)dr["Implementation Manager"];
                    Demand.ImplementationPriority = (int)dr["Implementation Priority"];
                    Demand.ImplementationStatus = (string)dr["Implementation Status"];
                    Demand.InternalInitiative = (bool)dr["Internal Initiative"];
                    Demand.ItemName = (string)dr["Demand Name"];
                    Demand.LOBDepartment = (string)dr["LOB Department"];
                    if ((DateTime)dr["PlannedEndDate"] > new DateTime(1900, 01, 02))
                        Demand.PlannedEndDate = (DateTime)dr["PlannedEndDate"];
                    if ((DateTime)dr["PlannedStartDate"] > new DateTime(1900, 01, 02))
                        Demand.PlannedStartDate = (DateTime)dr["PlannedStartDate"];
                    Demand.Priority = (int)dr["Priority"];
                    Demand.Status = (string)dr["Status"];
                    if ((DateTime)dr["SubmittedForReviewDt"] > new DateTime(1900, 01, 02))
                        Demand.SubmittedForReviewDt = (DateTime)dr["SubmittedForReviewDt"];

                    Demandlist.Add(Demand);
                }

            }
            return Demandlist;
        }


        public static List<DemandDashboardModel> New_GetDemandList(int ClientId)
        {

            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            List<DemandDashboardModel> Demandlist = new List<DemandDashboardModel>();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new Data.UnitOfWorks(contextFactory.GetContext());
                dt = unitOfWork.GetDataFromStoredProcedure("DMAllDemandReport", paramName, paramValue);
                foreach (DataRow dr in dt.Rows)
                {
                    DemandDashboardModel Demand = new DemandDashboardModel();

                    Demand.Id = (int)dr["Id"];
                    Demand.ClientId = ClientId;
                    if ((DateTime)dr["ActualEndDate"] > new DateTime(1900, 01, 02))
                        Demand.ActualEndDate = (DateTime)dr["ActualEndDate"];
                    if ((DateTime)dr["ActualStartDate"] > new DateTime(1900, 01, 02))
                        Demand.ActualStartDate = (DateTime)dr["ActualStartDate"];
                    Demand.ApprovedBy = (string)dr["Approved By"];
                    if ((DateTime)dr["Approved Date"] > new DateTime(1900, 01, 02))
                        Demand.ApprovedDate = (DateTime)dr["Approved Date"];
                    if ((DateTime)dr["CompletedDt"] > new DateTime(1900, 01, 02))
                        Demand.CompletedDt = (DateTime)dr["CompletedDt"];
                    //if ((DateTime)dr["CompletedDt"] > new DateTime(1900, 01, 02))
                    Demand.CreatedBy = (string)dr["Created By"];
                    if ((DateTime)dr["Created Month-Year"] > new DateTime(1900, 01, 02))
                        Demand.CreatedDate = (DateTime)dr["Created Month-Year"];
                    if ((DateTime)dr["LastUpdatedDate"] > new DateTime(1900, 01, 02))
                        Demand.LastUpdatedDate = (DateTime)dr["LastUpdatedDate"];
                    Demand.DemandNumber = (string)dr["DemandNumber"];
                    Demand.DemandSource = (string)dr["Demand Source"];
                    Demand.DemandType = (string)dr["Demand Type"];
                    Demand.ImplementationManager = (string)dr["Implementation Manager"];
                    Demand.ImplementationPriority = (int)dr["Implementation Priority"];
                    Demand.ImplementationStatus = (string)dr["Implementation Status"];
                    Demand.InternalInitiative = (bool)dr["Internal Initiative"];
                    Demand.ItemName = (string)dr["Demand Name"];
                    Demand.LOBDepartment = (string)dr["LOB Department"];
                    if ((DateTime)dr["PlannedEndDate"] > new DateTime(1900, 01, 02))
                        Demand.PlannedEndDate = (DateTime)dr["PlannedEndDate"];
                    if ((DateTime)dr["PlannedStartDate"] > new DateTime(1900, 01, 02))
                        Demand.PlannedStartDate = (DateTime)dr["PlannedStartDate"];
                    Demand.Priority = (int)dr["Priority"];
                    Demand.Status = (string)dr["Status"];
                    if ((DateTime)dr["SubmittedForReviewDt"] > new DateTime(1900, 01, 02))
                        Demand.SubmittedForReviewDt = (DateTime)dr["SubmittedForReviewDt"];

                    //description and additional fields
                    Demand.Comments = dr["Comments"] is DBNull?"":(string)dr["Comments"];
                    Demand.Classification = dr["Classification"] is DBNull ? "" : (string)dr["Classification"];

                    var TempSolution = dr["Solution"] is DBNull? "":(string)dr["Solution"];
                    if (TempSolution!="")
                    {
                        Demand.Solution = HttpUtility.HtmlDecode(TempSolution);
                        var temp = Regex.Replace(Demand.Solution, "<br />", Environment.NewLine);
                        temp = Regex.Replace(temp, "</p>", Environment.NewLine);
                        Demand.Solution = Regex.Replace(temp, "<.*?>", "");
                    }
                   
                    var TempImpComment = dr["StatusComment"] is DBNull ? "" : (string)dr["StatusComment"];
                    if (TempImpComment != "")
                    {
                        Demand.ImpComments = HttpUtility.HtmlDecode(TempImpComment);
                        var temp = Regex.Replace(Demand.ImpComments, "<br />", Environment.NewLine);
                        temp = temp.Replace("</p>", Environment.NewLine);
                        Demand.ImpComments = Regex.Replace(temp, "<.*?>", "");
                    }

                    var TempProblem = dr["Problem"] is DBNull?"":(string)dr["Problem"];
                    if (TempProblem != "")
                    {
                        Demand.Problem = HttpUtility.HtmlDecode(TempProblem);
                        var temp = Regex.Replace(Demand.Problem, "<br />", Environment.NewLine);
                        temp = temp.Replace("</p>", Environment.NewLine);
                        Demand.Problem = Regex.Replace(temp, "<.*?>", "");

                    }
                    var TempGoal = dr["Goal"] is DBNull?"":(string)dr["Goal"];
                    if (TempGoal != null)
                    {
                        Demand.Goal = HttpUtility.HtmlDecode(TempGoal);
                        var temp = Regex.Replace(Demand.Goal, "<br />", Environment.NewLine);
                        temp = temp.Replace("</p>", Environment.NewLine);
                        Demand.Goal = Regex.Replace(temp, "<.*?>", "");
                    }
                    Demand.DocumentNo = (string)dr["DocumentNo"];
                    Demand.EstimatedCost = (string)dr["EstimatedCost"];
                    Demand.DocumentType = (string)dr["DocumentType"];
                    Demand.DemandPlatformLead = (string)dr["DemandPlatformLead"];
                    Demand.Approved_Arch =(string)dr["Approved_Arch"];

                    Demandlist.Add(Demand);
                }
            }
            return Demandlist;
        }
        public static DataTable DMTrendingReportList2(int ClientId)
        {
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new Data.UnitOfWorks(contextFactory.GetContext());
                dt = unitOfWork.GetDataFromStoredProcedure("DMChallengeTrendingReport", paramName, paramValue);
            }
            return dt;
        }
        public static IList<DMClassification> GetClassification()
            {
            using (var context = new DbContextFactory())
                {
                var varRepo = new Repo<DMClassification>(context);
                return varRepo.GetAll().ToList();
                }
        }
        public static IList<DMClassification> _getClassification(int Clientid)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<DMClassification>(context);
                return varRepo.GetAll().Where(c=>c.ClientId== Clientid).ToList();
            }
        }

        //public string _getStatusName(int Stid)
        //{
        //    var x = "";
        //    if (Stid == 1)
        //    {
        //        x = "Created";
        //    }
        //    else if (Stid == 2)
        //    {
        //        x = "Submitted";
        //    }
        //    else if (Stid == 3)
        //    {
        //        x = "Approved";
        //    }
        //    else if (Stid == 4)
        //    {
        //        x = "Declined";
        //    }
        //    else if (Stid == 5)
        //    {
        //        x = "Closed";
        //    }
        //    return x;
        //    using (var contextFactory = new DbContextFactory())
        //    {

        //        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
        //        var n = unitOfWork.DMItemStatus.GetByID(Stid);
        //        return n.Status;
        //    }
            

        //}


        public static IList<DMCategory> GetCategory(int classificationid)
        {
            using (var context = new DbContextFactory())
                {
                var varRepo = new Repo<DMCategory>(context);
                return varRepo.Where(w=> w.ClassificationId == classificationid).ToList();
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

        //public static IEnumerable<SelectListItem> DMItemPriorityList
        //    {
        //    get
        //        {
        //        DbContextFactory Context = new DbContextFactory();
        //        Repo<NTTData.AMOPortal.Domain.Entities.DMItemPriority> priority = new Repo<NTTData.AMOPortal.Domain.Entities.DMItemPriority>(Context);
        //        List<NTTData.AMOPortal.Domain.Entities.DMItemPriority> priorityList = priority.GetAll().Where(i => i.IsDeleted == false).OrderBy(m => m.PriorityId).ToList();
        //        var Items = priorityList.Select(CList => new SelectListItem
        //        {
        //            Value = CList.Id.ToString(),
        //            Text = CList.PriorityId.ToString()
        //        });
        //        return Items;

        //        }
        //    }
        //public static IEnumerable<SelectListItem> DMDemandSourceList
        //{
        //    get
        //        {
        //        return (GetDemandSources().Select(clist => new SelectListItem
        //        {
        //            Value = clist.Id.ToString(),
        //            Text = clist.DemandSource.ToString(),
        //        }));
        //        }
        //    }

            public static IEnumerable<SelectListItem> DMLOBList
            {
            get
                {
                    return (GetLOBSources().Select(clist => new SelectListItem
                {
                    Value = clist.Id.ToString(),
                    Text = clist.DemandLOB.ToString(),
                }));
                }
            }

        //public static IEnumerable<SelectListItem> DMItemStatusList
        //    {
        //    get
        //        {
        //        DbContextFactory Context = new DbContextFactory();
        //        Repo<NTTData.AMOPortal.Domain.Entities.DMItemStatus> priority = new Repo<NTTData.AMOPortal.Domain.Entities.DMItemStatus>(Context);
        //        List<NTTData.AMOPortal.Domain.Entities.DMItemStatus> priorityList = priority.GetAll().Where(i => i.IsDeleted == false).OrderBy(m => m.Status).ToList();
        //        var Items = priorityList.Select(CList => new SelectListItem
        //        {
        //            Value = CList.Id.ToString(),
        //            Text = CList.Status.ToString()
        //        });
        //        return Items;
        //        }
        //    }

        //public static IList<DMChallengeCategoryMap> GetMapItemForChallengeItemId(int p)
        //    {
        //    using (var context = new DbContextFactory())
        //        {
        //        var varRepo = new Repo<DMChallengeCategoryMap>(context);
        //        return varRepo.Where(c=> c.ItemId == p).ToList();
        //        }
        //    }


        public  static IList<DMItemCategoryMap> GetMapItemForItemId(int p)
            {
            using (var context = new DbContextFactory())
                {
                var varRepo = new Repo<DMItemCategoryMap>(context);
                return varRepo.Where(c=> c.ItemId == p).ToList();
                }
            }

        internal static IList<DMPostDemand> GetPostItems(int p)
            {
            using (var context = new DbContextFactory())
                {
                var varRepo = new Repo<DMPostDemand>(context);
                return varRepo.Where(c => c.ClientId == p).ToList();
                }
            }

        public static IList<DMPostDemand> GetPostDemandItems(int p)
            {
            using (var context = new DbContextFactory())
                {
                var varRepo = new Repo<DMPostDemand>(context);
                return varRepo.GetAll().Where(c => c.ClientId == p).ToList();
                }
            }

        //for Challenges(Suggest  Demands) Filter:Only Approved Demands
        public static IList<DMPostDemand> _get_Approved_DemandItems(int p)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<DMPostDemand>(context);
                //Status 1- created ; 2 - Submitted; 3 - Approved; 4- Declined; 5- Closed;
                return varRepo.GetAll().Where(c => c.ClientId == p && c.StatusId == 3).ToList();
            }
        }

        public static IList<DMPostDemand> GetSuggestDemands(int p, int challengeId)
            {
            using (var context = new DbContextFactory())
                {
                var varRepo = new Repo<DMPostDemand>(context);
                return varRepo.GetAll().Where(c => c.ClientId == p).ToList();
                }
            }

        //public static IList<vwDMSuggestDemands> GetSuggestDemandsForChallengeId(int p, int challengeId)
        //    {
        //    using (var context = new DbContextFactory())
        //        {
        //        var varRepo = new Repo<vwDMSuggestDemands>(context);
        //        return varRepo.GetAll().Where(c => c.ClientId == p && c.ChallengeId == challengeId).ToList();
        //        }
        //    }

        //public static IList<DMPostChallenge> GetPostChallengeItems(int p)
        //    {
        //    using (var context = new DbContextFactory())
        //        {
        //        var varRepo = new Repo<DMPostChallenge>(context);
        //        return varRepo.GetAll().Where(c => c.ClientId == p).ToList();
        //        }
        //    }

        //public static IList<DMChallengeComments> GetChallengeComments(int p)
        //    {
        //    using (var context = new DbContextFactory())
        //        {
        //        var varRepo = new Repo<DMChallengeComments>(context);
        //        return varRepo.GetAll().Where(c => c.ItemId == p).ToList().OrderByDescending(c => c.CreatedDate).ToList();
        //        }
        //    }

        public static IList<DMDemandType> GetDemandTypes(int ClientId)
            {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<DMDemandType>(context);
                //return varRepo.GetAll().ToList();
                return varRepo.GetAll().Where(c => c.ClientId == ClientId).ToList();
            }
            //getClientId cv = new getClientId()
            //var id = 
        }
        public static IEnumerable<SelectListItem> _getDemandTypes(int Clientid)
        {
            var xc = GetDemandTypes(Clientid).Select(clist => new SelectListItem
            {
                Value = clist.Id.ToString(),
                Text = clist.DemandType.ToString(),
            });
            return xc;
        }
        //with client ID
        public static IList<DMDemandType> GetDemandTypes1(int ClientId)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<DMDemandType>(context);
                return varRepo.GetAll().Where(c=>c.ClientId==ClientId).ToList();
            }
        }
        //public static IList<DMDemandSource> GetDemandSources()
        //    {
        //    using (var context = new DbContextFactory())
        //        {
        //        var varRepo = new Repo<DMDemandSource>(context);
        //        return varRepo.GetAll().ToList();
        //        }
        //    }
        //with Client Id
        //public static IList<DMDemandSource> GetDemandSources1(int ClientId)
        //{
        //    using (var context = new DbContextFactory())
        //    {
        //        var varRepo = new Repo<DMDemandSource>(context);
        //        return varRepo.GetAll().Where(c=>c.ClientId==ClientId).ToList();
        //    }
        //}

        //public static IEnumerable<SelectListItem> _getDemandSources(int Clientid)
        //{
        //    var xc = GetDemandSources1(Clientid).Select(clist => new SelectListItem
        //    {
        //        Value = clist.Id.ToString(),
        //        Text = clist.DemandSource.ToString(),
        //    });
        //    return xc;
        //}

        public static IList<DMLOB> GetLOBSources()
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<DMLOB>(context);
                return varRepo.GetAll().ToList();
            }
        }
        //with clientId
        public static IList<DMLOB> GetLOBSources1(int ClientId)
        {
            using (var context = new DbContextFactory())
            {
                var varRepo = new Repo<DMLOB>(context);
                return varRepo.GetAll().Where(c=>c.ClientId==ClientId).ToList();
            }
        }
        
        public static IEnumerable<SelectListItem> _getLOBSources(int Clientid)
        {
            var xc = GetLOBSources1(Clientid).Select(clist => new SelectListItem
            {
                Value = clist.Id.ToString(),
                Text = clist.DemandLOB.ToString(),
            });
            return xc;
        }

        public static IList<DMItemComments> GetItemComments(int p)
            {
            using (var context = new DbContextFactory())
                {
                var varRepo = new Repo<DMItemComments>(context);
                return varRepo.GetAll().Where(c => c.ItemId == p).ToList().OrderByDescending(c => c.CreatedDate).ToList();
                }
            }

        //public static IList<DMChallengeITBenefits> GetChallengeITBenefits(int p)
        //    {
        //    using (var context = new DbContextFactory())
        //        {
        //        var varRepo = new Repo<DMChallengeITBenefits>(context);
        //        return varRepo.GetAll().Where(c => c.ItemId == p).ToList().OrderByDescending(c => c.CreatedDate).ToList();
        //        }
        //    }

        //public static IList<DMChallengePriorityHistory> GetPriorityHistoryDetails(int p)
        //{
        //    using (var context = new DbContextFactory())
        //    {
        //        var varRepo = new Repo<DMChallengePriorityHistory>(context);
        //        return varRepo.GetAll().Where(c => c.ChallengeId == p).ToList().OrderByDescending(c => c.CreatedDate).ToList();
        //    }
        //}

        //public static IList<vwDemandPriorityHistory> GetDemandPriorityHistoryDetails(int p)
        //    {
        //    using (var context = new DbContextFactory())
        //        {
        //        var varRepo = new Repo<vwDemandPriorityHistory>(context);
        //        var getDemand = varRepo.GetAll().Where(c => c.DemandId == p).ToList().OrderByDescending(c => c.CreatedDate).ToList();
        //        return getDemanda;
        //        }
        //    }

        //public static IList<vwDemandStatusHistory> GetDemandStatusHistoryDetails(int p)
        //    {
        //    using (var context = new DbContextFactory())
        //        {
        //        var varRepo = new Repo<vwDemandStatusHistory>(context);
        //        return varRepo.GetAll().Where(c => c.DemandId == p).ToList().OrderByDescending(c => c.CreatedDate).ToList();
        //        }
        //    }


        public static IList<DMItemITBenefits> GetItemITBenefits(int p)
            {
            using (var context = new DbContextFactory())
                {
                var varRepo = new Repo<DMItemITBenefits>(context);
                return varRepo.GetAll().Where(c => c.ItemId == p).ToList().OrderBy(c => c.CreatedDate).ToList();
                }
            }

        //public static IList<DMChallengeBusinessBenefits> GetChallengeBusinessBenefits(int p)
        //    {
        //    using (var context = new DbContextFactory())
        //        {
        //        var varRepo = new Repo<DMChallengeBusinessBenefits>(context);
        //        return varRepo.GetAll().Where(c => c.ItemId == p).ToList().OrderBy(c => c.CreatedDate).ToList();
        //        }
        //    }


        public static IList<DMItemBusinessBenefits> GetItemBusinessBenefits(int p)
            {
            using (var context = new DbContextFactory())
                {
                var varRepo = new Repo<DMItemBusinessBenefits>(context);
                return varRepo.GetAll().Where(c => c.ItemId == p).ToList().OrderBy(c => c.CreatedDate).ToList();
                }
            }

        //public static IList<DMDemand> GetDemand()
        //    {

        //    using (var context = new DbContextFactory())
        //        {
        //        var varRepo = new Repo<DMDemand>(context);
        //        return varRepo.GetAll().ToList();
        //        }
        //    }

        //internal static IList<DMPostDemand> GetPostDemands(int p)
        //    {
        //    using (var context = new DbContextFactory())
        //        {
        //        var varRepo = new Repo<DMPostDemand>(context);
        //        return varRepo.GetAll().Where(c => c.ClientId == p).ToList();
        //        }
        //    }

        //public static IList<vwDMaccountwithcat> GetCatAndDemand()
        //    {
        //    using (var context = new DbContextFactory())
        //        {
        //        var varRepo = new Repo<vwDMaccountwithcat>(context);
        //        return varRepo.GetAll().ToList();
        //        }
        //    }

        public static IList<DMMetricTarget> GetMetricTargetDetails(int Id)
        {
            IList<DemandMetricTargetModel> metrictargetdetails = new List<DemandMetricTargetModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IList<DMMetricTarget> MetricTargetDetail = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                if ( Id != 0)
                    MetricTargetDetail = unitOfWork.DMMetricTarget.Get(c => c.ItemId == Id).ToList();

                return MetricTargetDetail;
            }
        }

        public static int CreateMetricTarget(DemandMetricTargetModel metrictargetmodel, int Id)
        {
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var surQ = new DMMetricTarget();
                    surQ.ItemId = Id;
                    surQ.MetricTarget = metrictargetmodel.MetricTarget;
                    surQ.CreatedDate = DateTime.Now;
                    surQ.UpdatedDate = DateTime.Now;
                    surQ.CreatedBy = metrictargetmodel.CreatedBy;
                    surQ.UpdatedBy = metrictargetmodel.UpdatedBy;
                    unitOfWork.DMMetricTarget.Insert(surQ);
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

        public static bool UpdateMetricTarget(DemandMetricTargetModel metrictargetmodel)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.DMMetricTarget.GetByID(metrictargetmodel.Id);
                    updatetitem.MetricTarget = metrictargetmodel.MetricTarget;
                    updatetitem.UpdatedBy = metrictargetmodel.UpdatedBy;
                    updatetitem.UpdatedDate = DateTime.Now;
                    unitOfWork.DMMetricTarget.Update(updatetitem);
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
                unitOfWork.DMMetricTarget.Delete(Id);
                unitOfWork.Save();

                //TempData["SLACreatedDeleted"] = "Deleted successfully.";
                return true;
            }
        }

        //public static IList<DMRoleModel> GetRoleDetails(int ClientId, int uid)
        //{

        //    IList<DMRoleModel> Role = new List<DMRoleModel>();
        //    var entityMapper = new Mapper<DMRoleModel, DMRole>();

        //    using (var contextFactory = new DbContextFactory())
        //    {
        //        IEnumerable<DMRole> RoleDetails = null;
        //        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

        //        if (ClientId != null && ClientId != 0)
        //            RoleDetails = unitOfWork.DMRole.Get(cid => cid.ClientId == ClientId).ToList();

        //        if (RoleDetails != null)
        //        {
        //            foreach (var _roledetail in RoleDetails)
        //            {
        //                DMRoleModel p = new DMRoleModel();
        //                p.Id = _roledetail.Id;
        //                p.ClientId = _roledetail.ClientId;
        //                p.RoleName = _roledetail.RoleName;
        //                p.CreatedBy = _roledetail.CreatedBy;
        //                p.UpdatedBy = _roledetail.UpdatedBy;
        //                p.Createddate = _roledetail.Createddate;
        //                p.Updateddate = _roledetail.Updateddate;
        //                Role.Add(p);
        //            }
        //        }
        //        return Role;
        //    }

        //}


        //public static int CreateRole(DMRoleModel DMrolemodel, int Id)
        //{
        //    int id = 0;
        //    try
        //    {
        //        using (var contextFactory = new DbContextFactory())
        //        {
        //            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
        //            var rol = new DMRole();
        //            rol.ClientId = DMrolemodel.ClientId;
        //            rol.RoleName = DMrolemodel.RoleName;
        //            rol.Createddate = DateTime.Now;
        //            rol.Updateddate = DateTime.Now;
        //            rol.CreatedBy = DMrolemodel.CreatedBy;
        //            rol.UpdatedBy = DMrolemodel.UpdatedBy;
        //            unitOfWork.DMRole.Insert(rol);
        //            unitOfWork.Save();
        //            id = rol.Id;
        //            return id;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }
        //}

        //public static bool UpdateRole(DMRoleModel DMrolemodel)
        //{
        //    try
        //    {
        //        using (var contextFactory = new DbContextFactory())
        //        {
        //            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
        //            var updatetitem = unitOfWork.DMRole.GetByID(DMrolemodel.Id);
        //            updatetitem.ClientId = DMrolemodel.ClientId;
        //            updatetitem.RoleName = DMrolemodel.RoleName;
        //            updatetitem.UpdatedBy = DMrolemodel.UpdatedBy;
        //            updatetitem.Updateddate = DateTime.Now;
        //            unitOfWork.DMRole.Update(updatetitem);
        //            unitOfWork.Save();
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        //public static bool RemoveRole(int Id)
        //{
        //    using (var contextFactory = new DbContextFactory())
        //    {
        //        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
        //        unitOfWork.DMRole.Delete(Id);
        //        unitOfWork.Save();

        //        //TempData["SLACreatedDeleted"] = "Deleted successfully.";
        //        return true;
        //    }
        //}

        //public static IList<DMResourceResponseModel> GetResourceDetails(int ClientId, int uid)
        //{

        //    IList<DMResourceResponseModel> Resource = new List<DMResourceResponseModel>();
        //    var entityMapper = new Mapper<DMResourceResponseModel, DMResource>();

        //    using (var contextFactory = new DbContextFactory())
        //    {
        //        IEnumerable<DMResource> ResourceDetails = null;
        //        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

        //        if (ClientId != null && ClientId != 0)
        //            ResourceDetails = unitOfWork.DMResource.Get(cid => cid.ClientId == ClientId).ToList();

        //        if (ResourceDetails != null)
        //        {
        //            foreach (var _resourcedetail in ResourceDetails)
        //            {
        //                DMResourceResponseModel p = new DMResourceResponseModel();
        //                p.Id = _resourcedetail.Id;
        //                p.ClientId = _resourcedetail.ClientId;
        //                p.ResourceName = _resourcedetail.ResourceName;
        //                p.CreatedBy = _resourcedetail.CreatedBy;
        //                p.UpdatedBy = _resourcedetail.UpdatedBy;
        //                p.Createddate = _resourcedetail.Createddate;
        //                p.Updateddate = _resourcedetail.Updateddate;
        //                Resource.Add(p);
        //            }
        //        }
        //        return Resource;
        //    }

        //}
        //public static int CreateResource(DMResourceResponseModel DMresourcemodel, int Id)
        //{
        //    int id = 0;

        //    try
        //    {
        //        using (var contextFactory = new DbContextFactory())
        //        {
        //            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
        //            var res = new DMResource();
        //            //rol.Id = Id;
        //            res.ClientId = DMresourcemodel.ClientId;
        //            res.ResourceName = DMresourcemodel.ResourceName;
        //            res.Createddate = DateTime.Now;
        //            res.Updateddate = DateTime.Now;
        //            res.CreatedBy = DMresourcemodel.CreatedBy;
        //            res.UpdatedBy = DMresourcemodel.UpdatedBy;
        //            unitOfWork.DMResource.Insert(res);
        //            unitOfWork.Save();
        //            id = res.Id;
        //            return id;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }
        //}
        //public static bool UpdateResource(DMResourceResponseModel DMresourcemodel)
        //{
        //    try
        //    {
        //        using (var contextFactory = new DbContextFactory())
        //        {
        //            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
        //            var updatetitem = unitOfWork.DMResource.GetByID(DMresourcemodel.Id);
        //            updatetitem.ClientId = DMresourcemodel.ClientId;
        //            updatetitem.ResourceName = DMresourcemodel.ResourceName;
        //            updatetitem.UpdatedBy = DMresourcemodel.UpdatedBy;
        //            updatetitem.Updateddate = DateTime.Now;
        //            unitOfWork.DMResource.Update(updatetitem);
        //            unitOfWork.Save();
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
        //public static bool RemoveResource(int Id)
        //{
        //    using (var contextFactory = new DbContextFactory())
        //    {
        //        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
        //        unitOfWork.DMResource.Delete(Id);
        //        unitOfWork.Save();
        //        return true;
        //    }
        //}

        //public static IList<DMProgressTemplateModel> GetProgessDetails(int phaseId)
        //{
        //    IList<DMProgressTemplateModel> progresstemplate = new List<DMProgressTemplateModel>();
        //    var entityMapper = new Mapper<DMProgressTemplateModel, DMProgressTemplate>();

        //    using (var contextFactory = new DbContextFactory())
        //    {
        //        IEnumerable<DMProgressTemplate> ProgressDetails = null;
        //        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
        //        if (phaseId != null && phaseId != 0)
        //            ProgressDetails = unitOfWork.DMProgressTemplate.Get(cid => cid.Phase == phaseId).ToList();

        //        if (ProgressDetails != null)
        //        {
        //            foreach (var _progdetail in ProgressDetails)
        //            {
        //                DMProgressTemplateModel pro = new DMProgressTemplateModel();
        //                pro.ClientId = _progdetail.ClientId;
        //                pro.ProgressId = _progdetail.Id;
        //                pro.PhaseId = _progdetail.Phase;
        //                pro.Progress = _progdetail.Progress;
        //                progresstemplate.Add(pro);
        //            }
        //        }
        //        return progresstemplate;
        //    }
        //}


        //public static int CreateProgess(DMProgressTemplateModel DmProgresstemplatemModel, int PId)
        //{
        //    int id = 0;

        //    try
        //    {
        //        using (var contextFactory = new DbContextFactory())
        //        {
        //            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
        //            var ipt = new DMProgressTemplate();
        //            ipt.ClientId = DmProgresstemplatemModel.ClientId;
        //            ipt.Phase = PId;
        //            ipt.Progress = DmProgresstemplatemModel.Progress;
        //            ipt.CreatedBy = DmProgresstemplatemModel.CreatedBy;
        //            ipt.Createddate = DateTime.Now;
        //            ipt.UpdatedBy = DmProgresstemplatemModel.UpdatedBy;
        //            ipt.Updateddate = DateTime.Now;
        //            unitOfWork.DMProgressTemplate.Insert(ipt);
        //            unitOfWork.Save();
        //            id = ipt.Id;
        //            return id;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }
        //}



        //public static bool UpdateProgress(DMProgressTemplateModel DmProgresstemplatemModel)
        //{
        //    try
        //    {
        //        using (var contextFactory = new DbContextFactory())
        //        {
        //            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
        //            var progressdet = unitOfWork.DMProgressTemplate.GetByID(DmProgresstemplatemModel.ProgressId);

        //            progressdet.Progress = DmProgresstemplatemModel.Progress;
        //            progressdet.UpdatedBy = DmProgresstemplatemModel.UpdatedBy;
        //            progressdet.Updateddate = DateTime.Now;

        //            unitOfWork.DMProgressTemplate.Update(progressdet);
        //            unitOfWork.Save();
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}


        //public static bool RemoveProgress(int Id)
        //{
        //    using (var contextFactory = new DbContextFactory())
        //    {
        //        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
        //        unitOfWork.DMProgressTemplate.Delete(Id);
        //        unitOfWork.Save();
        //        return true;
        //    }
        //}

        /*-------------------------new meta data -------------------------------------*/

        public static IList<DMComplexityModel> GetComplexityDetails(int ClientId, int uid)
        {

            IList<DMComplexityModel> Complex = new List<DMComplexityModel>();
            Db db = new Db();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<DMComplexity> ComplexityDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (ClientId != 0)
                    //ComplexityDetails = unitOfWork.DMComplexity.Get(cid => cid.ClientId == ClientId).ToList();
                    ComplexityDetails = (from a in db.DMComplexity
                                         where a.ClientId == ClientId
                                         select a);
                if (ComplexityDetails != null)
                {
                    foreach (var _complexitydetail in ComplexityDetails)
                    {
                        DMComplexityModel cs = new DMComplexityModel();
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
        public static int CreateComplexity(DMComplexityModel imcomplexityModel, int Id)
        {
            int id = 0;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var com = new DMComplexity();
                    com.ClientId = imcomplexityModel.ClientId;
                    com.Complexity = imcomplexityModel.Complexity;
                    com.CreatedDt = DateTime.Now;
                    com.CreatedBy = imcomplexityModel.CreatedBy;
                    unitOfWork.DMComplexity.Insert(com);
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
        public static bool UpdateComplexity(DMComplexityModel imcomplexityModel)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.DMComplexity.GetByID(imcomplexityModel.Id);
                    updatetitem.ClientId = imcomplexityModel.ClientId;
                    updatetitem.Complexity = imcomplexityModel.Complexity;
                    updatetitem.UpdatedBy = imcomplexityModel.UpdatedBy;
                    updatetitem.UpdatedDt = DateTime.Now;
                    unitOfWork.DMComplexity.Update(updatetitem);
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
                unitOfWork.DMComplexity.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }

        public static IList<DMDemandTypeModel> GetDemandTypeDetails(int ClientId, int uid)
        {

            IList<DMDemandTypeModel> DemandType = new List<DMDemandTypeModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<DMDemandType> DemandTypeDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (ClientId != 0)
                    DemandTypeDetails = unitOfWork.DMDemandType.Get(cid => cid.ClientId == ClientId).ToList();

                if (DemandTypeDetails != null)
                {
                    foreach (var _demandtypedetail in DemandTypeDetails)
                    {
                        DMDemandTypeModel cs = new DMDemandTypeModel();
                        cs.Id = _demandtypedetail.Id;
                        cs.ClientId = _demandtypedetail.ClientId;
                        cs.DemandType = _demandtypedetail.DemandType;
                        cs.CreatedBy = _demandtypedetail.CreatedBy;
                        cs.UpdatedBy = _demandtypedetail.UpdatedBy;
                        cs.CreatedDate = _demandtypedetail.CreatedDate;
                        cs.UpdatedBy = _demandtypedetail.UpdatedBy;
                        DemandType.Add(cs);
                    }
                }
                return DemandType;
            }

        }
        public static int CreateDemandType(DMDemandTypeModel dmDemandTypeModel, int Id)
        {
            int id = 0;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var Demandtype = new DMDemandType();
                    Demandtype.ClientId = dmDemandTypeModel.ClientId;
                    Demandtype.DemandType = dmDemandTypeModel.DemandType;
                    Demandtype.CreatedDate = DateTime.Now;
                    Demandtype.CreatedBy = dmDemandTypeModel.CreatedBy;
                    unitOfWork.DMDemandType.Insert(Demandtype);
                    unitOfWork.Save();
                    id = Demandtype.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static bool UpdateDemandType(DMDemandTypeModel dmDemandTypeModel)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.DMDemandType.GetByID(dmDemandTypeModel.Id);
                    updatetitem.ClientId = dmDemandTypeModel.ClientId;
                    updatetitem.DemandType = dmDemandTypeModel.DemandType;
                    updatetitem.UpdatedBy = dmDemandTypeModel.UpdatedBy;
                    updatetitem.LastUpdatedDate = DateTime.Now;
                    unitOfWork.DMDemandType.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool RemoveDemandType(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.DMDemandType.Delete(Id);
                unitOfWork.Save();

                //TempData["SLACreatedDeleted"] = "Deleted successfully.";
                return true;
            }
        }


        //public static IList<DMDemandSourceModel> GetDemandSourceDetails(int ClientId, int uid)
        //{

        //    IList<DMDemandSourceModel> DemandSource = new List<DMDemandSourceModel>();
        //    using (var contextFactory = new DbContextFactory())
        //    {
        //        IEnumerable<DMDemandSource> DemandSourceDetails = null;
        //        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

        //        if (ClientId != null && ClientId != 0)
        //            DemandSourceDetails = unitOfWork.DMDemandSource.Get(cid => cid.ClientId == ClientId).ToList();

        //        if (DemandSourceDetails != null)
        //        {
        //            foreach (var _demandsourcedetail in DemandSourceDetails)
        //            {
        //                DMDemandSourceModel cs = new DMDemandSourceModel();
        //                cs.Id = _demandsourcedetail.Id;
        //                cs.ClientId = _demandsourcedetail.ClientId;
        //                cs.DemandSource = _demandsourcedetail.DemandSource;
        //                cs.CreatedBy = _demandsourcedetail.CreatedBy;
        //                cs.UpdatedBy = _demandsourcedetail.UpdatedBy;
        //                cs.CreatedDate = _demandsourcedetail.CreatedDate;
        //                cs.UpdatedBy = _demandsourcedetail.UpdatedBy;
        //                DemandSource.Add(cs);
        //            }
        //        }
        //        return DemandSource;
        //    }

        //}
        //public static int CreateDemandSource(DMDemandSourceModel dmDemandSourceModel, int Id)
        //{
        //    int id = 0;
        //    try
        //    {
        //        using (var contextFactory = new DbContextFactory())
        //        {
        //            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
        //            var demandsource = new DMDemandSource();
        //            demandsource.ClientId = dmDemandSourceModel.ClientId;
        //            demandsource.DemandSource = dmDemandSourceModel.DemandSource;
        //            demandsource.CreatedDate = DateTime.Now;
        //            demandsource.CreatedBy = dmDemandSourceModel.CreatedBy;
        //            unitOfWork.DMDemandSource.Insert(demandsource);
        //            unitOfWork.Save();
        //            id = demandsource.Id;
        //            return id;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }
        //}
        //public static bool UpdateDemandSource(DMDemandSourceModel dmDemandSourceModel)
        //{
        //    try
        //    {
        //        using (var contextFactory = new DbContextFactory())
        //        {
        //            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
        //            var updatetitem = unitOfWork.DMDemandSource.GetByID(dmDemandSourceModel.Id);
        //            updatetitem.ClientId = dmDemandSourceModel.ClientId;
        //            updatetitem.DemandSource = dmDemandSourceModel.DemandSource;
        //            updatetitem.UpdatedBy = dmDemandSourceModel.UpdatedBy;
        //            updatetitem.LastUpdatedDate = DateTime.Now;
        //            unitOfWork.DMDemandSource.Update(updatetitem);
        //            unitOfWork.Save();
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
        //public static bool RemoveDemandSource(int Id)
        //{
        //    using (var contextFactory = new DbContextFactory())
        //    {
        //        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
        //        unitOfWork.DMDemandSource.Delete(Id);
        //        unitOfWork.Save();
        //        return true;
        //    }
        //}

        public static IList<DMLOBModel> GetLOBDetails(int ClientId, int uid)
        {
            IList<DMLOBModel> LOB = new List<DMLOBModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<DMLOB> LOBDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                if (ClientId != 0)
                    LOBDetails = unitOfWork.DMLOB.Get(cid => cid.ClientId == ClientId).ToList();
                if (LOBDetails != null)
                {
                    foreach (var _lobdetail in LOBDetails)
                    {
                        DMLOBModel cs = new DMLOBModel();
                        cs.Id = _lobdetail.Id;
                        cs.ClientId = _lobdetail.ClientId;
                        cs.LOB = _lobdetail.DemandLOB;
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
        public static int CreateLOB(DMLOBModel DmlOBModel, int Id)
        {
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var Lob = new DMLOB();
                    Lob.ClientId = DmlOBModel.ClientId;
                    Lob.DemandLOB = DmlOBModel.LOB;
                    Lob.CreatedDate = DateTime.Now;
                    Lob.CreatedBy = DmlOBModel.CreatedBy;
                    unitOfWork.DMLOB.Insert(Lob);
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

        public static bool UpdateLOB(DMLOBModel DmlOBModel)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.DMLOB.GetByID(DmlOBModel.Id);
                    updatetitem.ClientId = DmlOBModel.ClientId;
                    updatetitem.DemandLOB = DmlOBModel.LOB;
                    updatetitem.UpdatedBy = DmlOBModel.UpdatedBy;
                    updatetitem.LastUpdatedDate = DateTime.Now;
                    unitOfWork.DMLOB.Update(updatetitem);
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
                unitOfWork.DMLOB.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }

        public static IList<DMClassificationModel> GetClassificationDetails(int ClientId,int uid)
        {
            IList<DMClassificationModel> Classification = new List<DMClassificationModel>();      

            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<DMClassification> ClassificationDetail = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                if (ClientId != 0)
                    ClassificationDetail = unitOfWork.DMClassification.Get(cid => cid.ClientId == ClientId).ToList();

                if (ClassificationDetail != null)
                {
                    foreach (var _classificationdetail in ClassificationDetail)
                    {
                        DMClassificationModel DMClassificationModel = new DMClassificationModel();
                        DMClassificationModel.CalId = _classificationdetail.Id;
                        DMClassificationModel.Classification = _classificationdetail.Classification;
                        DMClassificationModel.ClientId = _classificationdetail.ClientId;
                        DMClassificationModel.CreatedBy = _classificationdetail.CreatedBy;
                        DMClassificationModel.CreatedDate = _classificationdetail.CreatedDate;
                        DMClassificationModel.UpdatedBy = _classificationdetail.UpdatedBy;
                        DMClassificationModel.LastUpdatedDate = _classificationdetail.LastUpdatedDate;
                        Classification.Add(DMClassificationModel);
                    }
                }
                return Classification;
            }
        }
       
        public static int CreateClassification(DMClassificationModel DmclassificationModel)
        {
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    DMClassification ClassificationModel = new DMClassification();
                    ClassificationModel.Classification = DmclassificationModel.Classification;
                    ClassificationModel.ClientId = DmclassificationModel.ClientId;
                    ClassificationModel.CreatedBy = DmclassificationModel.CreatedBy;
                    ClassificationModel.CreatedDate = DmclassificationModel.CreatedDate;
                    unitOfWork.DMClassification.Insert(ClassificationModel);
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
        
        public static bool UpdateClassification(DMClassificationModel DmclassificationModel)
        {
            try
            {               
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var classificationinfo = unitOfWork.DMClassification.GetByID(DmclassificationModel.CalId);
                    classificationinfo.Classification = DmclassificationModel.Classification;
                    classificationinfo.UpdatedBy = DmclassificationModel.UpdatedBy;
                    classificationinfo.LastUpdatedDate = DmclassificationModel.LastUpdatedDate;
                    unitOfWork.DMClassification.Update(classificationinfo);
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
                unitOfWork.DMClassification.Delete(ClassId);
                unitOfWork.Save();
                return true;
            }
        }

        public static IList<DMCategoryModel> GetCategoryDetails(int CattId = 0)
        {
            IList<DMCategoryModel> categoryy = new List<DMCategoryModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<DMCategory> DMCategoryDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                if (CattId != 0)
                    DMCategoryDetails = unitOfWork.DMCategory.Get(cid => cid.ClassificationId == CattId).ToList();

                if (DMCategoryDetails != null)
                {
                    foreach (var _categorydetail in DMCategoryDetails)
                    {
                        DMCategoryModel categoryModel = new DMCategoryModel();
                        categoryModel.Category = _categorydetail.Category;
                        categoryModel.CaId = _categorydetail.Id;
                        categoryModel.clientid = _categorydetail.ClientId;
                        categoryy.Add(categoryModel);
                    }
                }
                return categoryy;
            }
        }

        public static int CreateCategory(DMCategoryModel DmcategoryModel)
        {
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    DMCategory CategoryInfo = new DMCategory();
                    CategoryInfo.ClientId = DmcategoryModel.clientid;
                    CategoryInfo.ClassificationId = DmcategoryModel.ClassificationId;
                    CategoryInfo.Category = DmcategoryModel.Category;               
                    CategoryInfo.CreatedBy = DmcategoryModel.CreatedBy;
                    CategoryInfo.CreatedDate = DmcategoryModel.CreatedDate;
                    unitOfWork.DMCategory.Insert(CategoryInfo);
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


        public static bool UpdateCategory(DMCategoryModel DmcategoryModel)
        {
            try
            {

                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var categoryinfo = unitOfWork.DMCategory.GetByID(DmcategoryModel.CaId);
                    // entityMapper.MapToEntity(Portfolioinput, appPortfolioinfo);                    
                    categoryinfo.Category = DmcategoryModel.Category;        
                    categoryinfo.UpdatedBy = DmcategoryModel.UpdatedBy;
                    categoryinfo.LastUpdatedDate = DmcategoryModel.LastUpdatedDate;
                    unitOfWork.DMCategory.Update(categoryinfo);
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
                unitOfWork.DMCategory.Delete(catId);
                unitOfWork.Save();
                return true;
            }
        }

        public static string[] GetDemandApprovers(int DemandId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var listApprovers = unitOfWork.DMDemandApprover.Get().Where(g => g.DemandId == DemandId);

            var strLst= listApprovers.Select(s => s.ApproverId.ToString()).ToArray();
            return strLst;
        }



        //-------------------------------------------Configuration Type--------------------------------------------//


        public static IList<DMConfigTypeModel> GetConfigurationtypeDetails(int ClientId, int uid)
        {
            IList<DMConfigTypeModel> Configuration = new List<DMConfigTypeModel>();

            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<DMConfigType> ConfigurationDetail = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                if (ClientId != 0)
                    ConfigurationDetail = unitOfWork.DMConfigType.Get(cid => cid.ClientId == ClientId).ToList();

                if (ConfigurationDetail != null)
                {
                    foreach (var _configurationdetail in ConfigurationDetail)
                    {
                        DMConfigTypeModel DMConfigTypeModel = new DMConfigTypeModel();
                        DMConfigTypeModel.CTId = _configurationdetail.Id;
                        DMConfigTypeModel.ConfigType = _configurationdetail.ConfigType;
                        DMConfigTypeModel.ClientId = _configurationdetail.ClientId;
                        DMConfigTypeModel.CreatedBy = _configurationdetail.CreatedBy;
                        DMConfigTypeModel.CreatedDt = _configurationdetail.CreatedDt;
                        DMConfigTypeModel.UpdatedBy = _configurationdetail.UpdatedBy;
                        DMConfigTypeModel.UpdatedDt = _configurationdetail.UpdatedDt;
                        Configuration.Add(DMConfigTypeModel);
                    }
                }
                return Configuration;
            }
        }

        public static int CreateConfigurationtype(DMConfigTypeModel DmconfigTypeModel)
        {
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    DMConfigType DmconfigType = new DMConfigType();
                    DmconfigType.ConfigType = DmconfigTypeModel.ConfigType;
                    DmconfigType.ClientId = DmconfigTypeModel.ClientId;
                    DmconfigType.CreatedBy = DmconfigTypeModel.CreatedBy;
                    DmconfigType.CreatedDt = DmconfigTypeModel.CreatedDt;
                    unitOfWork.DMConfigType.Insert(DmconfigType);
                    unitOfWork.Save();
                    id = DmconfigType.Id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
            return id;
        }

        public static bool UpdateConfigurationtype(DMConfigTypeModel DmconfigTypeModel)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var ConfigTypeInfo = unitOfWork.DMConfigType.GetByID(DmconfigTypeModel.CTId);
                    ConfigTypeInfo.ConfigType = DmconfigTypeModel.ConfigType;
                    ConfigTypeInfo.UpdatedBy = DmconfigTypeModel.UpdatedBy;
                    ConfigTypeInfo.UpdatedDt = DmconfigTypeModel.UpdatedDt;
                    unitOfWork.DMConfigType.Update(ConfigTypeInfo);
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
                unitOfWork.DMConfigType.Delete(CtypeId);
                unitOfWork.Save();
                return true;
            }
        }
        //---------------------------------------Configuration Item-----------------------------------------------//
        public static IList<DMConfigItemModel> GetConfigurationitemDetails(int ConId = 0)
        {
            IList<DMConfigItemModel> configitems = new List<DMConfigItemModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<DMConfigItem> DMConfigItemDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                if (ConId != 0)
                    DMConfigItemDetails = unitOfWork.DMConfigItem.Get(cid => cid.ConfigTypeId == ConId).ToList();

                if (DMConfigItemDetails != null)
                {
                    foreach (var _citemdetail in DMConfigItemDetails)
                    {
                        DMConfigItemModel CitemModel = new DMConfigItemModel();
                        CitemModel.ConfigItem = _citemdetail.ConfigItem;
                        CitemModel.CIId = _citemdetail.Id;
                        CitemModel.ClientId = _citemdetail.ClientId;
                        configitems.Add(CitemModel);
                    }
                }
                return configitems;
            }
        }

        public static int CreateConfigurationitem(DMConfigItemModel DmconfigItemModel)
        {
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    DMConfigItem configitemInfo = new DMConfigItem();
                    configitemInfo.ClientId = DmconfigItemModel.ClientId;
                    configitemInfo.ConfigTypeId = DmconfigItemModel.ConfigTypeId;
                    configitemInfo.ConfigItem = DmconfigItemModel.ConfigItem;
                    configitemInfo.CreatedBy = DmconfigItemModel.CreatedBy;
                    configitemInfo.CreatedDt = DmconfigItemModel.CreatedDt;
                    unitOfWork.DMConfigItem.Insert(configitemInfo);
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

        public static bool UpdateConfigurationitem(DMConfigItemModel DmconfigItemModel)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var configIteminfo = unitOfWork.DMConfigItem.GetByID(DmconfigItemModel.CIId);
                    configIteminfo.ConfigItem = DmconfigItemModel.ConfigItem;
                    configIteminfo.UpdatedBy = DmconfigItemModel.UpdatedBy;
                    configIteminfo.UpdatedDt = DmconfigItemModel.UpdatedDt;
                    unitOfWork.DMConfigItem.Update(configIteminfo);
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
                unitOfWork.DMConfigItem.Delete(CitemId);
                unitOfWork.Save();
                return true;
            }
        }

        public static bool RemoveDemand(int Id)
        {
            try {
                var paramName = new string[] { "DemandId" };
                var paramValue = new object[] { Id };
                // DataTable dt = new DataTable();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new Data.UnitOfWorks(contextFactory.GetContext());
                    var dt = unitOfWork.GetDataFromStoredProcedure("DeleteDMPostDemand", paramName, paramValue);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

           
        }
        //------------------------Task metadata-----------------------//

        public static IList<DMDemandTaskModel> GetTaskDetails(int ClientId,int uid)
        {
            Db db = new Db();
            IList<DMDemandTaskModel> DemandTask = new List<DMDemandTaskModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<DMTask> DemandTaskDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var task = (from a in db.DMTask
                            where a.ClientId == ClientId
                            select a.DemandTask).ToList();
                if (!task.Contains("Gate 01 - Ops Lead Review"))
                {
                    var Demandtask = new DMTask();
                    Demandtask.ClientId = ClientId;
                    Demandtask.DemandTask = "Gate 01 - Ops Lead Review";
                    Demandtask.CreatedDate = DateTime.Now;
                    Demandtask.CreatedBy = uid;
                    unitOfWork.DMTask.Insert(Demandtask);
                    unitOfWork.Save();
                }
                if (!task.Contains("Gate 16 - CLOSED"))
                {
                    var Demandtask = new DMTask();
                    Demandtask.ClientId = ClientId;
                    Demandtask.DemandTask = "Gate 16 - CLOSED";
                    Demandtask.CreatedDate = DateTime.Now;
                    Demandtask.CreatedBy = uid;
                    unitOfWork.DMTask.Insert(Demandtask);
                    unitOfWork.Save();
                }
                if (ClientId != 0)
                    DemandTaskDetails = (from a in db.DMTask
                                         where a.ClientId == ClientId
                                         select a).ToList();
                if (DemandTaskDetails != null)
                {
                    foreach (var _demandtaskdetail in DemandTaskDetails)
                    {
                        DMDemandTaskModel cs = new DMDemandTaskModel();
                        cs.Id = _demandtaskdetail.Id;
                        cs.ClientId = _demandtaskdetail.ClientId;
                        cs.DemandTask = _demandtaskdetail.DemandTask;
                        cs.CreatedBy = _demandtaskdetail.CreatedBy;
                        cs.UpdatedBy = _demandtaskdetail.UpdatedBy;
                        cs.CreatedDate = _demandtaskdetail.CreatedDate;
                        cs.UpdatedBy = _demandtaskdetail.UpdatedBy;
                        DemandTask.Add(cs);
                    }
                }
                return DemandTask;
            }
        }  
        public static int CreateDemandTask(DMDemandTaskModel dmDemandTaskModel, int Id)
        {
            int id = 0;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var Demandtask = new DMTask();
                    Demandtask.ClientId = dmDemandTaskModel.ClientId;
                    Demandtask.DemandTask = dmDemandTaskModel.DemandTask;
                    Demandtask.CreatedDate = DateTime.Now;
                    Demandtask.CreatedBy = dmDemandTaskModel.CreatedBy;
                    unitOfWork.DMTask.Insert(Demandtask);
                    unitOfWork.Save();
                    id = Demandtask.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static bool UpdateDemandTask(DMDemandTaskModel dmDemandTaskModel)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.DMTask.GetByID(dmDemandTaskModel.Id);
                    updatetitem.ClientId = dmDemandTaskModel.ClientId;
                    updatetitem.DemandTask = dmDemandTaskModel.DemandTask;
                    updatetitem.UpdatedBy = dmDemandTaskModel.UpdatedBy;
                    updatetitem.LastUpdatedDate = DateTime.Now;
                    unitOfWork.DMTask.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool RemoveDemandTask(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                Db db = new Db();
                var tasklist = (from a in db.DMTaskAndActivities
                                where a.TaskId == Id
                                select a.Id).ToList();
                foreach(var item in tasklist)
                {
                    unitOfWork.DMTask.Delete(item);
                    unitOfWork.Save();
                }
                unitOfWork.DMTask.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }


        //------------------------Stage metadata-----------------------//

        public static IList<DMDemandStageModel> GetStageDetails(int ClientId, int uid)
        {
            IList<DMDemandStageModel> DemandStage = new List<DMDemandStageModel>();
            using (var contextFactory = new DbContextFactory())
            {
                Db db = new Db();
                IEnumerable<DMStage> DemandStageDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var stage = (from a in db.DMStage
                            where a.ClientId == ClientId
                            select a.DemandStage).ToList();
                foreach(var item in Enum.GetValues(typeof(DMDefaultStage)))
                {
                    if (!stage.Contains(item.ToString()))
                    {
                        var addDemandStage= new DMStage();
                        addDemandStage.ClientId = ClientId;
                        addDemandStage.DemandStage = item.ToString();
                        addDemandStage.CreatedDate = DateTime.Now;
                        addDemandStage.CreatedBy = uid;
                        unitOfWork.DMStage.Insert(addDemandStage);
                        unitOfWork.Save();
                    }
                }
                

                if (ClientId != 0)
                    DemandStageDetails = (from a in db.DMStage
                                          where a.ClientId == ClientId
                                         select a).ToList();

                if (DemandStageDetails != null)
                {
                    foreach (var _demandtaskdetail in DemandStageDetails)
                    {
                        DMDemandStageModel cs = new DMDemandStageModel();
                        cs.Id = _demandtaskdetail.Id;
                        cs.ClientId = _demandtaskdetail.ClientId;
                        cs.DemandStage = _demandtaskdetail.DemandStage;
                        cs.CreatedBy = _demandtaskdetail.CreatedBy;
                        cs.UpdatedBy = _demandtaskdetail.UpdatedBy;
                        cs.CreatedDate = _demandtaskdetail.CreatedDate;
                        cs.UpdatedBy = _demandtaskdetail.UpdatedBy;
                        DemandStage.Add(cs);
                    }
                }
                return DemandStage;
            }
        }
        public static int CreateDemandStage(DMDemandStageModel dmDemandStageModel, int Id)
        {
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var DemandStage = new DMStage();
                    DemandStage.ClientId = dmDemandStageModel.ClientId;
                    DemandStage.DemandStage = dmDemandStageModel.DemandStage;
                    DemandStage.CreatedDate = DateTime.Now;
                    DemandStage.CreatedBy = dmDemandStageModel.CreatedBy;
                    unitOfWork.DMStage.Insert(DemandStage);
                    unitOfWork.Save();
                    id = DemandStage.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static bool UpdateDemandStage(DMDemandStageModel dmDemandStageModel)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.DMStage.GetByID(dmDemandStageModel.Id);
                    updatetitem.ClientId = dmDemandStageModel.ClientId;
                    updatetitem.DemandStage = dmDemandStageModel.DemandStage;
                    updatetitem.UpdatedBy = dmDemandStageModel.UpdatedBy;
                    updatetitem.LastUpdatedDate = DateTime.Now;
                    unitOfWork.DMStage.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool RemoveDemandStage(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.DMStage.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }

        //------------------------Document Type metadata-----------------------//

        public static IList<DMDemandDocumentTypeModel> GetDocumentTypeDetails(int ClientId)
        {
            IList<DMDemandDocumentTypeModel> DemandDocumentType = new List<DMDemandDocumentTypeModel>();
            using (var contextFactory = new DbContextFactory())
            {
                Db db = new Db();
                IEnumerable<DMDocumentType> DemandDocumentTypeDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (ClientId != 0)
                    DemandDocumentTypeDetails = (from a in db.DMDocumentType
                                          where a.ClientId == ClientId
                                          select a).ToList();

                if (DemandDocumentTypeDetails != null)
                {
                    foreach (var _demandDocumentTypedetail in DemandDocumentTypeDetails)
                    {
                        DMDemandDocumentTypeModel cs = new DMDemandDocumentTypeModel();
                        cs.Id = _demandDocumentTypedetail.Id;
                        cs.ClientId = _demandDocumentTypedetail.ClientId;
                        cs.DocumentType = _demandDocumentTypedetail.DocumentType;
                        cs.CreatedBy = _demandDocumentTypedetail.CreatedBy;
                        cs.UpdatedBy = _demandDocumentTypedetail.UpdatedBy;
                        cs.CreatedDate = _demandDocumentTypedetail.CreatedDt;
                        cs.UpdatedBy = _demandDocumentTypedetail.UpdatedBy;
                        DemandDocumentType.Add(cs);
                    }
                }
                return DemandDocumentType;
            }
        }
        public static int CreateDemandDocumentType(DMDemandDocumentTypeModel dmDemandDocumentTypeModel, int Id)
        {
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var DemandDocumentType = new DMDocumentType();
                    DemandDocumentType.ClientId = dmDemandDocumentTypeModel.ClientId;
                    DemandDocumentType.DocumentType = dmDemandDocumentTypeModel.DocumentType;
                    DemandDocumentType.CreatedDt = DateTime.Now;
                    DemandDocumentType.CreatedBy = dmDemandDocumentTypeModel.CreatedBy;
                    unitOfWork.DMDocumentType.Insert(DemandDocumentType);
                    unitOfWork.Save();
                    id = DemandDocumentType.Id;
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static bool UpdateDemandDocumentType(DMDemandDocumentTypeModel dmDemandDocumentTypeModel)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.DMDocumentType.GetByID(dmDemandDocumentTypeModel.Id);
                    updatetitem.ClientId = dmDemandDocumentTypeModel.ClientId;
                    updatetitem.DocumentType = dmDemandDocumentTypeModel.DocumentType;
                    updatetitem.UpdatedBy = dmDemandDocumentTypeModel.UpdatedBy;
                    updatetitem.UpdatedDt = DateTime.Now;
                    unitOfWork.DMDocumentType.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool RemoveDemandDocumentType(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.DMDocumentType.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }
        public static IList<DMChartDashboardModel> DMDashboardChartData(string Charttype, int ClientId, DateTime? StartDate, DateTime? EndDate)
        {
            IList<DMChartDashboardModel> Modellist = new List<DMChartDashboardModel>();
            var paramName = new string[] { "Charttype", "ClientId", "StartDate", "EndDate" };
            var paramValue = new object[] { Charttype, ClientId, StartDate, EndDate };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("DMDashboardGetChartData", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DMChartDashboardModel();


                    coldet.DemandCount = Convert.ToInt32(dr["DemandCount"]);

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

        public static IList<DMChartDashboardModel> DMDashboardTrendChartData(int ClientId, string Charttype, DateTime StartDate, DateTime EndDate)
        {
            IList<DMChartDashboardModel> Modellist = new List<DMChartDashboardModel>();
            var paramName = new string[] { "ClientId", "StartDate", "EndDate", "Dim" };
            var paramValue = new object[] { ClientId, StartDate, EndDate, Charttype };

            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("SP_DMDashboard1DemandCount_New", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DMChartDashboardModel();

                    coldet.DemandCount = Convert.ToInt32(dr["DemandCount"]);

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
                    catch { }
                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }
        public static IList<DMChartDashboardModel> DMDashboard2TrendChartData(int ClientId, string drpval, string dpnVal, DateTime StartDate, DateTime EndDate)
        {
            IList<DMChartDashboardModel> Modellist = new List<DMChartDashboardModel>();
            var paramName = new string[] { "ClientId", "StartDate", "EndDate", "Dim", "dpnVal" };
            var paramValue = new object[] { ClientId, StartDate, EndDate, drpval, dpnVal };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("SP_DMDashboard2DemandCount", paramName, paramValue);
                if (dpnVal.Length > 0)
                {
                    foreach (DataRow dr in res.Rows)
                    {
                        var coldet = new DMChartDashboardModel();
                        coldet.DemandCount = Convert.ToInt32(dr["DemandCount"]);

                        try
                        {

                            coldet.DrpCategory = dr["Category"].ToString();
                            coldet.Dim = dr["Dim_Value"].ToString(); ;// dr["Category"].ToString();
                        }
                        catch { }
                        Modellist.Add(coldet);
                    }
                }
                else
                {
                    foreach (DataRow dr in res.Rows)
                    {
                        var coldet = new DMChartDashboardModel();
                        coldet.DemandCount = Convert.ToInt32(dr["DemandCount"]);

                        try
                        {

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

        public static IList<DMChartDashboardModel> calculatePercent(IList<DMChartDashboardModel> model)
        {
            //IList<DashboardTrendModel> Modellist = new List<DashboardTrendModel>();
            // Modellist = model;
            var sum = 0;
            foreach (var item in model)
            {
                sum = sum + item.DemandCount;
            }
            if (sum != 0)
            {
                foreach (var item in model)
                {
                    item.Percent = Math.Round((decimal)(item.DemandCount * 100) / sum, 2);
                    item.AverageVal = Math.Round((decimal)(item.DemandCount) / model.Count, 2);
                }
            }

            return model;
        }

        public static IList<DMChartDashboardModel> calculateTrend(IList<DMChartDashboardModel> model)
        {
            int cnt = model.Count();
            if (cnt < 2)
                return model;
            var litem = model.ElementAt(cnt - 1);
            long LDemandCount = litem.DemandCount;
            long fDemandCount = model.ElementAt(0).DemandCount;
            var slope = (LDemandCount - fDemandCount) / (cnt - 1);
            var offset = fDemandCount - (slope * 1);
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
        public static IList<DemandDashboardModel> DMDashboardDrilldownData(string Charttype, int ClientId, DateTime? StartDate, DateTime? EndDate, string Category)
        {
            IList<DemandDashboardModel> Modellist = new List<DemandDashboardModel>();
            var paramName = new string[] { "Charttype", "ClientId", "StartDate", "EndDate", "CategoryValue" };
            var paramValue = new object[] { Charttype, ClientId, StartDate, EndDate, Category };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("DMDashboardGetChartData_drilldown", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DemandDashboardModel();
                    coldet.Id = Convert.ToInt32(dr["Id"]);
                    if (!(dr["DemandNumber"] is DBNull))
                        coldet.DemandNumber = dr["DemandNumber"].ToString();
                    else
                        coldet.DemandNumber = "--NA--";
                    coldet.ItemName = dr["DemandName"].ToString();
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
                    coldet.DemandSource = (string)dr["DemandSource"];
                    if (!(dr["DemandType"] is DBNull))
                        coldet.DemandType = dr["DemandType"].ToString();
                    else
                        coldet.DemandType = "--NA--";
                    coldet.Status = dr["Status"].ToString();
                    if (!(dr["WorkStream"] is DBNull))
                        coldet.LOBDepartment = dr["WorkStream"].ToString();
                    else
                        coldet.LOBDepartment = "--NA--";
                   
                    coldet.CreatedBy = dr["CreatedBy"].ToString();
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
                    

                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }
        public static IList<DemandDashboardModel> DMDashboard2DrilldownData(int ClientId, string drpval, string dpnVal, DateTime StartDate, DateTime EndDate, string Category, string Categoryseries)
        {
            IList<DemandDashboardModel> Modellist = new List<DemandDashboardModel>();
            var paramName = new string[] { "ClientId", "StartDate", "EndDate", "Dim", "dpnVal", "categoryValue", "CategorySeriesValue" };
            var paramValue = new object[] { ClientId, StartDate, EndDate, drpval, dpnVal, Category, Categoryseries };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("SP_DMDashboard2DemandCount_drilldown", paramName, paramValue);

                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DemandDashboardModel();
                    coldet.Id = Convert.ToInt32(dr["Id"]);
                    if (!(dr["DemandNumber"] is DBNull))
                        coldet.DemandNumber = dr["DemandNumber"].ToString();
                    else
                        coldet.DemandNumber = "--NA--";
                    coldet.ItemName = dr["DemandName"].ToString();
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
                    coldet.DemandSource = (string)dr["DemandSource"];
                    if (!(dr["DemandType"] is DBNull))
                        coldet.DemandType = dr["DemandType"].ToString();
                    else
                        coldet.DemandType = "--NA--";
                    coldet.Status = dr["Status"].ToString();
                    if (!(dr["WorkStream"] is DBNull))
                        coldet.LOBDepartment = dr["WorkStream"].ToString();
                    else
                        coldet.LOBDepartment = "--NA--";
                    coldet.CreatedBy = dr["CreatedBy"].ToString();
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
                    
                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }
        public static IList<DemandDashboardModel> DMTrendDrillDownData(int ClientId, string Charttype, DateTime StartDate, DateTime EndDate, string category, string InflowOutFlowType)
        {
            IList<DemandDashboardModel> Modellist = new List<DemandDashboardModel>();
            var paramName = new string[] { "ClientId", "StartDate", "EndDate", "Dim", "category", "InflowOutFlowType" };
            var paramValue = new object[] { ClientId, StartDate, EndDate, Charttype, category, InflowOutFlowType };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("SP_DMDashboard1DemandCount_DrillDown_New", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new DemandDashboardModel();
                    coldet.Id = Convert.ToInt32(dr["Id"]);
                    if (!(dr["DemandNumber"] is DBNull))
                        coldet.DemandNumber = dr["DemandNumber"].ToString();
                    else
                        coldet.DemandNumber = "--NA--";
                    coldet.ItemName = dr["DemandName"].ToString();
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

                    if (!(dr["DemandType"] is DBNull))
                        coldet.DemandType = dr["DemandType"].ToString();
                    else
                        coldet.DemandType = "--NA--";
                    coldet.Status = dr["Status"].ToString();
                    if (!(dr["WorkStream"] is DBNull))
                        coldet.LOBDepartment = dr["WorkStream"].ToString();
                    else
                        coldet.LOBDepartment = "--NA--";
                    coldet.DemandSource = (string)dr["DemandSource"];
                    coldet.CreatedBy = dr["CreatedBy"].ToString();
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
                    
                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }

    }
}