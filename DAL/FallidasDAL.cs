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
    public class FallidasDAL
    {
        [ThreadStatic] static MySqlConnection SqlConnection1;
        private static MySqlDataAdapter SqlDataAdapter1;
        private static MySqlCommand SqlSelectCommand1;
        private static MySqlCommand SqlInsertCommand1;
        private static MySqlCommand SqlUpdateCommand1;
        private static MySqlCommand SqlDeleteCommand1;
        public static DataSet dt;


        /*---------------------------------------- Ventas -----------------------------------------------*/

        public static bool ExisteVentaFallida(int id, string accion)
        {
            bool seleccionarFallida;

            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            SqlConnection1.Open();
            SqlSelectCommand1 = new MySqlCommand("VentasFallidas_Existe", SqlConnection1);
            SqlSelectCommand1.Parameters.AddWithValue("p_id", id);
            SqlSelectCommand1.Parameters.AddWithValue("p_accion", accion);
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            MySqlDataReader reader = SqlSelectCommand1.ExecuteReader();
            DataTable tbl = new DataTable();
            tbl.Load(reader);
            SqlConnection1.Close();
            if (tbl.Rows.Count > 0)
            {
                seleccionarFallida = true;
                return seleccionarFallida;
            }
            else
            {
                seleccionarFallida = false;
                return seleccionarFallida;
            }            
        }

        public static void BorrarVentaFallidas(int id, string accion)
        {
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            SqlConnection1.Open();
            SqlSelectCommand1 = new MySqlCommand("VentasFallidas_Borrar", SqlConnection1);
            SqlSelectCommand1.Parameters.AddWithValue("p_id", id);
            SqlSelectCommand1.Parameters.AddWithValue("p_accion", accion);
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            SqlSelectCommand1.ExecuteNonQuery();
            SqlConnection1.Close();
        }

        public static void BorrarVentasFallidasByAccion(string accion)
        {
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            SqlConnection1.Open();
            SqlSelectCommand1 = new MySqlCommand("VentasFallidas_BorrarByAccion", SqlConnection1);
            SqlSelectCommand1.Parameters.AddWithValue("p_accion", accion);
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            SqlSelectCommand1.ExecuteNonQuery();
            SqlConnection1.Close();
        }

        public static void GrabarVentasFallidas(DataSet dt, MySqlConnection conn)
        {
            try
            {
                MySqlDataAdapter da = AdaptadorVentasFallidas(conn);
                da.Update(dt, "VentasFallidas");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString(), "Trend", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dt.RejectChanges();
            }
        }

        private static MySqlDataAdapter AdaptadorVentasFallidas(MySqlConnection SqlConnection1)
        {
            SqlDataAdapter1 = new MySqlDataAdapter();
            SqlInsertCommand1 = new MySqlCommand("VentasFallidas_Insertar", SqlConnection1);
            SqlDeleteCommand1 = new MySqlCommand("VentasFallidas_Borrar", SqlConnection1);
            SqlDataAdapter1.DeleteCommand = SqlDeleteCommand1;
            SqlDataAdapter1.InsertCommand = SqlInsertCommand1;

            // IMPLEMENTACIÓN DE LA ORDEN INSERT
            SqlInsertCommand1.Parameters.Add("p_id", MySqlDbType.Int32, 10, "Id");
            SqlInsertCommand1.Parameters.Add("p_accion", MySqlDbType.VarChar, 20, "Accion");
            SqlInsertCommand1.CommandType = CommandType.StoredProcedure;

            // IMPLEMENTACIÓN DE LA ORDEN DELETE
            SqlDeleteCommand1.CommandType = CommandType.StoredProcedure;
            return SqlDataAdapter1;
        }

        public static DataTable VentasGetByAccion(string accion)
        {
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            SqlConnection1.Open();
            SqlSelectCommand1 = new MySqlCommand("VentasFallidas_GetByAccion", SqlConnection1);
            SqlSelectCommand1.Parameters.AddWithValue("p_accion", accion);
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            MySqlDataReader reader = SqlSelectCommand1.ExecuteReader();
            DataTable tbl = new DataTable();
            tbl.Load(reader);
            SqlConnection1.Close();
            return tbl;
        }


        /*---------------------------------------- VentasDetalle -----------------------------------------------*/

        public static void BorrarDetalleFallidasByVenta(int id)
        {
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            SqlConnection1.Open();
            SqlDeleteCommand1 = new MySqlCommand("VentasDetalleFallidas_DelByVenta", SqlConnection1);
            SqlDeleteCommand1.Parameters.AddWithValue("p_id_venta", id);
            SqlDeleteCommand1.CommandType = CommandType.StoredProcedure;
            SqlDeleteCommand1.ExecuteNonQuery();
            SqlConnection1.Close();
        }

        public static void BorrarVentasDetalleFallidasByAccion(string accion)
        {
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            SqlConnection1.Open();
            SqlSelectCommand1 = new MySqlCommand("VentasDetalleFallidas_BorrarByAccion", SqlConnection1);
            SqlSelectCommand1.Parameters.AddWithValue("p_accion", accion);
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            SqlSelectCommand1.ExecuteNonQuery();
            SqlConnection1.Close();
        }

        public static bool ExisteVentaDetalleFallida(int id, string accion)
        {
            bool seleccionarFallida;
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            SqlConnection1.Open();
            SqlSelectCommand1 = new MySqlCommand("VentasDetalleFallidas_Existe", SqlConnection1);
            SqlSelectCommand1.Parameters.AddWithValue("p_id", id);
            SqlSelectCommand1.Parameters.AddWithValue("p_accion", accion);
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            MySqlDataReader reader = SqlSelectCommand1.ExecuteReader();
            DataTable tbl = new DataTable();
            tbl.Load(reader);
            SqlConnection1.Close();
            if (tbl.Rows.Count > 0)
            {
                seleccionarFallida = true;
                return seleccionarFallida;
            }
            else
            {
                seleccionarFallida = false;
                return seleccionarFallida;
            }   
        }

        public static void BorrarDetalleFallidas(int id, string accion)
        {
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            SqlConnection1.Open();
            SqlDeleteCommand1 = new MySqlCommand("VentasDetalleFallidas_Borrar", SqlConnection1);
            SqlDeleteCommand1.Parameters.AddWithValue("p_id", id);
            SqlDeleteCommand1.Parameters.AddWithValue("p_accion", accion);
            SqlDeleteCommand1.CommandType = CommandType.StoredProcedure;
            SqlDeleteCommand1.ExecuteNonQuery();
            SqlConnection1.Close();
        }

        public static void GrabarVentasDetalleFallidas(DataSet dt, MySqlConnection conn)
        {
            try
            {
                MySqlDataAdapter da = AdaptadorVentasDetalleFallidas(conn);
                da.Update(dt, "VentasDetalleFallidas");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString(), "Trend", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dt.RejectChanges();
            }
        }

        private static MySqlDataAdapter AdaptadorVentasDetalleFallidas(MySqlConnection SqlConnection1)
        {
            SqlDataAdapter1 = new MySqlDataAdapter();
            SqlInsertCommand1 = new MySqlCommand("VentasDetalleFallidas_Insertar", SqlConnection1);
            SqlDataAdapter1.InsertCommand = SqlInsertCommand1;
            SqlInsertCommand1.Parameters.Add("p_id", MySqlDbType.Int32, 10, "Id");
            SqlInsertCommand1.Parameters.Add("p_id_venta", MySqlDbType.Int32, 10, "IdVenta");
            SqlInsertCommand1.Parameters.Add("p_accion", MySqlDbType.VarChar, 20, "Accion");
            SqlInsertCommand1.CommandType = CommandType.StoredProcedure;
            return SqlDataAdapter1;
        }

        public static DataTable VentasDetalleGetByAccion(string accion)
        {
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            SqlConnection1.Open();
            SqlSelectCommand1 = new MySqlCommand("VentasDetalleFallidas_GetByAccion", SqlConnection1);
            SqlSelectCommand1.Parameters.AddWithValue("p_accion", accion);
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            MySqlDataReader reader = SqlSelectCommand1.ExecuteReader();
            DataTable tbl = new DataTable();
            tbl.Load(reader);
            SqlConnection1.Close();
            return tbl;
        }


        /*---------------------------------------- TesoreriaMovimientos -----------------------------------------------*/

        public static bool ExisteMovTesoreriaFallida(int id, string accion)
        {
            bool seleccionarFallida;
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            SqlConnection1.Open();
            SqlSelectCommand1 = new MySqlCommand("TesoreriaMovimientosFallidas_Existe", SqlConnection1);
            SqlSelectCommand1.Parameters.AddWithValue("p_id", id);
            SqlSelectCommand1.Parameters.AddWithValue("p_accion", accion);
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            MySqlDataReader reader = SqlSelectCommand1.ExecuteReader();
            DataTable tbl = new DataTable();
            tbl.Load(reader);
            SqlConnection1.Close();
            if (tbl.Rows.Count > 0)
            {
                seleccionarFallida = true;
                return seleccionarFallida;
            }
            else
            {
                seleccionarFallida = false;
                return seleccionarFallida;
            }   
        }

        public static void BorrarTesoreriaFallidas(int id, string accion)
        {
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            SqlConnection1.Open();
            SqlSelectCommand1 = new MySqlCommand("TesoreriaMovimientosFallidas_Borrar", SqlConnection1);
            SqlSelectCommand1.Parameters.AddWithValue("p_id", id);
            SqlSelectCommand1.Parameters.AddWithValue("p_accion", accion);
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            SqlSelectCommand1.ExecuteNonQuery();
            SqlConnection1.Close();
        }

        public static void GrabarTesoreriaFallidas(DataSet dt)
        {
            try
            {
                SqlConnection1 = DALBase.GetConnection();
                MySqlDataAdapter da = AdaptadorTesoreriaFallidas(SqlConnection1);
                da.Update(dt, "TesoreriaMovimientosFallidas");
                SqlConnection1.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString(), "Trend", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dt.RejectChanges();
            }
        }

        private static MySqlDataAdapter AdaptadorTesoreriaFallidas(MySqlConnection SqlConnection1)
        {
            SqlDataAdapter1 = new MySqlDataAdapter();
            SqlInsertCommand1 = new MySqlCommand("TesoreriaMovimientosFallidas_Insertar", SqlConnection1);
            SqlUpdateCommand1 = new MySqlCommand("TesoreriaMovimientosFallidas_Actualizar", SqlConnection1);
            SqlDeleteCommand1 = new MySqlCommand("TesoreriaMovimientosFallidas_Borrar", SqlConnection1);
            SqlDataAdapter1.DeleteCommand = SqlDeleteCommand1;
            SqlDataAdapter1.UpdateCommand = SqlUpdateCommand1;
            SqlDataAdapter1.InsertCommand = SqlInsertCommand1;

            // IMPLEMENTACIÓN DE LA ORDEN INSERT
            SqlInsertCommand1.Parameters.Add("p_id", MySqlDbType.Int32, 10, "Id");
            SqlInsertCommand1.Parameters.Add("p_accion", MySqlDbType.VarChar, 20, "Accion");
            SqlInsertCommand1.CommandType = CommandType.StoredProcedure;

            // IMPLEMENTACIÓN DE LA ORDEN UPDATE
            SqlUpdateCommand1.Parameters.Add("p_id", MySqlDbType.Int32, 10, "Id");
            SqlUpdateCommand1.Parameters.Add("p_accion", MySqlDbType.VarChar, 20, "Accion");
            SqlUpdateCommand1.CommandType = CommandType.StoredProcedure;

            // IMPLEMENTACIÓN DE LA ORDEN DELETE
            SqlDeleteCommand1.CommandType = CommandType.StoredProcedure;
            return SqlDataAdapter1;
        }

        public static DataTable TesoreriaGetByAccion(string accion)
        {
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            SqlConnection1.Open();
            SqlSelectCommand1 = new MySqlCommand("TesoreriaMovimientosFallidas_GetByAccion", SqlConnection1);
            SqlSelectCommand1.Parameters.AddWithValue("p_accion", accion);
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            MySqlDataReader reader = SqlSelectCommand1.ExecuteReader();
            DataTable tbl = new DataTable();
            tbl.Load(reader);
            SqlConnection1.Close();
            return tbl;
        }

        public static void BorrarTesoreriaFallidasByAccion(string accion)
        {
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            SqlConnection1.Open();
            SqlSelectCommand1 = new MySqlCommand("TesoreriaMovimientosFallidas_BorrarByAccion", SqlConnection1);
            SqlSelectCommand1.Parameters.AddWithValue("p_accion", accion);
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            SqlSelectCommand1.ExecuteNonQuery();
            SqlConnection1.Close();
        }

        
        /*---------------------------------------- FondoCaja -----------------------------------------------*/

        public static bool ExisteFondoCajaFallida(int id, string accion)
        {
            bool seleccionarFallida;
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            SqlConnection1.Open();
            SqlSelectCommand1 = new MySqlCommand("FondoCajaFallidas_Existe", SqlConnection1);
            SqlSelectCommand1.Parameters.AddWithValue("p_id", id);
            SqlSelectCommand1.Parameters.AddWithValue("p_accion", accion);
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            MySqlDataReader reader = SqlSelectCommand1.ExecuteReader();
            DataTable tbl = new DataTable();
            tbl.Load(reader);
            SqlConnection1.Close();
            if (tbl.Rows.Count > 0)
            {
                seleccionarFallida = true;
                return seleccionarFallida;
            }
            else
            {
                seleccionarFallida = false;
                return seleccionarFallida;
            }   
        }

        public static void GrabarFondoCajaFallidas(DataSet dt, MySqlConnection conn)
        {
            try
            {
                MySqlDataAdapter da = AdaptadorFondoCajaFallidas(conn);
                da.Update(dt, "FondoCajaFallidas");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString(), "Trend", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dt.RejectChanges();
            }
        }

        private static MySqlDataAdapter AdaptadorFondoCajaFallidas(MySqlConnection SqlConnection1)
        {
            SqlDataAdapter1 = new MySqlDataAdapter();
            SqlInsertCommand1 = new MySqlCommand("FondoCajaFallidas_Insertar", SqlConnection1);
            SqlUpdateCommand1 = new MySqlCommand("FondoCajaFallidas_Actualizar", SqlConnection1);
            SqlDeleteCommand1 = new MySqlCommand("FondoCajaFallidas_Borrar", SqlConnection1);
            SqlDataAdapter1.DeleteCommand = SqlDeleteCommand1;
            SqlDataAdapter1.UpdateCommand = SqlUpdateCommand1;
            SqlDataAdapter1.InsertCommand = SqlInsertCommand1;

            // IMPLEMENTACIÓN DE LA ORDEN INSERT
            SqlInsertCommand1.Parameters.Add("p_id", MySqlDbType.Int32, 10, "Id");
            SqlInsertCommand1.Parameters.Add("p_accion", MySqlDbType.VarChar, 20, "Accion");
            SqlInsertCommand1.CommandType = CommandType.StoredProcedure;

            // IMPLEMENTACIÓN DE LA ORDEN UPDATE
            SqlUpdateCommand1.Parameters.Add("p_id", MySqlDbType.Int32, 10, "Id");
            SqlUpdateCommand1.Parameters.Add("p_accion", MySqlDbType.VarChar, 20, "Accion");
            SqlUpdateCommand1.CommandType = CommandType.StoredProcedure;

            // IMPLEMENTACIÓN DE LA ORDEN DELETE
            SqlDeleteCommand1.CommandType = CommandType.StoredProcedure;
            return SqlDataAdapter1;
        }

        public static DataTable FondoCajaGetByAccion(string accion)
        {
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            SqlConnection1.Open();
            SqlSelectCommand1 = new MySqlCommand("FondoCajaFallidas_GetByAccion", SqlConnection1);
            SqlSelectCommand1.Parameters.AddWithValue("p_accion", accion);
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            MySqlDataReader reader = SqlSelectCommand1.ExecuteReader();
            DataTable tbl = new DataTable();
            tbl.Load(reader);
            SqlConnection1.Close();
            return tbl;
        }

        public static void BorrarFondoCajaFallidasByAccion(string accion)
        {
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            SqlConnection1.Open();
            SqlSelectCommand1 = new MySqlCommand("FondoCajaFallidas_BorrarByAccion", SqlConnection1);
            SqlSelectCommand1.Parameters.AddWithValue("p_accion", accion);
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            SqlSelectCommand1.ExecuteNonQuery();
            SqlConnection1.Close();
        }

        /*---------------------------------------- Clientes -----------------------------------------------*/

        public static bool ExisteClienteFallida(int id, string accion)
        {
            bool seleccionarFallida;
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            SqlConnection1.Open();
            SqlSelectCommand1 = new MySqlCommand("ClientesFallidas_Existe", SqlConnection1);
            SqlSelectCommand1.Parameters.AddWithValue("p_id", id);
            SqlSelectCommand1.Parameters.AddWithValue("p_accion", accion);
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            MySqlDataReader reader = SqlSelectCommand1.ExecuteReader();
            DataTable tbl = new DataTable();
            tbl.Load(reader);
            SqlConnection1.Close();
            if (tbl.Rows.Count > 0)
            {
                seleccionarFallida = true;
                return seleccionarFallida;
            }
            else
            {
                seleccionarFallida = false;
                return seleccionarFallida;
            }
        }

        public static DataTable GetClienteFallida()
        {
            SqlConnection1 = DALBase.GetConnection();
            MySqlDataAdapter SqlDataAdapter1 = new MySqlDataAdapter();
            SqlSelectCommand1 = new MySqlCommand("ClientesFallidas_Get", SqlConnection1);
            SqlDataAdapter1.SelectCommand = SqlSelectCommand1;
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            DataSet ds = new DataSet();
            DataTable tbl = new DataTable();
            SqlDataAdapter1.Fill(tbl);
            tbl.TableName = "ClientesFallidas";
            tbl.Columns["Id"].Unique = true;
            tbl.PrimaryKey = new DataColumn[] { tbl.Columns["Id"] };
            SqlConnection1.Close();
            return tbl;
        }

        public static void BorrarClienteFallidas(int id, string accion)
        {
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            SqlConnection1.Open();
            SqlSelectCommand1 = new MySqlCommand("ClientesFallidas_Borrar", SqlConnection1);
            SqlSelectCommand1.Parameters.AddWithValue("p_id", id);
            SqlSelectCommand1.Parameters.AddWithValue("p_accion", accion);
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            SqlSelectCommand1.ExecuteNonQuery();
            SqlConnection1.Close();
        }

        
        public static void BorrarClienteFallidasByPK(int id)
        {
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            SqlConnection1.Open();
            SqlSelectCommand1 = new MySqlCommand("ClientesFallidas_BorrarByPK", SqlConnection1);
            SqlSelectCommand1.Parameters.AddWithValue("p_id", id);
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            SqlSelectCommand1.ExecuteNonQuery();
            SqlConnection1.Close();
        }

        public static void GrabarClienteFallidas(DataSet dt)
        {
            try
            {
                SqlConnection1 = DALBase.GetConnection();
                MySqlDataAdapter da = AdaptadorClienteFallidas(SqlConnection1);
                da.Update(dt, "clientesfallidas");
                SqlConnection1.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString(), "Trend", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dt.RejectChanges();
            }
        }

        private static MySqlDataAdapter AdaptadorClienteFallidas(MySqlConnection SqlConnection1)
        {
            SqlDataAdapter1 = new MySqlDataAdapter();
            SqlInsertCommand1 = new MySqlCommand("ClientesFallidas_Insertar", SqlConnection1);
            SqlUpdateCommand1 = new MySqlCommand("ClientesFallidas_Actualizar", SqlConnection1);
            SqlDeleteCommand1 = new MySqlCommand("ClientesFallidas_Borrar", SqlConnection1);
            SqlDataAdapter1.DeleteCommand = SqlDeleteCommand1;
            SqlDataAdapter1.UpdateCommand = SqlUpdateCommand1;
            SqlDataAdapter1.InsertCommand = SqlInsertCommand1;

            // IMPLEMENTACIÓN DE LA ORDEN INSERT
            SqlInsertCommand1.Parameters.Add("p_id", MySqlDbType.Int32, 10, "Id");
            SqlInsertCommand1.Parameters.Add("p_accion", MySqlDbType.VarChar, 20, "Accion");
            SqlInsertCommand1.CommandType = CommandType.StoredProcedure;

            // IMPLEMENTACIÓN DE LA ORDEN UPDATE
            SqlUpdateCommand1.Parameters.Add("p_id", MySqlDbType.Int32, 10, "Id");
            SqlUpdateCommand1.Parameters.Add("p_accion", MySqlDbType.VarChar, 20, "Accion");
            SqlUpdateCommand1.CommandType = CommandType.StoredProcedure;

            // IMPLEMENTACIÓN DE LA ORDEN DELETE
            SqlDeleteCommand1.CommandType = CommandType.StoredProcedure;
            return SqlDataAdapter1;
        }

                

    }

}
