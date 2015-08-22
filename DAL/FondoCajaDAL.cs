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
    public class FondoCajaDAL
    {

        public static DataSet CrearDataset()
        {
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            MySqlDataAdapter SqlDataAdapter1 = new MySqlDataAdapter();
            MySqlCommand SqlSelectCommand1 = new MySqlCommand("FondoCaja_Listar", SqlConnection1);
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter1.SelectCommand = SqlSelectCommand1; 
            DataSet dt = new DataSet();
            SqlDataAdapter1.Fill(dt, "FondoCaja");
            SqlConnection1.Close();
            return dt;
        }

        public static DataSet CrearDataset(string fecha, int idPc)
        {
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            MySqlDataAdapter SqlDataAdapter1 = new MySqlDataAdapter();
            MySqlCommand SqlSelectCommand1 = new MySqlCommand("FondoCaja_Inicial_Final", SqlConnection1);
            SqlDataAdapter1.SelectCommand = SqlSelectCommand1;
            SqlSelectCommand1.Parameters.AddWithValue("p_fecha", fecha);
            SqlSelectCommand1.Parameters.AddWithValue("p_idPc", idPc);
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            DataSet dt = new DataSet();
            SqlDataAdapter1.Fill(dt);
            SqlConnection1.Close();
            return dt;
        }

        public static void GrabarDB(DataSet dt, MySqlConnection SqlConnection1)
        {
            try
            {
                MySqlDataAdapter da = AdaptadorABM(SqlConnection1);
                da.Update(dt, "FondoCaja");
            }
            catch (MySqlException ex)
            {
              //  MessageBox.Show(ex.ToString(), "Trend", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dt.RejectChanges();
            }
        }

        private static MySqlDataAdapter AdaptadorABM(MySqlConnection SqlConnection1)
        {
            MySqlDataAdapter SqlDataAdapter1 = new MySqlDataAdapter();;
            MySqlCommand SqlInsertCommand1 = new MySqlCommand("FondoCaja_Insertar2", SqlConnection1);
            MySqlCommand SqlUpdateCommand1 = new MySqlCommand("FondoCaja_Actualizar2", SqlConnection1);
            SqlDataAdapter1.InsertCommand = SqlInsertCommand1;
            SqlDataAdapter1.UpdateCommand = SqlUpdateCommand1;

            // IMPLEMENTACIÓN DE LA ORDEN UPDATE
            SqlUpdateCommand1.Parameters.Add("p_id", MySqlDbType.Int32, 11, "IdFondoFONP");
            SqlUpdateCommand1.Parameters.Add("p_fecha", MySqlDbType.DateTime, 20, "FechaFONP");
            SqlUpdateCommand1.Parameters.Add("p_pc", MySqlDbType.Int32, 11, "IdPcFONP");
            SqlUpdateCommand1.Parameters.Add("p_importe", MySqlDbType.Double, 50, "ImporteFONP");
            SqlUpdateCommand1.CommandType = CommandType.StoredProcedure;

            // IMPLEMENTACIÓN DE LA ORDEN INSERT
            SqlInsertCommand1.Parameters.Add("p_id", MySqlDbType.Int32, 11, "IdFondoFONP");
            SqlInsertCommand1.Parameters.Add("p_fecha", MySqlDbType.DateTime, 20, "FechaFONP");
            SqlInsertCommand1.Parameters.Add("p_pc", MySqlDbType.Int32, 11, "IdPcFONP");
            SqlInsertCommand1.Parameters.Add("p_importe", MySqlDbType.Double, 50, "ImporteFONP");
            SqlInsertCommand1.CommandType = CommandType.StoredProcedure;
            return SqlDataAdapter1;
        }

        public static DataSet GetByPk(int idMov)
        {
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            MySqlDataAdapter SqlDataAdapter1 = new MySqlDataAdapter(); ;
            MySqlCommand SqlSelectCommand1 = new MySqlCommand("FondoCaja_GetByPk", SqlConnection1);
            SqlSelectCommand1.Parameters.AddWithValue("p_id", idMov);
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter1.SelectCommand = SqlSelectCommand1;   
            DataSet dt = new DataSet();
            SqlDataAdapter1.Fill(dt);
            SqlConnection1.Close();
            return dt;
        }

    }

}