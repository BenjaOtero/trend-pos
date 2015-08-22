using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public class FondoCaja
    {
        private DateTime _fecha;
        private int? _idPc;
        private double _importe;

        public DateTime Fecha { get { return _fecha; } set { _fecha = value; } }
        public int? IdPc { get { return _idPc; } set { _idPc = value; } }
        public double Importe { get { return _importe; } set { _importe = value; } }

        public FondoCaja()
        {
        }
    }
}
