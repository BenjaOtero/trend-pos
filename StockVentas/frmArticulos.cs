using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data;
using BL;


namespace StockVentas
{
    public partial class frmArticulos : Form
    {
        private frmVentas formVentas = null;
        private DataTable tablaOrigen;
        private DataTable tablaArticulos;
        private DataView viewArticulos;
        public static string buscado;

        public frmArticulos()
        {
            InitializeComponent();
        }

        public frmArticulos(ref frmVentas f, DataTable tblArticulos)
        {
            InitializeComponent();
            formVentas = f;
            this.tablaArticulos = tblArticulos;
        }

        private void frmArticulos_Load(object sender, EventArgs e)
        {
            System.Drawing.Icon ico = Properties.Resources.icono_app;
            this.Icon = ico;
            tablaOrigen = BL.ArticulosBLL.CrearDataset();
            viewArticulos = new DataView(tablaOrigen);
            viewArticulos.RowFilter = "IdArticuloART LIKE '000000000'";
            gvwDatos.DataSource = viewArticulos;
            gvwDatos.Columns["IdArticuloART"].HeaderText = "Código";
            gvwDatos.Columns["DescripcionART"].HeaderText = "Descripción";
            gvwDatos.Columns["PrecioCostoART"].DefaultCellStyle.Format = "c";
            gvwDatos.Columns["PrecioCostoART"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            gvwDatos.Columns["PrecioMayorART"].HeaderText = "Precio";
            gvwDatos.Columns["PrecioMayorART"].DefaultCellStyle.Format = "c";
            gvwDatos.Columns["PrecioMayorART"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            gvwDatos.Columns["DescripcionWebART"].Visible = false;
            gvwDatos.Columns["ActivoWebART"].Visible = false;
            gvwDatos.Columns["IdItemART"].Visible = false;
            gvwDatos.Columns["IdColorART"].Visible = false;
            gvwDatos.Columns["TalleART"].Visible = false;
            gvwDatos.Columns["IdProveedorART"].Visible = false;
            gvwDatos.Columns["PrecioCostoART"].Visible = false;
            gvwDatos.Columns["PrecioPublicoART"].Visible = false;
            gvwDatos.Columns["DescripcionWebART"].Visible = false;
            gvwDatos.Columns["FechaART"].Visible = false;
            gvwDatos.Columns["ImagenART"].Visible = false;
            gvwDatos.Columns["ImagenBackART"].Visible = false;
            gvwDatos.Columns["ImagenColorART"].Visible = false;
            gvwDatos.Columns["NuevoART"].Visible = false;
            gvwDatos.Columns["IdAliculotaIvaART"].Visible = false;
            gvwDatos.Sort(gvwDatos.Columns["DescripcionART"], ListSortDirection.Ascending);
            gvwDatos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            btnAceptar.Enabled = false;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string parametros = this.txtParametros.Text;
            if (rdArticulo.Checked == true)
            {
                viewArticulos.RowFilter = "IdArticuloART LIKE '" + parametros + "*'";
                if (viewArticulos.Count == 0)
                {
                    MessageBox.Show("No se encontraron registros coincidentes", "Trend", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnAceptar.Enabled = false;
                }
                else
                {
                    btnAceptar.Enabled = true;
                }
            }
            else
            {
                viewArticulos.RowFilter = "DescripcionART LIKE '*" + parametros + "*'";
                if (viewArticulos.Count == 0)
                {
                    MessageBox.Show("No se encontraron registros coincidentes", "Trend", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnAceptar.Enabled = false;
                }
                else
                {
                    btnAceptar.Enabled = true;
                }
            }            
        }       

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (formVentas != null)
            {
                formVentas.idArticulo = ((DataRowView)gvwDatos.CurrentRow.DataBoundItem)["IdArticuloART"].ToString();
                Close();
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void gvwDatos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnAceptar.PerformClick();
        }

    }
}
