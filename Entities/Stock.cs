using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public class Stock
    {
        private string _idArticulo;
        private int? _idLocal; 
        private int? _cantidad;
                
        public string IdArticulo { get { return _idArticulo; } set { _idArticulo = value; } }
        public int? IdLocal { get { return _idLocal; } set { _idLocal = value; } }
        public int? Cantidad { get { return _cantidad; } set { _cantidad = value; } }

        public Stock()
        {
        }
    }
}
