namespace IDCardClieck.Controls
{
    partial class RealTime
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panelEnhanced1 = new IDCardClieck.Controls.PanelEnhanced();
            this.lbl_time = new System.Windows.Forms.Label();
            this.panelEnhanced1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panelEnhanced1
            // 
            this.panelEnhanced1.BackgroundImage = global::IDCardClieck.Properties.Resources.顶部背景_logo_文字;
            this.panelEnhanced1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelEnhanced1.Controls.Add(this.lbl_time);
            this.panelEnhanced1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEnhanced1.Location = new System.Drawing.Point(0, 0);
            this.panelEnhanced1.Name = "panelEnhanced1";
            this.panelEnhanced1.Size = new System.Drawing.Size(1024, 110);
            this.panelEnhanced1.TabIndex = 3;
            // 
            // lbl_time
            // 
            this.lbl_time.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_time.AutoSize = true;
            this.lbl_time.BackColor = System.Drawing.Color.Transparent;
            this.lbl_time.Font = new System.Drawing.Font("黑体", 15F, System.Drawing.FontStyle.Bold);
            this.lbl_time.ForeColor = System.Drawing.Color.White;
            this.lbl_time.Location = new System.Drawing.Point(534, 64);
            this.lbl_time.Name = "lbl_time";
            this.lbl_time.Size = new System.Drawing.Size(284, 20);
            this.lbl_time.TabIndex = 0;
            this.lbl_time.Text = "                         ";
            this.lbl_time.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // RealTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelEnhanced1);
            this.Name = "RealTime";
            this.Size = new System.Drawing.Size(1024, 110);
            this.Load += new System.EventHandler(this.RealTime_Load);
            this.panelEnhanced1.ResumeLayout(false);
            this.panelEnhanced1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private PanelEnhanced panelEnhanced1;
        private System.Windows.Forms.Label lbl_time;
    }
}
