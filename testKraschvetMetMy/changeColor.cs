using NLog;
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
        // Для логирования
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // Конфиг файл
        public string cfg = "cfg.xml";

        public changeColor()
        {
            InitializeComponent();
        }

        private void changeColor_Shown(object sender, EventArgs e)
        {
            logger.Info("Делаем первоначальную отрисовку");
            loadColor();
            logger.Info("Закончили с первоначальной отрисовкой");
        }

        private void loadColor()
        {
            try
            {
                //Назначаем цвет, для фона, чтобы различать с другими цветами
                pbChangeColors.BackColor = Color.White;
                Bitmap bmp = new Bitmap(pbChangeColors.Width, pbChangeColors.Height);
                Graphics graphics = Graphics.FromImage(bmp);
                Font font = new Font("Arial", 8);

                int iLegendY = 25;      // Высота легенды номенклатур
                int iMarginX = 0;       // Отступ слева
                int iToolNamesX = 60;   // Ширина легенды оборудования


                XDocument xDoc = XDocument.Load(cfg);
                XElement xCFG = xDoc.Element("cfg");

                Dictionary<int, Dictionary<string, string>> arrClr = new Dictionary<int, Dictionary<string, string>>();

                // Заполняем словарь с цветом из конфиг файла
                foreach (XElement elm in xCFG.Elements("metalcolors").Nodes<XElement>())
                {
                    int id = Int32.Parse(elm.Element("id").Value.ToString());
                    string metal = elm.Element("metal").Value.ToString();
                    string color = elm.Element("color").Value.ToString();
                    Dictionary<string, string> d = new Dictionary<string, string>();
                    d.Add(metal, color);
                    arrClr.Add(id, d);
                }

                // Отрисовываем легенду
                foreach (KeyValuePair<int, Dictionary<string, string>> kvp in arrClr)
                {
                    int id = kvp.Key;
                    Dictionary<string, string> d = kvp.Value;

                    Brush brush = new SolidBrush(ColorTranslator.FromHtml(String.Format("{0}", d.Values.ElementAt(0))));
                    graphics.FillRectangle(brush, iToolNamesX + iMarginX + id * 60, 0, 60, iLegendY - 5);
                    graphics.DrawString(d.Keys.ElementAt(0), font, Brushes.Black, iToolNamesX + iMarginX + id * 60 + 5, 0 + 5);

                }

                pbChangeColors.Image = bmp;
            }
            catch (Exception ex) {
                logger.Error(String.Format("Произошла ошибка! \n Ошибка {0}", ex.Message));
            }
        }

        private void pbChangeColors_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                XDocument xDoc = XDocument.Load(cfg);
                XElement xCFG = xDoc.Element("cfg");

                // Получаем пиксель по нажатию, для определения цвета
                Bitmap b = ((Bitmap)pbChangeColors.Image);
                int x = e.X * b.Width / pbChangeColors.ClientSize.Width;
                int y = e.Y * b.Height / pbChangeColors.ClientSize.Height;
                Color c = b.GetPixel(x, y);

                if (c != Color.White)
                {
                    // Выбираем новый цвет и записываем в конфиг
                    ColorDialog cd = new ColorDialog();
                    if (cd.ShowDialog() == DialogResult.OK)
                    {
                        string nColor = ColorTranslator.ToHtml(cd.Color);
                        string oColor = ColorTranslator.ToHtml(c);
                        Console.WriteLine(nColor);

                        logger.Info(String.Format("Изменяем цвет {0} на цвет {1}", oColor, nColor));

                        foreach (XElement elm in xCFG.Elements("metalcolors").Nodes<XElement>())
                        {
                            if (String.Format("{0}", elm.Element("color").Value.ToString()) == oColor)
                            {
                                elm.Element("color").Value = nColor;
                            }
                        }
                    }
                }

                xDoc.Save(cfg);
                // Заново перерисовываем bitmap
                loadColor();
            }
            catch (Exception ex) {
                logger.Error(String.Format("Произошла ошибка! \n Ошибка {0}", ex.Message));
            }
        }

        private void changeColor_FormClosed(object sender, FormClosedEventArgs e)
        {
            logger.Info("Закрытие формы changeColor для смены цветов");
        }
    }
}
