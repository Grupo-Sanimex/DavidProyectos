using InventarioDatos.Models;
using InventarioDatos.ModelsDto;
using InventarioNegocio.Equipos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiInventarioEntity.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class EquipoController : ControllerBase
    {
        private readonly IEquipoNegocio _equipoNegocio;
        public EquipoController(IEquipoNegocio equipoNegocio)
        {
            _equipoNegocio = equipoNegocio;
        }
        [HttpGet]
        public IActionResult ObtenerEquipos()
        {
            var equipos = _equipoNegocio.ObtenerEquipos();
            return Ok(equipos);
        }
        [HttpGet("{id}")]
        public IActionResult ObtenerEquipoPorId(int id)
        {
            var equipo = _equipoNegocio.ObtenerEquipoPorId(id);
            if (equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }

        [HttpGet("EquipoDisponible")]
        public IActionResult ObtenerEquiposDisponibles()
        {
            var equipos = _equipoNegocio.ObtenerEquiposDisponibles();
            return Ok(equipos);
        }
        [HttpGet("Empleado/{id}")]
        public IActionResult ObtenerEquipoPorIdEmpleado(int id)
        {
            var equipo = _equipoNegocio.ObtenerEquipoPorIdEmpleado(id);
            if (equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }
        [HttpGet("buscar/{busqueda}")]
        public IActionResult ObtenerEquiposPorBusqueda(string busqueda)
        {
            var equipos = _equipoNegocio.BuscarEquipos(busqueda);
            // Siempre retorna 200 OK con la lista (vacía o no)
            return Ok(equipos ?? new EquipoDto());
        }

        [HttpPost]
        public IActionResult AgregarEquipo([FromBody] EquipoDto equipoDto)
        {
            if (equipoDto == null)
            {
                return BadRequest();
            }
            _equipoNegocio.AgregarEquipo(equipoDto);
            return CreatedAtAction(nameof(ObtenerEquipoPorId), new { id = equipoDto.IdEquipo }, equipoDto);
        }
        [HttpPut("{id}")]
        public IActionResult ActualizarEquipo(int id, [FromBody] EquipoDto equipoDto)
        {
            if (equipoDto == null || id != equipoDto.IdEquipo)
            {
                return BadRequest();
            }
            var equipo = _equipoNegocio.ObtenerEquipoPorId(id);
            if (equipo == null)
            {
                return NotFound();
            }
            _equipoNegocio.ActualizarEquipo(equipoDto);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult EliminarEquipo(int id)
        {
            var equipo = _equipoNegocio.ObtenerEquipoPorId(id);
            if (equipo == null)
            {
                return NotFound();
            }
            _equipoNegocio.EliminarEquipo(id);
            return NoContent();
        }
    }
}