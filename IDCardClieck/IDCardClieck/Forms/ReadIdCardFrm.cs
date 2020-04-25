using GSFramework;
using IDCardClieck.Common;
using IDCardClieck.Model;
using mshtml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDCardClieck.Forms
{
    [ComVisible(true)]
    [Guid("132ED6C0-F9B1-4809-A741-ED8E3047A99A")]
    [ProgId("GSFramework.ReadCardActiveX")]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComDefaultInterface(typeof(IOCardActiveX))]
    [ComSourceInterfaces(typeof(IOCardActiveXEvents))]
    public partial class ReadIdCardFrm : Form, IObjectSafety, IOCardActiveX
    {
        private EDZ objEDZ = new EDZ();
        UserSelectForm userSelectForm = null;
        ResultJSON resultJson = null;
        CheckoutModel model = new CheckoutModel();

        private int EdziPortID = -1;
        private string SAMID = string.Empty;
        private Thread _thread = null;
        bool _exit = true;
        private string _runState = "未启动";

        private System.Windows.Forms.Timer Timer = null;

        public ReadIdCardFrm(CheckoutModel checkModel, ResultJSON resultJSONTemp)
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            this.model = checkModel;
            this.resultJson = resultJSONTemp;

            InitializeComponent();

            Timer = new System.Windows.Forms.Timer() { Interval = 100 };
            Timer.Tick += new EventHandler(Timer_Tick);
            base.Opacity = 0;
            Timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (this.Opacity >= 1)
            {
                Timer.Stop();
            }
            else
            {
                base.Opacity += 0.2;
            }
        }

        ~ReadIdCardFrm()
        {
            Stop();
            DisposeFile();
        }

        private void ReadIdCardFrm_Load(object sender, EventArgs e)
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
            InitFont();
            SimpleLoading loadingfrm = new SimpleLoading(this);
            //将Loaing窗口，注入到 SplashScreenManager 来管理
            SplashScreenManager loading = new SplashScreenManager(loadingfrm);
            loading.ShowLoading();
            try
            {
                Start1("5211336");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                loading.CloseWaitForm();
            }
        }

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x02000000;
        //        return cp;
        //    }
        //}

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
                        OnStateChanged.BeginInvoke(_exit == true ? "stop" : "run", null, null);
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
            var fs = System.Drawing.FontFamily.Families;
            foreach (FontFamily ft in fs)
            {
                if (ft.Name == "方正宋体-人口信息")
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
                while (!_exit)
                {
                    ReadCardInfo();
                    Thread.Sleep(_readTimeOut);
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
            StringBuilder Name = new StringBuilder(31);
            StringBuilder Gender = new StringBuilder(3);
            StringBuilder Folk = new StringBuilder(10);
            StringBuilder BirthDay = new StringBuilder(9);
            StringBuilder Code = new StringBuilder(19);
            StringBuilder Address = new StringBuilder(71);
            StringBuilder Agency = new StringBuilder(31);
            StringBuilder ExpireStart = new StringBuilder(9);
            StringBuilder ExpireEnd = new StringBuilder(9);
            try
            {
                //打开端口
                for (int nI = 1001; nI <= 1016; nI++)
                {
                    int intOpenRet = InitComm(nI);
                    if (intOpenRet == 1)
                    {
                        EdziPortID = nI;
                        break;
                    }
                }
                if (EdziPortID == -1)
                {
                    SetText("端口打开失败，重新连接读卡器或者查看是否打开多个读卡页面!", lbl_msg);

                    this.pictureBox_error.Invoke(
                        new MethodInvoker(
                            delegate
                            {
                                this.pictureBox_error.Visible = true;
                            }
                            )
                        );

                    SetText("  端口打开失败，重新连接读卡器或者查看是否打开多个读卡页面!", label_MessageShow);
                    return;
                }

                SetText("阅读器已开启，请放入身份证...", lbl_msg);
                this.pictureBox_error.Invoke(
                        new MethodInvoker(
                            delegate
                            {
                                this.pictureBox_error.Visible = false;
                            }
                            )
                        );
                SetText("请将二代居民身份证放置在识读区", label_MessageShow);

                _runState = "已开启";

                if (string.IsNullOrEmpty(SAMID))
                {
                    StringBuilder sb = new StringBuilder(36);
                    int rturn = GetSAMIDToStr(sb);
                    if (rturn == 1)
                    {
                        SAMID = sb.ToString();
                    }
                    else
                    {
                        //0	协议包读写错误
                        //-1	通讯失败
                        //-3	接收错误协议包
                        //-4	读取包错误(base64串口设备)
                        //-5,-6,-8	读取超时

                        SAMID = "异常返回值：" + rturn.ToString();
                    }
                }

                //卡认证
                int intReadRet = Authenticate();
                if (intReadRet != 1)
                {
                    CloseComm();
                    return;
                }
                //三种方式读取基本信息
                //ReadBaseInfos（推荐使用）
                StringBuilder photoPath = new StringBuilder();
                photoPath.Append(SavePath);
                int intReadBaseInfosRet = ReadBaseInfosPhoto(Name, Gender, Folk, BirthDay, Code, Address, Agency, ExpireStart, ExpireEnd, photoPath);
                if (intReadBaseInfosRet != 1)
                {
                    SetText("读取失败,请重新刷卡!", lbl_msg);
                    this.pictureBox_error.Invoke(
                      new MethodInvoker(
                          delegate
                          {
                              this.pictureBox_error.Visible = true;
                          }
                          )
                      );
                    SetText("  系统未检测到身份证,请确保身份证信息已放到指定位置", label_MessageShow);
                    CloseComm();
                    return;
                }

                this.pictureBox_error.Invoke(
                        new MethodInvoker(
                            delegate
                            {
                                this.pictureBox_error.Visible = false;
                            }
                            )
                        );
                SetText("身份证信息读取成功", label_MessageShow);

                EDZ objEDZ = new EDZ();
                objEDZ.Name = Name.ToString();
                objEDZ.Sex_Code = Gender.ToString();
                objEDZ.NATION_Code = Folk.ToString();

                objEDZ.BIRTH = Convert.ToDateTime(BirthDay.ToString().Substring(0, 4) + "年" + BirthDay.ToString().Substring(4, 2) + "月" + BirthDay.ToString().Substring(6) + "日");
                objEDZ.ADDRESS = Address.ToString();
                objEDZ.IDC = Code.ToString();
                objEDZ.REGORG = Agency.ToString();
                objEDZ.STARTDATE = DateTime.Parse(ExpireStart.ToString().Substring(0, 4) + "年" + ExpireStart.ToString().Substring(4, 2) + "月" + ExpireStart.ToString().Substring(6) + "日");

                objEDZ.ENDDATE = (ExpireEnd.ToString() == "长期" ? DateTime.MaxValue : DateTime.Parse(ExpireEnd.ToString().Substring(0, 4) + "年" + ExpireEnd.ToString().Substring(4, 2) + "月" + ExpireEnd.ToString().Substring(6) + "日"));

                photoPath.Remove(0, photoPath.Length);

                photoPath.Append(Path.Combine(SavePath, "photo.bmp"));
                FileInfo objFile = new FileInfo(photoPath.ToString());

                if (objFile.Exists)
                {
                    FileStream fileStream = new FileStream(photoPath.ToString(), FileMode.Open, FileAccess.Read);
                    int byteLength = (int)fileStream.Length;
                    byte[] fileBytes = new byte[byteLength];
                    fileStream.Read(fileBytes, 0, byteLength);
                    fileStream.Close();
                    objEDZ.PIC_Image = Image.FromStream(new MemoryStream(fileBytes));
                    objEDZ.PIC_Byte = fileBytes;
                    File.Delete(photoPath.ToString());
                }

                //string errstr = string.Empty;
                //string key = new EncryptClass().Encrypt(_code,
                //    objEDZ.IDC.ToString()
                //    , objEDZ.Name,
                //    objEDZ.NATION_Code,
                //    objEDZ.Sex_Code,
                //    objEDZ.REGORG.Length.ToString()
                //    , out errstr);//System.Text.Encoding.GetEncoding("GB2312").GetString(SAMId).Replace("\0", "");
                //if (errstr != string.Empty)
                //{
                //    SetText(errstr, lbl_msg);
                //    return;
                //}
                //if (OnDataBind != null)
                //{
                //    OnDataBind.BeginInvoke(objEDZ.Name.ToString(), objEDZ.Sex_CName.ToString(), objEDZ.NATION_CName.ToString(),
                //        objEDZ.BIRTH.ToString("yyyy年MM月dd日"), objEDZ.IDC.ToString(), objEDZ.ADDRESS.ToString(),
                //        objEDZ.REGORG.ToString(), objEDZ.STARTDATE.ToString("yyyy年MM月dd日"),
                //        objEDZ.ENDDATE == DateTime.MaxValue ? "长期" : objEDZ.ENDDATE.ToString("yyyy年MM月dd日"),
                //        Convert.ToBase64String(objEDZ.PIC_Byte), string.IsNullOrEmpty(key) ? "ERROR:" + errstr : key, null, null);
                //}
                //if (owin != null && !string.IsNullOrEmpty(CardDataBindfuncName))
                //{
                //    this.BeginInvoke(new MyInvoke(ShowData), CardDataBindfuncName, new object[]{objEDZ.Name.ToString(), objEDZ.Sex_CName.ToString(), objEDZ.NATION_CName.ToString(),
                //                   objEDZ.BIRTH.ToString("yyyy年MM月dd日"), objEDZ.IDC.ToString(), objEDZ.ADDRESS.ToString(),
                //                   objEDZ.REGORG.ToString(), objEDZ.STARTDATE.ToString("yyyy年MM月dd日"),
                //                   objEDZ.ENDDATE == DateTime.MaxValue ? "长期" : objEDZ.ENDDATE.ToString("yyyy年MM月dd日"),
                //                   Convert.ToBase64String(objEDZ.PIC_Byte),string.IsNullOrEmpty(key)?"ERROR:"+errstr:key});
                //}

                SetText("身份证信息读取成功！" + DateTime.Now.ToString("(yyyy年MM月dd日 HH:mm:ss)"), this.lbl_msg);
                this.pictureBox_error.Invoke(
                      new MethodInvoker(
                          delegate
                          {
                              this.pictureBox_error.Visible = false;
                          }
                          )
                      );
                SetText("身份证信息读取成功", label_MessageShow);

                if (objEDZ.PIC_Image != null)
                {
                    SetImage(objEDZ.PIC_Image, pic_showIdCard);

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

                if (objEDZ.IDC=="610323199304191615")
                {
                    objEDZ.IDC = "640202198702180034";
                }


                string apistr = "http://26526tu163.zicp.vip/app/allInOneClient/getInitCheckData";
                //向java端进行注册请求
                StringBuilder postData = new StringBuilder();
                postData.Append("{");
                postData.Append("licence_code:\"" + this.model.sericalNumber + "\",");
                postData.Append("mac_code:\"" + this.model.registerCode + "\",");
                postData.Append("IDCard_code:\"" + objEDZ.IDC + "\"");
                postData.Append("}");
                //接口调用
                string strJSON = HttpHelper.PostUrl(apistr, postData.ToString());
                //返回结果
                CheckData json = HttpHelper.Deserialize<CheckData>(strJSON);
                if (json.result == "true")
                {
                    try
                    {
                        if (userSelectForm == null)
                        {
                            this.Invoke(
                                new MethodInvoker(
                                    delegate
                                    {
                                        this.Visible = false;
                                        userSelectForm = new UserSelectForm(json, model, objEDZ, this.resultJson);
                                        userSelectForm.Owner = this;
                                        userSelectForm.Show();
                                    }
                                    )
                                );

                        }
                        else
                        {
                            if (userSelectForm.IsDisposed == true)
                            {
                                this.Invoke(
                                  new MethodInvoker(
                                      delegate
                                      {
                                          this.Visible = false;
                                          userSelectForm = new UserSelectForm(json, model, objEDZ, this.resultJson);
                                          userSelectForm.Owner = this;
                                          userSelectForm.Show();
                                      }
                                      )
                                  );

                            }
                            else
                            {
                                this.Invoke(
                                  new MethodInvoker(
                                      delegate
                                      {
                                          this.Visible = false;
                                          userSelectForm.Visible = true;
                                      }
                                      )
                                  );
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
                else
                {
                    this.pictureBox_error.Invoke(
                      new MethodInvoker(
                          delegate
                          {
                              this.pictureBox_error.Visible = true;
                          }
                          )
                      );
                    SetText("  " + json.message.ToString(), label_MessageShow);
                }
            }
            catch (Exception exc)
            {
                this.pictureBox_error.Visible = true;
                SetText(exc.Message, this.lbl_msg);
            }
            finally
            {
                if (EdziPortID > -1)
                {
                    CloseComm();
                    //CloseSDTandHIDComm(EdziPortID);

                }
                EdziPortID = -1;
            }
        }


        #region 控件属性
        private string _savePath = string.Empty;
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

        private int _readTimeOut = 3000;
        /// <summary>
        /// 轮训读卡间隔时间，单位毫秒
        /// </summary>
        public int TimeOut
        {
            get { return _readTimeOut; }
            set { _readTimeOut = value; }
        }
        #endregion

        #region 二代证读卡dll入口


        [DllImport("Sdtapi.dll")]
        private static extern int InitComm(int iPort);
        [DllImport("Sdtapi.dll")]
        private static extern int Authenticate();
        [DllImport("Sdtapi.dll")]
        private static extern int ReadBaseInfos(StringBuilder Name, StringBuilder Gender, StringBuilder Folk,
                                                    StringBuilder BirthDay, StringBuilder Code, StringBuilder Address,
                                                        StringBuilder Agency, StringBuilder ExpireStart, StringBuilder ExpireEnd);
        [DllImport("Sdtapi.dll")]
        private static extern int ReadBaseInfosPhoto(StringBuilder Name, StringBuilder Gender, StringBuilder Folk,
                                                    StringBuilder BirthDay, StringBuilder Code, StringBuilder Address,
                                                        StringBuilder Agency, StringBuilder ExpireStart, StringBuilder ExpireEnd, StringBuilder directory);
        [DllImport("Sdtapi.dll")]
        private static extern int ReadBaseInfosFPPhoto(StringBuilder Name, StringBuilder Gender, StringBuilder Folk,
                                                    StringBuilder BirthDay, StringBuilder Code, StringBuilder Address,
                                                        StringBuilder Agency, StringBuilder ExpireStart, StringBuilder ExpireEnd, StringBuilder directory, StringBuilder pucFPMsg, ref int puiFPMsgLen);
        [DllImport("Sdtapi.dll")]
        private static extern int CloseComm();
        [DllImport("Sdtapi.dll")]
        private static extern int ReadBaseMsg(byte[] pMsg, ref int len);
        [DllImport("Sdtapi.dll")]
        private static extern int ReadBaseMsgW(byte[] pMsg, ref int len);
        [DllImport("Sdtapi.dll")]
        private static extern int Routon_IC_FindCard();
        [DllImport("Sdtapi.dll")]
        private static extern int Routon_IC_HL_ReadCardSN(StringBuilder SN);
        [DllImport("Sdtapi.dll")]
        private static extern int FindAllUSB(ref int SCount, ref int HCount);

        [DllImport("Sdtapi.dll")]
        private static extern int SelectUSB(int index);
        [DllImport("Sdtapi.dll")]
        private static extern int CloseSDTandHIDComm(int index);
        [DllImport("Sdtapi.dll")]
        private static extern int CardOn();

        [DllImport("Sdtapi.dll")]
        private static extern int GetSAMIDToStr(StringBuilder SAMID);

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
                if (EdziPortID != -1)
                {
                    CloseComm();
                    //CloseSDTandHIDComm(EdziPortID);
                    EdziPortID = -1;
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
            catch (Exception)
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
                this.pic_ImageShow.Image = null;
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
            string Address, string Agency, string ExpireStart, string ExpireEnd, string ImageBase64String, string customerString);
        public delegate void StateChangeHandler(string state);

        public event ReadDataHandler OnDataBind;
        public event StateChangeHandler OnStateChanged;
        #endregion

        public void DisposeFile()
        {
            GC.SuppressFinalize(true);
        }
        private void btn_Start_Click_1(object sender, EventArgs e)
        {
            Start1("5211336");
        }

        /// <summary>
        /// 返回首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myBtnExt1_BtnClick(object sender, EventArgs e)
        {
            this.Owner.Visible = true;
            this.Close();
        }

        private void ReadIdCardFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            if (this.Owner.Visible != true)
            {
                this.Owner.Visible = true;
            }
        }
    }
}
