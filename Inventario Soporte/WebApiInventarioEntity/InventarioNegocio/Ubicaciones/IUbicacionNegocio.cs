using InventarioDatos.ModelsDto;

namespace InventarioNegocio.Ubicaciones
{
    public interface IUbicacionNegocio
    {
        void ActualizarUbicacion(UbicacionDto ubicacionDto);
        void AgregarUbicacion(UbicacionDto ubicacionDto);
        void EliminarUbicacion(int id);
        List<UbicacionDto> ObtenerUbicaciones();
        UbicacionDto ObtenerUbicacionPorId(int id);
    }
}