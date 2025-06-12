using InventarioDatos.ModelsDto;

namespace InventarioNegocio.Empleados
{
    public interface IEmpleadoNegocio
    {
        EmpleadoDto ActualizarEmpleado(EmpleadoDto empleadoDto);
        EmpleadoDto AgregarEmpleado(EmpleadoDto empleadoDto);
        IEnumerable<EmpleadoDto> BuscarEmpleadosPorParametro(string parametro);
        void EliminarEmpleado(int id);
        EmpleadoDto ObtenerEmpleadoPorId(int id);
        IEnumerable<EmpleadoDto> ObtenerEmpleados();
    }
}