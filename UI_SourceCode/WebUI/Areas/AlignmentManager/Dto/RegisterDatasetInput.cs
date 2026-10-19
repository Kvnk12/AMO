using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
using WebUI.Areas.AlignmentManager.Objects;
using System.ComponentModel.DataAnnotations.Schema;


namespace NTTData.AMOPortal.WebUI.Dto
    {

    public class RegisterDatasetInput : Input
        {



        //public RegisterDatasetInput(int clientId)
        //    {
        //    this.ClientId = clientId;

        //    }



        public bool IsDeleted { get; set; }
        public IEnumerable<SelectListItem> GetAllTables
            {
            get
                {
                return AlignmentHelper.ListAllTableAndViews();
                }
            }
        public int ClientId { get; set; }

        [Required(ErrorMessage="Select Data Table from the list")]
        public string DatabaseObjectName { get; set; }

        [Required(ErrorMessage = "Please Enter Data Source name")]
        public string DataSetName { get; set; }

        public IList<DataSetColumnMap> registeredColumns
            {

            get
                {
                return AlignmentHelper.GetColumnMaps(DatabaseObjectName, Id);
                }

            set
            { 
                
                }
            }

        

        public string CreatedByUser { get; set; }

        public DateTime? CreatedDate { get { return _appDate; } set { _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now; } }

        private DateTime  _appDate = DateTime.Now; 
        
        public string LastUpdatedUser { get; set; }

        public IEnumerable<SelectListItem> RegDataList 
            {
            get
                {
                return AlignmentHelper.RegDataList(ClientId);
                }
                
            }

        public DateTime? LastUpdatedDate { get { return _appDate; } set {   _appDate = value.HasValue ? Convert.ToDateTime(value) : DateTime.Now ; } }


        }
    }