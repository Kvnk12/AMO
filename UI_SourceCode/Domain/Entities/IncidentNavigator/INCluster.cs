using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Entities.IncidentNavigator
{
    [Table("INCluster")]
  public  class INCluster
    {
        [Key]
        public int ClusterID { get; set; }
        public string ClusterLabel { get; set; }
        public int ClientId { get; set; }
        public string imagePerCluster { get; set; }

        public string TopWords { get; set; }

        public string GroupBy { get; set; }

    }
}
