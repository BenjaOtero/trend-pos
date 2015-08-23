using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using BL;

namespace StockVentas
{
    public partial class frmArqueoInter : Form
    {

        public frmArqueoInter()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            dateTimeFecha.Value = DateTime.Today;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            frmArqueoCaja arqueo = new frmArqueoCaja(dateTimeFecha);
            arqueo.Show();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
