using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Dominio.Models.webMayoreo
{
    public class ConsultasCliente
    {
        public string? numCliente { get; set; }
        public string? idSAP { get; set; }
        public string? claveArticulo { get; set; }
        public string? vecesConsultado { get; set; }
    }
}
