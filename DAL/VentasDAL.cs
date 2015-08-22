using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.IO;


namespace DAL
{
    public class VentasDAL
    {
        public static DataTable GetTabla()
        {
            DataTable tbl = DAL.VentasDAL.GetTabla();
            return tbl;
        }

        public static DataSet CrearDatasetForaneos()
        {
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            MySqlDataAdapter SqlDataAdapter1 = new MySqlDataAdapter();
            MySqlCommand SqlSelectCommand1 = new MySqlCommand("Ventas_Foraneos", SqlConnection1);
            SqlDataAdapter1.SelectCommand = SqlSelectCommand1;
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            DataSet dt = new DataSet();
            SqlDataAdapter1.Fill(dt);
            SqlConnection1.Close();
            return dt;
        }

        public static DataSet CrearDatasetArqueo(string fechaDesde, string fechaHasta, int pc)
        {
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            MySqlDataAdapter SqlDataAdapter1 = new MySqlDataAdapter();
            MySqlCommand SqlSelectCommand1 = new MySqlCommand("Ventas_Arqueo", SqlConnection1);
            SqlDataAdapter1.SelectCommand = SqlSelectCommand1;
            SqlSelectCommand1.Parameters.AddWithValue("p_fecha_desde", fechaDesde);
            SqlSelectCommand1.Parameters.AddWithValue("p_fecha_hasta", fechaHasta);
            SqlSelectCommand1.Parameters.AddWithValue("p_pc", pc);
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            DataSet dt = new DataSet();
            SqlDataAdapter1.Fill(dt);
            SqlConnection1.Close();
            return dt;
        }

        public static DataSet CrearDatasetVentasPesos(int forma, string desde, string hasta, string locales)
        {
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            MySqlDataAdapter SqlDataAdapter1 = new MySqlDataAdapter();
            MySqlCommand SqlSelectCommand1 = new MySqlCommand("VentasPesosCons_Listar", SqlConnection1);
            SqlDataAdapter1.SelectCommand = SqlSelectCommand1;
            SqlSelectCommand1.Parameters.AddWithValue("p_forma", forma);
            SqlSelectCommand1.Parameters.AddWithValue("p_locales", locales);
            SqlSelectCommand1.Parameters.AddWithValue("p_fechaDesde", desde);
            SqlSelectCommand1.Parameters.AddWithValue("p_fechaHasta", hasta);
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            DataSet dt = new DataSet();
            SqlDataAdapter1.Fill(dt);
            SqlConnection1.Close();
            return dt;
        }

        public static DataSet CrearDatasetVentas(int idVenta)
        {
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            MySqlDataAdapter SqlDataAdapter1 = new MySqlDataAdapter();
            MySqlCommand SqlSelectCommand1 = new MySqlCommand("Ventas_Schema", SqlConnection1);
            SqlSelectCommand1.Parameters.AddWithValue("p_id", idVenta);
            SqlDataAdapter1.SelectCommand = SqlSelectCommand1;
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            DataSet dt = new DataSet();
            SqlDataAdapter1.Fill(dt);
            SqlConnection1.Close();
            return dt;
        }

        public static void GrabarDB(DataSet dt, MySqlConnection conn, MySqlTransaction tr)
        {
            MySqlDataAdapter da = AdaptadorABM(dt, conn, tr);
            da.Update(dt, "Ventas");
        }       

        private static MySqlDataAdapter AdaptadorABM(DataSet dt, MySqlConnection SqlConnection1, MySqlTransaction tr)
        {
            MySqlDataAdapter SqlDataAdapter1 = new MySqlDataAdapter();
            MySqlCommand SqlInsertCommand1 = new MySqlCommand("Ventas_Insertar", SqlConnection1);
            MySqlCommand SqlUpdateCommand1 = new MySqlCommand("Ventas_Actualizar", SqlConnection1);
            MySqlCommand SqlDeleteCommand1 = new MySqlCommand("Ventas_Borrar", SqlConnection1);
            SqlInsertCommand1.Transaction = tr;
            SqlUpdateCommand1.Transaction = tr;
            SqlDeleteCommand1.Transaction = tr;
            SqlDataAdapter1.DeleteCommand = SqlDeleteCommand1;
            SqlDataAdapter1.InsertCommand = SqlInsertCommand1;
            SqlDataAdapter1.UpdateCommand = SqlUpdateCommand1;

            SqlInsertCommand1.Parameters.Add("p_id", MySqlDbType.Int32, 11, "IdVentaVEN");
            SqlInsertCommand1.Parameters.Add("p_id_pc", MySqlDbType.Int32, 11, "IdPCVEN");
            SqlInsertCommand1.Parameters.Add("p_fecha", MySqlDbType.DateTime, 20, "FechaVEN");
            SqlInsertCommand1.Parameters.Add("p_cliente", MySqlDbType.Int32, 11, "IdClienteVEN");
            SqlInsertCommand1.CommandType = CommandType.StoredProcedure;

            SqlUpdateCommand1.Parameters.Add("p_id", MySqlDbType.Int32, 11, "IdVentaVEN");
            SqlUpdateCommand1.Parameters.Add("p_id_pc", MySqlDbType.Int32, 11, "IdPCVEN");
            SqlUpdateCommand1.Parameters.Add("p_fecha", MySqlDbType.DateTime, 20, "FechaVEN");
            SqlUpdateCommand1.Parameters.Add("p_cliente", MySqlDbType.Int32, 11, "IdClienteVEN");
            SqlUpdateCommand1.CommandType = CommandType.StoredProcedure;

            SqlDeleteCommand1.Parameters.Add("p_id", MySqlDbType.Int32, 11, "IdVentaVEN");
            SqlDeleteCommand1.CommandType = CommandType.StoredProcedure;
            return SqlDataAdapter1;
        }

        public static DataTable GetByPK(int PK)
        {
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            MySqlDataAdapter SqlDataAdapter1 = new MySqlDataAdapter();
            MySqlCommand SqlSelectCommand1 = new MySqlCommand("Ventas_GetByPK", SqlConnection1);
            SqlDataAdapter1.SelectCommand = SqlSelectCommand1;
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            DataSet dt = new DataSet();
            SqlDataAdapter1.Fill(dt);
            SqlConnection1.Close();
            DataTable tbl = new DataTable();
            tbl = dt.Tables[0];
            return tbl;
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
            MySqlCommand SqlDeleteCommand1 = new MySqlCommand("Ventas_Borrar", SqlConnection1);
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
            foreach(DataRow row in tbl.Rows)
            {                
                PK = Convert.ToInt32(row[0].ToString());
                MySqlDataAdapter SqlDataAdapter1 = new MySqlDataAdapter();
                MySqlCommand SqlDeleteCommand1 = new MySqlCommand("Ventas_Borrar", SqlConnection1);
                SqlDataAdapter1.DeleteCommand = SqlDeleteCommand1;
                SqlDeleteCommand1.CommandType = CommandType.StoredProcedure;
                SqlDeleteCommand1.Parameters.AddWithValue("p_id", PK);
                SqlDeleteCommand1.ExecuteNonQuery();
            }
            SqlConnection1.Close();
        }

        public static DataSet GetLotesTarjetas(int idPc)
        {
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            MySqlDataAdapter SqlDataAdapter1 = new MySqlDataAdapter();
            MySqlCommand SqlSelectCommand1 = new MySqlCommand("Ventas_Lotes_Tarjetas", SqlConnection1);
            SqlSelectCommand1.Parameters.AddWithValue("p_pc", idPc);
            SqlDataAdapter1.SelectCommand = SqlSelectCommand1;
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            DataSet dt = new DataSet();
            SqlDataAdapter1.Fill(dt);
            SqlConnection1.Close();
            return dt;
        }

    }

}
