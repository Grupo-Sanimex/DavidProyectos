using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Dominio.Models.Carrito
{
    public class CotizacionDetalle
    {
        public string? codebar { get; set; }
        public string? description { get; set; }
        public string? cantidad { get; set; }
        public Decimal precioUnitario { get; set; }
        public string? status { get; set; }
        public string? nombreCliente { get; set; }
        public string? clasificacion { get; set; }
    }
}
