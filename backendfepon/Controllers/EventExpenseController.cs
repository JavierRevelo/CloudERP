using backendfepon.Data;
using backendfepon.DTOs.EventExpenseDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendfepon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventExpenseController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public EventExpenseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/EventExpense
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventExpenseDTO>>> GetEventExpenses()
        {
            try
            {
                var eventExpenses = await _context.EventExpenses
                    .Include(p => p.Transaction)
                    .Include(p => p.Responsible)
                    .Include(p => p.Event)
                    .Select(p => new EventExpenseDTO
                    {
                        Expense_Id = p.Expense_Id,
                        Transaction_Id = p.Transaction_Id,
                        Event_Name = p.Event.Title,
                        Responsible_Name = p.Responsible.AdministrativeMember.Name,
                    })
                    .ToListAsync();

                return Ok(eventExpenses);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener el gasto"));
            }
        }

        // GET: api/EventExpense/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventExpenseDTO>> GetEventExpense(int id)
        {
            try
            {
                var eventExpense = await _context.EventExpenses
                    .Include(p => p.Transaction)
                    .Include(p => p.Responsible)
                    .Include(p => p.Event)
                    .Where(p => p.Expense_Id == id)
                    .Select(p => new EventExpenseDTO
                    {
                        Expense_Id = p.Expense_Id,
                        Transaction_Id = p.Transaction_Id,
                        Event_Name = p.Event.Title,
                        Responsible_Name = p.Responsible.AdministrativeMember.Name,
                    })
                    .FirstOrDefaultAsync();

                if (eventExpense == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Gasto del evento no encontrado."));
                }

                return Ok(eventExpense);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener el gasto"));
            }
        }
    }
}
