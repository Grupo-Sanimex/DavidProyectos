using InventarioDatos.Models;
using InventarioDatos.ModelsDto;
using InventarioNegocio.Herramientas;
using InventarioNegocio.Usuarios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApiInventarioEntity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly string? secretKey;
        private readonly IUsuariosNegocio _usuariosNegocio;
        public AuthController(IUsuariosNegocio usuariosNegocio, IConfiguration configuration)
        {
            _usuariosNegocio = usuariosNegocio;
            secretKey = configuration.GetSection("settings").GetSection("secretKey").ToString();
        }
        [HttpPost]
        public async Task<IActionResult> Acceso([FromBody] Login request)
        {
            string usuario = request.Usuario;
            string contracena = request.Contracena;
            UsuarioDto usuarioDto =  _usuariosNegocio.ObtenerUsuarioPorUsuario(usuario);
                if (usuario != null)
                {
                    var seguridadService = new Md5();
                    string hash = seguridadService.ConvertirContraseña(contracena);
                    if (hash == usuarioDto.Contracena)
                    {
                        var keyBytes = Encoding.ASCII.GetBytes(secretKey);
                        var claims = new ClaimsIdentity();
                        claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuarioDto.IdUsuario.ToString()));

                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = claims,
                            Expires = DateTime.UtcNow.AddHours(1),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
                        };

                        var tokenHandler = new JwtSecurityTokenHandler();
                        var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);
                        string tokencreado = tokenHandler.WriteToken(tokenConfig);

                        // Construir respuesta con el objeto usuario y el token
                        var response = new
                        {
                            status = "success",
                            token = tokencreado,
                            idRol = usuarioDto.IdRol,
                            data = new
                            {
                                user = new
                                {
                                    id = usuarioDto.IdUsuario,
                                    idRol = usuarioDto.IdRol,
                                    correo = usuarioDto.Correo,
                                    nombre = usuarioDto.NombreCompleto,
                                    status = usuarioDto.Status
                                }
                            }
                        };

                        return StatusCode(StatusCodes.Status200OK, response);
                    }
                    else
                    {
                        var errorResponse = new
                        {
                            data = null as object,
                            status = "success",
                            token = "",
                            message = "Contraseña incorrecta"
                        };

                        return StatusCode(StatusCodes.Status200OK, errorResponse);
                    }
                }
                else
                {
                    var errorResponse = new
                    {
                        data = null as object,
                        status = "success",
                        token = "",
                        message = "Usuario no encontrado"
                    };

                    return StatusCode(StatusCodes.Status200OK, errorResponse);
                }
        }
    }
}
