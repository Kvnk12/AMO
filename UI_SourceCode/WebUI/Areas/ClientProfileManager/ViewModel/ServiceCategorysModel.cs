using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using WebUI.App_Start;
using NTTData.AMOPortal.Domain.Entities;


namespace WebUI.Areas.ClientProfileManager.ViewModel
{
    public class ServiceCategorysModel
    {
        public int Id { get; set; }

        public int ParentServiceCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IsActive { get; set; }
        public string IsOrderable { get; set; }
        public int ScheduleId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public static explicit operator ServiceCategorysModel(List<ServiceCategory> v)
        {
            throw new NotImplementedException();
        }
    }
}