using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sanimex.webapi.Datos.Servicio.UbicacionesServicio;
using sanimex.webapi.Dominio.Models.Logs;
using sanimex.webapi.Dominio.Models.Ubicaciones;
using sanimex.webapi.Dominio.Models.Usuarios;
using sanimex.webapi.Negocio.Ubicaciones;
using System.Security.Claims;

namespace sanimex.Webapi.Sap.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UbicacionController : ControllerBase
    {
        private readonly IUbicacionNegocio _ubicacionNegocio;

        public UbicacionController(IUbicacionNegocio ubicacionNegocio)
        {
            _ubicacionNegocio = ubicacionNegocio;
        }
        [HttpGet]
        [Route("Ubicaciones/")]
        public async Task<ActionResult<List<UbicacionesModel>>> ListarUbicaciones()
        {
            var ubicacionesModels = await _ubicacionNegocio.ListarUbicaciones();

            if (ubicacionesModels == null)
            {
                return NotFound("No se encontró el empleado");
            }
            else
            {
                return Ok(ubicacionesModels);
            }
        }
        [HttpGet]
        [Route("Ubicacion/")]
        public async Task<ActionResult<UbicacionesModel>> ListarUbicacion(int id)
        {
            var ubicacion = await _ubicacionNegocio.VerUbicacion(id);

            if (ubicacion == null)
            {
                return NotFound("No se encontró el empleado");
            }
            else
            {
                return Ok(ubicacion);
            }
        }

        [HttpGet]
        [Route("UbicacionSucursal/")]
        public async Task<ActionResult<List<UbicacionSucursal>>> UbicacionSucursal(string fechaConsulta)
        {
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //var usuarioId = "505";
            var ubicacionSucursal = await _ubicacionNegocio.UbicacionSucursal(fechaConsulta, usuarioId);
            if (ubicacionSucursal == null)
            {
                return NotFound("No se encontró la ubicacionSucursal");
            }
            else
            {

                var response = new
                {
                    address = ubicacionSucursal.Select(ubicacionS => new
                    {
                        claveSucursal = ubicacionS.claveSucursal,
                        nombreSucursal = ubicacionS.nombreSucursal
                    }).ToList()
                };

                return Ok(response);
            }
        }
        [HttpGet]
        [Route("VisitadorActivos/")]
        public async Task<ActionResult<List<VisitadorActivo>>> VisitadorActivos(string claveSucursal, string fechaUnitaria)
        {
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var visitadorActivo = await _ubicacionNegocio.VisitadorActivos(claveSucursal, fechaUnitaria, usuarioId);
            if (visitadorActivo == null)
            {
                return NotFound("No se encontró el empleado");
            }
            else
            {
                var response = new
                {
                    visitadorActivo = visitadorActivo.Select(visitador => new
                    {
                        claveSucursal = visitador.claveSucursal,
                        nombreSucursal = visitador.nombreSucursal,
                        idUsuario = visitador.numeroEmpleado,
                        numeroEmpleado =visitador.idUsuario,
                        nombre = visitador.nombre,
                        aPaterno = visitador.aPaterno,
                        aMaterno = visitador.aMaterno
                    })
                };

                return Ok(response);
            }
        }
        [HttpGet]
        [Route("UbicacionesMaps/")]
        public async Task<ActionResult<List<UbicacionesMaps>>> UbicacionesMaps(string claveSucursal, string numeroEmpleado, string fechaUnitaria)
        {
            var ubicacionesMaps = await _ubicacionNegocio.UbicacionesMaps(claveSucursal, numeroEmpleado, fechaUnitaria);
            if (ubicacionesMaps == null || !ubicacionesMaps.Any())
            {
                return NotFound("No se encontró el empleado");
            }
            else
            {
                var response = new
                {
                    ubicaciones = ubicacionesMaps.Select(ubicacion => new
                    {
                        id = ubicacion.id,
                        direccion = ubicacion.direccion,
                        latitud = ubicacion.latitud,
                        longitud = ubicacion.longitud,
                        horaUnitaria = ubicacion.horaUnitaria
                    })
                };
                return Ok(response);
            }
        }
        [HttpGet]
        [Route("UbicacionesVisitaMaps/")]
        public async Task<ActionResult<List<UbicacionesMaps>>> UbicacionesVisitaMaps(string claveSucursal, string numeroEmpleado, string fechaUnitaria)
        {
            var ubicacionesMaps = await _ubicacionNegocio.UbicacionesVisitaMaps(claveSucursal, numeroEmpleado, fechaUnitaria);
            if (ubicacionesMaps == null || !ubicacionesMaps.Any())
            {
                return NotFound("No se encontró el empleado");
            }
            else
            {
                var response = new
                {
                    ubicaciones = ubicacionesMaps.Select(ubicacion => new
                    {
                        id = ubicacion.id,
                        direccion = ubicacion.direccion,
                        latitud = ubicacion.latitud,
                        longitud = ubicacion.longitud,
                        horaUnitaria = ubicacion.horaUnitaria
                    })
                };
                return Ok(response);
            }
        }

        [HttpPost]
        [Route("InsertarUbicacion/")]
        public async Task<ActionResult<int>> InsertarUbicacion(string direccion, double latitud, double longitud, string claveSucursal, bool tipoIngreso)
        {
            // Obtiene el ID del usuario autenticado desde el token
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // en servidor
           int userId = Convert.ToInt32(usuarioId);
            // local
            //int userId = 3079;
            //int userId = 531;
            // Llama al método ObtenerAcceso de manera asíncrona
            int respuesta = await _ubicacionNegocio.InsertarUbicacion(direccion,latitud,longitud,claveSucursal, userId, tipoIngreso);

            // Verifica si se encontró el usuario
            if (respuesta == 0)
            {
                var response = new
                {
                    idDireccion = 0,
                };
                return Ok(response);
            }
            else
            {
                var response = new
                {
                    idDireccion = respuesta,
                };
                return Ok(response);
            }
        }
    }
}
