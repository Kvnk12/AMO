using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using System.IO;
using WebUI.Areas.ClientProfileManager.ViewModel;

namespace WebUI.Areas.ClientProfileManager
{
    public class XLAndTXTFileParser
    {

        private static IList<SelectListItem> GetXLFileColumns(MappingTemplateViewModel template)
        {
            string filename = "";
            try
            {
                filename =
                System.Web.HttpContext.Current.Server.MapPath("~/App_Data/" + template.TemplateName);
                File.WriteAllBytes(filename, template.TemplateFile);
            }
            //
            catch
            {
                 filename =
System.Web.HttpContext.Current.Server.MapPath("~/Views/" + template.TemplateName);
                File.WriteAllBytes(filename, template.TemplateFile);
            }

            ////read the data through OLEDB
            OleDbConnection xl = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\"");
            xl.Open();
            //Get columns
            DataTable dtTables = xl.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string strSheet = dtTables.Rows[0]["TABLE_NAME"].ToString();
            strSheet = strSheet.Replace("'", "");
            DataTable dtColumns = xl.GetSchema("Columns", new string[] { null, null, strSheet, null });
            List<SelectListItem> columns = new List<SelectListItem>();
            foreach (DataRow dr in dtColumns.Rows)
            {
                SelectListItem item = new SelectListItem();
                item.Text = dr[3].ToString();
                item.Value = dr[3].ToString();
                columns.Add(item);
            }
            xl.Close();
            return columns;
        }

        public static IList<SelectListItem> GetFileColumns(MappingTemplateViewModel template)
        {
            MappingTemplateViewModel actualTemplate;
            if (template.BaseMappingId==null)
            {
                actualTemplate = template;
            }
            else
            {
                actualTemplate = template.BaseMappingTemplateViewModel;
            }
            if (actualTemplate.FileFormat.ToLower().StartsWith("xls"))
            {
                //SaveXLFile(template);
                return GetXLFileColumns(actualTemplate);
            }
            else if (actualTemplate.FileFormat.ToLower() == "txt")
            {
                return GetTXTFileColumns(actualTemplate);
            }
            else
            {
               return new List<SelectListItem>() { new SelectListItem(){ Text = "XX", Value = "XX" } };
            }

        }

        private static IList<SelectListItem> GetTXTFileColumns(MappingTemplateViewModel template)
        {

            var stream = new StreamReader(new MemoryStream(template.TemplateFile));
            if (template.TemplateFile == null)
            {
                stream = new StreamReader(new MemoryStream(template.TemplateFile));
            }
            IList<SelectListItem> columns = new List<SelectListItem>();
            string strHeaderLine = "";
            string fileContent = stream.ReadToEnd();

            string[] strArr = fileContent.Split( new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            if (strArr.Length > 0)
                strHeaderLine = strArr[0];
           
            if (!string.IsNullOrEmpty(strHeaderLine))
            {
                IList<string> stringList = strHeaderLine.Split(template.FileDelimiter[0]).ToList();

                foreach(string str in stringList)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = str;
                    item.Value = str;
                    columns.Add(item);
                }
            }
            return columns;  //to be implemented

        }
    }
}