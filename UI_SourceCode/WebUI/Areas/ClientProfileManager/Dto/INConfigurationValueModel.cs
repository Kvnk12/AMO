using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.ClientProfileManager.Dto
{
    public class INConfigurationValueModel
    {
        public int Id { get; set; }
        public int KeyId { get; set; }
        public string ParameterName { get; set; }
        [Required]
        public string ParameterValue { get; set; }
        public int ClientId { get; set; }
        [Required]
        public string Description { get; set; }
        public int ActiveFlag { get; set; }
        public int MultiSelectFlag { get; set; }
        public DateTime? CreatedDt { get; set; }
        public DateTime? UpdatedDt { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public List<SelectListItem> extraProp { get; set; }
        public string[] selectedParams { get; set; }

        public IEnumerable<SelectListItem> INClusterParameterValueList
        {
            get
            {
                return ClientProfileManagerHelper.INClusterParameterValueList(this.KeyId,this.ClientId);
            }

        }
    }
}