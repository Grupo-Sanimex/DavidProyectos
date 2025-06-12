using sanimex.webapi.Datos.Servicio.WebServicesSap.Interfaces;
using sanimex.webapi.Dominio.Models.WebServiceSap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sanimex.webapi.Negocio.WebServiceSap
{
    public class WebServiceNegocio : IWebServiceNegocio
    {
        private readonly IWebServiceSap _webServiceSap;
        public WebServiceNegocio(IWebServiceSap webServiceSap)
        {
            _webServiceSap = webServiceSap;
        }
        public async Task<Webservice> AccesoWebservice(string nombreServicio)
        {
            return await _webServiceSap.AccesoWebservice(nombreServicio);
        }
    }
}
