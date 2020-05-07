namespace IDCardClieck.Forms
{
    partial class HomeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HomeForm));
            this.panel_home = new System.Windows.Forms.Panel();
            this.panel_home_fill = new System.Windows.Forms.Panel();
            this.panel_home_fill_fill = new System.Windows.Forms.Panel();
            this.myBtnExt1 = new IDCardClieck.Controls.MyBtnExt();
            this.panel_homt_top = new System.Windows.Forms.Panel();
            this.realTimeRight1 = new IDCardClieck.Controls.RealTimeRight();
            this.panel_home.SuspendLayout();
            this.panel_home_fill.SuspendLayout();
            this.panel_home_fill_fill.SuspendLayout();
            this.panel_homt_top.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_home
            // 
            this.panel_home.Controls.Add(this.panel_home_fill);
            this.panel_home.Controls.Add(this.panel_homt_top);
            this.panel_home.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_home.Location = new System.Drawing.Point(0, 0);
            this.panel_home.Name = "panel_home";
            this.panel_home.Size = new System.Drawing.Size(1008, 730);
            this.panel_home.TabIndex = 0;
            // 
            // panel_home_fill
            // 
            this.panel_home_fill.BackColor = System.Drawing.Color.White;
            this.panel_home_fill.Controls.Add(this.panel_home_fill_fill);
            this.panel_home_fill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_home_fill.Location = new System.Drawing.Point(0, 100);
            this.panel_home_fill.Name = "panel_home_fill";
            this.panel_home_fill.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.panel_home_fill.Size = new System.Drawing.Size(1008, 630);
            this.panel_home_fill.TabIndex = 1;
            // 
            // panel_home_fill_fill
            // 
            this.panel_home_fill_fill.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panel_home_fill_fill.BackgroundImage = global::IDCardClieck.Properties.Resources.微信图片_202004161526101;
            this.panel_home_fill_fill.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel_home_fill_fill.Controls.Add(this.myBtnExt1);
            this.panel_home_fill_fill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_home_fill_fill.Location = new System.Drawing.Point(0, 5);
            this.panel_home_fill_fill.Name = "panel_home_fill_fill";
            this.panel_home_fill_fill.Size = new System.Drawing.Size(1008, 625);
            this.panel_home_fill_fill.TabIndex = 2;
            this.panel_home_fill_fill.SizeChanged += new System.EventHandler(this.panel_home_fill_fill_SizeChanged);
            // 
            // myBtnExt1
            // 
            this.myBtnExt1.BackColor = System.Drawing.Color.White;
            this.myBtnExt1.BackgroundImage = global::IDCardClieck.Properties.Resources.开始查询;
            this.myBtnExt1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.myBtnExt1.BtnBackColor = System.Drawing.Color.White;
            this.myBtnExt1.BtnFont = new System.Drawing.Font("华文琥珀", 15F);
            this.myBtnExt1.BtnForeColor = System.Drawing.Color.White;
            this.myBtnExt1.BtnText = "";
            this.myBtnExt1.ConerRadius = 56;
            this.myBtnExt1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.myBtnExt1.FillColor = System.Drawing.Color.White;
            this.myBtnExt1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.myBtnExt1.ForeColor = System.Drawing.Color.White;
            this.myBtnExt1.IsRadius = true;
            this.myBtnExt1.IsShowRect = true;
            this.myBtnExt1.IsShowTips = false;
            this.myBtnExt1.Location = new System.Drawing.Point(0, 0);
            this.myBtnExt1.Margin = new System.Windows.Forms.Padding(0);
            this.myBtnExt1.Name = "myBtnExt1";
            this.myBtnExt1.RectColor = System.Drawing.Color.White;
            this.myBtnExt1.RectWidth = 0;
            this.myBtnExt1.Size = new System.Drawing.Size(176, 56);
            this.myBtnExt1.TabIndex = 3;
            this.myBtnExt1.TabStop = false;
            this.myBtnExt1.TipsText = "";
            this.myBtnExt1.BtnClick += new System.EventHandler(this.myBtnExt1_BtnClick_1);
            // 
            // panel_homt_top
            // 
            this.panel_homt_top.Controls.Add(this.realTimeRight1);
            this.panel_homt_top.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_homt_top.Location = new System.Drawing.Point(0, 0);
            this.panel_homt_top.Name = "panel_homt_top";
            this.panel_homt_top.Size = new System.Drawing.Size(1008, 100);
            this.panel_homt_top.TabIndex = 0;
            // 
            // realTimeRight1
            // 
            this.realTimeRight1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.realTimeRight1.Location = new System.Drawing.Point(0, 0);
            this.realTimeRight1.Name = "realTimeRight1";
            this.realTimeRight1.Size = new System.Drawing.Size(1008, 100);
            this.realTimeRight1.TabIndex = 0;
            // 
            // HomeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.panel_home);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HomeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "居民健康体检查询终端";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HomeForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel_home.ResumeLayout(false);
            this.panel_home_fill.ResumeLayout(false);
            this.panel_home_fill_fill.ResumeLayout(false);
            this.panel_homt_top.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_home;
        private System.Windows.Forms.Panel panel_homt_top;
        private System.Windows.Forms.Panel panel_home_fill;
        private System.Windows.Forms.Panel panel_home_fill_fill;
        private Controls.MyBtnExt myBtnExt1;
        private Controls.RealTimeRight realTimeRight1;
    }
}