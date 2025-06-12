using InventarioDatos.ModelsDto;

namespace InventarioNegocio.Departamentos
{
    public interface IDepartamentoNegocio
    {
        DepartamentoDto ActualizarDepartamento(DepartamentoDto departamentoDto);
        DepartamentoDto AgregarDepartamento(DepartamentoDto departamentoDto);
        void EliminarDepartamento(int id);
        DepartamentoDto ObtenerDepartamentoPorId(int id);
        IEnumerable<DepartamentoDto> ObtenerDepartamentos();
    }
}