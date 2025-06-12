using sanimex.webapi.Datos.Servicio.UsuarioServicio;
using sanimex.webapi.Dominio.Models;
using sanimex.webapi.Dominio.Models.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Negocio.Usuarios
{
    public class UsuarioNegocio : IUsuarioNegocio
    {
        private readonly IUsuarioServicio _usuarioServicio; // Usar una interfaz de repositorio para la capa de datos

        public UsuarioNegocio(IUsuarioServicio usuarioServicio)
        {
            _usuarioServicio = usuarioServicio; // Inyección de dependencias
        }
        public async Task<Usuario> ObtenerAcceso(int id) // Implementación del método
        {
            return await _usuarioServicio.ObtenerAcceso(id); // Llama al método de la capa de datos
        }
        public async Task<Usuario> Perfil(string id)
        {
            return await _usuarioServicio.Perfil(id);
        }
        public async Task<RolUsuario> Roles(int id)
        {
            return await _usuarioServicio.Roles(id);
        }
        public async Task<int> RolePermisos(int id)
        {
            return await _usuarioServicio.RolePermisos(id);
        }
        public string GuardarUsuarioApp(int numEmpleado, DateTime fechaInicio, TimeSpan horaInicio)
        {
            return _usuarioServicio.GuardarUsuarioApp(numEmpleado, fechaInicio, horaInicio);
        }
        public string ActivarUsuarioApp(int numEmpleado)
        {
            return _usuarioServicio.ActivarUsuarioApp(numEmpleado);
        }
        public string DesactivarUsuarioApp(int numEmpleado)
        {
            return _usuarioServicio.DesactivarUsuarioApp(numEmpleado);
        }
        public string ValidarUsuarioApp(int numEmpleado)
        {
            return _usuarioServicio.ValidarUsuarioApp(numEmpleado);
        }
    }
}
