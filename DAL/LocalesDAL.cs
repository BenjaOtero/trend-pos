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
    public class LocalesDAL
    {
        private static MySqlConnection SqlConnection1;
        private static MySqlDataAdapter SqlDataAdapter1;
        private static MySqlCommand SqlSelectCommand1;
        public static DataSet ds;
        public static DataTable dt;

        public static DataTable CrearDataset()
        {
            SqlConnection1 = DALBase.GetConnection();
            SqlDataAdapter1 = new MySqlDataAdapter();
            SqlSelectCommand1 = new MySqlCommand("Locales_Listar", SqlConnection1);
            SqlDataAdapter1.SelectCommand = SqlSelectCommand1;
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            ds = new DataSet();
            SqlDataAdapter1.Fill(ds, "Locales");
            SqlConnection1.Close();
            return ds.Tables[0];
        }

    }

}
