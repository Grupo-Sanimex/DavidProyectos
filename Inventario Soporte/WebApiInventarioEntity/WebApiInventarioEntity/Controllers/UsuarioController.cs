using InventarioDatos.ModelsDto;
using InventarioNegocio.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiInventarioEntity.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuariosNegocio _usuariosNegocio;
        public UsuarioController(IUsuariosNegocio usuariosNegocio)
        {
            _usuariosNegocio = usuariosNegocio;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var usuarios = _usuariosNegocio.ObtenerUsuarios();
            return Ok(usuarios);
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var usuario = _usuariosNegocio.ObtenerUsuarioPorId(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }
        [HttpPost]
        public IActionResult Post([FromBody] UsuarioDto usuarioDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _usuariosNegocio.AgregarUsuario(usuarioDto);
            return CreatedAtAction(nameof(Get), new { id = usuarioDto.IdUsuario }, usuarioDto);
        }
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UsuarioDto usuarioDto)
        {
            if (id != usuarioDto.IdUsuario || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var usuario = _usuariosNegocio.ObtenerUsuarioPorId(id);
            if (usuario == null)
            {
                return NotFound();
            }
            _usuariosNegocio.ActualizarUsuario(usuarioDto);
            return NoContent();
        }
        [HttpPut("Contracena/{id}")]
        public IActionResult PutContracena(int id, [FromBody] string nuevaContracena)
        {
            var usuario = _usuariosNegocio.ObtenerUsuarioPorId(id);
            if (usuario == null)
            {
                return NotFound();
            }
            _usuariosNegocio.ActualizarContracena(id, nuevaContracena);
            return NoContent();
        }
        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            var usuario = _usuariosNegocio.ObtenerUsuarioPorId(id);
            if (usuario == null)
            {
                return NotFound();
            }
            _usuariosNegocio.EliminarUsuario(id);
            return NoContent();
        }
    }
}
