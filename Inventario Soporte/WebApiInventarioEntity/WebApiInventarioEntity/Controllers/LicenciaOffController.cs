using InventarioDatos.ModelsDto;
using InventarioNegocio.LincenciasOffice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiInventarioEntity.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class LicenciaOffController : ControllerBase
    {
        private readonly ILicenciaOfficeNegocio _licenciaOfficeNegocio;
        public LicenciaOffController(ILicenciaOfficeNegocio licenciaOfficeNegocio)
        {
            _licenciaOfficeNegocio = licenciaOfficeNegocio;
        }
        [HttpGet]
        public IActionResult ObtenerLicenciasOffice()
        {
            var licenciasOffice = _licenciaOfficeNegocio.ObtenerLicenciasOffice();
            return Ok(licenciasOffice);
        }
        [HttpGet("{id}")]
        public IActionResult ObtenerLicenciaOfficePorId(int id)
        {
            var licenciaOffice = _licenciaOfficeNegocio.ObtenerLicenciaOfficePorId(id);
            if (licenciaOffice == null)
            {
                return NotFound();
            }
            return Ok(licenciaOffice);
        }
        [HttpGet("Equipo/{idEquipo}")]
        public IActionResult ObtenerLicenciaOfficePorEquipo(int idEquipo)
        {
            var licenciaOffice = _licenciaOfficeNegocio.ObtenerLicenciaOfficePorEquipo(idEquipo);
            if (licenciaOffice == null)
            {
                return NotFound();
            }
            return Ok(licenciaOffice);
        }
        [HttpPost]
        public IActionResult AgregarLicenciaOffice([FromBody] LicenciaOfficeDto licenciaOfficeDto)
        {
            if (licenciaOfficeDto == null)
            {
                return BadRequest();
            }
            var resultado = _licenciaOfficeNegocio.AgregarLicenciaOffice(licenciaOfficeDto);
            if (!resultado)
            {
                return BadRequest("Error al agregar la licencia.");
            }   
            return CreatedAtAction(nameof(ObtenerLicenciaOfficePorId), new { id = licenciaOfficeDto.IdLicencia }, licenciaOfficeDto);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarLicenciaOffice(int id, [FromBody] LicenciaOfficeDto licenciaOfficeDto)
        {
            if (licenciaOfficeDto == null || id != licenciaOfficeDto.IdLicencia)
            {
                return BadRequest();
            }
            var licenciaOffice = _licenciaOfficeNegocio.ObtenerLicenciaOfficePorId(id);
            if (licenciaOffice == null)
            {
                return NotFound();
            }
            var resultado = _licenciaOfficeNegocio.ActualizarLicenciaOffice(licenciaOfficeDto);
            if (!resultado)
            {
                return BadRequest("Error al actualizar la licencia.");
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult EliminarLicenciaOffice(int id)
        {
            var licenciaOffice = _licenciaOfficeNegocio.ObtenerLicenciaOfficePorId(id);
            if (licenciaOffice == null)
            {
                return NotFound();
            }
            _licenciaOfficeNegocio.EliminarLicenciaOffice(id);
            return NoContent();
        }
    }
}
