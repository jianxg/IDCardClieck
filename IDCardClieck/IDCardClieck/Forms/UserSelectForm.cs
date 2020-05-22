using GSFramework;
using HZH_Controls.Controls;
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
        int closeState = 1;//0不执行关闭时间 执行关闭事件
        public event EventHandler MySendDataUserInfoData;
        MyEventArgsUserInfoData myEventArgsUserInfoData = null;

        CheckoutModel model = new CheckoutModel();
        EDZ objEDZ = null;
        ResultJSON resultJSON = null;
        CheckData checkData = null;
        private System.Windows.Forms.Timer Timer = null;

        ReadIdCardFrm readIdCardFrm = null;
        HomeForm homeForm = null;

        public UserSelectForm(CheckData checkModel, CheckoutModel checkoutModel, EDZ eDZ,ResultJSON resultJSONTemp)
        {

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.objEDZ = eDZ;
            this.resultJSON = resultJSONTemp;
            this.model = checkoutModel;
            this.checkData = checkModel;
            InitializeComponent();

            Timer = new System.Windows.Forms.Timer() { Interval = 100 };
            Timer.Tick += new EventHandler(Timer_Tick);
            base.Opacity = 0;
            Timer.Start();
        }

        public UserSelectForm()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            InitializeComponent();
            Timer = new System.Windows.Forms.Timer() { Interval = 100 };
            Timer.Tick += new EventHandler(Timer_Tick);
            base.Opacity = 0;
            Timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (this.Opacity >= 1)
            {
                Timer.Stop();
            }
            else
            {
                base.Opacity += 0.2;
            }
        }

        private void UserSelectForm_Load(object sender, EventArgs e)
        {
            ucDatePickerExt1.CurrentTime = DateTime.Now.AddMonths(-1);
            ucDatePickerExt2.CurrentTime = DateTime.Now;
            SimpleLoading loadingfrm = new SimpleLoading(this);
            SplashScreenManager loading = new SplashScreenManager(loadingfrm);
            loading.ShowLoading();
            try
            {
                homeForm = (HomeForm)this.Owner.Owner;
                readIdCardFrm = (ReadIdCardFrm)this.Owner;
                MySendDataUserInfoData += this.ucTestGridTableCustom1.BindingUserInfoData;
                BindingData();
                SetDataGridtableData();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLine("UserSelectForm: " + ex.Message.ToString());
            }
            finally
            {
                loading.CloseWaitForm();
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        public void BindingData()
        {
            if (this.resultJSON.data.checkItemList.Count > 0)
            {
                this.comboBox1.DataSource = this.resultJSON.data.checkItemList;
                this.comboBox1.DisplayMember = "propName";
                this.comboBox1.ValueMember = "tempPropID";
            }
            if (this.resultJSON.data.hotCheckItemList.Count > 0)
            {
                for (int i = 0; i < this.resultJSON.data.hotCheckItemList.Count; i++)
                {
                    int x = 114 + (83 * i) + (83 * i);
                    if (x<this.pal_home_fill_fill_fill_3.Width)
                    {
                        UCBtnExt myBtnExt = new UCBtnExt();
                        myBtnExt.ConerRadius = 25;
                        myBtnExt.IsRadius = true;
                        myBtnExt.BackColor = System.Drawing.Color.Transparent;
                        myBtnExt.BtnBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(212)))), ((int)(((byte)(197)))));
                        myBtnExt.BtnFont = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                        myBtnExt.BtnForeColor = System.Drawing.Color.White;
                        myBtnExt.BtnText = this.resultJSON.data.hotCheckItemList[i].propName;
                        myBtnExt.Tag = this.resultJSON.data.hotCheckItemList[i].tempPropID;
                        myBtnExt.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                        myBtnExt.ForeColor = System.Drawing.Color.White;
                        myBtnExt.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(212)))), ((int)(((byte)(197)))));
                        myBtnExt.Location = new System.Drawing.Point(x, 6);
                        myBtnExt.RectColor = System.Drawing.Color.Transparent;
                        myBtnExt.RectWidth = 1;
                        myBtnExt.Size = new System.Drawing.Size(83, 32);
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
            string idcrardStr = this.objEDZ.IDC.Substring(0, 4) + "xxxxxxxxxx" + this.objEDZ.IDC.Substring(this.objEDZ.IDC.Length - 4, 4);
                this.lbl_userInfo.Text = this.objEDZ.Name + "   " + idcrardStr;
                if (checkData.data.Count > 0)
                {
                    myEventArgsUserInfoData = new MyEventArgsUserInfoData();
                    myEventArgsUserInfoData.data = checkData.data;
                    myEventArgsUserInfoData.HomeFormTemp = homeForm;
                    myEventArgsUserInfoData.eDZ = objEDZ;
                    myEventArgsUserInfoData.UserSelectFormTemp = (UserSelectForm)this;
                    myEventArgsUserInfoData.ReadIdCardFrmTemp = readIdCardFrm;
                    myEventArgsUserInfoData.checkoutModel = model;
                    OnMySendDataUserInfoData(myEventArgsUserInfoData);
                }
        }

        public void MyBtnExt_Click(object sender, EventArgs e)
        {
            SelectCheckData selectCheckData = GetCheckData(((UCBtnExt)sender).Tag.ToString(), null, null);
            if (selectCheckData.result == "true")
            {
                myEventArgsUserInfoData = new MyEventArgsUserInfoData();
                myEventArgsUserInfoData.data = selectCheckData.data.checkDataList;
                myEventArgsUserInfoData.HomeFormTemp = (HomeForm)this.Owner.Owner;
                myEventArgsUserInfoData.eDZ = objEDZ;
                myEventArgsUserInfoData.UserSelectFormTemp = (UserSelectForm)this;
                myEventArgsUserInfoData.ReadIdCardFrmTemp = readIdCardFrm;
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
            closeState = 0;
            this.Close();
            if (this.readIdCardFrm.Visible != true)
            {
                readIdCardFrm.Visible = true;
            }
        }

        /// <summary>
        /// 返回首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myBtnExt6_BtnClick(object sender, EventArgs e)
        {
            //this.Visible = false;
            //if (this.homeForm.Visible != true)
            //{
            //    this.homeForm.Visible = true;
            //}
            closeState = 0;
            this.Close();
            if (this.readIdCardFrm.Visible != true)
            {
                readIdCardFrm.Visible = true;
            }

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
            if (value != "全部")
            {
                SelectCheckData selectCheckData = GetCheckData(((CheckModel)this.comboBox1.SelectedItem).tempPropID.ToString(), null, null);
                if (selectCheckData != null)
                {
                    if (selectCheckData.result == "true")
                    {
                        myEventArgsUserInfoData = new MyEventArgsUserInfoData();
                        myEventArgsUserInfoData.data = selectCheckData.data.checkDataList;
                        myEventArgsUserInfoData.HomeFormTemp = (HomeForm)this.Owner.Owner;
                        myEventArgsUserInfoData.eDZ = objEDZ;
                        myEventArgsUserInfoData.UserSelectFormTemp = (UserSelectForm)this;
                        myEventArgsUserInfoData.checkoutModel = model;
                        myEventArgsUserInfoData.ReadIdCardFrmTemp = readIdCardFrm;
                        OnMySendDataUserInfoData(myEventArgsUserInfoData);
                    }
                    else
                    {
                        MessageBox.Show(selectCheckData.message.ToString());
                    }
                }
            }
        }

        public SelectCheckData GetCheckData(string propID,string propTimeStart,string propTimeEnd)
        {
            if (this.model.res ==0)
            {
                string url = EnConfigHelper.GetConfigValue("request", "url");
                string apistr = url + "/app/allInOneClient/getCheckData";
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
                myEventArgsUserInfoData.ReadIdCardFrmTemp = readIdCardFrm;
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

            public ReadIdCardFrm ReadIdCardFrmTemp { get; set; }

            public List<CheckDataListModel> data { get; set; }

            public EDZ eDZ { get; set; }

            public CheckoutModel checkoutModel { get; set; }
        }
        private void UserSelectForm_SizeChanged(object sender, EventArgs e)
        {
            this.ucTestGridTableCustom1.Refresh();
        }

        private void UserSelectForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (closeState != 0)
            {
                this.Visible = false;
                if (homeForm.IsDisposed == false)
                {
                    homeForm.Visible = true;
                }
            }
        }

        private void ucBtnExt1_BtnClick(object sender, EventArgs e)
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
                myEventArgsUserInfoData.ReadIdCardFrmTemp = readIdCardFrm;
                OnMySendDataUserInfoData(myEventArgsUserInfoData);
            }
            else
            {
                MessageBox.Show(selectCheckData.message.ToString());
            }
        }
    }
}
