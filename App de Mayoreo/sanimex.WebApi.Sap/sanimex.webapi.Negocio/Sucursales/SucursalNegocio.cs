using sanimex.webapi.Datos.Servicio.ProductoServicio.Implementacion;
using sanimex.webapi.Datos.Servicio.SucursalService;
using sanimex.webapi.Datos.Servicio.UsuarioServicio;
using sanimex.webapi.Dominio.Models.Sucursales;
using sanimex.webapi.Dominio.Models.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Negocio.Sucursales
{
    public class SucursalNegocio : ISucursalNegocio
    {
        private readonly ISucursalService _sucursalService;

        public SucursalNegocio(ISucursalService sucursalService)
        {
            _sucursalService = sucursalService;
        }
        public async Task<List<Sucursal>> ObtenerSucursalSupervisor(int idUsuario)
        {
            try
            {

                return await _sucursalService.ObtenerSucursalSupervisor(idUsuario);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Error en los parámetros de entrada: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el Sucursal: {ex.Message}");
            }
        }
        public async Task<List<Sucursal>> ObtenerSucursalAdmin()
        {
            try
            {

                return await _sucursalService.ObtenerSucursalesAdmin();
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Error en los parámetros de entrada: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el Sucursal: {ex.Message}");
            }
        }
        public async Task<List<Sucursal>> ObtenerSucursal(int idSucursal)
        {
            try
            {

                return await _sucursalService.ObtenerSucursales(idSucursal);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Error en los parámetros de entrada: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el Sucursal: {ex.Message}");
            }
        }
    }
}
