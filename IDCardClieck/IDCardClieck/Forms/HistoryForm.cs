﻿using IDCardClieck.Common;
using IDCardClieck.Model;
using LiveCharts;
using LiveCharts.Wpf;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using static IDCardClieck.Controls.UCTestGridTableCustom;
using Brushes = System.Windows.Media.Brushes;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using Panel = System.Windows.Controls.Panel;

namespace IDCardClieck.Forms
{
    public partial class HistoryForm : Form
    {
        public event EventHandler MySendDataTableData;
        private ModelTets modelTets = null;
        CheckoutModel model = new CheckoutModel();

        private System.Windows.Forms.Timer Timer = null;

        public HistoryForm()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            InitializeComponent();
            Timer = new System.Windows.Forms.Timer() { Interval = 100 };
            Timer.Tick += new EventHandler(Timer_Tick);
            base.Opacity = 0;
            Timer.Start();
        }

        public HistoryForm(CheckoutModel checkoutModel, ModelTets modelTetsTemp)
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.model = checkoutModel;
            this.modelTets = modelTetsTemp;

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

        private void HistoryForm_Load(object sender, EventArgs e)
        {
            SimpleLoading loadingfrm = new SimpleLoading(this);
            //将Loaing窗口，注入到 SplashScreenManager 来管理
            SplashScreenManager loading = new SplashScreenManager(loadingfrm);
            loading.ShowLoading();
            try
            {
                MySendDataTableData += this.ucTestGridTable1.BindingData;
                SetCartesianChartData();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                loading.CloseWaitForm();
            }
        }

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x02000000;
        //        return cp;
        //    }
        //}

        /// <summary>
        /// 填充cartesianChart数据
        /// </summary>
        public void SetCartesianChartData()
        {
            if (this.model.res == 0)
            {
                string apistr = "http://26526tu163.zicp.vip/app/allInOneClient/getHistoryCheckData";
                //向java端进行注册请求
                StringBuilder postData = new StringBuilder();
                postData.Append("{");
                postData.Append("licence_code:\"" + this.model.sericalNumber + "\",");
                postData.Append("mac_code:\"" + this.model.registerCode + "\",");
                postData.Append("IDCard_code:\"" + this.modelTets.edzTemp.IDC + "\",");
                postData.Append("propId:\"" + this.modelTets.propID + "\"");
                postData.Append("}");
                //接口调用
                string strJSON = HttpHelper.PostUrl(apistr, postData.ToString());
                //返回结果
                HistoryCheckData json = HttpHelper.Deserialize<HistoryCheckData>(strJSON);
                if (json.result == "true")
                {
                    this.lbl_userInfo.Text = this.modelTets.edzTemp.Name + "   " + this.modelTets.edzTemp.IDC;
                    if (json.data.Count > 0)
                    {
                        double d;
                        List<HistoryCheckDataDataModel> dlist = json.data.Where(a => double.TryParse(a.propValue, out d)).ToList();

                        List<double> listY = dlist.Select(a => double.Parse(a.propValue.ToString())).ToList();
                        List<string> listX = dlist.Select(a => a.orderNo.ToString()).ToList();

                        var lineSeries = new LineSeries
                        {
                            Values = new ChartValues<double>(listY),
                            Fill = Brushes.Transparent,
                            StrokeThickness = 1,
                            PointGeometry = null,
                            Stroke = System.Windows.Media.Brushes.OrangeRed
                        };
                        var barSeries = new ColumnSeries
                        {
                            Values = new ChartValues<double>(listY),                                                                                                                                           
                            Stroke = System.Windows.Media.Brushes.Turquoise
                        };

                        cartesianChart1.Series.Add(lineSeries);
                        cartesianChart1.Series.Add(barSeries);


                        cartesianChart1.AxisX.Add(new Axis
                        {
                            Labels = listX
                        }) ;

                        MyEventArgsTableData myEventArgsTableData = new MyEventArgsTableData();
                        myEventArgsTableData.historyCheckData = json;
                        OnMySendDataTableData(myEventArgsTableData);
                    }
                }
                else
                {
                    MessageBox.Show(json.message.ToString());
                }
            }
        }

        public void OnMySendDataTableData(MyEventArgsTableData e)
        {
            if (MySendDataTableData != null)
            {
                MySendDataTableData(this, e);
            }
        }

        public class MyEventArgsTableData : EventArgs
        {
            public HistoryCheckData  historyCheckData { get; set; }
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myBtnExt2_BtnClick(object sender, EventArgs e)
        {
            this.Close();
            this.modelTets.UserSelectFormTemp.Visible = true;
            this.modelTets.HomeFormTemp.Visible = false;
        }

        /// <summary>
        /// 返回首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myBtnExt1_BtnClick(object sender, EventArgs e)
        {
            this.Close();
            this.modelTets.HomeFormTemp.Visible = true;
        }

        private void HistoryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            if (modelTets.HomeFormTemp.Visible!=true)
            {
                this.modelTets.HomeFormTemp.Visible = true;
            }
        }
    }
}
