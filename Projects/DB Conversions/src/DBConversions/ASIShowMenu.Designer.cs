namespace DBConversions
{
    partial class ASIShowMenu
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
            this.lblConfirmation = new System.Windows.Forms.Label();
            this.btnAddContent = new System.Windows.Forms.Button();
            this.lblNodeId = new System.Windows.Forms.Label();
            this.rtbContent = new System.Windows.Forms.RichTextBox();
            this.txtNodeId = new System.Windows.Forms.TextBox();
            this.lblContent = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblConfirmation
            // 
            this.lblConfirmation.Location = new System.Drawing.Point(101, 366);
            this.lblConfirmation.Name = "lblConfirmation";
            this.lblConfirmation.Size = new System.Drawing.Size(222, 23);
            this.lblConfirmation.TabIndex = 7;
            // 
            // btnAddContent
            // 
            this.btnAddContent.Location = new System.Drawing.Point(127, 312);
            this.btnAddContent.Name = "btnAddContent";
            this.btnAddContent.Size = new System.Drawing.Size(132, 36);
            this.btnAddContent.TabIndex = 8;
            this.btnAddContent.Text = "Add Content";
            this.btnAddContent.UseVisualStyleBackColor = true;
            this.btnAddContent.Click += new System.EventHandler(this.btnAddContent_Click);
            // 
            // lblNodeId
            // 
            this.lblNodeId.AutoSize = true;
            this.lblNodeId.Location = new System.Drawing.Point(37, 44);
            this.lblNodeId.Name = "lblNodeId";
            this.lblNodeId.Size = new System.Drawing.Size(47, 13);
            this.lblNodeId.TabIndex = 9;
            this.lblNodeId.Text = "Node ID";
            // 
            // rtbContent
            // 
            this.rtbContent.Location = new System.Drawing.Point(127, 76);
            this.rtbContent.Name = "rtbContent";
            this.rtbContent.Size = new System.Drawing.Size(273, 221);
            this.rtbContent.TabIndex = 10;
            this.rtbContent.Text = "";
            // 
            // txtNodeId
            // 
            this.txtNodeId.Location = new System.Drawing.Point(127, 44);
            this.txtNodeId.Name = "txtNodeId";
            this.txtNodeId.Size = new System.Drawing.Size(100, 20);
            this.txtNodeId.TabIndex = 11;
            // 
            // lblContent
            // 
            this.lblContent.AutoSize = true;
            this.lblContent.Location = new System.Drawing.Point(38, 80);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(44, 13);
            this.lblContent.TabIndex = 12;
            this.lblContent.Text = "Content";
            // 
            // ASIShowMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 418);
            this.Controls.Add(this.lblContent);
            this.Controls.Add(this.txtNodeId);
            this.Controls.Add(this.rtbContent);
            this.Controls.Add(this.lblNodeId);
            this.Controls.Add(this.btnAddContent);
            this.Controls.Add(this.lblConfirmation);
            this.Name = "ASIShowMenu";
            this.Text = "ASIShowMenu";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblConfirmation;
        private System.Windows.Forms.Button btnAddContent;
        private System.Windows.Forms.Label lblNodeId;
        private System.Windows.Forms.RichTextBox rtbContent;
        private System.Windows.Forms.TextBox txtNodeId;
        private System.Windows.Forms.Label lblContent;
    }
}