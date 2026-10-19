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
    public partial class Configuration : Form
    {
        DataSet DS;
        
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn1
       (
           int nLeftRect,     // x-coordinate of upper-left corner
           int nTopRect,      // y-coordinate of upper-left corner
           int nRightRect,    // x-coordinate of lower-right corner
           int nBottomRect,   // y-coordinate of lower-right corner
           int nWidthEllipse, // height of ellipse
           int nHeightEllipse // width of ellipse
       );
        public Configuration()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            IntPtr ptr = NativeMethods.CreateRoundRectRgn(10, 10, this.Width, this.Height, 40, 40); // _BoarderRaduis can be adjusted to your needs, try 15 to start.
            this.Region = System.Drawing.Region.FromHrgn(ptr);
            NativeMethods.DeleteObject(ptr);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn1(0, 0, Width, Height, 20, 20));
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn1(0, 0, Width, Height, 20, 20));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ////PrivateFontCollection pfc = new PrivateFontCollection();
            ////pfc.AddFontFile("C:\\Users\\172983\\source\\repos\\AMO\\UI_SourceCode\\WebUI\\Content\\fonts\\helvetica-neue-family\\HelveticaNeue-Light.ttf");
            ////foreach(Control c in this.Controls)
            ////{
            ////    c.Font = new Font(pfc.Families[0], 15, FontStyle.Regular);
            ////}
            try
            {
                //dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.CornflowerBlue;
                //dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSteelBlue;
                //dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
                //dataGridView1.EnableHeadersVisualStyles = false;
                dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                //dataGridView1.RowsDefaultCellStyle.BackColor = Color.White;
                //dataGridView1.AlternatingRowsDefaultCellStyle.BackColor =Color.Gainsboro;
                //dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
                //dataGridView1.DefaultCellStyle.SelectionBackColor = Color.LightGray;
                //dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;
                //dataGridView1.Column[0].Width = 100;
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
                if (DS != null)
                    DS.WriteXml(@"..\RegExStore.xml");
                MessageBox.Show("Saved successfully.");
                Log("Regular expressions got updated. Data time:" + DateTime.Now.ToString());
            }
            catch (Exception ex)
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
            else
            {
                MessageBox.Show("Fields should not be left empty");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string input = textBox1.Text;
            string pattern = @textBox2.Text.Trim();
            string repText = textBox3.Text;
            var r = "";
            try
            {
                r = Regex.Replace(input, pattern, repText);
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

        private void btn_reset_Click(object sender, EventArgs e)
        {
            txtName.Text = "";
            txtDesc.Text = "";
            txtExText.Text = "";
            txtReplace.Text = "";
            txtRegEx.Text = "";
            // txtName.Focus();
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

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtBox_Validate(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                //e.Cancel = true;
                //txtName.Focus();
                errorProvider1.SetError(txtName, "Name should not be left blank!");
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
                errorProvider3.SetError(txtExText, "Example Text should not be left blank!");
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
                errorProvider2.SetError(txtDesc, "Description should not be left blank!");
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
                errorProvider5.SetError(txtRegEx, "Name should not be left blank!");
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
                errorProvider4.SetError(txtReplace, "Name should not be left blank!");
            }
            else
            {
                e.Cancel = false;
                errorProvider4.SetError(txtReplace, "");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            int rowIndex = dataGridView1.CurrentCell.RowIndex;
            dataGridView1.Rows.RemoveAt(rowIndex);
        }

        private void ref_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://www.regexlib.com/DisplayPatterns.aspx");
            Process.Start(sInfo);
        }

        private void txtName_BorderStyleChanged(object sender, EventArgs e)
        {
            IntPtr ptr = NativeMethods.CreateRoundRectRgn(10, 10, this.Width, this.Height, 40, 40); // _BoarderRaduis can be adjusted to your needs, try 15 to start.
            this.Region = System.Drawing.Region.FromHrgn(ptr);
            NativeMethods.DeleteObject(ptr);
        }

        private void label16_Click(object sender, EventArgs e)
        {
            var inputform1 = new DataFileMask();
            inputform1.Height = 650;
            inputform1.Width = 1200;
            inputform1.Show();
            this.Hide();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            var inputform1 = new DataFileMask();
            inputform1.Height = 650;
            inputform1.Width = 1200;
            inputform1.Show();
            this.Hide();
        }

        private void minimizebutton1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void maximizebutton2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }


        //private void DataFile_Click(object sender, EventArgs e)
        //{
        //    var inputform = new Form3();
        //    inputform.Height = 650;
        //    inputform.Width = 1200;
        //    inputform.Show();
        //    this.Hide();
        //}

        //private void panel19_Paint(object sender, PaintEventArgs e)
        //{
        //    var inputform1 = new Form3();
        //    inputform1.Height = 650;
        //    inputform1.Width = 1200;
        //    inputform1.Show();
        //    this.Hide();
        //}
    }


    public class NativeMethods
    {
        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        public static extern System.IntPtr CreateRoundRectRgn
         (
          int nLeftRect, // x-coordinate of upper-left corner
          int nTopRect, // y-coordinate of upper-left corner
          int nRightRect, // x-coordinate of lower-right corner
          int nBottomRect, // y-coordinate of lower-right corner
          int nWidthEllipse, // height of ellipse
          int nHeightEllipse // width of ellipse
         );

        [System.Runtime.InteropServices.DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        public static extern bool DeleteObject(System.IntPtr hObject);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
    }

}

