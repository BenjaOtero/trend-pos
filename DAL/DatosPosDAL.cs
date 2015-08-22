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
    public class DatosPosDAL
    {
        private static MySqlConnection SqlConnection1;
        private static MySqlDataAdapter SqlDataAdapter1;
        private static MySqlCommand SqlSelectCommand1;
        private static MySqlCommand SqlDeleteCommand1;
        public static DataSet dt;

        public static DataSet GetAll()
        {
            SqlConnection1 = DALBase.GetRemoteConnection();
            MySqlDataAdapter da = AdaptadorSELECT(SqlConnection1);
            dt = new DataSet();
            da.Fill(dt);
            SqlConnection1.Close();
            return dt;
        }

        private static MySqlDataAdapter AdaptadorSELECT(MySqlConnection SqlConnection1)
        {            
            SqlDataAdapter1 = new MySqlDataAdapter();
            SqlSelectCommand1 = new MySqlCommand("DatosPos_Listar", SqlConnection1);
            SqlDataAdapter1.SelectCommand = SqlSelectCommand1;
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            return SqlDataAdapter1;
        }

        public static void DeleteAll(Int16 existenClientesFallidas)
        {
            SqlConnection1 = DALBase.GetConnection();
            SqlConnection1.Open();
            SqlDataAdapter1 = new MySqlDataAdapter();
            SqlDeleteCommand1 = new MySqlCommand("DatosPos_Borrar", SqlConnection1);
            SqlDeleteCommand1.Parameters.AddWithValue("p_existe", existenClientesFallidas);
            SqlDataAdapter1.DeleteCommand = SqlDeleteCommand1;
            SqlDeleteCommand1.CommandType = CommandType.StoredProcedure;
            SqlDeleteCommand1.ExecuteNonQuery();
            SqlConnection1.Close();
        }
    }

}
