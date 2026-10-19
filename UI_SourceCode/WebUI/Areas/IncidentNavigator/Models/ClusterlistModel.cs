using Domain.Entities.ClientProfileManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebUI.Areas.IncidentNavigator.Models
{
    public class ClusterlistModel
    {
        [Required]
        public int ClusterIDfromCluster { get; set; }

        //  [Required]
        [MaxLength(256)]
        public string ClusterLablefromCluster { get; set; }

        [MaxLength(500)]
        public string imagePerCluster { get; set; }
    }
}