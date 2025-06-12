using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Dominio.Models.Ubicaciones
{
    public class VisitadorActivo
    {
        public string? claveSucursal { get; set; }
        public string? nombreSucursal { get; set; }
        public string? idUsuario { get; set; }
        public string? numeroEmpleado { get; set; }
        public string? nombre { get; set; }
        public string? aPaterno { get; set; }
        public string? aMaterno { get; set; }
    }
}
