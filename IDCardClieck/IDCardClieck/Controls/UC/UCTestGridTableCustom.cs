using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HZH_Controls.Controls;
using IDCardClieck.Common;
using static IDCardClieck.Forms.UserSelectForm;
using IDCardClieck.Forms;
using IDCardClieck.Model;
using GSFramework;

namespace IDCardClieck.Controls
{
    public partial class UCTestGridTableCustom : UserControl
    {
        public UCTestGridTableCustom()
        {
            InitializeComponent();
        }

        private void UCTestGridTableCustom_Load(object sender, EventArgs e)
        {

        }

        public void BindingUserInfoData(object sender, EventArgs e)
        {
            List<DataGridViewColumnEntity> lstCulumns = new List<DataGridViewColumnEntity>();
            MyEventArgsUserInfoData myEventArgsUserInfoData = e as MyEventArgsUserInfoData;
            List<CheckDataListModel> dt = myEventArgsUserInfoData.data;

            int columnsWidth = this.ucDataGridView1.Width / 6;
            lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "propID", HeadText = "NO", Width = columnsWidth, WidthType = SizeType.Absolute });
            lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "propName", HeadText = "检测项", Width = columnsWidth, WidthType = SizeType.Absolute });
            lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "propvalue", HeadText = "检测结果", Width = columnsWidth, WidthType = SizeType.Absolute });
            lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "endTime", HeadText = "检测时间", Width = columnsWidth, WidthType = SizeType.Absolute });
            lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "operation", HeadText = "操作", Width = columnsWidth, WidthType = SizeType.Absolute, CustomCellType = typeof(UCTestGridTable_CustomCell), TextAlign=ContentAlignment.BottomCenter});
            //lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "Birthday", HeadText = "生日", Width = 500, WidthType = SizeType.Absolute, Format = (a) => { return ((DateTime)a).ToString("yyyy-MM-dd"); } });
            //lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "Sex", HeadText = "性别", Width = 500, WidthType = SizeType.Absolute, Format = (a) => { return ((int)a) == 0 ? "女" : "男"; } });
            this.ucDataGridView1.Columns = lstCulumns;
            this.ucDataGridView1.IsShowCheckBox = false;

            List<object> lstSource = new List<object>();

            for (int i = 0; i < dt.Count; i++)
            {
                ModelTets model = new ModelTets()
                {
                    propID = dt[i].propID,
                    propvalue = dt[i].propvalue,
                    endTime = dt[i].endTime,
                    propName = dt[i].propName,
                    HomeFormTemp = myEventArgsUserInfoData.HomeFormTemp,
                    UserSelectFormTemp = myEventArgsUserInfoData.UserSelectFormTemp,
                    edzTemp = myEventArgsUserInfoData.eDZ,
                    checkoutModel = myEventArgsUserInfoData.checkoutModel
                };
                lstSource.Add(model);
            }

            this.ucDataGridView1.DataSource = lstSource;
        }

        public class ModelTets
        {
            public string propID { get; set; }
            public string propvalue { get; set; }
            public string startTime { get; set; }
            public string endTime { get; set; }
            public string propName { get; set; }
            public HomeForm HomeFormTemp { get; set; }
            public UserSelectForm UserSelectFormTemp { get; set; }
            public EDZ edzTemp { get; set; }
            public CheckoutModel checkoutModel { get; set; }
        }
    }
}
