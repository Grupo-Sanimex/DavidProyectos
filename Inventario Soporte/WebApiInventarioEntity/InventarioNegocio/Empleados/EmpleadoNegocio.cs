using InventarioDatos.Datos;
using InventarioDatos.Models;
using InventarioDatos.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioNegocio.Empleados
{
    public class EmpleadoNegocio : IEmpleadoNegocio
    {
        private readonly DatosDbContext _context;

        public EmpleadoNegocio(DatosDbContext context)
        {
            _context = context;
        }

        public IEnumerable<EmpleadoDto> ObtenerEmpleados()
        {
            return _context.Empleado
                .Select(e => new EmpleadoDto
                {
                    IdEmpleado = e.IdEmpleado,
                    Nombre = e.Nombre,
                    ApellidoP = e.ApellidoP,
                    ApellidoM = e.ApellidoM,
                    Puesto = e.Puesto,
                    UsuarioWindows = e.UsuarioWindows,
                    UsuarioAD = e.UsuarioAD,
                    Correo = e.Correo,
                    Pass = e.Pass,
                    Acceso = e.Acceso,
                    IdDepartamento = e.IdDepartamento ,
                    IdUbicacion = e.IdUbicacion,
                    Status = e.Status
                })
                .ToList();
        }

        public EmpleadoDto ObtenerEmpleadoPorId(int id)
        {
            var empleado = _context.Empleado.Find(id);
            if (empleado == null)
            {
                return null;
            }
            return new EmpleadoDto
            {
                IdEmpleado = empleado.IdEmpleado,
                Nombre = empleado.Nombre,
                ApellidoP = empleado.ApellidoP,
                ApellidoM = empleado.ApellidoM,
                Puesto = empleado.Puesto,
                UsuarioWindows = empleado.UsuarioWindows,
                UsuarioAD = empleado.UsuarioAD,
                Correo = empleado.Correo,
                Pass = empleado.Pass,
                Acceso = empleado.Acceso,
                IdDepartamento = empleado.IdDepartamento,
                IdUbicacion = empleado.IdUbicacion,
                Status = empleado.Status
            };
        }
        public IEnumerable<EmpleadoDto> BuscarEmpleadosPorParametro(string parametro)
        {
            if (string.IsNullOrWhiteSpace(parametro))
                return Enumerable.Empty<EmpleadoDto>();

            var partes = parametro.Trim().ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var query = _context.Empleado.AsQueryable();

            if (partes.Length == 1)
            {
                string parte = partes[0];
                query = query.Where(e =>
                    e.Nombre.ToLower().Contains(parte) ||
                    e.ApellidoP.ToLower().Contains(parte) ||
                    e.ApellidoM.ToLower().Contains(parte) ||
                    e.UsuarioWindows.ToLower().Contains(parte) ||
                    e.Correo.ToLower().Contains(parte)
                );
            }
            else if (partes.Length >= 2)
            {
                // Busca coincidencia exacta en nombre, apellido paterno y materno
                string nombre = partes[0];
                string apellidoP = partes.Length > 1 ? partes[1] : "";
                string apellidoM = partes.Length > 2 ? partes[2] : "";

                query = query.Where(e =>
                    e.Nombre.ToLower().Contains(nombre) &&
                    e.ApellidoP.ToLower().Contains(apellidoP) &&
                    (string.IsNullOrEmpty(apellidoM) || e.ApellidoM.ToLower().Contains(apellidoM))
                );
            }

            return query.Select(e => new EmpleadoDto
            {
                IdEmpleado = e.IdEmpleado,
                Nombre = e.Nombre,
                ApellidoP = e.ApellidoP,
                ApellidoM = e.ApellidoM,
                Puesto = e.Puesto,
                UsuarioWindows = e.UsuarioWindows,
                UsuarioAD = e.UsuarioAD,
                Correo = e.Correo,
                Pass = e.Pass,
                Acceso = e.Acceso,
                IdDepartamento = e.IdDepartamento,
                IdUbicacion = e.IdUbicacion,
                Status = e.Status
            }).ToList();
        }

        public EmpleadoDto AgregarEmpleado(EmpleadoDto empleadoDto)
        {
            bool existe = _context.Empleado.Any(d =>
                d.Nombre.ToLower() == empleadoDto.Nombre.ToLower() &&
                d.ApellidoP.ToLower() == empleadoDto.ApellidoP.ToLower() &&
                d.ApellidoM.ToLower() == empleadoDto.ApellidoM.ToLower() &&
                d.Puesto.ToLower() == empleadoDto.Puesto.ToLower() &&
                d.UsuarioWindows.ToLower() == empleadoDto.UsuarioWindows.ToLower() &&
                d.Correo.ToLower() == empleadoDto.Correo.ToLower() &&
                d.IdDepartamento == empleadoDto.IdDepartamento &&
                d.IdUbicacion == empleadoDto.IdUbicacion
            );

            if (existe)
            {
                return new EmpleadoDto(); // Objeto vacío (ID = 0 por defecto)
            }
            var empleado = new Empleado
            {
                Nombre = empleadoDto.Nombre,
                ApellidoP = empleadoDto.ApellidoP,
                ApellidoM = empleadoDto.ApellidoM,
                Puesto = empleadoDto.Puesto,
                UsuarioWindows = empleadoDto.UsuarioWindows,
                UsuarioAD = empleadoDto.UsuarioAD,
                Correo = empleadoDto.Correo,
                Pass = empleadoDto.Pass,
                Acceso = empleadoDto.Acceso,
                IdDepartamento = empleadoDto.IdDepartamento,
                IdUbicacion = empleadoDto.IdUbicacion,
                Status = empleadoDto.Status
            };
            _context.Empleado.Add(empleado);
            _context.SaveChanges();
            empleadoDto.IdEmpleado = empleado.IdEmpleado; // Actualizar el ID generado
            return empleadoDto;
        }

        public EmpleadoDto ActualizarEmpleado(EmpleadoDto empleadoDto)
        {
            var empleadoA = _context.Empleado.Find(empleadoDto.IdEmpleado);
            if (empleadoA == null)
            {
                return new EmpleadoDto(); // No existe el empleado con ese nombre
            }
            // Verificar si se intenta cambiar el nombre a uno que ya existe en otro registro
            // Validar duplicidad de UsuarioWindows, UsuarioAD y Correo
            bool usuarioDuplicado = _context.Empleado
                .Any(d => d.IdEmpleado != empleadoDto.IdEmpleado &&
                    (d.UsuarioWindows.ToLower() == empleadoDto.UsuarioWindows.ToLower() ||
                     d.UsuarioAD.ToLower() == empleadoDto.UsuarioAD.ToLower() ||
                     d.Correo.ToLower() == empleadoDto.Correo.ToLower()));

            if (usuarioDuplicado)
            {
                return new EmpleadoDto(); // Ya existe un empleado con alguno de esos datos
            }
            var empleado = _context.Empleado.Find(empleadoDto.IdEmpleado);
            empleado.Nombre = empleadoDto.Nombre;
            empleado.ApellidoP = empleadoDto.ApellidoP;
            empleado.ApellidoM = empleadoDto.ApellidoM;
            empleado.Puesto = empleadoDto.Puesto;
            empleado.UsuarioWindows = empleadoDto.UsuarioWindows;
            empleado.UsuarioAD = empleadoDto.UsuarioAD;
            empleado.Correo = empleadoDto.Correo;
            empleado.Pass = empleadoDto.Pass;
            empleado.Acceso = empleadoDto.Acceso;
            empleado.IdDepartamento = empleadoDto.IdDepartamento;
            empleado.IdUbicacion = empleadoDto.IdUbicacion;
            empleado.Status = empleadoDto.Status;
            _context.SaveChanges();
            return empleadoDto; // Retornar el DTO actualizado 
        }
        public void EliminarEmpleado(int id)
        {
            var empleado = _context.Empleado.Find(id);
            if (empleado != null)
            {
                if (empleado.Status == true)
                {
                    empleado.Status = false;
                }
                else
                {
                    empleado.Status = true;
                }
                _context.SaveChanges();
            }
        }
    }
}