namespace testKraschvetMetMy
{
    partial class main
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.экспортРезультатаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.выходToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.расчетToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gResults = new System.Windows.Forms.DataGridView();
            this.PartyID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Nomenclature = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Tool = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Begin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.End = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lGrid = new System.Windows.Forms.Label();
            this.pbReport = new System.Windows.Forms.PictureBox();
            this.lpbReport = new System.Windows.Forms.Label();
            this.сменаЦветаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbReport)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлыToolStripMenuItem,
            this.расчетToolStripMenuItem,
            this.сменаЦветаToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(572, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлыToolStripMenuItem
            // 
            this.файлыToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.настройкиToolStripMenuItem,
            this.экспортРезультатаToolStripMenuItem,
            this.toolStripSeparator1,
            this.выходToolStripMenuItem1});
            this.файлыToolStripMenuItem.Name = "файлыToolStripMenuItem";
            this.файлыToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлыToolStripMenuItem.Text = "Файл";
            // 
            // настройкиToolStripMenuItem
            // 
            this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
            this.настройкиToolStripMenuItem.Text = "Файлы с данными для импорта";
            this.настройкиToolStripMenuItem.Click += new System.EventHandler(this.настройкиToolStripMenuItem_Click);
            // 
            // экспортРезультатаToolStripMenuItem
            // 
            this.экспортРезультатаToolStripMenuItem.Name = "экспортРезультатаToolStripMenuItem";
            this.экспортРезультатаToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
            this.экспортРезультатаToolStripMenuItem.Text = "Экспорт результата";
            this.экспортРезультатаToolStripMenuItem.Click += new System.EventHandler(this.экспортРезультатаToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(245, 6);
            // 
            // выходToolStripMenuItem1
            // 
            this.выходToolStripMenuItem1.Name = "выходToolStripMenuItem1";
            this.выходToolStripMenuItem1.Size = new System.Drawing.Size(248, 22);
            this.выходToolStripMenuItem1.Text = "Выход";
            this.выходToolStripMenuItem1.Click += new System.EventHandler(this.выходToolStripMenuItem1_Click);
            // 
            // расчетToolStripMenuItem
            // 
            this.расчетToolStripMenuItem.Name = "расчетToolStripMenuItem";
            this.расчетToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.расчетToolStripMenuItem.Text = "Расчет";
            this.расчетToolStripMenuItem.Click += new System.EventHandler(this.расчетToolStripMenuItem_Click);
            // 
            // gResults
            // 
            this.gResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PartyID,
            this.Nomenclature,
            this.Tool,
            this.Begin,
            this.End});
            this.gResults.Location = new System.Drawing.Point(12, 44);
            this.gResults.Name = "gResults";
            this.gResults.ReadOnly = true;
            this.gResults.Size = new System.Drawing.Size(546, 384);
            this.gResults.TabIndex = 1;
            // 
            // PartyID
            // 
            this.PartyID.HeaderText = "Номер партии";
            this.PartyID.Name = "PartyID";
            this.PartyID.ReadOnly = true;
            // 
            // Nomenclature
            // 
            this.Nomenclature.HeaderText = "Номенклатура";
            this.Nomenclature.Name = "Nomenclature";
            this.Nomenclature.ReadOnly = true;
            // 
            // Tool
            // 
            this.Tool.HeaderText = "Оборудование";
            this.Tool.Name = "Tool";
            this.Tool.ReadOnly = true;
            // 
            // Begin
            // 
            this.Begin.HeaderText = "Начало этапа обработки";
            this.Begin.Name = "Begin";
            this.Begin.ReadOnly = true;
            // 
            // End
            // 
            this.End.HeaderText = "Конец этапа обработки";
            this.End.Name = "End";
            this.End.ReadOnly = true;
            // 
            // lGrid
            // 
            this.lGrid.AutoSize = true;
            this.lGrid.Location = new System.Drawing.Point(13, 28);
            this.lGrid.Name = "lGrid";
            this.lGrid.Size = new System.Drawing.Size(116, 13);
            this.lGrid.TabIndex = 2;
            this.lGrid.Text = "Таблица с расчетами";
            // 
            // pbReport
            // 
            this.pbReport.Location = new System.Drawing.Point(12, 485);
            this.pbReport.Name = "pbReport";
            this.pbReport.Size = new System.Drawing.Size(546, 273);
            this.pbReport.TabIndex = 4;
            this.pbReport.TabStop = false;
            // 
            // lpbReport
            // 
            this.lpbReport.AutoSize = true;
            this.lpbReport.Location = new System.Drawing.Point(13, 469);
            this.lpbReport.Name = "lpbReport";
            this.lpbReport.Size = new System.Drawing.Size(111, 13);
            this.lpbReport.TabIndex = 5;
            this.lpbReport.Text = "График с расчетами";
            // 
            // сменаЦветаToolStripMenuItem
            // 
            this.сменаЦветаToolStripMenuItem.Name = "сменаЦветаToolStripMenuItem";
            this.сменаЦветаToolStripMenuItem.Size = new System.Drawing.Size(88, 20);
            this.сменаЦветаToolStripMenuItem.Text = "Смена цвета";
            this.сменаЦветаToolStripMenuItem.Click += new System.EventHandler(this.сменаЦветаToolStripMenuItem_Click);
            // 
            // main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 787);
            this.Controls.Add(this.lpbReport);
            this.Controls.Add(this.pbReport);
            this.Controls.Add(this.lGrid);
            this.Controls.Add(this.gResults);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "main";
            this.Text = "Тестовое задание по составлению плана загрузки производственного оборудования пар" +
    "тиями поступающего сырья";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbReport)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem экспортРезультатаToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem расчетToolStripMenuItem;
        private System.Windows.Forms.DataGridView gResults;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartyID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nomenclature;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tool;
        private System.Windows.Forms.DataGridViewTextBoxColumn Begin;
        private System.Windows.Forms.DataGridViewTextBoxColumn End;
        private System.Windows.Forms.Label lGrid;
        private System.Windows.Forms.PictureBox pbReport;
        private System.Windows.Forms.Label lpbReport;
        private System.Windows.Forms.ToolStripMenuItem сменаЦветаToolStripMenuItem;
    }
}

