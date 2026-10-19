using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTTData.AMOPortal.Domain.Entities;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using Domain.Entities.ClientProfileManager;
using Domain.Entities.IncidentNavigator;
using Domain.Entities.PortfolioAnalyzer;
using Domain.Entities.Survey;
using Domain.Entities;
using Domain.Entities.Report;
using Domain.Entities.RetirementAccelerator;
using Domain.Entities.FDBKManager;
using Domain.Entities.A3MM;

namespace NTTData.AMOPortal.Data
{
    public class Db : DbContext
    {
        public Db()
        {
            Database.SetInitializer<Db>(null);
        }

       
        public DbSet<Tables> Tbles { get; set; }

        public DbSet<Columns> Column { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        //Added by Ram for UserApplication access feb17th
        public DbSet<UserApplications> UserApplications { get; set; }
        public DbSet<ServiceReqeustDataSet> ServiceReqeustDataSet { get; set; }

        //Added by Anandhi for Client Profile Begin
        public DbSet<Client> Clients { get; set; }
        public DbSet<Countries> Countries { get; set; }
        public DbSet<Verticals> IndustryVerticals { get; set; }
        public DbSet<SubVerticals> IndustrySubVerticals { get; set; }
        public DbSet<Currencies> Currency { get; set; }
        public DbSet<Cities> Cities { get; set; }
        public DbSet<Regions> Regions { get; set; }
        public DbSet<ClientStatus> ClientStatuses { get; set; }
        public DbSet<Referencable> Referencable { get; set; }
        public DbSet<Referred> Ref { get; set; }
        public DbSet<CustomerRating> Ratings { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Engagements> Engagement { get; set; }
        //Added by Anandhi for Client Profile End

        //For MFA 
        public DbSet<MFAUserAuthLogging> MFAUserAuthLogging { get; set; }
        public DbSet<MFAUserLoggedInDevice> MFAUserLoggedInDevice { get; set; }
        public DbSet<MFAType> MFAType { get; set; }


        //Added by Raj 
        public DbSet<Metric> Metrics { get; set; }
        
        public DbSet<MetricStatus> MetricStatuss { get; set; }
        public DbSet<MetricSpecification> MetricSpecifications { get; set; }
        

        

        public DbSet<Personnel> Personnels { get; set; }
        public DbSet<MetricActual> MetricActuals { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        //By Ratna On 21st May
        public DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }
        public DbSet<Problem> Problems { get; set; }

        //Added by Raj

        //Added by Anandhi for ClientAccount Begin
        public DbSet<ClientAccounSetup> ClientAccountSetup { get; set; }

        //Added by Raj

        //Added by Anandhi for ClientAccount Begin
        public DbSet<APREcoSystem> APREcoSystem { get; set; }

        //Added by Anandhi for ClientAccount End

        //12/30 - Senthil - SLA Metric - Begin
        public DbSet<Calculation> Calaculations { get; set; }
        public DbSet<CalculationDataElement> CalculationDataElements { get; set; }
        public DbSet<ChartIndicator> ChartIndicators { get; set; }
        public DbSet<ContextualElement> ContextualElements { get; set; }
        public DbSet<MetricPerspective> MetricPerspectives { get; set; }
        
        public DbSet<MetricType> MetricTypes { get; set; }
        public DbSet<MetricUnit> MetricUnits { get; set; }
        public DbSet<ObjectiveType> ObjectiveTypes { get; set; }
        public DbSet<PrimaryAudience> PrimaryAudiences { get; set; }
        public DbSet<Rules> Ruless { get; set; }
        public DbSet<Service> Services { get; set; }

        public DbSet<ServiceCategory> ServiceCategories { get; set; }
        public DbSet<ServiceComponent> ServiceComponents { get; set; }
        //12/30 - Senthil - SLA Metric - End

        //Ratna Kumar
        public DbSet<ClientAccountContract> ClientAccountContract { get; set; }
        public DbSet<AccountStatus> AccountStatus { get; set; }

        //Added by Shane / Ratna
        public DbSet<heatDist> HeatDists { get; set; }
        public DbSet<heatUseAlgorithm> HeatUseAlgorithms { get; set; }
        public DbSet<heatUseMetadata> HeatUseMetadata { get; set; }
        public DbSet<HeatUseReport> HeatUseReports { get; set; }
        public DbSet<heatUseThreshold> HeatUseThresholds { get; set; }
        public DbSet<heatVariable> HeatVariable { get; set; }
        public DbSet<heatVariableRelationship> HeatVariableRelationship { get; set; }
        public DbSet<mdmHeatmapConfig> MdmHeatmapConfig { get; set; }
        public DbSet<mdmMetadataHeat> MdmMetadataHeat { get; set; }
        public DbSet<RelationshipType> MdmRelationshipTypes { get; set; }
        public DbSet<mdmVariable> Variable { get; set; }
        public DbSet<mdmVariableCategory> VariableCategories { get; set; }
        public DbSet<mdmVariableLabel> VariableLabel { get; set; }
        //public DbSet<VariableMetaData> VariableMD { get; set; }
        public DbSet<VariableMDType> VariableMDTypes { get; set; }
        //public DbSet<VariableRelationship> VariableRels { get; set; }
        public DbSet<mdmVariableType> VariableType { get; set; }
        public DbSet<mdmVariableMdType> VariableMdType { get; set; }
        public DbSet<mdmVariableMD> VariableMdData { get; set; }
        public DbSet<vwVariableVariableLabel> vwVariableVariableLabel { get; set; }
        public DbSet<VwMDMrelationwithlabel> vwMDMrelationwithlabel { get; set; }
        public DbSet<vwmdmvartypes> vwmdmvartypes { get; set; }
        public DbSet<mdmVariableREL> VariableRel { get; set; }
        public DbSet<vwclientinfo> vwClientInfo { get; set; } 
        public DbSet<vwMetaDataDetails> vwMetaDataDetails { get; set; }
        public DbSet<ReportConfiguration> reportConfiguration { get; set; }
        public DbSet<ReportDataSet> ReportDataSet { get; set; }
        public DbSet<RegisterDataset> registerDataset { get; set; }
        public DbSet<vwRegDataSet> vwRegDataSet { get; set; }
        public DbSet<vwreportvartypes> vwreportvartypes { get; set; }
        public DbSet<vwAlmTreeView> vwAlmTreeView { get; set; }
        public DbSet<ReportVariable> ReportVariable { get; set; }
        public DbSet<ReportThreshold> ReportThreshold { get; set; }
        public DbSet<ReportFilter> ReportFilter { get; set; }
        public DbSet<RegisterDatasetMap> RegisterDatasetMap { get; set; }
        public DbSet<ReportAlgorithm> ReportAlgorithm { get; set; }
        public DbSet<ReportAlgorithmMD> ReportAlgorithmMD { get; set; }
        public DbSet<ReportAlgorithmStageMD> ReportAlgorithmStageMD { get; set; }
        public DbSet<ReportAlgorithmConfiguration> ReportAlgorithmConfiguration { get; set; }
        public DbSet<ReportGraphConfiguration> ReportGraphConfiguration { get; set; }
        public DbSet<ReportConfigurationMD> ReportConfigurationMD { get; set; }
        public DbSet<ReportChartTemplates> ReportChartTemplates { get; set; }
        public DbSet<ReportalgorithmDefn> ReportalgorithmDefn { get; set; }



        //////1/18 Raj
        ////public DbSet<BusinessCapabilitiesLevel1> BusinessCapabilitiesLevel1 { get; set; }
        ////public DbSet<BusinessCapabilitiesLevel2> BusinessCapabilitiesLevel2 { get; set; }
        ////public DbSet<BusinessCapabilitiesLevel3> BusinessCapabilitiesLevel3 { get; set; }
        //////2/28 Raj
        public DbSet<LineOfBusinesses> LineOfBusiness { get; set; }
        public DbSet<ApplicationLOBJunctions> ApplicationLOBJunction { get; set; }
        public DbSet<ApplicationEcoSystemJunctions> ApplicationEcoSystemJunctions { get; set; }
        public DbSet<ApplicationLOBEcoSystemJunctions> ApplicationLOBEcoSystemJunctions { get; set; }
        //3/3/2014 Raj
        public DbSet<ClientBCLevel1> ClientBCLevel1 { get; set; }
        public DbSet<ClientBCLevel2> ClientBCLevel2 { get; set; }
        public DbSet<ClientBCLevel3> ClientBCLevel3 { get; set; }
        public DbSet<JunctionLOBClientBCs> JunctionLOBClientBC { get; set; }
        public DbSet<JunctionAppLOBClientBCs> JunctionAppLOBClientBC { get; set; }

        public DbSet<SRFilters> SRFilter { get; set; }
        public DbSet<AppStoreApplications> AppStore { get; set; }

        public DbSet<IdeaDataSet> IdeaDataSets { get; set; }
        //5/1/2014 Raj
        public DbSet<ServiceCalendar> ServiceCalendar { get; set; }
        public DbSet<ServiceYear> ServiceYear { get; set; }

        
        //Ratna On 14th Oct 2014

        public DbSet<IMPostIdea> IMPostIdea { get; set; }

        public DbSet<MappingTemplate> MappingTemplate { get; set; }

        public DbSet<StoreApplication> StoreApplication { get; set; }

        public DbSet<SourceDataMapping> SourceDataMapping { get; set; }

        public DbSet<SourceFieldConcat> SourceFieldConcat { get; set; }
        public DbSet<JobDetail> JobDetails { get; set; }
        public DbSet<ClientTicketInfo> ClientTicketInfo { get; set; }
        public DbSet<TicketInfo> TicketInfo { get; set; }
        public DbSet<TicketInfoType> TicketInfoType { get; set; }
        public DbSet<JobErrorLog> JobErrorLog { get; set; }
        public DbSet<IMPostChallenge> IMPostChallenge { get; set; }

        public DbSet<vwIMclasswithcat> vwIMclasswithcat { get; set; }
        public DbSet<IMClassification> IMClassification { get; set; }
        public DbSet<IMCategory> IMCategory { get; set; }
        public DbSet<IMItemCategoryMap> IMItemCategoryMap { get; set; }

        public DbSet<vwIMaccountwithcat> vwIMaccountwithcat { get; set; }
        public DbSet<IMIdea> IMIdea { get; set; }
        //public DbSet<IMCategory2> IMCategory2 { get; set; }
        //public DbSet<IMCategory2Map> IMCategory2Map { get; set; }
        public DbSet<IMItemITBenefits> IMItemITBenefits { get; set; }
        public DbSet<IMItemBusinessBenefits> IMItemBusinessBenefits { get; set; }
        public DbSet<IMItemComments> IMItemIMItemComments { get; set; }
        public DbSet<IMItemPriority> IMItemPriority { get; set; }
        public DbSet<IMItemStatus> IMItemStatus { get; set; }
        public DbSet<vwIMSuggestIdeas> vwIMSuggestIdeas { get; set; }
        public DbSet<IMIdeaPriorityHistory> IMIdeaPriorityHistory { get; set; }
        public DbSet<IMIdeaStatusHistory> IMIdeaStatusHistory { get; set; }
        public DbSet<vwIdeaPriorityHistory> vwIdeaPriorityHistory { get; set; }
        public DbSet<vwIdeaStatusHistory> vwIdeaStatusHistory { get; set; }

        public DbSet<IMAnalysisRootCause> IMAnalysisRootCause { get; set; }
        public DbSet<IMCompletedAndNextSteps> IMCompletedAndNextSteps { get; set; }
        public DbSet<IMComplexity> IMComplexity { get; set; }
        public DbSet<IMConfigItem> IMConfigItem { get; set; }
        public DbSet<IMConfigItemsMap> IMConfigItemsMap { get; set; }
        public DbSet<IMConfigType> IMConfigType { get; set; }
        public DbSet<IMImpPriorityHistory> IMImpPriorityHistory { get; set; }
        public DbSet<IMRiskAndMitigation> IMRiskAndMitigation { get; set; }
        public DbSet<IMSolution> IMSolution { get; set; }
        public DbSet<IMTeam> IMTeam { get; set; }
        public DbSet<IMMetrics> IMMetrics { get; set; }

        public DbSet<vwMetriclistWithSpecifications> vwMetriclistWithSpecifications { get; set; }

        public DbSet<tblOverallSummery> tblOverallSummery { get; set; }
        public DbSet<v_DashboardOverallSummary> v_DashboardOverallSummary { get; set; }

        public DbSet<IMIdeaType> IMIdeaType { get; set; }
        public DbSet<IMIdeaSource> IMIdeaSource { get; set; }

        public DbSet<IMLOB> IMLOBSource { get; set; }
        //Enhancements
        public DbSet<IMProgressStatus> IMProgressStatus { get; set; }
        public DbSet<IMStage> IMStage { get; set; }
        public DbSet<IMStatuss> IMStatus { get; set; }
        public DbSet<IMPhase> IMPhase { get; set; }

        public DbSet<IMRole> IMRole { get; set; }
        public DbSet<IMResource> IMResource { get; set; }       
        public DbSet<IMProgressTemplate> IMProgressTemplate { get; set; }


        //
        //10/11/2017 Raj
        public DbSet<webpages_Membership> webpages_Membership { get; set; }
        public DbSet<UserPasswordLog> UserPasswordLog { get; set; }
        //
        //11/16/2017 Raj
        public DbSet<INCluster> INCluster { get; set; }
        public DbSet<INIncidentClustered> INIncidentClustered { get; set; }
        public DbSet<INIncidentServicehours> INIncidentServicehours { get; set; }

        //4/6/2018 Soumya  --KEDB
        public DbSet<KEDB> KEDB { get; set; }
        public DbSet<KEDBIncident> KEDBIncident { get; set; }

        

        //11/30/2017 Raj
        public DbSet<INAuditTable> INAuditTable { get; set; }
        //Added for Rule Engine Start 20/11/2017 - Edward/
        public DbSet<SlaCalSetup> SlaCalSetup { get; set; }
        public DbSet<SlaFieldCol> SlaFieldCol { get; set; }
        public DbSet<SLAClockType> SlaClockType { get; set; }



        //Added for Rule Engine End 

        //Added for PortfolioAnalyzer  
        public DbSet<Application> Applications { get; set; }
        public DbSet<APRAppCriticality> APRAppCriticality { get; set; }
        public DbSet<APRAppLifeCycle> APRAppLifeCycle { get; set; }
        public DbSet<APRAppSourceCodeType> APRAppSourceCodeType { get; set; }
        public DbSet<APRDomain> APRDomain { get; set; }
        public DbSet<APRAppArchitecture> APRAppArchitecture { get; set; }
        public DbSet<APRAppEnvironment> APRAppEnvironment { get; set; }
        public DbSet<APRAppProcessingType> APRAppProcessingType { get; set; }
        public DbSet<APRAppSecurity> APRAppSecurity { get; set; }
        public DbSet<APRAppType> APRAppType { get; set; }
        public DbSet<APRDisposition> APRDisposition { get; set; }
        public DbSet<APRDispositionAction> APRDispositionAction { get; set; }
        public DbSet<APRDispositionRationale> APRDispositionRationale { get; set; }
        public DbSet<APRAppFinancial> APRFinancial { get; set; }
        public DbSet<APRAppDisposition> APRAppDisposition { get; set; } //For Disposition Details 2/9/2018 -Das
        public DbSet<APRAppRedundancy> APRAppRedundancy { get; set; }
        public DbSet<APRAppInterface> APRAppInterface { get; set; }

        public DbSet<ClientAccessControl> ClientAccessControl { get; set; }

        //2/15/2018 Raj
        public DbSet<PersonnelRole> PersonnelRole { get; set; }

        //Survey
        public DbSet<Survey> Survey { get; set; }
        public DbSet<SurveyModel> SurveyModel { get; set; }
        public DbSet<SurveyCollection> SurveyCollection { get; set; }
        public DbSet<SurveyModelRatingFactor> SurveyModelRatingFactor { get; set; }
        public DbSet<SurveyUser> SurveyUser { get; set; }
        public DbSet<SurveyPerspective> SurveyPerspective { get; set; }
        public DbSet<PerspectiveCategory> PerspectiveCategory { get; set; }

        //keyword search
        public DbSet<KeywordSearch> KeywordSearch { get; set; }

        public DbSet<mdmDataUploadTable> mdmDataUploadTables { get; set; }
        public DbSet<mdmhstDataFileupload> mdmhstDataFileuploads { get; set; }
        
        //PI Data Mask
        public DbSet<PIDataMaskList> PIDataMaskList { get; set; }
        public DbSet<UserActionTracking> UserActionTracking { get; set; }
        //SSIs Integration UI
        public DbSet<INClusterConfigurationKey> INClusterConfigurationKey { get; set; }
        public DbSet<INClusterConfigurationValue> INClusterConfigurationValue { get; set; }
        public DbSet<INClusterParameterValue> INClusterParameterValue { get; set; }
        public DbSet<INDictionary> INDictionary { get; set; }
        public DbSet<Vendor> Vendor { get; set; }
        public DbSet<VendorProduct> VendorProduct { get; set; }
        public DbSet<VendorProductVersion> VendorProductVersion { get; set; }

        public DbSet<APRHostList> APRHostList { get; set; }
        public DbSet<APRAppHardware> APRAppHardware { get; set; }

        public DbSet<APRAppSoftware> APRAppSoftware { get; set; }

        public DbSet<INClusterActivityLog> INClusterActivityLog { get; set; }
        public DbSet<IMIdeaApprover> IMIdeaApprover { get; set; }


        public DbSet<IMMetricTarget> IMMetricTarget { get; set; }
        public DbSet<IMWbsTemplate> IMWbsTemplate { get; set; }

        public DbSet<IMIdeaTask> IMIdeaTask { get; set; }
        public DbSet<IMTaskDependency> IMTaskDependency { get; set; }
        public DbSet<IMTaskResource> IMTaskResource { get; set; }
        public DbSet<IMIdeaTaskPriority> IMIdeaTaskPriority { get; set; }

        //Analysis Report Saving
        public DbSet<Report> Report { get; set; }
        public DbSet<ReportView> ReportView { get; set; }
        public DbSet<ReportViewField> ReportViewField { get; set; }
        public DbSet<ReportViewFilter> ReportViewFilter { get; set; }
        public DbSet<ReportFieldType> ReportFieldType { get; set; }


        public DbSet<ExternalHosting> ExternalHosting { get; set; }
        public DbSet<ApplicationClassification> ApplicationClassification { get; set; }

        public DbSet<BAMClientBusinessCapabilities> BAMClientBusinessCapabilities { get; set; }
        public DbSet<BAMBusinessCapabilities> BAMBusinessCapabilities { get; set; }
        public DbSet<BAMDegreeofSupport> BAMDegreeofSupport { get; set; }

        //Framework Builder
        public DbSet<RAEstimateCategory> RAEstimateCategory { get; set; }
        public DbSet<RARoles> RARoles { get; set; }
        public DbSet<RAResourceGroup> RAResourceGroup { get; set; }
        public DbSet<RARetirementPhase> RARetirementPhase { get; set; }
        public DbSet<RARetirementStage> RARetirementStage { get; set; }
        public DbSet<RAWbsStatus> RAWbsStatus { get; set; }
        public DbSet<RAWorkProduct> RAWorkProduct { get; set; }
        public DbSet<RAProjectHealth> RAProjectHealth { get; set; }
        public DbSet<RAWbsTemplate> RAWbsTemplate { get; set; }
        public DbSet<RAWbsTemplateDetails> RAWbsTemplateDetails { get; set; }
        public DbSet<RAReplacementScope> RAReplacementScope { get; set; }
        public DbSet<RAAssessment> RAAssessment { get; set; }
        public DbSet<RADispCandidate> RADispCandidate { get; set; }
        public DbSet<RADispRecommendation> RADispRecommendation { get; set; }

        public DbSet<RAExecutionCostActivity> RAExecutionCostActivity { get; set; }
        public DbSet<RASupportCostActivity> RASupportCostActivity { get; set; }
        public DbSet<RAProjectManagementActivity> RAProjectManagementActivity { get; set; }
        public DbSet<RAOngoingArchivalActivity> RAOngoingArchivalActivity { get; set; }
        public DbSet<RASupportCost> RASupportCost { get; set; }
        public DbSet<RAExecutionCost> RAExecutionCost { get; set; }
        public DbSet<RAProjectManagement> RAProjectManagement { get; set; }
        public DbSet<RAOngoingArchivalSupport> RAOngoingArchivalSupport { get; set; }
        public DbSet<RATask> RATask { get; set; }
        public DbSet<RATaskDependency> RATaskDependency { get; set; }
        public DbSet<RATaskResource> RATaskResource { get; set; }
        public DbSet<EstimateCategories> EstimateCategory { get; set; }
        public DbSet<EstimateTypes> EstimateType { get; set; }
        public DbSet<RAResource> RAResource { get; set; }


        public DbSet<APROwningCompany> APROwningCompany { get; set; }
        public DbSet<APRDeploymentModel> APRDeploymentModel { get; set; }
        public DbSet<APRStandardStatus> APRStandardStatus { get; set; }
        public DbSet<APRDifferentiationLevel> APRDifferentiationLevel { get; set; }
        public DbSet<APRInterfaceMethod> APRInterfaceMethod { get; set; }
        public DbSet<APRApplicationCountry> APRApplicationCountry { get; set; }
        public DbSet<APRApplicationInterfaceMethod> APRApplicationInterfaceMethod { get; set; }

        public DbSet<APRDispositionSolution> APRDispositionSolution { get; set; }
        public DbSet<APRRedundancy> APRRedundancy { get; set; }
        public DbSet<APRDispAndDispSollution> APRDispAndDispSollution { get; set; }
        public DbSet<APRRoadmapTargetApp> APRRoadmapTargetApp { get; set; }

        public DbSet<INReport> INReport { get; set; }
        public DbSet<INClientReport> INClientReport { get; set; }
        public DbSet<INReportObservation> INReportObservation { get; set; }

        public DbSet<INAssigneeGroup> INAssigneeGroup { get; set; }
        public DbSet<INBusinessTower> INBusinessTower { get; set; }

        public DbSet<SourceFieldCalculate> SourceFieldCalculate { get; set; }

        public DbSet<TeamRule> TeamRule { get; set; }

        public DbSet<INContractRule> INContractRule { get; set; }

        public DbSet<SurveyINUser> SurveyINUser { get; set; }
        public DbSet<SurveyINCollection> SurveyINCollection { get; set; }
        public DbSet<SSIS_INUpdateQuery> SSIS_INUpdateQuery { get; set; }

        public DbSet<BCApplicationJunction> BCApplicationJunction { get; set; }
        public DbSet<Businessprocesshealth> Businessprocesshealth { get; set; }
        public DbSet<BusinessProcessImportance> BusinessProcessImportance { get; set; }

        public DbSet<AMSCustomAuditLog> AMSCustomAuditLog { get; set; }

        public DbSet<ServiceImprovementType> ServiceImprovementType { get; set; }

        public DbSet<ServiceImprovementGoal> ServiceImprovementGoal { get; set; }

        public DbSet<ServiceImprovementPlan_Q> ServiceImprovementPlan_Q { get; set; }

        public DbSet<ServiceImprovementPlanStatus> ServiceImprovementPlanStatus { get; set; }

        public DbSet<EngagementPhases> EngagementPhases { get; set; }
        public DbSet<ClientEngagement> ClientEngagement { get; set; }
        public DbSet<ClientEngagementFTE_Q> ClientEngagementFTE_Q { get; set; }
        public DbSet<EngagementLocation> EngagementLocation { get; set; }
        public DbSet<LeadershipRoles> LeadershipRoles { get; set; }
        public DbSet<UserLeadershipRole> UserLeadershipRole { get; set; }
        public DbSet<BaseLineData> BaseLineData { get; set; }    
        
        public DbSet<INColumnValue> INColumnValue { get; set; }
        public DbSet<INColumnValueRule> INColumnValueRule { get; set; }

        public DbSet<DMDemandType> DMDemandType { get; set; }
        public DbSet<DMLOB> DMLOB { get; set; }
        public DbSet<DMPostDemand> DMPostDemand { get; set; }
        public DbSet<DMDemandApprover> DMDemandApprover { get; set; }
        public DbSet<DMClassification> DMClassification { get; set; }
        public DbSet<DMCategory> DMCategory { get; set; }
        public DbSet<DMItemCategoryMap> DMItemCategoryMap { get; set; }
        public DbSet<DMConfigType> DMConfigType { get; set; }
        public DbSet<DMConfigItem> DMConfigItem { get; set; }
        public DbSet<DMConfigItemsMap> DMConfigItemsMap { get; set; }
        public DbSet<DMItemBusinessBenefits> DMItemBusinessBenefits { get; set; }
        public DbSet<DMItemITBenefits> DMItemITBenefits { get; set; }
        public DbSet<DMAnalysisRootCause> DMAnalysisRootCause { get; set; }
        public DbSet<DMSolution> DMSolution { get; set; }
        public DbSet<DMRiskAndMitigation> DMRiskAndMitigation { get; set; }
        public DbSet<DMMetricTarget> DMMetricTarget { get; set; }
        public DbSet<DMItemComments> DMItemComments { get; set; }
        public DbSet<DMImpPriorityHistory> DMImpPriorityHistory { get; set; }
        public DbSet<DMPriorityHistory> DMPriorityHistory { get; set; }
        public DbSet<DMStatusHistory> DMStatusHistory { get; set; }
        public DbSet<DMCompletedAndNextSteps> DMCompletedAndNextSteps { get; set; }
        public DbSet<DMItemStatus> DMItemStatus { get; set; }
        public DbSet<DMPostChallenge> DMPostChallenge { get; set; }
        public DbSet<DMProgressStatus> DMProgressStatus { get; set; }
        public DbSet<DMComplexity> DMComplexity { get; set; }
        public DbSet<DMTask> DMTask { get; set; }
        public DbSet<DMStage> DMStage { get; set; }
        public DbSet<DMTaskAndActivities> DMTaskAndActivities { get; set; }
        public DbSet<DMDocumentType> DMDocumentType { get; set; }
        public DbSet<DMTaskActivitiesHistory> DMTaskActivitiesHistory { get; set; }
        public DbSet<KEDBIssueTypes> KEDBIssueType { get; set; }
        public DbSet<KEDBSolutionCategory> KEDBSolutionCategory { get; set; }
        public DbSet<KEDBSolutionStatus> KEDBSolutionStatus { get; set; }
        public DbSet<KEDBBusinessFunction> KEDBBusinessFunction { get; set; }
        public DbSet<KEDBInstance> KEDBInstance { get; set; }
        public DbSet<KEDBStatusAudit> KEDBStatusAudit { get; set; }
        public DbSet<RPTSubscription> RPTSubscription { get; set; }
        public DbSet<IMAutoApprovers> IMAutoApprovers { get; set; }
        public DbSet<RPTJobDetail> RPTJobDetail { get; set; }
        public DbSet<RPTJobDetailArchive> RPTJobDetailArchive { get; set; }
        public DbSet<IMSystemImpactedService> IMSystemImpactedService { get; set; }
        public DbSet<ConfigAgingQuery> ConfigAgingQuery { get; set; }

        //Feedback Manager
        public DbSet<IMFDBKFeedbackType> IMFDBKFeedbackType { get; set; }
        public DbSet<IMFDBKProductApp> IMFDBKProductApp { get; set; }
        public DbSet<IMFDBKDigitalOffering> IMFDBKDigitalOffering { get; set; }
        public DbSet<InternalClient> InternalClient { get; set; }
        public DbSet<IMFDBKPost> IMFDBKPost { get; set; }
        public DbSet<IMFDBKStatus> IMFDBKStatus { get; set; }
        public DbSet<IMFDBKUser> IMFDBKUser { get; set; }
        public DbSet<IMFDBKClient> IMFDBKClient { get; set; }
        public DbSet<IMFDBKVote> IMFDBKVote { get; set; }
        public DbSet<IMFDBKComment> IMFDBKComment { get; set; }
        public DbSet<IMFDBKNotification> IMFDBKNotification { get; set; }
        public DbSet<IMFDBKDgtlOfferingMgr> IMFDBKDgtlOfferingMgr { get; set; }
        public DbSet<IMFDBKProductMgr> IMFDBKProductMgr { get; set; }
        public DbSet<IMFDBKProductGroups> IMFDBKProductGroups { get; set; }
        public DbSet<IMFDBKTag> IMFDBKTag { get; set; }
        public DbSet<IMFDBKMapTag> IMFDBKMapTag { get; set; }
        public DbSet<IMFDBKSentiment> IMFDBKSentiment { get; set; }
        public DbSet<IMFDBKActivityLogs> IMFDBKActivityLogs { get; set; }
        public DbSet<IMFDBKAttachment> IMFDBKAttachment { get; set; }

        public DbSet<Discipline> Discipline { get; set; }
        public DbSet<Component> Component { get; set; }
        public DbSet<Aspect> Aspect { get; set; }
        public DbSet<Assessment> Assessment { get; set; }
        public DbSet<QuestRespAttachments> QuestRespAttachments { get; set; }
        public DbSet<EngagementQuestionnaire> EngagementQuestionnaire { get; set; }
        public DbSet<QuestionnaireResponse> QuestionnaireResponse { get; set; }
        public DbSet<EngagementQuestionSet> EngagementQuestionSet { get; set; }

        public DbSet<ActionItemTrackerLog> ActionItemTrackerLog { get; set; }
        public DbSet<ActionItemTracker> ActionItemTracker { get; set; }
        public DbSet<IMIdeaSourceMaster> IMIdeaSourceMaster { get; set; }
        public DbSet<IMEmailtemplates> IMEmailtemplates { get; set; }
        public DbSet<A3MM_MultipleResponses> A3MM_MultipleResponses { get; set; }
        public DbSet<A3MM_MultipleFollowUpQuestions> A3MM_MultipleFollowUpQuestions { get; set; }
        public DbSet<ImplementationPlann> A3MM_ImplementationPlan { get; set; }
        public DbSet<BusinessDriverr> A3MM_BusinessDriver { get; set; }
        public DbSet<AMSCatalog> A3MM_Catalog { get; set; }
        public DbSet<A3SurveyType> A3SurveyType { get; set; }
        public DbSet<SB_ScheduleEmailConfig> SB_ScheduleEmailConfig { get; set; }
        public DbSet<IMOfflineApprover> IMOfflineApprover { get; set; }
        public DbSet<IMIdeaReassign_log> IMIdeaReassign_log { get; set; }
        public DbSet<ApprovalAttachments> ApprovalAttachments { get; set; }


    }

}
