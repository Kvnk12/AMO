using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;
using System.Web.Mvc;

namespace WebUI.Areas.PortfolioAnalyzer.Models
{
    public class PersonnelRoleModel
    {

    
        public int Id { get; set; }
        public int PersonnelId { get; set; }
        public int? AppId { get; set; }
        public int? ClientId { get; set; }
        public string Title { get; set; }
        public string Emailid { get; set; }
        [UIHint("PersonnelRoleModel")]
        public string IsSurveyParticipant { get; set; }
        public Boolean? ReceiveEmailNotification { get; set; }

        public PersonnelModel Personnel
        {
            get;
            set;
        }

    }
}