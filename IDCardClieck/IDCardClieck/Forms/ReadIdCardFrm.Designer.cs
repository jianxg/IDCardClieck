namespace IDCardClieck.Forms
{
    partial class ReadIdCardFrm
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
            this.panel_home_fill = new System.Windows.Forms.Panel();
            this.panel_home_fill_fill = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_Start = new System.Windows.Forms.Button();
            this.lbl_msg = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lbl_Code = new System.Windows.Forms.Label();
            this.lbl_Names = new System.Windows.Forms.Label();
            this.lbl_Gender = new System.Windows.Forms.Label();
            this.lbl_Folk = new System.Windows.Forms.Label();
            this.lbl_Birthday = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lbl_Agency = new System.Windows.Forms.Label();
            this.lbl_ExpireStart = new System.Windows.Forms.Label();
            this.pic_showIdCard = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lbl_ExpireEnd = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbl_Address = new System.Windows.Forms.Label();
            this.panel_home_fill_right = new System.Windows.Forms.Panel();
            this.panel_imageShow = new System.Windows.Forms.Panel();
            this.pic_ImageShow = new System.Windows.Forms.PictureBox();
            this.myBtnExt1 = new IDCardClieck.Controls.MyBtnExt();
            this.panel_showInfo = new System.Windows.Forms.Panel();
            this.pictureBox_error = new System.Windows.Forms.PictureBox();
            this.label_MessageShow = new System.Windows.Forms.Label();
            this.panel_home = new System.Windows.Forms.Panel();
            this.panel_homt_top = new System.Windows.Forms.Panel();
            this.realTime1 = new IDCardClieck.Controls.RealTime();
            this.panel_home_fill.SuspendLayout();
            this.panel_home_fill_fill.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_showIdCard)).BeginInit();
            this.panel_home_fill_right.SuspendLayout();
            this.panel_imageShow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_ImageShow)).BeginInit();
            this.panel_showInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_error)).BeginInit();
            this.panel_home.SuspendLayout();
            this.panel_homt_top.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_home_fill
            // 
            this.panel_home_fill.BackColor = System.Drawing.Color.White;
            this.panel_home_fill.Controls.Add(this.panel_home_fill_fill);
            this.panel_home_fill.Controls.Add(this.panel_home_fill_right);
            this.panel_home_fill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_home_fill.Location = new System.Drawing.Point(0, 100);
            this.panel_home_fill.Name = "panel_home_fill";
            this.panel_home_fill.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.panel_home_fill.Size = new System.Drawing.Size(1008, 630);
            this.panel_home_fill.TabIndex = 1;
            // 
            // panel_home_fill_fill
            // 
            this.panel_home_fill_fill.BackColor = System.Drawing.Color.Transparent;
            this.panel_home_fill_fill.BackgroundImage = global::IDCardClieck.Properties.Resources.banner;
            this.panel_home_fill_fill.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel_home_fill_fill.Controls.Add(this.panel1);
            this.panel_home_fill_fill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_home_fill_fill.Location = new System.Drawing.Point(0, 5);
            this.panel_home_fill_fill.Name = "panel_home_fill_fill";
            this.panel_home_fill_fill.Size = new System.Drawing.Size(667, 625);
            this.panel_home_fill_fill.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.btn_Start);
            this.panel1.Controls.Add(this.lbl_msg);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(82, 57);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(433, 279);
            this.panel1.TabIndex = 16;
            // 
            // btn_Start
            // 
            this.btn_Start.Location = new System.Drawing.Point(205, 252);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(75, 23);
            this.btn_Start.TabIndex = 17;
            this.btn_Start.Text = "启动";
            this.btn_Start.UseVisualStyleBackColor = true;
            this.btn_Start.Click += new System.EventHandler(this.btn_Start_Click_1);
            // 
            // lbl_msg
            // 
            this.lbl_msg.AutoSize = true;
            this.lbl_msg.BackColor = System.Drawing.Color.Transparent;
            this.lbl_msg.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_msg.Location = new System.Drawing.Point(7, 257);
            this.lbl_msg.Name = "lbl_msg";
            this.lbl_msg.Size = new System.Drawing.Size(40, 12);
            this.lbl_msg.TabIndex = 16;
            this.lbl_msg.Text = "     ";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(9, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(414, 246);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "居民身份证";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 79F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 73F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 122F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.lbl_Code, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbl_Names, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbl_Gender, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lbl_Folk, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lbl_Birthday, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label11, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.lbl_Agency, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.lbl_ExpireStart, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.pic_showIdCard, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.label5, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.lbl_ExpireEnd, 3, 6);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.lbl_Address, 1, 7);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(7, 23);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(398, 219);
            this.tableLayoutPanel1.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(9, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "身份号码";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(39, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "姓名";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(39, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 14);
            this.label3.TabIndex = 2;
            this.label3.Text = "性别";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(39, 78);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(37, 14);
            this.label10.TabIndex = 9;
            this.label10.Text = "民族";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(9, 104);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 14);
            this.label7.TabIndex = 6;
            this.label7.Text = "出生日期";
            // 
            // lbl_Code
            // 
            this.lbl_Code.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lbl_Code, 2);
            this.lbl_Code.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_Code.Location = new System.Drawing.Point(82, 0);
            this.lbl_Code.Name = "lbl_Code";
            this.lbl_Code.Size = new System.Drawing.Size(55, 14);
            this.lbl_Code.TabIndex = 11;
            this.lbl_Code.Text = "label6";
            // 
            // lbl_Names
            // 
            this.lbl_Names.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lbl_Names, 2);
            this.lbl_Names.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_Names.Location = new System.Drawing.Point(82, 26);
            this.lbl_Names.Name = "lbl_Names";
            this.lbl_Names.Size = new System.Drawing.Size(55, 14);
            this.lbl_Names.TabIndex = 11;
            this.lbl_Names.Text = "label6";
            // 
            // lbl_Gender
            // 
            this.lbl_Gender.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lbl_Gender, 2);
            this.lbl_Gender.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_Gender.Location = new System.Drawing.Point(82, 52);
            this.lbl_Gender.Name = "lbl_Gender";
            this.lbl_Gender.Size = new System.Drawing.Size(55, 14);
            this.lbl_Gender.TabIndex = 11;
            this.lbl_Gender.Text = "label6";
            // 
            // lbl_Folk
            // 
            this.lbl_Folk.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lbl_Folk, 2);
            this.lbl_Folk.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_Folk.Location = new System.Drawing.Point(82, 78);
            this.lbl_Folk.Name = "lbl_Folk";
            this.lbl_Folk.Size = new System.Drawing.Size(55, 14);
            this.lbl_Folk.TabIndex = 11;
            this.lbl_Folk.Text = "label6";
            // 
            // lbl_Birthday
            // 
            this.lbl_Birthday.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lbl_Birthday, 2);
            this.lbl_Birthday.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_Birthday.Location = new System.Drawing.Point(82, 104);
            this.lbl_Birthday.Name = "lbl_Birthday";
            this.lbl_Birthday.Size = new System.Drawing.Size(55, 14);
            this.lbl_Birthday.TabIndex = 11;
            this.lbl_Birthday.Text = "label6";
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(9, 130);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(67, 14);
            this.label11.TabIndex = 10;
            this.label11.Text = "签发机关";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(9, 156);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 14);
            this.label8.TabIndex = 7;
            this.label8.Text = "签发日期";
            // 
            // lbl_Agency
            // 
            this.lbl_Agency.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lbl_Agency, 2);
            this.lbl_Agency.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_Agency.Location = new System.Drawing.Point(82, 130);
            this.lbl_Agency.Name = "lbl_Agency";
            this.lbl_Agency.Size = new System.Drawing.Size(55, 14);
            this.lbl_Agency.TabIndex = 11;
            this.lbl_Agency.Text = "label6";
            // 
            // lbl_ExpireStart
            // 
            this.lbl_ExpireStart.AutoSize = true;
            this.lbl_ExpireStart.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_ExpireStart.Location = new System.Drawing.Point(82, 156);
            this.lbl_ExpireStart.Name = "lbl_ExpireStart";
            this.lbl_ExpireStart.Size = new System.Drawing.Size(55, 14);
            this.lbl_ExpireStart.TabIndex = 11;
            this.lbl_ExpireStart.Text = "label6";
            // 
            // pic_showIdCard
            // 
            this.pic_showIdCard.Location = new System.Drawing.Point(280, 3);
            this.pic_showIdCard.Name = "pic_showIdCard";
            this.tableLayoutPanel1.SetRowSpan(this.pic_showIdCard, 6);
            this.pic_showIdCard.Size = new System.Drawing.Size(106, 136);
            this.pic_showIdCard.TabIndex = 12;
            this.pic_showIdCard.TabStop = false;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(207, 156);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 14);
            this.label5.TabIndex = 4;
            this.label5.Text = "有效日期";
            // 
            // lbl_ExpireEnd
            // 
            this.lbl_ExpireEnd.AutoSize = true;
            this.lbl_ExpireEnd.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_ExpireEnd.Location = new System.Drawing.Point(280, 156);
            this.lbl_ExpireEnd.Name = "lbl_ExpireEnd";
            this.lbl_ExpireEnd.Size = new System.Drawing.Size(55, 14);
            this.lbl_ExpireEnd.TabIndex = 11;
            this.lbl_ExpireEnd.Text = "label6";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(39, 182);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 14);
            this.label4.TabIndex = 3;
            this.label4.Text = "住址";
            // 
            // lbl_Address
            // 
            this.lbl_Address.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lbl_Address, 3);
            this.lbl_Address.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_Address.Location = new System.Drawing.Point(82, 182);
            this.lbl_Address.Name = "lbl_Address";
            this.lbl_Address.Size = new System.Drawing.Size(52, 14);
            this.lbl_Address.TabIndex = 11;
            this.lbl_Address.Text = "柳崾";
            // 
            // panel_home_fill_right
            // 
            this.panel_home_fill_right.BackColor = System.Drawing.Color.Transparent;
            this.panel_home_fill_right.BackgroundImage = global::IDCardClieck.Properties.Resources.内页背景;
            this.panel_home_fill_right.Controls.Add(this.panel_imageShow);
            this.panel_home_fill_right.Controls.Add(this.myBtnExt1);
            this.panel_home_fill_right.Controls.Add(this.panel_showInfo);
            this.panel_home_fill_right.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel_home_fill_right.Location = new System.Drawing.Point(667, 5);
            this.panel_home_fill_right.Name = "panel_home_fill_right";
            this.panel_home_fill_right.Size = new System.Drawing.Size(341, 625);
            this.panel_home_fill_right.TabIndex = 0;
            // 
            // panel_imageShow
            // 
            this.panel_imageShow.Controls.Add(this.pic_ImageShow);
            this.panel_imageShow.Location = new System.Drawing.Point(14, 283);
            this.panel_imageShow.Name = "panel_imageShow";
            this.panel_imageShow.Size = new System.Drawing.Size(315, 225);
            this.panel_imageShow.TabIndex = 3;
            // 
            // pic_ImageShow
            // 
            this.pic_ImageShow.BackColor = System.Drawing.Color.White;
            this.pic_ImageShow.BackgroundImage = global::IDCardClieck.Properties.Resources.手持身份证;
            this.pic_ImageShow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pic_ImageShow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pic_ImageShow.Location = new System.Drawing.Point(0, 0);
            this.pic_ImageShow.Name = "pic_ImageShow";
            this.pic_ImageShow.Size = new System.Drawing.Size(315, 225);
            this.pic_ImageShow.TabIndex = 1;
            this.pic_ImageShow.TabStop = false;
            // 
            // myBtnExt1
            // 
            this.myBtnExt1.BackColor = System.Drawing.Color.White;
            this.myBtnExt1.BackgroundImage = global::IDCardClieck.Properties.Resources.返回首页;
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
            this.myBtnExt1.Location = new System.Drawing.Point(94, 569);
            this.myBtnExt1.Margin = new System.Windows.Forms.Padding(0);
            this.myBtnExt1.Name = "myBtnExt1";
            this.myBtnExt1.RectColor = System.Drawing.Color.White;
            this.myBtnExt1.RectWidth = 0;
            this.myBtnExt1.Size = new System.Drawing.Size(176, 56);
            this.myBtnExt1.TabIndex = 2;
            this.myBtnExt1.TabStop = false;
            this.myBtnExt1.TipsText = "";
            this.myBtnExt1.BtnClick += new System.EventHandler(this.myBtnExt1_BtnClick);
            // 
            // panel_showInfo
            // 
            this.panel_showInfo.Controls.Add(this.pictureBox_error);
            this.panel_showInfo.Controls.Add(this.label_MessageShow);
            this.panel_showInfo.Location = new System.Drawing.Point(13, 213);
            this.panel_showInfo.Name = "panel_showInfo";
            this.panel_showInfo.Size = new System.Drawing.Size(314, 68);
            this.panel_showInfo.TabIndex = 0;
            // 
            // pictureBox_error
            // 
            this.pictureBox_error.BackgroundImage = global::IDCardClieck.Properties.Resources.警告;
            this.pictureBox_error.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox_error.Location = new System.Drawing.Point(6, 1);
            this.pictureBox_error.Name = "pictureBox_error";
            this.pictureBox_error.Size = new System.Drawing.Size(18, 18);
            this.pictureBox_error.TabIndex = 4;
            this.pictureBox_error.TabStop = false;
            this.pictureBox_error.Visible = false;
            // 
            // label_MessageShow
            // 
            this.label_MessageShow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_MessageShow.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_MessageShow.ForeColor = System.Drawing.Color.White;
            this.label_MessageShow.Location = new System.Drawing.Point(0, 0);
            this.label_MessageShow.Name = "label_MessageShow";
            this.label_MessageShow.Size = new System.Drawing.Size(314, 68);
            this.label_MessageShow.TabIndex = 2;
            this.label_MessageShow.Text = "请将二代居民身份证放置在识读区";
            // 
            // panel_home
            // 
            this.panel_home.BackColor = System.Drawing.Color.White;
            this.panel_home.Controls.Add(this.panel_home_fill);
            this.panel_home.Controls.Add(this.panel_homt_top);
            this.panel_home.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_home.Location = new System.Drawing.Point(0, 0);
            this.panel_home.Name = "panel_home";
            this.panel_home.Size = new System.Drawing.Size(1008, 730);
            this.panel_home.TabIndex = 1;
            // 
            // panel_homt_top
            // 
            this.panel_homt_top.BackColor = System.Drawing.Color.White;
            this.panel_homt_top.Controls.Add(this.realTime1);
            this.panel_homt_top.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_homt_top.Location = new System.Drawing.Point(0, 0);
            this.panel_homt_top.Name = "panel_homt_top";
            this.panel_homt_top.Size = new System.Drawing.Size(1008, 100);
            this.panel_homt_top.TabIndex = 0;
            // 
            // realTime1
            // 
            this.realTime1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.realTime1.Location = new System.Drawing.Point(0, 0);
            this.realTime1.Name = "realTime1";
            this.realTime1.Size = new System.Drawing.Size(1008, 100);
            this.realTime1.TabIndex = 0;
            // 
            // ReadIdCardFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.panel_home);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReadIdCardFrm";
            this.Text = "身份证读取";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReadIdCardFrm_FormClosing);
            this.Load += new System.EventHandler(this.ReadIdCardFrm_Load);
            this.panel_home_fill.ResumeLayout(false);
            this.panel_home_fill_fill.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_showIdCard)).EndInit();
            this.panel_home_fill_right.ResumeLayout(false);
            this.panel_imageShow.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pic_ImageShow)).EndInit();
            this.panel_showInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_error)).EndInit();
            this.panel_home.ResumeLayout(false);
            this.panel_homt_top.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_home_fill;
        private System.Windows.Forms.Panel panel_home;
        private System.Windows.Forms.Panel panel_homt_top;
        private Controls.RealTime realTime1;
        private System.Windows.Forms.Panel panel_home_fill_fill;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbl_Code;
        private System.Windows.Forms.Label lbl_Names;
        private System.Windows.Forms.Label lbl_Gender;
        private System.Windows.Forms.Label lbl_Folk;
        private System.Windows.Forms.Label lbl_Birthday;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lbl_Agency;
        private System.Windows.Forms.Label lbl_ExpireStart;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbl_ExpireEnd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbl_Address;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbl_msg;
        private System.Windows.Forms.Button btn_Start;
        private System.Windows.Forms.Panel panel_home_fill_right;
        private System.Windows.Forms.Panel panel_imageShow;
        private System.Windows.Forms.PictureBox pic_ImageShow;
        private Controls.MyBtnExt myBtnExt1;
        private System.Windows.Forms.Panel panel_showInfo;
        private System.Windows.Forms.Label label_MessageShow;
        private System.Windows.Forms.PictureBox pictureBox_error;
        private System.Windows.Forms.PictureBox pic_showIdCard;
    }
}