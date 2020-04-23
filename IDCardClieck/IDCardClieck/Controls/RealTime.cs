using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDCardClieck.Controls
{
    public partial class RealTime : UserControl
    {
        private string weekstr = string.Empty;

        public RealTime()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //把得到的星期转换成中文
            switch (DateTime.Now.DayOfWeek.ToString())
            {
                case "Monday": weekstr = "星期一"; break;
                case "Tuesday": weekstr = "星期二"; break;
                case "Wednesday": weekstr = "星期三"; break;
                case "Thursday": weekstr = "星期四"; break;
                case "Friday": weekstr = "星期五"; break;
                case "Saturday": weekstr = "星期六"; break;
                case "Sunday": weekstr = "星期日"; break;
            }
            this.lbl_time.Text = DateTime.Now.ToString("HH:mm:ss") + " | " + DateTime.Now.ToString("yyyy-MM-dd") + " " + weekstr;
        }

        private void RealTime_Load(object sender, EventArgs e)
        {
            this.timer1.Start();
        }
    }
}
