using backendfepon.Data;
using backendfepon.DTOs.StateDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendfepon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public StateController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StateDTO>>> GetStates()
        {
            try
            {
                var states = await _context.States
                    .Select(s => new StateDTO
                    {
                        State_Name = s.State_Name
                    })
                    .ToListAsync();

                return Ok(states);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no se puede obtener los estados"));
            }
        }

        // GET: api/State/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StateDTO>> GetState(int id)
        {
            try
            {
                var state = await _context.States
                    .Where(p => p.State_Id == id)
                    .Select(p => new StateDTO
                    {
                        State_Name = p.State_Name
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
    }
}
