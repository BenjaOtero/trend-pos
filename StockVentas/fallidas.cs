using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Timers;
using System.ComponentModel;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using BL;

namespace StockVentas
{
    public class Fallidas
    {
        private static System.Timers.Timer aTimer;
        static BackgroundWorker bckWrk;
        DataSet ds;
        DataSet dsVentas;
        DataTable tblVentas;
        DataTable tblVentasDetalle;
        DataTable tblVentasFallidas;
        DataTable tblVentasDetalleFallidas;
        DataTable tblTesoreriaFallidas;
        DataTable tblFondoFallidas;
        DataTable tblClienteFallidas;
        private int? codigoError = null;
        int idVenta;

        public Fallidas()
        {
            bckWrk = new BackgroundWorker();
            bckWrk.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bckWrk_DoWork);
            bckWrk.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bckWrk_RunWorkerCompleted);
            aTimer = new System.Timers.Timer(10000);
            aTimer.Elapsed += new ElapsedEventHandler(GrabarFallidas);
            aTimer.Enabled = true;
        }

        private static void GrabarFallidas(object source, ElapsedEventArgs e)
        {
            aTimer.Enabled = false;
            bckWrk.RunWorkerAsync();            
        }

        public void bckWrk_DoWork(object sender, DoWorkEventArgs e)
        {
            if (BL.Utilitarios.HayInternet()) // si hay conexión a internet
            {
                #region ventas
                //
                //borrar ventas
                //
                DataTable tbl = BL.FallidasBLL.VentasGetByAccion("Deleted");
                if (tbl.Rows.Count > 0)
                {
                    //borro las ventas que no se borraron en el servidor remoto por falta de internet
                    BL.VentasBLL.BorrarByPK(tbl, ref codigoError);
                }
                //
                // agregar ventas
                //
                tbl = BL.FallidasBLL.VentasGetByAccion("Added");
                if (tbl.Rows.Count > 0)
                {
                    ds = BL.VentasBLL.CrearDatasetVentas(0);
                    dsVentas = ds.Clone(); // obtengo la estructura de las tablas ventas y ventasDetalle
                    tblVentas = dsVentas.Tables[0];
                    tblVentasDetalle = dsVentas.Tables[1];
                    tblVentas.TableName = "Ventas";
                    tblVentasDetalle.TableName = "VentasDetalle";
                    foreach (DataRow row in tbl.Rows) // obtengo las ventas que no se insertaron en el remote server para luego insert remoto
                    {
                        idVenta = Convert.ToInt32(row[0].ToString());
                        ds = BL.VentasBLL.CrearDatasetVentas(idVenta);
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            tblVentasFallidas = ds.Tables[0];
                            tblVentasDetalleFallidas = ds.Tables[1];
                            DataRow rwVentasFallidas = tblVentasFallidas.Rows[0];
                            tblVentas.ImportRow(rwVentasFallidas);
                            foreach (DataRow rwVentasDetalleFallidas in tblVentasDetalleFallidas.Rows)
                            {
                                tblVentasDetalle.ImportRow(rwVentasDetalleFallidas);
                            }
                        }
                    }
                    dsVentas.AcceptChanges();
                    foreach (DataRow rowVentas in tblVentas.Rows)
                    {
                        rowVentas.SetAdded();
                    }
                    foreach (DataRow rowDetalle in tblVentasDetalle.Rows)
                    {
                        rowDetalle.SetAdded();
                    }
                    if (tblVentas.GetChanges() != null || tblVentasDetalle.GetChanges() != null)
                    {
                        bool grabarFallidas = true;
                        BL.TransaccionesBLL.GrabarVentas(dsVentas, ref codigoError, grabarFallidas);                        
                    }
                }

                #endregion

                #region ventasDetalle

                //
                // borrar
                //
                tbl = BL.FallidasBLL.VentasDetalleGetByAccion("Deleted");
                if (tbl.Rows.Count > 0)
                {
                    BL.VentasDetalleBLL.BorrarByPK(tbl, ref codigoError);                    
                }
                //
                // agregar
                //
                tbl = BL.FallidasBLL.VentasDetalleGetByAccion("Added");
                if (tbl.Rows.Count > 0)
                {
                    ds = BL.VentasDetalleBLL.GetSchema(0);
                    dsVentas = ds.Clone(); // obtengo la estructura de las tablas ventasDetalle
                    tblVentasDetalle = dsVentas.Tables[1];
                    tblVentasDetalle.TableName = "VentasDetalle";
                    foreach (DataRow row in tbl.Rows) // obtengo las ventas que no se insertaron en el remote server para luego insert remoto
                    {
                        idVenta = Convert.ToInt32(row[0].ToString());
                        // obtengo las ventasDetalle que no se insertaron en el remote server para luego insert remoto
                        ds = BL.VentasDetalleBLL.GetFallidas(idVenta);
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            foreach (DataRow rwVentasDetalleFallidas in ds.Tables[0].Rows)
                            {
                                tblVentasDetalle.ImportRow(rwVentasDetalleFallidas);
                            }
                        }
                    }
                    foreach (DataRow rowDetalle in tblVentasDetalle.Rows)
                    {
                        rowDetalle.SetAdded();
                    }
                    if (dsVentas.HasChanges())
                    {
                        BL.VentasDetalleBLL.InsertFallidasRemoteServer(dsVentas, ref codigoError);
                    }
                }
                //
                // editar
                //
                tbl = BL.FallidasBLL.VentasDetalleGetByAccion("Modified");
                if (tbl.Rows.Count > 0)
                {
                    ds = BL.VentasDetalleBLL.GetSchema(0);
                    dsVentas = ds.Clone(); // obtengo la estructura de las tablas ventasDetalle
                    tblVentasDetalle = dsVentas.Tables[1];
                    tblVentasDetalle.TableName = "VentasDetalle";
                    foreach (DataRow row in tbl.Rows) // obtengo las ventas que no se insertaron en el remote server para luego insert remoto
                    {
                        idVenta = Convert.ToInt32(row[0].ToString());
                        // obtengo las ventasDetalle que no se insertaron en el remote server para luego insert remoto
                        ds = BL.VentasDetalleBLL.GetFallidas(idVenta);
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            foreach (DataRow rwVentasDetalleFallidas in ds.Tables[0].Rows)
                            {
                                tblVentasDetalle.ImportRow(rwVentasDetalleFallidas);
                            }
                        }
                    }
                    foreach (DataRow rowDetalle in tblVentasDetalle.Rows)
                    {
                        rowDetalle.SetModified();
                    }
                    if (dsVentas.HasChanges())
                    {
                        BL.VentasDetalleBLL.EditFallidasRemoteServer(dsVentas, ref codigoError);                        
                    }
                }

                #endregion

                #region TesoreriaMovimientos
                //
                //borrar TesoreriaMovimientos
                //
                tbl = BL.FallidasBLL.TesoreriaGetByAccion("Deleted");
                if (tbl.Rows.Count > 0)
                {
                    //borro las movimientos de tesoreria que no se borraron en el servidor remoto por falta de internet
                    BL.TesoreriaMovimientosBLL.BorrarByPK(tbl, ref codigoError);
                }
                //
                // agregar TesoreriaMovimientos
                //
                tbl = BL.FallidasBLL.TesoreriaGetByAccion("Added");
                if (tbl.Rows.Count > 0)
                {
                    ds = BL.TesoreriaMovimientosBLL.CrearDataset();
                    DataSet dsTesoreria = ds.Clone(); // obtengo la estructura de la tabla TesoreriaMovimientos
                    DataTable tblTesoreria = dsTesoreria.Tables[0];
                    tblTesoreria.TableName = "TesoreriaMovimientos";
                    foreach (DataRow row in tbl.Rows) // obtengo las ventas que no se insertaron en el remote server para luego insert remoto
                    {
                        int idMov = Convert.ToInt32(row[0].ToString());
                        ds = BL.TesoreriaMovimientosBLL.CrearDatasetMovimiento(idMov);
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            tblTesoreriaFallidas = ds.Tables[0];
                            DataRow rwTesoreriaFallidas = tblTesoreriaFallidas.Rows[0];
                            tblTesoreria.ImportRow(rwTesoreriaFallidas);
                        }
                    }
                    dsTesoreria.AcceptChanges();
                    foreach (DataRow rowTesoreria in tblTesoreria.Rows)
                    {
                        rowTesoreria.SetAdded();
                    }
                    if (tblTesoreria.GetChanges() != null)
                    {
                        bool grabarFallidas = true;
                        BL.TesoreriaMovimientosBLL.InsertFallidasRemoteServer(dsTesoreria, ref codigoError, grabarFallidas);
                    }
                }
                //
                // editar TesoreriaMovimientos
                //
                tbl = BL.FallidasBLL.TesoreriaGetByAccion("Modified");
                if (tbl.Rows.Count > 0)
                {
                    ds = BL.TesoreriaMovimientosBLL.CrearDataset();
                    DataSet dsTesoreria = ds.Clone(); // obtengo la estructura de la tabla TesoreriaMovimientos
                    DataTable tblTesoreria = dsTesoreria.Tables[0];
                    tblTesoreria.TableName = "TesoreriaMovimientos";
                    foreach (DataRow row in tbl.Rows) // obtengo las ventas que no se insertaron en el remote server para luego insert remoto
                    {
                        int idMov = Convert.ToInt32(row[0].ToString());
                        ds = BL.TesoreriaMovimientosBLL.CrearDatasetMovimiento(idMov);
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            tblTesoreriaFallidas = ds.Tables[0];
                            DataRow rwTesoreriaFallidas = tblTesoreriaFallidas.Rows[0];
                            tblTesoreria.ImportRow(rwTesoreriaFallidas);
                        }
                    }
                    dsTesoreria.AcceptChanges();
                    foreach (DataRow rowTesoreria in tblTesoreria.Rows)
                    {
                        rowTesoreria.SetModified();
                    }
                    if (tblTesoreria.GetChanges() != null)
                    {
                        bool grabarFallidas = true;
                        BL.TesoreriaMovimientosBLL.EditFallidasRemoteServer(dsTesoreria, ref codigoError, grabarFallidas);
                    }
                }
                #endregion

                #region FondoCaja
                //
                // agregar FondoCaja
                //
                tbl = BL.FallidasBLL.FondoCajaGetByAccion("Added");
                if (tbl.Rows.Count > 0)
                {
                    ds = BL.FondoCajaBLL.CrearDataset();
                    DataSet dsFondoCaja = ds.Clone(); // obtengo la estructura de la tabla 
                    DataTable tblFondoCaja = dsFondoCaja.Tables[0];
                    tblFondoCaja.TableName = "FondoCaja";
                    foreach (DataRow row in tbl.Rows) // obtengo las ventas que no se insertaron en el remote server para luego insert remoto
                    {
                        int idMov = Convert.ToInt32(row[0].ToString());
                        ds = BL.FondoCajaBLL.GetByPk(idMov);
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            tblFondoFallidas = ds.Tables[0];
                            DataRow rwFondoFallidas = tblFondoFallidas.Rows[0];
                            tblFondoCaja.ImportRow(rwFondoFallidas);
                        }
                    }
                    dsFondoCaja.AcceptChanges();
                    foreach (DataRow rowFondo in tblFondoCaja.Rows)
                    {
                        rowFondo.SetAdded();
                    }
                    if (tblFondoCaja.GetChanges() != null)
                    {
                        bool grabarFallidas = true;
                        BL.FondoCajaBLL.InsertFallidasRemoteServer(dsFondoCaja, ref codigoError, grabarFallidas);
                    }
                }
                //
                // editar FondoCaja
                //
                tbl = BL.FallidasBLL.FondoCajaGetByAccion("Modified");
                if (tbl.Rows.Count > 0)
                {
                    ds = BL.FondoCajaBLL.CrearDataset();
                    DataSet dsFondoCaja = ds.Clone(); // obtengo la estructura de la tabla TesoreriaMovimientos
                    DataTable tblFondoCaja = dsFondoCaja.Tables[0];
                    tblFondoCaja.TableName = "FondoCaja";
                    foreach (DataRow row in tbl.Rows) // obtengo las ventas que no se insertaron en el remote server para luego insert remoto
                    {
                        int idMov = Convert.ToInt32(row[0].ToString());
                        ds = BL.FondoCajaBLL.GetByPk(idMov);
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            tblFondoFallidas = ds.Tables[0];
                            DataRow rwFondoFallidas = tblFondoFallidas.Rows[0];
                            tblFondoCaja.ImportRow(rwFondoFallidas);
                        }
                    }
                    dsFondoCaja.AcceptChanges();
                    foreach (DataRow rowFondo in tblFondoCaja.Rows)
                    {
                        rowFondo.SetModified();
                    }
                    if (tblFondoCaja.GetChanges() != null)
                    {
                        bool grabarFallidas = true;
                        BL.FondoCajaBLL.EditFallidasRemoteServer(dsFondoCaja, ref codigoError, grabarFallidas);
                    }
                }
                #endregion

                #region Clientes
                // la inserción de registros fallidos en el remote server y el borrado de registros de la tabla local "clientesfallidas",
                // se hace toda através de ClientesBLL y ClientesDAL no se usa
                // como en ventas, ventasDetalle, TesoreriaMovimientos y FondoCaja las clases FallidasBLL y FallidasDAL
                //
                //borrar Clientes
                //
                tbl = BL.ClientesBLL.ClienteGetByAccion("Delete"); // OJO !!! que las otras tablas locales fallidas guardan 'Modified', 'Deleted', 'Added'
                if (tbl.Rows.Count > 0)
                {
                    //borro los clientes que no se borraron en el servidor remoto por falta de internet
                    BL.ClientesBLL.BorrarByPK(tbl, ref codigoError);
                }
                //
                // agregar Clientes
                //
                tbl = BL.ClientesBLL.ClienteGetByAccion("Add");// OJO !!! que las otras tablas locales fallidas guardan 'Modified', 'Deleted', 'Added'
                if (tbl.Rows.Count > 0)
                {
                    ds = BL.ClientesBLL.GetClientes();
                    DataSet dsCliente = ds.Clone(); // obtengo la estructura de la tabla Clientes
                    DataTable tblCliente = dsCliente.Tables[0];
                    tblCliente.TableName = "Clientes";
                    foreach (DataRow row in tbl.Rows) // obtengo los clientes que no se insertaron en el remote server para luego insert remoto
                    {
                        int idCliente = Convert.ToInt32(row[0].ToString());
                        ds = BL.ClientesBLL.GetRegistroFallido(idCliente);
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            tblClienteFallidas = ds.Tables[0];
                            DataRow rwClienteFallidas = tblClienteFallidas.Rows[0];
                            tblCliente.ImportRow(rwClienteFallidas);
                        }
                    }
                    dsCliente.AcceptChanges();
                    foreach (DataRow rowCliente in tblCliente.Rows)
                    {
                        rowCliente.SetAdded();
                    }
                    if (tblCliente.GetChanges() != null)
                    {
                        bool grabarFallidas = true;
                        BL.ClientesBLL.GrabarFallidasRemoteServer(dsCliente, ref codigoError, grabarFallidas, "Add");
                    }
                }
                //
                // editar Clientes
                //
                tbl = BL.ClientesBLL.ClienteGetByAccion("Change");// OJO !!! que las otras tablas locales fallidas guardan 'Modified', 'Deleted', 'Added'
                if (tbl.Rows.Count > 0)
                {
                    ds = BL.ClientesBLL.GetClientes();
                    DataSet dsCliente = ds.Clone(); // obtengo la estructura de la tabla Clientes
                    DataTable tblCliente = dsCliente.Tables[0];
                    tblCliente.TableName = "Clientes";
                    foreach (DataRow row in tbl.Rows) // obtengo los clientes que no se insertaron en el remote server para luego insert remoto
                    {
                        int idCliente = Convert.ToInt32(row[0].ToString());
                        ds = BL.ClientesBLL.GetRegistroFallido(idCliente);
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            tblClienteFallidas = ds.Tables[0];
                            DataRow rwClienteFallidas = tblClienteFallidas.Rows[0];
                            tblCliente.ImportRow(rwClienteFallidas);
                        }
                    }
                    dsCliente.AcceptChanges();
                    foreach (DataRow rowCliente in tblCliente.Rows)
                    {
                        rowCliente.SetModified();
                    }
                    if (tblCliente.GetChanges() != null)
                    {
                        bool grabarFallidas = true;
                        BL.ClientesBLL.GrabarFallidasRemoteServer(dsCliente, ref codigoError, grabarFallidas, "Change");
                    }
                }
                #endregion
            }
        }

        private void bckWrk_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            aTimer.Enabled = true;
            aTimer.Interval = 600000;
        }

            
    }
}
