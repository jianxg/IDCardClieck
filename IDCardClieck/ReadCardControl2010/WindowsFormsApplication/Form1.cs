
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

//
//                       _oo0oo_
//                      o8888888o
//                      88" . "88
//                      (| -_- |)
//                      0\  =  /0
//                    ___/`---'\___
//                  .' \\|     |// '.
//                 / \\|||  :  |||// \
//                / _||||| -:- |||||- \
//               |   | \\\  -  /// |   |
//               | \_|  ''\---/''  |_/ |
//               \  .-\__  '-'  ___/-. /
//             ___'. .'  /--.--\  `. .'___
//          ."" '<  `.___\_<|>_/___.' >' "".
//         | | :  `- \`.;`\ _ /`;.`/ - ` : | |
//         \  \ `_.   \_ __\ /__ _/   .-` /  /
//     =====`-.____`.___ \_____/___.-`___.-'=====
//                       `=---='
//
//
//     ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//
//            佛祖保佑   永无BUG   永不修改
//
//

namespace WindowsFormsApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //this.WindowState = FormWindowState.Minimized;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.ShowInTaskbar = true;

            //this.btn_Start.Enabled = true;
            //this.btn_Stop.Enabled = false;


            //GSFramework.EncryptClass c = new GSFramework.EncryptClass();
            //string errstr = string.Empty;
            //string code = c.Encrypt("5211336", "411023198801042031", "", "", "", "", out errstr);

            //MessageBox.Show(code);

            //return;
            newReadCard1.Start1("5211336");
            //this.btn_Start.Enabled = false;
            //this.btn_Stop.Enabled = true;
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            newReadCard1.Start1("5211336");

        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            newReadCard1.Stop();
        }


        private void btn_Clear_Click(object sender, EventArgs e)
        {
            newReadCard1.Clear();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (newReadCard1.IsRun)
            {
                newReadCard1.Stop();
                newReadCard1.Dispose();
            }
            this.notifyIcon1.Visible = false;
            //this.WindowState = FormWindowState.Minimized;
            //e.Cancel = true;
        }
        //SpVoice voice = null;
        private void newReadCard1_OnDataBand(string Name, string Gender, string Folk, string BirthDay, string Code, string Address, string Agency, string ExpireStart, string ExpireEnd, string ImageBase64String,string customerString)
        {
            Byte[] bitmapData = new Byte[ImageBase64String.Length];
            bitmapData = Convert.FromBase64String(ImageBase64String);

            System.IO.MemoryStream streamBitmap = new System.IO.MemoryStream(bitmapData);


            MessageBox.Show(customerString);

            //pictureBox1.Image =  Image.FromStream(streamBitmap);

            //SpVoiceClass svc = new SpVoiceClass();
            //SpeechVoiceSpeakFlags spFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
            //svc.Speak("姓名:" + Name, spFlags);

        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            this.notifyIcon1.Visible = false;
        }

        private void 退出EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (newReadCard1.IsRun)
            {
                newReadCard1.Stop();
                newReadCard1.Dispose();
            }
            this.notifyIcon1.Visible = false;
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void 显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            this.notifyIcon1.Visible = false;
        }

        private void newReadCard1_OnStateChanged(string state)
        {
//            this.Invoke((EventHandler)(delegate
//{

//    if (state == "run")
//    {
//        this.btn_Start.Enabled = false;
//        this.btn_Stop.Enabled = true;
//    }
//    else
//    {
//        this.btn_Stop.Enabled = false;
//        this.btn_Start.Enabled = true;
//    }
//}));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(newReadCard1.GetStatus());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(newReadCard1.GetSAMID());
        }

    }
}
    
