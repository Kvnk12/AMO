using System.Data;
using System.Xml;

namespace WebUI.Areas.IncidentNavigator.Models
{
    public static class PivotDataSourceExtensions
    {
        private static readonly string XMLA_WRAP = @"<soap:Envelope><soap:Body><ExecuteResponse><return>{0}</return></ExecuteResponse></soap:Body></soap:Envelope>";
        private static readonly string XMLA_DISCOVER_WRAP = @"<soap:Envelope><soap:Body><DiscoverResponse><return>{0}</return></DiscoverResponse></soap:Body></soap:Envelope>";

        public static string ToXmlaResult(this XmlReader reader)
        {
            return string.Format(XMLA_WRAP, reader.ReadOuterXml());
        }

        public static string ToXmlaDiscoverResult(this DataSet dataSet)
        {
            return string.Format(XMLA_DISCOVER_WRAP, dataSet.GetXml().Replace("NewDataSet", "root").Replace("rowsetTable", "row"));
        }
    }
}