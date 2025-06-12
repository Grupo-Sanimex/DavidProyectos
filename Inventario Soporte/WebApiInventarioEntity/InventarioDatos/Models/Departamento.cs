using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioDatos.Models
{
    public class Departamento
    {
        [Key]
        public int IdDepartamento { get; set; }

        [StringLength(100)]
        public string NombreDepartamento { get; set; }

        public bool Status { get; set; } = true;

        // Propiedades de navegación
        public ICollection<Empleado> Empleados { get; set; }
        public ICollection<Equipo> Equipos { get; set; }
    }
}
