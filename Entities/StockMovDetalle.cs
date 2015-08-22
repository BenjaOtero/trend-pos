using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public class StockMovDetalle
    {
        private int? _id;
        private int? _idMov;
        private string _articulo;
        private int? _cantidad;

        public int? Id { get { return _id; } set { _id = value; } }
        public int? IdMovD { get { return _idMov; } set { _idMov = value; } }
        public string Articulo { get { return _articulo; } set { _articulo = value; } }
        public int? Cantidad { get { return _cantidad; } set { _cantidad = value; } }
         
        public StockMovDetalle()
        {
        }
    }
}
