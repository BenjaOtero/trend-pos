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
    public class StockBLL 
    {

        public static DataSet CrearDataset(string whereLocales, int proveedor, string articulo, string descripcion, ref int? codigoError)
        {
            DataSet dt = new DataSet();
            try
            {
                dt = DAL.StockDAL.CrearDataset(whereLocales, proveedor, articulo, descripcion);
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1042) //no se pudo abrir la conexion por falta de internet
                {
                    codigoError = 1042;
                }
                else
                {
                    codigoError = ex.Number;
                }
            }
            return dt;
        }

    }
}
