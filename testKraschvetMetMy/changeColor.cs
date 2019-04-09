using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace testKraschvetMetMy
{
    public partial class changeColor : Form
    {
        public string cfg = "cfg.xml";
        public changeColor()
        {
            InitializeComponent();
        }

        private void changeColor_Shown(object sender, EventArgs e)
        {
            loadColor();
        }

        private void loadColor()
        {
            pbChangeColors.BackColor = Color.White;
            Bitmap bmp = new Bitmap(pbChangeColors.Width, pbChangeColors.Height);
            Graphics graphics = Graphics.FromImage(bmp);
            Font font = new Font("Arial", 8);

            int iLegendY = 25;
            int iMarginX = 0;
            int iToolNamesX = 5;

            XDocument xDoc = XDocument.Load(cfg);
            XElement xCFG = xDoc.Element("cfg");

            Dictionary<int, Dictionary<string, string>> arrClr = new Dictionary<int, Dictionary<string, string>>();

            foreach (XElement elm in xCFG.Elements("metalcolors").Nodes<XElement>())
            {
                int id = Int32.Parse(elm.Element("id").Value.ToString());
                string metal = elm.Element("metal").Value.ToString();
                string color = elm.Element("color").Value.ToString();
                Dictionary<string, string> d = new Dictionary<string, string>();
                d.Add(metal, color);
                arrClr.Add(id, d);
            }

            foreach (KeyValuePair<int, Dictionary<string, string>> kvp in arrClr)
            {
                int id = kvp.Key;
                Dictionary<string, string> d = kvp.Value;

                Brush brush = new SolidBrush(ColorTranslator.FromHtml(String.Format("#{0}", d.Values.ElementAt(0))));
                graphics.FillRectangle(brush, iToolNamesX + iMarginX + id * 60, 0, 60, iLegendY - 5);
                graphics.DrawString(d.Keys.ElementAt(0), font, Brushes.Black, iToolNamesX + iMarginX + id * 60 + 5, 0 + 5);

            }

            pbChangeColors.Image = bmp;
        }

        private void pbChangeColors_MouseClick(object sender, MouseEventArgs e)
        {
            Bitmap b = ((Bitmap)pbChangeColors.Image);
            int x = e.X * b.Width / pbChangeColors.ClientSize.Width;
            int y = e.Y * b.Height / pbChangeColors.ClientSize.Height;
            Color c = b.GetPixel(x, y);

            if (c != Color.White) {
                ColorDialog cd = new ColorDialog();
                if (cd.ShowDialog() == DialogResult.OK) {
                    Console.WriteLine(cd.Color);
                }
            }
        }
    }
}
