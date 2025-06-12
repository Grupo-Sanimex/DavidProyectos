
using sanimex.webapi.Dominio.Models.webMayoreo;

namespace sanimex.webapi.Negocio.WebsMayoreo
{
    public interface IWebMayoreoNegocio
    {
        Task<string> AgregarUsuarioWeb(int idPermiso, int numEmpleado, string nombre, string aPaterno, string aMaterno, string contrasena);
        bool BajaRol(int idRol);
        Task<string> BajaUsuarioWeb(int idUsuario);
        Task<string> GuardarRol(string perfil);
        object ListarRol(int idRol);
        Task<object> ListarRoles();
        object ListarUsuario(int idUsuario);
        object ListarUsuarios();
        Task<UsuarioWeb> ObtenerAccesoWeb(int numEmpleado);
    }
}