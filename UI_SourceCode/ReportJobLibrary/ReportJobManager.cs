using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Threading;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using log4net;

namespace ReportJobLibrary
{
    public enum ReportScheduleStatus
        {
            SCHEDULED = 1,
            STARTED = 2,
            INPROGRESS = 3,
            COMPLETED = 4,
            FAILED = 5,
        }
   
    public class ReportJobManager
    {
        private log4net.ILog log;
        static List<Dictionary<string, string>> reports;
        static int dateCheck = 0;
        static string reportServerUrl;
        static string NonTelerikReporturl;
        private String EmailFrom;
        private string reportServerApiUrl;
        private string username;
        private string password;
        string ConString;
        bool bCreatingJobs;
        String SupportMail2Bcc;
        String Bcc;
       
        //Task<List<Dictionary<string, string>>> telerikReportList;
        static HttpClient client;
        public  ReportJobManager()
        {
            ReadConfigurations();
        }
        public void ReadConfigurations()
        {
            log4net.Config.BasicConfigurator.Configure();
            log = log4net.LogManager.GetLogger("ReportJobManager");
            try
            {
               
                reportServerUrl = System.Configuration.ConfigurationManager.AppSettings["reportServerUrl"];
                NonTelerikReporturl = System.Configuration.ConfigurationManager.AppSettings["NonTelerikReporturl"];
                EmailFrom = System.Configuration.ConfigurationManager.AppSettings["EmailFrom"];
                SupportMail2Bcc = System.Configuration.ConfigurationManager.AppSettings["SupportMail2"];
                Bcc = System.Configuration.ConfigurationManager.AppSettings["EmailBcc"];

                // string reportServerUrl = System.Configuration.ConfigurationManager.AppSettings["reportServerUrl"];
                reportServerApiUrl = System.Configuration.ConfigurationManager.AppSettings["reportServerApiUrl"];
                username = System.Configuration.ConfigurationManager.AppSettings["username"];
                password = System.Configuration.ConfigurationManager.AppSettings["password"];
                ConString = ConfigurationManager.ConnectionStrings["Db"].ToString();
                client = new HttpClient();

                client.BaseAddress = new Uri(reportServerApiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                bCreatingJobs = false;

            }
            catch (Exception ex)
            {
                log.Error("Error initiating JobManager.",ex);
                bCreatingJobs = false;
            }

        }
        public void CreateCurrentReportSchedule()
        {
            log.Info("CreateCurrentReportSchedule : In.. Before Try");
            try
            {

                if (bCreatingJobs)
                    return;
                log.Info("CreateCurrentReportSchedule : In.." );

                SqlConnection conn = new SqlConnection(ConString);
                using (SqlCommand cmd = new SqlCommand("select * from RPTSubscription where status = 'Approved'  and CAST( GETDATE() AS date)   >=  CAST( StartDate AS date) and CAST( GETDATE() AS date)   <=  CAST( EndDate AS date) and  CONVERT(TIME, GETDATE())   >= ScheduleTime and id not in (select   RPTSubscriptionId  from rptjobdetail where ScheduleDateTime > getdate())", conn))
                {
                    conn.Open();

                    string ScheduleDateTime, ReportName, TelerikReportName, Status, StatusDetail;
                    int RPTSubscriptionId = 0;
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            conn.Close();
                            
                            foreach (DataRow row in dt.Rows)
                            {
                                bCreatingJobs = true;
                                RPTSubscriptionId = Int32.Parse(row["Id"].ToString());
                                ReportName = row["ReportName"].ToString();
                                TelerikReportName = row["TelerikReportName"].ToString();

                                ScheduleDateTime = System.DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
                                Status = StatusDetail = ReportScheduleStatus.SCHEDULED.ToString();
                                string ScheduleType = row["ScheduleType"].ToString();
                                int ScheduledWeekDay = 0; int ScheduledMonthDay = 0;
                                if (row["WeekDay"].ToString().Length > 0)
                                    ScheduledWeekDay = Int32.Parse(row["WeekDay"].ToString());
                                if (row["MonthDay"].ToString().Length > 0)
                                    ScheduledMonthDay = Int32.Parse(row["MonthDay"].ToString());
                                int WeekDay = (int)DateTime.Now.DayOfWeek + 1;
                                int MonthDay = (int)DateTime.Now.Date.Day;
                                
                                bool bReportSchedulednow = false;
                                switch (ScheduleType)
                                {

                                    case "Daily":
                                        bReportSchedulednow = true;
                                        break;

                                    case "Weekly":
                                        if (ScheduledWeekDay == WeekDay)
                                            bReportSchedulednow = true;
                                        break;

                                    case "Monthly":
                                        if (ScheduledMonthDay == MonthDay)
                                            bReportSchedulednow = true;
                                        else
                                        {
                                            DateTime today = DateTime.Today;

                                            DateTime tempDate = today.AddMonths(1);
                                            DateTime tempDate2 = new DateTime(tempDate.Year, tempDate.Month, 1);
                                            DateTime lastDayOfMonth = tempDate2.AddDays(-1);
                                            if (lastDayOfMonth.Day == MonthDay)
                                            {
                                                if (ScheduledMonthDay > MonthDay)
                                                    bReportSchedulednow = true;
                                            }


                                        }
                                        break;

                                    default:
                                        break;
                                }
                                if (bReportSchedulednow)
                                {
                                    int rowsAffected = 0;
                                    string RJcommandqry = "select Count( *) from RPTjobdetail where RPTSubscriptionId = " + RPTSubscriptionId.ToString() + " and  CAST(FORMAT (getdate(), 'yyyy-MM-dd') AS date)  = CAST(ScheduleDateTime AS date)";
                                    conn.Open();
                                    using (SqlCommand RJcommand = new SqlCommand(RJcommandqry.ToString(), conn))
                                    {
                                        int reccnt = (Int32)RJcommand.ExecuteScalar();
                                        if (reccnt <= 0)
                                        {
                                            //WriteToJobCreatorSCSLogFile("CreateCurrentReportSchedule : " + RPTSubscriptionId.ToString());
                                            log.Info("CreateCurrentReportSchedule : " + RPTSubscriptionId.ToString());

                                            using (SqlCommand cmd1 = new SqlCommand("insert into RPTjobdetail    (RPTSubscriptionId,ScheduleDateTime,  Status,    StatusDetail) VALUES  (@RPTSubscriptionId, @ScheduleDateTime, @Status,   @StatusDetail)", conn))
                                            {
                                                cmd1.CommandType = CommandType.Text;
                                                cmd1.Parameters.AddWithValue("@RPTSubscriptionId", RPTSubscriptionId);
                                                cmd1.Parameters.AddWithValue("@ScheduleDateTime", ScheduleDateTime);
                                                cmd1.Parameters.AddWithValue("@Status", Status);
                                                cmd1.Parameters.AddWithValue("@StatusDetail", StatusDetail);
                                                rowsAffected = cmd1.ExecuteNonQuery();
                                            }

                                            //WriteToJobCreatorSCSLogFile("Records inserted in to RPTjobdetail for RPTSubscriptionId  " + RPTSubscriptionId.ToString() + " is  " + rowsAffected.ToString());
                                            log.Info("Records inserted in to RPTjobdetail for RPTSubscriptionId  " + RPTSubscriptionId.ToString() + " is  " + rowsAffected.ToString());

                                        }

                                    }
                                    conn.Close();



                                }
                            }
                            bCreatingJobs = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // WriteToJobCreatorSCSLogFile("Error while Creating the report schedules in AMOReportJOBCreationSVC Service : " + ex.Message + " " + ex.StackTrace);
                log.Error("error", ex);
                bCreatingJobs = false;
            }
        }




        public void WriteToJobCreatorSCSLogFile(string text)
        {
            try
            {
                
                string date = DateTime.Now.ToShortDateString();
                string filename = "AMOReportsJOBCreatorLog_" + System.DateTime.Now.ToString("yyyyMMdd") + ".txt";
                string path = System.Configuration.ConfigurationManager.AppSettings["LogfilePath"] + filename;
                text = text + "  " + System.DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(string.Format(text, DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt")));
                    writer.Close();
                }
                //log.Info(string.Format(text, DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt")));

            }
            catch (Exception ex)
            {
               // log.Info(string.Format(text + ex.Message, DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt")));

            }
        }
        public void WriteToJobProcessorSCSLogFile(string text)
        {
            try
            {

                string date = DateTime.Now.ToShortDateString();
                string filename = "AMOReportsJOBProcessorLog_" + System.DateTime.Now.ToString("yyyyMMdd") + ".txt";
                string path = System.Configuration.ConfigurationManager.AppSettings["LogfilePath"] + filename;
                text = text + "  " + System.DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(string.Format(text, DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt")));
                    writer.Close();
                }
                //log.Info(string.Format(text, DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt")));

            }
            catch (Exception ex)
            {
                // log.Info(string.Format(text + ex.Message, DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt")));

            }
        }
        public void ProcessReportSchedule()
        {


            //log.Info("Started 'ProcessReportSchedule' ");
            log.Info("ProcessReportSchedule : In..");
            SqlConnection conn = new SqlConnection(ConString);
            int FailureCount = 0; int  RPTJobId = 0;
           
            try
            {
                // to check A3MM schedules every 4 hrs
                int hr = DateTime.Now.Hour;
                int min = DateTime.Now.Minute;
//                SurveyBuilderScheduleCheck();

                if (min % 10 == 0 )
                {
                    bCreatingJobs = false;
                    CreateCurrentReportSchedule();
                    return;

                }
                if ( hr % 6 == 0 && min < 10)
                    {
                        IdeaManagerEmailScheduleCheck();
                        SurveyBuilderScheduleCheck();

                        A3MMA3MM_ActionItemTrackerScheduleCheck();
                        A3MMA3MM_AssessmentScheduleCheck();
                        return;
                }

                //if (log == null)
                //{

                //    log4net.Config.BasicConfigurator.Configure();

                //    log = log4net.LogManager.GetLogger("ReportJobManager");
                //}
                if (ConString == null)
                {
                    log.Info("CreateCurrentReportSchedule : To read ReadConfigurations as Conn String  is null ");
                    ReadConfigurations();
                    log.Info("ConString   " + ConString);

                }
                log.Info("ProcessReportSchedule : In..");

                using (SqlCommand cmd = new SqlCommand("select top 1 ReportFor, ReportName,TelerikReportName,ReportParameters,Recipients,	EmailSubject,ReportFormat,FailureCount, RJ.* from RPTjobdetail RJ   left join  RPTSubscription RS on RJ.RPTSubscriptionId = RS.id where RJ.Status in ( 'Scheduled' ,'Failed' ) and  ISNULL(Failurecount, 0 ) < 3  and    GETDATE()    >=   ScheduleDateTime  order by Rj.id", conn))
                {
                    conn.Open();

                    string ScheduleDateTime, ReportFor, ReportName, TelerikReportName, Status, StatusDetail, ReportParameters, Recipients, EmailSubject, ReportFormat;
                    int RPTSubscriptionId = 0; 
                    string ReportStartDate, ReportEndDate;
                    ReportStartDate = ReportEndDate = System.DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
                    cmd.CommandType = CommandType.Text;
                    DataTable dt;// = new DataTable();

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (dt = new DataTable())
                        {
                            sda.Fill(dt);


                        }
                    }
                    conn.Close();
                    int rowsAffected=0;
                    if (dt.Rows.Count >0)
                    {
                                RPTJobId = Int32.Parse(dt.Rows[0]["Id"].ToString());
                                log.Debug("Started Processing the Job id : "+ RPTJobId);
                                ScheduleDateTime = System.DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
                                UpadateJobStatus(RPTJobId.ToString(), ReportScheduleStatus.STARTED.ToString(), ReportScheduleStatus.STARTED.ToString(), null, ScheduleDateTime);
                                DataRow row = dt.Rows[0];
                                RPTSubscriptionId = Int32.Parse(row["RPTSubscriptionId"].ToString());
                                ReportName = row["ReportName"].ToString();
                                TelerikReportName = row["TelerikReportName"].ToString();
                                ReportParameters = row["ReportParameters"].ToString();
                                Recipients = row["Recipients"].ToString();
                                EmailSubject = row["EmailSubject"].ToString();
                                ReportFormat = row["ReportFormat"].ToString();
                                ReportFor = row["ReportFor"].ToString();
                                switch (ReportFor)
                                {

                                    case "Current Week":
                                        ReportStartDate = System.DateTime.Now.AddDays(-7).ToString("MM/dd/yyyy").Replace('-', '/');

                                        break;

                                    case "Quarter":
                                        ReportStartDate = System.DateTime.Now.AddMonths(-3).ToString("MM/dd/yyyy").Replace('-', '/');

                                        break;
                                    case "Yearly":
                                        ReportStartDate = System.DateTime.Now.AddYears(-1).ToString("MM/dd/yyyy").Replace('-', '/');

                                        break; 
                                    case "Half Yearly":
                                ReportStartDate = System.DateTime.Now.AddMonths(-6).ToString("MM/dd/yyyy").Replace('-', '/');

                                break;
                                    case "Monthly":
                                        ReportStartDate = System.DateTime.Now.AddMonths(-1).ToString("MM/dd/yyyy").Replace('-', '/');

                                        break;


                                    default:
                                        ReportStartDate = System.DateTime.Now.AddMonths(-1).ToString("MM/dd/yyyy").Replace('-', '/');
                                        break;
                                }

                                if (ReportParameters.IndexOf("StartDate") > 0)
                                {
                                    string p1 = ReportParameters.Substring(0, ReportParameters.IndexOf("StartDate") + 11);
                                    string p9 = ReportParameters.Substring(p1.Length, ReportParameters.Length - p1.Length);
                                    string DelStDate = ReportParameters.Substring(p1.Length, ReportParameters.IndexOf(',', p1.Length, ReportParameters.Length - p1.Length) - p1.Length);
                                    p1 = p1 + ReportStartDate + "\"" + p9.Substring(DelStDate.Length, p9.Length - DelStDate.Length);

                                    if (ReportParameters.IndexOf("EndDate") > 0)
                                    {
                                        string pe1 = p1.Substring(0, p1.IndexOf("EndDate") + 9);
                                        string pe9 = p1.Substring(pe1.Length, p1.Length - pe1.Length);
                                        string DelEndDate = "";
                                        if (p1.IndexOf(',', pe1.Length, p1.Length - pe1.Length) > 0)
                                            DelEndDate = p1.Substring(pe1.Length, p1.IndexOf(',', pe1.Length, p1.Length - pe1.Length) - pe1.Length);
                                        else
                                            DelEndDate = p1.Substring(pe1.Length, p1.Length - pe1.Length - 1);

                                        p1 = pe1 + System.DateTime.Now.ToString("MM/dd/yyyy").Replace('-', '/') + "\"" + pe9.Substring(DelEndDate.Length, pe9.Length - DelEndDate.Length);

                                        EmailSubject = EmailSubject + "   Start To End Date: " + ReportStartDate + " To " + System.DateTime.Now.ToString("MM/dd/yyyy").Replace('-', '/');
                                    }
                                    ReportParameters = p1;

                                }

                                if (row["FailureCount"].ToString().Length > 0)
                                    FailureCount = Int32.Parse(row["FailureCount"].ToString());
                                ScheduleDateTime = System.DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
                                ReportParameters = ReportParameters.Substring(0, ReportParameters.Length - 1) + ", DateTimeNow : \"" + ScheduleDateTime.ToString().Replace('-', '/') + "\"}";
                                Status = StatusDetail = ReportScheduleStatus.SCHEDULED.ToString();
                                try
                                {
                                    if (TelerikReportName is null || TelerikReportName.Length<1)
                                    {
                                        //process Analysis report
                                        string body = string.Empty;
                                        log.Info("Non Telerik report subscription processing : ReportName - " + ReportName.ToString());

                                        string NonTelerikEmailTemplatePathString = ConfigurationManager.AppSettings["NonTelerikEmailTemplatePath"].ToString();
                                        using (StreamReader reader = new StreamReader(NonTelerikEmailTemplatePathString))//"../Templates/ReportSchedulled_EmailBody.html"))
                                        {
                                            body = reader.ReadToEnd();
                                        }
                                
                                        //body = body.Replace("{clientname}", clientname);
                                        string Reporturl = NonTelerikReporturl.ToString() + "Rpt"+ ReportName.ToString() + "Subscription.aspx"; 
                                        string ReportUID = GetEncodedString(Reporturl);
                                
                                
                                        Uri baseUri = new Uri(Reporturl.ToString());
                                        Uri FinalUri = new Uri(baseUri, "?id=" + ReportUID);
                                        string stFinalUri = FinalUri.ToString();
                                        body = body.Replace("{reportName}", ReportName);
                                        body = body.Replace("{reportLink}", FinalUri.ToString());

                                        log.Info("Final Url - " + FinalUri.ToString());
                                        SendEMail(Recipients, EmailSubject, body, "");

                                        string errmsg = "Completed and Successfully Sent mail ";
                                        conn.Open();
                                        using (SqlCommand cmd1 = new SqlCommand("update   RPTjobdetail set status = 'COMPLETED', StatusDetail = @StatusDetail, ReportUID = @ReportUID, RPTJobUrl= @RPTJobUrl , RPTStartDate = @RPTStartDate , RPTEndDate =@RPTEndDate where id = @RPTJobId", conn))
                                        {
                                            cmd1.CommandType = CommandType.Text;
                                            cmd1.Parameters.AddWithValue("@RPTJobId", RPTJobId);
                                            cmd1.Parameters.AddWithValue("@StatusDetail", errmsg);
                                            cmd1.Parameters.AddWithValue("@ReportUID", ReportUID);
                                            cmd1.Parameters.AddWithValue("@RPTStartDate", ReportStartDate);
                                            cmd1.Parameters.AddWithValue("@RPTEndDate", ReportEndDate);
                                            cmd1.Parameters.AddWithValue("@RPTJobUrl", stFinalUri);
                                            rowsAffected = cmd1.ExecuteNonQuery();
                                        }
                                        conn.Close();

                                    }
                                    else
                                    {
                                        CreateReportDocument(TelerikReportName, ReportParameters, ReportFormat, RPTJobId.ToString(), ReportFormat, FailureCount, Recipients, EmailSubject);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    log.Info("Error while creating report schedule for the Job id :" + ex.Message + " CallStack " + ex.StackTrace);

                                    log.Warn("Error while creating report schedule for the Job id : " + RPTJobId,ex);
                                    FailureCount++; int rowsAffected1 = 0;
                                    conn.Open();
                                    using (SqlCommand cmd1 = new SqlCommand("update   RPTjobdetail set status = 'Failed',StatusDetail = @StatusDetail ,JobStartDateTime =@ScheduleDateTime ,JobEndDateTime = @JobEndDateTime  ,FailureCount = @FailureCount where id = @RPTJobId", conn))
                                    {
                                        string errmsg = "Error while starting AMOReportJOBProcessorSVC Service in ProcessReportSchedule: " + ex.Message + " CallStack " + ex.StackTrace;
                                        ScheduleDateTime = System.DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
                                        cmd1.CommandType = CommandType.Text;
                                        cmd1.Parameters.AddWithValue("@RPTJobId", RPTJobId);
                                        cmd1.Parameters.AddWithValue("@StatusDetail", errmsg.ToString());
                                        cmd1.Parameters.AddWithValue("@FailureCount", FailureCount);
                                        cmd1.Parameters.AddWithValue("@ScheduleDateTime", ScheduleDateTime);
                                        string JobEndDateTime = System.DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
                                        cmd1.Parameters.AddWithValue("@JobEndDateTime", JobEndDateTime);
                                        rowsAffected1 = cmd1.ExecuteNonQuery();
                                    }
                                    conn.Close();

                        }
                    }
                    }
            }
            catch (Exception ex)
            {
                log.Error("Error while starting AMOReportJOBProcessorSVC Service in ProcessReportSchedule: ",ex);
                    FailureCount++; int rowsAffected1 = 0;
                    conn.Open();
                    using (SqlCommand cmd1 = new SqlCommand("update   RPTjobdetail set status = 'Failed',StatusDetail = @StatusDetail ,JobStartDateTime =@ScheduleDateTime ,JobEndDateTime = @JobEndDateTime  ,FailureCount = @FailureCount where id = @RPTJobId", conn))
                    {
                        string errmsg = "Error while starting AMOReportJOBProcessorSVC Service in ProcessReportSchedule: "  +ex.Message + " CallStack " + ex.StackTrace;
                        string ScheduleDateTime = System.DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Parameters.AddWithValue("@RPTJobId", RPTJobId);
                        cmd1.Parameters.AddWithValue("@StatusDetail", errmsg.ToString());
                        cmd1.Parameters.AddWithValue("@FailureCount", FailureCount);
                        cmd1.Parameters.AddWithValue("@ScheduleDateTime", ScheduleDateTime);
                        string JobEndDateTime = System.DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
                        cmd1.Parameters.AddWithValue("@JobEndDateTime", JobEndDateTime);
                        rowsAffected1 = cmd1.ExecuteNonQuery();
                    }
                    conn.Close();

        }
    }
        //public Uri GetURI(string Reporturl)
        //{
        //    string encoded = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        //    encoded = encoded
        //    .Replace("/", "_")
        //    .Replace("+", "-");
        //    Uri baseUri = new Uri(Reporturl.ToString());
        //    Uri myUri = new Uri(baseUri, "?id=" + encoded.Substring(0, 22));
        //    return myUri;
        //}
        public string GetEncodedString(string Reporturl)
        {
            string encoded = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            encoded = encoded
            .Replace("/", "_")
            .Replace("+", "-");
        //    Uri baseUri = new Uri(Reporturl.ToString());
        //    Uri myUri = new Uri(baseUri, "?id=" + encoded.Substring(0, 22));
            return encoded;
        }

        private async void CreateReportDocument(string reportName, object parameters, string format, string RPTJobId, string ext, int FailureCount, string Recipients, string EmailSubject)
        {
            string ScheduleDateTime = System.DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
            SqlConnection conn = new SqlConnection(ConString);
            int rowsAffected = 0;
            string message = "";

            var downloadPath = ""; 
            log.Debug("In the CreateReportDocument for the Job id : " + RPTJobId);
                try
                {

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(reportServerApiUrl);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        log.Debug("In the CreateReportDocument - GetAuthenticationToken for the Job id : " + RPTJobId);
                        var authToken = GetAuthenticationToken(username, password);
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
                        message += " Connected to Report Server";
                    
                        if (dateCheck != System.DateTime.Now.Day || reports == null)
                        {
                            dateCheck = System.DateTime.Now.Day;
                            var reports1 = await GetAllReports( reportServerApiUrl);
                            reports = reports1.FindAll(s => s != null);

                            message += " , GetAllReports";
                        // to check A3MM schedules
                        //A3MMA3MM_ActionItemTrackerScheduleCheck();
                        //A3MMA3MM_AssessmentScheduleCheck();


                    }
                    // Render the desired report.
                    var report = reports.Find(r => r["Name"].Equals(reportName))["Id"];
                        message += " , Found the Report name";
                        var documentUrl = await CreateDocument(client, reportServerApiUrl, format, report, parameters);
                        message += " , CreateDocument";

                        // Download the created document.
                        downloadPath = DownloadDocument(RPTJobId, documentUrl, reportName, false, ext);
                        log.Debug("DownloadDocument : " + downloadPath.ToString());

                        message += " , DownloadDocument";
                        //Convert to binary
                        byte[] bytes = GetBinaryFile(downloadPath);
                        message += " , GetBinaryFile";
                        // Open the document.
                        //     System.Diagnostics.Process.Start(downloadPath);


                        // Report generation end
                        rowsAffected = UpadateJobStatus(RPTJobId, ReportScheduleStatus.COMPLETED.ToString(), ReportScheduleStatus.COMPLETED.ToString(), bytes, ScheduleDateTime);


                    }



                }
                catch (NullReferenceException ex)
                {
                    log.Warn("In CreateReportDocument exception " , ex);

                    message += ", " + reportName + " NotFound in the list of reports : " + reports.Count.ToString() + " , " + ex.Message + " CallStack " + ex.StackTrace; 
                }
                catch (Exception ex)
                {
                    log.Warn("In CreateReportDocument exception " , ex);
                    message += " exception : " + ex.Message + " InnerException : " + ex.InnerException + " CallStack " + ex.StackTrace;
                }
                finally   // Will execute despite any exception
                {
                    if (rowsAffected <= 0)
                    {
                        FailureCount++; int rowsAffected1 = 0;
                        conn.Open();
                        using (SqlCommand cmd1 = new SqlCommand("update   RPTjobdetail set status = 'Failed',StatusDetail = @StatusDetail ,JobStartDateTime =@ScheduleDateTime ,JobEndDateTime = @JobEndDateTime  ,FailureCount = @FailureCount where id = @RPTJobId", conn))
                        {
                            string errmsg = message;
                            cmd1.CommandType = CommandType.Text;
                            cmd1.Parameters.AddWithValue("@RPTJobId", RPTJobId);
                            cmd1.Parameters.AddWithValue("@StatusDetail", errmsg.ToString());
                            cmd1.Parameters.AddWithValue("@FailureCount", FailureCount);
                            cmd1.Parameters.AddWithValue("@ScheduleDateTime", ScheduleDateTime);
                            string JobEndDateTime = System.DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
                            cmd1.Parameters.AddWithValue("@JobEndDateTime", JobEndDateTime);
                            rowsAffected1 = cmd1.ExecuteNonQuery();
                        }
                        conn.Close();
                    }
                }
    
            if (downloadPath.Length > 1)
            {
                try
                {
                    string body = string.Empty;

                    string EmailTemplatePathString = ConfigurationManager.AppSettings["EmailTemplatePath"].ToString();
                    using (StreamReader reader = new StreamReader(EmailTemplatePathString))//"../Templates/ReportSchedulled_EmailBody.html"))
                    {
                        body = reader.ReadToEnd();
                    }

                    body = body.Replace("{reportName}", reportName);
                    //body = body.Replace("{clientname}", clientname);

                    SendEMail(Recipients, EmailSubject, body, downloadPath.ToString());

                    string errmsg = "Completed and Successfully Sent mail ";
                    conn.Open();
                    using (SqlCommand cmd1 = new SqlCommand("update   RPTjobdetail set StatusDetail = @StatusDetail where id = @RPTJobId", conn))
                    {
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Parameters.AddWithValue("@RPTJobId", RPTJobId);
                        cmd1.Parameters.AddWithValue("@StatusDetail", errmsg.ToString());
                        rowsAffected = cmd1.ExecuteNonQuery();
                    }
                    conn.Close();

                }
                catch (Exception ex)
                {
                    log.Warn("In CreateReportDocument Completed But Send mail throwing exception " ,ex);

                    // Report generation end
                    string errmsg = "Completed But Send mail throwing exception : " + ex.Message + " InnerException : " + ex.InnerException + " CallStack " + ex.StackTrace;
                    conn.Open();
                    using (SqlCommand cmd1 = new SqlCommand("update   RPTjobdetail set StatusDetail = @StatusDetail where id = @RPTJobId", conn))
                    {
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Parameters.AddWithValue("@RPTJobId", RPTJobId);
                        cmd1.Parameters.AddWithValue("@StatusDetail", errmsg.ToString());
                        rowsAffected = cmd1.ExecuteNonQuery();
                    }
                    conn.Close();

                }
            }

        }

        static byte[] GetBinaryFile(string filename)
        {
            byte[] bytes;
            try { 
                using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    bytes = new byte[file.Length];
                    file.Read(bytes, 0, (int)file.Length);
                }
           }
            catch (Exception ex)
            {
                throw ex;
            }

            return bytes;
        }
        private string GetAuthenticationToken(string usernameInput, string passwordInput)
        {
            try
            {
                string message = "";
                message += " GetAuthenticationToken in";
                log.Debug("GetAuthenticationToken in");

                var data = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", usernameInput),
                new KeyValuePair<string, string>("password", passwordInput)
                });

                // POST Token
                message += " GetAuthenticationToken before Post";
                var response = client.PostAsync(reportServerUrl + "Token", data).Result;
                message += " GetAuthenticationToken after Post";
                response.EnsureSuccessStatusCode();

                var result = response.Content.ReadAsStringAsync().Result;
                message += " GetAuthenticationToken got Result";
                var token = JObject.Parse(result).SelectToken("access_token").ToString();
                message += " GetAuthenticationToken got Result";

                return token;
            }
            catch (Exception ex)
            {
                log.Warn("Error While CreateCurrentReportSchedule: {0} " ,ex);
                
            }
            return null;
        }

        private async Task<List<Dictionary<string, string>>> GetAllReports( string apiUrl)
        {
            // GET api/reportserver/reports
            var response = await client.GetAsync(apiUrl + "reports");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<Dictionary<string, string>>>();
            }

            return null;
        }
        static async Task<string> CreateDocument(HttpClient client, string apiUrl, string format, string reportId, object parameterValues)
        {
            object reportParams = Newtonsoft.Json.JsonConvert.DeserializeObject<Object>(parameterValues.ToString());
            var data = new { ReportId = reportId, Format = format, ParameterValues = reportParams };
            // POST api/reportserver/documents
            var response = await client.PostAsJsonAsync(apiUrl + "documents", data);
            response.EnsureSuccessStatusCode();

            var documentUrl = response.Headers.Location;

            return documentUrl.ToString();
        }

        static string DownloadDocument(string RPTJobId, string url, string reportName, bool asAttachment, string ext)
        {
            var queryString = asAttachment ? "?content-disposition=attachment" : "";
            var fileName =  reportName + '_'+RPTJobId.ToString() + '.' + ext;
            var folderName = System.IO.Path.GetTempPath();
            var filePath = System.IO.Path.Combine(folderName, fileName);

            using (var webClient = new System.Net.WebClient())
            {
                webClient.DownloadFile(url + queryString, filePath);
            }

            return filePath;
        }
        public void SendEMail(string emailid, string subject, string body, string PathToAttachment)
        {

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(EmailFrom, "AMS Report Manager");
            // mail.To
            string[] array = emailid.Split(';');

            mail.Bcc.Add(SupportMail2Bcc);
            if (PathToAttachment.Length > 1)
            {
                mail.Attachments.Add(new Attachment(PathToAttachment));
            }
            if (!string.IsNullOrEmpty(Bcc))
            {
                mail.Bcc.Add(Bcc);//mail.Bcc.Add(Bcc);
            }
            foreach (string value in array)
            {
                mail.Bcc.Add(value);
            }


            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            String HOST = System.Configuration.ConfigurationManager.AppSettings["smtpHost"];// WebConfigurationManager.AppSettings["smtpHost"];

            int PORT = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["smtpPort"]);// Convert.ToInt16(WebConfigurationManager.AppSettings["smtpPort"]);


            using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(HOST, PORT))
            {

                try
                {

                    String pwd = System.Configuration.ConfigurationManager.AppSettings["smtpUserPwd"];
                    String userid = System.Configuration.ConfigurationManager.AppSettings["smtpUserId"];
                    String TLS = System.Configuration.ConfigurationManager.AppSettings["smtpTLS"];
                    if (!string.IsNullOrEmpty(TLS) && TLS == "Yes")
                    {
                        client.EnableSsl = true;
                    }
                    if (!string.IsNullOrEmpty(pwd))
                    {
                        NetworkCredential Credentials = new NetworkCredential(userid, pwd);
                        client.Credentials = Credentials;
                    }
                    client.Send(mail);
                }
                catch (Exception ex)
                {
                    log.Warn("Error While sending mail from web server : {0} ", ex);
                    throw ex;
                }
            }

        }

        public void SendIMEMail(string emailid, string subject, string body, string PathToAttachment)
        {

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(EmailFrom, "Optimization Tool IM AutoNotification");
            // mail.To
            string[] array = emailid.Split(';');

            mail.Bcc.Add(SupportMail2Bcc);
            if (PathToAttachment.Length > 1)
            {
                mail.Attachments.Add(new Attachment(PathToAttachment));
            }
            if (!string.IsNullOrEmpty(Bcc))
            {
                mail.To.Add(Bcc);//mail.Bcc.Add(Bcc);
            }
            //mail.Bcc.Add("Geetha.Bommalata @nttdata.com");
            foreach (string value in array)
            {
                mail.To.Add(value);
            }


            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            String HOST = System.Configuration.ConfigurationManager.AppSettings["smtpHost"];// WebConfigurationManager.AppSettings["smtpHost"];

            int PORT = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["smtpPort"]);// Convert.ToInt16(WebConfigurationManager.AppSettings["smtpPort"]);


            using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(HOST, PORT))
            {

                try
                {

                    String pwd = System.Configuration.ConfigurationManager.AppSettings["smtpUserPwd"];
                    String userid = System.Configuration.ConfigurationManager.AppSettings["smtpUserId"];
                    String TLS = System.Configuration.ConfigurationManager.AppSettings["smtpTLS"];
                    if (!string.IsNullOrEmpty(TLS) && TLS == "Yes")
                    {
                        client.EnableSsl = true;
                    }
                    if (!string.IsNullOrEmpty(pwd))
                    {
                        NetworkCredential Credentials = new NetworkCredential(userid, pwd);
                        client.Credentials = Credentials;
                    }
                    client.Send(mail);
                }
                catch (Exception ex)
                {
                    log.Warn("Error While sending IM mail from web server : {0} ", ex);
                    throw ex;
                }
            }

        }
        private int UpadateJobStatus(string RPTJobId, string StatusVal, string StatusDetail, Byte[] bytes, string ScheduleDateTime)
        {
            try
            {
                int retval = 0; SqlConnection conn = new SqlConnection(ConString); conn.Open();
                log.Debug(" UpadateJobStatus: for the Job id : " + RPTJobId + " Status " + StatusVal + " StatusDetail : " + StatusDetail + " .\n ");
                if (bytes != null)//bytes.Length > 0 &&
                {
                    using (SqlCommand cmd1 = new SqlCommand("update RPTjobdetail set status = '" + StatusVal + "', StatusDetail = @StatusDetail ,JobStartDateTime =@ScheduleDateTime,JobEndDateTime =  @JobEndDateTime ,  ReportDocument = @ReportDocument  where id = @RPTJobId", conn))
                    {
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Parameters.AddWithValue("@RPTJobId", RPTJobId);
                        cmd1.Parameters.AddWithValue("@ReportDocument", bytes); //pass null for failed and started status
                        cmd1.Parameters.AddWithValue("@ScheduleDateTime", ScheduleDateTime);
                        cmd1.Parameters.AddWithValue("@StatusDetail", StatusDetail);
                        string JobEndDateTime = "";
                        JobEndDateTime = System.DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"); //update end date only for completed
                        cmd1.Parameters.AddWithValue("@JobEndDateTime", JobEndDateTime);

                        retval = cmd1.ExecuteNonQuery();
                    }
                    log.Debug(" UpadateJobStatus: with Report for the Job id : " + RPTJobId + " Status " + StatusVal + " StatusDetail : " + StatusDetail + " bytes :" + bytes.ToString() + ".\n ");

                }
                else
            {
                    using (SqlCommand cmd1 = new SqlCommand("update RPTjobdetail set status = '" + StatusVal + "', StatusDetail = @StatusDetail ,JobStartDateTime =@ScheduleDateTime where id = @RPTJobId", conn))
                    {
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Parameters.AddWithValue("@RPTJobId", RPTJobId);
                        cmd1.Parameters.AddWithValue("@ScheduleDateTime", ScheduleDateTime);
                        cmd1.Parameters.AddWithValue("@StatusDetail", StatusDetail);

                        retval = cmd1.ExecuteNonQuery();
                    }
                    log.Debug(" UpadateJobStatus: Inprocess of Report for the Job id : " + RPTJobId + " Status " + StatusVal + " StatusDetail : " + StatusDetail + ".\n ");


                }
                conn.Close();
                return retval;
            }
            catch (Exception ex)
            {
                log.Warn("Error While UpadateJobStatus: for the Job id : " + RPTJobId ,ex);
                return 0;
            }

        }

        private int UpadateA3MMJobStatus(string RPTJobId, string StatusVal, string StatusDetail, Byte[] bytes, string ScheduleDateTime)
        {
            try
            {
                int retval = 0; SqlConnection conn = new SqlConnection(ConString); conn.Open();
                log.Debug(" UpadateJobStatus: for the Job id : " + RPTJobId + " Status " + StatusVal + " StatusDetail : " + StatusDetail + " .\n ");
                if (bytes != null)//bytes.Length > 0 &&
                {
                    using (SqlCommand cmd1 = new SqlCommand("update RPTjobdetail set status = '" + StatusVal + "', StatusDetail = @StatusDetail ,JobStartDateTime =@ScheduleDateTime,JobEndDateTime =  @JobEndDateTime ,  ReportDocument = @ReportDocument  where id = @RPTJobId", conn))
                    {
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Parameters.AddWithValue("@RPTJobId", RPTJobId);
                        cmd1.Parameters.AddWithValue("@ReportDocument", bytes); //pass null for failed and started status
                        cmd1.Parameters.AddWithValue("@ScheduleDateTime", ScheduleDateTime);
                        cmd1.Parameters.AddWithValue("@StatusDetail", StatusDetail);
                        string JobEndDateTime = "";
                        JobEndDateTime = System.DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"); //update end date only for completed
                        cmd1.Parameters.AddWithValue("@JobEndDateTime", JobEndDateTime);

                        retval = cmd1.ExecuteNonQuery();
                    }
                    log.Debug(" UpadateJobStatus: with Report for the Job id : " + RPTJobId + " Status " + StatusVal + " StatusDetail : " + StatusDetail + " bytes :" + bytes.ToString() + ".\n ");

                }
                else
                {
                    using (SqlCommand cmd1 = new SqlCommand("update RPTjobdetail set status = '" + StatusVal + "', StatusDetail = @StatusDetail ,JobStartDateTime =@ScheduleDateTime where id = @RPTJobId", conn))
                    {
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Parameters.AddWithValue("@RPTJobId", RPTJobId);
                        cmd1.Parameters.AddWithValue("@ScheduleDateTime", ScheduleDateTime);
                        cmd1.Parameters.AddWithValue("@StatusDetail", StatusDetail);

                        retval = cmd1.ExecuteNonQuery();
                    }
                    log.Debug(" UpadateJobStatus: Inprocess of Report for the Job id : " + RPTJobId + " Status " + StatusVal + " StatusDetail : " + StatusDetail + ".\n ");


                }
                conn.Close();
                return retval;
            }
            catch (Exception ex)
            {
                log.Warn("Error While UpadateJobStatus: for the Job id : " + RPTJobId, ex);
                return 0;
            }

        }

        public string IdeaManagerEmailScheduleCheck()
        {

            log.Info("IdeaManagerEmailScheduleCheck : In.. Before Try");
       //     try
            {
                SqlConnection conn = new SqlConnection(ConString);
                using (SqlCommand cmd = new SqlCommand("select * from IM_ScheduleEmail where Isdeleted =0", conn))
                {
                    conn.Open();

                    string IMSch_name, selectionQuery, EmailedTo,  Errormessage, Subject, EmailBody;
                    string Approver, Creater, ImplementationManager, ItemName, MultipleApprovals, ProposedBy;
                    int Id;
                            // int RPTSubscriptionId = 0;
                    //string ReportStartDate, ReportEndDate;
                    //ReportStartDate = ReportEndDate = System.DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
                    cmd.CommandType = CommandType.Text;
                    DataTable dt;// = new DataTable();

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (dt = new DataTable())
                        {
                            sda.Fill(dt);
                            //Id	IMSch_name	selectionQuery	EmailedTo	CreatedDatetime	UpdatedDatetime	Status	Errormessage	Subject	EmailBody	Isdeleted
                            int n_scheduledIMEmailcnt = 0;
                            while (n_scheduledIMEmailcnt < dt.Rows.Count )
                            {
                                IMSch_name = dt.Rows[n_scheduledIMEmailcnt]["IMSch_name"].ToString();
                                selectionQuery = dt.Rows[n_scheduledIMEmailcnt]["selectionQuery"].ToString()+ " and IsDeleted = 0 and Id  not in  (select 1 union select Idea_ID from IM_ScheduleEmailStatus where  CONVERT(date, CreatedDatetime) = CONVERT(date,GETDATE()))";
                                EmailedTo = dt.Rows[n_scheduledIMEmailcnt]["EmailedTo"].ToString();
                                Subject = dt.Rows[n_scheduledIMEmailcnt]["Subject"].ToString();
                                EmailBody = dt.Rows[n_scheduledIMEmailcnt]["EmailBody"].ToString();
                                log.Debug("Started Processing the idea id : " + IMSch_name);
                                int Scheduleno = n_scheduledIMEmailcnt;
                                n_scheduledIMEmailcnt++;
                                try
                                {
                                    SqlCommand Ideacmd = new SqlCommand(selectionQuery, conn);
                                    cmd.CommandType = CommandType.Text;
                                    DataTable Ideadt;// = new DataTable();

                                    using (SqlDataAdapter Ideasda = new SqlDataAdapter(Ideacmd))
                                    {
                                        using (Ideadt = new DataTable())
                                        {
                                            Ideasda.Fill(Ideadt);
                                            //Id	IMSch_name	selectionQuery	EmailedTo	CreatedDatetime	UpdatedDatetime	Status	Errormessage	Subject	EmailBody	Isdeleted
                                            string Contentbody = EmailBody, Recipients, body;
                                            int n_Ideacnt = 0;

                                            while (n_Ideacnt <Ideadt.Rows.Count )
                                            {
                                                //go through all Ideas ,ProposedBy
                                                Approver = Ideadt.Rows[n_Ideacnt]["Approver"].ToString();
                                                Creater = Ideadt.Rows[n_Ideacnt]["Creater"].ToString();
                                                ImplementationManager = Ideadt.Rows[n_Ideacnt]["ImplementationManager"].ToString();
                                                ItemName = Ideadt.Rows[n_Ideacnt]["ItemName"].ToString();
                                                MultipleApprovals = Ideadt.Rows[n_Ideacnt]["MultipleApprovals"].ToString();
                                                ProposedBy = Ideadt.Rows[n_Ideacnt]["ProposedBy"].ToString();
                                                Id = Int32.Parse(Ideadt.Rows[n_Ideacnt]["Id"].ToString());
                                                Recipients = Ideadt.Rows[n_Ideacnt][EmailedTo].ToString();
                                                n_Ideacnt++;

                                                //Subject = dt.Rows[n_scheduledIMEmailcnt]["Subject"].ToString();
                                                //Subject = dt.Rows[n_scheduledIMEmailcnt]["Subject"].ToString();
                                                //Subject = dt.Rows[n_scheduledIMEmailcnt]["Subject"].ToString();
                                                //Subject = dt.Rows[n_scheduledIMEmailcnt]["Subject"].ToString();


                                                //       body = EmailBody.Replace("{ideaname}", ReportName);

                                                Contentbody = Contentbody.Replace("{Ideaname}", ItemName);
                                                Contentbody = Contentbody.Replace("{Approvername}", Approver);
                                                Contentbody = Contentbody.Replace("{Creatername}", Creater);
                                                Contentbody = Contentbody.Replace("{ImplementationManagername}", ImplementationManager);
                                                
                                                string IMScheduledEmailTemplatePathString = ConfigurationManager.AppSettings["IMScheduledEmailTemplatePath"].ToString();
                                                using (StreamReader reader = new StreamReader(IMScheduledEmailTemplatePathString))
                                                {
                                                    body = reader.ReadToEnd();
                                                }
                                                body = body.Replace("{EmailContent}", Contentbody);
                                                if(Recipients !=null && Recipients.Length>1)
                                                body = body.Replace("{ToName}", Recipients.Substring(0,Recipients.IndexOf('@')));
                                                else
                                                    body = body.Replace("{ToName}", "");


                                                //log.Info("Final Url - " + FinalUri.ToString());
                                                string ImProcessStatus = Scheduleno.ToString() + IMSch_name + " Success";
                                                string IErrormessage = "Completed and Successfully Sent mail ";
                                                if (Id == null || Recipients == null || Recipients.Length < 4)
                                                {
                                                    ImProcessStatus = Scheduleno.ToString() + IMSch_name + " Failed";
                                                    IErrormessage = "mail is not sent for Idea_ID " + Id + " and  EmailedTo be sent : " + EmailedTo + " and Recipients :" + Recipients;
                                                }
                                                else
                                                {
                                                    try
                                                    {
                                                        SendIMEMail(Recipients, Subject, body, "");
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        ImProcessStatus = Scheduleno.ToString() + IMSch_name + "Failed in SendIMEMail";
                                                        IErrormessage = "Exception while Sending Mail :" + ex.Message;
                                                    }
                                                }
                                                    if (conn.State != ConnectionState.Open)
                                                    conn.Open();

                                                using (SqlCommand cmd1 = new SqlCommand("insert into IM_ScheduleEmailStatus    ( Idea_ID, ItemName,EmailedTo, CreatedDatetime, UpdatedDatetime, Status, Errormessage, Subject) VALUES    (@Idea_ID,@ItemName,@EmailedTo,@UpdatedDatetime ,@UpdatedDatetime ,@Status,@Errormessage,@Subject)", conn))
                                                {
                                                    cmd1.CommandType = CommandType.Text;
                                                    cmd1.Parameters.AddWithValue("@Idea_ID", Id.ToString());
                                                    cmd1.Parameters.AddWithValue("@ItemName", ItemName);
                                                    cmd1.Parameters.AddWithValue("@EmailedTo", Recipients);
                                                    cmd1.Parameters.AddWithValue("@Status", ImProcessStatus);
                                                    cmd1.Parameters.AddWithValue("@Errormessage", IErrormessage);
                                                    cmd1.Parameters.AddWithValue("@Subject", Subject);
                                                    cmd1.Parameters.AddWithValue("@UpdatedDatetime", DateTime.Now);
                                                    int rowsAffected = cmd1.ExecuteNonQuery();
                                                }
                             //                   conn.Close();
                                                
                                            }
                                            
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    log.Info("Error while IdeaManagerEmailScheduleCheck :" + ex.Message + " CallStack " + ex.StackTrace);

                                    //log.Warn("Error while creating report schedule for the Job id : " + RPTJobId, ex);
                                    
                                }
                            }
                        }

                    }
                }
                
                    conn.Close();
            }
            log.Info("Completed IdeaManagerEmailScheduleCheck ");

            return "";
            
            }

        public string A3MMA3MM_ActionItemTrackerScheduleCheck()
        {

            log.Info("A3MMA3MM_ActionItemTrackerScheduleCheck : In.. Before Try");
            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["Db"].ToString(); //"Data Source=10.100.100.10;Initial Catalog=AMO_PORTAL_APPip_Test;User ID=amo_dev;Password=Nttamo@dev;Timeout=180";//ConfigurationManager.ConnectionStrings["Db"].ToString(); //
                SqlConnection conn = new SqlConnection(ConString);

                DateTime TodayDate = DateTime.Now;
                string EmailSubject = "A3MM Email Communication";
                string A3MMEmailTemplateBody = string.Empty;

                string A3MMEmailTemplatePathString = ConfigurationManager.AppSettings["A3MMEmailTemplatePath"].ToString();
                using (StreamReader reader = new StreamReader(A3MMEmailTemplatePathString))
                {
                    A3MMEmailTemplateBody = reader.ReadToEnd();
                }

                //Track Areas of improvement with periodical email communication – email to be sent to engagement point of contact for status updates
                string qry = " select AT.Id, AssessmentID, Assessmentname,Discipline_ID,UP.id  as Userid,UP.UserName,	UP.Email,	UP.FirstName,	Component_ID,	Aspect_ID,	ActionItem,	OwnerID,	PlannedCompletionDate,	AT.Status,	Comments,AT.ClientID,	 ClientName from A3MM_ActionItemTracker AT left join Clients C on AT.clientid = C.id"
                               + " left join A3MM_Assessment AA on AT.AssessmentID = AA.id left join UserProfile UP on AT.OwnerId = UP.ID "
                               + " where  AT.Status in ('Defined' , 'In Progress','Hold') and PlannedCompletionDate > DATEADD(day, -1, CAST(GETDATE() AS date)) and  PlannedCompletionDate < DATEADD(day, 1, CAST(GETDATE() AS date)) "
                               + "  and 		 AT.AssessmentID not in (select assessmentid  from A3MM_AssessmentEmailStatus  where  CreatedDatetime > CAST(GETDATE() AS date)	and A3MMTableName = 'A3MM_ActionItemTracker' )";
                //where PlannedCompletionDate > DATEADD(day, -2, CAST(GETDATE() AS date))
                log.Info("A3MMA3MM_ActionItemTrackerScheduleCheck : In Try Qry is"+qry.ToString() );
                using (SqlCommand cmd = new SqlCommand(qry, conn))
                {
                    conn.Open();

                    string ClientID, ClientName, AssessmentID, Assessmentname, FirstName, Email, Userid, UserName;
                    DateTime PlannedCompletionDate;
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        conn.Close();

                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            foreach (DataRow row in dt.Rows)
                            {

                                ClientID = row["ClientID"].ToString();
                                ClientName = row["ClientName"].ToString();
                                AssessmentID = row["AssessmentID"].ToString();
                                Assessmentname = row["Assessmentname"].ToString();
                                PlannedCompletionDate = Convert.ToDateTime(row["PlannedCompletionDate"]);
                                FirstName = row["FirstName"].ToString();
                                Email = row["Email"].ToString();
                                UserName = row["UserName"].ToString();
                                Userid = row["Userid"].ToString();

                                //bool bEmailSchedulednow = true;
                                string Due = "";
                                switch ((PlannedCompletionDate - DateTime.Now).TotalDays)
                                {

                                    case 1:
                                        Due = " due tomorrow";
                                        break;
                                    case 0:
                                        Due = " due today";
                                        break;
                                    case -1:
                                        Due = " Overdue";
                                        break;
                                    default:
                                        Due = "due by " + PlannedCompletionDate.ToShortDateString();
                                        //bEmailSchedulednow = false;
                                        break;
                                }
                                    log.Info("A3MMA3MM_ActionItemTrackerScheduleCheck : Before sending mail");

                                    int InsertedRecID = 0;
                                    EmailSubject = "Reminder for   Client " + ClientName + " - Assessmentname " + Assessmentname + " is " + Due.ToString();
                                    DateTime CurrentDateTime = DateTime.Now;
                                    InsertedRecID = Insert_A3MM_AssessmentEmailStatus(AssessmentID, ClientID, Assessmentname, "", "", EmailSubject, "Initiated", CurrentDateTime, "A3MM_ActionItemTracker");
                                    //WriteToFile_A3MMLog("A3MM Scheduled email  " + Due.ToString() + " is  for the AssessmentID " + AssessmentID.ToString() + " : " + Assessmentname.ToString());
                                    string body = A3MMEmailTemplateBody;

                                    Upadate_A3MM_AssessmentEmailStatus(InsertedRecID, AssessmentID, ClientID, Assessmentname, UserName, A3MMEmailTemplatePathString.ToString(), EmailSubject, "ReadPath. Check ErrMsg for path " , DateTime.Now);
                                    
                                    body = body.Replace("{AssessmentName}", Assessmentname);
                                    body = body.Replace("{Owner}", UserName.Substring(0, UserName.IndexOf(".")));
                                body = body.Replace("{Due}", Due);
                                body = body.Replace("{CompletionDate}", PlannedCompletionDate.ToShortDateString());
                                //  log.Info("Final Url - " + FinalUri.ToString());
                                log.Info("A3MMA3MM_ActionItemTrackerScheduleCheck : In.. sending mail");
                                    Upadate_A3MM_AssessmentEmailStatus(InsertedRecID, AssessmentID, ClientID, Assessmentname, UserName, "", EmailSubject, "About to send mail", DateTime.Now);

                                    SendA3MMEMail(UserName, EmailSubject , body, "");
                                   // WriteToFile_A3MMLog("Completed and Successfully Sent A3MM_ActionItemTracker mail  " + EmailSubject.ToString());
                                    Upadate_A3MM_AssessmentEmailStatus(InsertedRecID, AssessmentID, ClientID, Assessmentname, UserName, "", EmailSubject, "Completed", DateTime.Now);


                            }
                        }
                    }


                }


            }
            catch (Exception ex)
            {
                log.Info("A3MMA3MM_ActionItemTrackerScheduleCheck : In catch");

                WriteToFile_A3MMLog("Error while starting A3MMA3MM_ActionItemTrackerScheduleCheck  " + ex.Message + " " + ex.StackTrace);
                throw new Exception("Error while starting A3MMA3MM_ActionItemTrackerScheduleCheck Service; please check the log file for more detail.", ex);
            }
            return "";
        }
        public string A3MMA3MM_AssessmentScheduleCheck()
        {
            string AssessmentID = "not initialted", ClientID = "not initialted", Assessmentname = "not initialted", AssessmentTeam = "not initialted";
            string EmailSubject = "A3MM Email Communication";
            int InsertedRecID = 0;
            log.Info("A3MMA3MM_AssessmentScheduleCheck : In.. Before Try");
            string A3MMNextAssessmentEmailBody = string.Empty;
            //    log.Info("Non Telerik report subscription processing : ReportName - " + ReportName.ToString());

            string A3MMNextAssessmentEmailTemplatePathString = ConfigurationManager.AppSettings["A3MMNextAssessmentEmailTemplatePath"].ToString();
            using (StreamReader reader = new StreamReader(A3MMNextAssessmentEmailTemplatePathString))//"../Templates/ReportSchedulled_EmailBody.html"))
            {
                A3MMNextAssessmentEmailBody = reader.ReadToEnd();
            }

            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["Db"].ToString(); //"Data Source=10.100.100.10;Initial Catalog=AMO_PORTAL_APPip_Test;User ID=amo_dev;Password=Nttamo@dev;Timeout=180";//ConfigurationManager.ConnectionStrings["Db"].ToString(); //
                SqlConnection conn = new SqlConnection(ConString);

                DateTime TodayDate = DateTime.Now;
                //Track Areas of improvement with periodical email communication – email to be sent to engagement point of contact for status updates
                string qry = " ";

                //Communication to assessment team to schedule for next assessment (once a year for A3MM and every quarter for IVV)
                qry = " select AA.Id as AssessmentId,SurveyTypeID,	 AA.ClientId,C.ClientName,ContractName,	EngagementName,	StartDate,	EndDate,		AA.Status,EngagementTeamPointOfContact,	EngagementTeamtoCommunicate,	AssessmentTeam,	AA.AssessmentName,	QuestionaireVersion "
                                +" from A3MM_Assessment AA left "
                                +" join Clients C on AA.clientid = C.id  left "
                                +" join ClientAccountContracts CA  on AA.AccountID = CA.id"
                                + " where AA.Status like 'Publish%' AND CAST( StartDate AS date)  < CAST(GETDATE() AS date)  and DAY(StartDate) = DAY(DATEADD(day, 7, getdate())) and((SurveyTypeID = 2 and Month(StartDate) = Month(DATEADD(day, 7, getdate()))) or(SurveyTypeID = 1 and((Month(StartDate) - Month(DATEADD(day, 7, getdate()))) % 3) = 0) )"
                                + " and AA.ID not in (select assessmentid from A3MM_AssessmentEmailStatus where  CreatedDatetime > CAST(GETDATE() AS date) and A3MMTableName = 'A3MM_Assessment' )";

                log.Info("A3MMA3MM_AssessmentScheduleCheck : In Try Qry is " + qry.ToString());
                using (SqlCommand cmd = new SqlCommand(qry, conn))
                {
                    conn.Open();

                    string ClientName, SurveyTypeID, ContractName, EngagementName;
                    //AssessmentId,	EndDate,		Status,EngagementTeamPointOfContact,	EngagementTeamtoCommunicate,	AssessmentTeam,	,	QuestionaireVersion;
                    DateTime AssessmentStartDate;
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        conn.Close();

                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            foreach (DataRow row in dt.Rows)
                            {

                                ClientID = row["ClientID"].ToString();
                                ClientName = row["ClientName"].ToString();
                                AssessmentID = row["AssessmentID"].ToString();
                                Assessmentname = row["Assessmentname"].ToString();
                                SurveyTypeID = row["SurveyTypeID"].ToString();
                                ContractName = row["ContractName"].ToString();
                                EngagementName = row["EngagementName"].ToString();
                                AssessmentTeam = row["AssessmentTeam"].ToString();
                                AssessmentStartDate = Convert.ToDateTime(row["StartDate"]);
                                string Errormessage = "";
                                Errormessage = "Reminder for Schedule for next assessment for Client " + ClientName + " - Assessmentname " + Assessmentname;
                                InsertedRecID = Insert_A3MM_AssessmentEmailStatus(AssessmentID, ClientID, Assessmentname, AssessmentTeam, Errormessage, "Entered A3MMA3MM_AssessmentScheduleCheck", "Initiated", DateTime.Now,"A3MM_Assessment");

                                /*bool bEmailSchedulednow = true;
                                switch (SurveyTypeID)
                                {

                                    case "2"://A3MM every year
                                        if (AssessmentStartDate.Day != TodayDate.Day || AssessmentStartDate.Month != TodayDate.Month)
                                            bEmailSchedulednow = false;
                                        break;
                                    case "1"://IVV every Quarter
                                        if (AssessmentStartDate.Day != TodayDate.Day || (AssessmentStartDate.Month - TodayDate.Month) % 3 != 0)
                                            bEmailSchedulednow = false;
                                        break;
                                    default:
                                        bEmailSchedulednow = false;
                                        break;
                                }*/
                                //if (true)//bEmailSchedulednow)
                                //{
                                    log.Info("A3MMA3MM_AssessmentScheduleCheck : Before sending mail  AssessmentID " + AssessmentID.ToString() + " : " + Assessmentname.ToString());

                                    //WriteToFile_A3MMLog("A3MM Scheduled assessment email is  for the AssessmentID " + AssessmentID.ToString() + " : " + Assessmentname.ToString());
                                   // DateTime CurrentDateTime = DateTime.Now;
                                    Errormessage += "; Inserted rec in A3MM_AssessmentEmailStatus ";
                                //process Analysis report
                                    string body = A3MMNextAssessmentEmailBody;
                                    Upadate_A3MM_AssessmentEmailStatus(InsertedRecID, AssessmentID, ClientID, Assessmentname, AssessmentTeam, "", EmailSubject, "Read A3MMNextAssessmentEmailTemplatePath", DateTime.Now);
                                    body = body.Replace("{ClientName}", ClientName);
                                    body = body.Replace("{AssessmentName}", Assessmentname);
                                if (Convert.ToInt16(SurveyTypeID) == 1)
                                {
                                    body = body.Replace("{AssessmentDuration}", "quarter");
                                    body = body.Replace("{AssessmentType}", "IVV"); 
                                        }
                                else
                                {
                                    body = body.Replace("{AssessmentDuration}", "year");
                                    body = body.Replace("{AssessmentType}", "A3MM");
                                }


                                EmailSubject = "Reminder for Schedule for next assessment for Client " + ClientName + " - Assessmentname " + Assessmentname;
                                    //  log.Info("Final Url - " + FinalUri.ToString());
                                    log.Info("A3MMA3MM_AssessmentScheduleCheck : Sending mail to " + EmailSubject.ToString() + " for " + ClientName.ToString());
                                    Upadate_A3MM_AssessmentEmailStatus(InsertedRecID, AssessmentID, ClientID, Assessmentname, AssessmentTeam, "", EmailSubject, "About to send email", DateTime.Now);

                                    SendA3MMEMail(AssessmentTeam, EmailSubject.ToString(), body, "");


                                    //WriteToFile_A3MMLog("Completed and Successfully Sent A3MM_Assessment mail  " + EmailSubject.ToString());
                                    Upadate_A3MM_AssessmentEmailStatus(InsertedRecID, AssessmentID, ClientID, Assessmentname, AssessmentTeam, "", EmailSubject, "Completed", DateTime.Now);



                                //}
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Info("A3MMA3MM_AssessmentScheduleCheck : In Catch" + ex.Message + " " + ex.StackTrace);
               // WriteToFile_A3MMLog("Error while starting A3MMA3MM_AssessmentScheduleCheck  " + ex.Message + " " + ex.StackTrace);
                Upadate_A3MM_AssessmentEmailStatus(InsertedRecID, AssessmentID, ClientID, Assessmentname, AssessmentTeam, "Error while starting A3MMA3MM_AssessmentScheduleCheck  " + ex.Message + " " + ex.StackTrace, EmailSubject, "Initiated", DateTime.Now);

                throw new Exception("Error while starting A3MMA3MM_AssessmentScheduleCheck Service; please check the log file for more detail.", ex);
            }
            return "";
        }
        public static void SendA3MMEMail(string emailid, string subject, string body, string PathToAttachment)
        {

            MailMessage mail = new MailMessage();
            String EmailFrom = System.Configuration.ConfigurationManager.AppSettings["EmailFromA3MM"];
            String EmailFromDisplayName = System.Configuration.ConfigurationManager.AppSettings["EmailFromA3MMDisplayName"];// WebConfigurationManager.AppSettings[""];
            mail.From = new MailAddress(EmailFrom, EmailFromDisplayName);
            // mail.To
            string[] array = emailid.Split(';');

            String SupportMail2Bcc = System.Web.Configuration.WebConfigurationManager.AppSettings["SupportMail2"];
            mail.Bcc.Add(SupportMail2Bcc);
            //mail.Attachments.Add(new Attachment(PathToAttachment));
            String Bcc = System.Web.Configuration.WebConfigurationManager.AppSettings["EmailBcc"];
            if (!string.IsNullOrEmpty(Bcc))
            {
                mail.Bcc.Add(Bcc);
            }
            foreach (string value in array)
            {
                mail.Bcc.Add(value);
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            String HOST = System.Configuration.ConfigurationManager.AppSettings["smtpHost"];// WebConfigurationManager.AppSettings["smtpHost"];

            int PORT = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["smtpPort"]);// Convert.ToInt16(WebConfigurationManager.AppSettings["smtpPort"]);


            using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(HOST, PORT))
            {

                // Send the email. 
                try
                {

                    String pwd = System.Configuration.ConfigurationManager.AppSettings["smtpUserPwd"];// WebConfigurationManager.AppSettings["smtpUserPwd"];
                    String userid = System.Configuration.ConfigurationManager.AppSettings["smtpUserId"];//WebConfigurationManager.AppSettings["smtpUserId"];
                    String TLS = System.Configuration.ConfigurationManager.AppSettings["smtpTLS"];//WebConfigurationManager.AppSettings["smtpTLS"];
                    if (!string.IsNullOrEmpty(TLS) && TLS == "Yes")
                    {
                        client.EnableSsl = true;
                    }
                    if (!string.IsNullOrEmpty(pwd))
                    {
                        NetworkCredential Credentials = new NetworkCredential(userid, pwd);
                        client.Credentials = Credentials;
                    }
                    client.Send(mail);
                }
                catch (Exception ex)
                {
                    throw new Exception("error sending mail from web server", ex);
                }
            }

        }
        private int Insert_A3MM_AssessmentEmailStatus(string Assessmentid, string clientid, string Assessmentname, string EmailedTo, string Errormessage, string Subject, string StatusDetail, DateTime CurrentDateTime,string A3MMTableName)
        {
            try
            {
                int InsertedRecID = 0; SqlConnection conn = new SqlConnection(ConString);
                //WriteToFile_A3MMLog(" Insert_A3MM_AssessmentEmailStatus: Subject : " + Subject + " Status " + StatusDetail + " EmailedTo : " + EmailedTo + " .\n ");
                if (Assessmentid.Length > 0)
                {
                    string str1;
                    using (SqlCommand cmd1 = new SqlCommand("insert into A3MM_AssessmentEmailStatus (Assessmentid ,clientid,Assessmentname, EmailedTo, CreatedDatetime, UpdatedDatetime, Status , Errormessage, Subject,A3MMTableName) VALUES  (@Assessmentid ,@clientid,@Assessmentname, @EmailedTo, @CreatedDatetime, @UpdatedDatetime, @Status , @Errormessage, @Subject,@A3MMTableName);SELECT SCOPE_IDENTITY()", conn))
                    {
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Parameters.AddWithValue("@Assessmentid", Assessmentid);
                        cmd1.Parameters.AddWithValue("@clientid", clientid);
                        cmd1.Parameters.AddWithValue("@Assessmentname", Assessmentname);
                        cmd1.Parameters.AddWithValue("@EmailedTo", EmailedTo);
                        cmd1.Parameters.AddWithValue("@Errormessage", Errormessage);
                        cmd1.Parameters.AddWithValue("@Subject", Subject);
                        cmd1.Parameters.AddWithValue("@Status", StatusDetail);
                        cmd1.Parameters.AddWithValue("@CreatedDatetime", CurrentDateTime);
                        cmd1.Parameters.AddWithValue("@UpdatedDatetime", CurrentDateTime);
                        cmd1.Parameters.AddWithValue("@A3MMTableName", A3MMTableName);
                        conn.Open();

                        str1 = cmd1.ExecuteScalar().ToString();//.ExecuteNonQuery();
                        InsertedRecID = int.Parse(str1);
                    }

                    //WriteToFile_A3MMLog(" Insert_A3MM_AssessmentEmailStatus done: Subject : " + Subject + " Status " + StatusDetail + " EmailedTo : " + EmailedTo + " .\n ");

                }
                conn.Close();
                return InsertedRecID;
            }
            catch (Exception ex)
            {
                WriteToFile_A3MMLog("Error While Insert_A3MM_AssessmentEmailStatus: for the Assessmentid  : " + Assessmentid + " EXCEPTION Details " + ex.Message + " InnerException : " + ex.InnerException + " CallStack " + ex.StackTrace);
                return 0;
            }

        }

        private int Upadate_A3MM_AssessmentEmailStatus(int InsertedRecID, string Assessmentid, string clientid, string Assessmentname, string EmailedTo, string Errormessage, string Subject, string StatusDetail, DateTime CurrentDateTime)
        {
            try
            {
                SqlConnection conn = new SqlConnection(ConString);
                WriteToFile_A3MMLog(" Upadate_A3MM_AssessmentEmailStatus: Subject : " + Subject + " Status " + StatusDetail + " EmailedTo : " + EmailedTo + " .\n ");
                int rowsAffected = 0;
                string SqlCmdQry = "Update A3MM_AssessmentEmailStatus   set UpdatedDatetime =  @UpdatedDatetime , status = '" + StatusDetail + "', Assessmentname = '" + Assessmentname + "', EmailedTo = '" + EmailedTo + "', Errormessage = '" + Errormessage + "' , Subject = '" + Subject + "'  where id =" + InsertedRecID.ToString();
                if (Assessmentid.Length > 0)
                {

                    using (SqlCommand cmd1 = new SqlCommand(SqlCmdQry, conn))
                    {
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Parameters.AddWithValue("@UpdatedDatetime", CurrentDateTime);
                        conn.Open();

                        rowsAffected = cmd1.ExecuteNonQuery();
                        conn.Close();

                    }

                    WriteToFile_A3MMLog(" Upadate_A3MM_AssessmentEmailStatus done: Subject : " + Subject + " Status " + StatusDetail + " EmailedTo : " + EmailedTo + " .\n ");

                }
                return rowsAffected;
            }
            catch (Exception ex)
            {
                WriteToFile_A3MMLog("Error While Upadate_A3MM_AssessmentEmailStatus: for the Assessmentid  : " + Assessmentid + " EXCEPTION Details " + ex.Message + " InnerException : " + ex.InnerException + " CallStack " + ex.StackTrace);
                return 0;
            }

        }

        public static void SendIM_ScheduleEMail(string emailid, string subject, string body, string PathToAttachment)
        {

            MailMessage mail = new MailMessage();
            String EmailFrom = System.Configuration.ConfigurationManager.AppSettings["EmailFromA3MM"];
            String EmailFromDisplayName = System.Configuration.ConfigurationManager.AppSettings["EmailFromA3MMDisplayName"];// WebConfigurationManager.AppSettings[""];
            mail.From = new MailAddress(EmailFrom, EmailFromDisplayName);
            // mail.To
            string[] array = emailid.Split(';');

            String SupportMail2Bcc = System.Web.Configuration.WebConfigurationManager.AppSettings["SupportMail2"];
            mail.Bcc.Add(SupportMail2Bcc);
            //mail.Attachments.Add(new Attachment(PathToAttachment));
            String Bcc = System.Web.Configuration.WebConfigurationManager.AppSettings["EmailBcc"];
            if (!string.IsNullOrEmpty(Bcc))
            {
                mail.Bcc.Add(Bcc);
            }
            foreach (string value in array)
            {
                mail.Bcc.Add(value);
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            String HOST = System.Configuration.ConfigurationManager.AppSettings["smtpHost"];// WebConfigurationManager.AppSettings["smtpHost"];

            int PORT = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["smtpPort"]);// Convert.ToInt16(WebConfigurationManager.AppSettings["smtpPort"]);


            using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(HOST, PORT))
            {

                // Send the email. 
                try
                {

                    String pwd = System.Configuration.ConfigurationManager.AppSettings["smtpUserPwd"];// WebConfigurationManager.AppSettings["smtpUserPwd"];
                    String userid = System.Configuration.ConfigurationManager.AppSettings["smtpUserId"];//WebConfigurationManager.AppSettings["smtpUserId"];
                    String TLS = System.Configuration.ConfigurationManager.AppSettings["smtpTLS"];//WebConfigurationManager.AppSettings["smtpTLS"];
                    if (!string.IsNullOrEmpty(TLS) && TLS == "Yes")
                    {
                        client.EnableSsl = true;
                    }
                    if (!string.IsNullOrEmpty(pwd))
                    {
                        NetworkCredential Credentials = new NetworkCredential(userid, pwd);
                        client.Credentials = Credentials;
                    }
                    client.Send(mail);
                }
                catch (Exception ex)
                {
                    throw new Exception("error sending mail from web server", ex);
                }
            }

        }
    /*    private int Upadate_IM_ScheduleEmailStatus(int InsertedRecID, string Assessmentid, string clientid, string Assessmentname, string EmailedTo, string Errormessage, string Subject, string StatusDetail, DateTime CurrentDateTime)
        {
            try
            {
                SqlConnection conn = new SqlConnection(ConString);
                int rowsAffected = 0;
                string SqlCmdQry = "Update IM_ScheduleEmailStatus   set UpdatedDatetime =  @UpdatedDatetime , status = '" + StatusDetail + "', Assessmentname = '" + Assessmentname + "', EmailedTo = '" + EmailedTo + "', Errormessage = '" + Errormessage + "' , Subject = '" + Subject + "'  where id =" + InsertedRecID.ToString();
                if (Assessmentid.Length > 0)
                {

                    using (SqlCommand cmd1 = new SqlCommand(SqlCmdQry, conn))
                    {
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Parameters.AddWithValue("@UpdatedDatetime", CurrentDateTime);
                        conn.Open();

                        rowsAffected = cmd1.ExecuteNonQuery();
                        conn.Close();

                    }

                    WriteToFile_A3MMLog(" Upadate_IM_ScheduleEmailStatus done: Subject : " + Subject + " Status " + StatusDetail + " EmailedTo : " + EmailedTo + " .\n ");

                }
                return rowsAffected;
            }
            catch (Exception ex)
            {
                WriteToFile_A3MMLog("Error While Upadate_A3MM_AssessmentEmailStatus: for the Assessmentid  : " + Assessmentid + " EXCEPTION Details " + ex.Message + " InnerException : " + ex.InnerException + " CallStack " + ex.StackTrace);
                return 0;
            }

        }
    */

        private void WriteToFile_A3MMLog(string text)
        {
            /*
            try
            {

                string date = DateTime.Now.ToShortDateString();
                string filename = "A3MMAssessmentLog_" + System.DateTime.Now.ToString("yyyyMMdd") + ".txt";
                string path = System.Configuration.ConfigurationManager.AppSettings["LogfilePath"] + filename;
                text = text + "  " + System.DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(string.Format(text, DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt")));
                    writer.Close();
                }
                //log.Info(string.Format(text, DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt")));

            }
            catch (Exception ex)
            {
                // log.Info(string.Format(text + ex.Message, DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt")));

            }
*/
        }


        public string SurveyBuilderScheduleCheck()
        {
            string Survey_ID = "not initialted", ClientID = "not initialted", Assessmentname = "not initialted", AssessmentTeam = "not initialted";
            string EmailSubject = "Survey builder Scheduled Email Communication", Participant_Id="0";
            log.Info("SurveyBuilderScheduleCheck: In.. Before Try");
            string SurveyBuilderEmailBody = string.Empty;
            
            string SBScheduledEmailTemplatePathString = ConfigurationManager.AppSettings["SBScheduledEmailTemplatePath"].ToString();
            using (StreamReader reader = new StreamReader(SBScheduledEmailTemplatePathString))
            {
                SurveyBuilderEmailBody = reader.ReadToEnd();
            }
            string qry = " ", ClientName="", SurveyType="", SurveyName="";
            string Contentbody, Recipients="", Errormessage="", Successmessage = "", Surveylink ="", Enddate="";

            try
            {
                string ConString = ConfigurationManager.ConnectionStrings["Db"].ToString(); 
                SqlConnection conn = new SqlConnection(ConString);

                DateTime TodayDate = DateTime.Now;
                //Checking for two reminders
                for (int i = 1; i< 3;i++)
                    {
                    //Select all surveys which are to be send first reminders that is created date + 7 days

                    if (i == 1)
                    {
                        qry = "select distinct  sbc.Survey_ID,CAST(  S.Createddt  AS DATE) Createddt,toolid,SIU.UserSurveyToken as SIUSurveylink,SU.UserSurveyToken as SUSurveylink, s.ClientId, clientname, SurveyType,FirstReminder_Message  ,SecondReminder_Message,s.Enddate  ,SurveyUserEmail as SIUEmailedTo,(select emailaddress from Personnel where id = SU.personnelid) as SUEmailedTo,INTitle,  s.Name as SurveyName "
                                 + "  from SB_ScheduleEmailConfig SBC   left join Clients C on SBC.Clientid = c.id and FirstReminder_Message is not null "
                                 + " left join survey S on SBC.survey_id = S.id "
                                 + " left join surveyinuser SIU on SBC.survey_id = SIU.surveyid and SIU. surveystatus = 'Survey Sent' and S.ToolId = 13 "
                                  + " left join surveyuser SU on SBC.survey_id = SU.surveyid and SU.surveystatus = 'Survey Sent' and S.ToolId <> 13 and SU.UserSurveyToken is not NULL and len(SU.UserSurveyToken) > 19 "
                                  + " left join SB_ScheduleEmailStatus SBS on SBC.survey_id = SBS.survey_id and ( (toolid =13 and SIU.SurveyUserEmail = sbs.EmailedTo )or (toolid <>13 and (select top 1 emailaddress from Personnel where id = SU.personnelid) = sbs.EmailedTo  ) )  and SBS.Status = 'First Reminder Sent' and CONVERT(DATE, SBS.CreatedDatetime) = CONVERT(DATE, getdate()) "
                                 + " where SBS.id is null and ((ToolId <> 13 and DATEDIFF(day, SU.SurveySentDate, getdate()) = 7) or(ToolId <= 13  and DATEDIFF(day, SIU.SurveySentDate, getdate()) = 7))";

                    }
                    else
                    {
                           qry = "select distinct  sbc.Survey_ID,CAST(  S.Createddt  AS DATE) Createddt,toolid,SIU.UserSurveyToken as SIUSurveylink,SU.UserSurveyToken as SUSurveylink, s.ClientId, clientname, SurveyType,SurveyUserEmail as FirstReminder_Message  ,SecondReminder_Message,s.Enddate  ,SurveyUserEmail as SIUEmailedTo, "
                                 + " (select emailaddress from Personnel where id = SU.personnelid) as SUEmailedTo, INTitle,  s.Name as SurveyName "
                                  + " from SB_ScheduleEmailConfig SBC   left join Clients C on SBC.Clientid = c.id and FirstReminder_Message is not null "
                                 + " left join survey S on SBC.survey_id = S.id "
                                 + " left join surveyinuser SIU on SBC.survey_id = SIU.surveyid and SIU. surveystatus = 'Survey Sent' and S.ToolId = 13 "
                                  + " left join surveyuser SU on SBC.survey_id = SU.surveyid and SU.surveystatus = 'Survey Sent' and S.ToolId <> 13 and SU.UserSurveyToken is not NULL and len(SU.UserSurveyToken) > 19 "
                                  + " left join SB_ScheduleEmailStatus SBS on SBC.survey_id = SBS.survey_id and( (toolid =13 and SIU.SurveyUserEmail = sbs.EmailedTo )or (toolid <>13 and (select top 1 emailaddress from Personnel where id = SU.personnelid) = sbs.EmailedTo  ) ) and SBS.Status = 'Second Reminder Sent' and CONVERT(DATE, SBS.CreatedDatetime) = CONVERT(DATE, getdate()) "
                                 + " where SBS.id is null and DATEDIFF(day, getdate(), s.Enddate) = 7";
                    }

                log.Info("SurveyBuilderScheduleCheck : In Try Qry is " + qry.ToString());
                    using (SqlCommand cmd = new SqlCommand(qry, conn))
                    {
                        conn.Open();

                        DateTime SurveyDate;
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            conn.Close();

                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                foreach (DataRow row in dt.Rows)
                                {
                                    Recipients = "";
                                    Survey_ID = row["Survey_ID"].ToString();
                                    ClientID = row["ClientID"].ToString();
                                    ClientName = row["ClientName"].ToString();
                                    SurveyName = row["SurveyName"].ToString();
                                    SurveyType = row["SurveyType"].ToString();
                                    Surveylink = ""; 
                                    Enddate = row["Enddate"].ToString(); 
                                    string toolid= row["toolid"].ToString();
                                    if (i == 1)
                                    {
                                        Contentbody = row["FirstReminder_Message"].ToString();
                                        Successmessage = "First Reminder Sent";
                                    }
                                    else
                                    {
                                        Contentbody = row["SecondReminder_Message"].ToString();
                                        Successmessage = "Second Reminder Sent";
                                    }
                                    SurveyDate = Convert.ToDateTime(row["Createddt"]);
                                    Errormessage = "";
                                    Errormessage = "Reminder for Survey completion for Client " + ClientName + " - Assessmentname " + Assessmentname;
                                    EmailSubject = SurveyName.ToString() + EmailSubject;
                                    log.Info("SurveyBuilderScheduleCheck : Before sending mail  Survey_ID " + Survey_ID.ToString() + " : " + SurveyName.ToString());

                                    Errormessage += " Got Survey details of id :  " + Survey_ID.ToString() + "  name : " + SurveyName.ToString();
                                    //process Analysis report
                                    SBScheduledEmailTemplatePathString = ConfigurationManager.AppSettings["SBScheduledEmailTemplatePath"].ToString();
                                    string body;
                                    using (StreamReader reader = new StreamReader(SBScheduledEmailTemplatePathString))
                                    {
                                        body = reader.ReadToEnd();
                                    }
                                    string checkurl = NonTelerikReporturl.Substring(0, NonTelerikReporturl.IndexOf("Areas"));
                                    if (toolid.CompareTo("13")==0)
                                    {
                                        Surveylink = row["SIUSurveylink"].ToString();

                                        Recipients = row["SIUEmailedTo"].ToString();
                                        Participant_Id = row["SIUEmailedTo"].ToString();

                                    }
                                    else
                                    {
                                        Recipients = row["SUEmailedTo"].ToString();
                                        Participant_Id = row["SUEmailedTo"].ToString();
                                        Surveylink = row["SUSurveylink"].ToString();

                                        /*string RJcommandqry = "select top 1 UserSurveyToken from surveyuser where  surveyid =" + Survey_ID.ToString() + " and Completedt is null and UserSurveyToken is not null and len(UserSurveyToken) > 19  ";
                                        conn.Open();
                                        using (SqlCommand RJcommand = new SqlCommand(RJcommandqry.ToString(), conn))
                                        {
                                           
                                            Surveylink = (string)RJcommand.ExecuteScalar();
                                            Surveylink = checkurl + "ClientProfileManager/SurveyUser/index?nextgensurveyId=" + Survey_ID + "&userSurveyToken=" + Surveylink;
                                        }
                                        */
                                    }
                                    if(Surveylink.Length>0)
                                    Surveylink = checkurl + "ClientProfileManager/SurveyUser/index?nextgensurveyId=" + Survey_ID + "&userSurveyToken=" + Surveylink;

                                    //Surveylink = Uri.Action("index", "ClientProfileManager/SurveyUser", new { nextgensurveyId = Survey_ID, userSurveyToken = Surveylink }, "https");
                                    body = body.Replace("{Link}",  Surveylink);
                                    //body = body.Replace("{UserName}", Participant_Id);
                                    body = body.Replace("{expdate}", Enddate);
                                    body = body.Replace("{clientname}", ClientName);
                                    body = body.Replace("{surveyname}", SurveyName);
                                    body = body.Replace("{EmailContent}", Contentbody);
                                    /* mine
                                    body = body.Replace("{EmailContent}", Contentbody);
                                    if (Recipients != null && Recipients.Length > 1)
                                        body = body.Replace("{ToName}", Recipients.Substring(0, Recipients.IndexOf('@')));
                                    else
                                        body = body.Replace("{ToName}", "");*/

                                    /*
                                                 body = body.Replace("{Link}", Surveylink);
            //body = body.Replace("{UserName}", Participant);
            body = body.Replace("{expdate}", SurvexpDate);
            body = body.Replace("{clientname}", clientname);
            body = body.Replace("{surveyname}", survyname);

                                     * */


                                    EmailSubject = "Reminder for Schedule for survey for Client " + ClientName + " - SurveyName " + SurveyName;
                                    //  log.Info("Final Url - " + FinalUri.ToString());
                                    log.Info("SurveyBuilderScheduleCheck : Sending mail to " + EmailSubject.ToString() + " for " + ClientName.ToString());
                                    try
                                    {
                                        if (Recipients.Length > 5)
                                            SendSurveyBuilderEMail(Recipients, EmailSubject.ToString(), body, "");
                                    
                                    }
                                    catch (Exception ex)
                                    {
                                        Insert_SB_ScheduleEmailStatus(Participant_Id, Survey_ID, ClientID, SurveyName, Recipients, "Error while starting A3MMA3MM_AssessmentScheduleCheck  " + ex.Message + " " + ex.StackTrace, EmailSubject, i.ToString() + Errormessage.ToString() +" Reminder not Sent");
                                        throw new Exception("Error while starting SurveyBuilderScheduleCheck Service; please check the log file for more detail.", ex);
                                    }

                                    Insert_SB_ScheduleEmailStatus(Participant_Id, Survey_ID, ClientID, SurveyName, Recipients, "", EmailSubject, Successmessage);


                                    //}
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                log.Info("SurveyBuilderScheduleCheck : In Catch" + ex.Message + " " + ex.StackTrace);
                throw new Exception("Error while starting SurveyBuilderScheduleCheck Service; please check the log file for more detail.", ex);
            }
            return "";
        }
        public static void SendSurveyBuilderEMail(string EmailedTo, string subject, string body, string PathToAttachment)
        {

            MailMessage mail = new MailMessage();
            String EmailFrom = System.Configuration.ConfigurationManager.AppSettings["EmailFromA3MM"];
            mail.From = new MailAddress(EmailFrom, "Optimization Tool Survey Builder AutoNotification");

            // mail.To
            string[] array = EmailedTo.Split(';');
            foreach (string value in array)
            {
                mail.To.Add(value);
            }

            //  mail.Bcc.Add("deva.prithika@nttdata.com");
            mail.Bcc.Add("Geetha.Bommalata@nttdata.com");
            String SupportMail2Bcc = System.Web.Configuration.WebConfigurationManager.AppSettings["SupportMail2"];
            String Bcc = System.Web.Configuration.WebConfigurationManager.AppSettings["EmailBcc"];
            if (!string.IsNullOrEmpty(Bcc))
            {
                mail.Bcc.Add(Bcc);
            }
            foreach (string value in array)
            {
            //    mail.Bcc.Add(value);
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            String HOST = System.Configuration.ConfigurationManager.AppSettings["smtpHost"];// WebConfigurationManager.AppSettings["smtpHost"];

            int PORT = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["smtpPort"]);// Convert.ToInt16(WebConfigurationManager.AppSettings["smtpPort"]);


            using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(HOST, PORT))
            {

                // Send the email. 
                try
                {

                    String pwd = System.Configuration.ConfigurationManager.AppSettings["smtpUserPwd"];// WebConfigurationManager.AppSettings["smtpUserPwd"];
                    String userid = System.Configuration.ConfigurationManager.AppSettings["smtpUserId"];//WebConfigurationManager.AppSettings["smtpUserId"];
                    String TLS = System.Configuration.ConfigurationManager.AppSettings["smtpTLS"];//WebConfigurationManager.AppSettings["smtpTLS"];
                    if (!string.IsNullOrEmpty(TLS) && TLS == "Yes")
                    {
                        client.EnableSsl = true;
                    }
                    if (!string.IsNullOrEmpty(pwd))
                    {
                        NetworkCredential Credentials = new NetworkCredential(userid, pwd);
                        client.Credentials = Credentials;
                    }
                    client.Send(mail);
                }
                catch (Exception ex)
                {
                    throw new Exception("error sending mail from web server", ex);
                }
            }

        }

        private int Insert_SB_ScheduleEmailStatus(string Participant_Id, string survey_id, string clientid, string SurveyName, string EmailedTo, string Errormessage, string Subject, string Status)
        {
            try
            {
                SqlConnection conn = new SqlConnection(ConString);
                Participant_Id = "0";
                WriteToFile_A3MMLog(" Insert_SB_ScheduleEmailStatus: Subject : " + Subject + " Status " + Status + " EmailedTo : " + EmailedTo + " .\n ");
                int rowsAffected = 0;
                //string SqlCmdQry = "Update SB_ScheduleEmailStatus   set UpdatedDatetime =  @UpdatedDatetime , status = '" + Status + "', SurveyName = '" + SurveyName + "', EmailedTo = '" + EmailedTo + "', Errormessage = '" + Errormessage + "' , Subject = '" + Subject + "'  where Participant_Id =" + Participant_Id.ToString();
                if (survey_id.Length > 0)
                {
                    string CurrentDateTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
                    conn.Open();
                    using (SqlCommand cmd1 = new SqlCommand("insert into SB_ScheduleEmailStatus    (Clientid,survey_id, Participant_Id,EmailedTo,CreatedDatetime,  Status,    Errormessage,ItemName) VALUES  (@clientid,@survey_id,@Participant_Id,@EmailedTo, @CreatedDatetime, @Status,   @Errormessage,@ItemName)", conn))
                    {
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Parameters.AddWithValue("@Participant_Id", Participant_Id);
                        cmd1.Parameters.AddWithValue("@EmailedTo", EmailedTo);
                        cmd1.Parameters.AddWithValue("@clientid", clientid);
                        cmd1.Parameters.AddWithValue("@survey_id", survey_id);
                        cmd1.Parameters.AddWithValue("@Status", Status);
                        cmd1.Parameters.AddWithValue("@ErrorMessage", Errormessage);
                        cmd1.Parameters.AddWithValue("@ItemName", SurveyName); 
                        cmd1.Parameters.AddWithValue("@CreatedDatetime", DateTime.Now); 
                         rowsAffected = cmd1.ExecuteNonQuery();
                    }
                    conn.Close();
                    WriteToFile_A3MMLog(" Insert_SB_ScheduleEmailStatus done: Subject : " + Subject + " Status " + Status + " EmailedTo : " + EmailedTo + " .\n ");

                }
                return rowsAffected;
            }
            catch (Exception ex)
            {
                WriteToFile_A3MMLog("Error While Insert_SB_ScheduleEmailStatus: for the survey_id  : " + survey_id + " EXCEPTION Details " + ex.Message + " InnerException : " + ex.InnerException + " CallStack " + ex.StackTrace);
                return 0;
            }

        }

    }
}
