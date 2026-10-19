using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.Domain.Entities 
{
    public class UserRole : DelEntity
    {
       // public int Id { get; set; }
        public string RoleName { get; set; }
        public int Weightage { get; set; }
    }
}
