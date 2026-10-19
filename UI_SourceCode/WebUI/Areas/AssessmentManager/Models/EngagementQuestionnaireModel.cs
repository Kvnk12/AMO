using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.AssessmentManager.Models
{
    public class EngagementQuestionnaireModel
    {
        public int Id { get; set; }
        public string VersionID { get; set; }
        public int AccountID { get; set; }
        public int ContractID { get; set; }


        public int? AspectID { get; set; }
        public int? Discipline_ID { get; set; }
        public int? Component_ID { get; set; }
        public string Question { get; set; }
        public string ExampleAns { get; set; }
        public string Status { get; set; }

        public int? DataSourceID { get; set; }

        public int? QNo { get; set; }
        public string MaturityLevel { get; set; }
        public decimal MaturityLevelAVG { get; set; }
        public string Response { get; set; }
        public string AreaOfImprovement { get; set; }
        public string Feedback { get; set; }
        public string FollowUpQuestion { get; set; }
        public int? LastFeedbackBy { get; set; }
        public DateTime? LastFeedbackOn { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public int SurveyTypeID { get; set; }
        public string DisciplineName { get; set; }
        public string ComponentName { get; set; }
        public string AspectName { get; set; }

        public int AssessmentID { get; set; }

        public IEnumerable<SelectListItem> AssessmentList { get; set; }
        public IEnumerable<SelectListItem> SecondAssessmentList { get; set; }
        public IEnumerable<SelectListItem> ThirdAssessmentList { get; set; }
        public IEnumerable<SelectListItem> FourthAssessmentList { get; set; }

        public IEnumerable<SelectListItem> DisciplineList { get; set; }
        public IEnumerable<SelectListItem> DisciplineIDList { get; set; }
        public IEnumerable<SelectListItem> ComponentList { get; set; }
        public IEnumerable<SelectListItem> AspectList { get; set; }
        public IEnumerable<SelectListItem> DataSourceList { get; set; }

        public IEnumerable<SelectListItem> AccountNamesList { get; set; }
        public IEnumerable<SelectListItem> ContractList { get; set; }
        public IEnumerable<SelectListItem> EngagementPOCList { get; set; }
        public IEnumerable<SelectListItem> EngagementTeamToCommunicateList { get; set; }
        public IEnumerable<SelectListItem> AssessmentNameList { get; set; }
        public IEnumerable<SelectListItem> QuestionSetNoList { get; set; }
        public List<EngagementQuestionnaireModel> secondAssessmentDataList { get; set; }
        public List<EngagementQuestionnaireModel> thirdAssessmentDataList { get; set; }
        public List<EngagementQuestionnaireModel> fourthAssessmentDataList { get; set; }

        public int Count { get; set; }
    }
    public class DashModel
    {
        public string AspectName { get; set; }
        public double MaturityLevel { get; set; }
        public string Comment { get; set; }
        public double FirstMaturityLevel { get; set; }
        public string FirstComment { get; set; }
        public string FirstAssessmentName { get; set; }
        public string SecondAssessmentName { get; set; }
        public string ThirdAssessmentName { get; set; }
        public double SecondMaturityLevel { get; set; }
        public string SecondComment { get; set; }
        public double ThirdMaturityLevel { get; set; }
        public string ThirdComment { get; set; }
        public double FourMaturityLevel { get; set; }
        public string FourComment { get; set; }
    }
  

}