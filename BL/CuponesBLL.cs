using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.IO;
using DAL;


namespace BL
{
    public class CuponesBLL
    {

        public static DataTable GetByPk(string nroCupon)
        {
            DataTable tbl = DAL.CuponesDAL.GetByPk(nroCupon);
            return tbl;
        }

    }
}
