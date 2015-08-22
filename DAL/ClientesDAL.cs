using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace DAL
{
    public class ClientesDAL
    {

        public static DataSet GetClientes()
        {
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            MySqlDataAdapter SqlDataAdapter1 = new MySqlDataAdapter();
            MySqlCommand SqlSelectCommand1 = new MySqlCommand("Clientes_Listar", SqlConnection1);
            SqlDataAdapter1.SelectCommand = SqlSelectCommand1;
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            DataSet dt = new DataSet();
            SqlDataAdapter1.Fill(dt, "Clientes");
            SqlConnection1.Close();
            return dt;
        }

        public static DataSet GetRegistroFallido(int idMov)
        {
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            MySqlDataAdapter SqlDataAdapter1 = new MySqlDataAdapter();
            MySqlCommand SqlSelectCommand1 = new MySqlCommand("Clientes_GetByPk", SqlConnection1);
            SqlSelectCommand1.Parameters.AddWithValue("p_id", idMov);
            SqlDataAdapter1.SelectCommand = SqlSelectCommand1;
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            DataSet dt = new DataSet();
            SqlDataAdapter1.Fill(dt);
            SqlConnection1.Close();
            return dt;
        }

        public static void GrabarDB(DataSet dt, bool grabarFallidas)
        {
            MySqlConnection SqlConnection1;
            if (grabarFallidas == false)
            {
                SqlConnection1 = DALBase.GetConnection();
            }
            else
            {
                SqlConnection1 = DALBase.GetRemoteConnection();
            }
            MySqlDataAdapter da = AdaptadorABM(SqlConnection1);
            da.Update(dt, "Clientes");
            SqlConnection1.Close();
        }

        private static MySqlDataAdapter AdaptadorABM(MySqlConnection SqlConnection1)
        {            
            MySqlCommand SqlInsertCommand1;
            MySqlCommand SqlUpdateCommand1;
            MySqlCommand SqlDeleteCommand1;
            MySqlDataAdapter SqlDataAdapter1 = new MySqlDataAdapter();
            SqlInsertCommand1 = new MySqlCommand("Clientes_Insertar", SqlConnection1);
            SqlUpdateCommand1 = new MySqlCommand("Clientes_Actualizar", SqlConnection1);
            SqlDeleteCommand1 = new MySqlCommand("Clientes_Borrar", SqlConnection1);
            SqlDataAdapter1.DeleteCommand = SqlDeleteCommand1;
            SqlDataAdapter1.InsertCommand = SqlInsertCommand1;
            SqlDataAdapter1.UpdateCommand = SqlUpdateCommand1;


            // IMPLEMENTACIÓN DE LA ORDEN UPDATE
            SqlUpdateCommand1.Parameters.Add("p_id", MySqlDbType.Int32, 11, "IdClienteCLI");
            SqlUpdateCommand1.Parameters.Add("p_razon", MySqlDbType.VarChar, 50, "RazonSocialCLI");
            SqlUpdateCommand1.Parameters.Add("p_cuit", MySqlDbType.VarChar, 50, "CUIT");
            SqlUpdateCommand1.Parameters.Add("p_condicion", MySqlDbType.VarChar, 50, "CondicionIvaCLI");
            SqlUpdateCommand1.Parameters.Add("p_direccion", MySqlDbType.VarChar, 50, "DireccionCLI");
            SqlUpdateCommand1.Parameters.Add("p_localidad", MySqlDbType.VarChar, 50, "LocalidadCLI");
            SqlUpdateCommand1.Parameters.Add("p_provincia", MySqlDbType.VarChar, 50, "ProvinciaCLI");
            SqlUpdateCommand1.Parameters.Add("p_transporte", MySqlDbType.VarChar, 50, "TransporteCLI");
            SqlUpdateCommand1.Parameters.Add("p_contacto", MySqlDbType.VarChar, 50, "ContactoCLI");
            SqlUpdateCommand1.Parameters.Add("p_telefono", MySqlDbType.VarChar, 50, "TelefonoCLI");
            SqlUpdateCommand1.Parameters.Add("p_movil", MySqlDbType.VarChar, 50, "MovilCLI");
            SqlUpdateCommand1.Parameters.Add("p_correo", MySqlDbType.VarChar, 50, "CorreoCLI");
            SqlUpdateCommand1.Parameters.Add("p_fecha", MySqlDbType.Date, 50, "FechaNacCLI");
            SqlUpdateCommand1.CommandType = CommandType.StoredProcedure;

            // IMPLEMENTACIÓN DE LA ORDEN INSERT
            SqlInsertCommand1.Parameters.Add("p_id", MySqlDbType.Int32, 11, "IdClienteCLI");
            SqlInsertCommand1.Parameters.Add("p_razon", MySqlDbType.VarChar, 50, "RazonSocialCLI");
            SqlInsertCommand1.Parameters.Add("p_cuit", MySqlDbType.VarChar, 50, "CUIT");
            SqlInsertCommand1.Parameters.Add("p_condicion", MySqlDbType.VarChar, 50, "CondicionIvaCLI");
            SqlInsertCommand1.Parameters.Add("p_direccion", MySqlDbType.VarChar, 50, "DireccionCLI");
            SqlInsertCommand1.Parameters.Add("p_localidad", MySqlDbType.VarChar, 50, "LocalidadCLI");
            SqlInsertCommand1.Parameters.Add("p_provincia", MySqlDbType.VarChar, 50, "ProvinciaCLI");
            SqlInsertCommand1.Parameters.Add("p_transporte", MySqlDbType.VarChar, 50, "TransporteCLI");
            SqlInsertCommand1.Parameters.Add("p_contacto", MySqlDbType.VarChar, 50, "ContactoCLI");
            SqlInsertCommand1.Parameters.Add("p_telefono", MySqlDbType.VarChar, 50, "TelefonoCLI");
            SqlInsertCommand1.Parameters.Add("p_movil", MySqlDbType.VarChar, 50, "MovilCLI");
            SqlInsertCommand1.Parameters.Add("p_correo", MySqlDbType.VarChar, 50, "CorreoCLI");
            SqlInsertCommand1.Parameters.Add("p_fecha", MySqlDbType.Date, 50, "FechaNacCLI");
            SqlInsertCommand1.CommandType = CommandType.StoredProcedure;

            // IMPLEMENTACIÓN DE LA ORDEN DELETE
            SqlDeleteCommand1.Parameters.Add("p_id", MySqlDbType.Int32, 11, "IdClienteCLI");
            SqlDeleteCommand1.CommandType = CommandType.StoredProcedure;
            return SqlDataAdapter1;
        }

        public static void BorrarByPK(int PK, bool borrarRemotas)
        {
            MySqlConnection SqlConnection1;
            if (borrarRemotas != true)
            {
                SqlConnection1 = DALBase.GetConnection();
            }
            else
            {
                SqlConnection1 = DALBase.GetRemoteConnection();
            }
            SqlConnection1.Open();
            MySqlDataAdapter SqlDataAdapter1 = new MySqlDataAdapter();
            MySqlCommand SqlDeleteCommand1 = new MySqlCommand("Clientes_Borrar", SqlConnection1);
            SqlDataAdapter1.DeleteCommand = SqlDeleteCommand1;
            SqlDeleteCommand1.Parameters.AddWithValue("p_id", PK);
            SqlDeleteCommand1.CommandType = CommandType.StoredProcedure;
            SqlDeleteCommand1.ExecuteNonQuery();
            SqlConnection1.Close();
        }

        public static void BorrarByPK(DataTable tbl)
        {
            MySqlConnection SqlConnection1 = DALBase.GetRemoteConnection();
            int PK;
            SqlConnection1.Open();
            foreach (DataRow row in tbl.Rows)
            {
                PK = Convert.ToInt32(row[0].ToString());
                MySqlDataAdapter SqlDataAdapter1 = new MySqlDataAdapter();
                MySqlCommand SqlDeleteCommand1 = new MySqlCommand("Clientes_Borrar", SqlConnection1);
                SqlDataAdapter1.DeleteCommand = SqlDeleteCommand1;
                SqlDeleteCommand1.CommandType = CommandType.StoredProcedure;
                SqlDeleteCommand1.Parameters.AddWithValue("p_id", PK);
                SqlDeleteCommand1.ExecuteNonQuery();
            }
            SqlConnection1.Close();
        }

        public static void BorrarClienteFallidasByAccion(string accion)
        {
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            SqlConnection1.Open();
            MySqlCommand SqlSelectCommand1 = new MySqlCommand("ClientesFallidas_BorrarByAccion", SqlConnection1);
            SqlSelectCommand1.Parameters.AddWithValue("p_accion", accion);
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            SqlSelectCommand1.ExecuteNonQuery();
            SqlConnection1.Close();
        }

        public static DataTable ClienteGetByAccion(string accion)
        {
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            SqlConnection1.Open();
            MySqlCommand SqlSelectCommand1 = new MySqlCommand("ClientesFallidas_GetByAccion", SqlConnection1);
            SqlSelectCommand1.Parameters.AddWithValue("p_accion", accion);
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            MySqlDataReader reader = SqlSelectCommand1.ExecuteReader();
            DataTable tbl = new DataTable();
            tbl.Load(reader);
            SqlConnection1.Close();
            return tbl;
        }

        public static bool ExistenClientesFallidas()
        {
            bool existen = false;
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            SqlConnection1.Open();
            MySqlCommand SqlSelectCommand1 = new MySqlCommand("ClientesFallidas_Get", SqlConnection1);
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            MySqlDataReader reader = SqlSelectCommand1.ExecuteReader();
            DataTable tbl = new DataTable();
            tbl.Load(reader);
            SqlConnection1.Close();
            if (tbl.Rows.Count > 0) existen = true;
            return existen;
        }

        /*------------------------------------------------------------------------------------*/

        public static void InsertRemotos(DataSet dt, MySqlConnection conn, MySqlTransaction tr)
        {
            try
            {
                MySqlDataAdapter da = AdaptadorInsert(conn, tr);
                da.Update(dt, "Clientes");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString(), "Trend", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dt.RejectChanges();
            }
        }

        private static MySqlDataAdapter AdaptadorInsert(MySqlConnection SqlConnection1, MySqlTransaction tr)
        {
            MySqlDataAdapter SqlDataAdapter1 = new MySqlDataAdapter();
            MySqlCommand SqlInsertCommand1 = new MySqlCommand("Clientes_Insertar", SqlConnection1);
            SqlInsertCommand1.Transaction = tr;
            SqlDataAdapter1.InsertCommand = SqlInsertCommand1;

            // IMPLEMENTACIÓN DE LA ORDEN INSERT
            SqlInsertCommand1.Parameters.Add("p_id", MySqlDbType.Int32, 11, "IdClienteCLI");
            SqlInsertCommand1.Parameters.Add("p_razon", MySqlDbType.VarChar, 50, "RazonSocialCLI");
            SqlInsertCommand1.Parameters.Add("p_cuit", MySqlDbType.VarChar, 50, "CUIT");
            SqlInsertCommand1.Parameters.Add("p_condicion", MySqlDbType.VarChar, 50, "CondicionIvaCLI");
            SqlInsertCommand1.Parameters.Add("p_direccion", MySqlDbType.VarChar, 50, "DireccionCLI");
            SqlInsertCommand1.Parameters.Add("p_localidad", MySqlDbType.VarChar, 50, "LocalidadCLI");
            SqlInsertCommand1.Parameters.Add("p_provincia", MySqlDbType.VarChar, 50, "ProvinciaCLI");
            SqlInsertCommand1.Parameters.Add("p_transporte", MySqlDbType.VarChar, 50, "TransporteCLI");
            SqlInsertCommand1.Parameters.Add("p_contacto", MySqlDbType.VarChar, 50, "ContactoCLI");
            SqlInsertCommand1.Parameters.Add("p_telefono", MySqlDbType.VarChar, 50, "TelefonoCLI");
            SqlInsertCommand1.Parameters.Add("p_movil", MySqlDbType.VarChar, 50, "MovilCLI");
            SqlInsertCommand1.Parameters.Add("p_correo", MySqlDbType.VarChar, 50, "CorreoCLI");
            SqlInsertCommand1.Parameters.Add("p_fecha", MySqlDbType.VarChar, 50, "FechaNacCLI");
            SqlInsertCommand1.CommandType = CommandType.StoredProcedure;
            return SqlDataAdapter1;
        }



    }

}
