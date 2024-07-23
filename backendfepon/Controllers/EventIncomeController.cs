using backendfepon.Data;
using backendfepon.DTOs.EventIncomeDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendfepon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventIncomeController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public EventIncomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/EventIncome
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventIncomeDTO>>> GetEventIncomes()
        {
            try
            {
                var eventIncomes = await _context.EventIncomes
                    .Include(p => p.Transaction)
                    .Include(e => e.Event)
                    .Select(p => new EventIncomeDTO
                    {
                        Income_Id = p.Income_Id,
                        Transaction_Name = p.Transaction.Reason,
                        Event_Name = p.Event.Title
                    })
                    .ToListAsync();

                return Ok(eventIncomes);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener el ingreso"));
            }
        }

        // GET: api/EventIncome/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventIncomeDTO>> GetEventIncome(int id)
        {
            try
            {
                var eventIncome = await _context.EventIncomes
                    .Include(p => p.Transaction)
                    .Include(e => e.Event)
                    .Where(p => p.Income_Id == id)
                    .Select(p => new EventIncomeDTO
                    {
                        Income_Id = p.Income_Id,
                        Transaction_Name = p.Transaction.Reason,
                        Event_Name = p.Event.Title
                    })
                    .FirstOrDefaultAsync();

                if (eventIncome == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Ingreso del evento no encontrado."));
                }

                return Ok(eventIncome);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener el ingreso"));
            }
        }
    }
}
