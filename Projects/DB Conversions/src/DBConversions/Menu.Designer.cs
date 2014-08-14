namespace DBConversions
{
    partial class Menu
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
            this.BtnConvert = new System.Windows.Forms.Button();
            this.Lbl_Menu = new System.Windows.Forms.Label();
            this.lbl_confirmation = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BtnConvert
            // 
            this.BtnConvert.Location = new System.Drawing.Point(12, 44);
            this.BtnConvert.Name = "BtnConvert";
            this.BtnConvert.Size = new System.Drawing.Size(151, 23);
            this.BtnConvert.TabIndex = 4;
            this.BtnConvert.Text = "Convert Newsletters";
            this.BtnConvert.UseVisualStyleBackColor = true;
            this.BtnConvert.Click += new System.EventHandler(this.BtnConvert_Click);
            // 
            // Lbl_Menu
            // 
            this.Lbl_Menu.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Lbl_Menu.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl_Menu.Location = new System.Drawing.Point(37, 20);
            this.Lbl_Menu.Name = "Lbl_Menu";
            this.Lbl_Menu.Size = new System.Drawing.Size(66, 13);
            this.Lbl_Menu.TabIndex = 5;
            this.Lbl_Menu.Text = "Menu";
            this.Lbl_Menu.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbl_confirmation
            // 
            this.lbl_confirmation.Location = new System.Drawing.Point(27, 185);
            this.lbl_confirmation.Name = "lbl_confirmation";
            this.lbl_confirmation.Size = new System.Drawing.Size(222, 23);
            this.lbl_confirmation.TabIndex = 6;
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.lbl_confirmation);
            this.Controls.Add(this.Lbl_Menu);
            this.Controls.Add(this.BtnConvert);
            this.Name = "Menu";
            this.Text = "Data Migration";
            this.ResumeLayout(false);

        }

        #endregion

        
        private System.Windows.Forms.Button BtnConvert;
        private System.Windows.Forms.Label Lbl_Menu;
        private System.Windows.Forms.Label lbl_confirmation;
    }
}

