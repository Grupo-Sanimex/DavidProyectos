using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sanimex.webapi.Dominio.Models.Ubicaciones;
using sanimex.webapi.Dominio.Models.Usuarios;
using sanimex.webapi.Dominio.Models.webMayoreo;
using sanimex.webapi.Negocio.reporteWeb;
using System.Security.Claims;

namespace sanimex.Webapi.Sap.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ReporteWebController : ControllerBase
    {
        private readonly IReporteWebNegocio _reporteWeb;

        public ReporteWebController(IReporteWebNegocio reporteWeb)
        {
            _reporteWeb = reporteWeb;
        }
        [HttpGet]
        [Route("Gerentes/")]
        public async Task<ActionResult<List<Gerente>>> ObtenerGerentesLista()
        {

            List<Gerente> gerentes = await _reporteWeb.ObtenerGerentesLista();
            if (gerentes == null || gerentes.Count == 0)
            {
                return NotFound("No se encontró la gerentes");
            }
            else
            {
                var response = new
                {
                    status = "success",
                    data = new
                    {
                        gerentes = gerentes.Select(gerente => new
                        {
                            idGerente = gerente.idUsuario, 
                            gerente = new
                            {
                                idGerente = gerente.idUsuario, 
                                nombre = gerente.nombre, 
                                ApellidoP = gerente.aPaterno ?? "", 
                                ApellidoM = gerente.aMaterno ?? "",
                                numEmpleado = gerente.numEmpleado,
                                idSAP = gerente.idSAP
                            }
                        })
                    }
                };

                return Ok(response);
            }
        }

        [HttpGet]
        [Route("Cotizacion/")]
        public async Task<ActionResult<List<CotizacionS>>> Cotizaciones(int idGerente, string Fecha)
        {
            List<CotizacionS> cotizaciones = await _reporteWeb.Cotizaciones(idGerente, Fecha);
            if (cotizaciones == null)
            {
                return NotFound("No se encontró la gerentes");
            }
            else
            {
                var response = new
                {
                    status = "success",
                    data = new
                    {
                        cotizaciones = cotizaciones.Select(cotizacion => new
                        {
                            idDispositivo = cotizacion.idDispositivo,
                            gerente = new
                            {
                                idDispositivo = cotizacion.idDispositivo,
                                tipo_consulta = cotizacion.tipo_consulta,
                                claveSucursal = cotizacion.claveSucursal,
                                Sucursal = cotizacion.Sucursal,
                                nombre = cotizacion.nombre ?? "",
                                ApellidoM = cotizacion.aPaterno ?? "",
                                Status = cotizacion.Status,
                                totalCotizacion = cotizacion.totalCotizacion,
                                idClienteSAP = cotizacion.idClienteSAP,
                                idventa = cotizacion.idventa
                            }
                        })
                    }
                };
                return Ok(response);
            }
        }

        [HttpGet]
        [Route("CotizacionRango/")]
        public async Task<ActionResult<List<CotizacionS>>> CotizacionesRango(int idGerente, string FechaInicio, string FechaFin)
        {
            var cotizaciones = await _reporteWeb.CotizacionesRango(idGerente, FechaInicio, FechaFin);
            if (cotizaciones == null)
            {
                return NotFound("No se encontró la gerentes");
            }
            else
            {
                return Ok(cotizaciones);
            }

        }
        [HttpGet]
        [Route("ConsultasClientes/")]
        public async Task<ActionResult<List<ConsultasCliente>>> ConsultasClientes(string idUsuario, string Fecha)
        {
            var consultas = await _reporteWeb.ConsultasClientes(idUsuario, Fecha);
            if (consultas == null)
            {
                return NotFound("No se encontró la gerentes");
            }
            else
            {
                var response = new
                {
                    status = "success",
                    data = new
                    {
                        consultas = consultas.Select(consulta => new
                        {
                            consulta = new
                            {
                                numCliente = consulta.numCliente,
                                idSAP = consulta.idSAP,
                                claveArticulo = consulta.claveArticulo,
                                vecesConsultado = consulta.vecesConsultado
                            }
                        })
                    }
                };
                return Ok(response);
            }

        }
    }
}
