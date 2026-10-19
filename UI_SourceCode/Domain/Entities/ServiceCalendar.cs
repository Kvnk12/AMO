using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTTData.AMOPortal.Domain.Entities
{
      [Table("ServiceCalendar")]
    public class ServiceCalendar : DelEntity
    {
       // public int Servicecalendaridentifier { get; set; }
        public int Clientid { get; set; }
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
        //public string StartTime { get; set; }
        //public string EndTime { get; set; }
        public TimeSpan? Servicestarttime { get; set; }
        public TimeSpan? Serviceendtime { get; set; }

        public int AccountId { get; set; }
        public int ContractId { get; set; }

        public int? workinghours { get; set; }
        public int? Nonworkinghours { get; set; }

    }
}
