using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sanimex.webapi.Negocio.Clientes;
using sanimex.webapi.Negocio.Usuarios;
using System.Security.Claims;

namespace sanimex.Webapi.Sap.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PerfilController : ControllerBase
    {
        private readonly IUsuarioNegocio _usuarioNegocio;
        public PerfilController(IUsuarioNegocio usuarioNegocio) // Inyección de dependencia
        {
        _usuarioNegocio = usuarioNegocio;
        }
        [HttpGet]
        [Route("perfil")]
        public async Task<ActionResult<object>> ObtenerPerfil() // Elimina el parámetro id
        {
            // Obtiene el ID del usuario autenticado desde el token
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Obtiene el perfil del usuario utilizando el ID
            var usuario = await _usuarioNegocio.Perfil(usuarioId);

            if (usuario == null)
                return NotFound(new { message = "No se encontró el usuario" });

            // Construir el JSON con la estructura esperada
            var response = new
            {
                user = new
                {
                    _id = usuario._id,
                    idSucursal = usuario.idSucursal,
                    idPermiso = usuario.idPermiso,
                    idRol = usuario.idRol,
                    numEmpleado = usuario.numEmpleado,
                    correo = usuario.correo,
                    nombre = usuario.nombre,
                    aPaterno = usuario.aPaterno,
                    status = usuario.status,
                    __v = 1, // Versión o valor por defecto
                    telefono = usuario.telefono,
                    id = usuario.idUsuario
                }
            };

            return Ok(response); // Devuelve el perfil del usuario
        }
    }
    [ApiController]
    [Route("api/[controller]")]
    public class NetworkController : ControllerBase
    {
        [HttpGet("client-ip")]
        public IActionResult GetClientIP()
        {
            var clientIP = HttpContext.Connection.RemoteIpAddress?.ToString();

            if (string.IsNullOrEmpty(clientIP))
            {
                return StatusCode(500, "No se pudo obtener la dirección IP del cliente.");
            }

            return Ok(new { ClientIP = clientIP });
        }
    }
}
