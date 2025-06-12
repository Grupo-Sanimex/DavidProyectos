using sanimex.webapi.Dominio.Models.Logs;

namespace sanimex.webapi.Datos.Servicio.LogServicio.implementacion
{
    public interface ILogsServicio
    {
        Task<bool> GuardarConsultas(ClienteLogs cliente);
    }
}