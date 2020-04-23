using IDCardClieck.Common;
using IDCardClieck.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDCardClieck.Forms
{
    public partial class RegisterFrm : Form
    {
        CheckoutModel model = null;
        public ResultJSON json { get; set; }

        public RegisterFrm(CheckoutModel checkoutModel)
        {
            this.model = checkoutModel;
            InitializeComponent();
        }

        ~RegisterFrm()
        {
            Dispose();
        }

        private void RegisterFrm_Load(object sender, EventArgs e)
        {
            GetRegisterCode();
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
                        DialogResult dr = MessageBox.Show("注册成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (DialogResult == DialogResult.OK)
                        {
                            this.DialogResult = DialogResult.OK;//关键:设置登陆成功状态  
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("注册失败!");
                    }
                }
                else if (this.model.res == 2)//注册机器与本机不一致
                {
                    MessageBox.Show("注册机器与本机不一致!");
                }
                else if (this.model.res == 3)//软件试用已到期
                {
                    MessageBox.Show("软件试用已到期!");
                }
                else if (this.model.res == 4)//激活码与注册码不匹配
                {
                    MessageBox.Show("激活码与注册码不匹配!");
                }
                else//软件运行已到期
                {
                    MessageBox.Show("软件运行已到期!");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString());
            }

        }
    }
}
