using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sanimex.webapi.Dominio.Models;
using sanimex.webapi.Dominio.Models.Logs;
using sanimex.webapi.Dominio.Models.Usuarios;
using sanimex.webapi.Negocio.Clientes;
using sanimex.webapi.Negocio.Logs;
using sanimex.webapi.Negocio.SapServices;

namespace sanimex.Webapi.Sap.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteNegocio _clienteNegocio;
        private readonly ILogsNegocio _logNegocio;
        private readonly IClienteSapNegocio _clienteSapNegocio;

        public ClienteController(IClienteNegocio clienteNegocio, ILogsNegocio logNegocio, IClienteSapNegocio clienteSapNegocio) // Inyección de dependencia
        {
            _clienteNegocio = clienteNegocio;
            _logNegocio = logNegocio;
            _clienteSapNegocio = clienteSapNegocio;
        }

        [HttpGet]
        [Route("ClientesMayoreo/")]     
        public async Task<IActionResult> GetClientesMayoreo(string idCliente, string empresa, string rfcCte = "")
        {
            try
            {
                bool respuesta = await _clienteSapNegocio.ClientesMayoreo(idCliente, empresa, rfcCte);
                if (respuesta == false)
                {
                    var resultado = new
                    {
                        status = "error",
                        message = "No se encontro el cliente",
                        // Clientes = clientes,
                        // DireccionesClientes = direccionesClientes,
                    };
                    return StatusCode(StatusCodes.Status404NotFound, resultado);
                }
                else
                {
                    var resultado = new
                    {
                        status = "success",
                        message = "Cliente encontrado",
                        // Clientes = clientes,
                        // DireccionesClientes = direccionesClientes,
                    };
                    return StatusCode(StatusCodes.Status200OK, resultado);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            // https://localhost:7149/api/CliMayoreo/ClienteMayoreo?idCliente=4967&canalVenta=1&empresa=GSA
        }

        // Método asíncrono para obtener un usuario por ID
        [HttpGet]
        [Route("Obtener/{id}")]
        public async Task<IActionResult> Detalles(int id)
        {
            // Llama al método ObtenerAcceso de manera asíncrona
            var cliente = await _clienteNegocio.ObtenerCliente(id);

            // Verifica si se encontró el usuario
            if (cliente == null)
                return NotFound("No se encontró el empleado");
            else
                return Ok(cliente);
        }

        [HttpGet]
        [Route("HistoricoVtaMay/")]
        public async Task<ActionResult<List<Historico_Vta_May>>> HistoricoVtaMay(int ClienteSap)
        {
            var empleado = await _clienteNegocio.HistoricoVtaMay(ClienteSap);

            if (empleado == null)
            {
                return NotFound("No se encontró el empleado");
            }
            else
            {
                var response = new
                {
                    status = "success",
                    data = new
                    {
                        product = empleado.Select(producto => new
                        {
                            Codigo = producto.Codigo,
                            Descripcion = producto.Descripcion,
                            IMPORTE_ACTUAL = producto.IMPORTE_ACTUAL,
                            CANTIDAD_ACTUAL = producto.CANTIDAD_ACTUAL
                        })
                    }
                    };
                return Ok(response);
            }
        }
        // Método asíncrono para obtener un usuario por ID
        [HttpPost]
        [Route("GuardarLog/")]
        public async Task<IActionResult> GuardarConsultas(ClienteLogs cliente)
        {
            // Llama al método ObtenerAcceso de manera asíncrona
            bool respuesta = await _logNegocio.GuardarConsultas(cliente);

            // Verifica si se encontró el usuario
            if (!respuesta)
                return NotFound("No se registro el log");
            else
                return Ok("Log Guardado");
        }
        // Método asíncrono para obtener un usuario por ID
        [HttpGet]
        [Route("maxConsultas/")]
        public async Task<IActionResult> maxNumeroClientes(string idUsuario, int numCliente)
        {
            // Llama al método ObtenerAcceso de manera asíncrona
            var cuenta = await _clienteNegocio.maxNumeroClientes(idUsuario, numCliente);

            // Verifica si se encontró el usuario
            if (cuenta == null)
                return NotFound("No se encontró el resultado");
            else
                return Ok(cuenta);
        }
        [HttpGet]
        [Route("MetrosImporteMayoreo/")]
        public async Task<IActionResult> MetrosImporteMayoreo(string ClaveCliente)
        {
            // Llama al método ObtenerAcceso de manera asíncrona
            var conteo = await _clienteNegocio.MetrosImporteMayoreo(ClaveCliente);

            // Verifica si se encontró el usuario
            if (conteo == null)
                return NotFound("No se encontró el resultado");
            else
                return Ok(conteo);
        }
    }
}
