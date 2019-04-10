using NLog;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace testKraschvetMetMy
{
    public partial class main : Form
    {
        //Для логирования
        private static Logger logger = LogManager.GetCurrentClassLogger();

        //Конфиг файл
        private string cfg = "cfg.xml";

        //Массивы данных
        private ArrayList arrNomenclatures;
        private ArrayList arrTools;
        private ArrayList arrSchedule;
        private List<parties> lParties;

        public main()
        {
            InitializeComponent();
            logger.Info("Вход в приложение");
        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Вызов формы selectFilesImport
            logger.Info("Вызываем форму selectFilesImport для выбора файлов с данными");
            selectFilesImport s = new selectFilesImport();
            s.cfg = this.cfg;
            s.ShowDialog();
        }

        private void выходToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Узнаем хочет ли человек выйти или нет :D
            DialogResult dr = MessageBox.Show("Вы уверены, что хотите выйти?", "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dr == DialogResult.Yes)
            {
                logger.Info("Выход из приложения");
                Environment.Exit(0);
            }
        }

        private void расчетToolStripMenuItem_Click(object sender, EventArgs e)
        {

            logger.Info("Начало расчета");
            try
            {
                XDocument xDoc;
                try
                {
                    xDoc = XDocument.Load(cfg);
                    XElement xCFG = xDoc.Element("cfg");

                    //Заполнение массивов для расчета
                    logger.Info("Заполнение массивов для расчета");
                    foreach (XElement elm in xCFG.Elements("sourcefiles").Nodes<XElement>())
                    {
                        switch (elm.Name.ToString())
                        {
                            case "nomenclatures":
                                {
                                    logger.Info("Заполнение массива с номенклатурой");
                                    fillingArraysOfDataNomenclatures(elm.Value.ToString());
                                    break;
                                }
                            case "parties":
                                {
                                    logger.Info("Заполнение массива с партиями");
                                    fillingArraysOfDataParties(elm.Value.ToString());
                                    break;
                                }
                            case "machine_tools":
                                {
                                    logger.Info("Заполнение массива с оборудованием");
                                    fillingArraysOfDataMachineTools(elm.Value.ToString());
                                    break;
                                }
                            case "times":
                                {
                                    logger.Info("Заполнение массива со временем");
                                    fillingArraysOfDataTimes(elm.Value.ToString());
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    }

                }
                catch (Exception ex)
                {
                    logger.Error(String.Format("Конфигурационный файл не найден или поврежден! \n Ошибка {0}", ex.Message));
                    MessageBox.Show(String.Format("Конфигурационный файл не найден или поврежден, проведите настройку повторно! \n Ошибка {0}", ex.Message), "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                //Функция расчета
                calculation();

                logger.Info("Закончили с расчетом и отображением");

            }
            catch (Exception ex)
            {
                logger.Error(String.Format("Произошла ошибка! \n Ошибка {0}", ex.Message));
                MessageBox.Show(String.Format("Произошла ошибка! \n Ошибка {0}", ex.Message), "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void calculation()
        {
            logger.Info("Распределяем работу!");
            arrSchedule = new ArrayList();

            //Для каждого элемента из очереди партий
            foreach (parties p in lParties)
            {
                //Сортируем массив оборудования по признаку возрастания текущей занятости
                arrTools.Sort(new machineToolComparerCurrentLoad());

                foreach (machineTools mt in arrTools) {
                    // Находим наименее занятую машину, подходящую для обработки номенклатуры из партии
                    if (mt.checkNomenclature(p.nomenclatures.idNomenclatures))
                    {
                        workSchedule ws = new workSchedule(p, mt, mt.getCurrentLoad());
                        // Назначаем самой машине
                        mt.AssignJob(p);
                        // В общий план работ
                        arrSchedule.Add(ws);
                    }
                    break;
                }
            }
            // Если удалось хоть одну партию вписать в план, то стартуем вывод результатов
            if (arrSchedule.Count > 0)
            {
                // Предварительно сортируем машины по ID, так как перепутались в ходе назначения заданий
                IComparer myComparer = new machineToolComparerID();
                arrTools.Sort(myComparer);

                gResults.Rows.Clear();
                ShowReport();
                PrintResult();
            }
        }

        private void PrintResult()
        {
            // Добавляем результат в таблицу
            logger.Info("Добавляем результат в таблицу");
            foreach (workSchedule ws in arrSchedule) {
                gResults.Rows.Add(
                    ws.parties.idParties,
                    ws.parties.nomenclatures.nameNomenclatures,
                    ws.machineTools.getName(),
                    ws.timeStart,
                    ws.timeStart + ws.machineTools.getTimeForProcessingById(ws.parties.nomenclatures.idNomenclatures)
                    );
            }

            logger.Info("Закончили добавлять в таблицу");
        }

        private void ShowReport()
        {
            logger.Info("Отрисовка графиков!");

            Bitmap bmp = new Bitmap(pbReport.Width, pbReport.Height, pbReport.CreateGraphics());
            Graphics graphics = Graphics.FromImage(bmp);

            int iLegendY = 25;      // Высота легенды номенклатур
            int iMarginX = 0;       // Отступ слева
            int iToolNamesX = 60;   // Ширина легенды оборудования

            Font font = new Font("Arial", 8);
            Pen pen = new Pen(Color.Black, 1);
            Pen pen2 = new Pen(Color.Black, 2);

            ArrayList arrClr = new ArrayList();

            XDocument xDoc = XDocument.Load(cfg);
            XElement xCFG = xDoc.Element("cfg");

            // Заполняем массив цветов
            foreach (XElement elm in xCFG.Elements("metalcolors").Nodes<XElement>())
            {
                int id = Int32.Parse(elm.Element("id").Value.ToString());
                string metal = elm.Element("metal").Value.ToString();
                string color = elm.Element("color").Value.ToString();
                ArrayList al = new ArrayList() { metal, color };
                arrClr.Insert(id, al);
            }

            // Рисуем легенду к графику - номенклатуры
            for (int i = 0; i < arrNomenclatures.Count; ++i)
            {
                ArrayList al = (ArrayList)arrClr[i];
                Brush brush = new SolidBrush(ColorTranslator.FromHtml(String.Format("{0}", al[1])));
                graphics.FillRectangle(brush, iToolNamesX + iMarginX + i * 60, 0, 60, iLegendY - 5);
                graphics.DrawString(((nomenclatures)arrNomenclatures[i]).nameNomenclatures, font, Brushes.Black, iToolNamesX + iMarginX + i * 60 + 5, 0 + 5);
            }

            int iReportZoneHeigth = pbReport.Height - iLegendY - 20;    // Высота зоны всего графика загрузки оборудования
            int iToolHeight = iReportZoneHeigth / arrTools.Count;       // Высота зоны одного экземпляра оборудования

            //  Рассчитываем множитель масштаба по оси Х
            // Нашли максимальную загрузку, добавили отступ 60, собрали коэффициент как отношение ширины клиентской зоны графика к максимальной загрузке
            float fMaxLoad = 0;
            foreach (machineTools mt in arrTools) {
                if (mt.getCurrentLoad() > fMaxLoad)
                    fMaxLoad = mt.getCurrentLoad();
            }

            fMaxLoad += 60;
            int iReportZoneWidth = pbReport.Width - iMarginX - iToolNamesX; // Ширина зоны графика загрузки оборудования
            float fScaleCoeff = iReportZoneWidth / fMaxLoad;                // Множитель масштаба

            // Пишем названия машин и рисуем горизонтальные оси
            for (int i = 0; i < arrTools.Count; ++i)
            {
                // Горизонтальная ось
                graphics.DrawLine(pen, 0, iLegendY + (i + 1) * iToolHeight, pbReport.Width, iLegendY + (i + 1) * iToolHeight);
                // Название машины
                graphics.DrawString(((machineTools)arrTools[i]).getName(), new Font("Times New Roman", 10), Brushes.DarkCyan, iMarginX, iLegendY + i * iToolHeight + iToolHeight / 2 - 5);

                // Рисуем шкалу
                for (int j = 1; j <= fMaxLoad / 10; ++j)
                {
                    if (j % 5 == 0)
                    {
                        // Каждая пятая засечка большая и с подписью
                        graphics.DrawLine(pen2, iToolNamesX + j * 10 * fScaleCoeff, iLegendY + (i + 1) * iToolHeight - 5, iToolNamesX + j * 10 * fScaleCoeff, iLegendY + (i + 1) * iToolHeight + 5);
                        graphics.DrawString(String.Format("{0}", j * 10), new Font("Arial Narrow", 8), Brushes.Black, iToolNamesX + j * 10 * fScaleCoeff - 2, iLegendY + (i + 1) * iToolHeight + 5);
                    }
                    else
                    {
                        graphics.DrawLine(pen, iToolNamesX + j * 10 * fScaleCoeff, iLegendY + (i + 1) * iToolHeight - 3, iToolNamesX + j * 10 * fScaleCoeff, iLegendY + (i + 1) * iToolHeight + 3);
                    }
                }
            }

            // Рисуем вертикальную ось (момент времени 0)
            graphics.DrawLine(pen, iToolNamesX, iLegendY, iToolNamesX, iLegendY + iReportZoneHeigth + 20);

            int iLoadHeight = iToolHeight / 3;  // Высота элемента графика загрузки
            // Рисуем загрузку машин
            for (int i = 0; i < arrTools.Count; ++i)
            {
                int iCurrLoad = 0;
                for (int j = 0; j < ((machineTools)arrTools[i]).arrWorks.Count; ++j)
                {
                    machineTools mt = (machineTools)arrTools[i];
                    // Цвет выбираем такой же, как был в легенде номенклатур
                    Brush brush = new SolidBrush(ColorTranslator.FromHtml(String.Format("{0}", ((ArrayList)arrClr[((parties)mt.arrWorks[j]).nomenclatures.idNomenclatures])[1])));
                    // Прямоугольник загрузки
                    graphics.FillRectangle(brush,
                        iToolNamesX + iCurrLoad * fScaleCoeff + 1,
                        iLegendY + (i + 1) * iToolHeight - iLoadHeight - 7,
                        mt.getTimeForProcessingById(((parties)mt.arrWorks[j]).nomenclatures.idNomenclatures) * fScaleCoeff,
                        iLoadHeight
                        );
                    // Разделительная полосочка
                    graphics.DrawLine(pen,
                        iToolNamesX + (iCurrLoad) * fScaleCoeff,
                        iLegendY + (i + 1) * iToolHeight - iLoadHeight - 7,
                        iToolNamesX + (iCurrLoad) * fScaleCoeff,
                        iLegendY + (i + 1) * iToolHeight - 6
                        );
                    // Номер партии
                    graphics.DrawString(String.Format("{0}",
                        ((parties)mt.arrWorks[j]).idParties),
                        font,
                        Brushes.Black,
                        iToolNamesX + iCurrLoad * fScaleCoeff + 2,
                        iLegendY + (i + 1) * iToolHeight - iLoadHeight - 7 + 2
                        );
                    iCurrLoad += mt.getTimeForProcessingById(((parties)mt.arrWorks[j]).nomenclatures.idNomenclatures);
                }
            }

            pbReport.Image = bmp;
            logger.Info("Закончили рисовать!");
           
        }

        private void fillingArraysOfDataTimes(string fileName)
        {
            ExcelPackage ep = new ExcelPackage(new FileInfo(fileName));
            ExcelWorksheet ws = ep.Workbook.Worksheets[1];
            int countRow = ws.Dimension.End.Row;
            int countColumn = ws.Dimension.End.Column;

            for (int rowNum = 2; rowNum <= countRow; rowNum++)
            {
                int idTools = findToolByID(Int32.Parse(ws.Cells[rowNum, 1].Value.ToString()));
                int idNom = findNomenclatureByID(Int32.Parse(ws.Cells[rowNum, 2].Value.ToString()));

                if (idTools != -1 && idNom != -1)
                {
                    times t = new times((nomenclatures)arrNomenclatures[idNom], Int32.Parse(ws.Cells[rowNum, 3].Value.ToString()));
                    ((machineTools)arrTools[idTools]).addTime(t);
                }
            }
        }

        private int findToolByID(int id)
        {
            for (int i = 0; i < arrTools.Count; ++i)
            {
                if (((machineTools)arrTools[i]).getID() == id)
                    return i;
            }
            return -1;
        }

        private void fillingArraysOfDataMachineTools(string fileName)
        {
            arrTools = new ArrayList();

            ExcelPackage ep = new ExcelPackage(new FileInfo(fileName));
            ExcelWorksheet ws = ep.Workbook.Worksheets[1];
            int countRow = ws.Dimension.End.Row;
            int countColumn = ws.Dimension.End.Column;

            for (int rowNum = 2; rowNum <= countRow; rowNum++)
            {
                arrTools.Add(new machineTools(Int32.Parse(ws.Cells[rowNum, 1].Value.ToString()), ws.Cells[rowNum, 2].Value.ToString()));
            }

        }

        private void fillingArraysOfDataParties(string fileName)
        {
            lParties = new List<parties>();

            ExcelPackage ep = new ExcelPackage(new FileInfo(fileName));
            ExcelWorksheet ws = ep.Workbook.Worksheets[1];
            int countRow = ws.Dimension.End.Row;
            int countColumn = ws.Dimension.End.Column;

            for (int rowNum = 2; rowNum <= countRow; rowNum++)
            {
                int idNom = findNomenclatureByID(Int32.Parse(ws.Cells[rowNum, 2].Value.ToString()));
                if (idNom != -1)
                {
                    parties p = new parties(Int32.Parse(ws.Cells[rowNum, 1].Value.ToString()), ((nomenclatures)arrNomenclatures[idNom]));
                    lParties.Add(p);
                }
            }
        }

        private int findNomenclatureByID(int id)
        {
            for (int i = 0; i < arrNomenclatures.Count; ++i)
            {
                if (((nomenclatures)arrNomenclatures[i]).idNomenclatures == id)
                    return i;
            }
            return -1;
        }

        private void fillingArraysOfDataNomenclatures(string fileName)
        {
            arrNomenclatures = new ArrayList();

            ExcelPackage ep = new ExcelPackage(new FileInfo(fileName));
            ExcelWorksheet ws = ep.Workbook.Worksheets[1];
            int countRow = ws.Dimension.End.Row;
            int countColumn = ws.Dimension.End.Column;

            for (int rowNum = 2; rowNum <= countRow; rowNum++)
            {
                nomenclatures n = new nomenclatures(Int32.Parse(ws.Cells[rowNum, 1].Value.ToString()), ws.Cells[rowNum, 2].Value.ToString());
                this.arrNomenclatures.Add(n);
            }

        }

        private void сменаЦветаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Вызов формы changeColor для смены цветов
            logger.Info("Вызов формы changeColor для смены цветов");
            try
            {
                XDocument xDoc = XDocument.Load(cfg);
                XElement xCFG = xDoc.Element("cfg");

                if (xCFG.Elements("metalcolors").Nodes<XElement>().Count() > 0)
                {
                    changeColor cc = new changeColor();
                    cc.cfg = this.cfg;
                    cc.ShowDialog();
                }
                else
                {
                    logger.Error(String.Format("Конфигурационный файл не найден или поврежден, проведите настройку повторно!"));
                    MessageBox.Show(String.Format("Конфигурационный файл не найден или поврежден, проведите настройку повторно!"), "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex) {
                logger.Error(String.Format("Произошла ошибка! \n Ошибка {0}", ex.Message));
                MessageBox.Show(String.Format("Конфигурационный файл не найден или поврежден, проведите настройку повторно! \n Ошибка {0}", ex.Message), "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void экспортРезультатаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (arrSchedule != null)
            {
                logger.Info("Выгружаем результат");
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Лист Microsoft Excel (.xlsx) |*.xlsx";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string fn = sfd.FileName;
                    Console.WriteLine(fn);

                    // Проверяем существует файл или нет
                    if (File.Exists(fn))
                    {
                        try
                        {
                            // Удаляем находку
                            File.Delete(fn);
                        }
                        catch (Exception ex)
                        {
                            logger.Error(String.Format("Внимание! Не удалось создать файл для записи результатов. Возможно, файл открыт в другой программе, или недостаточно прав для записи. Ошибка: {0}", ex.Message));
                            MessageBox.Show("Внимание! Не удалось создать файл для записи результатов. Возможно, файл открыт в другой программе, или недостаточно прав для записи.", "Файл не создан", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                    ExcelPackage ep = new ExcelPackage(new FileInfo(fn));
                    var sheet = ep.Workbook.Worksheets.Add("Расписание обработки партий");

                    // Вывод результатов (общая таблица планирования)
                    sheet.Cells[1, 1].Value = "Номер партии";
                    sheet.Cells[2, 1].Value = "Номенклатура";
                    sheet.Cells[3, 1].Value = "Оборудование";
                    sheet.Cells[4, 1].Value = "Начало этапа обработки";
                    sheet.Cells[5, 1].Value = "Конец этапа обработки";

                    for (int i = 0; i < arrSchedule.Count; i++)
                    {
                        sheet.Cells[1, i + 2].Value = ((workSchedule)arrSchedule[i]).parties.idParties;
                        sheet.Cells[2, i + 2].Value = ((workSchedule)arrSchedule[i]).parties.nomenclatures.nameNomenclatures;
                        sheet.Cells[3, i + 2].Value = ((workSchedule)arrSchedule[i]).machineTools.getName();
                        sheet.Cells[4, i + 2].Value = ((workSchedule)arrSchedule[i]).timeStart;
                        sheet.Cells[5, i + 2].Value = ((workSchedule)arrSchedule[i]).timeStart + ((workSchedule)arrSchedule[i]).machineTools.getTimeForProcessingById(((workSchedule)arrSchedule[i]).parties.nomenclatures.idNomenclatures);
                    }

                    // Дополнительно в отдельные страницы впишем графики загрузки каждой машины
                    foreach (machineTools mt in arrTools)
                    {
                        string sheetName = String.Format("Расписание \"{0}\"", mt.getName());

                        var sheetTools = ep.Workbook.Worksheets.Add(sheetName);
                        sheetTools.Cells[1, 1].Value = "Номер партии";
                        sheetTools.Cells[2, 1].Value = "Номенклатура";
                        sheetTools.Cells[3, 1].Value = "Оборудование";
                        sheetTools.Cells[4, 1].Value = "Начало этапа обработки";
                        sheetTools.Cells[5, 1].Value = "Конец этапа обработки";

                        int t = 0;
                        for (int j = 0; j < mt.arrWorks.Count; ++j)
                        {
                            parties p = (parties)(mt.arrWorks[j]);
                            sheetTools.Cells[1, j + 2].Value = p.idParties;
                            sheetTools.Cells[2, j + 2].Value = p.nomenclatures.nameNomenclatures;
                            sheetTools.Cells[3, j + 2].Value = t;
                            sheetTools.Cells[4, j + 2].Value = (t += mt.getTimeForProcessingById(p.nomenclatures.idNomenclatures));
                        }
                    }

                    try
                    {
                        FileInfo fi = new FileInfo(fn);
                        ep.SaveAs(fi);
                        logger.Info(String.Format("Файл успешно выгружен: {0}", fi.FullName));
                        MessageBox.Show("Файл успешно сохранен.", "Файл создан", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(String.Format("Внимание! Не удалось создать файл для записи результатов. Возможно, файл открыт в другой программе, или недостаточно прав для записи. Ошибка: {0}", ex.Message));
                        MessageBox.Show("Внимание! Не удалось создать файл для записи результатов. Возможно, файл открыт в другой программе, или недостаточно прав для записи.", "Файл не создан", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else {
                logger.Error(String.Format("Для экспорта данных, необходимо сначало сделать \"Расчет\"!"));
                MessageBox.Show("Для экспорта данных, необходимо сначало сделать \"Расчет\"!", "Файл не создан", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}

