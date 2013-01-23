namespace SGRImport
{
    partial class frmImport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmImport));
            this.label1 = new System.Windows.Forms.Label();
            this.txtSpreadsheetPath = new System.Windows.Forms.TextBox();
            this.btnFileLookup = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.cmbCompanyList = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select the Spreadsheet";
            // 
            // txtSpreadsheetPath
            // 
            this.txtSpreadsheetPath.Enabled = false;
            this.txtSpreadsheetPath.Location = new System.Drawing.Point(136, 6);
            this.txtSpreadsheetPath.Name = "txtSpreadsheetPath";
            this.txtSpreadsheetPath.Size = new System.Drawing.Size(287, 20);
            this.txtSpreadsheetPath.TabIndex = 1;
            // 
            // btnFileLookup
            // 
            this.btnFileLookup.Location = new System.Drawing.Point(429, 4);
            this.btnFileLookup.Name = "btnFileLookup";
            this.btnFileLookup.Size = new System.Drawing.Size(29, 23);
            this.btnFileLookup.TabIndex = 2;
            this.btnFileLookup.Text = "...";
            this.btnFileLookup.UseVisualStyleBackColor = true;
            this.btnFileLookup.Click += new System.EventHandler(this.btnFileLookup_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "*.xls";
            this.openFileDialog.Filter = "Excel Files (*.xls) | *.xls | All Files (*.*) | *.*";
            // 
            // cmbCompanyList
            // 
            this.cmbCompanyList.FormattingEnabled = true;
            this.cmbCompanyList.Location = new System.Drawing.Point(136, 33);
            this.cmbCompanyList.Name = "cmbCompanyList";
            this.cmbCompanyList.Size = new System.Drawing.Size(287, 21);
            this.cmbCompanyList.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(79, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Company";
            // 
            // frmImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 262);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbCompanyList);
            this.Controls.Add(this.btnFileLookup);
            this.Controls.Add(this.txtSpreadsheetPath);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmImport";
            this.Text = "SGR Import";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSpreadsheetPath;
        private System.Windows.Forms.Button btnFileLookup;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ComboBox cmbCompanyList;
        private System.Windows.Forms.Label label2;
    }
}

