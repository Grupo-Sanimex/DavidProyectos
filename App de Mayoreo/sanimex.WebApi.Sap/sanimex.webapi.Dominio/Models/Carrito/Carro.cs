using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Dominio.Models.Carrito
{
    public class Carro
    {
        public string codigo { get; set; }
        public string descripsion { get; set; }
        public string sucursal { get; set; }
        public float precioFinal { get; set; }
        public int cantidad { get; set; }
        public string? ClaveCliente { get; set; }
        public List<string> Images { get; set; }
        public bool Recoge { get; set; }
        public bool Contado { get; set; }
        public string? userId { get; set; }
        public string? tipo_consulta { get; set; }
    }
}
