using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioDatos.ModelsDto
{
    public class LicenciaOffEqDto
    {
        public int IdLicencia { get; set; }
        public string Cuenta { get; set; }

        public string NumeroSerie { get; set; }
        public string Marca { get; set; }
        public bool Status { get; set; }
    }
}
