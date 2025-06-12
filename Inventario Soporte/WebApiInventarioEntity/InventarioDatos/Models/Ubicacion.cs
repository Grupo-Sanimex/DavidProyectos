using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioDatos.Models
{
    public class Ubicacion
    {
        [Key]
        public int IdUbicacion { get; set; }

        [StringLength(50)]
        public string Zona { get; set; }

        [StringLength(50)]
        public string Region { get; set; }

        [StringLength(50)]
        public string Centro { get; set; }

        [StringLength(50)]
        public string Estado { get; set; }

        [StringLength(100)]
        public string Sucursal { get; set; }

        public bool Status { get; set; } = true;

        // Propiedades de navegación
        public ICollection<Empleado> Empleados { get; set; }
        public ICollection<Equipo> Equipos { get; set; }
    }
}
