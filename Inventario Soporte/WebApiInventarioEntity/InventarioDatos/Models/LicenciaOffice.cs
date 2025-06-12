using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioDatos.Models
{
    public class LicenciaOffice
    {
        [Key]
        public int IdLicencia { get; set; }

        [StringLength(100)]
        public string Cuenta { get; set; }

        [ForeignKey("Equipo")]
        public int? IdEquipo { get; set; }

        public bool Status { get; set; } = true;

        // Propiedad de navegación
        public Equipo Equipo { get; set; }
    }
}
