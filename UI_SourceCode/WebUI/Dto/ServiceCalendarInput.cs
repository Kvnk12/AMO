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
    public class ServiceCalendarInput : Input
    {

        //  public int Servicecalendaridentifier { get; set; }
        public int Clientid { get; set; }
       [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? Calendardate { get; set; }
        public string Datetimezonecode { get; set; }
        public short? Calendaryear { get; set; }
        public DateTime? Fisicalyear { get; set; }
        public System.Byte? Calendarquarternumber { get; set; }
        public int? Fiscalquarternumber { get; set; }
        public System.Byte? Weekdayendnumber { get; set; }
        public System.Byte? Daydate { get; set; }
        public string Dayofweekname { get; set; }
        public System.Byte? Weeknumber { get; set; }
        public System.Byte? Monthnumber { get; set; }
        public string Monthname { get; set; }
        public string Holidayname { get; set; }
        public System.Byte? Holidayindicator { get; set; }
        public int? Weekendindicator { get; set; }
        //[DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public string StartTime { get; set; }
        //[DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public string EndTime { get; set; }
        public string monStartTime { get; set; }
        public string monEdnTime { get; set; }
        public string tueStart { get; set; }
        public string tueEdnTime { get; set; }
        public string wedStartTime { get; set; }
        public string wedEdnTime { get; set; }
        public string thuStartTime { get; set; }
        public string thuEdnTime { get; set; }
        public string friStartTime { get; set; }
        public string friEdnTime { get; set; }
        public string satStartTime { get; set; }
        public string satEdnTime { get; set; }
        public string sunStartTime { get; set; }
        public string sunEdnTime { get; set; }
        public string weekStartTime { get; set; }
        public string weekEndTime { get; set; }
        public string weekendStartTime { get; set; }
        public string weekendEndTime { get; set; }
        public string tabs { get; set; }
        public TimeSpan? Servicestarttime { get; set; }
        public TimeSpan? Serviceendtime { get; set; }
        public string nextWorkingStart { get; set; }
        public string nextWorkingEnd { get; set; }
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        //public DateTime nextWorkingStart { get; set; }
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        //public DateTime nextWorkingEnd { get; set; }
        public IList<ServiceCalendarInput> sclist { get; set; }
        public Boolean chkSameWeek { get; set; }
        public Boolean chkSameWeekend { get; set; }
        public Boolean IsExisting { get; set; }
        public DateTime holidaydate { get; set; }
        public Boolean isdeleted { get; set; }
        public int? AccountId { get; set; }
        public int? ContractId { get; set; }

        public int? workinghours { get; set; }
        public int? Nonworkinghours { get; set; }

        public IQueryable<Domain.Entities.ServiceCalendar> ServiceCalendarList
        {
            get;
            set;

        }

        public IQueryable<Domain.Entities.ServiceCalendar> ServiceCalendarSetupList
        {
            get;
            set;

        }

        public IList<Domain.Entities.ServiceCalendar> HolidayCalendarSetupList
        {
            get;
            set;

        }
        public IEnumerable<SelectListItem> ClientsList
        {
            get
            {


                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.Client> Clients = new Data.Repo<Domain.Entities.Client>(Context);
                List<NTTData.AMOPortal.Domain.Entities.Client> ClientsList = Clients.GetAll().OrderBy(m => m.ClientName).ToList();
                var Items = ClientsList.Select(CList => new SelectListItem
                {
                    Value = CList.Id.ToString(),
                    Text = CList.ClientName.ToString()
                });
                return Items;
            }
        }

        public IEnumerable<SelectListItem> YearList
        {
            get
            {
                DbContextFactory Context = new DbContextFactory();
                Repo<NTTData.AMOPortal.Domain.Entities.ServiceYear> ServiceYears = new Data.Repo<Domain.Entities.ServiceYear>(Context);
                List<NTTData.AMOPortal.Domain.Entities.ServiceYear> ServiceYearsList = ServiceYears.GetAll().OrderBy(m => m.Id).ToList();

                var Items = ServiceYearsList.Select(SList => new SelectListItem
                {
                    Value = SList.Year.ToString(),
                    Text = SList.Year.ToString()
                });
                return Items;

            }

        }

        public List<SelectListItem> ClientAccounts
        {

            get
            {
                Db db = new Db();
                //Repo<NTTData.AMOPortal.Domain.Entities.ClientAccount> accounts = new Data.Repo<Domain.Entities.ClientAccount>(Context);
                // List<NTTData.AMOPortal.Domain.Entities.ClientAccount> list = accounts.GetAll().Where(a => a.ClientId == ClientId).OrderBy(o => o.AccountName).ToList();

                var list = (from clientaccount in db.ClientAccountSetup
                            where clientaccount.ClientId == Clientid
                            select clientaccount).ToList();
                var Items = list.Select(Field => new SelectListItem
                {
                    Value = Field.Id.ToString(),
                    Text = Field.AccountName.ToString()
                });

                return Items.ToList();

            }

        }



        public List<SelectListItem> ClientAccountsContracts
        {

            get
            {
                Db db = new Db();
                //Repo<NTTData.AMOPortal.Domain.Entities.ClientAccount> accounts = new Data.Repo<Domain.Entities.ClientAccount>(Context);
                // List<NTTData.AMOPortal.Domain.Entities.ClientAccount> list = accounts.GetAll().Where(a => a.ClientId == ClientId).OrderBy(o => o.AccountName).ToList();

                var list = (from clientaccount in db.ClientAccountContract
                            where clientaccount.ClientId == Clientid && clientaccount.AccountId == AccountId
                            select clientaccount).ToList();
                var Items = list.Select(Field => new SelectListItem
                {
                    Value = Field.Id.ToString(),
                    Text = Field.ContractName.ToString()
                });

                return Items.ToList();

            }

        }


    }

    public class ServiceYearInput : Input
    {
        public string Year { get; set; }

    }


  


}