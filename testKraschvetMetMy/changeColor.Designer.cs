namespace testKraschvetMetMy
{
    partial class changeColor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pbChangeColors = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbChangeColors)).BeginInit();
            this.SuspendLayout();
            // 
            // pbChangeColors
            // 
            this.pbChangeColors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbChangeColors.Location = new System.Drawing.Point(2, 2);
            this.pbChangeColors.Name = "pbChangeColors";
            this.pbChangeColors.Size = new System.Drawing.Size(262, 98);
            this.pbChangeColors.TabIndex = 0;
            this.pbChangeColors.TabStop = false;
            this.pbChangeColors.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbChangeColors_MouseClick);
            // 
            // changeColor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(265, 102);
            this.Controls.Add(this.pbChangeColors);
            this.Name = "changeColor";
            this.Text = "Смена цвета";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.changeColor_FormClosed);
            this.Shown += new System.EventHandler(this.changeColor_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pbChangeColors)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbChangeColors;
    }
}