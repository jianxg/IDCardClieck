using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Timers;
using System.Threading;
using System.IO;
using System.Collections;
using System.Reflection;
using mshtml;
using System.Drawing.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace GSFramework
{
//    [ComImport, Guid("00000118-0000-0000-C000-000000000046"),
//InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
//    public interface IOleClientSite
//    {
//        void SaveObject();
//        void GetMoniker(uint dwAssign, uint dwWhichMoniker, object ppmk);
//        void GetContainer(out IOleContainer ppContainer);
//        void ShowObject();
//        void OnShowWindow(bool fShow);
//        void RequestNewObjectLayout();
//    }

//    [ComImport, Guid("0000011B-0000-0000-C000-000000000046"),
//    InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
//    public interface IOleContainer
//    {
//        void EnumObjects([In, MarshalAs(UnmanagedType.U4)] int grfFlags,
//         [Out, MarshalAs(UnmanagedType.LPArray)] object[] ppenum);
//        void ParseDisplayName([In, MarshalAs(UnmanagedType.Interface)] object pbc,
//         [In, MarshalAs(UnmanagedType.BStr)] string pszDisplayName,
//         [Out, MarshalAs(UnmanagedType.LPArray)] int[] pchEaten,
//         [Out, MarshalAs(UnmanagedType.LPArray)] object[] ppmkOut);
//        void LockContainer([In, MarshalAs(UnmanagedType.I4)] int fLock);
//    }

    [ComVisible(true)]
    [Guid("132ED6C0-F9B1-4809-A741-ED8E3047A99A")]
    [ProgId("GSFramework.INVSReadCardActiveX")]
    //[ClassInterface(ClassInterfaceType.None)]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComDefaultInterface(typeof(IOCardActiveX))]
    [ComSourceInterfaces(typeof(IOCardActiveXEvents))]
    public class INVSReadCardActiveX : UserControl, IObjectSafety, IOCardActiveX
    {
        //private string wztxtPath = "wz.txt";
        //private string zpbmpPath = "zp.bmp";
        //private string zpwltPath = "zp.wlt";

        private EDZ objEDZ = new EDZ();
        private int EdziPortID = 0;
        private string SAMID = string.Empty;
        private Thread _thread = null;
        bool _exit = true;
        private string _runState = "未启动";
        public bool SetExit
        {
            set
            {
                bool oldstate = _exit;
                _exit = value;
                if (oldstate != _exit)
                {
                    if (OnStateChanged != null)
                    {
                        OnStateChanged.BeginInvoke(_exit == true ? "stop" : "run",null,null);
                    }
                }
            }
        }
        public bool IsRun
        {
            get { return !_exit; }
        }

        public int Port
        {
            get { return EdziPortID; }
            set { EdziPortID = value; }
        }

        private void InitFont()
        {
            if (ContainFont())
            {
                Font ft = new System.Drawing.Font("方正宋体-人口信息", float.Parse("10.5"), FontStyle.Bold);
                SetFont(ft, this.lbl_Names);
                SetFont(ft, this.lbl_Address);
            }
        }

        private bool ContainFont()
        {
            var fs=System.Drawing.FontFamily.Families;
            foreach(FontFamily ft in fs)
            {
                if(ft.Name == "方正宋体-人口信息")
                {
                    return true;
                }
            }
            return false;

        }

        private void StartReadCard()
        {
            try
            {
                //wztxtPath = Path.Combine(_savePath, "wz.txt");
                //zpbmpPath = Path.Combine(_savePath, "zp.bmp");
                //zpwltPath = Path.Combine(_savePath, "zp.wlt");
                while (!_exit)
                {
                    Thread.Sleep(_readTimeOut);
                    ReadCardInfo();
                }
            }
            catch (Exception exc)
            {

                if (exc.Message.IndexOf("线程已从等待状态中断。") >= 0)
                {
                    //MessageBox.Show(exc.Message);
                    //Stop();
                    //SetText("阅读器已关闭！", this.lbl_msg);
                }
                else if (exc.Message.IndexOf("在创建窗口句柄之前") >= 0)
                { 
                }
                else
                {
                    //if (owin != null && InfofuncName != string.Empty)
                    //{
                    //    this.BeginInvoke(new MyInvoke(ShowData), InfofuncName, new object[] { exc.Message });
                    //}
                    SetText(exc.Message, lbl_msg);
                }

                SetExit = true;
            }
        }


        public void ReadCardInfo()
        {
            lock (this)
            {
                StringBuilder Name = new StringBuilder(100);
                Name.Append("".PadLeft(100));
                StringBuilder Gender = new StringBuilder(100);
                Gender.Append("".PadLeft(100));
                StringBuilder Folk = new StringBuilder(100);
                Folk.Append("".PadLeft(100));
                StringBuilder BirthDay = new StringBuilder(100);
                BirthDay.Append("".PadLeft(100));
                StringBuilder Code = new StringBuilder(100);
                Code.Append("".PadLeft(100));
                StringBuilder Address = new StringBuilder(100);
                Address.Append("".PadLeft(100));
                StringBuilder Agency = new StringBuilder(100);
                Agency.Append("".PadLeft(100));
                StringBuilder ExpireStart = new StringBuilder(100);
                ExpireStart.Append("".PadLeft(100));
                StringBuilder ExpireEnd = new StringBuilder(100);
                ExpireEnd.Append("".PadLeft(100));
                try
                {

                    int iRet = 0;
                    //iRet = InitComm(1001);
                    //MessageBox.Show("初始化1001返回值：" + iRet.ToString());
                    
                    if (EdziPortID == 0)
                    {
                        for (int i = 1; i < 16; i++)
                        {
                            iRet = InitComm(i);
                            if (iRet == 1)
                            {
                                EdziPortID = i;
                                break;
                            }
                        }
                    }
                    else
                    {
                        iRet = InitComm(EdziPortID);
                    }

                    if (EdziPortID == 0)
                    {
                        for (int j = 1001; j < 1016; j++)
                        {
                            iRet = InitComm(j);
                            if (iRet == 1)
                            {
                                EdziPortID = j;
                                break;
                            }
                        }
                    }

                    if (EdziPortID == 0)
                    {
                        SetText("端口打开失败，重新连接读卡器或者查看是否打开多个读卡页面!", lbl_msg);
                        return;
                    }

                    //if (InitComm(1001) != 1)
                    //{
                    //    //MessageBox.Show("阅读机具未连接");

                    //}
                    //else
                    //{
                    //    EdziPortID = 1001;
                    //}
                    //if (lbl_msg.Text != "请放置身份证...")
                    //{

                    //}
                    SetText("阅读器已开启，请放入身份证...", lbl_msg);
                    _runState = "已开启";

                    if (string.IsNullOrEmpty(SAMID))
                    {
                        StringBuilder sb = new StringBuilder(100);
                        sb.Append("".PadLeft(100));
                        int rturn = GetSAMIDToStr1(sb, 100);
                        if (rturn == 1)
                        {
                            SAMID = sb.ToString().Trim();
                        }
                        else
                        {
                            //0	协议包读写错误
                            //-1	通讯失败
                            //-3	接收错误协议包
                            //-4	读取包错误(base64串口设备)
                            //-5,-6,-8	读取超时

                            SAMID = "异常返回值：" + rturn.ToString().Trim();
                        }
                    }

                    //卡认证
                    iRet = Authenticate();
                    if (iRet != 1 && iRet != 144)
                    {
                        SetText("阅读器已开启，请放入身份证...", lbl_msg);
                        //MessageBox.Show("Authenticate:"+iRet.ToString());
                        //CloseComm();
                        return;
                    }
                    else if (iRet == -7)
                    {
                        SetText("请拿起身份证后重新放入读卡区...", lbl_msg);
                        //MessageBox.Show("Authenticate:"+iRet.ToString());
                        //CloseComm();
                        return;
                    }
                    
                    iRet = Read_Content(1);
                    if (iRet !=1)
                    {
                        SetText("读取失败,请重新刷卡!", lbl_msg);

                        //MessageBox.Show("Read_Content:" + iRet.ToString());
                        //MessageBox.Show("卡认证失败,请重新刷卡!");
                        //CloseComm();
                        return;
                    }

                    MessageBox.Show(SavePath);

                    EDZ objEDZ = new EDZ();

                    iRet = GetPeopleName(Name, 100);

                    iRet = GetPeopleAddress(Address, 100);

                    iRet = GetPeopleIDCode(Code, 100);

                    iRet = GetPeopleBirthday(BirthDay, 100);

                    iRet = GetPeopleNation(Folk, 100);

                    iRet = GetPeopleSex(Gender, 100);

                    iRet = GetDepartment(Agency, 100);

                    iRet = GetStartDate(ExpireStart, 100);

                    iRet = GetEndDate(ExpireEnd, 100);

                    iRet = GetAppAddress(0, Address, 100);

                   
                    if (File.Exists(Path.Combine(SavePath, "zp.bmp")))
                    {
                        FileStream fsReadPic = new FileStream(Path.Combine(SavePath, "zp.bmp"), FileMode.Open, FileAccess.Read);
                        int byteLength = (int)fsReadPic.Length;
                        byte[] fileBytes = new byte[byteLength];
                        fsReadPic.Read(fileBytes, 0, byteLength);
                        //fsReadPic.Close();
                        Bitmap bmTemp = new Bitmap(fsReadPic);
                        //SetImage(bmTemp, pictureBox1);
                        objEDZ.PIC_Image = bmTemp;
                        objEDZ.PIC_Byte = fileBytes;
                        fsReadPic.Close();
                    }
                    else
                    {
                        MessageBox.Show(Path.Combine(SavePath, "zp.bmp"));
                    }

                    objEDZ.Name = Name.ToString().Trim();
                    objEDZ.Sex_Code = Gender.ToString().Trim();
                    objEDZ.NATION_Code = Folk.ToString().Trim();

                    objEDZ.BIRTH = Convert.ToDateTime(BirthDay.ToString().Substring(0, 4) + "年" + BirthDay.ToString().Substring(4, 2) + "月" + BirthDay.ToString().Substring(6, 2) + "日");
                    objEDZ.ADDRESS = Address.ToString().Trim();
                    objEDZ.IDC = Code.ToString().Trim();
                    objEDZ.REGORG = Agency.ToString().Trim();
                    objEDZ.STARTDATE = DateTime.Parse(ExpireStart.ToString().Substring(0, 4) + "年" + ExpireStart.ToString().Substring(4, 2) + "月" + ExpireStart.ToString().Substring(6, 2) + "日");

                    objEDZ.ENDDATE = (ExpireEnd.ToString() == "长期" ? DateTime.MaxValue : DateTime.Parse(ExpireEnd.ToString().Substring(0, 4) + "年" + ExpireEnd.ToString().Substring(4, 2) + "月" + ExpireEnd.ToString().Substring(6, 2) + "日"));
                    
                    //MemoryStream ms = new MemoryStream();
                    //bmTemp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                    //byte[] buffer = new byte[ms.Length];
                    ////Image.Save()会改变MemoryStream的Position，需要重新Seek到Begin
                    //ms.Seek(0, SeekOrigin.Begin);
                    //ms.Read(buffer, 0, buffer.Length);
                    //objEDZ.PIC_Byte = buffer;
                    //ms.Close();

                    //photoPath.Remove(0,photoPath.Length);
                    //photoPath.Append(Path.Combine(SavePath,"photo.bmp"));
                    //MessageBox.Show(photoPath.ToString());
                    //MessageBox.Show(photoPath.ToString());
                    //FileInfo objFile = new FileInfo(photoPath.ToString());

                    //if (objFile.Exists)
                    //{
                    //    //MessageBox.Show("有照片:" + objFile.FullName);
                    //    FileStream fileStream = new FileStream(photoPath.ToString(), FileMode.Open, FileAccess.Read);
                    //    int byteLength = (int)fileStream.Length;
                    //    byte[] fileBytes = new byte[byteLength];
                    //    fileStream.Read(fileBytes, 0, byteLength);
                    //    fileStream.Close();
                    //    objEDZ.PIC_Image = Image.FromStream(new MemoryStream(fileBytes));
                    //    objEDZ.PIC_Byte = fileBytes;
                    //    File.Delete(photoPath.ToString());
                    //}
                    string errstr = string.Empty;
                    //string key = "";
                    string key = new EncryptClass().Encrypt(_code,
                        objEDZ.IDC.ToString()
                        , objEDZ.Name,
                        objEDZ.NATION_Code,
                        objEDZ.Sex_Code,
                        objEDZ.REGORG.Length.ToString()
                        , out errstr);//System.Text.Encoding.GetEncoding("GB2312").GetString(SAMId).Replace("\0", "");
                    if (errstr != string.Empty)
                    {
                        SetText(errstr, lbl_msg);
                        //return;
                    }
                    if (OnDataBind != null)
                    {
                        OnDataBind.BeginInvoke(objEDZ.Name.ToString(), objEDZ.Sex_CName.ToString(), objEDZ.NATION_CName.ToString(),
                            objEDZ.BIRTH.ToString("yyyy年MM月dd日"), objEDZ.IDC.ToString(), objEDZ.ADDRESS.ToString(),
                            objEDZ.REGORG.ToString(), objEDZ.STARTDATE.ToString("yyyy年MM月dd日"),
                            objEDZ.ENDDATE == DateTime.MaxValue ? "长期" : objEDZ.ENDDATE.ToString("yyyy年MM月dd日"),
                            Convert.ToBase64String(objEDZ.PIC_Byte), string.IsNullOrEmpty(key) ? "ERROR:" + errstr : key, null, null);
                    }
                    if (owin != null && !string.IsNullOrEmpty(CardDataBindfuncName))
                    {
                        this.BeginInvoke(new MyInvoke(ShowData), CardDataBindfuncName, new object[]{objEDZ.Name.ToString(), objEDZ.Sex_CName.ToString(), objEDZ.NATION_CName.ToString(),
                                   objEDZ.BIRTH.ToString("yyyy年MM月dd日"), objEDZ.IDC.ToString(), objEDZ.ADDRESS.ToString(),
                                   objEDZ.REGORG.ToString(), objEDZ.STARTDATE.ToString("yyyy年MM月dd日"),
                                   objEDZ.ENDDATE == DateTime.MaxValue ? "长期" : objEDZ.ENDDATE.ToString("yyyy年MM月dd日"),
                                   Convert.ToBase64String(objEDZ.PIC_Byte),string.IsNullOrEmpty(key)?"ERROR:"+errstr:key});
                    }

                    SetText("身份证信息读取成功！" + DateTime.Now.ToString("(yyyy年MM月dd日 HH:mm:ss)"), this.lbl_msg);

                    if (objEDZ.PIC_Image != null)
                    {
                        SetImage(objEDZ.PIC_Image, pictureBox1);

                    }

                    MessageBox.Show("getInfo:1" );
                    SetText(objEDZ.IDC.ToString().Trim(), this.lbl_Code);
                    SetText(objEDZ.Name.ToString().Trim(), this.lbl_Names);
                    SetText(objEDZ.Sex_CName.ToString().Trim(), this.lbl_Gender);
                    SetText(objEDZ.NATION_CName.ToString().Trim(), this.lbl_Folk);
                    SetText(objEDZ.BIRTH.ToString("yyyy年MM月dd日").Trim(), this.lbl_Birthday);
                    SetText(objEDZ.ADDRESS.ToString().Trim(), this.lbl_Address);
                    SetText(objEDZ.REGORG.ToString().Trim(), this.lbl_Agency);
                    SetText(objEDZ.STARTDATE.ToString("yyyy年MM月dd日").Trim(), this.lbl_ExpireStart);
                    SetText(objEDZ.ENDDATE == DateTime.MaxValue ? "长期" : objEDZ.ENDDATE.ToString("yyyy年MM月dd日"), this.lbl_ExpireEnd);

                }
                catch (Exception exc)
                {
                    if (exc.Message != "线程已从等待状态中断。" && !exc.Message.StartsWith("尝试读取或写入受保护的内存"))
                    {
                        SetText(exc.StackTrace, this.lbl_msg);
                    }
                 
                }
                finally
                {
                    //if (EdziPortID > 0)
                   // {
                        CloseComm();
                        //CloseSDTandHIDComm(EdziPortID);

                    //}
                    EdziPortID = 0;
                }
            }
          
            //关闭端口
            //int intCloseRet = CloseComm();
        }


        #region 控件属性
        private string _savePath = string.Empty;//System.IO.Directory.GetCurrentDirectory();
        /// <summary>
        /// 文件临时存储位置
        /// </summary>
        private string SavePath
        {
            get
            {
                if (_savePath == string.Empty)
                {
                    _savePath = System.IO.Path.GetDirectoryName(Assembly.GetAssembly(this.GetType()).Location);
                }
                return _savePath;
            }
            set
            {
                if (!string.IsNullOrEmpty(value.Trim()))
                {
                    _savePath = value;
                }
            }
        }

        private int _readTimeOut = 300;
        /// <summary>
        /// 轮训读卡间隔时间，单位毫秒
        /// </summary>
        public int TimeOut
        {
            get { return _readTimeOut; }
            set { _readTimeOut = value; }
        }
        #endregion

        #region 二代证阅读器接口
        /// <summary>
        /// 二代证阅读器接口
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32", EntryPoint = "GetModuleFileNameA")]
        static extern int GetModuleFileName(int hModule, string lpFileName, int nSize);

        //初始化设备(串口:1-16,USB:1001-1016)
        [DllImport("termb.dll")]
        static extern int InitComm(int X);
        //读卡完毕后，关闭设备
        [DllImport("termb.dll")]
        static extern int CloseComm();
        //证卡验证，返回值不需要判断
        [DllImport("termb.dll")]
        static extern int Authenticate();
        //读基本信息,iActive=1;读卡成功后照片信息存放在zp.bmp文件中;读追加地址,iActive=3;
        [DllImport("termb.dll")]
        static extern int Read_Content(int Active);

        //读卡成功后调用以下方法获取相应的身份证信息：
        //		const	int ERR_SUCCESS			= 1;//成功
        //		const	int ERR_FAIL		    	= 0;//失败
        //		const	int ERR_SAVESERIALNO		= -1;//存序列号失败 未授权机具
        //		const	int ERR_CANCELSERIALNO		= -1;//序列号取消  未授权机具
        //		const	int ERR_OPENECOMM		= -2;//打开串口
        //		const	int ERR_CLOSECOMM		= -3;//关闭串口
        //		const	int ERR_SAMSTATUS		= -4;//取sam状态失败
        //		const	int ERR_SAMID		    	= -5;//取samID失败
        //		const	int ERR_FINDCARD		= -6;//找卡错误
        //		const	int ERR_SELECTCARD		= -7;//选卡错误
        //		const	int ERR_BASEINFO		= -8;//读取基本信息错误
        //		const	int ERR_APPINFO			= -9;//读取附加信息错误
        //		const	int ERR_MNGINFO			= -10;//读取MNG信息错误
        //姓名
        [DllImport("termb.dll")]
        static extern int GetPeopleName(StringBuilder lpBuffer, uint strLen);
        //地址
        [DllImport("termb.dll")]
        static extern int GetPeopleAddress(StringBuilder lpBuffer, uint strLen);
        //身份证号码
        [DllImport("termb.dll")]
        static extern int GetPeopleIDCode(StringBuilder lpBuffer, uint strLen);
        //出生日期
        [DllImport("termb.dll")]
        static extern int GetPeopleBirthday(StringBuilder lpBuffer, uint strLen);
        //民族
        [DllImport("termb.dll")]
        static extern int GetPeopleNation(StringBuilder lpBuffer, uint strLen);
        //性别
        [DllImport("termb.dll")]
        static extern int GetPeopleSex(StringBuilder lpBuffer, uint strLen);
        //发证机关
        [DllImport("termb.dll")]
        static extern int GetDepartment(StringBuilder lpBuffer, uint strLen);
        //证件开始日期
        [DllImport("termb.dll")]
        static extern int GetStartDate(StringBuilder lpBuffer, uint strLen);
        //证件结束日期
        [DllImport("termb.dll")]
        static extern int GetEndDate(StringBuilder lpBuffer, uint strLen);
        //照片
        [DllImport("termb.dll")]
        static extern int GetPhotoBMP(StringBuilder lpBuffer, uint strLen);
        //追加地址
        [DllImport("termb.dll")]
        static extern int GetAppAddress(uint index, StringBuilder lpBuffer, uint strLen);

        [DllImport("termb.dll")]
        static extern int GetSAMIDToStr1(StringBuilder lpBuffer, uint strLen);
        #endregion

        #region 异步操作

        public delegate void SetTextCallback(string text, Label lb);
        public delegate void SetImageCallback(Image img, PictureBox pb);
        private void SetText(string txt, Label lb)
        {
            if (lb.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { txt, lb });
            }
            else
            {
               
                if (lb.Text != txt)
                {
                   
                    if (lb.Name == "lbl_msg")
                    {
                        if (txt == string.Empty)
                        {
                            return;
                        }
                        if (lb.Text == "阅读器已关闭！" && (txt == lb.Text || txt == "端口打开失败，重新连接读卡器或者查看是否打开多个读卡页面!"))
                        {
                            return;
                        } 
                        if ((lb.Text == "端口打开失败，重新连接读卡器或者查看是否打开多个读卡页面!" 
                            && txt == "阅读器已开启，请放入身份证...") 
                            || txt != "阅读器已开启，请放入身份证...")
                        {
                            if (owin != null && InfofuncName != string.Empty)
                            {
                                this.BeginInvoke(new MyInvoke(ShowData), InfofuncName, new object[] { txt });
                            }
                        }
                    }
                     lb.Text = txt;
                }
                
            }
        }
        private void SetFont(Font ft, Label lb)
        {
            if (lb.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { ft, lb });
            }
            else
            {
                lb.Font = ft;
            }
        }

        private void SetImage(Image img, PictureBox pb)
        {
            if (pb.InvokeRequired)
            {
                SetImageCallback d = new SetImageCallback(SetImage);
                this.Invoke(d, new object[] { img, pb });
            }
            else
            {
                pb.Image = img;
            }
        }


        #endregion

        #region UI设计

        private TableLayoutPanel tableLayoutPanel1;
        private Label label8;
        private Label label5;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label10;
        private Label label7;
        private Label label11;
        private Label lbl_Code;
        private Label lbl_Names;
        private Label lbl_Gender;
        private Label lbl_Folk;
        private Label lbl_Birthday;
        private Label lbl_Agency;
        private Label lbl_ExpireStart;
        private Label lbl_ExpireEnd;
        private GroupBox groupBox1;
        private Label lbl_msg;
        private Label label4;
        private Label lbl_Address;
        private PictureBox pictureBox1; 

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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(INVSReadCardActiveX));
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lbl_ExpireEnd = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbl_Address = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbl_msg = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
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
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 3, 0);
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
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(280, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.tableLayoutPanel1.SetRowSpan(this.pictureBox1, 6);
            this.pictureBox1.Size = new System.Drawing.Size(106, 136);
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
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
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(7, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(414, 246);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "居民身份证";
            // 
            // lbl_msg
            // 
            this.lbl_msg.AutoSize = true;
            this.lbl_msg.BackColor = System.Drawing.Color.Transparent;
            this.lbl_msg.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_msg.Location = new System.Drawing.Point(14, 260);
            this.lbl_msg.Name = "lbl_msg";
            this.lbl_msg.Size = new System.Drawing.Size(40, 12);
            this.lbl_msg.TabIndex = 15;
            this.lbl_msg.Text = "     ";
            // 
            // ReadCardActiveX
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.lbl_msg);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximumSize = new System.Drawing.Size(430, 282);
            this.Name = "ReadCardActiveX";
            this.Size = new System.Drawing.Size(430, 282);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #endregion

        #region ICardActiveX 成员
        public void Start()
        {
            if (!_exit) return;
            _runState = "正在启动";
            SetExit = false;
           
            if (_thread == null)
            {
                _thread = new Thread(new ThreadStart(StartReadCard));
                _thread.IsBackground = true;
                _thread.Start();
            }
            else
            {
                Stop();
                _thread = new Thread(new ThreadStart(StartReadCard));
                _thread.IsBackground = true;
                _thread.Start();
            }

            _runState = "已启动";
            //SetText("阅读器已开启，请放入身份证...", lbl_msg);
            //if (owin != null && InfofuncName != string.Empty)
            //{
            //    this.BeginInvoke(new MyInvoke(ShowData), InfofuncName, new object[] { "阅读器已开启，请放入身份证..." });
            //}
        }

        string _code = string.Empty;
        public void Start1(string code)
        {
            _code = code;
            if (!_exit) return;
            _runState = "正在启动";
            SetExit = false;
           
            if (_thread == null)
            {
                _thread = new Thread(new ThreadStart(StartReadCard));
                _thread.IsBackground = true;
                _thread.Start();
            }
            else
            {
                Stop();
                _thread = new Thread(new ThreadStart(StartReadCard));
                _thread.IsBackground = true;
                _thread.Start();
            }

            _runState = "已启动";
            //SetText("阅读器已开启，请放入身份证...", lbl_msg);
            //if (owin != null && InfofuncName != string.Empty)
            //{
            //    this.BeginInvoke(new MyInvoke(ShowData), InfofuncName, new object[] { "阅读器已开启，请放入身份证..." });
            //}
        }

        public void Stop()
        {
            try
            {
                _runState = "正在关闭";
                SetExit = true;
                if (EdziPortID != 0)
                {
                    CloseComm();
                    //CloseSDTandHIDComm(EdziPortID);
                    EdziPortID = 0;
                }

                if (_thread != null)
                {

                    if (_thread.ThreadState != ThreadState.Stopped && _thread.ThreadState != ThreadState.StopRequested)
                    {
                        _thread.Interrupt();
                    }
                    _thread = null;
                }
                Clear();
            }
            catch(Exception)
            { }
            finally
            {
                DisposeFile();
            }
            _runState = "已关闭";
            SetText("阅读器已关闭！", lbl_msg);
        }
        private delegate void InvokeCallback();
        public void Clear()
        {
            if (InvokeRequired)
            {
                this.Invoke(new InvokeCallback(Clear));
            }
            else
            {
                this.lbl_Address.Text =
                this.lbl_Agency.Text =
                this.lbl_Birthday.Text =
                this.lbl_Code.Text =
                this.lbl_ExpireEnd.Text =
                this.lbl_ExpireStart.Text =
                this.lbl_Folk.Text =
                this.lbl_Gender.Text =
                this.lbl_Names.Text = "";
                //this.lbl_msg.Text = "";
                this.pictureBox1.Image = null;
                objEDZ = null;
            }
        }

        public void SetMessage(string message)
        {
            SetText(message, this.lbl_msg);
        }

        #region 调用js

        object owin;
        string CardDataBindfuncName = string.Empty;
        string InfofuncName = string.Empty;

        public void SetFunc(object win, string databindfunc, string infofunc)
        {
            owin = win;
            CardDataBindfuncName = databindfunc.Trim();
            InfofuncName = infofunc.Trim();
        }

        //    public delegate void CardDataBindInvoke(string Name, string Gender, string Folk,
        //string BirthDay, string Code, string Address,
        //string Agency, string ExpireStart, string ExpireEnd, string ImageBase64String);

        //    public void ShowData(string Name, string Gender, string Folk,
        //        string BirthDay, string Code, string Address,
        //        string Agency, string ExpireStart, string ExpireEnd, string ImageBase64String)
        //    {

        //        IHTMLWindow2 htmlWin = owin as IHTMLWindow2;
        //        //以下是调用方法，由于仅仅是示例，所以直接放在SetFunc方法中了。实际开发中，大家根据情况放到相应地方。   
        //        //这里调用的方法我提供了两种：1，反射的方法；2，JS代码语法。   
        //        //大家可以根据自己熟悉的情况采用适合自己的方法。两种方法的效果都是一样的。   
        //        //方法1。   
        //        htmlWin.GetType().InvokeMember(CardDataBindfuncName,
        //           BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Public,
        //           null, htmlWin, new object[] { Name, Gender, Folk,
        //        BirthDay, Code, Address,
        //        Agency, ExpireStart, ExpireEnd, ImageBase64String });

        //        //方法2。   
        //        //string jsCode = string.Format("{0}('{1}')", funcName, "参数");
        //        //htmlWin.execScript(jsCode, "jscript");

        //        //CallJavaScript("GetData(\"" + Name + "\",\"" + Gender + "\",\"" + Folk + "\",\"" + BirthDay + "\",\"" + Code + "\",\"" + Address + "\",\"" + Agency + "\",\"" + ExpireStart + "\",\"" + ExpireEnd + "\",\"" + ImageBase64String + "\")");
        //    }


        public delegate void MyInvoke(string funName, object[] paras);
        public void ShowData(string funName, object[] paras)
        {
            IHTMLWindow2 htmlWin = owin as IHTMLWindow2;
            //以下是调用方法，由于仅仅是示例，所以直接放在SetFunc方法中了。实际开发中，大家根据情况放到相应地方。   
            //这里调用的方法我提供了两种：1，反射的方法；2，JS代码语法。   
            //大家可以根据自己熟悉的情况采用适合自己的方法。两种方法的效果都是一样的。   
            //方法1。   
            htmlWin.GetType().InvokeMember(funName,
               BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Public,
               null, htmlWin, paras);
        }


        #endregion

        public string GetCurrentCardInfo()
        {
            if (objEDZ == null || objEDZ.IDC.ToString() == string.Empty)
            {
                return "";
            }
            else
            {
                return objEDZ.Name.ToString() + "," + objEDZ.Sex_CName.ToString() + "," + objEDZ.NATION_CName.ToString() + "," +
                objEDZ.BIRTH.ToString("yyyy年MM月dd日") + "," + objEDZ.IDC.ToString() + "," + objEDZ.ADDRESS.ToString() + "," +
                objEDZ.REGORG.ToString() + "," + objEDZ.STARTDATE.ToString("yyyy年MM月dd日") + "," +
                (objEDZ.ENDDATE == DateTime.MaxValue ? "长期" : objEDZ.ENDDATE.ToString("yyyy年MM月dd日")) + "," +
                Convert.ToBase64String(objEDZ.PIC_Byte);
            }
        }

        public string GetStatus()
        {
            return _runState;
            //
            //return _runState;
        }

        public string GetSAMID()
        {
            return SAMID;
        }

        #endregion

        #region IObjectSafe 成员

        private const string _IID_IDispatch = "{00020400-0000-0000-C000-000000000046}";
        private const string _IID_IDispatchEx = "{a6ef9860-c720-11d0-9337-00a0c90dcaa9}";
        private const string _IID_IPersistStorage = "{0000010A-0000-0000-C000-000000000046}";
        private const string _IID_IPersistStream = "{00000109-0000-0000-C000-000000000046}";
        private const string _IID_IPersistPropertyBag = "{37D84F60-42CB-11CE-8135-00AA004BB851}";

        private const int INTERFACESAFE_FOR_UNTRUSTED_CALLER = 0x00000001;
        private const int INTERFACESAFE_FOR_UNTRUSTED_DATA = 0x00000002;
        private const int S_OK = 0;
        private const int E_FAIL = unchecked((int)0x80004005);
        private const int E_NOINTERFACE = unchecked((int)0x80004002);

        private bool _fSafeForScripting = true;
        private bool _fSafeForInitializing = true;

        public int GetInterfaceSafetyOptions(ref Guid riid,
                             ref int pdwSupportedOptions,
                             ref int pdwEnabledOptions)
        {
            int Rslt = E_FAIL;

            string strGUID = riid.ToString("B");
            pdwSupportedOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER | INTERFACESAFE_FOR_UNTRUSTED_DATA;
            switch (strGUID)
            {
                case _IID_IDispatch:
                case _IID_IDispatchEx:
                    Rslt = S_OK;
                    pdwEnabledOptions = 0;
                    if (_fSafeForScripting == true)
                        pdwEnabledOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER;
                    break;
                case _IID_IPersistStorage:
                case _IID_IPersistStream:
                case _IID_IPersistPropertyBag:
                    Rslt = S_OK;
                    pdwEnabledOptions = 0;
                    if (_fSafeForInitializing == true)
                        pdwEnabledOptions = INTERFACESAFE_FOR_UNTRUSTED_DATA;
                    break;
                default:
                    Rslt = E_NOINTERFACE;
                    break;
            }

            return Rslt;
        }

        public int SetInterfaceSafetyOptions(ref Guid riid,
                             int dwOptionSetMask,
                             int dwEnabledOptions)
        {
            int Rslt = E_FAIL;

            string strGUID = riid.ToString("B");
            switch (strGUID)
            {
                case _IID_IDispatch:
                case _IID_IDispatchEx:
                    if (((dwEnabledOptions & dwOptionSetMask) == INTERFACESAFE_FOR_UNTRUSTED_CALLER) &&
                         (_fSafeForScripting == true))
                        Rslt = S_OK;
                    break;
                case _IID_IPersistStorage:
                case _IID_IPersistStream:
                case _IID_IPersistPropertyBag:
                    if (((dwEnabledOptions & dwOptionSetMask) == INTERFACESAFE_FOR_UNTRUSTED_DATA) &&
                         (_fSafeForInitializing == true))
                        Rslt = S_OK;
                    break;
                default:
                    Rslt = E_NOINTERFACE;
                    break;
            }

            return Rslt;
        }

        #endregion

        #region ICardActiveXEvents 成员
        public delegate void ReadDataHandler(string Name, string Gender, string Folk, string BirthDay, string Code,
            string Address, string Agency, string ExpireStart, string ExpireEnd, string ImageBase64String,string customerString);
        public delegate void StateChangeHandler(string state);

        public event ReadDataHandler OnDataBind;
        public event StateChangeHandler OnStateChanged;
        #endregion

        public INVSReadCardActiveX()
        {
            InitializeComponent();
            this.lbl_Address.Text =
            this.lbl_Agency.Text =
            this.lbl_Birthday.Text =
            this.lbl_Code.Text =
            this.lbl_ExpireEnd.Text =
            this.lbl_ExpireStart.Text =
            this.lbl_Folk.Text =
            this.lbl_Gender.Text =
            this.lbl_Names.Text = "";
            InitFont();
            
        }
        ~INVSReadCardActiveX()
        {
            Stop();
            DisposeFile();
        }
        public void DisposeFile()
        {
          
            GC.SuppressFinalize(true);
        }

        

    }
}
