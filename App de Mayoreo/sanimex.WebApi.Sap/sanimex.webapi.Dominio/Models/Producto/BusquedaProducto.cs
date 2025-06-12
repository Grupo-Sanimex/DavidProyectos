using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Dominio.Models.Producto
{
    public class BusquedaProducto
    {
        public string Producto { get; set; }
        public List<string> Images { get; set; }

    }
}
