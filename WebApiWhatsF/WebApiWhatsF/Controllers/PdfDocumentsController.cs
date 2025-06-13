using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using WebApiWhatsF.Models;
using WebApiWhatsF.Servicios;
using WebApplpdfFinal6;

namespace WebApiWhatsF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfDocumentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly WhatsAppServicio _whatsAppService;

        public PdfDocumentsController(AppDbContext context, WhatsAppServicio whatsAppServicio)
        {
            this._context = context;
            _whatsAppService = whatsAppServicio;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PdfDocument>>> GetPdfDocuments()
        {
            return await _context.PdfDocuments.ToListAsync();
        }

        [HttpGet("pdf/{name}")]
        public async Task<IActionResult> VerPdfDocumentByName(string name)
        {
            var pdfDocument = await _context.PdfDocuments
                .FirstOrDefaultAsync(p => p.FileName == name);

            if (pdfDocument == null)
                return NotFound();

            // Obtener la ruta del archivo almacenada en la BD
            var filePath = pdfDocument.FilePath;

            if (!System.IO.File.Exists(filePath))
                return NotFound("El archivo no existe en el servidor.");

            // Devolver el archivo directamente desde la ruta
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "application/pdf", pdfDocument.FileName);
        }


        [HttpPost]
        public async Task<IActionResult> PostPdfDocument([FromForm] IFormFile file, [FromForm] string fileName)
        {
            if (file == null || fileName == null)
            {
                return BadRequest("El archivo o el nombre del archivo no fueron enviados correctamente.");
            }

            // Definir la carpeta donde se guardarán los archivos ruta local
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");


            // Verificar si la carpeta existe, si no, la crea
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Construir la ruta completa del archivo
            string filePath = Path.Combine(uploadsFolder, fileName);

            byte[] fileData;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileData = memoryStream.ToArray();
            }

            // Guardar el archivo físicamente en la ruta especificada
            await System.IO.File.WriteAllBytesAsync(filePath, fileData);

            // Guardar los datos en la base de datos
            var pdfDocument = new PdfDocument
            {
                FileName = fileName,
                FileData = fileData, // Guardar los bytes en la base de datos
                FilePath = filePath, // Guardar la ruta física del archivo
                ContentType = file.ContentType
            };

            _context.PdfDocuments.Add(pdfDocument);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { message = "File uploaded successfully", filePath });
        }


        [HttpPost("enviarPdf")]
        public async Task<IActionResult> EnviarMensajePdf([FromForm] string numeroCliente, [FromForm] string numeroAcesor, [FromForm] string nombreCotizacion, [FromForm] string totalCotizacion)
        {
            var resultado = await _whatsAppService.EnviarMensajeWhatsAppAsync(numeroCliente, numeroAcesor, nombreCotizacion, totalCotizacion);
            //var resultado = await _whatsAppService.EnviarMensajeWhatsAppAsyncD(numeroCliente, numeroAcesor, nombreCotizacion, totalCotizacion, telefonoSucursal);
            return Ok(new { mensaje = resultado });
        }

        [HttpPost("enviarPdfb")]
        public async Task<IActionResult> EnviarMensajePdfb([FromForm] string numeroCliente, [FromForm] string numeroAcesor, [FromForm] string nombreCotizacion, [FromForm] string totalCotizacion, [FromForm] string telefonoSucursal)
        {
            //var resultado = await _whatsAppService.EnviarMensajeWhatsAppAsync(numeroCliente, numeroAcesor, nombreCotizacion, totalCotizacion);
            var resultado = await _whatsAppService.EnviarMensajeWhatsAppAsyncD(numeroCliente, numeroAcesor, nombreCotizacion, totalCotizacion, telefonoSucursal);
            return Ok(new { mensaje = resultado });
        }

        [HttpDelete("{documentName}")]
        public async Task<IActionResult> DeletePdfDocument(string documentName)
        {
            var entity = await _context.PdfDocuments
                .FirstOrDefaultAsync(doc => doc.FileName == documentName);

            if (entity == null)
                return NotFound();

            // Definir la ruta del archivo en el servidor
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            string filePath = Path.Combine(uploadsFolder, documentName);

            // Registrar la ruta para depuración
            Console.WriteLine($"Intentando eliminar archivo: {filePath}");

            try
            {
                // Verificar si el archivo existe antes de eliminarlo
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    Console.WriteLine("Archivo eliminado correctamente.");
                }
                else
                {
                    Console.WriteLine("El archivo no existe en la ruta especificada.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el archivo: {ex.Message}");
                return StatusCode(500, "Error al eliminar el archivo en el servidor.");
            }

            // Eliminar el registro de la base de datos
            _context.PdfDocuments.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpPost("Webhook")]
        public IActionResult ReceiveWebhook([FromBody] object payload)
        {
            // Loguear la data recibida
            Console.WriteLine($"Webhook recibido: {payload}");

            // Procesar el webhook aquí
            return Ok(new { message = "Webhook recibido correctamente" });
        }
        [HttpGet("VerifyWebhook")]
        public IActionResult VerifyWebhook([FromQuery] string hub_mode, [FromQuery] string hub_challenge, [FromQuery] string hub_verify_token)
        {
            const string verifyToken = "MI_TOKEN_SECRETO"; // Debe coincidir con el que ingreses en Facebook

            if (hub_mode == "subscribe" && hub_verify_token == verifyToken)
            {
                return Ok(hub_challenge);
            }

            return Unauthorized();
        }

    }
}
