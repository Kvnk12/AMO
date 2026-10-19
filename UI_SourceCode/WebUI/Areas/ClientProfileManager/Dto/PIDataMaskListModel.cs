using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Areas.ClientProfileManager.Dto
{
    public class PIDataMaskListModel
    {
        public int Id { get; set; }
        [Required]
        public string DataFieldName { get; set; }
        [Required]
        public string DataMaskPattern { get; set; }
        public string DataFieldDescription { get; set; }
        //public int? ClientId { get; set; }
        public string DataMaskExample { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string ReplacementValue { get; set; }
        public string createdbyName { get; set; }

    }
}