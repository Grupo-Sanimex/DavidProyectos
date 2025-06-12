using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioDatos.ModelsDto
{
    public class UsuarioDto
    {
        public int IdUsuario { get; set; }

        [StringLength(100)]
        public string NombreCompleto { get; set; }

        [StringLength(100)]
        public string Correo { get; set; }

        [StringLength(100)]
        public string Puesto { get; set; }

        [StringLength(50)]
        public string UsuarioSesion { get; set; }

        [StringLength(300)]
        public string Contracena { get; set; }
      
        public int IdRol { get; set; }
        public bool Status { get; set; } = true;
    }
}
