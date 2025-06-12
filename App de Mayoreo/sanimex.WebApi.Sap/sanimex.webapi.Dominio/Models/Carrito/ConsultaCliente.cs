using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Dominio.Models.Carrito
{
    public class ConsultaCliente
    {
        public required string codebar { get; set; }
        public required string centrosCorredor { get; set; }
        public required string claveCliente { get; set; }

    }
}
