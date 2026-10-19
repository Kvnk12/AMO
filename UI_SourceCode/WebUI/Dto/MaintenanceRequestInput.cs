using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.Domain.Service;
using NTTData.AMOPortal.WebUI.Dto;
using NTTData.AMOPortal.WebUI.Mapper;
using NTTData.AMOPortal.Data;
using System.Globalization;
using Foolproof;
using System.ComponentModel;
using PagedList;


namespace NTTData.AMOPortal.WebUI.Dto
{
    public class MaintenanceRequestInput : Input
    {
        [Required]
        public string IncidentId { get; set; }
        [Required]
        public string RecordIncluded { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? AcknowledgedDateTime { get; set; }
        public DateTime? ResolutionDateTime { get; set; }
        public DateTime? ServiceRestoredDateTime { get; set; }
        public DateTime? LastUpdatedDateTime { get; set; }
        public DateTime? UploadDate { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Priority { get; set; }
        public string ImpactScope { get; set; }
        public string Urgency { get; set; }
        public string Type { get; set; }
        public string OutageType { get; set; }
        public string DivisionImpacted { get; set; }
        public string BusinessUnitImpacted { get; set; }
        public string ServiceCategory { get; set; }
        public string ServiceSubcategory { get; set; }
        public string ServiceArea { get; set; }
        public string RegionImpacted { get; set; }
        public string LocationsImpacted { get; set; }
        public string CustomerDept { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerTitle { get; set; }
        public string CustomerSatisfaction { get; set; }
        public string ImpactType { get; set; }
        public string ImpactDescription { get; set; }
        public string ConfigItemID { get; set; }
        public string ConfigType { get; set; }
        public string ConfigItemName { get; set; }
        public string EnvironmentImpacted { get; set; }
        public string Module { get; set; }
        public string NodeName { get; set; }
        public string RootCauseType { get; set; }
        public string RootCauseDescription { get; set; }
        public string ResolutionType { get; set; }
        public string ResolutionDescription { get; set; }
        public string SolutionID { get; set; }
        public string DeploymentRelease { get; set; }
        public string RelatedChangeIDs { get; set; }
        public string RelatedIDs { get; set; }
        public string RelatedProblemIDs { get; set; }
        public string Helpdesk { get; set; }
        public string AssignedToPerson { get; set; }
        public string AssignedToSupportGroup { get; set; }
        public string CreatedByGroup { get; set; }
        public string CreatedByPerson { get; set; }
        public string CreatedTimeZone { get; set; }
        public string CreatedTime { get; set; }
        public string CreatedWeekDay { get; set; }
        public string CreatedWeekNum { get; set; }
        public string CreatedMonth { get; set; }
        public string CreatedYear { get; set; }
        public string AcknowlededByGroup { get; set; }
        public string AcknowlededByPerson { get; set; }
        public string AcknowledgedTimeZone { get; set; }
        public string AcknowledgedTime { get; set; }
        public string AcknowledgedWeekDay { get; set; }
        public string AcknowledgedWeekNum { get; set; }
        public string AcknowledgedMonth { get; set; }
        public string AcknowledgedYear { get; set; }
        [Required]
        public string ResolvedByGroup { get; set; }
        public string ResolvedByPerson { get; set; }
        [Required]
        public string ResolvedByVendor { get; set; }
        public string ResolutionEffortHours { get; set; }
        public string ResolvedTimeZone { get; set; }
        public string ResolutionTime { get; set; }
        public string ResolutionWeekDay { get; set; }
        public string ResolutionWeekNum { get; set; }
        [Required]
        public string ResolutionMonth { get; set; }
        [Required]
        public string ResolutionYear { get; set; }
        public string ServiceRestoredByGroup { get; set; }
        public string ServiceRestoredByPerson { get; set; }
        public string ServiceRestoredTimeZone { get; set; }
        public string ServiceRestoredTime { get; set; }
        public string ServiceRestoredWeekDay { get; set; }
        public string ServiceRestoredWeekNum { get; set; }
        public string ServiceRestoredMonth { get; set; }
        public string ServiceRestoredYear { get; set; }
        public string LastUpdatedByPerson { get; set; }
        public string ModificationNotes { get; set; }
        public string DataSource { get; set; }
        public string DataSourceNotes { get; set; }
        public string ImpectDescription { get; set; }
        public string UploadedBy { get; set; }
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client Clients { get; set; }

        public MetricInput Metrics { get; set; }
        public MetricSpecificationInput MetricSpecifications { get; set; }
        public SLAMetricInput SLAMetrics { get; set; }
        public string SlALable { get; set; }
        public int metricId { get; set; }
        public string TargetValue { get; set; }
        public IEnumerable<string> SelectedPriority { get; set; }
        public IEnumerable<string> SelectedYear { get; set; }
        public IEnumerable<string> SelectedMonth { get; set; }
        public List<ResultLine> GetResult { get; set; }
        [Required]
        public string Waivedindicator { get; set; }
        public string Resolvedindicator { get; set; }

        public string Assignedbygroup { get; set; }
        public string Assignedbyperson { get; set; }
        public DateTime? Assigneddatetime { get; set; }
        public DateTime? Esitmatedresponsetimebygroup { get; set; }
        public DateTime? Esitmatedresponsedatetime { get; set; }
        public DateTime? Esitmatedresolutiondate { get; set; }
        public int? Totalsuspenddurationassignedtoack { get; set; }
        public int? Totalsuspenddurationacktoer { get; set; }
        public int? Totalsuspenddurationertoresolution { get; set; }

        public string ReportName { get; set; }
        public int Datasetid { get; set; }


        public IEnumerable<SelectListItem> Datasetlist
        {
            get
            {

                List<SelectListItem> list = new List<SelectListItem>();
                list.Insert(0, new SelectListItem() { Value = "0", Text = "" });
                list.Insert(0, new SelectListItem() { Value = "1", Text = "Incident Management" });
                list.Insert(0, new SelectListItem() { Value = "2", Text = "Problem Management" });
                list.Insert(0, new SelectListItem() { Value = "3", Text = "Maintenance Request" });
                return new SelectList((IEnumerable<SelectListItem>)list, "Value", "Text");
            }
        }

        public IEnumerable<SelectListItem> RecordIncludedList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.MaintenanceRequest> IncidentC = new Data.Repo<Domain.Entities.MaintenanceRequest>(Context);
                IEnumerable<MaintenanceRequest> PList = IncidentC.GetAll().Where(a => a.ClientId == ClientId).GroupBy(o => o.RecordIncluded).Select(g => g.FirstOrDefault()).ToList();

                var SRCList = (from p in PList
                               select p.RecordIncluded);

                var Items = SRCList.Select((RecordIncluded) => new SelectListItem
                {
                    Text = RecordIncluded.ToString(),
                    Value = RecordIncluded.ToString()
                });
                return Items;
            }
        }

        public IEnumerable<SelectListItem> ResolvedList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.MaintenanceRequest> IncidentC = new Data.Repo<Domain.Entities.MaintenanceRequest>(Context);
                IEnumerable<MaintenanceRequest> PList = IncidentC.GetAll().Where(a => a.ClientId == ClientId).GroupBy(o => o.Resolvedindicator).Select(g => g.FirstOrDefault()).ToList();

                var SRCList = (from p in PList
                               select p.Resolvedindicator);

                var Items = SRCList.Select((Resolvedindicator) => new SelectListItem
                {
                    Text = Resolvedindicator.ToString(),
                    Value = Resolvedindicator.ToString()
                });
                return Items;
            }
        }
        public IEnumerable<SelectListItem> ResolvedyearList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.MaintenanceRequest> IncidentC = new Data.Repo<Domain.Entities.MaintenanceRequest>(Context);
                IEnumerable<MaintenanceRequest> PList = IncidentC.GetAll().Where(a => a.ClientId == ClientId).GroupBy(o => o.ResolutionYear).Select(g => g.FirstOrDefault()).ToList();

                var SRCList = (from p in PList
                               select p.ResolutionYear);

                var Items = SRCList.Select((ResolutionYear) => new SelectListItem
                {
                    Text = ResolutionYear.ToString(),
                    Value = ResolutionYear.ToString()
                });
                return Items;
            }
        }
        public IEnumerable<SelectListItem> ResolvedmonthList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.MaintenanceRequest> IncidentC = new Data.Repo<Domain.Entities.MaintenanceRequest>(Context);
                IEnumerable<MaintenanceRequest> PList = IncidentC.GetAll().Where(a => a.ClientId == ClientId).GroupBy(o => o.ResolutionMonth).Select(g => g.FirstOrDefault()).ToList();

                var SRCList = (from p in PList
                               select p.ResolutionMonth);

                var Items = SRCList.Select((ResolutionMonth) => new SelectListItem
                {
                    Text = ResolutionMonth.ToString(),
                    Value = ResolutionMonth.ToString()
                });
                return Items;
            }
        }
        public IEnumerable<SelectListItem> ResolvedbyVendorList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.MaintenanceRequest> IncidentC = new Data.Repo<Domain.Entities.MaintenanceRequest>(Context);
                IEnumerable<MaintenanceRequest> PList = IncidentC.GetAll().Where(a => a.ClientId == ClientId).GroupBy(o => o.ResolvedByVendor).Select(g => g.FirstOrDefault()).ToList();

                var SRCList = (from p in PList
                               select p.ResolvedByVendor);

                var Items = SRCList.Select((ResolvedByVendor) => new SelectListItem
                {
                    Text = ResolvedByVendor.ToString(),
                    Value = ResolvedByVendor.ToString()
                });
                return Items;
            }
        }
        public IEnumerable<SelectListItem> ResolvedbyGroupList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.MaintenanceRequest> IncidentC = new Data.Repo<Domain.Entities.MaintenanceRequest>(Context);
                IEnumerable<MaintenanceRequest> PList = IncidentC.GetAll().Where(a => a.ClientId == ClientId).GroupBy(o => o.ResolvedByGroup).Select(g => g.FirstOrDefault()).ToList();

                var SRCList = (from p in PList
                               select p.ResolvedByGroup);

                var Items = SRCList.Select((ResolvedByGroup) => new SelectListItem
                {
                    Text = ResolvedByGroup.ToString(),
                    Value = ResolvedByGroup.ToString()
                });
                return Items;
            }
        }
        public IEnumerable<SelectListItem> WaivedList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.MaintenanceRequest> IncidentC = new Data.Repo<Domain.Entities.MaintenanceRequest>(Context);
                IEnumerable<MaintenanceRequest> PList = IncidentC.GetAll().Where(a => a.ClientId == ClientId).GroupBy(o => o.Waivedindicator).Select(g => g.FirstOrDefault()).ToList();

                var SRCList = (from p in PList
                               select p.Waivedindicator);

                var Items = SRCList.Select((Waivedindicator) => new SelectListItem
                {
                    Text = Waivedindicator.ToString(),
                    Value = Waivedindicator.ToString()
                });
                return Items;
            }
        }




        public IPagedList<MaintenanceRequest> GetPagedList(int pageNumber = 1)
        {

            var mrlist = MaintenanceRequestLists;

            int noOfRows = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DataTableNoOfRowsDisplay"].ToString());

            var list = mrlist.ToPagedList(pageNumber, noOfRows);

            return list;

        }

        //public IQueryable<MaintenanceRequest> iqList
        //    {
        //    get
        //        {
        //        DbContextFactory Context = new DbContextFactory();
        //        Repo<NTTData.AMOPortal.Domain.Entities.MaintenanceRequest> IncList = new Repo<MaintenanceRequest>(Context);
        //        IQueryable<NTTData.AMOPortal.Domain.Entities.MaintenanceRequest> e_IncList = IncList.GetAll();

        //        var incList = from p in e_IncList
        //                      where p.ClientId == ClientId && Id < 136
        //                      select p;

        //        return incList;
        //        }
        //    }


        public List<MaintenanceRequest> MaintenanceRequestLists
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.MaintenanceRequest> IncList = new Repo<MaintenanceRequest>(Context);
                List<NTTData.AMOPortal.Domain.Entities.MaintenanceRequest> e_IncList = IncList.GetAll().ToList();

                var incList = from p in e_IncList
                              where p.ClientId == ClientId
                              select p;

                return incList.ToList();
            }
        }


        public IEnumerable<SelectListItem> ClientsList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Client> ClientLists = new Repo<Client>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Client> e_ClientList = ClientLists.GetAll().ToList();

                var Items = e_ClientList.Select(ServiceL => new SelectListItem
                {
                    Value = ServiceL.Id.ToString(),
                    Text = ServiceL.ClientName.ToString()

                }).ToList();

                Items.Insert(0, new SelectListItem { Text = "-Select-", Value = "" });
                return Items;
            }
        }
        public List<NTTData.AMOPortal.Domain.Entities.Metric> MetricsList
        {

            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> Metrics = new Repo<Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> list = Metrics.GetAll().ToList();

                return list;

            }

        }
        public IEnumerable<SelectListItem> MetricAvailIdList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Metric> MetricLists = new Repo<NTTData.AMOPortal.Domain.Entities.Metric>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Metric> e_MetricList = MetricLists.GetAll().ToList();

                var ServiceSet = from metricSpec in e_MetricList
                                 where metricSpec.ServiceId == 3
                                 select metricSpec;

                List<NTTData.AMOPortal.Domain.Entities.Metric> metricService = ServiceSet.ToList();

                var Items = metricService.Select(ServiceL => new SelectListItem
                {
                    Text = ServiceL.MetricName.ToString(),
                    Value = ServiceL.Id.ToString()
                });
                return Items;
            }
        }
        public List<NTTData.AMOPortal.Domain.Entities.MetricSpecification> MetricsSpecList
        {

            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.MetricSpecification> MetricsSpc = new Repo<MetricSpecification>(Context);
                List<NTTData.AMOPortal.Domain.Entities.MetricSpecification> list = MetricsSpc.GetAll().ToList();

                return list;

            }

        }




        public IEnumerable<SelectListItem> GetPriority
        {
            get
            {
                Repo<NTTData.AMOPortal.Domain.Entities.MaintenanceRequest> MaintenanceRequest = GetMaintenanceRequestEntity.GetInstance();


                IEnumerable<MaintenanceRequest> PList = MaintenanceRequest.Where(c => c.ClientId == ClientId).GroupBy(o => o.Priority).Select(g => g.FirstOrDefault());

                var list = (from p in PList
                            select p.Priority);

                var Items = list.Select((priorityName) => new SelectListItem
                {
                    Text = priorityName,
                    Value = priorityName
                }).ToList();
                Items.Insert(0, new SelectListItem { Text = "All", Value = "" });

                return Items;
            }
        }

        public IEnumerable<SelectListItem> GetYear
        {
            get
            {

                Repo<NTTData.AMOPortal.Domain.Entities.MaintenanceRequest> MaintenanceRequest = GetMaintenanceRequestEntity.GetInstance();

                IEnumerable<MaintenanceRequest> PList = MaintenanceRequest.Where(c => c.ClientId == ClientId).GroupBy(o => o.CreatedYear).Select(g => g.FirstOrDefault());

                var list = (from p in PList
                            select p.CreatedYear);


                var Items = list.Select((year) => new SelectListItem
                {
                    Text = year,
                    Value = year
                }).ToList();
                Items.Insert(0, new SelectListItem { Text = "All", Value = "" });
                return Items;
            }
        }

        public IEnumerable<SelectListItem> GetMonth
        {
            get
            {

                Repo<NTTData.AMOPortal.Domain.Entities.MaintenanceRequest> MaintenanceRequest = GetMaintenanceRequestEntity.GetInstance();
                IEnumerable<MaintenanceRequest> PList = MaintenanceRequest.Where(c => c.ClientId == ClientId).GroupBy(o => o.CreatedMonth).Select(g => g.FirstOrDefault());

                var list = (from p in PList
                            select p.CreatedMonth);

                var Items = list.Select((month) => new SelectListItem
                {
                    Text = month,
                    Value = month
                }).ToList();
                Items.Insert(0, new SelectListItem { Text = "All", Value = "" });

                return Items;
            }
        }



        public IEnumerable<SelectListItem> GetImpactType
        {
            get
            {
                Repo<NTTData.AMOPortal.Domain.Entities.MaintenanceRequest> MaintenanceRequest = GetMaintenanceRequestEntity.GetInstance();


                IEnumerable<MaintenanceRequest> PList = MaintenanceRequest.GetAll().GroupBy(o => o.ImpactType).Select(g => g.FirstOrDefault());

                var list = (from p in PList
                            select p.ImpactType);

                //var list = (from p in MList
                //            where p.ImpactType != null
                //            select p.ImpactType)
                //                               .Distinct().ToList();
                var Items = list.Select((impactType) => new SelectListItem
                {
                    Text = impactType,
                    Value = impactType
                });
                return Items;
            }
        }

        public IEnumerable<SelectListItem> GetOutageType
        {
            get
            {
                Repo<NTTData.AMOPortal.Domain.Entities.MaintenanceRequest> MaintenanceRequest = GetMaintenanceRequestEntity.GetInstance();


                IEnumerable<MaintenanceRequest> PList = MaintenanceRequest.GetAll().GroupBy(o => o.OutageType).Select(g => g.FirstOrDefault());

                var list = (from p in PList
                            select p.OutageType);

                var Items = list.Select((outageType) => new SelectListItem
                {
                    Text = outageType,
                    Value = outageType
                });
                return Items;
            }
        }

        public IEnumerable<SelectListItem> GetEnvironmentImpected
        {
            get
            {
                Repo<NTTData.AMOPortal.Domain.Entities.MaintenanceRequest> MaintenanceRequest = GetMaintenanceRequestEntity.GetInstance();

                IEnumerable<MaintenanceRequest> PList = MaintenanceRequest.GetAll().GroupBy(o => o.EnvironmentImpacted).Select(g => g.FirstOrDefault());

                var list = (from p in PList
                            select p.EnvironmentImpacted);



                var Items = list.Select((environmentImpacted) => new SelectListItem
                {
                    Text = environmentImpacted,
                    Value = environmentImpacted
                });
                return Items;
            }
        }

        public IEnumerable<SelectListItem> GetResolutionType
        {
            get
            {

                Repo<NTTData.AMOPortal.Domain.Entities.MaintenanceRequest> MaintenanceRequest = GetMaintenanceRequestEntity.GetInstance();
                IEnumerable<MaintenanceRequest> PList = MaintenanceRequest.GetAll().GroupBy(o => o.ResolutionType).Select(g => g.FirstOrDefault());

                var list = (from p in PList
                            select p.ResolutionType);

                var Items = list.Select((resolutionType) => new SelectListItem
                {
                    Text = resolutionType,
                    Value = resolutionType
                });
                return Items;
            }
        }
    }


    public static class GetMaintenanceRequestEntity
    {
        private static Repo<MaintenanceRequest> _repoIncidentEntity = null;

        static GetMaintenanceRequestEntity()
        {
        }

        public static Repo<MaintenanceRequest> GetInstance()
        {
            if (_repoIncidentEntity == null)
            {
                DbContextFactory db = new DbContextFactory();
                _repoIncidentEntity = new Repo<MaintenanceRequest>(db);

            }
            return _repoIncidentEntity;
        }
    }
}