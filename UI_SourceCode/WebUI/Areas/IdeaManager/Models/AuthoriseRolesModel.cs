using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.IdeaManager.Models
{
    public class AuthoriseRolesModel
    {
        public bool Isadminorsysadmin { get; set; }
        public bool IMAutoApproval { get; set; }
    }
}