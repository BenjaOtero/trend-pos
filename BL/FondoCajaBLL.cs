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
    public class FondoCajaBLL
    {
        private static object _sync = new object();

        public static DataSet CrearDataset()
        {
            DataSet dt = DAL.FondoCajaDAL.CrearDataset();
            return dt;
        }

        public static DataSet CrearDataset(string fecha, int idPc)
        {
            DataSet dt = DAL.FondoCajaDAL.CrearDataset(fecha, idPc);
            dt.Tables[1].TableName = "FondoCaja";
            dt.Tables[2].TableName = "TesoreriaMovimientos";
            return dt;
        }

        public static void GrabarDB(DataSet dt, ref int? codigoError, bool grabarFallidas)
        {
            MySqlTransaction tr = null;
            try
            {
                if (grabarFallidas == false)
                {
                    DataSet dsRemoto = dt.GetChanges();
                    MySqlConnection SqlConnection1 = DALBase.GetConnection();
                    SqlConnection1.Open();
                    DAL.FondoCajaDAL.GrabarDB(dt, SqlConnection1);
                    SqlConnection1.Close();
                    lock (_sync)
                    {
                        Thread t = new Thread(() => bckWrk_DoWork(dsRemoto));
                        t.Start();
                    }
                }
                else
                {
                    MySqlConnection SqlConnection1 = DALBase.GetRemoteConnection();
                    SqlConnection1.Open();
                    DAL.FondoCajaDAL.GrabarDB(dt, SqlConnection1);
                    SqlConnection1.Close();                
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1042) //no se pudo abrir la conexion por falta de internet
                {
                    dt.RejectChanges();
                    codigoError = 1042;
                }
                else
                {
                    dt.RejectChanges();
                    if (tr != null)
                    {
                        tr.Rollback();
                    }                    
                    codigoError = ex.Number;
                }
            }
        }

        public static void InsertFallidasRemoteServer(DataSet dt, ref int? codigoError, bool grabarFallidas)
        {
            MySqlTransaction tr = null;
            try
            {
                MySqlConnection SqlConnection1 = DALBase.GetRemoteConnection();
                SqlConnection1.Open();
                DAL.FondoCajaDAL.GrabarDB(dt, SqlConnection1);
                SqlConnection1.Close();
                BL.FallidasBLL.BorrarFondoCajaFallidasByAccion("Added");
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1042) //no se pudo abrir la conexion por falta de internet
                {
                    dt.RejectChanges();
                    codigoError = 1042;
                }
                else
                {
                    dt.RejectChanges();
                    if (tr != null)
                    {
                        tr.Rollback();
                    }
                    codigoError = ex.Number;
                }
            }
            catch (TimeoutException)
            {
            }
        }

        public static void EditFallidasRemoteServer(DataSet dt, ref int? codigoError, bool grabarFallidas)
        {
            MySqlTransaction tr = null;
            try
            {
                MySqlConnection SqlConnection1 = DALBase.GetRemoteConnection();
                SqlConnection1.Open();
                DAL.FondoCajaDAL.GrabarDB(dt, SqlConnection1);
                SqlConnection1.Close();
                BL.FallidasBLL.BorrarFondoCajaFallidasByAccion("Modified");
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1042) //no se pudo abrir la conexion por falta de internet
                {
                    dt.RejectChanges();
                    codigoError = 1042;
                }
                else
                {
                    dt.RejectChanges();
                    if (tr != null)
                    {
                        tr.Rollback();
                    }
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
                MySqlConnection SqlConnection1 = DALBase.GetRemoteConnection();
                SqlConnection1.Open();
                DAL.FondoCajaDAL.GrabarDB(dsRemoto, SqlConnection1);
                SqlConnection1.Close();
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
            int PK = Convert.ToInt32(dsRemoto.Tables[1].Rows[0][0].ToString());
            if (!DAL.FallidasDAL.ExisteFondoCajaFallida(PK, "Added"))
            {
                if (!DAL.FallidasDAL.ExisteFondoCajaFallida(PK, "Modified"))
                {
                    MySqlConnection SqlConnection1 = DALBase.GetConnection();
                    string estado = dsRemoto.Tables[1].Rows[0].RowState.ToString();
                    DataTable tblFondoCajaFallidas = new DataTable();
                    tblFondoCajaFallidas.TableName = "FondoCajaFallidas";
                    tblFondoCajaFallidas.Columns.Add("Id", typeof(int));
                    tblFondoCajaFallidas.Columns.Add("Accion", typeof(string));
                    DataRow row = tblFondoCajaFallidas.NewRow();
                    row["Id"] = PK;
                    row["Accion"] = estado;
                    tblFondoCajaFallidas.Rows.Add(row);
                    DataSet ds = new DataSet();
                    ds.Tables.Add(tblFondoCajaFallidas);
                    DAL.FallidasDAL.GrabarFondoCajaFallidas(ds, SqlConnection1);
                    SqlConnection1.Close();
                }
            }
        }

        public static DataSet GetByPk(int idMov)
        {
            DataSet ds = FondoCajaDAL.GetByPk(idMov);
            return ds;
        }
    }
}
