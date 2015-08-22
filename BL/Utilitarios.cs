using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Management;
using System.Net.NetworkInformation;
using System.ComponentModel;
using System.ServiceProcess;
using System.Configuration;

namespace BL
{
    public class Utilitarios
    {
        static Button grabar;
        static DataTable tblTabla;

        public static void SoloNumeros(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.')
            {
                e.Handled = false;
            }
            if (e.KeyChar == '\b')
            {
                e.Handled = false;
            }
        }

        public static void SoloNumerosConComa(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            if (e.KeyChar == ',')
            {
                e.Handled = false;
            }
            if (e.KeyChar == '.')
            {
                // si se pulsa en el punto se convertirá en coma
                e.Handled = true; //anula la tecla "." pulsada
                SendKeys.Send(",");
            }
            if (e.KeyChar == '\b')
            {
                e.Handled = false;
            }
        }

        public static DataTable Pivot(DataTable dt, DataColumn pivotColumn, DataColumn pivotValue)
        {
            // find primary key columns 
            //(i.e. everything but pivot column and pivot value)
            DataTable temp = dt.Copy();
            temp.Columns.Remove(pivotColumn.ColumnName);
            temp.Columns.Remove(pivotValue.ColumnName);
            string[] pkColumnNames = temp.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToArray();

            // prep results table
            DataTable result = temp.DefaultView.ToTable(true, pkColumnNames).Copy();
            result.PrimaryKey = result.Columns.Cast<DataColumn>().ToArray();

            dt.AsEnumerable()
                .Select(r => r[pivotColumn.ColumnName].ToString())
                .Distinct().ToList()
                .ForEach(c => result.Columns.Add(c, pivotColumn.DataType));

            // load it
            foreach (DataRow row in dt.Rows)
            {
                // find row to update
                DataRow aggRow = result.Rows.Find(
                    pkColumnNames
                        .Select(c => row[c])
                        .ToArray());
                // the aggregate used here is LATEST 
                // adjust the next line if you want (SUM, MAX, etc...)
                aggRow[row[pivotColumn.ColumnName].ToString()] = row[pivotValue.ColumnName];
            }

            return result;
        }

        public static bool HayInternet()
        {
            bool conexion = false;
            Ping Pings = new Ping();
            int timeout = 3000;
            try
            {
                if (Pings.Send("dns26.cyberneticos.com", timeout).Status == IPStatus.Success)
                {
                    conexion = true;
                }
                conexion = true;
            }
            catch (PingException)
            {
                conexion = true;
            }
            return conexion;
        }

        public static DataSet ActualizarBD()
        {
            Int16 existenClientes = 1;
            DataSet ds = BL.DatosPosBLL.GetAll();
            DataTable tblArticulos = ds.Tables[0];
            tblArticulos.TableName = "Articulos";
            DataTable tblClientes = ds.Tables[1];
            tblClientes.TableName = "Clientes";
            DataTable tblFormasPago = ds.Tables[2];
            tblFormasPago.TableName = "FormasPago";
            DataTable tblAlicuotasIva = ds.Tables[3];
            tblAlicuotasIva.TableName = "AlicuotasIva";
            if (tblArticulos.Rows.Count > 0)
            {
                foreach (DataRow rowArticulo in tblArticulos.Rows)
                {
                    rowArticulo.SetAdded();
                }
            }
            if (!DAL.ClientesDAL.ExistenClientesFallidas()) //si hay registros pendientes de grabar en el remote server no actualizo clientes
            {
                if (tblClientes.Rows.Count > 0)
                {
                    foreach (DataRow rowCliente in tblClientes.Rows)
                    {
                        rowCliente.SetAdded();
                    }
                }
                existenClientes = 0;
            }
            if (tblFormasPago.Rows.Count > 0)
            {
                foreach (DataRow rowForma in tblFormasPago.Rows)
                {
                    rowForma.SetAdded();
                }
            }
            if (tblAlicuotasIva.Rows.Count > 0)
            {
                foreach (DataRow rowAlicuota in tblAlicuotasIva.Rows)
                {
                    rowAlicuota.SetAdded();
                }
            }
            if (ds.Tables.Count == 4)
            {
                BL.DatosPosBLL.DeleteAll(existenClientes);
                BL.ArticulosBLL.InsertarRemotos(ds);
                if(existenClientes ==0) BL.ClientesBLL.InsertRemotos(ds);
                BL.FormasPagoBLL.InsertRemotos(ds);
                BL.AlicuotasIvaBLL.InsertRemotos(ds);
            }
            return ds;
        }

        public static void SelTextoTextBox(object sender, EventArgs e)
        {
            try
            {
                TextBox tb = (TextBox)sender;
                tb.SelectionStart = 0;
                tb.SelectionLength = tb.Text.Length;
            }
            catch (InvalidCastException)
            {
                MaskedTextBox tb = (MaskedTextBox)sender;
                tb.SelectionStart = 0;
                tb.SelectionLength = tb.Text.Length;
            }
        }

        public static void EnterTab(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return) SendKeys.Send("{TAB}");
        }

        public static void AddEventosABM(Control grpCampos, ref Button btnGrabar, ref DataTable tbl)
        {
            tblTabla = tbl;
            tblTabla.ColumnChanged += new DataColumnChangeEventHandler(HabilitarGrabar);
            grabar = btnGrabar;
            foreach (Control ctl in grpCampos.Controls)
            {
                if (ctl is TextBox || ctl is MaskedTextBox)
                {
                    ctl.Enter += new System.EventHandler(SelTextoTextBox);
                    ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(EnterTab);
                    ctl.TextChanged += new System.EventHandler(HabilitarGrabar);
                }
            }
        }

        public static void HabilitarGrabar(object sender, EventArgs e)
        {
            if (grabar.Enabled == false)
            {
                grabar.Enabled = true;
            }
        }

        public static void DataBindingsAdd(BindingSource bnd, GroupBox grp)
        {
            foreach (Control ctl in grp.Controls)
            {
                if (ctl is TextBox)
                {
                    string campo = ctl.Name.Substring(3, ctl.Name.Length - 3);
                    ctl.DataBindings.Add("Text", bnd, campo, false, DataSourceUpdateMode.OnPropertyChanged);
                }
                else if (ctl is CheckBox)
                {
                    string campo = ctl.Name.Substring(3, ctl.Name.Length - 3);
                    //   ctl.DataBindings.Add("YesNo", bnd, campo, false, DataSourceUpdateMode.OnPropertyChanged);                
                }
            }
        }

        public static void ValidarComboBox(object sender, CancelEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            if (!string.IsNullOrEmpty(cmb.Text))
            {
                if (cmb.SelectedValue == null)
                {
                    e.Cancel = true;
                }
            }
        }
    }

}
