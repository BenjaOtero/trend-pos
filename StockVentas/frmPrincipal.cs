using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BL;
using System.Diagnostics;
using System.Threading;
using System.Net;
using System.IO;

namespace StockVentas
{
    public partial class frmPrincipal : Form
    {
        frmInicio instanciaInicio;
        public frmProgress progreso;

        public frmPrincipal(frmInicio instanciaInicio)
        {
            InitializeComponent();
            foreach (Control control in this.Controls)
            {
                MdiClient client = control as MdiClient;
                if (!(client == null))
                {
                    client.BackColor = this.BackColor;
                    break;
                }
            }
            this.instanciaInicio = instanciaInicio;
            Fallidas fllds = new Fallidas();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            System.Drawing.Icon ico = Properties.Resources.icono_app;
            this.Icon = ico;
            actualizarDatosToolStripMenuItem.Visible = false;
        }

        private void ventasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            frmVentas newMDIChild = new frmVentas();
            newMDIChild.MdiParent = this;
            newMDIChild.Show();
            Cursor.Current = Cursors.Arrow;
        }

        private void movimientosDeTesoreríaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTesoreriaMov newMDIChild = new frmTesoreriaMov();
            newMDIChild.MdiParent = this;
            newMDIChild.Show();
        }

        private void fondoDeCajaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmFondoCajaPunto newMDIChild = new frmFondoCajaPunto();
            newMDIChild.MdiParent = this;
            newMDIChild.Show();
        }

        private void stockDeArtículosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmStockInter newMDIChild = new frmStockInter();
            newMDIChild.MdiParent = this;
            newMDIChild.Show();
        }       

        private void arqueoDeCajaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmArqueoInter newMDIChild = new frmArqueoInter();
            newMDIChild.MdiParent = this;
            newMDIChild.Show();
        }

        private void enPesosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmVentasPesosInter newMDIChild = new frmVentasPesosInter();
            newMDIChild.MdiParent = this;
            newMDIChild.Show();
        }

        private void ventasEnPesos_Click(object sender, EventArgs e)
        {
            frmVentasPesosInter newMDIChild = new frmVentasPesosInter();
            newMDIChild.MdiParent = this;
            newMDIChild.Show();
        }

        private void btnArticulos_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            frmArticulos articulos = new frmArticulos();
            articulos.Show();
            Cursor.Current = Cursors.Arrow;
        }

        private void actualizarDatos_Click(object sender, EventArgs e)
        {
            frmProgress newMDIChild = new frmProgress("ActualizarBD", "cargar");
            newMDIChild.MdiParent = this;
            newMDIChild.Show();
        }

        private void actualizarDatos_Click999(object sender, EventArgs e)
        {
            frmProgress newMDIChild = new frmProgress("ActualizarBD", "cargar");
            newMDIChild.MdiParent = this;
            newMDIChild.Show();
        }

        private void salirToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void frmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
            instanciaInicio.cerrando = true;
            instanciaInicio.Visible = true;
        }

        private void threadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(() => bckWrk_DoWork());
            t.Start();
        }

        public static void bckWrk_DoWork()
        {
            DataSet ds = BL.Utilitarios.ActualizarBD();
        }

        private void lotesTarjetasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmVentasLotesTarjetas newMDIChild = new frmVentasLotesTarjetas();
            newMDIChild.MdiParent = this;
            newMDIChild.Show();
        }

        private void clientesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmClientes newMDIChild = new frmClientes();
            newMDIChild.MdiParent = this;
            newMDIChild.Show();
        }

        private void DownloadFileFTP()
        {
            string inputfilepath = @"C:\Windows\Temp\datos.sql.gz";
            string ftphost = "trendsistemas.com";
            string ftpfilepath = "/datos/2147483647_datos.sql.gz";

            string ftpfullpath = "ftp://" + ftphost + ftpfilepath;

            using (WebClient request = new WebClient())
            {
                request.Credentials = new NetworkCredential("benja@trendsistemas.com", "8953#AFjn");
                byte[] fileData = request.DownloadData(ftpfullpath);

                using (FileStream file = File.Create(inputfilepath))
                {
                    file.Write(fileData, 0, fileData.Length);
                    file.Close();
                }
                MessageBox.Show("Download Complete");
            }
        }

        private void actualizarDatosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DownloadFileFTP();
            Cursor.Current = Cursors.Arrow;
        }

    }
}
