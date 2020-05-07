using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IDCardClieck.Forms;
using GSFramework;
using IDCardClieck.Model;
using static IDCardClieck.Controls.UCTestGridTableCustom;

namespace IDCardClieck.Controls.UC
{
    public partial class UCTestGridTable_CustomCellB : UserControl,
        HZH_Controls.Controls.IDataGridViewCustomCell
    {
        private HistoryCheckDataDataModel m_object = null;

        public UCTestGridTable_CustomCellB()
        {
            InitializeComponent();
        }

        public void SetBindSource(object obj)
        {
            if (obj is HistoryCheckDataDataModel)
                m_object = (HistoryCheckDataDataModel)obj;

            if (m_object.propValue.Trim().Length > 0)
            {
                string[] strList = m_object.propValue.Split(';');
                if (strList.Length > 0)
                {
                    int width = m_object.cellWidth / strList.Length;
                    for (int i = 0; i < strList.Length; i++)
                    {
                        Label lab = new Label();
                        lab.Text = strList[i];
                        lab.Size = lab.Size = new System.Drawing.Size(width, 25);
                        lab.Location = new System.Drawing.Point(width * i, 10);
                        lab.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                        this.Controls.Add(lab);
                        lab.Click += new EventHandler(CustomBtn_Click);
                        lab.MouseEnter += lbl_MouseEnter;
                        lab.MouseLeave += lbl_MouseLeave;
                    }
                }
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
