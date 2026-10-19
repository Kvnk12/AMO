using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
using PagedList;

namespace NTTData.AMOPortal.WebUI.Dto
{
    public class SLAMDashboardInput : Input
    {
        [Required]
        public int ClientId { get; set; }
        public string IncludeRecord { get; set; }
        [Required]
        public string ServiceRequestId { get; set; }
        public string ServiceRequestCategory { get; set; }
        public string ServiceRequestType { get; set; }
        public string ServiceRequestStatus { get; set; }
        public string ServiceRequestStage { get; set; }
        public string Urgency { get; set; }
        public string ImpactScope { get; set; }
        public string Priority { get; set; }
        public string ServiceRequestTitle { get; set; }
        public string ServiceRequestDescription { get; set; }
        public string Benefit { get; set; }
        public int? AnnualBenefitAmount { get; set; }
        public string Impactofnotdoing { get; set; }
        public string Manualworkaroundavailable { get; set; }
        public string RevenueImpact { get; set; }
        public string RevenueImpactSize { get; set; }
        public string RegualtoryImpact { get; set; }
        public string RegualtoryImpactSize { get; set; }
        public string PeopleAffectedCount { get; set; }
        public string DivisionImpacted { get; set; }
        public string BusinessUnitImpacted { get; set; }
        public string ServiceCategory { get; set; }
        public string ServiceSubcategory { get; set; }
        public string ServiceArea { get; set; }
        public string RegionsImpacted { get; set; }
        public string LocationsImpacted { get; set; }
        public string RelatedProjectId { get; set; }
        public string RelatedIncIdentId { get; set; }
        public string RelatedProblemId { get; set; }
        public string RelatedServiceRequestId { get; set; }
        public string RelatedAssetId { get; set; }
        public string EnvironmentImpact { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public string CreatedByGroup { get; set; }
        public string AssignedToSupportGroup { get; set; }
        public DateTime? RequestedCompletionDate { get; set; }
        public DateTime? ExpectedCompletionDate { get; set; }
        public string MandatedCompletionDate { get; set; }
        [Required]
        public string RequestedByGroup { get; set; }
        public string RequestedForGroup { get; set; }
        public DateTime? AcknowledgedDateTime { get; set; }
        public string AcknowlededByGroup { get; set; }
        public DateTime? ROMEstimateActualCompletionDate { get; set; }
        public string ServiceRequestEstimateSize { get; set; }
        public int? ServiceRequestEstimatedHour { get; set; }
        public int? EACActualHours { get; set; }
        public DateTime? EstimatedStartDate { get; set; }
        public DateTime? EstimatedEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public DateTime? ResolutionDateTime { get; set; }
        public string ResolvedByGroup { get; set; }
        public string RootCauseType { get; set; }
        public string RootCauseDescription { get; set; }
        public string ResolutionType { get; set; }
        public string ResolutionDescription { get; set; }
        public string SolutionId { get; set; }
        public string DeploymentReleaseName { get; set; }
        public string RelatedChangeId { get; set; }
        public string RelatedErrorCode { get; set; }
        [Required]
        public string PrimarySystemAffected { get; set; }
        public string ConfigItemId { get; set; }
        public string ConfigItemName { get; set; }
        public string ConfigType { get; set; }
        public int? DefectReleasedtoProductionCount { get; set; }
        public DateTime? WarrantyEndDate { get; set; }
        public string CustomerSatisfaction { get; set; }
        public DateTime? ClosedDateTime { get; set; }
        public string ClosedByGroup { get; set; }
        public string DataSource { get; set; }
        public DateTime? UploadDate { get; set; }
        public string UploadedBy { get; set; }
        public string ModificationNotes { get; set; }
        public int? ECD_ACDYear { get; set; }
        public int? ECD_ACDMonth { get; set; }


        public IPagedList<ServiceReqeustDataSet> GetPagedList(int pageNumber = 1)
        {

            var mrlist = SRLists;

            int noOfRows = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DataTableNoOfRowsDisplay"].ToString());

            var list = mrlist.ToPagedList(pageNumber, noOfRows);

            return list;

        }


        public IEnumerable<SelectListItem> ClientsList
        {
            get
            {


                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Client> Clients = new Data.Repo<Domain.Entities.Client>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Client> ClientsList = Clients.GetAll().OrderBy(m => m.ClientName).ToList();
                var Items = ClientsList.Select(CList => new SelectListItem
                {
                    Value = CList.Id.ToString(),
                    Text = CList.ClientName.ToString()
                });
                return Items;
            }
        }

        public List<ServiceReqeustDataSet> SRLists
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> IncList = new Data.Repo<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet>(Context);
                List<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> e_IncList = IncList.GetAll().ToList();

                var SRList = from p in e_IncList
                             where p.ClientId == ClientId
                             select p;

                return SRList.ToList();
            }
        }

        //public IEnumerable<SelectListItem> SRFilterName
        //{
        //    get
        //    {


        //        DbContextFactory Context = new DbContextFactory();
        //        Repo<NTTData.AMOPortal.Domain.Entities.SRFilters> SRFs = new Data.Repo<Domain.Entities.SRFilters>(Context);
        //        List<NTTData.AMOPortal.Domain.Entities.SRFilters> SRFsList = SRFs.GetAll().ToList();
        //        var Items = SRFsList.Select(CList => new SelectListItem
        //        {
        //            Value = CList.Id.ToString(),
        //            Text = CList.Name.ToString()
        //        });
        //        return Items;
        //    }
        //}

        public IEnumerable<SelectListItem> IncludeRecordList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> ServiceRequestC = new Data.Repo<Domain.Entities.ServiceReqeustDataSet>(Context);
                IEnumerable<ServiceReqeustDataSet> PList = ServiceRequestC.GetAll().Where(a => a.ClientId == ClientId).GroupBy(o => o.IncludeRecord).Select(g => g.FirstOrDefault()).ToList();

                var SRCList = (from p in PList
                               select p.IncludeRecord);

                var Items = SRCList.Select((IncludeRecord) => new SelectListItem
                {
                    Text = IncludeRecord.ToString(),
                    Value = IncludeRecord.ToString()
                });
                return Items;
            }
        }

        public IEnumerable<SelectListItem> ECD_ACDYearList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> ServiceRequestC = new Data.Repo<Domain.Entities.ServiceReqeustDataSet>(Context);
                IEnumerable<ServiceReqeustDataSet> PList = ServiceRequestC.GetAll().Where(a => a.ClientId == ClientId).GroupBy(o => o.ECD_ACDYear).Select(g => g.FirstOrDefault()).ToList();

                var SRCList = (from p in PList
                               select p.ECD_ACDYear);

                var Items = SRCList.Select((ECD_ACDYear) => new SelectListItem
                {
                    Text = ECD_ACDYear.ToString(),
                    Value = ECD_ACDYear.ToString()
                });
                return Items;
            }
        }
        public IEnumerable<SelectListItem> ECD_ACDMonthList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> ServiceRequestC = new Data.Repo<Domain.Entities.ServiceReqeustDataSet>(Context);
                IEnumerable<ServiceReqeustDataSet> PList = ServiceRequestC.GetAll().Where(a => a.ClientId == ClientId).GroupBy(o => o.ECD_ACDMonth).Select(g => g.FirstOrDefault()).ToList();

                var SRCList = (from p in PList
                               select p.ECD_ACDMonth);

                var Items = SRCList.Select((ECD_ACDMonth) => new SelectListItem
                {
                    Text = ECD_ACDMonth.ToString(),
                    Value = ECD_ACDMonth.ToString()
                });
                return Items;
            }
        }

        public IEnumerable<SelectListItem> ServiceRequestCategoryList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> ServiceRequestC = new Data.Repo<Domain.Entities.ServiceReqeustDataSet>(Context);
                IEnumerable<ServiceReqeustDataSet> PList = ServiceRequestC.GetAll().Where(a => a.ClientId == ClientId).GroupBy(o => o.ServiceRequestCategory).Select(g => g.FirstOrDefault()).ToList();

                var SRCList = (from p in PList
                               select p.ServiceRequestCategory);

                var Items = SRCList.Select((ServiceRequestCategory) => new SelectListItem
                {
                    Text = ServiceRequestCategory.ToString(),
                    Value = ServiceRequestCategory.ToString()
                });
                return Items;
            }
        }

        public IEnumerable<SelectListItem> PriorityList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> PriorityL = new Data.Repo<Domain.Entities.ServiceReqeustDataSet>(Context);
                IEnumerable<ServiceReqeustDataSet> PList = PriorityL.GetAll().Where(a => a.ClientId == ClientId).GroupBy(o => o.Priority).Select(g => g.FirstOrDefault()).ToList();

                var SRCList = (from p in PList
                               select p.Priority);

                var Items = SRCList.Select((Priority) => new SelectListItem
                {
                    Text = Priority.ToString(),
                    Value = Priority.ToString()
                });
                return Items;
            }

        }

        public IEnumerable<SelectListItem> DivisionImpList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> DivisionImpL = new Data.Repo<Domain.Entities.ServiceReqeustDataSet>(Context);
                IEnumerable<ServiceReqeustDataSet> PList = DivisionImpL.GetAll().Where(a => a.ClientId == ClientId).GroupBy(o => o.DivisionImpacted).Select(g => g.FirstOrDefault()).ToList();

                var SRCList = (from p in PList
                               select p.DivisionImpacted);

                var Items = SRCList.Select((DivisionImpacted) => new SelectListItem
                {
                    Text = DivisionImpacted.ToString(),
                    Value = DivisionImpacted.ToString()
                });
                return Items;
            }

        }

        public IEnumerable<SelectListItem> BUImpactList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> BUImpactL = new Data.Repo<Domain.Entities.ServiceReqeustDataSet>(Context);
                IEnumerable<ServiceReqeustDataSet> PList = BUImpactL.GetAll().Where(a => a.ClientId == ClientId).GroupBy(o => o.BusinessUnitImpacted).Select(g => g.FirstOrDefault()).ToList();

                var SRCList = (from p in PList
                               select p.BusinessUnitImpacted);

                var Items = SRCList.Select((BusinessUnitImpacted) => new SelectListItem
                {
                    Text = BusinessUnitImpacted.ToString(),
                    Value = BusinessUnitImpacted.ToString()
                });
                return Items;
            }

        }

        public IEnumerable<SelectListItem> PYSystemAffList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> PYSystemAffL = new Data.Repo<Domain.Entities.ServiceReqeustDataSet>(Context);
                IEnumerable<ServiceReqeustDataSet> PList = PYSystemAffL.GetAll().Where(a => a.ClientId == ClientId).GroupBy(o => o.PrimarySystemAffected).Select(g => g.FirstOrDefault()).ToList();

                var SRCList = (from p in PList
                               select p.PrimarySystemAffected);

                var Items = SRCList.Select((PrimarySystemAffected) => new SelectListItem
                {
                    Text = PrimarySystemAffected.ToString(),
                    Value = PrimarySystemAffected.ToString()
                });
                return Items;
            }

        }

        public IEnumerable<SelectListItem> RegImpactList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> RegImpactL = new Data.Repo<Domain.Entities.ServiceReqeustDataSet>(Context);
                IEnumerable<ServiceReqeustDataSet> PList = RegImpactL.GetAll().Where(a => a.ClientId == ClientId).GroupBy(o => o.RegionsImpacted).Select(g => g.FirstOrDefault()).ToList();

                var SRCList = (from p in PList
                               select p.RegionsImpacted);

                var Items = SRCList.Select((RegionsImpacted) => new SelectListItem
                {
                    Text = RegionsImpacted.ToString(),
                    Value = RegionsImpacted.ToString()
                });
                return Items;
            }

        }

        public IEnumerable<SelectListItem> LocImpactList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> LocImpactL = new Data.Repo<Domain.Entities.ServiceReqeustDataSet>(Context);
                IEnumerable<ServiceReqeustDataSet> PList = LocImpactL.GetAll().Where(a => a.ClientId == ClientId).GroupBy(o => o.LocationsImpacted).Select(g => g.FirstOrDefault()).ToList();

                var SRCList = (from p in PList
                               select p.LocationsImpacted);

                var Items = SRCList.Select((LocationsImpacted) => new SelectListItem
                {
                    Text = LocationsImpacted.ToString(),
                    Value = LocationsImpacted.ToString()
                });
                return Items;
            }

        }

        public IEnumerable<SelectListItem> ServiceAreaList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> ServiceAreaL = new Data.Repo<Domain.Entities.ServiceReqeustDataSet>(Context);
                IEnumerable<ServiceReqeustDataSet> PList = ServiceAreaL.GetAll().Where(a => a.ClientId == ClientId).GroupBy(o => o.ServiceArea).Select(g => g.FirstOrDefault()).ToList();

                var SRCList = (from p in PList
                               select p.ServiceArea);

                var Items = SRCList.Select((ServiceArea) => new SelectListItem
                {
                    Text = ServiceArea.ToString(),
                    Value = ServiceArea.ToString()
                });
                return Items;
            }

        }

        public IEnumerable<SelectListItem> ServiceCatList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> ServiceCatL = new Data.Repo<Domain.Entities.ServiceReqeustDataSet>(Context);
                IEnumerable<ServiceReqeustDataSet> PList = ServiceCatL.GetAll().Where(a => a.ClientId == ClientId).GroupBy(o => o.ServiceCategory).Select(g => g.FirstOrDefault()).ToList();

                var SRCList = (from p in PList
                               select p.ServiceCategory);

                var Items = SRCList.Select((ServiceCategory) => new SelectListItem
                {
                    Text = ServiceCategory.ToString(),
                    Value = ServiceCategory.ToString()
                });
                return Items;
            }

        }

        public IEnumerable<SelectListItem> ServiceSubCatList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> ServiceSubCatL = new Data.Repo<Domain.Entities.ServiceReqeustDataSet>(Context);
                IEnumerable<ServiceReqeustDataSet> PList = ServiceSubCatL.GetAll().Where(a => a.ClientId == ClientId).GroupBy(o => o.ServiceSubcategory).Select(g => g.FirstOrDefault()).ToList();

                var SRCList = (from p in PList
                               select p.ServiceSubcategory);

                var Items = SRCList.Select((ServiceSubcategory) => new SelectListItem
                {
                    Text = ServiceSubcategory.ToString(),
                    Value = ServiceSubcategory.ToString()
                });
                return Items;
            }
        }

        //public SLAMDUpRows UPMetricList
        //{
        //    get
        //    {
        //        DbContextFactory Context = new DbContextFactory();
        //        Repo<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> UPMetric = new Data.Repo<Domain.Entities.ServiceReqeustDataSet>(Context);
        //        List<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> UPMetLists = UPMetric.GetAll().Where(a => a.ClientId == ClientId).ToList();

        //        if (IncludeRecord != null)
        //        {
        //            List<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> UPMetInclude = UPMetLists.Where(a => a.IncludeRecord == IncludeRecord).ToList();
        //            UPMetLists = UPMetInclude;
        //        }
        //        if (ECD_ACDYear != null)
        //        {
        //            List<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> UPMetInclude = UPMetLists.Where(a => a.ECD_ACDYear == ECD_ACDYear).ToList();
        //            UPMetLists = UPMetInclude;
        //        }
        //        if (ECD_ACDMonth != null)
        //        {
        //            List<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> UPMetInclude = UPMetLists.Where(a => a.ECD_ACDMonth == ECD_ACDMonth).ToList();
        //            UPMetLists = UPMetInclude;
        //        }
        //        if (ServiceRequestCategory != null)
        //        {
        //            List<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> UPMetInclude = UPMetLists.Where(a => a.ServiceRequestCategory == ServiceRequestCategory).ToList();
        //            UPMetLists = UPMetInclude;
        //        }
        //        if (Priority != null)
        //        {
        //            List<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> UPMetInclude = UPMetLists.Where(a => a.Priority == Priority).ToList();
        //            UPMetLists = UPMetInclude;
        //        }
        //        if (DivisionImpacted != null)
        //        {
        //            List<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> UPMetInclude = UPMetLists.Where(a => a.DivisionImpacted == DivisionImpacted).ToList();
        //            UPMetLists = UPMetInclude;
        //        }
        //        if (BusinessUnitImpacted != null)
        //        {
        //            List<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> UPMetInclude = UPMetLists.Where(a => a.BusinessUnitImpacted == BusinessUnitImpacted).ToList();
        //            UPMetLists = UPMetInclude;
        //        }
        //        if (PrimarySystemAffected != null)
        //        {
        //            List<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> UPMetInclude = UPMetLists.Where(a => a.PrimarySystemAffected == PrimarySystemAffected).ToList();
        //            UPMetLists = UPMetInclude;
        //        }
        //        if (RegionsImpacted != null)
        //        {
        //            List<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> UPMetInclude = UPMetLists.Where(a => a.RegionsImpacted == RegionsImpacted).ToList();
        //            UPMetLists = UPMetInclude;
        //        }
        //        if (LocationsImpacted != null)
        //        {
        //            List<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> UPMetInclude = UPMetLists.Where(a => a.LocationsImpacted == LocationsImpacted).ToList();
        //            UPMetLists = UPMetInclude;
        //        }
        //        if (ServiceArea != null)
        //        {
        //            List<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> UPMetInclude = UPMetLists.Where(a => a.ServiceArea == ServiceArea).ToList();
        //            UPMetLists = UPMetInclude;
        //        }
        //        if (ServiceCategory != null)
        //        {
        //            List<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> UPMetInclude = UPMetLists.Where(a => a.ServiceCategory == ServiceCategory).ToList();
        //            UPMetLists = UPMetInclude;
        //        }
        //        if (ServiceSubcategory != null)
        //        {
        //            List<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> UPMetInclude = UPMetLists.Where(a => a.ServiceSubcategory == ServiceSubcategory).ToList();
        //            UPMetLists = UPMetInclude;
        //        }

        //        if (!UPMetLists.Count().Equals(0))
        //        {
        //            var UPMet11 = from p in UPMetLists
        //                          group p by p.AssignedToSupportGroup into g
        //                          select g;

        //            var UPMet = from p in UPMetLists
        //                        group p by p.ClientId into g
        //                        select new SLAMDUpRows
        //                        (
        //                            g.Average(a => ((a.ROMEstimateActualCompletionDate - a.AcknowledgedDateTime).TotalDays)),
        //                            (g.Where(a => a.ServiceRequestStatus.Contains("Open")).Average(a => ((a.EstimatedEndDate - a.ExpectedCompletionDate).TotalDays)) + g.Where(a => a.ServiceRequestStatus.Contains("Open")).Average(a => ((a.EstimatedEndDate - a.ExpectedCompletionDate).TotalDays))) / 2,
        //                            //g.Average(a => ((a.EstimatedEndDate - a.ExpectedCompletionDate).TotalDays)),
        //                            g.Average(a => a.EACActualHours - a.ServiceRequestEstimatedHour),
        //                            g.Average(a => a.DefectReleasedtoProductionCount)
        //                        );
        //            return UPMet.FirstOrDefault();
        //        }
        //        else
        //        {
        //            return new SLAMDUpRows(0.0, 0.0, 0.0, 0.0);
        //        }
        //    }
        //}


        //public SLAMDUpRows UPRowTarget
        //{
        //    get
        //    {
        //        DbContextFactory Context = new DbContextFactory();
        //        Repo<NTTData.AMOPortal.Domain.Entities.MetricSpecification> UPTarget = new Data.Repo<Domain.Entities.MetricSpecification>(Context);
        //        List<NTTData.AMOPortal.Domain.Entities.MetricSpecification> UPTargetLists = UPTarget.GetAll().Where(a => a.ClientId == ClientId).ToList();
        //        var nRom = (from p in UPTargetLists
        //                    where p.MetricId == 98
        //                    select p).Select(a => a.MetricTarget).FirstOrDefault();
        //        var nVar = (from p in UPTargetLists
        //                    where p.MetricId == 99
        //                    select p).Select(a => a.MetricTarget).FirstOrDefault();
        //        var nEff = (from p in UPTargetLists
        //                    where p.MetricId == 100
        //                    select p).Select(a => a.MetricTarget).FirstOrDefault();
        //        var nDef = (from p in UPTargetLists
        //                    where p.MetricId == 102
        //                    select p).Select(a => a.MetricTarget).FirstOrDefault();
        //        return new SLAMDUpRows(nRom, nVar, nEff, nDef);
        //    }

        //}

        public SLAMDUpRows UPRowTarget
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.MetricSpecification> UPTarget = new Data.Repo<Domain.Entities.MetricSpecification>(Context);
                List<NTTData.AMOPortal.Domain.Entities.MetricSpecification> UPTargetLists = UPTarget.GetAll().Where(a => a.ClientId == ClientId).ToList();
                var nRom = (from p in UPTargetLists
                            where p.MetricId == 30
                            select p).Select(a => a.MetricTarget).FirstOrDefault();
                var nVar = (from p in UPTargetLists
                            where p.MetricId == 31
                            select p).Select(a => a.MetricTarget).FirstOrDefault();
                var nEff = (from p in UPTargetLists
                            where p.MetricId == 32
                            select p).Select(a => a.MetricTarget).FirstOrDefault();
                var nDef = (from p in UPTargetLists
                            where p.MetricId == 33
                            select p).Select(a => a.MetricTarget).FirstOrDefault();
                return new SLAMDUpRows(nRom, nVar, nEff, nDef);
            }

        }


        //public List<SLAMDLowRows> SLALowerPanel
        //{
        //    get
        //    {
        //        DbContextFactory Context = new DbContextFactory();
        //        Repo<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> UPMetric = new Data.Repo<Domain.Entities.ServiceReqeustDataSet>(Context);
        //        List<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> UPMetLists = UPMetric.GetAll().Where(a => a.ClientId == ClientId).ToList();

        //        if (IncludeRecord != null)
        //        {
        //            List<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> UPMetInclude = UPMetLists.Where(a => a.IncludeRecord == IncludeRecord).ToList();
        //            UPMetLists = UPMetInclude;
        //        }
        //        if (ECD_ACDYear != null)
        //        {
        //            List<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> UPMetInclude = UPMetLists.Where(a => a.ECD_ACDYear == ECD_ACDYear).ToList();
        //            UPMetLists = UPMetInclude;
        //        }
        //        if (ECD_ACDMonth != null)
        //        {
        //            List<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> UPMetInclude = UPMetLists.Where(a => a.ECD_ACDMonth == ECD_ACDMonth).ToList();
        //            UPMetLists = UPMetInclude;
        //        }
        //        if (ServiceRequestCategory != null)
        //        {
        //            List<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> UPMetInclude = UPMetLists.Where(a => a.ServiceRequestCategory == ServiceRequestCategory).ToList();
        //            UPMetLists = UPMetInclude;
        //        }
        //        if (Priority != null)
        //        {
        //            List<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> UPMetInclude = UPMetLists.Where(a => a.Priority == Priority).ToList();
        //            UPMetLists = UPMetInclude;
        //        }
        //        if (DivisionImpacted != null)
        //        {
        //            List<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> UPMetInclude = UPMetLists.Where(a => a.DivisionImpacted == DivisionImpacted).ToList();
        //            UPMetLists = UPMetInclude;
        //        }
        //        if (BusinessUnitImpacted != null)
        //        {
        //            List<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> UPMetInclude = UPMetLists.Where(a => a.BusinessUnitImpacted == BusinessUnitImpacted).ToList();
        //            UPMetLists = UPMetInclude;
        //        }
        //        if (PrimarySystemAffected != null)
        //        {
        //            List<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> UPMetInclude = UPMetLists.Where(a => a.PrimarySystemAffected == PrimarySystemAffected).ToList();
        //            UPMetLists = UPMetInclude;
        //        }
        //        if (RegionsImpacted != null)
        //        {
        //            List<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> UPMetInclude = UPMetLists.Where(a => a.RegionsImpacted == RegionsImpacted).ToList();
        //            UPMetLists = UPMetInclude;
        //        }
        //        if (LocationsImpacted != null)
        //        {
        //            List<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> UPMetInclude = UPMetLists.Where(a => a.LocationsImpacted == LocationsImpacted).ToList();
        //            UPMetLists = UPMetInclude;
        //        }
        //        if (ServiceArea != null)
        //        {
        //            List<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> UPMetInclude = UPMetLists.Where(a => a.ServiceArea == ServiceArea).ToList();
        //            UPMetLists = UPMetInclude;
        //        }
        //        if (ServiceCategory != null)
        //        {
        //            List<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> UPMetInclude = UPMetLists.Where(a => a.ServiceCategory == ServiceCategory).ToList();
        //            UPMetLists = UPMetInclude;
        //        }
        //        if (ServiceSubcategory != null)
        //        {
        //            List<NTTData.AMOPortal.Domain.Entities.ServiceReqeustDataSet> UPMetInclude = UPMetLists.Where(a => a.ServiceSubcategory == ServiceSubcategory).ToList();
        //            UPMetLists = UPMetInclude;
        //        }

        //        var UPMet1 = from p in UPMetLists
        //                     group p by p.AssignedToSupportGroup into g
        //                     select g;

        //        var UPMet = from p in UPMetLists
        //                    group p by p.AssignedToSupportGroup into g
        //                    select new SLAMDLowRows(
        //                        g.Key.ToString(),
        //                        g.Select(a => a.AssignedToSupportGroup).Count(),
        //                        g.Average(a => ((a.ROMEstimateActualCompletionDate - a.AcknowledgedDateTime).TotalDays)),
        //                        (g.Where(a => a.ServiceRequestStatus.Contains("Open")).Average(a => ((a.EstimatedEndDate - a.ExpectedCompletionDate).TotalDays)) + g.Where(a => a.ServiceRequestStatus.Contains("Open")).Average(a => ((a.EstimatedEndDate - a.ExpectedCompletionDate).TotalDays))) / 2,
        //                        //g.Average(a => ((a.EstimatedEndDate - a.ExpectedCompletionDate).TotalDays)),
        //                        g.Average(a => a.EACActualHours - a.ServiceRequestEstimatedHour),
        //                        g.Average(a => a.DefectReleasedtoProductionCount)
        //                        );
        //        return UPMet.ToList();

        //    }

        //}

        public SRFilters srFilters { get; set; }
    }


    public class SLAMDUpRows
    {
        public SLAMDUpRows(double ndaysROM, double ndaysVar, double ndaysEff, double ndaysDef)
        {
            nRom = ndaysROM;
            nVar = ndaysVar;
            nEff = ndaysEff;
            nDef = ndaysDef;
        }

        public double nRom { get; set; }
        public double nVar { get; set; }
        public double nEff { get; set; }
        public double nDef { get; set; }
    }

    public class SLAMDLowRows
    {
        public SLAMDLowRows(string Assignedtogroup, int count, double ndaysROM, double ndaysVar, double ndaysEff, double ndaysDef)
        {
            AssignGroup = Assignedtogroup;
            RCount = count;
            nRom = ndaysROM;
            nVar = ndaysVar;
            nEff = ndaysEff;
            nDef = ndaysDef;
        }
        public string AssignGroup { get; set; }
        public int RCount { get; set; }
        public double nRom { get; set; }
        public double nVar { get; set; }
        public double nEff { get; set; }
        public double nDef { get; set; }
    }
}

