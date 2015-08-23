using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace StockVentas
{
    public partial class frmVentasPesosInter : Form
    {
        DataTable tblLocales;
        DataTable tblFormasPago;

        public frmVentasPesosInter()
        {
            InitializeComponent();
        }

        private void frmVentasPesosInter_Load(object sender, EventArgs e)
        {
            tblLocales = BL.LocalesBLL.CrearDataset();
            tblFormasPago = BL.FormasPagoBLL.CrearDataset();
            DataView viewLocales = new DataView(tblLocales);
            viewLocales = new DataView(tblLocales);
            viewLocales.RowFilter = "IdLocalLOC <>'2' AND IdLocalLOC <>'1' AND IdLocalLOC <>'11' AND IdLocalLOC <>'12'";
            lstLocales.DataSource = viewLocales;
            lstLocales.DisplayMember = "NombreLOC";
            lstLocales.ValueMember = "IdLocalLOC";
            DataRow row = tblFormasPago.NewRow();
            row["IdFormaPagoFOR"] = 99;
            row["DescripcionFOR"] = "TODAS";
            tblFormasPago.Rows.Add(row);
            cmbForma.DataSource = tblFormasPago;
            cmbForma.ValueMember = "IdFormaPagoFOR";
            cmbForma.DisplayMember = "DescripcionFOR";
            lstLocales.SelectionMode = SelectionMode.MultiSimple;
            lstLocales.SelectedIndex = -1;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (lstLocales.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un local", "Trend", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Cursor.Current = Cursors.WaitCursor;
            frmVentasPesosCons frm = new frmVentasPesosCons(lstLocales);
            frm.fechaDesde = dateTimeDesde.Value.ToString("yyyy-MM-dd 00:00:00");
            frm.fechaHasta = dateTimeHasta.Value.ToString("yyyy-MM-dd HH:mm:ss");
            frm.forma = Convert.ToInt32(cmbForma.SelectedValue.ToString());
            frm.ShowDialog();
            Cursor.Current = Cursors.Arrow;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }


    }
}
