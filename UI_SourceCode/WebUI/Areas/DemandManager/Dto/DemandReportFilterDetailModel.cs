using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.DemandManager.Dto
{
    public class DemandReportFilterDetailModel
    {
        public int UserId { get; set; }
        public string ReportName { get; set; }
        public int ReportId { get; set; }
        public string ViewName { get; set; }

        public int ReportViewId { get; set; }
        //public string FieldName { get; set; }
        //public int FieldType { get; set; }
        //public int Sequence { get; set; }

        public List<DemandReportFields> FieldsList { get; set; }

    }
    public class DemandReportFields
    {
        //public int ReportViewId { get; set; }
        public string FieldName { get; set; }
        public string FldType { get; set; }
        public int FieldType { get; set; }
        public int Sequence { get; set; }
    }
    
    public enum DemandReportFor
    {
        IdeaManager = 1,
        IncidentNavigator = 2,
        PortfolioAnalyzer = 3,
        ClientProfileManager = 4,
        ChallengeManager =5,
        ClusterAnalysis =6,
        InflowOutflowAnalysis = 7,
        BackLogAnalysis=8,
        IncidentNavigatorNew=9,
        DemandManager=10
    }
}