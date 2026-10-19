using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.Domain.Entities
{
    [Table("UserPasswordLog")]
    public class UserPasswordLog : DelEntity
    {
        //[Key]
        //public int Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public UserProfile UserProfile { get; set; }
        public DateTime PasswordChangedDatetime { get; set; }
        public string IPAddress { get; set; }
        public bool isDeleted;
    }
}
