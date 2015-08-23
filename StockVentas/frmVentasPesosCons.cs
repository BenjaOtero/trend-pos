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
    public partial class frmVentasPesosCons : Form
    {
        public string fechaDesde;
        public string fechaHasta;
        public int forma;
        ListBox lstLocales;
        public int formaPago;
        string idLocal;
        string strLocales;
        string formularioOrigen = "frmVentasPesosCons";
        string accionProgress = "cargar";
        frmProgress progreso;

        public frmVentasPesosCons(ListBox locales)
        {
            InitializeComponent();
            this.lstLocales = locales;
        }

        private void frmVentasPesosCons_Load(object sender, EventArgs e)
        {
            foreach (DataRowView filaLocal in lstLocales.SelectedItems)
            {
                idLocal = filaLocal.Row[0].ToString();
                strLocales += "IdLocalLOC LIKE '" + idLocal + "' OR ";
                
            }
            strLocales = strLocales.Substring(0, strLocales.Length - 4);
            progreso = new frmProgress(forma, fechaDesde, fechaHasta, strLocales, formularioOrigen, accionProgress);
            progreso.ShowDialog();
            try
            {
                dataGridView1.DataSource = frmProgress.dsVentasPesosCons.Tables[0];
            }
            catch(Exception)
            {
                Close();
                return;
            }
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Columns["Venta"].DefaultCellStyle.Format = "C0";
            dataGridView1.Columns["Costo"].DefaultCellStyle.Format = "C0";
            dataGridView1.Columns["Utilidad bruta"].DefaultCellStyle.Format = "C0";
            dataGridView1.Columns["Valor agregado"].DefaultCellStyle.Format = "0\\%";
        }
    }
}
