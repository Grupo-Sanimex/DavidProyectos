using sanimex.webapi.Dominio.Models.webMayoreo;

namespace sanimex.webapi.Negocio.reporteWeb
{
    public interface IReporteWebNegocio
    {
        Task<List<ConsultasCliente>> ConsultasClientes(string idUsuario, string Fecha);
        Task<List<CotizacionS>> Cotizaciones(int idGerente, string Fecha);
        Task<List<CotizacionS>> CotizacionesRango(int idGerente, string FechaInicio, string FechaFin);
        Task<Gerente> Gerente();
        Task<List<Gerente>> ObtenerGerentesLista();
    }
}