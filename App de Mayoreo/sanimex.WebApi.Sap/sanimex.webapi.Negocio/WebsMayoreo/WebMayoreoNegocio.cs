using sanimex.webapi.Datos.Servicio.WebServicios;
using sanimex.webapi.Dominio.Models.webMayoreo;
using sanimex.webapi.Negocio.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Negocio.WebsMayoreo
{
    public class WebMayoreoNegocio : IWebMayoreoNegocio
    {
        private readonly IWebServicio _webServicio;
        public WebMayoreoNegocio(IWebServicio webServicio)
        {
            _webServicio = webServicio;
        }
        public async Task<UsuarioWeb> ObtenerAccesoWeb(int numEmpleado)
        {
            return await _webServicio.ObtenerAccesoWeb(numEmpleado);
        }
        public async Task<string> GuardarRol(string perfil)
        {
            return await _webServicio.GuardarRol(perfil);
        }
        public async Task<object> ListarRoles()
        {
            return await _webServicio.listarRoles();
        }
        public bool BajaRol(int idRol)
        {
            return _webServicio.BajaRol(idRol);
        }
        public object ListarRol(int idRol)
        {
            return _webServicio.ListarRol(idRol);
        }
        public object ListarUsuario(int idUsuario)
        {
            return _webServicio.ListarUsuario(idUsuario);
        }
        public object ListarUsuarios()
        {
            return _webServicio.ListarUsuarios();
        }
        public async Task<string> AgregarUsuarioWeb(int idPermiso, int numEmpleado, string nombre, string aPaterno, string aMaterno, string contrasena)
        {
            var seguridadService = new SeguridadNegocio();
            // Convertir la contraseña a su representación en MD5
            string contrasenaMD5 = seguridadService.ConvertirContraseña(contrasena);
            return await _webServicio.AgregarUsuarioWeb(idPermiso, numEmpleado, nombre, aPaterno, aMaterno, contrasenaMD5);
        }
        public async Task<string> BajaUsuarioWeb(int idUsuario)
        {
            return await _webServicio.BajaUsuarioWeb(idUsuario);
        }
    }
}
