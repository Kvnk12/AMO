using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.RetirementAccelerator.Models
{
    public class ExecutionCostModel
    {
        public int? Id { get; set; }
        public int RAApplicationId { get; set; }
        public string ExecutionActivity { get; set; }
        public int ExecutionActivityId { get; set; }
        public int? EstimatedHours { get; set; }
        public int? BlendedRate { get; set; }
        public int? EstimatedCost { get; set; }
        public int? EstimatedDuration { get; set; }
        public int? EstimatedFTE { get; set; }
        public byte[] Attachment { get; set; }
        public string AttachmentName { get; set; }
        /// <summary>
        /// To make file upload accross all Cost Screen 
        /// </summary>
        public int WhichCostTable
        {
            get
            {
                return (int)CostScreen.ExecutionCost;
            }
        }
    }

    public enum CostScreen
    {
        ExecutionCost = 1,
        ProjectManagement = 2,
        ArchivalSupport = 3
    }

    public class AttachmentModel
    {
        public byte[] file { get; set; }
        public string fileName { get; set; }
    }

}