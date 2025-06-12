using sanimex.webapi.Datos.Servicio.ClienteServicio.Interfaces;
using sanimex.webapi.Dominio.Models;
using sanimex.webapi.Dominio.Models.Clientes;
using sanimex.webapi.Dominio.Models.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Negocio.Clientes
{
    public class ClienteNegocio: IClienteNegocio
    {
        private readonly IClienteServicio _clienteServicio;

        public ClienteNegocio(IClienteServicio clienteServicio)
        {
            _clienteServicio = clienteServicio;
        }
        public async Task<ClienteBd> ObtenerCliente(int id) // Implementación del método
        {
            return await _clienteServicio.ObtenerCliente(id); // Llama al método de la capa de datos
        }
        public async Task<List<Historico_Vta_May>> HistoricoVtaMay(int ClienteSap)
        {
            return await _clienteServicio.HistoricoVtaMay(ClienteSap);
        }

        public async Task<string> maxNumeroClientes(string idUsuario, int numCliente)
        {
            return await _clienteServicio.maxNumeroClientes(idUsuario,numCliente);
        }
        public async Task<Metros_Importe_Mayoreo> MetrosImporteMayoreo(string ClaveCliente) 
        {
            return await _clienteServicio.MetrosImporteMayoreo(ClaveCliente); // Llama al método de la capa de datos
        }
    }
}
