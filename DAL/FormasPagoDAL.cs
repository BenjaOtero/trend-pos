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
    public class FormasPagoDAL
    {
        private static MySqlConnection SqlConnection1;
        private static MySqlDataAdapter SqlDataAdapter1;
        private static MySqlCommand SqlSelectCommand1;
        private static MySqlCommand SqlInsertCommand1;
        public static DataSet ds;
        public static DataTable dt;

        public static DataTable CrearDataset()
        {
            SqlConnection1 = DALBase.GetConnection();
            SqlDataAdapter1 = new MySqlDataAdapter();
            SqlSelectCommand1 = new MySqlCommand("FormasPago_Listar", SqlConnection1);
            SqlDataAdapter1.SelectCommand = SqlSelectCommand1;
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            ds = new DataSet();
            SqlDataAdapter1.Fill(ds, "FormasPago");
            SqlConnection1.Close();
            return ds.Tables[0];
        }

        public static void InsertRemotos(DataSet dt, MySqlConnection conn, MySqlTransaction tr)
        {
            try
            {
                MySqlDataAdapter da = AdaptadorInsert(conn, tr);
                da.Update(dt, "FormasPago");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString(), "Trend", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dt.RejectChanges();
            }
        }

        private static MySqlDataAdapter AdaptadorInsert(MySqlConnection SqlConnection1, MySqlTransaction tr)
        {
            SqlDataAdapter1 = new MySqlDataAdapter();
            SqlInsertCommand1 = new MySqlCommand("FormasPago_Insertar", SqlConnection1);
            SqlInsertCommand1.Transaction = tr;
            SqlDataAdapter1.InsertCommand = SqlInsertCommand1;

            // IMPLEMENTACIÓN DE LA ORDEN INSERT
            SqlInsertCommand1.Parameters.Add("p_id", MySqlDbType.Int32, 11, "IdFormaPagoFOR");
            SqlInsertCommand1.Parameters.Add("p_descripcion", MySqlDbType.VarChar, 50, "DescripcionFOR");
            SqlInsertCommand1.CommandType = CommandType.StoredProcedure;

            return SqlDataAdapter1;
        }

    }
}
