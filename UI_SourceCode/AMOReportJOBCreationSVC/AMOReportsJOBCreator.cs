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
using ReportJobLibrary;

namespace AMOReportJOBCreationSVC
{
    public partial class AMOReportsJOBCreator : ServiceBase
    {
        public Thread ReportExport = null;
        public AMOReportsJOBCreator()
        {
            
            InitializeComponent();
        }
        ReportJobManager jobManager;
        public log4net.ILog log;

        protected override void OnStart(string[] args)
        {
            //      WriteToFile("Service Started at "+  System.DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
            log4net.Config.BasicConfigurator.Configure();
            log = log4net.LogManager.GetLogger(typeof(Program));
            log.Debug("Starting JobCreator");
            try
            {
                ThreadStart start = new ThreadStart(ReportWork);
                ReportExport = new Thread(start);
                ReportExport.Start();
            }
            catch(Exception ex)
            {
                log.Error("Error in starting JobCreator", ex);


            }

        }
        public void ReportWork()
        {
            jobManager = new ReportJobManager();

            while (true)
            {
                Task t1 = new Task(jobManager.CreateCurrentReportSchedule);
                t1.Start();
                //CreateCurrentReportSchedule();
                Thread.Sleep(int.Parse(System.Configuration.ConfigurationManager.AppSettings["SleepTime"]));

            }
        }

        protected override void OnStop()
        {
    //        WriteToFile("Service Stopped at " + System.DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
        }
 

    
    }
}
