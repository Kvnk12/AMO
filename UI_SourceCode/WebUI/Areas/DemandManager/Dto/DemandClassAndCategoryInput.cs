using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
using WebUI.Areas.DemandManager.Object;


namespace NTTData.AMOPortal.WebUI.Dto
    {
    public class DemandClassAndCategoryInput : Input
        {

        public string ClientName { get; set; }

        public string Classification { get; set; }

        public int ClientId { get; set; }

        public int ClassifiationId { get; set; }

        public int CategoryId { get; set; }

        public string Category { get; set; }
        public string CatCreate { get; set; }
        public string ClassCreate { get; set; }




        public DMCategoryInput DMCategoryInput
            {
            get;
            set;
            }


        public DMClassificationInput DMClassificationInput
            {
            get;
            set;
            }


        public IEnumerable<SelectListItem> ClientList
            {
            get
                {
                return GetDropDownListItems.ClientList;
                }
            }


        public IList<DMClassification> GetClassification
        {
        get {
        return DemandManagerHelper._getClassification(this.ClientId);
            }
            }

        public IList<DMCategory> GetCategory
            {
            get
                {
                return DemandManagerHelper.GetCategory(this.ClassifiationId).ToList();
                }
            }

        }
    }