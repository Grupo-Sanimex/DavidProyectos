using sanimex.webapi.Datos.Servicio.ReportesServicio;
using sanimex.webapi.Dominio.Models.webMayoreo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Negocio.reporteWeb
{
    public class ReporteWebNegocio : IReporteWebNegocio
    {
        private readonly IReporteServicio _reporteServicio;
        public ReporteWebNegocio(IReporteServicio reporteServicio)
        {
            _reporteServicio = reporteServicio;
        }
        public async Task<Gerente> Gerente()
        {
            return await _reporteServicio.ObtenerGerentes();
        }
        public async Task<List<Gerente>> ObtenerGerentesLista()
        {
            return await _reporteServicio.ObtenerGerentesLista();
        }
        public async Task<List<CotizacionS>> Cotizaciones(int idGerente, string Fecha)
        {
            return await _reporteServicio.Cotizaciones(idGerente, Fecha);
        }
        public async Task<List<CotizacionS>> CotizacionesRango(int idGerente, string FechaInicio, string FechaFin)
        {
            return await _reporteServicio.CotizacionesRango(idGerente, FechaInicio, FechaFin);
        }
        public async Task<List<ConsultasCliente>> ConsultasClientes(string idUsuario, string Fecha)
        {
            return await _reporteServicio.ConsultasClientes(idUsuario, Fecha);
        }
    }
}
