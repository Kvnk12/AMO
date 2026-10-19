using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Threading;
using System.Data.SqlClient;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using ReportJobLibrary;

namespace AMOReportsJOBProcessorSVC
{
    public partial class AMOReportsJOBProcessor : ServiceBase
    {
      //  int SchedulingTime = Convert.ToInt32(ConfigurationSettings.AppSettings["ServiceTime"]);
        public Thread ReportExport = null;
        public AMOReportsJOBProcessor()
        {
            
            InitializeComponent();
        }
        ReportJobManager jobManager ;
        public log4net.ILog log;

        protected override void OnStart(string[] args)
        {
            log4net.Config.BasicConfigurator.Configure();
            log = log4net.LogManager.GetLogger(typeof(Program));
            log.Debug("Starting JobProcessor");
            try
            {
                ThreadStart start = new ThreadStart(ReportWork);
                ReportExport = new Thread(start);
                ReportExport.Start();
            }
            catch (Exception ex)
            {
                log.Error("Error in starting JobProcessor", ex);

            }

        }
        public void ReportWork()
        {
            jobManager = new ReportJobManager();

            while (true)
            {
                Task t1 = new Task(jobManager.ProcessReportSchedule);
                t1.Start();
                //ProcessReportSchedule();
                Thread.Sleep(int.Parse(System.Configuration.ConfigurationManager.AppSettings["SleepTime"])  );

            }
        }

        protected override void OnStop()
        {
           //  jobManager.WriteToFile("Service Stopped at " + System.DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
        }
  

    }
}
