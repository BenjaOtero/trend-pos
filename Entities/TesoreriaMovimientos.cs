using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public class TesoreriaMovimientos
    {
        private int? _idMov;
        private DateTime _fecha;
        private int? _idPc;
        private string _detalle;
        private double _importe;

        public int? IdMov { get { return _idMov; } set { _idMov = value; } }
        public DateTime Fecha { get { return _fecha; } set { _fecha = value; } }
        public int? IdPc { get { return _idPc; } set { _idPc = value; } }
        public string Detalle { get { return _detalle; } set { _detalle = value; } }
        public double Importe { get { return _importe; } set { _importe = value; } }
        
        public TesoreriaMovimientos()
        {
        }
    }
}
