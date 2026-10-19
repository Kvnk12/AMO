using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("MFAUserLoggedInDevice")]
    public class MFAUserLoggedInDevice
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string DeviceName { get; set; }
        public string DeviceIP { get; set; }
        public int ClientId { get; set; }
        //public string AuthType { get; set; }
        public int MFATypeId { get; set; }
        public DateTime CreatedDt { get; set; }
        public bool IsDeleted { get; set; }

    }
}
