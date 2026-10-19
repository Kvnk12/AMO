using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
using WebUI.Areas.AlignmentManager.Objects;
using WebUI.Models;

namespace WebUI.Areas.AlignmentManager.Models
{
    public class VariableCategoryModel : AMOModelBase
    {
        public VariableCategoryModel() { }
        
        public VariableCategoryModel(mdmVariableCategory category)
        {
            if (category != null)
            {
                Category = category;
                CategoryID = category.Id;
                CategoryName = category.VariableCategory;
            }
        }

        public mdmVariableCategory Category { get; set; }

        [MaxLength(255)]
        public string CategoryName { get; set; }

        public int CategoryID { get; set; }
    }
}