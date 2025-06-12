using sanimex.webapi.Dominio.Models;
using sanimex.webapi.Dominio.Models.Sucursales;
using sanimex.webapi.Dominio.Models.WebServiceSap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Datos.Servicio.WebServicesSap.Interfaces
{
    public interface IWebServiceSap
    {
        Task<Webservice> AccesoWebservice(string nombreServicio);
        Task<string> CorredorCentro(string centro);

    }
}
