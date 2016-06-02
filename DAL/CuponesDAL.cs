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
    public class CuponesDAL
    {
        public static DataTable GetByPk(string nroCupon)
        {
            MySqlConnection SqlConnection1 = DALBase.GetRemoteConnection();
            MySqlDataAdapter SqlDataAdapter1 = new MySqlDataAdapter();
            MySqlCommand SqlSelectCommand1 = new MySqlCommand("Cupones_GetByPk", SqlConnection1);
            SqlSelectCommand1.Parameters.AddWithValue("p_cupon", nroCupon);
            SqlDataAdapter1.SelectCommand = SqlSelectCommand1;
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            DataTable tbl = new DataTable();
            SqlDataAdapter1.Fill(tbl);
            SqlConnection1.Close();
            return tbl;
        }

    }
}
