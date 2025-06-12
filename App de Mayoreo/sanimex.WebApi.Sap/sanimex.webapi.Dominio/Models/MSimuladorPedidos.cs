namespace sanimex.webapi.Dominio.Models
{
    public class MSimuladorPedidos
    {
        public class Promo
        {
            public string Status { get; set; }
            public string Montoral { get; set; }
            public string Posicion { get; set; }
            public string Material { get; set; }
            public string Cantidad { get; set; }
            public string ClaseCondicion { get; set; }
            public string PorcentajeDesc { get; set; }
            public string SinDescripcion { get; set; }
            public string MontoDescuento { get; set; }
            public string ClaveDocumento { get; set; }
            public string PorcentajePosi { get; set; }
            public string PrecioFinal { get; set; }
            public string Descripcion { get; set; }
            public string ArticuloRegalo { get; set; }
            public string CantidadPadre { get; set; }
            public string CantidadHijo { get; set; }
            public string PrecioNuevo { get; set; }
            public string PromoAceptada { get; set; }
        }

        public class Item
        {
            public string ItmNumber { get; set; }
            public string Material { get; set; }
            public string ShortText { get; set; }
            public string NetValue { get; set; }
            public string Currency { get; set; }
            public string ReqQty { get; set; }
            public string Plant { get; set; }
            public string TargetQty { get; set; }
            public string StgeLoc { get; set; }
            public string Subtotal1 { get; set; }
            public string Subtotal2 { get; set; }
            public string Subtotal3 { get; set; }
            public string Subtotal4 { get; set; }
            public string Subtotal5 { get; set; }
            public string Subtotal6 { get; set; }
            public int Points { get; set; }
            public string Presentacion { get; set; }
            public string Peso { get; set; }
            public string ClaveProdServ { get; set; }
            public string ClaveUnidad { get; set; }
            public string Resurtible { get; set; }
            public string Importado { get; set; }
            public string Proveedor { get; set; }
            public string Contenido { get; set; }
            public string PromoCrest { get; set; }
            public decimal PrecioCombo { get; set; }
            public string Clasificacion { get; set; }
        }

        public class ErrorResponse
        {
            public string Number { get; set; }
            public string Message { get; set; }
            public string Row { get; set; }
        }
    }
}
