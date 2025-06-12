using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioDatos.ModelsDto
{
    public class EmpleadoDto
    {
        public int IdEmpleado { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }
        [Required]
        [StringLength(100)]
        public string ApellidoP { get; set; }
        [Required]
        [StringLength(100)]
        public string ApellidoM { get; set; }
        [Required]
        [StringLength(100)]
        public string Puesto { get; set; }
        [Required]
        [StringLength(50)]
        public string UsuarioWindows { get; set; }
        [Required]
        [StringLength(100)]
        public string UsuarioAD { get; set; }
        [Required]
        [StringLength(100)]
        public string Correo { get; set; }
        [Required]      
        [StringLength(300)]
        public string Pass { get; set; }
        [Required]
        [StringLength(50)]
        public string Acceso { get; set; }

        [Required]
        public int? IdDepartamento { get; set; }

        [Required]
        public int? IdUbicacion { get; set; }
        public bool Status { get; set; } = true;
    }
}
