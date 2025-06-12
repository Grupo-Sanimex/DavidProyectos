using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sanimex.WebApi.Sap.Services;

namespace sanimex.WebApi.Sap.Controllers
{
    //[EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CliMayoreoController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        private readonly IDisponibilidadService _disponibilidadService;
        private readonly IDispoMayoreoNegocio _disponibilidadMayoreo;
        private readonly ISimuladorPieza _simuladorPieza;
        private readonly ISimuladorPedidos _simuladorPedidos;

        public CliMayoreoController(IClienteService clienteService, IDisponibilidadService disponibilidadService, IDispoMayoreoNegocio disponibilidadMayoreo, ISimuladorPieza simuladorPieza, ISimuladorPedidos simuladorPedidos)
        {
            _clienteService = clienteService;
            _disponibilidadService = disponibilidadService;
            _disponibilidadMayoreo = disponibilidadMayoreo;
            _simuladorPieza = simuladorPieza;
            _simuladorPedidos = simuladorPedidos;
        }

         [HttpGet("ClienteMayoreo")]
         public async Task<IActionResult> GetClientesMayoreo(string idCliente, int canalVenta, string empresa, string rfcCte = "")
         {
             try
             {
                 var resultado = await _clienteService.ClientesMayoreo(idCliente,canalVenta,empresa, rfcCte);
                 return Ok(resultado);
             }
             catch (Exception ex)
             {
                 return StatusCode(500, ex.Message);
             }
             // https://localhost:7149/api/CliMayoreo/ClienteMayoreo?idCliente=4967&canalVenta=1&empresa=GSA
         }
         // Método del controlador que utiliza el servicio
         [HttpGet("DisponibilidadCentros")]
         public async Task<IActionResult> GetDisponibilidadxCentros(string barcode, string descripcion = "", string centrosCorredor = "", string sucHijoSap = "")
         {
             try
             {
                 // Llama al método del servicio
                 var resultado = await _disponibilidadService.DisponibilidadxCentrosAsync(barcode, descripcion, centrosCorredor, sucHijoSap);
                 return Ok(resultado);  // Devuelve el resultado en formato JSON
             }
             catch (Exception ex)
             {
                 // Devuelve un error 500 si algo sale mal
                 return StatusCode(500, $"Error interno: {ex.Message}");
             }
             // https://localhost:7149/api/CliMayoreo/DisponibilidadCentros?barcode=C20-02-0-01&centrosCorredor=G001&sucHijoSap=G001
         }

        // Método del controlador que utiliza el servicio
        [HttpGet("DisponibilidadMayoreo")]
        public async Task<IActionResult> GetDisponibilidadxMayoreo(string barcode, string centrosCorredor, bool soloExistencias = true)
        {
            try
            {
                // Llama al método del servicio
                var resultado = await _disponibilidadMayoreo.DisponibilidadxMayoreoAsync(barcode, centrosCorredor, soloExistencias);
                return Ok(resultado);  // Devuelve el resultado en formato JSON
            }
            catch (Exception ex)
            {
                // Devuelve un error 500 si algo sale mal
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
            // https://localhost:7149/api/CliMayoreo/DisponibilidadMayoreo?barcode=C20-02-0-01&centrosCorredor=G001&sucursalesDepend=G001
        }
        // Método del controlador que utiliza el servicio
        [HttpGet("SimuladorPieza")]
        public async Task<IActionResult> GetSimuladorPieza(string barcode, string noCliente, string centrosCorredor)
        {
            try
            {
                // Llama al método del servicio
                var resultado = await _simuladorPieza.SimuladorPiezaAsync(barcode, noCliente, centrosCorredor);
                return Ok(resultado);  // Devuelve el resultado en formato JSON
            }
            catch (Exception ex)
            {
                // Devuelve un error 500 si algo sale mal
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
            // https://localhost:7149/api/CliMayoreo/SimuladorPieza?barcode=C20-02-0-01&noCliente=4967&centrosCorredor=G001
        }
        // Método del controlador que utiliza el servicio
        [HttpGet("SimuladorPedidos")]
        public async Task<IActionResult> SimuladarPedidos(string codigosProductos, string noCliente, string canalVenta, string empresa, string claveSap, bool validador = true)
        {
            try
            {
                // Llama al método del servicio
                var resultado = await _simuladorPedidos.SimuladarPedidos(codigosProductos, noCliente, canalVenta, empresa, claveSap, validador);
                return Ok(resultado);  // Devuelve el resultado en formato JSON
            }
            catch (Exception ex)
            {
                // Devuelve un error 500 si algo sale mal
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
            // https://localhost:7149/api/CliMayoreo/SimuladorPedidos?codigosProductos=C20-02-0-01,C20-12-0-06&noCliente=4967&canalVenta=1&empresa=GSA&claveSap=G001
        }
    }
}
