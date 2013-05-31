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
        private string connectionStringAppend = string.Empty;

        public Convert(DatabaseTarget target)
        {
            InitializeComponent();
        }

        private void Convert_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Convert_Load(object sender, EventArgs e)
        {
        }
    }
}
