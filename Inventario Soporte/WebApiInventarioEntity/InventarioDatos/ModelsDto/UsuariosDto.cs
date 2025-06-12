using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioDatos.ModelsDto
{
    public class UsuariosDto
    {
        public int IdUsuario { get; set; }
        [Required]
        [StringLength(50)]
        public string UsuarioSesion { get; set; }
        [Required]
        [StringLength(300)]
        public string Contracena { get; set; }

        [Required]
        public int IdRol { get; set; }
        public bool Status { get; set; }
    }
}
