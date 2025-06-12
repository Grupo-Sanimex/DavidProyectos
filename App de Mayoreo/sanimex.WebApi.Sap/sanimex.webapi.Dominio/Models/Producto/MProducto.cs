using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Dominio.Models.Producto
{
    public class MProducto
    {
        public string? CodigoBarra { get; set; }
        public string? Descripcion { get; set; }
        public string? Proveedor { get; set; }
        public double Precio { get; set; }
        public List<string> Images { get; set; }
    }
}
