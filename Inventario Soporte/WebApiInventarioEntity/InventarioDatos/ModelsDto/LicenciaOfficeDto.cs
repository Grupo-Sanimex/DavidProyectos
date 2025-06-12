using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioDatos.ModelsDto
{
    public class LicenciaOfficeDto
    {
        public int IdLicencia { get; set; }

        [Required]
        [StringLength(100)]
        public string Cuenta { get; set; }

        [Required]
        public int? IdEquipo { get; set; }
        [Required]
        public bool Status { get; set; } = true;
    }
}
