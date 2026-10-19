using Data;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Kendo.Mvc.Export;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.WebUI.Dto;
using NTTData.AMOPortal.WebUI.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Telerik.Documents.SpreadsheetStreaming;
using WebUI.Areas.DemandManager.Dto;
using WebUI.Areas.DemandManager.Models;
using WebUI.Areas.DemandManager.Object;
using WebUI.Dto;
using WebUI.Areas.IncidentNavigator;


namespace WebUI.Areas.DemandManager.Controllers
{
    public class ViewDemandController : Controller
    {
        //
        // GET: /DemandManager/ViewDemand/

        public ActionResult Index()
        {
            var UserId = (int)Session["UserId"];
            //var Cl = (Client)Session["Client"];
            var Cl = (ClientDetail)Session["SessionClient"];
            var UserRoleWeight = GetDropDownListItems.UserRoleForClient(UserId, Cl.ClientId);
            bool IsAdmin = false;
            if (UserRoleWeight.Text.ToLower() == "admin" || UserRoleWeight.Text.ToLower() == "sysadmin") { IsAdmin = true; }
            bool IsCusUser = false;
            if (UserRoleWeight.Text.ToLower() == "customeruser") { IsCusUser = true; }
            DemandViewModel cv = new DemandViewModel();
            cv.IsSysAdmin = IsAdmin;
            cv.IsCusUser = IsCusUser;
            return View(cv);
        }

        [HttpPost]
        public ActionResult PreApproval([DataSourceRequest] DataSourceRequest request, int AllDemands = 1)
        {
            var UserId = (int)Session["UserId"];
            // var Cl = (Client)Session["Client"];
            var Cl = (ClientDetail)Session["SessionClient"];
            var UserRoleWeight = GetDropDownListItems.UserRoleForClient(UserId, Cl.ClientId);
            bool IsCusUser = false;
            if (UserRoleWeight.Text.ToLower() == "customeruser") { IsCusUser = true; }
            IEnumerable<DemandPreApprovalModel> DemandListTomodel = null;
            if (AllDemands == (int)DMViewDemand.ViewAllDemands)
            {
                DemandListTomodel = DMHelper.PreApproval(Cl.ClientId, IsCusUser);
            }
            else
            {
                DemandListTomodel = DMHelper.PreApproval(Cl.ClientId, UserId);
            }
            return Json(DemandListTomodel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SubmittedForReview([DataSourceRequest] DataSourceRequest request, int AllDemands = 1)
        {
            var UserId = (int)Session["UserId"];
            // var Cl = (Client)Session["Client"];
            var Cl = (ClientDetail)Session["SessionClient"];
            var UserRoleWeight = GetDropDownListItems.UserRoleForClient(UserId, Cl.ClientId);
            bool IsAdmin = false;
            bool IsCusUser = false;
            if (UserRoleWeight.Text.ToLower() == "customeruser") { IsCusUser = true; }
            if (UserRoleWeight.Text.ToLower() == "admin" || UserRoleWeight.Text.ToLower() == "sysadmin") { IsAdmin = true; }
            IEnumerable<SubmitForReviewModel> DemandListTomodel = null;
            if (AllDemands == (int)DMViewDemand.ViewAllDemands)
            {
                DemandListTomodel = DMHelper.SubmitForReview(Cl.ClientId, IsCusUser);
            }
            else
            {
                DemandListTomodel = DMHelper.SubmitForReview(Cl.ClientId, UserId, IsAdmin);
            }
            return Json(DemandListTomodel.AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Approval([DataSourceRequest] DataSourceRequest request, int AllDemands = 1)
        {
            var UserId = (int)Session["UserId"];
            //var Cl = (Client)Session["Client"];
            var Cl = (ClientDetail)Session["SessionClient"];
            var UserRoleWeight = GetDropDownListItems.UserRoleForClient(UserId, Cl.ClientId);
            bool IsCusUser = false;
            if (UserRoleWeight.Text.ToLower() == "customeruser") { IsCusUser = true; }
            bool IsAdmin = false;
            if (UserRoleWeight.Text.ToLower() == "admin" || UserRoleWeight.Text.ToLower() == "sysadmin") { IsAdmin = true; }
            IEnumerable<DemandApprovedModel> DemandListTomodel = null;
            if (AllDemands == (int)DMViewDemand.ViewAllDemands)
            {
                DemandListTomodel = DMHelper.Approval(Cl.ClientId, IsCusUser);
            }
            else
            {
                DemandListTomodel = DMHelper.Approval(Cl.ClientId, UserId, IsAdmin);
            }
            return Json(DemandListTomodel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Implementation([DataSourceRequest] DataSourceRequest request, int AllDemands = 1)
        {
            var UserId = (int)Session["UserId"];
            //var Cl = (Client)Session["Client"];
            var Cl = (ClientDetail)Session["SessionClient"];
            var UserRoleWeight = GetDropDownListItems.UserRoleForClient(UserId, Cl.ClientId);
            bool IsCusUser = false;
            if (UserRoleWeight.Text.ToLower() == "customeruser") { IsCusUser = true; }
            bool IsAdmin = false;
            if (UserRoleWeight.Text.ToLower() == "admin" || UserRoleWeight.Text.ToLower() == "sysadmin" || UserRoleWeight.Text.ToLower() == "implementationmanager") {
                IsAdmin = true;
            }
            IEnumerable<DemandImplementationModel> DemandListTomodel = null;
            if (AllDemands == (int)DMViewDemand.ViewAllDemands)
            {
                DemandListTomodel = DMHelper.Implementation(Cl.ClientId, IsCusUser);
            }
            else
            {
                DemandListTomodel = DMHelper.Implementation(Cl.ClientId, UserId, IsAdmin);
            }
            return Json(DemandListTomodel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Completion([DataSourceRequest] DataSourceRequest request, int AllDemands = 1)
        {
            var UserId = (int)Session["UserId"];
            //var Cl = (Client)Session["Client"];
            var Cl = (ClientDetail)Session["SessionClient"];
            var UserRoleWeight = GetDropDownListItems.UserRoleForClient(UserId, Cl.ClientId);
            bool IsCusUser = false;
            if (UserRoleWeight.Text.ToLower() == "customeruser") { IsCusUser = true; }
            bool IsAdmin = false;
            if (UserRoleWeight.Text.ToLower() == "admin" || UserRoleWeight.Text.ToLower() == "sysadmin" || UserRoleWeight.Text.ToLower() == "implementationmanager") { IsAdmin = true; }
            IEnumerable<DemandCompletionModel> DemandListTomodel = null;
            if (AllDemands == (int)DMViewDemand.ViewAllDemands)
            {
                DemandListTomodel = DMHelper.Completion(Cl.ClientId, IsCusUser);
            }
            else
            {
                DemandListTomodel = DMHelper.Completion(Cl.ClientId, UserId, IsAdmin);
            }
            return Json(DemandListTomodel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }



        //-----------------------------------------------Search Demand-----------------------------------------------------//
        public ActionResult DemandSearch([DataSourceRequest] DataSourceRequest request, string stringLike = "")
        {
            var UserId = (int)Session["UserId"];
            // var Cl = (Client)Session["Client"];
            var Cl = (ClientDetail)Session["SessionClient"];
            var UserRoleWeight = GetDropDownListItems.UserRoleForClient(UserId, Cl.ClientId);
            bool IsCusUser = false;
            if (UserRoleWeight.Text.ToLower() == "customeruser") { IsCusUser = true; }
            IEnumerable<DemandSearchModel> DemandSearchModel = new List<DemandSearchModel>();
            if (stringLike.Trim() != "")
            {
                DemandSearchModel = DMHelper.DemandSearch(stringLike.Trim(), Cl.ClientId, IsCusUser);
            }
            return Json(DemandSearchModel.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }



        //-----------------------------------------------Export To Excel -- Server-----------------------------------------------------//
        [CustomAuthorize(new string[] { "sysadmin", "admin", "superuser" }, new string[] { "Demand Manager" }, null)]
        [HttpPost]
        public FileStreamResult ExportServer(string AllDemands)
        {
            var UserId = (int)Session["UserId"];
            var UserName = DMHelper.GetUserName(UserId);
            // var Cl = (Client)Session["Client"];
            var Cl = (ClientDetail)Session["SessionClient"];
            var UserRoleWeight = GetDropDownListItems.UserRoleForClient(UserId, Cl.ClientId);
            bool IsAdmin = false;
            if (UserRoleWeight.Text.ToLower() == "admin" || UserRoleWeight.Text.ToLower() == "sysadmin") { IsAdmin = true; }
            var tempWhichDemands = JsonConvert.DeserializeObject(HttpUtility.UrlDecode(AllDemands));
            var WhichDemands = Convert.ToInt32(tempWhichDemands);
            string fileName = "";
            var ds = DMHelper.exportDataToDataSet(Cl.ClientId, UserId, IsAdmin, WhichDemands);
            Stream stream = new MemoryStream();
            SpreadsheetDocument spreadsheet = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook);
            //  Create the Excel file contents.  This function is used when creating an Excel file either writing 
            //  to a file, or writing to a MemoryStream.
            spreadsheet.AddWorkbookPart();
            spreadsheet.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

            //  My thanks to James Miera for the following line of code (which prevents crashes in Excel 2010)
            spreadsheet.WorkbookPart.Workbook.Append(new BookViews(new WorkbookView()));

            //  If we don't add a "WorkbookStylesPart", OLEDB will refuse to connect to this .xlsx file !
            WorkbookStylesPart stylesPart = spreadsheet.WorkbookPart.AddNewPart<WorkbookStylesPart>();
            Stylesheet stylesheet = new Stylesheet();
            stylesPart.Stylesheet = stylesheet;


            //  Loop through each of the DataTables in our DataSet, and create a new Excel Worksheet for each.
            uint worksheetNumber = 1;
            Sheets sheets = spreadsheet.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());
            foreach (DataTable dt in ds.Tables)
            {
                //  For each worksheet you want to create
                string worksheetName = dt.TableName;

                //  Create worksheet part, and add it to the sheets collection in workbook
                WorksheetPart newWorksheetPart = spreadsheet.WorkbookPart.AddNewPart<WorksheetPart>();
                Sheet sheet = new Sheet() { Id = spreadsheet.WorkbookPart.GetIdOfPart(newWorksheetPart), SheetId = worksheetNumber, Name = worksheetName };

                // If you want to define the Column Widths for a Worksheet, you need to do this *before* appending the SheetData  
                // http://social.msdn.microsoft.com/Forums/en-US/oxmlsdk/thread/1d93eca8-2949-4d12-8dd9-15cc24128b10/

                DocumentFormat.OpenXml.Spreadsheet.Columns cs = new DocumentFormat.OpenXml.Spreadsheet.Columns();
                Column c = new Column()
                {
                    Min = 1U,
                    Max = 2U,
                    Width = 44.33203125D,
                    CustomWidth = true
                };
                cs.Append(c);
                sheets.Append(cs);
                //Style-end


                sheets.Append(sheet);

                //  Append this worksheet's data to our Workbook, using OpenXmlWriter, to prevent memory problems
                WriteDataTableToExcelWorksheet(dt, newWorksheetPart);

                worksheetNumber++;
            }

            spreadsheet.WorkbookPart.Workbook.Save();

            spreadsheet.Close();

            if (WhichDemands == 1)
            {
                //View My Demands
                fileName = "NTT_DM_" + Cl.ClientName + "_" + UserName + "_Demands.xlsx";
            }
            else
            {
                //All Demands
                fileName = "NTT_DM_" + Cl.ClientName + "_" + "All" + "_Demands.xlsx";
            }
            stream.Position = 0;
            string mimeType = "application/ms-excel";
          //  string mimeType = Helpers.GetMimeType(SpreadDocumentFormat.Xlsx);
            var fileStreamResult = new FileStreamResult(stream, mimeType);
            fileStreamResult.FileDownloadName = fileName;
            fileStreamResult.FileStream.Seek(0, SeekOrigin.Begin);

            return fileStreamResult;


        }
        //Cell Data Writing
        private static void WriteDataTableToExcelWorksheet(DataTable dt, WorksheetPart worksheetPart)
        {
            OpenXmlWriter writer = OpenXmlWriter.Create(worksheetPart, Encoding.ASCII);
            writer.WriteStartElement(new Worksheet());
            writer.WriteStartElement(new SheetData());

            string cellValue = "";

            //  Create a Header Row in our Excel file, containing one header for each Column of data in our DataTable.
            //
            //  We'll also create an array, showing which type each column of data is (Text or Numeric), so when we come to write the actual
            //  cells of data, we'll know if to write Text values or Numeric cell values.
            int numberOfColumns = dt.Columns.Count;
            bool[] IsNumericColumn = new bool[numberOfColumns];
            bool[] IsDateColumn = new bool[numberOfColumns];

            string[] excelColumnNames = new string[numberOfColumns];
            for (int n = 0; n < numberOfColumns; n++)
                excelColumnNames[n] = GetExcelColumnName(n);

            //
            //  Create the Header row in our Excel Worksheet
            //
            uint rowIndex = 1;

            writer.WriteStartElement(new Row { RowIndex = rowIndex });
            for (int colInx = 0; colInx < numberOfColumns; colInx++)
            {
                DataColumn col = dt.Columns[colInx];
                AppendTextCell(excelColumnNames[colInx] + "1", col.ColumnName, ref writer);
                IsNumericColumn[colInx] = (col.DataType.FullName == "System.Decimal") || (col.DataType.FullName == "System.Int32") || (col.DataType.FullName == "System.Double") || (col.DataType.FullName == "System.Single");
                IsDateColumn[colInx] = (col.DataType.FullName == "System.DateTime");
            }
            writer.WriteEndElement();   //  End of header "Row"

            //
            //  Now, step through each row of data in our DataTable...
            //
            double cellNumericValue = 0;
            foreach (DataRow dr in dt.Rows)
            {
                // ...create a new row, and append a set of this row's data to it.
                ++rowIndex;

                writer.WriteStartElement(new Row { RowIndex = rowIndex });

                for (int colInx = 0; colInx < numberOfColumns; colInx++)
                {
                    cellValue = dr.ItemArray[colInx].ToString();
                    cellValue = ReplaceHexadecimalSymbols(cellValue);

                    // Create cell with data
                    if (IsNumericColumn[colInx])
                    {
                        //  For numeric cells, make sure our input data IS a number, then write it out to the Excel file.
                        //  If this numeric value is NULL, then don't write anything to the Excel file.
                        cellNumericValue = 0;
                        if (double.TryParse(cellValue, out cellNumericValue))
                        {
                            cellValue = cellNumericValue.ToString();
                            AppendNumericCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, ref writer);
                        }
                    }
                    else if (IsDateColumn[colInx])
                    {
                        //  This is a date value.
                        DateTime dtValue;
                        string strValue = "";
                        if (DateTime.TryParse(cellValue, out dtValue))
                            strValue = dtValue.ToShortDateString();
                        AppendTextCell(excelColumnNames[colInx] + rowIndex.ToString(), strValue, ref writer);
                    }
                    else
                    {
                        //  For text cells, just write the input data straight out to the Excel file.
                        AppendTextCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, ref writer);
                    }
                }
                writer.WriteEndElement(); //  End of Row
            }
            writer.WriteEndElement(); //  End of SheetData
            writer.WriteEndElement(); //  End of worksheet

            writer.Close();
        }

        private static void AppendTextCell(string cellReference, string cellStringValue, ref OpenXmlWriter writer)
        {
            //  Add a new Excel Cell to our Row 
            writer.WriteElement(new Cell
            {
                CellValue = new CellValue(cellStringValue),
                CellReference = cellReference,
                DataType = CellValues.String
            });
        }

        private static void AppendNumericCell(string cellReference, string cellStringValue, ref OpenXmlWriter writer)
        {
            //  Add a new Excel Cell to our Row 
            writer.WriteElement(new Cell
            {
                CellValue = new CellValue(cellStringValue),
                CellReference = cellReference,
                DataType = CellValues.Number
            });
        }

        private static string ReplaceHexadecimalSymbols(string txt)
        {
            string r = "[\x00-\x08\x0B\x0C\x0E-\x1F\x26]";
            return Regex.Replace(txt, r, "", RegexOptions.Compiled);
        }

        //  Convert a zero-based column index into an Excel column reference  (A, B, C.. Y, Y, AA, AB, AC... AY, AZ, B1, B2..)
        public static string GetExcelColumnName(int columnIndex)
        {
            //  eg  (0) should return "A"
            //      (1) should return "B"
            //      (25) should return "Z"
            //      (26) should return "AA"
            //      (27) should return "AB"
            //      ..etc..
            char firstChar;
            char secondChar;
            char thirdChar;

            if (columnIndex < 26)
            {
                return ((char)('A' + columnIndex)).ToString();
            }

            if (columnIndex < 702)
            {
                firstChar = (char)('A' + (columnIndex / 26) - 1);
                secondChar = (char)('A' + (columnIndex % 26));

                return string.Format("{0}{1}", firstChar, secondChar);
            }

            int firstInt = columnIndex / 26 / 26;
            int secondInt = (columnIndex - firstInt * 26 * 26) / 26;
            if (secondInt == 0)
            {
                secondInt = 26;
                firstInt = firstInt - 1;
            }
            int thirdInt = (columnIndex - firstInt * 26 * 26 - secondInt * 26);

            firstChar = (char)('A' + firstInt - 1);
            secondChar = (char)('A' + secondInt - 1);
            thirdChar = (char)('A' + thirdInt);

            return string.Format("{0}{1}{2}", firstChar, secondChar, thirdChar);
        }

        public ActionResult DeleteDemand()
        {
            return View("DeleteDemand");
        }
        public ActionResult ReadAllDemands([DataSourceRequest] DataSourceRequest request)
        {
            //var client = (Client)Session["Client"];
            var client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            return Json(DemandManagerHelper.New_GetDemandList(ClientId).ToDataSourceResult(request));
        }


        public ActionResult DemandDelete([DataSourceRequest] DataSourceRequest request, DemandDashboardModel demanddetail)
        {
            if (demanddetail != null)
            {
                var v = DemandManagerHelper.RemoveDemand(demanddetail.Id);
            }

            return Json(new[] { demanddetail }.ToDataSourceResult(request, ModelState));
        }


        public ActionResult DemandReport()
        {             
            DMReportParamModel im = prepareCommonParameters();

            ClientDetail client = (ClientDetail)Session["SessionClient"];
            IEnumerable<string> userEmailList = IncidentNavigatorHelper.getEmailUserPersonnel(client.ClientId);

            ViewBag.userEmailList = userEmailList;
            return View("DemandReport", im);

        }

        private DMReportParamModel prepareCommonParameters()
        {
            //  Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int uid = (int)Session["UserId"];
            DMReportParamModel mod = new DMReportParamModel();
            mod.ClientName = client.ClientName;
            mod.ClientId = client.ClientId;
            mod.UserId = uid;
            mod.ToYear = DateTime.Today.Date;
            mod.FromYear = DateTime.Today.Date.AddMonths(-6); // cuurentdate - 6months
            mod.DateRange = mod.FromYear.Value.ToString("MM/dd/yyyy") + " To " + mod.ToYear.Value.ToString("MM/dd/yyyy");
            mod.CreatedDt = DateTime.Now;
            GetFilterInfo(mod, client.ClientId);


            return mod;
        }

        public static void GetFilterInfo(DMReportParamModel mod, int ClientId)
        {
            //IList<TrendingReportInput> Tredingdata = new List<TrendingReportInput>();
            var paramName = new string[] { "ClientId" };
            var paramValue = new object[] { ClientId };
            DataTable dt = new DataTable();

            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                DataSet ds = new DataSet();


                ds = unitOfWork.GetDataFromStoredProcedureDS("FetchDMFilterInfo", paramName, paramValue);

                List<SelectListItem> ls;

                for (int t = 0; t < ds.Tables.Count; t++)
                {
                    int i = 0;
                    ls = new List<SelectListItem>();
                    foreach (DataRow row in ds.Tables[t].Rows)
                    {
                        ls.Add(new SelectListItem() { Text = row.ItemArray[0].ToString(), Value = i.ToString() });
                        i++;
                    }
                    switch (t)
                    {
                        case 0:
                            mod.TypeList = ls;
                            break;
                        case 1:
                            mod.StatusList = ls;
                            break;
                        case 2:
                            mod.PriorityList = ls;
                            break;
                        case 3:
                            mod.LOBList = ls;
                            break;

                    }

                }

                return;
            }

        }


    }
}
