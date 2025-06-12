using sanimex.webapi.Dominio.Models;
using sanimex.webapi.Dominio.Models.Usuarios;

namespace sanimex.webapi.Negocio.Usuarios
{
    public interface IUsuarioNegocio
    {
        string ActivarUsuarioApp(int numEmpleado);
        string DesactivarUsuarioApp(int numEmpleado);
        string GuardarUsuarioApp(int numEmpleado, DateTime fechaInicio, TimeSpan horaInicio);
        Task<Usuario> ObtenerAcceso(int id);
        Task<Usuario> Perfil(string id);
        Task<int> RolePermisos(int id);
        Task<RolUsuario> Roles(int id);
        string ValidarUsuarioApp(int numEmpleado);
    }
}