using AutoMapper;
using backendfepon.Data;
using backendfepon.DTOs.FinantialRequestDTOs;
using backendfepon.DTOs.PermissionDTOs;
using backendfepon.DTOs.ProductDTOs;
using backendfepon.DTOs.TransactionDTOs;
using backendfepon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace backendfepon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinantialRequestController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
       // private readonly ILogger _logger;

        public FinantialRequestController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
           // _logger = logger;
        }

        // GET: api/FinantialRequest
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FinantialRequestDTO>>> GetFinantialRequests()
        {

            var finantialRequests = await _context.FinancialRequests
            .Include(p => p.Events)
            .Include(p => p.Financial_Request_State)
            //.Where(p => p.Events.Financial_Request_Id == Constants.DEFAULT_STATE)
           .Select(p => new FinantialRequestDTO
           {
               id = p.Request_Id,
              eventName= p.Events.Title,
               requestStatusName = p.Financial_Request_State.State_Description,
               value = p.Value,
               reason = p.Reason


           })
           .ToListAsync();

            return Ok(finantialRequests);
        }

        // GET: api/Transaction/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FinantialRequestDTO>> GetFinantialRequest(int id)
        {
            var finantialRequest = await _context.FinancialRequests
            .Include(p => p.Events)
            .Include(p => p.Financial_Request_State)
            .Where(p => p.Request_Id == id)
           .Select(p => new FinantialRequestDTO
           {
               id = p.Request_Id,
               eventName = p.Events.Title,
               requestStatusName = p.Financial_Request_State.State_Description,
               value = p.Value,
               reason = p.Reason

           })
           .FirstOrDefaultAsync();

            if (finantialRequest == null)
            {
                return NotFound();
            }

            return finantialRequest;
        }

        // POST: api/FinancialRequest
        [HttpPost]
        public async Task<ActionResult<FinantialRequestDTO>> PostFinancialRequest(CreateUpdateFinantialRequestDTO financialRequestDTO)
        {
            try
            {
                // Find the Request Status ID based on the name
                var requestStatus = await _context.FinancialRequestStates.FirstOrDefaultAsync(rs => rs.State_Description == financialRequestDTO.requestStatusName);
                if (requestStatus == null)
                {
                    return BadRequest("Invalid Request Status name.");
                }

                // Find the Event based on the name
                var findEvent = await _context.Events.FirstOrDefaultAsync(e => e.Title == financialRequestDTO.eventName);
                if (findEvent == null)
                {
                    return BadRequest("Invalid event name.");
                }

                // Map the DTO to the entity
                var financialRequest = _mapper.Map<FinancialRequest>(financialRequestDTO);
                financialRequest.Request_Status_Id = requestStatus.Request_State_Id;

                // Add the new FinancialRequest to the context
                _context.FinancialRequests.Add(financialRequest);
                await _context.SaveChangesAsync();

                // Update the Event's Financial_Request_Id with the newly created FinancialRequest's ID
                findEvent.Financial_Request_Id = financialRequest.Request_Id;
                _context.Entry(findEvent).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                // Map the created FinancialRequest to a DTO
                var createdFinancialRequestDTO = _mapper.Map<FinantialRequestDTO>(financialRequest);

                return CreatedAtAction(nameof(GetFinantialRequest), new { id = financialRequest.Request_Id }, createdFinancialRequestDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible crear y actualizar el evento con nuevo permiso", ex));
            }
        }


        // PUT: api/FinancialRequests/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFinantialRequest(int id, CreateUpdateFinantialRequestDTO updatedFinantialRequest)
        {
            var financialRequest = await _context.FinancialRequests.FindAsync(id);

            if (financialRequest == null)
            {
                return BadRequest("Invalid Financial Request ID.");
            }

            // Find the Request Status ID based on the name
            var requestStatus = await _context.FinancialRequestStates.FirstOrDefaultAsync(rs => rs.State_Description == updatedFinantialRequest.requestStatusName);
            if (requestStatus == null)
            {
                return BadRequest("Invalid Request Status name.");
            }

            var findEvent = await _context.Events.FirstOrDefaultAsync(rs => rs.Title== updatedFinantialRequest.eventName);
            if (requestStatus == null)
            {
                return BadRequest("Invalid event name.");
            }

            // Map the updated properties to the existing financial request
            _mapper.Map(updatedFinantialRequest, financialRequest);
           // financialRequest.Administrative_Member_Id = administrativeMember.Administrative_Member_Id; // Set the Administrative_Member_Id manually
            financialRequest.Request_Status_Id = requestStatus.Request_State_Id; // Set the Request_Status_Id manually

            _context.Entry(financialRequest).State = EntityState.Modified;

            //actualizar evento

            //_mapper.Map(updatedEvent, existingEvent);
            findEvent.Financial_Request_Id = id;
            findEvent.Budget_Status= updatedFinantialRequest.requestStatusName;

            _context.Entry(findEvent).State = EntityState.Modified;



            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FinantialRequestExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        


        /*
        [HttpPut("/updateFinantial/{id}")]
        public async Task<IActionResult> UpdateEventPermission(int id, CreateUpdateFinantialRequestDTO permissionDTO)
        {
            try
            {
                var status = await _context.FinancialRequestStates.FirstOrDefaultAsync(s => s.State_Description == permissionDTO.requestStatusName);
                // Crear un nuevo objeto Permission con los datos del DTO
                var newPermission = new FinancialRequest
                {
                    Request_Status_Id = status.Request_State_Id,
                    Value = permissionDTO.value,
                    Reason = permissionDTO.reason,
                    // Asignar otros campos según sea necesario
                };

                // Agregar el nuevo Permission a la base de datos
                _context.FinancialRequests.Add(newPermission);
                await _context.SaveChangesAsync();

                // Obtener el ID del nuevo Permission generado
                var newPermissionId = await _context.FinancialRequests.FirstOrDefaultAsync(s => s.Reason == permissionDTO.reason);

                // Buscar el evento que se va a actualizar
                var existingEvent = await _context.Events.FindAsync(id);
                if (existingEvent == null)
                {
                    return BadRequest(GenerateErrorResponse(400, "ID del evento no válido."));
                }

                // Asignar el nuevo Permission_Id al evento existente
                existingEvent.Financial_Request_Id = newPermissionId.Request_Id;

                // Guardar los cambios en el evento
                _context.Entry(existingEvent).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, GenerateErrorResponse(500, "Ocurrió un error interno del servidor, no es posible actualizar el evento con nuevo permiso", ex));
            }
        }*/
        
        

        // DELETE: api/FinantialRequest/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFinantialRequest(int id)
        {
            var finantialRequest = await _context.FinancialRequests.FindAsync(id);
            if (finantialRequest == null)
            {
                return NotFound();
            }

            _context.FinancialRequests.Remove(finantialRequest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FinantialRequestExists(int id)
        {
            return _context.FinancialRequests.Any(e => e.Request_Id == id);
        }
    }
}
