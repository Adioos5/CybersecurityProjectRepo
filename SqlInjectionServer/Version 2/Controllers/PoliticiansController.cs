using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SQL_INJ_API.Controllers
{
    [ApiController]
    [Route("api/politicians")]
    public class PoliticiansController : ControllerBase
    {
        private readonly PoliticiansDbContext _dbContext;

        public PoliticiansController(PoliticiansDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string query)
        {
            // Logika pobierania danych z bazy danych lub innych źródeł danych
            var items = _dbContext.Politicians.FromSqlRaw("SELECT * FROM Politicians WHERE Name='" + query+"'").ToListAsync();
            return Ok(items);
        }

        [HttpPost]
        public IActionResult Post([FromBody] string value)
        {
            // Logika zapisywania danych do bazy danych lub innych źródeł danych

            // Przykładowa odpowiedź
            var response = new { Message = "Success", Value = value };

            return Ok(response);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string value)
        {
            // Logika aktualizacji danych w bazie danych lub innych źródłach danych

            // Przykładowa odpowiedź
            var response = new { Message = "Success", Value = value };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // Logika usuwania danych z bazy danych lub innych źródeł danych

            // Przykładowa odpowiedź
            var response = new { Message = "Success", Id = id };

            return Ok(response);
        }
    }
}
