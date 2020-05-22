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

            if (m_object.propName == "中医体质辨识")
            {
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
            else
            {
                Label lab = new Label();
                lab.Text = m_object.propValue;
                lab.Size = lab.Size = new System.Drawing.Size(50, 35);
                lab.AutoSize = true;
                lab.Location = new System.Drawing.Point(m_object.cellWidth / 2 - 25, 10);
                lab.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134))); ;
                this.Controls.Add(lab);


                PictureBox pictureBox = new PictureBox();
                pictureBox.Image = global::IDCardClieck.Properties.Resources._1bf599a9c28c72631daa35d46cbf072a;
                pictureBox.Image = global::IDCardClieck.Properties.Resources._16pic_7629519_5089b8c8;
                
                pictureBox.Name = "pictureBox1";
                pictureBox.Size = new System.Drawing.Size(15, 35);
                pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                pictureBox.TabStop = false;
                pictureBox.Location = new System.Drawing.Point(lab.Location.X + lab.Width + 10,7);
                this.Controls.Add(pictureBox);
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
