using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatagridviewTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.dataGridView1.Columns.Add("a", "a");
            this.dataGridView1.Columns.Add("b", "b");
            this.dataGridView1.Columns.Add("c", "c");

            for (int i = 0; i < 3; i++)
                this.dataGridView1.Rows.Add();

            for (int i = 0; i < 3; i++)
            {
                Label[] lab = new Label[2];
                lab[0] = new Label();
                lab[0].Text = "one";
                lab[1] = new Label();
                lab[1].Text = "two";
                this.dataGridView1.Controls.Add(lab[0]);
                this.dataGridView1.Controls.Add(lab[1]);
                Rectangle rect = this.dataGridView1.GetCellDisplayRectangle(2, i, false);
                lab[0].Size = lab[1].Size = new Size(rect.Width / 2, rect.Height);
                lab[0].Location = new Point(rect.Left, rect.Top);
                lab[1].Location = new Point(rect.Left + lab[0].Width, rect.Top);
                lab[0].Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                lab[1].Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                lab[0].Click += new EventHandler(CustomBtn_Click);
                lab[1].Click += new EventHandler(CustomBtn_Click);
            }
        }

        void CustomBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show((sender as Label).Text);
        }

        //在有滚动条时，在下面的两个事件中重定位一下Button的位置:
        //  滚动DataGridView时调整Button位置
        private void DataGridView_Scroll(object sender, ScrollEventArgs e)
        {

        }
        //  改变DataGridView列宽时调整Button位置
        private void DataGridView_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
        }
    }
}
