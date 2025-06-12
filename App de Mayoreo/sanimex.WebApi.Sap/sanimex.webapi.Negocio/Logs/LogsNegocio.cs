using sanimex.webapi.Datos.Servicio.LogServicio;
using sanimex.webapi.Datos.Servicio.LogServicio.implementacion;
using sanimex.webapi.Dominio.Models.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Negocio.Logs
{
    public class LogsNegocio : ILogsNegocio
    {
        private readonly ILogsServicio _logsServicio;

        public LogsNegocio(ILogsServicio logsServicio)
        {
            _logsServicio = logsServicio;
        }

        public async Task<bool> GuardarConsultas(ClienteLogs cliente)
        {
            bool respuesta = await _logsServicio.GuardarConsultas(cliente);
            if (!respuesta)
                return false;
            else
                return true;
        }
    }
}
