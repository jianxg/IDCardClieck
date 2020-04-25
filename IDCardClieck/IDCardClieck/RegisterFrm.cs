using IDCardClieck.Common;
using IDCardClieck.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            loadingfrm = new SimpleLoading(this);
            //将Loaing窗口，注入到 SplashScreenManager 来管理
            loading = new SplashScreenManager(loadingfrm);
            loading.ShowLoading();
            try
            {
                //读取注册表，判断是否已经激活
                GetRegisterCode();
            }
            catch (Exception ex)
            {                 /*可选处理异常*/
                LogHelper.WriteLine("程序主入口:" + ex.Message.ToString());
            }
            finally {
                loading.CloseWaitForm();
            }
        }

        /// <summary>
        /// 获取注册码
        /// </summary>
        public void GetRegisterCode()
        {
            try
            {
                if (this.model.res == 0)
                {
                    string apistr = "http://26526tu163.zicp.vip/app/allInOneClient/getClientStatus";
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
                        this.json.data.checkItemList.Insert(0, new CheckModel { tempPropID = 0, propName = "全部" });
                        this.DialogResult = DialogResult.OK;//关键:设置登陆成功状态  
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(json.message.ToString());
                    }
                }
                else if (this.model.res == 1)//软件尚未注册
                {
                    if (this.txt_cdkey.Text.ToString().Trim().Length>0)
                    {
                        this.model.sericalNumber = this.txt_cdkey.Text.ToString().Trim();
                        //date
                        string dateNow = RegeditTime.GetNowDate();
                        //生成序列号
                        this.model.registerCode = RegeditTime.CreatSerialNumber(this.model.sericalNumber, dateNow);

                        string apistr = "http://26526tu163.zicp.vip/app/allInOneClient/startRegister";
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
                            //写入到注册表
                            RegeditTime.WriteSetting(this.model.path, this.model.registerCodeName, this.model.registerCode);
                            loading.CloseWaitForm();
                            DialogResult dr = MessageBox.Show("注册成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            if (dr == DialogResult.OK)
                            {
                                this.DialogResult = DialogResult.OK;//关键:设置登陆成功状态  
                                this.Close();
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
