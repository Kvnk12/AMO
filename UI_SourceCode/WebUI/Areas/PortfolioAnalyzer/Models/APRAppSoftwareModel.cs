using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;
using System.Web.Mvc;
using Domain.Entities.PortfolioAnalyzer;

namespace WebUI.Areas.PortfolioAnalyzer.Models
{
    public class APRAppSoftwareModel
    {
        public int Id { get; set; }
        public int? ApplicationId { get; set; }
        [UIHint("VendorId")]
        public int VendorId { get; set; }
        [UIHint("VendorProductId")]
        public int VendorProductId { get; set; }
        [UIHint("VendorProductVersionId")]
        public int VendorProductVersionId { get; set; }
        [UIHint("LifeCycleStage")]
        public string LifeCycleStage { get; set; }
        [UIHint("Age")]
        public decimal? MaxNoLicenseUsed { get; set; }

        public int? LatestVersionInMarket { get; set; }
        public int? LastVersionSupported { get; set; }
        [UIHint("Cost")]
        public decimal? TotalLicenseCost { get; set; }
        public string Comments { get; set; }

        public VendorModel Vendors
        {
            get;
            set;
        }
    }
}