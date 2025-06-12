using sanimex.webapi.Datos.Servicio.UbicacionesServicio;
using sanimex.webapi.Dominio.Models.Logs;
using sanimex.webapi.Dominio.Models.Ubicaciones;
using sanimex.webapi.Dominio.Models.Usuarios;
using sanimex.webapi.Negocio.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Negocio.Ubicaciones
{
    public class UbicacionNegocio : IUbicacionNegocio
    {
        private readonly IUbicacionServicio _ubicacionServicio;
        private readonly IUsuarioNegocio _usuarioNegocio;

        public UbicacionNegocio(IUbicacionServicio ubicacionServicio, IUsuarioNegocio usuarioNegocio)
        {
            _ubicacionServicio = ubicacionServicio;
            _usuarioNegocio = usuarioNegocio;
        }

        public async Task<int> InsertarUbicacion(string direccion, double latitud, double longitud, string claveSucursal, int numeroEmpleado, bool tipoIngreso)
        {
            RolUsuario rol = await _usuarioNegocio.Roles(numeroEmpleado);
            return await _ubicacionServicio.InsertarUbicacion(direccion, latitud, longitud, claveSucursal, numeroEmpleado, rol.idRol.ToString(), tipoIngreso);
        }

        public async Task<List<UbicacionSucursal>> UbicacionSucursal(string fechaConsulta, string idUsuario)
        {
            return await _ubicacionServicio.UbicacionSucursal(fechaConsulta, idUsuario);
        }
        public async Task<List<VisitadorActivo>> VisitadorActivos(string claveSucursal, string fechaUnitaria, string usuarioId)
        {
            return await _ubicacionServicio.VisitadorActivos(claveSucursal, fechaUnitaria, usuarioId);
        }
        public async Task<List<UbicacionesMaps>> UbicacionesMaps(string claveSucursal, string numeroEmpleado, string fechaUnitaria)
        {
            return await _ubicacionServicio.UbicacionesMaps(claveSucursal, numeroEmpleado, fechaUnitaria);
        }
        public async Task<List<UbicacionesMaps>> UbicacionesVisitaMaps(string claveSucursal, string numeroEmpleado, string fechaUnitaria)
        {
            return await _ubicacionServicio.UbicacionesVisitaMaps(claveSucursal, numeroEmpleado, fechaUnitaria);
        }

        public async Task<List<UbicacionesModel>> ListarUbicaciones()
        {
            return await _ubicacionServicio.ListarUbicaciones();
        }
        public async Task<UbicacionesModel> VerUbicacion(int id)
        {
            return await _ubicacionServicio.VerUbicacion(id);
        }

    }
}
