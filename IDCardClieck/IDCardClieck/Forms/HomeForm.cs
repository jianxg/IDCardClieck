using IDCardClieck.Common;
using IDCardClieck.Model;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace IDCardClieck.Forms
{
    public partial class HomeForm : Form
    {
        ReadIdCardFrm readIdCardFrm = null;
        ResultJSON resultJSON = null;
        CheckoutModel model = new CheckoutModel();

        public HomeForm(CheckoutModel checkoutModel,ResultJSON resultJSONTemp)
        {
            this.model = checkoutModel;
            this.resultJSON = resultJSONTemp;
            InitializeComponent();

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            int width = this.panel_home_fill_fill.Width / 2 - this.myBtnExt1.Width / 2;
            int height = this.panel_home_fill_fill.Height / 2 - this.myBtnExt1.Height / 2 + this.myBtnExt1.Width / 2;
            this.myBtnExt1.Location = new Point(width, height);
        }

        private void panel_home_fill_fill_SizeChanged(object sender, EventArgs e)
        {
            int width = this.panel_home_fill_fill.Width / 2 - this.myBtnExt1.Width / 2;
            int height = this.panel_home_fill_fill.Height / 2 - this.myBtnExt1.Height / 2 + this.myBtnExt1.Width / 2;
            this.myBtnExt1.Location = new Point(width, height);
        }

        /// <summary>
        /// 开始查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myBtnExt1_BtnClick_1(object sender, EventArgs e)
        {
            try
            {
                if (readIdCardFrm == null)
                {
                    this.Visible = false;
                    readIdCardFrm = new ReadIdCardFrm(model, this.resultJSON);
                    readIdCardFrm.Owner = this;
                    readIdCardFrm.Show();
                }
                else
                {
                    if (readIdCardFrm.IsDisposed == true)
                    {
                        this.Visible = false;
                        readIdCardFrm = new ReadIdCardFrm(model, this.resultJSON);
                        readIdCardFrm.Owner = this;
                        readIdCardFrm.Show();
                    }
                    else
                    {
                        this.Visible = false;
                        readIdCardFrm.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                /*可选处理异常*/
                LogHelper.WriteLine("HomeForm:" + ex.Message.ToString());
            }
        }

        private void HomeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("真的要退出程序吗？", "退出程序", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
            else
            {
                Dispose();
                Application.Exit();
            }

        }
    }
}
