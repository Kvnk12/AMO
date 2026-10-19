using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTTData.AMOPortal.Domain.Abstract;
using WebMatrix.WebData;


namespace NTTData.AMOPortal.Service
{
    public class FormsAuthProvider : IAuthProvider
    {
        public bool Authenticate(string UserName, string Password, bool RememberMe)
        {
            if (WebSecurity.Login(UserName, Password, persistCookie: RememberMe))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
