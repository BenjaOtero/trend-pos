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
        DataSet dsForaneos;
        DataTable tblLocales;
        DataTable tblFormasPago;
        DateTime dtFechaHasta;
        frmProgress progreso;
        public string strFechaDesde;
        public string strFechaHasta;
        public int forma;
        string idLocal;
        string strLocales;

        public frmVentasPesosInter()
        {
            InitializeComponent();
        }

        private void frmVentasPesosInter_Load(object sender, EventArgs e)
        {
            System.Drawing.Icon ico = Properties.Resources.icono_app;
            this.Icon = ico;
            this.ControlBox = true;
            this.MaximizeBox = false;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            dsForaneos = BL.VentasBLL.CrearDatasetForaneos();

            tblLocales = dsForaneos.Tables[3];
            tblFormasPago = dsForaneos.Tables[2];
            DataView viewLocales = new DataView(tblLocales);
            viewLocales = new DataView(tblLocales);
            viewLocales.RowFilter = "IdLocalLOC <>'2' AND IdLocalLOC <>'1'";
            lstLocales.DataSource = viewLocales;
            lstLocales.DisplayMember = "NombreLOC";
            lstLocales.ValueMember = "IdLocalLOC";
            cmbForma.DataSource = tblFormasPago;
            cmbForma.ValueMember = "IdFormaPagoFOR";
            cmbForma.DisplayMember = "DescripcionFOR";
            cmbForma.SelectedValue = 99;
            lstLocales.SelectionMode = SelectionMode.MultiSimple;
            lstLocales.SelectedIndex = -1;
            DateTime baseDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dateTimeDesde.Value = baseDate;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if(lstLocales.SelectedIndex == -1)
            {
            MessageBox.Show("Debe seleccionar un local.", "Trend",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (rdTotales.Checked)
            {
                strLocales = string.Empty;
                forma = Convert.ToInt32(cmbForma.SelectedValue.ToString());
                strFechaDesde = dateTimeDesde.Value.ToString("yyyy-MM-dd 00:00:00");
                dtFechaHasta = dateTimeHasta.Value.AddDays(1);
                strFechaHasta = dtFechaHasta.ToString("yyyy-MM-dd 00:00:00");
                foreach (DataRowView filaLocal in lstLocales.SelectedItems)
                {
                    idLocal = filaLocal.Row[0].ToString();
                    strLocales += "IdLocalLOC LIKE '" + idLocal + "' OR ";

                }
                strLocales = strLocales.Substring(0, strLocales.Length - 4);
                progreso = new frmProgress(forma, strFechaDesde, strFechaHasta, strLocales, "frmVentasPesosCons", "cargar");
                progreso.ShowDialog();
                DataTable tblVentasPesos = frmProgress.dsVentasPesosCons.Tables[0];
                frmVentasPesosCons frm = new frmVentasPesosCons(tblVentasPesos);
                frm.Show();
            }
            else
            {
                string origen = "frmVentasPesosInter_diarias";
                string accion = "cargar";
                string fecha_desde = dateTimeDesde.Value.ToString("yyyy-MM-dd");
                dtFechaHasta = dateTimeHasta.Value.AddDays(1);
                string fecha_hasta = dtFechaHasta.ToString("yyyy-MM-dd");
                int local = Convert.ToInt32(lstLocales.SelectedValue.ToString());                
                string formaPago = cmbForma.Text;
                frmProgress newMDIChild = new frmProgress(fecha_desde, fecha_hasta, local, formaPago, origen, accion);
                newMDIChild.ShowDialog();
                DataTable tblVentasDiarias = frmProgress.tblEstatica;                
                fecha_desde = dateTimeDesde.Value.ToString("dd-MM-yyyy");
                fecha_hasta = dateTimeHasta.Value.ToString("dd-MM-yyyy");
                string nombreLocal = lstLocales.Text;
                frmVentasPesosDiarias frmDiarias = new frmVentasPesosDiarias(tblVentasDiarias, fecha_desde, fecha_hasta, nombreLocal);
                frmDiarias.Show();
            }

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void rdTotales_Click(object sender, EventArgs e)
        {
            lstLocales.SelectedItem = 0;
            lstLocales.SelectionMode = SelectionMode.MultiSimple;
        }

        private void rdDiarios_Click(object sender, EventArgs e)
        {
            lstLocales.SelectedItem = 0;
            lstLocales.SelectionMode = SelectionMode.One;
        }



    }
}
