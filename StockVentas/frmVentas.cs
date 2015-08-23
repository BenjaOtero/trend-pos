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
            btnImprimir.CausesValidation = false;
            btnImprimir.Enabled = false;   
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
                btnImprimir.Enabled = true;
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
                if (txtArticulo.Text == "000000004" || txtArticulo.Text == "000000006") // seña y nota de credito entra factura
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
                btnImprimir.Enabled = true;
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
                if (dgvDatos.RowCount == 0) btnImprimir.Enabled = false;
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

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            string path = Application.StartupPath;
            if (!File.Exists(path + @"\licencia.lcn"))
            {
                ImprimirFactura();
            }
            else
            { 
                //mensaje con link para solicitar el servicio
            }
            if (dgvDatos.RowCount > 0)
            {
                int x = dgvDatos.RowCount;
            }
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
            dsClientes = BL.ClientesBLL.GetClientes();
            tblClientes = dsClientes.Tables[0];            
            tblClientes.DefaultView.Sort = "RazonSocialCLI";
            cmbCliente.ValueMember = "IdClienteCLI";
            cmbCliente.DisplayMember = "RazonSocialCLI";
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

        private void ImprimirFactura()
        {
            DataTable tblIVA = new DataTable();
            tblIVA.Columns.Add("Articulo", typeof(string));
            tblIVA.Columns.Add("Descripcion", typeof(string));
            tblIVA.Columns.Add("Cantidad", typeof(int));
            tblIVA.Columns.Add("IdAlicuota", typeof(int));
            tblIVA.Columns.Add("PorcentajeIva", typeof(decimal));
            tblIVA.Columns.Add("Precio", typeof(decimal));
            tblIVA.Columns.Add("SubtotalSinIva", typeof(decimal));
            tblIVA.Columns.Add("SubtotalIva", typeof(decimal));
            foreach(DataGridViewRow rowGrid in dgvDatos.Rows)
            {
                string articulo = rowGrid.Cells["IdArticuloDVEN"].Value.ToString();
                DataRow[] foundRow = tblArticulos.Select("IdArticuloART = '" + articulo + "'");
                string descripcion = foundRow[0]["DescripcionART"].ToString();
                int cantidad = Convert.ToInt32(rowGrid.Cells["CantidadDVEN"].Value.ToString());
                int idAlicuota = Convert.ToInt16(foundRow[0]["IdAliculotaIvaART"].ToString());
                decimal porcentajeIva = Convert.ToDecimal(foundRow[0]["PorcentajeALI"].ToString()) / 100 + 1;
                decimal precio = decimal.Round(Convert.ToDecimal(foundRow[0]["PrecioPublicoART"].ToString()) / porcentajeIva, 2);
                decimal ivaImporte = decimal.Round(Convert.ToDecimal(foundRow[0]["PrecioPublicoART"].ToString()) - precio, 2);
                DataRow fila = tblIVA.NewRow();
                fila["Articulo"] = articulo;
                fila["Descripcion"] = descripcion;
                fila["Cantidad"] = cantidad;
                fila["IdAlicuota"] = idAlicuota;
                fila["PorcentajeIva"] = porcentajeIva;
                fila["Precio"] = precio;
                fila["SubtotalSinIva"] = cantidad * precio;
                fila["SubtotalIva"] = cantidad * ivaImporte;
                tblIVA.Rows.Add(fila);
                
            }

            string idCliente = cmbCliente.SelectedValue.ToString();
            DataRow[] foundCliente = tblClientes.Select("IdClienteCLI = '" + idCliente + "'");
            string condicionIva = foundCliente[0]["CondicionIvaCLI"].ToString();
            int cabeceraCbteTipo;
            int detalleDocTipo;
            string detalleDocNro;
            if (condicionIva == "RESPONSABLE INSCRIPTO")
            {
                cabeceraCbteTipo = 1;
                detalleDocTipo = 80;
                detalleDocNro = foundCliente[0]["CUIT"].ToString();
            }
            else
            {
                cabeceraCbteTipo = 6;
                detalleDocTipo = 99;
                detalleDocNro = "0";
            }         

            // Ver WSFEv1 Fallos conexión en Camuzzo
            WSAFIPFE.Factura fe = new WSAFIPFE.Factura();
            Boolean bResultado = false;
            bResultado = fe.iniciar(WSAFIPFE.Factura.modoFiscal.Test, "27220379588", @"C:\Trend\Factura-electronica\Carolina Navarro\pedido.pfx", @" ");
            if (bResultado)
            {
                fe.ArchivoCertificadoPassword = "";
                bResultado = fe.f1ObtenerTicketAcceso();
                if (bResultado)
                {
                    fe.F1CabeceraCantReg = 1;
                    fe.F1CabeceraPtoVta = 1;

                    /*Según el manual del desarrollador (pagina 15), el error 10007 se da por que no informas alguno de los 
                     * tipos validos son 01 02 03 04 05 34 39 60 63 para comprobantes A y 06 07 08 09 10 35 40 64 y 61 para los B.*/
                    fe.F1CabeceraCbteTipo = cabeceraCbteTipo;
                    int nroComp = fe.F1CompUltimoAutorizado(1, cabeceraCbteTipo) + 1;
                    fe.f1Indice = 0;
                    fe.F1DetalleConcepto = 1;  //Concepto del comprobante.  01-Productos, 02-Servicios, 03-Productos y Servicios
                    fe.F1DetalleDocTipo = detalleDocTipo;    // 96: DNI, 80: CUIT, 99: Consumidor Final
                    fe.F1DetalleDocNro = detalleDocNro;
                 //   fe.F1DetalleDocNro = "30570135585";
                    fe.F1DetalleCbteDesde = nroComp;
                    fe.F1DetalleCbteHasta = nroComp; // Número de comprobante hasta. En caso de ser un solo comprobante, este dato coincide con el anterior.

                    /* F1DetalleCbteFch: Fecha del comprobante, cuyo formato es "aaaammdd". Para un concepto de factura igual a 1, 
                        la fecha de emisión puede ser hasta 5 días posteriores a la de generación.  
                        Si el concepto es 2 o 3, puede ser hasta 10 días anteriores o posteriores a la fecha de generación. 
                        Al ser un dato opcional, si no se asigna fecha, por defecto se asignará la fecha del proceso.*/
                    string fecha = DateTime.Now.ToString("yyyyMMdd");
                    fe.F1DetalleCbteFch = fecha;

                  /*  fe.F1DetalleTributoItemCantidad = 1;  //preguntar Ariel Cantidad de Tributos relacionados al comprobante
                    fe.f1IndiceItem = 0;
                    fe.F1DetalleTributoId = 3;
                    fe.F1DetalleTributoDesc = "Impuesto Municipal Matanza";
                    fe.F1DetalleTributoBaseImp = 0;
                    fe.F1DetalleTributoAlic = 5.2;
                    fe.F1DetalleTributoImporte = 0;*/
                    var groupedData = from b in tblIVA.AsEnumerable()
                                      group b by b.Field<int>("IdAlicuota") into g
                                      select new
                                      {
                                          DetalleIvaId = g.Key,
                                          Count = g.Count(),
                                          DetalleIvaBaseImp = g.Sum(x => x.Field<decimal>("SubtotalSinIva")),
                                          DetalleIvaImporte = g.Sum(x => x.Field<decimal>("SubtotalIva"))
                                      };
                    int DetalleIvaItemCantidad = groupedData.Count();
                    fe.F1DetalleIvaItemCantidad = DetalleIvaItemCantidad;
                    int indiceItem = 0;
                    foreach (var registro in groupedData)
                    {
                        fe.f1IndiceItem = indiceItem;
                        //En F1DetalleIvaId va el código de la alícuota o tasa (obtenido de una lista de AFIP: 5 para 21% 4 para 10.50%, etc).
                        fe.F1DetalleIvaId = registro.DetalleIvaId;
                        fe.F1DetalleIvaBaseImp = Convert.ToDouble(registro.DetalleIvaBaseImp);  //El precio del producto
                        fe.F1DetalleIvaImporte = Convert.ToDouble(registro.DetalleIvaImporte);  //El importe del impuesto.
                        indiceItem++;
                    }

                    
               /*     fe.f1IndiceItem = 0;
                    fe.F1DetalleIvaId = 5;  //El código de la alícuota o tasa (obtenido de una lista de AFIP: 5 para 21% 4 para 10.50%, etc).
                    fe.F1DetalleIvaBaseImp = 100;  //El precio del producto
                    fe.F1DetalleIvaImporte = 21;  //El importe del impuesto.

                    fe.f1IndiceItem = 1;
                    fe.F1DetalleIvaId = 4;
                    fe.F1DetalleIvaBaseImp = 50;
                    fe.F1DetalleIvaImporte = 5.25;*/


                    var detalleImpNeto = tblIVA.AsEnumerable().Sum(x => x.Field<decimal>("SubtotalSinIva"));
                    var detalleImpIva = tblIVA.AsEnumerable().Sum(x => x.Field<decimal>("SubtotalIva"));
                    double detalleImpTotal = Convert.ToDouble(detalleImpNeto + detalleImpIva);

                    fe.F1DetalleImpTotal = detalleImpTotal; // total factura
                    fe.F1DetalleImpTotalConc = 0; // preguntar Ariel  Importe Neto No Grabado, debe ser mayor a cero y menor o igual al importe total (F1DetalleImpTotal).
                    fe.F1DetalleImpNeto = Convert.ToDouble(detalleImpNeto); // total bases imponibles
                    fe.F1DetalleImpOpEx = 0; // preguntar Ariel
                    fe.F1DetalleImpTrib = 0; // preguntar Ariel
                    fe.F1DetalleImpIva = Convert.ToDouble(detalleImpIva); ;  // total ivas
                    fe.F1DetalleFchServDesde = ""; // se debe poner fecha para servicios o para productos y servicios. Para productos solos puede ser vacío
                    fe.F1DetalleFchServHasta = ""; // Idem anterior completar si F1DetalleConcepto > 1
                    fe.F1DetalleFchVtoPago = ""; // Idem anterior completar si F1DetalleConcepto > 1
                    fe.F1DetalleMonId = "PES";
                    fe.F1DetalleMonCotiz = 1;

                    fe.F1DetalleCbtesAsocItemCantidad = 0;
                    fe.F1DetalleOpcionalItemCantidad = 0;

                    fe.ArchivoXMLRecibido = @"c:\recibido.xml";
                    fe.ArchivoXMLEnviado = @"c:\enviado.xml";
                //    bResultado = fe.F1CAESolicitar();
                    if (bResultado)
                    {
                        MessageBox.Show("resultado verdadero ");
                    }
                    else
                    {
                        MessageBox.Show("resultado falso ");
                    }
                    MessageBox.Show("resultado global AFIP: " + fe.F1RespuestaResultado);
                    MessageBox.Show("es reproceso? " + fe.F1RespuestaReProceso);
                    MessageBox.Show("registros procesados por AFIP: " + fe.F1RespuestaCantidadReg.ToString());
                    MessageBox.Show("error genérico global:" + fe.f1ErrorMsg1);
                    if (fe.F1RespuestaCantidadReg > 0)
                    {
                        fe.f1Indice = 0;
                        MessageBox.Show("resultado detallado comprobante: " + fe.F1RespuestaDetalleResultado);
                        MessageBox.Show("cae comprobante: " + fe.F1RespuestaDetalleCae);
                        MessageBox.Show("número comprobante:" + fe.F1RespuestaDetalleCbteDesdeS);
                        MessageBox.Show("error detallado comprobante: " + fe.F1RespuestaDetalleObservacionMsg1);
                    }
                    rptFactura informeFactura = new rptFactura(tblIVA, foundCliente);
                    informeFactura.Show();
                }
                else
                {
                    MessageBox.Show("fallo acceso " + fe.UltimoMensajeError);
                }
            }
            else
            {
                MessageBox.Show("error inicio " + fe.UltimoMensajeError);
            }        
        }

    }
}
