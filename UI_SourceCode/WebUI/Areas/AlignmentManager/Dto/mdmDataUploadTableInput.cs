using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
using WebUI.Areas.AlignmentManager.Objects;
using System.Data;
using WebUI.Areas.AlignmentManager.Dto;


namespace NTTData.AMOPortal.WebUI.Dto
{
    public class mdmDataUploadTableInput : Input
    {

        public string DataTablename { get; set; }
        public int ClientId { get; set; }
        public DateTime DataTableCreatedatetime { get; set; }

        public DateTime DataTableLastUploaddatetime { get; set; }


        public HttpPostedFileBase httpPostedFileData { get; set; }

        public mdmhstDataFileuploadInput fileuploadhst { get; set; }

        public string lblsheetname { get; set; }

        public string lblfilename { get; set; }

        public bool chktruncate { get; set; }


        public List<mdmhstDataFileupload> DataFileLists
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.mdmhstDataFileupload> IncList = new Data.Repo<NTTData.AMOPortal.Domain.Entities.mdmhstDataFileupload>(Context);
                List<NTTData.AMOPortal.Domain.Entities.mdmhstDataFileupload> e_IncList = IncList.GetAll().ToList();

                var incList = from p in e_IncList
                              where p.ClientId == ClientId && DataTablename == DataTablename
                              select p;

                return incList.ToList();
            }
        }

        public IEnumerable<SelectListItem> ClientList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Client> ClientLists = new Data.Repo<NTTData.AMOPortal.Domain.Entities.Client>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Client> e_ClientList = ClientLists.GetAll().ToList();

                var Items = e_ClientList.Select(ServiceL => new SelectListItem
                {
                    Value = ServiceL.Id.ToString(),
                    Text = ServiceL.ClientName.ToString()
                });
                return Items;
            }
        }

        public IEnumerable<SelectListItem> PPtablename
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.mdmDataUploadTable> mdu = new Data.Repo<Domain.Entities.mdmDataUploadTable>(Context);
                List<NTTData.AMOPortal.Domain.Entities.mdmDataUploadTable> PList = mdu.GetAll().Where(m => m.ClientId == ClientId).ToList();


                var Items = PList.Select(ServiceL => new SelectListItem
                {
                    Value = ServiceL.Id.ToString(),
                    Text = ServiceL.DataTablename.ToString()

                }).ToList();
                return Items;
            }

        }

        public IEnumerable<SelectListItem> SheetnameList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.mdmDataUploadTable> mdu = new Data.Repo<Domain.Entities.mdmDataUploadTable>(Context);
                List<NTTData.AMOPortal.Domain.Entities.mdmDataUploadTable> PList = mdu.GetAll().Where(m => m.ClientId == 0).ToList();


                var Items = PList.Select(ServiceL => new SelectListItem
                {
                    Value = ServiceL.Id.ToString(),
                    Text = ServiceL.DataTablename.ToString()

                }).ToList();
                return Items;
            }

        }


    }

    public class mdmhstDataFileuploadInput : Input
    {
        public int Datatableid { get; set; }
        public int ClientId { get; set; }

        public string datafilename { get; set; }

        public DateTime dateuploaded { get; set; }

        public string sheetname { get; set; }


        public string hdrstatus { get; set; }

        public bool chktruncate { get; set; }

        public string datatablename { get; set; }

    }
}