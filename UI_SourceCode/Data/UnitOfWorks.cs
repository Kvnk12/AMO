using Domain.Abstract;
using Domain.Entities.ClientProfileManager;
using Domain.Entities.Survey;
using Domain.Entities.IncidentNavigator;
using Domain.Entities.PortfolioAnalyzer;
using NTTData.AMOPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.Report;
using Domain.Entities.RetirementAccelerator;
using Domain.Entities.FDBKManager;
using Domain.Entities.A3MM;

namespace Data
{
    public class UnitOfWorks : IDisposable, IUnitOfWorks
    {
        private DbContext context;


        public DbContext Context { get; set; }
        public UnitOfWorks(DbContext cxt)
        {
            context = cxt;
        }

        //common tables
        private IGenericRepository<Countries> _country;
        public IGenericRepository<Countries> Countries
        {
            get
            {
                return _country ?? (_country = new GenericRepository<Countries>(context));
            }
        }



        //MFA
        private IGenericRepository<MFAUserLoggedInDevice> _MFAUserLoggedInDevice;
        public IGenericRepository<MFAUserLoggedInDevice> MFAUserLoggedInDevice
        {
            get
            {
                return _MFAUserLoggedInDevice ?? (_MFAUserLoggedInDevice = new GenericRepository<MFAUserLoggedInDevice>(context));
            }
        }
        private IGenericRepository<MFAUserAuthLogging> _MFAUserAuthLogging;
        public IGenericRepository<MFAUserAuthLogging> MFAUserAuthLogging
        {
            get
            {
                return _MFAUserAuthLogging ?? (_MFAUserAuthLogging = new GenericRepository<MFAUserAuthLogging>(context));
            }
        }
        private IGenericRepository<MFAType> _MFAType;
        public IGenericRepository<MFAType> MFAType
        {
            get
            {
                return _MFAType ?? (_MFAType = new GenericRepository<MFAType>(context));
            }
        }

        private IGenericRepository<IMSystemImpactedService> _IMSystemImpactedService;
        public IGenericRepository<IMSystemImpactedService> IMSystemImpactedService
        {
            get
            {
                return _IMSystemImpactedService ?? (_IMSystemImpactedService = new GenericRepository<IMSystemImpactedService>(context));
            }
        }

        //Source template 
        private IGenericRepository<MappingTemplate> _mappingTemplate;
        public IGenericRepository<MappingTemplate> MappingTemplate
        {
            get
            {
                return _mappingTemplate ?? (_mappingTemplate = new GenericRepository<MappingTemplate>(context));
            }
        }



        private IGenericRepository<StoreApplication> _storeApplication;
        public IGenericRepository<StoreApplication> StoreApplication
        {
            get
            {
                return _storeApplication ?? (_storeApplication = new GenericRepository<StoreApplication>(context));
            }
        }

        private IGenericRepository<AppStoreApplications> _appStoreApplications;
        public IGenericRepository<AppStoreApplications> AppStoreApplications
        {
            get
            {
                return _appStoreApplications ?? (_appStoreApplications = new GenericRepository<AppStoreApplications>(context));
            }
        }

        private IGenericRepository<JobErrorLog> _jobErr;
        public IGenericRepository<JobErrorLog> JobErrorLog
        {
            get
            {
                return _jobErr ?? (_jobErr = new GenericRepository<JobErrorLog>(context));
            }
        }

        private IGenericRepository<UserProfile> _userProfile;
        public IGenericRepository<UserProfile> UserProfile
        {
            get
            {
                return _userProfile ?? (_userProfile = new GenericRepository<UserProfile>(context));
            }
        }

        private IGenericRepository<SourceDataMapping> _sourceDataMapping;
        public IGenericRepository<SourceDataMapping> SourceDataMapping
        {
            get
            {
                return _sourceDataMapping ?? (_sourceDataMapping = new GenericRepository<SourceDataMapping>(context));
            }
        }

        private IGenericRepository<SourceFieldConcat> _sourceFieldConcat;
        public IGenericRepository<SourceFieldConcat> SourceFieldConcat
        {
            get
            {
                return _sourceFieldConcat ?? (_sourceFieldConcat = new GenericRepository<SourceFieldConcat>(context));
            }
        }

        private IGenericRepository<Client> _clients;
        public IGenericRepository<Client> Clients
        {
            get
            {
                return _clients ?? (_clients = new GenericRepository<Client>(context));
            }
        }

        private IGenericRepository<JobDetail> _jobDetails;
        public IGenericRepository<JobDetail> JobDetails
        {
            get
            {
                return _jobDetails ?? (_jobDetails = new GenericRepository<JobDetail>(context));
            }
        }

        private IGenericRepository<TicketInfo> _TicketInfo;
        public IGenericRepository<TicketInfo> TicketInfo
        {
            get
            {
                return _TicketInfo ?? (_TicketInfo = new GenericRepository<TicketInfo>(context));
            }
        }
        private IGenericRepository<TicketInfoType> _TicketInfoType;
        public IGenericRepository<TicketInfoType> TicketInfoType
        {
            get
            {
                return _TicketInfoType ?? (_TicketInfoType = new GenericRepository<TicketInfoType>(context));
            }
        }

        /// <summary>
        /// //Incident Navigator 
        /// </summary>
        //Raj added on 10/11/2017


        private IGenericRepository<INClusterParameterValue> _inClusterParameterValue;
        public IGenericRepository<INClusterParameterValue> INClusterParameterValue
        {
            get
            {
                return _inClusterParameterValue ?? (_inClusterParameterValue = new GenericRepository<INClusterParameterValue>(context));
            }
        }



        private IGenericRepository<INCluster> _inCluster;
        public IGenericRepository<INCluster> INClusters
        {
            get
            {
                return _inCluster ?? (_inCluster = new GenericRepository<INCluster>(context));
            }
        }
        private IGenericRepository<Incident> _incident;
        public IGenericRepository<Incident> Incident
        {
            get
            {
                return _incident ?? (_incident = new GenericRepository<Incident>(context));
            }
        }
        private IGenericRepository<AMSCustomAuditLog> _AMSCustomAuditLog;
        public IGenericRepository<AMSCustomAuditLog> AMSCustomAuditLog
        {
            get
            {
                return _AMSCustomAuditLog ?? (_AMSCustomAuditLog = new GenericRepository<AMSCustomAuditLog>(context));
            }
        }


        private IGenericRepository<INIncidentClustered> _iNIncidentClustered;
        public IGenericRepository<INIncidentClustered> INIncidentClustered
        {
            get
            {
                return _iNIncidentClustered ?? (_iNIncidentClustered = new GenericRepository<INIncidentClustered>(context));
            }
        }


        private IGenericRepository<INAuditTable> _iNAuditTable;
        public IGenericRepository<INAuditTable> INAuditTable
        {
            get
            {
                return _iNAuditTable ?? (_iNAuditTable = new GenericRepository<INAuditTable>(context));
            }
        }

        private IGenericRepository<INIncidentServicehours> _iNIncidentServicehours;
        public IGenericRepository<INIncidentServicehours> INIncidentServicehours
        {
            get
            {
                return _iNIncidentServicehours ?? (_iNIncidentServicehours = new GenericRepository<INIncidentServicehours>(context));
            }
        }


        private IGenericRepository<APREcoSystem> _aPREcoSystem;
        public IGenericRepository<APREcoSystem> APREcoSystem
        {
            get
            {
                return _aPREcoSystem ?? (_aPREcoSystem = new GenericRepository<APREcoSystem>(context));
            }
        }

        private IGenericRepository<ApplicationLOBEcoSystemJunctions> _applicationLOBEcoSystemJunctions;
        public IGenericRepository<ApplicationLOBEcoSystemJunctions> ApplicationLOBEcoSystemJunction
        {
            get
            {
                return _applicationLOBEcoSystemJunctions ?? (_applicationLOBEcoSystemJunctions = new GenericRepository<ApplicationLOBEcoSystemJunctions>(context));
            }
        }

        private IGenericRepository<ApplicationEcoSystemJunctions> _applicationEcoSystemJunctions;
        public IGenericRepository<ApplicationEcoSystemJunctions> ApplicationEcoSystemJunction
        {
            get
            {
                return _applicationEcoSystemJunctions ?? (_applicationEcoSystemJunctions = new GenericRepository<ApplicationEcoSystemJunctions>(context));
            }
        }
        //2/6/2018 Soumya KEDB

        private IGenericRepository<KEDB> _kedb;
        public IGenericRepository<KEDB> KEDB
        {
            get
            {
                return _kedb ?? (_kedb = new GenericRepository<KEDB>(context));
            }
        }

        private IGenericRepository<KEDBInstance> _kedbInstance;
        public IGenericRepository<KEDBInstance> KEDBInstance
        {
            get
            {
                return _kedbInstance ?? (_kedbInstance = new GenericRepository<KEDBInstance>(context));
            }
        }

        private IGenericRepository<KEDBStatusAudit> _kedbStatusAudit;
        public IGenericRepository<KEDBStatusAudit> KEDBStatusAudit
        {
            get
            {
                return _kedbStatusAudit ?? (_kedbStatusAudit = new GenericRepository<KEDBStatusAudit>(context));
            }
        }

        private IGenericRepository<KEDBIncident> _kedbincident;
        public IGenericRepository<KEDBIncident> KEDBIncident
        {
            get
            {
                return _kedbincident ?? (_kedbincident = new GenericRepository<KEDBIncident>(context));
            }
        }
        private IGenericRepository<KEDBIssueTypes> _issuetype;
        public IGenericRepository<KEDBIssueTypes> KEDBIssueTypes
        {
            get
            {
                return _issuetype ?? (_issuetype = new GenericRepository<KEDBIssueTypes>(context));
            }
        }
        private IGenericRepository<ClientAccounSetup> _account;
        public IGenericRepository<ClientAccounSetup> ClientAccounSetup
        {
            get
            {
                return _account ?? (_account = new GenericRepository<ClientAccounSetup>(context));
            }
        }
       
        private IGenericRepository<KEDBBusinessFunction> _busfunct;
        public IGenericRepository<KEDBBusinessFunction> KEDBBusinessFunction
        {
            get
            {
                return _busfunct ?? (_busfunct = new GenericRepository<KEDBBusinessFunction>(context));
            }
        }
        private IGenericRepository<KEDBSolutionCategory> _solcat;
        public IGenericRepository<KEDBSolutionCategory> KEDBSolutionCategory
        {
            get
            {
                return _solcat ?? (_solcat = new GenericRepository<KEDBSolutionCategory>(context));
            }
        }

        private IGenericRepository<KeywordSearch> _keywordsearch;
        public IGenericRepository<KeywordSearch> KeywordSearch
        {
            get
            {
                return _keywordsearch ?? (_keywordsearch = new GenericRepository<KeywordSearch>(context));
            }
        }

        private IGenericRepository<RPTSubscription> _RPTSubscription;
        public IGenericRepository<RPTSubscription> RPTSubscription
        {
            get
            {
                return _RPTSubscription ?? (_RPTSubscription = new GenericRepository<RPTSubscription>(context));
            }
        }

        //Raj Ends

        private IGenericRepository<SlaCalSetup> _SLASetup;
        public IGenericRepository<SlaCalSetup> SLASetup
        {
            get
            {
                return _SLASetup ?? (_SLASetup = new GenericRepository<SlaCalSetup>(context));
            }
        }
        //BAM - Business Capabilities
        private IGenericRepository<BAMBusinessCapabilities> _BAMBusinessCapabilities;
        public IGenericRepository<BAMBusinessCapabilities> BAMBusinessCapabilities
        {
            get
            {
                return _BAMBusinessCapabilities ?? (_BAMBusinessCapabilities = new GenericRepository<BAMBusinessCapabilities>(context));
            }
        }
        private IGenericRepository<BAMClientBusinessCapabilities> _BAMClientBusinessCapabilities;
        public IGenericRepository<BAMClientBusinessCapabilities> BAMClientBusinessCapabilities
        {
            get
            {
                return _BAMClientBusinessCapabilities ?? (_BAMClientBusinessCapabilities = new GenericRepository<BAMClientBusinessCapabilities>(context));
            }
        }
        private IGenericRepository<BAMDegreeofSupport> _BAMDegreeofSupport;
        public IGenericRepository<BAMDegreeofSupport> BAMDegreeofSupport
        {
            get
            {
                return _BAMDegreeofSupport ?? (_BAMDegreeofSupport = new GenericRepository<BAMDegreeofSupport>(context));
            }
        }

        //Portfolio Analyzer

        private IGenericRepository<APRAppCriticality> _aprAppCriticality;
        public IGenericRepository<APRAppCriticality> APRAppCriticality
        {
            get
            {
                return _aprAppCriticality ?? (_aprAppCriticality = new GenericRepository<APRAppCriticality>(context));
            }
        }

        private IGenericRepository<Application> _application;
        public IGenericRepository<Application> Application
        {
            get
            {
                return _application ?? (_application = new GenericRepository<Application>(context));
            }
        }

        private IGenericRepository<APRAppLifeCycle> _aprAppLifeCycle;
        public IGenericRepository<APRAppLifeCycle> APRAppLifeCycle
        {
            get
            {
                return _aprAppLifeCycle ?? (_aprAppLifeCycle = new GenericRepository<APRAppLifeCycle>(context));
            }
        }

        private IGenericRepository<APRAppSourceCodeType> _aprAppSourceCodeType;
        public IGenericRepository<APRAppSourceCodeType> APRAppSourceCodeType
        {
            get
            {
                return _aprAppSourceCodeType ?? (_aprAppSourceCodeType = new GenericRepository<APRAppSourceCodeType>(context));
            }
        }

        private IGenericRepository<APRDomain> _aprDomain;
        public IGenericRepository<APRDomain> APRDomain
        {
            get
            {
                return _aprDomain ?? (_aprDomain = new GenericRepository<APRDomain>(context));
            }
        }
        private IGenericRepository<APRAppArchitecture> _aprAppArchitecture;
        public IGenericRepository<APRAppArchitecture> APRAppArchitecture
        {
            get
            {
                return _aprAppArchitecture ?? (_aprAppArchitecture = new GenericRepository<APRAppArchitecture>(context));
            }
        }

        private IGenericRepository<APRDispositionRationale> _aprAppDispositionRationale;
        public IGenericRepository<APRDispositionRationale> APRDispositionRationale
        {
            get
            {
                return _aprAppDispositionRationale ?? (_aprAppDispositionRationale = new GenericRepository<APRDispositionRationale>(context));
            }
        }
        private IGenericRepository<APRDispositionAction> _aprAppDispositionAction;
        public IGenericRepository<APRDispositionAction> APRDispositionAction
        {
            get
            {
                return _aprAppDispositionAction ?? (_aprAppDispositionAction = new GenericRepository<APRDispositionAction>(context));
            }
        }

        private IGenericRepository<APRDisposition> _aprAppDisposition;
        public IGenericRepository<APRDisposition> APRDisposition
        {
            get
            {
                return _aprAppDisposition ?? (_aprAppDisposition = new GenericRepository<APRDisposition>(context));
            }
        }
        private IGenericRepository<APRAppType> _aprAppType;
        public IGenericRepository<APRAppType> APRAppType
        {
            get
            {
                return _aprAppType ?? (_aprAppType = new GenericRepository<APRAppType>(context));
            }
        }
        private IGenericRepository<APRAppSecurity> _aprAppSecurity;
        public IGenericRepository<APRAppSecurity> APRAppSecurity
        {
            get
            {
                return _aprAppSecurity ?? (_aprAppSecurity = new GenericRepository<APRAppSecurity>(context));
            }
        }
        private IGenericRepository<APRAppProcessingType> _aprAppProcessingType;
        public IGenericRepository<APRAppProcessingType> APRAppProcessingType
        {
            get
            {
                return _aprAppProcessingType ?? (_aprAppProcessingType = new GenericRepository<APRAppProcessingType>(context));
            }
        }
        private IGenericRepository<APRAppEnvironment> _aprAppEnvironment;
        public IGenericRepository<APRAppEnvironment> APRAppEnvironment
        {
            get
            {
                return _aprAppEnvironment ?? (_aprAppEnvironment = new GenericRepository<APRAppEnvironment>(context));
            }
        }
        private IGenericRepository<APRAppFinancial> _aprFinancial;
        public IGenericRepository<APRAppFinancial> APRAppFinancial
        {
            get
            {
                return _aprFinancial ?? (_aprFinancial = new GenericRepository<APRAppFinancial>(context));
            }
        }

        private IGenericRepository<ApplicationClassification> _applicationClassification;
        public IGenericRepository<ApplicationClassification> ApplicationClassification
        {
            get
            {
                return _applicationClassification ?? (_applicationClassification = new GenericRepository<ApplicationClassification>(context));
            }
        }

        private IGenericRepository<ExternalHosting> _externalHosting;
        public IGenericRepository<ExternalHosting> ExternalHosting
        {
            get
            {
                return _externalHosting ?? (_externalHosting = new GenericRepository<ExternalHosting>(context));
            }
        }

        //For APR_DispositionDetails   2/9/2018 - Das

        private IGenericRepository<APRAppDisposition> _aprappDisposition;
        public IGenericRepository<APRAppDisposition> APRAppDisposition
        {
            get
            {
                return _aprappDisposition ?? (_aprappDisposition = new GenericRepository<APRAppDisposition>(context));
            }
        }
        //APRAppRedundancy
        private IGenericRepository<APRAppRedundancy> _appRedundancy;
        public IGenericRepository<APRAppRedundancy> APRAppRedundancy
        {
            get
            {
                return _appRedundancy ?? (_appRedundancy = new GenericRepository<APRAppRedundancy>(context));
            }
        }
        // apr interface
        private IGenericRepository<APRAppInterface> _aprappinterface;
        public IGenericRepository<APRAppInterface> APRAppInterface
        {
            get
            {
                return _aprappinterface ?? (_aprappinterface = new GenericRepository<APRAppInterface>(context));
            }
        }

        private IGenericRepository<ClientTicketInfo> _clientticketinfo;
        public IGenericRepository<ClientTicketInfo> ClientTicketInfo
        {
            get
            {
                return _clientticketinfo ?? (_clientticketinfo = new GenericRepository<ClientTicketInfo>(context));
            }
        }



        private IGenericRepository<ClientStatus> _ClientStatus;
        public IGenericRepository<ClientStatus> ClientStatus
        {
            get
            {
                return _ClientStatus ?? (_ClientStatus = new GenericRepository<ClientStatus>(context));
            }
        }



        //End

        //added Raj 2/15/2018
        private IGenericRepository<Personnel> _personnel;
        public IGenericRepository<Personnel> Personnel
        {
            get
            {
                return _personnel ?? (_personnel = new GenericRepository<Personnel>(context));
            }
        }

        private IGenericRepository<PersonnelRole> _personnelRole;
        public IGenericRepository<PersonnelRole> PersonnelRole
        {
            get
            {
                return _personnelRole ?? (_personnelRole = new GenericRepository<PersonnelRole>(context));
            }
        }
        //end

        private IGenericRepository<LineOfBusinesses> _lineOfBusinesses;
        public IGenericRepository<LineOfBusinesses> LineOfBusinesses
        {
            get
            {
                return _lineOfBusinesses ?? (_lineOfBusinesses = new GenericRepository<LineOfBusinesses>(context));
            }
        }

        private IGenericRepository<ClientAccessControl> _ClientAccessControl;
        public IGenericRepository<ClientAccessControl> ClientAccessControl
        {
            get
            {
                return _ClientAccessControl ?? (_ClientAccessControl = new GenericRepository<ClientAccessControl>(context));
            }
        }



        private IGenericRepository<ApplicationLOBJunctions> _applicationLOBJunctions;
        public IGenericRepository<ApplicationLOBJunctions> ApplicationLOBJunctions
        {
            get
            {
                return _applicationLOBJunctions ?? (_applicationLOBJunctions = new GenericRepository<ApplicationLOBJunctions>(context));
            }
        }


        //Survey related
        private IGenericRepository<Survey> _survey;
        public IGenericRepository<Survey> Survey
        {
            get
            {
                return _survey ?? (_survey = new GenericRepository<Survey>(context));
            }
        }

        private IGenericRepository<SurveyModel> _surveyModel;
        public IGenericRepository<SurveyModel> SurveyModel
        {
            get
            {
                return _surveyModel ?? (_surveyModel = new GenericRepository<SurveyModel>(context));
            }
        }
        private IGenericRepository<SurveyModelRatingFactor> _surveyModelRatingFactor;
        public IGenericRepository<SurveyModelRatingFactor> SurveyModelRatingFactor
        {
            get
            {
                return _surveyModelRatingFactor ?? (_surveyModelRatingFactor = new GenericRepository<SurveyModelRatingFactor>(context));
            }
        }
        private IGenericRepository<SurveyCollection> _surveyCollection;
        public IGenericRepository<SurveyCollection> SurveyCollection
        {
            get
            {
                return _surveyCollection ?? (_surveyCollection = new GenericRepository<SurveyCollection>(context));
            }
        }
        private IGenericRepository<SurveyPerspective> _surveyPerspective;
        public IGenericRepository<SurveyPerspective> SurveyPerspective
        {
            get
            {
                return _surveyPerspective ?? (_surveyPerspective = new GenericRepository<SurveyPerspective>(context));
            }
        }

        private IGenericRepository<SurveyUser> _surveyUser;
        public IGenericRepository<SurveyUser> SurveyUser
        {
            get
            {
                return _surveyUser ?? (_surveyUser = new GenericRepository<SurveyUser>(context));
            }
        }
        private IGenericRepository<PerspectiveCategory> _perspectiveCategory;
        public IGenericRepository<PerspectiveCategory> PerspectiveCategory
        {
            get
            {
                return _perspectiveCategory ?? (_perspectiveCategory = new GenericRepository<PerspectiveCategory>(context));
            }
        }

        private IGenericRepository<Vendor> _vendor;
        public IGenericRepository<Vendor> Vendor
        {
            get
            {
                return _vendor ?? (_vendor = new GenericRepository<Vendor>(context));
            }
        }

        private IGenericRepository<VendorProduct> _vendorProduct;
        public IGenericRepository<VendorProduct> VendorProduct
        {
            get
            {
                return _vendorProduct ?? (_vendorProduct = new GenericRepository<VendorProduct>(context));
            }
        }

        private IGenericRepository<VendorProductVersion> _vendorProductVersion;
        public IGenericRepository<VendorProductVersion> VendorProductVersion
        {
            get
            {
                return _vendorProductVersion ?? (_vendorProductVersion = new GenericRepository<VendorProductVersion>(context));
            }
        }

        private IGenericRepository<APRHostList> _aprHostlist;
        public IGenericRepository<APRHostList> APRHostList
        {
            get
            {
                return _aprHostlist ?? (_aprHostlist = new GenericRepository<APRHostList>(context));
            }
        }


        private IGenericRepository<APRAppHardware> _aprAppHardware;
        public IGenericRepository<APRAppHardware> APRAppHardware
        {
            get
            {
                return _aprAppHardware ?? (_aprAppHardware = new GenericRepository<APRAppHardware>(context));
            }
        }

        private IGenericRepository<APRAppSoftware> _aprAppSoftware;
        public IGenericRepository<APRAppSoftware> APRAppSoftware
        {
            get
            {
                return _aprAppSoftware ?? (_aprAppSoftware = new GenericRepository<APRAppSoftware>(context));
            }
        }


        //Idea Manager
        private IGenericRepository<IMPostIdea> _imPostIdea;
        public IGenericRepository<IMPostIdea> IMPostIdea
        {
            get
            {
                return _imPostIdea ?? (_imPostIdea = new GenericRepository<IMPostIdea>(context));
            }
        }

        private IGenericRepository<IMPostChallenge> _imPostChallenge;
        public IGenericRepository<IMPostChallenge> IMPostChallenege
        {
            get
            {
                return _imPostChallenge ?? (_imPostChallenge = new GenericRepository<IMPostChallenge>(context));
            }
        }

        private IGenericRepository<IMIdeaType> _imIdeaType;
        public IGenericRepository<IMIdeaType> IMIdeaType
        {
            get
            {
                return _imIdeaType ?? (_imIdeaType = new GenericRepository<IMIdeaType>(context));
            }
        }

        private IGenericRepository<IMIdeaSource> _imIdeaSource;
        public IGenericRepository<IMIdeaSource> IMIdeaSource
        {
            get
            {
                return _imIdeaSource ?? (_imIdeaSource = new GenericRepository<IMIdeaSource>(context));
            }
        }

        private IGenericRepository<IMLOB> _imlob;
        public IGenericRepository<IMLOB> IMLOB
        {
            get
            {
                return _imlob ?? (_imlob = new GenericRepository<IMLOB>(context));
            }
        }

        private IGenericRepository<IMClassification> _imClassification;
        public IGenericRepository<IMClassification> IMClassification
        {
            get
            {
                return _imClassification ?? (_imClassification = new GenericRepository<IMClassification>(context));
            }
        }

        private IGenericRepository<IMCategory> _imCategory;
        public IGenericRepository<IMCategory> IMCategory
        {
            get
            {
                return _imCategory ?? (_imCategory = new GenericRepository<IMCategory>(context));
            }
        }
        //User application table - to find User assocaited to Client

        private IGenericRepository<UserApplications> _UserApplications;
        public IGenericRepository<UserApplications> UserApplications
        {
            get
            {
                return _UserApplications ?? (_UserApplications = new GenericRepository<UserApplications>(context));
            }
        }


        //Category Map -- IMItemCategoryMap
        private IGenericRepository<IMItemCategoryMap> _IMItemCategoryMap;
        public IGenericRepository<IMItemCategoryMap> IMItemCategoryMap
        {
            get
            {
                return _IMItemCategoryMap ?? (_IMItemCategoryMap = new GenericRepository<IMItemCategoryMap>(context));
            }
        }

        private IGenericRepository<IMItemPriority> _imItemPriority;
        public IGenericRepository<IMItemPriority> IMItemPriority
        {
            get
            {
                return _imItemPriority ?? (_imItemPriority = new GenericRepository<IMItemPriority>(context));
            }
        }

        private IGenericRepository<IMItemStatus> _imItemStatus;
        public IGenericRepository<IMItemStatus> IMItemStatus
        {
            get
            {
                return _imItemStatus ?? (_imItemStatus = new GenericRepository<IMItemStatus>(context));
            }
        }


        private IGenericRepository<IMIdeaStatusHistory> _imIdeaStatusHistory;
        public IGenericRepository<IMIdeaStatusHistory> IMIdeaStatusHistory
        {
            get
            {
                return _imIdeaStatusHistory ?? (_imIdeaStatusHistory = new GenericRepository<IMIdeaStatusHistory>(context));
            }
        }

        private IGenericRepository<IMIdeaPriorityHistory> _imIdeaPriorityHistory;
        public IGenericRepository<IMIdeaPriorityHistory> IMIdeaPriorityHistory
        {
            get
            {
                return _imIdeaPriorityHistory ?? (_imIdeaPriorityHistory = new GenericRepository<IMIdeaPriorityHistory>(context));
            }
        }
        //private IGenericRepository<IMChallengeStatusHistory> _imChallengeStatusHistory;
        //public IGenericRepository<IMChallengeStatusHistory> IMChallengeStatusHistory
        //{
        //    get
        //    {
        //        return _imChallengeStatusHistory ?? (_imChallengeStatusHistory = new GenericRepository<IMChallengeStatusHistory>(context));
        //    }
        //}

        //private IGenericRepository<IMChallengePriorityHistory> _imChallengePriorityHistory;
        //public IGenericRepository<IMChallengePriorityHistory> IMChallengePriorityHistory
        //{
        //    get
        //    {
        //        return _imChallengePriorityHistory ?? (_imChallengePriorityHistory = new GenericRepository<IMChallengePriorityHistory>(context));
        //    }
        //}
        //---------//

        private IGenericRepository<IMItemBusinessBenefits> _imItemBusinessBenefits;
        public IGenericRepository<IMItemBusinessBenefits> IMItemBusinessBenefits
        {
            get
            {
                return _imItemBusinessBenefits ?? (_imItemBusinessBenefits = new GenericRepository<IMItemBusinessBenefits>(context));
            }
        }

        //private IGenericRepository<IMChallengeBusinessBenefits> _imChallengeBusinessBenefits;
        //public IGenericRepository<IMChallengeBusinessBenefits> IMChallengeBusinessBenefits
        //{
        //    get
        //    {
        //        return _imChallengeBusinessBenefits ?? (_imChallengeBusinessBenefits = new GenericRepository<IMChallengeBusinessBenefits>(context));
        //    }
        //}

        private IGenericRepository<IMItemITBenefits> _imItemITBenefits;
        public IGenericRepository<IMItemITBenefits> IMItemITBenefits
        {
            get
            {
                return _imItemITBenefits ?? (_imItemITBenefits = new GenericRepository<IMItemITBenefits>(context));
            }
        }

        private IGenericRepository<ServiceImprovementType> _ServiceImprovementType;
        public IGenericRepository<ServiceImprovementType> ServiceImprovementType
        {
            get
            {
                return _ServiceImprovementType ?? (_ServiceImprovementType = new GenericRepository<ServiceImprovementType>(context));
            }
        }


        private IGenericRepository<ServiceImprovementGoal> _ServiceImprovementGoal;
        public IGenericRepository<ServiceImprovementGoal> ServiceImprovementGoal
        {
            get
            {
                return _ServiceImprovementGoal ?? (_ServiceImprovementGoal = new GenericRepository<ServiceImprovementGoal>(context));
            }
        }



        private IGenericRepository<ServiceImprovementPlan_Q> _ServiceImprovementPlan_Q;
        public IGenericRepository<ServiceImprovementPlan_Q> ServiceImprovementPlan_Q
        {
            get
            {
                return _ServiceImprovementPlan_Q ?? (_ServiceImprovementPlan_Q = new GenericRepository<ServiceImprovementPlan_Q>(context));
            }
        }



        private IGenericRepository<ServiceImprovementPlanStatus> _ServiceImprovementPlanStatus;
        public IGenericRepository<ServiceImprovementPlanStatus> ServiceImprovementPlanStatus
        {
            get
            {
                return _ServiceImprovementPlanStatus ?? (_ServiceImprovementPlanStatus = new GenericRepository<ServiceImprovementPlanStatus>(context));
            }
        }
        //private IGenericRepository<IMChallengeITBenefits> _imChallengeITBenefits;
        //public IGenericRepository<IMChallengeITBenefits> IMChallengeITBenefits
        //{
        //    get
        //    {
        //        return _imChallengeITBenefits ?? (_imChallengeITBenefits = new GenericRepository<IMChallengeITBenefits>(context));
        //    }
        //}

        private IGenericRepository<IMIdeaApprover> _imIdeaApprover;
        public IGenericRepository<IMIdeaApprover> IMIdeaApprover
        {
            get
            {
                return _imIdeaApprover ?? (_imIdeaApprover = new GenericRepository<IMIdeaApprover>(context));
            }
        }

        //private IGenericRepository<IMChallengeApprover> _imChallengeApprover;
        //public IGenericRepository<IMChallengeApprover> IMChallengeApprover
        //{
        //    get
        //    {
        //        return _imChallengeApprover ?? (_imChallengeApprover = new GenericRepository<IMChallengeApprover>(context));
        //    }
        //}


        private IGenericRepository<IMMetricTarget> _imMetricTarget;
        public IGenericRepository<IMMetricTarget> IMMetricTarget
        {
            get
            {
                return _imMetricTarget ?? (_imMetricTarget = new GenericRepository<IMMetricTarget>(context));
            }
        }
        //---//
        private IGenericRepository<IMProgressStatus> _imProgressStatus;
        public IGenericRepository<IMProgressStatus> IMProgressStatus
        {
            get
            {
                return _imProgressStatus ?? (_imProgressStatus = new GenericRepository<IMProgressStatus>(context));
            }
        }

        private IGenericRepository<IMStage> _imIMStage;
        public IGenericRepository<IMStage> IMStage
        {
            get
            {
                return _imIMStage ?? (_imIMStage = new GenericRepository<IMStage>(context));
            }
        }

        private IGenericRepository<IMStatuss> _imIMStatus;
        public IGenericRepository<IMStatuss> IMStatus
        {
            get
            {
                return _imIMStatus ?? (_imIMStatus = new GenericRepository<IMStatuss>(context));
            }
        }

        private IGenericRepository<IMPhase> _imIMPhase;
        public IGenericRepository<IMPhase> IMPhase
        {
            get
            {
                return _imIMPhase ?? (_imIMPhase = new GenericRepository<IMPhase>(context));
            }
        }

        private IGenericRepository<IMRole> _imRole;
        public IGenericRepository<IMRole> IMRole
        {
            get
            {
                return _imRole ?? (_imRole = new GenericRepository<IMRole>(context));
            }
        }

        private IGenericRepository<IMResource> _imResource;
        public IGenericRepository<IMResource> IMResource
        {
            get
            {
                return _imResource ?? (_imResource = new GenericRepository<IMResource>(context));
            }
        }


        private IGenericRepository<IMProgressTemplate> _imProgressTemplate;
        public IGenericRepository<IMProgressTemplate> IMProgressTemplate
        {
            get
            {
                return _imProgressTemplate ?? (_imProgressTemplate = new GenericRepository<IMProgressTemplate>(context));
            }
        }


        private IGenericRepository<IMWbsTemplate> _imWbsTemplate;
        public IGenericRepository<IMWbsTemplate> IMWbsTemplate
        {
            get
            {
                return _imWbsTemplate ?? (_imWbsTemplate = new GenericRepository<IMWbsTemplate>(context));
            }
        }

        //-----------//

        private IGenericRepository<IMIdeaTask> _imIdeaTask;
        public IGenericRepository<IMIdeaTask> IMIdeaTask
        {
            get
            {
                return _imIdeaTask ?? (_imIdeaTask = new GenericRepository<IMIdeaTask>(context));
            }
        }

        private IGenericRepository<IMTaskDependency> _imTaskDependency;
        public IGenericRepository<IMTaskDependency> IMTaskDependency
        {
            get
            {
                return _imTaskDependency ?? (_imTaskDependency = new GenericRepository<IMTaskDependency>(context));
            }
        }

        private IGenericRepository<IMTaskResource> _imTaskResource;
        public IGenericRepository<IMTaskResource> IMTaskResource
        {
            get
            {
                return _imTaskResource ?? (_imTaskResource = new GenericRepository<IMTaskResource>(context));
            }
        }

        private IGenericRepository<IMAnalysisRootCause> _IMAnalysisRootCause;
        public IGenericRepository<IMAnalysisRootCause> IMAnalysisRootCause
        {
            get
            {
                return _IMAnalysisRootCause ?? (_IMAnalysisRootCause = new GenericRepository<IMAnalysisRootCause>(context));
            }
        }

        private IGenericRepository<IMCompletedAndNextSteps> _IMCompletedAndNextSteps;
        public IGenericRepository<IMCompletedAndNextSteps> IMCompletedAndNextSteps
        {
            get
            {
                return _IMCompletedAndNextSteps ?? (_IMCompletedAndNextSteps = new GenericRepository<IMCompletedAndNextSteps>(context));
            }
        }
        private IGenericRepository<IMComplexity> _IMComplexity;
        public IGenericRepository<IMComplexity> IMComplexity
        {
            get
            {
                return _IMComplexity ?? (_IMComplexity = new GenericRepository<IMComplexity>(context));
            }
        }
        private IGenericRepository<IMConfigItem> _IMConfigItem;
        public IGenericRepository<IMConfigItem> IMConfigItem
        {
            get
            {
                return _IMConfigItem ?? (_IMConfigItem = new GenericRepository<IMConfigItem>(context));
            }
        }
        private IGenericRepository<IMConfigItemsMap> _IMConfigItemsMap;
        public IGenericRepository<IMConfigItemsMap> IMConfigItemsMap
        {
            get
            {
                return _IMConfigItemsMap ?? (_IMConfigItemsMap = new GenericRepository<IMConfigItemsMap>(context));
            }
        }
        private IGenericRepository<IMConfigType> _IMConfigType;
        public IGenericRepository<IMConfigType> IMConfigType
        {
            get
            {
                return _IMConfigType ?? (_IMConfigType = new GenericRepository<IMConfigType>(context));
            }
        }
        private IGenericRepository<IMImpPriorityHistory> _IMImpPriorityHistory;
        public IGenericRepository<IMImpPriorityHistory> IMImpPriorityHistory
        {
            get
            {
                return _IMImpPriorityHistory ?? (_IMImpPriorityHistory = new GenericRepository<IMImpPriorityHistory>(context));
            }
        }
        private IGenericRepository<IMRiskAndMitigation> _IMRiskAndMitigation;
        public IGenericRepository<IMRiskAndMitigation> IMRiskAndMitigation
        {
            get
            {
                return _IMRiskAndMitigation ?? (_IMRiskAndMitigation = new GenericRepository<IMRiskAndMitigation>(context));
            }
        }
        private IGenericRepository<IMSolution> _IMSolution;
        public IGenericRepository<IMSolution> IMSolution
        {
            get
            {
                return _IMSolution ?? (_IMSolution = new GenericRepository<IMSolution>(context));
            }
        }
        private IGenericRepository<IMTeam> _IMTeam;
        public IGenericRepository<IMTeam> IMTeam
        {
            get
            {
                return _IMTeam ?? (_IMTeam = new GenericRepository<IMTeam>(context));
            }
        }

        private IGenericRepository<ClientAccountContract> _ClientAccountContract;
        public IGenericRepository<ClientAccountContract> ClientAccountContract
        {
            get
            {
                return _ClientAccountContract ?? (_ClientAccountContract = new GenericRepository<ClientAccountContract>(context));
            }
        }



        private IGenericRepository<IMMetrics> _IMMetrics;
        public IGenericRepository<IMMetrics> IMMetrics
        {
            get
            {
                return _IMMetrics ?? (_IMMetrics = new GenericRepository<IMMetrics>(context));
            }
        }

        private IGenericRepository<IMItemComments> _imItemComments;
        public IGenericRepository<IMItemComments> IMItemComments
        {
            get
            {
                return _imItemComments ?? (_imItemComments = new GenericRepository<IMItemComments>(context));
            }
        }


        //Clustering Configuration -- IncidentNavigator[MasterData]
        private IGenericRepository<INClusterConfigurationKey> _inClusterConfigurationKey;
        public IGenericRepository<INClusterConfigurationKey> INClusterConfigurationKey
        {
            get
            {
                return _inClusterConfigurationKey ?? (_inClusterConfigurationKey = new GenericRepository<INClusterConfigurationKey>(context));
            }
        }
        private IGenericRepository<INClusterConfigurationValue> _inClusterConfigurationValue;
        public IGenericRepository<INClusterConfigurationValue> INClusterConfigurationValue
        {
            get
            {
                return _inClusterConfigurationValue ?? (_inClusterConfigurationValue = new GenericRepository<INClusterConfigurationValue>(context));
            }
        }
        private IGenericRepository<UserActionTracking> _userActionTracking;
        public IGenericRepository<UserActionTracking> UserActionTracking
        {
            get
            {
                return _userActionTracking ?? (_userActionTracking = new GenericRepository<UserActionTracking>(context));
            }
        }

        //ActivityLog for ClusterConfiguration
        private IGenericRepository<INClusterActivityLog> _inClusterActivityLog;
        public IGenericRepository<INClusterActivityLog> INClusterActivityLog
        {
            get
            {
                return _inClusterActivityLog ?? (_inClusterActivityLog = new GenericRepository<INClusterActivityLog>(context));
            }
        }

        //For PIData Masking
        private IGenericRepository<PIDataMaskList> _piDataMaskList;
        public IGenericRepository<PIDataMaskList> PIDataMaskList
        {
            get
            {
                return _piDataMaskList ?? (_piDataMaskList = new GenericRepository<PIDataMaskList>(context));
            }
        }


        private IGenericRepository<SourceFieldCalculate> _SourceFieldCalculate;
        public IGenericRepository<SourceFieldCalculate> SourceFieldCalculate
        {
            get
            {
                return _SourceFieldCalculate ?? (_SourceFieldCalculate = new GenericRepository<SourceFieldCalculate>(context));
            }
        }


        //----------------------Analysis Report Saving-----------------------//
        private IGenericRepository<Report> _report;
        public IGenericRepository<Report> Report
        {
            get
            {
                return _report ?? (_report = new GenericRepository<Report>(context));
            }
        }

        private IGenericRepository<ReportView> _reportView;
        public IGenericRepository<ReportView> ReportView
        {
            get
            {
                return _reportView ?? (_reportView = new GenericRepository<ReportView>(context));
            }
        }
        private IGenericRepository<ReportViewField> _reportViewField;
        public IGenericRepository<ReportViewField> ReportViewField
        {
            get
            {
                return _reportViewField ?? (_reportViewField = new GenericRepository<ReportViewField>(context));
            }
        }

        private IGenericRepository<ReportFieldType> _reportFieldType;
        public IGenericRepository<ReportFieldType> ReportFieldType
        {
            get
            {
                return _reportFieldType ?? (_reportFieldType = new GenericRepository<ReportFieldType>(context));
            }
        }

        private IGenericRepository<ReportViewFilter> _reportViewFilter;
        public IGenericRepository<ReportViewFilter> ReportViewFilter
        {
            get
            {
                return _reportViewFilter ?? (_reportViewFilter = new GenericRepository<ReportViewFilter>(context));
            }
        }

        //-------------------------------------------------------------------------------//

        //---Framework Builder

        private IGenericRepository<RAWbsTemplateDetails> _WbsTemplateDetails;
        public IGenericRepository<RAWbsTemplateDetails> RAWbsTemplateDetails
        {
            get
            {
                return _WbsTemplateDetails ?? (_WbsTemplateDetails = new GenericRepository<RAWbsTemplateDetails>(context));
            }
        }

        private IGenericRepository<RARoles> _Roles;
        public IGenericRepository<RARoles> RARoles
        {
            get
            {
                return _Roles ?? (_Roles = new GenericRepository<RARoles>(context));
            }
        }

        private IGenericRepository<RAResourceGroup> _ResourceGroup;
        public IGenericRepository<RAResourceGroup> RAResourceGroup
        {
            get
            {
                return _ResourceGroup ?? (_ResourceGroup = new GenericRepository<RAResourceGroup>(context));
            }
        }

        private IGenericRepository<RAResource> _Resource;
        public IGenericRepository<RAResource> RAResource
        {
            get
            {
                return _Resource ?? (_Resource = new GenericRepository<RAResource>(context));
            }
        }

        private IGenericRepository<RARetirementPhase> _RetirementPhase;
        public IGenericRepository<RARetirementPhase> RARetirementPhase
        {
            get
            {
                return _RetirementPhase ?? (_RetirementPhase = new GenericRepository<RARetirementPhase>(context));
            }
        }


        private IGenericRepository<RARetirementStage> _RetirementStage;
        public IGenericRepository<RARetirementStage> RARetirementStage
        {
            get
            {
                return _RetirementStage ?? (_RetirementStage = new GenericRepository<RARetirementStage>(context));
            }
        }

        private IGenericRepository<RAWbsStatus> _WbsStatus;
        public IGenericRepository<RAWbsStatus> RAWbsStatus
        {
            get
            {
                return _WbsStatus ?? (_WbsStatus = new GenericRepository<RAWbsStatus>(context));
            }
        }

        private IGenericRepository<RAWorkProduct> _WorkProduct;
        public IGenericRepository<RAWorkProduct> RAWorkProduct
        {
            get
            {
                return _WorkProduct ?? (_WorkProduct = new GenericRepository<RAWorkProduct>(context));
            }
        }

        private IGenericRepository<RAProjectHealth> _ProjectHealth;
        public IGenericRepository<RAProjectHealth> RAProjectHealth
        {
            get
            {
                return _ProjectHealth ?? (_ProjectHealth = new GenericRepository<RAProjectHealth>(context));
            }
        }


        private IGenericRepository<RAWbsTemplate> _WbsTemplate;
        public IGenericRepository<RAWbsTemplate> RAWbsTemplate
        {
            get
            {
                return _WbsTemplate ?? (_WbsTemplate = new GenericRepository<RAWbsTemplate>(context));
            }
        }


        private IGenericRepository<RAEstimateCategory> _raEstimateCategory;
        public IGenericRepository<RAEstimateCategory> RAEstimateCategory
        {
            get
            {
                return _raEstimateCategory ?? (_raEstimateCategory = new GenericRepository<RAEstimateCategory>(context));
            }
        }

        private IGenericRepository<RAReplacementScope> _ReplacementScope;
        public IGenericRepository<RAReplacementScope> RAReplacementScope
        {
            get
            {
                return _ReplacementScope ?? (_ReplacementScope = new GenericRepository<RAReplacementScope>(context));
            }
        }

        private IGenericRepository<RAAssessment> _RAAssessment;
        public IGenericRepository<RAAssessment> RAAssessment
        {
            get
            {
                return _RAAssessment ?? (_RAAssessment = new GenericRepository<RAAssessment>(context));
            }
        }

        private IGenericRepository<RADispCandidate> _RADispCandidate;
        public IGenericRepository<RADispCandidate> RADispCandidate
        {
            get
            {
                return _RADispCandidate ?? (_RADispCandidate = new GenericRepository<RADispCandidate>(context));
            }
        }

        private IGenericRepository<RADispRecommendation> _RADispRecommendation;
        public IGenericRepository<RADispRecommendation> RADispRecommendation
        {
            get
            {
                return _RADispRecommendation ?? (_RADispRecommendation = new GenericRepository<RADispRecommendation>(context));
            }
        }

        private IGenericRepository<RAExecutionCostActivity> _RAExecutionCostActivity;
        public IGenericRepository<RAExecutionCostActivity> RAExecutionCostActivity
        {
            get
            {
                return _RAExecutionCostActivity ?? (_RAExecutionCostActivity = new GenericRepository<RAExecutionCostActivity>(context));
            }
        }

        private IGenericRepository<RASupportCostActivity> _RASupportCostActivity;
        public IGenericRepository<RASupportCostActivity> RASupportCostActivity
        {
            get
            {
                return _RASupportCostActivity ?? (_RASupportCostActivity = new GenericRepository<RASupportCostActivity>(context));
            }
        }

        private IGenericRepository<RAProjectManagementActivity> _RAProjectManagementActivity;
        public IGenericRepository<RAProjectManagementActivity> RAProjectManagementActivity
        {
            get
            {
                return _RAProjectManagementActivity ?? (_RAProjectManagementActivity = new GenericRepository<RAProjectManagementActivity>(context));
            }
        }

        private IGenericRepository<RAOngoingArchivalActivity> _RAOngoingArchivalActivity;
        public IGenericRepository<RAOngoingArchivalActivity> RAOngoingArchivalActivity
        {
            get
            {
                return _RAOngoingArchivalActivity ?? (_RAOngoingArchivalActivity = new GenericRepository<RAOngoingArchivalActivity>(context));
            }
        }

        private IGenericRepository<RASupportCost> _RASupportCost;
        public IGenericRepository<RASupportCost> RASupportCost
        {
            get
            {
                return _RASupportCost ?? (_RASupportCost = new GenericRepository<RASupportCost>(context));
            }
        }

        private IGenericRepository<RAExecutionCost> _RAExecutionCost;
        public IGenericRepository<RAExecutionCost> RAExecutionCost
        {
            get
            {
                return _RAExecutionCost ?? (_RAExecutionCost = new GenericRepository<RAExecutionCost>(context));
            }
        }

        private IGenericRepository<RAProjectManagement> _RAProjectManagement;
        public IGenericRepository<RAProjectManagement> RAProjectManagement
        {
            get
            {
                return _RAProjectManagement ?? (_RAProjectManagement = new GenericRepository<RAProjectManagement>(context));
            }
        }

        private IGenericRepository<RAOngoingArchivalSupport> _RAOngoingArchivalSupport;
        public IGenericRepository<RAOngoingArchivalSupport> RAOngoingArchivalSupport
        {
            get
            {
                return _RAOngoingArchivalSupport ?? (_RAOngoingArchivalSupport = new GenericRepository<RAOngoingArchivalSupport>(context));
            }
        }

        private IGenericRepository<EstimateTypes> _EstimateType;
        public IGenericRepository<EstimateTypes> EstimateType
        {
            get
            {
                return _EstimateType ?? (_EstimateType = new GenericRepository<EstimateTypes>(context));
            }
        }

        private IGenericRepository<EstimateCategories> _EstimateCategory;
        public IGenericRepository<EstimateCategories> EstimateCategory
        {
            get
            {
                return _EstimateCategory ?? (_EstimateCategory = new GenericRepository<EstimateCategories>(context));
            }
        }

        private IGenericRepository<RATask> _RATask;
        public IGenericRepository<RATask> RATask
        {
            get
            {
                return _RATask ?? (_RATask = new GenericRepository<RATask>(context));
            }
        }
        private IGenericRepository<RATaskResource> _RATaskResource;
        public IGenericRepository<RATaskResource> RATaskResource
        {
            get
            {
                return _RATaskResource ?? (_RATaskResource = new GenericRepository<RATaskResource>(context));
            }
        }
        private IGenericRepository<RATaskDependency> _RATaskDependency;
        public IGenericRepository<RATaskDependency> RATaskDependency
        {
            get
            {
                return _RATaskDependency ?? (_RATaskDependency = new GenericRepository<RATaskDependency>(context));
            }
        }



        private IGenericRepository<APRDeploymentModel> _deploymentModel;
        public IGenericRepository<APRDeploymentModel> DeploymentModel
        {
            get
            {
                return _deploymentModel ?? (_deploymentModel = new GenericRepository<APRDeploymentModel>(context));
            }
        }
        private IGenericRepository<APRDifferentiationLevel> _differentiationLevelModel;
        public IGenericRepository<APRDifferentiationLevel> DifferentiationLevelModel
        {
            get
            {
                return _differentiationLevelModel ?? (_differentiationLevelModel = new GenericRepository<APRDifferentiationLevel>(context));
            }
        }
        private IGenericRepository<APRStandardStatus> _standardStatusModel;
        public IGenericRepository<APRStandardStatus> StandardStatusModel
        {
            get
            {
                return _standardStatusModel ?? (_standardStatusModel = new GenericRepository<APRStandardStatus>(context));
            }
        }
        //APROwningCompany
        private IGenericRepository<APROwningCompany> _aprOwningCompany;
        public IGenericRepository<APROwningCompany> APROwningCompany
        {
            get
            {
                return _aprOwningCompany ?? (_aprOwningCompany = new GenericRepository<APROwningCompany>(context));
            }
        }
        private IGenericRepository<APRInterfaceMethod> _aPRInterfaceMethod;
        public IGenericRepository<APRInterfaceMethod> APRInterfaceMethod
        {
            get
            {
                return _aPRInterfaceMethod ?? (_aPRInterfaceMethod = new GenericRepository<APRInterfaceMethod>(context));
            }
        }
        private IGenericRepository<APRApplicationInterfaceMethod> _aPRApplicationInterfaceMethod;
        public IGenericRepository<APRApplicationInterfaceMethod> APRApplicationInterfaceMethod
        {
            get
            {
                return _aPRApplicationInterfaceMethod ?? (_aPRApplicationInterfaceMethod = new GenericRepository<APRApplicationInterfaceMethod>(context));
            }
        }
        private IGenericRepository<APRApplicationCountry> _aPRApplicationCountry;
        public IGenericRepository<APRApplicationCountry> APRApplicationCountry
        {
            get
            {
                return _aPRApplicationCountry ?? (_aPRApplicationCountry = new GenericRepository<APRApplicationCountry>(context));
            }
        }

        private IGenericRepository<APRRoadmapTargetApp> _aPRRoadmapTargetApp;
        public IGenericRepository<APRRoadmapTargetApp> APRRoadmapTargetApp
        {
            get
            {
                return _aPRRoadmapTargetApp ?? (_aPRRoadmapTargetApp = new GenericRepository<APRRoadmapTargetApp>(context));
            }
        }
        private IGenericRepository<APRDispAndDispSollution> _aPRDispAndDispSollution;
        public IGenericRepository<APRDispAndDispSollution> APRDispAndDispSollution
        {
            get
            {
                return _aPRDispAndDispSollution ?? (_aPRDispAndDispSollution = new GenericRepository<APRDispAndDispSollution>(context));
            }
        }
        private IGenericRepository<APRRedundancy> _aPRRedundancy;
        public IGenericRepository<APRRedundancy> APRRedundancy
        {
            get
            {
                return _aPRRedundancy ?? (_aPRRedundancy = new GenericRepository<APRRedundancy>(context));
            }
        }
        private IGenericRepository<APRDispositionSolution> _aPRDispositionSolution;
        public IGenericRepository<APRDispositionSolution> APRDispositionSolution
        {
            get
            {
                return _aPRDispositionSolution ?? (_aPRDispositionSolution = new GenericRepository<APRDispositionSolution>(context));
            }
        }

        private IGenericRepository<INReport> _INReport;
        public IGenericRepository<INReport> INReport
        {
            get
            {
                return _INReport ?? (_INReport = new GenericRepository<INReport>(context));
            }
        }

        private IGenericRepository<INClientReport> _INClientReport;
        public IGenericRepository<INClientReport> INClientReport
        {
            get
            {
                return _INClientReport ?? (_INClientReport = new GenericRepository<INClientReport>(context));
            }
        }

        private IGenericRepository<INReportObservation> _INReportObservation;
        public IGenericRepository<INReportObservation> INReportObservation
        {
            get
            {
                return _INReportObservation ?? (_INReportObservation = new GenericRepository<INReportObservation>(context));
            }
        }

        private IGenericRepository<INBusinessTower> _INBusinessTower;
        public IGenericRepository<INBusinessTower> INBusinessTower
        {
            get
            {
                return _INBusinessTower ?? (_INBusinessTower = new GenericRepository<INBusinessTower>(context));
            }
        }

        private IGenericRepository<INAssigneeGroup> _INAssigneeGroup;
        public IGenericRepository<INAssigneeGroup> INAssigneeGroup
        {
            get
            {
                return _INAssigneeGroup ?? (_INAssigneeGroup = new GenericRepository<INAssigneeGroup>(context));
            }
        }
        private IGenericRepository<SurveyINUser> _SurveyINUser;
        public IGenericRepository<SurveyINUser> SurveyINUser
        {
            get
            {
                return _SurveyINUser ?? (_SurveyINUser = new GenericRepository<SurveyINUser>(context));
            }
        }

        private IGenericRepository<ClientEngagement> _ClientEngagement;
        public IGenericRepository<ClientEngagement> ClientEngagement
        {
            get
            {
                return _ClientEngagement ?? (_ClientEngagement = new GenericRepository<ClientEngagement>(context));
            }
        }
        private IGenericRepository<ClientEngagementFTE_Q> _ClientEngagementFTE_Q;
        public IGenericRepository<ClientEngagementFTE_Q> ClientEngagementFTE_Q
        {
            get
            {
                return _ClientEngagementFTE_Q ?? (_ClientEngagementFTE_Q = new GenericRepository<ClientEngagementFTE_Q>(context));
            }
        }
        private IGenericRepository<EngagementLocation> _EngagementLocation;
        public IGenericRepository<EngagementLocation> EngagementLocation
        {
            get
            {
                return _EngagementLocation ?? (_EngagementLocation = new GenericRepository<EngagementLocation>(context));
            }
        }

        private IGenericRepository<UserLeadershipRole> _UserLeadershipRole;
        public IGenericRepository<UserLeadershipRole> UserLeadershipRole
        {
            get
            {
                return _UserLeadershipRole ?? (_UserLeadershipRole = new GenericRepository<UserLeadershipRole>(context));
            }
        }

        private IGenericRepository<LeadershipRoles> _LeadershipRoles;
        public IGenericRepository<LeadershipRoles> LeadershipRoles
        {
            get
            {
                return _LeadershipRoles ?? (_LeadershipRoles = new GenericRepository<LeadershipRoles>(context));
            }
        }

        private IGenericRepository<BaseLineData> _BaseLineData;
        public IGenericRepository<BaseLineData> BaseLineData
        {
            get
            {
                return _BaseLineData ?? (_BaseLineData = new GenericRepository<BaseLineData>(context));
            }
        }
        private IGenericRepository<TeamRule> _TeamRule;
        public IGenericRepository<TeamRule> TeamRule
        {
            get
            {
                return _TeamRule ?? (_TeamRule = new GenericRepository<TeamRule>(context));
            }
        }

        private IGenericRepository<INContractRule> _ContractRule;
        public IGenericRepository<INContractRule> INContractRule
        {
            get
            {
                return _ContractRule ?? (_ContractRule = new GenericRepository<INContractRule>(context));
            }
        }


        private IGenericRepository<INColumnValueRule> _ColumnRule;
        public IGenericRepository<INColumnValueRule> INColumnValueRule
        {
            get
            {
                return _ColumnRule ?? (_ColumnRule = new GenericRepository<INColumnValueRule>(context));
            }
        }

        private IGenericRepository<INColumnValue> _ColumnValue;
        public IGenericRepository<INColumnValue> INColumnValue
        {
            get
            {
                return _ColumnValue ?? (_ColumnValue = new GenericRepository<INColumnValue>(context));
            }
        }

        private IGenericRepository<INDictionary> _INDictionary;
        public IGenericRepository<INDictionary> INDictionary
        {
            get
            {
                return _INDictionary ?? (_INDictionary = new GenericRepository<INDictionary>(context));
            }
        }

        private IGenericRepository<SurveyINCollection> _SurveyINCollection;
        public IGenericRepository<SurveyINCollection> SurveyINCollection
        {
            get
            {
                return _SurveyINCollection ?? (_SurveyINCollection = new GenericRepository<SurveyINCollection>(context));
            }
        }
        private IGenericRepository<SSIS_INUpdateQuery> _SSIS_INUpdateQuery;
        public IGenericRepository<SSIS_INUpdateQuery> SSIS_INUpdateQuery
        {
            get
            {
                return _SSIS_INUpdateQuery ?? (_SSIS_INUpdateQuery = new GenericRepository<SSIS_INUpdateQuery>(context));
            }
        }


        private IGenericRepository<BCApplicationJunction> _BCApplicationJunction;
        public IGenericRepository<BCApplicationJunction> BCApplicationJunction
        {
            get
            {
                return _BCApplicationJunction ?? (_BCApplicationJunction = new GenericRepository<BCApplicationJunction>(context));
            }
        }
        private IGenericRepository<Businessprocesshealth> _Businesprocesshealth;
        public IGenericRepository<Businessprocesshealth> Businessprocesshealth
        {
            get
            {
                return _Businesprocesshealth ?? (_Businesprocesshealth = new GenericRepository<Businessprocesshealth>(context));
            }
        }
        private IGenericRepository<BusinessProcessImportance> _BusinessProcessImportance;
        public IGenericRepository<BusinessProcessImportance> BusinessProcessImportance
        {
            get
            {
                return _BusinessProcessImportance ?? (_BusinessProcessImportance = new GenericRepository<BusinessProcessImportance>(context));
            }
        }

        //Demand Manager
        private IGenericRepository<DMDemandType> _dmDemandType;
        public IGenericRepository<DMDemandType> DMDemandType
        {
            get
            {
                return _dmDemandType ?? (_dmDemandType = new GenericRepository<DMDemandType>(context));
            }
        }
        private IGenericRepository<DMLOB> _dmlob;
        public IGenericRepository<DMLOB> DMLOB
        {
            get
            {
                return _dmlob ?? (_dmlob = new GenericRepository<DMLOB>(context));
            }
        }
        private IGenericRepository<DMPostDemand> _dmPostdemand;
        public IGenericRepository<DMPostDemand> DMPostDemand
        {
            get
            {
                return _dmPostdemand ?? (_dmPostdemand = new GenericRepository<DMPostDemand>(context));
            }
        }
        private IGenericRepository<DMDemandApprover> _dmDemandApprover;
        public IGenericRepository<DMDemandApprover> DMDemandApprover
        {
            get
            {
                return _dmDemandApprover ?? (_dmDemandApprover = new GenericRepository<DMDemandApprover>(context));
            }
        }
        private IGenericRepository<DMClassification> _dmClassification;
        public IGenericRepository<DMClassification> DMClassification
        {
            get
            {
                return _dmClassification ?? (_dmClassification = new GenericRepository<DMClassification>(context));
            }
        }
        private IGenericRepository<DMCategory> _dmCategory;
        public IGenericRepository<DMCategory> DMCategory
        {
            get
            {
                return _dmCategory ?? (_dmCategory = new GenericRepository<DMCategory>(context));
            }
        }
        private IGenericRepository<DMItemCategoryMap> _DMItemCategoryMap;
        public IGenericRepository<DMItemCategoryMap> DMItemCategoryMap
        {
            get
            {
                return _DMItemCategoryMap ?? (_DMItemCategoryMap = new GenericRepository<DMItemCategoryMap>(context));
            }
        }
        private IGenericRepository<DMConfigItem> _DMConfigItem;
        public IGenericRepository<DMConfigItem> DMConfigItem
        {
            get
            {
                return _DMConfigItem ?? (_DMConfigItem = new GenericRepository<DMConfigItem>(context));
            }
        }
        private IGenericRepository<DMConfigItemsMap> _DMConfigItemsMap;
        public IGenericRepository<DMConfigItemsMap> DMConfigItemsMap
        {
            get
            {
                return _DMConfigItemsMap ?? (_DMConfigItemsMap = new GenericRepository<DMConfigItemsMap>(context));
            }
        }
        private IGenericRepository<DMConfigType> _DMConfigType;
        public IGenericRepository<DMConfigType> DMConfigType
        {
            get
            {
                return _DMConfigType ?? (_DMConfigType = new GenericRepository<DMConfigType>(context));
            }
        }

        private IGenericRepository<IMAutoApprovers> _IMAutoApprovers;
        public IGenericRepository<IMAutoApprovers> IMAutoApprovers
        {
            get
            {
                return _IMAutoApprovers ?? (_IMAutoApprovers = new GenericRepository<IMAutoApprovers>(context));
            }
        }

        private IGenericRepository<DMItemBusinessBenefits> _dmItemBusinessBenefits;
        public IGenericRepository<DMItemBusinessBenefits> DMItemBusinessBenefits
        {
            get
            {
                return _dmItemBusinessBenefits ?? (_dmItemBusinessBenefits = new GenericRepository<DMItemBusinessBenefits>(context));
            }
        }
        private IGenericRepository<DMItemITBenefits> _dmItemITBenefits;
        public IGenericRepository<DMItemITBenefits> DMItemITBenefits
        {
            get
            {
                return _dmItemITBenefits ?? (_dmItemITBenefits = new GenericRepository<DMItemITBenefits>(context));
            }
        }
        private IGenericRepository<DMAnalysisRootCause> _DMAnalysisRootCause;
        public IGenericRepository<DMAnalysisRootCause> DMAnalysisRootCause
        {
            get
            {
                return _DMAnalysisRootCause ?? (_DMAnalysisRootCause = new GenericRepository<DMAnalysisRootCause>(context));
            }
        }
        private IGenericRepository<DMSolution> _DMSolution;
        public IGenericRepository<DMSolution> DMSolution
        {
            get
            {
                return _DMSolution ?? (_DMSolution = new GenericRepository<DMSolution>(context));
            }
        }
        private IGenericRepository<DMImpPriorityHistory> _DMImpPriorityHistory;
        public IGenericRepository<DMImpPriorityHistory> DMImpPriorityHistory
        {
            get
            {
                return _DMImpPriorityHistory ?? (_DMImpPriorityHistory = new GenericRepository<DMImpPriorityHistory>(context));
            }
        }
        private IGenericRepository<DMRiskAndMitigation> _DMRiskAndMitigation;
        public IGenericRepository<DMRiskAndMitigation> DMRiskAndMitigation
        {
            get
            {
                return _DMRiskAndMitigation ?? (_DMRiskAndMitigation = new GenericRepository<DMRiskAndMitigation>(context));
            }
        }
        private IGenericRepository<DMMetricTarget> _dmMetricTarget;
        public IGenericRepository<DMMetricTarget> DMMetricTarget
        {
            get
            {
                return _dmMetricTarget ?? (_dmMetricTarget = new GenericRepository<DMMetricTarget>(context));
            }
        }
        private IGenericRepository<DMItemComments> _dmItemComments;
        public IGenericRepository<DMItemComments> DMItemComments
        {
            get
            {
                return _dmItemComments ?? (_dmItemComments = new GenericRepository<DMItemComments>(context));
            }
        }
        private IGenericRepository<DMCompletedAndNextSteps> _DMCompletedAndNextSteps;
        public IGenericRepository<DMCompletedAndNextSteps> DMCompletedAndNextSteps
        {
            get
            {
                return _DMCompletedAndNextSteps ?? (_DMCompletedAndNextSteps = new GenericRepository<DMCompletedAndNextSteps>(context));
            }
        }
        private IGenericRepository<DMPriorityHistory> _dmPriorityHistory;
        public IGenericRepository<DMPriorityHistory> DMPriorityHistory
        {
            get
            {
                return _dmPriorityHistory ?? (_dmPriorityHistory = new GenericRepository<DMPriorityHistory>(context));
            }
        }
        private IGenericRepository<DMStatusHistory> _dmStatusHistory;
        public IGenericRepository<DMStatusHistory> DMStatusHistory
        {
            get
            {
                return _dmStatusHistory ?? (_dmStatusHistory = new GenericRepository<DMStatusHistory>(context));
            }
        }
        private IGenericRepository<DMItemStatus> _dmItemStatus;
        public IGenericRepository<DMItemStatus> DMItemStatus
        {
            get
            {
                return _dmItemStatus ?? (_dmItemStatus = new GenericRepository<DMItemStatus>(context));
            }
        }
        private IGenericRepository<DMPostChallenge> _dmPostChallenge;
        public IGenericRepository<DMPostChallenge> DMPostChallenege
        {
            get
            {
                return _dmPostChallenge ?? (_dmPostChallenge = new GenericRepository<DMPostChallenge>(context));
            }
        }
        private IGenericRepository<DMProgressStatus> _dmProgressStatus;
        public IGenericRepository<DMProgressStatus> DMProgressStatus
        {
            get
            {
                return _dmProgressStatus ?? (_dmProgressStatus = new GenericRepository<DMProgressStatus>(context));
            }
        }
        private IGenericRepository<DMComplexity> _DMComplexity;
        public IGenericRepository<DMComplexity> DMComplexity
        {
            get
            {
                return _DMComplexity ?? (_DMComplexity = new GenericRepository<DMComplexity>(context));
            }
        }
        private IGenericRepository<DMDocumentType> _DMDocumentType;
        public IGenericRepository<DMDocumentType> DMDocumentType
        {
            get
            {
                return _DMDocumentType ?? (_DMDocumentType = new GenericRepository<DMDocumentType>(context));
            }
        }
        
        private IGenericRepository<DMTask> _dmTask;
        public IGenericRepository<DMTask> DMTask
        {
            get
            {
                return _dmTask ?? (_dmTask = new GenericRepository<DMTask>(context));
            }
        }
        private IGenericRepository<DMStage> _DMStage;
        public IGenericRepository<DMStage> DMStage
        {
            get
            {
                return _DMStage ?? (_DMStage = new GenericRepository<DMStage>(context));
            }
        }
        private IGenericRepository<DMTaskAndActivities> _DMTaskAndActivities;
        public IGenericRepository<DMTaskAndActivities> DMTaskAndActivities
        {
            get
            {
                return _DMTaskAndActivities ?? (_DMTaskAndActivities = new GenericRepository<DMTaskAndActivities>(context));
            }
        }
        private IGenericRepository<DMTaskActivitiesHistory> _DMTaskActivitiesHistory;
        public IGenericRepository<DMTaskActivitiesHistory> DMTaskActivitiesHistory
        {
            get
            {
                return _DMTaskActivitiesHistory ?? (_DMTaskActivitiesHistory = new GenericRepository<DMTaskActivitiesHistory>(context));
            }
        }
        private IGenericRepository<ServiceCalendar> _ServiceCalendar;
        public IGenericRepository<ServiceCalendar> ServiceCalendar
        {
            get
            {
                return _ServiceCalendar ?? (_ServiceCalendar = new GenericRepository<ServiceCalendar>(context));
            }
        }
        private IGenericRepository<RPTJobDetail> _RPTJobDetail;
        public IGenericRepository<RPTJobDetail> RPTJobDetail
        {
            get
            {
                return _RPTJobDetail ?? (_RPTJobDetail = new GenericRepository<RPTJobDetail>(context));
            }
        }
        private IGenericRepository<RPTJobDetailArchive> _RPTJobDetailArchive;
        public IGenericRepository<RPTJobDetailArchive> RPTJobDetailArchive
        {
            get
            {
                return _RPTJobDetailArchive ?? (_RPTJobDetailArchive = new GenericRepository<RPTJobDetailArchive>(context));
            }
        }

        private IGenericRepository<ConfigAgingQuery> _ConfigAgingQuery;
        public IGenericRepository<ConfigAgingQuery> ConfigAgingQuery
        {
            get
            {
                return _ConfigAgingQuery ?? (_ConfigAgingQuery = new GenericRepository<ConfigAgingQuery>(context));
            }
        }
        private IGenericRepository<Verticals> _Verticals;
        public IGenericRepository<Verticals> Verticals
        {
            get
            {
                return _Verticals ?? (_Verticals = new GenericRepository<Verticals>(context));
            }
        }
        private IGenericRepository<SubVerticals> _SubVerticals;
        public IGenericRepository<SubVerticals> SubVerticals
        {
            get
            {
                return _SubVerticals ?? (_SubVerticals = new GenericRepository<SubVerticals>(context));
            }
        }
        private IGenericRepository<MetricSpecification> _MetricSpecification;
        public IGenericRepository<MetricSpecification> MetricSpecification
        {
            get
            {
                return _MetricSpecification ?? (_MetricSpecification = new GenericRepository<MetricSpecification>(context));
            }
        }


        private IGenericRepository<Metric> _Metric;
        public IGenericRepository<Metric> Metric
        {
            get
            {
                return _Metric ?? (_Metric = new GenericRepository<Metric>(context));
            }
        }

        //Feedback Manager
        private IGenericRepository<IMFDBKFeedbackType> _feedbackType;
        public IGenericRepository<IMFDBKFeedbackType> FeedbackType
        {
            get
            {
                return _feedbackType ?? (_feedbackType = new GenericRepository<IMFDBKFeedbackType>(context));
            }
        }
        private IGenericRepository<IMFDBKProductApp> _feedbackProduct;
        public IGenericRepository<IMFDBKProductApp> FeedbackProduct
        {
            get
            {
                return _feedbackProduct ?? (_feedbackProduct = new GenericRepository<IMFDBKProductApp>(context));
            }
        }
        private IGenericRepository<IMFDBKDigitalOffering> _feedbackDigital;
        public IGenericRepository<IMFDBKDigitalOffering> FeedbackDigital
        {
            get
            {
                return _feedbackDigital ?? (_feedbackDigital = new GenericRepository<IMFDBKDigitalOffering>(context));
            }
        }
        private IGenericRepository<InternalClient> _InternalClient;
        public IGenericRepository<InternalClient> InternalClient
        {
            get
            {
                return _InternalClient ?? (_InternalClient = new GenericRepository<InternalClient>(context));
            }
        }
        private IGenericRepository<IMFDBKPost> _IMFDBKPost;
        public IGenericRepository<IMFDBKPost> IMFDBKPost
        {
            get
            {
                return _IMFDBKPost ?? (_IMFDBKPost = new GenericRepository<IMFDBKPost>(context));
            }
        }
        private IGenericRepository<IMFDBKStatus> _IMFDBKStatus;
        public IGenericRepository<IMFDBKStatus> IMFDBKStatus
        {
            get
            {
                return _IMFDBKStatus ?? (_IMFDBKStatus = new GenericRepository<IMFDBKStatus>(context));
            }
        }
        private IGenericRepository<IMFDBKUser> _IMFDBKUser;
        public IGenericRepository<IMFDBKUser> IMFDBKUser
        {
            get
            {
                return _IMFDBKUser ?? (_IMFDBKUser = new GenericRepository<IMFDBKUser>(context));
            }
        }
        private IGenericRepository<IMFDBKProductMgr> _IMFDBKProductMgr;
        public IGenericRepository<IMFDBKProductMgr> IMFDBKProductMgr
        {
            get
            {
                return _IMFDBKProductMgr ?? (_IMFDBKProductMgr = new GenericRepository<IMFDBKProductMgr>(context));
            }
        }
        private IGenericRepository<IMFDBKDgtlOfferingMgr> _IMFDBKDgtlOfferingMgr;
        public IGenericRepository<IMFDBKDgtlOfferingMgr> IMFDBKDgtlOfferingMgr
        {
            get
            {
                return _IMFDBKDgtlOfferingMgr ?? (_IMFDBKDgtlOfferingMgr = new GenericRepository<IMFDBKDgtlOfferingMgr>(context));
            }
        }
        private IGenericRepository<IMFDBKClient> _IMFDBKClient;
        public IGenericRepository<IMFDBKClient> IMFDBKClient
        {
            get
            {
                return _IMFDBKClient ?? (_IMFDBKClient = new GenericRepository<IMFDBKClient>(context));
            }
        }
        private IGenericRepository<IMFDBKComment> _IMFDBKComment;
        public IGenericRepository<IMFDBKComment> IMFDBKComment
        {
            get
            {
                return _IMFDBKComment ?? (_IMFDBKComment = new GenericRepository<IMFDBKComment>(context));
            }
        }
        private IGenericRepository<IMFDBKVote> _IMFDBKVote;
        public IGenericRepository<IMFDBKVote> IMFDBKVote
        {
            get
            {
                return _IMFDBKVote ?? (_IMFDBKVote = new GenericRepository<IMFDBKVote>(context));
            }
        }
        private IGenericRepository<IMFDBKNotification> _IMFDBKNotification;
        public IGenericRepository<IMFDBKNotification> IMFDBKNotification
        {
            get
            {
                return _IMFDBKNotification ?? (_IMFDBKNotification = new GenericRepository<IMFDBKNotification>(context));
            }
        }
        private IGenericRepository<IMFDBKProductGroups> _IMFDBKProductGroups;
        public IGenericRepository<IMFDBKProductGroups> IMFDBKProductGroups
        {
            get
            {
                return _IMFDBKProductGroups ?? (_IMFDBKProductGroups = new GenericRepository<IMFDBKProductGroups>(context));
            }
        }

        private IGenericRepository<IMFDBKTag> _IMFDBKTag;
        public IGenericRepository<IMFDBKTag> IMFDBKTag
        {
            get
            {
                return _IMFDBKTag ?? (_IMFDBKTag = new GenericRepository<IMFDBKTag>(context));
            }
        }

        private IGenericRepository<IMFDBKMapTag> _IMFDBKMapTag;
        public IGenericRepository<IMFDBKMapTag> IMFDBKMapTag
        {
            get
            {
                return _IMFDBKMapTag ?? (_IMFDBKMapTag = new GenericRepository<IMFDBKMapTag>(context));
            }
        }
        private IGenericRepository<IMFDBKActivityLogs> _IMFDBKActivityLogs;
        public IGenericRepository<IMFDBKActivityLogs> IMFDBKActivityLogs
        {
            get
            {
                return _IMFDBKActivityLogs ?? (_IMFDBKActivityLogs = new GenericRepository<IMFDBKActivityLogs>(context));
            }
        }

        private IGenericRepository<IMFDBKAttachment> _IMFDBKAttachment;
        public IGenericRepository<IMFDBKAttachment> IMFDBKAttachment
        {
            get
            {
                return _IMFDBKAttachment ?? (_IMFDBKAttachment = new GenericRepository<IMFDBKAttachment>(context));
            }
        }


        private IGenericRepository<Discipline> _Discipline;
        public IGenericRepository<Discipline> Discipline
        {
            get
            {
                return _Discipline ?? (_Discipline = new GenericRepository<Discipline>(context));
            }
        }

        private IGenericRepository<Component> _Component;
        public IGenericRepository<Component> Component
        {
            get
            {
                return _Component ?? (_Component = new GenericRepository<Component>(context));
            }
        }

        private IGenericRepository<Aspect> _Aspect;
        public IGenericRepository<Aspect> Aspect
        {
            get
            {
                return _Aspect ?? (_Aspect = new GenericRepository<Aspect>(context));
            }
        }

        private IGenericRepository<Assessment> _Assessment;
        public IGenericRepository<Assessment> Assessment
        {
            get
            {
                return _Assessment ?? (_Assessment = new GenericRepository<Assessment>(context));
            }
        }
        private IGenericRepository<QuestRespAttachments> _QuestRespAttachments;
        public IGenericRepository<QuestRespAttachments> QuestRespAttachments
        {
            get
            {
                return _QuestRespAttachments ?? (_QuestRespAttachments = new GenericRepository<QuestRespAttachments>(context));
            }
        }

        

        private IGenericRepository<EngagementQuestionnaire> _EngagementQuestionnaire;
        public IGenericRepository<EngagementQuestionnaire> EngagementQuestionnaire
        {
            get
            {
                return _EngagementQuestionnaire ?? (_EngagementQuestionnaire = new GenericRepository<EngagementQuestionnaire>(context));
            }
        }


        private IGenericRepository<QuestionnaireResponse> _QuestionnaireResponse;
        public IGenericRepository<QuestionnaireResponse> QuestionnaireResponse
        {
            get
            {
                return _QuestionnaireResponse ?? (_QuestionnaireResponse = new GenericRepository<QuestionnaireResponse>(context));
            }
        }


        private IGenericRepository<EngagementQuestionSet> _EngagementQuestionSet;
        public IGenericRepository<EngagementQuestionSet> EngagementQuestionSet
        {
            get
            {
                return _EngagementQuestionSet ?? (_EngagementQuestionSet = new GenericRepository<EngagementQuestionSet>(context));
            }
        }


        

        

        private IGenericRepository<ActionItemTracker> _ActionItemTracker;
        public IGenericRepository<ActionItemTracker> ActionItemTracker
        {
            get
            {
                return _ActionItemTracker ?? (_ActionItemTracker = new GenericRepository<ActionItemTracker>(context));
            }
        }

        private IGenericRepository<ActionItemTrackerLog> _ActionItemTrackerLog;
        public IGenericRepository<ActionItemTrackerLog> ActionItemTrackerLog
        {
            get
            {
                return _ActionItemTrackerLog ?? (_ActionItemTrackerLog = new GenericRepository<ActionItemTrackerLog>(context));
            }
        }


        private IGenericRepository<A3MM_MultipleResponses> _A3MM_MultipleResponses;
        public IGenericRepository<A3MM_MultipleResponses> A3MM_MultipleResponses
        {
            get
            {
                return _A3MM_MultipleResponses ?? (_A3MM_MultipleResponses = new GenericRepository<A3MM_MultipleResponses>(context));
            }
        }

        private IGenericRepository<A3MM_MultipleFollowUpQuestions> _A3MM_MultipleFollowUpQuestions;
        public IGenericRepository<A3MM_MultipleFollowUpQuestions> A3MM_MultipleFollowUpQuestions
        {
            get
            {
                return _A3MM_MultipleFollowUpQuestions ?? (_A3MM_MultipleFollowUpQuestions = new GenericRepository<A3MM_MultipleFollowUpQuestions>(context));
            }
        }


        private IGenericRepository<A3SurveyType> _A3SurveyType;
        public IGenericRepository<A3SurveyType> A3SurveyType
        {
            get
            {
                return _A3SurveyType ?? (_A3SurveyType = new GenericRepository<A3SurveyType>(context));
            }
        }

        private IGenericRepository<SB_ScheduleEmailConfig> _SB_ScheduleEmailConfig;
        public IGenericRepository<SB_ScheduleEmailConfig> SB_ScheduleEmailConfig
        {
            get
            {
                return _SB_ScheduleEmailConfig ?? (_SB_ScheduleEmailConfig = new GenericRepository<SB_ScheduleEmailConfig>(context));
            }
        }

        private IGenericRepository<IMIdeaSourceMaster> _IMIdeaSourceMaster;
        public IGenericRepository<IMIdeaSourceMaster> IMIdeaSourceMaster
        {
            get
            {
                return _IMIdeaSourceMaster ?? (_IMIdeaSourceMaster = new GenericRepository<IMIdeaSourceMaster>(context));
            }
        }
        private IGenericRepository<IMEmailtemplates> _IMEmailtemplates;
        public IGenericRepository<IMEmailtemplates> IMEmailtemplates
        {
            get
            {
                return _IMEmailtemplates ?? (_IMEmailtemplates = new GenericRepository<IMEmailtemplates>(context));
            }
        }
        private IGenericRepository<IMOfflineApprover> _IMOfflineApprover;
        public IGenericRepository<IMOfflineApprover> IMOfflineApprover
        {
            get
            {
                return _IMOfflineApprover ?? (_IMOfflineApprover = new GenericRepository<IMOfflineApprover>(context));
            }
        }
        private IGenericRepository<IMIdeaReassign_log> _IMIdeaReassign_log;
        public IGenericRepository<IMIdeaReassign_log> IMIdeaReassign_log
        {
            get
            {
                return _IMIdeaReassign_log ?? (_IMIdeaReassign_log = new GenericRepository<IMIdeaReassign_log>(context));
            }
        }
        private IGenericRepository<ApprovalAttachments> _ApprovalAttachments;
        public IGenericRepository<ApprovalAttachments> ApprovalAttachments
        {
            get
            {
                return _ApprovalAttachments ?? (_ApprovalAttachments = new GenericRepository<ApprovalAttachments>(context));
            }
        }

        //-------------------------------------------------------------------------------//
        public void Save()
        {
            context.SaveChanges();
        }

        public void ExecProcedure(string StoredProcedureName, string[] ParametersName, params object[] ParametersValues)
        {
            var sqlConn = new SqlConnection(context.Database.Connection.ConnectionString);
            try
            {
                var datatable = new DataTable();

                var cmdRecords = new SqlCommand(StoredProcedureName, sqlConn);
                //var daReport = new SqlDataAdapter(cmdRecords);
                sqlConn.Open();
                using (cmdRecords)
                {
                    cmdRecords.CommandType = CommandType.StoredProcedure;
                    cmdRecords.CommandTimeout = 100;
                    int i = 0;
                    foreach (string oName in ParametersName)
                    {
                        SqlParameter ForeignKeyTablePrm = new SqlParameter(oName, ParametersValues[i]);
                        cmdRecords.Parameters.Add(ForeignKeyTablePrm);
                        i++;
                    }
                    cmdRecords.ExecuteNonQuery();
                }
            }
            finally
            {
                if (sqlConn.State == ConnectionState.Open)
                    sqlConn.Close();
            }

        }

        public DataTable GetDataFromStoredProcedure(string StoredProcedureName, string[] ParametersName, params object[] ParametersValues)
        {
            var datatable = new DataTable();
            var sqlConn = new SqlConnection(context.Database.Connection.ConnectionString);
            var cmdRecords = new SqlCommand(StoredProcedureName, sqlConn);
            var daReport = new SqlDataAdapter(cmdRecords);
            using (cmdRecords)
            {
                cmdRecords.CommandType = CommandType.StoredProcedure;
                cmdRecords.CommandTimeout = 100;
                int i = 0;
                foreach (string oName in ParametersName)
                {
                    SqlParameter ForeignKeyTablePrm = new SqlParameter(oName, ParametersValues[i]);
                    cmdRecords.Parameters.Add(ForeignKeyTablePrm);
                    i++;
                }
                daReport.Fill(datatable);
            }
            return datatable;

        }

        public DataSet GetDataFromStoredProcedureDS(string StoredProcedureName, string[] ParametersName, params object[] ParametersValues)
        {
            var ds = new DataSet();
            var sqlConn = new SqlConnection(context.Database.Connection.ConnectionString);
            var cmdRecords = new SqlCommand(StoredProcedureName, sqlConn);
            var daReport = new SqlDataAdapter(cmdRecords);
            using (cmdRecords)
            {
                cmdRecords.CommandType = CommandType.StoredProcedure;
                cmdRecords.CommandTimeout = 100;
                int i = 0;
                foreach (string oName in ParametersName)
                {
                    SqlParameter ForeignKeyTablePrm = new SqlParameter(oName, ParametersValues[i]);
                    cmdRecords.Parameters.Add(ForeignKeyTablePrm);
                    i++;
                }
                daReport.Fill(ds);
            }
            return ds;

        }



        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}
