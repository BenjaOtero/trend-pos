using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public class Articulos
    {
        private string _idArticulo;
        private int? _idItem;
        private int? _idColor;
        private string _talle;
        private int? _idProveedor;
        private string _descripcion;
        private string _descripcionWeb;
        private decimal? _precioCosto;
        private decimal? _precioPublico;
        private decimal? _precioMayor;
        private DateTime? _fecha;
        private string _imagen;

        public string IdArticulo { get { return _idArticulo; } set { _idArticulo = value; } }
        public int? IdItem { get { return _idItem; } set { _idItem = value; } }
        public int? IdColor { get { return _idColor; } set { _idColor = value; } }
        public string Talle { get { return _talle; } set { _talle = value; } }
        public int? IdProveedor { get { return _idProveedor; } set { _idProveedor = value; } }
        public string Descripcion { get { return _descripcion; } set { _descripcion = value; } }
        public string DescripcionWeb { get { return _descripcionWeb; } set { _descripcionWeb = value; } }
        public decimal? PrecioCosto { get { return _precioCosto; } set { _precioCosto = value; } }
        public decimal? PrecioPublico { get { return _precioPublico; } set { _precioPublico = value; } }
        public decimal? PrecioMayor { get { return _precioMayor; } set { _precioMayor = value; } }
        public DateTime? Fecha { get { return _fecha; } set { _fecha = value; } }
        public string Imagen { get { return _imagen; } set { _imagen = value; } }

        public Articulos()
        {
        }

    }
}
