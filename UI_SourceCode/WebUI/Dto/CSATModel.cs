using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTTData.AMOPortal.WebUI.Dto
{
    public class CSATModel
    {
        public int Id { get; set; }
        public string TicketID { get; set; }
        public string Type { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }

        public int SurveyID { get; set; }
    private string _Description;

        public string Description
        {
            get
            {
                //return Description.Length > 1000 ? Description.Substring(0, 999) : Description;
                return _Description;
            }

            set
            {
                if (value != null && value.Length >= 1000)
                    _Description = value.Substring(0, 999);
                else
                    _Description = value;
            }
        }



        public string Track { get; set; }

        public string ShortDescription { get; set; }

        public int Question1 { get; set; }
        public int Question2 { get; set; }
        public int Question3 { get; set; }
        public int Question4 { get; set; }
        public string Comments { get; set; }
        public decimal Weightage1 { get; set; }
        public decimal Weightage2 { get; set; }
        public decimal Weightage3 { get; set; }
        public decimal Weightage4 { get; set; }

        public string TicketClosedDate { get; set; }

        public string SurveySentdate { get; set; }

        public List<string> QuestionArray { get; set; }

        public double Total { get; set; }
        //public  Dictionary<string,decimal?> QuestionsArray { get; set; }

        //public List<object> QuestionsArray { get; set; }
    }
}