using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Dominio.Models.Ubicaciones
{
    public class UbicacionesModel
    {
        public int id { get; set; }
        public string? direccion { get; set; }
        public decimal latitud { get; set; }
        public decimal longitud { get; set; }
        public string? claveSucursal { get; set; }
        public DateTime fechaUnitaria { get; set; }
        public TimeSpan horaUnitaria { get; set; }
        public string numeroEmpleado { get; set; }
    }
}
