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
using HZH_Controls.Controls;
using static IDCardClieck.Forms.HistoryForm;
using static IDCardClieck.Forms.UserSelectForm;
using IDCardClieck.Model;

namespace IDCardClieck.Controls.UC
{
    public partial class UCTestGridTable : UserControl
    {
        public UCTestGridTable()
        {
            InitializeComponent();
        }

        private void UCTestGridTable_Load(object sender, EventArgs e)
        {

        }

        public void BindingData(object sender,EventArgs e)
        {
            List<DataGridViewColumnEntity> lstCulumns = new List<DataGridViewColumnEntity>();
            MyEventArgsTableData myEventArgsTableData = e as MyEventArgsTableData;
            HistoryCheckData historyCheckData = myEventArgsTableData.historyCheckData;
            int columnsWidth = this.ucDataGridView1.Width - 500;
            lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "orderNo", HeadText = "NO", Width = 100, WidthType = SizeType.Absolute });
            lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "propName", HeadText = "检测项", Width = 200, WidthType = SizeType.Absolute });
            lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "propvalue", HeadText = "检测结果", Width = columnsWidth, WidthType = SizeType.Absolute, CustomCellType = typeof(UCTestGridTable_CustomCellB), TextAlign = ContentAlignment.BottomCenter });
            lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "propTime", HeadText = "检测时间", Width = 200, WidthType = SizeType.Absolute });
            this.ucDataGridView1.Columns = lstCulumns;
            this.ucDataGridView1.IsShowCheckBox = false;

            List<object> lstSource = new List<object>();
            for (int i = 0; i < historyCheckData.data.Count; i++)
            {
                HistoryCheckDataDataModel model = new HistoryCheckDataDataModel()
                {
                    orderNo = historyCheckData.data[i].orderNo,
                    propName = historyCheckData.data[i].propName,
                    propTime = historyCheckData.data[i].propTime,
                    propValue = historyCheckData.data[i].propValue.ToString(),
                    cellWidth = columnsWidth,
                };
                lstSource.Add(model);
            }
            this.ucDataGridView1.DataSource = lstSource;
        }
    }
}
