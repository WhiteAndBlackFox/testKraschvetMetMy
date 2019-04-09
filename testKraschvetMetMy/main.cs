using OfficeOpenXml;
using System;
using System.Collections;
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

        private ArrayList arrNomenclatures;
        private ArrayList arrTools;
        private ArrayList arrSchedule;
        private List<parties> lParties;

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
                XDocument xDoc;
                try
                {
                    xDoc = XDocument.Load(cfg);
                    XElement xCFG = xDoc.Element("cfg");

                    foreach (XElement elm in xCFG.Elements("sourcefiles").Nodes<XElement>())
                    {
                        switch (elm.Name.ToString())
                        {
                            case "nomenclatures":
                                {
                                    fillingArraysOfDataNomenclatures(elm.Value.ToString());
                                    break;
                                }
                            case "parties":
                                {
                                    fillingArraysOfDataParties(elm.Value.ToString());
                                    break;
                                }
                            case "machine_tools":
                                {
                                    fillingArraysOfDataMachineTools(elm.Value.ToString());
                                    break;
                                }
                            case "times":
                                {
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
                    MessageBox.Show(String.Format("Конфигурационный файл не найден или поврежден, проведите настройку повторно! \n Ошибка {0}", ex.Message), "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                calculation();

            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Произошла ошибка! \n Ошибка {0}", ex.Message), "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void calculation()
        {
            arrSchedule = new ArrayList();

            foreach (parties p in lParties)
            {
                arrTools.Sort(new machineToolComparerCurrentLoad());

                for (int i = 0; i < arrTools.Count; i++)
                {
                    if (((machineTools)arrTools[i]).checkNomenclature(p.nomenclatures.idNomenclatures))
                    {
                        workSchedule ws = new workSchedule(p, (machineTools)arrTools[i], ((machineTools)arrTools[i]).getCurrentLoad());
                        ((machineTools)arrTools[i]).AssignJob(p);
                        arrSchedule.Add(ws);
                    }
                    break;
                }
            }
            if (arrSchedule.Count > 0)
            {
                IComparer myComparer = new machineToolComparerID();
                arrTools.Sort(myComparer);

                gResults.Rows.Clear();
                ShowReport();
                PrintResult();
            }
        }

        private void PrintResult()
        {
            for (int i = 0; i < arrSchedule.Count; ++i)
            {
                gResults.Rows.Add(
                    ((workSchedule)arrSchedule[i]).parties.idParties,
                    ((workSchedule)arrSchedule[i]).parties.nomenclatures.nameNomenclatures,
                    ((workSchedule)arrSchedule[i]).machineTools.getName(),
                    ((workSchedule)arrSchedule[i]).timeStart,
                    ((workSchedule)arrSchedule[i]).timeStart + ((workSchedule)arrSchedule[i]).machineTools.getTimeForProcessingById(((workSchedule)arrSchedule[i]).parties.nomenclatures.idNomenclatures)
                    );
            }
        }

        private void ShowReport()
        {
            Bitmap bmp = new Bitmap(pbReport.Width, pbReport.Height, pbReport.CreateGraphics());
            Graphics graphics = Graphics.FromImage(bmp);

            int iLegendY = 25;
            int iMarginX = 0;
            int iToolNamesX = 60;

            Font font = new Font("Arial", 8);
            Pen pen = new Pen(Color.Black, 1);
            Pen pen2 = new Pen(Color.Black, 2);

            ArrayList arrClr = new ArrayList();

            XDocument xDoc = XDocument.Load(cfg);
            XElement xCFG = xDoc.Element("cfg");

            foreach (XElement elm in xCFG.Elements("metalcolors").Nodes<XElement>())
            {
                int id = Int32.Parse(elm.Element("id").Value.ToString());
                string metal = elm.Element("metal").Value.ToString();
                string color = elm.Element("color").Value.ToString();
                ArrayList al = new ArrayList() { metal, color };
                arrClr.Insert(id, al);
            }

            for (int i = 0; i < arrNomenclatures.Count; ++i)
            {
                ArrayList al = (ArrayList)arrClr[i];
                Brush brush = new SolidBrush(ColorTranslator.FromHtml(String.Format("#{0}", al[1])));
                graphics.FillRectangle(brush, iToolNamesX + iMarginX + i * 60, 0, 60, iLegendY - 5);
                graphics.DrawString(((nomenclatures)arrNomenclatures[i]).nameNomenclatures, font, Brushes.Black, iToolNamesX + iMarginX + i * 60 + 5, 0 + 5);
            }

            int iReportZoneHeigth = pbReport.Height - iLegendY - 20;
            int iToolHeight = iReportZoneHeigth / arrTools.Count;

            float fMaxLoad = 0;
            for (int i = 0; i < arrTools.Count; ++i)
            {
                if (((machineTools)arrTools[i]).getCurrentLoad() > fMaxLoad) fMaxLoad = ((machineTools)arrTools[i]).getCurrentLoad();
            }
            fMaxLoad += 60;
            int iReportZoneWidth = pbReport.Width - iMarginX - iToolNamesX;
            float fScaleCoeff = iReportZoneWidth / fMaxLoad;

            for (int i = 0; i < arrTools.Count; ++i)
            {
                graphics.DrawLine(pen, 0, iLegendY + (i + 1) * iToolHeight, pbReport.Width, iLegendY + (i + 1) * iToolHeight);
                graphics.DrawString(((machineTools)arrTools[i]).getName(), new Font("Times New Roman", 10), Brushes.DarkCyan, iMarginX, iLegendY + i * iToolHeight + iToolHeight / 2 - 5);

                for (int j = 1; j <= fMaxLoad / 10; ++j)
                {
                    if (j % 5 == 0)
                    {
                        graphics.DrawLine(pen2, iToolNamesX + j * 10 * fScaleCoeff, iLegendY + (i + 1) * iToolHeight - 5, iToolNamesX + j * 10 * fScaleCoeff, iLegendY + (i + 1) * iToolHeight + 5);
                        graphics.DrawString(String.Format("{0}", j * 10), new Font("Arial Narrow", 8), Brushes.Black, iToolNamesX + j * 10 * fScaleCoeff - 2, iLegendY + (i + 1) * iToolHeight + 5);
                    }
                    else
                    {
                        graphics.DrawLine(pen, iToolNamesX + j * 10 * fScaleCoeff, iLegendY + (i + 1) * iToolHeight - 3, iToolNamesX + j * 10 * fScaleCoeff, iLegendY + (i + 1) * iToolHeight + 3);
                    }
                }
            }

            graphics.DrawLine(pen, iToolNamesX, iLegendY, iToolNamesX, iLegendY + iReportZoneHeigth + 20);

            int iLoadHeight = iToolHeight / 3;
            for (int i = 0; i < arrTools.Count; ++i)
            {
                int iCurrLoad = 0;
                for (int j = 0; j < ((machineTools)arrTools[i]).arrWorks.Count; ++j)
                {
                    machineTools mt = (machineTools)arrTools[i];
                    Brush brush = new SolidBrush(ColorTranslator.FromHtml(String.Format("#{0}", ((ArrayList)arrClr[((parties)mt.arrWorks[j]).nomenclatures.idNomenclatures])[1])));
                    graphics.FillRectangle(brush,
                        iToolNamesX + iCurrLoad * fScaleCoeff + 1,
                        iLegendY + (i + 1) * iToolHeight - iLoadHeight - 7,
                        mt.getTimeForProcessingById(((parties)mt.arrWorks[j]).nomenclatures.idNomenclatures) * fScaleCoeff,
                        iLoadHeight
                        );
                    graphics.DrawLine(pen,
                        iToolNamesX + (iCurrLoad) * fScaleCoeff,
                        iLegendY + (i + 1) * iToolHeight - iLoadHeight - 7,
                        iToolNamesX + (iCurrLoad) * fScaleCoeff,
                        iLegendY + (i + 1) * iToolHeight - 6
                        );
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
            XDocument xDoc = XDocument.Load(cfg);
            XElement xCFG = xDoc.Element("cfg");

            if (xCFG.Elements("metalcolors").Nodes<XElement>().Count() > 0) {
                changeColor cc = new changeColor();
                cc.cfg = this.cfg;
                cc.ShowDialog();
            }
        }
    }
}
