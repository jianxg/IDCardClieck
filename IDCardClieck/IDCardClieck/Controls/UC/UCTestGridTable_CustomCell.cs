using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IDCardClieck.Common;
using IDCardClieck.Forms;
using static IDCardClieck.Controls.UCTestGridTableCustom;
using GSFramework;
using IDCardClieck.Model;

namespace IDCardClieck.Controls
{
    public partial class UCTestGridTable_CustomCell : UserControl,
        HZH_Controls.Controls.IDataGridViewCustomCell
    {
        private HomeForm HomeFormTemp = null;
        private UserSelectForm UserSelectFormTemp = null;
        private HistoryForm historyForm = null;
        private EDZ eDZ = null;
        private CheckoutModel model = null; 
        private ModelTets m_object = null;
        public UCTestGridTable_CustomCell()
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
        }

        private void ucBtnExt1_BtnClick(object sender, EventArgs e)
        {
            if (m_object != null)
            {
                if (historyForm==null)
                {
                    historyForm = new HistoryForm(model, m_object);
                    historyForm.Show();
                }
                else
                {
                    if (historyForm.IsDisposed==true)
                    {
                        historyForm = new HistoryForm(model,m_object);
                        historyForm.Show();
                    }
                    else
                    {
                        historyForm.Visible = true;
                    }
                }
                this.UserSelectFormTemp.Visible = false;
            }
        }

    }
}
