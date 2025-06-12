using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Dominio.Models.webMayoreo
{
    public class CotizacionS
    {
        public string idDispositivo { get; set; }
        public string tipo_consulta { get; set; }
        public string claveSucursal { get; set; }
        public string Sucursal { get; set; }
        public string nombre { get; set; }
        public string aPaterno { get; set; }
        public string Status { get; set; }
        public string totalCotizacion { get; set; }
        public string idClienteSAP { get; set; }
        public string idventa { get; set; }
    }
}
