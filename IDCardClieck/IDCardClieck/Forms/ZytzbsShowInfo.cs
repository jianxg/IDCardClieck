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
            
        public ZytzbsShowInfo()
        {
            InitializeComponent();
        }

        public ZytzbsShowInfo(string str,string nameTemp)
        {
            documentStr = str;
            nameStr = nameTemp;
            InitializeComponent();
            this.webBrowser1.Url = new System.Uri(System.Windows.Forms.Application.StartupPath + "\\kindeditor\\e.html", System.UriKind.Absolute);
            this.webBrowser1.ObjectForScripting = this;
        }

        private void ZytzbsShowInfo_Load(object sender, EventArgs e)
        {
            this.Text = nameStr;
            this.webBrowser1.Document.Write(this.documentStr);
        }

        private void webBrowser1_Resize(object sender, EventArgs e)
        {
            this.webBrowser1.Refresh();
        }
    }
}
