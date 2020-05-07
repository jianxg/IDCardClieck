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
    public partial class UCTestGridTable_CustomCellA : UserControl,
        HZH_Controls.Controls.IDataGridViewCustomCell
    {
        private HomeForm HomeFormTemp = null;
        private UserSelectForm UserSelectFormTemp = null;
        private HistoryForm historyForm = null;
        private EDZ eDZ = null;
        private CheckoutModel model = null;
        private ModelTets m_object = null;
        public UCTestGridTable_CustomCellA()
        {
            InitializeComponent();
        }

        public void SetBindSource(object obj)
        {
            if (obj is ModelTets)
                m_object = (ModelTets)obj;
            HomeFormTemp = (HomeForm)m_object.HomeFormTemp;
            UserSelectFormTemp = (UserSelectForm)m_object.UserSelectFormTemp;
            eDZ = (EDZ)m_object.edzTemp;
            model = (CheckoutModel)m_object.checkoutModel;

            if (m_object.propName== "中医体质辨识")
            {
                if (m_object.propvalue.Trim().Length>0)
                {
                    string[] strList = m_object.propvalue.Split(';');
                    if (strList.Length>0)
                    {
                        int width = m_object.cellWidth / strList.Length;
                        for (int i = 0; i < strList.Length; i++)
                        {
                            Label lab = new Label();
                            lab.Text = strList[i];
                            lab.Size = lab.Size = new System.Drawing.Size(width, 25);
                            lab.Location = new System.Drawing.Point(width * i, 0);
                            lab.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                            this.Controls.Add(lab);
                            lab.Click += new EventHandler(CustomBtn_Click);
                        }
                    }
                }
            }
            else
            {
                Label lab = new Label();
                lab.Text = m_object.propvalue;
                lab.Size = lab.Size = new System.Drawing.Size(50, 25);
                lab.Location = new System.Drawing.Point(0, 0);
                lab.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134))); ;
                this.Controls.Add(lab);
            }
        }

        void CustomBtn_Click(object sender, EventArgs e)
        {
            ZytzbsShowInfo zytzbsShowInfo = new ZytzbsShowInfo(((Label)sender).Text);
            zytzbsShowInfo.Show();
        }

    }
}
