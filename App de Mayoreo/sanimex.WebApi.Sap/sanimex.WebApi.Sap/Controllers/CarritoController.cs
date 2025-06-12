using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sanimex.webapi.Dominio.Models;
using sanimex.webapi.Dominio.Models.Carrito;
using sanimex.webapi.Dominio.Models.Ubicaciones;
using sanimex.webapi.Negocio.Carrito;
using sanimex.webapi.Negocio.Producto;
using System.Security.Claims;
using static sanimex.webapi.Dominio.Models.Disponibilidad;

namespace sanimex.Webapi.Sap.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CarritoController : ControllerBase
    {

        private static Dictionary<string, List<Carro>> _carritosPorUsuario = new Dictionary<string, List<Carro>>();
        private readonly IProductoNegocio _productoNegocio;
        private readonly ICarroNegocio _carroNegocio;
        public CarritoController(IProductoNegocio productoNegocio, ICarroNegocio carroNegocio)
        {
            _productoNegocio = productoNegocio;
            _carroNegocio = carroNegocio;
        }
        // Método para obtener el carrito del usuario
        // Listar carrito
        [HttpGet("listar")]
        public IActionResult ObtenerCarrito()
        {
            // Obtiene el ID del usuario autenticado desde el token
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

             string userId = usuarioId;
            //string? userId = "248";
            // Inicializa el carrito si no existe para el usuario
            if (!_carritosPorUsuario.ContainsKey(userId))
            {
                _carritosPorUsuario[userId] = new List<Carro>();
            }

            // Obtiene el carrito del usuario
            if (_carritosPorUsuario.TryGetValue(userId, out var listaCarritos) && listaCarritos.Any())
            {
                var response = new
                {
                    status = "success",
                    Data = new
                    {
                        Cart = new
                        {
                                 Items = listaCarritos.Select(carrito => new
                                {
                                    Product = new 
                                    {
                                        _id = carrito.codigo,
                                        name = carrito.codigo + " - "+ carrito.descripsion,
                                        images = carrito.Images != null ? carrito.Images.ToList() : new List<string> { carrito.codigo + ".jpg" },
                                        price = carrito.precioFinal,
                                        discount = 0,
                                        inCart = true,
                                        isFav = false,
                                        recoge = carrito.Recoge,
                                        contado = carrito.Contado,
                                        cliente = carrito.ClaveCliente,
                                        id = carrito.codigo,
                                    },
                                     quantity = carrito.cantidad,
                                     _id = carrito.codigo,
                                     id = carrito.codigo
                                 }).ToList() // Convertir a lista
                            }
                        }
                };

                return Ok(response);
            }
            else
            {
                var response = new
                {
                    status = "success",
                    Data = new
                    {
                        Cart = new
                        {
                            Items = listaCarritos.Select(carrito => new
                            {
                                Product = new
                                {
                                    _id = carrito.codigo,
                                    name = carrito.codigo + " - " + carrito.descripsion,
                                    images = carrito.Images != null ? carrito.Images.ToList() : new List<string> { carrito.codigo + ".jpg" },
                                    price = carrito.precioFinal,
                                    discount = 0,
                                    inCart = true,
                                    isFav = false,
                                    recoge = carrito.Recoge,
                                    contado = carrito.Contado,
                                    cliente = carrito.ClaveCliente,
                                    id = carrito.codigo,
                                },
                                quantity = carrito.cantidad,
                                _id = carrito.codigo,
                                id = carrito.codigo
                            }).ToList() // Convertir a lista
                        }
                    }
            };

                return NotFound(response);
            }
        }

        [HttpGet("historialGerenteSucursal")]
        public async Task<IActionResult> HistorialGerenteSucursal(string fechaConsulta)
        {
            try
            {
                // Obtiene el ID del usuario autenticado desde el token
                var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                string? userId = Convert.ToString(usuarioId);
                //string userId = "505";
                List<HisCotizaGerenteSucursal> hisCotizaGerenteSucursals = await _carroNegocio.HisCotizaGerenteSucursal(userId!, fechaConsulta);
                if (hisCotizaGerenteSucursals.Count > 0)
                {
                    var response = new
                    {
                        historicoVtaMay = hisCotizaGerenteSucursals.Select(hisCotizaGerenteSucursals => new
                        {
                            claveSucursal = hisCotizaGerenteSucursals.claveSucursal,
                        }).ToList()
                    };
                    return Ok(response);
                }
                else
                {
                    return NotFound("No se encontraron cotizaciones.");
                }
            }
            catch (Exception Ex)
            {
                return Ok(Results.Problem(Ex.Message));
            }
        }

        [HttpGet("historialGerente")]
        public async Task<IActionResult> HistorialCarrito(string fechaConsulta)
        {
            try
            {
                // Obtiene el ID del usuario autenticado desde el token
                var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                string? userId = Convert.ToString(usuarioId);
                //string userId = "531";
                List<HisCtoMasterGerente> hisCtoMasterGerente = await _carroNegocio.ListarHistorialCMasterGerente(userId!, fechaConsulta);
                if (hisCtoMasterGerente.Count > 0)
                {
                    var response = new
                    {
                        historicoVtaMay = hisCtoMasterGerente.Select(ubicacionS => new
                        {
                            nombre = ubicacionS.nombre,
                            aPaterno = ubicacionS.aPaterno,
                            aMaterno = ubicacionS.aMaterno,
                            numEmpleado = ubicacionS.numEmpleado,
                            idDispositivo = ubicacionS.idDispositivo

                        }).ToList()
                    };

                    return Ok(response);
                }
                else
                {
                    var response = new
                    {
                        historicoVtaMay = hisCtoMasterGerente.Select(ubicacionS => new
                        {
                        }).ToList()
                    };

                    return Ok(response);
                }
            }
            catch (Exception Ex)
            {
                return Ok(Results.Problem(Ex.Message));
            }
        }
        [HttpGet("hisVisitadorGerente")]
        public async Task<IActionResult> HistorialCarritoVisitador(string fechaConsulta, string idvistador)
        {
            try
            {
                List<HisCtoMaster> hisCtoMaster = await _carroNegocio.ListarHistorialCMaster(idvistador!, fechaConsulta);
                if (hisCtoMaster.Count > 0)
                {
                    var response = new
                    {
                        historicoVtaMay = hisCtoMaster.Select(hisCtoMaster => new
                        {
                            idCotizacion = hisCtoMaster.idCotizacion,
                            totalCotizacion = hisCtoMaster.totalCotizacion,
                            idClienteSAP = hisCtoMaster.idClienteSAP,
                            status = hisCtoMaster.Status,
                            fecha = hisCtoMaster.fecha,
                            hora = hisCtoMaster.hora,
                            idventa = hisCtoMaster.idventa ?? string.Empty,
                        }).ToList()
                    };
                    return Ok(response);
                }
                else
                {
                    return NotFound("No se encontraron cotizaciones.");
                }
            }
            catch (Exception Ex)
            {
                return Ok(Results.Problem(Ex.Message));
            }
        }

        [HttpGet("historialVisitador")]
        public async Task<IActionResult> HistorialCarritoVisitador(string fechaConsulta)
        {
            try
            {
                // Obtiene el ID del usuario autenticado desde el token
                var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                string? userId = Convert.ToString(usuarioId);
                //string userId = "3079";
                List<HisCtoMaster> hisCtoMaster = await _carroNegocio.ListarHistorialCMaster(userId!, fechaConsulta);
                if (hisCtoMaster.Count > 0)
                {
                    var response = new
                    {
                        historicoVtaMay = hisCtoMaster.Select(hisCtoMaster => new
                        {
                            idCotizacion = hisCtoMaster.idCotizacion,
                            totalCotizacion = hisCtoMaster.totalCotizacion,
                            idClienteSAP = hisCtoMaster.idClienteSAP,
                            status = hisCtoMaster.Status,
                            fecha = hisCtoMaster.fecha,
                            hora = hisCtoMaster.hora,
                            idventa = hisCtoMaster.idventa ?? string.Empty,
                        }).ToList()
                    };
                    return Ok(response);
                }
                else
                {
                    return NotFound("No se encontraron cotizaciones.");
                }
            }
            catch (Exception Ex)
            {
                return Ok(Results.Problem(Ex.Message));
            }
        }

        [HttpGet("CotizacionDetalle")]
        public async Task<IActionResult> CotizacionDetalle(string idCotizacion)
        {
            try
            {
                // Obtiene el ID del usuario autenticado desde el token
                var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                string? userId = Convert.ToString(usuarioId);
                //string userId = "248";
                List<CotizacionDetalle> cotizacionDetalle = await _carroNegocio.CotizacionDetalle(userId!, idCotizacion);
                HisCtoCliente? hisCtoCliente = await _carroNegocio.ConsultaCliente(userId!, idCotizacion);
                if (cotizacionDetalle.Count > 0)
                {
                    var response = new
                    {
                        cotizacionDetalle = cotizacionDetalle.Select(cotizacionDetalle => new
                        {
                            codebar = cotizacionDetalle.codebar,
                            description = cotizacionDetalle.description,
                            cantidad = cotizacionDetalle.cantidad,
                            precioUnitario = cotizacionDetalle.precioUnitario,
                            status = cotizacionDetalle.status,
                            cliente = hisCtoCliente!.nombreCliente ,
                            clasificacion = hisCtoCliente.clasificacion
                        }).ToList()
                    };
                    return Ok(response);
                }
                else
                {
                    return NotFound("No se encontraron cotizaciones.");
                }
            }
            catch (Exception Ex)
            {
                return Ok(Results.Problem(Ex.Message));
            }
        }
        private List<Carro> ObtenerCarritoPorUsuario()
        {
            // Obtiene el ID del usuario autenticado desde el token
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            string? userId = Convert.ToString(usuarioId);
            //string? userId = "248";

            if (!_carritosPorUsuario.ContainsKey(userId!))
            {
                _carritosPorUsuario[userId!] = new List<Carro>();
            }
            return _carritosPorUsuario[userId!];
        }

        [HttpPost("agregar")]
        public async Task<IActionResult> AgregarProducto(string codigo, string descripsion, string sucursal,
     float precioFinal, int cantidad, string ClaveCliente, bool Recoge, bool Contado, bool tipo_consulta)
        {
            string tipo = "0";
            if (tipo_consulta)
            {
                tipo = "1";
            }
            // Get authenticated user ID from token
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(usuarioId))
            {
                return Unauthorized("Usuario no autenticado");
            }

            var carrito = ObtenerCarritoPorUsuario();

            // Create new product
            var nuevoProducto = new Carro
            {
                codigo = codigo,
                descripsion = descripsion,
                sucursal = sucursal,
                precioFinal = precioFinal,
                cantidad = cantidad,
                ClaveCliente = ClaveCliente,
                Recoge = Recoge,
                Contado = Contado,
                tipo_consulta = tipo
            };

            // If cart is empty, add first product and return
            if (!carrito.Any())
            {
                carrito.Add(nuevoProducto);
                return Ok(carrito);
            }

            // Validate client consistency
            var primerProducto = carrito.First();
            if (!primerProducto.ClaveCliente.Equals(nuevoProducto.ClaveCliente))
            {
                return BadRequest("Todos los productos deben pertenecer al mismo cliente");
            }

            // Validate Recoge and Contado consistency with first product
            if (primerProducto.Recoge != nuevoProducto.Recoge ||
                primerProducto.Contado != nuevoProducto.Contado)
            {
                return BadRequest("Cambio Recoge o Entrega: Todos los productos deben mantener el mismo estado de Recoge y Contado que el primer producto");
            }

            // Check for existing product with same characteristics
            var productoExistente = carrito.FirstOrDefault(p =>
                p.codigo == nuevoProducto.codigo &&
                p.sucursal == nuevoProducto.sucursal &&
                p.Recoge == nuevoProducto.Recoge &&
                p.Contado == nuevoProducto.Contado);

            if (productoExistente != null)
            {
                productoExistente.cantidad += nuevoProducto.cantidad;
            }
            else
            {
                // Validate maximum cart size
                if (carrito.Count >= 25)
                {
                    return BadRequest("El carrito no puede tener más de 25 productos diferentes");
                }
                carrito.Add(nuevoProducto);
            }

            return Ok(carrito);
        }

        [HttpPost("InsertCarrito")]
        public async Task<IActionResult> InsertCarrito()
        {
            try
            {
                // Obtiene el ID del usuario autenticado desde el token
                var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                string? userId = Convert.ToString(usuarioId);

                int idGenerado = await _carroNegocio.GuardarConsultas(userId!, _carritosPorUsuario[userId!]);
                bool completo = await _carroNegocio.GuardarCodigoAsync(idGenerado, _carritosPorUsuario[userId!]);
                if (completo == true)
                {
                    _carritosPorUsuario[userId!] = new List<Carro>();
                }
                return Ok(completo);
            }
            catch (Exception Ex)
            {
                return Ok(Results.Problem(Ex.Message));
            }
        }


        // Eliminar producto
        [HttpDelete("eliminar")]
        public async Task<IActionResult> EliminarProducto(string codigo)
        {
            // Obtiene el ID del usuario autenticado desde el token
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            string? userId = Convert.ToString(usuarioId);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Se requiere un identificador de usuario.");
            }

            var carrito = ObtenerCarritoPorUsuario();
            var producto = carrito.FirstOrDefault(p => p.codigo == codigo);
            if (producto == null)
            {
                return NotFound("El producto no se encontró en el carrito.");
            }

            carrito.Remove(producto);
            return Ok(carrito);
        }

        // Vaciar carrito
        [HttpDelete("vaciar")]
        public async Task<IActionResult> VaciarCarrito()
        {
            // Obtiene el ID del usuario autenticado desde el token
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            string? userId = Convert.ToString(usuarioId);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Se requiere un identificador de usuario.");
            }

            var carrito = ObtenerCarritoPorUsuario();
            carrito.Clear();
            return Ok("El carrito ha sido vaciado.");
        }

        // Incrementar producto
        [HttpDelete("incrementar")]
        public async Task<IActionResult> IncrementarProducto(string codigo, string sucursal)
        {
            // Obtiene el ID del usuario autenticado desde el token
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            string? userId = Convert.ToString(usuarioId);
            //string? userId = "248";
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Se requiere un identificador de usuario.");
            }

            var carrito = ObtenerCarritoPorUsuario();
            var producto = carrito.FirstOrDefault(p => p.codigo == codigo && p.sucursal == sucursal);
            if (producto == null)
            {
                return NotFound("El producto no se encontró en el carrito.");
            }

            producto.cantidad++;
            return Ok(carrito);
        }

        [HttpDelete("incrementarDiez")]
        public async Task<IActionResult> IncrementarProductoDiez(string codigo, string sucursal)
        {
            int incremento = 10;
            // Obtiene el ID del usuario autenticado desde el token
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            string? userId = Convert.ToString(usuarioId);
            //string? userId = "248";
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Se requiere un identificador de usuario.");
            }

            var carrito = ObtenerCarritoPorUsuario();
            var producto = carrito.FirstOrDefault(p => p.codigo == codigo && p.sucursal == sucursal);
            if (producto == null)
            {
                return NotFound("El producto no se encontró en el carrito.");
            }

            producto.cantidad += incremento;
            return Ok(carrito);
        }

        // Decrementar producto
        [HttpDelete("decrementar")]
        public async Task<IActionResult> DecrementarProducto(string codigo, string sucursal)
        {
            // Obtiene el ID del usuario autenticado desde el token
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            string? userId = Convert.ToString(usuarioId);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Se requiere un identificador de usuario.");
            }
            var carrito = ObtenerCarritoPorUsuario();
            var producto = carrito.FirstOrDefault(p => p.codigo == codigo && p.sucursal == sucursal);
            if (producto == null)
            {
                return NotFound("El producto no se encontró en el carrito.");
            }

            if (producto.cantidad > 1)
            {
                producto.cantidad--;
            }
            else
            {
                producto.cantidad = 1;
            }

            return Ok(carrito);
        }

        [HttpDelete("decrementarDiez")]
        public async Task<IActionResult> DecrementarProductoDiez(string codigo, string sucursal)
        {
            // Obtiene el ID del usuario autenticado desde el token
            var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            string? userId = Convert.ToString(usuarioId);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Se requiere un identificador de usuario.");
            }
            var carrito = ObtenerCarritoPorUsuario();
            var producto = carrito.FirstOrDefault(p => p.codigo == codigo && p.sucursal == sucursal);
            if (producto == null)
            {
                return NotFound("El producto no se encontró en el carrito.");
            }

            if (producto.cantidad > 10)
            {
                producto.cantidad-=10;
            }
            else
            {
                producto.cantidad--;
            }
            return Ok(carrito);
        }


    }

}
