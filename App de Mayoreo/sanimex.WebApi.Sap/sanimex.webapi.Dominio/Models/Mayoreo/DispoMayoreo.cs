using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Dominio.Models.Mayoreo
{
    public class DispoMayoreo
    {
        public string Codigo { get; set; }
        public string ClaveSAP { get; set; }
        public string NombreSucursal { get; set; }
        public string StockLibre { get; set; }
        public string StockDispo { get; set; }
    }
}
