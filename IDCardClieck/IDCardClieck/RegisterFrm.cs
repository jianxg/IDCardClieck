using IDCardClieck.Common;
using IDCardClieck.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDCardClieck.Forms
{
    public partial class RegisterFrm : Form
    {
        CheckoutModel model = null;
        public ResultJSON json { get; set; }

        SimpleLoading loadingfrm = null;
        SplashScreenManager loading = null;

        public RegisterFrm(CheckoutModel checkoutModel)
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.model = checkoutModel;
            InitializeComponent();
        }

        ~RegisterFrm()
        {
            Dispose();
        }

        private void RegisterFrm_Load(object sender, EventArgs e)
        {
            try
            {
                GetRegisterCode();
            }
            catch (Exception ex)
            {
                /*可选处理异常*/
                LogHelper.WriteLine("RegisterFrm:" + ex.Message.ToString());
            }
        }

        private void RegisterFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ok_Click(object sender, EventArgs e)
        {
            //读取注册表，判断是否已经激活
            GetRegisterCode();
        }

        /// <summary>
        /// 获取注册码
        /// </summary>
        public void GetRegisterCode()
        {
            try
            {
                loadingfrm = new SimpleLoading(this);
                //将Loaing窗口，注入到 SplashScreenManager 来管理
                loading = new SplashScreenManager(loadingfrm);
                loading.ShowLoading();

                if (this.model.res == 0)
                {
                    try
                    {
                        string url = EnConfigHelper.GetConfigValue("request", "url");
                        string apistr = url + "/app/allInOneClient/getClientStatus";
                        StringBuilder postData = new StringBuilder();
                        postData.Append("{");
                        postData.Append("licence_code:\"" + this.model.sericalNumber + "\",");
                        postData.Append("mac_code:\"" + this.model.registerCode + "\"");
                        postData.Append("}");
                        //接口调用
                        string strJSON = HttpHelper.PostUrl(apistr, postData.ToString());
                        //返回结果
                        json = HttpHelper.Deserialize<ResultJSON>(strJSON);
                        if (json.result == "true")
                        {
                            loading.CloseWaitForm();
                            this.json.data.checkItemList.Insert(0, new CheckModel { tempPropID = 0, propName = "全部" });
                            this.DialogResult = DialogResult.OK;//关键:设置登陆成功状态  
                            this.Close();
                        }
                        else
                        {
                            loading.CloseWaitForm();
                            MessageBox.Show("激活码已存在:" + json.message.ToString());
                            /*可选处理异常*/
                            LogHelper.WriteLine("RegisterFrm:" + json.message.ToString());
                            this.Close();
                            this.Dispose();
                        }
                    }
                    catch (Exception ex)                   
                    {
                        loading.CloseWaitForm();
                        MessageBox.Show("服务器出错:" + ex.Message.ToString());
                        /*可选处理异常*/
                        LogHelper.WriteLine("RegisterFrm:" + ex.Message.ToString());
                        this.Close();
                        this.Dispose();
                    }
                }
                else if (this.model.res == 1)//软件尚未注册
                {
                    try
                    {
                        if (this.txt_cdkey.Text.ToString().Trim().Length > 0)
                        {
                            this.model.sericalNumber = this.txt_cdkey.Text.ToString().Trim();
                            //date
                            string dateNow = RegeditTime.GetNowDate();
                            //生成序列号
                            this.model.registerCode = RegeditTime.CreatSerialNumber(this.model.sericalNumber, dateNow);

                            string url = EnConfigHelper.GetConfigValue("request", "url");
                            string apistr = url + "/app/allInOneClient/startRegister";
                            //向java端进行注册请求

                            StringBuilder postData = new StringBuilder();
                            postData.Append("{");
                            postData.Append("licence_code:\"" + this.model.sericalNumber + "\",");
                            postData.Append("mac_code:\"" + this.model.registerCode + "\"");
                            postData.Append("}");
                            //接口调用

                            string strJSON = HttpHelper.PostUrl(apistr, postData.ToString());
                            //返回结果
                            json = HttpHelper.Deserialize<ResultJSON>(strJSON);
                            if (json.result == "true")
                            {
                                string url1 = EnConfigHelper.GetConfigValue("request", "url");
                                string apistr1 = url1 + "/app/allInOneClient/getClientStatus";
                                StringBuilder postData1 = new StringBuilder();
                                postData1.Append("{");
                                postData1.Append("licence_code:\"" + this.model.sericalNumber + "\",");
                                postData1.Append("mac_code:\"" + this.model.registerCode + "\"");
                                postData1.Append("}");
                                //接口调用
                                string strJSON1 = HttpHelper.PostUrl(apistr1, postData1.ToString());
                                //返回结果
                                json = HttpHelper.Deserialize<ResultJSON>(strJSON1);
                                if (json.result == "true")
                                {
                                    //写入到注册表
                                    RegeditTime.WriteSetting(this.model.path, this.model.registerCodeName, this.model.registerCode);

                                    this.model.res = 0;
                                    loading.CloseWaitForm();

                                    this.json.data.checkItemList.Insert(0, new CheckModel { tempPropID = 0, propName = "全部" });

                                    DialogResult dr = MessageBox.Show("注册成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    if (dr == DialogResult.OK)
                                    {
                                        loading.CloseWaitForm();
                                        this.DialogResult = DialogResult.OK;//关键:设置登陆成功状态  
                                        this.Close();
                                    }
                                }
                                else
                                {
                                    loading.CloseWaitForm();
                                    lbl_note.Text = "错误：" + json.message.ToString() + "";
                                }
                            }
                            else
                            {
                                loading.CloseWaitForm();
                                lbl_note.Text = "注册失败：" + json.message.ToString() + "";
                            }
                        }
                        else
                        {
                            loading.CloseWaitForm();
                            lbl_note.Text = "请输入激活码";
                        }
                    }
                    catch (Exception ex)
                    {
                        loading.CloseWaitForm();
                        MessageBox.Show("服务器出错:" + ex.Message.ToString());
                        /*可选处理异常*/
                        LogHelper.WriteLine("RegisterFrm:" + ex.Message.ToString());
                        this.Close();
                        this.Dispose();
                    }
                }
                else if (this.model.res == 2)//注册机器与本机不一致
                {
                    loading.CloseWaitForm();
                    MessageBox.Show("注册机器与本机不一致!");
                    this.Close();
                    this.Dispose();
                }
                else if (this.model.res == 3)//软件试用已到期
                {
                    loading.CloseWaitForm();
                    MessageBox.Show("软件试用已到期!");
                    this.Close();
                    this.Dispose();
                }
                else if (this.model.res == 4)//激活码与注册码不匹配
                {
                    loading.CloseWaitForm();
                    MessageBox.Show("激活码与注册码不匹配!");
                    this.Close();
                    this.Dispose();
                }
                else//软件运行已到期
                {
                    loading.CloseWaitForm();
                    MessageBox.Show("软件运行已到期!");
                    this.Close();
                    this.Dispose();
                }
            }
            catch (Exception e)
            {
                loading.CloseWaitForm();
                MessageBox.Show("服务器出错:" + e.Message.ToString());
                this.Close();
                this.Dispose();
            }
        }
    }
}
