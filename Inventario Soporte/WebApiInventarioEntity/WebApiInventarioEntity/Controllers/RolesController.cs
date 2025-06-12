using InventarioDatos.ModelsDto;
using InventarioNegocio.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiInventarioEntity.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRolNegocio _rolNegocio;
        public RolesController(IRolNegocio rolNegocio)
        {
            _rolNegocio = rolNegocio;
        }
        [HttpGet]
        public IActionResult ObtenerRoles()
        {
            var roles = _rolNegocio.ObtenerRoles();
            return Ok(roles);
        }
        [HttpGet("{id}")]
        public IActionResult ObtenerRolPorId(int id)
        {
            var rol = _rolNegocio.ObtenerRolPorId(id);
            if (rol == null)
            {
                return NotFound();
            }
            return Ok(rol);
        }
        [HttpPost]
        public IActionResult AgregarRol([FromBody] RolDto roldto)
        {
            if (roldto == null)
            {
                return BadRequest();
            }
            _rolNegocio.AgregarRol(roldto);
            return CreatedAtAction(nameof(ObtenerRolPorId), new { id = roldto.IdRol }, roldto);
        }
        [HttpPut("{id}")]
        public IActionResult ActualizarRol(int id, [FromBody] RolDto roldto)
        {
            if (roldto == null || id != roldto.IdRol)
            {
                return BadRequest();
            }
            var rol = _rolNegocio.ObtenerRolPorId(id);
            if (rol == null)
            {
                return NotFound();
            }
            _rolNegocio.ActualizarRol(roldto);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult EliminarRol(int id)
        {
            var rol = _rolNegocio.ObtenerRolPorId(id);
            if (rol == null)
            {
                return NotFound();
            }
            _rolNegocio.EliminarRol(id);
            return NoContent();
        }

    }
}
