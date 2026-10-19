using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebUI.App_Start
{
    public class CsrfHandler
    {
        public static void Validate(Page page, HiddenField forgeryToken)
        {
            if (!page.IsPostBack)
            {
                Guid antiforgeryToken = Guid.NewGuid();
                page.Session["AntiforgeryToken"] = antiforgeryToken;
                forgeryToken.Value = antiforgeryToken.ToString();
            }
            else
            {
                Guid stored = (Guid)page.Session["AntiforgeryToken"];
                Guid sent = new Guid(forgeryToken.Value);
                if (sent != stored)
                {
                    // you can throw an exception, in my case I'm just logging the user out
                    throw new Exception("CSRF validation error.");
                }
            }
        }
    }
}
