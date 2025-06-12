using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioDatos.Models
{
    public class Rol
    {
        [Key]
        public int IdRol { get; set; }

        [StringLength(50)]
        public string NombreRol { get; set; }

        public bool Status { get; set; } = true;

        // Propiedad de navegación
        public ICollection<Usuario> Usuarios { get; set; }
    }
}
