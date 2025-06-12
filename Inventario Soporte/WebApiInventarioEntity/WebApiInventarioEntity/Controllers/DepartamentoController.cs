using InventarioDatos.Models;
using InventarioDatos.ModelsDto;
using InventarioNegocio.Departamentos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiInventarioEntity.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class DepartamentoController : ControllerBase
    {
        private readonly IDepartamentoNegocio _departamentoNegocio;

        public DepartamentoController(IDepartamentoNegocio departamentoNegocio)
        {
            _departamentoNegocio = departamentoNegocio;
        }

        [HttpGet]
        public IActionResult GetDepartamentos()
        {
            var departamentos = _departamentoNegocio.ObtenerDepartamentos();
            return Ok(departamentos);
        }

        [HttpGet("{id}")]
        public IActionResult GetDepartamento(int id)
        {
            var departamento = _departamentoNegocio.ObtenerDepartamentoPorId(id);
            if (departamento == null)
            {
                return NotFound();
            }
            return Ok(departamento);
        }

        [HttpPost]
        public IActionResult PostDepartamento([FromBody] DepartamentoDto departamento)
        {
            if (departamento == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultado = _departamentoNegocio.AgregarDepartamento(departamento);

            if (resultado.IdDepartamento == 0)
            {
                return BadRequest(ModelState);
            }

            return CreatedAtAction(nameof(GetDepartamento), new { id = resultado.IdDepartamento }, resultado);
        }



        [HttpPut("{id}")]
        public IActionResult PutDepartamento(int id, [FromBody] DepartamentoDto departamento)
        {
            if (departamento == null || departamento.IdDepartamento != id || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var departamentoExistente = _departamentoNegocio.ObtenerDepartamentoPorId(id);
            if (departamentoExistente == null)
            {
                return NotFound();
            }
            var resultado = _departamentoNegocio.ActualizarDepartamento(departamento);
            if (resultado.IdDepartamento == 0)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDepartamento(int id)
        {
            var departamento = _departamentoNegocio.ObtenerDepartamentoPorId(id);
            if (departamento == null)
            {
                return NotFound();
            }
            _departamentoNegocio.EliminarDepartamento(id);
            return NoContent();
        }
    }
}
