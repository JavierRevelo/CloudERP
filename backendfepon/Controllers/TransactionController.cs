using AutoMapper;
using backendfepon.Cypher;
using backendfepon.Data;
using backendfepon.DTOs.ProductDTOs;
using backendfepon.DTOs.TransactionDTOs;
using backendfepon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;


namespace backendfepon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        private readonly byte[]? _key;
        private readonly byte[]? _iv;

        public TransactionController(ApplicationDbContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;

            _iv = Convert.FromBase64String(_configuration["AESConfig:IV"]);
            _key = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(_configuration["AESConfig:KEY"]));
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> GetTransactions()
        {
            CypherAES cy =new CypherAES();
            var transactions = await _context.Transactions
            .Include(p => p.DestinationAccount)
           .Select(p => new TransactionDTO
           {
               id = p.Transaction_Id,
               transactionType= p.TransactionType.Transaction_Type_Name,
               date = p.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
               originAccount = cy.DecryptStringFromBytes_Aes(p.OriginAccount.Account_Name, _key, _iv) ,
               destinationAccount = cy.DecryptStringFromBytes_Aes(p.DestinationAccount.Account_Name, _key, _iv),
               value = p.Value,
               description = p.Reason,

           })
           .ToListAsync();

            return Ok(transactions);
        }

        // GET: api/Transaction/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDTO>> GetTransaction(int id)
        {
            CypherAES cy = new CypherAES();
            var transaction = await _context.Transactions
            .Include(p => p.DestinationAccount)
            .Where(p => p.Transaction_Id == id)
           .Select(p => new TransactionDTO
           {
               id = p.Transaction_Id,
               transactionType = p.TransactionType.Transaction_Type_Name,
               date = p.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
               originAccount = cy.DecryptStringFromBytes_Aes(p.OriginAccount.Account_Name, _key, _iv),
               destinationAccount = cy.DecryptStringFromBytes_Aes(p.DestinationAccount.Account_Name, _key, _iv),
               value = p.Value,
               description = p.Reason,
           })
           .FirstOrDefaultAsync();

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }

        [HttpPost]
        public async Task<ActionResult<TransactionDTO>> PostTransaction(CreateUpdateTransactionDTO transactionDTO)

        {
            CypherAES cy = new CypherAES();

            try
            {
                var originAccount = await _context.CAccountinngAccounts.FirstOrDefaultAsync(a => a.AccountType.Account_Type_Name == transactionDTO.Origin_Account);
                if (originAccount == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Nombre de cuenta de origen no válido."));
                }

                var destinationAccount = await _context.CAccountinngAccounts.FirstOrDefaultAsync(a => cy.DecryptStringFromBytes_Aes(a.Account_Name, _key, _iv) == transactionDTO.Destination_Account);
                if (destinationAccount == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Nombre de cuenta de destino no válido."));
                }

                var transactionType = await _context.TransactionTypes.FirstOrDefaultAsync(a => a.Transaction_Type_Name == transactionDTO.transactionType);
                if (transactionType == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "Nombre de cuenta de destino no válido."));
                }

                var transaction = _mapper.Map<Transaction>(transactionDTO);
                transaction.Origin_Account = originAccount.Account_Id;
                transaction.Destination_Account = destinationAccount.Account_Id;
                transaction.Transaction_Type_Id = transactionType.Transaction_Type_Id;

                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();

                var createdTransactionDTO = _mapper.Map<TransactionDTO>(transaction);
                return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Transaction_Id }, createdTransactionDTO);
            }
            catch (DbUpdateException ex)
            {
                // Handle database update exceptions
                return StatusCode(500, GenerateErrorResponse(500, "Error al actualizar la base de datos, no es posible crear la Asociación", ex));
            }
            catch (AutoMapperMappingException ex)
            {
                // Handle AutoMapper exceptions
                return StatusCode(500, GenerateErrorResponse(500, "Error en la configuración del mapeo, no es posible crear la Asociación", ex));
            }
            catch (Exception ex)
            {
                // Handle all other exceptions
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible crear la Asociación", ex));
            }
        }
    }
}
