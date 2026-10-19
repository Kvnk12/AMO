using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ClientProfileManager
{
    [Table("StoreApplication")]
    public class StoreApplication
    {
        [Key, Column(Order = 0)]
        public int MappingId { get; set; }

        [Key, Column(Order = 1)]
        public int ApplicationId { get; set; }
    }
}
