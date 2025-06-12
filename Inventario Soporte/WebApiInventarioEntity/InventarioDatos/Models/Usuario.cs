using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioDatos.Models
{
    public class Usuario
    {
        [Key]
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

        [ForeignKey("Rol")]
        public int IdRol { get; set; }
        public bool Status { get; set; }
        // Propiedades de navegación
        public Rol Rol { get; set; }
    }
}
