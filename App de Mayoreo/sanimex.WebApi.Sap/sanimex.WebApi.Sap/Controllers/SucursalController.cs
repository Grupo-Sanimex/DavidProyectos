using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sanimex.webapi.Dominio.Models;
using sanimex.webapi.Dominio.Models.Sucursales;
using sanimex.webapi.Dominio.Models.Usuarios;
using sanimex.webapi.Negocio.Producto;
using sanimex.webapi.Negocio.Sucursales;
using sanimex.webapi.Negocio.Usuarios;
using System.Security.Claims;

namespace sanimex.Webapi.Sap.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class SucursalController : ControllerBase
    {
        private readonly ISucursalNegocio _sucursalNegocio;
        private readonly IUsuarioNegocio _usuarioNegocio;
        public SucursalController(ISucursalNegocio sucursalNegocio, IUsuarioNegocio usuarioNegocio) // Inyección de dependencia
        {
            _sucursalNegocio = sucursalNegocio; 
            _usuarioNegocio = usuarioNegocio;
        }
        [HttpGet]
        [Route("SupervisorSucursal/")]
        public async Task<ActionResult> SupervisorSucursal()
        {
            List<Sucursal> lista;
            // Obtiene el ID del usuario autenticado desde el token
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            int idUsuario = Convert.ToInt32(usuarioId);

            RolUsuario rolUsuario = await _usuarioNegocio.Roles(idUsuario);

            int nivelRol = await _usuarioNegocio.RolePermisos(rolUsuario.idRol);

            if (nivelRol == 1) { 
                lista = await _sucursalNegocio.ObtenerSucursalAdmin(); 
            } else if(nivelRol == 2)
            {
                // Llama al método Obtener ObtenerSucursalSupervisor de manera asíncrona
                lista = await _sucursalNegocio.ObtenerSucursalSupervisor(idUsuario);
            }
            else
            {
                // nivelRol 0 Llama al método Obtenerla unica sucursal
                lista = await _sucursalNegocio.ObtenerSucursal(rolUsuario.idSucursal);
            }

            // Verifica si se encontró el producto
            if (lista == null || lista.Count == 0)
            {
                return NotFound(new { status = "error", message = "No se encontró el producto" });
            }

            // Construir la respuesta en el formato JSON solicitado
            var response = new
            {
                status = "success",
                data = new
                {
                    wishlists = lista.Select(sucursal => new
                    {
                        _idSucursal = sucursal.idSucursal, // Identificador de la wishlist
                        idRol = rolUsuario.idRol,
                        idUsuario = idUsuario, // ID del usuario
                        product = new
                        {
                            _idSucursal = sucursal.idSucursal, // ID de la sucursal dentro de producto
                            nombreSucursal = sucursal.nombre, // Nombre de la sucursal
                            idSAP = sucursal.idSAP ?? "", // ID SAP
                            idUsuario = sucursal.idUsuario ?? "", // ID Usuario
                            idRol = rolUsuario.idRol
                        },
                        __v = 0 // Versión del documento
                    })
                }
            };
            return Ok(response);
        }
    }
}
