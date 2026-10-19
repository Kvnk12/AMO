using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.ClientProfileManager.ViewModel
{
    public class JobDetailViewModel
    {
        public int Id { get; set; }

        public int? MappingId { get; set; }

        //public 

        public int? ClientId { get; set; }

        public string ClientName { get; set; }

        public int? ApplicationId { get; set; }

        public string ApplicationName { get; set; }

        public string FileName { get; set; }

        public string UserId { get; set; }

        public DateTime JobStartTime { get; set; }

        public DateTime? JobEndTime { get; set; }

        public string RunStatus { get; set; }

        public string Error { get; set; }

        public int TotalRecordRecieved { get; set; }

        public int AddRowCount { get; set; }

        public int UpdateRowCount { get; set; }

        public int DuplicateRowCount { get; set; }

        public int RejectedRecordsCount { get; set; }

        public int IgnoredRecords { get; set; }
        public bool? IsChecked { get; set; }
    }
}