using Nancy.Json;
using Newtonsoft.Json;
using sanimex.webapi.Datos.Servicio.ClienteServicio.Implementacion;
using sanimex.webapi.Datos.Servicio.ClienteServicio.Interfaces;
using sanimex.webapi.Datos.Servicio.ProductoServicio;
using sanimex.webapi.Datos.Servicio.ProductoServicio.Implementacion;
using sanimex.webapi.Datos.Servicio.SucursalService;
using sanimex.webapi.Dominio.Models;
using sanimex.webapi.Dominio.Models.Clientes;
using sanimex.webapi.Dominio.Models.Logs;
using sanimex.webapi.Dominio.Models.Mayoreo;
using sanimex.webapi.Dominio.Models.Producto;
using sanimex.webapi.Dominio.Models.Sucursales;
using sanimex.webapi.Negocio.Clientes;
using sanimex.webapi.Negocio.Logs;
using sanimex.webapi.Negocio.SapServices;
using sanimex.WebApi.Sap.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static sanimex.webapi.Dominio.Models.Disponibilidad;
using static System.Net.Mime.MediaTypeNames;


namespace sanimex.webapi.Negocio.Producto
{
    public class ProductoNegocio : IProductoNegocio
    {
        private readonly IProductoService _productoService;
        private readonly IDispoMayoreoNegocio _dispoMayoreoNegocio;
        private readonly ISimuladorPedidoNegocio _simuladorPedido;
        private readonly ISucursalService _sucursalService;
        private readonly ILogsNegocio _logsNegocio;
        private readonly IClienteNegocio _clienteNegocio;

        string _CodigoBarra = "";
        string _centrosCorredor = "";
        string _ClaveSap = "";
        bool _tipoEntrega = false;
        bool _tipoPago = false;
        string _claveCliente = string.Empty;
        string _idUsuario = string.Empty;
        bool _tipoConsulta = false; // Variable para determinar el tipo de consulta (mayoreo o cliente)
        int _idDireccion = 0;
        public ProductoNegocio(IProductoService productoService, IDispoMayoreoNegocio dispoMayoreoNegocio, ISimuladorPedidoNegocio simuladorPedido, ISucursalService sucursalService, ILogsNegocio logsNegocio, IClienteNegocio clienteNegocio)
        {
            _productoService = productoService;
            _dispoMayoreoNegocio = dispoMayoreoNegocio;
            _simuladorPedido = simuladorPedido;
            _sucursalService = sucursalService;
            _logsNegocio = logsNegocio;
            _clienteNegocio = clienteNegocio;
        }

        // extraer datops del producto

        public async Task<MProducto> ProductoCarrito(string CodigoBarra)
        {
            return await _productoService.Producto(CodigoBarra);
        }
        public async Task<List<BusquedaProducto>> ObtenerProducto(string busqueda, int limite)
        {
            try
            {
                if (string.IsNullOrEmpty(busqueda))
                {
                    throw new ArgumentException("La búsqueda no puede estar vacía.");
                }
                return await _productoService.ObtenerProducto(busqueda, limite);
            }
            catch (ArgumentException ex)
            {
                // Manejo específico para argumentos inválidos
                // Registrar el error o lanzar una excepción manejable
                throw new ArgumentException($"Error en los parámetros de entrada: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Manejo general de errores
                // Registrar el error o lanzar una excepción manejable
                // Aquí puedes usar una herramienta de logging como Serilog o NLog
                // Logger.LogError(ex, "Error al obtener el producto");
                throw new Exception($"Error al obtener el producto: {ex.Message}");
            }
        }



         public async Task<ProductoUnitario> ProductoUnitario(string CodigoBarra, string centrosCorredor, string idUsuario)
          {
              _CodigoBarra = CodigoBarra;
              _centrosCorredor = centrosCorredor;
              _ClaveSap = centrosCorredor;
              _idUsuario = idUsuario;

              if (Regex.IsMatch(centrosCorredor, @"^N"))
              {
                  _claveCliente = "0000011533";
              }
              else{
                  _claveCliente = "0000003223";
              }

              try
              {
                  DispoMayoreo? dispoMayoreo;
                  if (string.IsNullOrEmpty(CodigoBarra))
                  {
                      throw new ArgumentException("La búsqueda no puede estar vacía.");
                  }
                  ProductoUnitario mProducto = await CargarProductoUnitario();
                  // crear log de la busqueda completada correctamente e insertar en la bd
                  // Obtiene el ID del usuario autenticado desde el token
                  return mProducto;

              }
              catch (ArgumentException ex)
              {
                  throw new ArgumentException($"Error en los parámetros de entrada: {ex.Message}");
              }
              catch (Exception ex)
              {
                  throw new Exception($"Error al obtener el producto: {ex.Message}");
              }
          }


        // nueva version
        //public async Task<ProductoUnitario> ProductoUnitario(string CodigoBarra, string centrosCorredor, string idUsuario, int claveCliente)
        //{
        //    _CodigoBarra = CodigoBarra;
        //    _centrosCorredor = centrosCorredor;
        //    _ClaveSap = centrosCorredor;
        //    _idUsuario = idUsuario;
        //    _claveCliente = claveCliente.ToString();
        //    try
        //    {
        //        DispoMayoreo? dispoMayoreo;
        //        if (string.IsNullOrEmpty(CodigoBarra))
        //        {
        //            throw new ArgumentException("La búsqueda no puede estar vacía.");
        //        }
        //        ProductoUnitario mProducto = await CargarProductoUnitario();
        //        // crear log de la busqueda completada correctamente e insertar en la bd
        //        // Obtiene el ID del usuario autenticado desde el token
        //        return mProducto;

        //    }
        //    catch (ArgumentException ex)
        //    {
        //        throw new ArgumentException($"Error en los parámetros de entrada: {ex.Message}");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Error al obtener el producto: {ex.Message}");
        //    }
        //}



        //public async Task<ProductoCliente> ClienteUnitario(string CodigoBarra, string centrosCorredor, bool tipoEntrega, bool tipoPago, string ClaveCliente, string idUsuario)
        //{
        //    _CodigoBarra = CodigoBarra;
        //    _centrosCorredor = centrosCorredor;
        //    _ClaveSap = centrosCorredor;
        //    _tipoEntrega = tipoEntrega;
        //    _tipoPago = tipoPago;
        //    _claveCliente = ClaveCliente;
        //    _idUsuario = idUsuario;
        //    _tipoConsulta = false;
        //    try
        //    {
        //        DispoMayoreo? dispoMayoreo;
        //        if (string.IsNullOrEmpty(ClaveCliente))
        //        {
        //            throw new ArgumentException("La búsqueda no puede estar vacía.");
        //        }
        //        ProductoCliente mProducto = await CargarClienteBusqueda();
        //        // crear log de la busqueda completada correctamente e insertar en la bd
        //        // Obtiene el ID del usuario autenticado desde el token
        //        return mProducto;

        //    }
        //    catch (ArgumentException ex)
        //    {
        //        throw new ArgumentException($"Error en los parámetros de entrada: {ex.Message}");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Error al obtener el producto: {ex.Message}");
        //    }
        //}
        // nueva version
        public async Task<ProductoCliente> ClienteUnitario(string CodigoBarra, string centrosCorredor, bool tipoEntrega, bool tipoPago, string ClaveCliente, string idUsuario, bool TipoConsulta, int idDireccion)
        {
            _CodigoBarra = CodigoBarra;
            _centrosCorredor = centrosCorredor;
            _ClaveSap = centrosCorredor;
            _tipoEntrega = tipoEntrega;
            _tipoPago = tipoPago;
            _claveCliente = ClaveCliente;
            _idUsuario = idUsuario;
            _tipoConsulta = TipoConsulta;
            _idDireccion = idDireccion;
            try
            {
                DispoMayoreo? dispoMayoreo;
                if (string.IsNullOrEmpty(ClaveCliente))
                {
                    throw new ArgumentException("La búsqueda no puede estar vacía.");
                }
                ProductoCliente mProducto = await CargarClienteBusqueda();
                // crear log de la busqueda completada correctamente e insertar en la bd
                // Obtiene el ID del usuario autenticado desde el token
                return mProducto;

            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Error en los parámetros de entrada: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el producto: {ex.Message}");
            }
        }

        public async Task<DataSet> GetResultadosElasticSearchAsync(string CodigoBarra) // Implementación del método
        {
            return await _productoService.GetResultadosElasticSearchAsync(CodigoBarra, "", "", ""); // Llama al método de la capa de datos
        }


        public async Task<ProductoCliente> CargarClienteBusqueda()
        {
            string CodigoBarraAnterior = "";
            decimal PrecioProducto = 0;
            ProductoCliente producto;
            try
            {
                List<MDisponibilidadMayoreo> productos = await _dispoMayoreoNegocio.DisponibilidadxMayoreoAsync(_CodigoBarra, _centrosCorredor);
                // Si quieres devolver un solo producto, puedes acceder al primer elemento
                MDisponibilidadMayoreo mDisponibilidadMayoreo = productos.FirstOrDefault();


                DataSet setResultados = await _productoService.GetResultadosElasticSearchAsync(_CodigoBarra, "", "", "");

                if (setResultados != null && setResultados.Tables.Count > 0)
                {
                    var tabResultados = setResultados.Tables["resd"];

                    if (tabResultados != null && tabResultados.Rows.Count >= 1)
                    {
                        if (tabResultados.Rows.Count == 1)
                        {
                            DataRow rowProducto = tabResultados.Rows[0];

                            Hashtable preciosPadre = await ObtenerDescuentoSimulador(rowProducto["Sanimex.Product.Code"].ToString(), _claveCliente);
                            PrecioProducto = Math.Round(Convert.ToDecimal(preciosPadre["PrecioProducto"]) * 1.16M, 2);

                            PrecioCliente precioCliente = await CalculaDescuentos_Mayoreo(_tipoEntrega, _tipoPago, _claveCliente, PrecioProducto);

                            Metros_Importe_Mayoreo metros_Importe_Mayoreo = await _clienteNegocio.MetrosImporteMayoreo(_claveCliente);
                            float metrosXCaja = await _productoService.MetroXCaja(_CodigoBarra);
                            string maxNumeroCliente = string.Empty;
                            if (_claveCliente != "")
                            {
                                maxNumeroCliente = await _clienteNegocio.maxNumeroClientes(_idUsuario, Convert.ToInt32(_claveCliente));
                            }

                            if (precioCliente.Nombre_Completo == "Cliente no existe")
                            {
                                producto = new ProductoCliente
                                {
                                    Nombre_Completo = precioCliente.Nombre_Completo,
                                    Clasifica = precioCliente.Clasifica ?? "",
                                    descRecoge = "",
                                    descContado = "",
                                    precio_Final = 0,
                                    ACTUAL_IMP = 0.0,
                                    ACTUAL_MT = 0.0
                                };
                            }
                            else
                            {
                                producto = new ProductoCliente
                                {
                                    Nombre_Completo = precioCliente.Nombre_Completo,
                                    Clasifica = precioCliente.Clasifica ?? "",
                                    descRecoge = precioCliente.descRecoge,
                                    descContado = precioCliente.descContado,
                                    precio_Final = precioCliente.precio_Final,
                                    PrecioMetroProducto = (float)Math.Round(precioCliente.precio_Final / metrosXCaja, 2),
                                    ACTUAL_IMP = metros_Importe_Mayoreo.ACTUAL_IMP,
                                    ACTUAL_MT = metros_Importe_Mayoreo.ACTUAL_MT
                                };
                                if ((_claveCliente != null && maxNumeroCliente != "Superaste tu límite de búsqueda por cliente")
                                    && (_tipoEntrega ^ _tipoPago))
                                {
                                    ClienteLogs clienteLogs = new ClienteLogs();
                                    clienteLogs.idUsuario = _idUsuario;
                                    clienteLogs.numCliente = _claveCliente;
                                    clienteLogs.idSAP = _centrosCorredor;
                                    clienteLogs.claveArticulo = rowProducto["Sanimex.Product.Code"].ToString();
                                    clienteLogs.TipoConsulta = _tipoConsulta;
                                    clienteLogs.idDireccion = _idDireccion;
                                    await _logsNegocio.GuardarConsultas(clienteLogs);
                                    CodigoBarraAnterior = _CodigoBarra;
                                }else if (CodigoBarraAnterior != _CodigoBarra && (_tipoEntrega || _tipoPago))
                                {
                                    ClienteLogs clienteLogs = new ClienteLogs();
                                    clienteLogs.idUsuario = _idUsuario;
                                    clienteLogs.numCliente = _claveCliente;
                                    clienteLogs.idSAP = _centrosCorredor;
                                    clienteLogs.claveArticulo = rowProducto["Sanimex.Product.Code"].ToString();
                                    clienteLogs.TipoConsulta = _tipoConsulta;
                                    clienteLogs.idDireccion = _idDireccion;
                                    await _logsNegocio.GuardarConsultas(clienteLogs);
                                    CodigoBarraAnterior = _CodigoBarra;
                                }
                            }
                            if (maxNumeroCliente == "Superaste tu límite de búsqueda por cliente")
                            {
                                producto = new ProductoCliente
                                {
                                    Nombre_Completo = maxNumeroCliente,
                                    Clasifica = "",
                                    descRecoge = "",
                                    descContado = "",
                                    precio_Final = 0,
                                    ACTUAL_IMP = 0.0,
                                    ACTUAL_MT = 0.0
                                };
                            }
                            return producto; // Ruta que devuelve un producto         
                        }
                        else
                        {
                            Console.WriteLine("Se encontraron múltiples resultados, no se puede procesar.", "Atención");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No se encontró ningún resultado con la palabra de búsqueda, por favor verifique sus parámetros.", "Atención");
                    }
                }
                else
                {
                    Console.WriteLine("Error al consultar productos en la base de datos.", "Atención");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en la búsqueda de productos para mostrar en pantalla: " + ex.Message, "Atención");
            }
            return null; // Puedes devolver null o una instancia de CargarProducto vacía, según tu necesidad
        }

        public async Task<ProductoUnitario> CargarProductoUnitario()
        {
            decimal PrecioProducto = 0;
            ProductoUnitario producto;
            try
            {
                List<MDisponibilidadMayoreo> productos = await _dispoMayoreoNegocio.DisponibilidadxMayoreoAsync(_CodigoBarra, _centrosCorredor);

                DataSet setResultados = await _productoService.GetResultadosElasticSearchAsync(_CodigoBarra, "", "", "");

                if (setResultados != null && setResultados.Tables.Count > 0)
                {
                    var tabResultados = setResultados.Tables["resd"];

                    if (tabResultados != null && tabResultados.Rows.Count >= 1)
                    {
                        if (tabResultados.Rows.Count == 1)
                        {
                            DataRow rowProducto = tabResultados.Rows[0];

                            Hashtable preciosPadre = await ObtenerDescuentoSimulador(rowProducto["Sanimex.Product.Code"].ToString(), _claveCliente);

                            double precioProducto = Convert.ToDouble(preciosPadre["PrecioProducto"]) * 1.16;
                            double metrosCuadrados = Convert.ToDouble(rowProducto["Sanimex.FeatureProduct.SquareMeter"]);

                            if (metrosCuadrados == 0)
                            {
                                throw new DivideByZeroException("El valor de metros cuadrados no puede ser 0.");
                            }
                            float precioMetroProducto = (float)Math.Round(precioProducto / metrosCuadrados, 2);
                            producto = new ProductoUnitario
                            {
                                disponibles = productos,
                                Code = rowProducto["Sanimex.Product.Code"].ToString(),
                                Description = rowProducto["Sanimex.Product.Description"].ToString(),
                                Weight = rowProducto["Sanimex.FeatureProduct.Weight"].ToString(),
                                PrecioProducto = (float)Math.Round(Convert.ToDouble(preciosPadre["PrecioProducto"]) * 1.16, 2),
                                PrecioMetroProducto = precioMetroProducto,
                                Color = rowProducto["Sanimex.FeatureProduct.Color"].ToString(),
                                SquareMeter = rowProducto["Sanimex.FeatureProduct.SquareMeter"].ToString(),
                                Classification = rowProducto["Sanimex.ClassificationProduct.Classification"].ToString(),
                                Proveedor = rowProducto["Sanimex.ClassificationProduct.Provider"]?.ToString() ?? ""
                            };
                            return producto; // Ruta que devuelve un producto         
                        }
                        else
                        {
                            Console.WriteLine("Se encontraron múltiples resultados, no se puede procesar.", "Atención");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No se encontró ningún resultado con la palabra de búsqueda, por favor verifique sus parámetros.", "Atención");
                    }
                }
                else
                {
                    Console.WriteLine("Error al consultar productos en la base de datos.", "Atención");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en la búsqueda de productos para mostrar en pantalla: " + ex.Message, "Atención");
            }

            // Asegúrate de devolver un valor en caso de error o si no se encontró un producto
            return null; // Puedes devolver null o una instancia de CargarProducto vacía, según tu necesidad
        }

        public async Task<PrecioCliente> CalculaDescuentos_Mayoreo(bool tipoEntrega, bool tipoPago, string? ClaveCliente, decimal PrecioProducto)
        {
            string tipoClienteClasificado;
            decimal descClasRecoge = 0;
            decimal desClasContado = 0;

            decimal subtotal2 = 0;
            decimal subtotal1 = 0;
            decimal precioEspecialMay = 0;
            // datos para el modelo 
            string Nombre_Completo = "";
            string Clasifica = "";
            string descRecoge = "";
            string DescContado = "";
            float Precio_Final = 0;
            try
            {
                int facturaBonificacion = 0;
                string tipoCondicion = "";

                tipoCondicion = tipoPago ? "Contado" : "Credito";

                int posicion = 10;
                int productoBloqueadoNC;
                if (ClaveCliente == null)
                {
                    Nombre_Completo = "";
                }
                else
                {
                    Nombre_Completo = await _productoService.Nombre_Cliente_Cotiza_May(ClaveCliente);

                    if (string.IsNullOrEmpty(Nombre_Completo))
                    {
                        Nombre_Completo = "Cliente no existe";
                    }
                }
                tipoClienteClasificado = await _productoService.Tipo_Cliente_Clas_May(ClaveCliente);
                Clasifica = tipoClienteClasificado;
                productoBloqueadoNC = await _productoService.Productos_Bloqueados_NC(_CodigoBarra, _ClaveSap);

                descClasRecoge = await _productoService.Descuentos_Especiales_Mayoreo(tipoClienteClasificado, _CodigoBarra, "Recoge", _ClaveSap);
                desClasContado = await _productoService.Descuentos_Especiales_Mayoreo(tipoClienteClasificado, _CodigoBarra, "Contado", _ClaveSap);


                decimal nvoPrecioMaterial = 0;

                if (productoBloqueadoNC == 0 && facturaBonificacion == 0)
                {
                    if (tipoEntrega)
                    {
                        descClasRecoge = descClasRecoge == 0 ? 0.04m : Math.Round(descClasRecoge, 2);
                        subtotal1 = PrecioProducto;
                        double descuentoEntrega = (double)(subtotal1 * descClasRecoge);
                        double pctAplicar = 1 - (double)descClasRecoge;
                        decimal antPrecio = subtotal1;
                        nvoPrecioMaterial = Math.Round(antPrecio * (decimal)pctAplicar, 2);

                        precioEspecialMay = nvoPrecioMaterial;
                        subtotal2 = Math.Round(nvoPrecioMaterial, 2);
                        descRecoge = (descClasRecoge * 100).ToString();
                        precioEspecialMay = nvoPrecioMaterial;
                    }


                    if (tipoCondicion == "Contado")
                    {
                        if (subtotal2 == 0)
                        {
                            subtotal2 = PrecioProducto;
                        }
                        desClasContado = desClasContado == 0 ? 0.02m : Math.Round(desClasContado, 2);
                        double descuentoPago = (double)(subtotal2 * desClasContado);
                        double pctAplicar = 1 - (double)desClasContado;
                        decimal antPrecio = subtotal2;
                        nvoPrecioMaterial = Math.Round(antPrecio * (decimal)pctAplicar, 2);

                        DescContado = (desClasContado * 100).ToString();
                        precioEspecialMay = nvoPrecioMaterial;
                    }
                }

                Precio_Final = nvoPrecioMaterial == 0 ? (float)PrecioProducto : (float)nvoPrecioMaterial;
                posicion += 10;
                PrecioCliente producto = new PrecioCliente
                {
                    Nombre_Completo = Nombre_Completo,
                    Clasifica = Clasifica,
                    descRecoge = descRecoge,
                    descContado = DescContado,
                    precio_Final = Precio_Final
                };
                return producto;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en Cálculo de los descuentos de mayoreo: " + ex.Message, "Atención");
            }
            return null;
        }


        public async Task<Hashtable> ObtenerDescuentoSimulador(string codebar, string claveCliente)
        {
            try
            {
                TipoVenta tipoVenta = await _sucursalService.ObtenerTipoVenta(_ClaveSap);

                if (string.IsNullOrEmpty(claveCliente) || claveCliente.Length > 10)
                {
                    throw new ArgumentException("El valor debe tener entre 1 y 10 dígitos.");
                }

                string claveClienteFinal = claveCliente.PadLeft(10, '0');



                var hashRetorno = new Hashtable();
                string cliente = tipoVenta.CanalVenta == 1 ? "MN00000001" : claveClienteFinal;

                DataSet mDataSet;

                mDataSet = await _simuladorPedido.SimuladorPiezaAsync(codebar, cliente, _ClaveSap);

                hashRetorno = LeerTablasDescuento(mDataSet.Tables["promos"], mDataSet.Tables["ItemsOut"]);


                return hashRetorno;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Sustituto para MsgBox en entornos de servidor/servicio
                return null;
            }

        }

        public async Task<Hashtable> ObtenerDescuentoSimulador(string codebar)
        {
            try
            {
                TipoVenta tipoVenta = await _sucursalService.ObtenerTipoVenta(_ClaveSap);
                var hashRetorno = new Hashtable();
                string claveClienteFinal = _ClaveSap.PadLeft(10, '0');
                string empleadoVenta = tipoVenta.CanalVenta == 1 ? "MN00000001" : claveClienteFinal;

                DataSet mDataSet;

                mDataSet = await _simuladorPedido.SimuladorPiezaAsync(codebar, empleadoVenta, _ClaveSap);

                hashRetorno = LeerTablasDescuento(mDataSet.Tables["promos"], mDataSet.Tables["ItemsOut"]);


                return hashRetorno;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Sustituto para MsgBox en entornos de servidor/servicio
                return null;
            }

        }
        // nueva version 

        //public async Task<Hashtable> ObtenerDescuentoSimulador(string codebar, string claveCliente)
        //{
        //    try
        //    {
        //        TipoVenta tipoVenta = await _sucursalService.ObtenerTipoVenta(_ClaveSap);

        //        if (string.IsNullOrEmpty(claveCliente) || claveCliente.Length > 10)
        //        {
        //            throw new ArgumentException("El valor debe tener entre 1 y 10 dígitos.");
        //        }

        //        string claveClienteFinal = claveCliente.PadLeft(10, '0');



        //        var hashRetorno = new Hashtable();
        //        string cliente = tipoVenta.CanalVenta == 1 ? "MN00000001" : claveClienteFinal;

        //        DataSet mDataSet;

        //        mDataSet = await _simuladorPedido.SimuladorPiezaAsync(codebar, cliente, _ClaveSap);

        //        hashRetorno = LeerTablasDescuento(mDataSet.Tables["promos"], mDataSet.Tables["ItemsOut"]);


        //        return hashRetorno;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message); // Sustituto para MsgBox en entornos de servidor/servicio
        //        return null;
        //    }

        //}

        public Hashtable LeerTablasDescuento(DataTable tablaProms, DataTable tablaItems)
        {
            try
            {
                // Declaración de las cadenas de configuración
                string arrCamposCerrados = "Y997";
                string arrCamposAbiertos = "Y992,Y991,Y990,Y996";
                string arrClavePromocionDefault = "YVK0,MWST,Y993,Y995";

                string promosAut = "K004,YK04,K005";
                double porcentajeAplicado = 0;
                string descuentoAplicado = "";

                // Obtención del precio del producto desde la tabla de items
                double precioProducto = Convert.ToDouble(tablaItems.Rows[0]["SUBTOTAL2"]);

                // Iteración por cada fila en la tabla de promociones
                foreach (DataRow viewDescto in tablaProms.Rows)
                {
                    string mPromo = viewDescto["clasecondicion"].ToString();

                    // Verificación si la clave de condición está en la lista predeterminada
                    int finda = arrClavePromocionDefault.IndexOf(mPromo, StringComparison.OrdinalIgnoreCase);

                    // Si la promoción es autorizada (parte de promosAut)
                    if (promosAut.IndexOf(mPromo, StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        viewDescto["promoAceptada"] = true; // Se marca como aceptada
                        porcentajeAplicado += Convert.ToDouble(viewDescto["porcentajeDesc"]);
                        descuentoAplicado = mPromo;
                    }
                }

                // Creación del Hashtable con los resultados finales
                var leerTablasDescuento = new Hashtable
            {
                { "PrecioProducto", precioProducto },
                { "DescuentoSimulador", Math.Abs(porcentajeAplicado) }
            };

                return leerTablasDescuento;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Sustituto para MsgBox en entornos de servidor
                return null;
            }
        }
    }
}
