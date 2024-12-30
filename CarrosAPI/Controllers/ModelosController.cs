using CarrosAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace CarrosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelosController : ControllerBase
    {
        private readonly CarrosDbContext _context;

        public ModelosController(CarrosDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModeloCarro>>> GetModelos()
        {
            return await _context.ModelosCarros.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ModeloCarro>> GetModelo(int id)
        {
            var modelo = await _context.ModelosCarros.FindAsync(id);

            if (modelo == null)
            {
                return NotFound();
            }

            return modelo;
        }
        [HttpGet("modelo/{modelo}")]
        public ActionResult<IEnumerable<ModeloCarro>> GetPorModelo(string modelo)
        {
            var modelos = _context.ModelosCarros.Where(m => m.Modelo == modelo).ToList();

            if (modelos == null || !modelos.Any())
            {
                return NotFound(new { Message = "Nenhum modelo encontrado para o modelo especificado." });
            }

            return Ok(modelos);
        }
        [HttpGet("marca/{marca}")]
        public ActionResult<IEnumerable<ModeloCarro>> GetPorMarca(string marca)
        {
            var modelos = _context.ModelosCarros.Where(m => m.Marca == marca).ToList();

            if (modelos == null || !modelos.Any())
            {
                return NotFound(new { Message = "Nenhum modelo encontrado para a marca especificada." });
            }

            return Ok(modelos);
        }


        [HttpPost]
        public async Task<ActionResult<ModeloCarro>> PostModelo(ModeloCarro modelo)
        {
            _context.ModelosCarros.Add(modelo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetModelo), new { id = modelo.Id }, modelo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutModelo(int id, ModeloCarro modelo)
        {
            if (id != modelo.Id)
            {
                return BadRequest();
            }

            _context.Entry(modelo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ModelosCarros.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModelo(int id)
        {
            var modelo = await _context.ModelosCarros.FindAsync(id);
            if (modelo == null)
            {
                return NotFound();
            }

            _context.ModelosCarros.Remove(modelo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("previsao/{cidade}")]
        public async Task<IActionResult> GetPrevisaoPorCidade(string cidade)
        {
            string apiKey = "d3c56ab755bbf66c9228fe7954ab85df";

            // 1. Buscar as coordenadas da cidade
            string geoUrl = $"https://api.openweathermap.org/geo/1.0/direct?q={cidade}&limit=1&appid={apiKey}";
            using (var client = new HttpClient())
            {
                var geoResponse = await client.GetAsync(geoUrl);
                if (!geoResponse.IsSuccessStatusCode)
                {
                    return BadRequest($"Erro ao buscar coordenadas para a cidade: {cidade}");
                }

                var geoData = await geoResponse.Content.ReadAsStringAsync();
                var geoJson = JArray.Parse(geoData);

                if (!geoJson.Any())
                {
                    return NotFound($"Cidade '{cidade}' não encontrada.");
                }

                double latitude = (double)geoJson[0]["lat"];
                double longitude = (double)geoJson[0]["lon"];

                // 2. Buscar a previsão do tempo usando as coordenadas
                string weatherUrl = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={apiKey}&units=metric";
                var weatherResponse = await client.GetAsync(weatherUrl);
                if (!weatherResponse.IsSuccessStatusCode)
                {
                    return BadRequest("Erro ao buscar previsão do tempo.");
                }

                var weatherData = await weatherResponse.Content.ReadAsStringAsync();
                var weatherJson = JObject.Parse(weatherData);

                // 3. Retornar os dados formatados
                return Ok(new
                {
                    Cidade = cidade,
                    Temperatura = weatherJson["main"]["temp"].ToString() + "°C",
                    Clima = weatherJson["weather"][0]["description"].ToString(),
                    Icone = $"https://openweathermap.org/img/wn/{weatherJson["weather"][0]["icon"]}.png"
                });
            }
        }
    }
}
