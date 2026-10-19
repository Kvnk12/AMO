using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.ClientProfileManager.ViewModel
{
    public class SurveyUserViewModel
    {
        /// <summary>
        /// input from Personnel and PersonnelRole table 
        /// </summary>
        public int Id { get; set; }
        public int clientID { get; set; }
        
        public int PersonnelId { get;set; }
        public string PersonnelName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}