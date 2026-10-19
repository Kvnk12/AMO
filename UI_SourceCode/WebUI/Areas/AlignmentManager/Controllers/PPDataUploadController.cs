using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.Domain.Service;
using NTTData.AMOPortal.WebUI.Dto;
using NTTData.AMOPortal.WebUI.Mapper;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Service;
using System.Reflection;
using System.Globalization;
using NTTData.AMOPortal.WebUI.Controllers;
using WebUI.Areas.AlignmentManager.Dto;
using WebUI.Dto;
using WebUI.Areas.AlignmentManager.Objects;
using System.Text;
using System.Data.Common;
using System.Data.OleDb;
using System.Data;
using Data;

namespace WebUI.Areas.AlignmentManager.Controllers
{

    public class PPDataUploadController : Cruder<mdmDataUploadTable, mdmDataUploadTableInput>
    {
        public PPDataUploadController(ICrudService<mdmDataUploadTable> service, IMapper<mdmDataUploadTable, mdmDataUploadTableInput> v)
            : base(service, v)
        {


        }

        public ActionResult ViewDataUpload(int clientid)
        {
            mdmDataUploadTableInput input = new mdmDataUploadTableInput();
            input.ClientId = clientid;
            return View("ViewDataList", input);
        }

        public ActionResult DataUploadView(int clientid)
        {
            mdmDataUploadTableInput input = new mdmDataUploadTableInput();
            input.ClientId = clientid;
            ModelState.AddModelError("Inst", "Browse a file and click on Attach");
            return View("Create", input);
        }

        public ActionResult DelJobIDData(int id)
        {
            if (id != 0)
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                    var paramNameset1 = new string[] { "jobid" };
                    var paramValueset1 = new object[] { id };
                    unitOfWork.ExecProcedure("DelPPUpload", paramNameset1, paramValueset1);

                }
            }
            DbContextFactory Dbcon = new DbContextFactory();
            Repo<mdmhstDataFileupload> repohDatatbl = new Repo<mdmhstDataFileupload>(Dbcon);
            Mapper<mdmhstDataFileupload, mdmhstDataFileuploadInput> DtblhcreateMapper = new Mapper<mdmhstDataFileupload, mdmhstDataFileuploadInput>();
            mdmhstDataFileupload entityhstdatatbl = new mdmhstDataFileupload();
            CrudService<mdmhstDataFileupload> servicehdtbl = new CrudService<mdmhstDataFileupload>(repohDatatbl);
            var r = servicehdtbl.Get(id);
            servicehdtbl.Delete(id);
            mdmDataUploadTableInput input = new mdmDataUploadTableInput();
            input.ClientId = r.ClientId;
            return View("ViewDataList", input);

        }


        [HttpPost]
        public ActionResult DataUploadView(mdmDataUploadTableInput input, string btnUpload)
        {
            if (btnUpload.ToLower() == "attach")
            {
                if (System.IO.Path.GetFileName(input.fileuploadhst.datafilename).Contains(".xls") ||
                    System.IO.Path.GetFileName(input.fileuploadhst.datafilename).Contains(".xlsx"))
                {
                    input.httpPostedFileData.SaveAs(System.IO.Path.Combine(@System.Configuration.ConfigurationManager.AppSettings["PPUploadPath"].ToString(), System.IO.Path.GetFileName(input.fileuploadhst.datafilename)));
                    input.lblfilename = System.IO.Path.GetFileName(input.fileuploadhst.datafilename);
                    ModelState.AddModelError("CreateMsg1", "File attached successfully");
                }
                else
                {
                    ModelState.AddModelError("CreateMsg4", "File should be in Excel Format(.xls, .xlsx)");
                }
            }
            else
            {
                if (input.lblsheetname == null || input.fileuploadhst.hdrstatus == null || input.DataTablename == null)
                {
                    input.lblfilename = System.IO.Path.GetFileName(input.fileuploadhst.datafilename);
                    ModelState.AddModelError("CreateMsg9", "Provide all mandatory elements data");
                    return View("Create", input);
                }


                if (input.DataTablename != null)
                {

                    DbContextFactory Dbcon = new DbContextFactory();
                    Repo<mdmDataUploadTable> repoDatatbl = new Repo<mdmDataUploadTable>(Dbcon);
                    Mapper<mdmDataUploadTable, mdmDataUploadTableInput> DtblcreateMapper = new Mapper<mdmDataUploadTable, mdmDataUploadTableInput>();
                    mdmDataUploadTable entitydatatbl = new mdmDataUploadTable();
                    CrudService<mdmDataUploadTable> servicedtbl = new CrudService<mdmDataUploadTable>(repoDatatbl);

                    Repo<mdmhstDataFileupload> repohDatatbl = new Repo<mdmhstDataFileupload>(Dbcon);
                    Mapper<mdmhstDataFileupload, mdmhstDataFileuploadInput> DtblhcreateMapper = new Mapper<mdmhstDataFileupload, mdmhstDataFileuploadInput>();
                    mdmhstDataFileupload entityhstdatatbl = new mdmhstDataFileupload();
                    CrudService<mdmhstDataFileupload> servicehdtbl = new CrudService<mdmhstDataFileupload>(repohDatatbl);
                    mdmDataUploadTable tblcheck = service.GetAll().Where(pc => pc.ClientId == input.ClientId && pc.DataTablename == input.DataTablename).FirstOrDefault();
                    int tblnameid;
                    if (tblcheck == null)
                    {
                        input.DataTableCreatedatetime = System.DateTime.Now;
                        input.DataTableLastUploaddatetime = System.DateTime.Now;
                        tblnameid = servicedtbl.Create(DtblcreateMapper.MapToEntity(input, entitydatatbl));

                    }
                    else
                    {
                        tblnameid = tblcheck.Id;
                    }
                    input.fileuploadhst.ClientId = input.ClientId;
                    input.fileuploadhst.Datatableid = tblnameid;
                    input.fileuploadhst.dateuploaded = System.DateTime.Now;
                    input.fileuploadhst.sheetname = input.lblsheetname.Replace("$", "");
                    input.fileuploadhst.chktruncate = input.chktruncate;

                    input.fileuploadhst.datatablename = input.DataTablename;
                    var Id = servicehdtbl.Create(DtblhcreateMapper.MapToEntity(input.fileuploadhst, entityhstdatatbl));

                    using (var contextFactory = new DbContextFactory())
                    {
                        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());

                        var paramNameset1 = new string[] { "recordid" };
                        var paramValueset1 = new object[] { Id };
                        unitOfWork.ExecProcedure("sp_PPDataUpload", paramNameset1, paramValueset1);

                    }

                    ModelState.AddModelError("CreateMsg", "File Uploaded successfully");
                }
                else { ModelState.AddModelError("CreateMsg3", "Provide a Tablename for Upload"); }
            }


            return View("Create", input);
        }

        [HttpPost]
        public JsonResult DTAutoComplete(string prefix, int clientid)
        {
            DbContextFactory Context = new DbContextFactory();
            Repo<NTTData.AMOPortal.Domain.Entities.mdmDataUploadTable> IncList = new Repo<NTTData.AMOPortal.Domain.Entities.mdmDataUploadTable>(Context);
            List<NTTData.AMOPortal.Domain.Entities.mdmDataUploadTable> e_IncList = IncList.GetAll().Where(m => m.ClientId == clientid).ToList();

            var incList = (from p in e_IncList
                           where p.DataTablename.ToUpper().StartsWith(prefix.ToUpper())
                           select new
                           {
                               label = p.DataTablename,
                               val = p.Id
                           }).ToList();
            return Json(incList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetExcelSheetNames(string path)
        {
            List<string> sheets = new List<string>();
            var vpath = System.Configuration.ConfigurationManager.AppSettings["PPUploadPath"].ToString() + path;
            //string[] urlParts = vpath.Split('/');
            //string vfile = urlParts[urlParts. - 1];
            var varline = "";
            if (path.Contains(".xlsx"))
            {

                //varline = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\080715.AMERICAS\\Desktop\\Se2Files\\02152018\\FileUploadTest.xlsx;Extended Properties=\"Excel 12.0; Xml;HDR=Yes\";";
                varline = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + vpath + ";Extended Properties=\"Excel 12.0; Xml;HDR=Yes\";";
                //varline = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"D:\\BulkDataUpload\\FileUploadTest.xlsx\";Extended Properties=\"Excel 12.0; Xml;HDR=Yes\";";
            }
            else
            {
                varline = "Provider=Microsoft.Jet.OleDb.4.0;Data Source=" + vpath + ";Extended Properties=\"Excel 8.0; Xml;HDR=Yes\";";
            }

            //String connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + excelFile + ";Extended Properties=Excel 8.0;";
            OleDbConnection connExcel = new OleDbConnection(varline);

            OleDbCommand cmdExcel = new OleDbCommand();
            cmdExcel.Connection = connExcel;
            try
            {

                connExcel.Open();

                DataTable testTable = connExcel.GetSchema("Tables");

                String[] excelSheets = new String[testTable.Rows.Count];
                int i = 0;

                foreach (DataRow row in testTable.Rows)
                {
                    excelSheets[i] = row["TABLE_NAME"].ToString();

                    if (excelSheets[i].EndsWith("$"))
                    {
                        sheets.Add(row["TABLE_NAME"].ToString());
                        i++;
                    }
                    else
                    {
                        i++;
                    }

                }
            }
            finally
            {

                connExcel.Close();
            }
            return Json(sheets, JsonRequestBehavior.AllowGet);


        }

    }
}