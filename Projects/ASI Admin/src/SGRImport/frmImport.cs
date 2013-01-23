using asi.asicentral.database.mappings;
using asi.asicentral.model.sgr;
using asi.asicentral.services;
using asi.asicentral.services.interfaces;
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

        public frmImport()
        {
            InitializeComponent();
            _objectService = new ObjectService(new asi.asicentral.services.Container(new EFRegistry()));
        }

        private void btnFileLookup_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                txtSpreadsheetPath.Text = openFileDialog.SafeFileName;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                IList<Company> companies = _objectService.GetAll<Company>().OrderBy(company => company.Name).ToList();
                cmbCompanyList.DataBindings.Add("Name", companies, "Id");
                base.OnLoad(e);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception" + exception.Message);
            }
        }
    }
}
