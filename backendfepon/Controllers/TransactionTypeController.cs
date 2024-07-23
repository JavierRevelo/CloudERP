using backendfepon.Data;
using backendfepon.DTOs.StateDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendfepon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionTypeController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public TransactionTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StateDTO>>> GetStates()
        {
            try
            {
                var states = await _context.TransactionTypes
                    .Select(s => new StateDTO
                    {
                        State_Name = s.Transaction_Type_Name,
                    })
                    .ToListAsync();

                return Ok(states);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no se puede obtener los estados"));
            }
        }
    }
}
