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

namespace IDCardClieck.Controls
{
    public partial class UCTestGridTable_CustomCellIcon : UserControl, HZH_Controls.Controls.IDataGridViewCustomCell
    {
        public UCTestGridTable_CustomCellIcon()
        {
            InitializeComponent();
        }

        public void SetBindSource(object obj)
        {
            if (obj is TestGridModel)
            {
                this.BackgroundImage = Properties.Resources.rowicon;
            }
        }

    }
}
