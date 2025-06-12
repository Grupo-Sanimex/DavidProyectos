namespace sanimex.webapi.Dominio.Models
{
    public class MSimuladorPieza
    {
        public class Promocion
        {
            public int Status { get; set; }
            public decimal Montal { get; set; }
            public string Posicion { get; set; }
            public string Material { get; set; }
            public decimal Cantidad { get; set; }
            public string ClaseCondicion { get; set; }
            public decimal PorcentajeDesc { get; set; }
            public string SinDescripcion { get; set; }
            public decimal MontoDescuento { get; set; }
            public string ClaveDocumento { get; set; }
            public string PorcentajePosi { get; set; }
            public decimal PrecioFinal { get; set; }
            public string Descripcion { get; set; }
            public string ArticuloRegalo { get; set; }
            public decimal CantidadPadre { get; set; }
            public decimal CantidadHijo { get; set; }
            public decimal PrecioNuevo { get; set; }
            public bool PromoAceptada { get; set; }
        }

        public class Item
        {
            public string ItmNumber { get; set; }
            public string Material { get; set; }
            public string ShortText { get; set; }
            public decimal NetValue { get; set; }
            public string Currency { get; set; }
            public decimal ReqQty { get; set; }
            public string Plant { get; set; }
            public decimal TargetQty { get; set; }
            public string StgeLoc { get; set; }
            public decimal Subtotal1 { get; set; }
            public decimal Subtotal2 { get; set; }
            public decimal Subtotal3 { get; set; }
            public decimal Subtotal4 { get; set; }
            public decimal Subtotal5 { get; set; }
            public decimal Subtotal6 { get; set; }
            public int Points { get; set; }
        }

        public class Error
        {
            public string Number { get; set; }
            public string Message { get; set; }
            public string Row { get; set; }
        }
    }
}
