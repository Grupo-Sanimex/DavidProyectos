using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioDatos.ModelsDto
{
    public class EmpleadoDtoDetalle
    {
        public int IdEmpleado { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string ApellidoP { get; set; }

        [StringLength(100)]
        public string ApellidoM { get; set; }

        [Required]
        [StringLength(100)]
        public string Puesto { get; set; }

        [Required]
        public int IdDepartamento { get; set; }

        [Required]
        public int IdUbicacion { get; set; }

        public bool Status { get; set; } = true;

        // Propiedades adicionales para visualización (opcional)
        public string NombreDepartamento { get; set; }
        public string Sucursal { get; set; }
    }
}
