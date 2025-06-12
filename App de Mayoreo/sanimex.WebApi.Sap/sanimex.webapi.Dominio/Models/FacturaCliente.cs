namespace sanimex.webapi.Dominio.Models
{
    public class FacturaCliente
    {
        public string Folio { get; set; }
        public string FechaVenta { get; set; }
        public string MontoVenta { get; set; }
        public string FechaPago { get; set; }
        public string Saldo { get; set; }
        public string DiasCredito { get; set; }
        public string FolioVenta { get; set; }
    }
}
