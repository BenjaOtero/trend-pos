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
    public class AlicuotasIvaDAL
    {
        public static void InsertRemotos(DataSet dt, MySqlConnection conn, MySqlTransaction tr)
        {
            try
            {
                MySqlDataAdapter da = AdaptadorInsert(conn, tr);
                da.Update(dt, "AlicuotasIva");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString(), "Trend", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dt.RejectChanges();
            }
        }

        private static MySqlDataAdapter AdaptadorInsert(MySqlConnection SqlConnection1, MySqlTransaction tr)
        {
            MySqlDataAdapter SqlDataAdapter1 = new MySqlDataAdapter();
            MySqlCommand SqlInsertCommand1 = new MySqlCommand("AlicuotasIva_Insertar", SqlConnection1);
            SqlInsertCommand1.Transaction = tr;
            SqlDataAdapter1.InsertCommand = SqlInsertCommand1;

            // IMPLEMENTACIÓN DE LA ORDEN INSERT
            SqlInsertCommand1.Parameters.Add("p_id", MySqlDbType.Int16, 2, "IdAlicuotaALI");
            SqlInsertCommand1.Parameters.Add("p_porcentaje", MySqlDbType.Decimal, 12, "PorcentajeALI");
            SqlInsertCommand1.CommandType = CommandType.StoredProcedure;

            return SqlDataAdapter1;
        }
    }
}
