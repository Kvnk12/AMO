using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Areas.IdeaManager.Dto
{
    public class IMRoleModel
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        [Required]
        public string RoleName { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? Createddate { get; set; }
        public DateTime? Updateddate { get; set; }
    }
}