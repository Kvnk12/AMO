using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;
using System.Web.Mvc;
using Domain.Entities.PortfolioAnalyzer;

namespace WebUI.Areas.PortfolioAnalyzer.Models
{
    public class APREcoSystemModel
    {
        public int Id { get; set; }
        public string EcosystemName { get; set; }
        public int ClientId { get; set; }
        public bool InScope { get; set; }
    }
}