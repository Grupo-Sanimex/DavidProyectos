using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Dominio.Models.webMayoreo
{
    public class UsuarioWeb
    {
        public int idUsuario { get; set; }
        public string idPermiso { get; set; }
        public string numEmpleado { get; set; }
        public string nombre { get; set; }
        public string aPaterno { get; set; }
        public string aMaterno { get; set; }
        public string contrasena { get; set; }
        public string fechaCreacion { get; set; }
        public string status { get; set; }
    }
}
