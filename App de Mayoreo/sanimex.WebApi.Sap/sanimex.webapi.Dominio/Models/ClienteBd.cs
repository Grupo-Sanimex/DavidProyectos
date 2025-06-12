using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Dominio.Models
{
    public class ClienteBd
    {
        public int idClienteMayoreo { get; set; }
        public string? nombre { get; set; }
        public string? apaterno { get; set; }
        public string? amaterno { get; set; }
        public string? rfc { get; set; }
        public bool status { get; set; }
        public int creditoTotal { get; set; }
        public int creditoDisponible { get; set; }
        public int diasCredito { get; set; }
        public string? correo { get; set; }
        public string? telefonoMovil { get; set; }
        public string? telefonoCasa { get; set; }
    }
}
