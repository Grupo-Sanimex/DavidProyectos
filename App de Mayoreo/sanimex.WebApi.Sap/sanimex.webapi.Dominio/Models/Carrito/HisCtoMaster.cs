using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Dominio.Models.Carrito
{
    public class HisCtoMaster
    {
        public string? idCotizacion { get; set; }
        public string? totalCotizacion { get; set; }
        public string? idClienteSAP { get; set; }
        public string? Status { get; set; }
        public string? fecha { get; set; }
        public string? hora { get; set; }
        public string? idventa { get; set; }

    }
}
