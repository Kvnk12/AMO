using Data;
using System.Data;
using Domain.Abstract;
using Domain.Entities.ClientProfileManager;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Areas.ClientProfileManager.Dto;
using NTTData.AMOPortal.WebUI.Mapper;
using NTTData.AMOPortal.Domain.Service;
using System.IO;
using System.Text;
using Domain.Entities.IncidentNavigator;
using PagedList;
using PagedList.Mvc;
using log4net;
using WebUI.Areas.ClientProfileManager.ViewModel;
using Domain.Entities;
using System.Web.Mvc;
using WebUI.Areas.IncidentNavigator.Dto;
using Domain.Entities.Survey;
using WebUI.Dto;
using DocumentFormat.OpenXml.Drawing.ChartDrawing;
using Telerik.Web.UI;
using System.Globalization;
using WebUI.Models;

namespace WebUI.Areas.ClientProfileManager
{
    public class ClientProfileManagerHelper//:IDisposable 
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ClientProfileManagerHelper));
        // protected readonly ICrudService<Domain.Entities.ClientProfileManager.MappingTemplate> service;
        public static IList<MappingTemplate> GetAllTemplates(int? ClientId, bool getAll = false)
        {
            ////using (var contextFactory = new DbContextFactory())
            ////{
                Db db = new Db();
                // var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                if (getAll)
                    //return unitOfWork.MappingTemplate.Get().ToList();
                    return (from t in db.MappingTemplate
                            select t).ToList();
                else
                {
                    if (ClientId > 0)
                        return (from t in db.MappingTemplate
                                where !t.IsDeleted && t.ClientId == ClientId
                                select t).ToList();
                    //return unitOfWork.MappingTemplate.Get(template => !template.IsDeleted).Where(cid => cid.ClientId == ClientId).ToList();
                    else
                        return (from t in db.MappingTemplate
                                where !t.IsDeleted && t.ClientId != null
                                select t).ToList();
                    // return unitOfWork.MappingTemplate.Get(template => !template.IsDeleted && template.ClientId != null).ToList();
                    //return unitOfWork.MappingTemplate.Get(template => !template.IsDeleted).ToList();
                }
           // }
        }

        public static IList<MappingTemplateViewModel> GetAllTemplatesModel(int? ClientId, bool getAll = false)
        {
            //using (var contextFactory = new DbContextFactory())
            //{
                Db db = new Db();
                // var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                if (getAll)
                {
                    return (from t in db.MappingTemplate
                            join sa in db.StoreApplication on t.Id equals sa.MappingId
                            join saa in db.AppStore on sa.ApplicationId equals saa.Id
                            join c in db.Clients on t.ClientId equals c.Id into tmpClients
                            join u in db.UserProfiles on t.UpdatedBy equals u.Id into tmpuserUpdated
                            join uc in db.UserProfiles on t.CreatedBy equals uc.Id into tmpuserCreated
                            join ua in db.UserProfiles on t.AcknowledgeBy equals ua.Id into tmpuserAck
                            from cls in tmpClients.DefaultIfEmpty()
                            from userUpdated in tmpuserUpdated.DefaultIfEmpty()
                            from userCreated in tmpuserCreated.DefaultIfEmpty()
                            from userAck in tmpuserAck.DefaultIfEmpty()
                            select new MappingTemplateViewModel
                            {
                                ClientId = t.ClientId,
                                AppStagingTable = saa.StageTable,
                                AcknowledgeByUserName = userAck.FirstName + " " + userAck.LastName,
                                AcknowledgeBy = t.AcknowledgeBy,
                                AcknowledgeOn = t.AcknowledgeOn,
                                AppName = saa.ApplicationName,
                                BaseMappingId = t.BaseMappingId,
                                ClientName = cls.ClientName,
                                CreatedBy = t.CreatedBy,// userCreated.Id,
                                CreatedByUserName = userCreated.FirstName + " " + userCreated.LastName,
                                CreatedOn = t.CreatedOn,
                                Description = t.TemplateName,
                                TemplateName = t.TemplateName,
                                FileDelimiter = t.FileDelimiter,
                                FileFormat = t.FileFormat,
                                GlobalMappingIdentifier = t.GlobalMappingIdentifier,
                                TemplateFile = t.TemplateFile,
                                IsStandard = t.IsStandard,
                                MappingDone = t.MappingDone,
                                MappingId = t.Id,
                                StoreApplicationId = sa.ApplicationId,
                                UpdatedBy = t.UpdatedBy, // userUpdated.Id,
                                UpdatedByUserName = userUpdated.FirstName + "" + userUpdated.LastName,
                                UpdatedOn = t.UpdatedOn,
                                //ArchiveRequired = t.ArchiveRequired

                            }).ToList();

                }
                else
                {
                    if (ClientId > 0)
                        return (from t in db.MappingTemplate
                                join sa in db.StoreApplication on t.Id equals sa.MappingId
                                join saa in db.AppStore on sa.ApplicationId equals saa.Id
                                //join c in db.Clients on t.ClientId equals c.Id into tmpClients
                                join u in db.UserProfiles on t.UpdatedBy equals u.Id into tmpuserUpdated
                                join uc in db.UserProfiles on t.CreatedBy equals uc.Id into tmpuserCreated
                                join ua in db.UserProfiles on t.AcknowledgeBy equals ua.Id into tmpuserAck
                               // from cls in tmpClients.DefaultIfEmpty()
                                from userUpdated in tmpuserUpdated.DefaultIfEmpty()
                                from userCreated in tmpuserCreated.DefaultIfEmpty()
                                from userAck in tmpuserAck.DefaultIfEmpty()
                                where t.ClientId == ClientId && !t.IsDeleted
                                select new MappingTemplateViewModel
                                {
                                    ClientId = t.ClientId,
                                    AppStagingTable = saa.StageTable,
                                    AcknowledgeByUserName = userAck.FirstName + " " + userAck.LastName,
                                    AcknowledgeBy = t.AcknowledgeBy,
                                    AcknowledgeOn = t.AcknowledgeOn,
                                    AppName = saa.ApplicationName,
                                    BaseMappingId = t.BaseMappingId,
                                    //ClientName = cls.ClientName,
                                    CreatedBy = t.CreatedBy, //userCreated.Id,
                                    CreatedByUserName = userCreated.FirstName + " " + userCreated.LastName,
                                    CreatedOn = t.CreatedOn,
                                    Description = t.TemplateName,
                                    TemplateName = t.TemplateName,
                                    FileDelimiter = t.FileDelimiter,
                                    FileFormat = t.FileFormat,
                                    GlobalMappingIdentifier = t.GlobalMappingIdentifier,
                                  //  TemplateFile = t.TemplateFile,
                                    IsStandard = t.IsStandard,
                                    MappingDone = t.MappingDone,
                                    MappingId = t.Id,
                                    StoreApplicationId = sa.ApplicationId,
                                    UpdatedBy = t.UpdatedBy,// userUpdated.Id,
                                    UpdatedByUserName = userUpdated.FirstName + "" + userUpdated.LastName,
                                    UpdatedOn = t.UpdatedOn,
                                    //ArchiveRequired = t.ArchiveRequired

                                }).ToList();
                    else
                        return (from t in db.MappingTemplate
                                join sa in db.StoreApplication on t.Id equals sa.MappingId
                                join saa in db.AppStore on sa.ApplicationId equals saa.Id
                                join c in db.Clients on t.ClientId equals c.Id into tmpClients
                                join u in db.UserProfiles on t.UpdatedBy equals u.Id into tmpuserUpdated
                                join uc in db.UserProfiles on t.CreatedBy equals uc.Id into tmpuserCreated
                                join ua in db.UserProfiles on t.AcknowledgeBy equals ua.Id into tmpuserAck
                                from cls in tmpClients.DefaultIfEmpty()
                                from userUpdated in tmpuserUpdated.DefaultIfEmpty()
                                from userCreated in tmpuserCreated.DefaultIfEmpty()
                                from userAck in tmpuserAck.DefaultIfEmpty()
                                where t.ClientId != null && !t.IsDeleted
                                select new MappingTemplateViewModel
                                {
                                    ClientId = t.ClientId,
                                    AppStagingTable = saa.StageTable,
                                    AcknowledgeByUserName = userAck.FirstName + " " + userAck.LastName,
                                    AcknowledgeBy = t.AcknowledgeBy,
                                    AcknowledgeOn = t.AcknowledgeOn,
                                    AppName = saa.ApplicationName,
                                    BaseMappingId = t.BaseMappingId,
                                    ClientName = cls.ClientName,
                                    CreatedBy = t.CreatedBy, //userCreated.Id,
                                    CreatedByUserName = userCreated.FirstName + " " + userCreated.LastName,
                                    CreatedOn = t.CreatedOn,
                                    Description = t.TemplateName,
                                    TemplateName = t.TemplateName,
                                    FileDelimiter = t.FileDelimiter,
                                    FileFormat = t.FileFormat,
                                    GlobalMappingIdentifier = t.GlobalMappingIdentifier,
                                    TemplateFile = t.TemplateFile,
                                    IsStandard = t.IsStandard,
                                    MappingDone = t.MappingDone,
                                    MappingId = t.Id,
                                    StoreApplicationId = sa.ApplicationId,
                                    UpdatedBy = t.UpdatedBy, // userUpdated.Id,
                                    UpdatedByUserName = userUpdated.FirstName + "" + userUpdated.LastName,
                                    UpdatedOn = t.UpdatedOn,
                                    //ArchiveRequired = t.ArchiveRequired

                                }).ToList();
                }
          //  }
        }

        public static IList<TicketInfoViewModel> GetTicketInfoTypes(int? ClientId)
        {
            IList<TicketInfoViewModel> ticketInfoDetailViewModels = new List<TicketInfoViewModel>();
            
            if (ClientId != null)
            {
                //using (var contextFactory = new DbContextFactory())
                //{
                //    IEnumerable<TicketInfoType> tickInfoTypes = null;
                //    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                //    tickInfoTypes = unitOfWork.TicketInfoType.Get().ToList();
                Db db = new Db();
                var tickInfoTypes = ( from tctype in db.TicketInfoType  select tctype).ToList();
                if (tickInfoTypes != null)
                    {
                        foreach (var ticketInfotype in tickInfoTypes)
                        {
                            TicketInfoViewModel ticketInfoViewModel = new TicketInfoViewModel();
                            ticketInfoViewModel.TicketInfoType = ticketInfotype.Description;
                            ticketInfoViewModel.TicketInfoTypeId = ticketInfotype.Id;
                            ticketInfoViewModel.ClientId = ClientId.GetValueOrDefault();
                            ticketInfoDetailViewModels.Add(ticketInfoViewModel);
                        }
                    }

               // }
            }
            return ticketInfoDetailViewModels;
        }

        public static List<ExportTicketInfoViewModel> GetAllClientTicketInfoDetail(ClientTicketinfoSearchModel searchCriteria, int userid)
        {
            Db db = new Db();
            var clientTicketInfo = (from ct in db.ClientTicketInfo
                                    join cls in db.Clients on ct.ClientId equals cls.Id
                                    join usrapp in db.UserApplications.Select(s => new { ClientId = s.ClientId, UserId = s.UserId }).Distinct() on cls.Id equals usrapp.ClientId
                                    join clsstatus in db.ClientStatuses on cls.ClientStatus equals clsstatus.Id
                                    join v in db.IndustryVerticals on cls.VerticalId equals v.Id into tmpverticals
                                    join sv in db.IndustrySubVerticals on cls.SubVerticalId equals sv.Id into tmpsubverticals
                                    where usrapp.UserId == userid
                                    from verticals in tmpverticals.DefaultIfEmpty()
                                    from subverticals in tmpsubverticals.DefaultIfEmpty()

                                    select new ExportTicketInfoViewModel
                                    {
                                        TicketId = ct.Id,
                                        ClientId = cls.Id,
                                        ClientName = cls.ClientName,
                                        ClientStatus = cls.ClientStatus,
                                        ClientStatusName = clsstatus.Status,
                                        Year = ct.Year,
                                        Quarter = ct.Quarter,
                                        FTEsNearShore = ct.NttFTENearShore,
                                        FTEsOnShore = ct.NttFTEOnsite,
                                        OffShore = ct.NttFTEOffshore,
                                        SrMTTRSev1 = ct.SrMTTRSev1,
                                        SrMTTRSev2 = ct.SrMTTRSev2,
                                        SrMTTRSev3 = ct.InMTTRSev3,
                                        SrMTTRSev4 = ct.InMTTRSev4,

                                        SRBacklog = ct.SRBacklog,

                                        InMTTRSev1 = ct.InMTTRSev1,
                                        InMTTRSev2 = ct.InMTTRSev2,
                                        InMTTRSev3 = ct.InMTTRSev3,
                                        InMTTRSev4 = ct.InMTTRSev4,

                                        ProblemsCount = ct.ProblemsNumber,
                                        ProblemsEffortHrs = ct.ProblemsEffortHrs,
                                        ProblemsEffortPerUnit = ct.ProblemsEffortPerUnit,

                                        IncidentsCount = ct.IncidentNumber,
                                        IncidentEffortPerUnit = ct.IncidentEffortPerUnit,
                                        IncidentEffortHrs = ct.IncidentEffortHrs,

                                        UserSupportSRCount = ct.UssrNumber,
                                        UserSupportSREffortHrs = ct.UssrEffortHrs,
                                        UserSupportSREffortPerUnit = ct.UssrEffortPerUnit,

                                        EnhancementsCount = ct.EnhanceNumber,
                                        EnhacementEffortHrs = ct.EnhaceEffortHrs,
                                        EnhancementEffortPerUnit = ct.EnhanceEffortPerUnit,

                                        Comment = ct.Comment,

                                        IndustryVertical = verticals.VerticalName,
                                        SubVertical = subverticals.SubVerticalName,

                                        TotalContractValue = cls.TotalContractValue,
                                        NTTGlobalClientManager = cls.GlobalClientManager,
                                        NTTGlobalDeliveryManager = cls.GlobalDeliveryManager,
                                        //LastCustomerSurvey  = . ,
                                        //YearOfRelationship =  ,
                                        Rating = cls.CustomerRating,

                                    });

            if (searchCriteria.year > 0)
            {
                clientTicketInfo = clientTicketInfo.Where(w => w.Year == searchCriteria.year);
            }
            if (searchCriteria.quarter > 0)
            {
                clientTicketInfo = clientTicketInfo.Where(w => w.Quarter == searchCriteria.quarter);
            }
            if (searchCriteria.clientstatus > 1)
            {
                clientTicketInfo = clientTicketInfo.Where(w => w.ClientStatus == searchCriteria.clientstatus);
            }

            return clientTicketInfo.ToList();
        }

        public static IList<TicketInfoViewModel> GetTicketInfoDetail(int? ClientId)
        {


            var db = new Db();
            var ticketInfoDetailViewModels = (from tkt in db.TicketInfo
                                              join tktyp in db.TicketInfoType on tkt.TicketInfoTypeId equals tktyp.Id
                                              where (tkt.ClientId == ClientId)
                                              select new TicketInfoViewModel
                                              {
                                                  Id = tkt.Id,
                                                  TicketInfoType = tktyp.Description,
                                                  Incidents = tkt.Incidents,
                                                  ClientId = tkt.ClientId,
                                                  Comments = tkt.Comments,
                                                  TicketInfoTypeId = tkt.TicketInfoTypeId,
                                                  Enhancements = tkt.Enhancements,
                                                  FromDate = tkt.FromDate,
                                                  Todate = tkt.Todate,
                                                  Problems = tkt.Problems,
                                                  UserSupportSR = tkt.UserSupportSR
                                              }
                    ).OrderBy(o => o.FromDate).ToList();
            return ticketInfoDetailViewModels;

        }

        public static IList<JobDetailViewModel> GetJobDetail(int? ClientId)
        {
            //IEnumerable<JobDetail> jobDetail = null;
            IList<JobDetailViewModel> jobDetailViewModels = new List<JobDetailViewModel>();
            //var entityMapper = new Mapper<JobDetailViewModel, JobDetail>();
            var db = new Db();
            var jobdetail = (from jd in db.JobDetails
                             join jr in db.JobErrorLog  on jd.Id equals jr.JobId into tmperr
                             join app in db.AppStore on jd.ApplicationId equals app.Id
                             from jrr in tmperr.DefaultIfEmpty()
                             where jd.ClientId== ClientId
                             select new JobDetailViewModel
                             {  AddRowCount=jd.AddRowCount,
                                 ApplicationName=app.ApplicationName,
                                 ApplicationId=jd.ApplicationId,
                                 ClientId=jd.ClientId,
                                 ClientName="",
                                 Id=jd.Id,
                                 DuplicateRowCount=jd.DuplicateRowCount,
                                 Error= jrr.ErrorDesc,
                                  FileName=jd.FileName,
                                  IgnoredRecords=jd.IgnoredRecords,
                                  IsChecked=jd.IsChecked,
                                  JobEndTime= jd.JobEndTime,
                                  JobStartTime=jd.JobStartTime,
                                  MappingId=jd.MappingId,
                                  RejectedRecordsCount=jd.RejectedRecordsCount,
                                  RunStatus=jd.RunStatus,
                                  TotalRecordRecieved=jd.TotalRecordRecieved,
                                  UpdateRowCount=jd.UpdateRowCount,
                                  UserId=jd.UserId
                             }).Distinct().OrderByDescending(o=> o.JobStartTime).ToList();
            //using (var contextFactory = new DbContextFactory())
            //{
            //    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            //    if (ClientId != null && ClientId != 0)
            //        jobDetail = unitOfWork.JobDetails.Get(cid => cid.ClientId == ClientId).ToList();  //&& cid.IsChecked==false
            //    else
            //    {
            //        jobDetail = unitOfWork.JobDetails.Get(cid => cid.ClientId == 0 || cid.ClientId == null).ToList();  //cid=>cid.IsChecked == false
            //    }
            foreach(var a in jobdetail)
            {
                a.JobEndTime = a.JobEndTime.Value.Date;
                a.JobStartTime = a.JobStartTime.Date;
                jobDetailViewModels.Add(a);
            }

            //}
            //if (jobDetail != null)
            //{
            //    using (var contextFactory = new DbContextFactory())
            //    {
            //        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            //        foreach (var job in jobDetail)
            //        {
            //            var jobDetailViewModel = new JobDetailViewModel();
            //            entityMapper.MapToEntity(job, jobDetailViewModel);
            //            //  var client = unitOfWork.Clients.Get(field => field.Id == job.ClientId);
            //            //  jobDetailViewModel.ClientName = client.First().ClientName;
            //            if (job.ApplicationId > 0)
            //            {
            //                var application = unitOfWork.AppStoreApplications.Get(field => field.Id == job.ApplicationId);
            //                if (application.Count() > 0)
            //                {
            //                    jobDetailViewModel.ApplicationName = application.FirstOrDefault().ApplicationName;
            //                }
            //            }
            //            var jobErr = unitOfWork.JobErrorLog.Get(field => field.JobId == job.Id && !string.IsNullOrEmpty(field.ELocation));
            //            if (jobErr.Count() > 0)
            //            {
            //                jobDetailViewModel.Error = jobErr.FirstOrDefault().ErrorDesc;
            //            }
            //            jobDetailViewModels.Add(jobDetailViewModel);
            //        }
            //    }
            //}
            return jobDetailViewModels;
        }



        ////public static string GetClientName(int clientId)
        ////{
        ////    if (clientId <= 0)
        ////        return null;

        ////    using (var contextFactory = new DbContextFactory())
        ////    {
        ////        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

        ////        var client = unitOfWork.Clients.GetByID(clientId);
        ////        if (client != null)
        ////            return client.ClientName;
        ////    }


        ////    return null;
        ////}

        public static IList<SourceDataMappingViewModel> GetDataMappings(int mappingId)
        {
         //   IEnumerable<SourceDataMapping> sourceDataMappings = null;
         //   IList<SourceDataMappingViewModel> SourceDataMappingViewModels = new List<SourceDataMappingViewModel>();
           // var entityMapper = new Mapper<SourceDataMappingViewModel, SourceDataMapping>();

            //using (var contextFactory = new DbContextFactory())
            //{
            //    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var db = new Db();
            var sourceDataMappings = (from sm in db.SourceDataMapping
                                      join upl in db.UserProfiles on sm.CreatedBy equals upl.Id into createduser
                                      join upl2 in db.UserProfiles on sm.CreatedBy equals upl2.Id into updateduser
                                      where sm.MappingId == mappingId
                                      from cusr in createduser
                                      from uusr in updateduser
                                      select new SourceDataMappingViewModel
                                      {
                                          Id = sm.Id,
                                          MappingId = sm.MappingId,
                                          CreatedBy = sm.CreatedBy,
                                          CreatedByUserFName = cusr.FirstName,
                                          CreatedByUserLName = cusr.LastName,
                                          UpdatedBy = sm.UpdatedBy,
                                          UpdatedByUserFName = uusr.FirstName,
                                          UpdatedByUserLName = uusr.LastName,
                                          CreatedOn = sm.CreatedOn,
                                          DataMasking = sm.DataMasking,
                                          DataType = sm.DataType,
                                          DefVal = sm.DefVal,
                                          DisplayName = sm.DisplayName,
                                          DLength = sm.DLength,
                                          IsMandatory = sm.IsMandatory,
                                          MapColDesc = sm.MapColDesc,
                                          MappingCol = sm.MappingCol,
                                          MappingTable = sm.MappingTable,
                                          SField = sm.SField,
                                          UpdatedOn = sm.UpdatedOn
                                           
                                         
                                      });

            ////from profile in unitOfWork.UserProfile.Get()
            ////where userIds.Contains(profile.Id)
            ////select profile;
            //sourceDataMappings.
            // sourceDataMappings = unitOfWork.SourceDataMapping.Get(mapping => mapping.MappingId == mappingId);

            // }
            var returnSourceDataMappings = sourceDataMappings.ToList();
            if (returnSourceDataMappings == null || !returnSourceDataMappings.Any())
                return null;

            ////if (sourceDataMappings.Count() <= 0)
            ////{
            ////    return GetAppColumns("Incident", mappingId);
            ////}

            

                foreach (var mapping in returnSourceDataMappings)
                {
                    try
                    {
                    IEnumerable<SourceFieldConcat> sourceFieldConcats;
                    IEnumerable<SourceFieldCalculate> sourceFieldCalculate;
                    using (var contextFactory = new DbContextFactory())
                    {
                        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                         sourceFieldConcats = unitOfWork.SourceFieldConcat.Get(field => field.SourceDataMappingId == mapping.Id);
                         sourceFieldCalculate = unitOfWork.SourceFieldCalculate.Get(field => field.SourceDataMappingId == mapping.Id);
                    }
                        if (sourceFieldConcats != null && sourceFieldConcats.Count() > 0)
                        {
                            var concatFields = string.Empty;
                            var concatFields2 = string.Empty;
                            foreach (var field in sourceFieldConcats)
                            {
                                if (string.IsNullOrEmpty(concatFields))
                                {
                                    concatFields = field.SourceField;
                                    concatFields2 = field.SourceField + "~" + field.SeparatedBy;
                                }
                                else
                                {
                                    concatFields += ", " + field.SourceField;
                                    concatFields2 += "^" + field.SourceField + "~" + field.SeparatedBy;
                                }
                            }

                            mapping.FieldConcatinationsForDisplay = concatFields;
                            mapping.FieldConcatinations = concatFields2;
                        }

                        // - To fetch Mapping Calucate table entries
                        
                        if (sourceFieldCalculate != null && sourceFieldCalculate.Count() > 0)
                        {
                            var CalculateFields = string.Empty;
                           var CalculateFields2 = string.Empty;
                            foreach (var field in sourceFieldCalculate)
                            {
                                
                                string strconst = "";
                                try {
                                    if (field.Constant.HasValue)
                                        strconst = field.Constant.Value.ToString();
                                    else
                                        strconst = "0";
                                }
                                catch { strconst = "0"; }
                            if (string.IsNullOrEmpty(CalculateFields))
                            {
                                CalculateFields = field.Operator + " " + field.SourceField + " " + strconst;//  field.Constant == null? "0": field.Constant.ToString();
                                CalculateFields2 = field.Operator + "~" + field.SourceField + "~" + strconst;// field.Constant == null ? "0" : field.Constant.ToString();
                                    // if (field.SourceField == "_const_")
                                    //     CalculateFields2 += "~" + field.Constant;
                                }
                                else
                                {
                                CalculateFields = CalculateFields + " " + field.Operator + " " + field.SourceField + " " + strconst; // field.Constant==null ?  "0" : field.Constant.Value.ToString();
                                CalculateFields2 = CalculateFields2 + "^" + field.Operator + "~" + field.SourceField + "~" + strconst;// field.Constant == null ? "0" : field.Constant.Value.ToString();
                                    //if (field.SourceField == "_const_")
                                    //    CalculateFields2 += "~" + field.Constant;
                                }
                            }

                            mapping.FieldCalculationsForDisplay = CalculateFields;
                            mapping.FieldCalculations = CalculateFields2;
                        }

                        

                        // SourceDataMappingViewModels.Add(mapping);
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            

            return returnSourceDataMappings;

        }

        ////public static IList<SourceFieldConcat> GetSourceFieldConcats(int SourceDataMappingId)
        ////{
        ////    try
        ////    {
        ////        using (var contextFactory = new DbContextFactory())
        ////        {
        ////            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
        ////            return unitOfWork.SourceFieldConcat.Get(calc => calc.SourceDataMappingId == SourceDataMappingId).ToList();

        ////        }
        ////    }
        ////    catch (Exception)
        ////    {
        ////        return null;
        ////    }


        ////}

        public static MappingTemplateViewModel GetTemplateModelbyId(int mappingId)
        {
            var db = new Db();
            return (from t in db.MappingTemplate
                    join sa in db.StoreApplication on t.Id equals sa.MappingId

                    join saa in db.AppStore on sa.ApplicationId equals saa.Id
                    join u in db.UserProfiles on t.UpdatedBy equals u.Id into tmpuserUpdated
                    join uc in db.UserProfiles on t.CreatedBy equals uc.Id into tmpuserCreated
                    join ua in db.UserProfiles on t.AcknowledgeBy equals ua.Id into tmpuserAck
                    join c in db.Clients on t.ClientId equals c.Id into tmpClients
                    where t.Id == mappingId
                    from userUpdated in tmpuserUpdated.DefaultIfEmpty()
                    from userCreated in tmpuserCreated.DefaultIfEmpty()
                    from userAck in tmpuserAck.DefaultIfEmpty()
                    from client in tmpClients.DefaultIfEmpty()

                    select new MappingTemplateViewModel
                    {
                        ClientId = t.ClientId,
                        AcknowledgeByUserName = userAck.FirstName + " " + userAck.LastName,
                        AcknowledgeBy = t.AcknowledgeBy,
                        AcknowledgeOn = t.AcknowledgeOn,
                        AppStagingTable = saa.StageTable,
                        AppName = saa.ApplicationName,
                        BaseMappingId = t.BaseMappingId,
                        ClientName = client.ClientName,
                        CreatedBy = userCreated.Id,
                        CreatedByUserName = userCreated.FirstName + " " + userCreated.LastName,
                        CreatedOn = t.CreatedOn,
                        Description = t.TemplateName,
                        TemplateName = t.TemplateName,
                        FileDelimiter = t.FileDelimiter,
                        FileFormat = t.FileFormat,
                        GlobalMappingIdentifier = t.GlobalMappingIdentifier,
                        TemplateFile = t.TemplateFile,
                        IsStandard = t.IsStandard,
                        // MappingDone = t.AcknowledgeBy ?? false;
                        MappingId = t.Id,
                        StoreApplicationId = sa.ApplicationId,
                        UpdatedBy = userUpdated.Id,
                        UpdatedByUserName = userUpdated.FirstName + "" + userUpdated.LastName,
                        UpdatedOn = t.UpdatedOn,
                        // ArchiveRequired = t.ArchiveRequired

                    }).FirstOrDefault();

        }

        ////public static MappingTemplate GetTemplatebyId(int mappingId)
        ////{
        ////    using (var contextFactory = new DbContextFactory())
        ////    {
        ////        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
        ////        return unitOfWork.MappingTemplate.GetByID(mappingId);
        ////    }
        ////}

        public static bool UploadDataFile(FileUploadViewModel fileUploadViewModel)
        {
            bool result = false;
            if (string.IsNullOrEmpty(fileUploadViewModel.DataFileName))
                return false;

            if (fileUploadViewModel.httpPostedFileData != null)
            {

                string appDefaultJobName = "";
                string path = "";    //@"\\WN7X64-GFBFT12\shared\";
                //get the SSIS SQL agent job name for the Application
                bool validFileName = false;
                if (fileUploadViewModel.StoreApplicationId > 0)
                {
                    using (var contextFactory = new DbContextFactory())
                    {
                        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                        var app = unitOfWork.AppStoreApplications.GetByID(fileUploadViewModel.StoreApplicationId);
                        path = app.DataUploadSharedFolder;
                        //appDefaultJobName = app.DataUploadJob;
                        var clienttemplates = unitOfWork.MappingTemplate.Get(cl => cl.ClientId == fileUploadViewModel.ClientId);
                        if (clienttemplates.Count() > 0)
                        {
                            foreach (var template in clienttemplates)
                            {
                                if (fileUploadViewModel.DataFileName.ToLower().StartsWith(template.GlobalMappingIdentifier.ToLower()))
                                {
                                    var dt = GetScheduleJobs(template.GlobalMappingIdentifier, 0);
                                    if (dt.Count() > 0)
                                        appDefaultJobName = dt[0].Name;
                                    validFileName = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                if (validFileName)
                {
                    //if (!Directory.Exists(path))
                    //{
                    //    Directory.CreateDirectory(path);
                    //}

                    fileUploadViewModel.httpPostedFileData.SaveAs(path + Path.GetFileName(fileUploadViewModel.DataFileName));
                    // ViewBag.Message = "File uploaded successfully.";
                    if (fileUploadViewModel.ScheduleJob && !string.IsNullOrEmpty(appDefaultJobName))
                    {
                        using (var contextFactory = new DbContextFactory())
                        {
                            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                            var paramName = new string[] { "job_name" };
                            var paramValue = new object[] { appDefaultJobName };
                            var dt = unitOfWork.GetDataFromStoredProcedure("msdb.dbo.sp_start_job", paramName, paramValue);
                        }
                    }
                    result = true;
                }
                else
                {
                    result = false;
                    // throw new Exception("Please check the file name.");
                }

            }
            return result;
        }

        public static bool DeleteTicketInfo(int ticketInfoId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.TicketInfo.Delete(ticketInfoId);
                unitOfWork.Save();
                return true;
            }
        }
        public static bool AddTicketInfo(ticketInfoInput ticketInfoinput)
        {
            var entityMapper = new Mapper<TicketInfo, TicketInfoViewModel>();
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    foreach (var ticketInfo in ticketInfoinput.ticketInfoInputList)
                    {
                        if (!(ticketInfo.Incidents <= 0 && ticketInfo.Problems <= 0 && ticketInfo.UserSupportSR <= 0 && ticketInfo.Enhancements <= 0))
                        {
                            var dnTicketinfo = new TicketInfo();
                            entityMapper.MapToEntity(ticketInfo, dnTicketinfo);
                            dnTicketinfo.FromDate = ticketInfoinput.fromDt;
                            dnTicketinfo.Todate = ticketInfoinput.toDt;
                            // dnTicketinfo.CreatedOn = DateTime.Now;
                            // dnTicketinfo.UpdatedOn = DateTime.Now;
                            unitOfWork.TicketInfo.Insert(dnTicketinfo);
                        }
                    }
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }


        //public static bool AddNewMappingTemplate(MappingTemplateViewModel mappingTemplateViewModel)
        public static bool AddNewMappingTemplate(MappingTemplateViewModel mappingTemplateViewModel)
        {
            Log.Info("Hit to helper method");
            var entityMapper = new Mapper<MappingTemplate, MappingTemplateViewModel>();
            try
            {
                var mappingTemplate = new MappingTemplate();
                mappingTemplate.Id = GetNextMappingId();
                if (mappingTemplate.Id <= 0)
                    return false;
                Log.Info("MappingTemplateViewModel " + mappingTemplateViewModel);
                entityMapper.MapToEntity(mappingTemplateViewModel, mappingTemplate);
                Log.Debug("MappingTemplateViewModel "+mappingTemplateViewModel);
                mappingTemplate.CreatedOn = DateTime.Now;
                mappingTemplate.UpdatedOn = DateTime.Now;
                if (mappingTemplateViewModel.httpPostedTemplateFile != null)
                {
                    Stream filestream = mappingTemplateViewModel.httpPostedTemplateFile.InputStream;
                    BinaryReader br = new BinaryReader(filestream);
                    Byte[] bytes = br.ReadBytes((Int32)filestream.Length);
                    mappingTemplate.TemplateFile = bytes;
                }

                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    unitOfWork.MappingTemplate.Insert(mappingTemplate);

                    if (mappingTemplateViewModel.StoreApplicationId > 0)
                    {
                        var storeApp = new StoreApplication() { ApplicationId = mappingTemplateViewModel.StoreApplicationId, MappingId = mappingTemplate.Id };
                        unitOfWork.StoreApplication.Insert(storeApp);
                    }

                    unitOfWork.Save();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        ////public static IList<StoreApplication> GetStoreApplications(int mappingId)
        ////{
        ////    using (var contextFactory = new DbContextFactory())
        ////    {
        ////        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
        ////        return unitOfWork.StoreApplication.Get(mid => mid.MappingId == mappingId).ToList();
        ////    }
        ////}

        public static void GetCSVAppStoreApplicationsName(IList<StoreApplication> storeApps, out string appName, out string appStgTable)
        {
            if (storeApps == null || storeApps.Count <= 0)
            {
                appName = "";
                appStgTable = "";
            }

            var csvAppNames = new StringBuilder();
            var csvAppStgTable = new StringBuilder();

            IEnumerable<AppStoreApplications> applicationsList;
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                applicationsList = unitOfWork.AppStoreApplications.Get();
            }

            foreach (var storeApp in storeApps)
            {
                var application = applicationsList.FirstOrDefault(app => app.Id == storeApp.ApplicationId);
                if (application != null)
                {
                    if (csvAppNames.Length <= 0)
                    {
                        csvAppNames.Append(application.ApplicationName.Trim());
                        csvAppStgTable.Append(application.StageTable);
                    }
                    else
                    {
                        csvAppNames.Append(", " + application.ApplicationName.Trim());
                        csvAppStgTable.Append(", " + application.StageTable);
                    }
                }
            }
            appName = csvAppNames.ToString();
            appStgTable = csvAppStgTable.ToString();
            //return csvAppNames.ToString();
        }

        private static string GetFullName(int userId, IEnumerable<UserProfile> userProfiles)
        {
            if (userId <= 0 || userProfiles == null || userProfiles.Count() <= 0)
                return string.Empty;

            string fullname = string.Empty;

            var user = userProfiles.FirstOrDefault(profile => profile.Id == userId);

            if (user == null)
                return string.Empty;

            if (!string.IsNullOrEmpty(user.FirstName))
                fullname = user.FirstName;
            if (!string.IsNullOrEmpty(user.LastName))
                fullname += " " + user.LastName;
            return fullname;
        }

        public static Dictionary<int, string> GetFullNames(List<int> userIds)
        {
            if (userIds == null || !userIds.Any())
                return null;
            try
            {
                var userNameInfo = new Dictionary<int, string>();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var fileteredUserProfiles = from profile in unitOfWork.UserProfile.Get()
                                                where userIds.Contains(profile.Id)
                                                select profile;

                    if (fileteredUserProfiles == null || fileteredUserProfiles.Count() <= 0)
                        return userNameInfo;

                    foreach (var profile in fileteredUserProfiles)
                    {
                        userNameInfo.Add(profile.Id, GetFullName(profile.Id, fileteredUserProfiles));
                    }
                    return userNameInfo;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static void RestoreCreatedByOnInformation(IList<SourceDataMappingViewModel> sourceDataMappingViewModels)
        {
            if (sourceDataMappingViewModels == null || sourceDataMappingViewModels.Count() <= 0)
                return;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    foreach (var sourceDataMappingViewModel in sourceDataMappingViewModels)
                    {
                        if (sourceDataMappingViewModel.Id > 0)
                        {
                            var oldData = unitOfWork.SourceDataMapping.GetByID(sourceDataMappingViewModel.Id);
                            sourceDataMappingViewModel.CreatedBy = oldData.CreatedBy;
                            sourceDataMappingViewModel.CreatedOn = oldData.CreatedOn;
                        }
                    }
                }
            }
            catch (Exception) { }
        }
        public static bool AddUpdateDataMappings(IList<SourceDataMappingViewModel> sourceDataMappingViewModels, bool acknowledge)
        {
            var entityMapper = new Mapper<SourceDataMapping, SourceDataMappingViewModel>();
            if (sourceDataMappingViewModels == null || sourceDataMappingViewModels.Count <= 0)
                return false;
            var currentDate = DateTime.Now; // .Date;
            try
            {
                // RestoreCreatedByOnInformation(sourceDataMappingViewModels);
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    foreach (var sourceDataMappingViewModel in sourceDataMappingViewModels)
                    {
                        SourceDataMapping sourceDataMapping = new SourceDataMapping();
                        entityMapper.MapToEntity(sourceDataMappingViewModel, sourceDataMapping);
                        //sourceDataMapping.MappingTable = "Incident_stg";
                        //sourceDataMapping.MappingTable=
                        if (sourceDataMapping.Id > 0)
                        {
                            sourceDataMapping.UpdatedOn = currentDate;
                            unitOfWork.SourceDataMapping.Update(sourceDataMapping);


                            //delete all concatenations 
                            var concatenations = unitOfWork.SourceFieldConcat.Get(s => s.SourceDataMappingId == sourceDataMapping.Id);
                            foreach (var c in concatenations)
                            {
                                unitOfWork.SourceFieldConcat.Delete(c.Id);
                            }
                            //add concatenations
                            foreach (var field in sourceDataMappingViewModel.SourceFieldConcats)
                            {

                                var concatField = new SourceFieldConcat
                                {
                                    SourceDataMappingId = sourceDataMapping.Id,
                                    MappingId = sourceDataMappingViewModel.MappingId,
                                    SourceField = field.SourceField,
                                    SeparatedBy = field.SeparatedBy,
                                    CreatedBy = field.CreatedBy,
                                    CreatedOn = currentDate,
                                    // = currentDate
                                };
                                unitOfWork.SourceFieldConcat.Insert(concatField);
                            }

                            //delete all Calculations 
                            var calculations = unitOfWork.SourceFieldCalculate.Get(s => s.SourceDataMappingId == sourceDataMapping.Id);
                            foreach (var c in calculations)
                            {
                                unitOfWork.SourceFieldCalculate.Delete(c.Id);
                            }
                            //add concatenations
                            foreach (var field in sourceDataMappingViewModel.SourceFieldCalculate)
                            {

                                var concatField = new SourceFieldCalculate
                                {
                                    SourceDataMappingId = sourceDataMapping.Id,
                                    SourceField = field.SourceField,
                                    Operator = field.Operator,
                                    Constant = field.Constant,
                                    CreatedBy = field.CreatedBy,
                                    CreatedDt = currentDate,
                                    UpdatedDt = currentDate,
                                    UpdatedBy = field.CreatedBy
                                    // = currentDate
                                };
                                unitOfWork.SourceFieldCalculate.Insert(concatField);
                            }


                        }
                        else
                        {
                            sourceDataMapping.UpdatedOn = currentDate;
                            sourceDataMapping.CreatedOn = currentDate;
                            sourceDataMapping.Id = GetNextSourceDataMappingId();
                            if (sourceDataMapping.Id > 0)
                            {
                                unitOfWork.SourceDataMapping.Insert(sourceDataMapping);
                                if (sourceDataMappingViewModel.SourceFieldConcats != null && sourceDataMappingViewModel.SourceFieldConcats.Count > 0)
                                {
                                    foreach (var field in sourceDataMappingViewModel.SourceFieldConcats)
                                    {
                                        var concatField = new SourceFieldConcat
                                        {
                                            SourceDataMappingId = sourceDataMapping.Id,
                                            MappingId = sourceDataMappingViewModel.MappingId,
                                            SourceField = field.SourceField,
                                            SeparatedBy = field.SeparatedBy,
                                            CreatedBy = field.CreatedBy,
                                            CreatedOn = currentDate,
                                            // = currentDate
                                        };

                                        unitOfWork.SourceFieldConcat.Insert(concatField);
                                    }
                                }

                                if (sourceDataMappingViewModel.SourceFieldCalculate != null && sourceDataMappingViewModel.SourceFieldCalculate.Count > 0)
                                {
                                    foreach (var field in sourceDataMappingViewModel.SourceFieldCalculate)
                                    {
                                        var concatField = new SourceFieldCalculate
                                        {
                                            SourceDataMappingId = sourceDataMapping.Id,
                                            SourceField = field.SourceField,
                                            Operator = field.Operator,
                                            Constant = field.Constant,
                                            CreatedBy = field.CreatedBy,
                                            CreatedDt = currentDate,
                                            UpdatedBy = field.CreatedBy,
                                            UpdatedDt = currentDate
                                            // = currentDate
                                        };

                                        unitOfWork.SourceFieldCalculate.Insert(concatField);
                                    }
                                }

                                                                                                                          

                            }
                        }
                    }
                    var mappingTemplate = unitOfWork.MappingTemplate.GetByID(sourceDataMappingViewModels[0].MappingId);
                    if (mappingTemplate != null)
                        mappingTemplate.MappingDone = true;
                    if (acknowledge)
                    {
                        mappingTemplate.AcknowledgeBy = sourceDataMappingViewModels[0].UpdatedBy;
                        //
                        mappingTemplate.AcknowledgeOn = currentDate;
                    }
                    else
                    {
                        mappingTemplate.AcknowledgeBy = null; //sourceDataMappingViewModels[0].UpdatedBy;
                        mappingTemplate.AcknowledgeOn = null; // currentDate;
                    }

                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool AddUpdateSourceFieldConcats(IList<SourceFieldConcatModel> sourceFieldConcats)
        {
            //var entityMapper = new Mapper<SourceDataMapping, SourceDataMappingViewModel>();
            if (sourceFieldConcats == null || sourceFieldConcats.Count <= 0)
                return false;


            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                    foreach (var sourceFieldConcat in sourceFieldConcats)
                    {
                        SourceFieldConcat srcconcat = new SourceFieldConcat();
                       
                        srcconcat.MappingId = sourceFieldConcat.MappingId;
                        srcconcat.SeparatedBy = sourceFieldConcat.SeparatedBy;
                        srcconcat.SourceDataMappingId = sourceFieldConcat.SourceDataMappingId;
                        srcconcat.SourceField = sourceFieldConcat.SourceField;
                        srcconcat.CreatedBy = sourceFieldConcat.CreatedBy;
                        srcconcat.CreatedOn = sourceFieldConcat.CreatedOn;
                        if (sourceFieldConcat.Id > 0)
                        {
                            srcconcat.Id = sourceFieldConcat.Id;
                            unitOfWork.SourceFieldConcat.Update(srcconcat);
                        }
                        else
                        {
                            unitOfWork.SourceFieldConcat.Insert(srcconcat);
                        }
                    }
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        private static int GetNextMappingId()
        {
            int id = -1;
            var paramName = new string[] { "SequenceName" };
            var paramValue = new object[] { "MappingIDSequence" };

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                var res = unitOfWork.GetDataFromStoredProcedure("GetNextSequenceValue", paramName, paramValue);
                var response = res.Rows[0][0];
                int.TryParse(response.ToString(), out id);
            }
            return id;
        }

        private static int GetNextSourceDataMappingId()
        {
            int id = -1;
            var paramName = new string[] { "SequenceName" };
            var paramValue = new object[] { "SourceDataMappingIDSequence" };

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                var res = unitOfWork.GetDataFromStoredProcedure("GetNextSequenceValue", paramName, paramValue);
                var response = res.Rows[0][0];
                int.TryParse(response.ToString(), out id);
            }
            return id;
        }

        public static DataTable GetRejectedRecords(int jobid)
        {
            IList<SourceDataMappingViewModel> AppColumnDetail = new List<SourceDataMappingViewModel>();
            var paramName = new string[] { "Jobid" };
            var paramValue = new object[] { jobid };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                dt = unitOfWork.GetDataFromStoredProcedure("GetRejectedRecords", paramName, paramValue);
            }
            return dt;
        }


        public static IList<SourceDataMappingViewModel> GetAppColumns(string TableName, int mappingId)
        {
            IList<SourceDataMappingViewModel> AppColumnDetail = new List<SourceDataMappingViewModel>();
            var paramName = new string[] { "tableName" };
            var paramValue = new object[] { TableName };

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                //var res = unitOfWork.GetDataFromStoredProcedure("GetTableColumnNames", paramName, paramValue);
                var res = unitOfWork.GetDataFromStoredProcedure("GetTableColumnNameswithFilterForTower", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new SourceDataMappingViewModel();
                    coldet.DataType = dr["DataType"].ToString();
                    if (dr["DataType"].ToString() == "int")
                        coldet.DefVal = "0";
                    coldet.MappingCol = dr["AppField"].ToString();
                    coldet.IsMandatory = !Convert.ToBoolean(dr["isNullable"]);
                    coldet.DisplayName = dr["AppField"].ToString();
                    coldet.MappingTable = TableName;
                    coldet.MappingId = mappingId;
                    coldet.MapColDesc = dr["Description"].ToString();
                    try
                    {
                        coldet.DLength = Convert.ToInt32(dr["Size"]);
                    }

                    catch { }
                    AppColumnDetail.Add(coldet);
                }
            }
            return AppColumnDetail.ToList() ;
        }

        public static IList<ScheduleJobsViewModel> CreateScheduleJobs(scheduleJobInput jobInput, out string ValidationErr)
        {
            IList<ScheduleJobsViewModel> jobs = new List<ScheduleJobsViewModel>();
            string scheduleon = "";
            if (jobInput.ScheduleType == "MONTHLY")
                scheduleon = jobInput.MonthDay;
            else if (jobInput.ScheduleType == "WEEKLY")
                scheduleon = jobInput.WeekDay;
            ValidationErr = "Success";
            int templatecounts = 0;
            string SSISPackagPath = "";
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var app = unitOfWork.AppStoreApplications.GetByID(jobInput.ApplicationId);
                //path = app.DataUploadSharedFolder;
                SSISPackagPath = app.SSISPackageFolder;
                string clientApp= jobInput._templateIdentifier;
                 //string clientApp = jobInput.ClientName.Replace(" ", "") + "_" + jobInput.ApplicationId.ToString();
                 templatecounts = unitOfWork.MappingTemplate.Get(cl => cl.ClientId == jobInput.ClientId && cl.AcknowledgeBy != null && cl.GlobalMappingIdentifier.StartsWith(clientApp)).Count();

            }
            jobs = GetScheduleJobs(jobInput._templateIdentifier, jobInput.ApplicationId);
            if (jobs.Where(s => s.FrequencyType.ToLower() == jobInput.ScheduleType.ToLower()).Count() > 0)
            {
                ValidationErr = "There is already a job exist for the given schedule detail.";
            }
            else
            {
                using (var contextFactory = new DbContextFactory())
                {
                    if (templatecounts > 0)
                    {
                        string str = jobInput.time.ToString("HHmmss");
                        int timepart = Convert.ToInt32(str);
                        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                        var paramName = new string[] { "SSISPackagePath", "ClientName", "AppId", "ScheduleType", "scheduleon", "ScheduleTime" };
                        var paramValue = new object[] { SSISPackagPath,jobInput._templateIdentifier, jobInput.ApplicationId.ToString(), jobInput.ScheduleType, scheduleon, timepart };
                        var res = unitOfWork.GetDataFromStoredProcedure("SSIS_CreateSQLJobsForClients", paramName, paramValue);
                    }
                    else
                    {
                        ValidationErr = "There is no matching and acknowledged data mapping definition.";
                    }
                }
                jobs = GetScheduleJobs(jobInput._templateIdentifier, jobInput.ApplicationId);
            }
            //
            return jobs;
        }

        public static IList<ScheduleJobsViewModel> GetScheduleJobs(string client, int appid)
        {
            IList<ScheduleJobsViewModel> jobs = new List<ScheduleJobsViewModel>();
            var paramName = new string[] { "clientname", "AppId" };
            var paramValue = new object[] { client, appid };

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                var res = unitOfWork.GetDataFromStoredProcedure("SSIS_GetSQLJobs", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new ScheduleJobsViewModel();
                    coldet.Name = dr["job_name"].ToString();

                    coldet.FrequencyType = dr["frequency"].ToString();
                    coldet.JobEnabled = dr["job_enabled"].ToString();
                    //coldet = dr["scheduleName"].ToString();

                    coldet.NextRrunDate = dr["Next_Run_Date"].ToString();
                    coldet.Description = dr["Job_Description"].ToString();

                    jobs.Add(coldet);
                }
            }
            return jobs;
        }

        public static IList<ScheduleJobsViewModel> DeleteJob(string TemplateIdentifier, int appid, string jobname)
        {
            IList<ScheduleJobsViewModel> jobs = new List<ScheduleJobsViewModel>();


            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                var paramNameset1 = new string[] { "job_name", "delete_unused_schedule" };
                var paramValueset1 = new object[] { jobname, 1 };
                unitOfWork.ExecProcedure("msdb.dbo.sp_delete_job", paramNameset1, paramValueset1);

                var paramName = new string[] { "clientname", "AppId" };
                var paramValue = new object[] { TemplateIdentifier, appid };
                var res = unitOfWork.GetDataFromStoredProcedure("SSIS_GetSQLJobs", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new ScheduleJobsViewModel();
                    coldet.Name = dr["job_name"].ToString();

                    coldet.FrequencyType = dr["frequency"].ToString();
                    coldet.JobEnabled = dr["job_enabled"].ToString();
                    //coldet = dr["scheduleName"].ToString();

                    coldet.NextRrunDate = dr["Next_Run_Date"].ToString();
                    coldet.Description = dr["Job_Description"].ToString();

                    jobs.Add(coldet);
                }
            }
            return jobs;
        }


        //   EXEC  @job_name= N'SSIS_INSLA_DefaultJob', @delete_unused_schedule= 1

        public static IList<JobDetailViewModel> GetJobDetailByDate(DateTime? StartDate, DateTime? EndDate, int? ClientId)
        {
            IEnumerable<JobDetail> jobDetail = null;
            IList<JobDetailViewModel> jobDetailViewModels = new List<JobDetailViewModel>();
            var entityMapper = new Mapper<JobDetailViewModel, JobDetail>();

            DateTime stdate = DateTime.MinValue;
            DateTime enddate = DateTime.MaxValue;
            if (StartDate != null)
                stdate = (DateTime)StartDate;
            if (EndDate != null)
                enddate = (DateTime)EndDate;
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                if (ClientId != null && ClientId != 0)
                    jobDetail = unitOfWork.JobDetails.Get(cid => cid.ClientId == ClientId && cid.JobStartTime >= stdate && (cid.JobEndTime <= enddate || cid.JobEndTime == null)).ToList();  //&& cid.IsChecked==false
                else
                {
                    jobDetail = unitOfWork.JobDetails.Get(cid => (cid.ClientId == 0 || cid.ClientId == null) && cid.JobStartTime >= stdate && (cid.JobEndTime <= enddate || cid.JobEndTime == null)).ToList();  //cid=>cid.IsChecked == false
                }

            }
            if (jobDetail != null)
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    foreach (var job in jobDetail)
                    {
                        var jobDetailViewModel = new JobDetailViewModel();
                        entityMapper.MapToEntity(job, jobDetailViewModel);

                        if (job.ApplicationId > 0)
                        {
                            var application = unitOfWork.AppStoreApplications.Get(field => field.Id == job.ApplicationId);
                            if (application.Count() > 0)
                            {
                                jobDetailViewModel.ApplicationName = application.FirstOrDefault().ApplicationName;
                            }
                        }
                        var jobErr = unitOfWork.JobErrorLog.Get(field => field.JobId == job.Id && !string.IsNullOrEmpty(field.ELocation));
                        if (jobErr.Count() > 0)
                        {
                            jobDetailViewModel.Error = jobErr.FirstOrDefault().ErrorDesc;
                        }
                        jobDetailViewModels.Add(jobDetailViewModel);
                    }
                }
            }
            return jobDetailViewModels;
        }






        public static ClientDetailsModel GetClientDetails(int clientId)
        {
            ClientDetailsModel Cd = new ClientDetailsModel();

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var Cdetails = unitOfWork.Clients.GetByID(clientId);
                // var ticInfo = unitOfWork.TicketInfo.Get().Where(wh => wh.ClientId == clientId).ToList();

                Cd.ClientName = Cdetails.ClientName;
                Cd.ClientStatus = Cdetails.ClientStatus;
                Cd.TotalContractVal = Cdetails.TotalContractValueToDate;
                Cd.VerticalId = Cdetails.VerticalId;
                Cd.WorkStartTime = Cdetails.DateCreated;
                Cd.WorkEndTime = Cdetails.DateCreated;
                //Cd.ResolutionTime = Cdetails.
                Cd.NttClientManager = Cdetails.GlobalClientManager;
                Cd.NttDeliveryManager = Cdetails.GlobalDeliveryManager;
                Cd.ClientStatus = Cdetails.ClientStatus;
                Cd.CustSurveyDate = Cdetails.CustomerSurveyDate;
                Cd.CustRating = Cdetails.CustomerRating;
                Cd.OnshoreFTE = Cdetails.AvgAnnualFTEsOnShore;
                Cd.OffshoreFTE = Cdetails.AvgAnnualFTEsOffShore;
                Cd.NearShoreFTE = Cdetails.AvgAnnualFTEsNearShore;
                Cd.ClientTicketInfoList = GetTicketInfoDetail(clientId);/*.ToPagedList(page ?? 1, 16)*/
                                                                        //if (fileteredUserProfiles == null || fileteredUserProfiles.Count() <= 0)
                                                                        //    return userNameInfo;

                //foreach (var profile in fileteredUserProfiles)
                //{
                //    userNameInfo.Add(profile.Id, GetFullName(profile.Id, fileteredUserProfiles));
                //}
                //return userNameInfo;


            }
            return Cd;
        }


        public static IList<DashboardViewModel> CPMDashboard(string ClientIds, int quarter1, int year1, int quarter2, int year2)
        {
            var clientList = (HttpUtility.UrlDecode(ClientIds)).Split(',');
            IList<DashboardViewModel> Clusterlist = new List<DashboardViewModel>();
            DataSet ds = new DataSet();
            foreach (var ClientId in clientList)
            {
                var paramName = new string[] { "ClientId", "Quater1", "year1", "Quater2", "year2" };
                var paramValue = new object[] { ClientId, quarter1, year1, quarter2, year2 };

                DataTable dt = new DataTable();

                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var res = unitOfWork.GetDataFromStoredProcedure("CPMDashboard", paramName, paramValue);


                    foreach (DataRow dr in res.Rows)
                    {
                        var coldet = new DashboardViewModel();

                        coldet.ClientName = dr["ClientName"].ToString();
                        coldet.IncidentPercentage = dr["Incidents"].ToString();
                        coldet.SRPercentage = dr["ServiceRequest"].ToString();
                        coldet.ProblemVsIncident = dr["ProblemsVsIncidents"].ToString();
                        coldet.FtePercentage = dr["FTEs"].ToString();
                        coldet.MTTRPercentage = dr["MTTR"].ToString();
                        //    coldet.ColumnTitle = dr[0].ToString();
                        //    coldet.RowTitle = dr[1].ToString();


                        //    //coldet.imagePerCluster = dr["imagePerCluster"].ToString();
                        try
                        {

                            coldet.EffortPerIncident = Convert.ToDecimal(dr["EffortPerIncident"]);
                            coldet.TicketsPerFTE = Convert.ToDecimal(dr["TicketsPerFTE"]);
                            //coldet.Resolutiontime = Convert.ToInt16(dr["Resolutiontime"]);
                            //coldet.PrimaryKeyIncidentId = Convert.ToInt32(dr["PrimaryKeyIncidentId"]);
                        }

                        catch { }
                        //    ds.Tables.Add(transposedTable);
                        Clusterlist.Add(coldet);
                    }



                }



            }
            return Clusterlist;
        }


        public static IList<PerformanceComplianceModel> CPMPerformanceComplianceReport(int uid, int year, int Quarter)
        {
            IList<PerformanceComplianceModel> Modellist = new List<PerformanceComplianceModel>();
            var paramName = new string[] { "uid", "year", "Quarter" };
            var paramValue = new object[] { uid, year, Quarter };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("CPMPerformanceDataComplianceReport", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new PerformanceComplianceModel();


                    coldet.ClientName = dr["clientname"].ToString();



                    try
                    {

                        coldet.NoOfIncidents = Convert.ToDecimal(dr["NoofIncidents"]);
                        coldet.NoOfUserSuport = Convert.ToDecimal(dr["NoofUserSupportSR"]);
                        coldet.NoOfEnhancement = Convert.ToDecimal(dr["NoEnhancements"]);
                        coldet.NoOfProblem = Convert.ToDecimal(dr["NoofProblem"]);
                        coldet.NoOfSRBacklog = Convert.ToDecimal(dr["NoofSRBacklog"]);
                        coldet.EffortHrIncidents = Convert.ToDecimal(dr["EffortHrsonIncident"]);
                        coldet.EffortHrRequest = Convert.ToDecimal(dr["EffortHrsonRequest"]);
                        coldet.EffortHrEnhancement = Convert.ToDecimal(dr["EffortHrsonEnhancements"]);
                        coldet.EffortPerIncident = Convert.ToDecimal(dr["EffortPerIncident"]);
                        coldet.EffortPerRequest = Convert.ToDecimal(dr["Effortperrequest"]);
                    }

                    catch { }
                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }


        //// get ticket info list-Helper
        //public static IList<ClientTicketInfoViewModel> ClientProfileAnalysisDashboard()
        //{


        //    var db = new Db();
        //    var tinfo = (from tkt in db.ClientTicketInfo
        //                 join c in db.Clients on tkt.ClientId equals c.Id



        //                 select new
        //                 {
        //                     Id = tkt.Id,
        //                     ClientId = tkt.ClientId,
        //                     ClientName = c.ClientName,
        //                     //FromYear = tkt.FromYear,
        //                     //FromQuarter = tkt.FromQuarter,
        //                     Year = tkt.Year, //new DateTime(tkt.Year,1,1),  
        //                     Quarter = tkt.Quarter,
        //                     IncidentNumber = tkt.IncidentNumber,
        //                     IncidentEffortHrs = tkt.IncidentEffortHrs,
        //                     IncidentEffortPerUnit = tkt.IncidentEffortPerUnit,
        //                     UssrNumber = tkt.UssrNumber,
        //                     UssrEffortHrs = tkt.UssrEffortHrs,
        //                     UssrEffortPerUnit = tkt.UssrEffortPerUnit,
        //                     EnhanceNumber = tkt.EnhanceNumber,
        //                     EnhaceEffortHrs = tkt.EnhaceEffortHrs,
        //                     EnhanceEffortPerUnit = tkt.EnhanceEffortPerUnit,
        //                     ProblemsNumber = tkt.ProblemsNumber,
        //                     ProblemsEffortHrs = tkt.ProblemsEffortHrs,
        //                     ProblemsEffortPerUnit = tkt.ProblemsEffortPerUnit,
        //                     SRBacklog = tkt.SRBacklog,
        //                     InMTTRSev1 = tkt.InMTTRSev1,
        //                     InMTTRSev2 = tkt.InMTTRSev2,
        //                     InMTTRSev3 = tkt.InMTTRSev3,
        //                     InMTTRSev4 = tkt.InMTTRSev4,
        //                     SrMTTRSev1 = tkt.SrMTTRSev1,
        //                     SrMTTRSev2 = tkt.SrMTTRSev2,
        //                     SrMTTRSev3 = tkt.SrMTTRSev3,
        //                     SrMTTRSev4 = tkt.SrMTTRSev4,
        //                     NttFTENearShore = tkt.NttFTENearShore,
        //                     NttFTEOffshore = tkt.NttFTEOffshore,
        //                     NttFTEOnsite = tkt.NttFTEOnsite,
        //                     NoOfApplications = tkt.NoOfApplications,
        //                     Comment = tkt.Comment
        //                 });

        //    IList<ClientTicketInfoViewModel> clientticketInfoDetailViewModels = tinfo.Select(tkt => new ClientTicketInfoViewModel
        //    {
        //        Id = tkt.Id,
        //        ClientId = tkt.ClientId,
        //        ClientName = tkt.ClientName,
        //        //FromYear = tkt.FromYear,
        //        //FromQuarter = tkt.FromQuarter,
        //        Year = tkt.Year, //new DateTime(tkt.Year,1,1),  
        //        Quarter = tkt.Quarter,
        //        IncidentNumber = tkt.IncidentNumber,
        //        IncidentEffortHrs = tkt.IncidentEffortHrs,
        //        IncidentEffortPerUnit = tkt.IncidentEffortPerUnit,
        //        UssrNumber = tkt.UssrNumber,
        //        UssrEffortHrs = tkt.UssrEffortHrs,
        //        UssrEffortPerUnit = tkt.UssrEffortPerUnit,
        //        EnhanceNumber = tkt.EnhanceNumber,
        //        EnhaceEffortHrs = tkt.EnhaceEffortHrs,
        //        EnhanceEffortPerUnit = tkt.EnhanceEffortPerUnit,
        //        ProblemsNumber = tkt.ProblemsNumber,
        //        ProblemsEffortHrs = tkt.ProblemsEffortHrs,
        //        ProblemsEffortPerUnit = tkt.ProblemsEffortPerUnit,
        //        SRBacklog = tkt.SRBacklog,
        //        InMTTRSev1 = tkt.InMTTRSev1,
        //        InMTTRSev2 = tkt.InMTTRSev2,
        //        InMTTRSev3 = tkt.InMTTRSev3,
        //        InMTTRSev4 = tkt.InMTTRSev4,
        //        SrMTTRSev1 = tkt.SrMTTRSev1,
        //        SrMTTRSev2 = tkt.SrMTTRSev2,
        //        SrMTTRSev3 = tkt.SrMTTRSev3,
        //        SrMTTRSev4 = tkt.SrMTTRSev4,
        //        NttFTENearShore = tkt.NttFTENearShore,
        //        NttFTEOffshore = tkt.NttFTEOffshore,
        //        NttFTEOnsite = tkt.NttFTEOnsite,
        //        NoOfApplications = tkt.NoOfApplications,
        //        Comment = tkt.Comment


        //    }
        //            ).ToList();
        //    return clientticketInfoDetailViewModels;

        //}


        public static IList<ClientTicketInfoViewModel> CPMAnalysisReport(int uid)
        {
            IList<ClientTicketInfoViewModel> Modellist = new List<ClientTicketInfoViewModel>();
            var paramName = new string[] { "uid" };
            var paramValue = new object[] { uid };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("CPMAnalysisReport", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {
                    var coldet = new ClientTicketInfoViewModel();
                    coldet.ClientName = dr["clientname"].ToString();
                    try
                    {
                        coldet.Year = Convert.ToInt16(dr["Year"]);
                        coldet.Quarter = Convert.ToInt16(dr["Quarter"]);//new DateTime(tkt.Year,1,1),  
                                                                        //        Quarter = tkt.Quarter,
                        coldet.IncidentNumber = Convert.ToInt16(dr["IncidentNumber"]);
                        coldet.UssrNumber = Convert.ToDecimal(dr["UssrNumber"]);
                        coldet.EnhanceNumber = Convert.ToDecimal(dr["EnhanceNumber"]);
                        coldet.ProblemsNumber = Convert.ToDecimal(dr["ProblemsNumber"]);
                        coldet.SRBacklog = Convert.ToDecimal(dr["SRBacklog"]);
                        coldet.UssrEffortHrs = Convert.ToDecimal(dr["UssrEffortHrs"]);
                        coldet.UssrEffortPerUnit = Convert.ToDecimal(dr["UssrEffortPerUnit"]);
                        coldet.EnhaceEffortHrs = Convert.ToDecimal(dr["EnhaceEffortHrs"]);
                        coldet.EnhanceEffortPerUnit = Convert.ToDecimal(dr["EnhanceEffortPerUnit"]);
                        coldet.IncidentEffortHrs = Convert.ToDecimal(dr["IncidentEffortHrs"]);
                        coldet.ProblemsEffortHrs = Convert.ToDecimal(dr["ProblemsEffortHrs"]);
                        coldet.ProblemsEffortPerUnit = Convert.ToDecimal(dr["ProblemsEffortPerUnit"]);
                        coldet.IncidentEffortPerUnit = Convert.ToDecimal(dr["IncidentEffortPerUnit"]);
                        coldet.InMTTRSev1 = Convert.ToDecimal(dr["InMTTRSev1"]);
                        coldet.InMTTRSev2 = Convert.ToDecimal(dr["InMTTRSev2"]);
                        coldet.InMTTRSev3 = Convert.ToDecimal(dr["InMTTRSev3"]);
                        coldet.InMTTRSev4 = Convert.ToDecimal(dr["InMTTRSev4"]);
                        coldet.SrMTTRSev1 = Convert.ToDecimal(dr["SrMTTRSev1"]);
                        coldet.SrMTTRSev2 = Convert.ToDecimal(dr["SrMTTRSev2"]);
                        coldet.SrMTTRSev3 = Convert.ToDecimal(dr["SrMTTRSev3"]);
                        coldet.SrMTTRSev4 = Convert.ToDecimal(dr["SrMTTRSev4"]);
                        coldet.NttFTEOnsite = Convert.ToDecimal(dr["NttFTEOnsite"]);
                        coldet.NttFTEOffshore = Convert.ToDecimal(dr["NttFTEOffshore"]);
                        coldet.NttFTENearShore = Convert.ToDecimal(dr["NttFTENearShore"]);
                        coldet.NoOfApplications = Convert.ToInt32(dr["NoOfApplications"]);



                    }

                    catch { }
                    Modellist.Add(coldet);
                }

            }
            return Modellist;
        }

        //For Cluster Configuration

        public static int AddConfig(INConfigurationValueModel ConfigValue)
        {
            int id = 0;

            try
            {
                
                using (var contextFactory = new DbContextFactory())
                {
                    INClusterConfigurationValue cv = new INClusterConfigurationValue();
                    cv.ClientId = ConfigValue.ClientId;
                    cv.ActiveFlag =Convert.ToInt32(ConfigValue.ActiveFlag);
                    cv.CreatedBy = ConfigValue.CreatedBy;
                    cv.CreatedDt = ConfigValue.CreatedDt;
                    cv.Description = ConfigValue.Description;
                    cv.ParameterName = ConfigValue.ParameterName;
                    cv.ParameterValue = ConfigValue.ParameterValue;
                    cv.KeyId = ConfigValue.KeyId;
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    unitOfWork.INClusterConfigurationValue.Insert(cv);
                    unitOfWork.Save();
                    id = cv.ID;

                }
            }
            catch (Exception)
            {
                return 0;
            }
            return id;
        }

        public static bool UpdateConfig(INConfigurationValueModel ConfigValue)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var cv = unitOfWork.INClusterConfigurationValue.GetByID(ConfigValue.Id);
                    if(cv.ParameterValue!=ConfigValue.ParameterValue)
                    {
                        cv.ClientId = ConfigValue.ClientId;
                        cv.ActiveFlag = Convert.ToInt32(ConfigValue.ActiveFlag);
                        cv.MultiSelectFlag = ConfigValue.MultiSelectFlag;
                        cv.CreatedBy = ConfigValue.CreatedBy;
                        cv.CreatedDt = ConfigValue.CreatedDt;
                        cv.UpdatedBy = ConfigValue.UpdatedBy;
                        cv.UpdatedDt = DateTime.Now;
                        if(ConfigValue.Description != null)
                        cv.Description = ConfigValue.Description.Trim();
                        //coz Fetched Data is DisplayName, ConfigValue.ParameterName consists DisplayName
                        cv.ParameterName = cv.ParameterName;
                        if (ConfigValue.ParameterValue != null)
                            cv.ParameterValue = ConfigValue.ParameterValue.Trim();
                        cv.KeyId = ConfigValue.KeyId;
                        unitOfWork.INClusterConfigurationValue.Update(cv);
                        unitOfWork.Save();
                    }
                    
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static bool DeleteConfig(int Id)
        {

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.INClusterConfigurationValue.Delete(Id);
                unitOfWork.Save();

                //TempData["SLACreatedDeleted"] = "Deleted successfully.";
                return true;
            }


        }


        public static IEnumerable<SelectListItem> INClusterParameterValueList(int KeyID, int ClientID)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            //var ddd = unitOfWork.INClusterParameterValue.Get().Where(x => x.ClientId == ClientID && x.KeyId == KeyID).ToList();
            var ddd = unitOfWork.INClusterParameterValue.Get().Where(x => x.KeyId == KeyID).OrderBy(o=>o.SeqNo).ToList();
            var item = ddd.Select(p => new SelectListItem { Value = p.ParameterValue, Text = p.Description });
            return item;
        }

        //For 
        public static DataTable NextGenToolUsers(int uid)
        {
            IList<TrendingReportInput> Tredingdata = new List<TrendingReportInput>();
            var paramName = new string[] { "UserId" };
            var paramValue = new object[] { uid };
            DataTable dt = new DataTable();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                dt = unitOfWork.GetDataFromStoredProcedure("NextGenToolUsers", paramName, paramValue);
            }
            return dt;
        }


        public static IList<PerspectiveCategoryModel> GetPrespectiveCategory(int? ClientId,int ToolId)
        {
            IList<PerspectiveCategoryModel> Pcategory = new List<PerspectiveCategoryModel>();
            var entityMapper = new Mapper<PerspectiveCategoryModel, PerspectiveCategory>();

            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<PerspectiveCategory> PerspectiveDetail = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                if (ClientId != null && ClientId != 0)
                    PerspectiveDetail = unitOfWork.PerspectiveCategory.Get(cid => cid.ClientId == ClientId&&cid.ToolId== ToolId).ToList();

                if (PerspectiveDetail != null)
                {
                    foreach (var _perspectivedetail in PerspectiveDetail)
                    {
                        PerspectiveCategoryModel PerspectiveModel = new PerspectiveCategoryModel();
                        PerspectiveModel.PerspectiveCategoryName = _perspectivedetail.PerspectiveCategoryName;
                        PerspectiveModel.Id = _perspectivedetail.Id;
                        PerspectiveModel.ClientId = _perspectivedetail.ClientId;
                        PerspectiveModel.ToolId = _perspectivedetail.ToolId;
                        Pcategory.Add(PerspectiveModel);
                    }
                }
                return Pcategory;
            }

        }


        public static int AddPerspectiveCategory(PerspectiveCategoryModel perspectivecategoryinput)
        {
            int id = 0;
            try
            {

                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    PerspectiveCategory PerspectiveCategoryinfo = new PerspectiveCategory();
                    PerspectiveCategoryinfo.PerspectiveCategoryName = perspectivecategoryinput.PerspectiveCategoryName;
                    PerspectiveCategoryinfo.ClientId = perspectivecategoryinput.ClientId;
                    PerspectiveCategoryinfo.ToolId = perspectivecategoryinput.ToolId;
                    unitOfWork.PerspectiveCategory.Insert(PerspectiveCategoryinfo);
                    unitOfWork.Save();
                    id = PerspectiveCategoryinfo.Id;

                }
            }
            catch (Exception)
            {
                return 0;
            }
            return id;
        }


        public static bool EditPerspectiveCategory(PerspectiveCategoryModel perspectivecategoryinput)
        {
            try
            {
                var entityMapper = new Mapper<PerspectiveCategory, PerspectiveCategoryModel>();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var perspectivecategoryinfo = unitOfWork.PerspectiveCategory.GetByID(perspectivecategoryinput.Id);
                    // entityMapper.MapToEntity(Portfolioinput, appPortfolioinfo);
                    perspectivecategoryinfo.PerspectiveCategoryName = perspectivecategoryinput.PerspectiveCategoryName;
                    unitOfWork.PerspectiveCategory.Update(perspectivecategoryinfo);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool DestroyPerspectiveCategory(int PId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.PerspectiveCategory.Delete(PId);
                unitOfWork.Save();

                //TempData["SLACreatedDeleted"] = "Deleted successfully.";
                return true;
            }
        }


        public static IList<SurveyPerspectiveModel> GetSurveyPerspective(int PerCatId =0,int toolid=0)
        {
            IList<SurveyPerspectiveModel> Pcategory = new List<SurveyPerspectiveModel>();          
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<SurveyPerspective> SurveyPerspectiveDet = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                if (PerCatId != 0)
                    SurveyPerspectiveDet = unitOfWork.SurveyPerspective.Get(cid => cid.PerspectiveCategoryId == PerCatId&&cid.ToolId==toolid).ToList();

                if (SurveyPerspectiveDet != null)
                {
                    foreach (var _perspectivedetail in SurveyPerspectiveDet)
                    {
                        SurveyPerspectiveModel PerspectiveModel = new SurveyPerspectiveModel();
                        PerspectiveModel.PerspectiveName = _perspectivedetail.PerspectiveName;
                        PerspectiveModel.PerspectiveType = _perspectivedetail.PerspectiveType;
                        PerspectiveModel.Weight = _perspectivedetail.Weight;
                        PerspectiveModel.SId = _perspectivedetail.Id;
                        PerspectiveModel.ClientId = _perspectivedetail.ClientId;
                        Pcategory.Add(PerspectiveModel);
                    }
                }
                return Pcategory;
            }

        }
       

        public static int AddSurveyPerspective(SurveyPerspectiveModel SurveyPerspectiveinput)
        {
            int id = 0;
            try
            {

                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    SurveyPerspective SurveyPerspectiveinfo = new SurveyPerspective();
                    SurveyPerspectiveinfo.ClientId = SurveyPerspectiveinput.ClientId;
                    SurveyPerspectiveinfo.PerspectiveCategoryId = SurveyPerspectiveinput.PerspectiveCategoryId;
                    SurveyPerspectiveinfo.PerspectiveName = SurveyPerspectiveinput.PerspectiveName;
                    SurveyPerspectiveinfo.PerspectiveType = SurveyPerspectiveinput.PerspectiveType;
                    SurveyPerspectiveinfo.Weight = SurveyPerspectiveinput.Weight;
                    SurveyPerspectiveinfo.CreatedBy = SurveyPerspectiveinput.CreatedBy;
                    SurveyPerspectiveinfo.Createddt = SurveyPerspectiveinput.Createddt;
                    SurveyPerspectiveinfo.ToolId = SurveyPerspectiveinput.ToolId;
                    unitOfWork.SurveyPerspective.Insert(SurveyPerspectiveinfo);
                    unitOfWork.Save();
                    id = SurveyPerspectiveinfo.Id;

                }
            }
            catch (Exception)
            {
                return 0;
            }
            return id;
        }


        public static bool EditSurveyPerspective(SurveyPerspectiveModel SurveyPerspectiveinput)
        {
            try
            {

                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var SurveyPerspectiveinfo = unitOfWork.SurveyPerspective.GetByID(SurveyPerspectiveinput.SId);
                    // entityMapper.MapToEntity(Portfolioinput, appPortfolioinfo);                    
                    SurveyPerspectiveinfo.PerspectiveName = SurveyPerspectiveinput.PerspectiveName;
                    SurveyPerspectiveinfo.PerspectiveType = SurveyPerspectiveinput.PerspectiveType;
                    SurveyPerspectiveinfo.Weight = SurveyPerspectiveinput.Weight;
                    SurveyPerspectiveinfo.UpdatedBy = SurveyPerspectiveinput.UpdatedBy;
                    SurveyPerspectiveinfo.updateddt = SurveyPerspectiveinput.updateddt;
                    SurveyPerspectiveinfo.ToolId = SurveyPerspectiveinput.ToolId;
                    unitOfWork.SurveyPerspective.Update(SurveyPerspectiveinfo);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool DestorySurveyPerspective(int SPId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.SurveyPerspective.Delete(SPId);
                unitOfWork.Save();
                return true;
            }
        }

        public static void deleteAllPerspectives(int CatId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var listDel = unitOfWork.SurveyPerspective.Get().Where(g => g.PerspectiveCategoryId == CatId);
            foreach(var d in listDel)
            {
                unitOfWork.SurveyPerspective.Delete(d.Id);
            }
            unitOfWork.Save();
        }



        //------------------------------------Teams helper--------------------------------//
        public static IList<IMTeamModel> GetTeamsDetails(int ClientId, int uid)
        {

            IList<IMTeamModel> Teams = new List<IMTeamModel>();
            using (var contextFactory = new DbContextFactory())
            {
                IEnumerable<IMTeam> TeamsDetails = null;
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                if (ClientId != 0)
                    TeamsDetails = unitOfWork.IMTeam.Get(cid => cid.ClientId == ClientId).ToList();

                if (TeamsDetails != null)
                {
                    foreach (var _teamdetail in TeamsDetails)
                    {
                        IMTeamModel cs = new IMTeamModel();
                        cs.Id = _teamdetail.Id;
                        cs.ClientId = _teamdetail.ClientId;
                        cs.Team = _teamdetail.Team;
                        cs.IdeaSourceVal = _teamdetail.IdeaSourceVal;
                        cs.CreatedBy = _teamdetail.CreatedBy;
                        cs.UpdatedBy = _teamdetail.UpdatedBy;
                        cs.CreatedDt = _teamdetail.CreatedDt;
                        cs.UpdatedDt = _teamdetail.UpdatedDt;
                        Teams.Add(cs);
                    }
                }
                return Teams;
            }

        }
        public static int CreateTeams(IMTeamModel imTeamModel)
        {
            int id = 0;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var tea = new IMTeam();
                    tea.ClientId = imTeamModel.ClientId;
                    tea.Team = imTeamModel.Team;
                    tea.IdeaSourceVal = imTeamModel.IdeaSourceVal;
                    tea.CreatedDt = DateTime.Now;
                    tea.CreatedBy = imTeamModel.CreatedBy;
                    unitOfWork.IMTeam.Insert(tea);
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

        public static bool UpdateTeams(IMTeamModel imTeamModel)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.IMTeam.GetByID(imTeamModel.Id);
                    updatetitem.ClientId = imTeamModel.ClientId;
                    updatetitem.Team = imTeamModel.Team;
                    updatetitem.IdeaSourceVal = imTeamModel.IdeaSourceVal;
                    updatetitem.UpdatedBy = imTeamModel.UpdatedBy;
                    updatetitem.UpdatedDt = DateTime.Now;
                    unitOfWork.IMTeam.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RemoveTeams(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.IMTeam.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }
        public static bool UpdateDispositionDetail(DispositionAutomationModel DispAutomation, int ClientId)
        {
            //Db db = new Db();
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var CliendDetail = unitOfWork.Clients.GetByID(ClientId);
                    var IsIsDispositionEnable = (DispAutomation.IsDisposition) ? 1 : 0;
                    CliendDetail.EnableDispositionAutomation = IsIsDispositionEnable;
                    CliendDetail.SurveyTypeName = DispAutomation.SurveyType;
                    unitOfWork.Clients.Update(CliendDetail);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static DispositionAutomationModel getClientDetail(int ClientId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            DispositionAutomationModel model = new DispositionAutomationModel();

            var CommonDetails = unitOfWork.Clients.Get(g => g.Id == ClientId).FirstOrDefault();
          
            if (CommonDetails != null)
            {
                var EnableDisAutomation = (CommonDetails.EnableDispositionAutomation==1) ? true : false;
                model.IsDisposition = EnableDisAutomation;
                model.SurveyType = CommonDetails.SurveyTypeName;
            }
            return model;

        }

        public static bool UpdateAlternativeDetail(AlternativeDashboardModel AltDashboard, int ClientId)
        {
           // Db db = new Db();
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var CliendDetail = unitOfWork.Clients.GetByID(ClientId);
                    var IsAlternativeEnable = (AltDashboard.IncludeAlternativeDashboard) ? 1 : 0;
                    CliendDetail.IncludeAlternativeDashboard = IsAlternativeEnable;
                    unitOfWork.Clients.Update(CliendDetail);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static AlternativeDashboardModel getAlternativeClientDetail(int ClientId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            AlternativeDashboardModel model = new AlternativeDashboardModel();

            var CommonDetails = unitOfWork.Clients.Get(g => g.Id == ClientId).FirstOrDefault();

            if (CommonDetails != null)
            {
                var EnableAltAutomation = (CommonDetails.IncludeAlternativeDashboard == 1) ? true : false;
                model.IncludeAlternativeDashboard= EnableAltAutomation;
            }
            return model;

        }

        public static bool UpdateProfile(byte[] img,string s,int ClientId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
         
            var cl = unitOfWork.Clients.Get(g => g.Id == ClientId).FirstOrDefault();
            if (cl != null)
            {
                cl.CPMFile = img;
                cl.Filename = s;
                unitOfWork.Clients.Update(cl);
                unitOfWork.Save();


            }

            return true;
        }
       
        public static bool UpdateClientProfile(byte[] img, string s, int ClientId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

            var cl = unitOfWork.Clients.Get(g => g.Id == ClientId).FirstOrDefault();
            if (cl != null)
            {
                cl.EmailTemplateImageCPMFile = img;
                cl.EmailTemplateImageName = s;
                unitOfWork.Clients.Update(cl);
                unitOfWork.Save();


            }

            return true;
        }
        public static string getFullName(int userId)
        {
            Db db = new Db();
            var fullName = (from b in db.UserProfiles
                            where b.Id == userId
                            select b.FirstName + " " + b.LastName).FirstOrDefault();

            return fullName;
        }

        public static  Client getProfileClientDetail(int ClientId)
        {
            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
          //  Client model = new Client();

            var CommonDetails = unitOfWork.Clients.Get(g => g.Id == ClientId).FirstOrDefault();

            //if (CommonDetails != null)
            //{
            //    model.Filename = CommonDetails.Filename;
            //    model.CPMFile = CommonDetails.CPMFile;
            //    model.EmailTemplateImageName = CommonDetails.EmailTemplateImageName;
            //    model.EmailTemplateImageCPMFile = CommonDetails.EmailTemplateImageCPMFile;
            //}
            return CommonDetails;

        }

        public static byte[] getProfileClientFile(int ClientId)
        {
            //var contextFactory = new DbContextFactory();
            //var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            //// Client model = new Client();
            Db db = new Db();


            // byte[] imgfile=null;
            var imgfile = (from d in db.Clients where d.Id==ClientId select d.CPMFile).FirstOrDefault(); //unitOfWork.Clients.Get(g => g.Id == ClientId).FirstOrDefault();

            ////if (CommonDetails != null)
            ////{
                //model.Filename = CommonDetails.Filename;
            //    imgfile = CommonDetails.CPMFile;
           // }
            return imgfile;

        }


        public static IEnumerable<LogModel> getauthLogs(int ClientId)
        {
            Db db = new Db();
           // var contextFactory = new DbContextFactory();
            //var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            var temp = (from lg in db.MFAUserAuthLogging
                        join up in db.UserProfiles on lg.UserId equals up.Id
                        join mt in db.MFAType on lg.MFATypeId equals mt.Id
                        where lg.ClientId == ClientId
                        select new
                        {
                            UserName = up.FirstName + " " + up.LastName,
                            AuthType = mt.AuthType,
                            Verified = lg.VerifiedTime,
                            Date = lg.RequestedTime
                        }
                );


            var returnList = temp.Select(f => new LogModel
            {
                Auth = f.AuthType,
                UserName = f.UserName,
                IsVerified = f.Verified==null?false:true,
                Date = f.Date
            }).AsEnumerable();
            return returnList;
        }

    

        public static IList<SurveyINUserModel> readINSurvey(int ClientId, int surveyid)
        {

            IList<SurveyINUserModel> EstCategoryList = new List<SurveyINUserModel>();
            ////using (var contextFactory = new DbContextFactory())
            ////{
                ////IEnumerable<SurveyINUser> RowDetails = null;
                //////IEnumerable<Survey> SurveyDetail = null;
                ////var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                //////  var SurveyDetail = unitOfWork.Survey.Get(e => e.ClientId == ClientId && e.ToolId == 13).OrderByDescending(o => o.Createddt).FirstOrDefault();
                ////var surveyId = surveyid;// SurveyDetail.Id;
                ////RowDetails = unitOfWork.SurveyINUser.Get(e => e.ClientId == ClientId&&e.SurveyId== surveyId).ToList();


                Db db = new Db();
            int surveyId = 0;
            surveyId = (from s in db.Survey where s.ClientId==ClientId && s.ToolId==13 select s ).OrderByDescending(o => o.Createddt).FirstOrDefault().Id;
            var personnellist = (from sin in db.SurveyINUser
                                     join inc in db.Incidents on sin.IncidentId equals inc.Id
                                     where inc.ClientId == ClientId && sin.SurveyId ==surveyId
                                     select new SurveyINUserModel
                                     {
                                         Id = sin.Id,
                                         TicketId = sin.TicketId,
                                         INCreatedDate = sin.INCreatedDate,
                                         INClosedDate = sin.INClosedDate,
                                         SurveyUserName = sin.SurveyUserName,
                                         SurveyUserEmail = sin.SurveyUserEmail,
                                         SurveyStatus = sin.SurveyStatus,
                                         Completedt = sin.Completedt,
                                         SurveyId = sin.SurveyId,
                                         ClientId = sin.ClientId,
                                         UserSurveyToken = sin.UserSurveyToken,
                                         SurveySentDate = sin.SurveySentDate,
                                         validtill = sin.validtill,
                                         INTitle = sin.INTitle,
                                         ClientPriority=inc.clientpriority,
                                         shortDescription=inc.ShortDescription,
                                         Track=inc.Tower

                                     }
                                     ).OrderByDescending(od=>od.Id).ToList() ;
                           
                return personnellist;
            

        }

        public static int addINSurvey(SurveyINUserModel Input)
        {
            int ra_id = 0;

            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var _Details = new SurveyINUser();
                    //_Details.TeamId = Input.TeamId;
                    //_Details.INFieldName = Input.INFieldName;
                    //_Details.FieldValue = Input.FieldValue;
                    //_Details.Operator = Input.Operator;
                    //_Details.GroupEndOperator = Input.GroupEndOperator;
                    //_Details.GroupStartOperator = Input.GroupStartOperator;
                    unitOfWork.SurveyINUser.Insert(_Details);
                    unitOfWork.Save();
                    ra_id = _Details.Id;

                    return ra_id;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }



        public static bool updateINSurvey(SurveyINUserModel _Input)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var updatetitem = unitOfWork.SurveyINUser.GetByID(_Input.Id);
                    updatetitem.SurveyUserName = _Input.SurveyUserName;
                    updatetitem.SurveyUserEmail = _Input.SurveyUserEmail;
                    unitOfWork.SurveyINUser.Update(updatetitem);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static bool RemoveINSurvey(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.SurveyINUser.Delete(Id);
                unitOfWork.Save();
                return true;
            }
        }

        public static IEnumerable<ClientAccountList> getClientAccountList(int clientId)
        {
            IList<ClientAccountList> accList = new List<ClientAccountList>();
            using (var contextFactory = new DbContextFactory())
            {
                Db db = new Db();
                var accdetails = (from a in db.ClientAccountSetup
                                  where a.ClientId == clientId && a.IsDeleted == false && a.Id!=0
                                  select new ClientAccountList {
                                      AccountId = a.Id,
                                      AccountName = a.AccountName,
                                      OpenDate = a.OpenDate,
                                      CloseDate = a.CloseDate,
                                      ClientId = a.ClientId,
                                      AccountStatuses = a.AccountStatusId,
                                      ClientAccountSponsor = a.ClientAccountSponsor,
                                      FiscalYearActiveValue = a.FiscalYearActiveValue,
                                      FiscalYearContractRollupCheck = a.FiscalYearContractRollupCheck,
                                      FiscalYearPlannedValue = a.FiscalYearPlannedValue,
                                      FiscalYearPlannedRollupCheck = a.FiscalYearPlannedRollupCheck,
                                      FiscalYearPipelineValue = a.FiscalYearPipelineValue,
                                      FiscalYearPipelineRollupCheck = a.FiscalYearPipelineRollupCheck,
                                      FiscalYearExecutedValue = a.FiscalYearExecutedValue,
                                      FiscalYearExecutedRollupCheck = a.FiscalYearExecutedRollupCheck,
                                      AverageAnnualFTEs = a.AverageAnnualFTEs,
                                      AverageAnnualFTEsOnOffShoreCheck = a.AverageAnnualFTEsOnOffShoreCheck,
                                      AverageAnnualFTEsRollupCheck = a.AverageAnnualFTEsRollupCheck,
                                      AverageAnnualFTEsOnShore = a.AverageAnnualFTEsOnShore,
                                      AverageAnnualFTEsOnShoreRollupCheck = a.AverageAnnualFTEsOnShoreRollupCheck,
                                      AverageAnnualFTEsOffShore = a.AverageAnnualFTEsOffShore,
                                      AverageAnnualFTEsOffSchoreCheck = a.AverageAnnualFTEsOffSchoreCheck,
                                      GlobalClientManager = a.GlobalClientManager,
                                      GlobalDeliveryManager = a.GlobalDeliveryManager,
                                      AverageAnnualRevenue = a.AverageAnnualRevenue,
                                      AccountStatus = a.Status,
                                      RevenueToDate = a.RevenueToDate,
                                      SumOfFTEOnShoreOffShore = a.SumOfFTEOnShoreOffShore,
                                      AverageAnnualRevenueCheck = a.AverageAnnualRevenueCheck,
                                      RevenueToDateCheck = a.RevenueToDateCheck,
                                  }).ToList();
                
                accList = accdetails;
            }
            return accList;
        }
        public static IEnumerable<ClientAccContract> getAccContractList(int clientId, int accid)
        {
            IList<ClientAccContract> AccContractList = new List<ClientAccContract>();
            using (var contextFactory = new DbContextFactory())
            {
                Db db = new Db();
                var contdetails = (from a in db.ClientAccountContract
                                  where a.ClientId == clientId && a.AccountId==accid && a.IsDeleted == false && a.Id != 0
                                   select new ClientAccContract
                                  {
                                        ContractId=a.Id,
                                        AccountId=a.AccountId,
                                        ContractName=a.ContractName,
                                        ContractStatus=a.ContractStatus,
                                        ClientId=a.ClientId,
                                        NTTDataClientManager = a.NTTDataClientManager,
                                        NTTDataDeliveryManager = a.NTTDataDeliveryManager,
                                        ClientContractSponspor = a.ClientContractSponspor,
                                        ContractOpenDate = a.ContractOpenDate,
                                        ContractCloseDate = a.ContractCloseDate,
                                        FYActiveContractValue = a.FYActiveContractValue,
                                        RollUpFYActiveContractValue = a.RollUpFYActiveContractValue,
                                        FYPlannedContractValue = a.FYPlannedContractValue,
                                        RollUpFYPlannedContractValue = a.RollUpFYPlannedContractValue,
                                        FYPipeLineContractValue = a.FYPipeLineContractValue,
                                        RollUpFYPipeLineContractValue = a.RollUpFYPipeLineContractValue,
                                        FYExecutedContractValue = a.FYExecutedContractValue,
                                        RollUpFYExecutedContractValue = a.RollUpFYExecutedContractValue,
                                        AverageAnnualRevenue = a.AverageAnnualRevenue,
                                        RollUpAverageAnnualRevenue = a.RollUpAverageAnnualRevenue,
                                        RevenueToDate = a.RevenueToDate,
                                        RollUpRevenueToDate = a.RollUpRevenueToDate,
                                        AverageAnnualFTE = a.AverageAnnualRevenue,
                                        RollUpAverageAnnualFTE = a.RollUpAverageAnnualFTE,
                                        SumOfFTEOnShoreOffShore = a.SumOfFTEOnShoreOffShore,
                                        AverageAnnualFTEOnshore = a.AverageAnnualFTEOnshore,
                                        RollUpAverageAnnualFTEOnshore = a.RollUpAverageAnnualFTEOnshore,
                                        AverageAnnualFTEoffShore = a.AverageAnnualFTEoffShore,
                                        RollUpAverageAnnualFTEoffShore = a.RollUpAverageAnnualFTEoffShore,
                                        ContractType = a.ContractType,
                                        HoursOfService = a.HoursOfService
                             }).ToList();
                foreach (var item in contdetails)
                {
                    var contractList = new ClientAccContract();
                      contractList.ContractId = item.ContractId;
                      contractList.AccountId = item.AccountId;
                      contractList.ContractName = item.ContractName;
                      contractList.ContractStatus = item.ContractStatus;
                    contractList.ClientId = item.ClientId;
                    contractList.NTTDataClientManager = item.NTTDataClientManager;
                      contractList.NTTDataDeliveryManager = item.NTTDataDeliveryManager;
                      contractList.ClientContractSponspor = item.ClientContractSponspor;
                      contractList.ContractOpenDate = item.ContractOpenDate;
                      contractList.ContractCloseDate = item.ContractCloseDate;
                      contractList.FYActiveContractValue = item.FYActiveContractValue;
                      contractList.RollUpFYActiveContractValue = item.RollUpFYActiveContractValue;
                      contractList.FYPlannedContractValue = item.FYPlannedContractValue;
                      contractList.RollUpFYPlannedContractValue = item.RollUpFYPlannedContractValue;
                      contractList.FYPipeLineContractValue = item.FYPipeLineContractValue;
                      contractList.RollUpFYPipeLineContractValue = item.RollUpFYPipeLineContractValue;
                      contractList.FYExecutedContractValue = item.FYExecutedContractValue;
                      contractList.RollUpFYExecutedContractValue = item.RollUpFYExecutedContractValue;
                      contractList.AverageAnnualRevenue = item.AverageAnnualRevenue;
                      contractList.RollUpAverageAnnualRevenue = item.RollUpAverageAnnualRevenue;
                      contractList.RevenueToDate = item.RevenueToDate;
                    contractList.RollUpRevenueToDate = item.RollUpRevenueToDate;
                      contractList.AverageAnnualFTE = item.AverageAnnualRevenue;
                      contractList.RollUpAverageAnnualFTE = item.RollUpAverageAnnualFTE;
                      contractList.SumOfFTEOnShoreOffShore = item.SumOfFTEOnShoreOffShore;
                      contractList.AverageAnnualFTEOnshore = item.AverageAnnualFTEOnshore;
                      contractList.RollUpAverageAnnualFTEOnshore = item.RollUpAverageAnnualFTEOnshore;
                      contractList.AverageAnnualFTEoffShore = item.AverageAnnualFTEoffShore;
                      contractList.RollUpAverageAnnualFTEoffShore = item.RollUpAverageAnnualFTEoffShore;
                      contractList.ContractType = item.ContractType;
                      contractList.HoursOfService = item.HoursOfService;
                       AccContractList.Add(contractList);
                }
            }
            return AccContractList;
        }

        public static int CreateNewClientAccount(int ClientId,ClientAccountList model)
        {
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    ClientAccounSetup cv = new ClientAccounSetup();
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    cv.AccountName = model.AccountName;
                    cv.OpenDate = model.OpenDate;
                    cv.CloseDate = model.CloseDate;
                    cv.ClientId = ClientId;
                    cv.AccountStatusId = model.AccountStatuses;
                    cv.ClientAccountSponsor = model.ClientAccountSponsor;
                    cv.FiscalYearActiveValue = model.FiscalYearActiveValue;
                    cv.FiscalYearContractRollupCheck = model.FiscalYearContractRollupCheck;
                    cv.FiscalYearPlannedValue = model.FiscalYearPlannedValue;
                    cv.FiscalYearPlannedRollupCheck = model.FiscalYearPlannedRollupCheck;
                    cv.FiscalYearPipelineValue = model.FiscalYearPipelineValue;
                    cv.FiscalYearPipelineRollupCheck = model.FiscalYearPipelineRollupCheck;
                    cv.FiscalYearExecutedValue = model.FiscalYearExecutedValue;
                    cv.FiscalYearExecutedRollupCheck = model.FiscalYearExecutedRollupCheck;
                    cv.AverageAnnualFTEs = model.AverageAnnualFTEs;
                    cv.AverageAnnualFTEsOnOffShoreCheck = model.AverageAnnualFTEsOnOffShoreCheck;
                    cv.AverageAnnualFTEsRollupCheck = model.AverageAnnualFTEsRollupCheck;
                    cv.AverageAnnualFTEsOnShore = model.AverageAnnualFTEsOnShore;
                    cv.AverageAnnualFTEsOnShoreRollupCheck = model.AverageAnnualFTEsOnShoreRollupCheck;
                    cv.AverageAnnualFTEsOffShore = model.AverageAnnualFTEsOffShore;
                    cv.AverageAnnualFTEsOffSchoreCheck = model.AverageAnnualFTEsOffSchoreCheck;
                    cv.GlobalClientManager = model.GlobalClientManager;
                    cv.GlobalDeliveryManager = model.GlobalDeliveryManager;
                    cv.AverageAnnualRevenue = model.AverageAnnualRevenue;
                    cv.Status = model.AccountStatus;
                    cv.RevenueToDate = model.RevenueToDate;
                    cv.SumOfFTEOnShoreOffShore = model.SumOfFTEOnShoreOffShore;
                    cv.AverageAnnualRevenueCheck = model.AverageAnnualRevenueCheck;
                    cv.RevenueToDateCheck = model.RevenueToDateCheck;
                    unitOfWork.ClientAccounSetup.Insert(cv);
                    unitOfWork.Save();
                    id = cv.Id;
                }
                return id;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static bool UpdateClientAccount(int ClientId,ClientAccountList model)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var cv = unitOfWork.ClientAccounSetup.GetByID(model.AccountId);
                    cv.AccountName = model.AccountName;
                    cv.OpenDate = model.OpenDate;
                    cv.CloseDate = model.CloseDate;
                    cv.ClientId = ClientId;
                    cv.AccountStatusId = model.AccountStatuses;
                    cv.ClientAccountSponsor = model.ClientAccountSponsor;
                    cv.FiscalYearActiveValue = model.FiscalYearActiveValue;
                    cv.FiscalYearContractRollupCheck = model.FiscalYearContractRollupCheck;
                    cv.FiscalYearPlannedValue = model.FiscalYearPlannedValue;
                    cv.FiscalYearPlannedRollupCheck = model.FiscalYearPlannedRollupCheck;
                    cv.FiscalYearPipelineValue = model.FiscalYearPipelineValue;
                    cv.FiscalYearPipelineRollupCheck = model.FiscalYearPipelineRollupCheck;
                    cv.FiscalYearExecutedValue = model.FiscalYearExecutedValue;
                    cv.FiscalYearExecutedRollupCheck = model.FiscalYearExecutedRollupCheck;
                    cv.AverageAnnualFTEs = model.AverageAnnualFTEs;
                    cv.AverageAnnualFTEsOnOffShoreCheck = model.AverageAnnualFTEsOnOffShoreCheck;
                    cv.AverageAnnualFTEsRollupCheck = model.AverageAnnualFTEsRollupCheck;
                    cv.AverageAnnualFTEsOnShore = model.AverageAnnualFTEsOnShore;
                    cv.AverageAnnualFTEsOnShoreRollupCheck = model.AverageAnnualFTEsOnShoreRollupCheck;
                    cv.AverageAnnualFTEsOffShore = model.AverageAnnualFTEsOffShore;
                    cv.AverageAnnualFTEsOffSchoreCheck = model.AverageAnnualFTEsOffSchoreCheck;
                    cv.GlobalClientManager = model.GlobalClientManager;
                    cv.GlobalDeliveryManager = model.GlobalDeliveryManager;
                    cv.AverageAnnualRevenue = model.AverageAnnualRevenue;
                    cv.Status = model.AccountStatus;
                    cv.RevenueToDate = model.RevenueToDate;
                    cv.SumOfFTEOnShoreOffShore = model.SumOfFTEOnShoreOffShore;
                    cv.AverageAnnualRevenueCheck = model.AverageAnnualRevenueCheck;
                    cv.RevenueToDateCheck = model.RevenueToDateCheck;
                    unitOfWork.ClientAccounSetup.Update(cv);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool DeleteClientAccount(int AccountId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var av = unitOfWork.ClientAccounSetup.GetByID(AccountId);
                av.IsDeleted = true;
                var cv = unitOfWork.ClientAccountContract.Get(a=>a.AccountId==av.Id);
                foreach(var item in cv)
                {
                    item.IsDeleted = true;
                    unitOfWork.ClientAccountContract.Update(item);
                }
                unitOfWork.ClientAccounSetup.Update(av);
                unitOfWork.Save();
                return true;
            }
        }
        public static int CreateAccContract(int ClientId, ClientAccContract model)
        {
            int id = 0;
            try
            { 
                using (var contextFactory = new DbContextFactory())
                {
                    ClientAccountContract cv = new ClientAccountContract();
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    cv.ClientId = ClientId;
                    cv.AccountId = model.AccountId;
                    cv.ContractName = model.ContractName;
                    cv.ContractStatus = model.ContractStatus;
                    cv.NTTDataClientManager = model.NTTDataClientManager;
                    cv.NTTDataDeliveryManager = model.NTTDataDeliveryManager;
                    cv.ClientContractSponspor = model.ClientContractSponspor;
                    cv.ContractOpenDate = model.ContractOpenDate;
                    cv.ContractCloseDate = model.ContractCloseDate;
                    cv.FYActiveContractValue = model.FYActiveContractValue;
                    cv.RollUpFYActiveContractValue = model.RollUpFYActiveContractValue;
                    cv.FYPlannedContractValue = model.FYPlannedContractValue;
                    cv.RollUpFYPlannedContractValue = model.RollUpFYPlannedContractValue;
                    cv.FYPipeLineContractValue = model.FYPipeLineContractValue;
                    cv.RollUpFYPipeLineContractValue = model.RollUpFYPipeLineContractValue;
                    cv.FYExecutedContractValue = model.FYExecutedContractValue;
                    cv.RollUpFYExecutedContractValue = model.RollUpFYExecutedContractValue;
                    cv.AverageAnnualRevenue = model.AverageAnnualRevenue;
                    cv.RollUpAverageAnnualRevenue = model.RollUpAverageAnnualRevenue;
                    cv.RevenueToDate = model.RevenueToDate;
                    cv.RollUpRevenueToDate = model.RollUpRevenueToDate;
                    cv.AverageAnnualFTE = model.AverageAnnualRevenue;
                    cv.RollUpAverageAnnualFTE = model.RollUpAverageAnnualFTE;
                    cv.SumOfFTEOnShoreOffShore = model.SumOfFTEOnShoreOffShore;
                    cv.AverageAnnualFTEOnshore = model.AverageAnnualFTEOnshore;
                    cv.RollUpAverageAnnualFTEOnshore = model.RollUpAverageAnnualFTEOnshore;
                    cv.AverageAnnualFTEoffShore = model.AverageAnnualFTEoffShore;
                    cv.RollUpAverageAnnualFTEoffShore = model.RollUpAverageAnnualFTEoffShore;
                    cv.ContractType = model.ContractType;
                    cv.HoursOfService = model.HoursOfService;
                    unitOfWork.ClientAccountContract.Insert(cv);
                    unitOfWork.Save();
                    id = cv.Id;
                }
                return id;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static bool UpdateAccContract(int ClientId, ClientAccContract model)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var cv = unitOfWork.ClientAccountContract.GetByID(model.ContractId);
                    cv.ClientId = ClientId;
                    cv.AccountId = model.AccountId;
                    cv.ContractName = model.ContractName;
                    cv.ContractStatus = model.ContractStatus;
                    cv.NTTDataClientManager = model.NTTDataClientManager;
                    cv.NTTDataDeliveryManager = model.NTTDataDeliveryManager;
                    cv.ClientContractSponspor = model.ClientContractSponspor;
                    cv.ContractOpenDate = model.ContractOpenDate;
                    cv.ContractCloseDate = model.ContractCloseDate;
                    cv.FYActiveContractValue = model.FYActiveContractValue;
                    cv.RollUpFYActiveContractValue = model.RollUpFYActiveContractValue;
                    cv.FYPlannedContractValue = model.FYPlannedContractValue;
                    cv.RollUpFYPlannedContractValue = model.RollUpFYPlannedContractValue;
                    cv.FYPipeLineContractValue = model.FYPipeLineContractValue;
                    cv.RollUpFYPipeLineContractValue = model.RollUpFYPipeLineContractValue;
                    cv.FYExecutedContractValue = model.FYExecutedContractValue;
                    cv.RollUpFYExecutedContractValue = model.RollUpFYExecutedContractValue;
                    cv.AverageAnnualRevenue = model.AverageAnnualRevenue;
                    cv.RollUpAverageAnnualRevenue = model.RollUpAverageAnnualRevenue;
                    cv.RevenueToDate = model.RevenueToDate;
                    cv.RollUpRevenueToDate = model.RollUpRevenueToDate;
                    cv.AverageAnnualFTE = model.AverageAnnualRevenue;
                    cv.RollUpAverageAnnualFTE = model.RollUpAverageAnnualFTE;
                    cv.SumOfFTEOnShoreOffShore = model.SumOfFTEOnShoreOffShore;
                    cv.AverageAnnualFTEOnshore = model.AverageAnnualFTEOnshore;
                    cv.RollUpAverageAnnualFTEOnshore = model.RollUpAverageAnnualFTEOnshore;
                    cv.AverageAnnualFTEoffShore = model.AverageAnnualFTEoffShore;
                    cv.RollUpAverageAnnualFTEoffShore = model.RollUpAverageAnnualFTEoffShore;
                    cv.ContractType = model.ContractType;
                    cv.HoursOfService = model.HoursOfService;
                    unitOfWork.ClientAccountContract.Update(cv);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool DeleteAccContract(int ContractId)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var cv = unitOfWork.ClientAccountContract.GetByID(ContractId);
                cv.IsDeleted = true;
                unitOfWork.ClientAccountContract.Update(cv);
                unitOfWork.Save();
                return true;
            }
        }



        //Report Subscription

        public static int CreateReportSubscription(RTPSubcriptionModel RPT)
        {
            int id = 0;
            try

            {
                Db db = new Db();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var rptObject = new RPTSubscription();
                    rptObject.ClientID = RPT.ClientID;
                    rptObject.EmailSubject = RPT.EmailSubject;
                    rptObject.Recipients = RPT.Recipients;
                    rptObject.ReportFor = RPT.ReportFor;
                    rptObject.StartDate = DateTime.Parse(RPT.StartDate);
                    rptObject.EndDate = DateTime.Parse(RPT.EndDate);
                    rptObject.ScheduleType = RPT.ScheduleType;
                    rptObject.ReportParameters = RPT.ReportParameters;
                    rptObject.ReportFormat = RPT.ReportFormat;
                    rptObject.ReportName = RPT.ReportName;
                    rptObject.TelerikReportName = RPT.TelerikReportName;
                    rptObject.Status = RPT.Status;
                    rptObject.CreatedBy = RPT.CreatedBy;
                    rptObject.CreatedDate = RPT.CreatedDate;
                    rptObject.UpdatedBy = RPT.CreatedBy;
                    rptObject.UpdatedDate = RPT.CreatedDate;
                    rptObject.ScheduleTime = TimeSpan.Parse(RPT.ScheduleTime);
                    rptObject.WeekDay = default;
                    rptObject.MonthDay = default;
                    rptObject.Timezone = RPT.Timezone;
                    rptObject.AppID = RPT.AppID;
                    if (RPT.ScheduleType == "Weekly")
                    {
                        rptObject.WeekDay = RPT.WeekDay;
                    }
                    if (RPT.ScheduleType == "Monthly")
                    {
                        rptObject.MonthDay = RPT.MonthDay;
                    }
                    unitOfWork.RPTSubscription.Insert(rptObject);
                    unitOfWork.Save();
                    //db.RTPSubcription.Add(rptObject);
                    //db.SaveChanges();

                    id = rptObject.Id;
                    return id;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public static int UpdateReportSubscription(RTPSubcriptionModel RPT)
        {
            int id = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());



                    var rptObject = unitOfWork.RPTSubscription.GetByID(RPT.Id);


                    rptObject.EmailSubject = RPT.EmailSubject;
                    rptObject.Recipients = RPT.Recipients;
                    rptObject.ReportFor = RPT.ReportFor;
                    rptObject.StartDate = DateTime.Parse(RPT.StartDate);
                    rptObject.EndDate = DateTime.Parse(RPT.EndDate);
                    rptObject.ScheduleType = RPT.ScheduleType;
                    rptObject.ReportFormat = RPT.ReportFormat;
                    rptObject.TelerikReportName = RPT.TelerikReportName;
                    rptObject.UpdatedBy = RPT.UpdatedBy;
                    rptObject.UpdatedDate = RPT.UpdatedDate;
                    rptObject.ScheduleTime = TimeSpan.Parse(RPT.ScheduleTime);
                   
                    rptObject.WeekDay = default;
                    rptObject.MonthDay = default;
                    if (RPT.ScheduleType == "Weekly")
                    {
                        rptObject.WeekDay = RPT.WeekDay;
                    }
                    if (RPT.ScheduleType == "Monthly")
                    {
                        rptObject.MonthDay = RPT.MonthDay;
                    }
                    rptObject.Timezone = RPT.Timezone;
                    unitOfWork.RPTSubscription.Update(rptObject);
                    unitOfWork.Save();
                    return rptObject.Id;
                }
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public static List<RTPSubcriptionModel> GetRptSubscription(int clientid,string reportBy)
        {
            List<RTPSubcriptionModel> rec = new List<RTPSubcriptionModel>();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                Db db = new Db();
                var repSub = new List<RPTSubscription>();
                if (reportBy != "noreport") {
                     repSub = (from a in db.RPTSubscription
                                  where a.ClientID == clientid & a.ReportName == reportBy & (a.Status == "Approved" || a.Status == "Draft")
                               orderby a.Id descending  
                               select a).ToList();
                }
                else
                {
                     repSub = (from a in db.RPTSubscription
                               orderby a.Id descending
                               where a.ClientID == clientid & a.Status == "Approved"  || a.Status == "Draft"
                                  select a).ToList();
                }



                foreach (var RPT in repSub)
                {
                  
                        var rptObject = new RTPSubcriptionModel();
                        rptObject.Id = RPT.Id;
                        rptObject.ClientID = RPT.ClientID;
                        rptObject.ReportName = RPT.ReportName;
                        rptObject.Recipients = RPT.Recipients;
                        rptObject.StartDate = RPT.StartDate.ToString("MM/dd/yyyy");
                        rptObject.EndDate = RPT.EndDate.ToString("MM/dd/yyyy");
                        rptObject.ReportParameters = RPT.ReportParameters;
                        rptObject.ScheduleType = RPT.ScheduleType;
                        rptObject.CreatedBy = RPT.CreatedBy;
                        rptObject.CreatedDate = RPT.CreatedDate;
                        rptObject.UpdatedBy = RPT.UpdatedBy;
                        rptObject.UpdatedDate = RPT.UpdatedDate;
                        rptObject.Status = RPT.Status;
                        rptObject.ReportFor = RPT.ReportFor;
                        rptObject.CreatedByUser = (from b in db.UserProfiles
                                                   where b.Id == rptObject.UpdatedBy
                                                   select b.FirstName).FirstOrDefault();
                        rptObject.UpdatedByUser = (from b in db.UserProfiles
                                                   where b.Id == rptObject.UpdatedBy
                                                   select b.FirstName).FirstOrDefault();
                        rec.Add(rptObject);
                    }
                }
            
            return rec;
        }

        public static List<RTPSubcriptionModel> GetReportDetails(int ID)
        {
            List<RTPSubcriptionModel> rec = new List<RTPSubcriptionModel>();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                Db db = new Db();
                var repSub = (from a in db.RPTSubscription
                              where a.Id == ID
                              select a).ToList();
                // var repSub = unitOfWork.RPTSubscription.Get(x => x.ClientID == clientid).ToList();
                foreach (var RPT in repSub)
                {
                    var rptObject = new RTPSubcriptionModel();
                    rptObject.Id = RPT.Id;
                    rptObject.ClientID = RPT.ClientID;
                    rptObject.ReportName = RPT.ReportName;
                    rptObject.Recipients = RPT.Recipients;
                    rptObject.EmailSubject = RPT.EmailSubject;
                    rptObject.StartDate = RPT.StartDate.ToString("MM/dd/yyyy");
                    rptObject.EndDate = RPT.EndDate.ToString("MM/dd/yyyy");
                    rptObject.ReportParameters = RPT.ReportParameters;
                    rptObject.ReportFor = RPT.ReportFor;
                    rptObject.ReportFormat = RPT.ReportFormat;
                    rptObject.TelerikReportName = RPT.TelerikReportName;
                    rptObject.ScheduleType = RPT.ScheduleType;
                    rptObject.CreatedBy = RPT.CreatedBy;
                    rptObject.CreatedDate = RPT.CreatedDate;
                    rptObject.UpdatedBy = RPT.UpdatedBy;
                    rptObject.UpdatedDate = RPT.UpdatedDate;
                    rptObject.Status = RPT.Status;
                    rptObject.CreatedByUser = (from b in db.UserProfiles
                                               where b.Id == rptObject.UpdatedBy
                                               select b.FirstName).FirstOrDefault();
                    rptObject.UpdatedByUser = (from b in db.UserProfiles
                                               where b.Id == rptObject.UpdatedBy
                                               select b.FirstName).FirstOrDefault();
                    var timearray = RPT.ScheduleTime.ToString().Split(':');
                    rptObject.selectHours = timearray[0];
                    rptObject.selectMinutes = timearray[1];
                    rptObject.MonthDay = RPT.MonthDay;
                    rptObject.WeekDay = RPT.WeekDay;
                    rptObject.Timezone = RPT.Timezone;
                    rptObject.AppID = RPT.AppID;
                    rec.Add(rptObject);
                }
            }
            return rec;
        }

        public static List<RTPJobDetailModel> GetRptJobDetailList(int clientid,string reportBy1)
        {
            List<RTPJobDetailModel> rec = new List<RTPJobDetailModel>();
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                Db db = new Db();

                if (reportBy1 != "noreport")
                {
                    rec = (from a in db.RPTJobDetail
                           join b in db.RPTSubscription on a.RPTSubscriptionId equals b.Id
                           where b.ClientID == clientid && b.ReportName==reportBy1 && b.Status != "Deleted"
                           orderby a.Id descending
                           select new RTPJobDetailModel
                           {
                               Id = a.Id,
                               ReportName = b.ReportName,
                               TelerikReportName = b.TelerikReportName,
                               Recipients = b.Recipients,
                               ScheduleType = b.ScheduleType,
                               ReportFor = b.ReportFor,
                               JobScheduledDate = a.ScheduleDateTime,
                               ReportParameters = b.ReportParameters,
                               ReportFormat = b.ReportFormat,
                               ReportDocument = a.ReportDocument,
                               RPTJobUrl = a.RPTJobUrl
                           }).ToList();
                }
                else
                {
                    rec = (from a in db.RPTJobDetail
                           join b in db.RPTSubscription on a.RPTSubscriptionId equals b.Id
                           where b.ClientID == clientid && b.Status != "Deleted"
                           orderby a.Id descending
                           select new RTPJobDetailModel
                           {
                               Id = a.Id,
                               ReportName = b.ReportName,
                               Recipients = b.Recipients,
                               ScheduleType = b.ScheduleType,
                               ReportFor = b.ReportFor,
                               JobScheduledDate = a.ScheduleDateTime,
                               ReportParameters = b.ReportParameters,
                               TelerikReportName = b.TelerikReportName,
                               ReportFormat = b.ReportFormat,
                               ReportDocument = a.ReportDocument,
                               RPTJobUrl = a.RPTJobUrl
                           }).ToList();
                }
                   
                
            }
            return rec;
        }
        public static int ArchiveRptDetails(int Id)
        {
            int aid = 0;
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var jobDetail = unitOfWork.RPTJobDetail.Get(g => g.Id == Id).FirstOrDefault();
                    if (jobDetail != null)
                    {
                        unitOfWork.RPTJobDetail.Delete(Id);
                        var rda = new RPTJobDetailArchive();
                        rda.RPTSubscriptionId = jobDetail.RPTSubscriptionId;
                        rda.ScheduleDateTime = jobDetail.ScheduleDateTime;
                        rda.ReportDocument = jobDetail.ReportDocument;
                        rda.Status = jobDetail.Status;
                        rda.JobStartDateTime = jobDetail.JobStartDateTime;
                        rda.JobEndDateTime = jobDetail.JobEndDateTime;
                        rda.FailureCount = jobDetail.FailureCount;
                        rda.StatusDetail = jobDetail.StatusDetail;
                        rda.AchivedDateTime = DateTime.Now;

                        unitOfWork.RPTJobDetailArchive.Insert(rda);
                        unitOfWork.Save();
                        aid = rda.Id;

                    }
                    return aid;
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public static bool DeleteReportSub(int Id)
        {
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                unitOfWork.RPTSubscription.Delete(Id);
                unitOfWork.Save();

                return true;
            }
        }

        public static int CopyReportSub(int Id)
        {
            int id = 0;
            try

            {
                Db db = new Db();
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var RPT = unitOfWork.RPTSubscription.Get(g => g.Id == Id).FirstOrDefault();
                    var rptObject = new RPTSubscription();
                    rptObject.ClientID = RPT.ClientID;
                    rptObject.EmailSubject = RPT.EmailSubject;
                    rptObject.Recipients = RPT.Recipients;
                    rptObject.ReportFor = RPT.ReportFor;
                    rptObject.StartDate = RPT.StartDate;
                    rptObject.EndDate = RPT.EndDate;
                    rptObject.ScheduleType = RPT.ScheduleType;
                    rptObject.ReportParameters = RPT.ReportParameters;
                    rptObject.ReportFormat = RPT.ReportFormat;
                    rptObject.ReportName = RPT.ReportName;
                    rptObject.TelerikReportName = RPT.TelerikReportName;
                    rptObject.Status = "Draft";
                    rptObject.CreatedBy = RPT.CreatedBy;
                    rptObject.CreatedDate = RPT.CreatedDate;
                    rptObject.UpdatedBy = RPT.CreatedBy;
                    rptObject.UpdatedDate = RPT.CreatedDate;
                    rptObject.ScheduleTime = RPT.ScheduleTime;
                    rptObject.WeekDay = default;
                    rptObject.MonthDay = default;
                    if (RPT.ScheduleType == "Weekly")
                    {
                        rptObject.WeekDay = RPT.WeekDay;
                    }
                    if (RPT.ScheduleType == "Monthly")
                    {
                        rptObject.MonthDay = RPT.MonthDay;
                    }
                    unitOfWork.RPTSubscription.Insert(rptObject);
                    unitOfWork.Save();
                    //db.RTPSubcription.Add(rptObject);
                    //db.SaveChanges();

                    id = rptObject.Id;
                    return id;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public static IEnumerable<MetricModel> GetListOfMetrics(string ServiceName,string ServiceCatName)
        {
            IList<MetricModel> SlaList = new List<MetricModel>();



            DataTable dt = new DataTable();
            var paramName = new string[] { "service", "serviceCategory" };
            var paramValue = new object[] { ServiceName, ServiceCatName };

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                var res = unitOfWork.GetDataFromStoredProcedure("[GetMetricLibrary]", paramName, paramValue);
                foreach (DataRow dr in res.Rows)
                {

                    var coldet = new MetricModel();

                    coldet.Id = Convert.ToInt32(dr["Id"]);
                    if (!(dr["MetricName"] is DBNull))
                        coldet.MetricName = dr["MetricName"].ToString();
                    if (!(dr["Objective"] is DBNull))
                        coldet.Objective = dr["Objective"].ToString();
                    if (!(dr["ObjectiveType"] is DBNull))
                        coldet.ObjectiveType = dr["ObjectiveType"].ToString();
                    if (!(dr["MetricUnit"] is DBNull))
                        coldet.MetricUnit = dr["MetricUnit"].ToString();
                    if (!(dr["Calculation"] is DBNull))
                        coldet.Calculation = dr["Calculation"].ToString();
                    if (!(dr["CalculationDataElement"] is DBNull))
                        coldet.CalculationDataElement = dr["CalculationDataElement"].ToString();
                    if (!(dr["ContextualDataElement"] is DBNull))
                        coldet.ContextualDataElement = dr["ContextualDataElement"].ToString();
                    if (!(dr["inUse"] is DBNull))
                        coldet.inUse = Convert.ToBoolean(dr["inUse"]);
                    if (!(dr["ServiceCategory"] is DBNull))
                        coldet.ServiceCategory = dr["ServiceCategory"].ToString();
                    //coldet.Description = dr["Description"].ToString();
                    if (!(dr["Service"] is DBNull))
                        coldet.Service = dr["Service"].ToString();
                    //coldet.Priority = dr["Priority"].ToString();
                    if (!(dr["ServiceComponent"] is DBNull))
                        coldet.ServiceComponent = dr["ServiceComponent"].ToString();
                    if (!(dr["MetricPerspective"] is DBNull))
                        coldet.MetricPerspective = dr["MetricPerspective"].ToString();
                    if (!(dr["MetricType"] is DBNull))
                        coldet.MetricType = dr["MetricType"].ToString();
                    if (!(dr["ChartIndicator"] is DBNull))
                        coldet.ChartIndicator = dr["ChartIndicator"].ToString();
                    if (!(dr["Rules"] is DBNull))
                        coldet.Rules = dr["Rules"].ToString();
                    if (!(dr["PrimaryAudience"] is DBNull))
                        coldet.PrimaryAudience = dr["PrimaryAudience"].ToString();
                    if (!(dr["CreatedBy"] is DBNull))
                        coldet.CreatedBy = dr["CreatedBy"].ToString();
                    if (!(dr["UpdatedBy"] is DBNull))
                        coldet.UpdatedBy = dr["UpdatedBy"].ToString();
                    if (!(dr["CreatedDate"] is DBNull))
                        coldet.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                    if (!(dr["UpdatedDate"] is DBNull))
                        coldet.UpdatedDate = Convert.ToDateTime(dr["UpdatedDate"]);


                    SlaList.Add(coldet);
                }

            }

            return SlaList;

        }

        public static bool SaveSLAMetric(MetricModel dt,string userName)
        {
            int Id = 0;
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                var mod = new Metric();
                mod.MetricName = dt.MetricName;
                mod.Objective = dt.Objective;
                mod.ObjectiveType = dt.ObjectiveType;
                mod.Calculation = dt.Calculation;
                mod.CalculationDataElement = dt.CalculationDataElement;
                mod.ContextualDataElement = dt.ContextualDataElement;
                mod.inUse = dt.inUse;
                mod.ServiceCategoryId = dt.ServiceCategoryId;
                mod.ServiceId = dt.ServiceId;
                mod.ServiceComponentId = dt.ServiceComponentId;
              
                mod.MetricPerspectiveId = dt.MetricPerspectiveId;
                mod.MetricTypeId = dt.MetricTypeId;
                mod.ChartIndicator = dt.ChartIndicator;
                mod.MetricUnit = dt.MetricUnit;
                mod.Rules = dt.Rules;
                mod.PrimaryAudience = dt.PrimaryAudience;
                mod.ObjectiveTypeId = 274;
                mod.MetricPerspectiveId = 1;
                mod.CalculationDataId = 272;
                mod.CalculationId = 274;
                mod.MetricUnitId = 222;
                mod.ContextId = 282;
                mod.ChartIndicatorId = 283;
                mod.RulesId = 270;
                mod.PrimaryAudienceId = 267;
                mod.DefaultCalculationId = 1;
                mod.CreatedDate = DateTime.Now;
                mod.UpdatedDate = DateTime.Now;
                mod.CreatedBy = userName;
                mod.UpdatedBy = userName;
                //mod.RowVersion = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                if (dt.Id > 0)
                {
                    mod.Id = dt.Id;
                    unitOfWork.Metric.Update(mod);
                }
                else
                {
                    unitOfWork.Metric.Insert(mod);
                }
                unitOfWork.Save();
                Id = mod.Id;
                return true;
            }



        }

    }
}
