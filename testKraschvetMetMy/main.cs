using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testKraschvetMetMy
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settings s = new settings();
            s.Show();
        }
    }
}
