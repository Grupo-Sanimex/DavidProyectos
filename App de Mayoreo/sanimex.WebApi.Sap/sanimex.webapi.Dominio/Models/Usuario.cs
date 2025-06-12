namespace sanimex.webapi.Dominio.Models
{
    public class Usuario
    {
        public string? idUsuario { get; set; }
        public string _id { get; set; }
        public string? contrasena { get; set; }
        public string? idSucursal { get; set; }
        public string idPermiso { get; set; }
        public int idRol { get; set; }
        public string numEmpleado { get; set; }
        public string? correo { get; set; }
        public string? nombre { get; set; }
        public string? aPaterno { get; set; }
        public string? telefono { get; set; }
        public bool status { get; set; }
    }
}
