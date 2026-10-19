using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NTTData.AMOPortal.WebUI.Controllers;
using WebUI.Areas.ClientProfileManager;
namespace NextGenAMOUnitTesting
{
    [TestClass]
    public class ClientProfileManagerTest
    {
        [TestMethod]
        public void UserRole()
        {
            //var controller = new AccountController();
            var x=  ClientProfileManagerHelper.GetAllTemplates(1, false);
            Assert.IsNotNull(x);

        }

    }
}
