using CallCentersRD_API.Data.DTOs;
using CallCentersRD_API.Data.Entities;
using CallCentersRD_API.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CallCentersRD_API.Controllers
{
    [Route("api/responses")]
    [ApiController]
    public class ResponsesController:ControllerBase
    {
        private readonly CallCenterDbContext _context;
        public ResponsesController(CallCenterDbContext context)
        {
            _context = context;
        }

        // GET: api/responses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionResponse>>> GetResponses()
        {
            return await _context.Responses.ToListAsync();
        }

        // GET: api/responses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pregunta>> GetResponses(int id)
        {
            var respuesta = await _context.Responses.FindAsync(id);

            if (respuesta == null)
            {
                return NotFound();
            }

            return Ok(respuesta);
        }

        // PUT: api/responses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRespuesta(QuestionResponse response)
        {
            if (response == null)
            {
                return BadRequest();
            }

            _context.Entry(response).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResponseExists(response.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(response);
        }

        private bool ResponseExists(int id)
        {
            return _context.Responses.Any(e => e.Id == id);
        }

        // POST: api/responses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<QuestionResponse>> PostResponse(QuestionResponseDto preguntaDto)
        {
            var Pregunta = await _context.Preguntas.FindAsync(preguntaDto.questionId);
            if(Pregunta == null)
            {
                return NotFound("The question ID sent does not exist.");
            }

            var user = await _context.Users.FindAsync(preguntaDto.userId);

            if (user == null)
            {
                return NotFound("The user sent does not exist.");
            }

            if (preguntaDto.responseContent.Length < 1)
            {
                return BadRequest("Response text must not be empty"); 
            }

            QuestionResponse questionResponse = new QuestionResponse()
            {
                answerDate = DateTime.Now,
                questionContent = Pregunta.pregunta,
                questionId = preguntaDto.questionId,
                responseContent = preguntaDto.responseContent,
                userId = preguntaDto.userId,
                responserName = user.Name
            };

            _context.Responses.Add(questionResponse);
            await _context.SaveChangesAsync();
            return Ok(questionResponse);
        }

    }
}
