using InventarioDatos.Datos;
using InventarioDatos.Models;
using InventarioDatos.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioNegocio.Roles
{
    public class RolNegocio : IRolNegocio
    {
        private readonly DatosDbContext _datosDbContext;
        public RolNegocio(DatosDbContext datosDbContext)
        {
            _datosDbContext = datosDbContext;
        }
        public List<RolDto> ObtenerRoles()
        {
            var roles = _datosDbContext.Rol.ToList();
            return roles.Select(r => new RolDto
            {
                IdRol = r.IdRol,
                NombreRol = r.NombreRol,
                Status = r.Status
            }).ToList();
        }
        public RolDto ObtenerRolPorId(int id)
        {
            var rol = _datosDbContext.Rol.FirstOrDefault(r => r.IdRol == id);
            if (rol != null)
            {
                return new RolDto
                {
                    IdRol = rol.IdRol,
                    NombreRol = rol.NombreRol,
                    Status = rol.Status
                };
            }
            return null;
        }
        public void AgregarRol(RolDto roldto)
        {
            var rol = new Rol
            {
                NombreRol = roldto.NombreRol,
                Status = true
            };
            _datosDbContext.Rol.Add(rol);
            _datosDbContext.SaveChanges();
        }
        public void ActualizarRol(RolDto roldto)
        {
            var rol = _datosDbContext.Rol.FirstOrDefault(r => r.IdRol == roldto.IdRol);
            if (rol != null)
            {
                rol.NombreRol = roldto.NombreRol;
                _datosDbContext.SaveChanges();
            }
        }
        public void EliminarRol(int id)
        {
            var rol = _datosDbContext.Rol.FirstOrDefault(d => d.IdRol == id);
            if (rol != null)
            {
                if (rol.Status == false)
                {
                    rol.Status = true;
                }
                else 
                {
                    rol.Status = false;
                }
                _datosDbContext.SaveChanges();
            }
        }
    }
}
