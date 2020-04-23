using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IDCardClieck.Common;
using System.Data.SqlClient;

using IDCardClieck.Forms;

namespace IDCardClieck.Controls
{
    public partial class UserLogin : UserControl
    {
        private static string secretKey = "www.gs-softwares.com";
        private DataTable dt = null;

        public UserLogin()
        {
            InitializeComponent();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (this.txt_account.Text.Trim().Length==0)
            {
                this.lbl_LoginResult.Text = "请输入用户名!";
            }
            else if (this.txt_password.Text.Trim().Length == 0)
            {
                this.lbl_LoginResult.Text = "请输入密码!";
            }
            else if (this.txt_registerNum.Text.Trim().Length == 0)
            {
                this.lbl_LoginResult.Text = "请激活客户端!";
            }
            else
            {
                string account = Encryption.Encode(this.txt_account.Text.Trim(), secretKey);//账号
                string password = Encryption.Encode(this.txt_password.Text.Trim(), secretKey);//密码
                StringBuilder str = new StringBuilder();
                str.Append("select * from User_Infos where Account=@Account and Password=@Password and CDKey=@CDKey");
                SqlParameter[] sqlParameters = null;
                sqlParameters = new SqlParameter[]
                    {
                    new SqlParameter("@Account",this.txt_account.Text.Trim()),
                    new SqlParameter("@Password",this.txt_password.Text.Trim()),
                    new SqlParameter("@CDKey",this.txt_registerNum.Text.Trim())
                    };
                dt = SqlHelper.Query(str.ToString(), sqlParameters).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    this.lbl_LoginResult.Text = "登录成功!";
                }
                else
                {
                    this.lbl_LoginResult.Text = "登录失败!";
                }
            } 
        }


        /// <summary>
        /// 事件处理函数：刷新注册码控件文本字符串
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void RefreshRegisterCode(object sender,EventArgs e)
        {
            MyRefeshRegisterEventArgs myRefeshRegisterEventArgs = e as MyRefeshRegisterEventArgs;
            if (myRefeshRegisterEventArgs.Res == 0 && myRefeshRegisterEventArgs.RegisterCode.Trim().Length > 0)
            {
                this.txt_registerNum.Text = myRefeshRegisterEventArgs.RegisterCode;
                this.txt_registerNum.Enabled = false;
            }
            else
            {
                if (myRefeshRegisterEventArgs.Res == 1)
                {
                    this.lbl_LoginResult.Text = "软件尚未注册!";
                    this.txt_registerNum.Enabled = false;
                }
                else if(myRefeshRegisterEventArgs.Res == 2)
                {
                    this.lbl_LoginResult.Text = "注册机器与本机不一致!";
                    this.txt_registerNum.Enabled = false;
                }
                else if (myRefeshRegisterEventArgs.Res == 3)
                {
                    this.lbl_LoginResult.Text = "软件注册已到期!";
                    this.txt_registerNum.Enabled = false;
                }
                else if (myRefeshRegisterEventArgs.Res == 3)
                {
                    this.lbl_LoginResult.Text = "激活码与注册码不匹配!";
                    this.txt_registerNum.Enabled = false;
                }
                else
                {
                    this.lbl_LoginResult.Text = "注册码已失效!";
                    this.txt_registerNum.Enabled = false;
                }
            }
        }






    }
}
