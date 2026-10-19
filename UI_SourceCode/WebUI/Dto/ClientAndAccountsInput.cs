using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;
using System.ComponentModel.DataAnnotations;
using NTTData.AMOPortal.Domain.Entities;


namespace NTTData.AMOPortal.WebUI.Dto
    {
    public class ClientAndAccountsInput : Input
        {

        public int ClientId { get; set; }
        public int AccountId { get; set; }
        public int ContractId { get; set; }
        public string AccountName { get; set; }
        public string ContractCreate { get; set; }
        public string AccountCreate { get; set; }

        public ClientAccountContractInput clientAccountContractInput
            {
            get;
            set;
            }


        public ClientAccountInput clientAcctInput
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

        //public IEnumerable<SelectListItem> ClientAccountList
        //    {
        //    get
        //        {
        //        return GetDropDownListItems.ClientAccountList;
        //        }
        //    }

        public IQueryable<Domain.Entities.ClientAccount> ClientAccounts
            {
            get
                {
                return IenumberableLists.ClientAccountDbSet;
                }


            }


        public IQueryable<Domain.Entities.ClientAccount> ClientAccountsFilterList
            {

            get
                {

                Db db = new Db();


                IQueryable<Domain.Entities.ClientAccount> clientAccounts = from r in db.ClientAccountSetup  
                                                                           where r.ClientId == this.ClientId
                                                                           select new ClientAccount
                                                                           {
                                                                                    AccountName =  r.AccountName,
                                                                                    Status = r.Status,
                                                                                    ClientId = r.ClientId,
                                                                                    Id= r.Id     
                                                                                };

                 

                return clientAccounts;
                }
            }



        public IQueryable<Domain.Entities.ClientAccountContract> ClientAccountContractFilterList
            {

            get
                {

                Db db = new Db();


                IQueryable<Domain.Entities.ClientAccountContract> clientAccountContracts = from r in db.ClientAccountContract
                                                                           where (r.ClientId == this.ClientId &&  r.AccountId == this.AccountId) 
                                                                           select r;
                return clientAccountContracts;

                }
            }





        //public IQueryable<Domain.Entities.ClientAccountContract> ClientAccountContracts
        //    {
        //    get
        //        {
        //        return IenumberableLists.ClientAccountContractDbSet;
        //        }

        //    }


        }
    }