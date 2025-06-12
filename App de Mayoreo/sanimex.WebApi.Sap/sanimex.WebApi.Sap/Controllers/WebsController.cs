using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using sanimex.webapi.Dominio.Models;
using sanimex.webapi.Dominio.Models.webMayoreo;
using sanimex.webapi.Negocio.ControlAcceso;
using sanimex.webapi.Negocio.Seguridad;
using sanimex.webapi.Negocio.WebsMayoreo;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace sanimex.Webapi.Sap.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class WebsController : ControllerBase
    {
        private readonly IWebMayoreoNegocio _webMayoreoNegocio;
        private readonly string? secretKey;
        private readonly IControlAccesoNegocio _controlAccesoNegocio;
        public WebsController(IWebMayoreoNegocio webMayoreoNegocio, IConfiguration config, IControlAccesoNegocio controlAccesoNegocio)
        {
            _webMayoreoNegocio = webMayoreoNegocio;
            secretKey = config.GetSection("settings").GetSection("secretKey").ToString();
            _controlAccesoNegocio = controlAccesoNegocio;
        }
       
        [HttpGet("ListarRol/{idRol}")]
        public IActionResult ListaRol(int idRol)
        {
            var result = _webMayoreoNegocio.ListarRol(idRol);
            return Ok(result);
        }
        [HttpGet("ListarRoles")]
        public IActionResult ListarRoles()
        {
            var result = _webMayoreoNegocio.ListarRoles();
            return Ok(result);
        }
        [HttpPost("GuardarRol")]
        public async Task<IActionResult> Post(string perfil)
        {
            string result = await _webMayoreoNegocio.GuardarRol(perfil);
            return Ok(result);
        }
        [HttpDelete("BajaRol/{idRol}")]
        public IActionResult Delete(int idRol)
        {
            bool result = _webMayoreoNegocio.BajaRol(idRol);
            if (!result)
            {
                return BadRequest("Error al dar de baja el rol");
            }
            return Ok(result);
        }
        [HttpGet("ListarUsuario/{idUsuario}")]
        public IActionResult ListarUsuario(int idUsuario)
        {
            var result = _webMayoreoNegocio.ListarUsuario(idUsuario);
            return Ok(result);
        }
        [HttpGet("ListarUsuarios")]
        public IActionResult ListarUsuarios()
        {
            var result = _webMayoreoNegocio.ListarUsuarios();
            return Ok(result);
        }
        [HttpPost("AgregarUsuarioWeb")]
        public async Task<IActionResult> Post(int idPermiso, int numEmpleado, string nombre, string aPaterno, string aMaterno, string contrasena)
        {
            string result = await _webMayoreoNegocio.AgregarUsuarioWeb(idPermiso, numEmpleado, nombre, aPaterno, aMaterno, contrasena);
            return Ok(result);
        }
        [HttpDelete("BajaUsuarioWeb/{idUsuario}")]
        public async Task<IActionResult> BajaUsuario(int idUsuario)
        {
            string result = await _webMayoreoNegocio.BajaUsuarioWeb(idUsuario);
            if (result == "Error")
            {
                return BadRequest("Error al dar de baja el usuario");
            }
            return Ok(result);
        }
    }
}
