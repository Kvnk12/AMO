
namespace AMOReportsJOBProcessorSVC
{
    partial class AMOReportsJOBProcessor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        public enum ReportScheduleStatus
        {
            SCHEDULED = 1,
            STARTED = 2,
            INPROGRESS = 3,
            COMPLETED = 4,
            FAILED = 5,

        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.ServiceName = "AMOReportsJOBProcessor";
        }

        #endregion
    }
}
