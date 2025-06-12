using InventarioDatos.ModelsDto;

namespace InventarioNegocio.Usuarios
{
    public interface IUsuariosNegocio
    {
        void ActualizarContracena(int usuarioId, string nuevaContracena);
        void ActualizarUsuario(UsuarioDto usuarioDto);
        void AgregarUsuario(UsuarioDto usuariodto);
        void EliminarUsuario(int id);
        UsuarioDto ObtenerUsuarioPorId(int id);
        UsuarioDto ObtenerUsuarioPorUsuario(string usuario);
        List<UsuarioDto> ObtenerUsuarios();
    }
}