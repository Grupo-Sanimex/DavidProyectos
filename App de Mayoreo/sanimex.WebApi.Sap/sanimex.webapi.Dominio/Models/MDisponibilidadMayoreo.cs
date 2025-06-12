namespace sanimex.webapi.Dominio.Models
{
    public class MDisponibilidadMayoreo
    {
        public string Codigo { get; set; }
        public string ClaveSAP { get; set; }
        public string NombreSucursal { get; set; }
        public int StockLibre { get; set; }
        public int StockDispo { get; set; }
    }
}
