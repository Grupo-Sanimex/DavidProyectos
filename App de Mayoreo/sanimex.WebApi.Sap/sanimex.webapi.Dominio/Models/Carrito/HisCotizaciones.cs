using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Dominio.Models.Carrito
{
    public class HisCotizaciones
    {
        public int idCotizacion { get; set; }
        public string idDispositivo { get; set; }
        public int idSucursal { get; set; }
        public string Status { get; set; }
        public DateTime fechaAlta { get; set; }
        public float totalCotizacion { get; set; }
        public string idClienteSAP { get; set; }
    }
}
