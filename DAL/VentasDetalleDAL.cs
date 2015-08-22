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
    public class VentasDetalleDAL
    {
        public static DataTable GetTabla()
        {
            DataTable tbl = new DataTable();
            tbl.TableName = "VentasDetalle";
            tbl.Columns.Add("IdDVEN", typeof(int));
            tbl.Columns.Add("IdVentaDVEN", typeof(int));
            tbl.Columns.Add("IdLocalDVEN", typeof(int));
            tbl.Columns.Add("IdArticuloDVEN", typeof(string));
            tbl.Columns.Add("DescripcionDVEN", typeof(string));
            tbl.Columns.Add("CantidadDVEN", typeof(int));
            tbl.Columns.Add("PrecioPublicoDVEN", typeof(double));
            tbl.Columns.Add("PrecioCostoDVEN", typeof(double));
            tbl.Columns.Add("PrecioMayorDVEN", typeof(double));
            tbl.Columns.Add("IdFormaPagoDVEN", typeof(int));
            tbl.Columns.Add("NroCuponDVEN", typeof(int));
            tbl.Columns.Add("NroFacturaDVEN", typeof(int));
            tbl.Columns.Add("IdEmpleadoDVEN", typeof(int));
            tbl.Columns.Add("LiquidadoDVEN", typeof(int));
            tbl.Columns.Add("EsperaDVEN", typeof(int));
            tbl.Columns.Add("DevolucionDVEN", typeof(int));
            tbl.PrimaryKey = new DataColumn[] { tbl.Columns["IdDVEN"] };
            return tbl;
        }

        public static void GrabarDB(DataSet dt, MySqlConnection conn, MySqlTransaction tr)
        {
            MySqlDataAdapter da = AdaptadorABM(conn, tr);
            da.Update(dt, "VentasDetalle");
        }

        private static MySqlDataAdapter AdaptadorABM(MySqlConnection SqlConnection1, MySqlTransaction tr)
        {
            MySqlDataAdapter SqlDataAdapter1 = new MySqlDataAdapter();
            MySqlCommand SqlInsertCommand1 = new MySqlCommand("VentasDetalle_Insertar", SqlConnection1);
            MySqlCommand SqlUpdateCommand1 = new MySqlCommand("VentasDetalle_Actualizar", SqlConnection1);
            MySqlCommand SqlDeleteCommand1 = new MySqlCommand("VentasDetalle_Borrar", SqlConnection1);
            SqlInsertCommand1.Transaction = tr;
            SqlUpdateCommand1.Transaction = tr;
            SqlDeleteCommand1.Transaction = tr;
            SqlDataAdapter1.DeleteCommand = SqlDeleteCommand1;
            SqlDataAdapter1.InsertCommand = SqlInsertCommand1;
            SqlDataAdapter1.UpdateCommand = SqlUpdateCommand1;

            // IMPLEMENTACIÓN DE LA ORDEN INSERT
            SqlInsertCommand1.Parameters.Add("p_id_detalle", MySqlDbType.Int32, 11, "IdDVEN");
            SqlInsertCommand1.Parameters.Add("p_id_venta", MySqlDbType.Int32, 11, "IdVentaDVEN");
            SqlInsertCommand1.Parameters.Add("p_id_local", MySqlDbType.Int32, 11, "IdLocalDVEN");
            SqlInsertCommand1.Parameters.Add("p_articulo", MySqlDbType.VarChar, 50, "IdArticuloDVEN");
            SqlInsertCommand1.Parameters.Add("p_cantidad", MySqlDbType.Int32, 11, "CantidadDVEN");
            SqlInsertCommand1.Parameters.Add("p_publico", MySqlDbType.Double, 11, "PrecioPublicoDVEN");
            SqlInsertCommand1.Parameters.Add("p_costo", MySqlDbType.Double, 11, "PrecioCostoDVEN");
            SqlInsertCommand1.Parameters.Add("p_mayor", MySqlDbType.Double, 11, "PrecioMayorDVEN");
            SqlInsertCommand1.Parameters.Add("p_forma_pago", MySqlDbType.Int32, 11, "IdFormaPagoDVEN");
            SqlInsertCommand1.Parameters.Add("p_nro_cupon", MySqlDbType.Int32, 11, "NroCuponDVEN");
            SqlInsertCommand1.Parameters.Add("p_nro_factura", MySqlDbType.Int32, 11, "NroFacturaDVEN");
            SqlInsertCommand1.Parameters.Add("p_id_empleado", MySqlDbType.Int32, 11, "IdEmpleadoDVEN");
            SqlInsertCommand1.Parameters.Add("p_liquidado", MySqlDbType.Bit, 11, "LiquidadoDVEN");
            SqlInsertCommand1.Parameters.Add("p_devolucion", MySqlDbType.Bit, 11, "DevolucionDVEN");
            SqlInsertCommand1.CommandType = CommandType.StoredProcedure;

            // IMPLEMENTACIÓN DE LA ORDEN UPDATE
            SqlUpdateCommand1.Parameters.Add("p_id_detalle", MySqlDbType.Int32, 11, "IdDVEN");
            SqlUpdateCommand1.Parameters.Add("p_id_venta", MySqlDbType.Int32, 11, "IdVentaDVEN");
            SqlUpdateCommand1.Parameters.Add("p_id_local", MySqlDbType.Int32, 11, "IdLocalDVEN");
            SqlUpdateCommand1.Parameters.Add("p_articulo", MySqlDbType.VarChar, 50, "IdArticuloDVEN");
            SqlUpdateCommand1.Parameters.Add("p_cantidad", MySqlDbType.Int32, 11, "CantidadDVEN");
            SqlUpdateCommand1.Parameters.Add("p_publico", MySqlDbType.Double, 11, "PrecioPublicoDVEN");
            SqlUpdateCommand1.Parameters.Add("p_costo", MySqlDbType.Double, 11, "PrecioCostoDVEN");
            SqlUpdateCommand1.Parameters.Add("p_mayor", MySqlDbType.Double, 11, "PrecioMayorDVEN");
            SqlUpdateCommand1.Parameters.Add("p_forma_pago", MySqlDbType.Int32, 11, "IdFormaPagoDVEN");
            SqlUpdateCommand1.Parameters.Add("p_nro_cupon", MySqlDbType.Int32, 11, "NroCuponDVEN");
            SqlUpdateCommand1.Parameters.Add("p_nro_factura", MySqlDbType.Int32, 11, "NroFacturaDVEN");
            SqlUpdateCommand1.Parameters.Add("p_id_empleado", MySqlDbType.Int32, 11, "IdEmpleadoDVEN");
            SqlUpdateCommand1.Parameters.Add("p_liquidado", MySqlDbType.Bit, 11, "LiquidadoDVEN");
            SqlUpdateCommand1.Parameters.Add("p_devolucion", MySqlDbType.Bit, 11, "DevolucionDVEN");
            SqlUpdateCommand1.CommandType = CommandType.StoredProcedure;

            // IMPLEMENTACIÓN DE LA ORDEN DELETE
            SqlDeleteCommand1.Parameters.Add("p_id_detalle", MySqlDbType.Int32, 11, "IdDVEN");
            SqlDeleteCommand1.CommandType = CommandType.StoredProcedure;
            return SqlDataAdapter1;
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
                MySqlCommand SqlDeleteCommand1 = new MySqlCommand("VentasDetalle_Borrar", SqlConnection1);
                SqlDataAdapter1.DeleteCommand = SqlDeleteCommand1;
                SqlDeleteCommand1.CommandType = CommandType.StoredProcedure;
                SqlDeleteCommand1.Parameters.AddWithValue("p_id_detalle", PK);
                SqlDeleteCommand1.ExecuteNonQuery();
            }
            SqlConnection1.Close();
        }

        public static DataSet GetSchema(int idVenta)
        {
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            MySqlDataAdapter SqlDataAdapter1 = new MySqlDataAdapter();
            MySqlCommand SqlSelectCommand1 = new MySqlCommand("Ventas_Schema", SqlConnection1);
            SqlSelectCommand1.Parameters.AddWithValue("p_id", idVenta);
            SqlDataAdapter1.SelectCommand = SqlSelectCommand1;
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            DataSet dt = new DataSet();
            SqlDataAdapter1.Fill(dt);
            SqlConnection1.Close();
            return dt;
        }

        public static DataSet GetFallidas(int idVenta)
        {
            MySqlConnection SqlConnection1 = DALBase.GetConnection();
            MySqlDataAdapter SqlDataAdapter1 = new MySqlDataAdapter();
            MySqlCommand SqlSelectCommand1 = new MySqlCommand("VentasDetalle_GetFallidas", SqlConnection1);
            SqlSelectCommand1.Parameters.AddWithValue("p_id", idVenta);
            SqlDataAdapter1.SelectCommand = SqlSelectCommand1;
            SqlSelectCommand1.CommandType = CommandType.StoredProcedure;
            DataSet dt = new DataSet();
            SqlDataAdapter1.Fill(dt);
            SqlConnection1.Close();
            return dt;
        }
        
    }
}
