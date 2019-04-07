using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace testKraschvetMetMy
{
    public partial class main : Form
    {
        private string cfg = "cfg.xml";
        public main()
        {
            InitializeComponent();
        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectFilesImport s = new selectFilesImport();
            s.cfg = this.cfg;
            s.Show();
        }

        private void выходToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы уверены, что хотите выйти?", "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dr == DialogResult.Yes)
                Environment.Exit(0);
        }

        private void расчетToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                XDocument xDoc = XDocument.Load(cfg);
                XElement xCFG = xDoc.Element("cfg");


            }
            catch (Exception ex) {
                MessageBox.Show(String.Format("Конфигурационный файл не найден или поврежден, проведите настройку повторно! \n Ошибка {0}", ex.Message), "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
    }
}
