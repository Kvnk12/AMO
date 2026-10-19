using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Drawing.Text;
using System.Runtime.InteropServices;

namespace DataMaskingUtility
{
    public partial class DataFileMask : Form
    {
        DataSet DS;
        DataTable CompleteData;
        public DataFileMask()
        {
            InitializeComponent();
        }

        private void Form3_Paint_1(object sender, PaintEventArgs e)
        {
            IntPtr ptr = NativeMethods.CreateRoundRectRgn(10, 10, this.Width, this.Height, 40, 40); // _BoarderRaduis can be adjusted to your needs, try 15 to start.
            this.Region = System.Drawing.Region.FromHrgn(ptr);
            NativeMethods.DeleteObject(ptr);
        }

        string strSheet = "";
        string path = "";
        string path1 = "";
        string filename = "";
        //string Wordfilename = "";
        string srcfolderName = "";
        string extension = "";
        private void button3_Click(object sender, EventArgs e)
        {
            cgklstSrcFlds.Items.Clear();
            CompleteData = new DataTable();
            using (OpenFileDialog dialog = new OpenFileDialog() { Filter = "Excel Workbook|*.xlsx|Worksheet|*.xls|Text|*.txt", ValidateNames = true })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var plsWait = new PleaseWait();
                    plsWait.Show(this);
                    IList<string> ne = new List<string>();
                    path = dialog.FileName;
                    txtSourceFile.Text = path;
                    filename = System.IO.Path.GetFileName(path);
                    extension = filename.Substring(filename.LastIndexOf('.') + 1);
                    if (extension == "xlsx" || extension == "xls")
                    {
                        srcfolderName = System.IO.Path.GetDirectoryName(path);
                        string ConStr = "PROVIDER=Microsoft.ACE.OLEDB.12.0;Data Source=" + txtSourceFile.Text + ";Extended Properties='Excel 12.0; HDR=Yes;'";
                        OleDbConnection con = new OleDbConnection(ConStr);
                        con.Open();
                        DataTable dtTables = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        strSheet = dtTables.Rows[0]["TABLE_NAME"].ToString();
                        DataTable dtColumns = con.GetSchema("Columns", new string[] { null, null, strSheet, null });
                        OleDbCommand cmd = new OleDbCommand("select * from [" + strSheet + "]", con);
                        CompleteData.Load(cmd.ExecuteReader());
                        con.Close();
                        con.Dispose();
                        int i = 0;
                        List<string> columns = new List<string>();
                        foreach (DataRow dr in dtColumns.Rows)
                        {
                            string item = dr[3].ToString();//dr[3]- column name

                            cgklstSrcFlds.Items.Insert(i, item);
                            i++;
                            columns.Add(item);
                        }                  
                        cgklstSrcFlds.BackColor = Color.White;
                        cgklstSrcFlds.Enabled = true;
                        plsWait.Close();
                    }
                    else if (extension == "txt")
                    {
                        cgklstSrcFlds.Items.Clear();
                        cgklstSrcFlds.BackColor = Color.LightGray;
                        plsWait.Close();
                        //Implement data masking for text file
                    }
                }

            }
        }
        //Destination path variable
        string savepath = "";
        string wordtextFile = "";
        string wordtext = "";
        string delimSelected = "";
        string ReplacementwordText = "";
        List<string> wordList = new List<string>();

        private void Form3_Load(object sender, EventArgs e)
        {
            DS = new DataSet();
            DS.ReadXml(@"..\RegExStore.xml");
        }

        private void mask_data_Click(object sender, EventArgs e)
        {
            List<string> wordList1 = new List<string>();
            List<string> wordList2 = new List<string>();
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("Please select the Source file");
                return;
            }
            else if (string.IsNullOrEmpty(savepath))
            {
                MessageBox.Show("Please select the destination Folder!");
                return;
            }

            delimSelected = "";
            wordList.Clear();
            ReplacementwordText = "";
            if (Wordfilename != null)
            {
                foreach (var wrd in Wordfilename)
                {
                    wordtextFile = wrd;
                    if (wordtextFile != "")
                    {
                        wordtext = File.ReadAllText(wordtextFile);
                        delimSelected = delimcomboBox.Text;
                        ReplacementwordText = textReplWord.Text.ToString();
                        if (string.IsNullOrEmpty(delimSelected))
                        {
                            MessageBox.Show("Please select Delimiter");
                            return;
                        }
                        else if (string.IsNullOrEmpty(ReplacementwordText))
                        {
                            MessageBox.Show("Please enter the Replacement text");
                            return;
                        }
                        var list = wordtext.Split(delimSelected[0]);
                        wordList.AddRange(list);
                    }
                }
                wordList1 = wordList.Select(seq => seq).Where(a => a.Contains(" ")).ToList();
                wordList2 = wordList.Select(seq => seq).Where(a => a.Contains("+") || a.Contains("-")).ToList();
            }
            List<string> SelectedColumns = new List<string>();
            foreach (string s in cgklstSrcFlds.CheckedItems)
            {
                //MessageBox.Show(s);
                SelectedColumns.Add(s);
            }
            if (MessageBox.Show("Data masking may take several minutes depending upon number of records/data elements to be masked. Press OK to continue.", "Datamasking", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }

            if (extension == "xlsx" || extension == "xls")
            {
                if (SelectedColumns.Count == 0)
                {
                    MessageBox.Show("Please select the columns which are to be masked!");
                }
                else
                {
                    var plsWait = new PleaseWait();
                    plsWait.Show(this);
                    foreach (DataRow _dr in CompleteData.Rows)
                    {
                        foreach (var sc in SelectedColumns)
                        {
                            if (_dr[sc] != null)
                            {
                                string OriginalText = _dr[sc].ToString(); //.Trim();
                                                                          //OriginalText = OriginalText.Replace("'", "''");
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
                                                    maskedText = Regex.Replace(maskedText, word.Trim(), ReplacementwordText);
                                                else
                                                    maskedText = maskedText.Replace(word, ReplacementwordText);
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
                                                maskedText = maskedText.Replace(word, ReplacementwordText);
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
                                                maskedText = System.Text.RegularExpressions.Regex.Replace(maskedText, pattern, ReplacementwordText);
                                                //maskedText = Regex.Replace(maskedText, word.Trim(), ReplacementwordText);
                                            }
                                        }
                                        catch (Exception ex) { }
                                    }
                                }
                                //for all patterns
                                foreach (DataRow patternDR in DS.Tables[0].Rows)
                                {
                                    string pattern = patternDR["RegEx"].ToString();
                                    string ReplacementText = patternDR["ReplacementText"].ToString();
                                    maskedText = Regex.Replace(maskedText, pattern, ReplacementText);
                                }
                                if (OriginalText != maskedText)
                                {                                   
                                    try
                                    {
                                        _dr[sc] = maskedText;
                                        // int i = cmd.ExecuteNonQuery();
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
                    string destPath = savepath + @"\Masked_" + filename;
                    DataSet dataset = new DataSet();
                    CompleteData.TableName = strSheet.Replace("$", "");
                    dataset.Tables.Add(CompleteData);
                    bool x = CreateExcelFile.CreateExcelDocument(dataset, destPath);
                    MessageBox.Show("Datamasking completed successfully. Please check the masked file in destination folder.");
                    txtSourceFile.Text = "";
                    txtDestinationFile.Text = "";
                    textWordFile.Text = "";
                    textReplWord.Text = "";
                    delimcomboBox.Text = string.Empty;
                    cgklstSrcFlds.Items.Clear();
                    // con.Close();
                    plsWait.Close();                   
                }
            }
            else if (extension == "txt")
            {
                var plsWait = new PleaseWait();
                plsWait.Show(this);
                string textFile = @txtSourceFile.Text;
                string text = File.ReadAllText(textFile);
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
                                    maskedText = Regex.Replace(maskedText, word.Trim(), ReplacementwordText);
                                else
                                    maskedText = maskedText.Replace(word, ReplacementwordText);
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
                                maskedText = maskedText.Replace(word, ReplacementwordText);
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
                                maskedText = System.Text.RegularExpressions.Regex.Replace(maskedText, pattern, ReplacementwordText);
                                //maskedText = Regex.Replace(maskedText, word.Trim(), ReplacementwordText);
                            }
                        }
                        catch (Exception ex) { }
                    }
                }
                //for all patterns
                foreach (DataRow patternDR in DS.Tables[0].Rows)
                {
                    string pattern = patternDR["RegEx"].ToString();
                    string ReplacementText = patternDR["ReplacementText"].ToString();
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
                string destPath = savepath + @"\Masked_" + filename;
                using (StreamWriter writer = File.CreateText(destPath))
                {
                    writer.WriteLine("");
                }
                File.WriteAllText(destPath, maskedText);
                MessageBox.Show("Datamasking completed successfully. Please check the masked file in destination folder.");
                txtSourceFile.Text = "";
                txtDestinationFile.Text = "";
                cgklstSrcFlds.Items.Clear();
                plsWait.Close();
                textWordFile.Text = "";
                textReplWord.Text = "";
                delimcomboBox.Text = string.Empty;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            var folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                savepath = folderDialog.SelectedPath;
                txtDestinationFile.Text = savepath;
                // folderDialog.SelectedPath -- your result
            }
        }

        List<string> Wordfilename = new List<string>();
        private void button7_Click(object sender, EventArgs e)
        {
            textWordFile.Text = "";
            using (OpenFileDialog dialog = new OpenFileDialog() { Filter = "Text|*.txt", ValidateNames = true, Multiselect = true })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var plsWait = new PleaseWait();
                    plsWait.Show(this);
                    foreach (String file in dialog.FileNames)
                    {
                        IList<string> ne = new List<string>();
                        path1 = file;
                        textWordFile.Text = textWordFile.Text + ";" + path1;
                        Wordfilename.Add(path1);
                    }
                    plsWait.Close();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void closebutton3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void minimizebutton1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void maximizebutton2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void label15_Click(object sender, EventArgs e)
        {
            var inputform1 = new Configuration();
            inputform1.Height = 650;
            inputform1.Width = 1200;
            inputform1.Show();
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            var inputform1 = new Configuration();
            inputform1.Height = 650;
            inputform1.Width = 1200;
            inputform1.Show();
            this.Hide();
        }
    }

    //public static class Extensions
    //{
    //    /// <summary>
    //    /// Replaces the entire word in the string
    //    /// </summary>
    //    /// <param name="source">The source string</param>
    //    /// <param name="wordToReplace">The word to replace</param>
    //    /// <param name="replacementWord">The replacement word</param>
    //    /// <param name="regexOptions">The search options</param>
    //    /// <returns>The modified source string</returns>
    //    public static string ReplaceWord(this string source, string wordToReplace, string replacementWord
    //            , System.Text.RegularExpressions.RegexOptions regexOptions = System.Text.RegularExpressions.RegexOptions.None)
    //    {
    //        var pattern = $@"\b{System.Text.RegularExpressions.Regex.Escape(wordToReplace)}\b";
    //        var result = System.Text.RegularExpressions.Regex.Replace(source, pattern, replacementWord, regexOptions);
    //        return result;
    //    }
    //}


}
