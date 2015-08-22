using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public class VentasDetalle
    {
        private int? _idDetalle;
        private int? _idVenta;
        private string _articulo;
        private int? _cantidad;
        private double? _publico;
        private double? _costo;
        private double? _mayor;
        private int? _idFormaPago;
        private int? _nroCupon;
        private int? _nroFactura;
        private int? _idEmpleado;
        private bool _liquidado;
        private bool _devolucion;


        public int? IdDetalle { get { return _idDetalle; } set { _idDetalle = value; } }
        public int? IdVenta { get { return _idVenta; } set { _idVenta = value; } }
        public string Articulo { get { return _articulo; } set { _articulo = value; } }
        public int? Cantidad { get { return _cantidad; } set { _cantidad = value; } }
        public double? Publico { get { return _publico; } set { _publico = value; } }
        public double? Costo { get { return _costo; } set { _costo = value; } }
        public double? Mayor { get { return _mayor; } set { _mayor = value; } }
        public int? IdFormaPago { get { return _idFormaPago; } set { _idFormaPago = value; } }
        public int? NroCupon { get { return _nroCupon; } set { _nroCupon = value; } }
        public int? NroFactura { get { return _nroFactura; } set { _nroFactura = value; } }
        public int? IdEmpleado { get { return _idEmpleado; } set { _idEmpleado = value; } }
        public bool Liquidado { get { return _liquidado; } set { _liquidado = value; } }
        public bool Devolucion { get { return _liquidado; } set { _devolucion = value; } }

        public VentasDetalle()
        {

        }
    }
}
