using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Dominio.Models.Producto
{
    public class PrecioCliente
    {
        public string Nombre_Completo { get; set; }
        public string Clasifica { get; set; }
        public string descRecoge { get; set; }
        public string descContado { get; set; }
        public float precio_Final { get; set; }
    }
}
