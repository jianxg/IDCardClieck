using IDCardClieck.Common;
using IDCardClieck.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDCardClieck
{
    public partial class LoginForm : Form
    {
        public delegate void MyRefreshOwnerRegisterEventHandler(object sender,MyRefeshRegisterEventArgs e);
        public event MyRefreshOwnerRegisterEventHandler MyRefreshOwnerRegisterEvent;

        public delegate void MyGetRegisterEventHandler();

        //激活码 注册码
        string sericalNumber = string.Empty, cdKey = string.Empty;
        public LoginForm()
        {
            InitializeComponent();
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            GetRegisterCode();
        }

        /// <summary>
        /// 获取注册码
        /// </summary>
        public void GetRegisterCode()
        {
            string path = "Software\\IDCardRegisterCode";
            int res = RegeditTime.InitRegedit(ref sericalNumber,ref cdKey, path, "registerCode");
            MyRefeshRegisterEventArgs myRefeshRegisterEventArgs = new MyRefeshRegisterEventArgs();
            myRefeshRegisterEventArgs.RegisterCode = sericalNumber;
            myRefeshRegisterEventArgs.Res = res;
            MyRefreshOwnerRegisterEvent += this.userLogin1.RefreshRegisterCode;
            //校验通过
            if (res == 0)
            {
                OnMyRefreshOwnerRegisterEvent(myRefeshRegisterEventArgs);
            }
            else if (res == 1)//软件尚未注册
            {
                RegisterFrm registerFrm = new RegisterFrm();
                registerFrm.Owner = this;
                registerFrm.ShowDialog();
            }
            else if (res == 2)//注册机器与本机不一致
            {
                OnMyRefreshOwnerRegisterEvent(myRefeshRegisterEventArgs);
            }
            else if (res == 3)//软件试用已到期
            {
                OnMyRefreshOwnerRegisterEvent(myRefeshRegisterEventArgs);
            }
            else if (res == 4)//激活码与注册码不匹配
            {
                OnMyRefreshOwnerRegisterEvent(myRefeshRegisterEventArgs);
            }
            else//软件运行已到期
            {
                OnMyRefreshOwnerRegisterEvent(myRefeshRegisterEventArgs);
            }
        }


        /// <summary>
        /// 刷新用户登录界面控件
        /// </summary>
        public class MyRefeshRegisterEventArgs : EventArgs
        {
            /// <summary>
            /// 注册码
            /// </summary>
            public string RegisterCode { get; set; }

            public string CDKey { get; set; }

            public int Res { get; set; }

        }

        /// <summary>
        /// 事件触发函数
        /// </summary>
        /// <param name="e"></param>
        public void OnMyRefreshOwnerRegisterEvent(MyRefeshRegisterEventArgs e)
        {
            if (MyRefreshOwnerRegisterEvent != null)
            {
                MyRefreshOwnerRegisterEvent(this, e);
            }
        }

    }
}
