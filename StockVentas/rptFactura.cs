using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace StockVentas
{
    public partial class rptFactura : Form
    {
        DataTable tblIVA;
        DataRow[] foundCliente;

        public rptFactura(DataTable tblIVA, DataRow[] foundCliente)
        {
            InitializeComponent();
            this.tblIVA = tblIVA;
            this.foundCliente = foundCliente;
        }

        private void rptFactura_Load(object sender, EventArgs e)
        {
            string cabeceraCbteTipo;
            string codigoComprobante;
            string condicionIva = foundCliente[0]["CondicionIvaCLI"].ToString();
            string cuit = foundCliente[0]["CUIT"].ToString();
            string razonSocial = foundCliente[0]["RazonSocialCLI"].ToString();
            string direccion = foundCliente[0]["RazonSocialCLI"].ToString();
            string localidad = foundCliente[0]["RazonSocialCLI"].ToString();
            string provincia = foundCliente[0]["ProvinciaCLI"].ToString();
            if (condicionIva == "RESPONSABLE INSCRIPTO")
            {
                cabeceraCbteTipo = "A";
                codigoComprobante = "COD.01";
            }
            else
            {
                cabeceraCbteTipo = "B";
                codigoComprobante = "COD.06";
            }
       /*     ReportParameter[] parametros = new ReportParameter[8];
            parametros[0] = new ReportParameter("cabeceraCbteTipo", cabeceraCbteTipo);
            parametros[1] = new ReportParameter("condicionIvaCliente", condicionIva);
            parametros[2] = new ReportParameter("cuit", cuit);
            parametros[3] = new ReportParameter("razonSocial", razonSocial);
            parametros[4] = new ReportParameter("direccion", razonSocial);
            parametros[5] = new ReportParameter("razonSocial", razonSocial);
            parametros[6] = new ReportParameter("razonSocial", razonSocial);
            parametros[7] = new ReportParameter("codigoComprobante", codigoComprobante);*/


            this.reportViewer1.ProcessingMode = ProcessingMode.Local;
            string path = Application.StartupPath + @"\Informes\factura.rdlc";
            this.reportViewer1.LocalReport.ReportPath = path;
            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", tblIVA));
          //  this.reportViewer1.LocalReport.SetParameters(parametros);
            this.reportViewer1.RefreshReport();
            this.HorizontalScroll.Enabled = false;
        }
    }
}
