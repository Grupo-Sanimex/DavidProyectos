using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Dominio.Models.Producto
{
    public class ProductoCliente
    {
        // modelo datos del cliente
        public string Nombre_Completo { get; set; }
        public string Clasifica { get; set; }
        public string descRecoge { get; set; }
        public string descContado { get; set; }
        public float precio_Final { get; set; }
        public float PrecioMetroProducto { get; set; }

        // modelo datos del Metros_Importe_Mayoreo
        public double ACTUAL_IMP { get; set; }
        public double ACTUAL_MT { get; set; }
    }
}
