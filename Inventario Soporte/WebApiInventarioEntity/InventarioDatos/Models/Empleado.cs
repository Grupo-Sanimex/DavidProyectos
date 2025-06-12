using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioDatos.Models
{
    public class Empleado
    {
        [Key]
        public int IdEmpleado { get; set; }

        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(100)]
        public string ApellidoP { get; set; }

        [StringLength(100)]
        public string ApellidoM { get; set; }

        [StringLength(100)]
        public string Puesto { get; set; }

        [StringLength(50)]
        public string UsuarioWindows { get; set; }
        [StringLength(100)]
        public string UsuarioAD { get; set; }

        [StringLength(100)]
        public string Correo { get; set; }
        [StringLength(300)]
        public string Pass { get; set; }
        [StringLength(50)]
        public string Acceso { get; set; }

        [ForeignKey("Departamento")]
        public int? IdDepartamento { get; set; }

        [ForeignKey("Ubicacion")]
        public int? IdUbicacion { get; set; }

        public bool Status { get; set; }

        // Propiedades de navegación
        public Departamento Departamento { get; set; }
        public Ubicacion Ubicacion { get; set; }
        public ICollection<Usuario> Usuarios { get; set; }
        public ICollection<Equipo> Equipos { get; set; }
    }
}
