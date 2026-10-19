using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataMaskingUtility
{
    public partial class License : Form
    {
        public License()
        {
            InitializeComponent();
        }

        private void License_Load(Object sender, EventArgs e)
        {
            var licenseform = new License();
            licenseform.Height = 650;
            licenseform.Width = 1200;
        }

        private void cmdContinue_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                //Application.Run(new DatamaskingInput());
                //var inputform=new DatamaskingInput();
                var inputform = new Configuration();
                //var inputform = new Form3();
                inputform.Height = 650;
                inputform.Width = 1200;
                inputform.Show();
                this.Hide();
            }
            else
                MessageBox.Show("Please acknowledge the license agreement by checking the the above check-box.");
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                cmdContinue.BackColor = System.Drawing.Color.FromArgb(91, 121, 171);
                cmdContinue.Enabled = true;
                cmdCancel.BackColor = System.Drawing.Color.Silver;
                cmdCancel.Enabled = true;

            }
            else
            {
                cmdContinue.BackColor = Color.Silver;
                cmdContinue.Enabled = false;
                cmdCancel.BackColor = Color.Silver;
                cmdCancel.Enabled = false;
            }
        }
    }
}
