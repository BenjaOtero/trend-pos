using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using BL;

namespace StockVentas
{
    public partial class frmStockInter : Form
    {
        DataTable tblStock;
        DataTable tblLocales;
        DataTable dtCruzada;        
        int proveedor = 0;

        public frmStockInter()
        {
            InitializeComponent();
        }

        private void frmStockMovInter_Load(object sender, EventArgs e)
        {
            tblLocales = BL.LocalesBLL.CrearDataset();
            DataView viewLocales = new DataView(tblLocales);
            viewLocales = new DataView(tblLocales);
            viewLocales.RowFilter = "IdLocalLOC <>'1' AND IdLocalLOC <>'2'";
            lstLocales.DataSource = viewLocales;
            lstLocales.DisplayMember = "NombreLOC";
            lstLocales.ValueMember = "IdLocalLOC";
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            string whereLocales = null;
            string articulo = "";
            string descripcion = "";
            Cursor.Current = Cursors.WaitCursor;
            string idLocal;
            foreach (DataRowView filaLocal in lstLocales.SelectedItems)
            {
                idLocal = filaLocal.Row[0].ToString();
                whereLocales += "IdLocalSTK LIKE '" + idLocal + "' OR ";
            }
            whereLocales = whereLocales.Substring(0, whereLocales.Length - 4);
            if (rdArticulo.Checked == true)
            {
                articulo = txtParametros.Text;    
            }
            else
            {
                descripcion = txtParametros.Text;
            }
            string origen = "frmStock";
            string accion = "cargar";
            frmProgress newMDIChild = new frmProgress(origen, accion, whereLocales, proveedor, articulo, descripcion);
            newMDIChild.ShowDialog();
            tblStock = frmProgress.dtEstatico.Tables[0];
            if (rdPantalla.Checked == true)
            {
                try
                {
                    DataColumn columnaPivot = tblStock.Columns["NombreLOC"];
                    DataColumn valorPivot = tblStock.Columns["Cantidad"];
                    dtCruzada = BL.Utilitarios.Pivot(tblStock, columnaPivot, valorPivot);
                }
                catch
                {
                    if (whereLocales == null)
                    {
                        MessageBox.Show("Debe seleccionar un local.", "Trend", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No se encontraron artículos coincidentes", "Trend", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }                    
                    return;
                }
            }
            else
            { 
            
            }
            frmStockInforme stockInforme = new frmStockInforme(dtCruzada);
            stockInforme.ShowDialog();
            Cursor.Current = Cursors.Arrow;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
