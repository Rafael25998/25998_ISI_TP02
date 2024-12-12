using CarrosAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    }
}
