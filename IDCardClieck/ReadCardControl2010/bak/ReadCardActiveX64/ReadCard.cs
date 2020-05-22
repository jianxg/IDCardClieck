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

namespace GSFramework
{
    [ComImport, Guid("00000118-0000-0000-C000-000000000046"),
InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOleClientSite
    {
        void SaveObject();
        void GetMoniker(uint dwAssign, uint dwWhichMoniker, object ppmk);
        void GetContainer(out IOleContainer ppContainer);
        void ShowObject();
        void OnShowWindow(bool fShow);
        void RequestNewObjectLayout();
    }

    [ComImport, Guid("0000011B-0000-0000-C000-000000000046"),
    InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOleContainer
    {
        void EnumObjects([In, MarshalAs(UnmanagedType.U4)] int grfFlags,
         [Out, MarshalAs(UnmanagedType.LPArray)] object[] ppenum);
        void ParseDisplayName([In, MarshalAs(UnmanagedType.Interface)] object pbc,
         [In, MarshalAs(UnmanagedType.BStr)] string pszDisplayName,
         [Out, MarshalAs(UnmanagedType.LPArray)] int[] pchEaten,
         [Out, MarshalAs(UnmanagedType.LPArray)] object[] ppmkOut);
        void LockContainer([In, MarshalAs(UnmanagedType.I4)] int fLock);
    }

    [ComVisible(true)]
    [Guid("5F6C4DA2-0A2E-4d68-955F-611551560C9F")]
    [ProgId("GSFramework.ReadCardActiveX64")]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComDefaultInterface(typeof(IOCardActiveX))]
    [ComSourceInterfaces(typeof(IOCardActiveXEvents))]
    public class ReadCardActiveX64 : UserControl, IObjectSafety, IOCardActiveX
    {        
        private EDZ objEDZ = new EDZ();
        private int EdziIfOpen = 1;
        private string wztxtPath = "wz.txt";
        private string zpbmpPath = "zp.bmp";
        private string zpwltPath = "zp.wlt";
        private int EdziPortID = -1;
        private Thread _thread = null;
        bool _exit = false;
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

        private void StartReadCard()
        {
            try
            {
                while (!_exit)
                {
                    Thread.Sleep(_readTimeOut);
                    ReadCardInfo();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                SetExit = true;
            }
        }

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

        /// <summary>
        /// 读取卡信息
        /// </summary>
        public void ReadCardInfo()
        {
            try
            {
                //打开端口
                bool bUsbPort = false;
                int intOpenPortRtn = 0;
                int rtnTemp = 0;
                int pucIIN = 0;
                int pucSN = 0;
                int puiCHMsgLen = 0;
                int puiPHMsgLen = 0;

                if (EdziPortID == -1)
                {
                    //检测usb口的机具连接，必须先检测usb
                    for (int iPort = 1001; iPort <= 1016; iPort++)
                    {
                        intOpenPortRtn = SDT_OpenPort(iPort);
                        if (intOpenPortRtn == 144)
                        {
                            EdziPortID = iPort;
                            bUsbPort = true;
                            break;
                        }
                    }

                    //检测串口的机具连接
                    if (!bUsbPort)
                    {
                        for (int iPort = 1; iPort <= 2; iPort++)
                        {
                            intOpenPortRtn = SDT_OpenPort(iPort);
                            if (intOpenPortRtn == 144)
                            {
                                EdziPortID = iPort;
                                bUsbPort = false;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    intOpenPortRtn = SDT_OpenPort(EdziPortID);
                }

                if (intOpenPortRtn != 144)
                {
                    EdziPortID = -1;
                    SetText("端口打开失败，重新连接读卡器或者查看是否打开多个读卡器页面!", lbl_msg);
                    return;
                }
                //else
                //{
                //    SetText("请将身份证放入读卡器中...", lbl_msg);
                //}
                //在这里，如果您想下一次不用再耗费检查端口的检查的过程，您可以把 EdziPortID 保存下来，可以保存在注册表中，也可以保存在配置文件中，我就不多写了，但是，
                //您要考虑机具连接端口被用户改变的情况哦

                //卡认证
                rtnTemp = SDT_StartFindIDCard(EdziPortID, ref pucIIN, EdziIfOpen);
                if (rtnTemp != 159)
                {
                    rtnTemp = SDT_StartFindIDCard(EdziPortID, ref pucIIN, EdziIfOpen);  //再找卡
                    if (rtnTemp != 159)
                    {
                        //未找到身份证
                        rtnTemp = SDT_ClosePort(EdziPortID);
                        return;
                    }
                }

                //读卡
                rtnTemp = SDT_SelectIDCard(EdziPortID, ref pucSN, EdziIfOpen);
                if (rtnTemp != 144)
                {
                    rtnTemp = SDT_SelectIDCard(EdziPortID, ref pucSN, EdziIfOpen);  //再选卡
                    if (rtnTemp != 144)
                    {
                        rtnTemp = SDT_ClosePort(EdziPortID);
                        SetText("读卡失败!", lbl_msg);
                        return;
                    }
                }

                FileInfo objFile = new FileInfo(wztxtPath);
                if (objFile.Exists)
                {
                    objFile.Attributes = FileAttributes.Normal;
                    objFile.Delete();
                }
                objFile = new FileInfo(zpbmpPath);
                if (objFile.Exists)
                {
                    objFile.Attributes = FileAttributes.Normal;
                    objFile.Delete();
                }
                objFile = new FileInfo(zpwltPath);
                if (objFile.Exists)
                {
                    objFile.Attributes = FileAttributes.Normal;
                    objFile.Delete();
                }

                rtnTemp = SDT_ReadBaseMsgToFile(EdziPortID, wztxtPath, ref puiCHMsgLen, zpwltPath, ref puiPHMsgLen, EdziIfOpen);
                if (rtnTemp != 144)
                {
                    rtnTemp = SDT_ClosePort(EdziPortID);
                    SetText("读卡失败!", lbl_msg);
                    return;
                }
                //下面解析照片，注意，如果在C盘根目录下没有机具厂商的授权文件Termb.Lic，照片解析将会失败
                if (bUsbPort)
                    rtnTemp = GetBmp(zpwltPath, 2);
                else
                    rtnTemp = GetBmp(zpwltPath, 1);
                switch (rtnTemp)
                {
                    case 0:
                        SetText("调用sdtapi.dll错误!", lbl_msg);
                        break;
                    case 1:   //正常
                        break;
                    case -1:
                        SetText("相片解码错误!", lbl_msg);
                        break;
                    case -2:
                        SetText("wlt文件后缀错误!", lbl_msg);
                        break;
                    case -3:
                        SetText("wlt文件打开错误!", lbl_msg);
                        break;
                    case -4:
                        SetText("wlt文件格式错误!", lbl_msg);
                        break;
                    case -5:
                        SetText("软件未授权!", lbl_msg);
                        break;
                    case -6:
                        SetText("设备连接错误!", lbl_msg);
                        break;
                }
                rtnTemp = SDT_ClosePort(EdziPortID);
                FileInfo f = new FileInfo(wztxtPath);
                FileStream fs = f.OpenRead();
                byte[] bt = new byte[fs.Length];
                fs.Read(bt, 0, (int)fs.Length);
                fs.Close();
                File.Delete(wztxtPath);
                string str = System.Text.UnicodeEncoding.Unicode.GetString(bt);

                objEDZ = new EDZ();
                objEDZ.Name = System.Text.UnicodeEncoding.Unicode.GetString(bt, 0, 30).Trim();
                objEDZ.Sex_Code = System.Text.UnicodeEncoding.Unicode.GetString(bt, 30, 2).Trim();
                objEDZ.NATION_Code = System.Text.UnicodeEncoding.Unicode.GetString(bt, 32, 4).Trim();
                string strBird = System.Text.UnicodeEncoding.Unicode.GetString(bt, 36, 16).Trim();
                objEDZ.BIRTH = Convert.ToDateTime(strBird.Substring(0, 4) + "年" + strBird.Substring(4, 2) + "月" + strBird.Substring(6) + "日");
                objEDZ.ADDRESS = System.Text.UnicodeEncoding.Unicode.GetString(bt, 52, 70).Trim();
                objEDZ.IDC = System.Text.UnicodeEncoding.Unicode.GetString(bt, 122, 36).Trim();
                objEDZ.REGORG = System.Text.UnicodeEncoding.Unicode.GetString(bt, 158, 30).Trim();
                string strTem = System.Text.UnicodeEncoding.Unicode.GetString(bt, 188, bt.GetLength(0) - 188).Trim();
                objEDZ.STARTDATE = Convert.ToDateTime(strTem.Substring(0, 4) + "年" + strTem.Substring(4, 2) + "月" + strTem.Substring(6, 2) + "日");
                strTem = strTem.Substring(8);
                if (strTem.Trim() != "长期")
                {
                    objEDZ.ENDDATE = Convert.ToDateTime(strTem.Substring(0, 4) + "年" + strTem.Substring(4, 2) + "月" + strTem.Substring(6, 2) + "日");
                }
                else
                {
                    objEDZ.ENDDATE = DateTime.MaxValue;
                }

                objFile = new FileInfo(zpbmpPath);
                if (objFile.Exists)
                {
                    FileStream fileStream = new FileStream(zpbmpPath, FileMode.Open, FileAccess.Read);
                    int byteLength = (int)fileStream.Length;
                    byte[] fileBytes = new byte[byteLength];
                    fileStream.Read(fileBytes, 0, byteLength);
                    fileStream.Close();
                    objEDZ.PIC_Image = Image.FromStream(new MemoryStream(fileBytes));
                    objEDZ.PIC_Byte = fileBytes;
                    File.Delete(zpbmpPath);
                }

                if (OnDataBind != null)
                {
                    OnDataBind.BeginInvoke(objEDZ.Name.ToString(), objEDZ.Sex_CName.ToString(), objEDZ.NATION_CName.ToString(),
                        objEDZ.BIRTH.ToString("yyyy年MM月dd日"), objEDZ.IDC.ToString(), objEDZ.ADDRESS.ToString(),
                        objEDZ.REGORG.ToString(), objEDZ.STARTDATE.ToString("yyyy年MM月dd日"),
                        objEDZ.ENDDATE == DateTime.MaxValue ? "长期" : objEDZ.ENDDATE.ToString("yyyy年MM月dd日"),
                        Convert.ToBase64String(objEDZ.PIC_Byte), null, null);
                }
                if (owin != null && !string.IsNullOrEmpty(funcName))
                {
                    this.BeginInvoke(new MyInvoke(ShowData), objEDZ.Name.ToString(), objEDZ.Sex_CName.ToString(), objEDZ.NATION_CName.ToString(),
                           objEDZ.BIRTH.ToString("yyyy年MM月dd日"), objEDZ.IDC.ToString(), objEDZ.ADDRESS.ToString(),
                           objEDZ.REGORG.ToString(), objEDZ.STARTDATE.ToString("yyyy年MM月dd日"),
                           objEDZ.ENDDATE == DateTime.MaxValue ? "长期" : objEDZ.ENDDATE.ToString("yyyy年MM月dd日"),
                           Convert.ToBase64String(objEDZ.PIC_Byte));
                }

                SetText("身份证读卡成功！" + DateTime.Now.ToString("(yyyy年MM月dd日 HH:mm:ss)"), this.lbl_msg);

                if (objEDZ.PIC_Image != null)
                {
                    SetImage(objEDZ.PIC_Image, pictureBox1);

                }

                SetText(objEDZ.IDC.ToString().Trim(), this.lbl_Code);
                SetText(objEDZ.Name.ToString().Trim(), this.lbl_Names);
                SetText(objEDZ.Sex_CName.ToString().Trim(), this.lbl_Gender);
                SetText(objEDZ.NATION_CName.ToString().Trim(), this.lbl_Folk);
                SetText(objEDZ.BIRTH.ToString("yyyy年MM月dd日").Trim(), this.lbl_Birthday);
                SetText(objEDZ.ADDRESS.ToString().Trim(), this.lbl_Address);
                SetText(objEDZ.REGORG.ToString().Trim(), this.lbl_Agency);
                SetText(objEDZ.STARTDATE.ToString("yyyy年MM月dd日").Trim(), this.lbl_ExpireStart);
                SetText(objEDZ.ENDDATE == DateTime.MaxValue ? "长期" : objEDZ.ENDDATE.ToString("yyyy年MM月dd日"), this.lbl_ExpireEnd);

               
                //ShowData(objEDZ.Name.ToString() + "," + objEDZ.Sex_CName.ToString() + "," + objEDZ.NATION_CName.ToString() + "," +
                //objEDZ.BIRTH.ToString("yyyy年MM月dd日") + "," + objEDZ.IDC.ToString() + "," + objEDZ.ADDRESS.ToString() + "," +
                //objEDZ.REGORG.ToString() + "," + objEDZ.STARTDATE.ToString("yyyy年MM月dd日") + "," +
                //(objEDZ.ENDDATE == DateTime.MaxValue ? "长期" : objEDZ.ENDDATE.ToString("yyyy年MM月dd日")) + "," +
                //Convert.ToBase64String(objEDZ.PIC_Byte)
                //);
            }
            catch(Exception exc) 
            {
                MessageBox.Show(exc.Message);
            }
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

        private int _readTimeOut = 1000;
        /// <summary>
        /// 轮训读卡间隔时间，单位毫秒
        /// </summary>
        public int TimeOut
        {
            get { return _readTimeOut; }
            set { _readTimeOut = value; }
        }
        #endregion

        #region 调用js

        object owin;
        string funcName;
        
        public void SetFunc(object win, string func)
        {
            owin = win;
            funcName = func;
        }

        public delegate void MyInvoke(string Name, string Gender, string Folk,
    string BirthDay, string Code, string Address,
    string Agency, string ExpireStart, string ExpireEnd, string ImageBase64String);

        public void ShowData(string Name, string Gender, string Folk,
            string BirthDay, string Code, string Address,
            string Agency, string ExpireStart, string ExpireEnd, string ImageBase64String)
        {

            IHTMLWindow2 htmlWin = owin as IHTMLWindow2;
            //以下是调用方法，由于仅仅是示例，所以直接放在SetFunc方法中了。实际开发中，大家根据情况放到相应地方。   
            //这里调用的方法我提供了两种：1，反射的方法；2，JS代码语法。   
            //大家可以根据自己熟悉的情况采用适合自己的方法。两种方法的效果都是一样的。   
            //方法1。   
            htmlWin.GetType().InvokeMember(funcName,
               BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Public,
               null, htmlWin, new object[] { Name, Gender, Folk,
            BirthDay, Code, Address,
            Agency, ExpireStart, ExpireEnd, ImageBase64String });

            //方法2。   
            //string jsCode = string.Format("{0}('{1}')", funcName, "参数");
            //htmlWin.execScript(jsCode, "jscript");

            //CallJavaScript("GetData(\"" + Name + "\",\"" + Gender + "\",\"" + Folk + "\",\"" + BirthDay + "\",\"" + Code + "\",\"" + Address + "\",\"" + Agency + "\",\"" + ExpireStart + "\",\"" + ExpireEnd + "\",\"" + ImageBase64String + "\")");
        }


        #endregion

        #region 二代证读卡dll入口

        //首先，声明通用接口
        [DllImport("sdtapi.dll")]
        public static extern int SDT_OpenPort(int iPortID);

        [DllImport("sdtapi.dll")]
        public static extern int SDT_ClosePort(int iPortID);
        [DllImport("sdtapi.dll")]
        public static extern int SDT_PowerManagerBegin(int iPortID, int iIfOpen);
        [DllImport("sdtapi.dll")]
        public static extern int SDT_AddSAMUser(int iPortID, string pcUserName, int iIfOpen);
        [DllImport("sdtapi.dll")]
        public static extern int SDT_SAMLogin(int iPortID, string pcUserName, string pcPasswd, int iIfOpen);
        [DllImport("sdtapi.dll")]
        public static extern int SDT_SAMLogout(int iPortID, int iIfOpen);
        [DllImport("sdtapi.dll")]
        public static extern int SDT_UserManagerOK(int iPortID, int iIfOpen);
        [DllImport("sdtapi.dll")]
        public static extern int SDT_ChangeOwnPwd(int iPortID, string pcOldPasswd, string pcNewPasswd, int iIfOpen);
        [DllImport("sdtapi.dll")]
        public static extern int SDT_ChangeOtherPwd(int iPortID, string pcUserName, string pcNewPasswd, int iIfOpen);
        [DllImport("sdtapi.dll")]
        public static extern int SDT_DeleteSAMUser(int iPortID, string pcUserName, int iIfOpen);

        [DllImport("sdtapi.dll")]
        public static extern int SDT_StartFindIDCard(int iPortID, ref int pucIIN, int iIfOpen);
        [DllImport("sdtapi.dll")]
        public static extern int SDT_SelectIDCard(int iPortID, ref int pucSN, int iIfOpen);
        [DllImport("sdtapi.dll")]
        public static extern int SDT_ReadBaseMsg(int iPortID, string pucCHMsg, ref int puiCHMsgLen, string pucPHMsg, ref int puiPHMsgLen, int iIfOpen);
        [DllImport("sdtapi.dll")]
        public static extern int SDT_ReadBaseMsgToFile(int iPortID, string fileName1, ref int puiCHMsgLen, string fileName2, ref int puiPHMsgLen, int iIfOpen);

        [DllImport("sdtapi.dll")]
        public static extern int SDT_WriteAppMsg(int iPortID, ref byte pucSendData, int uiSendLen, ref byte pucRecvData, ref int puiRecvLen, int iIfOpen);
        [DllImport("sdtapi.dll")]
        public static extern int SDT_WriteAppMsgOK(int iPortID, ref byte pucData, int uiLen, int iIfOpen);

        [DllImport("sdtapi.dll")]
        public static extern int SDT_CancelWriteAppMsg(int iPortID, int iIfOpen);
        [DllImport("sdtapi.dll")]
        public static extern int SDT_ReadNewAppMsg(int iPortID, ref byte pucAppMsg, ref int puiAppMsgLen, int iIfOpen);
        [DllImport("sdtapi.dll")]
        public static extern int SDT_ReadAllAppMsg(int iPortID, ref byte pucAppMsg, ref int puiAppMsgLen, int iIfOpen);
        [DllImport("sdtapi.dll")]
        public static extern int SDT_UsableAppMsg(int iPortID, ref byte ucByte, int iIfOpen);

        [DllImport("sdtapi.dll")]
        public static extern int SDT_GetUnlockMsg(int iPortID, ref byte strMsg, int iIfOpen);
        [DllImport("sdtapi.dll")]
        public static extern int SDT_GetSAMID(int iPortID, ref byte StrSAMID, int iIfOpen);

        [DllImport("sdtapi.dll")]
        public static extern int SDT_SetMaxRFByte(int iPortID, byte ucByte, int iIfOpen);
        [DllImport("sdtapi.dll")]
        public static extern int SDT_ResetSAM(int iPortID, int iIfOpen);

        [DllImport("WltRS.dll")]
        public static extern int GetBmp(string file_name, int intf);

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
                lb.Text = txt;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReadCardActiveX64));
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
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 121F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 123F));
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
            this.label1.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
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
            this.label2.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
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
            this.label3.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
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
            this.label10.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
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
            this.label7.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
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
            this.lbl_Code.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
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
            this.lbl_Names.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
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
            this.lbl_Gender.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
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
            this.lbl_Folk.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
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
            this.lbl_Birthday.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
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
            this.label11.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
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
            this.label8.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
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
            this.lbl_Agency.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_Agency.Location = new System.Drawing.Point(82, 130);
            this.lbl_Agency.Name = "lbl_Agency";
            this.lbl_Agency.Size = new System.Drawing.Size(55, 14);
            this.lbl_Agency.TabIndex = 11;
            this.lbl_Agency.Text = "label6";
            // 
            // lbl_ExpireStart
            // 
            this.lbl_ExpireStart.AutoSize = true;
            this.lbl_ExpireStart.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_ExpireStart.Location = new System.Drawing.Point(82, 156);
            this.lbl_ExpireStart.Name = "lbl_ExpireStart";
            this.lbl_ExpireStart.Size = new System.Drawing.Size(55, 14);
            this.lbl_ExpireStart.TabIndex = 11;
            this.lbl_ExpireStart.Text = "label6";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(279, 3);
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
            this.label5.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(206, 156);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 14);
            this.label5.TabIndex = 4;
            this.label5.Text = "有效日期";
            // 
            // lbl_ExpireEnd
            // 
            this.lbl_ExpireEnd.AutoSize = true;
            this.lbl_ExpireEnd.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_ExpireEnd.Location = new System.Drawing.Point(279, 156);
            this.lbl_ExpireEnd.Name = "lbl_ExpireEnd";
            this.lbl_ExpireEnd.Size = new System.Drawing.Size(55, 14);
            this.lbl_ExpireEnd.TabIndex = 11;
            this.lbl_ExpireEnd.Text = "label6";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
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
            this.lbl_Address.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_Address.Location = new System.Drawing.Point(82, 182);
            this.lbl_Address.Name = "lbl_Address";
            this.lbl_Address.Size = new System.Drawing.Size(55, 14);
            this.lbl_Address.TabIndex = 11;
            this.lbl_Address.Text = "label6";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
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
            this.lbl_msg.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
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
            SetExit = false;
            wztxtPath = Path.Combine(_savePath, "wz.txt");
            zpbmpPath = Path.Combine(_savePath, "zp.bmp");
            zpwltPath = Path.Combine(_savePath, "zp.wlt");
            if (_thread == null)
            {
                _thread = new Thread(new ThreadStart(StartReadCard));
                _thread.Start();
            }
            else
            {
                Stop();
                _thread = new Thread(new ThreadStart(StartReadCard));
                _thread.Start();
            }
            this.lbl_msg.Text = "阅读器已开启，请放入身份证！";
        }

        public void Stop()
        {
            try
            {
                Clear();
                SetExit = true;
                if (_thread.ThreadState != ThreadState.Stopped)
                {
                    Thread.Sleep(TimeOut);
                }
                _thread.Interrupt();
                if (EdziPortID != -1)
                {
                    SDT_ClosePort(EdziPortID);
                    EdziPortID = -1;
                }
                _thread = null;
            }
            catch
            { }
            finally
            {
                DisposeFile();
            }
            this.lbl_msg.Text = "阅读器已关闭！";
        }

        public void Clear()
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
            this.lbl_msg.Text = "";
            this.pictureBox1.Image = null;
            objEDZ = null;
        }

        public void SetMessage(string message)
        {
            SetText(message, this.lbl_msg);
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
            string Address, string Agency, string ExpireStart, string ExpireEnd, string ImageBase64String);
        public delegate void StateChangeHandler(string state);

        public event ReadDataHandler OnDataBind;
        public event StateChangeHandler OnStateChanged;
        #endregion

        public ReadCardActiveX64()
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
        }
        ~ReadCardActiveX64()
        {
            Stop();
            DisposeFile();
        }
        public void DisposeFile()
        {
            FileInfo objFile = new FileInfo("wz.txt");
            if (objFile.Exists)
            {
                objFile.Attributes = FileAttributes.Normal;
                objFile.Delete();
            }
            objFile = new FileInfo("zp.bmp");
            if (objFile.Exists)
            {
                objFile.Attributes = FileAttributes.Normal;
                objFile.Delete();
            }
            objFile = new FileInfo("zp.wlt");
            if (objFile.Exists)
            {
                objFile.Attributes = FileAttributes.Normal;
                objFile.Delete();
            }
            GC.SuppressFinalize(true);
        }

    }
}
