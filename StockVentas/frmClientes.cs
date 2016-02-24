using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BL;
using System.Text.RegularExpressions;

namespace StockVentas
{
    public partial class frmClientes : Form
    {
        frmVentas instanciaVentas = null;
        DataSet dsClientes;
        private DataTable tblClientes;
        DataTable tblFallidas;
        private int? codigoError = null;

        public enum FormState
        {
            inicial,
            edicion,
            insercion,
            eliminacion
        }

        public frmClientes()
        {
          InitializeComponent();
        }

        public frmClientes(ref frmVentas instanciaVentas)
        {
            InitializeComponent();
            this.instanciaVentas = instanciaVentas;
        }

        private void frmClientes_Load(object sender, EventArgs e)
        {
            System.Drawing.Icon ico = Properties.Resources.icono_app;
            this.Icon = ico;
            this.ControlBox = true;
            this.MaximizeBox = false;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            dsClientes = BL.ClientesBLL.GetClientes(1);
            tblClientes = dsClientes.Tables[0];
            BL.Utilitarios.AddEventosABM(grpCampos, ref btnGrabar, ref tblClientes);
            tblFallidas = new DataTable();
            tblFallidas.TableName = "ClientesFallidas";
            tblFallidas.Columns.Add("Id", typeof(int));
            tblFallidas.Columns.Add("Accion", typeof(string));
            tblFallidas.Columns["Id"].Unique = true;
            tblFallidas.PrimaryKey = new DataColumn[] { tblFallidas.Columns["Id"] };
            DataView viewClientes = new DataView(tblClientes);
            bindingSource1.DataSource = viewClientes;
            bindingNavigator1.BindingSource = bindingSource1;
            BL.Utilitarios.DataBindingsAdd(bindingSource1, grpCampos);
            grpBotones.CausesValidation = false;
            btnCancelar.CausesValidation = false;
            btnSalir.CausesValidation = false;
            Dictionary<Int32, String> condiciones = new Dictionary<int, string>();
            condiciones.Add(1, "Consumidor Final");
            condiciones.Add(2, "Responsable Inscripto");
            condiciones.Add(3, "Responsable Monotributo");
            cmbCondicion.DataSource = new BindingSource(condiciones, null);
            cmbCondicion.DisplayMember = "Value";
            cmbCondicion.ValueMember = "Value";
            cmbCondicion.DataBindings.Add("SelectedValue", bindingSource1, "CondicionIvaCLI", false, DataSourceUpdateMode.OnPropertyChanged);
            gvwDatos.DataSource = bindingSource1;
            gvwDatos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gvwDatos.Columns["IdClienteCLI"].HeaderText = "Nº cliente";
            gvwDatos.Columns["NombreCLI"].HeaderText = "Nombre";
            gvwDatos.Columns["ApellidoCLI"].HeaderText = "Apellido";
            gvwDatos.Columns["CorreoCLI"].HeaderText = "Correo";
            gvwDatos.Columns["RazonSocialCLI"].Visible = false;
            gvwDatos.Columns["CUIT"].Visible = false;
            gvwDatos.Columns["DireccionCLI"].Visible = false;
            gvwDatos.Columns["LocalidadCLI"].Visible = false;
            gvwDatos.Columns["ProvinciaCLI"].Visible = false;
            gvwDatos.Columns["TransporteCLI"].Visible = false;
            gvwDatos.Columns["ContactoCLI"].Visible = false;
            gvwDatos.Columns["TelefonoCLI"].Visible = false;
            gvwDatos.Columns["MovilCLI"].Visible = false;
            gvwDatos.Columns["FechaNacCLI"].Visible = false;
            gvwDatos.Columns["CondicionIvaCLI"].Visible = false;
            gvwDatos.Columns["NombreCompletoCLI"].Visible = false;
            bindingSource1.Sort = "RazonSocialCLI";
            int itemFound = bindingSource1.Find("RazonSocialCLI", "PUBLICO");
            bindingSource1.Position = itemFound;
            tblClientes.RowChanging += new DataRowChangeEventHandler(Row_Changing);
            tblClientes.RowDeleting += new DataRowChangeEventHandler(Row_Deleting);
            SetStateForm(FormState.inicial);  
        }        

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string parametros = txtParametros.Text;
            bindingSource1.Filter = "NombreCLI LIKE '" + parametros + "*' OR ApellidoCLI LIKE '" + parametros + "*'";
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            bindingSource1.AddNew();
            Random rand = new Random();
            int clave = rand.Next(1, 2000000000);
            bindingSource1.Position = bindingSource1.Count - 1;
            txtIdClienteCLI.ReadOnly = false;
            txtIdClienteCLI.Text = clave.ToString();
            txtIdClienteCLI.ReadOnly = true;
            txtRazonSocialCLI.Focus();
            SetStateForm(FormState.insercion);  
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (txtRazonSocialCLI.Text == "PUBLICO")
            {
                MessageBox.Show("No se puede modificar el registro porque es un registro propio del sistema.", "Trend Gestión",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            } 
            SetStateForm(FormState.edicion);
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            if (txtRazonSocialCLI.Text == "PUBLICO")
            {
                MessageBox.Show("No se puede borrar el registro porque es un registro propio del sistema.", "Trend Gestión",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            } 
            if (MessageBox.Show("¿Desea borrar este registro?", "Buscar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                bindingSource1.RemoveCurrent();
                bindingSource1.EndEdit();
            }
            SetStateForm(FormState.inicial); 
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarFormulario())
                {
                    bindingSource1.EndEdit();
                    bindingSource1.Position = 0;
                    bindingSource1.Sort = "ApellidoCLI";
                    SetStateForm(FormState.inicial);
                    bindingSource1.RemoveFilter();
                }
            }
            catch (ConstraintException)
            {
                string mensaje = "No se puede agregar el cliente '" + txtNombreCLI.Text.ToUpper() + "' porque ya existe un cliente con el mismo correo electrónico.";
                MessageBox.Show(mensaje, "Trend", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCorreoCLI.Focus();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            bindingSource1.CancelEdit();
            SetStateForm(FormState.inicial);
            errorProvider1.Clear();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {          
            Close();
        }

        private void frmClientes_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ValidarFormulario())
            {
                tblClientes.RowChanging -= new DataRowChangeEventHandler(Row_Changing);
                bindingSource1.EndEdit();
                if (tblClientes.GetChanges() != null)
                {
                    BL.ClientesBLL.GrabarDB(dsClientes, tblFallidas, ref codigoError, false);
                }
                bindingSource1.RemoveFilter();
                if (instanciaVentas != null) instanciaVentas.idCliente = Convert.ToInt32(txtIdClienteCLI.Text);
            }
            else e.Cancel = true;
        }

        private void Row_Deleting(object sender, DataRowChangeEventArgs e)
        {
            int clave = Convert.ToInt32(e.Row["IdClienteCLI"].ToString());
            DataRow foundRow = tblFallidas.Rows.Find(clave);
            if (foundRow != null)
            {
                string accionNueva = e.Action.ToString();
                string accionAnterior = foundRow["Accion"].ToString();
                switch (accionAnterior)
                {
                    case "Add":
                        foundRow.Delete();
                        break;
                    case "Change":
                        foundRow["Accion"] = "Delete";
                        break;
                }
            }
            else
            {
                DataRow row = tblFallidas.NewRow();
                row["Id"] = Convert.ToInt32(e.Row["IdClienteCLI"].ToString());
                row["Accion"] = e.Action.ToString(); ;
                tblFallidas.Rows.Add(row);
            }
        }

        private void Row_Changing(object sender, DataRowChangeEventArgs e)
        {
            int clave = Convert.ToInt32(e.Row["IdClienteCLI"].ToString());
            DataRow foundRow = tblFallidas.Rows.Find(clave);
            if (foundRow != null)
            {
                string accionNueva = e.Action.ToString();
                string accionAnterior = foundRow["Accion"].ToString();
                if (accionAnterior == "delete" && accionNueva == "add") //coincide el id de registro borrado con el nuevo añadido
                {
                    foundRow["Accion"] = "change";
                }
            }
            else
            {
                DataRow row = tblFallidas.NewRow();
                row["Id"] = Convert.ToInt32(e.Row["IdClienteCLI"].ToString());
                row["Accion"] = e.Action.ToString(); ;
                tblFallidas.Rows.Add(row);
            }
        }

        private void grpCampos_Enter(object sender, EventArgs e)
        {

        }

        private bool ValidarFormulario()
        {
            if (string.IsNullOrEmpty(txtNombreCLI.Text))
            {
                this.errorProvider1.SetError(txtNombreCLI, "Debe escribir un nombre.");
                txtNombreCLI.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtApellidoCLI.Text))
            {
                this.errorProvider1.SetError(txtApellidoCLI, "Debe escribir un apellido.");
                txtApellidoCLI.Focus();
                return false;
            }
            if (!IsValidEmail(txtCorreoCLI.Text))
            {
                this.errorProvider1.SetError(txtCorreoCLI, "Verifique la dirección de correo electrónico.");
                return false;
            }
            return true;
        }

        private void ValidarCampos(object sender, CancelEventArgs e)
        {
            if ((sender == (object)txtNombreCLI))
            {
                if (string.IsNullOrEmpty(txtNombreCLI.Text))
                {
                    this.errorProvider1.SetError(txtNombreCLI, "Debe escribir un nombre.");
                    e.Cancel = true;
                }
            }
            if ((sender == (object)txtApellidoCLI))
            {
                if (string.IsNullOrEmpty(txtApellidoCLI.Text))
                {
                    this.errorProvider1.SetError(txtApellidoCLI, "Debe escribir un apellido.");
                    e.Cancel = true;
                }
            }
            if ((sender == (object)txtCorreoCLI))
            {
                if (!IsValidEmail(txtCorreoCLI.Text))
                {
                    this.errorProvider1.SetError(txtCorreoCLI, "Verifique la dirección de correo electrónico.");
                    e.Cancel = true;
                }
            }
        }

        private void CamposValidado(object sender, EventArgs e)
        {
            errorProvider1.Clear();
        }

        private void AddEventosValidacion()
        {
            foreach (Control ctl in grpCampos.Controls)
            {
                if (ctl is TextBox || ctl is MaskedTextBox || ctl is ComboBox)
                {
                    ctl.Validating += new System.ComponentModel.CancelEventHandler(this.ValidarCampos);
                    ctl.Validated += new System.EventHandler(this.CamposValidado);
                }
            }
        }

        private void DelEventosValidacion()
        {
            foreach (Control ctl in grpCampos.Controls)
            {
                if (ctl is TextBox || ctl is MaskedTextBox || ctl is ComboBox)
                {
                    ctl.Validating -= new System.ComponentModel.CancelEventHandler(this.ValidarCampos);
                    ctl.Validated -= new System.EventHandler(this.CamposValidado);
                }
            }
            this.errorProvider1.Clear();
        }

        public static bool IsValidEmail(string email)
        {
            string expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, string.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void SetStateForm(FormState state)
        {
            if (state == FormState.inicial)
            {
                gvwDatos.Enabled = true;
                txtIdClienteCLI.ReadOnly = true;
                txtIdClienteCLI.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
                txtRazonSocialCLI.ReadOnly = true;
                txtRazonSocialCLI.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
                txtNombreCLI.ReadOnly = true;
                txtNombreCLI.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
                txtApellidoCLI.ReadOnly = true;
                txtApellidoCLI.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
                txtCUIT.ReadOnly = true;
                txtCUIT.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
                txtDireccionCLI.ReadOnly = true;
                txtDireccionCLI.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
                txtLocalidadCLI.ReadOnly = true;
                txtLocalidadCLI.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
                txtProvinciaCLI.ReadOnly = true;
                txtProvinciaCLI.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
                txtTransporteCLI.ReadOnly = true;
                txtTransporteCLI.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
                txtContactoCLI.ReadOnly = true;
                txtContactoCLI.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
                txtTelefonoCLI.ReadOnly = true;
                txtTelefonoCLI.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
                txtMovilCLI.ReadOnly = true;
                txtMovilCLI.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
                txtCorreoCLI.ReadOnly = true;
                txtCorreoCLI.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
                txtFechaNacCLI.ReadOnly = true;
                txtFechaNacCLI.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
                btnBuscar.Enabled = true;
                btnNuevo.Enabled = true;
                btnEditar.Enabled = true;
                btnBorrar.Enabled = true;
                btnGrabar.Enabled = false;
                btnCancelar.Enabled = false;
                btnSalir.Enabled = true;
                DelEventosValidacion();
            }
            if (state == FormState.insercion)
            {
                gvwDatos.Enabled = false;
                txtRazonSocialCLI.ReadOnly = false;
                txtNombreCLI.ReadOnly = false;
                txtApellidoCLI.ReadOnly = false;
                txtCUIT.ReadOnly = false;
                txtDireccionCLI.ReadOnly = false;
                txtLocalidadCLI.ReadOnly = false;
                txtProvinciaCLI.ReadOnly = false;
                txtTransporteCLI.ReadOnly = false;
                txtContactoCLI.ReadOnly = false;
                txtTelefonoCLI.ReadOnly = false;
                txtMovilCLI.ReadOnly = false;
                txtCorreoCLI.ReadOnly = false;
                txtFechaNacCLI.ReadOnly = false;
                txtIdClienteCLI.Clear();
                txtRazonSocialCLI.Clear();
                txtCUIT.Clear();
                txtDireccionCLI.Clear();
                txtLocalidadCLI.Clear();
                txtProvinciaCLI.Clear();
                txtTransporteCLI.Clear();
                txtContactoCLI.Clear();
                txtTelefonoCLI.Clear();
                txtMovilCLI.Clear();
                txtCorreoCLI.Clear();
                txtFechaNacCLI.Clear();
                btnBuscar.Enabled = false;
                btnNuevo.Enabled = false;
                btnEditar.Enabled = false;
                btnBorrar.Enabled = false;
                btnGrabar.Enabled = false;
                btnCancelar.Enabled = true;
                btnSalir.Enabled = false;
                txtNombreCLI.Focus();
                AddEventosValidacion();
            }
            if (state == FormState.edicion)
            {
                gvwDatos.Enabled = false;
                txtRazonSocialCLI.ReadOnly = false;
                txtNombreCLI.ReadOnly = false;
                txtApellidoCLI.ReadOnly = false;
                txtCUIT.ReadOnly = false;
                txtDireccionCLI.ReadOnly = false;
                txtLocalidadCLI.ReadOnly = false;
                txtProvinciaCLI.ReadOnly = false;
                txtTransporteCLI.ReadOnly = false;
                txtContactoCLI.ReadOnly = false;
                txtTelefonoCLI.ReadOnly = false;
                txtMovilCLI.ReadOnly = false;
                txtCorreoCLI.ReadOnly = false;
                txtFechaNacCLI.ReadOnly = false;
                btnBuscar.Enabled = false;
                btnNuevo.Enabled = false;
                btnEditar.Enabled = false;
                btnBorrar.Enabled = false;
                btnGrabar.Enabled = false;
                btnCancelar.Enabled = true;
                btnSalir.Enabled = false;
                txtNombreCLI.Focus();
                AddEventosValidacion();
            }
        }

    }
}
