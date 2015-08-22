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
    public class PcsBLL
    {

        public DataSet dt;
        public DataTable tblClientes;

        public static DataTable CrearDataset()
        {
            DataTable dt = DAL.PcsDAL.CrearDataset();
            return dt;
        }

    }
}
