using IDCardClieck.Common;
using IDCardClieck.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDCardClieck.Forms
{
    [ComVisible(true)]
    public partial class ZytzbsShowInfo : Form
    {
        private string documentStr = string.Empty;
        private string nameStr = string.Empty;
        public ResultJson_Zytzbs json { get; set; }

        SimpleLoading loadingfrm = null;
        SplashScreenManager loading = null;

        public ZytzbsShowInfo()
        {
            InitializeComponent();
        }

        public ZytzbsShowInfo(string nameTemp)
        {
            nameStr = nameTemp;
            InitializeComponent();
            this.webBrowser1.Url = new System.Uri(System.Windows.Forms.Application.StartupPath + "\\kindeditor\\e.html", System.UriKind.Absolute);
            this.webBrowser1.ObjectForScripting = this;
        }

        private void ZytzbsShowInfo_Load(object sender, EventArgs e)
        {
            this.Text = nameStr;
            loadingfrm = new SimpleLoading(this);
            //将Loaing窗口，注入到 SplashScreenManager 来管理
            loading = new SplashScreenManager(loadingfrm);
            loading.ShowLoading();
            try
            {
                string url = EnConfigHelper.GetConfigValue("request", "url");
                string apistr = url + "/app/allInOneClient/getConstitutionTcm";
                //向java端进行注册请求
                StringBuilder postData = new StringBuilder();
                postData.Append("{");
                postData.Append("constitution_name:\"" + nameStr.Split('(')[0] + "\",");
                postData.Append("}");
                //接口调用
                string strJSON = HttpHelper.PostUrl(apistr, postData.ToString());
                //返回结果
                json = HttpHelper.Deserialize<ResultJson_Zytzbs>(strJSON);
                if (json.result == "true")
                {
                    loading.CloseWaitForm();
                    this.webBrowser1.Document.Write(json.data.cContent);
                }
                else
                {
                    loading.CloseWaitForm();
                    this.webBrowser1.Document.Write(json.message.ToString());
                }
            }
            catch (Exception ex)
            {
                loading.CloseWaitForm();
                this.webBrowser1.Document.Write(ex.Message.ToString());
                /*可选处理异常*/
                LogHelper.WriteLine("RegisterFrm:" + ex.Message.ToString());

            }
        }

        private void webBrowser1_Resize(object sender, EventArgs e)
        {
            this.webBrowser1.Refresh();
        }
    }
}
