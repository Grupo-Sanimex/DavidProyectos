namespace sanimex.WebApi.Sap.Services
{
    public interface IDisponibilidadService
    {
        Task<object> DisponibilidadxCentrosAsync(string barcode, string descripcion, string centrosCorredor, string sucHijoSap);
    }
}
