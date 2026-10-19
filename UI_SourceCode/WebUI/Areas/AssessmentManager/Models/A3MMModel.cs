using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.AssessmentManager.Models
{
    public class A3MMModel
    {
        public int Id { get; set; }
        public int QuestionResponseID { get; set; }
        public int DisciplineId { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int? ParentID { get; set; }
        public int? SubQOrder { get; set; }
        public int? QuestionOrder { get; set; }
        public string DisciplineName { get; set; }
        public int ComponentId { get; set; }
        public int? ActionItemID { get; set; }
        public string ComponentName { get; set; }
        public string ComponentDefinition { get; set; }
        public string AspectName { get; set; }
        public string MaturityLevel { get; set; }
        public int? MaturityLevelInt { get; set; }
        public int? AspectID { get; set; }
        public int? SurveyTypeID { get; set; }
        public int? DatasourceId { get; set; }
        public int? QuestionId { get; set; }
        public int EngagementPocID { get; set; }
        public int EngagementComID { get; set; }
        public int AccountID { get; set; }
        public string AccountName { get; set; }
        public int ContractID { get; set; }

        public string VersionID { get; set; }
        public string Question { get; set; }
        public string Response { get; set; }
        public string AssessmentResponse { get; set; }
        public string FollowupQuestion { get; set; }
        public string AreasofImprovement { get; set; }
        public string Feedback { get; set; }
        public string LastUpdatedByName { get; set; }
        public string LastFeedbackByName { get; set; }

        public int? QNo { get; set; }
        public string SQNo { get; set; }
        public string ExampleAnswer { get; set; }
        public string Status { get; set; }
        public string QSetStatus { get; set; }
        public string Datasource { get; set; }
        public int AssessmentID { get; set; }
        public string EngagementName { get; set; }
        public string QuestionIgnore { get; set; }
        public string AssessmentTeam { get; set; }
        public string AssessmentName { get; set; }
        public string Attachments { get; set; }
        public string UserRole { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime AnalysisDate { get; set; }
        public DateTime ReviewDate { get; set; }
        public DateTime PlannedCompletionDate { get; set; }
        public string UserName { get; set; }
        public DateTime? CreatedDt { get; set; }
        public int isdrpchanged { get; set; }
        public int isdiscChanged { get; set; }
        public int iscompChanged { get; set; }
        public int isAspChanged { get; set; }

        public IEnumerable<SelectListItem> AssessmentList { get; set; }

        public IEnumerable<SelectListItem> DisciplineList { get; set; }
        public IEnumerable<SelectListItem> ComponentList { get; set; }
        public IEnumerable<SelectListItem> AspectList { get; set; }
        public IEnumerable<SelectListItem> DataSourceList { get; set; }

        public IEnumerable<SelectListItem> AccountNamesList { get; set; }
        public IEnumerable<SelectListItem> ContractList { get; set; }
        public IEnumerable<SelectListItem> EngagementPOCList { get; set; }
        public IEnumerable<SelectListItem> AssessmentTeamNameList { get; set; }
        public IEnumerable<SelectListItem> EngagementTeamToCommunicateList { get; set; }
        public IEnumerable<SelectListItem> AssessmentNameList { get; set; }
        public IEnumerable<SelectListItem> SecondAssessmentNameList { get; set; }
        public IEnumerable<SelectListItem> ThirdAssessmentNameList { get; set; }
        public IEnumerable<SelectListItem> FourthAssessmentNameList { get; set; }
        public IEnumerable<SelectListItem> AssessmentNameListDropDown { get; set; }
        public IEnumerable<SelectListItem> QuestionSetNoList { get; set; }
        public List<ResponseAttachment> RespAttachment { get; set; }
        public List<MultipleFollowUpQuestions> MultipleFollowUpQuestions { get; set; }
        public List<MultipleResponses> MultipleResponses { get; set; }
        public string[] Contract { get; set; }
        public string startDateIgnore { get; set; }
        public string analysisDateIgnore { get; set; }
        public string reviewDateIgnore { get; set; }
        public string PCDateIgnore { get; set; }

        public List<A3MMModel> QuestionSet { get; set; }
       // public List<Qset> Qset { get; set; }
        public List<Qset> UnAnsQset { get; set; }
        public List<Qset> IncompleteQset { get; set; }
        public List<Qset> ResponseInProgressQset { get; set; }
        public List<Qset> RespondedQset { get; set; }
        public List<Qset> RatingInProgressQset { get; set; }
        public List<Qset> CompleteQset { get; set; }
        public int LastFeedbackBy { get; set; }
        public DateTime? LastFeedbackOn { get; set; }
        public int LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }

        public static implicit operator List<object>(A3MMModel v)
        {
            throw new NotImplementedException();
        }

        public string isView { get; set; }
        public int fromAssessmentOverview { get; set; }
        public bool isEditdrpdwnchange { get; set; }
        public bool isEditDisciplineComponentChange { get; set; }

        public string DiscplineAbbrevation { get; set; }

        public string ComponentAbbrevation { get; set; }
        public string QuestionIdentifier { get; set; }
        public int countOfQOfSameDisandComp { get; set; }
        public string engagementComments { get; set; }
        public string SelectedSqNo { get; set; }
        public int Followuporder { get; set; }   
        public string EngagementRating { get; set; }

        //public string engagementRating { get; set; }
        public string responseStatus { get; set; }

        public int ImplementationPlanId { get; set; }
        public string ImplementationPlan { get; set; }
        public int BusinessDriverId { get; set; }
        public string BusinessDriver { get; set; }
        public int AMSCatalogId { get; set; }
        public string AMSCatalog { get; set; }
        public int currentresId {  get; set; }
        public int currentfollowupId {  get; set; }
        public int Priority { get; set; }
        public int SubResponseId { get; set; }
        public int SubResponseOrder { get; set; }
        public bool boolFollowResponseCount { get; set; }

    }
    public class Qset
    {
        public string SQNo { get; set; }
        public int Questionorder { get; set; }
        public string QuestionStatus { get; set; }
        public int AssessmentId { get; set; } 
    }

    public class ResponseAttachment
    {
        public int Id { get; set; }
        public string filename { get; set; }
        public string fileCont { get; set; }
    }
    public class A3MMDigiJrny
    {
        public IEnumerable<SelectListItem> AssessmentList { get; set; }
        public string Account { get; set; }
        public string Contract { get; set; }
        public string AssessmentName { get; set; }
        public string ContractDuration { get; set; }
        public List<A3MMEngagementLevels> EngagementList { get; set; }
        public List<SelectListItem> ComponentList { get; set; }
        public List<string> NAEngagementList { get; set; }

        public A3MMDigiJrny secondAssessmentDataList { get; set; }
        public A3MMDigiJrny ThirdAssessmentDataList { get; set; }
        public A3MMDigiJrny FourthAssessmentDataList { get; set; }
    }
    public class A3MMEngagementLevels
    {
        public string level { get; set; }
        public string EnggComponent { get; set; }
        public string ComponentAbbrevation { get; set; }
    }
    public class Component
    {
        public int Id { get; set; }
        public string ComponentName { get; set; }
        public string ComponentDefinition { get; set; }
        public int DisciplineId { get; set; }
        public string ComponentAbbrevation { get; set; }
    }



    public class A3MMQuestionSet
    {
        public IEnumerable<SelectListItem> AssessmentList { get; set; }
        public int ID { get; set; }
        public string versionID { get; set; }
        public int? surveyTypeID { get; set; }
        public string surveyType { get; set; }
        public string status { get; set; }
        public int clientID { get; set; }
        public int LastUpdatedBy { get; set; }
        public string LastUpdatedByName { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public int? isdeleted { get; set; }
    }

    public class A3MMDCAFilter
    {
        public string AssessmentID { get; set; }
        public int? DisciplineId { get; set; }
        public int? ComponentId { get; set; }
        public int? AspectID { get; set; }
        public int? isdiscChanged { get; set; }
        public int? iscompChanged { get; set; }
        public int? isAspChanged { get; set; }
    }
    public class AssessmentOverviewExport
    {
        public string SQNo { get; set; }
        public string DisciplineName { get; set; }
        public string ComponentName { get; set; }
        public string AspectName { get; set; }
        public string Question { get; set; }
        public string ExampleAnswer { get; set; }
        public string Response { get; set; }
       // public string Feedback { get; set; }
        public string FollowupQuestion { get; set; }
        public string Followup_Response { get; set; }
        public string MaturityLevel { get; set; }
        public string AreasofImprovement { get; set; }
    }

    public class MultipleFollowUpQuestions

    {
        public int Id { get; set; }
        public int ResponseID { get; set; }
        public string FollowUpQNumber { get; set; }
        public string SubFollowUpQuestion { get; set; }
        public int ClientID { get; set; }
        public int? SubFollowUpQOrder { get; set; }
        public int AssessmentId { get; set; }
        public int SubResponseId { get; set; }
        public bool isFCompleted { get; set; }

    }
    public class MultipleResponses

    {
        public int Id { get; set; }
        public int ResponseID { get; set; }
        public string ResponseNumber { get; set; }
        public string SubResponse { get; set; }
        public int ClientID { get; set; }
        public int AssessmentId { get; set; }
        public int SubResponseOrder { get; set; }
        public string SQNo { get; set; }
        public bool isRCompleted { get; set; }
    }
}