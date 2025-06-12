using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sanimex.webapi.Datos.Servicio.CarritoServico;
using sanimex.webapi.Dominio.Models.Carrito;
using sanimex.webapi.Dominio.Models.Logs;
using sanimex.webapi.Dominio.Models.Producto;
using sanimex.webapi.Negocio.Producto;


namespace sanimex.webapi.Negocio.Carrito
{
    public class CarroNegocio : ICarroNegocio
    {
        private readonly ICarritoServicio _carritoServicio;
        private readonly IProductoNegocio _productoNegocio;
        public CarroNegocio(ICarritoServicio carritoServicio, IProductoNegocio productoNegocio)
        {
            _carritoServicio = carritoServicio;
            _productoNegocio = productoNegocio;
        }
        public async Task<int> GuardarConsultas(string idUsuario, List<Carro> carro)
        {
            int idGenerado = await _carritoServicio.GuardarCarro(idUsuario, carro);
            return idGenerado;
        }
        public async Task<bool> GuardarCodigoAsync(int idGenerado, List<Carro> carro)
        {
            bool respuesta = await _carritoServicio.GuardarCodigoAsync(idGenerado, carro);
            if (!respuesta)
                return false;
            else
                return true;
        }
        public async Task<List<HisCotizaGerenteSucursal>> HisCotizaGerenteSucursal(string idUsuario, string fechaConsulta)
        {
            List<HisCotizaGerenteSucursal> hisCotizaGerenteSucursals = await _carritoServicio.HisCotizaGerenteSucursal(idUsuario, fechaConsulta);
            return hisCotizaGerenteSucursals;
        }
        public async Task<List<HisCtoMasterGerente>> ListarHistorialCMasterGerente(string idUsuario, string fechaConsulta)
        {
            List<HisCtoMasterGerente> HisCtoMasterGerente = await _carritoServicio.ListarHistorialCMasterGerente(idUsuario, fechaConsulta);
            return HisCtoMasterGerente;
        }
        public async Task<List<HisCtoMaster>> ListarHistorialCMaster(string idUsuario, string fechaConsulta)
        {
            List<HisCtoMaster> HisCtoMaster = await _carritoServicio.ListarHistorialCMaster(idUsuario, fechaConsulta);
            return HisCtoMaster;
        }
        public async Task<List<CotizacionDetalle>> CotizacionDetalle(string idUsuario, string idCotizacion)
        {
            List<CotizacionDetalle> CotizacionDetalle = await _carritoServicio.CotizacionDetalle(idUsuario, idCotizacion);
            return CotizacionDetalle;
        }
        public async Task<HisCtoCliente?> ConsultaCliente(string idUsuario, string idCotizacion)
        {
            ConsultaCliente? consultaCliente = await _carritoServicio.ConsultaCliente(idCotizacion);
            if (consultaCliente == null)
            {
                return null;
            }

            PrecioCliente? productoCliente = await _productoNegocio.CalculaDescuentos_Mayoreo(
                false,
                false,
                consultaCliente.claveCliente,
                0
            );

            if (productoCliente == null)
            {
                return null;
            }

            HisCtoCliente hisCtoCliente = new HisCtoCliente
            {
                nombreCliente = productoCliente.Nombre_Completo,
                clasificacion = productoCliente.Clasifica
            };

            return hisCtoCliente;
        }

    }
}
