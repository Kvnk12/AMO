using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.IncidentNavigator.AspxReport
{
    public class INTrendingReportEntity
    {

        public int ID { get; set; }
        public int Clientid { get; set; }
        public string TicketId { get; set; }

        private string _Incidentid;
        public string Incidentid
        {   get { return string.IsNullOrEmpty(_Incidentid.Trim()) ? "NULL" : _Incidentid; }
            set { _Incidentid = value; }
        }
       // public DateTime? Createddatetime { get; set; }

        private DateTime? _Createddatetime;
        public DateTime? Createddatetime
        {
            get
            {
                return _Createddatetime == null ? new DateTime().Date : _Createddatetime.Value.Date;
            }
            set
            {
                _Createddatetime = value;
            }
        }

        //   public string Status { get; set; }

        private string _Status;
        public string Status
        {
            get { return string.IsNullOrEmpty(_Status) ? "NULL" : _Status; }
            set { _Status = value; }
        }

        //public string Title { get; set; }
        private string _Title;
        public string Title
        {
            get { return string.IsNullOrEmpty(_Title) ? "NULL" : _Title; }
            set { _Title = value; }
        }


        // public string Priority { get; set; }
        private string _Priority;
        public string Priority
        {
            get { return string.IsNullOrEmpty(_Priority) ? "NULL" : _Priority; }
            set { _Priority = value; }
        }

        private string _ClientPriority;
        public string ClientPriority
        {
            get { return string.IsNullOrEmpty(_ClientPriority) ? "NULL" : _ClientPriority; }
            set { _ClientPriority = value; }
        }

        // public string Urgency { get; set; }

        private string _Urgency;
        public string Urgency
        {
            get { return string.IsNullOrEmpty(_Urgency) ? "NULL" : _Urgency; }
            set { _Urgency = value; }
        }

        //public string Type { get; set; }
        private string _Type;
        public string Type
        {
            get { return string.IsNullOrEmpty(_Type) ? "NULL" : _Type; }
            set { _Type = value; }
        }
        //public string Outagetype { get; set; }
        private string _Outagetype;
        public string Outagetype
        {
            get { return string.IsNullOrEmpty(_Outagetype) ? "NULL" : _Outagetype; }
            set { _Outagetype = value; }
        }

        //public double Age { get; set; }
        private int _Age;
        public int Age
        {
            //get { return Math.Round((DateTime.Now - CreatedDate).TotalDays,2) ; }
            get { return Convert.ToInt32((DateTime.Now - CreatedDate).TotalDays); }
            set { _Age = value; }
        }

        //public string Divisionimpacted { get; set; }
        private string _Divisionimpacted;
        public string Divisionimpacted
        {
            get { return string.IsNullOrEmpty(_Divisionimpacted) ? "NULL" : _Divisionimpacted; }
            set { _Divisionimpacted = value; }
        }
        // public string Businessunitimpacted { get; set; }
        private string _Businessunitimpacted;
        public string Businessunitimpacted
        {
            get { return string.IsNullOrEmpty(_Businessunitimpacted) ? "NULL" : _Businessunitimpacted; }
            set { _Businessunitimpacted = value; }
        }
        // public string Servicecategory { get; set; }
        private string _Servicecategory;
        public string Servicecategory
        {
            get { return string.IsNullOrEmpty(_Servicecategory) ? "NULL" : _Servicecategory; }
            set { _Servicecategory = value; }
        }
        // public string Servicesubcategory { get; set; }
        private string _Servicesubcategory;
        public string Servicesubcategory
        {
            get { return string.IsNullOrEmpty(_Servicesubcategory) ? "NULL" : _Servicesubcategory; }
            set { _Servicesubcategory = value; }
        }
        // public string Servicearea { get; set; }
        private string _Servicearea;
        public string Servicearea
        {
            get { return string.IsNullOrEmpty(_Servicearea) ? "NULL" : _Servicearea; }
            set { _Servicearea = value; }
        }
        //  public string Regionimpacted { get; set; }
        private string _Regionimpacted;
        public string Regionimpacted
        {
            get { return string.IsNullOrEmpty(_Regionimpacted) ? "NULL" : _Regionimpacted; }
            set { _Regionimpacted = value; }
        }
        // public string Locationsimpacted { get; set; }
        private string _Locationsimpacted;
        public string Locationsimpacted
        {
            get { return string.IsNullOrEmpty(_Locationsimpacted) ? "NULL" : _Locationsimpacted; }
            set { _Locationsimpacted = value; }
        }
        // public string Customerdept { get; set; }
        private string _Customerdept;
        public string Customerdept
        {
            get { return string.IsNullOrEmpty(_Customerdept) ? "NULL" : _Customerdept; }
            set { _Customerdept = value; }
        }
        // public string Customerlastname { get; set; }
        private string _Customerlastname;
        public string Customerlastname
        {
            get { return string.IsNullOrEmpty(_Customerlastname) ? "NULL" : _Customerlastname; }
            set { _Customerlastname = value; }
        }
        // public string Customerfirstname { get ; set; }
        private string _Customerfirstname;
        public string Customerfirstname
        {
            get { return string.IsNullOrEmpty(_Customerfirstname) ? "NULL" : _Customerfirstname; }
            set { _Customerfirstname = value; }
        }
        //  public string Customertitle { get; set; }
        private string _Customertitle;
        public string Customertitle
        {
            get { return string.IsNullOrEmpty(_Customertitle) ? "NULL" : _Customertitle; }
            set { _Customertitle = value; }
        }
        // public string Customersatisfaction { get; set; }
        private string _Customersatisfaction;
        public string Customersatisfaction
        {
            get { return string.IsNullOrEmpty(_Customersatisfaction) ? "NULL" : _Customersatisfaction; }
            set { _Customersatisfaction = value; }
        }
        //  public string Impacttype { get; set; }
        private string _Impacttype;
        public string Impacttype
        {
            get { return string.IsNullOrEmpty(_Impacttype) ? "NULL" : _Impacttype; }
            set { _Impacttype = value; }
        }

        //  public string Impactdescription { get; set; }
        //  public string Configitemid { get; set; }
        private string _Configitemid;
        public string Configitemid
        {
            get { return string.IsNullOrEmpty(_Configitemid) ? "NULL" : _Configitemid; }
            set { _Configitemid = value; }
        }
        //  public string Configtype { get; set; }
        private string _Configtype;
        public string Configtype
        {
            get { return string.IsNullOrEmpty(_Configtype) ? "NULL" : _Configtype; }
            set { _Configtype = value; }
        }
        // public string Configitemname { get; set; }
        private string _Configitemname;
        public string Configitemname
        {
            get { return string.IsNullOrEmpty(_Configitemname) ? "NULL" : _Configitemname; }
            set { _Configitemname = value; }
        }
        //public string Environmentimpacted { get; set; }
        private string _Environmentimpacted;
        public string Environmentimpacted
        {
            get { return string.IsNullOrEmpty(_Environmentimpacted) ? "NULL" : _Environmentimpacted; }
            set { _Environmentimpacted = value; }
        }
        //  public string Module { get; set; }
        //  public string Nodename { get; set; }
        private string _Nodename;
        public string Nodename
        {
            get { return string.IsNullOrEmpty(_Nodename) ? "NULL" : _Nodename; }
            set { _Nodename = value; }
        }
        // public string Rootcausetype { get; set; }
        private string _Rootcausetype;
        public string Rootcausetype
        {
            get { return string.IsNullOrEmpty(_Rootcausetype) ? "NULL" : _Rootcausetype; }
            set { _Rootcausetype = value; }
        }
        //  public string Rootcausedescription { get; set; }
        private string _Rootcausedescription;
        public string Rootcausedescription
        {
            get { return string.IsNullOrEmpty(_Rootcausedescription) ? "NULL" : _Rootcausedescription; }
            set { _Rootcausedescription = value; }
        }
        //  public string Resolutiontype { get; set; }
        private string _Resolutiontype;
        public string Resolutiontype
        {
            get { return string.IsNullOrEmpty(_Resolutiontype) ? "NULL" : _Resolutiontype; }
            set { _Resolutiontype = value; }
        }
        // public string Resolutiondescription { get; set; }
        //  public string Solutionid { get; set; }
        private string _Solutionid;
        public string Solutionid
        {
            get { return string.IsNullOrEmpty(_Solutionid) ? "NULL" : _Solutionid; }
            set { _Solutionid = value; }
        }
        //  public string Deploymentrelease { get; set; }
        private string _Deploymentrelease;
        public string Deploymentrelease
        {
            get { return string.IsNullOrEmpty(_Deploymentrelease) ? "NULL" : _Deploymentrelease; }
            set { _Deploymentrelease = value; }
        }
        // public string Relatedchangeids { get; set; }
        private string _Relatedchangeids;
        public string Relatedchangeids
        {
            get { return string.IsNullOrEmpty(_Relatedchangeids) ? "NULL" : _Relatedchangeids; }
            set { _Relatedchangeids = value; }
        }
        // public string Relatedids { get; set; }
        private string _Relatedids;
        public string Relatedids
        {
            get { return string.IsNullOrEmpty(_Relatedids) ? "NULL" : _Relatedids; }
            set { _Relatedids = value; }
        }
        //public string Relatedproblemids { get; set; }
        private string _Relatedproblemids;
        public string Relatedproblemids
        {
            get { return string.IsNullOrEmpty(_Relatedproblemids) ? "NULL" : _Relatedproblemids; }
            set { _Relatedproblemids = value; }
        }
        // public string Helpdesk { get; set; }
        private string _Helpdesk;
        public string Helpdesk
        {
            get { return string.IsNullOrEmpty(_Helpdesk) ? "NULL" : _Helpdesk; }
            set { _Helpdesk = value; }
        }
        // public string Assignedtoperson { get; set; }
        private string _Assignedtoperson;
        public string Assignedtoperson
        {
            get { return string.IsNullOrEmpty(_Assignedtoperson) ? "NULL" : _Assignedtoperson; }
            set { _Assignedtoperson = value; }
        }
        //public string Assignedtosupportgroup { get; set; }
        private string _Assignedtosupportgroup;
        public string Assignedtosupportgroup
        {
            get { return string.IsNullOrEmpty(_Assignedtosupportgroup) ? "NULL" : _Assignedtosupportgroup; }
            set { _Assignedtosupportgroup = value; }
        }
        // public string Createdbygroup { get; set; }
        private string _Createdbygroup;
        public string Createdbygroup
        {
            get { return string.IsNullOrEmpty(_Createdbygroup) ? "NULL" : _Createdbygroup; }
            set { _Createdbygroup = value; }
        }
        // public string Createdbyperson { get; set; }
        private string _Createdbyperson;
        public string Createdbyperson
        {
            get { return string.IsNullOrEmpty(_Createdbyperson) ? "NULL" : _Createdbyperson; }
            set { _Createdbyperson = value; }
        }
        // public string Createdtimezone { get; set; }
        private string _Createdtimezone;
        public string Createdtimezone
        {
            get { return string.IsNullOrEmpty(_Createdtimezone) ? "NULL" : _Createdtimezone; }
            set { _Createdtimezone = value; }
        }
        private string _createdtime;
        public string Createdtime {
            get {
                return String.IsNullOrWhiteSpace(_createdtime) ? "NULL":_createdtime.Trim().Split(':').ToArray().ElementAt(0);
            }
            set
            {
                _createdtime = value;
            }
        }
        public DateTime ResolvedDate
        {
            get
            {
                return Resolutiondatetime == null ? new DateTime().Date : Resolutiondatetime.Value.Date;
            }
        }
        public DateTime CreatedDate
        {
            get
            {
                return Createddatetime == null ? new DateTime().Date : Createddatetime.Value.Date;
            }
        }
        //  public string Createdweekday { get; set; }
        private string _Createdweekday;
        public string Createdweekday
        {
            get { //return string.IsNullOrEmpty(_Createdweekday) ? "NULL" : _Createdweekday; 
               
                if (string.IsNullOrEmpty(_Createdweekday))
                {
                    return "UNKNOWN";
                }
                else
                {
                    _Createdweekday = _Createdweekday.Trim();
                    switch (_Createdweekday)
                    {
                        case "Sun":
                            _Createdweekday = "01-" + _Createdweekday;
                            break;
                        case "Mon":
                            _Createdweekday = "02-" + _Createdweekday;
                            break;
                        case "Tue":
                            _Createdweekday = "03-" + _Createdweekday;
                            break;
                        case "Wed":
                            _Createdweekday = "04-" + _Createdweekday;
                            break;
                        case "Thu":
                            _Createdweekday = "05-" + _Createdweekday;
                            break;
                        case "Fri":
                            _Createdweekday = "06-" + _Createdweekday;
                            break;
                        case "Sat":
                            _Createdweekday = "07-" + _Createdweekday;
                            break;
                    }
                    return _Createdweekday;
                }
            }
            set { _Createdweekday = value; }
        }
        //  public string Createdweeknum { get; set; }
        private string _Createdweeknum;
        public string Createdweeknum
        {
            get { return string.IsNullOrEmpty(_Createdweeknum) ? "NULL" : _Createdweeknum; }
            set { _Createdweeknum = value; }
        }
        // public string Createdmonth { get; set; }
        private string _Createdmonth;
        public string Createdmonth
        {
            get
            {
                if (string.IsNullOrEmpty(_Createdmonth))
                {
                    return null;
                }
                else
                {
                    switch (_Createdmonth)
                    {
                        case "Jan":
                            _Createdmonth = "01-" + _Createdmonth;
                            break;
                        case "Feb":
                            _Createdmonth = "02-" + _Createdmonth;
                            break; 
                        case "Mar":
                            _Createdmonth = "03-" + _Createdmonth;
                            break; 
                        case "Apr":
                            _Createdmonth = "04-" + _Createdmonth;
                            break;
                        case "May":
                            _Createdmonth = "05-" + _Createdmonth;
                            break;
                        case "Jun":
                            _Createdmonth = "06-" + _Createdmonth;
                            break; 
                        case "Jul":
                            _Createdmonth = "07-" + _Createdmonth;
                            break; 
                        case "Aug":
                            _Createdmonth = "08-" + _Createdmonth;
                            break;
                        case "Sep":
                            _Createdmonth = "09-" + _Createdmonth;
                            break;
                        case "Oct":
                            _Createdmonth = "10-" + _Createdmonth;
                            break; 
                        case "Nov":
                            _Createdmonth = "11-" + _Createdmonth;
                            break;
                        case "Dec":
                            _Createdmonth = "12-" + _Createdmonth;
                            break;
                       
                    }
                      

                    return _Createdmonth;
                }
                // return string.IsNullOrEmpty(_Createdmonth) ? "NULL" : _Createdmonth; 
            }
            set { _Createdmonth = value; }
        }
        // public string Createdyear { get; set; }
        private string _Createdyear;
        public string Createdyear
        {
            get { return string.IsNullOrEmpty(_Createdyear) ? "NULL" : _Createdyear; }
            set { _Createdyear = value; }
        }
        // public string Acknowlededbygroup { get; set; }
        private string _Acknowlededbygroup;
        public string Acknowlededbygroup
        {
            get { return string.IsNullOrEmpty(_Acknowlededbygroup) ? "NULL" : _Acknowlededbygroup; }
            set { _Acknowlededbygroup = value; }
        }
        // public string Acknowlededbyperson { get; set; }
        private string _Acknowlededbyperson;
        public string Acknowlededbyperson
        {
            get { return string.IsNullOrEmpty(_Acknowlededbyperson) ? "NULL" : _Acknowlededbyperson; }
            set { _Acknowlededbyperson = value; }
        }
        // public DateTime? Acknowledgeddatetime { get; set; }
        private DateTime? _Acknowledgeddatetime;
        public DateTime? Acknowledgeddatetime
        {
            get
            {
                return _Acknowledgeddatetime == null ? new DateTime().Date : _Acknowledgeddatetime.Value.Date;
            }
            set
            {
                _Acknowledgeddatetime = value;
            }
        }
        // public string Acknowledgedtimezone { get; set; }
        private string _Acknowledgedtimezone;
        public string Acknowledgedtimezone
        {
            get { return string.IsNullOrEmpty(_Acknowledgedtimezone) ? "NULL" : _Acknowledgedtimezone; }
            set { _Acknowledgedtimezone = value; }
        }
        // public string Acknowledgedtime { get; set; }
        private string _Acknowledgedtime;
        public string Acknowledgedtime
        {
            get { return string.IsNullOrEmpty(_Acknowledgedtime) ? "NULL" : _Acknowledgedtime; }
            set { _Acknowledgedtime = value; }
        }
        

        // public string Acknowledgedweekday { get; set; }
        private string _Acknowledgedweekday;
        public string Acknowledgedweekday
        {
            get {              
                if (string.IsNullOrEmpty(_Acknowledgedweekday))
                {
                    return "UNKNOWN";
                }
                else
                {
                    _Acknowledgedweekday = _Acknowledgedweekday.Trim();
                    switch (_Acknowledgedweekday)
                    {
                        case "Sun":
                            _Acknowledgedweekday = "01-" + _Acknowledgedweekday;
                            break;
                        case "Mon":
                            _Acknowledgedweekday = "02-" + _Acknowledgedweekday;
                            break;
                        case "Tue":
                            _Acknowledgedweekday = "03-" + _Acknowledgedweekday;
                            break;
                        case "Wed":
                            _Acknowledgedweekday = "04-" + _Acknowledgedweekday;
                            break;
                        case "Thu":
                            _Acknowledgedweekday = "05-" + _Acknowledgedweekday;
                            break;
                        case "Fri":
                            _Acknowledgedweekday = "06-" + _Acknowledgedweekday;
                            break;
                        case "Sat":
                            _Acknowledgedweekday = "07-" + _Acknowledgedweekday;
                            break;
                    }
                    return _Acknowledgedweekday;
                }
                //return string.IsNullOrEmpty(_Acknowledgedweekday) ? "NULL" : _Acknowledgedweekday;
            }
            set { _Acknowledgedweekday = value; }
        }
        //public string Acknowledgedweeknum { get; set; }
        private string _Acknowledgedweeknum;
        public string Acknowledgedweeknum
        {
            get { return string.IsNullOrEmpty(_Acknowledgedweeknum) ? "NULL" : _Acknowledgedweeknum; }
            set { _Acknowledgedweeknum = value; }
        }
        //  public string Acknowledgedmonth { get; set; }
        private string _Acknowledgedmonth;
        public string Acknowledgedmonth
        {
            get 
            {
                if (string.IsNullOrEmpty(_Acknowledgedmonth))
                {
                    return null;
                }
                else
                {
                    switch (_Acknowledgedmonth)
                    {
                        case "Jan":
                            _Acknowledgedmonth = "01-" + _Acknowledgedmonth;
                            break;
                        case "Feb":
                            _Acknowledgedmonth = "02-" + _Acknowledgedmonth;
                            break;
                        case "Mar":
                            _Acknowledgedmonth = "03-" + _Acknowledgedmonth;
                            break;
                        case "Apr":
                            _Acknowledgedmonth = "04-" + _Acknowledgedmonth;
                            break;
                        case "May":
                            _Acknowledgedmonth = "05-" + _Acknowledgedmonth;
                            break;
                        case "Jun":
                            _Acknowledgedmonth = "06-" + _Acknowledgedmonth;
                            break;
                        case "Jul":
                            _Acknowledgedmonth = "07-" + _Acknowledgedmonth;
                            break;
                        case "Aug":
                            _Acknowledgedmonth = "08-" + _Acknowledgedmonth;
                            break;
                        case "Sep":
                            _Acknowledgedmonth = "09-" + _Acknowledgedmonth;
                            break;
                        case "Oct":
                            _Acknowledgedmonth = "10-" + _Acknowledgedmonth;
                            break;
                        case "Nov":
                            _Acknowledgedmonth = "11-" + _Acknowledgedmonth;
                            break;
                        case "Dec":
                            _Acknowledgedmonth = "12-" + _Acknowledgedmonth;
                            break;                       

                    }


                    return _Acknowledgedmonth;
                }
                //   return string.IsNullOrEmpty(_Acknowledgedmonth) ? "NULL" : _Acknowledgedmonth;
            }
            set { _Acknowledgedmonth = value; }
        }
        //public string Acknowledgedyear { get; set; }
        private string _Acknowledgedyear;
        public string Acknowledgedyear
        {
            get { return string.IsNullOrEmpty(_Acknowledgedyear) ? "NULL" : _Acknowledgedyear; }
            set { _Acknowledgedyear = value; }
        }
        // public string Resolvedbygroup { get; set; }
        private string _Resolvedbygroup;
        public string Resolvedbygroup
        {
            get { return string.IsNullOrEmpty(_Resolvedbygroup) ? "NULL" : _Resolvedbygroup; }
            set { _Resolvedbygroup = value; }
        }
        // public string Resolvedbyperson { get; set; }
        private string _Resolvedbyperson;
        public string Resolvedbyperson
        {
            get { return string.IsNullOrEmpty(_Resolvedbyperson) ? "NULL" : _Resolvedbyperson; }
            set { _Resolvedbyperson = value; }
        }
        // public string Resolvedbyvendor { get; set; }
        private string _Resolvedbyvendor;
        public string Resolvedbyvendor
        {
            get { return string.IsNullOrEmpty(_Resolvedbyvendor) ? "NULL" : _Resolvedbyvendor; }
            set { _Resolvedbyvendor = value; }
        }
        // public string Resolutionefforthours { get; set; }
        private string _Resolutionefforthours;
        public string Resolutionefforthours
        {
            get { return string.IsNullOrEmpty(_Resolutionefforthours) ? "NULL" : _Resolutionefforthours; }
            set { _Resolutionefforthours = value; }
        }
        //  public DateTime? Resolutiondatetime { get; set; }

        private DateTime? _Resolutiondatetime;
        public DateTime? Resolutiondatetime
        {
            get
            {
                return _Resolutiondatetime == null ? new DateTime().Date : _Resolutiondatetime.Value.Date;
            }
            set
            {
                _Resolutiondatetime = value;
            }
        }

        //public string Resolvedtimezone { get; set; }
        private string _Resolvedtimezone;
        public string Resolvedtimezone
        {
            get { return string.IsNullOrEmpty(_Resolvedtimezone) ? "NULL" : _Resolvedtimezone; }
            set { _Resolvedtimezone = value; }
        }
        public int? Resolutiontime { get; set; }
        // public string Resolutionweekday { get; set; }
        private string _Resolutionweekday;
        public string Resolutionweekday
        {
            get {
                // return string.IsNullOrEmpty(_Resolutionweekday) ? "NULL" : _Resolutionweekday; 
              
                if (string.IsNullOrEmpty(_Resolutionweekday))
                {
                    return "UNKNOWN";
                }
                else
                {
                    _Resolutionweekday = _Resolutionweekday.Trim();
                    switch (_Resolutionweekday)
                    {
                        case "Sun":
                            _Resolutionweekday = "01-" + _Resolutionweekday;
                            break;
                        case "Mon":
                            _Resolutionweekday = "02-" + _Resolutionweekday;
                            break;
                        case "Tue":
                            _Resolutionweekday = "03-" + _Resolutionweekday;
                            break;
                        case "Wed":
                            _Resolutionweekday = "04-" + _Resolutionweekday;
                            break;
                        case "Thu":
                            _Resolutionweekday = "05-" + _Resolutionweekday;
                            break;
                        case "Fri":
                            _Resolutionweekday = "06-" + _Resolutionweekday;
                            break;
                        case "Sat":
                            _Resolutionweekday = "07-" + _Resolutionweekday;
                            break;
                    }
                  
                    return _Resolutionweekday;
                }
            }
            set { _Resolutionweekday = value; }
        }
        //   public string Resolutionweeknum { get; set; }
        private string _Resolutionweeknum;
        public string Resolutionweeknum
        {
            get { return string.IsNullOrEmpty(_Resolutionweeknum) ? "NULL" : _Resolutionweeknum; }
            set { _Resolutionweeknum = value; }
        }
        // public string Servicerestoredbygroup { get; set; }
        private string _Servicerestoredbygroup;
        public string Servicerestoredbygroup
        {
            get { return string.IsNullOrEmpty(_Servicerestoredbygroup) ? "NULL" : _Servicerestoredbygroup; }
            set { _Servicerestoredbygroup = value; }
        }
        // public string Servicerestoredbyperson { get; set; }
        private string _Servicerestoredbyperson;
        public string Servicerestoredbyperson
        {
            get { return string.IsNullOrEmpty(_Servicerestoredbyperson) ? "NULL" : _Servicerestoredbyperson; }
            set { _Servicerestoredbyperson = value; }
        }
       // public DateTime? Servicerestoreddatetime { get; set; }

        private DateTime? _Servicerestoreddatetime;
        public DateTime? Servicerestoreddatetime
        {
            get
            {
                return _Servicerestoreddatetime == null ? new DateTime().Date : _Servicerestoreddatetime.Value.Date;
            }
            set
            {
                _Servicerestoreddatetime = value;
            }
        }

        ////public string Servicerestoreddate
        ////{
        ////    get
        ////    {
        ////        return Servicerestoreddatetime == null ? "NV" : Servicerestoreddatetime.Value.ToString("yyyy-MM-dd");
        ////    }
        ////}

        //  public string Servicerestoredtimezone { get; set; }
        private string _Servicerestoredtimezone;
        public string Servicerestoredtimezone
        {
            get { return string.IsNullOrEmpty(_Servicerestoredtimezone) ? "NULL" : _Servicerestoredtimezone; }
            set { _Servicerestoredtimezone = value; }
        }
        // public string Servicerestoredtime { get; set; }
        private string _Servicerestoredtime;
        public string Servicerestoredtime
        {
            get { return string.IsNullOrEmpty(_Servicerestoredtime) ? "NULL" : _Servicerestoredtime; }
            set { _Servicerestoredtime = value; }
        }
        // public string Servicerestoredweekday { get; set; }
        private string _Servicerestoredweekday;
        public string Servicerestoredweekday
        {
            get {
                // return string.IsNullOrEmpty(_Servicerestoredweekday) ? "NULL" : _Servicerestoredweekday;
         
                if (string.IsNullOrEmpty(_Servicerestoredweekday))
                {
                    return "UNKNOWN";
                }
                else
                {
                    _Servicerestoredweekday = _Servicerestoredweekday.Trim();
                    switch (_Servicerestoredweekday)
                    {
                        case "Sun":
                            _Servicerestoredweekday = "01-" + _Servicerestoredweekday;
                            break;
                        case "Mon":
                            _Servicerestoredweekday = "02-" + _Servicerestoredweekday;
                            break;
                        case "Tue":
                            _Servicerestoredweekday = "03-" + _Servicerestoredweekday;
                            break;
                        case "Wed":
                            _Servicerestoredweekday = "04-" + _Servicerestoredweekday;
                            break;
                        case "Thu":
                            _Servicerestoredweekday = "05-" + _Servicerestoredweekday;
                            break;
                        case "Fri":
                            _Servicerestoredweekday = "06-" + _Servicerestoredweekday;
                            break;
                        case "Sat":
                            _Servicerestoredweekday = "07-" + _Servicerestoredweekday;
                            break;
                    }
                    return _Servicerestoredweekday;
                }
            }
            set { _Servicerestoredweekday = value; }
        }
        //public string Servicerestoredweeknum { get; set; }
        private string _Servicerestoredweeknum;
        public string Servicerestoredweeknum
        {
            get { return string.IsNullOrEmpty(_Servicerestoredweeknum) ? "NULL" : _Servicerestoredweeknum; }
            set { _Servicerestoredweeknum = value; }
        }
        // public string Servicerestoredmonth { get; set; }
        private string _Servicerestoredmonth;
        public string Servicerestoredmonth
        {
            get {
                if (string.IsNullOrEmpty(_Servicerestoredmonth))
                {
                    return null;
                }
                else
                {
                    switch (_Servicerestoredmonth)
                    {
                        case "Jan":
                            _Servicerestoredmonth = "01-" + _Servicerestoredmonth;
                            break;
                        case "Feb":
                            _Servicerestoredmonth = "02-" + _Servicerestoredmonth;
                            break;
                        case "Mar":
                            _Servicerestoredmonth = "03-" + _Servicerestoredmonth;
                            break;
                        case "Apr":
                            _Servicerestoredmonth = "04-" + _Servicerestoredmonth;
                            break;
                        case "May":
                            _Servicerestoredmonth = "05-" + _Servicerestoredmonth;
                            break;
                        case "Jun":
                            _Servicerestoredmonth = "06-" + _Servicerestoredmonth;
                            break;
                        case "Jul":
                            _Servicerestoredmonth = "07-" + _Servicerestoredmonth;
                            break;
                        case "Aug":
                            _Servicerestoredmonth = "08-" + _Servicerestoredmonth;
                            break;
                        case "Sep":
                            _Servicerestoredmonth = "09-" + _Servicerestoredmonth;
                            break;
                        case "Oct":
                            _Servicerestoredmonth = "10-" + _Servicerestoredmonth;
                            break;
                        case "Nov":
                            _Servicerestoredmonth = "11-" + _Servicerestoredmonth;
                            break;
                        case "Dec":
                            _Servicerestoredmonth = "12-" + _Servicerestoredmonth;
                            break;
                    }
                    return _Servicerestoredmonth;
                }
                // return string.IsNullOrEmpty(_Servicerestoredmonth) ? "NULL" : _Servicerestoredmonth; 
            }
            set { _Servicerestoredmonth = value; }
        }
        //public string Servicerestoredyear { get; set; }
        private string _Servicerestoredyear;
        public string Servicerestoredyear
        {
            get { return string.IsNullOrEmpty(_Servicerestoredyear) ? "NULL" : _Servicerestoredyear; }
            set { _Servicerestoredyear = value; }
        }
        //  public string Lastupdatedbyperson { get; set; }
        private string _Lastupdatedbyperson;
        public string Lastupdatedbyperson
        {
            get { return string.IsNullOrEmpty(_Lastupdatedbyperson) ? "NULL" : _Lastupdatedbyperson; }
            set { _Lastupdatedbyperson = value; }
        }
       // public DateTime? Lastupdateddatetime { get; set; }

        private DateTime? _Lastupdateddatetime;
        public DateTime? Lastupdateddatetime
        {
            get
            {
                return _Lastupdateddatetime == null ? new DateTime().Date : _Lastupdateddatetime.Value.Date;
            }
            set
            {
                _Lastupdateddatetime = value;
            }
        }

        // public string Datasource { get; set; }
        private string _Datasource;
        public string Datasource
        {
            get { return string.IsNullOrEmpty(_Datasource) ? "NULL" : _Datasource; }
            set { _Datasource = value; }
        }
        // public string Datasourcenotes { get; set; }
        private string _Datasourcenotes;
        public string Datasourcenotes
        {
            get { return string.IsNullOrEmpty(_Datasourcenotes) ? "NULL" : _Datasourcenotes; }
            set { _Datasourcenotes = value; }
        }
        // public string Assignedbygroup { get; set; }
        private string _Assignedbygroup;
        public string Assignedbygroup
        {
            get { return string.IsNullOrEmpty(_Assignedbygroup) ? "NULL" : _Assignedbygroup; }
            set { _Assignedbygroup = value; }
        }
        //    public string Assignedbyperson { get; set; }
        private string _Assignedbyperson;
        public string Assignedbyperson
        {
            get { return string.IsNullOrEmpty(_Assignedbyperson) ? "NULL" : _Assignedbyperson; }
            set { _Assignedbyperson = value; }
        }
       // public DateTime? Assigneddatetime { get; set; }
        private DateTime? _Assigneddatetime;
        public DateTime? Assigneddatetime
        {
            get
            {
                return _Assigneddatetime == null ? new DateTime().Date : _Assigneddatetime.Value.Date;
            }
            set
            {
                _Assigneddatetime = value;
            }
        }

        ////public string Assigneddate
        ////{
        ////    get
        ////    {
        ////        return Assigneddatetime == null ? "NV" : Assigneddatetime.Value.ToString("yyyy-MM-dd");
        ////    }
        ////}

        // public string Esitmatedresponsetimebygroup { get; set; }
        private string _Esitmatedresponsetimebygroup;
        public string Esitmatedresponsetimebygroup
        {
            get { return string.IsNullOrEmpty(_Esitmatedresponsetimebygroup) ? "NULL" : _Esitmatedresponsetimebygroup; }
            set { _Esitmatedresponsetimebygroup = value; }
        }
        public DateTime? Esitmatedresponsedatetime { get; set; }
        ////public string Esitmatedresponsedate
        ////{
        ////    get
        ////    {
        ////        return Esitmatedresponsedatetime == null ? "NV" : Esitmatedresponsedatetime.Value.ToString("yyyy-MM-dd");
        ////    }
        ////}


        // public string Resolutionyear { get; set; }
        private string _Resolutionyear;
        public string Resolutionyear
        {
            get { return string.IsNullOrEmpty(_Resolutionyear) ? "NULL" : _Resolutionyear; }
            set { _Resolutionyear = value; }
        }
        // public string Resolutionmonth { get; set; }
        private string _Resolutionmonth;
        public string Resolutionmonth
        {
            get {
                if (string.IsNullOrEmpty(_Resolutionmonth))
                {
                    return null;
                }
                else
                {
                    _Resolutionmonth = _Resolutionmonth.Trim();
                    switch (_Resolutionmonth)
                    {
                        case "Jan":
                            _Resolutionmonth = "01-" + _Resolutionmonth;
                            break;
                        case "Feb":
                            _Resolutionmonth = "02-" + _Resolutionmonth;
                            break;
                        case "Mar":
                            _Resolutionmonth = "03-" + _Resolutionmonth;
                            break;
                        case "Apr":
                            _Resolutionmonth = "04-" + _Resolutionmonth;
                            break;
                        case "May":
                            _Resolutionmonth = "05-" + _Resolutionmonth;
                            break;
                        case "Jun":
                            _Resolutionmonth = "06-" + _Resolutionmonth;
                            break;
                        case "Jul":
                            _Resolutionmonth = "07-" + _Resolutionmonth;
                            break;
                        case "Aug":
                            _Resolutionmonth = "08-" + _Resolutionmonth;
                            break;
                        case "Sep":
                            _Resolutionmonth = "09-" + _Resolutionmonth;
                            break;
                        case "Oct":
                            _Resolutionmonth = "10-" + _Resolutionmonth;
                            break;
                        case "Nov":
                            _Resolutionmonth = "11-" + _Resolutionmonth;
                            break;
                        case "Dec":
                            _Resolutionmonth = "12-" + _Resolutionmonth;
                            break;
                    }
                    return _Resolutionmonth;
                }
                // return string.IsNullOrEmpty(_Resolutionmonth) ? "NULL" : _Resolutionmonth;
            }
            set { _Resolutionmonth = value; }
        }
        // public string Waivedindicator { get; set; }
        private string _Waivedindicator;
        public string Waivedindicator
        {
            get { return string.IsNullOrEmpty(_Waivedindicator) ? "NULL" : _Waivedindicator; }
            set { _Waivedindicator = value; }
        }
       // public DateTime? Esitmatedresolutiondate { get; set; }

        private DateTime? _Esitmatedresolutiondate;
        public DateTime? Esitmatedresolutiondate
        {
            get
            {
                return _Esitmatedresolutiondate == null ? new DateTime().Date : _Esitmatedresolutiondate.Value.Date;
            }
            set
            {
                _Esitmatedresolutiondate = value;
            }
        }


        public int? Totalsuspenddurationassignedtoack { get; set; }
        public int? Totalsuspenddurationacktoer { get; set; }
        public int? Totalsuspenddurationertoresolution { get; set; }
        //public string Resolvedindicator { get; set; }
        private string _Resolvedindicator;
        public string Resolvedindicator
        {
            get { return string.IsNullOrEmpty(_Resolvedindicator) ? "NULL" : _Resolvedindicator; }
            set { _Resolvedindicator = value; }
        }
        //public string IsNTTGroup { get; set; }
        private string _IsNTTGroup;
        public string IsNTTGroup
        {
            get { return string.IsNullOrEmpty(_IsNTTGroup) ? "NULL" : _IsNTTGroup; }
            set { _IsNTTGroup = value; }
        }

        // public string SupportGroupType { get; set; }
        private string _SupportGroupType;
        public string SupportGroupType
        {
            get { return string.IsNullOrEmpty(_SupportGroupType) ? "NULL" : _SupportGroupType; }
            set { _SupportGroupType = value; }
        }
    //    public DateTime? InProgressdatetime { get; set; }

        private DateTime? _InProgressdatetime;
        public DateTime? InProgressdatetime
        {
            get
            {
                return _InProgressdatetime == null ? new DateTime().Date : _InProgressdatetime.Value.Date;
            }
            set
            {
                _InProgressdatetime = value;
            }
        }

        // public DateTime? Closeddatetime { get; set; }

        private DateTime? _Closeddatetime;
        public DateTime? Closeddatetime
        {
            get
            {
                return _Closeddatetime == null ? new DateTime().Date : _Closeddatetime.Value.Date;
            }
            set
            {
                _Closeddatetime = value;
            }
        }



       // public DateTime? ScheduledstartdateTime { get; set; }
        private DateTime? _ScheduledstartdateTime;
        public DateTime? ScheduledstartdateTime
        {
            get
            {
                return _ScheduledstartdateTime == null ? new DateTime().Date : _ScheduledstartdateTime.Value.Date;
            }
            set
            {
                _ScheduledstartdateTime = value;
            }
        }


        //  public DateTime? ScheduledenddateTime { get; set; }

        private DateTime? _ScheduledenddateTime;
        public DateTime? ScheduledenddateTime
        {
            get
            {
                return _ScheduledenddateTime == null ? new DateTime().Date : _ScheduledenddateTime.Value.Date;
            }
            set
            {
                _ScheduledenddateTime = value;
            }
        }

        public int? Scheduledchangecount { get; set; }
        // public string Company { get; set; }
        private string _Company;
        public string Company
        {
            get { return string.IsNullOrEmpty(_Company) ? "NULL" : _Company; }
            set { _Company = value; }
        }
        // public string Site { get; set; }
        private string _Site;
        public string Site
        {
            get { return string.IsNullOrEmpty(_Site) ? "NULL" : _Site; }
            set { _Site = value; }
        }
        // public string Assigneecompany { get; set; }
        private string _Assigneecompany;
        public string Assigneecompany
        {
            get { return string.IsNullOrEmpty(_Assigneecompany) ? "NULL" : _Assigneecompany; }
            set { _Assigneecompany = value; }
        }
        // public string Assigneeorganization { get; set; }
        private string _Assigneeorganization;
        public string Assigneeorganization
        {
            get { return string.IsNullOrEmpty(_Assigneeorganization) ? "NULL" : _Assigneeorganization; }
            set { _Assigneeorganization = value; }
        }
        public int? Estimatedworkeffort { get; set; }
        public int? Actualworkeffort { get; set; }
        //public DateTime? ActualstartdateTime { get; set; }


        private DateTime? _ActualstartdateTime;
        public DateTime? ActualstartdateTime
        {
            get
            {
                return _ActualstartdateTime == null ? new DateTime().Date : _ActualstartdateTime.Value.Date;
            }
            set
            {
                _ActualstartdateTime = value;
            }
        }

        ////public string Actualstartdate
        ////{
        ////    get
        ////    {
        ////        return ActualstartdateTime == null ? "NV" : ActualstartdateTime.Value.ToString("yyyy-MM-dd");
        ////    }
        ////}

        public int? Recyclecount { get; set; }
        // public string Statusreason { get; set; }
        private string _Statusreason;
        public string Statusreason
        {
            get { return string.IsNullOrEmpty(_Statusreason) ? "NULL" : _Statusreason; }
            set { _Statusreason = value; }
        }
        // public string ConfigItemCategory { get; set; }
        private string _ConfigItemCategory;
        public string ConfigItemCategory
        {
            get { return string.IsNullOrEmpty(_ConfigItemCategory) ? "NULL" : _ConfigItemCategory; }
            set { _ConfigItemCategory = value; }
        }
        public int? ResponseTime { get; set; }
        //  public string ResponseTimeMetSLA { get; set; }
        private string _ResponseTimeMetSLA;
        public string ResponseTimeMetSLA
        {
            get { return string.IsNullOrEmpty(_ResponseTimeMetSLA) ? "NULL" : _ResponseTimeMetSLA; }
            set { _ResponseTimeMetSLA = value; }
        }
        // public string ResolutionTimeMetSLA { get; set; }
        private string _ResolutionTimeMetSLA;
        public string ResolutionTimeMetSLA
        {
            get { return string.IsNullOrEmpty(_ResolutionTimeMetSLA) ? "NULL" : _ResolutionTimeMetSLA; }
            set { _ResolutionTimeMetSLA = value; }
        }
        public int? EstimateResponseTime { get; set; }
        // public string ClosureCategory { get; set; }
        private string _ClosureCategory;
        public string ClosureCategory
        {
            get { return string.IsNullOrEmpty(_ClosureCategory) ? "NULL" : _ClosureCategory; }
            set { _ClosureCategory = value; }
        }
        // public string EstimatedResponseTimeMetSLA { get; set; }
        private string _EstimatedResponseTimeMetSLA;
        public string EstimatedResponseTimeMetSLA
        {
            get { return string.IsNullOrEmpty(_EstimatedResponseTimeMetSLA) ? "NULL" : _EstimatedResponseTimeMetSLA; }
            set { _EstimatedResponseTimeMetSLA = value; }
        }
        //   public string EstResptarget { get; set; }
        private string _EstResptarget;
        public string EstResptarget
        {
            get { return string.IsNullOrEmpty(_EstResptarget) ? "NULL" : _EstResptarget; }
            set { _EstResptarget = value; }
        }
        // public string Reslntargettime { get; set; }
        private string _Reslntargettime;
        public string Reslntargettime
        {
            get { return string.IsNullOrEmpty(_Reslntargettime) ? "NULL" : _Reslntargettime; }
            set { _Reslntargettime = value; }
        }
        //public string ResolutionExceptionIndicator { get; set; }
        private string _ResolutionExceptionIndicator;
        public string ResolutionExceptionIndicator
        {
            get { return string.IsNullOrEmpty(_ResolutionExceptionIndicator) ? "NULL" : _ResolutionExceptionIndicator; }
            set { _ResolutionExceptionIndicator = value; }
        }
        //public string ResolutionExceptionReason { get; set; }
        private string _ResolutionExceptionReason;
        public string ResolutionExceptionReason
        {
            get { return string.IsNullOrEmpty(_ResolutionExceptionReason) ? "NULL" : _ResolutionExceptionReason; }
            set { _ResolutionExceptionReason = value; }
        }
        //public string ResponseExceptionIndicator { get; set; }
        private string _ResponseExceptionIndicator;
        public string ResponseExceptionIndicator
        {
            get { return string.IsNullOrEmpty(_ResponseExceptionIndicator) ? "NULL" : _ResponseExceptionIndicator; }
            set { _ResponseExceptionIndicator = value; }
        }
        //public string ResponseExceptionReason { get; set; }
        private string _ResponseExceptionReason;
        public string ResponseExceptionReason
        {
            get { return string.IsNullOrEmpty(_ResponseExceptionReason) ? "NULL" : _ResponseExceptionReason; }
            set { _ResponseExceptionReason = value; }
        }
        //public string Resptargettime { get; set; }
        private string _Resptargettime;
        public string Resptargettime
        {
            get { return string.IsNullOrEmpty(_Resptargettime) ? "NULL" : _Resptargettime; }
            set { _Resptargettime = value; }
        }
        //public string Severity { get; set; }
        private string _Severity;
        public string Severity
        {
            get { return string.IsNullOrEmpty(_Severity) ? "NULL" : _Severity; }
            set { _Severity = value; }
        }
        //public string WorkGroupLevel { get; set; }
        private string _WorkGroupLevel;
        public string WorkGroupLevel
        {
            get { return string.IsNullOrEmpty(_WorkGroupLevel) ? "NULL" : _WorkGroupLevel; }
            set { _WorkGroupLevel = value; }
        }
        //public string AffectedItemName { get; set; }
        private string _AffectedItemName;
        public string AffectedItemName
        {
            get { return string.IsNullOrEmpty(_AffectedItemName) ? "NULL" : _AffectedItemName; }
            set { _AffectedItemName = value; }
        }
        public decimal? SLACostPerTicket { get; set; }
        public int? INResolutiontime { get; set; }
        public decimal? INCostPerTicket { get; set; }
        // public string AccountName { get; set; }
        private string _AccountName;
        public string AccountName
        {
            get { return string.IsNullOrEmpty(_AccountName) ? "NULL" : _AccountName; }
            set { _AccountName = value; }
        }
        //public string ContractName{ get; set; }
        private string _ContractName;
        public string ContractName
        {
            get { return string.IsNullOrEmpty(_ContractName) ? "NULL" : _ContractName; }
            set { _ContractName = value; }
        }

        //newly added Properties by DP
        //public string Active { get; set; }
        private string _Active;
        public string Active
        {
            get { return string.IsNullOrEmpty(_Active) ? "NULL" : _Active; }
            set { _Active = value; }
        }
    //    public DateTime? Activityduedatetime { get; set; }

        private DateTime? _Activityduedatetime;
        public DateTime? Activityduedatetime
        {
            get
            {
                return _Activityduedatetime == null ? new DateTime().Date : _Activityduedatetime.Value.Date;
            }
            set
            {
                _Activityduedatetime = value;
            }
        }

        //public DateTime? ActualenddateTime { get; set; }

        private DateTime? _ActualenddateTime;
        public DateTime? ActualenddateTime
        {
            get
            {
                return _ActualenddateTime == null ? new DateTime().Date : _ActualenddateTime.Value.Date;
            }
            set
            {
                _ActualenddateTime = value;
            }
        }

        // public string Additionalassigneelist { get; set; }
        private string _Additionalassigneelist;
        public string Additionalassigneelist
        {
            get { return string.IsNullOrEmpty(_Additionalassigneelist) ? "NULL" : _Additionalassigneelist; }
            set { _Additionalassigneelist = value; }
        }
        // public string Approval { get; set; }
        private string _Approval;
        public string Approval
        {
            get { return string.IsNullOrEmpty(_Approval) ? "NULL" : _Approval; }
            set { _Approval = value; }
        }
        //public string Approvalhistory { get; set; }
        private string _Approvalhistory;
        public string Approvalhistory
        {
            get { return string.IsNullOrEmpty(_Approvalhistory) ? "NULL" : _Approvalhistory; }
            set { _Approvalhistory = value; }
        }
     //   public DateTime? Approvalsetdatetime { get; set; }

        private DateTime? _Approvalsetdatetime;
        public DateTime? Approvalsetdatetime
        {
            get
            {
                return _Approvalsetdatetime == null ? new DateTime().Date : _Approvalsetdatetime.Value.Date;
            }
            set
            {
                _Approvalsetdatetime = value;
            }
        }

        // public string Businessduration { get; set; }
        private string _Businessduration;
        public string Businessduration
        {
            get { return string.IsNullOrEmpty(_Businessduration) ? "NULL" : _Businessduration; }
            set { _Businessduration = value; }
        }
        //public string Closenotes { get; set; }
        private string _Closenotes;
        public string Closenotes
        {
            get { return string.IsNullOrEmpty(_Closenotes) ? "NULL" : _Closenotes; }
            set { _Closenotes = value; }
        }
        //public string Closedby { get; set; }
        private string _Closedby;
        public string Closedby
        {
            get { return string.IsNullOrEmpty(_Closedby) ? "NULL" : _Closedby; }
            set { _Closedby = value; }
        }
        //public string Commentsworknotes { get; set; }
        private string _Commentsworknotes;
        public string Commentsworknotes
        {
            get { return string.IsNullOrEmpty(_Commentsworknotes) ? "NULL" : _Commentsworknotes; }
            set { _Commentsworknotes = value; }
        }
        //public string Correlationdisplay { get; set; }
        private string _Correlationdisplay;
        public string Correlationdisplay
        {
            get { return string.IsNullOrEmpty(_Correlationdisplay) ? "NULL" : _Correlationdisplay; }
            set { _Correlationdisplay = value; }
        }
        //public string CorrelationID { get; set; }
        private string _CorrelationID;
        public string CorrelationID
        {
            get { return string.IsNullOrEmpty(_CorrelationID) ? "NULL" : _CorrelationID; }
            set { _CorrelationID = value; }
        }
        // public string Deliveryplan { get; set; }
        private string _Deliveryplan;
        public string Deliveryplan
        {
            get { return string.IsNullOrEmpty(_Deliveryplan) ? "NULL" : _Deliveryplan; }
            set { _Deliveryplan = value; }
        }
        //public string Deliverytask { get; set; }
        private string _Deliverytask;
        public string Deliverytask
        {
            get { return string.IsNullOrEmpty(_Deliverytask) ? "NULL" : _Deliverytask; }
            set { _Deliverytask = value; }
        }
        //public string Domain { get; set; }
        private string _Domain;
        public string Domain
        {
            get { return string.IsNullOrEmpty(_Domain) ? "NULL" : _Domain; }
            set { _Domain = value; }
        }
        //public string Domainpath { get; set; }
        private string _Domainpath;
        public string Domainpath
        {
            get { return string.IsNullOrEmpty(_Domainpath) ? "NULL" : _Domainpath; }
            set { _Domainpath = value; }
        }
        //public string Escalation { get; set; }
        private string _Escalation;
        public string Escalation
        {
            get { return string.IsNullOrEmpty(_Escalation) ? "NULL" : _Escalation; }
            set { _Escalation = value; }
        }
      //  public DateTime? Followupdatetime { get; set; }

        private DateTime? _Followupdatetime;
        public DateTime? Followupdatetime
        {
            get
            {
                return _Followupdatetime == null ? new DateTime().Date : _Followupdatetime.Value.Date;
            }
            set
            {
                _Followupdatetime = value;
            }
        }

        //public string Grouplist { get; set; }
        private string _Grouplist;
        public string Grouplist
        {
            get { return string.IsNullOrEmpty(_Grouplist) ? "NULL" : _Grouplist; }
            set { _Grouplist = value; }
        }
        //public string Knowledge { get; set; }
        private string _Knowledge;
        public string Knowledge
        {
            get { return string.IsNullOrEmpty(_Knowledge) ? "NULL" : _Knowledge; }
            set { _Knowledge = value; }
        }
        //public string Location { get; set; }
        private string _Location;
        public string Location
        {
            get { return string.IsNullOrEmpty(_Location) ? "NULL" : _Location; }
            set { _Location = value; }
        }
        //public string MadeSLA { get; set; }
        private string _MadeSLA;
        public string MadeSLA
        {
            get { return string.IsNullOrEmpty(_MadeSLA) ? "NULL" : _MadeSLA; }
            set { _MadeSLA = value; }
        }
      //  public DateTime? Openeddatetime { get; set; }

        private DateTime? _Openeddatetime;
        public DateTime? Openeddatetime
        {
            get
            {
                return _Openeddatetime == null ? new DateTime().Date : _Openeddatetime.Value.Date;
            }
            set
            {
                _Openeddatetime = value;
            }
        }

        //public string Openedby { get; set; }
        private string _Openedby;
        public string Openedby
        {
            get { return string.IsNullOrEmpty(_Openedby) ? "NULL" : _Openedby; }
            set { _Openedby = value; }
        }
        public int? Order { get; set; }
        // public string ParentticketID { get; set; }
        private string _ParentticketID;
        public string ParentticketID
        {
            get { return string.IsNullOrEmpty(_ParentticketID) ? "NULL" : _ParentticketID; }
            set { _ParentticketID = value; }
        }
        public int? Reassignmentcount { get; set; }
       // public DateTime? SLAduedatetime { get; set; }

        private DateTime? _SLAduedatetime;
        public DateTime? SLAduedatetime
        {
            get
            {
                return _SLAduedatetime == null ? new DateTime().Date : _SLAduedatetime.Value.Date;
            }
            set
            {
                _SLAduedatetime = value;
            }
        }

        // public string Tags { get; set; }
        private string _Tags;
        public string Tags
        {
            get { return string.IsNullOrEmpty(_Tags) ? "NULL" : _Tags; }
            set { _Tags = value; }
        }
        // public string Timeworked { get; set; }
        private string _Timeworked;
        public string Timeworked
        {
            get { return string.IsNullOrEmpty(_Timeworked) ? "NULL" : _Timeworked; }
            set { _Timeworked = value; }
        }
        public int? Updatescount { get; set; }
        //public string Uponapprovalstep { get; set; }
        private string _Uponapprovalstep;
        public string Uponapprovalstep
        {
            get { return string.IsNullOrEmpty(_Uponapprovalstep) ? "NULL" : _Uponapprovalstep; }
            set { _Uponapprovalstep = value; }
        }
        //public string Uponrejectstep { get; set; }
        private string _Uponrejectstep;
        public string Uponrejectstep
        {
            get { return string.IsNullOrEmpty(_Uponrejectstep) ? "NULL" : _Uponrejectstep; }
            set { _Uponrejectstep = value; }
        }
        //public string Userinput { get; set; }
        private string _Userinput;
        public string Userinput
        {
            get { return string.IsNullOrEmpty(_Userinput) ? "NULL" : _Userinput; }
            set { _Userinput = value; }
        }
        //public string Watchlist { get; set; }
        private string _Watchlist;
        public string Watchlist
        {
            get { return string.IsNullOrEmpty(_Watchlist) ? "NULL" : _Watchlist; }
            set { _Watchlist = value; }
        }
        //public string Worknotes { get; set; }
        private string _Worknotes;
        public string Worknotes
        {
            get { return string.IsNullOrEmpty(_Worknotes) ? "NULL" : _Worknotes; }
            set { _Worknotes = value; }
        }
        //public string Worknoteslist { get; set; }
        private string _Worknoteslist;
        public string Worknoteslist
        {
            get { return string.IsNullOrEmpty(_Worknoteslist) ? "NULL" : _Worknoteslist; }
            set { _Worknoteslist = value; }
        }
        //public string Businessresolvetime { get; set; }
        private string _Businessresolvetime;
        public string Businessresolvetime
        {
            get { return string.IsNullOrEmpty(_Businessresolvetime) ? "NULL" : _Businessresolvetime; }
            set { _Businessresolvetime = value; }
        }
        //public string Caller { get; set; }
        private string _Caller;
        public string Caller
        {
            get { return string.IsNullOrEmpty(_Caller) ? "NULL" : _Caller; }
            set { _Caller = value; }
        }
        //public string Causedbychange { get; set; }
        private string _Causedbychange;
        public string Causedbychange
        {
            get { return string.IsNullOrEmpty(_Causedbychange) ? "NULL" : _Causedbychange; }
            set { _Causedbychange = value; }
        }
        public int? ChildIncidentscount { get; set; }
        //public string Incidentstate { get; set; }
        private string _Incidentstate;
        public string Incidentstate
        {
            get { return string.IsNullOrEmpty(_Incidentstate) ? "NULL" : _Incidentstate; }
            set { _Incidentstate = value; }
        }
        //public DateTime? Lastreopenedatdatetime { get; set; }

        private DateTime? _Lastreopenedatdatetime;
        public DateTime? Lastreopenedatdatetime
        {
            get
            {
                return _Lastreopenedatdatetime == null ? new DateTime().Date : _Lastreopenedatdatetime.Value.Date;
            }
            set
            {
                _Lastreopenedatdatetime = value;
            }
        }

        //public string Lastreopenedby { get; set; }
        private string _Lastreopenedby;
        public string Lastreopenedby
        {
            get { return string.IsNullOrEmpty(_Lastreopenedby) ? "NULL" : _Lastreopenedby; }
            set { _Lastreopenedby = value; }
        }
        //public string Notify { get; set; }
        private string _Notify;
        public string Notify
        {
            get { return string.IsNullOrEmpty(_Notify) ? "NULL" : _Notify; }
            set { _Notify = value; }
        }
        //public string ParentIncidentID { get; set; }
        private string _ParentIncidentID;
        public string ParentIncidentID
        {
            get { return string.IsNullOrEmpty(_ParentIncidentID) ? "NULL" : _ParentIncidentID; }
            set { _ParentIncidentID = value; }
        }
        //public string TotalResolvetime { get; set; }
        private string _TotalResolvetime;
        public string TotalResolvetime
        {
            get { return string.IsNullOrEmpty(_TotalResolvetime) ? "NULL" : _TotalResolvetime; }
            set { _TotalResolvetime = value; }
        }
        //public string Knownerror { get; set; }
        private string _Knownerror;
        public string Knownerror
        {
            get { return string.IsNullOrEmpty(_Knownerror) ? "NULL" : _Knownerror; }
            set { _Knownerror = value; }
        }
        //public string Majorproblem { get; set; }
        private string _Majorproblem;
        public string Majorproblem
        {
            get { return string.IsNullOrEmpty(_Majorproblem) ? "NULL" : _Majorproblem; }
            set { _Majorproblem = value; }
        }
        //public string Problemstate { get; set; }
        private string _Problemstate;
        public string Problemstate
        {
            get { return string.IsNullOrEmpty(_Problemstate) ? "NULL" : _Problemstate; }
            set { _Problemstate = value; }
        }
        public int? RelatedIncidentscount { get; set; }
        //public string Workaround { get; set; }
        private string _Workaround;
        public string Workaround
        {
            get { return string.IsNullOrEmpty(_Workaround) ? "NULL" : _Workaround; }
            set { _Workaround = value; }
        }
        //public string Backoutplan { get; set; }
        private string _Backoutplan;
        public string Backoutplan
        {
            get { return string.IsNullOrEmpty(_Backoutplan) ? "NULL" : _Backoutplan; }
            set { _Backoutplan = value; }
        }
      //  public DateTime? CABdatetime { get; set; }

        private DateTime? _CABdatetime;
        public DateTime? CABdatetime
        {
            get
            {
                return _CABdatetime == null ? new DateTime().Date : _CABdatetime.Value.Date;
            }
            set
            {
                _CABdatetime = value;
            }
        }

        //public string CABdelegate { get; set; }
        private string _CABdelegate;
        public string CABdelegate
        {
            get { return string.IsNullOrEmpty(_CABdelegate) ? "NULL" : _CABdelegate; }
            set { _CABdelegate = value; }
        }
        //public string CABrecommendation { get; set; }
        private string _CABrecommendation;
        public string CABrecommendation
        {
            get { return string.IsNullOrEmpty(_CABrecommendation) ? "NULL" : _CABrecommendation; }
            set { _CABrecommendation = value; }
        }
        //public string CABrequired { get; set; }
        private string _CABrequired;
        public string CABrequired
        {
            get { return string.IsNullOrEmpty(_CABrequired) ? "NULL" : _CABrequired; }
            set { _CABrequired = value; }
        }
        //public string Changeplan { get; set; }
        private string _Changeplan;
        public string Changeplan
        {
            get { return string.IsNullOrEmpty(_Changeplan) ? "NULL" : _Changeplan; }
            set { _Changeplan = value; }
        }
        //public string Closecode { get; set; }
        private string _Closecode;
        public string Closecode
        {
            get { return string.IsNullOrEmpty(_Closecode) ? "NULL" : _Closecode; }
            set { _Closecode = value; }
        }
        public DateTime? Conflictlastrundatetime { get; set; }
        //public string Conflictstatus { get; set; }
        private string _Conflictstatus;
        public string Conflictstatus
        {
            get { return string.IsNullOrEmpty(_Conflictstatus) ? "NULL" : _Conflictstatus; }
            set { _Conflictstatus = value; }
        }
        //public string Implementationplan { get; set; }
        private string _Implementationplan;
        public string Implementationplan
        {
            get { return string.IsNullOrEmpty(_Implementationplan) ? "NULL" : _Implementationplan; }
            set { _Implementationplan = value; }
        }
        //public string Onhold { get; set; }
        private string _Onhold;
        public string Onhold
        {
            get { return string.IsNullOrEmpty(_Onhold) ? "NULL" : _Onhold; }
            set { _Onhold = value; }
        }
        //public string OnHoldChangeTasks { get; set; }
        private string _OnHoldChangeTasks;
        public string OnHoldChangeTasks
        {
            get { return string.IsNullOrEmpty(_OnHoldChangeTasks) ? "NULL" : _OnHoldChangeTasks; }
            set { _OnHoldChangeTasks = value; }
        }
        //public string Outsidemaintenanceschedule { get; set; }
        private string _Outsidemaintenanceschedule;
        public string Outsidemaintenanceschedule
        {
            get { return string.IsNullOrEmpty(_Outsidemaintenanceschedule) ? "NULL" : _Outsidemaintenanceschedule; }
            set { _Outsidemaintenanceschedule = value; }
        }
        //public string Phase { get; set; }
        private string _Phase;
        public string Phase
        {
            get { return string.IsNullOrEmpty(_Phase) ? "NULL" : _Phase; }
            set { _Phase = value; }
        }
        //public string Phasestate { get; set; }
        private string _Phasestate;
        public string Phasestate
        {
            get { return string.IsNullOrEmpty(_Phasestate) ? "NULL" : _Phasestate; }
            set { _Phasestate = value; }
        }
        //public string Productionsystem { get; set; }
        private string _Productionsystem;
        public string Productionsystem
        {
            get { return string.IsNullOrEmpty(_Productionsystem) ? "NULL" : _Productionsystem; }
            set { _Productionsystem = value; }
        }
        //public string Reason { get; set; }
        private string _Reason;
        public string Reason
        {
            get { return string.IsNullOrEmpty(_Reason) ? "NULL" : _Reason; }
            set { _Reason = value; }
        }
        //public string Requestedby { get; set; }
        private string _Requestedby;
        public string Requestedby
        {
            get { return string.IsNullOrEmpty(_Requestedby) ? "NULL" : _Requestedby; }
            set { _Requestedby = value; }
        }
        //public DateTime? Requestedbydatetime { get; set; }

        private DateTime? _Requestedbydatetime;
        public DateTime? Requestedbydatetime
        {
            get
            {
                return _Requestedbydatetime == null ? new DateTime().Date : _Requestedbydatetime.Value.Date;
            }
            set
            {
                _Requestedbydatetime = value;
            }
        }

        //public string Reviewcomments { get; set; }
        private string _Reviewcomments;
        public string Reviewcomments
        {
            get { return string.IsNullOrEmpty(_Reviewcomments) ? "NULL" : _Reviewcomments; }
            set { _Reviewcomments = value; }
        }
      //  public DateTime? Reviewdatetime { get; set; }

        private DateTime? _Reviewdatetime;
        public DateTime? Reviewdatetime
        {
            get
            {
                return _Reviewdatetime == null ? new DateTime().Date : _Reviewdatetime.Value.Date;
            }
            set
            {
                _Reviewdatetime = value;
            }
        }

        public int? Reviewstatus { get; set; }
        //public string Risk { get; set; }
        private string _Risk;
        public string Risk
        {
            get { return string.IsNullOrEmpty(_Risk) ? "NULL" : _Risk; }
            set { _Risk = value; }
        }
        //public string Changerequestscope { get; set; }
        private string _Changerequestscope;
        public string Changerequestscope
        {
            get { return string.IsNullOrEmpty(_Changerequestscope) ? "NULL" : _Changerequestscope; }
            set { _Changerequestscope = value; }
        }
        //public string Changerequesttype { get; set; }
        private string _Changerequesttype;
        public string Changerequesttype
        {
            get { return string.IsNullOrEmpty(_Changerequesttype) ? "NULL" : _Changerequesttype; }
            set { _Changerequesttype = value; }
        }
        private string _Tower;
        public string Tower
        {
            get { return string.IsNullOrEmpty(_Tower) ? "NULL" : _Tower; }
            set { _Tower = value; }
        }
    }
}