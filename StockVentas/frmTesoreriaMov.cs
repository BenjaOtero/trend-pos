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
    public partial class frmTesoreriaMov : Form
    {
        DataSet dsTesoreriaMov;
        DataTable tblTesoreriaMov;
        DataTable tblLocales;
        DataTable tblPcs;
        DataView viewTesoreria;
        DataView viewLocal;
        DataView viewPc; 
        DataRowView rowView;
        public int idLocal;
        public int idPc;
        public string PK = string.Empty;
        private int? codigoError = null;

        public frmTesoreriaMov()
        {
            InitializeComponent();
        }

        private void frmTesoreriaMov_Load(object sender, EventArgs e)
        {
            System.Drawing.Icon ico = Properties.Resources.icono_app;
            this.Icon = ico;
            this.ControlBox = true;
            this.MaximizeBox = false;
            tblLocales = BL.LocalesBLL.CrearDataset();
            viewLocal = new DataView(tblLocales);
            viewLocal.RowFilter = "IdLocalLOC = '13'";  // Local 13 es Jesus Maria
            lstLocales.ValueMember = "IdLocalLOC";
            lstLocales.DisplayMember = "NombreLOC";
            lstLocales.DataSource = viewLocal;
            tblPcs = BL.PcsBLL.CrearDataset();
            string local = lstLocales.SelectedValue.ToString();
            viewPc = new DataView(tblPcs);
            viewPc.RowFilter = "IdPC = '1'"; // Pc 1 es caja1 de Jesus Maria
            viewPc.Sort = "Detalle ASC";
            lstPc.ValueMember = "IdPC";
            lstPc.DisplayMember = "Detalle";
            lstPc.DataSource = viewPc; 
            this.lstLocales.SelectedValueChanged += new System.EventHandler(this.lstLocales_SelectedValueChanged);

            dsTesoreriaMov = BL.TesoreriaMovimientosBLL.CrearDataset();
            tblTesoreriaMov = dsTesoreriaMov.Tables[0];
            viewTesoreria = new DataView(tblTesoreriaMov);            
            if (PK == "")
            {
                Random rand = new Random();
                int clave = rand.Next(1, 2000000000);
                lblClave.Text = clave.ToString();
                lblClave.ForeColor = System.Drawing.Color.DarkRed;

                viewTesoreria.RowStateFilter = DataViewRowState.Added;
                rowView = viewTesoreria.AddNew();
                rowView["IdMovTESM"] = clave.ToString();
                rowView["FechaTESM"] = DateTime.Today;
                rowView["IdPcTESM"] = 1;
                rowView.EndEdit();                
            }
            else
            {
                lstLocales.SelectedValue = idLocal;
                lstPc.SelectedValue = idPc;
                lblClave.Text = PK;
                lblClave.ForeColor = System.Drawing.Color.DarkRed;
                viewTesoreria.RowFilter = "IdMovTESM = '" + PK + "'";
                rowView = viewTesoreria[0];
                txtDetalle.Focus();
            }
            dateTimePicker1.DataBindings.Add("Text", rowView, "FechaTESM", false, DataSourceUpdateMode.OnPropertyChanged);
            lstPc.DataBindings.Add("SelectedValue", rowView, "IdPcTESM", false, DataSourceUpdateMode.OnPropertyChanged);
            txtDetalle.DataBindings.Add("Text", rowView, "DetalleTESM", false, DataSourceUpdateMode.OnPropertyChanged);
            txtImporte.DataBindings.Add("Text", rowView, "ImporteTESM", false, DataSourceUpdateMode.OnPropertyChanged);
            txtImporte.KeyPress += new KeyPressEventHandler(BL.Utilitarios.SoloNumerosConComa);
        }

        private void frmTesoreriaMov_Shown(object sender, EventArgs e)
        {
            txtDetalle.Focus();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (txtDetalle.Text == "")
            {
                MessageBox.Show("Debe escribir un detalle", "Trend");
                txtDetalle.Focus();
                return;
            }
            if (txtImporte.Text == "")
            {
                MessageBox.Show("Debe escribir un importe", "Trend");
                txtImporte.Focus();
                return;
            }
            rowView.EndEdit();
            if (tblTesoreriaMov.GetChanges() != null)
            {
                Cursor.Current = Cursors.WaitCursor;
                BL.TesoreriaMovimientosBLL.GrabarDB(dsTesoreriaMov, ref codigoError, false);
                this.DialogResult = DialogResult.OK;
                Cursor.Current = Cursors.Arrow;
            }
            Close();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lstLocales_SelectedValueChanged(object sender, EventArgs e)
        {
            string local = lstLocales.SelectedValue.ToString();
            viewPc = new DataView(tblPcs);
            viewPc.RowFilter = "IdLocalPC = '" + local + "'";
            viewPc.Sort = "Detalle ASC";
            lstPc.ValueMember = "IdPC";
            lstPc.DisplayMember = "Detalle";
            lstPc.DataSource = viewPc;
        }



    }
}
