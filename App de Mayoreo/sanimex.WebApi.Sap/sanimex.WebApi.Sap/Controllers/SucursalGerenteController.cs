using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sanimex.webapi.Dominio.Models.GerentesSucursal;
using sanimex.webapi.Negocio.GerentesSucurNegocio;
using System.Data;

namespace sanimex.Webapi.Sap.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class SucursalGerenteController : ControllerBase
    {
        private readonly IGerenteSucurNegocio _gerenteSucurNegocio;

        public SucursalGerenteController(IGerenteSucurNegocio gerenteSucurNegocio)
        {
            _gerenteSucurNegocio = gerenteSucurNegocio;
        }

        [HttpGet]
        public async Task<IActionResult> GerenteSucursal(int id)
        {
            // Llama al método ObtenerAcceso de manera asíncrona
            GerenteSucursal gerenteSucursal = await _gerenteSucurNegocio.GerenteSucursal(id);

            return Ok(gerenteSucursal);
        }
        [HttpPost]
        public async Task<IActionResult> GuardarGerenteSucursal(int idGerente, string Sucursales)
        {
            bool res = await _gerenteSucurNegocio.GuardarSucursalGerente(idGerente, Sucursales);
            if (res) {
                return Ok("Sucursales Guardadas");
                    } else {
                return Ok("Error al guardar");
            }
        }
    }
}
