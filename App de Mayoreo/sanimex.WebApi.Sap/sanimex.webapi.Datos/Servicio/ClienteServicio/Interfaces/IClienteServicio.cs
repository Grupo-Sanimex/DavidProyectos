using sanimex.webapi.Dominio.Models;
using sanimex.webapi.Dominio.Models.Clientes;
using sanimex.webapi.Dominio.Models.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Datos.Servicio.ClienteServicio.Interfaces
{
    public interface IClienteServicio
    {
        Task<ClienteBd> ObtenerCliente(int id); // Declaración del método que devuelve un objeto Usuario
        Task<List<Historico_Vta_May>> HistoricoVtaMay(int ClienteSap);

        Task<string> maxNumeroClientes(string idUsuario, int numCliente);

        Task<Metros_Importe_Mayoreo> MetrosImporteMayoreo(string ClaveCliente);
    }
}
