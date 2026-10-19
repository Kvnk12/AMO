using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTTData.AMOPortal.Domain.Abstract;
using NTTData.AMOPortal.Domain.Entities;
using NTTData.AMOPortal.Domain.Service;
using NTTData.AMOPortal.WebUI.Dto;
using NTTData.AMOPortal.WebUI.Mapper;
using NTTData.AMOPortal.Data;
using NTTData.AMOPortal.Service;
using System.Globalization;
using WebUI.Dto;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Data;
using NTTData.AMOPortal.WebUI.Filters;

namespace NTTData.AMOPortal.WebUI.Controllers
{
    public class ServiceCalendarController : Controller
    {
        //Entities
       // private ServiceCalendar entityServiceCalendar;

        //Service

       // private CrudService<ServiceCalendar> ServiceCalendarService;

        //Mappers

       // private Mapper<ServiceCalendar, ServiceCalendarInput> ServiceCalendarMapper;
        //private Mapper<ServiceCalendar, ServiceCalendarModel> NewServiceCalendarMapper;
        //Repo

        //private Repo<ServiceCalendar> repoServiceCalendar;

        //public ServiceCalendarController(ICrudService<ServiceCalendar> service, IMapper<ServiceCalendar, ServiceCalendarInput> v)
        //    : base(service, v)
        //{

        //    DbContextFactory Dbcon = new DbContextFactory();
        //    repoServiceCalendar = new Repo<ServiceCalendar>(Dbcon);
        //    ServiceCalendarMapper = new Mapper<ServiceCalendar, ServiceCalendarInput>();
        //    NewServiceCalendarMapper = new Mapper<ServiceCalendar, ServiceCalendarModel>();
        //    entityServiceCalendar = new ServiceCalendar();
        //    ServiceCalendarService = new CrudService<ServiceCalendar>(repoServiceCalendar);
        //}





      
        public static List<DateTime> GetDates(int year, int month)
        {
            var dates = new List<DateTime>();

            // Loop from the first day of the month until we hit the next month, moving forward a day at a time
            for (var date = new DateTime(year, month, 1); date.Month == month; date = date.AddDays(1))
            {
                dates.Add(date);
            }

            return dates;
        }

        public static List<ServiceCalendarModel> getList(int client, int cal, int acc=0, int contract=0)
        {

            var Inputs = new List<ServiceCalendarModel>();
            Db db = new Db();
            //var s = (from r in db.ServiceCalendar
            //         where (r.Clientid == client && r.Calendaryear == cal)
            //         select r).OrderBy(r => r.Calendardate).ToList();
           
                //var sc = (from r in db.ServiceCalendar
                var Selresult = (from r in db.ServiceCalendar
                         where (r.Clientid == client && r.Calendaryear == cal && r.AccountId == acc && r.ContractId == contract)
                         select r)/*.OrderBy(r => r.Calendardate)*/.ToList();
            //s = sc;
            var s = Selresult.OrderBy(a => a.Calendardate).ToList();
                if (s != null)
                {
                    for (int i = 0; i < s.Count();)
                    {
                        var Input = new ServiceCalendarModel();
                        Input.Calendardate = s[i].Calendardate;
                        Input.Calendaryear = s[i].Calendaryear;
                        Input.Monthnumber = s[i].Monthnumber;
                        Input.Daydate = s[i].Daydate;
                        Input.Holidayname = s[i].Holidayname;                      
                        if (s[i].Servicestarttime != null)
                            Input.Servicestarttime = s[i].Servicestarttime;
                    //Input.StartTime = string.Format("{hh:mm tt}",s[i].Servicestarttime.Value);
                    Input.Clientid = client;
                        if (s[i].Serviceendtime != null)                          
                        Input.Serviceendtime = s[i].Serviceendtime;
                        //Input.EndTime = string.Format("{hh:mm tt}",s[i].Serviceendtime);
                        Input.Dayofweekname = s[i].Dayofweekname;                                             
                        i++;
                        if (String.Format("{0:d}", Input.Calendardate) != "12/31/" + cal)
                        {
                            if (i < s.Count())
                            {
                                if (s[i].Holidayindicator == 1)
                                {
                                    if (i < (s.Count()-1))
                                    {
                                        i++;                                       
                                        Input.nextWorkingStart = (String.Format("{0:d}", s[i].Calendardate)) + " " + s[i].Servicestarttime == null ? "" : DateTime.Today.Add(s[i].Servicestarttime.Value).ToString("hh:mm:ss tt");
                                        Input.nextWorkingEnd = (String.Format("{0:d}", s[i].Calendardate)) + " " + s[i].Servicestarttime == null ? "" : DateTime.Today.Add(s[i].Serviceendtime.Value).ToString("hh:mm:ss tt");                                      
                                        i--;
                                    }
                                }

                                else
                                {
                                    Input.nextWorkingStart = (String.Format("{0:d}", s[i].Calendardate)) + " " + s[i].Servicestarttime == null ? "" : DateTime.Today.Add(s[i].Servicestarttime.Value).ToString("hh:mm:ss tt");
                                    Input.nextWorkingEnd = (String.Format("{0:d}", s[i].Calendardate)) + " " + s[i].Servicestarttime == null ? "" : DateTime.Today.Add(s[i].Serviceendtime.Value).ToString("hh:mm:ss tt");

                                }
                            }
                        }
                        else
                        {
                            Input.nextWorkingStart = "";
                            Input.nextWorkingEnd = "";
                        }
                        Inputs.Add(Input);
                    }
                
                return Inputs;
            }
            else
            {
                var cv = new List<ServiceCalendarModel>();
                return cv;
            }
               
        }

        public ActionResult GenerateServiceCalendar( int Calendaryear = 0, string tabs = "", int Account=0, int Contract=0)
        {
            ServiceCalendarModel srCalendarInput = new ServiceCalendarModel();

           // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int Clientid = client.ClientId;

            srCalendarInput.Clientid = Clientid;
            srCalendarInput.Calendaryear = Convert.ToInt16(Calendaryear);
            srCalendarInput.tabs = tabs;
            srCalendarInput.AccountId = Account;
            srCalendarInput.ContractId = Contract;
            if (Clientid > 0 && Calendaryear > 0)
            {
                Db db = new Db();
                var s = from r in db.ServiceCalendar
                        where (r.Clientid == Clientid && r.Calendaryear == Calendaryear && r.AccountId == Account && r.ContractId == Contract)
                        select r;

               // srCalendarInput.ServiceCalendarList = s;
               // srCalendarInput.ServiceCalendarSetupList = s.Take(7);
                //srCalendarInput.HolidayCalendarSetupList = s.Where(r => r.Holidayindicator == 1).ToList();
                //  }
                //  GetDates(2015, 3);

               // srCalendarInput.sclist = getList(Clientid, Calendaryear, Account, Contract);
                //if (srCalendarInput.sclist.Count > 0)
                //{
                //    srCalendarInput.IsExisting = true;
                //}
                //else
                //{
                //    srCalendarInput.IsExisting = false;
                //}
                //foreach (var v in srCalendarInput.ServiceCalendarSetupList)
                //{
                //    string weekdaynum = v.Weekdayendnumber.ToString();
                    
                   
                //    switch (weekdaynum)
                //    {
                //        case "1":
                //            if (v.Servicestarttime != null)
                //            //srCalendarInput.sunStartTime = v.Servicestarttime.Value.ToString();
                //            {
                //                DateTime st1 = DateTime.Today.Add(v.Servicestarttime.Value);
                //                srCalendarInput.sunStartTime = st1.ToString("hh:mm:ss tt");
                //                //srCalendarInput.sunStartTime = dt.ToShortTimeString();
                //            }//.ToString(("hh:mm:ss tt");
                //            if (v.Serviceendtime != null)
                //            {
                //                DateTime et1 = DateTime.Today.Add(v.Serviceendtime.Value);
                //                srCalendarInput.sunEdnTime = et1.ToString("hh:mm:ss tt");
                //                //srCalendarInput.sunEdnTime = dt.ToShortTimeString();
                //                //srCalendarInput.sunEdnTime = v.Serviceendtime.Value.ToString();
                //                //srCalendarInput.sunEdnTime = v.Serviceendtime.Value.ToString("hh:mm:ss tt");
                //            }

                //            break;

                //        case "2":
                //            if (v.Servicestarttime != null)
                //            {
                //                DateTime st1 = DateTime.Today.Add(v.Servicestarttime.Value);
                //                srCalendarInput.monStartTime = st1.ToString("hh:mm:ss tt");
                //                //srCalendarInput.monStartTime = dt.ToShortTimeString();
                //                //srCalendarInput.monStartTime = v.Servicestarttime.Value.ToString();
                //                // srCalendarInput.monStartTime = v.Servicestarttime.Value.ToString("hh:mm:ss tt");
                //            }
                //            if (v.Serviceendtime != null)
                //            {
                //                DateTime et1 = DateTime.Today.Add(v.Serviceendtime.Value);
                //                srCalendarInput.monEdnTime = et1.ToString("hh:mm:ss tt");
                //                //srCalendarInput.monEdnTime = dt.ToShortTimeString();
                //                //srCalendarInput.monEdnTime = v.Serviceendtime.Value.ToString();
                //                //srCalendarInput.monEdnTime = v.Serviceendtime.Value.ToString("hh:mm:ss tt");
                //            }
                //            break;

                //        case "3":
                //            if (v.Servicestarttime != null)
                //            {
                //                DateTime st1 = DateTime.Today.Add(v.Servicestarttime.Value);
                //                srCalendarInput.tueStart = st1.ToString("hh:mm:ss tt");
                //                //srCalendarInput.tueStart = dt.ToShortTimeString();
                //                //srCalendarInput.tueStart = v.Servicestarttime.Value.ToString();
                //                //srCalendarInput.tueStart = v.Servicestarttime.Value.ToString("hh:mm:ss tt");
                //            }
                //            if (v.Serviceendtime != null)
                //            {
                //                DateTime et1 = DateTime.Today.Add(v.Serviceendtime.Value);
                //                srCalendarInput.tueEdnTime = et1.ToString("hh:mm:ss tt");
                //                //srCalendarInput.tueEdnTime = dt.ToShortTimeString();
                //                // srCalendarInput.tueEdnTime = v.Serviceendtime.Value.ToString();
                //                //srCalendarInput.tueEdnTime = v.Serviceendtime.Value.ToString("hh:mm:ss tt");
                //            }
                //            break;

                //        case "4":
                //            if (v.Servicestarttime != null)
                //            {
                //                DateTime st1 = DateTime.Today.Add(v.Servicestarttime.Value);
                //                srCalendarInput.wedStartTime = st1.ToString("hh:mm:ss tt");
                //                //srCalendarInput.wedStartTime = dt.ToShortTimeString();
                //                //srCalendarInput.wedStartTime = v.Servicestarttime.Value.ToString();
                //                //srCalendarInput.wedStartTime = v.Servicestarttime.Value.ToString("hh:mm:ss tt");
                //            }
                //            if (v.Serviceendtime != null)
                //            {
                //                DateTime et1 = DateTime.Today.Add(v.Serviceendtime.Value);
                //                srCalendarInput.wedEdnTime = et1.ToString("hh:mm:ss tt");
                //                //srCalendarInput.wedEdnTime = dt.ToShortTimeString();
                //                // srCalendarInput.wedEdnTime = v.Serviceendtime.Value.ToString();
                //                //srCalendarInput.wedEdnTime = v.Serviceendtime.Value.ToString("hh:mm:ss tt");
                //            }
                //            break;

                //        case "5":
                //            if (v.Servicestarttime != null)
                //            {
                //                DateTime st1 = DateTime.Today.Add(v.Servicestarttime.Value);
                //                srCalendarInput.thuStartTime = st1.ToString("hh:mm:ss tt");
                //                //srCalendarInput.thuStartTime = dt.ToShortTimeString();
                //                // srCalendarInput.thuStartTime = v.Servicestarttime.Value.ToString();
                //                //srCalendarInput.thuStartTime = v.Servicestarttime.Value.ToString("hh:mm:ss tt");
                //            }
                //            if (v.Serviceendtime != null)
                //            {
                //                DateTime et1 = DateTime.Today.Add(v.Serviceendtime.Value);
                //                srCalendarInput.thuEdnTime = et1.ToString("hh:mm:ss tt");
                //                //srCalendarInput.thuEdnTime = dt.ToShortTimeString();
                //                // srCalendarInput.thuEdnTime = v.Serviceendtime.Value.ToString();
                //                //srCalendarInput.thuEdnTime = v.Serviceendtime.Value.ToString("hh:mm:ss tt");
                //            }
                //            break;

                //        case "6":
                //            if (v.Servicestarttime != null)
                //            {
                //                DateTime st1 = DateTime.Today.Add(v.Servicestarttime.Value);
                //                srCalendarInput.friStartTime = st1.ToString("hh:mm:ss tt");
                //                //srCalendarInput.friStartTime = dt.ToShortTimeString();
                //                // srCalendarInput.friStartTime = v.Servicestarttime.Value.ToString();
                //                //srCalendarInput.friStartTime = v.Servicestarttime.Value.ToString("hh:mm:ss tt");
                //            }
                //            if (v.Serviceendtime != null)
                //            {
                //                DateTime et1 = DateTime.Today.Add(v.Serviceendtime.Value);
                //                srCalendarInput.friEdnTime = et1.ToString("hh:mm:ss tt");
                //               // srCalendarInput.friEdnTime = dt.ToShortTimeString();
                //                //  srCalendarInput.friEdnTime = v.Serviceendtime.Value.ToString();
                //                // srCalendarInput.friEdnTime = v.Serviceendtime.Value.ToString("hh:mm:ss tt");
                //            }
                //            break;

                //        case "7":
                //            if (v.Servicestarttime != null)
                //            {
                //                DateTime st1 = DateTime.Today.Add(v.Servicestarttime.Value);
                //                srCalendarInput.satStartTime = st1.ToString("hh:mm:ss tt");
                //                //srCalendarInput.satStartTime = dt.ToShortTimeString();
                //                //  srCalendarInput.satStartTime = v.Servicestarttime.Value.ToString();
                //                //srCalendarInput.satStartTime = v.Servicestarttime.Value.ToString("hh:mm:ss tt");
                //            }
                //            if (v.Serviceendtime != null)
                //            {
                //                DateTime et1 = DateTime.Today.Add(v.Serviceendtime.Value);
                //                srCalendarInput.satEdnTime = et1.ToString("hh:mm:ss tt");
                //                //srCalendarInput.satEdnTime = dt.ToShortTimeString();
                //                // srCalendarInput.satEdnTime = v.Serviceendtime.Value.ToString();
                //                //srCalendarInput.satEdnTime = v.Serviceendtime.Value.ToString("hh:mm:ss tt");
                //            }
                //            break;
                //    }
                //}
            }
            return View(srCalendarInput);
        }
        [HttpPost]
        public ActionResult GenerateServiceCalendar(ServiceCalendarModel input)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            input.Clientid = client.ClientId;
            if (input.Clientid == 0 || input.Calendaryear == 0 || input.Calendaryear == null)
            {
                ModelState.AddModelError("CreateMsg", "Please select a calendar and year");
                //return View("Create", input);
                return View("GenerateServiceCalendar", input);
            }
            if (input.Clientid > 0 && input.Calendaryear > 0 )
            {
                //GetDates(2015, 3);
                var dates = new List<DateTime>();

                int month;
                // int year;
                //if (input.Calendaryear.HasValue)
                //{
                //    year = input.Calendaryear.Value;
                //}
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    for (month = 1; month <= 12; month++)
                  {
                    //Loop from the first day of the month until we hit the next month, moving forward a day at a time
                    for (var date = new DateTime(input.Calendaryear.Value, month, 1); date.Month == month; date = date.AddDays(1))
                    {

                        switch (date.DayOfWeek.ToString())
                        {
                            case "Sunday":

                                if (input.chkSameWeekend.ToString() == "True")
                                {
                                    input.Servicestarttime = DateTime.Parse(input.weekendStartTime).TimeOfDay;
                                    input.Serviceendtime = DateTime.Parse(input.weekendEndTime).TimeOfDay;
                                    //input.StartTime = input.weekendStartTime;
                                    //input.EndTime = input.weekendEndTime;
                                }
                                else
                                {
                                    //input.StartTime = input.sunStartTime;
                                    //input.EndTime = input.sunEdnTime;
                                    if (input.sunStartTime != null && input.sunEdnTime != null)
                                    {
                                        input.Servicestarttime = DateTime.Parse(input.sunStartTime).TimeOfDay;
                                        input.Serviceendtime = DateTime.Parse(input.sunEdnTime).TimeOfDay;
                                    }
                                    else
                                    {
                                        input.Weekendindicator = 1;
                                        input.Servicestarttime = new TimeSpan();
                                        input.Serviceendtime = new TimeSpan();

                                    }
                                }
                                input.Weekdayendnumber = 1;
                                input.Dayofweekname = date.DayOfWeek.ToString();
                                break;

                            case "Monday":
                                if (input.chkSameWeek.ToString() == "True")
                                {
                                    input.Servicestarttime = DateTime.Parse(input.weekStartTime).TimeOfDay;
                                    input.Serviceendtime = DateTime.Parse(input.weekEndTime).TimeOfDay;
                                    //input.StartTime = input.weekStartTime;
                                    //input.EndTime = input.weekEndTime;
                                }
                                else
                                {

                                    input.Servicestarttime = DateTime.Parse(input.monStartTime).TimeOfDay;
                                    input.Serviceendtime = DateTime.Parse(input.monEdnTime).TimeOfDay;
                                    //input.StartTime = input.monStartTime;
                                    //input.EndTime = input.monEdnTime;
                                }
                                input.Weekdayendnumber = 2;
                                input.Dayofweekname = date.DayOfWeek.ToString();
                                input.Weekendindicator = 0;
                                break;

                            case "Tuesday":
                                if (input.chkSameWeek.ToString() == "True")
                                {
                                    input.Servicestarttime = DateTime.Parse(input.weekStartTime).TimeOfDay;
                                    input.Serviceendtime = DateTime.Parse(input.weekEndTime).TimeOfDay;
                                    //input.StartTime = input.weekStartTime;
                                    //input.EndTime = input.weekEndTime;
                                }
                                else
                                {
                                    input.Servicestarttime = DateTime.Parse(input.tueStart).TimeOfDay;
                                    input.Serviceendtime = DateTime.Parse(input.tueEdnTime).TimeOfDay;
                                    //input.StartTime = input.tueStart;
                                    //input.EndTime = input.tueEdnTime;
                                }
                                input.Weekdayendnumber = 3;
                                input.Dayofweekname = date.DayOfWeek.ToString();
                                input.Weekendindicator = 0;
                                break;

                            case "Wednesday":
                                if (input.chkSameWeek.ToString() == "True")
                                {
                                    input.Servicestarttime = DateTime.Parse(input.weekStartTime).TimeOfDay;
                                    input.Serviceendtime = DateTime.Parse(input.weekEndTime).TimeOfDay;
                                    //input.StartTime = input.weekStartTime;
                                    //input.EndTime = input.weekEndTime;
                                }
                                else
                                {
                                    input.Servicestarttime = DateTime.Parse(input.wedStartTime).TimeOfDay;
                                    input.Serviceendtime = DateTime.Parse(input.wedEdnTime).TimeOfDay;
                                    //input.StartTime = input.wedStartTime;
                                    //input.EndTime = input.wedEdnTime;
                                }
                                input.Weekdayendnumber = 4;
                                input.Dayofweekname = date.DayOfWeek.ToString();
                                input.Weekendindicator = 0;
                                break;

                            case "Thursday":
                                if (input.chkSameWeek.ToString() == "True")
                                {
                                    input.Servicestarttime = DateTime.Parse(input.weekStartTime).TimeOfDay;
                                    input.Serviceendtime = DateTime.Parse(input.weekEndTime).TimeOfDay;
                                    //input.StartTime = input.weekStartTime;
                                    //input.EndTime = input.weekEndTime;
                                }
                                else
                                {
                                    input.Servicestarttime = DateTime.Parse(input.thuStartTime).TimeOfDay;
                                    input.Serviceendtime = DateTime.Parse(input.thuEdnTime).TimeOfDay;
                                    //input.StartTime = input.thuStartTime;
                                    //input.EndTime = input.thuEdnTime;
                                }
                                input.Weekdayendnumber = 5;
                                input.Dayofweekname = date.DayOfWeek.ToString();
                                input.Weekendindicator = 0;
                                break;

                            case "Friday":
                                if (input.chkSameWeek.ToString() == "True")
                                {
                                    input.Servicestarttime = DateTime.Parse(input.weekStartTime).TimeOfDay;
                                    input.Serviceendtime = DateTime.Parse(input.weekEndTime).TimeOfDay;
                                    //input.StartTime = input.weekStartTime;
                                    //input.EndTime = input.weekEndTime;
                                }
                                else
                                {
                                    input.Servicestarttime = DateTime.Parse(input.friStartTime).TimeOfDay;
                                    input.Serviceendtime = DateTime.Parse(input.friEdnTime).TimeOfDay;
                                    //input.StartTime = input.friStartTime;
                                    //input.EndTime = input.friEdnTime;
                                }
                                input.Weekdayendnumber = 6;
                                input.Dayofweekname = date.DayOfWeek.ToString();
                                input.Weekendindicator = 0;
                                break;

                            case "Saturday":
                                if (input.chkSameWeekend.ToString() == "True")
                                {
                                    input.Servicestarttime = DateTime.Parse(input.weekendStartTime).TimeOfDay;
                                    input.Serviceendtime = DateTime.Parse(input.weekendEndTime).TimeOfDay;
                                    //input.StartTime = input.weekendStartTime;
                                    //input.EndTime = input.weekendEndTime;
                                }
                                else
                                {
                                    //input.StartTime = input.satStartTime;
                                    //input.EndTime = input.satEdnTime;
                                    if (input.satStartTime != null && input.satEdnTime != null)
                                    {
                                        input.Servicestarttime = DateTime.Parse(input.satStartTime).TimeOfDay;
                                        input.Serviceendtime = DateTime.Parse(input.satEdnTime).TimeOfDay;
                                    }
                                    else
                                    {
                                        input.Weekendindicator = 1;
                                        input.Servicestarttime = new TimeSpan();
                                        input.Serviceendtime = new TimeSpan();
                                    }
                                }
                                input.Weekdayendnumber = 7;
                                input.Dayofweekname = date.DayOfWeek.ToString();
                                break;
                        }
                        input.Holidayindicator = 0;
                        input.Calendardate = date;
                        input.Monthnumber = Convert.ToByte(date.Month);
                        input.Calendaryear = Convert.ToInt16(date.Year);
                        input.Monthname = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month);
                        input.Weeknumber = Convert.ToByte(CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday));
                        input.Daydate = Convert.ToByte(date.Day);
                        input.Calendarquarternumber = Convert.ToByte((date.Month - 1) / 3 + 1);
                        //if (input.chkSameWeekend.ToString() == "false")
                        //{
                        //    input.Weekendindicator = 1;
                        //}
                        TimeSpan starttime = new TimeSpan();
                        TimeSpan endtime = new TimeSpan();
                        if (input.Servicestarttime != null)
                        {
                            starttime = input.Servicestarttime.Value;
                        }
                        if (input.Serviceendtime != null)
                        {
                            endtime = input.Serviceendtime.Value;
                        }
                        var totalhrs = DateTime.Today.Add(endtime).Subtract(DateTime.Today.Add(starttime)).TotalHours;
                        input.workinghours = Convert.ToInt32(totalhrs) * 60;
                        input.Nonworkinghours = (24 * 60 - input.workinghours);
                            //var totalhrs = DateTime.Today.Add(input.Serviceendtime.Value).Subtract(DateTime.Today.Add(input.Servicestarttime.Value)).TotalHours;
                            //input.workinghours = Convert.ToInt32(totalhrs);
                            //input.Nonworkinghours = (24 - input.workinghours);

                            ServiceCalendar existingServiceCalendar = unitOfWork.ServiceCalendar.Get(c => c.Daydate == input.Daydate && c.Calendaryear == input.Calendaryear && c.Monthnumber == input.Monthnumber && c.Clientid == input.Clientid && c.AccountId == input.AccountId && c.ContractId == input.ContractId).FirstOrDefault();
                            if (existingServiceCalendar != null)
                            {
                               // input.Id = existingServiceCalendar.Id;
                                //input.Holidayindicator = existingServiceCalendar.Holidayindicator;
                               // input.Holidayname = existingServiceCalendar.Holidayname;
                                existingServiceCalendar.Servicestarttime = input.Servicestarttime;
                                existingServiceCalendar.Serviceendtime = input.Serviceendtime;
                                existingServiceCalendar.Weekendindicator = input.Weekendindicator;
                                existingServiceCalendar.Weekdayendnumber = input.Weekdayendnumber;
                                existingServiceCalendar.Dayofweekname = input.Dayofweekname;
                                existingServiceCalendar.Holidayindicator = existingServiceCalendar.Holidayindicator;
                                existingServiceCalendar.Calendardate = input.Calendardate;
                                existingServiceCalendar.Monthnumber = input.Monthnumber;
                                existingServiceCalendar.Calendaryear = input.Calendaryear;
                                existingServiceCalendar.Monthname = input.Monthname;
                                existingServiceCalendar.Weeknumber = input.Weeknumber;
                                existingServiceCalendar.Daydate = input.Daydate;
                                existingServiceCalendar.Calendarquarternumber = input.Calendarquarternumber;
                                existingServiceCalendar.workinghours = input.workinghours;
                                existingServiceCalendar.Nonworkinghours = input.Nonworkinghours;

                                unitOfWork.ServiceCalendar.Update(existingServiceCalendar);
                               // NewServiceCalendarMapper.MapToEntity(input, ServiceCalendarService.Get(input.Id));
                               // ServiceCalendarService.Save();
                                //TempData["Success"] = "Service hours calendar updated successfully";
                                // ModelState.AddModelError("CreateMsg", "Service hours calendar updated Successfully.");
                            }
                            else
                            {
                                ServiceCalendar newServiceCalendar = new ServiceCalendar();
                                Mapper<ServiceCalendar, ServiceCalendarModel> NewServiceCalendarMapper = new Mapper<ServiceCalendar, ServiceCalendarModel>();
                                var enitityCalendar = NewServiceCalendarMapper.MapToEntity(input, newServiceCalendar);
                                unitOfWork.ServiceCalendar.Insert(enitityCalendar);
                                //var ServiceCalendarID = ServiceCalendarService.Create(NewServiceCalendarMapper.MapToEntity(input, entityServiceCalendar));
                                //  input.Id = Convert.ToInt32(SsubVerticalId);
                                //  ModelState.AddModelError("CreateMsg", "Service hours calendar Created Successfully.");

                                //TempData["Success"] = "Service hours calendar Created successfully";

                            }
                    }
                  }
                    unitOfWork.Save();
                }
                ModelState.AddModelError("Success", "Service hours calendar created/updated Successfully.");
            }
            return Json("success");
        }
       
        [HttpPost]
        public ActionResult GenerateServiceHolidayCalendar(ServiceCalendarModel input)
        {
            // Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            input.Clientid = client.ClientId;
            input.Calendardate = Convert.ToDateTime(input.Calendardatestr);

            if (input.Clientid == 0 || input.Calendaryear == 0 || input.Calendaryear == null)
            {
                ModelState.AddModelError("CreateMsg", "Please select a calendar and year");
                //return View("Create", input);
                return View("GenerateServiceCalendar", input);
            }
            if (input.AccountId == null)
                input.AccountId = 0;
            if (input.ContractId == null)
                input.ContractId = 0;
            if (input.Clientid > 0 && input.Calendaryear > 0 )
            {
                input.tabs = "Tab2";
                if (input.Calendardate.HasValue)
                {
                    input.Weekdayendnumber = Convert.ToByte((int)input.Calendardate.Value.DayOfWeek + 1);
                    input.Dayofweekname = input.Calendardate.Value.DayOfWeek.ToString();
                    input.Calendardate = input.Calendardate.Value;
                    input.Monthnumber = Convert.ToByte(input.Calendardate.Value.Month);
                    input.Calendaryear = Convert.ToInt16(input.Calendardate.Value.Year);
                    input.Monthname = (CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(input.Calendardate.Value.Month));
                    input.Weeknumber = Convert.ToByte(CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(input.Calendardate.Value, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday));
                    input.Daydate = Convert.ToByte(input.Calendardate.Value.Day);
                    input.Calendarquarternumber = Convert.ToByte((input.Calendardate.Value.Month - 1) / 3 + 1);
                    input.Holidayindicator = 1;
                    input.Holidayname = input.Holidayname;
                    input.AccountId = input.AccountId;
                    input.ContractId = input.ContractId;

                    //input.StartTime = input.StartTime;
                    //input.Servicestarttime = DateTime.ParseExact(input.StartTime,"hh:mm tt",CultureInfo.InvariantCulture).TimeOfDay;
                    //input.Serviceendtime = DateTime.ParseExact(input.EndTime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
                    input.Servicestarttime = DateTime.Parse(input.StartTime).TimeOfDay;
                    input.Serviceendtime = DateTime.Parse(input.EndTime).TimeOfDay;
                    var totalhrs1 = DateTime.Today.Add(input.Serviceendtime.Value).Subtract(DateTime.Today.Add(input.Servicestarttime.Value)).TotalHours;

                    //var selcDate= input.Calendardate
                    //input.EndTime = input.EndTime;
                    using (var contextFactory = new DbContextFactory())
                    {
                        var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                        ServiceCalendar existingServiceCalendar = unitOfWork.ServiceCalendar.Get(c => c.Daydate == input.Daydate &&c.Calendaryear==input.Calendaryear&& c.Monthnumber==input.Monthnumber && c.Clientid == input.Clientid && c.AccountId == input.AccountId && c.ContractId == input.ContractId).FirstOrDefault() ;
                        if (existingServiceCalendar != null)
                        {
                            input.Id = existingServiceCalendar.Id;
                            input.workinghours = Convert.ToInt32(totalhrs1) * 60;
                            input.Nonworkinghours = (24 * 60 - input.workinghours);
                            existingServiceCalendar.workinghours = input.workinghours;
                            existingServiceCalendar.Nonworkinghours = input.Nonworkinghours;
                            existingServiceCalendar.Weekdayendnumber = input.Weekdayendnumber;
                            existingServiceCalendar.Dayofweekname = input.Dayofweekname;
                            existingServiceCalendar.Calendardate = input.Calendardate.Value;
                            existingServiceCalendar.Monthnumber = input.Monthnumber;
                            existingServiceCalendar.Calendaryear =input.Calendaryear;
                            existingServiceCalendar.Monthname =input.Monthname;
                            existingServiceCalendar.Weeknumber = input.Weeknumber;
                            existingServiceCalendar.Daydate = input.Daydate;
                            existingServiceCalendar.Calendarquarternumber =input.Calendarquarternumber;
                            existingServiceCalendar.Holidayindicator = input.Holidayindicator;
                            existingServiceCalendar.Holidayname = input.Holidayname;
                            existingServiceCalendar.AccountId = input.AccountId.Value;
                            existingServiceCalendar.ContractId = input.ContractId.Value;
                            existingServiceCalendar.Servicestarttime = input.Servicestarttime;
                            existingServiceCalendar.Serviceendtime =input.Serviceendtime;
                            unitOfWork.ServiceCalendar.Update(existingServiceCalendar);

                           // NewServiceCalendarMapper.MapToEntity(input, ServiceCalendarService.Get(input.Id));
                           // ServiceCalendarService.Save();
                            //  ModelState.AddModelError("CreateMsg", "Service hours calendar updated Successfully.");
                            //  TempData["Success"] = "Holiday hours calendar updated successfully";

                        }
                        else
                        {
                            ServiceCalendar newServiceCalendar = new ServiceCalendar();
                            Mapper<ServiceCalendar, ServiceCalendarModel> NewServiceCalendarMapper = new Mapper<ServiceCalendar, ServiceCalendarModel>();
                            var enitityCalendar = NewServiceCalendarMapper.MapToEntity(input, newServiceCalendar);
                            unitOfWork.ServiceCalendar.Insert(enitityCalendar);
                            //var SsubVerticalId = ServiceCalendarService.Create(NewServiceCalendarMapper.MapToEntity(input, entityServiceCalendar));
                            //ModelState.AddModelError("CreateMsg", "Service hours calendar Created Successfully.");
                            //TempData["Success"] = "Holiday hours calendar Created successfully";

                        }
                        unitOfWork.Save();
                    }

                    TempData["Success"] = "Holiday hours calendar created successfully";

                }
              
            }
            return Json("success");
        }



        public ActionResult Refresh(int Calendaryear = 0, string tabs = "3", int Account = 0, int Contract = 0)
        {

            ServiceCalendarInput srCalendarInput = new ServiceCalendarInput();
          //  Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int Clientid = client.ClientId;

            srCalendarInput.Clientid = Clientid;
            srCalendarInput.Calendaryear = Convert.ToInt16(Calendaryear);
            srCalendarInput.AccountId = Account;
            srCalendarInput.ContractId = Contract;
            srCalendarInput.tabs = tabs;
            if (Clientid > 0 && Calendaryear > 0)
            {
                Db db = new Db();
                var s = from r in db.ServiceCalendar
                        where (r.Clientid == Clientid && r.Calendaryear == Calendaryear && r.AccountId == Account && r.ContractId == Contract)
                        select r;

                
                srCalendarInput.ServiceCalendarList = s;
                srCalendarInput.HolidayCalendarSetupList = s.Where(r => r.Holidayindicator == 1).ToList();

               
               // srCalendarInput.sclist = getList(Clientid, Calendaryear, Account, Contract);
                if (srCalendarInput.sclist.Count > 0)
                {
                    srCalendarInput.IsExisting = true;
                }
                else
                {
                    srCalendarInput.IsExisting = false;
                }
            }
            //srCalendarInput.AccountId = Account;
            //srCalendarInput.ContractId = Contract;
            return RedirectToAction("GenerateServiceCalendar", new { Calendaryear = Calendaryear, tabs = tabs, Account = Account, Contract = Contract });

        }




        public ActionResult DeleteHoliday(int Id, int Year)
        {
            ServiceCalendar serviceCal = new ServiceCalendar();
            ServiceCalendarInput serInp = new ServiceCalendarInput();
            Mapper<ServiceCalendar, ServiceCalendarInput> serMapper = new Mapper<ServiceCalendar, ServiceCalendarInput>();
            DbContextFactory Dbcon = new DbContextFactory();
            Repo<ServiceCalendar> serrepo = new Repo<ServiceCalendar>(Dbcon);
            CrudService<ServiceCalendar> serCrud = new CrudService<ServiceCalendar>(serrepo);
            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int Clientid = client.ClientId;
            using (Db db = new Db())
            {
                ServiceCalendar existingServiceCalendar = db.ServiceCalendar.FirstOrDefault(c => c.Id == Id);
                if (existingServiceCalendar != null)
                {
                    serInp.Id = Id;
                    serInp.Clientid = Clientid;
                    serInp.isdeleted = true;
                    if (existingServiceCalendar.Servicestarttime != null)
                        serInp.Servicestarttime = existingServiceCalendar.Servicestarttime;
                    //serInp.StartTime = existingServiceCalendar.Servicestarttime.Value.ToString("hh:mm");
                    if (existingServiceCalendar.Serviceendtime != null)
                        serInp.Serviceendtime = existingServiceCalendar.Serviceendtime;
                    //serInp.EndTime = existingServiceCalendar.Serviceendtime.Value.ToString("hh:mm");
                    serInp.Weekdayendnumber = Convert.ToByte((int)existingServiceCalendar.Calendardate.Value.DayOfWeek + 1);
                    serInp.Dayofweekname = existingServiceCalendar.Calendardate.Value.DayOfWeek.ToString();
                    serInp.Calendardate = existingServiceCalendar.Calendardate.Value;
                    serInp.Monthnumber = Convert.ToByte(existingServiceCalendar.Calendardate.Value.Month);
                    serInp.Calendaryear = Convert.ToInt16(existingServiceCalendar.Calendardate.Value.Year);
                    serInp.Monthname = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(existingServiceCalendar.Calendardate.Value.Month);
                    serInp.Weeknumber = Convert.ToByte(CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(existingServiceCalendar.Calendardate.Value, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday));
                    serInp.Daydate = Convert.ToByte(existingServiceCalendar.Calendardate.Value.Day);
                    serInp.Calendarquarternumber = Convert.ToByte((existingServiceCalendar.Calendardate.Value.Month - 1) / 3 + 1);
                    serInp.Holidayindicator =existingServiceCalendar.Holidayindicator;
                    serInp.Holidayname = existingServiceCalendar.Holidayname;
                    serMapper.MapToEntity(serInp, serCrud.Get(serInp.Id));
                    serCrud.Save();
                 
                }
               
            }


            //return RedirectToAction("GenerateServiceCalendar", new { serInp.Clientid, serInp.Calendaryear, tabs = "Tab2" });
            return RedirectToAction("GenerateServiceCalendar", new { Calendaryear = serInp.Calendaryear, tabs = "Tab2", Account = serInp.AccountId, Contract = serInp.ContractId });
        }


        [System.Web.Mvc.HttpGet]
        public JsonResult ClientAccountsContract(int accountid)
        {
            //Client client = (Client)Session["Client"];
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int clientid = client.ClientId;
            DbContextFactory Context = new DbContextFactory();
            Repo<NTTData.AMOPortal.Domain.Entities.ClientAccountContract> accounts = new Data.Repo<Domain.Entities.ClientAccountContract>(Context);
            List<NTTData.AMOPortal.Domain.Entities.ClientAccountContract> list = accounts.GetAll().Where(a => a.ClientId == clientid && a.AccountId == accountid).OrderBy(o => o.ContractName).ToList();
            var Items = list.Select(Field => new SelectListItem
            {
                Value = Field.Id.ToString(),
                Text = Field.ContractName.ToString()
            });

            return Json(Items, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NewServiceCalender()
        {
            ServiceCalendarInput mod = new ServiceCalendarInput();
            return View(mod);
        }
        public ActionResult getContractId(int Accid = 0)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            Db db = new Db();
            var list = (from clientaccount in db.ClientAccountContract
                        where clientaccount.ClientId == ClientId && clientaccount.AccountId == Accid
                        select clientaccount).ToList();
            var Items = list.Select(Field => new SelectListItem
            {
                Value = Field.Id.ToString(),
                Text = Field.ContractName.ToString()
            });
            return Json(Items, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReadServiceCalender(int Acccid, int Contrid, int SelectedYear,[DataSourceRequest] DataSourceRequest request)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            List<ServiceCalendarModel> model = getList(ClientId, SelectedYear, Acccid, Contrid);
            // return Json(model.AsEnumerable().ToDataSourceResult(request));           
            // return Json(model.AsEnumerable().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            var jsonResult = Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
       [HttpPost]
        public ActionResult ReadHolidayCalender(int Acccid, int Contrid, int SelectedYear, [DataSourceRequest] DataSourceRequest request)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;

            var contextFactory = new DbContextFactory();
            var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
            List<ServiceCalendarHolidayModel> model = unitOfWork.ServiceCalendar.Get().Where(f => f.AccountId == Acccid&&f.ContractId== Contrid&&f.Calendaryear== SelectedYear &&f.Holidayindicator==1 && f.IsDeleted==false)
                .Select(s=> new ServiceCalendarHolidayModel{Clientid = s.Clientid, AccountId=s.AccountId,ContractId=s.ContractId,Calendaryear=s.Calendaryear,Holidayname=s.Holidayname,
                Servicestarttime=s.Servicestarttime,Serviceendtime=s.Serviceendtime,Id=s.Id,
                    Calendardate=s.Calendardate
                }).ToList();

            var jsonResult = Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            //return jsonResult;
            return Json(model.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

        }
       
        [HttpPost]
        public ActionResult DeleteHolidayCalender( int scid ,int scCalendaryear)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
           
                int selectedyear = (int)scCalendaryear;
                var model = DeleteList(scid, selectedyear, ClientId);

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreateHolidayCalender([DataSourceRequest] DataSourceRequest request, ServiceCalendarInput scinput, int Acccid, int Contrid, int SelectedYear)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            int ClientId = client.ClientId;
            int id = 0;
            Db db = new Db();
            ServiceCalendar existingServiceCalendar = db.ServiceCalendar.FirstOrDefault(c => c.Calendardate == scinput.Calendardate && c.Clientid == ClientId && c.AccountId == Acccid && c.ContractId == Contrid);
            if (scinput != null && existingServiceCalendar==null)
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var input = new ServiceCalendar();
                    input.Clientid = ClientId;
                    input.Weekdayendnumber = Convert.ToByte((int)scinput.Calendardate.Value.DayOfWeek + 1);
                    input.Dayofweekname = scinput.Calendardate.Value.DayOfWeek.ToString();
                    input.Calendardate = scinput.Calendardate.Value;
                    input.Monthnumber = Convert.ToByte(scinput.Calendardate.Value.Month);
                    input.Calendaryear = Convert.ToInt16(scinput.Calendardate.Value.Year);
                    input.Monthname = (CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(scinput.Calendardate.Value.Month));
                    input.Weeknumber = Convert.ToByte(CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(scinput.Calendardate.Value, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday));
                    input.Daydate = Convert.ToByte(scinput.Calendardate.Value.Day);
                    input.Calendarquarternumber = Convert.ToByte((scinput.Calendardate.Value.Month - 1) / 3 + 1);
                    input.Holidayindicator = 1;
                    input.Holidayname = scinput.Holidayname;
                    input.AccountId = Acccid;
                    input.ContractId = Contrid; 
                    input.Servicestarttime = DateTime.Parse(scinput.StartTime).TimeOfDay;
                    input.Serviceendtime = DateTime.Parse(scinput.EndTime).TimeOfDay;
                    var totalhrs1 = DateTime.Today.Add(scinput.Serviceendtime.Value).Subtract(DateTime.Today.Add(scinput.Servicestarttime.Value)).TotalHours;
                    input.workinghours = Convert.ToInt32(totalhrs1) * 60;
                    input.Nonworkinghours = (24 * 60 - input.workinghours);

                    unitOfWork.ServiceCalendar.Insert(input);
                    unitOfWork.Save();
                    id = input.Id;
                }
            }
            else
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    var input = unitOfWork.ServiceCalendar.GetByID(existingServiceCalendar.Id);
                    input.Clientid = ClientId;
                    input.Weekdayendnumber = Convert.ToByte((int)scinput.Calendardate.Value.DayOfWeek + 1);
                    input.Dayofweekname = scinput.Calendardate.Value.DayOfWeek.ToString();
                    input.Calendardate = scinput.Calendardate.Value;
                    input.Monthnumber = Convert.ToByte(scinput.Calendardate.Value.Month);
                    input.Calendaryear = Convert.ToInt16(scinput.Calendardate.Value.Year);
                    input.Monthname = (CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(scinput.Calendardate.Value.Month));
                    input.Weeknumber = Convert.ToByte(CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(scinput.Calendardate.Value, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday));
                    input.Daydate = Convert.ToByte(scinput.Calendardate.Value.Day);
                    input.Calendarquarternumber = Convert.ToByte((scinput.Calendardate.Value.Month - 1) / 3 + 1);
                    input.Holidayindicator = 1;
                    input.Holidayname = scinput.Holidayname;
                    input.AccountId = Acccid;
                    input.ContractId = Contrid;
                    input.Servicestarttime = DateTime.Parse(scinput.StartTime).TimeOfDay;
                    input.Serviceendtime = DateTime.Parse(scinput.EndTime).TimeOfDay;
                    var totalhrs1 = DateTime.Today.Add(scinput.Serviceendtime.Value).Subtract(DateTime.Today.Add(scinput.Servicestarttime.Value)).TotalHours;
                    input.workinghours = Convert.ToInt32(totalhrs1) * 60;
                    input.Nonworkinghours = (24 * 60 - input.workinghours);

                    unitOfWork.ServiceCalendar.Update(input);
                    unitOfWork.Save();
                   // return 1;
                }
            }
            return Json(new[] { id }.ToDataSourceResult(request, ModelState));
        }
        public static bool DeleteList(int Id, int Year,int clientid)
        {
            ServiceCalendar serviceCal = new ServiceCalendar();
            ServiceCalendarModel serInp = new ServiceCalendarModel();
            Mapper<ServiceCalendar, ServiceCalendarModel> serMapper = new Mapper<ServiceCalendar, ServiceCalendarModel>();
            DbContextFactory Dbcon = new DbContextFactory();
            Repo<ServiceCalendar> serrepo = new Repo<ServiceCalendar>(Dbcon);
            CrudService<ServiceCalendar> serCrud = new CrudService<ServiceCalendar>(serrepo);
            using (var contextFactory = new DbContextFactory())
            {
                var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                ServiceCalendar existingServiceCalendar = unitOfWork.ServiceCalendar.Get(c => c.Id == Id  && c.Calendaryear == Year && c.Clientid == clientid).FirstOrDefault();
                existingServiceCalendar.Holidayindicator = 0;
                existingServiceCalendar.Holidayname = null;
                unitOfWork.ServiceCalendar.Update(existingServiceCalendar);
                unitOfWork.Save();
                return true;
            }
            //using (Db db = new Db())
            //{
            //    ServiceCalendar existingServiceCalendar = db.ServiceCalendar.FirstOrDefault(c => c.Id == Id);
            //    if (existingServiceCalendar != null)
            //    {
            //        serInp.Id = Id;
            //        serInp.Clientid = clientid;
            //        serInp.isdeleted = true;
            //        if (existingServiceCalendar.Servicestarttime != null)
            //            serInp.Servicestarttime = existingServiceCalendar.Servicestarttime;
            //        //serInp.StartTime = existingServiceCalendar.Servicestarttime.Value.ToString("hh:mm");
            //        if (existingServiceCalendar.Serviceendtime != null)
            //            serInp.Serviceendtime = existingServiceCalendar.Serviceendtime;
            //        //serInp.EndTime = existingServiceCalendar.Serviceendtime.Value.ToString("hh:mm");
            //        serInp.Weekdayendnumber = Convert.ToByte((int)existingServiceCalendar.Calendardate.Value.DayOfWeek + 1);
            //        serInp.Dayofweekname = existingServiceCalendar.Calendardate.Value.DayOfWeek.ToString();
            //        serInp.Calendardate = existingServiceCalendar.Calendardate.Value;
            //        serInp.Monthnumber = Convert.ToByte(existingServiceCalendar.Calendardate.Value.Month);
            //        serInp.Calendaryear = Convert.ToInt16(existingServiceCalendar.Calendardate.Value.Year);
            //        serInp.Monthname = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(existingServiceCalendar.Calendardate.Value.Month);
            //        serInp.Weeknumber = Convert.ToByte(CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(existingServiceCalendar.Calendardate.Value, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday));
            //        serInp.Daydate = Convert.ToByte(existingServiceCalendar.Calendardate.Value.Day);
            //        serInp.Calendarquarternumber = Convert.ToByte((existingServiceCalendar.Calendardate.Value.Month - 1) / 3 + 1);
            //        serInp.Holidayindicator = 0;
            //        serInp.Holidayname = null;
            //        serMapper.MapToEntity(serInp, serCrud.Get(serInp.Id));
            //        serCrud.Save();

            //    }

            //}

        }
        [HttpPost]
        public ActionResult EditServiceCalender(ServiceCalendarModel input)
        {
            ClientDetail client = (ClientDetail)Session["SessionClient"];
            input.Clientid = client.ClientId;

            var model = updateServiceCalendar(input);

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public static bool updateServiceCalendar(ServiceCalendarModel model)
        {
            try
            {
                using (var contextFactory = new DbContextFactory())
                {
                    var unitOfWork = new UnitOfWorks(contextFactory.GetContext());
                    ServiceCalendar existingServiceCalendar = unitOfWork.ServiceCalendar.Get(c => c.Daydate == model.Daydate&& c.Monthnumber==model.Monthnumber&&c.Calendaryear==model.Calendaryear && c.Clientid == model.Clientid && c.AccountId == model.AccountId && c.ContractId == model.ContractId).FirstOrDefault();
                    model.Servicestarttime = DateTime.Parse(model.StartTime).TimeOfDay;
                    model.Serviceendtime = DateTime.Parse(model.EndTime).TimeOfDay;
                    var totalhrs1 = DateTime.Today.Add(model.Serviceendtime.Value).Subtract(DateTime.Today.Add(model.Servicestarttime.Value)).TotalHours;
                    model.workinghours = Convert.ToInt32(totalhrs1) * 60;
                    model.Nonworkinghours = (24 * 60 - model.workinghours);
                    existingServiceCalendar.workinghours = model.workinghours;
                    existingServiceCalendar.Nonworkinghours = model.Nonworkinghours;

                    existingServiceCalendar.Servicestarttime = model.Servicestarttime;
                    existingServiceCalendar.Serviceendtime = model.Serviceendtime;
                    unitOfWork.ServiceCalendar.Update(existingServiceCalendar);
                    unitOfWork.Save();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

    }


}