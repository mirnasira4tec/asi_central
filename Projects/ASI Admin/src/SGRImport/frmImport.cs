﻿using asi.asicentral.database.mappings;
using asi.asicentral.model.sgr;
using asi.asicentral.services;
using asi.asicentral.services.interfaces;
using SGRImport.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SGRImport
{
    public partial class frmImport : Form
    {
        private IObjectService _objectService;
        private ExcelUtil _excelFile;
        private bool _loadingExcel = false;

        public frmImport()
        {
            InitializeComponent();
            _objectService = new ObjectService(new asi.asicentral.services.Container(new EFRegistry()));
        }

        private void btnFileLookup_Click(object sender, EventArgs e)
        {
            openFileDialog.FileName = "*.xls";
            DialogResult result = openFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                _loadingExcel = true;
                txtSpreadsheetPath.Text = openFileDialog.SafeFileName;
                //open the file
                if (_excelFile != null)
                {
                    _excelFile.Dispose();
                    _excelFile = null;
                }
                _excelFile = new ExcelUtil(openFileDialog.FileName);
                //read the headers
                string[] headers = _excelFile.GetHeaders();
                //set drop down default values + values
                UpdateHeaderCombo(cmbName, headers, "Product");
                UpdateHeaderCombo(cmbModel, headers, "Model");
                UpdateHeaderCombo(cmbPrice, headers, "Price");
                UpdateHeaderCombo(cmbMin, headers, "Min");
                UpdateHeaderCombo(cmbTerms, headers, "Terms");
                UpdateHeaderCombo(cmbCeiling, headers, "Ceiling");
                UpdateHeaderCombo(cmbSpecs, headers, "Specs");
                UpdateHeaderCombo(cmbCategory, headers, "Category");
                _loadingExcel = false;
                //populate example rows
                PopulatePreview();
            }
        }

        private void PopulatePreview()
        {
            if (!_loadingExcel)
            {
                IList<ProductView> productList = GetProductList(8);
                //display in the grid
                dgProducts.DataSource = productList;
            }
        }

        private void UpdateHeaderCombo(ComboBox comboBox, string[] headers, string valueMatch)
        {
            var autoCompleteSource = new AutoCompleteStringCollection();
            autoCompleteSource.AddRange(headers);
            comboBox.AutoCompleteCustomSource = autoCompleteSource;
            comboBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            comboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox.Items.Clear();
            comboBox.Text = null;
            comboBox.Items.Add("");
            foreach (string value in headers)
            {
                comboBox.Items.Add(value);
                if (comboBox.Text == string.Empty && value.Contains(valueMatch)) 
                    comboBox.Text = value;
            }
        }

        private int FindIndex(string text, ComboBox.ObjectCollection list) 
        {
            int index = 0;
            if (!string.IsNullOrWhiteSpace(text)) index = list.IndexOf(text);
            return index;
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                IList<Company> companies = _objectService.GetAll<Company>().OrderBy(company => company.Name).ToList();
                //set the auto complete
                cmbCompanyList.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                var autoCompleteSource = new AutoCompleteStringCollection();
                autoCompleteSource.AddRange(companies.Select(company => company.Name).ToArray());
                cmbCompanyList.AutoCompleteCustomSource = autoCompleteSource;
                cmbCompanyList.AutoCompleteSource = AutoCompleteSource.CustomSource;
                cmbCompanyList.DisplayMember = "Name";
                cmbCompanyList.ValueMember = "Id";
                cmbCompanyList.DataSource = companies;
                base.OnLoad(e);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception" + exception.Message);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            _excelFile.Dispose();
            _objectService.Dispose();
        }

        private void cmbName_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulatePreview();
        }

        private void cmbModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulatePreview();
        }

        private void cmbPrice_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulatePreview();
        }

        private void cmbCeiling_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulatePreview();
        }

        private void cmbMin_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulatePreview();
        }

        private void cmbTerms_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulatePreview();
        }

        private void cmbSpecs_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulatePreview();
        }

        private void cmbCompanyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Company company = cmbCompanyList.SelectedItem as Company;
            txtImage.Text = company != null ? company.Name : string.Empty;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                Company company = (Company)cmbCompanyList.SelectedItem;
                IList<ProductView> productList = GetProductList(0);
                foreach (ProductView productView in productList)
                {
                    Product product = productView.GetProduct();
                    product.Company = company;
                    //set the images
                    string imagePath = txtImage.Text;
                    if (!string.IsNullOrWhiteSpace(imagePath)) imagePath += "/";
                    else imagePath = string.Empty;
                    product.ImageSmall = imagePath + product.ModelNumber + "_sm.jpg";
                    product.ImageLarge = imagePath + product.ModelNumber + "_lg.jpg";
                    //update the category
                    string categoryName = (!string.IsNullOrWhiteSpace(productView.Category) ? productView.Category.Trim() : "All");
                    Category category = company.Categories.Where(cat => cat.Name == categoryName).FirstOrDefault();
                    if (category == null)
                    {
                        //category not asssigned to the company, looking in the database
                        category = _objectService.GetAll<Category>().Where(cat => cat.Name == categoryName).FirstOrDefault();
                        if (category == null)
                        {
                            //not in the database, add it
                            category = new Category();
                            category.Name = categoryName;
                        }
                        company.Categories.Add(category);
                    }
                    product.Categories.Add(category);
                    _objectService.Add<Product>(product);
                }
                //_objectService.SaveChanges();
                MessageBox.Show("The products have been imported");
            }
            catch (Exception exception)
            {
                MessageBox.Show("Sorry, the import did not work: " + exception.Message);
            }
        }

        private IList<ProductView> GetProductList(int rows)
        {
            //read first 10 lines of spreadsheet, or less
            int[] cols = new int[8];
            cols[0] = FindIndex(cmbName.Text, cmbName.Items);
            cols[1] = FindIndex(cmbModel.Text, cmbName.Items);
            cols[2] = FindIndex(cmbPrice.Text, cmbName.Items);
            cols[3] = FindIndex(cmbCeiling.Text, cmbName.Items);
            cols[4] = FindIndex(cmbMin.Text, cmbName.Items);
            cols[5] = FindIndex(cmbTerms.Text, cmbName.Items);
            cols[6] = FindIndex(cmbSpecs.Text, cmbName.Items);
            cols[7] = FindIndex(cmbCategory.Text, cmbName.Items);
            //create product list
            List<ProductView> productList = new List<ProductView>();
            int i = 2; //skip the first row, excel starts at 1
            string cellValue = _excelFile.GetValue(i, cols[0]);
            while ( (rows < 1 || i < rows + 2) && cellValue != null)
            {
                decimal tempDecimal;
                //create/populate product based on selected columns
                ProductView product = new ProductView();
                productList.Add(product);
                product.Name = cellValue;
                product.ModelNumber = _excelFile.GetValue(i, cols[1]);
                if (Decimal.TryParse(_excelFile.GetValue(i, cols[2]), out tempDecimal)) product.Price = tempDecimal;
                if (Decimal.TryParse(_excelFile.GetValue(i, cols[3]), out tempDecimal)) product.PriceCeiling = tempDecimal;
                product.MinimumOrderQuantity = _excelFile.GetValue(i, cols[4]);
                product.PaymentTerms = _excelFile.GetValue(i, cols[5]);
                product.KeySpecifications = _excelFile.GetValue(i, cols[6]);
                cellValue = _excelFile.GetValue(i++, cols[0]);
            }
            return productList;
        }
    }
}