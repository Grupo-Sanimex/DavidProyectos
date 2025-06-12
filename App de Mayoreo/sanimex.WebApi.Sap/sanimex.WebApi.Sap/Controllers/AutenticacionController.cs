using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using sanimex.webapi.Dominio.Models;
using sanimex.webapi.Negocio.ControlAcceso;
using sanimex.webapi.Negocio.Seguridad;
using sanimex.webapi.Negocio.Usuarios;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static sanimex.webapi.Dominio.Models.MSimuladorPedidos;

namespace sanimex.WebApi.Sap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        private readonly string? secretKey;
        private readonly IUsuarioNegocio _usuarioNegocio;
        private readonly IControlAccesoNegocio _controlAccesoNegocio;
        public AutenticacionController(IConfiguration config, IUsuarioNegocio usuarioNegocio, IControlAccesoNegocio controlAccesoNegocio)
        {
            secretKey = config.GetSection("settings").GetSection("secretKey").ToString();
            _usuarioNegocio = usuarioNegocio;
            _controlAccesoNegocio = controlAccesoNegocio;
        }
        [HttpPost]
        [Route("Acceso")]

        public async Task<IActionResult> Acceso(int email, string password)
        {
            bool horaEntrada = await _controlAccesoNegocio.validarHora();
            if (horaEntrada)
            {
                Usuario usuario = await _usuarioNegocio.ObtenerAcceso(email);
                if (usuario != null)
                {
                    var seguridadService = new SeguridadNegocio();
                    string hash = seguridadService.ConvertirContraseña(password);
                    if (hash == usuario.contrasena)
                    {
                        var keyBytes = Encoding.ASCII.GetBytes(secretKey);
                        var claims = new ClaimsIdentity();
                        claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario._id));

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
                            idRol = usuario.idRol,
                            message = "Adelante",
                            data = new
                            {
                                user = new
                                {
                                    _id = usuario.idUsuario,
                                    idSucursal = usuario.idSucursal,
                                    idPermiso = usuario.idPermiso,
                                    idRol = usuario.idRol,
                                    numEmpleado = usuario.numEmpleado,
                                    correo = usuario.correo,
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

        [HttpGet]
        [Route("ValidaToken")]
        public  IActionResult ValidaToken()
        {
            // Obtiene el ID del usuario autenticado desde el token
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (usuarioId == null)
            {
                var response = new
                {
                    status = "error",
                    message = "Genera nuevo token",
                };
                return StatusCode(StatusCodes.Status401Unauthorized, response);
            }
            else
            {
                var response = new
                {
                    status = "success",
                    message = "Adelante",
                };
                return StatusCode(StatusCodes.Status200OK, response);
            }

        }
    }
    
    }
