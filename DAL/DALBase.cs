using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data.OleDb;

namespace DAL
{
    public class DALBase
    {
        public DALBase()
        {

        }

        public static MySqlConnection GetConnection()
        {
            string connectionString;
            MySqlConnection objCon;
            connectionString = ConfigurationManager.ConnectionStrings["DBMainLocal"].ConnectionString;
            objCon = new MySqlConnection(connectionString);
            return objCon;
        }

        public static MySqlConnection GetRemoteConnection()
        {
            string connectionString;
            MySqlConnection objCon;
            //  connectionString = ConfigurationManager.ConnectionStrings["DBMain"].ConnectionString;
            //connectionString = ConfigurationManager.ConnectionStrings["DBPruebas"].ConnectionString;
            connectionString = ConfigurationManager.ConnectionStrings["NcSoftwa_local"].ConnectionString;
            objCon = new MySqlConnection(connectionString);
            return objCon;
        }

        public static OleDbConnection GetConnectionAccess()
        {
            string connectionString;
            OleDbConnection objCon;
            connectionString = ConfigurationManager.ConnectionStrings["DBAccess"].ConnectionString;
            objCon = new OleDbConnection(connectionString);
            return objCon;
        }
    }
}
