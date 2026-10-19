using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;
using System.Web.Mvc;

namespace WebUI.Areas.PortfolioAnalyzer.Models
{
    public class APRAppHardwareModel
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        [UIHint("APRAppHardwareModel")]
        public int HostId { get; set; }
       
        public string HostName { get; set; }

        [UIHint("AppEnvironment")]
        public string Environment { get; set; }
        [UIHint("Date")]
        public DateTime? AppDeployedDate { get; set; }
        [UIHint("Age")]
        public decimal? AppAge { get; set; }
        [UIHint("String")]
        public string AppAllocatedStorage { get; set; }
        [UIHint("String")]
        public string CPUConsumptionVolume { get; set; }
        [UIHint("String")]
        public string CPUConsumptionUnit { get; set; }
        [UIHint("Cost")]
        public decimal? AppAllocatedHardwareCost { get; set; }
        [UIHint("String")]
        public string Comments { get; set; }
       
        public APRHostlistModel Host
        {
            get;
            set;
        }

    }
}