using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioDatos.ModelsDto
{
    public class DepartamentoDto
    {
        public int IdDepartamento { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreDepartamento { get; set; }

        public bool Status { get; set; } = true;
    }
}
