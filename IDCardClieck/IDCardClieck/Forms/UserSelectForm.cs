using GSFramework;
using IDCardClieck.Common;
using IDCardClieck.Controls;
using IDCardClieck.Model;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDCardClieck.Forms
{
    public partial class UserSelectForm : Form
    {
        public event EventHandler MySendDataUserInfoData;
        MyEventArgsUserInfoData myEventArgsUserInfoData = null;

        CheckoutModel model = new CheckoutModel();
        EDZ objEDZ = null;
        ResultJSON resultJSON = null;
        CheckData checkData = null;

        public UserSelectForm(CheckData checkModel, CheckoutModel checkoutModel, EDZ eDZ,ResultJSON resultJSONTemp)
        {
            this.objEDZ = eDZ;
            this.resultJSON = resultJSONTemp;
            this.model = checkoutModel;
            this.checkData = checkModel;
            InitializeComponent();
        }

        public UserSelectForm()
        {
            InitializeComponent();
        }

        private void UserSelectForm_Load(object sender, EventArgs e)
        {
            MySendDataUserInfoData += this.ucTestGridTableCustom1.BindingUserInfoData;

            BindingData();
            SetDataGridtableData();
        }

        public void BindingData()
        {
            if (this.resultJSON.data.checkItemList.Count > 0)
            {
                this.resultJSON.data.checkItemList.Insert(0, new CheckModel { tempPropID = 0, propName = "全部" });
                this.comboBox1.DataSource = this.resultJSON.data.checkItemList;
                this.comboBox1.DisplayMember = "propName";
                this.comboBox1.ValueMember = "tempPropID";
            }
            if (this.resultJSON.data.hotCheckItemList.Count > 0)
            {
                for (int i = 0; i < this.resultJSON.data.hotCheckItemList.Count; i++)
                {
                    int x = 137 + (72 * i) + (72 * i);
                    if (x<this.pal_home_fill_fill_fill_3.Width)
                    {
                        MyBtnExt myBtnExt = new MyBtnExt();
                        myBtnExt.ConerRadius = 25;
                        myBtnExt.IsRadius = true;
                        myBtnExt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
                        myBtnExt.BtnBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
                        myBtnExt.BtnFont = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                        myBtnExt.BtnForeColor = System.Drawing.Color.White;
                        myBtnExt.BtnText = this.resultJSON.data.hotCheckItemList[i].propName;
                        myBtnExt.Tag = this.resultJSON.data.hotCheckItemList[i].tempPropID;
                        myBtnExt.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                        myBtnExt.ForeColor = System.Drawing.Color.White;
                        myBtnExt.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
                        myBtnExt.Location = new System.Drawing.Point(x, 10);
                        myBtnExt.RectColor = System.Drawing.Color.Transparent;
                        myBtnExt.RectWidth = 1;
                        myBtnExt.Size = new System.Drawing.Size(72, 28);
                        myBtnExt.BtnClick += new EventHandler(MyBtnExt_Click); 
                        this.pal_home_fill_fill_fill_3.Controls.Add(myBtnExt);
                    }
                }
            }
        }

        /// <summary>
        /// 填充datagridtable数据
        /// </summary>
        public void SetDataGridtableData()
        {
                this.lbl_userInfo.Text = this.objEDZ.Name + "   " + this.objEDZ.IDC;
                if (checkData.data.Count > 0)
                {
                    myEventArgsUserInfoData = new MyEventArgsUserInfoData();
                    myEventArgsUserInfoData.data = checkData.data;
                    myEventArgsUserInfoData.HomeFormTemp = (HomeForm)this.Owner.Owner;
                    myEventArgsUserInfoData.eDZ = objEDZ;
                    myEventArgsUserInfoData.UserSelectFormTemp = (UserSelectForm)this;
                    myEventArgsUserInfoData.checkoutModel = model;
                    OnMySendDataUserInfoData(myEventArgsUserInfoData);
                }
        }

        public void MyBtnExt_Click(object sender, EventArgs e)
        {
            SelectCheckData selectCheckData = GetCheckData(((MyBtnExt)sender).Tag.ToString(), null, null);
            if (selectCheckData.result == "true")
            {
                myEventArgsUserInfoData = new MyEventArgsUserInfoData();
                myEventArgsUserInfoData.data = selectCheckData.data.checkDataList;
                myEventArgsUserInfoData.HomeFormTemp = (HomeForm)this.Owner.Owner;
                myEventArgsUserInfoData.eDZ = objEDZ;
                myEventArgsUserInfoData.UserSelectFormTemp = (UserSelectForm)this;
                myEventArgsUserInfoData.checkoutModel = model;
                OnMySendDataUserInfoData(myEventArgsUserInfoData);
            }
            else
            {
                MessageBox.Show(selectCheckData.message.ToString());
            }
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myBtnExt7_BtnClick(object sender, EventArgs e)
        {
            ReadIdCardFrm readIdCardFrm = (ReadIdCardFrm)this.Owner;
            HomeForm homeForm = (HomeForm)this.Owner.Owner;
            this.Close();
            this.Visible = false;
            readIdCardFrm.Visible = true;
            homeForm.Visible = false;
        }

        /// <summary>
        /// 返回首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myBtnExt6_BtnClick(object sender, EventArgs e)
        {
            HomeForm homeForm = (HomeForm)this.Owner.Owner;
            this.Close();
            this.Visible = false;
            homeForm.Visible = true;
        }

        private void UserSelectForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            this.Owner.Owner.Visible = true;
        }

        public void OnMySendDataUserInfoData(MyEventArgsUserInfoData e)
        {
            if (MySendDataUserInfoData != null)
            {
                MySendDataUserInfoData(this, e);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string value = ((CheckModel)this.comboBox1.SelectedItem).propName.ToString();
            if (value!="全部")
            {
                SelectCheckData selectCheckData = GetCheckData(((CheckModel)this.comboBox1.SelectedItem).tempPropID.ToString(),null,null);
                if (selectCheckData.result == "true")
                {
                    myEventArgsUserInfoData = new MyEventArgsUserInfoData();
                    myEventArgsUserInfoData.data = selectCheckData.data.checkDataList;
                    myEventArgsUserInfoData.HomeFormTemp = (HomeForm)this.Owner.Owner;
                    myEventArgsUserInfoData.eDZ = objEDZ;
                    myEventArgsUserInfoData.UserSelectFormTemp = (UserSelectForm)this;
                    myEventArgsUserInfoData.checkoutModel = model;
                    OnMySendDataUserInfoData(myEventArgsUserInfoData);
                }
                else
                {
                    MessageBox.Show(selectCheckData.message.ToString());
                }
            }
        }

        public SelectCheckData GetCheckData(string propID,string propTimeStart,string propTimeEnd)
        {
            if (this.model.res ==0)
            {
                string apistr = "http://26526tu163.zicp.vip/app/allInOneClient/getCheckData";
                //向java端进行注册请求
                StringBuilder postData = new StringBuilder();
                postData.Append("{");
                postData.Append("licence_code:\"" + this.model.sericalNumber + "\",");
                postData.Append("mac_code:\"" + this.model.registerCode + "\",");
                postData.Append("IDCard_code:\"" + this.objEDZ.IDC + "\",");
                if (propID != "0")
                {
                    postData.Append("propID:\"" + propID + "\",");
                }
                if (propTimeStart != null)
                {
                    postData.Append("propTimeStart:\"" + propTimeStart + "\",");
                }
                if (propTimeEnd != null)
                {
                    postData.Append("propTimeEnd:\"" + propTimeEnd + "\",");
                }

                postData.Append("}");
                //接口调用
                string strJSON = HttpHelper.PostUrl(apistr, postData.ToString());
                //返回结果
                SelectCheckData json = HttpHelper.Deserialize<SelectCheckData>(strJSON);
                if (json != null)
                {
                    return json;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
            
        }

        private void myBtnExt1_BtnClick(object sender, EventArgs e)
        {
            SelectCheckData selectCheckData = GetCheckData(((CheckModel)this.comboBox1.SelectedItem).tempPropID.ToString(), this.ucDatePickerExt1.CurrentTime.ToString(), this.ucDatePickerExt2.CurrentTime.ToString());
            if (selectCheckData.result == "true")
            {
                myEventArgsUserInfoData = new MyEventArgsUserInfoData();
                myEventArgsUserInfoData.data = selectCheckData.data.checkDataList;
                myEventArgsUserInfoData.HomeFormTemp = (HomeForm)this.Owner.Owner;
                myEventArgsUserInfoData.eDZ = objEDZ;
                myEventArgsUserInfoData.UserSelectFormTemp = (UserSelectForm)this;
                myEventArgsUserInfoData.checkoutModel = model;
                OnMySendDataUserInfoData(myEventArgsUserInfoData);
            }
            else
            {
                MessageBox.Show(selectCheckData.message.ToString());
            }
        }

        public class MyEventArgsUserInfoData : EventArgs
        {
            public HomeForm HomeFormTemp { get; set; }

            public UserSelectForm UserSelectFormTemp { get; set; }

            public List<CheckDataListModel> data { get; set; }

            public EDZ eDZ { get; set; }

            public CheckoutModel checkoutModel { get; set; }
        }

        private void UserSelectForm_SizeChanged(object sender, EventArgs e)
        {
            this.ucTestGridTableCustom1.Refresh();
        }
    }
}
