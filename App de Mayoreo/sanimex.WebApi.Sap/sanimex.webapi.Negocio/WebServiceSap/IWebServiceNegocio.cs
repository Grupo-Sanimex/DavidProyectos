using sanimex.webapi.Dominio.Models;
using sanimex.webapi.Dominio.Models.WebServiceSap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Negocio.WebServiceSap
{
    public interface IWebServiceNegocio
    {
        Task<Webservice> AccesoWebservice(string nombreServicio);
    }
}
