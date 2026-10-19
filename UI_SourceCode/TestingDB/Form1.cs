using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReportJobLibrary;
using log4net;


namespace TestingDB
{
    public partial class Form1 : Form
    {
        public enum ReportScheduleStatus
        {
            SCHEDULED = 1,
            STARTED = 2,
            INPROGRESS = 3,
            COMPLETED = 4,
            FAILED = 5,
            
        }
        public log4net.ILog log;
        public Form1()
        {
            log4net.Config.BasicConfigurator.Configure();
            log = log4net.LogManager.GetLogger(typeof(Program));
            log.Info("Form1 constructor");
            jobManager = new ReportJobManager();

            InitializeComponent();
        }
        ReportJobManager jobManager;
        private void button1_Click(object sender, EventArgs e)
        {
            //WriteToFile("Button Click");
            Task t1 = new Task(CreateJob);
            log.Info("job is getting called.");
            t1.Start();
            log.Info("Initiated");
           
            //CreateCurrentReportSchedule();
            // ProcessReportSchedule();
        }
        private void CreateJob()
        {
            jobManager.CreateCurrentReportSchedule();

        }

        private void button2_Click(object sender, EventArgs e)
        {

            Task t1 = new Task(ProcessJob);

            t1.Start();
            
        }
        private void ProcessJob()
        {
            jobManager.ProcessReportSchedule();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
