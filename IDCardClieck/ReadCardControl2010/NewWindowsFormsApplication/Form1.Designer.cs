

using GSFramework;
namespace WindowsFormsApplication
{
    partial class Form1
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btn_Start = new System.Windows.Forms.Button();
            this.btn_Stop = new System.Windows.Forms.Button();
            this.btn_Clear = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.显示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.newReadCard1 = new GSFramework.INVSReadCardActiveX();
            this.button2 = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Start
            // 
            this.btn_Start.Location = new System.Drawing.Point(205, 302);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(75, 27);
            this.btn_Start.TabIndex = 1;
            this.btn_Start.Text = "启动";
            this.btn_Start.UseVisualStyleBackColor = true;
            this.btn_Start.Click += new System.EventHandler(this.btn_Start_Click);
            // 
            // btn_Stop
            // 
            this.btn_Stop.Location = new System.Drawing.Point(286, 302);
            this.btn_Stop.Name = "btn_Stop";
            this.btn_Stop.Size = new System.Drawing.Size(75, 27);
            this.btn_Stop.TabIndex = 2;
            this.btn_Stop.Text = "停止";
            this.btn_Stop.UseVisualStyleBackColor = true;
            this.btn_Stop.Click += new System.EventHandler(this.btn_Stop_Click);
            // 
            // btn_Clear
            // 
            this.btn_Clear.Location = new System.Drawing.Point(367, 302);
            this.btn_Clear.Name = "btn_Clear";
            this.btn_Clear.Size = new System.Drawing.Size(75, 27);
            this.btn_Clear.TabIndex = 3;
            this.btn_Clear.Text = "清空";
            this.btn_Clear.UseVisualStyleBackColor = true;
            this.btn_Clear.Click += new System.EventHandler(this.btn_Clear_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "二代证读卡工具";
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.显示ToolStripMenuItem,
            this.退出EToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(116, 48);
            // 
            // 显示ToolStripMenuItem
            // 
            this.显示ToolStripMenuItem.Name = "显示ToolStripMenuItem";
            this.显示ToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.显示ToolStripMenuItem.Text = "显示(&S)";
            this.显示ToolStripMenuItem.Click += new System.EventHandler(this.显示ToolStripMenuItem_Click);
            // 
            // 退出EToolStripMenuItem
            // 
            this.退出EToolStripMenuItem.Name = "退出EToolStripMenuItem";
            this.退出EToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.退出EToolStripMenuItem.Text = "退出(&E)";
            this.退出EToolStripMenuItem.Click += new System.EventHandler(this.退出EToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 302);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(89, 27);
            this.button1.TabIndex = 1;
            this.button1.Text = "获取设备状态";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // newReadCard1
            // 
            this.newReadCard1.BackColor = System.Drawing.Color.Transparent;
            this.newReadCard1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("newReadCard1.BackgroundImage")));
            this.newReadCard1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.newReadCard1.Location = new System.Drawing.Point(12, 12);
            this.newReadCard1.Margin = new System.Windows.Forms.Padding(0);
            this.newReadCard1.MaximumSize = new System.Drawing.Size(430, 282);
            this.newReadCard1.MinimumSize = new System.Drawing.Size(430, 282);
            this.newReadCard1.Name = "newReadCard1";
            this.newReadCard1.Port = -1;
            this.newReadCard1.Size = new System.Drawing.Size(430, 282);
            this.newReadCard1.TabIndex = 4;
            this.newReadCard1.TimeOut = 1000;
            this.newReadCard1.OnDataBind += new GSFramework.INVSReadCardActiveX.ReadDataHandler(this.newReadCard1_OnDataBand);
            this.newReadCard1.OnStateChanged += new GSFramework.INVSReadCardActiveX.StateChangeHandler(this.newReadCard1_OnStateChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(107, 302);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(72, 27);
            this.button2.TabIndex = 5;
            this.button2.Text = "获取SAMID";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 337);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.newReadCard1);
            this.Controls.Add(this.btn_Clear);
            this.Controls.Add(this.btn_Stop);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btn_Start);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(470, 373);
            this.MinimumSize = new System.Drawing.Size(470, 373);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "通软科技身份证读取测试工具";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Start;
        private System.Windows.Forms.Button btn_Stop;
        private System.Windows.Forms.Button btn_Clear;
        private INVSReadCardActiveX newReadCard1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 显示ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出EToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

