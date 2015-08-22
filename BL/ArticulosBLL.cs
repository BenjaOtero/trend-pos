using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.IO;
using System.Windows.Forms;
using DAL;


namespace BL
{
    public class ArticulosBLL
    {
        public DataTable tblArticulos;
        public DataTable tblArticulosItems;
        public DataTable tblColores;
        public DataTable tblProveedores;
        public DataTable tblLocales;

        public static DataTable CrearDataset()
        {
            DataTable dt = DAL.ArticulosDAL.CrearDataset();
            return dt;
        }

        public static void InsertarRemotos(DataSet dt)
        {
            MySqlTransaction tr = null;
            try
            {
                MySqlConnection SqlConnection1 = DALBase.GetConnection();
                SqlConnection1.Open();
                tr = SqlConnection1.BeginTransaction();
                DAL.ArticulosDAL.InsertarRemotos(dt, SqlConnection1, tr);
                tr.Commit();
                SqlConnection1.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString(), "Trend", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dt.RejectChanges();
                tr.Rollback();
            }
        }

    }
}
