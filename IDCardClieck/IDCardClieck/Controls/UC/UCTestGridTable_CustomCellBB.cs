using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static IDCardClieck.Controls.UCTestGridTableCustom;
using IDCardClieck.Forms;
using IDCardClieck.Model;

namespace IDCardClieck.Controls.UC
{
    public partial class UCTestGridTable_CustomCellBB : UserControl, HZH_Controls.Controls.IDataGridViewCustomCell
    {
        private HistoryCheckDataDataModel m_object = null;
        public UCTestGridTable_CustomCellBB()
        {
            InitializeComponent();
        }

        public void SetBindSource(object obj)
        {
            if (obj is HistoryCheckDataDataModel)
                m_object = (HistoryCheckDataDataModel)obj;

            if (m_object.pscope.Length > 0)
            {
                Label lab = new Label();
                lab.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                lab.AutoSize = true;
                lab.Text = m_object.pscope;

                Graphics g = lab.CreateGraphics();
                SizeF StrSize = g.MeasureString(lab.Text, lab.Font);
                int widthValue = (int)StrSize.Width;

                lab.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                lab.Location = new Point(this.panel1.Width / 2 - widthValue / 2, 10);

                if (widthValue > 150)
                {

                    lab.Location = new Point(0, 0);
                    lab.AutoSize = false;
                    lab.Dock = DockStyle.Fill;
                }
                this.panel1.Controls.Add(lab);
            }
        }

        void CustomBtn_Click(object sender, EventArgs e)
        {
            ZytzbsShowInfo zytzbsShowInfo = new ZytzbsShowInfo(((Label)sender).Text);
            zytzbsShowInfo.Show();
        }

        void lbl_MouseLeave(object sender, EventArgs e)
        {
            ((Label)sender).ForeColor = Color.FromArgb(255, System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(170)))), ((int)(((byte)(205))))));
        }

        void lbl_MouseEnter(object sender, EventArgs e)
        {
            ((Label)sender).ForeColor = Color.FromArgb(255, System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(218)))), ((int)(((byte)(234))))));
        }
    }
}
