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
    public partial class frmVentas : Form
    {
        private frmVentas instanciaVentas;
        DataSet dsVentas;
        DataSet dsForaneos;
        DataTable tblVentas;
        DataTable tblVentasDetalle;
        DataTable tblDetalleOriginal;
        DataSet dsClientes;
        DataTable tblClientes; 
        DataView viewVentas;
        DataView viewDetalle;
        DataView viewDetalleOriginal;
        DataRowView rowView;
        DataTable tblLocales;
        DataTable tblPcs;
        DataTable tblArticulos;
        DataTable tblFormasPago;
        DataView viewLocal;
        DataGridViewComboBoxColumn cmbFormaPago;
        DataGridViewCheckBoxColumn chkDevolucion;
        public string PK = string.Empty;
        public int idPc;
        private int? codigoError = null;
        string ArticuloOld = string.Empty;
        bool editar = false;
        public string idArticulo;
        public int? idCliente = null;
        string articuloOld = string.Empty;

        public enum FormState
        {
            inicial,
            edicion,
            insercion,
            eliminacion
        }

        public frmVentas()
        {
            InitializeComponent();
            tblVentas = BL.VentasBLL.GetTabla();
            tblVentasDetalle = BL.VentasDetalleBLL.GetTabla();
        }

        public frmVentas(string PK, int idPc, DataTable tblVentas, DataTable tblVentasDetalle)
            : this()
        {
            this.PK = PK;
            this.idPc = idPc;
            this.tblVentas = tblVentas;
            tblVentas.TableName = "Ventas";
            this.tblVentasDetalle = tblVentasDetalle;
            tblVentasDetalle.TableName = "VentasDetalle";
        }

        private void frmVentas_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            System.Drawing.Icon ico = Properties.Resources.icono_app;
            this.Icon = ico;
            this.ControlBox = true;
            this.MaximizeBox = false;
            instanciaVentas = this;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ControlBox = true;
            this.MaximizeBox = false;
            this.CancelButton = btnSalir;
            btnSalir.Visible = false;
            dateTimePicker1.Visible = false;
            lblFecha.Text = DateTime.Today.ToLongDateString();
            lblTotal.ForeColor = System.Drawing.Color.DarkRed;
            ToolTip tooltip = new ToolTip();
            tooltip.SetToolTip(btnClientes, "Agregar nuevo cliente");
            dsForaneos = BL.VentasBLL.CrearDatasetForaneos();
            tblLocales = dsForaneos.Tables[3];
            tblPcs = dsForaneos.Tables[4];
            CargarComboLocales();            
            tblArticulos = dsForaneos.Tables[0];
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
            tblFormasPago = dsForaneos.Tables[2];
            tblFormasPago.TableName = "FormasPago";
            cmbForma.ValueMember = "IdFormaPagoFOR";
            cmbForma.DisplayMember = "DescripcionFOR";
            cmbForma.DropDownStyle = ComboBoxStyle.DropDown;
            cmbForma.DataSource = tblFormasPago;
            cmbForma.SelectedValue = -1;
            cmbForma.BackColor = Color.White;

            AutoCompleteStringCollection formasColection = new AutoCompleteStringCollection();
            foreach (DataRow row in tblFormasPago.Rows)
            {
                formasColection.Add(Convert.ToString(row["DescripcionFOR"]));
            }
            cmbForma.AutoCompleteCustomSource = formasColection;
            cmbForma.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbForma.AutoCompleteSource = AutoCompleteSource.CustomSource;


            lblCosto.Visible = false;
            txtCosto.Visible = false;
            btnEditar.CausesValidation = false;
            btnBorrar.CausesValidation = false;
            btnArticulos.CausesValidation = false; 
            lblNro.ForeColor = System.Drawing.Color.DarkRed;
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
            dgvDatos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDatos.DataSource = viewDetalle;
            dgvDatos.AllowUserToOrderColumns = false;
            dgvDatos.Columns["IdDVEN"].Visible = false;
            dgvDatos.Columns["IdVentaDVEN"].Visible = false;
            dgvDatos.Columns["IdLocalDVEN"].Visible = false;
            dgvDatos.Columns["PrecioCostoDVEN"].Visible = false;
            dgvDatos.Columns["PrecioMayorDVEN"].Visible = false;
            dgvDatos.Columns["IdFormaPagoDVEN"].Visible = false;
            dgvDatos.Columns["NroCuponDVEN"].Visible = false;
            dgvDatos.Columns["NroFacturaDVEN"].Visible = false;
            dgvDatos.Columns["IdEmpleadoDVEN"].Visible = false;
            dgvDatos.Columns["LiquidadoDVEN"].Visible = false;
            dgvDatos.Columns["EsperaDVEN"].Visible = false;
            dgvDatos.Columns["DescripcionDVEN"].HeaderText = "Descripción";
            dgvDatos.Columns["CantidadDVEN"].Width = 55;
            dgvDatos.Columns["CantidadDVEN"].HeaderText = "Cantidad";
            dgvDatos.Columns["IdArticuloDVEN"].HeaderText = "Artículo";
            dgvDatos.Columns["PrecioPublicoDVEN"].Width = 100;
            dgvDatos.Columns["PrecioPublicoDVEN"].HeaderText = "Precio";
            dgvDatos.Columns["NroFacturaDVEN"].Width = 100;
            dgvDatos.Columns["NroFacturaDVEN"].HeaderText = "Nº fact.";
            dgvDatos.Columns.Remove("DevolucionDVEN");            
            dgvDatos.Columns.Remove("IdFormaPagoDVEN");
            cmbFormaPago = new DataGridViewComboBoxColumn();
            cmbFormaPago.Name = "FormaPago";
            cmbFormaPago.HeaderText = "Forma de pago";
            cmbFormaPago.DataSource = tblFormasPago;
            cmbFormaPago.ValueMember = "IdFormaPagoFOR";
            cmbFormaPago.DisplayMember = "DescripcionFOR";
            cmbFormaPago.DataPropertyName = "IdFormaPagoDVEN";
            dgvDatos.Columns.Insert(7, cmbFormaPago);
            chkDevolucion = new DataGridViewCheckBoxColumn();
            chkDevolucion.Name = "DevolucionDVEN";
            chkDevolucion.Width = 40;
            chkDevolucion.HeaderText = "Dev.";
            chkDevolucion.DataPropertyName = "DevolucionDVEN";
            chkDevolucion.TrueValue = 1;
            chkDevolucion.FalseValue = 0;
            dgvDatos.Columns.Insert(12, chkDevolucion);
            if (PK == "") //registro nuevo
            {
                tblVentas.PrimaryKey = new DataColumn[] { tblVentas.Columns["IdVentaVEN"] };
                tblDetalleOriginal = tblVentasDetalle.Copy(); //tblDetalleOriginal se usa para controlar errores de guardado remoto por falta de internet
                Random rand = new Random();
                int clave = rand.Next(1, 2000000000);
                bool existe = true;
                while (existe == true)
                {
                    DataRow foundRow = dsVentas.Tables["Ventas"].Rows.Find(clave);
                    if (foundRow == null)
                    {
                        existe = false;
                    }
                    else
                    {
                        clave = rand.Next(1, 2000000000);
                    }
                }
                lblNro.Text = clave.ToString();
                viewVentas.RowStateFilter = DataViewRowState.Added;
                viewDetalle.RowStateFilter = DataViewRowState.Added;
                rowView = viewVentas.AddNew();
                rowView["IdVentaVEN"] = clave.ToString();
                rowView["FechaVEN"] = DateTime.Now;
                rowView["IdPCVEN"] = 1;
                rowView.EndEdit();
            }
            else // editar registros
            {
                tblDetalleOriginal = tblVentasDetalle.Copy(); //tblDetalleOriginal se usa para controlar errores de guardado remoto por falta de internet
                viewVentas.RowFilter = "IdVentaVEN = '" + PK + "'";
                rowView = viewVentas[0];
                viewDetalle.RowFilter = "IdVentaDVEN = '" + PK + "'";
                lblNro.Text = viewVentas[0][0].ToString();
                cmbLocal.Enabled = false;
                // viewDetalleOriginal  se usa para registrar en tabla fallidas errores de guardado remoto por falta de internet
                viewDetalleOriginal = new DataView(tblDetalleOriginal);
                viewDetalleOriginal.RowFilter = "IdVentaDVEN = '" + PK + "'";
            }
            dateTimePicker1.DataBindings.Add("Text", rowView, "FechaVEN", false, DataSourceUpdateMode.OnPropertyChanged);
            cmbLocal.DataBindings.Add("SelectedValue", rowView, "IdPCVEN", false, DataSourceUpdateMode.OnPropertyChanged);
            cmbCliente.DataBindings.Add("SelectedValue", rowView, "IdClienteVEN", false, DataSourceUpdateMode.OnPropertyChanged);
            rowView.CancelEdit();
            lblTotal.Text = "$" + CalcularTotal().ToString();
            cmbCliente.Validating += new System.ComponentModel.CancelEventHandler(BL.Utilitarios.ValidarComboBox);
            txtPrecio.KeyPress += new KeyPressEventHandler(BL.Utilitarios.SoloNumerosConComa);
            txtCantidad.KeyPress += new KeyPressEventHandler(BL.Utilitarios.SoloNumeros);
            txtArticulo.Enter += new System.EventHandler(Utilitarios.SelTextoTextBox);
            txtCantidad.Enter += new System.EventHandler(Utilitarios.SelTextoTextBox);
            txtPrecio.Enter += new System.EventHandler(Utilitarios.SelTextoTextBox);
            txtArticulo.KeyDown += new System.Windows.Forms.KeyEventHandler(Utilitarios.EnterTab);
            txtCantidad.KeyDown += new System.Windows.Forms.KeyEventHandler(Utilitarios.EnterTab);
            txtPrecio.KeyDown += new System.Windows.Forms.KeyEventHandler(Utilitarios.EnterTab);
            cmbForma.KeyDown += new System.Windows.Forms.KeyEventHandler(Utilitarios.EnterTab);
            cmbForma.Validating += new System.ComponentModel.CancelEventHandler(BL.Utilitarios.ValidarComboBox);
            chkDev.KeyDown += new System.Windows.Forms.KeyEventHandler(Utilitarios.EnterTab);
        //    tblVentasDetalle.ColumnChanged += new DataColumnChangeEventHandler(HabilitarGrabar);
            SetStateForm(FormState.insercion);
            DialogResult respuesta = MessageBox.Show("SOLICITAR DATOS CLIENTE", "Trend Gestión", MessageBoxButtons.OK, MessageBoxIcon.Question);
            if (respuesta == DialogResult.OK)
            {
                frmClientes newMDIChild = new frmClientes();
                newMDIChild.ShowDialog();
            }
        }

        private void frmVentas_Activated(object sender, EventArgs e)
        {
            CargarComboClientes();
            if (PK != "")
            {
                foreach (DataGridViewRow fila in dgvDatos.Rows)
                {
                    if (!fila.IsNewRow)
                    {
                        string articulo = fila.Cells[3].Value.ToString();
                        DataRow[] foundRow = tblArticulos.Select("IdArticuloART = '" + articulo + "'");
                        fila.Cells[4].Value = foundRow[0]["DescripcionART"].ToString();
                    }
                }
            }
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            frmClientes clientes = new frmClientes(ref instanciaVentas);
            clientes.FormClosed += frmClientes_FormClosed;
            clientes.ShowDialog();
        }

        private void txtArticulo_Enter(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtArticulo.Text)) articuloOld = string.Empty;
        }

        private void txtArticulo_Validating(object sender, CancelEventArgs e)
        {
           if (string.IsNullOrEmpty(txtArticulo.Text)) return;
            if (articuloOld == txtArticulo.Text) return;
            DataRow[] foundRow = tblArticulos.Select("IdArticuloART = '" + txtArticulo.Text + "'");
            if (foundRow.Length == 0)
            {
                e.Cancel = true;
            }
            else
            {
                DataRow filaActual = foundRow[0];
                txtDescripcion.Text = filaActual["DescripcionART"].ToString();
                if (txtArticulo.Text == "0000000004" || txtArticulo.Text == "0000000006") // seña y nota de credito entra factura
                {
                    txtCantidad.Text = "-1";
                }
                else
                {
                    txtCantidad.Text = "1";
                }
                txtPrecio.Text = filaActual["PrecioMayorART"].ToString();
                cmbForma.SelectedValue = "1";
                txtCosto.Text = filaActual["PrecioCostoART"].ToString();
                articuloOld = txtArticulo.Text;
            }            
        }

        private void txtCantidad_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCantidad.Text) || txtCantidad.Text == "0")
            {
                e.Cancel = true;
            }
            if (txtArticulo.Text == "0000000004" || txtArticulo.Text == "0000000006") // seña y nota de credito entra factura
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
            if(!ValidadRegistro()) return;
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
              //  SetStateForm(FormState.insercion);
            }
            dgvDatos.CellEnter -= new DataGridViewCellEventHandler(dgvDatos_CellEnter);
            SetStateForm(FormState.insercion);
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
                articuloOld = txtArticulo.Text;
                dgvDatos.CellEnter += new DataGridViewCellEventHandler(dgvDatos_CellEnter);
                SetStateForm(FormState.edicion);
            }
        }

        private void btnCancelEdit_Click(object sender, EventArgs e)
        {
            dgvDatos.CellEnter -= new DataGridViewCellEventHandler(dgvDatos_CellEnter);
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
            frmArticulos articulos = new frmArticulos(ref instanciaVentas, tblArticulos);
            articulos.Show(this);
            articulos.FormClosed += frmArticulos_FormClosed;
            Cursor.Current = Cursors.Arrow;
        }

        private void frmVentas_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false; // permite cerrar el form por mas que 'this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;'
            Cursor.Current = Cursors.WaitCursor;
            if (tblVentasDetalle.GetChanges() != null)
            {
                DataTable tblActual = new DataTable();
                tblActual.Columns.Add("Id", typeof(int));
                tblActual.Columns.Add("Accion", typeof(string));

                //agrego el RowState de las filas por si falla la insercion / modificacion / borrado en el servidor remoto poder
                //guardar dicha informacion en la tabla local 'VentasDetalleFallidas'
                foreach (DataRowView row in viewDetalle)
                {
                    DataRow rowActual = tblActual.NewRow();
                    rowActual["Id"] = row["IdDVEN"];
                    rowActual["Accion"] = row.Row.RowState.ToString();
                    tblActual.Rows.Add(rowActual);
                }
                rowView.EndEdit();
                bool grabarFallidas = false;
                if (PK == "") //registro nuevo
                {
                    BL.TransaccionesBLL.GrabarVentas(dsVentas, ref codigoError, grabarFallidas);
                }
                else
                {
                    BL.TransaccionesBLL.GrabarVentas(dsVentas, ref codigoError, viewDetalleOriginal, tblActual, grabarFallidas);
                    this.DialogResult = DialogResult.OK;
                }
            }
            else
            {
                tblVentas.RejectChanges();
            }
            Cursor.Current = Cursors.Arrow;
        }

        public void CargarComboLocales()
        {
            if (PK == "") //registro nuevo
            {
                var query =
                        from local in tblLocales.AsEnumerable()
                        from pc in tblPcs.AsEnumerable()
                        where (local.Field<Int32>("IdLocalLOC") == pc.Field<Int32>("IdLocalPC"))
                            && (pc.Field<string>("Detalle") == "Caja1")
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
                viewLocal.RowFilter = "IdPC = 1";
                cmbLocal.ValueMember = "IdPC";
                cmbLocal.DisplayMember = "NombreLOC";
                cmbLocal.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbLocal.DataSource = viewLocal;
                cmbLocal.SelectedIndex = -1;
            }
            else
            {
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
        }

        public void CargarComboClientes()
        {
            dsClientes = BL.ClientesBLL.GetClientes(0);
            tblClientes = dsClientes.Tables[0];
            tblClientes.DefaultView.Sort = "NombreCompletoCLI";
            cmbCliente.ValueMember = "IdClienteCLI";
            cmbCliente.DisplayMember = "NombreCompletoCLI";
            cmbCliente.DropDownStyle = ComboBoxStyle.DropDown;
            cmbCliente.DataSource = tblClientes;
            if (idCliente == null) cmbCliente.SelectedValue = 1;
            else cmbCliente.SelectedValue = idCliente;

            AutoCompleteStringCollection clientesColection = new AutoCompleteStringCollection();
            foreach (DataRow row in tblClientes.Rows)
            {
                clientesColection.Add(Convert.ToString(row["RazonSocialCLI"]));
            }
            cmbCliente.AutoCompleteCustomSource = clientesColection;
            cmbCliente.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbCliente.AutoCompleteSource = AutoCompleteSource.CustomSource;
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

        private void frmClientes_FormClosed(object sender, FormClosedEventArgs e)
        {
            CargarComboClientes();
            txtArticulo.Focus();
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

        private bool ValidadRegistro()
        {
            bool validar = true;
            foreach (Control ctl in grpABM.Controls)
            {
                string tipo = ctl.GetType().ToString();
                if (tipo == "System.Windows.Forms.TextBox")
                { 
                    if(string.IsNullOrEmpty(ctl.Text))
                    {
                        validar = false;
                        break;
                    }
                }

            }
            return validar;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
