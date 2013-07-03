using asi.asicentral.services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Store_Database_Conversion
{
    public enum DatabaseTarget { Local, Development, Staging, Live }

    public partial class Convert : Form
    {
        private static int COMMIT_COUNT = 100;
        private string _connectionStringAppend = string.Empty;
        DatabaseTarget _target;
        private DatabaseService _databaseService;

        public Convert(DatabaseTarget target)
        {
            InitializeComponent();
            _target = target;
            _databaseService = new DatabaseService(target);
            lblAction.Text = "Trying to convert the data for the " + target + " environment";
        }

        private void Convert_FormClosed(object sender, FormClosedEventArgs e)
        {
            _databaseService.Dispose();
            Application.Exit();
        }

        private void Convert_Load(object sender, EventArgs e)
        {
            SetMessages();
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            toolStripStatusLabel.Text = "Transferring the data...";
            this.Refresh();
            pbStatus.Visible = true;
            int toMoveCount = _databaseService.GetLegacyCount();
            int step = toMoveCount / COMMIT_COUNT;
            int current = 0;
            try
            {
                for (int i = 0; i <= step; i++)
                {
                    DatabaseService databaseService = new DatabaseService(_target);
                    int nextStep = Math.Min(toMoveCount, current + COMMIT_COUNT);
                    //get the records between current and nextStep
                    databaseService.ProcessLegacyRecords(current, nextStep);
                    current = nextStep;
                    pbStatus.Value = (current * pbStatus.Maximum) / toMoveCount;
                    this.Refresh();
                    databaseService.Dispose();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Failed to process some records: " + exception.Message);
            }
            Console.WriteLine(current);
            Cursor.Current = Cursors.Default;
            pbStatus.Visible = false;
            SetMessages();
        }

        private void SetMessages()
        {
            Cursor.Current = Cursors.WaitCursor;
            toolStripStatusLabel.Text = "Reading the content of the databases";
            try
            {
                lblAction.Text = "Trying to convert the data for the " + _target + " environment, converting " +
                    _databaseService.GetLegacyCount() + " record(s) into existing " +
                    _databaseService.GetNewCount() + " record(s)";
                toolStripStatusLabel.Text = "Ready";
            }
            catch (Exception exception)
            {
                toolStripStatusLabel.Text = "Failed getting the data from databases";
                LogService.GetLog(this.GetType()).Error(exception.Message);
            }
            this.Refresh();
            Cursor.Current = Cursors.Default;
        }
    }
}
