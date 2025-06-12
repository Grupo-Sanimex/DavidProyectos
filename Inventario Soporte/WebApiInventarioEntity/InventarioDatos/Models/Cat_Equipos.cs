using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioDatos.Models
{
    public class Cat_Equipos
    {
        [Key]
        public int catequipo_id { get; set; }

        [StringLength(100)]
        public int catequipo_nombre { get; set; }
        [StringLength(100)]
        public int usuario { get; set; }
        public DateTime Fecha { get; set; }
    }
}
