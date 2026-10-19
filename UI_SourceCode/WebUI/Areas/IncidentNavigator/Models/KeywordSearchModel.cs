using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebUI.Areas.IncidentNavigator.Dto;
namespace WebUI.Areas.IncidentNavigator.Models
{
    public class KeywordSearchModel
    {
        public IList<KeywordListInput> listofAllkeywords { get; set; }
        public IList<KeywordListInput> listofkeywords { get; set; }
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string WordLanguage { get; set; }
        public string WordValue { get; set; }
        public string WordDescription { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool isdeleted { get; set; }
        public bool Selected { get; set; }
        public List<AlphabetList> AlphaList { get; set; }
        public string DateFilter { get; set; }

        [DisplayFormat(DataFormatString = "{dd/MMM/yyyy}")]
        public DateTime StartDate { get; set; }
        [DisplayFormat(DataFormatString = "{dd/MMM/yyyy}")]
        public DateTime EndDate { get; set; }
        public string TotalIncidents { get; set; }
    }
    public class AlphabetList
    {
        public string A_bet { get; set; }
        public string NoAlphaWords { get; set; }
        public string NoTotAlphaINC { get; set; }
        public string NoDtAlphaINC { get; set; }
        //public List<WordList> WordsForAlphabet { get; set; }
    }
    //public class WordList
    //{
    //    public string WordId { get; set; }
    //    public string Word { get; set; }
    //    public string NoOfIncidents { get; set; }
    //}
}