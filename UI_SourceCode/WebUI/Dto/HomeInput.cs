using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
using WebUI.Areas.ClientProfileManager.ViewModel;

namespace NTTData.AMOPortal.WebUI.Dto
{
    [Serializable]
    public class HomeInput //: Input
    {
        public int Id { get; set; }
        public Boolean PortfolioAnalyzer { get; set; }
        public Boolean BusinessAlignment { get; set; }
        public Boolean SLAManager { get; set; }
        public Boolean ServiceCatalog { get; set; }
        public Boolean IdeaManager { get; set; }
        public Boolean MigrationPlanner { get; set; }
        public Boolean RetirementAccelerator { get; set; }
        public Boolean KnowledgeManager { get; set; }
        public Boolean StaffingModelOptimizer { get; set; }
        public Boolean CloudMigrator { get; set; }
        public Boolean CodeAnalyzer { get; set; }
        public Boolean PerformancePredictor { get; set; }
        public Boolean IncidentNavigator { get; set; }
        public Boolean SourcingeffectivenessEvaluator { get; set; }
        public Boolean ClientProfileManager { get; set; }
        public string UserRole { get; set; }
        public int UserId { get; set; }
        public bool SupportEcosystem { get; set; }

        //data for ServiceDeliveryStats
        public ServiceDeliveryStatsModel ClientStats { get; set; }
        public Boolean TransformationAccelerator { get; set; }
        public Boolean DemandManager { get; set; }
        public Boolean FeedbackManager { get; set; }
        public Boolean AssessmentManager { get; set; }

        public string UserLoginId { get; set; }

        public string UserDisplayName { get; set; }
        public List<UserApplication> UserApplications { get; set; }

        [Serializable]
        public class UserApplication
        {
            public int UserId { get; set; }
            public int ClientId { get; set; }
            public int RoleId { get; set; }
            public int ApplicationId { get; set; }
            public string ApplicationName { get; set; }
            public string RoleName { get; set; }

            public bool isDeleted { get; set; }

        }

    }
}