using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
//using System.IO;
using System.Text.RegularExpressions;
using System.IO;

namespace DataMaskingLib
{
    //nothingssd 
    public class MaskXLFile
    {
        public static bool MaskXLInputXLFile(string  filepath, string columns, List<RegExModel> RegModelList, string destinationFileNameFormat,List<string> wordList, List<string> wordList1, List<string> wordList2,string wordCharToRepl)
        {

            DataTable CompleteData = new DataTable();
            bool blnReturn = false;
                if (!string.IsNullOrEmpty(filepath))
                {
                    
                    IList<string> ne = new List<string>();
                    string ConStr = "PROVIDER=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath + ";Extended Properties='Excel 12.0; HDR=Yes;'";

                    OleDbConnection con = new OleDbConnection(ConStr);
                    con.Open();
                    DataTable dtTables = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    string strSheet = dtTables.Rows[0]["TABLE_NAME"].ToString();
                //  DataTable dtColumns = con.GetSchema("Columns", new string[] { null, null, strSheet, null });
                    OleDbCommand cmd = new OleDbCommand("select * from [" + strSheet + "]", con);
                    CompleteData.Load(cmd.ExecuteReader());

                    con.Close();
                    con.Dispose();

                    string[] columnlst = columns.Split(',');

                DataSet ds = new DataSet();
                DataTable maskedTable = MaskRecords(CompleteData, columnlst, RegModelList,  wordList,  wordList1, wordList2, wordCharToRepl);
                maskedTable.TableName = strSheet.Replace("$", "");
                ds.Tables.Add(maskedTable);
                string destpath = "";
                if (string.IsNullOrEmpty(destinationFileNameFormat))
                {
                    destpath = filepath.Substring(0, filepath.LastIndexOf(@".xlsx"))+"_masked.xlsx";
                }
                else
                {
                    destpath = filepath.Substring(0, filepath.LastIndexOf(@"\")+1) + destinationFileNameFormat+ DateTime.Now.ToString("MMddyyyyhhmmss") + ".xlsx";
                }
                blnReturn = CreateExcelFile.CreateExcelDocument(ds, destpath);
                                
            }
            return blnReturn;
        }
        
        private static DataTable MaskRecords(DataTable CompleteData,  string[] columnlst, List<RegExModel> RegModelList, List<string> wordList, List<string> wordList1, List<string> wordList2,string wordCharToRepl)
        {
            foreach (DataRow _dr in CompleteData.Rows)
            {
                foreach (var sc in columnlst)
                {
                    if (_dr[sc] != null)
                    {
                        string OriginalText = _dr[sc].ToString(); //.Trim();
                        string maskedText = OriginalText.Replace("'", "''").Trim();

                        var srcWords = OriginalText.Split(new Char[] { ',', '\n', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        var interWord = srcWords.Select(i => i.ToString()).Intersect(wordList);
                        var interWord2 = srcWords.Select(i => i.ToString()).Intersect(wordList2);
                        if (wordList1 != null)
                            foreach (var word in wordList1)
                            {
                                try
                                {
                                    if (word.Length > 1)
                                    {
                                        if (!(word.Contains("+") || word.Contains("-")))
                                            maskedText = Regex.Replace(maskedText, word.Trim(), wordCharToRepl);
                                        else
                                            maskedText = maskedText.Replace(word, wordCharToRepl);
                                    }
                                }
                                catch (Exception ex) { }
                            }
                        if (interWord2 != null)
                            foreach (var word in interWord2)
                            {
                                try
                                {
                                    if (word.Length > 1)
                                    {
                                        maskedText = maskedText.Replace(word, wordCharToRepl);
                                    }
                                }
                                catch (Exception ex) { }
                            }
                        if (interWord != null)
                        {
                            foreach (var word in interWord)
                            {
                                var pattern = $@"\b{System.Text.RegularExpressions.Regex.Escape(word)}\b";
                                try
                                {
                                    if (word.Length > 1)
                                    {
                                        maskedText = System.Text.RegularExpressions.Regex.Replace(maskedText, pattern, wordCharToRepl);
                                        //maskedText = Regex.Replace(maskedText, word.Trim(), ReplacementwordText);
                                    }
                                }
                                catch (Exception ex) { }
                            }
                        }

                        //for all patterns
                        foreach (var patternDR in RegModelList)
                        {
                            string pattern = patternDR.RegEx;// patternDR["RegEx"].ToString();
                            string ReplacementText = patternDR.ReplacementText; // ["ReplacementText"].ToString();
                            maskedText = Regex.Replace(maskedText, pattern, ReplacementText);
                        }
                        if (OriginalText != maskedText)
                        {
                            try
                            {
                                _dr[sc] = maskedText;
                                OriginalText = maskedText;
                            }
                            catch (Exception ex)
                            {
                                // MessageBox.Show("Error while masking.");
                                // OriginalText = maskedText;
                            }

                        }
                    }
                }
            }

            return CompleteData;
        }
        public static bool MaskTXTInputTXTFile(string  filepath, List<RegExModel> RegModelList, string destinationFileNameFormat,List<string> wordList, List<string> wordList1, List<string> wordList2,string wordCharToRepl)
        {
            string text = File.ReadAllText(filepath);
            if (!string.IsNullOrEmpty(filepath))
                {
                    
                    IList<string> ne = new List<string>();
                    string ConStr = "PROVIDER=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath + ";Extended Properties='Excel 12.0; HDR=Yes;'";

                string maskedTxt = MaskTXTRecords(text,RegModelList,  wordList,  wordList1, wordList2, wordCharToRepl);
                
                string destpath = "";
                if (string.IsNullOrEmpty(destinationFileNameFormat))
                {
                    destpath = filepath.Substring(0, filepath.LastIndexOf(@".txt"))+"_masked.txt";
                }
                else
                {
                    destpath = filepath.Substring(0, filepath.LastIndexOf(@"\")+1) + destinationFileNameFormat+ DateTime.Now.ToString("MMddyyyyhhmmss") + ".txt";
                }
                using (StreamWriter writer = File.CreateText(destinationFileNameFormat))
                {
                    writer.WriteLine("");
                }
                File.WriteAllText(destinationFileNameFormat, maskedTxt);
            }
            return true;
        }
        
        private static string MaskTXTRecords(string text, List<RegExModel> RegModelList, List<string> wordList, List<string> wordList1, List<string> wordList2,string wordCharToRepl)
        {
           
            string OriginalText = text;
            string maskedText = OriginalText.Replace("'", "''").Trim();

            var srcWords = OriginalText.Split(new Char[] { ',', '\n', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var interWord = srcWords.Select(i => i.ToString()).Intersect(wordList);
            var interWord2 = srcWords.Select(i => i.ToString()).Intersect(wordList2);
            if (wordList1 != null)
                foreach (var word in wordList1)
                {
                    try
                    {
                        if (word.Length > 1)
                        {
                            if (!(word.Contains("+") || word.Contains("-")))
                                maskedText = Regex.Replace(maskedText, word.Trim(), wordCharToRepl);
                            else
                                maskedText = maskedText.Replace(word, wordCharToRepl);
                        }
                    }
                    catch (Exception ex) { }
                }
            if (interWord2 != null)
                foreach (var word in interWord2)
                {
                    try
                    {
                        if (word.Length > 1)
                        {
                            maskedText = maskedText.Replace(word, wordCharToRepl);
                        }
                    }
                    catch (Exception ex) { }
                }
            if (interWord != null)
            {
                foreach (var word in interWord)
                {
                    var pattern = $@"\b{System.Text.RegularExpressions.Regex.Escape(word)}\b";
                    try
                    {
                        if (word.Length > 1)
                        {
                            maskedText = System.Text.RegularExpressions.Regex.Replace(maskedText, pattern, wordCharToRepl);
                            //maskedText = Regex.Replace(maskedText, word.Trim(), ReplacementwordText);
                        }
                    }
                    catch (Exception ex) { }
                }
            }
            //for all patterns
            foreach (var patternDR in RegModelList)
            {
                string pattern = patternDR.RegEx;// patternDR["RegEx"].ToString();
                string ReplacementText = patternDR.ReplacementText; // ["ReplacementText"].ToString();
                maskedText = Regex.Replace(maskedText, pattern, ReplacementText);
            }
            if (OriginalText != maskedText)
            {
                try
                {
                    OriginalText = maskedText;
                }
                catch (Exception ex)
                {
                }

            }
            return maskedText;
        }

    }
}
