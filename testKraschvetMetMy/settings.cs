using System;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using OfficeOpenXml;

namespace testKraschvetMetMy
{
    public partial class settings : Form
    {

        String cfg = "cfg.xml";

        public settings()
        {
            InitializeComponent();
        }

        private void settings_Shown(object sender, EventArgs e)
        {
            if (!File.Exists(cfg))
            {
                createCfgXML();
            }

            var xdoc = XDocument.Load(cfg);
            foreach (XElement elm in xdoc.Descendants())
            {
                switch (elm.Name.ToString())
                {
                    case "machine_tools":
                        {
                            machine_tools_textBox.Text = elm.Value.ToString();
                            break;
                        }
                    case "nomenclatures":
                        {
                            nomenclatures_textBox.Text = elm.Value.ToString();
                            if (elm.Value.ToString() != "") {
                                getCountNomenclature(elm.Value.ToString());
                            }
                            break;
                        }
                    case "parties":
                        {
                            parties_textBox.Text = elm.Value.ToString();
                            break;
                        }
                    case "times":
                        {
                            times_textBox.Text = elm.Value.ToString();
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }

        private void getCountNomenclature(String namefile)
        {
            ExcelPackage ep = new ExcelPackage(new FileInfo(namefile));
            ExcelWorksheet ws = ep.Workbook.Worksheets[1];
            int countRow = ws.Dimension.End.Row;
            int countColumn = ws.Dimension.End.Column;
            for (int rowNum = 1; rowNum <= countRow; rowNum++) 
            {
                var id = ws.Cells[rowNum, 1];
                var val = ws.Cells[rowNum, 2];
                Console.WriteLine(String.Format("{0} -> {1}", id.Text, val.Text));
            }
        }

        private void createCfgXML()
        {
            XDocument xdoc = new XDocument(new XElement("cfg", 
                new XElement("sourcefiles", 
                    new XElement("machine_tools", null),
                    new XElement("nomenclatures", null),
                    new XElement("parties", null),
                    new XElement("times", null)), 
                new XElement("metalcolor", null)));
            xdoc.Save(cfg);
        }

        private String getFullNameFile() {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Лист Microsoft Excel (.xlsx) |*.xlsx";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.Cancel)
                return null;
            return ofd.FileName;
        }

        private void parties_button_Click(object sender, EventArgs e)
        {
            string file = getFullNameFile();
            parties_textBox.Text = file;
        }

        private void nomenclatures_button_Click(object sender, EventArgs e)
        {
            string file = getFullNameFile();
            nomenclatures_textBox.Text = file;
        }

        private void machine_tools_button_Click(object sender, EventArgs e)
        {
            string file = getFullNameFile();
            machine_tools_textBox.Text = file;
        }

        private void times_button_Click(object sender, EventArgs e)
        {
            string file = getFullNameFile();
            times_textBox.Text = file;
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void save_Click(object sender, EventArgs e)
        {
            XDocument xdoc = XDocument.Load(cfg);
            XElement files = xdoc.Element("cfg");
            
            foreach (XElement elm in files.Elements("sourcefiles").Nodes<XElement>()) {
                Console.WriteLine(String.Format("{0} -> {1}", elm.Name, elm.Value));
                switch (elm.Name.ToString())
                {
                    case "machine_tools":
                        {
                            elm.Value = machine_tools_textBox.Text;
                            break;
                        }
                    case "nomenclatures":
                        {
                            elm.Value = nomenclatures_textBox.Text;
                            break;
                        }
                    case "parties":
                        {
                            elm.Value = parties_textBox.Text;
                            break;
                        }
                    case "times":
                        {
                            elm.Value = times_textBox.Text;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            xdoc.Save(cfg);
        }
    }
}
