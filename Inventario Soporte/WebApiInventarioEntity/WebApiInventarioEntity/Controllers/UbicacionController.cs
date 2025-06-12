using InventarioDatos.ModelsDto;
using InventarioNegocio.Ubicaciones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiInventarioEntity.Controllers
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
        public IActionResult ObtenerUbicaciones()
        {
            var ubicaciones = _ubicacionNegocio.ObtenerUbicaciones();
            return Ok(ubicaciones);
        }
        [HttpGet("{id}")]
        public IActionResult ObtenerUbicacionPorId(int id)
        {
            var ubicacion = _ubicacionNegocio.ObtenerUbicacionPorId(id);
            if (ubicacion == null)
            {
                return NotFound();
            }
            return Ok(ubicacion);
        }
        [HttpPost]
        public IActionResult AgregarUbicacion([FromBody] UbicacionDto ubicacionDto)
        {
            if (ubicacionDto == null)
            {
                return BadRequest("UbicacionDto no puede ser nulo");
            }
            _ubicacionNegocio.AgregarUbicacion(ubicacionDto);
            return CreatedAtAction(nameof(ObtenerUbicacionPorId), new { id = ubicacionDto.IdUbicacion }, ubicacionDto);
        }
        [HttpPut("{id}")]
        public IActionResult ActualizarUbicacion(int id, [FromBody] UbicacionDto ubicacionDto)
        {
            if (ubicacionDto == null)
            {
                return BadRequest("UbicacionDto no puede ser nulo");
            }
            var ubicacionExistente = _ubicacionNegocio.ObtenerUbicacionPorId(id);
            if (ubicacionExistente == null)
            {
                return NotFound();
            }
            _ubicacionNegocio.ActualizarUbicacion(ubicacionDto);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult EliminarUbicacion(int id)
        {
            var ubicacionExistente = _ubicacionNegocio.ObtenerUbicacionPorId(id);
            if (ubicacionExistente == null)
            {
                return NotFound();
            }
            _ubicacionNegocio.EliminarUbicacion(id);
            return NoContent();
        }
    }
}
