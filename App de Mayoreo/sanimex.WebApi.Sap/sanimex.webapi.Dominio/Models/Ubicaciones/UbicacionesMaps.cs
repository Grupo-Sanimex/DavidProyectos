using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Dominio.Models.Ubicaciones
{
    public class UbicacionesMaps
    {
        public int id { get; set; }
        public string? direccion { get; set; }
        public double latitud { get; set; }
        public double longitud { get; set; }
        public TimeSpan horaUnitaria { get; set; }
    }
}
