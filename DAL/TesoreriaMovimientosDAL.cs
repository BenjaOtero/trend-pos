using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

using System.Threading;

namespace DAL
{
    public class TesoreriaMovimientosDAL
    {

        public static DataSet CrearDataset()
        {
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            MySqlDataAdapter SqlDataAdapter1 = new MySqlDataAdapter();
            MySqlCommand SqlSelectCommand1 = new MySqlCommand("TesoreriaMov_Listar", SqlConnection1);
            SqlDataAdapter1.SelectCommand = SqlSelectCommand1;
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            DataSet dt = new DataSet();
            SqlDataAdapter1.Fill(dt, "TesoreriaMovimientos");
            SqlConnection1.Close();
            return dt;
        }

        public static DataSet CrearDatasetMovimiento(int idMov)
        {
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            MySqlDataAdapter SqlDataAdapter1 = new MySqlDataAdapter();
            MySqlCommand SqlSelectCommand1 = new MySqlCommand("TesoreriaMovimientos_GetByPk", SqlConnection1);
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
            da.Update(dt, "TesoreriaMovimientos");
            SqlConnection1.Close();
        }

        private static MySqlDataAdapter AdaptadorABM(MySqlConnection SqlConnection1)
        {
            MySqlDataAdapter SqlDataAdapter1 = new MySqlDataAdapter();
            MySqlCommand SqlInsertCommand1 = new MySqlCommand("TesoreriaMov_Insertar", SqlConnection1);
            MySqlCommand SqlUpdateCommand1 = new MySqlCommand("TesoreriaMov_Actualizar", SqlConnection1);
            MySqlCommand SqlDeleteCommand1 = new MySqlCommand("TesoreriaMov_Borrar", SqlConnection1);
            SqlDataAdapter1.DeleteCommand = SqlDeleteCommand1;
            SqlDataAdapter1.InsertCommand = SqlInsertCommand1;
            SqlDataAdapter1.UpdateCommand = SqlUpdateCommand1;



            // IMPLEMENTACIÓN DE LA ORDEN UPDATE
            SqlUpdateCommand1.Parameters.Add("p_id", MySqlDbType.Int32, 11, "IdMovTESM");
            SqlUpdateCommand1.Parameters.Add("p_fecha", MySqlDbType.DateTime, 20,"FechaTESM");
            SqlUpdateCommand1.Parameters.Add("p_pc", MySqlDbType.Int32, 11, "IdPcTESM");
            SqlUpdateCommand1.Parameters.Add("p_detalle", MySqlDbType.VarChar, 200, "DetalleTESM");
            SqlUpdateCommand1.Parameters.Add("p_importe", MySqlDbType.Double, 50, "ImporteTESM");
            SqlUpdateCommand1.CommandType = CommandType.StoredProcedure;

            // IMPLEMENTACIÓN DE LA ORDEN INSERT
            SqlInsertCommand1.Parameters.Add("p_id", MySqlDbType.Int32, 11, "IdMovTESM");
            SqlInsertCommand1.Parameters.Add("p_fecha", MySqlDbType.DateTime, 20, "FechaTESM");
            SqlInsertCommand1.Parameters.Add("p_pc", MySqlDbType.Int32, 11, "IdPcTESM");
            SqlInsertCommand1.Parameters.Add("p_detalle", MySqlDbType.VarChar, 200, "DetalleTESM");
            SqlInsertCommand1.Parameters.Add("p_importe", MySqlDbType.Double, 50, "ImporteTESM");
            SqlInsertCommand1.CommandType = CommandType.StoredProcedure;

            // IMPLEMENTACIÓN DE LA ORDEN DELETE
            SqlDeleteCommand1.Parameters.Add("p_id", MySqlDbType.Int32, 11, "IdMovTESM");
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
            MySqlCommand SqlDeleteCommand1 = new MySqlCommand("TesoreriaMov_Borrar", SqlConnection1);
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
                MySqlCommand SqlDeleteCommand1 = new MySqlCommand("TesoreriaMov_Borrar", SqlConnection1);
                SqlDataAdapter1.DeleteCommand = SqlDeleteCommand1;
                SqlDeleteCommand1.CommandType = CommandType.StoredProcedure;
                SqlDeleteCommand1.Parameters.AddWithValue("p_id", PK);
                SqlDeleteCommand1.ExecuteNonQuery();
            }
            SqlConnection1.Close();
        }

    }

}
