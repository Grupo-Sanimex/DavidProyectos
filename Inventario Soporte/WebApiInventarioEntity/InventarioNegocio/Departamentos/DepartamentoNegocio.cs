using InventarioDatos.Datos;
using InventarioDatos.Models;
using InventarioDatos.ModelsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioNegocio.Departamentos
{
    public class DepartamentoNegocio : IDepartamentoNegocio
    {
        private readonly DatosDbContext _context;

        public DepartamentoNegocio(DatosDbContext context)
        {
            _context = context;
        }

        public IEnumerable<DepartamentoDto> ObtenerDepartamentos()
        {
            return _context.Departamento
                .Select(d => new DepartamentoDto
                {
                    IdDepartamento = d.IdDepartamento,
                    NombreDepartamento = d.NombreDepartamento,
                    Status = d.Status
                })
                .ToList();
        }

        public DepartamentoDto ObtenerDepartamentoPorId(int id)
        {
            var departamento = _context.Departamento.Find(id);
            if (departamento == null)
            {
                return null;
            }
            return new DepartamentoDto
            {
                IdDepartamento = departamento.IdDepartamento,
                NombreDepartamento = departamento.NombreDepartamento,
                Status = departamento.Status
            };
        }

        public DepartamentoDto AgregarDepartamento(DepartamentoDto departamentoDto)
        {
            bool existe = _context.Departamento
                .Any(d => d.NombreDepartamento.ToLower() == departamentoDto.NombreDepartamento.ToLower());

            if (existe)
            {
                return new DepartamentoDto(); // Objeto vacío (ID = 0 por defecto)
            }

            var departamento = new Departamento
            {
                NombreDepartamento = departamentoDto.NombreDepartamento,
                Status = departamentoDto.Status
            };
            _context.Departamento.Add(departamento);
            _context.SaveChanges();

            departamentoDto.IdDepartamento = departamento.IdDepartamento;
            return departamentoDto;
        }



        public DepartamentoDto ActualizarDepartamento(DepartamentoDto departamentoDto)
        {
            var departamento = _context.Departamento.Find(departamentoDto.IdDepartamento);
            if (departamento == null)
            {
                return new DepartamentoDto(); // No existe el departamento
            }

            // Verificar si se intenta cambiar el nombre a uno que ya existe en otro registro
            bool nombreDuplicado = _context.Departamento
                .Any(d => d.IdDepartamento != departamentoDto.IdDepartamento &&
                          d.NombreDepartamento.ToLower() == departamentoDto.NombreDepartamento.ToLower());

            if (nombreDuplicado)
            {
                // No se permite cambiar el nombre porque ya existe en otro registro
                return new DepartamentoDto(); // O puedes lanzar una excepción o retornar un error específico
            }

            // Actualizar los campos
            departamento.NombreDepartamento = departamentoDto.NombreDepartamento;
            departamento.Status = departamentoDto.Status;
            _context.SaveChanges();
            return departamentoDto;
        }
        public void EliminarDepartamento(int id)
        {
            var departamento = _context.Departamento.Find(id);
            if (departamento != null)
            {
                departamento.Status = false;
                _context.SaveChanges();
            }
        }
    }
}