using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public class Clientes
    {
        private int? _idCliente;
        private string _razonSocial;
        private string _cuit;
        private string _direccion;
        private string _localidad;
        private string _provincia;
        private string _transporte;
        private string _contacto;        
        private string _telefono;
        private string _movil;

        public int? IdCliente { get { return _idCliente; } set { _idCliente = value; } }
        public string RazonSocial { get { return _razonSocial; } set { _razonSocial = value; } }
        public string Cuit { get { return _cuit; } set { _cuit = value; } }
        public string Direccion { get { return _direccion; } set { _direccion = value; } }
        public string Localidad { get { return _localidad; } set { _localidad = value; } }
        public string Provincia { get { return _provincia; } set { _provincia = value; } }
        public string Transporte { get { return _transporte; } set { _transporte = value; } }
        public string Contacto { get { return _contacto; } set { _contacto = value; } }
        public string Telefono { get { return _telefono; } set { _telefono = value; } }
        public string Movil { get { return _movil; } set { _movil = value; } }


        public Clientes()
        {
        }
    }
}
