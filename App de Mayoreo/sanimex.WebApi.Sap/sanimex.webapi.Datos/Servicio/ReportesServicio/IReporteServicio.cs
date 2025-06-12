using sanimex.webapi.Dominio.Models.webMayoreo;

namespace sanimex.webapi.Datos.Servicio.ReportesServicio
{
    public interface IReporteServicio
    {
        Task<List<ConsultasCliente>> ConsultasClientes(string idUsuario, string Fecha);
        Task<List<CotizacionS>> Cotizaciones(int idGerente, string Fecha);
        Task<List<CotizacionS>> CotizacionesRango(int idGerente, string FechaInicio, string FechaFin);
        Task<Gerente> ObtenerGerentes();
        Task<List<Gerente>> ObtenerGerentesLista();
    }
}