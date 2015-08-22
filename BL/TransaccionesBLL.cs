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
using System.Diagnostics;


namespace BL
{
    public class TransaccionesBLL
    {
        private static object _sync = new object();

        public TransaccionesBLL()
        {
        }

        // constructor para grabar inserciones de registros
        public static void GrabarVentas(DataSet dtVentas, ref int? codigoError, bool grabarFallidas)
        {
            MySqlTransaction tr = null;
            try
            {
                if (grabarFallidas == false)
                {
                    DataSet dsRemoto = dtVentas.GetChanges();
                    MySqlConnection SqlConnection1 = DALBase.GetConnection();
                    SqlConnection1.Open();
                    tr = SqlConnection1.BeginTransaction();
                    DAL.VentasDAL.GrabarDB(dtVentas, SqlConnection1, tr);
                    DAL.VentasDetalleDAL.GrabarDB(dtVentas, SqlConnection1, tr);
                    tr.Commit();
                    SqlConnection1.Close();
                    lock (_sync)
                    {
                        Thread t = new Thread(() => bckWrk_DoWork(dsRemoto));
                        t.Start();
                    }
                }
                else //grabo las ventas que no se grabaron en localhost en el remote server
                {
                    MySqlConnection SqlConnection1 = DALBase.GetRemoteConnection();
                    SqlConnection1.Open();
                    tr = SqlConnection1.BeginTransaction();
                    DAL.VentasDAL.GrabarDB(dtVentas, SqlConnection1, tr);
                    DAL.VentasDetalleDAL.GrabarDB(dtVentas, SqlConnection1, tr);
                    tr.Commit();
                    BL.FallidasBLL.BorrarVentasFallidasByAccion("Added");
                    SqlConnection1.Close();
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1042) //no se pudo abrir la conexion por falta de internet
                {
                    if (grabarFallidas == false)
                    {
                        dtVentas.RejectChanges();
                    }
                    codigoError = 1042;
                }
                else
                {
                    if (grabarFallidas == false)
                    {
                        dtVentas.RejectChanges();
                    }
                    if (tr != null)
                    {
                        tr.Rollback();
                    }
                    codigoError = ex.Number;
                }
            }
            catch (TimeoutException)
            {
                if (grabarFallidas == false)
                {
                    dtVentas.RejectChanges();
                }
            }
        }

        // constructor para grabar ediciones de registros
        public static void GrabarVentas(DataSet dtVentas, ref int? codigoError, 
            DataView viewDetalleOriginal, DataTable tblActual, bool grabarFallidas)
        {
            MySqlTransaction tr = null;
            try
            {
                if (grabarFallidas == false)
                {
                    DataSet dsRemoto = dtVentas.GetChanges();
                    MySqlConnection SqlConnection1 = DALBase.GetConnection();
                    SqlConnection1.Open();
                    tr = SqlConnection1.BeginTransaction();
                    DAL.VentasDAL.GrabarDB(dtVentas, SqlConnection1, tr);
                    DAL.VentasDetalleDAL.GrabarDB(dtVentas, SqlConnection1, tr);
                    tr.Commit();
                    SqlConnection1.Close();
                    lock (_sync)
                    {
                        Thread t = new Thread(() => bckWrk_DoWork(dsRemoto, tblActual, viewDetalleOriginal));
                        t.Start();
                    }
                }
                else // grabo las ventas que no se grabaron por falta de internet
                {
                    MySqlConnection SqlConnection1 = DALBase.GetRemoteConnection();
                    SqlConnection1.Open();
                    tr = SqlConnection1.BeginTransaction();
                    DAL.VentasDAL.GrabarDB(dtVentas, SqlConnection1, tr);
                    DAL.VentasDetalleDAL.GrabarDB(dtVentas, SqlConnection1, tr);
                    tr.Commit();
                    SqlConnection1.Close();                    
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1042) //no se pudo abrir la conexion por falta de internet
                {
                    if (grabarFallidas == false)
                    {
                        dtVentas.RejectChanges();
                    }
                    codigoError = 1042;
                }
                else
                {
                    if (grabarFallidas == false)
                    {
                        dtVentas.RejectChanges();
                    }
                    if (tr != null)
                    {
                        tr.Rollback();
                    }
                    codigoError = ex.Number;
                }
            }
        }

        // borrar ventas desde el arqueo de caja
        public static void BorrarVentasByPK(int PK, ref int? codigoError, bool borrarRemotas)
        {
            try
            {
                DAL.VentasDAL.BorrarByPK(PK, borrarRemotas);
                if (borrarRemotas == false)
                {
                    lock (_sync)
                    {
                        Thread t = new Thread(() => bckWrk_borrarVentas_DoWork(PK));
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

        // hilo para inserciones remotas
        public static void bckWrk_DoWork(DataSet dsRemoto)
        {
            MySqlTransaction tr = null;
            try
            {
                MySqlConnection SqlConnection1 = DALBase.GetRemoteConnection();
                SqlConnection1.Open();
                tr = SqlConnection1.BeginTransaction();
                DAL.VentasDAL.GrabarDB(dsRemoto, SqlConnection1, tr);
                DAL.VentasDetalleDAL.GrabarDB(dsRemoto, SqlConnection1, tr);
                tr.Commit();
                SqlConnection1.Close();
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1042 || ex.Number == 0) //no se pudo abrir la conexion por falta de internet o timeout expired
                {
                    Fallidas_bckWrk(dsRemoto);
                }
                else
                {
                    if (tr != null)
                    {
                        tr.Rollback();
                    }
                }
            }
            catch (TimeoutException)
            {
                Fallidas_bckWrk(dsRemoto);
            }
        }

        // hilo para ediciones remotas
        public static void bckWrk_DoWork(DataSet dsRemoto, DataTable tablaActual, DataView viewOriginal)
        {
            MySqlTransaction tr = null;
            try
            {
                MySqlConnection SqlConnection1 = DALBase.GetRemoteConnection();
                SqlConnection1.Open();
                tr = SqlConnection1.BeginTransaction();
                DAL.VentasDAL.GrabarDB(dsRemoto, SqlConnection1, tr);
                DAL.VentasDetalleDAL.GrabarDB(dsRemoto, SqlConnection1, tr);
                tr.Commit();
                SqlConnection1.Close();
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1042 || ex.Number == 0) //no se pudo abrir la conexion por falta de internet o timeout expired
                {
                    Fallidas_bckWrk(dsRemoto, tablaActual, viewOriginal);
                }
                else
                {
                    if (tr != null)
                    {
                        tr.Rollback();
                    }
                }
            }
            catch (TimeoutException)
            {
                Fallidas_bckWrk(dsRemoto, tablaActual, viewOriginal);
            }
        }

        // trata fallidas hilo inserciones remotas
        private static void Fallidas_bckWrk(DataSet dsRemoto)
        {            
            string estado = dsRemoto.Tables[0].Rows[0].RowState.ToString();
            if (estado == "Added") // estoy insertando venta
            {
                MySqlConnection SqlConnection1 = DALBase.GetConnection();
                int PK = Convert.ToInt32(dsRemoto.Tables[0].Rows[0][0].ToString());
                DataTable tblVentasFallidas = new DataTable();
                tblVentasFallidas.TableName = "VentasFallidas";
                tblVentasFallidas.Columns.Add("Id", typeof(int));
                tblVentasFallidas.Columns.Add("Accion", typeof(string));
                DataRow row = tblVentasFallidas.NewRow();
                row["Id"] = PK;
                row["Accion"] = estado;
                tblVentasFallidas.Rows.Add(row);
                DataSet ds = new DataSet();
                ds.Tables.Add(tblVentasFallidas);
                DAL.FallidasDAL.GrabarVentasFallidas(ds, SqlConnection1);
                SqlConnection1.Close();
            }
        }

        // trata fallidas hilo ediciones remotas
        private static void Fallidas_bckWrk(DataSet dsRemoto, DataTable tablaActual, DataView viewOriginal)
        {
            string estado = dsRemoto.Tables[0].Rows[0].RowState.ToString();
            if (estado != "Added")  //estoy editando venta
            {
                int PK = Convert.ToInt32(dsRemoto.Tables[0].Rows[0][0].ToString());
                // No falló la inserción anterior y va haber que editarla en el remote server
                if (DAL.FallidasDAL.ExisteVentaFallida(PK, "Added") == false)
                {
                    MySqlConnection SqlConnection1 = DALBase.GetConnection();                    
                    DataTable tblVentasDetalleFallidas = new DataTable();
                    tblVentasDetalleFallidas.TableName = "VentasDetalleFallidas";
                    tblVentasDetalleFallidas.Columns.Add("Id", typeof(int));
                    tblVentasDetalleFallidas.Columns.Add("IdVenta", typeof(int));
                    tblVentasDetalleFallidas.Columns.Add("Accion", typeof(string));
                    foreach (DataRow rowDetalle in tablaActual.Rows)
                    {
                        int PkDetalle = Convert.ToInt32(rowDetalle["Id"].ToString());
                        if (rowDetalle["Accion"].ToString() == "Added")
                        {
                            DataRow rowFallidas = tblVentasDetalleFallidas.NewRow();
                            rowFallidas["Id"] = PkDetalle;
                            rowFallidas["IdVenta"] = PK;
                            rowFallidas["Accion"] = "Added";
                            tblVentasDetalleFallidas.Rows.Add(rowFallidas);
                        }
                        else if (rowDetalle["Accion"].ToString() == "Modified")
                        {
                            //si no existe una ventaDetalle 'Modified' o 'Added' con la misma pK la agrego a VentasDetalleFallidas'
                            if (DAL.FallidasDAL.ExisteVentaDetalleFallida(PkDetalle, "Modified") == false)
                            {
                                if (DAL.FallidasDAL.ExisteVentaDetalleFallida(PkDetalle, "Added") == false)
                                {
                                    DataRow rowFallidas = tblVentasDetalleFallidas.NewRow();
                                    rowFallidas["Id"] = PkDetalle;
                                    rowFallidas["IdVenta"] = PK;
                                    rowFallidas["Accion"] = "Modified";
                                    tblVentasDetalleFallidas.Rows.Add(rowFallidas);
                                }
                            }
                        }
                    }
                    foreach (DataRowView rowDetalle in viewOriginal)  // busco filas borradas en tblVentasDetalle
                    {
                        DataRow[] foundRow = tablaActual.Select("Id = '" + rowDetalle["IdDVEN"].ToString() + "'");
                        if (foundRow.Length == 0)
                        {
                            int PkDetalle = Convert.ToInt32(rowDetalle["IdDVEN"].ToString());
                            //si existe una ventaDetalle 'Added' con la misma PkDetalle la borro y nada mas, si no existe
                            //agrego una 'Deleted' a VentasDetalleFallidas
                            if (DAL.FallidasDAL.ExisteVentaDetalleFallida(PkDetalle, "Added"))
                            {
                                DAL.FallidasDAL.BorrarDetalleFallidas(PkDetalle, "Added");
                            }
                            else if (DAL.FallidasDAL.ExisteVentaDetalleFallida(PkDetalle, "Modified"))
                            {
                                DAL.FallidasDAL.BorrarDetalleFallidas(PkDetalle, "Modified");
                                DataRow rowFallidas = tblVentasDetalleFallidas.NewRow();
                                rowFallidas["Id"] = PkDetalle;
                                rowFallidas["IdVenta"] = PK;
                                rowFallidas["Accion"] = "Deleted";
                                tblVentasDetalleFallidas.Rows.Add(rowFallidas);
                            }
                            else
                            {
                                DataRow rowFallidas = tblVentasDetalleFallidas.NewRow();
                                rowFallidas["Id"] = PkDetalle;
                                rowFallidas["IdVenta"] = PK;
                                rowFallidas["Accion"] = "Deleted";
                                tblVentasDetalleFallidas.Rows.Add(rowFallidas);
                            }
                        }
                    }
                    DataSet ds = new DataSet();
                    ds.Tables.Add(tblVentasDetalleFallidas);
                    if (ds.HasChanges())
                    {
                        DAL.FallidasDAL.GrabarVentasDetalleFallidas(ds, SqlConnection1);
                    }
                    SqlConnection1.Close();
                }
            }
        }

        // hilo para borrar registros remotos
        public static void bckWrk_borrarVentas_DoWork(int pK)
        {
            try
            {
                bool borrarRemotas = true;
                DAL.VentasDAL.BorrarByPK(pK, borrarRemotas);
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

        // trata fallidas hilo borrar registros remotos
        private static void Fallidas_bckWrk_borrar(int pK)
        {
            //si existe una venta 'Added' con la misma pK que la venta borrada, borro la venta 'Added'
            if (DAL.FallidasDAL.ExisteVentaFallida(pK, "Added"))
            {
                DAL.FallidasDAL.BorrarVentaFallidas(pK, "Added");
            }
            //si existe una venta 'Modified' con la misma pK que la venta borrada, borro la venta 'Modified' y agrego la venta 'Deleted'
            else if (DAL.FallidasDAL.ExisteVentaFallida(pK, "Modified"))
            {
                DAL.FallidasDAL.BorrarVentaFallidas(pK, "Modified");
                MySqlConnection SqlConnection1 = DALBase.GetConnection();
                DataTable tblVentasFallidas = new DataTable();
                tblVentasFallidas.TableName = "VentasFallidas";
                tblVentasFallidas.Columns.Add("Id", typeof(int));
                tblVentasFallidas.Columns.Add("Accion", typeof(string));
                DataRow row = tblVentasFallidas.NewRow();
                row["Id"] = pK;
                row["Accion"] = "Deleted";
                tblVentasFallidas.Rows.Add(row);
                DataSet ds = new DataSet();
                ds.Tables.Add(tblVentasFallidas);
                DAL.FallidasDAL.GrabarVentasFallidas(ds, SqlConnection1);
                SqlConnection1.Close();
            }
            else
            {
                MySqlConnection SqlConnection1 = DALBase.GetConnection();
                DataTable tblVentasFallidas = new DataTable();
                tblVentasFallidas.TableName = "VentasFallidas";
                tblVentasFallidas.Columns.Add("Id", typeof(int));
                tblVentasFallidas.Columns.Add("Accion", typeof(string));
                DataRow row = tblVentasFallidas.NewRow();
                row["Id"] = pK;
                row["Accion"] = "Deleted";
                tblVentasFallidas.Rows.Add(row);
                DataSet ds = new DataSet();
                ds.Tables.Add(tblVentasFallidas);
                DAL.FallidasDAL.GrabarVentasFallidas(ds, SqlConnection1);
                SqlConnection1.Close();
                DAL.FallidasDAL.BorrarDetalleFallidasByVenta(pK);
            }
        }

    }
}
