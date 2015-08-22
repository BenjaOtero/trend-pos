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
    public class ProveedoresDAL
    {
        private static MySqlConnection SqlConnection1;
        private static MySqlDataAdapter SqlDataAdapter1;
        private static MySqlCommand SqlSelectCommand1;
        public static DataSet dt;

        public static DataTable CrearTabla()
        {
            MySqlDataAdapter da = AdaptadorSELECT();
            dt = new DataSet();
            da.Fill(dt, "Proveedores");
            DataTable tbl = dt.Tables[0];
            return tbl;
        }

        private static MySqlDataAdapter AdaptadorSELECT()
        {
            SqlConnection1 = DALBase.GetConnection();
            SqlDataAdapter1 = new MySqlDataAdapter();
            SqlSelectCommand1 = new MySqlCommand("Proveedores_Listar", SqlConnection1);
            SqlDataAdapter1.SelectCommand = SqlSelectCommand1;
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            return SqlDataAdapter1;
        }


    }

}
