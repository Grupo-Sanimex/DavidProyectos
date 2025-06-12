using InventarioDatos.ModelsDto;

namespace InventarioNegocio.Roles
{
    public interface IRolNegocio
    {
        void ActualizarRol(RolDto roldto);
        void AgregarRol(RolDto roldto);
        void EliminarRol(int id);
        List<RolDto> ObtenerRoles();
        RolDto ObtenerRolPorId(int id);
    }
}