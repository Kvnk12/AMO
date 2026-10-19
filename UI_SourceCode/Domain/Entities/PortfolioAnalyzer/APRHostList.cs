using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.PortfolioAnalyzer
{
    [Table("APRHostList")]
    public class APRHostList
    {
        [Key]
        public int Id { get; set; }
        public string HostName { get; set; }
        public int ClientId { get; set; }
        public string IPAddress { get; set; }
        public string OperatingSystem { get; set; }
        public string OSVersion { get; set; }
        public string AssetTag { get; set; }
        public string HostServerModel { get; set; }
        public int? TotalCPUs { get; set; }
        public int? NoOfCoreProcessors { get; set; }
        public string CPUSpeed { get; set; }
        public string RAM { get; set; }
        public string DiskSpace { get; set; }
        public string ServerBuildingOrCenter { get; set; }

        public string ServerLocationCountry { get; set; }

        public string VendorName { get; set; }

        
    }
}
