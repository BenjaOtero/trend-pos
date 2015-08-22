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
    public class ArticulosDAL
    {
        [ThreadStatic] static MySqlConnection SqlConnection1;
        private static MySqlDataAdapter SqlDataAdapter1;
        private static MySqlCommand SqlSelectCommand1;
        private static MySqlCommand SqlInsertCommand1;

        public static DataSet ds;
        public static DataTable dt;

        public static DataTable CrearDataset()
        {
            SqlConnection1 = DALBase.GetConnection();
            MySqlDataAdapter da = AdaptadorSELECT(SqlConnection1);
            ds = new DataSet();
            da.Fill(ds, "Articulos");
            SqlConnection1.Close();
            return ds.Tables["Articulos"];
        }

        private static MySqlDataAdapter AdaptadorSELECT(MySqlConnection SqlConnection1)
        {            
            SqlDataAdapter1 = new MySqlDataAdapter();
            SqlSelectCommand1 = new MySqlCommand("Articulos_Listar", SqlConnection1);
            SqlDataAdapter1.SelectCommand = SqlSelectCommand1;
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            return SqlDataAdapter1;
        }

        public static void InsertarRemotos(DataSet dt, MySqlConnection conn, MySqlTransaction tr)
        {
            try
            {
                MySqlDataAdapter da = AdaptadorInsert(conn, tr);
                da.Update(dt, "Articulos");
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
            SqlInsertCommand1 = new MySqlCommand("Articulos_Insertar", SqlConnection1);
            SqlInsertCommand1.Transaction = tr;
            SqlDataAdapter1.InsertCommand = SqlInsertCommand1;

            // IMPLEMENTACIÓN DE LA ORDEN INSERT
            SqlInsertCommand1.Parameters.Add("p_id", MySqlDbType.VarChar, 55, "IdArticuloART");
            SqlInsertCommand1.Parameters.Add("p_idItem", MySqlDbType.Int32, 11, "IdItemART");
            SqlInsertCommand1.Parameters.Add("p_idColor", MySqlDbType.Int32, 11, "IdColorART");
            SqlInsertCommand1.Parameters.Add("p_idAlicuota", MySqlDbType.Int16, 3, "IdAliculotaIvaART");
            SqlInsertCommand1.Parameters.Add("p_talle", MySqlDbType.VarChar, 50, "TalleART");
            SqlInsertCommand1.Parameters.Add("p_idProveedor", MySqlDbType.Int32, 11, "IdProveedorART");
            SqlInsertCommand1.Parameters.Add("p_descripcion", MySqlDbType.VarChar, 55, "DescripcionART");
            SqlInsertCommand1.Parameters.Add("p_descripcionWeb", MySqlDbType.VarChar, 50, "DescripcionWebART");
            SqlInsertCommand1.Parameters.Add("p_precioCosto", MySqlDbType.Decimal, 19, "PrecioCostoART");
            SqlInsertCommand1.Parameters.Add("p_precioPublico", MySqlDbType.Decimal, 19, "PrecioPublicoART");
            SqlInsertCommand1.Parameters.Add("p_precioMayor", MySqlDbType.Decimal, 19, "PrecioMayorART");
            SqlInsertCommand1.Parameters.Add("p_fecha", MySqlDbType.DateTime, 19, "FechaART");
            SqlInsertCommand1.Parameters.Add("p_imagen", MySqlDbType.VarChar, 50, "ImagenART");
            SqlInsertCommand1.Parameters.Add("p_imagenBack", MySqlDbType.VarChar, 50, "ImagenBackART");
            SqlInsertCommand1.Parameters.Add("p_imagenColor", MySqlDbType.VarChar, 50, "ImagenColorART");
            SqlInsertCommand1.Parameters.Add("p_activoWeb", MySqlDbType.Int32, 1, "ActivoWebART");
            SqlInsertCommand1.Parameters.Add("p_nuevo", MySqlDbType.Int32, 1, "NuevoART");
            SqlInsertCommand1.CommandType = CommandType.StoredProcedure;

            return SqlDataAdapter1;
        }

    }

}
