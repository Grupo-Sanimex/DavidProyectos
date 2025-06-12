using InventarioDatos.ModelsDto;

namespace InventarioNegocio.Equipos
{
    public interface IEquipoNegocio
    {
        void ActualizarEquipo(EquipoDto equipo);
        void AgregarEquipo(EquipoDto equipoDto);
        EquipoDto BuscarEquipos(string busqueda);
        void EliminarEquipo(int id);
        EquipoDto ObtenerEquipoPorId(int id);
        IEnumerable<EquipoDto> ObtenerEquipoPorIdEmpleado(int id);
        List<EquipoDto> ObtenerEquipos();
        List<EquipoDto> ObtenerEquiposDisponibles();
    }
}