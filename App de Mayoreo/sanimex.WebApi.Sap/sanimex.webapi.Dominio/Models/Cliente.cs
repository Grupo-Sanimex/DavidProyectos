namespace sanimex.webapi.Dominio.Models
{
    public class Cliente
    {
        public string IdCliente { get; set; }
        public string Rfc { get; set; }
        public string CreditoAutorizado { get; set; }
        public string CreditoConsumido { get; set; }
        public string CreditoDisponible { get; set; }
        public string DiasDeCredito { get; set; }
        public string Bloqueado { get; set; }
        public string Estatus { get; set; }
        public string Bonificacion { get; set; }
        public string RazonSocial { get; set; }
        public int IdTipoPersona { get; set; }
        public string RegimenFiscal { get; set; }
        public string RazonBloqueo { get; set; }
    }
}
