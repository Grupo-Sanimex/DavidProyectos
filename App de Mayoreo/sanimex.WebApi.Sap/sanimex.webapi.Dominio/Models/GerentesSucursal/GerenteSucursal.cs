using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Dominio.Models.GerentesSucursal
{
    public class GerenteSucursal
    {
        public int idSucursalApp { get; set; }

        [Required]
        public int idGerente { get; set; }
        [Required]
        public string claveSucursal { get; set; }
        [Required]
        public DateTime fechaCreacion { get; set; }
        [Required]
        public DateTime fechaModificacion { get; set; }
        public bool status { get; set; }
    }
}
