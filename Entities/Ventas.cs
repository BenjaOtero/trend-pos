using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public class Ventas
    {
        private int? _idVenta;
        private int? _idPc;
        private DateTime? _fecha;
        private int? _idCliente;

        public int? IdVenta { get { return _idVenta; } set { _idVenta = value; } }
        public int? IdPc { get { return _idPc; } set { _idPc = value; } }
        public DateTime? Fecha { get { return _fecha; } set { _fecha = value; } }
        public int? IdCliente { get { return _idCliente; } set { _idCliente = value; } }

        public Ventas()
        {

        }
    }
}
