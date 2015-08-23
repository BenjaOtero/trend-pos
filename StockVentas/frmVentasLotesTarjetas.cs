using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StockVentas
{
    public partial class frmVentasLotesTarjetas : Form
    {
        public frmVentasLotesTarjetas()
        {
            InitializeComponent();
        }

        private void frmVentasLotesTarjetas_Load(object sender, EventArgs e)
        {
            System.Drawing.Icon ico = Properties.Resources.icono_app;
            this.Icon = ico;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            textBox1.Text = DateTime.Today.ToLongDateString();
            DataSet dt = BL.VentasBLL.GetLotesTarjetas(1);
            dataGridView1.DataSource = dt.Tables[0];
            dataGridView1.Columns["DescripcionFOR"].HeaderText = "Tarjeta";
            dataGridView1.Columns["subtotal_tarjeta"].HeaderText = "Total";
            dataGridView1.Columns["subtotal_tarjeta"].DefaultCellStyle.Format = "c";
            dataGridView1.Columns["subtotal_tarjeta"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            DataTable tblTotal = dt.Tables[1];
            txtTotal.TextAlign = HorizontalAlignment.Left;
            string total = tblTotal.Rows[0][0].ToString();
            txtTotal.Text = "$ " + total;


        }
    }
}
