using InventarioDatos.ModelsDto;
using InventarioNegocio.Empleados;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiInventarioEntity.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        private readonly IEmpleadoNegocio _empleadoNegocio;
        public EmpleadoController(IEmpleadoNegocio empleadoNegocio)
        {
            _empleadoNegocio = empleadoNegocio;
        }
        [HttpGet]
        public IActionResult GetEmpleados()
        {
            var empleados = _empleadoNegocio.ObtenerEmpleados();
            return Ok(empleados);
        }

        [HttpGet("{id}")]
        public IActionResult GetEmpleado(int id)
        {
            var empleado = _empleadoNegocio.ObtenerEmpleadoPorId(id);
            if (empleado == null)
            {
                return NotFound();
            }
            return Ok(empleado);
        }
        [HttpGet("Buscar/{busqueda}")]
        public IActionResult GetEmpleadosPorParametro(string busqueda)
        {
            var empleados = _empleadoNegocio.BuscarEmpleadosPorParametro(busqueda);
            if (empleados == null || !empleados.Any())
            {
                return NotFound();
            }
            return Ok(empleados);
        }

        [HttpPost]
        public IActionResult PostEmpleado([FromBody] EmpleadoDto empleado)
        {
            if (empleado == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var resultado = _empleadoNegocio.AgregarEmpleado(empleado);
            if (resultado.IdEmpleado == 0)
            {
                return BadRequest(ModelState);
            }
            return CreatedAtAction(nameof(GetEmpleado), new { id = empleado.IdEmpleado }, empleado);
        }

        [HttpPut("{id}")]
        public IActionResult PutEmpleado(int id, [FromBody] EmpleadoDto empleado)
        {
            if (empleado == null || empleado.IdEmpleado != id || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var empleadoExistente = _empleadoNegocio.ObtenerEmpleadoPorId(id);
            if (empleadoExistente == null)
            {
                return NotFound();
            }
            var resultado = _empleadoNegocio.ActualizarEmpleado(empleado);
            if (resultado.IdEmpleado == 0)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEmpleado(int id)
        {
            var empleado = _empleadoNegocio.ObtenerEmpleadoPorId(id);
            if (empleado == null)
            {
                return NotFound();
            }
            _empleadoNegocio.EliminarEmpleado(id);
            return NoContent();
        }
    }
}