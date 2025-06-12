using InventarioDatos.Datos;
using InventarioDatos.Models;
using InventarioDatos.ModelsDto;
using InventarioNegocio.Herramientas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioNegocio.Usuarios
{
    public class UsuariosNegocio : IUsuariosNegocio
    {
        private readonly DatosDbContext _datosDbContext;
        private readonly Md5 _md5;
        public UsuariosNegocio(DatosDbContext datosDbContext, Md5 md5)
        {
            _datosDbContext = datosDbContext;
            _md5 = md5;
        }
        public List<UsuarioDto> ObtenerUsuarios()
        {
            return _datosDbContext.Usuario
                //.Where(u => u.Status == true)
                .Select(u => new UsuarioDto
                {
                    IdUsuario = u.IdUsuario,
                    NombreCompleto = u.NombreCompleto,
                    Correo = u.Correo,
                    Puesto = u.Puesto,
                    UsuarioSesion = u.UsuarioSesion,
                    Contracena = u.Contracena,
                    IdRol = u.IdRol,
                    Status = u.Status
                }).ToList();
        }
        public UsuarioDto ObtenerUsuarioPorId(int id)
        {
            return _datosDbContext.Usuario
                .Where(u => u.IdUsuario == id)
                .Select(u => new UsuarioDto
                {
                    IdUsuario = u.IdUsuario,
                    NombreCompleto = u.NombreCompleto,
                    Correo = u.Correo,
                    Puesto = u.Puesto,
                    UsuarioSesion = u.UsuarioSesion,
                    Contracena = u.Contracena,
                    IdRol = u.IdRol,
                    Status = u.Status
                }).FirstOrDefault();
        }
        public UsuarioDto ObtenerUsuarioPorUsuario(string usuario)
        {
            return _datosDbContext.Usuario
                .Where(u => u.UsuarioSesion == usuario)
                .Select(u => new UsuarioDto
                {
                    IdUsuario = u.IdUsuario,
                    NombreCompleto = u.NombreCompleto,
                    Correo = u.Correo,
                    Puesto = u.Puesto,
                    UsuarioSesion = u.UsuarioSesion,
                    Contracena = u.Contracena,
                    IdRol = u.IdRol,
                    Status = u.Status
                }).FirstOrDefault();
        }
        public void AgregarUsuario(UsuarioDto usuariodto)
        {
            var contracenaF = _md5.ConvertirContraseña(usuariodto.Contracena);
            var nuevoUsuario = new Usuario
            {
                NombreCompleto = usuariodto.NombreCompleto,
                Correo = usuariodto.Correo,
                Puesto = usuariodto.Puesto,
                UsuarioSesion = usuariodto.UsuarioSesion,
                Contracena = contracenaF,
                IdRol = usuariodto.IdRol,
                Status = true
            };
            _datosDbContext.Usuario.Add(nuevoUsuario);
            _datosDbContext.SaveChanges();
        }
        public void ActualizarUsuario(UsuarioDto usuarioDto)
        {
            var usuario = _datosDbContext.Usuario.FirstOrDefault(u => u.IdUsuario == usuarioDto.IdUsuario);
            if (usuario != null)
            {
                usuario.IdRol = usuarioDto.IdRol;
                usuario.Status = usuarioDto.Status;
                _datosDbContext.SaveChanges();
            }
        }
        public void ActualizarContracena(int usuarioId, string nuevaContracena)
        {
            var usuario = _datosDbContext.Usuario.FirstOrDefault(u => u.IdUsuario == usuarioId && u.Status == true);
            if (usuario != null)
            {
                usuario.Contracena = _md5.ConvertirContraseña(nuevaContracena);
                _datosDbContext.SaveChanges();
            }
        }
        public void EliminarUsuario(int id)
        {
            var usuario = _datosDbContext.Usuario.FirstOrDefault(u => u.IdUsuario == id && u.Status == true);
            if (usuario != null)
            {
                // Cambiar el estado a inactivo
                usuario.Status = false;

                // Guardar los cambios en la base de datos
                _datosDbContext.SaveChanges();
            }
        }
    }
}