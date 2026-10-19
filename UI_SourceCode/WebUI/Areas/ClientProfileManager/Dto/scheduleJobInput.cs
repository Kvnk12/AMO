using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.Areas.ClientProfileManager.ViewModel;
using System.Web.Mvc;
using NTTData.AMOPortal.Data;

namespace WebUI.Areas.ClientProfileManager.Dto
{
    public class scheduleJobInput
    {
        public int ClientId { get; set; }
        public int ApplicationId { get; set; }
        public string ClientName { get; set; }
        public string ScheduleType { get; set; }
        public string WeekDay { get; set; }
        public string MonthDay { get; set; }
        public DateTime time { get; set; }
        public string _templateIdentifier { get; set; }
        public List<SelectListItem> TemplateIdentifiers
        {

            get
            {
                Db db = new Db();
                var TempIdentifier = (from strapp in db.StoreApplication
                               join srctmp in db.MappingTemplate on strapp.MappingId equals srctmp.Id
                               // join disp in db.APRDisposition on dispandln.DispId equals disp.Id
                               where strapp.ApplicationId == ApplicationId && srctmp.ClientId== ClientId && srctmp.AcknowledgeBy!= null
                                      select new SelectListItem
                                      {
                                          Value = srctmp.GlobalMappingIdentifier,
                                          Text = srctmp.GlobalMappingIdentifier

                                      }
                               ).ToList();
                //var Items = TempIdentifier.Select(Field => new SelectListItem
                //{
                //    Value = Field.DataTemplateId.ToString(),
                //    Text = Field.DataTemplateId.ToString()
                //});

                return TempIdentifier.ToList();

            }
           
           

        }


        // public ScheduleJobsViewModel SQLJob { get; set; }
        // public IEnumerable<ScheduleJobsViewModel> SQLjobList {get; set;}
    }
}
