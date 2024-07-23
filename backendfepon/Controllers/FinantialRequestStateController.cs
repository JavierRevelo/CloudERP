using backendfepon.Data;
using backendfepon.DTOs.FinantialRequestStateDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendfepon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinantialRequestStateController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public FinantialRequestStateController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FinantialRequestStateDTO>>> GetFinantialStates()
        {
            try
            {
                var states = await _context.FinancialRequestStates
                    .Select(s => new FinantialRequestStateDTO
                    {
                        stateDescription = s.State_Description
                    })
                    .ToListAsync();

                return Ok(states);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener el estado de solicitud financiera"));
            }
        }

        // GET: api/FinantialRequestState/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FinantialRequestStateDTO>> GetFinantialState(int id)
        {
            try
            {
                var finantialRequestState = await _context.FinancialRequestStates
                    .Where(s => s.Request_State_Id == id)
                    .Select(s => new FinantialRequestStateDTO
                    {
                        stateDescription = s.State_Description
                    })
                    .FirstOrDefaultAsync();

                if (finantialRequestState == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Estado de solicitud financiera no encontrado."));
                }

                return Ok(finantialRequestState);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener el estado de solicitud financiera"));
            }
        }
    }
}

