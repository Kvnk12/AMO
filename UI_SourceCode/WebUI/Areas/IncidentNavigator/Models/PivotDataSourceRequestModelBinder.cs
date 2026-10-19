using System.IO;
using System.Web.Mvc;
using System.Xml.Linq;
using System.Linq;
using System;

namespace WebUI.Areas.IncidentNavigator.Models
{
    public class PivotDataSourceRequestModelBinder : IModelBinder
    {
        private static readonly XNamespace nx = "urn:schemas-microsoft-com:xml-analysis";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var request = new PivotDataSourceRequest();

            var document = XElement.Load(new StreamReader(controllerContext.RequestContext.HttpContext.Request.InputStream));

            request.Statement = (string)document.Descendants(nx + "Statement").FirstOrDefault();

            if (!string.IsNullOrEmpty(request.Statement))
            {
                request.Statement = request.Statement.Replace("&amp;", "&");
            }

            request.Command = (string)document.Descendants(nx + "RequestType").FirstOrDefault();

            request.Restrictions = document.Descendants(nx + "RestrictionList").Elements().ToDictionary(n => n.Name.LocalName, n => (string)n.Value);

            return request;
        }

    }
}