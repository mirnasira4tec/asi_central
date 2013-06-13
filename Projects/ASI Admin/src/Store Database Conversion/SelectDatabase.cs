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
    public partial class SelectDatabase : Form
    {
        public SelectDatabase()
        {
            InitializeComponent();
        }

        private void ShowConvert(DatabaseTarget target)
        {
            Convert convert = new Convert(target);
            this.Hide();
            convert.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowConvert(DatabaseTarget.Local);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ShowConvert(DatabaseTarget.Development);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ShowConvert(DatabaseTarget.Staging);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ShowConvert(DatabaseTarget.Live);
        }
    }
}
