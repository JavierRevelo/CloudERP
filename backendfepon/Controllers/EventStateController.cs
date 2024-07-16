using backendfepon.Data;
using backendfepon.DTOs.EventStateDTO;
using backendfepon.DTOs.StateDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendfepon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventStateController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventStateController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/State
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventStateDTO>>> GetStates()
        {
            try
            {
                var states = await _context.EventStates
                    .Select(s => new EventStateDTO
                    {
                        EventState_Name = s.Event_State_Name
                    })
                    .ToListAsync();

                return Ok(states);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no se puede obtener los estados del evento"));
            }
        }

        // GET: api/State/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventStateDTO>> GetState(int id)
        {
            try
            {
                var state = await _context.EventStates
                    .Where(p => p.Event_State_Id == id)
                    .Select(p => new EventStateDTO
                    {
                        EventState_Name = p.Event_State_Name
                    })
                    .FirstOrDefaultAsync();

                if (state == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Estado no encontrado."));
                }

                return Ok(state);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no se puede obtener el estado."));
            }
        }

        private ObjectResult GenerateErrorResponse(int statusCode, string message)
        {
            return StatusCode(statusCode, new { error = message });
        }
    }
}