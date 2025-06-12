using InventarioDatos.ModelsDto;

namespace InventarioNegocio.LincenciasOffice
{
    public interface ILicenciaOfficeNegocio
    {
        bool ActualizarLicenciaOffice(LicenciaOfficeDto licenciaOfficeDto);
        bool AgregarLicenciaOffice(LicenciaOfficeDto licenciaOfficeDto);
        void EliminarLicenciaOffice(int id);
        LicenciaOffEqDto ObtenerLicenciaOfficePorEquipo(int idEquipo);
        LicenciaOfficeDto ObtenerLicenciaOfficePorId(int id);
        List<LicenciaOfficeDto> ObtenerLicenciasOffice();
    }
}