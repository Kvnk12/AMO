using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using WinSCP;
using System.Configuration;
using System.IO;
using DataMaskingLib;
using System.Linq;

namespace MaskAndSFTP
{
    class Program
    {
        static string UniQId = "";
        static void Main(string[] args)
        {
            UniQId = Guid.NewGuid().ToString("N");
            string sourcepath = "";
            string destinationFileNameFormat = "";
            string archivefilepath = "";
            string wordfilepath = "";
            string wordDelimiter = "";
            string wordCharToRepl = "";
             
            try
            {
                string logmsg = "Datamasking utility initiated; instance Id: " + UniQId + " Date time:" + DateTime.Now.ToString();
                Log(logmsg);
                Console.WriteLine(logmsg);
                string sftpServer = ConfigurationManager.AppSettings["nttnextgensftpserver"];
                string sftpuser = ConfigurationManager.AppSettings["sftpuserid"];
                string sftppwd = ConfigurationManager.AppSettings["sftppwd"];
                sourcepath = ConfigurationManager.AppSettings["sourcefilepath"];
                archivefilepath = ConfigurationManager.AppSettings["archivefilepath"];
                wordfilepath = ConfigurationManager.AppSettings["pathOfwordfiles"];
                wordDelimiter = ConfigurationManager.AppSettings["wordDelimiter"];
                wordCharToRepl = ConfigurationManager.AppSettings["wordCharToRepl"];

                string columnsToMask = ConfigurationManager.AppSettings["columnsToMask"];
                destinationFileNameFormat = ConfigurationManager.AppSettings["nextgentemplateId"];
                if (args.Length > 0)
                {
                    //source file path(folder only)
                    if (!string.IsNullOrEmpty(args[0]))
                    {
                        sourcepath = args[0];
                    }
                    //masked columns
                    if (!string.IsNullOrEmpty(args[1]))
                    {
                        columnsToMask = args[1];
                    }
                    //masked columns
                    if (!string.IsNullOrEmpty(args[2]))
                    {
                        archivefilepath = args[2];
                    }
                }

                //mask all input files and copy it to source folder
                ProcessDirectory(sourcepath, columnsToMask, wordfilepath, wordDelimiter, wordCharToRepl,destinationFileNameFormat);
                // Setup session options

                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Sftp,
                    HostName = sftpServer,
                    UserName = sftpuser,
                    Password = sftppwd,
                    GiveUpSecurityAndAcceptAnySshHostKey = true
                    // SshHostKeyFingerprint = "ssh-rsa 256 Y8rVpQ+UUj5X3qRTRTWjOW2bJa5+psUo3/aQ61MsOmE"
                };

                using (Session session = new Session())
                {
                    // Connect
                    if (!session.Opened)
                        session.Open(sessionOptions);
                    string log = "Instance Id: " + UniQId +  "-NTT data NextGen Tool sftp connection established. Host:" + sessionOptions.HostName +  " UserName:" + sessionOptions.UserName +  " Date time: " + DateTime.Now.ToString();
                    Log(log);
                    Console.WriteLine(log);
                    // Console.WriteLine("NTT data NextGenTool sftp connection established");
                    // Upload files
                   // Console.WriteLine("Masked file being transferred to NextGenTool.");
                    log = "Instance Id: " + UniQId + "-Masked file(s) being transferred to NextGen Tool. Date time: " + DateTime.Now.ToString();
                    Log(logmsg);
                    Console.WriteLine(log);
                    session.PutFiles(sourcepath + destinationFileNameFormat + @"*.xlsx", "./");
                    session.PutFiles(sourcepath + @"\*_masked.xlsx", "./");
                    Console.WriteLine("SFTP file transfer completed.");
                    log = "Instance Id: " + UniQId + "-SFTP file transfer completed. Date time: " + DateTime.Now.ToString();
                    Log(log);
                    Console.WriteLine(log);
                    if (session.Opened)
                        session.Close();
                }
               if (!Directory.Exists(archivefilepath))
                {
                    Directory.CreateDirectory(archivefilepath);
                }
                try
                {
                    /// File.Copy(sourcepath + @"\*_masked.xlsx", sourcepath + @"\archive\*.*");
                    string[] fileEntries = Directory.GetFiles(sourcepath, "*.xlsx");
                    foreach (string filename in fileEntries)
                    {
                        File.Move(filename, archivefilepath+@"\"+ filename.Substring(filename.LastIndexOf(@"\")+1));
                    }
                    string log = "Instance Id: " + UniQId + "-Source file and masked files are archived. Date time: " + DateTime.Now.ToString();
                    Log(log);
                    Console.WriteLine(log);
                    // Directory.Move(sourcepath , archivefilepath);
                }
                catch (Exception exx)
                {
                    //string str = exx.Message;
                    string log = "Instance Id: " + UniQId + "-Error occured while archiving."+ exx.Message +" Date time: " + DateTime.Now.ToString();
                    Log(log);
                    Console.WriteLine(log);
                }
            }
            catch (Exception ex)
            {
                string log = "Instance Id: " + UniQId + "-Error occured." + ex.Message + " Date time: " + DateTime.Now.ToString();
                Log(log);
                Console.WriteLine(log);
                try
                {
                    // Directory.Move(sourcepath, archivefilepath);
                    string[] fileEntries = Directory.GetFiles(sourcepath, "*.xlsx");
                    foreach (string filename in fileEntries)
                    {
                        File.Move(filename, archivefilepath + @"\" + filename.Substring(filename.LastIndexOf(@"\") + 1));
                    }
                }
                catch(Exception exx)
                {
                    //string str = exx.Message;
                    log = "Instance Id: " + UniQId + "-Error occured while archiving." + exx.Message + " Date time: " + DateTime.Now.ToString();
                    Log(log);
                    Console.WriteLine(log);
                }
            }
        }
        private static void Log(string Logg)
        {
            string path = @"..\Logs.txt";
            if (!File.Exists(path))
            {
                string createText = "LOGS." + Environment.NewLine;
                File.WriteAllText(path, createText);
            }

            string appendText = Logg + Environment.NewLine;
            File.AppendAllText(path, appendText);

        }
        
        public static void ProcessDirectory(string targetDirectory, string columnnames,string wordfilepath,string wordDelimiter,string wordCharToRepl, string destinationFileNameFormat)
        {
            string wordtextFile = "";
            string wordtext = "";
            List<string> wordList = new List<string>();
            List<string> wordList1 = new List<string>();
            List<string> wordList2 = new List<string>();
            // Process the list of files found in the directory.
            var Wordfilename = wordfilepath.Split(';');
            if (Wordfilename != null)
            {
                foreach (var wrd in Wordfilename)
                {
                    wordtextFile = wrd;
                    if (wordtextFile != "")
                    {
                        wordtext = File.ReadAllText(wordtextFile);
                        var list = wordtext.Split(wordDelimiter[0]);
                        wordList.AddRange(list);
                    }
                }
                wordList1 = wordList.Select(seq => seq).Where(a => a.Contains(" ")).ToList();
                wordList2 = wordList.Select(seq => seq).Where(a => a.Contains("+") || a.Contains("-")).ToList();
            }

            string[] fileEntries = Directory.GetFiles(targetDirectory, "*.xlsx");
            DataSet DS = new DataSet();
            DS.ReadXml(@"..\RegExStore.xml");
            List<RegExModel> regExList = new List<RegExModel>();
            foreach(DataRow dr in DS.Tables[0].Rows)
            {
                RegExModel regExmodel = new RegExModel();
                regExmodel.RegEx = dr["RegEx"].ToString();
                regExmodel.ReplacementText = dr["ReplacementText"].ToString();
                regExmodel.TextValue = dr["TextValue"].ToString();
                regExmodel.DataFieldName = dr["DataFieldName"].ToString();
                regExmodel.DataFieldDesc = dr["DataFieldDesc"].ToString();
                regExList.Add(regExmodel);
            }
            foreach (string fileName in fileEntries)
            {
                //Console.WriteLine(fileName.Substring(0, fileName.LastIndexOf(@"\")+1) + destinationFileNameFormat + ".xlsx");
                Console.WriteLine("Processing data masking'"+ fileName +"' .....");
                string log = "Instance Id: " + UniQId + "-Data masking is inprogress for'" + fileName + "'. Date time: " + DateTime.Now.ToString();
                Log(log);
                Console.WriteLine(log);
                MaskXLFile.MaskXLInputXLFile(fileName, columnnames, regExList, destinationFileNameFormat,wordList,wordList1,wordList2, wordCharToRepl);
                //Console.WriteLine("Masking completed for '" + fileName + "' .....");
                log = "Instance Id: " + UniQId + "-Data masking is completed for '" + fileName + "'. Date time: " + DateTime.Now.ToString();
                Log(log);
                Console.WriteLine(log);
            }

            string[] fileEntries1 = Directory.GetFiles(targetDirectory, "*.xls");
            foreach (string fileName in fileEntries1)
            {
                //Console.WriteLine(fileName.Substring(0, fileName.LastIndexOf(@"\")+1) + destinationFileNameFormat + ".xlsx");
                Console.WriteLine("Processing data masking'" + fileName + "' .....");
                string log = "Instance Id: " + UniQId + "-Data masking is inprogress for'" + fileName + "'. Date time: " + DateTime.Now.ToString();
                Log(log);
                Console.WriteLine(log);
                MaskXLFile.MaskXLInputXLFile(fileName, columnnames, regExList, destinationFileNameFormat, wordList, wordList1, wordList2, wordCharToRepl);
                //Console.WriteLine("Masking completed for '" + fileName + "' .....");
                log = "Instance Id: " + UniQId + "-Data masking is completed for '" + fileName + "'. Date time: " + DateTime.Now.ToString();
                Log(log);
                Console.WriteLine(log);
            }

            string[] fileEntries2 = Directory.GetFiles(targetDirectory, "*.txt");
            foreach (string fileName in fileEntries2)
            {
                //Console.WriteLine(fileName.Substring(0, fileName.LastIndexOf(@"\")+1) + destinationFileNameFormat + ".xlsx");
                Console.WriteLine("Processing data masking'" + fileName + "' .....");
                string log = "Instance Id: " + UniQId + "-Data masking is inprogress for'" + fileName + "'. Date time: " + DateTime.Now.ToString();
                Log(log);
                Console.WriteLine(log);
                MaskXLFile.MaskTXTInputTXTFile(fileName, regExList, destinationFileNameFormat, wordList, wordList1, wordList2, wordCharToRepl);
                //Console.WriteLine("Masking completed for '" + fileName + "' .....");
                log = "Instance Id: " + UniQId + "-Data masking is completed for '" + fileName + "'. Date time: " + DateTime.Now.ToString();
                Log(log);
                Console.WriteLine(log);
            }
        }
    }
}
