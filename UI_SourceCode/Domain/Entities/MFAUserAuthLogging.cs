using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("MFAUserAuthLogging")]
    public class MFAUserAuthLogging
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ClientId { get; set; }
        public int MFATypeId { get; set; }
        public string OTP { get; set; }
        public DateTime RequestedTime { get; set; }
        public DateTime? VerifiedTime { get; set; }
        public string DeviceName { get; set; }
        public string DeviceIP { get; set; }
    }
}
