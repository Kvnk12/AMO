using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.ClientProfileManager.Dto
{
    public class INConfigurationKeyModel
    {
        public int Id { get; set; }
        public int KeyId { get; set; }
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }
        public int ClientId { get; set; }
        public string Description { get; set; }
        public bool ActiveFlag { get; set; }
        public int MultiSelectFlag { get; set; }
        public DateTime CreatedDt { get; set; }
        public DateTime UpdatedDt { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }
}