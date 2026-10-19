using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.RetirementAccelerator.Models
{
    public class ProjectManagementModel
    {
        public int Id { get; set; }
        public int RAApplicationId { get; set; }
        public int ProjectManagementActivityId { get; set; }
        public string ProjActivity { get; set; }
        public int? EstimatedHours { get; set; }
        public int? EstimatedCost { get; set; }
        public string EstimateAssumptions { get; set; }
        public Byte[] Attachment { get; set; }
        public string AttachmentName { get; set; }
        /// <summary>
        /// To make file upload accross all Cost Screen 
        /// </summary>
        public int WhichCostTable
        {
            get
            {
                return (int)CostScreen.ProjectManagement;
            }
        }
    }
}