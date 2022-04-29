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

        // GET: api/responses/byUserId/5
        [HttpGet("byUserId/{userId}")]
        public async Task<ActionResult<List<QuestionResponse>>> GetUserResponses(int userId)
        {
            var respuestas = await _context.Responses.ToListAsync();
            var users = await _context.Users.FindAsync(userId);
            List<QuestionResponse> userResponses = new List<QuestionResponse>();
            if (userId<=0 || users ==null)
            {
                return BadRequest();
            }

            foreach (QuestionResponse item in respuestas)
            {
                if (item.userId == userId)
                {
                    userResponses.Add(item);
                }
            }

            return Ok(userResponses);
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

        // GET: api/responseCounter
        [HttpGet("responseCounter")]
        public async Task<ActionResult<UserResponseCount>> responseCounter(int userId)
        {
            List<Pregunta> preguntas = await _context.Preguntas.ToListAsync();
            List<QuestionResponse> respuestas = await _context.Responses.ToListAsync();
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

            
            UserResponseCount counts = new UserResponseCount()
            {
                answeredQuestions = questionsMadeToThisUserId.Count(),
                notAnsweredQuestions= Convert.ToInt32(totalQuestions)- questionsMadeToThisUserId.Count(),
            };

            return counts;
        }


        // GET: api/hasUserCompletedQuestions/{userId}
        [HttpGet("hasUserCompletedQuestions/{userId}")]
        public async Task<ActionResult<bool>> hasUserCompletedQuestions(int userId)
        {
            List<Pregunta> preguntas = await _context.Preguntas.ToListAsync();
            List<QuestionResponse> respuestas = await _context.Responses.ToListAsync();
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

            bool retunringValue = questionsMadeToThisUserId.Count()>=Convert.ToInt32(totalQuestions);

            return retunringValue;
        }


        // DELETE: api/Response/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResponse(int id)
        {
            var response = await _context.Responses.FindAsync(id);
            if (Response == null)
            {
                return NotFound();
            }

            _context.Responses.Remove(response);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
