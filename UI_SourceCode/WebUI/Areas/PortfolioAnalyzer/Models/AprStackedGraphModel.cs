using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.PortfolioAnalyzer.Models
{
    public class AprStackedGraphModel
    {
        public int QId { get; set; }
        public string QuestionIdentifier { get; set; }
        public int SurveyId { get; set; }
        public int PerspectiveTypeId { get; set; }

        public string Hint { get; set; }
        public int PoorRatingCount { get; set; }
        public int AvgRatingCount { get; set; }
        public int GoodRatingCount { get; set; }
    }
}