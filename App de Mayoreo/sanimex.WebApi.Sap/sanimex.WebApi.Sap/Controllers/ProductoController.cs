using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sanimex.webapi.Dominio.Models;
using sanimex.webapi.Dominio.Models.Producto;
using sanimex.webapi.Negocio.Clientes;
using sanimex.webapi.Negocio.Producto;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using static sanimex.webapi.Dominio.Models.Disponibilidad;

namespace sanimex.Webapi.Sap.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoNegocio _productoNegocio;
        public ProductoController(IProductoNegocio productoNegocio) // Inyección de dependencia
        {
            _productoNegocio = productoNegocio;
        }
        [HttpGet]
        [Route("Buscar/")]
        public async Task<ActionResult> Buscar(string busqueda)
        {
            int limite = 10;
            // Llama al método ObtenerProducto de manera asíncrona
            var lista = await _productoNegocio.ObtenerProducto(busqueda, limite);

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
                    products = lista.Select(producto => new
                    {
                        _id = producto.Producto.Split(' ')[0], // Suponiendo que tu modelo tiene un campo Id
                        name = producto.Producto.ToUpper().Contains("FLETE") ? producto.Producto : producto.Producto,
                        description = producto.Producto,
                        images = producto.Images != null ? producto.Images.ToList() : new List<string> { producto.Producto.Split(' ')[0]+".jpg" } // Mapeo de la lista de imágenes
                    })
                }
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("CBarraUnitario/")]
        public async Task<ActionResult> BuscarCodigoBarra(string product_id, string centrosCorredor)
        {
            // Obtiene el ID del usuario autenticado desde el token
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // string usuarioId = "531";
            // Llama al método ObtenerProducto de manera asíncrona
            var producto = await _productoNegocio.ProductoUnitario(product_id,centrosCorredor, usuarioId.ToString());

            // Verifica si se encontró el producto
            if (producto == null)
            {
                return NotFound(new { status = "error", message = "No se encontró el producto" });
            }

            // Construir la respuesta en el formato JSON solicitado
             var response = new
             {
                 status = "success",
                     Product = new
                     {
                       // info producto
                    id = producto.Code,
                    Codigo = producto.Code,
                    Description = producto.Description,
                    images = producto.Images != null ? producto.Images.ToList() : new List<string> { producto.Code + ".jpg" },
                    Weight = producto.Weight,
                    PrecioProducto = producto.PrecioProducto,
                    PrecioMetroProducto = producto.PrecioMetroProducto,
                    Color = producto.Color,
                    SquareMeter = producto.SquareMeter,
                    Classification = producto.Classification,
                    Proveedor = producto.Proveedor,

                     },
                 // info disponibles
                    Disponibles = producto.disponibles
                     
             };
            
            return Ok(response);
        }
        //nueva version
        //[HttpGet]
        //[Route("CBarraUnitario/")]
        //public async Task<ActionResult> BuscarCodigoBarra(string product_id, string centrosCorredor, int ClaveCliente)
        //{
        //    // Obtiene el ID del usuario autenticado desde el token
        //    var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    //string usuarioId = "531";
        //    // Llama al método ObtenerProducto de manera asíncrona
        //    var producto = await _productoNegocio.ProductoUnitario(product_id, centrosCorredor, usuarioId.ToString(), ClaveCliente);

        //    // Verifica si se encontró el producto
        //    if (producto == null)
        //    {
        //        return NotFound(new { status = "error", message = "No se encontró el producto" });
        //    }

        //    // Construir la respuesta en el formato JSON solicitado
        //    var response = new
        //    {
        //        status = "success",
        //        Product = new
        //        {
        //            // info producto
        //            id = producto.Code,
        //            Codigo = producto.Code,
        //            Description = producto.Description,
        //            images = producto.Images != null ? producto.Images.ToList() : new List<string> { producto.Code + ".jpg" },
        //            Weight = producto.Weight,
        //            PrecioProducto = producto.PrecioProducto,
        //            PrecioMetroProducto = producto.PrecioMetroProducto,
        //            Color = producto.Color,
        //            SquareMeter = producto.SquareMeter,
        //            Classification = producto.Classification,
        //            Proveedor = producto.Proveedor,

        //        },
        //        // info disponibles
        //        Disponibles = producto.disponibles

        //    };

        //    return Ok(response);
        //}


        //[HttpGet]
        //[Route("CBarraCliente/")]
        //public async Task<ActionResult> BuscarCliente(string product_id, string centrosCorredor, bool tipoEntrega, bool tipoPago, string? ClaveCliente)
        //{
        //    // Obtiene el ID del usuario autenticado desde el token
        //    var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //   //string usuarioId = "531";
        //    // Llama al método ObtenerProducto de manera asíncrona
        //    var producto = await _productoNegocio.ClienteUnitario(product_id, centrosCorredor, tipoEntrega, tipoPago, ClaveCliente, usuarioId);

        //    // Verifica si se encontró el producto
        //    if (producto == null)
        //    {
        //        return NotFound(new { status = "error", message = "No se encontró el producto" });
        //    }
        //    // Construir la respuesta en el formato JSON solicitado
        //    if (producto.Nombre_Completo == "Cliente no existe")
        //    {
        //        return BadRequest("Cliente no existe");
        //    }
        //    else if (producto.Nombre_Completo == "Superaste tu límite de búsqueda por cliente")
        //    {
        //        return BadRequest("Superaste tu límite de búsqueda por cliente");
        //    }
        //    else
        //    {
        //        var response = new
        //        {
        //            status = "success",
        //            Client = new
        //            {
        //                NombreCliente = producto.Nombre_Completo,
        //                Clasificacion = producto.Clasifica,
        //                DescuentoRecoje = producto.descRecoge,
        //                DescuentoContado = producto.descContado,
        //                PrecioFinal = producto.precio_Final,
        //                PrecioMetroFinal = producto.PrecioMetroProducto,
        //                actualimporte = producto.ACTUAL_IMP,
        //                actualmetros = producto.ACTUAL_MT
        //            }
        //        };
        //        return Ok(response);
        //    }

        //}
        // nueva version
        [HttpGet]
        [Route("CBarraCliente/")]
        public async Task<ActionResult> BuscarCliente(string product_id, string centrosCorredor, bool tipoEntrega, bool tipoPago, string? ClaveCliente, bool TipoConsulta, int idUbicacion)
        {
            // Obtiene el ID del usuario autenticado desde el token
             var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //string usuarioId = "531";
            // Llama al método ObtenerProducto de manera asíncrona
            var producto = await _productoNegocio.ClienteUnitario(product_id, centrosCorredor, tipoEntrega, tipoPago, ClaveCliente, usuarioId, TipoConsulta, idUbicacion);

            // Verifica si se encontró el producto
            if (producto == null)
            {
                return NotFound(new { status = "error", message = "No se encontró el producto" });
            }
            // Construir la respuesta en el formato JSON solicitado
            if (producto.Nombre_Completo == "Cliente no existe")
            {
                return BadRequest("Cliente no existe");
            }else if (producto.Nombre_Completo == "Superaste tu límite de búsqueda por cliente") 
            {
                return BadRequest("Superaste tu límite de búsqueda por cliente");
            }
            else
            {
                var response = new
                {
                    status = "success",
                    Client = new
                    {
                        NombreCliente = producto.Nombre_Completo,
                        Clasificacion = producto.Clasifica,
                        DescuentoRecoje = producto.descRecoge,
                        DescuentoContado = producto.descContado,
                        PrecioFinal = producto.precio_Final,
                        PrecioMetroFinal = producto.PrecioMetroProducto,
                        actualimporte = producto.ACTUAL_IMP,
                        actualmetros = producto.ACTUAL_MT
                    }
                };
                return Ok(response);
            }
        }

        [HttpGet]
        [Route("PrecioProducto/{CodigoBarra}")]
        public async Task<IActionResult> Detalles(string CodigoBarra)
        {
            // Llama al método ObtenerAcceso de manera asíncrona
            DataSet producto = await _productoNegocio.GetResultadosElasticSearchAsync(CodigoBarra);

                return Ok(producto);
        }

        // cargar producto
        [HttpGet]
        [Route("Producto")]
        public async Task<IActionResult> ProductoCarrito(string CodigoBarra)
        {
            // Llama al método ObtenerAcceso de manera asíncrona
            MProducto producto = await _productoNegocio.ProductoCarrito(CodigoBarra);

            return Ok(producto);
        }

    }
}
