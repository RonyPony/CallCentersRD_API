using CallCentersRD_API.Data.Entities;
using CallCentersRD_API.Database;
using CallCentersRD_API.dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CallCentersRD_API.Controllers
{

    [Route("api/preguntas")]
    [ApiController]
    public class PreguntasController : ControllerBase
    {
        private readonly CallCenterDbContext _context;
        public PreguntasController(CallCenterDbContext context)
        {
            _context = context;
        }

        // GET: api/Preguntas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pregunta>>> GetPreguntas()
        {
            return await _context.Preguntas.ToListAsync();
        }

        // GET: api/Preguntas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pregunta>> GetUser(long id)
        {
            var Pregunta = await _context.Preguntas.FindAsync(id);

            if (Pregunta == null)
            {
                return NotFound();
            }

            return Pregunta;
        }

        // PUT: api/Preguntas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long id, Pregunta Pregunta)
        {
            if (id != Pregunta.Id)
            {
                return BadRequest();
            }

            //_context.Entry(Pregunta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Preguntas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pregunta>> PostUser(Pregunta pregunta)
        {
            pregunta.creationDate = DateTime.Now;
            _context.Preguntas.Add(pregunta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = pregunta.Id }, pregunta);
        }

        
        // DELETE: api/Preguntas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var Pregunta = await _context.Preguntas.FindAsync(id);
            if (Pregunta == null)
            {
                return NotFound();
            }

            _context.Preguntas.Remove(Pregunta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(long id)
        {
            return _context.Preguntas.Any(e => e.Id == id);
        }
    }
}
