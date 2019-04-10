using System;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using OfficeOpenXml;
using System.Collections.Generic;
using NLog;

namespace testKraschvetMetMy
{
    public partial class selectFilesImport : Form
    {
        // Для логирования
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // Конфиг файл
        public string cfg = "cfg.xml";

        // Массив изначальных цветов
        static string[] ColourValues = new string[] {
        "FF0000", "00FF00", "0000FF", "FFFF00", "FF00FF", "00FFFF", "000000",
        "800000", "008000", "000080", "808000", "800080", "008080", "808080",
        "C00000", "00C000", "0000C0", "C0C000", "C000C0", "00C0C0", "C0C0C0",
        "400000", "004000", "000040", "404000", "400040", "004040", "404040",
        "200000", "002000", "000020", "202000", "200020", "002020", "202020",
        "600000", "006000", "000060", "606000", "600060", "006060", "606060",
        "A00000", "00A000", "0000A0", "A0A000", "A000A0", "00A0A0", "A0A0A0",
        "E00000", "00E000", "0000E0", "E0E000", "E000E0", "00E0E0", "E0E0E0",
    };

        public selectFilesImport()
        {
            InitializeComponent();
        }

        private void settings_Shown(object sender, EventArgs e)
        {
            logger.Info("Проверяем существования конфигурационного файла");
            if (!File.Exists(cfg))
            {
                logger.Info("Конфиг файл не создан, так что генерим его");
                createCfgXML();
            }

            logger.Info("Подгружаем данные с конфиг файла");
            var xdoc = XDocument.Load(cfg);
            // Определяем какие поля должны быть для валидации
            Dictionary<string, List<string>> dictValidFile = new Dictionary<string, List<string>>() {
                { "machine_tools", new List<string>() { "id", "name" } },
                { "nomenclatures", new List<string>() { "id", "nomenclature" } },
                { "parties", new List<string>() { "id", "nomenclature id" } },
                { "times", new List<string>() { "machine tool id", "nomenclature id", "operation time" } },
            };

            // Заполняем форму и проверяем валидацию
            foreach (XElement elm in xdoc.Descendants())
            {
                switch (elm.Name.ToString())
                {
                    case "machine_tools":
                        {
                            if (elm.Value.ToString() != "")
                            {
                                logger.Info(String.Format("Проверяем валидацию у оборудования"));
                                if (ckeckValidateImportFile(elm.Value.ToString(), dictValidFile[elm.Name.ToString()]))
                                {
                                    machine_tools_textBox.Text = elm.Value.ToString();
                                }
                                else
                                {
                                    machine_tools_textBox.Text = "";
                                }
                            }
                            break;
                        }
                    case "nomenclatures":
                        {
                            if (elm.Value.ToString() != "")
                            {
                                logger.Info(String.Format("Проверяем валидацию у номенклатуры"));
                                if (ckeckValidateImportFile(elm.Value.ToString(), dictValidFile[elm.Name.ToString()]))
                                {
                                    nomenclatures_textBox.Text = elm.Value.ToString();                                    
                                }
                                else
                                {
                                    nomenclatures_textBox.Text = "";
                                }

                            }
                            break;
                        }
                    case "parties":
                        {

                            if (elm.Value.ToString() != "")
                            {
                                logger.Info(String.Format("Проверяем валидацию у партии"));
                                if (ckeckValidateImportFile(elm.Value.ToString(), dictValidFile[elm.Name.ToString()]))
                                    parties_textBox.Text = elm.Value.ToString();
                                else
                                    parties_textBox.Text = "";
                            }
                            break;
                        }
                    case "times":
                        {
                            if (elm.Value.ToString() != "")
                            {
                                logger.Info(String.Format("Проверяем валидацию у времени"));
                                if (ckeckValidateImportFile(elm.Value.ToString(), dictValidFile[elm.Name.ToString()]))
                                    times_textBox.Text = elm.Value.ToString();
                                else
                                    times_textBox.Text = "";
                            }
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }

        private Boolean ckeckValidateImportFile(string fileName, List<string> namesValid)
        {
            try
            {
                using (ExcelPackage ep = new ExcelPackage(new FileInfo(fileName)))
                {
                    ExcelWorksheet ws = ep.Workbook.Worksheets[1];
                    for (int i = 0; i < namesValid.Count; i++) {
                        // Проверка идет по заголовку
                        if (ws.Cells[1, i + 1].Text != namesValid[i]) {
                            DialogResult dr = MessageBox.Show("Проверьте верно ли выбран верный файл (проверьте, чтобы первая строка начиналась c обозначения столбцов).\n Хотите продолжить с выбранным файлом?", "Внимание!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (dr == DialogResult.No)
                                return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(String.Format("Произошла ошибка! \n Ошибка {0}", ex.Message));
                MessageBox.Show(String.Format("Во вермя подгрузки файла произошла ошибка, проверьте существование файла и доступ до него. \n Ошибка: {0}", ex.Message), "Внимание!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void getMetalColors(String fileName)
        {
            //Генерим цвета для номенклатуры
            logger.Info("Генерим цвета для номенклатуры");
            ExcelPackage ep = new ExcelPackage(new FileInfo(fileName));
            ExcelWorksheet ws = ep.Workbook.Worksheets[1];
            int countRow = ws.Dimension.End.Row;
            int countColumn = ws.Dimension.End.Column;

            XDocument xDoc = XDocument.Load(cfg);
            XElement xCFG = xDoc.Element("cfg");
            XElement xMetalColors = xCFG.Element("metalcolors");
            //Очищаем предыдущие настройки для будующего поколения :D
            xMetalColors.RemoveAll();

            Random rnd = new Random();

            for (int rowNum = 2; rowNum <= countRow; rowNum++)
            {
                var id = ws.Cells[rowNum, 1];
                var val = ws.Cells[rowNum, 2];

                // Выбираем рандомно цвет и записываем в конфиг
                XElement xElement = new XElement("metalcolor",
                    new XElement("id", id.Text),
                    new XElement("metal", val.Text),
                    new XElement("color", String.Format("#{0}", ColourValues[rnd.Next(0, ColourValues.Length - 1)])));
                xMetalColors.Add(xElement);
            }
            xDoc.Save(cfg);
        }

        private void createCfgXML()
        {
            // Генерим новый конфиг файл, если не найшли предыдущий :D
            XDocument xdoc = new XDocument(new XElement("cfg",
                new XElement("sourcefiles",
                    new XElement("machine_tools", null),
                    new XElement("nomenclatures", null),
                    new XElement("parties", null),
                    new XElement("times", null)),
                new XElement("metalcolors", null)));
            xdoc.Save(cfg);
        }

        private String getFullNameFile()
        {
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
            saveToCFG();
        }

        private Boolean saveToCFG()
        {
            //Сохраняем конфиг файл
            logger.Info("Сохраняем конфиг файл");
            try
            {
                if (machine_tools_textBox.Text == "" && nomenclatures_textBox.Text == "" && parties_textBox.Text == "" && times_textBox.Text == "")
                {
                    MessageBox.Show("Есть незаполненные поля, просьба заполнить все поля, для корректной работы ПО!", "Внимание!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                XDocument xDoc = XDocument.Load(cfg);
                XElement xCFG = xDoc.Element("cfg");

                // Записываем в конфиг данные о выбранных файлах
                foreach (XElement elm in xCFG.Elements("sourcefiles").Nodes<XElement>())
                {
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
                xDoc.Save(cfg);
                getMetalColors(nomenclatures_textBox.Text);
                MessageBox.Show("Данные успешно сохранены!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                logger.Error(String.Format("Произошла ошибка! \n Ошибка {0}", ex.Message));
                MessageBox.Show(String.Format("Во вермя сохранения произошла ошибка! \n Ошибка: {0}", ex.Message), "Внимание!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            return true;
        }

        private void commit_Click(object sender, EventArgs e)
        {
            if (saveToCFG())
            {
                logger.Info("Закрываем форму selectFilesImport");
                this.Close();
            }
        }
        
    }
}
