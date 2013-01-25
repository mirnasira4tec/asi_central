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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmImport));
            this.label1 = new System.Windows.Forms.Label();
            this.txtSpreadsheetPath = new System.Windows.Forms.TextBox();
            this.btnFileLookup = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.cmbCompanyList = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.dgProducts = new System.Windows.Forms.DataGridView();
            this.cmbName = new System.Windows.Forms.ComboBox();
            this.cmbModel = new System.Windows.Forms.ComboBox();
            this.cmbPrice = new System.Windows.Forms.ComboBox();
            this.cmbCeiling = new System.Windows.Forms.ComboBox();
            this.cmbMin = new System.Windows.Forms.ComboBox();
            this.cmbTerms = new System.Windows.Forms.ComboBox();
            this.cmbSpecs = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtImage = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.Category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modelNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priceCeilingDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.minimumOrderQuantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.paymentTermsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.keySpecificationsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imageSmallDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imageLargeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.companyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgProducts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productBindingSource)).BeginInit();
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
            this.cmbCompanyList.SelectedIndexChanged += new System.EventHandler(this.cmbCompanyList_SelectedIndexChanged);
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(50, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(148, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Model Number";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(250, 91);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Price";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(349, 91);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Price Ceiling";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(449, 91);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Min Order Quant";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(551, 91);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Payment Terms";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(649, 91);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(94, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "Key Specifications";
            // 
            // dgProducts
            // 
            this.dgProducts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgProducts.AutoGenerateColumns = false;
            this.dgProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.modelNumberDataGridViewTextBoxColumn,
            this.priceDataGridViewTextBoxColumn,
            this.priceCeilingDataGridViewTextBoxColumn,
            this.minimumOrderQuantityDataGridViewTextBoxColumn,
            this.paymentTermsDataGridViewTextBoxColumn,
            this.keySpecificationsDataGridViewTextBoxColumn,
            this.Category,
            this.imageSmallDataGridViewTextBoxColumn,
            this.imageLargeDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn,
            this.companyDataGridViewTextBoxColumn});
            this.dgProducts.DataSource = this.productBindingSource;
            this.dgProducts.Location = new System.Drawing.Point(7, 135);
            this.dgProducts.Name = "dgProducts";
            this.dgProducts.Size = new System.Drawing.Size(852, 237);
            this.dgProducts.TabIndex = 12;
            // 
            // cmbName
            // 
            this.cmbName.FormattingEnabled = true;
            this.cmbName.Location = new System.Drawing.Point(49, 108);
            this.cmbName.Name = "cmbName";
            this.cmbName.Size = new System.Drawing.Size(98, 21);
            this.cmbName.TabIndex = 13;
            this.cmbName.SelectedIndexChanged += new System.EventHandler(this.cmbName_SelectedIndexChanged);
            // 
            // cmbModel
            // 
            this.cmbModel.FormattingEnabled = true;
            this.cmbModel.Location = new System.Drawing.Point(151, 107);
            this.cmbModel.Name = "cmbModel";
            this.cmbModel.Size = new System.Drawing.Size(98, 21);
            this.cmbModel.TabIndex = 14;
            this.cmbModel.SelectedIndexChanged += new System.EventHandler(this.cmbModel_SelectedIndexChanged);
            // 
            // cmbPrice
            // 
            this.cmbPrice.FormattingEnabled = true;
            this.cmbPrice.Location = new System.Drawing.Point(253, 107);
            this.cmbPrice.Name = "cmbPrice";
            this.cmbPrice.Size = new System.Drawing.Size(93, 21);
            this.cmbPrice.TabIndex = 15;
            this.cmbPrice.SelectedIndexChanged += new System.EventHandler(this.cmbPrice_SelectedIndexChanged);
            // 
            // cmbCeiling
            // 
            this.cmbCeiling.FormattingEnabled = true;
            this.cmbCeiling.Location = new System.Drawing.Point(352, 108);
            this.cmbCeiling.Name = "cmbCeiling";
            this.cmbCeiling.Size = new System.Drawing.Size(92, 21);
            this.cmbCeiling.TabIndex = 16;
            this.cmbCeiling.SelectedIndexChanged += new System.EventHandler(this.cmbCeiling_SelectedIndexChanged);
            // 
            // cmbMin
            // 
            this.cmbMin.FormattingEnabled = true;
            this.cmbMin.Location = new System.Drawing.Point(452, 107);
            this.cmbMin.Name = "cmbMin";
            this.cmbMin.Size = new System.Drawing.Size(92, 21);
            this.cmbMin.TabIndex = 17;
            this.cmbMin.SelectedIndexChanged += new System.EventHandler(this.cmbMin_SelectedIndexChanged);
            // 
            // cmbTerms
            // 
            this.cmbTerms.FormattingEnabled = true;
            this.cmbTerms.Location = new System.Drawing.Point(550, 107);
            this.cmbTerms.Name = "cmbTerms";
            this.cmbTerms.Size = new System.Drawing.Size(92, 21);
            this.cmbTerms.TabIndex = 18;
            this.cmbTerms.SelectedIndexChanged += new System.EventHandler(this.cmbTerms_SelectedIndexChanged);
            // 
            // cmbSpecs
            // 
            this.cmbSpecs.FormattingEnabled = true;
            this.cmbSpecs.Location = new System.Drawing.Point(652, 107);
            this.cmbSpecs.Name = "cmbSpecs";
            this.cmbSpecs.Size = new System.Drawing.Size(92, 21);
            this.cmbSpecs.TabIndex = 19;
            this.cmbSpecs.SelectedIndexChanged += new System.EventHandler(this.cmbSpecs_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(25, 65);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(109, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "Images: /images/sgr/";
            // 
            // txtImage
            // 
            this.txtImage.Location = new System.Drawing.Point(136, 60);
            this.txtImage.Name = "txtImage";
            this.txtImage.Size = new System.Drawing.Size(287, 20);
            this.txtImage.TabIndex = 21;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(677, 391);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 22;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(596, 390);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 23;
            this.btnImport.Text = "&Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(753, 91);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(49, 13);
            this.label11.TabIndex = 24;
            this.label11.Text = "Category";
            // 
            // cmbCategory
            // 
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(750, 107);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(92, 21);
            this.cmbCategory.TabIndex = 25;
            // 
            // Category
            // 
            this.Category.DataPropertyName = "Category";
            this.Category.HeaderText = "Category";
            this.Category.Name = "Category";
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.Visible = false;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            // 
            // modelNumberDataGridViewTextBoxColumn
            // 
            this.modelNumberDataGridViewTextBoxColumn.DataPropertyName = "ModelNumber";
            this.modelNumberDataGridViewTextBoxColumn.HeaderText = "ModelNumber";
            this.modelNumberDataGridViewTextBoxColumn.Name = "modelNumberDataGridViewTextBoxColumn";
            // 
            // priceDataGridViewTextBoxColumn
            // 
            this.priceDataGridViewTextBoxColumn.DataPropertyName = "Price";
            this.priceDataGridViewTextBoxColumn.HeaderText = "Price";
            this.priceDataGridViewTextBoxColumn.Name = "priceDataGridViewTextBoxColumn";
            // 
            // priceCeilingDataGridViewTextBoxColumn
            // 
            this.priceCeilingDataGridViewTextBoxColumn.DataPropertyName = "PriceCeiling";
            this.priceCeilingDataGridViewTextBoxColumn.HeaderText = "PriceCeiling";
            this.priceCeilingDataGridViewTextBoxColumn.Name = "priceCeilingDataGridViewTextBoxColumn";
            // 
            // minimumOrderQuantityDataGridViewTextBoxColumn
            // 
            this.minimumOrderQuantityDataGridViewTextBoxColumn.DataPropertyName = "MinimumOrderQuantity";
            this.minimumOrderQuantityDataGridViewTextBoxColumn.HeaderText = "MinimumOrderQuantity";
            this.minimumOrderQuantityDataGridViewTextBoxColumn.Name = "minimumOrderQuantityDataGridViewTextBoxColumn";
            // 
            // paymentTermsDataGridViewTextBoxColumn
            // 
            this.paymentTermsDataGridViewTextBoxColumn.DataPropertyName = "PaymentTerms";
            this.paymentTermsDataGridViewTextBoxColumn.HeaderText = "PaymentTerms";
            this.paymentTermsDataGridViewTextBoxColumn.Name = "paymentTermsDataGridViewTextBoxColumn";
            // 
            // keySpecificationsDataGridViewTextBoxColumn
            // 
            this.keySpecificationsDataGridViewTextBoxColumn.DataPropertyName = "KeySpecifications";
            this.keySpecificationsDataGridViewTextBoxColumn.HeaderText = "KeySpecifications";
            this.keySpecificationsDataGridViewTextBoxColumn.Name = "keySpecificationsDataGridViewTextBoxColumn";
            // 
            // imageSmallDataGridViewTextBoxColumn
            // 
            this.imageSmallDataGridViewTextBoxColumn.DataPropertyName = "ImageSmall";
            this.imageSmallDataGridViewTextBoxColumn.HeaderText = "ImageSmall";
            this.imageSmallDataGridViewTextBoxColumn.Name = "imageSmallDataGridViewTextBoxColumn";
            this.imageSmallDataGridViewTextBoxColumn.Visible = false;
            // 
            // imageLargeDataGridViewTextBoxColumn
            // 
            this.imageLargeDataGridViewTextBoxColumn.DataPropertyName = "ImageLarge";
            this.imageLargeDataGridViewTextBoxColumn.HeaderText = "ImageLarge";
            this.imageLargeDataGridViewTextBoxColumn.Name = "imageLargeDataGridViewTextBoxColumn";
            this.imageLargeDataGridViewTextBoxColumn.Visible = false;
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            this.isActiveDataGridViewCheckBoxColumn.Visible = false;
            // 
            // companyDataGridViewTextBoxColumn
            // 
            this.companyDataGridViewTextBoxColumn.DataPropertyName = "Company";
            this.companyDataGridViewTextBoxColumn.HeaderText = "Company";
            this.companyDataGridViewTextBoxColumn.Name = "companyDataGridViewTextBoxColumn";
            this.companyDataGridViewTextBoxColumn.Visible = false;
            // 
            // productBindingSource
            // 
            this.productBindingSource.DataSource = typeof(SGRImport.model.ProductView);
            // 
            // frmImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(861, 426);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtImage);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cmbSpecs);
            this.Controls.Add(this.cmbTerms);
            this.Controls.Add(this.cmbMin);
            this.Controls.Add(this.cmbCeiling);
            this.Controls.Add(this.cmbPrice);
            this.Controls.Add(this.cmbModel);
            this.Controls.Add(this.cmbName);
            this.Controls.Add(this.dgProducts);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbCompanyList);
            this.Controls.Add(this.btnFileLookup);
            this.Controls.Add(this.txtSpreadsheetPath);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmImport";
            this.Text = "SGR Import";
            ((System.ComponentModel.ISupportInitialize)(this.dgProducts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productBindingSource)).EndInit();
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
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DataGridView dgProducts;
        private System.Windows.Forms.BindingSource productBindingSource;
        private System.Windows.Forms.ComboBox cmbName;
        private System.Windows.Forms.ComboBox cmbModel;
        private System.Windows.Forms.ComboBox cmbPrice;
        private System.Windows.Forms.ComboBox cmbCeiling;
        private System.Windows.Forms.ComboBox cmbMin;
        private System.Windows.Forms.ComboBox cmbTerms;
        private System.Windows.Forms.ComboBox cmbSpecs;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtImage;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modelNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn priceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn priceCeilingDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn minimumOrderQuantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn paymentTermsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn keySpecificationsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Category;
        private System.Windows.Forms.DataGridViewTextBoxColumn imageSmallDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn imageLargeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn companyDataGridViewTextBoxColumn;
    }
}

