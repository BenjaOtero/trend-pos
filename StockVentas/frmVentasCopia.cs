using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic;

namespace StockVentas
{
    public partial class frmVentasCopia : Form
    {
        private frmVentasCopia instancia;
        public DataSet dsVentas;
        DataTable tblVentas;
        DataTable tblVentasDetalle;
        DataView viewVentas;
        DataView viewDetalle;
        DataRowView rowView;
        DataTable tblLocales;
        DataTable tblPcs;
        DataTable tblClientes;
        DataTable tblArticulos;
        DataTable tblFormasPago;
        DataView viewLocal;
        bool editar = false;
        public string PK = string.Empty;
        public int idPc;
        public string idArticulo;
        public bool formClosing = false;

        public enum FormState
        {
            inicial,
            edicion,
            insercion,
            eliminacion
        }

        public frmVentasCopia()
        {
            InitializeComponent();
            instancia = this;
            tblVentas = BL.VentasBLL.GetTabla();
            tblVentasDetalle = BL.VentasDetalleBLL.GetTabla();
            cargarComboLocales();
        }

        public frmVentasCopia(string PK, int idPc, DataTable tblVentas, DataTable tblVentasDetalle):this()
        {
            this.PK = PK;
            this.idPc = idPc;
            this.tblVentas = tblVentas;
            tblVentas.TableName = "Ventas";
            this.tblVentasDetalle = tblVentasDetalle;
            tblVentasDetalle.TableName = "VentasDetalle";
            tblLocales = BL.GetDataBLL.Locales();
            tblPcs = BL.GetDataBLL.Pc();
            var query =
                from local in tblLocales.AsEnumerable()
                from pc in tblPcs.AsEnumerable()
                where (local.Field<Int32>("IdLocalLOC") == pc.Field<Int32>("IdLocalPC"))
                    && (pc.Field<int>("IdPC") == idPc)
                select new
                {
                    Local = local.Field<string>("NombreLOC"),
                    IdPc = pc.Field<Int32>("IdPC"),
                    IdLocal = local.Field<Int32>("IdLocalLOC")
                };
            DataTable dtTemp = new DataTable();
            dtTemp.Columns.Add("IdPC", typeof(Int32));
            dtTemp.Columns.Add("IdLocalLOC", typeof(Int32));
            dtTemp.Columns.Add("NombreLOC", typeof(string));
            foreach (var registro in query)
            {
                DataRow fila = dtTemp.NewRow();
                fila["IdPC"] = registro.IdPc;
                fila["IdLocalLOC"] = registro.IdLocal;
                fila["NombreLOC"] = registro.Local;
                dtTemp.Rows.Add(fila);
            }
            viewLocal = new DataView(dtTemp);
            cmbLocal.ValueMember = "IdPC";
            cmbLocal.DisplayMember = "NombreLOC";
            cmbLocal.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLocal.DataSource = viewLocal;
        }

        private void frmVentas_Load(object sender, EventArgs e)
        {
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ControlBox = true;
            this.MaximizeBox = false;
            tblArticulos = BL.GetDataBLL.Articulos();
            tblArticulos.TableName = "Articulos";
            var source = new AutoCompleteStringCollection();
            String[] stringArray =
                Array.ConvertAll<DataRow, String>(tblArticulos.Select(), delegate(DataRow row) { return (String)row["IdArticuloART"]; });
            source.AddRange(stringArray);
            txtArticulo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtArticulo.AutoCompleteCustomSource = source;
            txtArticulo.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtArticulo.BackColor = Color.White;
            txtDescripcion.BackColor = Color.White;
            txtDescripcion.TabStop = false;
            txtDescripcion.ReadOnly = true;
            txtCantidad.BackColor = Color.White;
            txtPrecio.BackColor = Color.White;            
            tblFormasPago = BL.GetDataBLL.FormasPago();
            cmbForma.ValueMember = "IdFormaPagoFOR";
            cmbForma.DisplayMember = "DescripcionFOR";
            cmbForma.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbForma.DataSource = tblFormasPago;
            cmbForma.SelectedValue = -1;
            cmbForma.BackColor = Color.White;
            lblCosto.Visible = false;
            txtCosto.Visible = false;
            btnEditar.CausesValidation = false;
            btnBorrar.CausesValidation = false;
            btnArticulos.CausesValidation = false;            
            dsVentas = new DataSet();
            dsVentas.DataSetName = "dsVentas";
            if (tblVentas.DataSet == null)
            {
                dsVentas.Tables.Add(tblVentas);
                dsVentas.Tables.Add(tblVentasDetalle);
            }
            tblVentasDetalle.PrimaryKey = new DataColumn[] { tblVentasDetalle.Columns["IdDVEN"] };
            viewVentas = new DataView(tblVentas);
            viewDetalle = new DataView(tblVentasDetalle);
            bindingSource1.DataSource = viewDetalle;
            bindingNavigator1.BindingSource = bindingSource1;
            dgvDatos.ReadOnly = true;
            dgvDatos.AllowUserToAddRows = false;
            dgvDatos.DataSource = bindingSource1;
            dgvDatos.Columns["IdDVEN"].Visible = false;
            dgvDatos.Columns["IdVentaDVEN"].Visible = false;
            dgvDatos.Columns["IdLocalDVEN"].Visible = false;
            dgvDatos.Columns["PrecioCostoDVEN"].Visible = false;
            dgvDatos.Columns["PrecioMayorDVEN"].Visible = false;
            dgvDatos.Columns["IdFormaPagoDVEN"].Visible = false;
            dgvDatos.Columns["NroCuponDVEN"].Visible = false;
            dgvDatos.Columns["IdEmpleadoDVEN"].Visible = false;
            dgvDatos.Columns["LiquidadoDVEN"].Visible = false;
            dgvDatos.Columns["EsperaDVEN"].Visible = false;
            dgvDatos.Columns["CantidadDVEN"].Width = 55;
            dgvDatos.Columns["CantidadDVEN"].HeaderText = "Cantidad";
            dgvDatos.Columns["IdArticuloDVEN"].HeaderText = "Artículo";
            dgvDatos.Columns["PrecioPublicoDVEN"].Width = 100;
            dgvDatos.Columns["PrecioPublicoDVEN"].HeaderText = "Precio";
            dgvDatos.Columns["PrecioPublicoDVEN"].DefaultCellStyle.Format = "C2";
            dgvDatos.Columns["NroFacturaDVEN"].Width = 100;
            dgvDatos.Columns["NroFacturaDVEN"].HeaderText = "Nº fact.";
            dgvDatos.Columns.Remove("DevolucionDVEN");            
            dgvDatos.Columns.Remove("IdFormaPagoDVEN");
            DataGridViewComboBoxColumn cmbFormaPago = new DataGridViewComboBoxColumn();
            cmbFormaPago.Name = "FormaPago";
            cmbFormaPago.HeaderText = "Forma de pago";
            cmbFormaPago.DataSource = tblFormasPago;
            cmbFormaPago.ValueMember = "IdFormaPagoFOR";
            cmbFormaPago.DisplayMember = "DescripcionFOR";
            cmbFormaPago.DataPropertyName = "IdFormaPagoDVEN";
            dgvDatos.Columns.Insert(7, cmbFormaPago);
            DataGridViewCheckBoxColumn chkDevolucion = new DataGridViewCheckBoxColumn();
            chkDevolucion.Name = "DevolucionDVEN";
            chkDevolucion.Width = 40;
            chkDevolucion.HeaderText = "Dev.";
            chkDevolucion.DataPropertyName = "DevolucionDVEN";
            chkDevolucion.TrueValue = 1;
            chkDevolucion.FalseValue = 0;
            dgvDatos.Columns.Insert(12, chkDevolucion);
            dgvDatos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            lblTotal.Text = "$ 0";
            if (PK == "") //registro nuevo
            {
                lblNro.ForeColor = System.Drawing.Color.DarkRed;
                Random rand = new Random();
                int clave = rand.Next(1, 2000000000);
                lblNro.Text = clave.ToString();
                rowView = viewVentas.AddNew();
                rowView["IdVentaVEN"] = clave.ToString();
                rowView["FechaVEN"] = DateTime.Now;
                rowView.EndEdit();
                SetStateForm(FormState.inicial);   
            }
            else // editar registros
            {
                viewVentas.RowFilter = "IdVentaVEN = '" + PK + "'";
                rowView = viewVentas[0];
                viewDetalle.RowFilter = "IdVentaDVEN = '" + PK + "'";
                lblNro.Text = viewVentas[0][0].ToString();
                cmbLocal.Enabled = false;
                lblTotal.Text = "$ " + CalcularTotal();
                dateTimePicker1.TabStop = false;
                cmbLocal.TabStop = false;                
                cmbCliente.Focus();
                SetStateForm(FormState.insercion);   
            }
            dateTimePicker1.DataBindings.Add("Text", rowView, "FechaVEN", false, DataSourceUpdateMode.OnPropertyChanged);
            cmbLocal.DataBindings.Add("SelectedValue", rowView, "IdPCVEN", false, DataSourceUpdateMode.OnPropertyChanged);
            cmbCliente.DataBindings.Add("SelectedValue", rowView, "IdClienteVEN", false, DataSourceUpdateMode.OnPropertyChanged);
            rowView.CancelEdit();
            cmbLocal.SelectedIndexChanged += new EventHandler(this.ValidarMaestro);
            cmbCliente.SelectedIndexChanged += new EventHandler(this.ValidarMaestro);
            txtPrecio.KeyPress += new KeyPressEventHandler(BL.Utilitarios.SoloNumerosConComa);
            txtCantidad.KeyPress += new KeyPressEventHandler(BL.Utilitarios.SoloNumeros);
            txtArticulo.Enter += new System.EventHandler(Utilitarios.SelTextoTextBox);
            txtCantidad.Enter += new System.EventHandler(Utilitarios.SelTextoTextBox);
            txtPrecio.Enter += new System.EventHandler(Utilitarios.SelTextoTextBox);
            cmbForma.Enter += new System.EventHandler(Utilitarios.SelTextoTextBox);
            txtArticulo.KeyDown += new System.Windows.Forms.KeyEventHandler(Utilitarios.EnterTab);
            txtCantidad.KeyDown += new System.Windows.Forms.KeyEventHandler(Utilitarios.EnterTab);
            txtPrecio.KeyDown += new System.Windows.Forms.KeyEventHandler(Utilitarios.EnterTab);
            cmbForma.KeyDown += new System.Windows.Forms.KeyEventHandler(Utilitarios.EnterTab);
            chkDev.KeyDown += new System.Windows.Forms.KeyEventHandler(Utilitarios.EnterTab);            
        }

        private void frmVentas_Activated(object sender, EventArgs e)
        {
            tblClientes = BL.GetDataBLL.Clientes();
            tblClientes.DefaultView.Sort = "RazonSocialCLI";
            cmbCliente.ValueMember = "IdClienteCLI";
            cmbCliente.DisplayMember = "RazonSocialCLI";
            cmbCliente.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCliente.DataSource = tblClientes;
            cmbCliente.SelectedValue = 1;
        }

        private void txtArticulo_Validating(object sender, CancelEventArgs e)
        {
            DataRow[] foundRow = tblArticulos.Select("IdArticuloART = '" + txtArticulo.Text + "'");
            if (foundRow.Length == 0)
            {
                e.Cancel = true;
            }
            else
            {
                DataRow filaActual = foundRow[0];
                txtDescripcion.Text = filaActual["DescripcionART"].ToString();
                if (txtArticulo.Text == "000000004" || txtArticulo.Text == "000000006") // seña y nota de credito entra factura
                {
                    txtCantidad.Text = "-1";
                }
                else
                {
                    txtCantidad.Text = "1";
                }
                txtPrecio.Text = filaActual["PrecioPublicoART"].ToString();
                cmbForma.SelectedValue = "1";
                txtCosto.Text = filaActual["PrecioCostoART"].ToString();
            }
        }

        private void txtCantidad_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCantidad.Text) || txtCantidad.Text == "0")
            {
                e.Cancel = true;
            }
            if (txtArticulo.Text == "000000004" || txtArticulo.Text == "000000006") // seña y nota de credito entra factura
            {
                txtCantidad.Text = "-1";
            }
            if (chkDev.Checked)
            {
                int cantidad = Convert.ToInt32(txtCantidad.Text);
                if (cantidad > 0) cantidad = cantidad * -1;
                else cantidad = cantidad * 1;
                txtCantidad.Text = cantidad.ToString();
            }
        }

        private void txtPrecio_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPrecio.Text))
            {
                e.Cancel = true;
            }
        }

        private void chkDev_Validating(object sender, CancelEventArgs e)
        {
            if (chkDev.Checked)
            {
                int cantidad = Convert.ToInt32(txtCantidad.Text);
                if (cantidad > 0) cantidad = cantidad * -1;
                else cantidad = cantidad * 1;
                txtCantidad.Text = cantidad.ToString();
            }
        }

        private void dgvDatos_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txtArticulo.Text = dgvDatos.CurrentRow.Cells["IdArticuloDVEN"].Value.ToString();
                txtDescripcion.Text = dgvDatos.CurrentRow.Cells["DescripcionDVEN"].Value.ToString();
                txtCantidad.Text = dgvDatos.CurrentRow.Cells["CantidadDVEN"].Value.ToString();
                txtPrecio.Text = dgvDatos.CurrentRow.Cells["PrecioPublicoDVEN"].Value.ToString();
                cmbForma.SelectedValue = dgvDatos.CurrentRow.Cells["FormaPago"].Value;
                int valor = Convert.ToInt32(dgvDatos.CurrentRow.Cells["DevolucionDVEN"].Value);
                if (valor == 0) chkDev.Checked = false;
                else chkDev.Checked = true;
            }
            catch (NullReferenceException)
            {
                return;
            }
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            if (!editar)
            {
                DataRow row = tblVentasDetalle.NewRow();
                Random rand = new Random();
                int clave = rand.Next(1, 2000000000);
                row["IdDVEN"] = clave;
                row["IdVentaDVEN"] = lblNro.Text;
                int intPc = Convert.ToInt32(cmbLocal.SelectedValue.ToString());
                viewLocal.RowFilter = "IdPc = " + intPc;
                int intLocal = Convert.ToInt32(viewLocal[0][1].ToString());
                row["IdLocalDVEN"] = intLocal;
                row["IdArticuloDVEN"] = txtArticulo.Text;
                row["DescripcionDVEN"] = txtDescripcion.Text;
                row["CantidadDVEN"] = txtCantidad.Text;
                row["PrecioPublicoDVEN"] = txtPrecio.Text;
                row["PrecioCostoDVEN"] = txtCosto.Text;
                row["PrecioMayorDVEN"] = 0;
                row["IdFormaPagoDVEN"] = cmbForma.SelectedValue;
                int checkeado;
                if (chkDev.Checked) checkeado = 1;
                else checkeado = 0;
                row["DevolucionDVEN"] = checkeado;
                tblVentasDetalle.Rows.Add(row);
                txtArticulo.Clear();
                txtDescripcion.Clear();
                txtCantidad.Clear();
                txtPrecio.Clear();
                cmbForma.SelectedValue = -1;
                chkDev.Checked = false;
                txtArticulo.Focus();
                lblTotal.Text = "$" + CalcularTotal().ToString();
            }
            else
            {
                int idDVEN = Convert.ToInt32(dgvDatos.CurrentRow.Cells["IdDVEN"].Value.ToString());
                DataRow foundRow = tblVentasDetalle.Rows.Find(idDVEN);
                foundRow.BeginEdit();
                foundRow["IdArticuloDVEN"] = txtArticulo.Text;
                foundRow["DescripcionDVEN"] = txtDescripcion.Text;
                foundRow["CantidadDVEN"] = txtCantidad.Text;
                foundRow["PrecioPublicoDVEN"] = txtPrecio.Text;
                foundRow["PrecioCostoDVEN"] = txtCosto.Text;
                foundRow["PrecioMayorDVEN"] = 0;
                foundRow["IdFormaPagoDVEN"] = cmbForma.SelectedValue;
                int checkeado;
                if (chkDev.Checked) checkeado = 1;
                else checkeado = 0;
                foundRow["DevolucionDVEN"] = checkeado;
                foundRow.EndEdit();
                txtArticulo.Clear();
                txtDescripcion.Clear();
                txtCantidad.Clear();
                txtPrecio.Clear();
                cmbForma.SelectedValue = -1;
                chkDev.Checked = false;
                txtArticulo.Focus();
                lblTotal.Text = "$" + CalcularTotal().ToString();
                SetStateForm(FormState.insercion);
            }

        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.RowCount > 0)
            {
                txtArticulo.Text = dgvDatos.CurrentRow.Cells["IdArticuloDVEN"].Value.ToString();
                txtDescripcion.Text = dgvDatos.CurrentRow.Cells["DescripcionDVEN"].Value.ToString();
                txtCantidad.Text = dgvDatos.CurrentRow.Cells["CantidadDVEN"].Value.ToString();
                txtPrecio.Text = dgvDatos.CurrentRow.Cells["PrecioPublicoDVEN"].Value.ToString();
                txtCosto.Text = dgvDatos.CurrentRow.Cells["PrecioCostoDVEN"].Value.ToString();
                cmbForma.SelectedValue = dgvDatos.CurrentRow.Cells["FormaPago"].Value;
                int valor = Convert.ToInt32(dgvDatos.CurrentRow.Cells["DevolucionDVEN"].Value);
                if (valor == 0) chkDev.Checked = false;
                else chkDev.Checked = true;
                dgvDatos.CellEnter += new DataGridViewCellEventHandler(dgvDatos_CellEnter);
                SetStateForm(FormState.edicion);
            }
        }

        private void btnCancelEdit_Click(object sender, EventArgs e)
        {
            SetStateForm(FormState.insercion);
        }   

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            try
            {
                int idDVEN = Convert.ToInt32(dgvDatos.CurrentRow.Cells["IdDVEN"].Value.ToString());
                DataRow foundRow = tblVentasDetalle.Rows.Find(idDVEN);
                foundRow.Delete();
                lblTotal.Text = "$" + CalcularTotal().ToString();
            }
            catch (NullReferenceException)
            {
                return;
            }
        }

        private void btnArticulos_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            frmArticulos articulos = new frmArticulos(ref instancia, tblArticulos);
            articulos.Show(this);
            articulos.FormClosed += frmArticulos_FormClosed;
            Cursor.Current = Cursors.Arrow;
        }

        private void frmVentas_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false; // permite cerrar el form por mas que 'this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;'
            if (tblVentasDetalle.GetChanges() != null)
            {
                DialogResult respuesta = MessageBox.Show("¿Actualizar base de datos?", "Trend",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (respuesta)
                {
                    case DialogResult.Yes:
                        this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
                        if (string.IsNullOrEmpty(PK))
                        {
                            rowView.EndEdit();
                            frmProgress progreso = new frmProgress(dsVentas, "frmVentas", "grabar");
                            progreso.Show();
                        }
                        else
                        {
                            formClosing = true;
                            this.Tag = "ActualizarArqueo";
                        }
                        break;
                    case DialogResult.No:
                        this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
                        tblVentasDetalle.RejectChanges();
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }

        private void ValidarMaestro(object sender, EventArgs e)
        {
            try
            {
                if (cmbLocal.SelectedIndex == -1 || cmbCliente.SelectedIndex == -1)
                {
                    grpABM.Enabled = false;
                }
                else
                {
                    grpABM.Enabled = true;
                    SetStateForm(FormState.insercion);   
                }
            }
            catch (NullReferenceException)
            {
                dgvDatos.Enabled = false;
            }
        }

        public void cargarComboLocales()
        {
            tblLocales = BL.GetDataBLL.Locales();
            tblPcs = BL.GetDataBLL.Pc();
            var query =
                    from local in tblLocales.AsEnumerable()
                    from pc in tblPcs.AsEnumerable()
                    where (local.Field<Int32>("IdLocalLOC") == pc.Field<Int32>("IdLocalPC"))
                        && (pc.Field<string>("Detalle") == "Administracion")
                    select new
                    {
                        Local = local.Field<string>("NombreLOC"),
                        IdPc = pc.Field<Int32>("IdPC"),
                        IdLocal = local.Field<Int32>("IdLocalLOC")
                    };
            DataTable dtTemp = new DataTable();
            dtTemp.Columns.Add("IdPC", typeof(Int32));
            dtTemp.Columns.Add("IdLocalLOC", typeof(Int32));
            dtTemp.Columns.Add("NombreLOC", typeof(string));
            foreach (var registro in query)
            {
                DataRow fila = dtTemp.NewRow();
                fila["IdPC"] = registro.IdPc;
                fila["IdLocalLOC"] = registro.IdLocal;
                fila["NombreLOC"] = registro.Local;
                dtTemp.Rows.Add(fila);
            }
            viewLocal = new DataView(dtTemp);
            cmbLocal.ValueMember = "IdPC";
            cmbLocal.DisplayMember = "NombreLOC";
            cmbLocal.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLocal.DataSource = viewLocal;
            cmbLocal.SelectedIndex = -1;
        }

        private void Grabar()
        {
            rowView.EndEdit();
            frmProgress progreso = new frmProgress(dsVentas, "frmVentas", "grabar");
            progreso.Show();
        }

        private double CalcularTotal()
        {
            int cantidad;
            double precio;
            double total = 0;
            foreach (DataRowView fila in viewDetalle)
            {
                cantidad = Convert.ToInt32(fila["CantidadDVEN"].ToString());
                precio = Convert.ToDouble(fila["PrecioPublicoDVEN"].ToString());
                total = total + (cantidad * precio);
            }
            return total;
        }        

        private void frmArticulos_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!string.IsNullOrEmpty(idArticulo))
            {
                Clipboard.SetDataObject(idArticulo);
                txtArticulo.Focus();
                SendKeys.Send("^(v)");
                SendKeys.Send("{TAB}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
          //  Help.ShowHelp(button1, @"N:\NcSoftware\02_Access\HelpNc\NcPunto.chm");
            DataTable temp = tblVentasDetalle.Copy();
        }

        public void SetStateForm(FormState state)
        {
            if (state == FormState.inicial)
            {
                grpABM.Enabled = false;
                btnCancelEdit.Enabled = false;
            }
            if (state == FormState.insercion)
            {
                txtArticulo.Clear();
                txtDescripcion.Clear();
                txtCantidad.Clear();
                txtPrecio.Clear();
                cmbForma.SelectedValue = -1;
                chkDev.Checked = false;
                btnEditar.Enabled = true;
                btnCancelEdit.Enabled = false;
                btnBorrar.Enabled = true;
                editar = false;
                txtArticulo.Focus();
            }
            if (state == FormState.edicion)
            {
                btnEditar.Enabled = false;
                btnCancelEdit.Enabled = true;
                btnBorrar.Enabled = false;
                editar = true;
            }
        }

    }
}
