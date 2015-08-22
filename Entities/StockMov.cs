using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public class StockMov
    {
        private int? _idMov;
        private DateTime? _fecha;
        private int? _origen; 
        private int? _destino;

        public int? IdMovM { get { return _idMov; } set { _idMov = value; } }
        public DateTime? Fecha { get { return _fecha; } set { _fecha = value; } }
        public int? Origen { get { return _origen; } set { _origen = value; } }
        public int? Destino { get { return _destino; } set { _destino = value; } }

        public StockMov()
        {

        }
    }
}
