using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using BL;

namespace StockVentas
{
    public partial class frmArqueoCaja : Form
    {
        public frmArqueoCaja frmInstancia;
        DataSet dt;
        DataTable tblArqueo;
        DataTable tblVentas;
        DataTable tblVentasDetalle;
        DataTable tblArticulos;
        DataTable tblTesoreria;
        DataTable tblSumaTesoreria;
        DataTable tblFondoCajaInicial;
        DataTable tblFondoCajaFinal;
        DataTable tblEfectivo;
        DataTable tblTarjeta;
        DataView viewVentas;
        public int idLocal;
        public string nombreLocal;
        public int idPc;
        private int? codigoError = null;
        public int primerArranque;
        string strFechaDesde;
        string strFechaHasta;
        DateTimePicker dtPicker;

        public frmArqueoCaja(DateTimePicker dtPicker)
        {
            InitializeComponent();
            Cursor.Current = Cursors.WaitCursor;
            this.dtPicker = dtPicker;
            DataGridViewImageColumn imageColumn2 = new DataGridViewImageColumn();
            Image image2 = global::StockVentas.Properties.Resources.document_edit;
            imageColumn2.Image = image2;
            imageColumn2.Name = "Editar";
            dgvTesoreria.Columns.Add(imageColumn2);
            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            Image image = global::StockVentas.Properties.Resources.delete16;
            imageColumn.Image = image;
            imageColumn.Name = "Borrar";
            dgvTesoreria.Columns.Add(imageColumn);
            dgvTesoreria.CellClick += new DataGridViewCellEventHandler(dgvTesoreria_CellClick);
            DataGridViewImageColumn imageColumn3 = new DataGridViewImageColumn();
            imageColumn3.Image = image2;
            imageColumn3.Name = "Editar";
            dgvVentas.Columns.Add(imageColumn3);
            DataGridViewImageColumn imageColumn4 = new DataGridViewImageColumn();
            imageColumn4.Image = image;
            imageColumn4.Name = "Borrar";
            dgvVentas.Columns.Add(imageColumn4);
            dgvVentas.CellClick += new DataGridViewCellEventHandler(dgvVentas_CellClick);
            tblArticulos = BL.ArticulosBLL.CrearDataset();
        }

        private void frmArqueoCaja_Load(object sender, EventArgs e)
        {
            System.Drawing.Icon ico = Properties.Resources.icono_app;
            this.Icon = ico;
            lblLocal.Text = "Jesus Maria";
            lblFecha.Text = dtPicker.Value.ToLongDateString();
        }

        private void frmArqueoCajaAdmin_Activated(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgvVentas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dtPicker.Value.Date != DateTime.Today) return;
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == dgvVentas.Columns["Editar"].Index)
            {
                string idVenta = dgvVentas.CurrentRow.Cells["IdVentaVEN"].Value.ToString();
                frmVentas ventas = new frmVentas(idVenta, idPc, tblVentas, tblVentasDetalle);
                ventas.FormClosed += editVentas_FormClosed;
                ventas.ShowDialog();                
            }
            if (e.ColumnIndex == dgvVentas.Columns["Borrar"].Index)
            {
                if (MessageBox.Show("¿Desea borrar este registro y todos los movimientos relacionados?", "Trend",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    int PK = Convert.ToInt32(dgvVentas.CurrentRow.Cells["IdVentaVEN"].Value.ToString());
                    viewVentas = new DataView(tblVentas);
                    viewVentas.RowFilter = "[IdVentaVEN] = '" + PK + "'";
                    foreach (DataRowView row in viewVentas)
                    {
                        row.Delete();
                    }                    
                    bool borrarRemotas = false;
                    BL.TransaccionesBLL.BorrarVentasByPK(PK, ref codigoError, borrarRemotas);
                    if (codigoError != null)
                    {
                        Close();
                        return;
                    }
                    tblVentas.AcceptChanges();
                    CargarDatos();
                    Cursor.Current = Cursors.Arrow;
                }
            }
        }

        void editVentas_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form frm = sender as Form;
            if (frm.DialogResult == DialogResult.OK)
            {
                CargarDatos();
            }
        }

        private void dgvTesoreria_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dtPicker.Value.Date != DateTime.Today) return;
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == dgvTesoreria.Columns["Editar"].Index)
            {
                frmTesoreriaMov frm = new frmTesoreriaMov();
                frm.FormClosed += editTesoreria_FormClosed;
                frm.PK = dgvTesoreria.CurrentRow.Cells["IdMovTESM"].Value.ToString();
                frm.idLocal = idLocal;
                frm.idPc = idPc;
                frm.Show();               
            }
            if (e.ColumnIndex == dgvTesoreria.Columns["Borrar"].Index)
            {
                if (MessageBox.Show("¿Desea borrar este registro?", "Trend",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    int PK = Convert.ToInt32(dgvTesoreria.CurrentRow.Cells["IdMovTESM"].Value.ToString());
                    BL.TesoreriaMovimientosBLL.BorrarByPK(PK, ref codigoError, false);
                    if (codigoError != null)
                    {
                        Close();
                        return;
                    }                    
                    dgvTesoreria.Rows.Remove(dgvTesoreria.Rows[e.RowIndex]);
                    tblTesoreria.AcceptChanges();
                    CargarDatos();
                    Cursor.Current = Cursors.Arrow;
                }
            }
        }

        void editTesoreria_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form frm = sender as Form;
            if (frm.DialogResult == DialogResult.OK)
            {
                CargarDatos();
            }
        }

        public void OrganizarTablas()
        {
            tblArqueo = dt.Tables[0].Copy();
            tblArqueo.Columns.Remove("IdClienteVEN");
            tblArqueo.Columns.Remove("IdDVEN");
            tblArqueo.Columns.Remove("IdVentaDVEN");
            tblArqueo.Columns.Remove("IdLocalDVEN");
            tblArqueo.Columns.Remove("PrecioCostoDVEN");
            tblArqueo.Columns.Remove("PrecioMayorDVEN");
            tblArqueo.Columns.Remove("IdFormaPagoDVEN");
            tblArqueo.Columns.Remove("NroCuponDVEN");
            tblArqueo.Columns.Remove("IdEmpleadoDVEN");
            tblArqueo.Columns.Remove("NroFacturaDVEN");
            tblArqueo.Columns.Remove("LiquidadoDVEN");
            tblArqueo.Columns.Remove("EsperaDVEN");
            tblArqueo.Columns.Remove("DevolucionDVEN");
            tblVentas = dt.Tables[0].Copy();
            tblVentas.TableName = "Ventas";
            tblVentas.Columns.Remove("IdDVEN");
            tblVentas.Columns.Remove("IdVentaDVEN");
            tblVentas.Columns.Remove("IdLocalDVEN");
            tblVentas.Columns.Remove("IdArticuloDVEN");
            tblVentas.Columns.Remove("Descripcion");
            tblVentas.Columns.Remove("CantidadDVEN");
            tblVentas.Columns.Remove("PrecioPublicoDVEN");
            tblVentas.Columns.Remove("PrecioCostoDVEN");
            tblVentas.Columns.Remove("PrecioMayorDVEN");
            tblVentas.Columns.Remove("IdFormaPagoDVEN");
            tblVentas.Columns.Remove("NroCuponDVEN");
            tblVentas.Columns.Remove("IdEmpleadoDVEN");
            tblVentas.Columns.Remove("NroFacturaDVEN");
            tblVentas.Columns.Remove("LiquidadoDVEN");
            tblVentas.Columns.Remove("EsperaDVEN");
            tblVentas.Columns.Remove("DevolucionDVEN");
            tblVentas.Columns.Remove("Forma pago");
            tblVentas.Columns.Remove("Subtotal");
            tblVentas.AcceptChanges();
            tblVentasDetalle = dt.Tables[0].Copy();
            tblVentasDetalle.TableName = "VentasDetalle";
            tblVentasDetalle.Columns.Remove("IdVentaVEN");
            tblVentasDetalle.Columns.Remove("FechaVEN");
            tblVentasDetalle.Columns.Remove("IdPCVEN");
            tblVentasDetalle.Columns.Remove("IdClienteVEN");
            tblVentasDetalle.Columns.Remove("Forma pago");
            tblVentasDetalle.Columns.Remove("Subtotal");
            tblVentasDetalle.Columns["Descripcion"].ColumnName = "DescripcionDVEN";
            tblVentasDetalle.AcceptChanges();
        }

        public void CargarDatos()
        {
            Cursor.Current = Cursors.WaitCursor;
            idPc = 1;
            idLocal = 13;
       //     strFechaDesde = DateTime.Today.ToString("yyyy-MM-dd 00:00:00"); //fecha string para mysql
            strFechaDesde = dtPicker.Value.ToString("yyyy-MM-dd 00:00:00"); //fecha string para mysql

            strFechaHasta = dtPicker.Value.AddDays(1).ToString("yyyy-MM-dd 00:00:00");
            dt = BL.VentasBLL.CrearDatasetArqueo(strFechaDesde, strFechaHasta, idPc, ref codigoError);
            if (dt == null)
            {
                Close();
                return;
            }
            OrganizarTablas();
            tblFondoCajaInicial = dt.Tables[1];
            tblFondoCajaFinal = dt.Tables[2];
            tblTesoreria = dt.Tables[3];
            tblEfectivo = dt.Tables[4];
            tblTarjeta = dt.Tables[5];
            tblSumaTesoreria = dt.Tables[6];            
            dt.Tables[3].TableName = "TesoreriaMovimientos";
            dgvVentas.DataSource = tblArqueo;
            
            dgvVentas.Columns["IdPCVEN"].Visible = false;
            dgvVentas.Columns["Subtotal"].Visible = false;
            dgvVentas.Columns["IdVentaVEN"].HeaderText = "Nº venta";
            dgvVentas.Columns["FechaVEN"].HeaderText = "Fecha";
            dgvVentas.Columns["IdArticuloDVEN"].HeaderText = "Artículo";
            dgvVentas.Columns["CantidadDVEN"].HeaderText = "Cantidad";
            dgvVentas.Columns["PrecioPublicoDVEN"].HeaderText = "Precio";
            dgvTesoreria.DataSource = tblTesoreria;
            dgvTesoreria.AllowUserToOrderColumns = false;
            dgvTesoreria.Columns["FechaTESM"].Visible = false;
            dgvTesoreria.Columns["IdPcTESM"].Visible = false;
            dgvTesoreria.Columns["IdMovTESM"].HeaderText = "Nº mov.";
            dgvTesoreria.Columns["DetalleTESM"].HeaderText = "Detalle";
            dgvTesoreria.Columns["ImporteTESM"].HeaderText = "Importe";
            double dblEfectivo;
            if (String.IsNullOrEmpty(tblEfectivo.Rows[0][0].ToString()))
            {
                dblEfectivo = 0;                
            }
            else
            {
                dblEfectivo = Convert.ToDouble(tblEfectivo.Rows[0][0].ToString());
            }
            double dblTarjeta;
            if (String.IsNullOrEmpty(tblTarjeta.Rows[0][0].ToString()))
            {
                dblTarjeta = 0;
            }
            else
            {
                dblTarjeta = Convert.ToDouble(tblTarjeta.Rows[0][0].ToString());
            }
            double dblCajaInicial;
            if (tblFondoCajaInicial.Rows.Count > 0)
            {
                dblCajaInicial = Convert.ToDouble(tblFondoCajaInicial.Rows[0][0].ToString());
            }
            else
            {
                dblCajaInicial = 0;
            }
            double dblTesoreria;
            if (String.IsNullOrEmpty(tblSumaTesoreria.Rows[0][0].ToString()))
            {
                dblTesoreria = 0;
            }
            else
            {
                dblTesoreria = Convert.ToDouble(tblSumaTesoreria.Rows[0][0].ToString());
            }
            double dblEftvoExistente = dblEfectivo + dblCajaInicial + dblTesoreria;
            double dblCajaFinal;
            if (tblFondoCajaFinal.Rows.Count > 0)
            {
                dblCajaFinal = Convert.ToDouble(tblFondoCajaFinal.Rows[0][0].ToString());
            }
            else
            {
                dblCajaFinal = 0;
            }
            double dblEftvoEntregar = dblEftvoExistente - dblCajaFinal;
            double dblVentaTotal = dblEfectivo + dblTarjeta;
            lblEfectivo.Text = "$ " + dblEfectivo.ToString();
            lblTarjeta.Text = "$ " + dblTarjeta.ToString();
            lblCajaInicial.Text = "$ " + dblCajaInicial.ToString();
            lblTesoreria.Text = "$ " + dblTesoreria.ToString();
            lblEfectivoExistente.Text = "$ " + dblEftvoExistente.ToString();
            lblCajaFinal.Text = "$ " + dblCajaFinal.ToString();
            lblEfectivoEntregar.Text = "$ " + dblEftvoEntregar.ToString();
            lblTotal.Text = "$ " + dblVentaTotal.ToString();   
        }

    }

}
