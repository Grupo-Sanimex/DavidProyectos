using sanimex.webapi.Dominio.Models.Ubicaciones;

namespace sanimex.webapi.Datos.Servicio.UbicacionesServicio
{
    public interface IUbicacionServicio
    {
        Task<int> InsertarUbicacion(string direccion, double latitud, double longitud, string claveSucursal, int numeroEmpleado, string idRol, bool tipoIngreso);
        Task<List<UbicacionesModel>> ListarUbicaciones();
        Task<List<UbicacionesMaps>> UbicacionesMaps(string claveSucursal, string numeroEmpleado, string fechaUnitaria);
        Task<List<UbicacionesMaps>> UbicacionesVisitaMaps(string claveSucursal, string numeroEmpleado, string fechaUnitaria);
        Task<List<UbicacionSucursal>> UbicacionSucursal(string fechaConsulta, string idUsuario);
        Task<UbicacionesModel> VerUbicacion(int id);
        Task<List<VisitadorActivo>> VisitadorActivos(string claveSucursal, string fechaUnitaria, string usuarioId);
    }
}