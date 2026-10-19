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
//using Excel = Microsoft.Office.Interop.Excel;
//using Microsoft.Office.Interop.Excel;

namespace DataMaskingUtility
{
    public partial class DatamaskingInput : Form
    {
        
        DataSet DS;
        DataTable CompleteData;
        public DatamaskingInput()
        {
            InitializeComponent();
        }
        
        

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                DS = new DataSet();
                DS.ReadXml(@"..\RegExStore.xml");
                dataGridView1.DataSource = DS.Tables[0];
                dataGridView1.Refresh();
            }
            catch
            {
                CreateDynamicDataSet();
                dataGridView1.DataSource = DS.Tables[0];
                dataGridView1.Refresh();
            }
        }

  
        private void CreateDynamicDataSet()
        {
           // DataSet ds = new DataSet("DS");
           // ds.Namespace = "RegExNS";
            DataTable stdTable = new DataTable("RegExNS");

            DataColumn col0 = new DataColumn("DataFieldName");
            DataColumn col1 = new DataColumn("DataFieldDesc");
            DataColumn col2 = new DataColumn("RegEx"); 
            DataColumn col3 = new DataColumn("TextValue");
            DataColumn col4 = new DataColumn("ReplacementText");
            stdTable.Columns.Add(col0);
            stdTable.Columns.Add(col1);
            stdTable.Columns.Add(col2);
            stdTable.Columns.Add(col3);
            stdTable.Columns.Add(col4);
            DS.Tables.Add(stdTable);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (DS!=null)
                    DS.WriteXml(@"..\RegExStore.xml");
                MessageBox.Show("Saved successfully.");
                Log("Regular expressions got updated. Data time:" + DateTime.Now.ToString());
            }
            catch(Exception ex)
            {
                string s = ex.Message;
                MessageBox.Show("Error occured while saving data into configuration file.");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(txtName.Text) && !string.IsNullOrWhiteSpace(txtDesc.Text) && !string.IsNullOrWhiteSpace(txtExText.Text) && !string.IsNullOrWhiteSpace(txtRegEx.Text) && !string.IsNullOrWhiteSpace(txtReplace.Text))
            {

                string input = txtExText.Text;
                string pattern = @txtRegEx.Text.Trim();
                var r = "";
                try
                {
                    r = Regex.Replace(input, pattern, "****");
                }
                catch (ArgumentException exx)
                {
                    MessageBox.Show("Invalid Regular Expression!", exx.Message.ToString());
                }
                if (input == r)
                {
                    MessageBox.Show("RegEx is not proper!, Output is same as Input.");
                }
                else if (input != r)
                {
                    DataTable regExtable = DS.Tables[0];
                    DataColumn namcol = regExtable.Columns[0];


                    foreach (DataRow dr in DS.Tables[0].Rows)
                    {

                        if (dr["DataFieldName"].ToString() == txtName.Text)
                        {
                            MessageBox.Show("This item already Exist");
                            return;
                        }
                    }
                    DataRow newRow = regExtable.NewRow();
                    string trimRegex = txtRegEx.Text;
                    trimRegex = trimRegex.Trim();
                    newRow["DataFieldName"] = txtName.Text;
                    newRow["DataFieldDesc"] = txtDesc.Text;
                    newRow["RegEx"] = txtRegEx.Text.Trim();
                    newRow["TextValue"] = txtExText.Text;
                    newRow["ReplacementText"] = txtReplace.Text;
                    regExtable.Rows.Add(newRow);
                    MessageBox.Show("Successfully Added!");
                    txtName.Text = "";
                    txtDesc.Text = "";
                    txtRegEx.Text = "";
                    txtExText.Text = "";
                    txtReplace.Text = "";


                }
            }
            else {
                MessageBox.Show("Fields should not be left empty");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string input = textBox1.Text;
            string pattern = @textBox2.Text.Trim();
            var r = "";
            try
            {
                r = Regex.Replace(input, pattern, "****");
            }
            catch (ArgumentException exxx)
            {
                MessageBox.Show("Invalid Regular Expression!", exxx.Message);
            }
            if (input == r)
            {
                MessageBox.Show("RegEx is not proper!, Output is same as Input.");
            }
            else if (input != r)
            {
                string output = "Input String is : " + input + "\n" + "Masked Output : " + r;
                MessageBox.Show(output);
            }


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
                    if (extension == "xlsx"|| extension == "xls")
                    {
                        srcfolderName = System.IO.Path.GetDirectoryName(path);
                        // System.IO.File.Copy(path, srcfolderName+@"\Masked_"+ filename, true);

                        //string ConStr = "PROVIDER=Microsoft.ACE.OLEDB.12.0;Data Source=" + srcfolderName + @"\Masked_" + filename + ";Extended Properties='Excel 12.0; HDR=Yes;'";
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

                        //  CompleteData.Columns.Add("NttRowNum", typeof(Int32));

                        int i = 0;
                        List<string> columns = new List<string>();
                        foreach (DataRow dr in dtColumns.Rows)
                        {
                            string item = dr[3].ToString();//dr[3]- column name
                            
                            cgklstSrcFlds.Items.Insert(i, item);
                            i++;
                            columns.Add(item);
                        }
                        //CheckedListBox ch = new CheckedListBox();
                        //ch.DataSource = columns;

                        //cgklstSrcFlds.DataSource = columns;
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
            
            delimSelected ="";
            wordList.Clear();
            ReplacementwordText = "";
            if (Wordfilename != null)
            {
                foreach (var wrd in Wordfilename)
                {
                    wordtextFile =wrd;
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
                        var list= wordtext.Split(delimSelected[0]);
                        wordList.AddRange(list);
                        //var list1 = wordList.Select(seq => seq).Where(a => a.Contains(" ")).ToList();
                        //wordList1.AddRange(list1);
                        //var list2 = wordList.Select(seq => seq).Where(a => a.Contains("+") || a.Contains("-")).ToList();
                        //wordList2.AddRange(list2);
                        // wordList = wordtext.Split(new Char[] { delimSelected[0], ' ' });
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
            
             if (extension == "xlsx"|| extension == "xls")
             {
                if (SelectedColumns.Count == 0)
                {
                    MessageBox.Show("Please select the columns which are to be masked!");
                }
                else
                {
                    var plsWait = new PleaseWait();
                    plsWait.Show(this);
                    //lblWait.Text = "Please wait...";
                    // lblWait.Focus();
                    // string ConStr = "PROVIDER=Microsoft.ACE.OLEDB.12.0;Data Source='" + srcfolderName + @"\Masked_" + filename + "';Extended Properties='Excel 12.0; HDR=Yes;'";
                    // OleDbConnection con = new OleDbConnection(ConStr);


                    //DataTable dtTables = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    //string strSheet = dtTables.Rows[0]["TABLE_NAME"].ToString();
                    //DataTable dtColumns = con.GetSchema("Columns", new string[] { null, null, strSheet, null });


                    //con.Close();
                    //OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM [" + strSheet + "]", con);

                    //DataSet dataset = new DataSet();
                    //adapter.Fill(dataset);//, "sheet data");

                    foreach (DataRow _dr in CompleteData.Rows)
                    {
                        foreach (var sc in SelectedColumns)
                        {

                            if (_dr[sc] != null)
                            {
                                string OriginalText = _dr[sc].ToString(); //.Trim();
                                                                          //OriginalText = OriginalText.Replace("'", "''");
                                string maskedText = OriginalText.Replace("'", "''").Trim();
                                
                                //var origWordStr = OriginalText.Replace(",", " ");
                                //origWordStr = OriginalText.Replace("  ", " ");
                                //var srcWords = origWordStr.Split(' ');
                                var srcWords = OriginalText.Split(new Char[] { ',', '\n',' ' },StringSplitOptions.RemoveEmptyEntries);
                                var interWord= srcWords.Select(i => i.ToString()).Intersect(wordList);
                                var interWord2= srcWords.Select(i => i.ToString()).Intersect(wordList2);
                                if(wordList1!=null)
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
                                if(interWord2!=null)
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
                                if (interWord != null) { 
                                    foreach (var word in interWord)
                                    {
                                        var pattern = $@"\b{System.Text.RegularExpressions.Regex.Escape(word)}\b";
                                        try
                                        {
                                            if(word.Length>1)
                                            {
                                                maskedText = System.Text.RegularExpressions.Regex.Replace(maskedText, pattern, ReplacementwordText);
                                                //maskedText = Regex.Replace(maskedText, word.Trim(), ReplacementwordText);
                                             }
                                        }
                                        catch(Exception ex) {}
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
                                    //  string updateSQLStatement = "UPDATE [" + strSheet + "]" + " SET [" + sc + "]='" + maskedText + "' where [" + sc + "]= '" + OriginalText + "'";
                                    //  OleDbCommand cmd = new OleDbCommand(updateSQLStatement, con);
                                    //adapter.UpdateCommand = new OleDbCommand("UPDATE [" + strSheet + "]" + " SET [" + sc + "]='" + maskedText + "' where [" + sc + "]= '" + OriginalText + "'", con);
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
                    //  ExportDataSetToExcel(CompleteData, destPath);
                    //   adapter.Update(dataset);
                    // lblWait.Text = " ";

                    MessageBox.Show("Datamasking completed successfully. Please check the masked file in destination folder.");
                    txtSourceFile.Text = "";
                    txtDestinationFile.Text = "";
                    textWordFile.Text = "";
                    textReplWord.Text = "";
                    delimcomboBox.Text = string.Empty;
                    cgklstSrcFlds.Items.Clear();
                    //   con.Close();
                    plsWait.Close();
                    
                    // con.Dispose();

                    //string destPath = savepath + @"\Masked_" + filename;
                    //if(System.IO.File.Exists(destPath))
                    //{
                    //    destPath= savepath+ @"\Copy_Masked_" + filename;
                    //}
                    //System.IO.File.Move(srcfolderName + @"\Masked_" + filename, destPath);


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

        ////private void ExportDataSetToExcel(DataTable table, string fileName)
        ////{
        ////    //var convertedFilesDirectory = Path.Combine(Environment.CurrentDirectory + @"\Converted Files\");

        ////    //if (!Directory.Exists(convertedFilesDirectory))
        ////    //{
        ////    //    Directory.CreateDirectory(convertedFilesDirectory);
        ////    //}

        ////  //  var convertedFilePath = Path.Combine(convertedFilesDirectory, fileName);

        ////    //Creae an Excel application instance
        ////    Excel.Application excelApp = new Excel.Application();

        ////    // Create Wb
        ////    var wb = excelApp.Workbooks.Add();
        ////    wb.SaveAs(fileName);
        ////    wb.Close();

        ////    //Create an Excel workbook instance and open it from the predefined location
        ////    Excel.Workbook excelWorkBook = excelApp.Workbooks.Open(fileName);

        ////    //foreach (DataTable table in ds.Tables)
        ////    //{
        ////        //Add a new worksheet to workbook with the Datatable name
        ////        Excel.Worksheet excelWorkSheet = excelWorkBook.Sheets.Add();
        ////        excelWorkSheet.Name = strSheet;

        ////        for (int i = 1; i < table.Columns.Count + 1; i++)
        ////        {
        ////            excelWorkSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;
        ////        }

        ////        for (int j = 0; j < table.Rows.Count; j++)
        ////        {
        ////            for (int k = 0; k < table.Columns.Count; k++)
        ////            {
        ////                excelWorkSheet.Cells[j + 2, k + 1] = table.Rows[j].ItemArray[k].ToString();
        ////            }
        ////        }
        ////    //}

        ////    excelWorkBook.Save();
        ////    excelWorkBook.Close();
        ////    excelApp.Quit();

        ////}

        ////private void ExportDataSetToExcel(DataSet ds, string destfile)
        ////{
        ////    //Creae an Excel application instance
        ////    Excel.Application excelApp = new Excel.Application();

        ////    //Create an Excel workbook instance and open it from the predefined location
        ////    Excel.Workbook excelWorkBook = excelApp.Workbooks.Open(destfile);

        ////    foreach (DataTable table in ds.Tables)
        ////    {
        ////        //Add a new worksheet to workbook with the Datatable name
        ////        Excel.Worksheet excelWorkSheet = excelWorkBook.Sheets.Add();
        ////        excelWorkSheet.Name = table.TableName;

        ////        for (int i = 1; i < table.Columns.Count + 1; i++)
        ////        {
        ////            excelWorkSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;
        ////        }

        ////        for (int j = 0; j < table.Rows.Count; j++)
        ////        {
        ////            for (int k = 0; k < table.Columns.Count; k++)
        ////            {
        ////                excelWorkSheet.Cells[j + 2, k + 1] = table.Rows[j].ItemArray[k].ToString();
        ////            }
        ////        }
        ////    }

        ////    excelWorkBook.Save();
        ////    excelWorkBook.Close();
        ////    excelApp.Quit();

        ////}


        //Destination Folder
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

        private void txtBox_Validate(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                //e.Cancel = true;
                //txtName.Focus();
                errorProvider1.SetError(txtName, "Name should not be left blank!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtName, "");
            }
        }

        private void txtExTest_validate(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtExText.Text))
            {
                //e.Cancel = true;
                //txtExText.Focus();
                errorProvider3.SetError(txtExText, "Example Text should not be left blank!");
            }
            else
            {
                e.Cancel = false;
                errorProvider3.SetError(txtExText, "");
            }
        }

        private void txtBoxDesc_validate(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDesc.Text))
            {
                //e.Cancel = true;
                //txtDesc.Focus();
                errorProvider2.SetError(txtDesc, "Description should not be left blank!");
            }
            else
            {
                e.Cancel = false;
                errorProvider2.SetError(txtDesc, "");
            }
        }

        private void txtBoxRegEx_validate(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRegEx.Text))
            {
                //e.Cancel = true;
                //txtRegEx.Focus();
                errorProvider5.SetError(txtRegEx, "Name should not be left blank!");
            }
            else
            {
                e.Cancel = false;
                errorProvider5.SetError(txtRegEx, "");
            }
        }

        private void txtBoxReplace_Validate(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtReplace.Text))
            {
                //e.Cancel = true;
                //txtReplace.Focus();
                errorProvider4.SetError(txtReplace, "Name should not be left blank!");
            }
            else
            {
                e.Cancel = false;
                errorProvider4.SetError(txtReplace, "");
            }
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            txtName.Text= "";
            txtDesc.Text = "";
            txtExText.Text = "";
            txtReplace.Text = "";
            txtRegEx.Text = "";
           // txtName.Focus();
        }

        private void txtRegEx_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void DatamaskingInput_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
           // lblWait.Visible = false;
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
                        textWordFile.Text = textWordFile.Text+";"+ path1;
                        Wordfilename.Add(path1);
                    }
                    plsWait.Close();
                      
                }

            }

        }

        //private static string ReplaceWord(this string source, string wordToReplace, string replacementWord
        //            , System.Text.RegularExpressions.RegexOptions regexOptions = System.Text.RegularExpressions.RegexOptions.None)
        //{
        //    var pattern = $@"\b{System.Text.RegularExpressions.Regex.Escape(wordToReplace)}\b";
        //    var result = System.Text.RegularExpressions.Regex.Replace(source, pattern, replacementWord, regexOptions);
        //    return result;
        //}
    }
    public static class Extensions
    {
        /// <summary>
        /// Replaces the entire word in the string
        /// </summary>
        /// <param name="source">The source string</param>
        /// <param name="wordToReplace">The word to replace</param>
        /// <param name="replacementWord">The replacement word</param>
        /// <param name="regexOptions">The search options</param>
        /// <returns>The modified source string</returns>
        public static string ReplaceWord(this string source, string wordToReplace, string replacementWord
                , System.Text.RegularExpressions.RegexOptions regexOptions = System.Text.RegularExpressions.RegexOptions.None)
        {
            var pattern = $@"\b{System.Text.RegularExpressions.Regex.Escape(wordToReplace)}\b";
            var result = System.Text.RegularExpressions.Regex.Replace(source, pattern, replacementWord, regexOptions);
            return result;
        }
    }
}
