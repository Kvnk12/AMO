using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;
using WebUI.Areas.IdeaManager.Objects;


namespace NTTData.AMOPortal.WebUI.Dto
    {
    public class ClassAndCategoryInput : Input
        {

        public string ClientName { get; set; }

        public string Classification { get; set; }

        public int ClientId { get; set; }

        public int ClassifiationId { get; set; }

        public int CategoryId { get; set; }

        public string Category { get; set; }
        public string CatCreate { get; set; }
        public string ClassCreate { get; set; }




        public IMCategoryInput IMCategoryInput
            {
            get;
            set;
            }


        public IMClassificationInput IMClassificationInput
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


        public IList<IMClassification> GetClassification
        {
        get {
        return IdeaManagerHelper._getClassification(this.ClientId);
            }
            }

        public IList<IMCategory> GetCategory
            {
            get
                {
                return IdeaManagerHelper.GetCategory(this.ClassifiationId).ToList();
                }
            }

     
        //public IQueryable<Domain.Entities.ClientAccount> ClientAccounts
        //    {
        //    get
        //        {
        //        return IenumberableLists.ClientAccountDbSet;
        //        }


        //    }


        //public IQueryable<Domain.Entities.ClientAccount> ClientAccountsFilterList
        //    {

        //    get
        //        {

        //        Db db = new Db();


        //        IQueryable<Domain.Entities.ClientAccount> clientAccounts = from r in db.ClientAccountSetup  
        //                                                                   where r.ClientId == this.ClientId
        //                                                                   select new ClientAccount
        //                                                                   {
        //                                                                            AccountName =  r.AccountName,
        //                                                                            Status = r.Status,
        //                                                                            ClientId = r.ClientId,
        //                                                                            Id= r.Id     
        //                                                                        };

                 

        //        return clientAccounts;
        //        }
        //    }



        //public IQueryable<Domain.Entities.ClientAccountContract> ClientAccountContractFilterList
        //    {

        //    get
        //        {

        //        Db db = new Db();


        //        IQueryable<Domain.Entities.ClientAccountContract> clientAccountContracts = from r in db.ClientAccountContract
        //                                                                   where (r.ClientId == this.ClientId &&  r.AccountId == this.AccountId) 
        //                                                                   select r;
        //        return clientAccountContracts;

        //        }
        //    }



        }
    }