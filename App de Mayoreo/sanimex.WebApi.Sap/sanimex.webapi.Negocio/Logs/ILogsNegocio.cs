using sanimex.webapi.Dominio.Models.Logs;

namespace sanimex.webapi.Negocio.Logs
{
    public interface ILogsNegocio
    {
        Task<bool> GuardarConsultas(ClienteLogs cliente);
    }
}