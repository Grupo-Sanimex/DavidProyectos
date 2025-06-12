using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Dominio.Models.Producto
{
    public class ProductoUnitario
    {
        // modelo disponibilidad
        public List<MDisponibilidadMayoreo> disponibles { get; set; }
        // modelo de datos del producto
        public string Code { get; set; }
        public string Description { get; set; }
        public List<string> Images { get; set; }
        public string Weight { get; set; }
        public float PrecioProducto { get; set; }
        public float PrecioMetroProducto { get; set; }
        public string Color { get; set; }
        public string SquareMeter { get; set; }
        public string Classification { get; set; }
        public string Proveedor { get; set; }
    }
}
