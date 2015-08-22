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
    public class ProveedoresBLL
    {


        public static DataTable CrearTabla()
        {
            DataTable tbl = DAL.ProveedoresDAL.CrearTabla();
            return tbl;
        }

    }
}
