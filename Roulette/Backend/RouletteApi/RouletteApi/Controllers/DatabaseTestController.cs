using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RuletteInfrastructure.Data;
namespace RouletteApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseTestController : ControllerBase
    {
        private readonly RouletteContext _context;

        public DatabaseTestController(RouletteContext context)
        {
            _context = context;
        }

        [HttpGet("testconnection")]
        public async Task<IActionResult> TestConnection()
        {
            try
            {
                // Intenta conectarse a la base de datos
                await _context.Database.OpenConnectionAsync();
                await _context.Database.CloseConnectionAsync();

                return Ok("Conexión a la base de datos exitosa.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al conectar con la base de datos: {ex.Message}");
            }
        }
    }
}
