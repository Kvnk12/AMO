using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.RetirementAccelerator.Models
{
    public class OngoingArchivalSupportModel
    {
        public int Id { get; set; }
        public int RAApplicationId { get; set; }
        public string OngoingActivity { get; set; }
        public int OngoingArchivalActivityId { get; set; }
        public int? EstimatedHours { get; set; }
        public int? EstimatedCost { get; set; }
        public string EstimateAssumptions { get; set; }
        public byte[] Attachment { get; set; }
        public string AttachmentName { get; set; }
        /// <summary>
        /// To make file upload accross all Cost Screen 
        /// </summary>
        public int WhichCostTable
        {
            get
            {
                return (int)CostScreen.ArchivalSupport;
            }
        }
    }
}