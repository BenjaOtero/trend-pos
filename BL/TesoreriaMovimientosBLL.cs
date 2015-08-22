using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using DAL;
using System.Threading;


namespace BL
{
    public class TesoreriaMovimientosBLL
    {
        private static object _sync = new object();

        public static DataSet CrearDataset()
        {
            DataSet dt = DAL.TesoreriaMovimientosDAL.CrearDataset();
            return dt;
        }

        public static DataSet CrearDatasetMovimiento(int idMov)
        {
            DataSet ds = new DataSet();
            ds = DAL.TesoreriaMovimientosDAL.CrearDatasetMovimiento(idMov);
            return ds;
        }

        public static void GrabarDB(DataSet dt, ref int? codigoError, bool grabarFallidas)
        {
            try
            {
                if (grabarFallidas == false)
                {
                    DataSet dsRemoto;
                    dsRemoto = dt.GetChanges();
                    DAL.TesoreriaMovimientosDAL.GrabarDB(dt, grabarFallidas);
                    lock (_sync)
                    {
                        Thread t = new Thread(() => bckWrk_DoWork(dsRemoto));
                        t.Start();
                    }
                }
                else
                {
                    DAL.TesoreriaMovimientosDAL.GrabarDB(dt, grabarFallidas);
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1042) //no se pudo abrir la conexion por falta de internet
                {
                    dt.RejectChanges(); ;
                    codigoError = 1042;
                }
                else
                {
                    dt.RejectChanges();
                    codigoError = ex.Number;
                }
            }
        }

        public static void InsertFallidasRemoteServer(DataSet dt, ref int? codigoError, bool grabarFallidas)
        {
            try
            {
                DAL.TesoreriaMovimientosDAL.GrabarDB(dt, grabarFallidas);
                DAL.FallidasDAL.BorrarTesoreriaFallidasByAccion("Added");
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1042) //no se pudo abrir la conexion por falta de internet
                {
                    dt.RejectChanges(); ;
                    codigoError = 1042;
                }
                else
                {
                    dt.RejectChanges();
                    codigoError = ex.Number;
                }
            }
            catch (TimeoutException)
            {
            }
        }

        public static void EditFallidasRemoteServer(DataSet dt, ref int? codigoError, bool grabarFallidas)
        {
            try
            {
                DAL.TesoreriaMovimientosDAL.GrabarDB(dt, grabarFallidas);
                BL.FallidasBLL.BorrarTesoreriaFallidasByAccion("Modified");
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1042) //no se pudo abrir la conexion por falta de internet
                {
                    dt.RejectChanges(); ;
                    codigoError = 1042;
                }
                else
                {
                    dt.RejectChanges();
                    codigoError = ex.Number;
                }
            }
            catch (TimeoutException)
            {
            }
        }

        public static void BorrarByPK(int PK, ref int? codigoError, bool borrarRemotas)
        {
            try
            {
                DAL.TesoreriaMovimientosDAL.BorrarByPK(PK, borrarRemotas);
                if (borrarRemotas == false)
                {
                    lock (_sync)
                    {
                        Thread t = new Thread(() => bckWrk_borrar_DoWork(PK));
                        t.Start();
                    }
                }
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
        }

        //borrar fallidas en remote server
        public static void BorrarByPK(DataTable tbl, ref int? codigoError)
        {
            try
            {
                DAL.TesoreriaMovimientosDAL.BorrarByPK(tbl);
                //borro los registros de la tabla TesoreriaMovimientosFallidas que hacen referencia a los movimientos que no se borraron 
                BL.FallidasBLL.BorrarTesoreriaFallidasByAccion("Deleted");
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
            catch (TimeoutException)
            {
            }
        }

        public static void bckWrk_DoWork(DataSet dsRemoto)
        {            
            try
            {
                DAL.TesoreriaMovimientosDAL.GrabarDB(dsRemoto, true);
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1042 || ex.Number == 0) //no se pudo abrir la conexion por falta de internet o timeout expired
                {
                    Fallidas_bckWrk(dsRemoto);
                }
            }
            catch (TimeoutException)
            {
                Fallidas_bckWrk(dsRemoto);
            }
        }

        private static void Fallidas_bckWrk(DataSet dsRemoto)
        {
            int PK = Convert.ToInt32(dsRemoto.Tables[0].Rows[0][0].ToString());
            if (!DAL.FallidasDAL.ExisteMovTesoreriaFallida(PK, "Added"))
            {
                string estado = dsRemoto.Tables[0].Rows[0].RowState.ToString();
                DataTable tblTesoreriaFallidas = new DataTable();
                tblTesoreriaFallidas.TableName = "TesoreriaMovimientosFallidas";
                tblTesoreriaFallidas.Columns.Add("Id", typeof(int));
                tblTesoreriaFallidas.Columns.Add("Accion", typeof(string));
                DataRow row = tblTesoreriaFallidas.NewRow();
                row["Id"] = PK;
                row["Accion"] = estado;
                tblTesoreriaFallidas.Rows.Add(row);
                DataSet ds = new DataSet();
                ds.Tables.Add(tblTesoreriaFallidas);
                DAL.FallidasDAL.GrabarTesoreriaFallidas(ds);
            }
        }

        private static void bckWrk_borrar_DoWork(int pK)
        {
            try
            {
                bool borrarRemotas = true;
                DAL.TesoreriaMovimientosDAL.BorrarByPK(pK, borrarRemotas);
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1042 || ex.Number == 0) //no se pudo abrir la conexion por falta de internet o timeout expired
                {
                    Fallidas_bckWrk_borrar(pK);
                }
            }
            catch (TimeoutException)
            {
                Fallidas_bckWrk_borrar(pK);
            }
        }

        private static void Fallidas_bckWrk_borrar(int pK)
        {
            //si existe una Movimiento 'Added' con la misma pK que el movimiento, borro la venta 'Added'
            if (DAL.FallidasDAL.ExisteMovTesoreriaFallida(pK, "Added"))
            {
                DAL.FallidasDAL.BorrarTesoreriaFallidas(pK, "Added");
            }
            else
            {
                if (DAL.FallidasDAL.ExisteMovTesoreriaFallida(pK, "Modified"))
                {
                    DAL.FallidasDAL.BorrarTesoreriaFallidas(pK, "Modified");
                }
                DataTable tblTesoreriaFallidas = new DataTable();
                tblTesoreriaFallidas.TableName = "TesoreriaMovimientosFallidas";
                tblTesoreriaFallidas.Columns.Add("Id", typeof(int));
                tblTesoreriaFallidas.Columns.Add("Accion", typeof(string));
                DataRow row = tblTesoreriaFallidas.NewRow();
                row["Id"] = pK;
                row["Accion"] = "Deleted";
                tblTesoreriaFallidas.Rows.Add(row);
                DataSet ds = new DataSet();
                ds.Tables.Add(tblTesoreriaFallidas);
                DAL.FallidasDAL.GrabarTesoreriaFallidas(ds);
            }
        }
    }
}
