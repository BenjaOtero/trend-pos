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

        private void salirToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void frmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
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

    }
}
