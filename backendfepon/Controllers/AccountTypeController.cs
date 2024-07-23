using backendfepon.Data;
using backendfepon.DTOs.AccountTypeDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendfepon.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountTypeController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public AccountTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountTypeDTO>>> GetAccountTypes()
        {
            try
            {
                var accountTypes = await _context.AccountTypes
                    .Select(s => new AccountTypeDTO
                    {
                        Account_Type_Name = s.Account_Type_Name
                    })
                    .ToListAsync();

                return Ok(accountTypes);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener los tipos de cuenta"));
            }
        }

        // GET: api/AccountType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountTypeDTO>> GetAccountType(int id)
        {
            try
            {
                var accountType = await _context.AccountTypes
                    .Where(p => p.Account_Type_Id == id)
                    .Select(p => new AccountTypeDTO
                    {
                        Account_Type_Name = p.Account_Type_Name
                    })
                    .FirstOrDefaultAsync();

                if (accountType == null)
                {
                    return NotFound(GenerateErrorResponse(404, "Tipo de cuenta no encontrado."));
                }

                return Ok(accountType);
            }
            catch
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible obtener el tipo de cuenta"));
            }
        }
    }
}
