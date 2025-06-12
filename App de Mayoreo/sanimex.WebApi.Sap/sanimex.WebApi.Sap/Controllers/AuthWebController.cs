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
    [ApiController]
    public class AuthWebController : ControllerBase
    {
        private readonly IWebMayoreoNegocio _webMayoreoNegocio;
        private readonly string? secretKey;
        private readonly IControlAccesoNegocio _controlAccesoNegocio;
        public AuthWebController(IWebMayoreoNegocio webMayoreoNegocio, IConfiguration config, IControlAccesoNegocio controlAccesoNegocio)
        {
            _webMayoreoNegocio = webMayoreoNegocio;
            secretKey = config.GetSection("settings").GetSection("secretKey").ToString();
            _controlAccesoNegocio = controlAccesoNegocio;
        }
        [HttpPost]
        [Route("AccesoWeb")]
        public async Task<IActionResult> ObtenerAccesoWeb(int numeroUsuario, string password)
        {
            bool horaEntrada = await _controlAccesoNegocio.validarHora();
            if (horaEntrada)
            {
                UsuarioWeb usuario = await _webMayoreoNegocio.ObtenerAccesoWeb(numeroUsuario);
                if (usuario != null)
                {
                    var seguridadService = new SeguridadNegocio();
                    string hash = seguridadService.ConvertirContraseña(password);
                    if (hash == usuario.contrasena)
                    {
                        var keyBytes = Encoding.ASCII.GetBytes(secretKey);
                        var claims = new ClaimsIdentity();
                        claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, Convert.ToString(usuario.idUsuario)));

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
                            idRol = usuario.idPermiso,
                            message = "Adelante",
                            data = new
                            {
                                user = new
                                {
                                    _id = usuario.idUsuario,
                                    idPermiso = usuario.idPermiso,
                                    numEmpleado = usuario.numEmpleado,
                                    nombre = usuario.nombre,
                                    aPaterno = usuario.aPaterno,
                                    status = usuario.status,
                                    __v = 1,
                                    id = usuario.idUsuario
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
            else
            {
                var errorResponse = new
                {
                    data = null as object,
                    status = "success",
                    token = "",
                    message = "No se puede acceder a esta hora"
                };

                return StatusCode(StatusCodes.Status200OK, errorResponse);
            }
        }
    }
}
