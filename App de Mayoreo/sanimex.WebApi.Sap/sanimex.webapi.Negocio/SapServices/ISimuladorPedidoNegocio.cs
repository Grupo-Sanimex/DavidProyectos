using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Negocio.SapServices
{
    public interface ISimuladorPedidoNegocio
    {
        Task<DataSet> SimuladorPiezaAsync(string codebar, string noCliente, string ClaveSap);
    }
}
