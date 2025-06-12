namespace sanimex.webapi.Dominio.Models
{
    public class Disponibilidad
    {
        // Clase que representa cada producto
        public class Producto
        {
            public string Codigo { get; set; }
            public string Descripcion { get; set; }
            public string StockLibre { get; set; }
        }
        public class SucursalHijoSap
        {
            public string SucHijoSap { get; set; }
            public List<Producto> Productos { get; set; }
        }

    }
}
