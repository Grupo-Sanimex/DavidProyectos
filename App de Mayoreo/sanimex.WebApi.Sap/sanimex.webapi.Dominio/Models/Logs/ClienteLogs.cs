using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Dominio.Models.Logs
{
    public class ClienteLogs
    {
        public string? idUsuario { get; set; }
        public string? numCliente { get; set; }
        public string? idSAP { get; set; }
        public string? claveArticulo { get; set; }
        public string? numeroPedido { get; set; }
        public DateTime Fecha { get; set; }
        public bool TipoConsulta { get; set; }
        public int idDireccion { get; set; }
    }
}
