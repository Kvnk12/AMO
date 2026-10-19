using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Areas.IncidentNavigator.Dto;
namespace WebUI.Areas.IncidentNavigator.Models
{
    public class KeywordReportListModel
    {
       
        public int Id { get; set; }
        public string WordValue { get; set; }
        public string WordType { get; set; }
        public int TotalIncidentCount { get; set; }
        public int WordTotIncCount { get; set; }
        public int WordDtIncCount { get; set; }
        public decimal WordIncidentPer { get; set; }


    }
}