using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace sanimex.webapi.Datos.Comun
{
    public class Datos: IDatos
    {
        private readonly IConfiguration _configuration;

        public Datos(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnectionString()
        {
            // Obtén el entorno actual
            var environment = _configuration["Environment"];

            // Devuelve la cadena de conexión adecuada
            if (environment == "2")
            {
                return _configuration.GetConnectionString("Connection Desarrollo")!;
            }
            else
            {
                return _configuration.GetConnectionString("Connection Produccion")!;
            }
        }
    }
}
