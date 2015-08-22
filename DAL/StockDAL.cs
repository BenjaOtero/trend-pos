using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.Common;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace DAL
{
    public class StockDAL
    {
        private static MySqlConnection SqlConnection1;
        private static MySqlDataAdapter SqlDataAdapter1;
        private static MySqlCommand SqlSelectCommand1;
        public static DataSet dt;

        public static DataSet CrearDataset(string whereLocales, int proveedor, string articulo, string descripcion)
        {
            SqlConnection1 = DALBase.GetRemoteConnection();
            SqlDataAdapter1 = new MySqlDataAdapter();
            SqlSelectCommand1 = new MySqlCommand("Stock_Cons", SqlConnection1);
            SqlDataAdapter1.SelectCommand = SqlSelectCommand1;
            SqlSelectCommand1.Parameters.AddWithValue("p_locales", whereLocales);
            SqlSelectCommand1.Parameters.AddWithValue("p_proveedor", proveedor);
            SqlSelectCommand1.Parameters.AddWithValue("p_articulo", articulo);
            SqlSelectCommand1.Parameters.AddWithValue("p_descripcion", descripcion);
            SqlSelectCommand1.Parameters.AddWithValue("p_activoWeb", 0);
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            dt = new DataSet();
            SqlDataAdapter1.Fill(dt, "Stock");
            SqlConnection1.Close();
            return dt;
        }
     

    }

}
