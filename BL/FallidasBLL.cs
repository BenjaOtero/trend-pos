using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.IO;
using DAL;

namespace BL
{
    public class FallidasBLL
    {
        /*---------------------------------------- Ventas -----------------------------------------------*/

        public static DataTable VentasGetByAccion(string accion)
        {
            DataTable tbl = DAL.FallidasDAL.VentasGetByAccion(accion);
            return tbl;
        }

        public static void BorrarVentasFallidasByAccion(string accion)
        {
            DAL.FallidasDAL.BorrarVentasFallidasByAccion(accion);
        }

        public static DataTable VentasDetalleGetByAccion(string accion)
        {
            DataTable tbl = DAL.FallidasDAL.VentasDetalleGetByAccion(accion);
            return tbl;
        }

        public static void BorrarVentasDetalleFallidasByAccion(string accion)
        {
            DAL.FallidasDAL.BorrarVentasDetalleFallidasByAccion(accion);
        }


        /*------------------------------------ TesoreriaMovimientos ---------------------------------------*/

        public static DataTable TesoreriaGetByAccion(string accion)
        {
            DataTable tbl = DAL.FallidasDAL.TesoreriaGetByAccion(accion);
            return tbl;
        }

        public static void BorrarTesoreriaFallidasByAccion(string accion)
        {
            DAL.FallidasDAL.BorrarTesoreriaFallidasByAccion(accion);
        }


        /*---------------------------------------- FondoCaja -----------------------------------------------*/

        public static DataTable FondoCajaGetByAccion(string accion)
        {
            DataTable tbl = DAL.FallidasDAL.FondoCajaGetByAccion(accion);
            return tbl;
        }

        public static void BorrarFondoCajaFallidasByAccion(string accion)
        {
            DAL.FallidasDAL.BorrarFondoCajaFallidasByAccion(accion);
        }

        /*------------------------------------ Clientes ---------------------------------------*/


    }
}
