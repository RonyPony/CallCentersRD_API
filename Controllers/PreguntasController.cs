using CallCentersRD_API.Data.DTOs;
using CallCentersRD_API.Data.Entities;
using CallCentersRD_API.Database;
using CallCentersRD_API.dto;
using CallCentersRD_API.Services;
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


        // GET: api/oneQuestion
        [HttpGet("oneQuestion")]
        public async Task<ActionResult<Pregunta>> GetOneQuestion(int userId)
        {
            List<Pregunta> preguntas = await _context.Preguntas.ToListAsync();
            List<QuestionResponse>respuestas =await _context.Responses.ToListAsync();
            List<int> questionsMadeToThisUserId = new List<int>();
            List<int> allQuestionsId = new List<int>();
            var totalQuestions = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["questionsAmount"];
            foreach (QuestionResponse resp in respuestas)
            {
                if (resp.userId == userId)
                {
                    questionsMadeToThisUserId.Add(resp.questionId);
                }
            }
            foreach (Pregunta preg in preguntas)
            {
                allQuestionsId.Add(preg.Id);
            }

            if (questionsMadeToThisUserId.Count()>=Convert.ToInt32(totalQuestions))
            {
                return Unauthorized("This user has already completed the "+ Convert.ToInt32(totalQuestions) + " questions.");
            }
            Pregunta selectedQuestion = getArandomQuestion(preguntas, questionsMadeToThisUserId);
            

            return selectedQuestion;
        }

        

        private Pregunta getArandomQuestion(List<Pregunta>preguntas,List<int>questionsMade)
        {
            Pregunta finalQuestion = new Pregunta();
            Random rnd = new Random();
            int r = rnd.Next(preguntas.Count);
            finalQuestion = preguntas[r];
            while (questionsMade.Contains(finalQuestion.Id))
            {
                Random rnd2 = new Random();
                int r2 = rnd.Next(preguntas.Count);
                finalQuestion = preguntas[r2];
                
            }
            return finalQuestion;
            
        }

        // GET: api/Preguntas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pregunta>> GetPregunta(int id)
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
        public async Task<IActionResult> PutPregunta(Pregunta Pregunta)
        {
            if (Pregunta==null)
            {
                return BadRequest();
            }

            _context.Entry(Pregunta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PreguntaExists(Pregunta.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(Pregunta);
        }

        // POST: api/Preguntas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pregunta>> PostPregunta(questionDto preguntaDto)
        {
            Pregunta pregunta = new Pregunta()
            {
                pregunta = preguntaDto.pregunta,
                creationDate = DateTime.Now,
                enable = preguntaDto.enable
            };

            pregunta.creationDate = DateTime.Now;
            _context.Preguntas.Add(pregunta);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPregunta), new { id = pregunta.Id }, pregunta);
        }


        // POST: api/Preguntas/enable/{id}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("enable/{id}")]
        public async Task<ActionResult<Pregunta>> EnableQuestion(int id)
        {
            Pregunta pregunta = await _context.Preguntas.FindAsync(id);
            if (!pregunta.enable)
            {
                pregunta.enable = true;
                _context.Entry(pregunta).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Question enabled succesfuly");
            }
            else
            {
                return BadRequest("This question was already active");
            }
        }

        // POST: api/Preguntas/disable/{id}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("disable/{id}")]
        public async Task<ActionResult<Pregunta>> DisableQuestion(int id)
        {
            Pregunta pregunta = await _context.Preguntas.FindAsync(id);
            if (pregunta.enable)
            {
                pregunta.enable = false;
                _context.Entry(pregunta).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok("Question disabled succesfuly");
            }
            else
            {
                return BadRequest("This question was already disabled");
            }
        }


        // DELETE: api/Preguntas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePregunta(int id)
        {
            var Pregunta = await _context.Preguntas.FindAsync(id);
            if (Pregunta == null)
            {
                return NotFound();
            }

            _context.Preguntas.Remove(Pregunta);
            await _context.SaveChangesAsync();

            return Ok("Deleted");
        }

        private bool PreguntaExists(int id)
        {
            return _context.Preguntas.Any(e => e.Id == id);
        }
    }
}
