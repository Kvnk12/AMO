using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.Domain.Abstract
{
    public interface IAuthProvider
    {
        bool Authenticate(string UserName, string Password,bool RememberMe);
    }
}
